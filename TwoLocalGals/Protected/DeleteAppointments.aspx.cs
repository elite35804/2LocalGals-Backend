using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Nexus.Protected
{
    public partial class WebFormDeleteAppointments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);

                if (Globals.GetUserAccess(this) < 5)
                    Globals.LogoutUser(this);

                this.Page.Title = "2LG Delete Appointments";

                StartDate.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                EndDate.Attributes.Add("onkeydown", "return (event.keyCode!=13);");

                int customerID = Globals.SafeIntParse(Request["custID"]);

                CustomerStruct customer;
                string error = Database.GetCustomerByID(Globals.GetFranchiseMask(), customerID, out customer);

                if (error == null) TitleLabel.Text = customer.customerTitle + " - Delete Appointment Range";
                else ErrorLabel.Text = error;

                DeleteRangeButton.Enabled = (error == null && customerID > 0);

                if (!IsPostBack && Request.UrlReferrer != null)
                    ViewState["PreviousPageUrl"] = Request.UrlReferrer.ToString();
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Page_Load EX: " + ex.Message;
            }
        }

        public void DeleteRangeButtonClick(object sender, EventArgs e)
        {
            try
            {
                int customerID = Globals.SafeIntParse(Request["custID"]);

                DateTime startDate = Globals.DateTimeParse(StartDate.Text);
                DateTime endDate = Globals.DateTimeParse(EndDate.Text);

                List<TransactionStruct> transList = Database.GetTransactions(Globals.GetFranchiseMask(), customerID, startDate, endDate, "T.dateCreated");
                if (transList.Count == 0)
                {
                    string error = Database.DeleteAppointmentRange(Globals.GetFranchiseMask(), customerID, startDate, endDate);

                    if (error == null) CancelClick(sender, e);
                    else ErrorLabel.Text = error;
                }
                else ErrorLabel.Text = "Delete canceled, appointments within range contain transactions";
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "DeleteRangeButtonClick EX: " + ex.Message;
            }
        }

        public void CancelClick(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["PreviousPageUrl"] != null)
                    Response.Redirect(Globals.BuildQueryString(ViewState["PreviousPageUrl"].ToString(), "DoScroll", "Y"));
                else
                    Response.Redirect("Customers.aspx?custID=" + Globals.SafeIntParse(Request["custID"]));
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "CancelClick EX: " + ex.Message;
            }
        }
    }
}