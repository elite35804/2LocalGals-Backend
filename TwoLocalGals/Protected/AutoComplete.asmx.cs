using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Nexus
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]

    public class AutoComplete : System.Web.Services.WebService
    {
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetCustomerCompletionList(string prefixText, int count)
        {
            List<string> ret = new List<string>();

            try
            {
                int franchiseMask = Globals.GetFranchiseMask();
                string[] split = prefixText.Split(' ');
                List<CustomerStruct> customerList = Database.SearchCustomerList(franchiseMask, split[0]);

                foreach (CustomerStruct c in customerList)
                {
                    string line = c.customerTitle + ", ";
                    if (!string.IsNullOrEmpty(c.bestPhone)) line += Globals.FormatPhone(c.bestPhone) + ", ";
                    if (!string.IsNullOrEmpty(c.alternatePhoneOne)) line += Globals.FormatPhone(c.alternatePhoneOne) + ", ";
                    if (!string.IsNullOrEmpty(c.alternatePhoneTwo)) line += Globals.FormatPhone(c.alternatePhoneTwo) + ", ";
                    if (!string.IsNullOrEmpty(c.locationAddress)) line += Globals.FormatPhone(c.locationAddress) + ", ";
                    if (!string.IsNullOrEmpty(c.email)) line += Globals.FormatPhone(c.email) + ", ";
                    line += "ID=" + c.customerID;

                    for (int i = 1; i < split.Length; i++)
                        if (!line.ToLower().Contains(split[i].ToLower())) goto SKIP_ADD;

                    ret.Add(line);
                    if (ret.Count >= count) break;

                SKIP_ADD: ;
                }
            }
            catch { }
            return ret.ToArray();
        }
    }
}
