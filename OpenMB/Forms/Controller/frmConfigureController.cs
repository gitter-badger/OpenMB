﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mogre;
using OpenMB.Configure;
using OpenMB.Forms.Model;
using OpenMB.Localization;
using OpenMB.Core;

namespace OpenMB.Forms.Controller
{
    public class frmConfigureController
    {
        private Root root;
        private LOCATE selectedlocate;
        private GameConfigXml gameXmlConfig;
        public frmConfigure form;
        public AudioConfigure AudioConfig;
        public GameConfigure GameConfig;
        public GraphicConfigure GraphicConfig;
        public ResourceConfigure ResourceConfig;
        public LOCATE CurrentLoacte
        {
            get
            {
                return selectedlocate;
            }
        }

        public frmConfigureController(frmConfigure form)
        {
            this.form = form;

            root = new Root();

            AudioConfig = new AudioConfigure();
            GameConfig = new GameConfigure();
            GraphicConfig = new GraphicConfigure();
            ResourceConfig = new ResourceConfigure();
            gameXmlConfig = GameConfigXml.Load("game.xml", root);

            form.Controller = this;
        }

        public void Init()
        {
            LoadGraphicConfigure();
            LoadAudioConfigure();
            LoadGameConfigure();
            LoadResourceConfigure();
        }

        private void LoadPluginConfigure()
        {

        }

        private void LoadResourceConfigure()
        {
            ResourceConfig.ResourceRootDir = gameXmlConfig.ResourcesConfig.ResourceRootDir;

            var fileSystemResXmls = gameXmlConfig.ResourcesConfig.Resources.Where(o => o.Type == "FileSystem");
            if (fileSystemResXmls.Count() > 0)
            {
                var fileSystemResXml = fileSystemResXmls.ElementAt(0);
                foreach (var fileSystemResource in fileSystemResXml.ResourceLocs)
                {
                    ResourceConfig.FileSystemResources.Add(fileSystemResource);
                }
            }
            var zipResXmls = gameXmlConfig.ResourcesConfig.Resources.Where(o => o.Type == "Zip");
            if (zipResXmls.Count() > 0)
            {
                var zipResXml = zipResXmls.ElementAt(0);
                foreach (var fileSystemResource in zipResXml.ResourceLocs)
                {
                    ResourceConfig.ZipResources.Add(fileSystemResource);
                }
            }
        }

        private void LoadGameConfigure()
        {
            selectedlocate = LocateSystem.Instance.ConvertReadableStringToLocate(gameXmlConfig.LocateConfig.CurrentLocate);
            GameConfig.CurrentSelectedLocate = LocateSystem.Instance.ConvertLocateShortStringToReadableString(selectedlocate.ToString());
            GameConfig.IsEnableEditMode = gameXmlConfig.CoreConfig.IsEnableEditMode;
        }

        private void LoadGraphicConfigure()
        {
            if (!string.IsNullOrEmpty(gameXmlConfig.GraphicConfig.CurrentRenderSystem) && gameXmlConfig.GraphicConfig.Renderers.Count>0)
            {
                for (int i = 0; i < gameXmlConfig.GraphicConfig.Renderers.Count; i++)
                {
                    GraphicConfig.RenderSystemNames.Add(gameXmlConfig.GraphicConfig.Renderers[i].Name);
                }
                GraphicConfig.RenderSystem = gameXmlConfig.GraphicConfig.CurrentRenderSystem;
                GetGraphicSettingsByName(GraphicConfig.RenderSystem);
            }
            else
            {
                var renderers = root.GetAvailableRenderers();
                foreach(var renderer in renderers)
                {
                    GraphicConfig.RenderSystemNames.Add(renderer.Name);
                }
                if (GraphicConfig.RenderSystemNames.Count > 0)
                {
                    GetGraphicSettingsByName(GraphicConfig.RenderSystemNames[0]);
                }
            }
        }

        internal void InitLocates()
        {
            GameConfig.AvaliableLocates.Clear();
            foreach (var locateStr in LocateSystem.Instance.AvaliableLocates)
            {
                GameConfig.AvaliableLocates.Add(LocateSystem.Instance.ConvertLocateShortStringToReadableString(locateStr));
            }
        }

        private void LoadAudioConfigure()
        {
            AudioConfig.IsEnableSound = gameXmlConfig.AudioConfig.EnableSound;
            AudioConfig.IsEnableMusic = gameXmlConfig.AudioConfig.EnableMusic;
        }

        public void GetGraphicSettingsByName(string renderSystemName)
        {
            gameXmlConfig.GraphicConfig.CurrentRenderSystem = renderSystemName;
            GraphicConfig.RenderParams.Clear();
            ConfigOptionMap configOptionMap = root.GetRenderSystemByName(renderSystemName).GetConfigOptions();
            if (gameXmlConfig.GraphicConfig.Renderers.Count > 0)
            {
                List<GameGraphicParameterConfigXml> dic = gameXmlConfig.GraphicConfig[renderSystemName];
                List<string> graphicSettings = new List<string>();
                if (dic != null && dic.Count > 0)
                {
                    for (int i = 0; i < configOptionMap.Count; i++)
                    {
                        if (configOptionMap[dic[i].Name].possibleValues.IsEmpty)
                        {
                            continue;
                        }
                        GraphicConfig.RenderParams.Add(dic[i].Name + ":" + (configOptionMap[dic[i].Name].possibleValues.Contains(dic[i].Value) ? dic[i].Value : configOptionMap[dic[i].Name].possibleValues[0]));
                    }
                }
                else
                {
                    foreach (var pair in configOptionMap)
                    {
                        GraphicConfig.RenderParams.Add(pair.Key + ":" + pair.Value.possibleValues[0]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < configOptionMap.Count; i++)
                {
                    GraphicConfig.RenderParams.Add(configOptionMap.ElementAt(i).Key + ":" + configOptionMap[configOptionMap.ElementAt(i).Key].possibleValues[0]);
                }
            }
        }

        public void InsertPossibleValue(string renderSystemName, string renderConfigKey, string renderConfigValue)
        {
            GraphicConfig.PossibleValues.Clear();
            ConfigOptionMap configOptionMap = root.GetRenderSystemByName(renderSystemName).GetConfigOptions();

            foreach (string psv in configOptionMap[renderConfigKey].possibleValues)
            {
                GraphicConfig.PossibleValues.Add(psv);
            }
            GraphicConfig.CurrentPossibleValue = renderConfigValue;
        }

        public void UpdateGraphicConfigByValue(string renderSystemName, string renderConfigKey, string renderConfigNewValue)
        {
            gameXmlConfig.GraphicConfig[gameXmlConfig.GraphicConfig.CurrentRenderSystem].Where(o => o.Name == renderConfigKey).First().Value = renderConfigNewValue;
            GetGraphicSettingsByName(renderSystemName);
        }

        public bool CheckConfigure()
        {
            if (gameXmlConfig.LocateConfig.CurrentLocate != GameConfig.CurrentSelectedLocate.ToString())
            {
                return true;
            }

            return false;
        }

        public GameConfigXml SaveConfigure()
        {
            if (string.IsNullOrEmpty(gameXmlConfig.ModConfig.ModDir))
            {
                gameXmlConfig.ModConfig.ModDir = "Mods/";
            }

			gameXmlConfig.CoreConfig.IsEnableCheatMode = GameConfig.IsEnableCheatMode;
            gameXmlConfig.CoreConfig.IsEnableEditMode = GameConfig.IsEnableEditMode;

			gameXmlConfig.AudioConfig.EnableMusic = AudioConfig.IsEnableMusic;
            gameXmlConfig.AudioConfig.EnableSound = AudioConfig.IsEnableSound;

			if (gameXmlConfig.LocateConfig.CurrentLocate != GameConfig.CurrentSelectedLocate.ToString())
            {
                LocateSystem.Instance.InitLocateSystem(LocateSystem.Instance.ConvertReadableStringToLocate(GameConfig.CurrentSelectedLocate.ToString()));// Init Locate System
            }
            gameXmlConfig.LocateConfig.CurrentLocate = GameConfig.CurrentSelectedLocate.ToString();

			gameXmlConfig.GraphicConfig.CurrentRenderSystem = GraphicConfig.RenderSystem;

            gameXmlConfig.ResourcesConfig.Resources.Clear();
            gameXmlConfig.ResourcesConfig.Resources.Add(new GameResourceConfigXml() { Type = "FileSystem", ResourceLocs = new List<string>() });
            foreach (var resource in ResourceConfig.FileSystemResources)
            {
                gameXmlConfig.ResourcesConfig.Resources[0].ResourceLocs.Add(resource);
            }
            gameXmlConfig.ResourcesConfig.Resources.Add(new GameResourceConfigXml() { Type = "Zip", ResourceLocs = new List<string>() });
            foreach (var resource in ResourceConfig.ZipResources)
            {
                gameXmlConfig.ResourcesConfig.Resources[1].ResourceLocs.Add(resource);
            }

			gameXmlConfig.Save("game.xml");

            return gameXmlConfig;
        }
    }
}
