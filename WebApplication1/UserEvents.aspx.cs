﻿using System;
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

    public partial class UserEvents : System.Web.UI.Page
    {
        string UserId;
        protected void Page_Load(object sender, EventArgs e)
        {
            UserId = Request.QueryString["UserId"];
            Label2.Text = "User: " + Request.QueryString["UserName"];

            if (String.IsNullOrEmpty(UserId)) 
                UserId = Session["userid"].ToString(); //Request.QueryString["UserID"];

            if (!this.IsPostBack)
            {
                //PopulateEventTypes();
                this.BindGeneric();
            }
        }

        private void PopulateEventTypes()
        {
            //string query = "SELECT EventTypeId, EventTypeName from EventTypes";
            //SqlCommand cmd = new SqlCommand(query);
            //string constr = ConfigurationManager.ConnectionStrings["appmgrConnectionString"].ConnectionString;
            //SqlConnection con = new SqlConnection(constr);
            //SqlDataAdapter sda = new SqlDataAdapter();
            //cmd.Connection = con;
            //sda.SelectCommand = cmd;
            //DataSet ds = new DataSet();
            //sda.Fill(ds);

            //ddlEventType.DataSource = ds;
            //ddlEventType.DataTextField = "EventTypeName";
            //ddlEventType.DataValueField = "EventTypeId";
            //ddlEventType.DataBind();
        }
        /* Clear controls values and Set default value to controls */
        private void ClearControls()
        {
            hfAddEdit.Value = "ADD";
            btnSave.Text = "ADD";
            lblHeading.Text = "Add Event Details";
            hfAddEditId.Value = "0";
            hfDeleteId.Value = "0";

            txtEventDesc.Text = string.Empty;
            //ddlEventType.Text = string.Empty;
        }

        /* Bind Employee Grid*/
        private void BindGeneric()
        {
            /* Code For Bind Employee Grid*/
            string query = "SELECT G.EventId, EventDesc, EventTypeName, isnull(GU.UserId, 0) as UserId FROM Events G left join EventTypes GT on G.Type = GT.EventTypeId left join EventUsers GU on GU.EventId = G.EventId and GU.UserId = " + UserId;
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
        protected void Edit(object sender, EventArgs e)
        {
            /* Change label text of lblHeading on Edit button Click */
            lblHeading.Text = "Update Event Details";

            /* Sets CommandArgument value to hidden field hfAddEditId */
            hfAddEditId.Value = (sender as Button).CommandArgument;

            
            /* Sets value from Grid cell to textboxes txtEventDesc,txtCountry and txtEventType */
            txtEventDesc.Text = ((sender as Button).NamingContainer as GridViewRow).Cells[1].Text;
            ddlEventType.SelectedIndex = ddlEventType.Items.IndexOf(ddlEventType.Items.FindByText(((sender as Button).NamingContainer as GridViewRow).Cells[2].Text));
            
            
            /* Change text of button as Update*/
            btnSave.Text = "Update";

            /* Apply Bootstrap Collapse and Expand Class for Grid cells attribute */
            BootstrapCollapsExpand();

            /* Show AddUpdateEmployee Modal Popup */
            mpeAddUpdate.Show();
        }

        /*Add Employee Detail*/
        protected void Add(object sender, EventArgs e)
        {
            /* Clear Controls Value */
            ClearControls();

            /* Apply Bootstrap Collapse and Expand Class for Grid cells attribute */
            BootstrapCollapsExpand();

            /* Show mpeAddUpdate Modal Popup */
            mpeAddUpdate.Show();
        }

        /* Save or Update Employee Details*/
        protected void Save(object sender, EventArgs e)
        {
            /* Code For INSERT OR UPDATE */
            string constr = ConfigurationManager.ConnectionStrings["appmgrConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);

            /* Set genericId from hfAddEditId value for INSERT or UPDATE */
            int genericId = Convert.ToInt32(hfAddEditId.Value);

            string query = string.Empty;
            /* To Check Employee Id For Insert or Update and sets query string variable text*/
            if (genericId > 0)
            {
                query = "UPDATE UserEvents SET EventName = @EventDesc, EventDesc = @EventDesc, Type = @EventType WHERE EventId = @Id";
            }
            else
            {
                query = "INSERT INTO UserEvents(EventName, EventDesc, Type) VALUES(@EventDesc, @EventDesc, @EventType)";
            }

            SqlCommand cmd = new SqlCommand(query);

            if (genericId > 0)
            {
                cmd.Parameters.AddWithValue("@Id", genericId);
            }
            cmd.Parameters.AddWithValue("@EventDesc", txtEventDesc.Text.Trim());
            cmd.Parameters.AddWithValue("@EventType", ddlEventType.SelectedValue); //txtEventType.Text
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            /* Bind Employee Grid*/
            BindGeneric();

            /* Hide mpeAddUpdate Modal Popup */
            mpeAddUpdate.Hide();

            /* Clear Controls Value */
            ClearControls();
        }

        /* Delete Emploee Detail*/
        protected void Delete(object sender, EventArgs e)
        {
            /* Apply CommandArgument value to hidden field hfDeleteId */
            hfDeleteId.Value = (sender as Button).CommandArgument;

            /* Apply Bootstrap Collapse and Expand Class for Grid cells attribute*/
            BootstrapCollapsExpand();

            /* Show DeleteEmployee Modal Popup */
            mpeDelete.Show();
        }

        /* If Select Yes on Delete Modal Popup */
        protected void Yes(object sender, EventArgs e)
        {
            /* Code to Delete Employee Record */
            string constr = ConfigurationManager.ConnectionStrings["appmgrConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            int genericId = Convert.ToInt32(hfDeleteId.Value);
            SqlCommand cmd = new SqlCommand("DELETE FROM UserEvents WHERE EventId = @Id");
            cmd.Parameters.AddWithValue("@Id", genericId);
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            /* Bind Grid Again To see latest Records*/
            BindGeneric();

            /* Hide Delete Employee Modal Popup */
            mpeDelete.Hide();

            /*Clear Controls Value*/
            ClearControls();
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
                GridView1.HeaderRow.Cells[3].Attributes["data-hide"] = "expand";
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


                Button btn = (Button)e.Row.FindControl("btnUsers");

                Label lbl = (Label)e.Row.FindControl("lblUserId");
                if (lbl.Text != "0")
                {
                    btn.BackColor = System.Drawing.Color.Green;
                    btn.Text = "Leave";
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

            if (e.CommandName == "Users")
            {
                Control ctrl = e.CommandSource as Control;
                if (ctrl != null)
                {
                    GridViewRow row = ctrl.Parent.NamingContainer as GridViewRow;
                    //LinkButton fullname = (LinkButton)row.FindControl("lbName");
                    Label lbl = (Label)row.FindControl("lblUserId");
                    if (lbl.Text != "0")
                        ChangeEventStatus(true, e.CommandArgument.ToString());
                    else
                        ChangeEventStatus(false, e.CommandArgument.ToString());
                }
                BindGeneric();
            }
        }


        protected void ChangeEventStatus(Boolean Leave, string EventNumber)
        {
            string constr = ConfigurationManager.ConnectionStrings["appmgrConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            string query = string.Empty;

            if (Leave)
                query = "DELETE FROM EventUsers WHERE EventId = @EventId and UserId = @UserId";
            else
                query = "INSERT INTO EventUsers(UserId, EventId) VALUES(@UserId, @EventId)";
            SqlCommand cmd = new SqlCommand(query);

            int genericId = Convert.ToInt32(hfAddEditId.Value);
            cmd.Parameters.AddWithValue("@EventId", EventNumber);

            cmd.Parameters.AddWithValue("@UserId", UserId);
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}