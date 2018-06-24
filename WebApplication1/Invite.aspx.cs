using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace WebApplication1
{
    public partial class Invite : System.Web.UI.Page
    {
        string EventId;
        protected void Page_Load(object sender, EventArgs e)
        {
            mpeAddUpdate.Show();
        }


//        /* Save or Update Employee Details*/
        protected void Save(object sender, EventArgs e)
        {
            /* Code For INSERT OR UPDATE */
            //string constr = ConfigurationManager.ConnectionStrings["appmgrConnectionString"].ConnectionString;
            //SqlConnection con = new SqlConnection(constr);

            ///* Set genericId from hfAddEditId value for INSERT or UPDATE */
            //int genericId = Convert.ToInt32(hfAddEditId.Value);

            //string query = string.Empty;
            ///* To Check Employee Id For Insert or Update and sets query string variable text*/
            //if (genericId > 0)
            //{
            //    query = "UPDATE Invite SET PollDesc = @PollDesc WHERE PollDefId = @Id ";
            //}
            //else
            //{
            //    query = "INSERT INTO Invite(PollDesc, Status, EventId) VALUES(@PollDesc, 0, " + EventId + ") ";
            //}

            //SqlCommand cmd = new SqlCommand(query);

            //if (genericId > 0)
            //{
            //    cmd.Parameters.AddWithValue("@Id", genericId);
            //}
            //cmd.Parameters.AddWithValue("@PollDesc", txtPollDesc.Text.Trim());
            ////cmd.Parameters.AddWithValue("@PollStatus", ddlPollStatus.SelectedValue); //txtPollStatus.Text
            //cmd.Connection = con;
            //con.Open();
            //cmd.ExecuteNonQuery();
            //con.Close();

            ///* Bind Employee Grid*/
            //BindGeneric();

            clsEmailHandler.SendInvitation(txtEmail.Text, "Join PollMasters today!!", Session["username"].ToString());
            ///* Hide mpeAddUpdate Modal Popup */
            mpeAddUpdate.Hide();

            ///* Clear Controls Value */
            //ClearControls();
            

        }
    }
}