using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nexus;
using System.Reflection;

namespace TwoLocalGals
{
    public partial class CustomerPortalMobile : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RefreshPoints();
        }

        public void LinkSaveCommand(object sender, CommandEventArgs e)
        {
            try
            {
                if (e.CommandArgument != null)
                {
                    bool ret = true;
                    MethodInfo saveMethod = this.Page.GetType().GetMethod("SaveChanges");
                    if (saveMethod != null) ret = (bool)saveMethod.Invoke(this.Page, null);
                    if (ret) Response.Redirect(e.CommandArgument.ToString());
                }
            }
            catch { }
        }

        public void SetActiveMenuItem(int index)
        {
            switch (index)
            {
                case 1: AccountMainMenuButton.Style["background-color"] = "#31475E"; break;
                case 2: AppointmentsMainMenuButton.Style["background-color"] = "#31475E"; break;
                case 3: RequestServiceMainMenuButton.Style["background-color"] = "#31475E"; break;
                case 4: GiftCardMainMenuButton.Style["background-color"] = "#31475E"; break;
                case 5: ReferralMainMenuButton.Style["background-color"] = "#31475E"; break;
                case 6: PartnersMainMenuButton.Style["background-color"] = "#31475E"; break;
            }
        }

        public void RefreshPoints()
        {
            decimal points, ratePerHour;
            string error = Database.GetCustomerPoints(-1, Globals.GetPortalCustomerID(this.Page), out points, out ratePerHour);
            if (error == null) CustomerPoints.InnerHtml = @"You have <span style=""font-weight:bold;"">" + Globals.FormatPoints(points) + @"</span> points worth <span style=""font-weight:bold;"">" + Globals.FormatHours(points / ratePerHour) + @"</span> hours";
        }
    }
}