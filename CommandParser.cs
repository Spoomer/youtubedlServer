using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace youtubedlServer
{
    class CommandParser
    {
        public Settings _Settings { get; set; }
        public Dictionary<string, string> CommandAndValue { get; set; } = new Dictionary<string, string>();

        public CommandParser(Settings settings)
        {
            _Settings = settings;
        }
        public void ParseCommand(List<string> commands)
        {
            foreach (var command in commands)
            {
                var commandArr = command.Split("<>");
                CommandAndValue.Add(commandArr[0], commandArr[1]);
            }
            ExecuteCommands();
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
            YoutubeDLController youtubeDLController = YoutubeDLController.Instance();
            youtubeDLController.Output = Path.Combine(_Settings.ExportFolder, "%(title)s");
            youtubeDLController.VideoUrl = cAV.Value;
            switch (cAV.Key.ToUpper())
            {
                case "GET-VIDEO":
                    youtubeDLController.Download();
                    break;
                default:
                    break;
            }
        }
    }
}
