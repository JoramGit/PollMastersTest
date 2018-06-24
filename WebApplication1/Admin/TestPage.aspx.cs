using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Web.Security;

namespace WebApplication1
{
    public partial class TestPage : System.Web.UI.Page
    {
        string UserId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["roles"].ToString() == "User")
            {

            }
            
            UserId = Request.QueryString["UserId"];

            if (String.IsNullOrEmpty(UserId))
                UserId = Session["userid"].ToString(); //Request.QueryString["UserID"];

            if (String.IsNullOrEmpty(UserId))
                UserId = "-1";

            if (!this.IsPostBack)
            {
                this.BindGeneric();
            }
        }

        private void PopulateEventTypes()
        {

        }
        /* Clear controls values and Set default value to controls */


        /* Bind Employee Grid*/
        private void BindGeneric()
        {
            /* Code For Bind Employee Grid*/
            string query = "SELECT G.EventId, EventDesc, EventTypeName, isnull(GU.UserId, 0) as UserId FROM Events G left join EventTypes GT on G.Type = GT.EventTypeId left join EventUsers GU on GU.EventId = G.EventId and GU.UserId = " + UserId;
            //string query = "SELECT EventId, EventDesc, EventTypeName FROM TestPage G left join EventTypes GT on G.Type = GT.EventTypeId";
            SqlCommand cmd = new SqlCommand(query);
            string constr = ConfigurationManager.ConnectionStrings["appmgrConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlDataAdapter sda = new SqlDataAdapter();
            cmd.Connection = con;
            sda.SelectCommand = cmd;
            DataSet ds = new DataSet();
            sda.Fill(ds);

            GridView1.DataSource = ds;
            GridView1.DataBind();

            /* Apply Bootstrap Collapse and Expand Class for Grid cells attribute*/
            BootstrapCollapsExpand();
        }

        /* Edit Employee Detail*/


        /*Add Employee Detail*/
 


        /* Apply Bootstrap Collapse and Expand Class for Grid cells attribute*/
        private void BootstrapCollapsExpand()
        {
            if (this.GridView1.Rows.Count > 0)
            {
                //Attribute to show the Plus Minus Button.
                GridView1.HeaderRow.Cells[0].Attributes["data-class"] = "expand";

                //Attribute to hide column in Phone.
                GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[2].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[4].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[5].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[6].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[7].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[8].Attributes["data-hide"] = "phone";
                //Adds THEAD and TBODY to GridView.
                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (Session["roles"].ToString() == "User")
            {
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
            }
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btn = (Button)e.Row.FindControl("btnJoin");

                Label lbl = (Label)e.Row.FindControl("lblUserId");
                if (lbl.Text != "0")
                {
                    btn.BackColor = System.Drawing.Color.Green;
                    btn.Text = "Leave";
                }
                else
                {
                    btn = (Button)e.Row.FindControl("btnPolls");
                    btn.Visible = false;
                }
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int index = GridView1.SelectedRow.RowIndex;
            //Response.Redirect("PollsDefs.aspx?EventID=" + GridView1.DataKeys[index].Values[0]);
        }

        protected void GridView1_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
            string EventName = GridView1.Rows[gvr.RowIndex].Cells[1].Text;

            int rowIndex = Convert.ToInt32(e.CommandArgument); // Get the current row
            if (e.CommandName == "Users")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("EventUsers.aspx?EventID=" + rowIndex + "&EventName="+ EventName);
            }
            if (e.CommandName == "Polls")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("Pollsdefs.aspx?EventID=" + rowIndex + "&EventName="+ EventName);
            }
            if (e.CommandName == "Join")
            {
                Control ctrl = e.CommandSource as Control;
                if (ctrl != null)
                {
                    GridViewRow row = ctrl.Parent.NamingContainer as GridViewRow;
                    //LinkButton fullname = (LinkButton)row.FindControl("lbName");
                    Label lbl = (Label)row.FindControl("lblUserId");
                    if (lbl.Text != "0")
                        ChangeTestPagetatus(true, e.CommandArgument.ToString());
                    else
                        ChangeTestPagetatus(false, e.CommandArgument.ToString());
                }
                BindGeneric();
            }

        }

        protected void ChangeTestPagetatus(Boolean Leave, string EventNumber)
        {
            //string constr = ConfigurationManager.ConnectionStrings["appmgrConnectionString"].ConnectionString;
            //SqlConnection con = new SqlConnection(constr);
            //string query = string.Empty;

            //if (Leave)
            //    query = "DELETE FROM EventUsers WHERE EventId = @EventId and UserId = @UserId";
            //else
            //    query = "INSERT INTO EventUsers(UserId, EventId) VALUES(@UserId, @EventId)";
            //SqlCommand cmd = new SqlCommand(query);

            //int genericId = Convert.ToInt32(hfAddEditId.Value);
            //cmd.Parameters.AddWithValue("@EventId", EventNumber);

            //cmd.Parameters.AddWithValue("@UserId", UserId);
            //cmd.Connection = con;
            //con.Open();
            //cmd.ExecuteNonQuery();
            //con.Close();
        }
    }
}