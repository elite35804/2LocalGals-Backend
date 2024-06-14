using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nexus;

namespace TwoLocalGals.Protected
{
    public partial class PortalAddTip : System.Web.UI.Page
    {
        int customerID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);
                customerID = Globals.GetPortalCustomerID(this);
                if (customerID <= 0) Globals.LogoutUser(this);

                if (!IsPostBack)
                {
                    Globals.SetPreviousPage(this, null);
                    LoadAppointment();
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Error Page_Load EX: " + ex.Message;
            }
        }

        private void LoadAppointment()
        {
            try
            {
                AppStruct app;
                string error = Database.GetApointmentpByID(-1, Globals.SafeIntParse(Request["appID"]), out app);

                if (error != null)
                {
                    ErrorLabel.Text = "Error Loading Appointment Error: " + error;
                }
                else
                {
                    if (app.customerID != this.customerID)
                    {
                        Globals.LogoutUser(this);
                    }
                    else
                    {
                        AppointmentLabel.Text = "Add tip for <b>" + app.contractorTitle + "</b> for work done on <b>" + app.appointmentDate.ToString("dddd MM/dd/yyyy") + "</b>";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Error Loading Appointment EX: " + ex.Message;
            }
        }

        protected void AddTipClick(object sender, EventArgs e)
        {
            try
            {
                decimal tip = Globals.FormatMoney(Tip.Text);
                if (tip <= 0 || tip > 1000)
                {
                    ErrorLabel.Text = "Invalid Tip Amount";
                }
                else
                {
                    string error = Database.AddAppointmentTip(Globals.SafeIntParse(Request["appID"]), this.customerID, tip);
                    if (error != null)
                    {
                        ErrorLabel.Text = "Error Saving Tip: " + error;
                    }
                    else
                    {
                        CancelClick(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Error Adding Tip EX: " + ex.Message;
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
    }
}