using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nexus;

namespace TwoLocalGals.Protected
{
    public partial class PortalRequestService : System.Web.UI.Page
    {
        private int customerID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);
                customerID = Globals.GetPortalCustomerID(this);
                if (customerID <= 0) Globals.LogoutUser(this);
                ((CustomerPortal)this.Page.Master).SetActiveMenuItem(3);

                int delID = Globals.SafeIntParse(Request["delID"]);
                if (delID >= 1000) Database.DeleteServiceRequest(delID, customerID);

                List<ServiceRequestStruct> requestList = Database.GetServiceRequests(customerID);
                if (requestList.Count == 0)
                {
                    RequestTable.Style["display"] = "none";
                }
                else
                {
                    bool colorChange = false;
                    foreach (ServiceRequestStruct request in requestList)
                    {
                        TableRow row = new TableRow();
                        row.Cells.Add(Globals.FormatedTableCell(request.requestDate.ToString("d")));
                        row.Cells.Add(Globals.FormatedTableCell(request.timePrefix + " " + request.timeSuffix));
                        row.Cells.Add(Globals.FormatedTableCell(request.notes));
                        row.Cells.Add(Globals.FormatedTableCell(@"<a href=""PortalRequestService.aspx?delID=" + request.serviceRequestID + @""">Remove</a>"));

                        if (colorChange) row.Style["background-color"] = "#D9D9D9";
                        colorChange = !colorChange;
                        RequestTable.Rows.Add(row);
                    }
                }

                if (!IsPostBack)
                {
                    Globals.SetPreviousPage(this, null);
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Error Page_Load EX: " + ex.Message;
            }
        }

        protected void SendRequestClick(object sender, EventArgs e)
        {
            try
            {
                customerID = Globals.GetPortalCustomerID(this);
                if (customerID <= 0) Globals.LogoutUser(this);

                ServiceRequestStruct request = new ServiceRequestStruct();
                request.customerID = customerID;
                request.requestDate = Globals.SafeDateParse(RequestDate.Text);
                request.timePrefix = TimePrefix.Text;
                request.timeSuffix = TimeSuffix.Text;
                request.notes = Notes.Text;

                if (request.requestDate.Date < Globals.UtcToMst(DateTime.UtcNow))
                {
                    ErrorLabel.Text = "Invalid Date, Please Select a Future Date";
                }
                else
                {
                    string error = Database.InsertServiceRequest(request);
                    if (error != null)
                    {
                        ErrorLabel.Text = "Error Sending Cleaning Request: " + error;
                    }
                    else
                    {
                        Response.Redirect("PortalRequestService.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Error Sending Cleaning Request EX: " + ex.Message;
            }
        }
    }
}