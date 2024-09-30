using System.Net.Mail;
using System.Net;
using CatechistHelper.Domain.Common;

namespace CatechistHelper.Infrastructure.Utils
{
    public static class MailUtil
    {
        public static string SendEmail(string to, string subject, string body, string attachFile)
        {
            try
            {
                MailMessage msg = new MailMessage(AppConfig.MailSetting.EmailSender, to, subject, body);
                msg.IsBodyHtml = true;

                using (var client = new SmtpClient(AppConfig.MailSetting.HostEmail, AppConfig.MailSetting.PortEmail))
                {
                    client.EnableSsl = true;
                    if (!string.IsNullOrEmpty(attachFile))
                    {
                        Attachment attachment = new Attachment(attachFile);
                        msg.Attachments.Add(attachment);
                    }
                    NetworkCredential credential = new NetworkCredential(AppConfig.MailSetting.EmailSender, AppConfig.MailSetting.PasswordSender);
                    client.UseDefaultCredentials = false;
                    client.Credentials = credential;
                    client.Send(msg);
                }


            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "";
        }
    }
}
