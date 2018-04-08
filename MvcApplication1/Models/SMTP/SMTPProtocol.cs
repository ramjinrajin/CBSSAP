using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace MvcApplication1.Models.SMTP
{


    public static class SMTPProtocol
    {
        public static void NotifyPartners(string subject, string body, string emailTo)
        {
          
            MailMessage message = new MailMessage();
            message.From = new MailAddress("otp_admin@inzenjer.in");
            message.To.Add(new MailAddress(emailTo));

            message.Subject = subject;
            message.Body =body;
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.Host = "mail.inzenjer.in";
            client.Port = 25;
            client.UseDefaultCredentials = false;
            client.EnableSsl = false;
            client.Credentials = new NetworkCredential("otp_admin@inzenjer.in", "admin@1");
            client.Send(message);
        }
    }
}