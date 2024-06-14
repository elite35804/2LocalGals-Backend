using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nexus;

namespace TwoLocalGals.Protected
{
    public partial class Analytics : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);

                if (Globals.GetUserAccess(this) < 7)
                    Globals.LogoutUser(this);

                this.Page.Title = "2LG Analytics";

                if (!IsPostBack)
                {
                    ContractorType.Items.Clear();
                    ContractorType.Items.AddRange(Globals.GetServicesList(Database.GetFranchiseServiceMask(Globals.GetFranchiseMask())));

                    if (!string.IsNullOrEmpty(Globals.GetCookieValue("AnalyticsContractorType")))
                        ContractorType.SelectedValue = Globals.GetCookieValue("AnalyticsContractorType");

                    int selectedMask = Request.Cookies["AnalyticsMask"] != null ? Globals.SafeIntParse(Request.Cookies["AnalyticsMask"].Value) : -1;
                    if (selectedMask == 0) selectedMask = -1;
                    foreach (ListItem franchise in Globals.GetFranchiseList(Globals.GetFranchiseMask(), selectedMask))
                    {
                        CheckBox checkBox = new CheckBox();
                        checkBox.ID = "FRANCHECK" + franchise.Value;
                        checkBox.Text = franchise.Text;
                        checkBox.Checked = franchise.Selected;
                        MenuPanel.Controls.AddAt(MenuPanel.Controls.Count - 2, checkBox);
                    }

                    DateTime mst = Globals.UtcToMst(DateTime.UtcNow);
                    DateTime startDate = Globals.SafeDateParse(Globals.GetCookieValue("AnalyticsStartDate"));
                    DateTime endDate = Globals.SafeDateParse(Globals.GetCookieValue("AnalyticsEndDate"));
                    if (string.IsNullOrEmpty(Request["Range"]) || startDate == DateTime.MinValue) startDate = Globals.StartOfWeek(mst - TimeSpan.FromDays(98));
                    if (string.IsNullOrEmpty(Request["Range"]) || endDate == DateTime.MinValue) endDate = mst;
                    StartDate.Text = startDate.ToString("d");
                    EndDate.Text = endDate.ToString("d");
                    Response.Cookies.Add(new HttpCookie("AnalyticsStartDate", StartDate.Text));
                    Response.Cookies.Add(new HttpCookie("AnalyticsEndDate", EndDate.Text));
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Page_Load EX: " + ex.Message;
            }
        }

        public void ApplyButtonClick(object sender, EventArgs e)
        {
            try
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
                Globals.SetCookieValue("AnalyticsMask", selectedMask.ToString());
                Globals.SetCookieValue("AnalyticsStartDate", StartDate.Text);
                Globals.SetCookieValue("AnalyticsEndDate", EndDate.Text);
                Globals.SetCookieValue("AnalyticsContractorType", ContractorType.SelectedValue.ToString());
                Response.Redirect("Analytics.aspx?Range=True");
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "ApplyButtonClick EX: " + ex.Message;
            }
        }
    }
}