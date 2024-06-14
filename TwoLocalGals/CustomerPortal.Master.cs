using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using Nexus;

namespace TwoLocalGals
{
    public partial class CustomerPortal : System.Web.UI.MasterPage
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
                case 1: AccountMainMenuButton.CssClass = "MainMenuItemActive"; break;
                case 2: AppointmentsMainMenuButton.CssClass = "MainMenuItemActive"; break;
                case 3: RequestServiceMainMenuButton.CssClass = "MainMenuItemActive"; break;
                case 4: GiftCardMainMenuButton.CssClass = "MainMenuItemActive"; break;
                case 5: ReferralMainMenuButton.CssClass = "MainMenuItemActive"; break;
                case 6: PartnersMainMenuButton.CssClass = "MainMenuItemActive"; break;
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