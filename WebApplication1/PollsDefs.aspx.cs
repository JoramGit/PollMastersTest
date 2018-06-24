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
    public partial class PollsDefs : System.Web.UI.Page
    {
        string EventId;
        static string prevPage = String.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            EventId = Request.QueryString["EventID"];
            Label2.Text = "Polls in Event: " + Request.QueryString["EventName"];
            //Label2.Text = "<a href=\"Admin/Events.aspx?EventId=" + EventId + "\"> Event: " + Request.QueryString["EventName"];
            if (Session["roles"].ToString() == "User")
            {
                btnAdd.Visible = false;
            }
            if (!this.IsPostBack)
            {
                //PopulatePollStatus();
                prevPage = Request.UrlReferrer != null? Request.UrlReferrer.ToString():prevPage;
                this.BindGeneric();
            }
        }

        //private void PopulatePollValue()
        //{
        //    string query = "SELECT PollOptionId, OPtionDesc, PollValue from PollValue";
        //    SqlCommand cmd = new SqlCommand(query);
        //    string constr = ConfigurationManager.ConnectionStrings["appmgrConnectionString"].ConnectionString;
        //    SqlConnection con = new SqlConnection(constr);
        //    SqlDataAdapter sda = new SqlDataAdapter();
        //    cmd.Connection = con;
        //    sda.SelectCommand = cmd;
        //    DataSet ds = new DataSet();
        //    sda.Fill(ds);

        //    ddlStatus.DataSource = ds;
        //    ddlStatus.DataTextField = "PollValue";
        //    ddlStatus.DataValueField = "PollOptionId";
        //    ddlStatus.DataBind();
        //}

        /* Clear controls values and Set default value to controls */
        private void ClearControls()
        {
            hfAddEdit.Value = "ADD";
            btnSave.Text = "ADD";
            lblHeading.Text = "Add Poll Details";
            hfAddEditId.Value = "0";
            hfDeleteId.Value = "0";

            txtPollDesc.Text = string.Empty;
            txtFinishDate.Text = string.Empty;
            txtFinishTime.Text = string.Empty;
            //ddlPollStatus.Text = string.Empty;
        }

        /* Bind Employee Grid*/
        private void BindGeneric()
        {
            /* Code For Bind Employee Grid*/
            string query = string.Format(@"
                                            
                                            SELECT PD.PollDefId, PD.PollDesc, PD.Status as Pollstatus, convert(varchar(25), PD.Finish, 120) as Finish,  max(isnull(EU.EventUserId,0))
                                                ,case when max(isnull(EU.EventUserId, 0)) = 0 then 0 else 1 END AS HasVoted
                                                FROM PollsDefs PD
                                                left join Polls P on PD.PollDefId = P.PollDefId
                                                left join EventUsers EU on EU.EventUserId = P.UserEventId and EU.UserId = {0}
                                                WHERE PD.EventId = {1} 
                                                group by PD.PollDefId, PD.PollDesc, PD.Status, convert(varchar(25), PD.Finish, 120)
                                                ORDER BY Status Desc, convert(varchar(25), Finish, 120) ASC ", Session["userid"].ToString(), EventId);
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
            lblHeading.Text = "Update Poll Details";

            /* Sets CommandArgument value to hidden field hfAddEditId */
            hfAddEditId.Value = (sender as Button).CommandArgument;

            
            /* Sets value from Grid cell to textboxes txtPollDesc,txtCountry and txtPollStatus */
            txtPollDesc.Text = ((sender as Button).NamingContainer as GridViewRow).Cells[1].Text;

            string dateTime = ((sender as Button).NamingContainer as GridViewRow).Cells[2].Text;
            string[] dateTimeArr = dateTime.Split(' ');
            string datePart = dateTimeArr[0];
            string timePart = dateTimeArr[1];

            txtFinishDate.Text = datePart;
            txtFinishTime.Text = timePart;
            //ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(((sender as Button).NamingContainer as GridViewRow).Cells[2].Text));
            
            
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
                query = "UPDATE PollsDefs SET PollDesc = @PollDesc, Finish = @Finish WHERE PollDefId = @Id ";
            }
            else
            {
                query = "INSERT INTO PollsDefs(PollDesc, Finish, Status, EventId) VALUES(@PollDesc,@Finish, 0, " + EventId + ") ";
            }

            SqlCommand cmd = new SqlCommand(query);

            if (genericId > 0)
            {
                cmd.Parameters.AddWithValue("@Id", genericId);
            }
            cmd.Parameters.AddWithValue("@PollDesc", txtPollDesc.Text.Trim());
            cmd.Parameters.AddWithValue("@Finish", txtFinishDate.Text.Trim() + " " + txtFinishTime.Text.Trim());

            //cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue); //txtPollStatus.Text
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
            SqlCommand cmd = new SqlCommand("DELETE FROM PollOptions WHERE PollDefId = @Id;DELETE FROM PollsDefs WHERE PollDefId = @Id;DELETE FROM Polls WHERE PollDefId = @Id");
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
                //Adds THEAD and TBODY to GridView.
                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (Session["roles"].ToString() == "User" &&!e.Row.RowType.Equals(DataControlRowType.EmptyDataRow))
            {
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[10].Visible = false;
                e.Row.Cells[7].Visible = false;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GridView1, "Select$" + e.Row.RowIndex);
                //    e.Row.Attributes["style"] = "cursor:pointer";
                //    //e.Row.Attributes["id"] = GridView1.DataKeys[e.Row.RowIndex].Value.ToString();

                Button btn = (Button)e.Row.FindControl("btnStatus");
                Button btnVoted = (Button)e.Row.FindControl("btnPollOptions");
                HiddenField hfStatus = (e.Row.FindControl("hfStatus") as HiddenField);
                HiddenField hfHasVoted = (e.Row.FindControl("hfHasVoted") as HiddenField);

                Label lbl = (Label)e.Row.FindControl("lblStatus");
                if (hfStatus.Value != "0")
                {
                    btn.BackColor = System.Drawing.Color.Green;
                    btn.Text = "Deactivate";
                    lbl.Text = "Active";

                    if (Session["roles"].ToString() == "User")
                    {
                        Button btn2 = (Button)e.Row.FindControl("btnPollResults");
                        btn2.BackColor = System.Drawing.Color.LightGray;
                    }
                }
                else
                {
                    e.Row.BackColor = System.Drawing.Color.Beige;
                    lbl.Text = "Closed";
                }

                if (hfHasVoted.Value == "1")
                {
                    btnVoted.BackColor = System.Drawing.Color.Green;
                }


                if (Session["roles"].ToString() == "User")
                {
                    btn = (Button)e.Row.FindControl("btnPollOptions");
                    btn.Text = "Vote";
                }
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int index = GridView1.SelectedRow.RowIndex;
            //Response.Redirect("PollOptions.aspx?PollID=" + GridView1.DataKeys[index].Values[0]);
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
            GridViewRow gvr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
            string pollName = GridView1.Rows[gvr.RowIndex].Cells[1].Text;
            HiddenField isPollActive = (GridView1.Rows[gvr.RowIndex].FindControl("hfStatus") as HiddenField);
            int rowIndex = Convert.ToInt32(e.CommandArgument); // Get the current row
            if (e.CommandName == "PollOptions")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("PollOptions.aspx?PollId=" + rowIndex + "&PollName=" + pollName + "&isPollActive=" + isPollActive.Value);
            }

            if (e.CommandName == "PollResults")
            {
                if (isPollActive.Value == "0" || Session["roles"].ToString() != "User")
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    Response.Redirect("PollResults.aspx?PollId=" + rowIndex + "&PollName=" + pollName + "&isPollActive=" + isPollActive.Value);
                }
            }

            if (e.CommandName == "Status")
            {
                if (isPollActive.Value != "0")
                    ChangeEventStatus(false, e.CommandArgument.ToString());
                else
                    ChangeEventStatus(true, e.CommandArgument.ToString());
                BindGeneric();
            }

            if (e.CommandName == "Clear")
            {
                int index = Convert.ToInt32(e.CommandArgument);

                string constr = ConfigurationManager.ConnectionStrings["appmgrConnectionString"].ConnectionString;
                SqlConnection con = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand("DELETE FROM Polls WHERE PollDefId = @Id");
                cmd.Parameters.AddWithValue("@Id", rowIndex);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        protected void ChangeEventStatus(Boolean Activate, string PollId)
        {
            string message = string.Empty;
            int res;
            string constr = ConfigurationManager.ConnectionStrings["appmgrConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Set_Poll_Status"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PollId", PollId);
                        cmd.Parameters.Add("@Activate", SqlDbType.Bit).Value = Activate;
                        cmd.Connection = con;
                        con.Open();
                        res = Convert.ToInt32(cmd.ExecuteScalar());
                        con.Close();
                    }
                }

                switch (res)
                {
                    case -1:
                        message = "This Poll has not enough valid options! Please add at least 2 poll options.";
                        ClientScript.RegisterStartupScript(GetType(), "alert", "alert('" + message + "');", true);
                        break;
                    case 2:
                        message = "";
                        break;
                    case 1:
                        SqlCommand cmd = new SqlCommand();
                        string query = @"select u.userid, u.Username, u.email, PD.PollDesc, E.EventDesc 
                                            from Eventusers gu 
                                            left join users u on u.userid = gu.userid 
                                            left join PollsDefs PD on PD.EventId = GU.EventId
                                            left join Events E on E.EventId = PD.EventId
                                            where GU.Eventid = {0} and PD.PollDefId = {1};
                                            SELECT PollOptionId, OptionDesc FROM PollOptions WHERE PollDefId = {1}";
                        query = string.Format(query, EventId, PollId);
                        cmd = new SqlCommand(query);
                        con.Open();
                        SqlDataAdapter sda = new SqlDataAdapter();
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        sda.Fill(ds);

                        string userid;
                        string userName;
                        string eventName;
                        string email;
                        string title;
                        string opnum = "";
                        string opval = "";
                        string returnval;
                        string buttons = "";
                        string html = @"
                        <table border='0' cellpadding='0' cellspacing='0' class='btn btn-primary'>
                          <tbody>
                            <tr>
                              <td align='center'>
                                <table border='0' cellpadding='0' cellspacing='0'>
                                  <tbody>
                                    <tr>
                                      <td> <a href='{0}/PollLandingPage.aspx?ReturnString={1}' target='_blank'>{2}</a> </td>
                                    </tr>
                                  </tbody>
                                </table>
                              </td>
                            </tr>
                          </tbody>
                        </table>";

                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            userid = row[0].ToString();
                            userName= row[1].ToString();
                            email = row[2].ToString();
                            title = row[3].ToString();
                            eventName = row[4].ToString();
                            buttons = "";
                            foreach (DataRow row2 in ds.Tables[1].Rows)
                            {
                                opnum = row2[0].ToString();
                                opval = row2[1].ToString();
                                returnval = string.Format("UserId={0}&EventId={1}&PollId={2}&PollAnswer={3}&ShowPoll=0", userid, EventId, PollId, opnum);
                                buttons += string.Format(html, ConfigurationManager.AppSettings["Server"], clsEmailHandler.Encrypt(returnval), opval, opval);
                                //buttons += string.Format("<form action='{0}/PollLandingPage.aspx?ReturnString={1}' method='post'><input type='submit' value='{2}' /></form>", ConfigurationManager.AppSettings["Server"], clsEmailHandler.Encrypt(returnval), opval);
                            }

                            html = @"
                            <table border='0' cellpadding='0' cellspacing='0' class='btn btn-info'>
                              <tbody>
                                <tr>
                                  <td align='center'>
                                    <table border='0' cellpadding='0' cellspacing='0'>
                                      <tbody>
                                        <tr>
                                          <td> <a href='{0}/PollLandingPage.aspx?ReturnString={1}' target='_blank'>{2}</a> </td>
                                        </tr>
                                      </tbody>
                                    </table>
                                  </td>
                                </tr>
                              </tbody>
                            </table>";
                            returnval = string.Format("ShowPoll=1&PollId={2}", userid, EventId, PollId, opnum);
                            buttons += string.Format(html, ConfigurationManager.AppSettings["Server"], clsEmailHandler.Encrypt(returnval), "Show Poll", opval);

                            //clsEmailHandler.SendPoll(buttons, email, title, userName, eventName);
                        }
                    break;
                }

            }
            
            
              ////send emails

            //string buttonvalue = txtPollDesc.Text.Trim();
            //string polloptionid = genericId.ToString();

            //if (genericId > 0)
            //{
            //    query = string.Format("select u.userid, u.email from Eventusers gu left join users u on u.userid = gu.userid where Eventid = {0}; SELECT PollOptionId, OptionDesc FROM PollOptions WHERE PollDefId = {1}", EventId, genericId);
            //    cmd = new SqlCommand(query);
            //    con.Open();
            //    SqlDataAdapter sda = new SqlDataAdapter();
            //    cmd.Connection = con;
            //    sda.SelectCommand = cmd;
            //    DataSet ds = new DataSet();
            //    sda.Fill(ds);

            //    string userid;
            //    string email;
            //    string opnum = "";
            //    string opval = "";
            //    string returnval;
            //    string buttons = "";
            //    foreach (DataRow row in ds.Tables[0].Rows)
            //    {
            //        userid = row[0].ToString();
            //        email = row[1].ToString();
            //        buttons = "";
            //        foreach (DataRow row2 in ds.Tables[1].Rows)
            //        {
            //            opnum = row2[0].ToString();
            //            opval = row2[1].ToString();
            //            returnval = string.Format("UserId={0}&EventId={1}&PollId={2}&PollAnswer={3}", userid, EventId, genericId, opnum);
            //            buttons += string.Format("<form action='http://localhost:60094/PollLandingPage.aspx?ReturnString={0}' method='post'><input type='submit' value='{1}' /></form>", clsEmailHandler.Encrypt(returnval), opval);
            //        }
            //        clsEmailHandler.SendPoll(buttons, email);
            //    }


            //}
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(prevPage);
        }
    }
}