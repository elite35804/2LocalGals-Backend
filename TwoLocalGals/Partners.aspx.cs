using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nexus;
using System.Drawing;

namespace TwoLocalGals
{
    public partial class Partners : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int franMask = Globals.SafeIntParse(Request["franMask"]);
                if (franMask == 0) franMask = -1;

                string selectedCategory = Request["category"];
                if (selectedCategory == "Show All") selectedCategory = null;

                BusinessType.Items.Add("Show All");
                List<string> categoryList = Database.GetFranchiseDropDown(franMask, "partnerCategoryList");
                categoryList.Sort();
                foreach (string category in categoryList)
                {
                    BusinessType.Items.Add(category);
                    if (category == selectedCategory)
                        BusinessType.SelectedIndex = BusinessType.Items.Count - 1;
                }

                bool changeColor = false;
                foreach (DBRow row in Database.GetPartnersByCategory(selectedCategory, franMask, "category, companyName"))
                {
                    if (row.GetBool("approved"))
                    {
                        PartnersDiv.InnerHtml += @"
                        <div class=""ComapnyDiv"" style=""background-color:" + (changeColor ? "#FFF" : "#E0F8E0") + @";"">
                            <h3><b>" + row.GetString("category") + @": </b>" + row.GetString("companyName") + @"</h3>
                            <h3><b>Phone: </b>" + row.GetString("phoneNumber") + @"</h3>
                            <h3><b>Website: </b><a href=""" + row.GetString("webAddress") + @""">" + row.GetString("webAddress").Replace("http://", "").Trim('/') + @"</a></h3>
                            <p>" + row.GetString("description") + @"</p>
                        </div>";
                        changeColor = !changeColor;
                    }
                }
            }
        }

        protected void CategoryChanged(object sender, EventArgs e)
        {
            try
            {
                string url = Globals.BuildQueryString("Partners.aspx", "franMask", Request["franMask"] ?? "0");
                url = Globals.BuildQueryString(url, "category", BusinessType.Text);
                Response.Redirect(url);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Error CategoryChanged EX: " + ex.Message;
            }
        }
    }
}