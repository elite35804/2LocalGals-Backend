using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nexus;

namespace TwoLocalGals.Protected
{
    public partial class PortalPartners : System.Web.UI.Page
    {
        int customerID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);
                customerID = Globals.GetPortalCustomerID(this);
                if (customerID <= 0) Globals.LogoutUser(this);
                ((CustomerPortal)this.Page.Master).SetActiveMenuItem(6);

                if (!IsPostBack)
                {
                    Globals.SetPreviousPage(this, null);

                    foreach (string category in Database.GetFranchiseDropDown(Globals.GetPortalFranchiseMask(this), "partnerCategoryList"))
                        BusinessType.Items.Add(category);

                    PartnersLink.HRef = Globals.BuildQueryString("/Partners.aspx", "franMask", Globals.GetPortalFranchiseMask(this));
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Error Page_Load EX: " + ex.Message;
            }
        }

        public void SubmitClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLabel.Text = "";
                SuccessLabel.Text = "";

                if (string.IsNullOrEmpty(CompanyName.Text))
                {
                    ErrorLabel.Text = "Company Name cannot be blank";
                    return;
                }

                if (string.IsNullOrEmpty(WebAddress.Text))
                {
                    ErrorLabel.Text = "Web Address cannot be blank";
                    return;
                }

                if (string.IsNullOrEmpty(PhoneNumber.Text))
                {
                    ErrorLabel.Text = "Phone Number cannot be blank";
                    return;
                }

                if (string.IsNullOrEmpty(BusinessType.Text))
                {
                    ErrorLabel.Text = "Business Type cannot be blank";
                    return;
                }

                if (string.IsNullOrEmpty(Description.Text))
                {
                    ErrorLabel.Text = "Business Description cannot be blank";
                    return;
                }

                string companyName = CompanyName.Text;
                DBRow getRow = Database.GetPartnerByCompany(companyName);
                if (getRow != null)
                {
                    ErrorLabel.Text = "This company has already been submited.";
                }
                else
                {
                    DBRow row = new DBRow();
                    
                    row.SetValue("companyName", CompanyName.Text);
                    row.SetValue("franchiseMask", Globals.GetPortalFranchiseMask(this));
                    row.SetValue("category", BusinessType.Text);
                    row.SetValue("phoneNumber", Globals.FormatPhone(PhoneNumber.Text));
                    row.SetValue("webAddress", WebAddress.Text);
                    row.SetValue("description", Description.Text);
                    
                    string error = Database.DynamicSetWithKeyString("Partners", "companyName", ref companyName, row);
                    if (error != null)
                    {
                        ErrorLabel.Text = "Error Submitting Information: " + error;
                    }
                    else
                    {
                        SuccessLabel.Text = "Your company has been submitted.  Once approved,  it will be added to our Partner list";
                        SubmitButton.Enabled = false;
                        CancelButton.Enabled = false;
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Error SubmitClick EX: " + ex.Message;
            }
        }

        public void CancelClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(Globals.GetPeviousPage(this, "PortalPartners.aspx"));
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Error Cancel EX: " + ex.Message;
            }
        }
    }
}