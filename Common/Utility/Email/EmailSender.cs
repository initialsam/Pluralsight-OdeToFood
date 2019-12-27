using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utility.Email
{
   
    public class EmailSender
    {
        //private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string Host { get; set; }
        public int Port { get; set; }
        public bool SslRequired { get; set; }
        public bool AuthRequired { get; set; }
        public NetworkCredential Credential { get; set; }
        public string SenderAddress { get; set; }
        public List<string> ReceiverAddresses { get; set; }
        public List<string> BCCReceiverAddresses { get; set; }
        private List<EmailFileDTO> EmailFiles { get; set; } = new List<EmailFileDTO>();

        public string AttachmentFilePhysicalPath { get; set; }
        public Encoding TextEncode { get; set; }

        public EmailSender(SMTPServerDTO smtp, List<string> receivers, List<string> bccReceivers)
        {
            this.Host = smtp.SMTP;
            this.Port = smtp.SMTPport;
            this.SenderAddress = smtp.SenderMailAddress;
            this.Credential = new NetworkCredential(smtp.SenderMailAddress, smtp.SenderPassword);
            this.SslRequired = smtp.SslRequired;
            this.AuthRequired = smtp.AuthRequired;
            this.ReceiverAddresses = receivers;
            this.BCCReceiverAddresses = bccReceivers;
            this.TextEncode = Encoding.UTF8;
        }
        /// <summary>
        /// 夾帶檔案
        /// </summary>
        public void AttachmentFile(MemoryStream memoryStream, string filename)
        {
            EmailFiles.Add(new EmailFileDTO() { FileName = filename, MemoryStream = memoryStream });
        }
        /// <summary>
        /// 夾帶檔案
        /// </summary>
        public void AttachmentFile(byte[] bytes, string filename)
        {
            EmailFiles.Add(new EmailFileDTO() { FileName = filename, MemoryStream = new MemoryStream(bytes) });
        }
        public async Task SendMailAsync(string subject, string body)
        {
            SmtpClient sender;
            MailMessage mail;
            SetSender(subject, body, out sender, out mail);
            await sender.SendMailAsync(mail);
        }
      
        public bool SendMail(string subject, string body)
        {
            try
            {
                SmtpClient sender;
                MailMessage mail;
                SetSender(subject, body, out sender, out mail);
                sender.Send(mail);
                if (mail.Attachments != null)
                {
                    for (int i = mail.Attachments.Count - 1; i >= 0; i--)
                    {
                        mail.Attachments[i].Dispose();
                    }
                    mail.Attachments.Clear();
                    mail.Attachments.Dispose();
                }
                mail.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                //_log.WarnFormat("EmailSender Send fail, {0}", ex);
                return false;
            }
        }

        private void SetSender(string subject, string body, out SmtpClient sender, out MailMessage mail)
        {
            sender = new SmtpClient(this.Host, this.Port);
            if (this.AuthRequired && this.Credential != null)
                sender.Credentials = this.Credential;
            sender.EnableSsl = this.SslRequired;

            mail = new MailMessage();
            mail.BodyEncoding = this.TextEncode;
            mail.SubjectEncoding = this.TextEncode;
            mail.From = new MailAddress(this.SenderAddress);
            foreach (string receiver in this.ReceiverAddresses.Where(x => !String.IsNullOrWhiteSpace(x)))
            {
                mail.To.Add(receiver);
            }
            foreach (string receiver in this.BCCReceiverAddresses.Where(x => !String.IsNullOrWhiteSpace(x)))
            {
                mail.Bcc.Add(receiver);
            }
            mail.IsBodyHtml = true;
            mail.Subject = subject;
            mail.Body = body;
            AttachmentFile(mail);
        }

        /// <summary>
        /// 夾帶檔案
        /// </summary>
        /// <param name="mail"></param>
        private void AttachmentFile(MailMessage mail)
        {
            foreach (var file in EmailFiles)
            {
                mail.Attachments.Add(new Attachment(file.MemoryStream, file.FileName));
            }

            if(!String.IsNullOrEmpty(AttachmentFilePhysicalPath))
            {
                mail.Attachments.Add(new Attachment(AttachmentFilePhysicalPath));
            }
        }

     
    }
}
