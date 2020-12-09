using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace youtubedlServer
{
    class MailParser
    {
        public List<string> Commands { get; set; } = new List<string>();
        public List<string> GetContent(List<MimeMessage> mails, List<string> acceptedMailAddresses)
        {
            foreach (var mail in mails)
            {
                if (acceptedMailAddresses.Contains(mail.From.Mailboxes.First().Address))
                {
                    using (var iter = new MimeIterator(mail))
                    {
                        while (iter.MoveNext())
                        {
                            Console.WriteLine(iter.Current.ContentType.MimeType);
                            var textpart = iter.Current as TextPart;
                            if (textpart != null)
                            {
                                //split text in lines with different linebreaks
                                string[] lines = textpart.Text.Split(
                                                new[] { "\r\n", "\r", "\n" },
                                                StringSplitOptions.None
                                                );
                                //erase empty elements e.g. empty line in the mail
                                lines = lines.Where(line => !string.IsNullOrEmpty(line)).ToArray();
                                Commands.AddRange(lines);
                            }

                        }
                    }                   
                }
            }
            return Commands;
        }
    }
}
