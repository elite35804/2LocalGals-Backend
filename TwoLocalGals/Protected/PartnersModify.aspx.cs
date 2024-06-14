using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nexus;

namespace TwoLocalGals.Protected
{
    public partial class PartnersModify : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);

                if (Globals.GetUserAccess(this) < 7)
                    Globals.LogoutUser(this);

                this.Page.Title = "2LG Users";

                if (!IsPostBack)
                {
                    Globals.SetPreviousPage(this, new string[] { "Reports.aspx" });

                    int access = Globals.GetUserAccess(this);
                    int franchiseMask =  Globals.GetFranchiseMask();
                    int partnerFranchiseMask = 0;
                    string selectedCategory = null;
                    string companyName = Request["company"];

                    PartnerList.Items.Add("(New Partner)");
                    foreach (DBRow val in Database.GetPartnersByCategory(null, franchiseMask, "approved, companyName"))
                    {
                        PartnerList.Items.Add(val.GetString("companyName"));
                        if (val.GetString("companyName") == companyName)
                            PartnerList.SelectedIndex = PartnerList.Items.Count - 1;
                    }

                    if (!string.IsNullOrEmpty(companyName))
                    {
                        DBRow row = Database.GetPartnerByCompany(companyName);
                        if (row == null)
                        {
                            Globals.LogoutUser(this);
                        }
                        else
                        {
                            partnerFranchiseMask = row.GetInt("franchiseMask");
                            CompanyName.Text = row.GetString("companyName");
                            selectedCategory = row.GetString("category");
                            PhoneNumber.Text = row.GetString("phoneNumber");
                            WebAddress.Text = row.GetString("webAddress");
                            Description.Text = row.GetString("description");
                            Approved.Checked = row.GetBool("approved");
                        }
                    }

                    FranchiseList.Items.Clear();
                    FranchiseList.Items.AddRange(Globals.GetFranchiseList(franchiseMask, partnerFranchiseMask));

                    foreach (string category in Database.GetFranchiseDropDown(franchiseMask, "partnerCategoryList"))
                    {
                        BusinessType.Items.Add(category);
                        if (category == selectedCategory)
                            BusinessType.SelectedIndex = BusinessType.Items.Count - 1;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Page_Load EX: " + ex.Message;
            }
        }

        protected void PartnerChanged(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                {
                    if (PartnerList.SelectedIndex == 0) NewClick(sender, e);
                    else Response.Redirect(Globals.BuildQueryString("PartnersModify.aspx", "company", PartnerList.SelectedValue));
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Error PartnerChanged EX: " + ex.Message;
            }
        }

        public void NewClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                {
                    Response.Redirect("PartnersModify.aspx");
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "NewClick EX: " + ex.Message;
            }
        }

        public void DeleteClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request["company"]))
                {
                    string errror = Database.DynamicDeleteWithKey("Partners", "companyName", Request["company"]);
                    if (errror != null)
                    {
                        ErrorLabel.Text = "Error Delete: " + errror;
                    }
                    else
                    {
                        CancelClick(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "DeleteClick EX: " + ex.Message;
            }
        }


        public void CancelClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(Globals.GetPeviousPage(this, "PartnersModify.aspx"));
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
                if (SaveChanges())
                {
                    CancelClick(sender, e);
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SaveClick EX: " + ex.Message;
            }
        }

        public bool SaveChanges()
        {
            try
            {
                if (!string.IsNullOrEmpty(CompanyName.Text))
                {
                    int partnerFranchiseMask = 0;
                    foreach (ListItem item in FranchiseList.Items)
                        if (item.Selected) partnerFranchiseMask |= Globals.IDToMask(Globals.SafeIntParse(item.Value));

                    if (partnerFranchiseMask == 0)
                    {
                        ErrorLabel.Text = "You must select a Franchise";
                        return false;
                    }

                    DBRow row = new DBRow();
                    row.SetValue("companyName", CompanyName.Text);
                    row.SetValue("franchiseMask", partnerFranchiseMask);
                    row.SetValue("category", BusinessType.SelectedValue);
                    row.SetValue("phoneNumber", Globals.FormatPhone(PhoneNumber.Text));
                    row.SetValue("webAddress", WebAddress.Text);
                    row.SetValue("description", Description.Text);
                    row.SetValue("approved", Approved.Checked);

                    string keyValue = Request["company"];
                    if (string.IsNullOrEmpty(keyValue))
                        keyValue = CompanyName.Text;

                    string error = Database.DynamicSetWithKeyString("Partners", "companyName", ref keyValue, row);
                    if (error != null)
                    {
                        ErrorLabel.Text = error;
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SaveChanges EX: " + ex.Message;
                return false;
            }
        }
    }
}