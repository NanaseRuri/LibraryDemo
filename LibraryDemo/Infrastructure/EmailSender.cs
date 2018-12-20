using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace LibraryDemo.Infrastructure
{
    public class EmailSender
    {
        IConfiguration emailConfig = new ConfigurationBuilder().AddJsonFile("Mail.json").Build().GetSection("Mail");
        public SmtpClient SmtpClient=new SmtpClient();

        public EmailSender()
        {            
            SmtpClient.EnableSsl = Boolean.Parse(emailConfig["UseSsl"]);
            SmtpClient.UseDefaultCredentials = bool.Parse(emailConfig["UseDefaultCredentials"]);
            SmtpClient.Credentials = new NetworkCredential(emailConfig["Username"], emailConfig["Password"]);
            SmtpClient.Port = Int32.Parse(emailConfig["ServerPort"]);
            SmtpClient.Host = emailConfig["ServerName"];
            SmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        }
    }
}
