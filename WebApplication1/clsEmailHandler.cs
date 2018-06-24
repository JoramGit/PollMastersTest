using System;
using System.Collections.Generic;
using System.Linq;

using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Configuration;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using System.Web;
namespace WebApplication1
{
    public class clsEmailHandler
    {
        public static void SendInvitation(string email, string description, string sender)
        {

            string body = PopulateBody("", description, "", "", "~/EmailInvitationTemplate.htm");
            if (email != "")
            {
                SendHtmlFormattedEmail(email, string.Format("{0} Invited You to join", sender), body);
            }
        }
        
        
        public static void SendPoll(string pollButtons, string email, string title, string userName, string eventName)
        {

            string body = PopulateBody(userName, title, pollButtons, eventName, "~/EmailPollTemplate.htm");
            if (email != "")
            {
                SendHtmlFormattedEmail(email, string.Format("New Poll: {0}", title), body);
            }
        }

        private static string PopulateBody(string userName, string description, string buttonvalue, string eventName, string emailTemplate)
        {
            string ButtonsString = buttonvalue;
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(emailTemplate)))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{Event}", eventName);
            body = body.Replace("{UserName}", userName);
            //body = body.Replace("{Title}", title);
            //body = body.Replace("{Url}", url);
            body = body.Replace("{Description}", description);
            body = body.Replace("{Buttons}", buttonvalue);
            return body;
        }

        private static void SendHtmlFormattedEmail(string recepientEmail, string subject, string body)
        {
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["UserName"]);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                mailMessage.To.Add(new MailAddress(recepientEmail));
                SmtpClient smtp = new SmtpClient();


                smtp.Host = ConfigurationManager.AppSettings["Host"];
                smtp.EnableSsl = false;// Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                NetworkCred.UserName = ConfigurationManager.AppSettings["UserName"];
                NetworkCred.Password = ConfigurationManager.AppSettings["Password"];
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = int.Parse(ConfigurationManager.AppSettings["Port"]);



                //smtp.Host = ConfigurationManager.AppSettings["Host"];
                //smtp.Port = int.Parse(ConfigurationManager.AppSettings["Port"]);
                //smtp.UseDefaultCredentials = false;
                //smtp.EnableSsl = false;
                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;




                //          smtp.Host = "smtpout.europe.secureserver.net";
                //          smtp.Port = 25;
                //          smtp.UseDefaultCredentials = false;
                //          smtp.EnableSsl = false;
                //          smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //          smtp.Credentials = new System.Net.NetworkCredential("info@igabbai.com", "Netanel1");
                //          ServicePointManager.ServerCertificateValidationCallback =
                //delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                //{ return true; };

                smtp.Send(mailMessage);
            }
        }
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
    }
}