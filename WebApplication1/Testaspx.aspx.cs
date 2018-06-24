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
    public partial class Testaspx : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                TestDropDown();
                this.BindEmployee();
            }
            


 
        }

        private void TestDropDown()
        {

            string query = "SELECT id, Salary from testSalary";
            SqlCommand cmd = new SqlCommand(query);
            string constr = ConfigurationManager.ConnectionStrings["appmgrConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlDataAdapter sda = new SqlDataAdapter();
            cmd.Connection = con;
            sda.SelectCommand = cmd;
            DataSet ds = new DataSet();
            sda.Fill(ds);

            ddlSalary.DataSource = ds;
            ddlSalary.DataTextField = "Salary";
            ddlSalary.DataValueField = "id";
            ddlSalary.DataBind();
        }
        /* Clear controls values and Set default value to controls */
        private void ClearControls()
        {
            hfAddEdit.Value = "ADD";
            btnSave.Text = "ADD";
            lblHeading.Text = "Add Employee Details";
            hfAddEditEmployeeId.Value = "0";
            hfDeleteEmployeeId.Value = "0";
            txtCountry.Text = string.Empty;
            txtName.Text = string.Empty;
            ddlSalary.Text = string.Empty;
        }

        /* Bind Employee Grid*/
        private void BindEmployee()
        {
            /* Code For Bind Employee Grid*/
            string query = "SELECT E.Id, E.Name, E.Country, TS.Salary as Salary FROM Employee E left join TestSalary TS on E.Salary = TS.id";
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
            lblHeading.Text = "Update Employee Details";

            /* Sets CommandArgument value to hidden field hfAddEditEmployeeId */
            hfAddEditEmployeeId.Value = (sender as Button).CommandArgument;

            /* Sets value from Grid cell to textboxes txtName,txtCountry and txtSalary */
            txtName.Text = ((sender as Button).NamingContainer as GridViewRow).Cells[1].Text;
            txtCountry.Text = ((sender as Button).NamingContainer as GridViewRow).Cells[2].Text;
            //ddlSalary.Selected = ((sender as Button).NamingContainer as GridViewRow).Cells[3].Text;
            ddlSalary.SelectedIndex = ddlSalary.Items.IndexOf(ddlSalary.Items.FindByText(((sender as Button).NamingContainer as GridViewRow).Cells[3].Text));
            /* Change text of button as Update*/
            btnSave.Text = "Update";

            /* Apply Bootstrap Collapse and Expand Class for Grid cells attribute */
            BootstrapCollapsExpand();

            /* Show AddUpdateEmployee Modal Popup */
            mpeAddUpdateEmployee.Show();
        }

        /*Add Employee Detail*/
        protected void Add(object sender, EventArgs e)
        {
            /* Clear Controls Value */
            ClearControls();

            /* Apply Bootstrap Collapse and Expand Class for Grid cells attribute */
            BootstrapCollapsExpand();

            /* Show mpeAddUpdateEmployee Modal Popup */
            mpeAddUpdateEmployee.Show();
        }

        /* Save or Update Employee Details*/
        protected void Save(object sender, EventArgs e)
        {
            /* Code For INSERT OR UPDATE */
            string constr = ConfigurationManager.ConnectionStrings["appmgrConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);

            /* Set employeeId from hfAddEditEmployeeId value for INSERT or UPDATE */
            int employeeId = Convert.ToInt32(hfAddEditEmployeeId.Value);

            string query = string.Empty;
            /* To Check Employee Id For Insert or Update and sets query string variable text*/
            if (employeeId > 0)
            {
                query = "UPDATE Employee SET Name = @Name, Country = @Country, Salary = @Salary WHERE Id = @Id";
            }
            else
            {
                query = "INSERT INTO Employee(Name, Country, Salary) VALUES(@Name, @Country, @Salary)";
            }

            SqlCommand cmd = new SqlCommand(query);

            if (employeeId > 0)
            {
                cmd.Parameters.AddWithValue("@Id", employeeId);
            }
            cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
            cmd.Parameters.AddWithValue("@Country", txtCountry.Text.Trim());
            cmd.Parameters.AddWithValue("@Salary", ddlSalary.SelectedValue); //txtSalary.Text
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            /* Bind Employee Grid*/
            BindEmployee();

            /* Hide mpeAddUpdateEmployee Modal Popup */
            mpeAddUpdateEmployee.Hide();

            /* Clear Controls Value */
            ClearControls();
        }

        /* Delete Emploee Detail*/
        protected void Delete(object sender, EventArgs e)
        {
            /* Apply CommandArgument value to hidden field hfDeleteEmployeeId */
            hfDeleteEmployeeId.Value = (sender as Button).CommandArgument;

            /* Apply Bootstrap Collapse and Expand Class for Grid cells attribute*/
            BootstrapCollapsExpand();

            /* Show DeleteEmployee Modal Popup */
            mpeDeleteEmployee.Show();
        }

        /* If Select Yes on Delete Modal Popup */
        protected void Yes(object sender, EventArgs e)
        {
            /* Code to Delete Employee Record */
            string constr = ConfigurationManager.ConnectionStrings["appmgrConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            int EmployeeId = Convert.ToInt32(hfDeleteEmployeeId.Value);
            SqlCommand cmd = new SqlCommand("DELETE FROM Employee WHERE Id = @Id");
            cmd.Parameters.AddWithValue("@Id", EmployeeId);
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            /* Bind Grid Again To see latest Records*/
            BindEmployee();

            /* Hide Delete Employee Modal Popup */
            mpeDeleteEmployee.Hide();

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
                GridView1.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[4].Attributes["data-hide"] = "expand";
                GridView1.HeaderRow.Cells[5].Attributes["data-hide"] = "expand";
                //Adds THEAD and TBODY to GridView.
                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
    }
}