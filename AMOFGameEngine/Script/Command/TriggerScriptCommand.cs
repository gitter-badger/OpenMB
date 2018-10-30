﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Script.Command
{
    public class TriggerScriptCommand : ScriptCommand
    {
        private string[] commandArgs;
        public override string[] CommandArgs
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string CommandName
        {
            get
            {
                return "trigger";
            }
        }

        public override ScriptCommandType CommandType
        {
            get
            {
                return ScriptCommandType.Block;
            }
        }

        public override List<IScriptCommand> SubCommands
        {
            get;
            set;
        }
        public TriggerScriptCommand()
        {
            commandArgs = new string[] {
                "TriggerName",
                "ExecuteTime",
                "FreezeTime"
            };
            SubCommands = new List<IScriptCommand>();
        }

        public override void Execute(params object[] executeArgs)
        {
            
        }
    }
}
