using System.Collections.Generic;
using System.Net.Mail;
using System.Configuration;
using System.IO;
using Reusable.Attachments;

namespace Reusable.Email
{
    public class EmailService
    {
        private SmtpClient server;
        public string EmailAddress { get; set; }
        public string Password { get; set; }

        public List<string> To = new List<string>();
        public List<string> Cc = new List<string>();
        public List<string> Bcc = new List<string>();

        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string AttachmentsFolder { get; set; }

        public EmailService(string EmailServer, int EmailPort)
        {
            server = new SmtpClient(EmailServer, EmailPort);
        }

        public void SendMail()
        {
            try
            {
                server.EnableSsl = true;
                server.DeliveryMethod = SmtpDeliveryMethod.Network;
                server.UseDefaultCredentials = false;
                server.Credentials = new System.Net.NetworkCredential(EmailAddress, Password);

                MailMessage message = new MailMessage();
                message.From = new MailAddress(From, From);

                foreach (var to in To)
                {
                    message.To.Add(new MailAddress(to));
                }
                foreach (var cc in Cc)
                {
                    message.CC.Add(new MailAddress(cc));
                }
                foreach (var bcc in Bcc)
                {
                    message.Bcc.Add(new MailAddress(bcc));
                }
                message.Subject = Subject;
                message.IsBodyHtml = true;
                message.BodyEncoding = System.Text.Encoding.UTF8;

                message.Body = Body;

                string baseAttachmentsPath = ConfigurationManager.AppSettings["EmailAttachments"];
                var attachments = AttachmentsIO.getAttachmentsFromFolder(AttachmentsFolder, "EmailAttachments");
                foreach (var attachment in attachments)
                {
                    string filePath = baseAttachmentsPath + attachment.Directory + "\\" + attachment.FileName;
                    FileInfo file = new FileInfo(filePath);
                    message.Attachments.Add(new System.Net.Mail.Attachment(new FileStream(filePath, FileMode.Open, FileAccess.Read), attachment.FileName));
                }

                server.Send(message);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
