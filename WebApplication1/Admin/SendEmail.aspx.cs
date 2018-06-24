using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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


public partial class _Default : System.Web.UI.Page
{
    protected void SendEmail(object sender, EventArgs e)
    {
        string body = this.PopulateBody("John",
            "Fetch multiple values as Key Value pair in ASP.Net AJAX AutoCompleteExtender",
            "http://www.aspsnippets.com/Articles/Fetch-multiple-values-as-Key-Value-pair-" + 
            "in-ASP.Net-AJAX-AutoCompleteExtender.aspx",
            "Here Mudassar Ahmed Khan has explained how to fetch multiple column values i.e." +
            " ID and Text values in the ASP.Net AJAX Control Toolkit AutocompleteExtender"
            + "and also how to fetch the select text and value server side on postback", "MyEvent");
        this.SendHtmlFormattedEmail("joramsilberman@gmail.com", "New article published!", body);
    }

    private string PopulateBody(string userName, string title, string url, string description, string eventName)
    {
        string test = Encrypt("UserId=1&EventId=1&PollId=1&PollAnswer=1");
        //string ButtonsString = " <form action='http://localhost:60094/PollLandingPage.aspx?ReturnString= {0}' method='get'><input type='submit' value='joramjoramjoram' /></form>";

        string ButtonsString = string.Format(" <form action='http://localhost:60094/PollLandingPage.aspx?ReturnString={0}' method='post'><input type='submit' value='joramjoramjoram' /></form>", test);


        ButtonsString = string.Format(" <form action='http://localhost:60094/PollLandingPage.aspx?ReturnString={0}' method='post'><input type='submit' value='joramjoramjoram' /></form>", "123");
        ButtonsString = ButtonsString + string.Format(" <form action='http://localhost:60094/PollLandingPage.aspx?ReturnString={0}' method='post'><input type='submit' value='joramjoramjoram' /></form>", "456");

        string body = string.Empty;
        using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplate.htm")))
        {
            body = reader.ReadToEnd();
        }
        body = body.Replace("{Event}", eventName);
        body = body.Replace("{UserName}", userName);
        body = body.Replace("{Title}", title);
        body = body.Replace("{Url}", url);
        body = body.Replace("{Description}", description);
        body = body.Replace("{Buttons}", ButtonsString);
        return body;
    }

    private void SendHtmlFormattedEmail(string recepientEmail, string subject, string body)
    {
        using (MailMessage mailMessage = new MailMessage())
        {
            mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["UserName"]);
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;
            mailMessage.To.Add(new MailAddress(recepientEmail));
            SmtpClient smtp = new SmtpClient();


            //smtp.Host = ConfigurationManager.AppSettings["Host"];
            //smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
            //System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
            //NetworkCred.UserName = ConfigurationManager.AppSettings["UserName"];
            //NetworkCred.Password = ConfigurationManager.AppSettings["Password"];
            //smtp.UseDefaultCredentials = true;
            //smtp.Credentials = NetworkCred;
            //smtp.Port = int.Parse(ConfigurationManager.AppSettings["Port"]);



            smtp.Host = "153.77.171.52";
            smtp.Port = 25;
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = false;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;




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
    private string Encrypt(string clearText)
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