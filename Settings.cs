using System;
using System.Text;
using System.Text.Json;
using System.IO;
using System.Numerics;
using System.Collections.Generic;

namespace youtubedlServer
{
    class Settings
    {
        public string MailHost { get; set; }
        public int MailPort { get; set; }
        public bool MailUseSSL { get; set; }
        public string MailUser { get; set; }
        public string ShiftedMailPassword { get; set; }

        public List<string> AcceptedMailAddresses { get; set; } = new List<string>();

        public string ExportFolder { get; set; }

        public static Settings CreateSettings()
        {
            Settings settings = new Settings();
            Console.WriteLine("Mail Host:");
            settings.MailHost = Console.ReadLine();
            Console.WriteLine("Mail Port:");
            int port;
            while (!int.TryParse(Console.ReadLine(), out port))
            {
                Console.WriteLine("only numbers... try again");
            }
            settings.MailPort = port;
            Console.WriteLine("Use SSL to communicate with Mailserver? (y)es or (n)o:");
            while (true)
            {
                var answer = Console.ReadLine();
                if (answer == "y")
                {
                    settings.MailUseSSL = true;
                    break;
                }
                else if (answer == "n")
                {
                    settings.MailUseSSL = false;
                    break;
                }
                else
                {
                    Console.WriteLine("Try again! only y or n");
                }
            }

            Console.WriteLine("Mail User:");
            settings.MailUser = Console.ReadLine();
            Console.WriteLine("Mail Password:");
            
            settings.ShiftPassword((string)Console.ReadLine());

            Console.WriteLine("Which Sender-Mail-Addresses are allowed for getting commands? " +
                "Paste the Mail-Address and press after each enter! After you are finished, type 'end'");
            string response;
            while((response=Console.ReadLine())!="end")
            {
                settings.AcceptedMailAddresses.Add(response);
            }
            Console.WriteLine("Export folder path:");
            settings.ExportFolder = Console.ReadLine();

            File.WriteAllText("settings.json",settings.ToString());

            return settings;

        }

        public static Settings GetSettings()
        {
            return JsonSerializer.Deserialize<Settings>(File.ReadAllText("settings.json"));
        }



        //shift password before save them -not secure but also not plaintext
        //secure settings.json via fileaccess restrictions
        private void ShiftPassword(string password)
        {
            var chara = password.ToCharArray();
            StringBuilder shiftedPassword = new StringBuilder(chara.Length);
            for (int i = 0; i < chara.Length; i++)
            {
                var charint = Convert.ToUInt32(chara[i]);
                charint = BitOperations.RotateLeft(charint, 2);
                shiftedPassword.Append(Convert.ToChar(charint));
            }
            ShiftedMailPassword = shiftedPassword.ToString();
        }
        public string UnshiftPassword()
        {
            var chara = ShiftedMailPassword.ToCharArray();
            StringBuilder unshiftedPassword = new StringBuilder(chara.Length);
            for (int i = 0; i < chara.Length; i++)
            {
                var charint = Convert.ToUInt32(chara[i]);
                charint = BitOperations.RotateRight(charint, 2);
                unshiftedPassword.Append(Convert.ToChar(charint));
            }
            return unshiftedPassword.ToString();
        }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}

