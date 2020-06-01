﻿using System;
using System.Collections.Generic;
using Mogre;
using Mogre_Procedural.MogreBites;
using OpenMB.Mods;
using OpenMB.Network;
using OpenMB.Widgets;

namespace OpenMB.States
{
    public class Multiplayer : AppState
    {
        private delegate bool ServerStartDelegate();
        private GameServer thisServer;
        private Dictionary<string, string> option;
        private StringVector serverState;
        private ParamsPanelWidget serverpanel;
        private bool isEscapeMenuOpened;

        public Multiplayer()
        {
            option = new Dictionary<string, string>();
            serverState = new StringVector();
			thisServer = new GameServer();
		}

        public override void enter(Mods.ModData e = null)
        {
            modData = e;
            sceneMgr = GameManager.Instance.root.CreateSceneManager(Mogre.SceneType.ST_GENERIC, "MenuSceneMgr");
            ColourValue cvAmbineLight = new ColourValue(0.7f, 0.7f, 0.7f);
            sceneMgr.AmbientLight = cvAmbineLight;
            camera = sceneMgr.CreateCamera("multiplayerCam");
            GameManager.Instance.viewport.Camera = camera;
            camera.AspectRatio = GameManager.Instance.viewport.ActualWidth / GameManager.Instance.viewport.ActualHeight;
            GameManager.Instance.viewport.OverlaysEnabled = true;

            GameManager.Instance.keyboard.KeyPressed += new MOIS.KeyListener.KeyPressedHandler(mKeyboard_KeyPressed);
            GameManager.Instance.keyboard.KeyReleased += new MOIS.KeyListener.KeyReleasedHandler(mKeyboard_KeyReleased);

            BuildGameListUI();
        }

        #region UI

        private void BuildGameListUI()
        {
            UIManager.Instance.CreateButton(UIWidgetLocation.TL_RIGHT, "btnJoin", "Join",50);
            UIManager.Instance.CreateButton(UIWidgetLocation.TL_RIGHT, "btnHost", "Host", 50);
            UIManager.Instance.CreateButton(UIWidgetLocation.TL_RIGHT, "btnExit", "Exit", 50);
        }
        void HostGameUI()
        {
        }

        private void BuildEscapeMenu()
        {
            UIManager.Instance.DestroyAllWidgets();
            UIManager.Instance.CreateButton(UIWidgetLocation.TL_CENTER, "choose_side", "Choose Side", 200f);
            UIManager.Instance.CreateButton(UIWidgetLocation.TL_CENTER, "choose_chara", "Choose Character", 200f);
            UIManager.Instance.CreateButton(UIWidgetLocation.TL_CENTER, "exit_multiplayer", "Exit", 200f);
            this.isEscapeMenuOpened = true;
        }
        #endregion


        void Server_OnEscapePressed()
        {
            ShowEscapeMenu();
        }

        private void ShowEscapeMenu()
        {
        }

        bool mKeyboard_KeyReleased(MOIS.KeyEvent arg)
        {
            return true;
        }

        bool mKeyboard_KeyPressed(MOIS.KeyEvent arg)
        {
            if (arg.key == MOIS.KeyCode.KC_ESCAPE)
            {
                if (!this.isEscapeMenuOpened)
                {
                    this.BuildEscapeMenu();
                }
                else
                {
                    UIManager.Instance.DestroyAllWidgets();
                    this.serverpanel = UIManager.Instance.CreateParamsPanel(UIWidgetLocation.TL_CENTER, "serverpanel", 400f, this.serverState);
                    this.isEscapeMenuOpened = false;
                }
            }
            return true;
        }

        public bool ServerStart()
        {
            thisServer.Init();
            return thisServer.Go();
        }

        public override bool pause()
        {
            return base.pause();
        }

        public override void update(double timeSinceLastFrame)
        {
            if (thisServer!=null&&thisServer.Started)
            {
                thisServer.Update();
                thisServer.GetServerState(ref serverState);
                if(!isEscapeMenuOpened)
                    serverpanel.SetAllParamValues(serverState);
            }
        }

        public override void exit()
        {
            if (sceneMgr != null)
            {
                sceneMgr.DestroyCamera(camera);
                GameManager.Instance.root.DestroySceneManager(sceneMgr);
            }
            if (thisServer != null)
            {
                thisServer.Exit();
            }
        }
    }
}
