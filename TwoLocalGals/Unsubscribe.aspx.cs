using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nexus;

namespace TwoLocalGals
{
    public partial class Unsubscribe : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int customerID = Globals.SafeIntParse(Globals.Decrypt(Request["A"]));
                if (customerID > 0)
                {
                    DBRow row = new DBRow();
                    row.SetValue("sendPromotions", false);
                    Database.DynamicSetWithKeyInt("Customers", "customerID", ref customerID, row);
                }
            }
            catch { }
        }
    }
}