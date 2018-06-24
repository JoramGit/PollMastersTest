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
    public partial class PollOptions : System.Web.UI.Page
    {
        string PollId;
        string isPollActive;
        string MyVote;
        static string prevPage = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            PollId = Request.QueryString["PollId"];
            if (Session["roles"].ToString() == "User")
            {
                btnAdd.Visible = false;
            }
            if (!this.IsPostBack)
            {
                //PopulatePollValue();
                prevPage = Request.UrlReferrer != null? Request.UrlReferrer.ToString():prevPage;
                this.BindGeneric();
            }
        }

 
        /* Clear controls values and Set default value to controls */
        private void ClearControls()
        {
            hfAddEdit.Value = "ADD";
            btnSave.Text = "ADD";
            lblHeading.Text = "Add Poll Details";
            hfAddEditId.Value = "0";
            hfDeleteId.Value = "0";

            txtOptionDesc.Text = string.Empty;
            txtPollValue.Text = string.Empty;
            txtIsCorrectAnswer.Text = string.Empty;
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
                        cmd.Parameters.AddWithValue("@UserId", Session["userid"].ToString());
                        cmd.Parameters.AddWithValue("@PollId", PollId);
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        sda.Fill(ds);


                        DataRow row = ds.Tables[0].Rows[0];
                        MyVote = row[4].ToString();
                        isPollActive = row[3].ToString();
                        GridView1.DataSource = ds.Tables[1];
                        GridView1.DataBind();

                        {
                            string PollActive;
                            if (row[3].ToString() == "1")
                                PollActive = "<span style='color: Green;'>The poll is Active</span>";
                            else
                                PollActive = "<span style='color: red;'>The poll is not Active</span>";
                            Label2.Text = string.Format(PollActive + "<br />" + "The Poll: \"{0}\" has currently {1} votes. I voted: \"{2}\"", row[1].ToString(), row[0].ToString(), row[2].ToString());
                        }
                    }
                }

            }
            
            
            ///* Code For Bind Employee Grid*/
            //string query = "SELECT PollOptionId, OptionDesc, PollValue FROM PollOptions P WHERE PollDefId = " + PollId;
            //SqlCommand cmd = new SqlCommand(query);
            //string constr = ConfigurationManager.ConnectionStrings["appmgrConnectionString"].ConnectionString;
            //SqlConnection con = new SqlConnection(constr);
            //SqlDataAdapter sda = new SqlDataAdapter();
            //cmd.Connection = con;
            //sda.SelectCommand = cmd;
            //DataSet ds = new DataSet();
            //sda.Fill(ds);

            //GridView1.DataSource = ds;
            //GridView1.DataBind();

            /* Apply Bootstrap Collapse and Expand Class for Grid cells attribute*/
            BootstrapCollapsExpand();
        }

        /* Edit Employee Detail*/
        protected void Edit(object sender, EventArgs e)
        {
            /* Change label text of lblHeading on Edit button Click */
            lblHeading.Text = "Update Poll Option Details";

            /* Sets CommandArgument value to hidden field hfAddEditId */
            hfAddEditId.Value = (sender as Button).CommandArgument;

            
            /* Sets value from Grid cell to textboxes txtOptionDesc,txtCountry and txtPollValue */
            txtOptionDesc.Text = ((sender as Button).NamingContainer as GridViewRow).Cells[2].Text;
            txtPollValue.Text = ((sender as Button).NamingContainer as GridViewRow).Cells[3].Text;
            txtIsCorrectAnswer.Text = ((sender as Button).NamingContainer as GridViewRow).Cells[4].Text;
                        
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
                query = "UPDATE PollOptions SET OptionDesc = @OptionDesc, PollValue = @PollValue, IsCorrect = @IsCorrect WHERE PollOptionId = @Id ";
            }
            else
            {
                query = "INSERT INTO PollOptions(OptionDesc, PollValue, IsCorrect, PollDefId) VALUES(@OptionDesc, @PollValue, @IsCorrect," + PollId + ") ";
            }

            SqlCommand cmd = new SqlCommand(query);

            if (genericId > 0)
            {
                cmd.Parameters.AddWithValue("@Id", genericId);
            }
            cmd.Parameters.AddWithValue("@OptionDesc", txtOptionDesc.Text.Trim());
            cmd.Parameters.AddWithValue("@PollValue", txtPollValue.Text.Trim());
            cmd.Parameters.AddWithValue("@IsCorrect", txtIsCorrectAnswer.Text.Trim());
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
            SqlCommand cmd = new SqlCommand("DELETE FROM PollOptions WHERE PollOptionId = @Id");
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
                //Adds THEAD and TBODY to GridView.
                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (Session["roles"].ToString() == "User" &&!e.Row.RowType.Equals(DataControlRowType.EmptyDataRow))
            {
                e.Row.Cells[4].Visible = false;
                //e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
            }

            

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (Convert.ToBoolean(e.Row.Cells[1].Text == MyVote)) //e.Row.DataItem
                    {
                        e.Row.BackColor = System.Drawing.Color.LightPink;
                    }

                    if (isPollActive != "1")
                    {
                        Button btn = (Button)e.Row.FindControl("btnVote");
                        btn.BackColor = System.Drawing.Color.Green;
                        btn.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                //ErrorLabel.Text = ex.Message;
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            GridViewRow gvr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
            //string pollName = GridView1.Rows[gvr.RowIndex].Cells[1].Text;
            //string isPollActive = GridView1.Rows[gvr.RowIndex].Cells[2].Text;

            int rowIndex = Convert.ToInt32(e.CommandArgument); // Get the current row

            if (e.CommandName == "Vote")
            {
                int index = Convert.ToInt32(e.CommandArgument);

                string constr = ConfigurationManager.ConnectionStrings["appmgrConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("Apply_User_Answer"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@UserId", Session["userid"].ToString());
                            //cmd.Parameters.AddWithValue("@EventId", EventId);
                            cmd.Parameters.AddWithValue("@PollId", PollId);
                            cmd.Parameters.AddWithValue("@PollAnswer", rowIndex);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                BindGeneric();
//                string constr = ConfigurationManager.ConnectionStrings["appmgrConnectionString"].ConnectionString;
//                SqlConnection con = new SqlConnection(constr);
//                SqlCommand cmd = new SqlCommand(@"update P 
//	                                                set PollAnswer = @Id
//                                                from Polls P
//                                                inner join EventUsers EU on EU.EventUserId = P.UserEventId
//                                                where P.PollDefId = @PollDefId and EU.UserId = @UserId ");
//                cmd.Parameters.AddWithValue("@Id", rowIndex);
//                cmd.Parameters.AddWithValue("@PollDefId", PollId);
//                cmd.Parameters.AddWithValue("@UserId", Session["userid"].ToString());
//                cmd.Connection = con;
//                con.Open();
//                cmd.ExecuteNonQuery();
//                con.Close();
//                BindGeneric();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(prevPage);
        }
    }
}


