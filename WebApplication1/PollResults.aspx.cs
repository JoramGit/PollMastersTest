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
    public partial class PollResults : System.Web.UI.Page
    {
        string PollId;
        string isPollActive;
        static string prevPage = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            PollId = Request.QueryString["PollId"];
            if (!this.IsPostBack)
            {
                //PopulatePollValue();
                prevPage = Request.UrlReferrer != null? Request.UrlReferrer.ToString():prevPage;
                this.BindGeneric();
            }
        }

 
        /* Clear controls values and Set default value to controls */
        //private void ClearControls()
        //{
        //    hfAddEdit.Value = "ADD";
        //    btnSave.Text = "ADD";
        //    lblHeading.Text = "Add Poll Details";
        //    hfAddEditId.Value = "0";
        //    hfDeleteId.Value = "0";

        //    txtOptionDesc.Text = string.Empty;
        //    txtPollValue.Text = string.Empty;
        //    txtIsCorrectAnswer.Text = string.Empty;
        //}

        /* Bind Employee Grid*/
        private void BindGeneric()
        {
            /* Code For Bind Employee Grid*/
            string query = string.Format(@"SELECT U.Username,PD.PollDesc, PO.OptionDesc as vote, isnull(PO.PollValue, 0) * isnull(CHARINDEX(PO.IsCorrect, 'T'),0) as points
                                            FROM Users U 
	                                            inner join EventUsers EU on EU.UserId = U.UserId
	                                            inner join Polls P on P.UserEventId = EU.EventUserId
	                                            inner join PollsDefs PD on PD.PollDefId = P.PollDefId
	                                            left join PollOptions PO on PO.PollDefId = P.PollDefId and PO.PollOptionId = P.PollAnswer
                                            where PD.PollDefId = {0} order by isnull(PO.PollValue, 0) * isnull(CHARINDEX(PO.IsCorrect, 'T'),0) desc", PollId);
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
        //protected void Edit(object sender, EventArgs e)
        //{
        //    /* Change label text of lblHeading on Edit button Click */
        //    lblHeading.Text = "Update Poll Option Details";

        //    /* Sets CommandArgument value to hidden field hfAddEditId */
        //    hfAddEditId.Value = (sender as Button).CommandArgument;

            
        //    /* Sets value from Grid cell to textboxes txtOptionDesc,txtCountry and txtPollValue */
        //    txtOptionDesc.Text = ((sender as Button).NamingContainer as GridViewRow).Cells[1].Text;
        //    txtPollValue.Text = ((sender as Button).NamingContainer as GridViewRow).Cells[2].Text;
        //    txtIsCorrectAnswer.Text = ((sender as Button).NamingContainer as GridViewRow).Cells[3].Text;
                        
        //    /* Change text of button as Update*/
        //    btnSave.Text = "Update";

        //    /* Apply Bootstrap Collapse and Expand Class for Grid cells attribute */
        //    BootstrapCollapsExpand();

        //    /* Show AddUpdateEmployee Modal Popup */
        //    mpeAddUpdate.Show();
        //}

        ///*Add Employee Detail*/
        //protected void Add(object sender, EventArgs e)
        //{
        //    /* Clear Controls Value */
        //    ClearControls();

        //    /* Apply Bootstrap Collapse and Expand Class for Grid cells attribute */
        //    BootstrapCollapsExpand();

        //    /* Show mpeAddUpdate Modal Popup */
        //    mpeAddUpdate.Show();
        //}

        ///* Save or Update Employee Details*/
        //protected void Save(object sender, EventArgs e)
        //{
        //    /* Code For INSERT OR UPDATE */
        //    string constr = ConfigurationManager.ConnectionStrings["appmgrConnectionString"].ConnectionString;
        //    SqlConnection con = new SqlConnection(constr);

        //    /* Set genericId from hfAddEditId value for INSERT or UPDATE */
        //    int genericId = Convert.ToInt32(hfAddEditId.Value);

        //    string query = string.Empty;
        //    /* To Check Employee Id For Insert or Update and sets query string variable text*/
        //    if (genericId > 0)
        //    {
        //        query = "UPDATE PollResults SET OptionDesc = @OptionDesc, PollValue = @PollValue, IsCorrect = @IsCorrect WHERE PollOptionId = @Id ";
        //    }
        //    else
        //    {
        //        query = "INSERT INTO PollResults(OptionDesc, PollValue, IsCorrect, PollDefId) VALUES(@OptionDesc, @PollValue, @IsCorrect," + PollId + ") ";
        //    }

        //    SqlCommand cmd = new SqlCommand(query);

        //    if (genericId > 0)
        //    {
        //        cmd.Parameters.AddWithValue("@Id", genericId);
        //    }
        //    cmd.Parameters.AddWithValue("@OptionDesc", txtOptionDesc.Text.Trim());
        //    cmd.Parameters.AddWithValue("@PollValue", txtPollValue.Text.Trim());
        //    cmd.Parameters.AddWithValue("@IsCorrect", txtIsCorrectAnswer.Text.Trim());
        //    cmd.Connection = con;
        //    con.Open();
        //    cmd.ExecuteNonQuery();
        //    con.Close();

        //    /* Bind Employee Grid*/
        //    BindGeneric();

        //    /* Hide mpeAddUpdate Modal Popup */
        //    mpeAddUpdate.Hide();

        //    /* Clear Controls Value */
        //    ClearControls();
        //}

        ///* Delete Emploee Detail*/
        //protected void Delete(object sender, EventArgs e)
        //{
        //    /* Apply CommandArgument value to hidden field hfDeleteId */
        //    hfDeleteId.Value = (sender as Button).CommandArgument;

        //    /* Apply Bootstrap Collapse and Expand Class for Grid cells attribute*/
        //    BootstrapCollapsExpand();

        //    /* Show DeleteEmployee Modal Popup */
        //    mpeDelete.Show();
        //}

        ///* If Select Yes on Delete Modal Popup */
        //protected void Yes(object sender, EventArgs e)
        //{
        //    /* Code to Delete Employee Record */
        //    string constr = ConfigurationManager.ConnectionStrings["appmgrConnectionString"].ConnectionString;
        //    SqlConnection con = new SqlConnection(constr);
        //    int genericId = Convert.ToInt32(hfDeleteId.Value);
        //    SqlCommand cmd = new SqlCommand("DELETE FROM PollResults WHERE PollOptionId = @Id");
        //    cmd.Parameters.AddWithValue("@Id", genericId);
        //    cmd.Connection = con;
        //    con.Open();
        //    cmd.ExecuteNonQuery();
        //    con.Close();

        //    /* Bind Grid Again To see latest Records*/
        //    BindGeneric();

        //    /* Hide Delete Employee Modal Popup */
        //    mpeDelete.Hide();

        //    /*Clear Controls Value*/
        //    ClearControls();
        //}

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
                //Adds THEAD and TBODY to GridView.
                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(prevPage);
        }
    }
}