﻿using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script.Command
{
    class AssignScriptCommand : ScriptCommand
    {
        private string[] commandArgs;
        public AssignScriptCommand()
        {
            commandArgs = new string[] {
				"destValue",
				"srcValue"
			};
        }
        public override string[] CommandArgs
        {
            get
            {
                return commandArgs;
            }
        }

        public override string CommandName
        {
            get
            {
                return "assign";
            }
        }

        public override ScriptCommandType CommandType
        {
            get
            {
                return ScriptCommandType.Line;
            }
        }

        public override void Execute(params object[] executeArgs)
        {
            if (CommandArgs.Length == 2)
            {
                GameWorld world = executeArgs[0] as GameWorld;

                string varname = (string)CommandArgs[0];
                string varvalue = (string)CommandArgs[1];

                string realData = null;
                if (varvalue.StartsWith("%"))//local var
                {
                    realData = Context.GetLocalValue(varvalue.Substring(1));
                }
                else if (varvalue.StartsWith("$"))//global var
                {
                    realData = world.GetGlobalValue(varvalue.Substring(1));
                }
                else
                {
                    realData = varvalue;
                }

                if (varname.StartsWith("%"))//local var
                {
                    Context.ChangeLocalValue(varname.Substring(1), realData);
                }
                else if(varname.StartsWith("$"))//global var
                {
                    world.ChangeGobalValue(varname.Substring(1), realData);
                }
            }
            else
            {
                GameManager.Instance.log.LogMessage("[Script Error]: Assign: Invalid argument number");
            }
        }
    }
}
