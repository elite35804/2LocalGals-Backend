using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nexus;
using System.Drawing;

namespace TwoLocalGals.Protected
{
    public partial class SendPromotions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);
                if (Globals.GetUserAccess(this) < 5) Globals.LogoutUser(this);
                SendEmailButton.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(SendEmailButton, null) + ";");
                SendPreviewButton.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(SendPreviewButton, null) + ";");

                int selectedMask = Globals.SafeIntParse(Globals.GetCookieValue("SendPromotionsMask"));
                if (selectedMask == 0) selectedMask = -1;
                foreach (ListItem franchise in Globals.GetFranchiseList(Globals.GetFranchiseMask(), selectedMask))
                {
                    CheckBox checkBox = new CheckBox();
                    checkBox.ID = "FRANCHECK" + franchise.Value;
                    checkBox.Text = franchise.Text;
                    checkBox.Checked = franchise.Selected;
                    checkBox.CssClass = "FranchiseCheckbox";
                    FranchiseCell.Controls.Add(checkBox);
                }

                if (!this.IsPostBack)
                {
                    ServiceType.Items.Clear();
                    ServiceType.Items.AddRange(Globals.GetServicesList(Database.GetFranchiseServiceMask(Globals.GetFranchiseMask())));

                    Globals.SetPreviousPage(this, null);
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Page_Load EX: " + ex.Message;
            }
        }

        protected void CancelClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(Globals.GetPeviousPage(this, "Schedule.aspx"));
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Error Cancel EX: " + ex.Message;
            }
        }

        protected void SendEmailClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLabel.Text = "";
                SendEmailButton.ForeColor = Color.Red;

                if (SubjectTextBox.Text.Length > 200)
                {
                    ErrorLabel.Text = "Subject must be less than 200 charcters.";
                }
                else
                {
                    int selectedMask = 0;
                    foreach (string value in Request.Form)
                    {
                        if (value.Contains("FRANCHECK"))
                        {
                            string franID = value.Substring(value.IndexOf("FRANCHECK") + 9, 2);
                            selectedMask |= Globals.IDToMask(Globals.SafeIntParse(franID));
                        }
                    }
                    Globals.SetCookieValue("SendPromotionsMask", selectedMask.ToString());

                    int sectionMask = Globals.IDToMask(Globals.SafeIntParse(ServiceType.SelectedValue));

                    int queueCount = 0;

                    if (AccountStatus.Text == "Contractors")
                    {
                        foreach (ContractorStruct contractor in Database.GetContractorList(selectedMask, Globals.SafeIntParse(ServiceType.SelectedValue), false, true, false, false))
                        {
                            if (Globals.ValidEmail(contractor.email))
                            {
                                DBRow massEmail = new DBRow();
                                massEmail.SetValue("contractorID", contractor.contractorID);
                                massEmail.SetValue("subject", Globals.Base64Encode(SubjectTextBox.Text));
                                massEmail.SetValue("body", Globals.Base64Encode(BodyTextBox.Text));

                                int massEmailID = 0;
                                Database.DynamicSetWithKeyInt("MassEmail", "massEmailID", ref massEmailID, massEmail);
                                if (massEmailID > 0) queueCount++;
                            }
                        }
                    }
                    else
                    {
                        foreach (CustomerStruct customer in Database.GetCustomers(selectedMask, "sendPromotions = 1 AND accountStatus = '" + AccountStatus.Text + "' AND sectionMask & " + sectionMask + " > 0", "franchiseMask, customerID"))
                        {
                            if (Globals.ValidEmail(customer.email))
                            {
                                DBRow massEmail = new DBRow();
                                massEmail.SetValue("customerID", customer.customerID);
                                massEmail.SetValue("subject", Globals.Base64Encode(SubjectTextBox.Text));
                                massEmail.SetValue("body", Globals.Base64Encode(BodyTextBox.Text));

                                int massEmailID = 0;
                                Database.DynamicSetWithKeyInt("MassEmail", "massEmailID", ref massEmailID, massEmail);
                                if (massEmailID > 0) queueCount++;
                            }
                        }
                    }

                    SendEmailButton.Text = "Batch Email (" + queueCount + " Queued)";
                    SendEmailButton.ForeColor = Color.Green;
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Error SendEmailClick EX: " + ex.Message;
            }
        }

        public void SendPreviewClick(object sender, EventArgs e)
        {
            try
            {
                ErrorLabel.Text = "";
                SendPreviewButton.ForeColor = Color.Red;
                if (SubjectTextBox.Text.Length > 200)
                {
                    ErrorLabel.Text = "Subject must be less than 200 charcters.";
                }
                else
                {
                    int selectedMask = 0;
                    foreach (string value in Request.Form)
                    {
                        if (value.Contains("FRANCHECK"))
                        {
                            string franID = value.Substring(value.IndexOf("FRANCHECK") + 9, 2);
                            selectedMask |= Globals.IDToMask(Globals.SafeIntParse(franID));
                        }
                    }
                    Globals.SetCookieValue("SendPromotionsMask", selectedMask.ToString());

                    if (SearchBox.Text.Contains("ID="))
                    {
                        int testCustomerID = Globals.SafeIntParse(SearchBox.Text.Substring(SearchBox.Text.IndexOf("ID=") + 3));
                        if (testCustomerID <= 0)
                        {
                            ErrorLabel.Text = "Invalid CustomerID";
                        }
                        else
                        {
                            DBRow massEmail = new DBRow();
                            massEmail.SetValue("customerID", testCustomerID);
                            massEmail.SetValue("subject", Globals.Base64Encode(SubjectTextBox.Text));
                            massEmail.SetValue("body", Globals.Base64Encode(BodyTextBox.Text));

                            string error = SendEmail.SendPromotion(massEmail);
                            if (error != null)
                            {
                                ErrorLabel.Text = "Error Sending Test Email: " + error;
                            }
                            else
                            {
                                SendPreviewButton.ForeColor = Color.Green;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SendPreviewClick EX: " + ex.Message;
            }
        }
    }
}