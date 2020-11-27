using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace youtubedlServer
{
    class CommandParser
    {
        public Dictionary<string, string> CommandAndValue { get; set; } = new Dictionary<string, string>();

        public void ParseCommand(List<string> commands)
        {
            foreach (var command in commands)
            {
                var commandArr = command.Split(":");
                CommandAndValue.Add(commandArr[0], commandArr[1]);
            }
        }

        public void ExecuteCommands()
        {
            foreach (var CAV in CommandAndValue)
            {
                ExecuteCommand(CAV);
            }
        }

        private void ExecuteCommand(KeyValuePair<string, string> cAV)
        {
            switch (cAV.Key.ToUpper())
            {
                case "GET":
                    
                    break;
                default:
                    break;
            }
        }
    }
}
