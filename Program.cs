using MimeKit;
using System;
using System.IO;
using System.Linq;

namespace youtubedlServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Settings settings;
            if (!File.Exists("settings.json"))
            {
                settings = Settings.CreateSettings();
            }
            else
            {
                settings = Settings.GetSettings();
            }

            ImapMailClient imapMailClient = new ImapMailClient(settings);
            if(imapMailClient.ConnectedAndAuthenticated == false)
            {
                Console.WriteLine("Login failed..abort..");
                imapMailClient.Disconnect();
                return;
            }
            var mails = imapMailClient.GetMails();
            imapMailClient.Disconnect();

            MailParser mailParser = new MailParser();
            var commands = mailParser.GetContent(mails,settings.AcceptedMailAddresses);

            CommandParser commandParser = new CommandParser();
            commandParser.ParseCommand(commands);

        }
    }
}
