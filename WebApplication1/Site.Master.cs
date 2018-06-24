﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;

namespace WebApplication1
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void OnMenuItemDataBound(object sender, MenuEventArgs e)
        {
            if (SiteMap.CurrentNode != null)
            {
                if (e.Item.Text == SiteMap.CurrentNode.Title)
                {
                    if (e.Item.Parent != null)
                    {
                        e.Item.Parent.Selected = true;
                    }
                    else
                    {
                        e.Item.Selected = true;
                    }
                }
            }
        }

        protected void ValidateUser(object sender, EventArgs e)
        {
            int userId = 0;
            string roles = string.Empty;
            string constr = ConfigurationManager.ConnectionStrings["appmgrConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Validate_User"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", User.Text);
                    cmd.Parameters.AddWithValue("@Password", Password.Text);
                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    userId = Convert.ToInt32(reader["UserId"]);
                    roles = reader["Roles"].ToString();
                    Session["userid"] = userId;
                    Session["roles"] = roles;
                    Session["username"] = User.Text;
                    con.Close();

                    //userId = Convert.ToInt32(cmd.ExecuteScalar());
                    //con.Close();
                }
                switch (userId)
                {
                    case -1:
                        //Login1.FailureText = "Username and/or password is incorrect.";
                        break;
                    case -2:
                        //Login1.FailureText = "Account has not been activated.";
                        break;
                    default:
                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, User.Text, DateTime.Now, DateTime.Now.AddMinutes(2880), RememberMe.Checked, roles, FormsAuthentication.FormsCookiePath);
                        string hash = FormsAuthentication.Encrypt(ticket);
                        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);

                        if (ticket.IsPersistent)
                        {
                            cookie.Expires = ticket.Expiration;
                        }
                        Response.Cookies.Add(cookie);
                        //Response.Redirect("~/Admin/UserEvents.aspx?UserId=" + userId);
                        Response.Redirect(FormsAuthentication.GetRedirectUrl(User.Text, RememberMe.Checked));
                        break;
                    //FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, Login1.UserName, DateTime.Now, DateTime.Now.AddMinutes(2880), Login1.RememberMeSet, roles, FormsAuthentication.FormsCookiePath);
                    //string hash = FormsAuthentication.Encrypt(ticket);
                    //HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);

                    //if (ticket.IsPersistent)
                    //{
                    //    cookie.Expires = ticket.Expiration;
                    //}
                    //Response.Cookies.Add(cookie);
                    ////Response.Redirect(FormsAuthentication.GetRedirectUrl(Login1.UserName, Login1.RememberMeSet));               
                    //FormsAuthentication.SetAuthCookie(Login1.UserName, true);
                    //Response.Redirect("UserEvents.aspx?UserId=" + userId); 
                    //break;
                }
            }
        }
    }
}
