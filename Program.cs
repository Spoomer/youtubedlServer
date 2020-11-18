using System;
using System.IO;

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
            foreach (var mail in mails)
            {
                Console.WriteLine(mail.Body.ContentType.MimeType);
            }
            imapMailClient.Disconnect();

        }
    }
}
