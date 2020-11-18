using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;

namespace youtubedlServer
{
    class ImapMailClient
    {
        public ImapClient Client { get; set; }

        public bool ConnectedAndAuthenticated { get; set; }

        public IMailFolder CurrentFolder { get; set; }

        public ImapMailClient(Settings settings)
        {
            Client = new ImapClient();
            Client.Connect(settings.MailHost, settings.MailPort, settings.MailUseSSL);
            Client.Authenticate(settings.MailUser, settings.UnshiftPassword());
            if (Client.IsConnected && Client.IsAuthenticated)
            {
                ConnectedAndAuthenticated = true;
            }
            else
            {
                ConnectedAndAuthenticated = false;
            }
            
        }

        public List<MimeMessage> GetMails()
        {
            if (Client.IsConnected==false)
            {
                return null;
            }
            List<MimeMessage> messages = new List<MimeMessage>();
            foreach (var uid in Client.Inbox.Search(SearchQuery.NotSeen))
            {
                messages.Add(Client.Inbox.GetMessage(uid));
                Client.Inbox.SetFlags(uid, MessageFlags.Seen, true);
            }
            Client.Disconnect(true);
            return messages;
        }

        public void Disconnect()
        {
            if (Client.IsConnected)
            {
                Client.Disconnect(true);
            }
        }
        
    }
}
