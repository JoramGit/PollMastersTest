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
    public partial class Deactivate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string constr = ConfigurationManager.ConnectionStrings["appmgrConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(@"update Pollsdefs
	                                                set status = 0
                                                where dateadd(HOUR, 7, getdate()) > Finish;
                                            select E.EventDesc, PD.PollDesc, PD.Finish, U.Username, U.Email
                                            from Events E
                                            left join  EventUsers EU on EU.EventId = E.EventId
                                            left join PollsDefs PD on E.EventId = PD.EventId
                                            left join Polls P on PD.PollDefId = P.PollDefId and P.UserEventId = EU.EventUserId
                                            left join Users U on EU.UserId= U.UserId
                                            where CAST(CONVERT(CHAR(17),dateadd(minute,+ 10,dateadd(HOUR, 7, getdate())),113) AS datetime) = PD.Finish and Pollanswer is null");
            DateTime dt = DateTime.Now; // Or whatever
            cmd.Parameters.AddWithValue("@dateTime", dt.ToString("yyyy-MM-dd hh:mm:ss"));
            //cmd.Connection = con;
            //con.Open();
            //cmd.ExecuteNonQuery();
            //con.Close();
            SqlDataAdapter sda = new SqlDataAdapter();
            cmd.Connection = con;
            sda.SelectCommand = cmd;
            DataSet ds = new DataSet();
            sda.Fill(ds);
            con.Close();





            //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('"+ "before"+ "');", true);
            //clsEmailHandler.SendInvitation("joramsilberman@gmail.com", "Deactivated!!","joram");
            //ClientScript.RegisterStartupScript(this.GetType(), "myalert2", "alert('" + "after" + "');", true);
        }

    }
}
