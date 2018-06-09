﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mogre;
using AMOFGameEngine.Forms.Model;
using AMOFGameEngine.Localization;
using AMOFGameEngine.Utilities;

namespace AMOFGameEngine.Forms.Controller
{
    public class frmConfigureController
    {
        private Root r;
        private LOCATE selectedlocate;
        private AMOFGameEngine.Utilities.ConfigFile gameCfg;
        private AMOFGameEngine.Utilities.ConfigFile ogreCfg;
        private ConfigFileParser parser;
        public frmConfigure form;
        public AudioConfigure AudioConfig;
        public GameConfigure GameConfig;
        public GraphicConfigure GraphicConfig;
        //public event Action GraphicRenderSystemChanged;
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

            AudioConfig = new AudioConfigure();
            GameConfig = new GameConfigure();
            GraphicConfig = new GraphicConfigure();

            parser = new ConfigFileParser();
            gameCfg = parser.Load("game.cfg");
            ogreCfg = parser.Load("ogre.cfg");
            r = new Root();

            form.Controller = this;
        }

        public void Init()
        {
            LoadGraphicConfigure();
            LoadAudioConfigure();
            LoadGameConfigure();

        }

        private void LoadGameConfigure()
        {
            selectedlocate = LocateSystem.Singleton.ConvertLocateShortStringToLocateInfo(gameCfg["Localized"]["Current"]);
            GameConfig.CurrentSelectedLocate = LocateSystem.Singleton.CovertLocateInfoStringToReadableString(selectedlocate.ToString());
            GameConfig.AvaliableLocates.Clear();
            DirectoryInfo di = new DirectoryInfo("./locate/");
            FileSystemInfo[] fsi = di.GetFileSystemInfos();
            foreach (var dir in fsi)
            {
                if(File.Exists(string.Format(@"{0}\GameQuickString.ucs", dir.FullName))&&
                    File.Exists(string.Format(@"{0}\GameStrings.ucs", dir.FullName)) &&
                    File.Exists(string.Format(@"{0}\GameUI.ucs", dir.FullName)))
                {
                    //valid locate directory
                    LocateSystem.Singleton.RegisterLocate(dir.Name);
                    GameConfig.AvaliableLocates.Add(LocateSystem.Singleton.CovertLocateInfoStringToReadableString(dir.Name));
                }
            }
        }

        private void LoadGraphicConfigure()
        {
            for (int i = 0; i < ogreCfg.Sections.Count; i++)
            {
                if(!string.IsNullOrEmpty(ogreCfg.Sections[i].Name))
                {
                    GraphicConfig.RenderSystemNames.Add(ogreCfg.Sections[i].Name);
                }
            }
            GraphicConfig.RenderSystem = ogreCfg[""]["Render System"];
            GetGraphicSettingsByName(GraphicConfig.RenderSystem);
        }

        private void LoadAudioConfigure()
        {
            if (gameCfg["Audio"]["EnableSound"] == "1")
            {
                AudioConfig.IsEnableSound = true;
            }
            else
            {
                AudioConfig.IsEnableSound = false;
            }
            if (gameCfg["Audio"]["EnableMusic"] == "1")
            {
                AudioConfig.IsEnableMusic = true;
            }
            else
            {
                AudioConfig.IsEnableMusic = false;
            }
        }

        public void GetGraphicSettingsByName(string renderSystemName)
        {
            GraphicConfig.RenderParams.Clear();
            List<ConfigFileKeyValuePair> dic = ogreCfg[renderSystemName].KeyValuePairs;
            List<string> graphicSettings = new List<string>();
            if (dic != null)
            {
                for (int i = 0; i < dic.Count; i++)
                {
                    GraphicConfig.RenderParams.Add(dic[i].Key + ":" + dic[i].Value);
                }
            }
        }

        public void InsertPossibleValue(string renderSystemName, string renderConfigKey, string renderConfigValue)
        {
            GraphicConfig.PossibleValues.Clear();
            ConfigOptionMap configOptionMap = r.GetRenderSystemByName(renderSystemName).GetConfigOptions();

            foreach (string psv in configOptionMap[renderConfigKey].possibleValues)
            {
                GraphicConfig.PossibleValues.Add(psv);
            }
            GraphicConfig.CurrentPossibleValue = renderConfigValue;
        }

        public void UpdateGraphicConfigByValue(string renderSystemName, string renderConfigKey, string renderConfigNewValue)
        {
            ogreCfg[renderSystemName][renderConfigKey] = renderConfigNewValue;
            GetGraphicSettingsByName(renderSystemName);
        }

        public Tuple<Dictionary<string,string>,AMOFGameEngine.Utilities.ConfigFile> SaveConfigure()
        {
            Dictionary<string, string> gameOptions = new Dictionary<string, string>();

            gameOptions.Add("IsEnableMusic", AudioConfig.IsEnableMusic.ToString());
            gameOptions.Add("IsEnableSound", AudioConfig.IsEnableSound.ToString());
            gameOptions.Add("Language", GameConfig.CurrentSelectedLocate.ToString());

            ogreCfg[""]["Render System"] = GraphicConfig.RenderSystem;
            var kpls = ogreCfg[GraphicConfig.RenderSystem].KeyValuePairs;
            int length = kpls.Count;
            for (int i = 0; i < length; i++)
            {
                kpls[i].Value = GraphicConfig.RenderParams[i].Split(':')[1];
            }
            gameCfg["Audio"]["EnableMusic"] = AudioConfig.IsEnableMusic ? "1" : "0";
            gameCfg["Audio"]["EnableSound"] = AudioConfig.IsEnableSound ? "1" : "0";
            gameCfg["Localized"]["Current"] = LocateSystem.Singleton.CovertReadableStringToLocateShortString(GameConfig.CurrentSelectedLocate);

            parser.Save(gameCfg);
            parser.Save(ogreCfg);

            return new Tuple<Dictionary<string,string>,Utilities.ConfigFile>(gameOptions, ogreCfg);
        }
    }
}