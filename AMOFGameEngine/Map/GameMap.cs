﻿using AMOFGameEngine.Game;
using AMOFGameEngine.Mods;
using AMOFGameEngine.Screen;
using AMOFGameEngine.Script;
using AMOFGameEngine.Trigger;
using AMOFGameEngine.Utilities;
using DotSceneLoader;
using Mogre;
using Mogre.PhysX;
using MOIS;
using org.critterai.nav;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Map
{
    public delegate void MapLoadhandler();

    /// <summary>
    /// Define a map in the game
    /// </summary>
    public class GameMap : IMap
    {
        private string mapName;
        private List<ITrigger> mapTriggers;
        private DotSceneLoader.DotSceneLoader mapLoader;
        private List<Character> agents;
        private List<GameObject> staticObjects;
        private List<ActorNode> actorNodeList;
        private ScriptLoader scriptLoader;
        private SceneManager scm;
        //private TerrainGroup terrianGroup;
        private Scene physicsScene;
        //private NavmeshQuery query;
        private ModData modData;
        private Physics physics;
        private Character playerAgent;
        private Camera cam;
        private GameWorld world;
        private AIMesh aimesh;
        private Mogre.Vector3 moveOffset;
        private List<Mogre.Vector3> aimeshVertexData;
        private List<Mogre.Vector3> aimeshIndexData;
        private GameMapEditor editor;

        public ModData ModData
        {
            get
            {
                return modData;
            }
        }

        public Scene PhysicsScene
        {
            get
            {
                return physicsScene;
            }
        }

        public NavmeshQuery NavmeshQuery
        {
            get
            {
                return null;
            }
        }

        public SceneManager SceneManager
        {
            get
            {
                return world.SceneManager;
            }
        }

        public Camera Camera
        {
            get
            {
                return world.Camera;
            }
        }

        public event MapLoadhandler LoadMapStarted;
        public event MapLoadhandler LoadMapFinished;

        public GameMap(string name, GameWorld world)
        {
            mapName = name;
            scriptLoader = new ScriptLoader();
            mapTriggers = new List<ITrigger>();
            actorNodeList = new List<ActorNode>();
            this.world = world;
            scm = world.SceneManager;
            modData = world.ModData;
            cam = world.Camera;
            physicsScene = world.PhysicsScene;
            physics = world.PhysicsScene.Physics;
            aimeshIndexData = new List<Mogre.Vector3>();
            aimeshVertexData = new List<Mogre.Vector3>();
            editor = new GameMapEditor(this);

            GameManager.Instance.mMouse.MouseMoved += Mouse_MouseMoved;
            GameManager.Instance.mMouse.MousePressed += Mouse_MousePressed;
            GameManager.Instance.mMouse.MouseReleased += Mouse_MouseReleased;
            GameManager.Instance.mKeyboard.KeyPressed += Keyboard_KeyPressed;
            GameManager.Instance.mKeyboard.KeyReleased += Keyboard_KeyReleased;
        }

        private bool Keyboard_KeyReleased(KeyEvent arg)
        {
            ScreenManager.Instance.InjectKeyReleased(arg);
            return true;
        }

        private bool Keyboard_KeyPressed(KeyEvent arg)
        {
            if (GameManager.Instance.EDIT_MODE)
            {
                if (ScreenManager.Instance.CheckScreenIsVisual("InnerGameEditor") &&
                   (GameManager.Instance.mKeyboard.IsKeyDown(KeyCode.KC_LSHIFT) &&
                    GameManager.Instance.mKeyboard.IsKeyDown(KeyCode.KC_E)))
                {
                    ScreenManager.Instance.ExitCurrentScreen();
                }
                else if(GameManager.Instance.mKeyboard.IsKeyDown(KeyCode.KC_LSHIFT) &&
                    GameManager.Instance.mKeyboard.IsKeyDown(KeyCode.KC_E))
                {
                    ScreenManager.Instance.ChangeScreen("InnerGameEditor", editor);
                }
            }
            ScreenManager.Instance.InjectKeyPressed(arg);
            return true;
        }

        private bool Mouse_MouseReleased(MouseEvent arg, MouseButtonID id)
        {
            ScreenManager.Instance.InjectMouseReleased(arg, id);
            return true;
        }

        private bool Mouse_MousePressed(MouseEvent arg, MouseButtonID id)
        {
            ScreenManager.Instance.InjectMousePressed(arg, id);
            return true;
        }

        private bool Mouse_MouseMoved(MouseEvent arg)
        {
            ScreenManager.Instance.InjectMouseMove(arg);
            
            Degree deCameraYaw = new Degree(arg.state.X.rel * -0.1f);
            cam.Yaw(deCameraYaw);
            Degree deCameraPitch = new Degree(arg.state.Y.rel * -0.1f);
            cam.Pitch(deCameraPitch);

            if (GameManager.Instance.EDIT_MODE)
            {

            }
            return true;
        }

        public void Destroy()
        {
            GameManager.Instance.mMouse.MouseMoved -= Mouse_MouseMoved;
            GameManager.Instance.mMouse.MousePressed -= Mouse_MousePressed;
            GameManager.Instance.mMouse.MouseReleased -= Mouse_MouseReleased;
            GameManager.Instance.mKeyboard.KeyPressed -= Keyboard_KeyPressed;
            GameManager.Instance.mKeyboard.KeyReleased -= Keyboard_KeyReleased;
        }

        public void LoadAsync()
        {
            mapLoader = new DotSceneLoader.DotSceneLoader();
            mapLoader.LoadSceneStarted += mapLoader_LoadMapStarted;
            mapLoader.LoadSceneFinished += mapLoader_LoadMapFinished;
            mapLoader.ParseDotSceneAsync(mapName, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, scm);
        }

        private void mapLoader_LoadMapFinished()
        {
            if(LoadMapFinished!=null)
            {
                agents = new List<Character>();
                staticObjects = new List<GameObject>();
                scriptLoader.Parse(System.IO.Path.GetFileNameWithoutExtension(mapName) + ".script", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
                scriptLoader.Execute(world);

                //MeshManager.Singleton.CreatePlane("floor", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
                //    new Plane(Mogre.Vector3.UNIT_Y, 0), 100, 100, 10, 10, true, 1, 10, 10, Mogre.Vector3.UNIT_Z);
                //Entity floor = scm.CreateEntity("Floor", "floor");
                //floor.SetMaterialName("Examples/Rockwall");
                //floor.CastShadows = (false);
                //scm.RootSceneNode.AttachObject(floor);
                //ActorDesc actorDesc = new ActorDesc();
                //actorDesc.Density = 4;
                //actorDesc.Body = null;
                //actorDesc.Shapes.Add(physics.CreateTriangleMesh(new
                //    StaticMeshData(floor.GetMesh())));
                //Actor floorActor = physicsScene.CreateActor(actorDesc);
                //actorNodeList.Add(new ActorNode(scm.RootSceneNode, floorActor));
                //
                //Navmesh floorNavMesh = MeshToNavmesh.LoadNavmesh(floor);
                //org.critterai.Vector3 pointStart = new org.critterai.Vector3(0, 0, 0);
                //org.critterai.Vector3 pointEnd = new org.critterai.Vector3(0, 0, 0);
                //org.critterai.Vector3 extents = new org.critterai.Vector3(2, 2, 2);

                //NavStatus status = NavmeshQuery.Create(floorNavMesh, 100, out query);

                playerAgent = null;

                aimesh = mapLoader.AIMesh;
                editor.Initization(aimesh);

                LoadMapFinished();
            }
        }

        private void mapLoader_LoadMapStarted()
        {
            if (LoadMapStarted != null)
            {
                LoadMapStarted();
            }
        }

        public void Update(float timeSinceLastFrame)
        {
            updateAgents(timeSinceLastFrame);
            moveOffset = new Mogre.Vector3(0, 0, 0);
            getInput();
            moveCamera();
            PhysicsScene.FlushStream();
            PhysicsScene.FetchResults(SimulationStatuses.AllFinished, true);
            PhysicsScene.Simulate(timeSinceLastFrame);
        }
        private void updateAgents(double timeSinceLastFrame)
        {
            if (agents == null)
            {
                return;
            }
            for (int i = 0; i < agents.Count; i++)
            {
                agents[i].Update((float)timeSinceLastFrame);
            }
        }

        public string GetName()
        {
            return mapName;
        }

        public List<Character> GetAgents()
        {
            return agents;
        }

        public List<GameObject> GetStaticObjects()
        {
            return staticObjects;
        }

        public void SpawnNewCharacter(string characterID, Mogre.Vector3 position, string teamId, bool isBot = true)
        {
            var searchRet = ModData.CharacterInfos.Where(o => o.ID == characterID);
            if (searchRet.Count() > 0)
            {
                Character character = new Character(
                    world, cam, agents.Count, teamId, searchRet.First().Name + agents.Count, searchRet.First().MeshName, position, !isBot);
                if (!isBot)
                {
                    playerAgent = character;
                }
                character.OnCharacterUseWeaponAttack += Character_OnCharacterUseWeaponAttack;
                character.OnCharacterDie += Character_OnCharacterDie;
                agents.Add(character);
            }
        }

        private void Character_OnCharacterUseWeaponAttack(int arg1, int arg2, double arg3)
        {

        }

        private void Character_OnCharacterDie(int obj)
        {

        }
        private void getInput()
        {
            if (GameManager.Instance.mKeyboard.IsKeyDown(KeyCode.KC_A))
                moveOffset.x = -10;

            if (GameManager.Instance.mKeyboard.IsKeyDown(KeyCode.KC_D))
                moveOffset.x = 10;

            if (GameManager.Instance.mKeyboard.IsKeyDown(KeyCode.KC_W))
                moveOffset.z = -10;

            if (GameManager.Instance.mKeyboard.IsKeyDown(KeyCode.KC_S))
                moveOffset.z = 10;

            if (GameManager.Instance.mKeyboard.IsKeyDown(KeyCode.KC_Q))
                moveOffset.y = -10;

            if (GameManager.Instance.mKeyboard.IsKeyDown(KeyCode.KC_E))
                moveOffset.y = 10;
        }
        private void moveCamera()
        {
            if (GameManager.Instance.mKeyboard.IsKeyDown(KeyCode.KC_LSHIFT))
                cam.MoveRelative(moveOffset);
            cam.MoveRelative(moveOffset / 10);
        }
    }
}
