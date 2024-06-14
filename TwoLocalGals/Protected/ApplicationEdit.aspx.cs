using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nexus;

namespace TwoLocalGals.Protected
{
    public partial class ApplicationEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);
                int userAccess = Globals.GetUserAccess(this);

                if (userAccess < 5)
                    Globals.LogoutUser(this);

                if (!IsPostBack)
                {
                    DBRow contractor = Database.GetContractorDynamicByID(Globals.GetFranchiseMask(), Globals.SafeIntParse(Request["conID"]));

                    FirstName.Text = contractor.GetString("firstName");
                    LastName.Text = contractor.GetString("lastName");
                    Address.Text = contractor.GetString("address");
                    City.Text = contractor.GetString("city");
                    State.Text = contractor.GetString("state");
                    Zip.Text = contractor.GetString("zip");
                    BestPhone.Text = Globals.FormatPhone(contractor.GetString("bestPhone"));
                    AlternatePhone.Text = Globals.FormatPhone(contractor.GetString("alternatePhone"));
                    Email.Text = contractor.GetString("email");
                    SSN.Text = contractor.GetString("ssn");
                    Birthday.Text = contractor.GetDate("birthday").ToString("d");
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Page_Load EX: " + ex.Message;
            }
        }

        public void CancelClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Contractors.aspx?conID=" + Globals.SafeIntParse(Request["conID"]));
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "CancelClick EX: " + ex.Message;
            }
        }

        public void SaveClick(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SaveClick EX: " + ex.Message;
            }
        }

    }
}