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

    public partial class PollStatistics : System.Web.UI.Page
    {
        string userId;
        string pollId;
        protected void Page_Load(object sender, EventArgs e)
        {
            userId = Request.QueryString["UserId"];
            pollId = Request.QueryString["PollId"];
            //Label2.Text = "User: " + Request.QueryString["UserName"];

            //if (String.IsNullOrEmpty(UserId)) 
            //    UserId = Session["userid"].ToString(); //Request.QueryString["UserID"];
            ////UserId = "1";
            if (!this.IsPostBack)
            {
                //PopulateEventTypes();
                this.BindGeneric();
            }
        }

        /* Bind Employee Grid*/
        private void BindGeneric()
        {
            string constr = ConfigurationManager.ConnectionStrings["appmgrConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Get_Poll_Statistics"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@PollId", pollId);
                        //sda.SelectCommand = cmd;
                        //DataSet ds = new DataSet();
                        //sda.Fill(ds);


                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        sda.Fill(ds);

                        GridView1.DataSource = ds.Tables[1];
                        GridView1.DataBind();

                        DataRow row = ds.Tables[0].Rows[0];
                        {
                            Label2.Text = string.Format("The Poll: \"{0}\" has currently {1} votes. I voted: \"{2}\"", row[1].ToString(), row[0].ToString(), row[2].ToString());
                        }
                    }
                }

            }
            
            
            
            /* Code For Bind Employee Grid*/
            //string query = "SELECT G.EventId, EventDesc, EventTypeName, isnull(GU.UserId, 0) as UserId FROM Events G left join EventTypes GT on G.Type = GT.EventTypeId left join EventUsers GU on GU.EventId = G.EventId and GU.UserId = " + UserId;
            //SqlCommand cmd = new SqlCommand(query);
            //string constr = ConfigurationManager.ConnectionStrings["appmgrConnectionString"].ConnectionString;
            //SqlConnection con = new SqlConnection(constr);
            //SqlDataAdapter sda = new SqlDataAdapter();
            //cmd.Connection = con;
            //sda.SelectCommand = cmd;
            //DataSet ds = new DataSet();
            //sda.Fill(ds);

            //GridView1.DataSource = ds.Tables[1];
            //GridView1.DataBind();

            /* Apply Bootstrap Collapse and Expand Class for Grid cells attribute*/
            BootstrapCollapsExpand();
        }


        /* Apply Bootstrap Collapse and Expand Class for Grid cells attribute*/
        private void BootstrapCollapsExpand()
        {
            if (this.GridView1.Rows.Count > 0)
            {
                //Attribute to show the Plus Minus Button.
                GridView1.HeaderRow.Cells[1].Attributes["data-class"] = "expand";

                //Attribute to hide column in Phone.
                GridView1.HeaderRow.Cells[0].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[2].Attributes["data-hide"] = "phone";
                //GridView1.HeaderRow.Cells[3].Attributes["data-hide"] = "expand";
                //Adds THEAD and TBODY to GridView.
                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GridView1, "Select$" + e.Row.RowIndex);
                //e.Row.Attributes["style"] = "cursor:pointer";
                //e.Row.Attributes["id"] = GridView1.DataKeys[e.Row.RowIndex].Value.ToString();


            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int index = GridView1.SelectedRow.RowIndex;
            //Response.Redirect("PollsDefs.aspx?EventID=" + GridView1.DataKeys[index].Values[0]);
        }

    }
}