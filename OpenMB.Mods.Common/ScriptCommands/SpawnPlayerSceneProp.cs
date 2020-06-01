﻿using OpenMB.Game;
using OpenMB.Script.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace OpenMB.Mods.Common.ScriptCommands
{
    /// <summary>
    /// Spawn a scene prop and make player can control it
    /// </summary>
    public class SpawnPlayerScenePropScriptCommand : ScriptCommand
    {
        private string[] commandArgs;
        public override string CommandName
        {
            get
            {
                return "spawn_player_scene_prop";
            }
        }

        public override ScriptCommandType CommandType
        {
            get
            {
                return ScriptCommandType.Line;
            }
        }
        public override string[] CommandArgs
        {
            get
            {
                return commandArgs;
            }
        }

        public SpawnPlayerScenePropScriptCommand()
        {
            commandArgs = new string[]
            {
                "scenePropId",
                "position"
            };
        }

        public override void Execute(params object[] executeArgs)
        {
            GameWorld world = executeArgs[0] as GameWorld;
            string vectorName = getParamterValue(CommandArgs[1], world);

            var vector = world.GlobalValueTable.GetRecord(vectorName);
            if (vector == null)
            {
                GameManager.Instance.log.LogMessage("Invalid Vector Name!", LogMessage.LogType.Error);
                return;
            }
            world.CreatePlayerSceneProp(getParamterValue(CommandArgs[0], world),
                new Vector3() {
                    x = float.Parse(vector.NextNodes[0].Value),
                    y = float.Parse(vector.NextNodes[1].Value),
                    z = float.Parse(vector.NextNodes[2].Value),
                });
        }
    }
}
