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
            message.From = new MailAddress("info@inzenjer.in");

            message.To.Add(new MailAddress(emailTo));

            message.Subject = subject;
            message.Body = body;

            SmtpClient client = new SmtpClient();
            client.Host = "relay-hosting.secureserver.net";
            client.Port = 25;
            client.UseDefaultCredentials = false;
            client.EnableSsl = false;
            client.Credentials = new NetworkCredential("info@inzenjer.in", "InZenjer@2017");
            client.Send(message);
        }
    }
}