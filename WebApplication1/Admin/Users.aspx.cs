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
    public partial class Users : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["roles"].ToString() == "User")
            {
                btnAdd.Visible = false;
            }
            if (!this.IsPostBack)
            {
                PopulateRoles();
                this.BindGeneric();
            }
        }

        private void PopulateRoles()
        {
            string query = "SELECT RoleId, RoleName from Roles";
            SqlCommand cmd = new SqlCommand(query);
            string constr = ConfigurationManager.ConnectionStrings["appmgrConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlDataAdapter sda = new SqlDataAdapter();
            cmd.Connection = con;
            sda.SelectCommand = cmd;
            DataSet ds = new DataSet();
            sda.Fill(ds);

            ddlRoles.DataSource = ds;
            ddlRoles.DataTextField = "RoleName";
            ddlRoles.DataValueField = "RoleId";
            ddlRoles.DataBind();
        }
        /* Clear controls values and Set default value to controls */
        private void ClearControls()
        {
            hfAddEdit.Value = "ADD";
            btnSave.Text = "ADD";
            lblHeading.Text = "Add User Details";
            hfAddEditId.Value = "0";
            hfDeleteId.Value = "0";

            txtUserName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtEmail.Text = string.Empty;

            //ddlRoles.Text = string.Empty;
        }

        /* Bind Employee Grid*/
        private void BindGeneric()
        {
            /* Code For Bind Employee Grid*/
            string query = @"SELECT U.UserId, U.UserName, U.Email, U.Password, R.RoleName as Role, sum(isnull(PO.PollValue,0)) GeneralScore
                FROM Users U 
	                Left Join Roles R on R.RoleId = U.RoleId
	                left join EventUsers EU on EU.UserId = U.UserId
	                left join Polls P on P.UserEventId = EU.EventUserId
	                left join PollsDefs PD on PD.PollDefId = P.PollDefId
	                left join PollOptions PO on PO.PollDefId = P.PollDefId and PO.PollOptionId = P.PollAnswer and PO.IsCorrect = 'T'
	                group by U.UserId, U.UserName, U.Email, U.Password, R.RoleName";
            //                --	where U.RoleId = 2
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
            txtUserName.Text = ((sender as Button).NamingContainer as GridViewRow).Cells[1].Text;
            txtPassword.Text = ((sender as Button).NamingContainer as GridViewRow).Cells[2].Text;
            txtEmail.Text = ((sender as Button).NamingContainer as GridViewRow).Cells[3].Text;
            ddlRoles.SelectedIndex = ddlRoles.Items.IndexOf(ddlRoles.Items.FindByText(((sender as Button).NamingContainer as GridViewRow).Cells[4].Text));
            
            
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
            SqlCommand cmd = new SqlCommand();
            /* To Check Employee Id For Insert or Update and sets query string variable text*/
            if (genericId > 0)
            {
                query = "UPDATE Users SET UserName = @UserName, Password = @Password, Email = @Email, RoleId = @Role WHERE UserId = @Id";
                cmd.Parameters.AddWithValue("@Id", genericId);
            }
            else
            {
                query = "Insert_User";
                cmd.CommandType = CommandType.StoredProcedure;
            }
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@UserName", txtUserName.Text.Trim());
            cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());
            cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
            cmd.Parameters.AddWithValue("@Role", ddlRoles.SelectedValue);
                
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
            SqlCommand cmd = new SqlCommand("DELETE FROM Polls WHERE UserEventId in (select EventUserId from EventUsers where UserId = @Id);DELETE FROM EventUsers WHERE UserId = @Id;DELETE FROM Users WHERE UserId = @Id;");
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
                GridView1.HeaderRow.Cells[4].Attributes["data-hide"] = "expand";
                GridView1.HeaderRow.Cells[5].Attributes["data-hide"] = "expand";
                GridView1.HeaderRow.Cells[6].Attributes["data-hide"] = "expand";
                GridView1.HeaderRow.Cells[7].Attributes["data-hide"] = "expand";
                GridView1.HeaderRow.Cells[8].Attributes["data-hide"] = "expand";
                //Adds THEAD and TBODY to GridView.
                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (Session["roles"].ToString() == "User" && !e.Row.RowType.Equals(DataControlRowType.EmptyDataRow))
            {

                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
            }
            
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GridView1, "Select$" + e.Row.RowIndex);
            //    e.Row.Attributes["style"] = "cursor:pointer";
            //    //e.Row.Attributes["id"] = GridView1.DataKeys[e.Row.RowIndex].Value.ToString();
            //}
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int index = GridView1.SelectedRow.RowIndex;
            //Response.Redirect("Invite.aspx?UserId=" + GridView1.DataKeys[index].Values[0]);
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
            string userName = GridView1.Rows[gvr.RowIndex].Cells[1].Text;

            int rowIndex = Convert.ToInt32(e.CommandArgument); // Get the current row
            if (e.CommandName == "Events")
            {
                Response.Redirect("~/Events.aspx?UserId=" + rowIndex + "&UserName=" + userName);

            }
        }
    }
}