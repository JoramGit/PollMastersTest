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
    public partial class EventUsers : System.Web.UI.Page
    {
        string EventId;
        static string prevPage = String.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            EventId = Request.QueryString["EventID"];
            Label2.Text = "Users in Event: " + Request.QueryString["EventName"];
            //Label2.Text = "<a href=\"Admin/Events.aspx?EventId=" + EventId + "\"> Event: " + Request.QueryString["EventName"];
            if (Session["roles"].ToString() == "User")
            {
                btnAdd.Visible = false;
            }
            if (!this.IsPostBack)
            {
                PopulateUsers();
                prevPage = Request.UrlReferrer != null? Request.UrlReferrer.ToString():prevPage;
                this.BindGeneric();
            }
        }

        private void PopulateUsers()
        {
            string query = string.Format("SELECT UserName, UserId from Users where userid not in (select userid from EventUsers where Eventid = {0})", EventId);
            SqlCommand cmd = new SqlCommand(query);
            string constr = ConfigurationManager.ConnectionStrings["appmgrConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlDataAdapter sda = new SqlDataAdapter();
            cmd.Connection = con;
            sda.SelectCommand = cmd;
            DataSet ds = new DataSet();
            sda.Fill(ds);

            ddlUsers.DataSource = ds;
            ddlUsers.DataTextField = "UserName";
            ddlUsers.DataValueField = "UserId";
            ddlUsers.DataBind();
        }
        /* Clear controls values and Set default value to controls */
        private void ClearControls()
        {
            hfAddEdit.Value = "ADD";
            btnSave.Text = "ADD";
            lblHeading.Text = "Add User Details";
            hfAddEditId.Value = "0";
            hfDeleteId.Value = "0";

            //txtUserName.Text = string.Empty;
            //ddlUsers.Text = string.Empty;
        }

        /* Bind Employee Grid*/
        private void BindGeneric()
        {
            /* Code For Bind Employee Grid*/
            string query = string.Format(@"SELECT  GU.EventUserId, U.UserName, GU.UserId, sum(isnull(PO.PollValue,0)) EventScore
                                FROM EventUsers GU 
	                                Left Join Users U on GU.UserId = U.UserId 
	                                Left Join Roles R on R.RoleId = U.RoleId
	                                left join EventUsers EU on EU.UserId = U.UserId and EU.EventId = GU.EventId
	                                left join Polls P on P.UserEventId = EU.EventUserId
	                                left join PollsDefs PD on PD.PollDefId = P.PollDefId and PD.EventId = GU.EventId
	                                left join PollOptions PO on PO.PollDefId = P.PollDefId and PO.PollOptionId = P.PollAnswer and PO.IsCorrect = 'T'
                                WHERE GU.EventId = {0}
                                group by GU.EventUserId, U.UserName, GU.UserId
                                order by sum(isnull(PO.PollValue,0)) desc", EventId);
            //string query = "SELECT EventUserId, UserName, GU.UserId FROM EventUsers GU Left Join Users U on GU.UserId = U.UserId WHERE EventId = " + EventId;
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
            lblHeading.Text = "Update User Details";

            /* Sets CommandArgument value to hidden field hfAddEditId */
            hfAddEditId.Value = (sender as Button).CommandArgument;

            
            /* Sets value from Grid cell to textboxes txtUserName,txtCountry and txtEventType */
            //txtUserName.Text = ((sender as Button).NamingContainer as GridViewRow).Cells[1].Text;
            ddlUsers.SelectedIndex = ddlUsers.Items.IndexOf(ddlUsers.Items.FindByText(((sender as Button).NamingContainer as GridViewRow).Cells[2].Text));
            
            
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
                query = "UPDATE EventUsers SET UserName = @UserName WHERE UserId = @Id";
            }
            else
            {
                query = "INSERT INTO EventUsers(UserId, EventId) VALUES(@UserId, " + EventId + ") ";
            }

            SqlCommand cmd = new SqlCommand(query);

            if (genericId > 0)
            {
                cmd.Parameters.AddWithValue("@Id", genericId);
            }
            //cmd.Parameters.AddWithValue("@UserName", txtUserName.Text.Trim());
            cmd.Parameters.AddWithValue("@UserId", ddlUsers.SelectedValue); //txtEventType.Text
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
            SqlCommand cmd = new SqlCommand("DELETE FROM EventUsers WHERE EventUserId = @Id");
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
                //GridView1.HeaderRow.Cells[4].Attributes["data-hide"] = "expand";
                //Adds THEAD and TBODY to GridView.
                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GridView1, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";
                //e.Row.Attributes["id"] = GridView1.DataKeys[e.Row.RowIndex].Value.ToString();
            }

            if (Session["roles"].ToString() == "User" && !e.Row.RowType.Equals(DataControlRowType.EmptyDataRow))
            {
                e.Row.Cells[3].Visible = false;
            }


        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = GridView1.SelectedRow.RowIndex;
            Response.Redirect("Invite.aspx?UserId=" + GridView1.DataKeys[index].Values[0]);
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(prevPage);
        }
    }
}