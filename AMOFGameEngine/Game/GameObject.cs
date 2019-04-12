﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre.PhysX;

namespace AMOFGameEngine.Game
{
    public class GameObject : IUpdate
    {
        protected int id;
        protected MoveInfo moveInfo;
        protected Vector3 position;
        protected Camera camera;
        protected SceneManager sceneManager;
        protected SceneNode entNode;
        protected Entity entity;
        protected GameWorld world;
        protected Physics physics;
        protected Scene physicsScene;
        protected HealthInfo health;
        private Actor entityActor;
        public int ID
        {
            get
            {
                return id;
            }
        }
        public MoveInfo MoveInfo
        {
            get
            {
                return moveInfo;
            }
        }
        public bool IsStatic
        {
            get
            {
                return moveInfo == null;
            }
        }
        public Vector3 Position
        {
            get
            {
                return position;
            }
        }
        public GameWorld World
        {
            get
            {
                return world;
            }
        }
        public SceneManager SceneManager
        {
            get
            {
                return sceneManager;
            }
        }
        public SceneNode MeshNode
        {
            get
            {
                return entNode;
            }
        }
        public Entity Mesh
        {
            get
            {
                return entity;
            }
        }
        public Actor PhysicsActor
        {
            get
            {
                return entityActor;
            }

            set
            {
                entityActor = value;
            }
        }
        public HealthInfo Health
        {
            get
            {
                return health;
            }
        }

        public GameObject(int id, GameWorld world)
        {
            this.id = id;
            this.world = world;
            camera = world.Camera;
            sceneManager = world.SceneManager;
            physicsScene = world.PhysicsScene;
            physics = world.PhysicsScene.Physics;
            moveInfo = null;
            health = new HealthInfo(this);
        }


        protected virtual void create(){ }
        public virtual void Update(float timeSinceLastFrame) { }

        public virtual void Dispose() { }
    }
}
