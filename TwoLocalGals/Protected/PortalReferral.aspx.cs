using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nexus;

namespace TwoLocalGals.Protected
{
    public partial class PortalReferral : System.Web.UI.Page
    {
        private int customerID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);
                customerID = Globals.GetPortalCustomerID(this);
                if (customerID <= 0) Globals.LogoutUser(this);
                ((CustomerPortal)this.Page.Master).SetActiveMenuItem(5);

                if (!IsPostBack)
                {
                    if (Request["success"] == "t") NormalDiv.Visible = false;
                    else SuccessDiv.Visible = false;

                    Globals.SetPreviousPage(this, null);
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Error Page_Load EX: " + ex.Message;
            }
        }

        protected void CancelClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(Globals.GetPeviousPage(this, "PortalAppointments.aspx"));
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Error Cancel EX: " + ex.Message;
            }
        }

        protected void SendClick(object sender, EventArgs e)
        {
            try
            {
                 CustomerStruct customer;
                string error = Database.GetCustomerByID(-1, customerID, out customer);
                if (error != null)
                {
                    ErrorLabel.Text = "Error Loading Customer(" + customerID + "): " + error;
                }
                else
                {
                    if (string.IsNullOrEmpty(Name.Text))
                    {
                        ErrorLabel.Text = "Name Cannot be Blank";
                    }
                    else
                    {
                        if (!Globals.ValidEmail(Email.Text))
                        {
                            ErrorLabel.Text = "Invalid Email Address";
                        }
                        else
                        {
                            error = SendEmail.SendReferral(customer, Name.Text, Email.Text);
                            if (error != null)
                            {
                                ErrorLabel.Text = "Send Email Error: " + error;
                            }
                            else
                            {
                                Response.Redirect("PortalReferral.aspx?success=t");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Error SendClick EX: " + ex.Message;
            }
        }
    }
}