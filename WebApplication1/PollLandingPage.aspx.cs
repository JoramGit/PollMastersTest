using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace WebApplication1
{
    public partial class PollLandingPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string UserId;
            string EventId;
            string PollId;
            string PollAnswer;
            string showPoll;

            string ReturnString = Request.QueryString["ReturnString"];
            ReturnString = Decrypt(ReturnString);

            Dictionary<string,string> dicQueryString = 
                    ReturnString.Split('&')
                         .ToDictionary(c => c.Split('=')[0],
                                       c => Uri.UnescapeDataString(c.Split('=')[1]));


            PollId = dicQueryString["PollId"];
            showPoll = dicQueryString["ShowPoll"]; 
            string constr = ConfigurationManager.ConnectionStrings["appmgrConnectionString"].ConnectionString;

            if (showPoll != "1")
            {
                UserId = dicQueryString["UserId"];
                EventId = dicQueryString["EventId"];
                PollAnswer = dicQueryString["PollAnswer"];
                try
                {
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand("Apply_User_Answer"))
                        {
                            using (SqlDataAdapter sda = new SqlDataAdapter())
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@UserId", UserId);
                                //cmd.Parameters.AddWithValue("@EventId", EventId);
                                cmd.Parameters.AddWithValue("@PollId", PollId);
                                cmd.Parameters.AddWithValue("@PollAnswer", PollAnswer);
                                cmd.Connection = con;
                                con.Open();
                                //isPollActive = Convert.ToInt32(cmd.ExecuteScalar());
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                    }
                }
                finally
                {
                    Session["userid"] = UserId;
                    //Response.Redirect("PollStatistics.aspx?PollId=" + PollId + "&UserId=" + UserId);
                }
            }
            Response.Redirect("PollOptions.aspx?PollId=" + PollId);
        }

        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}