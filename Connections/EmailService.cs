using SendGrid;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Connections.SendGrid
{
    public class EmailService
    {
        public static async Task SendAsync(string recipient, string subject, string body)
        {
            await configSendGridasync(recipient, subject, body);
        }

        private static async Task configSendGridasync(string recipient, string subject, string body)
        {
            var myMessage = new SendGridMessage();
            myMessage.AddTo(recipient);
            myMessage.From = new System.Net.Mail.MailAddress(
                                "DONOTREPLY@emptech.com", "Emptech E-Mail");
            myMessage.Subject = subject;
            myMessage.Text = body;
            myMessage.Html = body;

            var credentials = new NetworkCredential(
                       ConfigurationManager.AppSettings["emailServiceSendGridUserName"],
                       ConfigurationManager.AppSettings["emailServiceSendGridPassword"]
                       );

            // Create a Web transport for sending email.
            var transportWeb = new Web(credentials);

            // Send the email.
            if (transportWeb != null)
            {
                await transportWeb.DeliverAsync(myMessage);
            }
            else
            {
                Trace.TraceError("Failed to create Web transport.");
                await Task.FromResult(0);
            }
        }
    }

}
