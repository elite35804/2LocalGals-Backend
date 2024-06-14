using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nexus;

namespace TwoLocalGals.Protected
{
    public partial class PortalReview : System.Web.UI.Page
    {
        private int customerID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);
                customerID = Globals.GetPortalCustomerID(this);
                if (customerID <= 0) Globals.LogoutUser(this);

                if (!IsPostBack)
                {
                    Globals.SetPreviousPage(this, null);
                    LoadAppointment();
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Error Page_Load EX: " + ex.Message;
            }
        }

        private void LoadAppointment()
        {
            try
            {
                AppStruct app;
                string error = Database.GetApointmentpByID(-1, Globals.SafeIntParse(Request["appID"]), out app);

                if (error != null)
                {
                    ErrorLabel.Text = "Error Loading Appointment Error: " + error;
                }
                else
                {
                    if (app.customerID != this.customerID)
                    {
                        Globals.LogoutUser(this);
                    }
                    else
                    {
                        AppointmentLabel.Text = "Submit review for work done on <b>" + app.appointmentDate.ToString("dddd MM/dd/yyyy") + "</b>";

                        List<AppStruct> sameApps = Database.GetSameApointments(app);
                        sameApps.Add(app);

                        foreach (AppStruct sameApp in sameApps)
                        {
                            FollowUpStruct followUp = Database.GetFollowUpByID(sameApp.appointmentID);
                            if (sameApp.appType == 1)
                            {
                                HousekeepingSection.Style["display"] = "table";
                                if (followUp.createdOn > DateTime.MinValue)
                                {
                                    if (followUp.schedulingSatisfaction == 1) HK_SchedulingSatisfaction1.Checked = true;
                                    if (followUp.schedulingSatisfaction == 2) HK_SchedulingSatisfaction2.Checked = true;
                                    if (followUp.schedulingSatisfaction == 3) HK_SchedulingSatisfaction3.Checked = true;
                                    if (followUp.schedulingSatisfaction == 4) HK_SchedulingSatisfaction4.Checked = true;
                                    if (followUp.schedulingSatisfaction == 5) HK_SchedulingSatisfaction5.Checked = true;

                                    if (followUp.timeManagement == 1) HK_ContractorTimeManagement1.Checked = true;
                                    if (followUp.timeManagement == 2) HK_ContractorTimeManagement2.Checked = true;
                                    if (followUp.timeManagement == 3) HK_ContractorTimeManagement3.Checked = true;
                                    if (followUp.timeManagement == 4) HK_ContractorTimeManagement4.Checked = true;
                                    if (followUp.timeManagement == 5) HK_ContractorTimeManagement5.Checked = true;

                                    if (followUp.professionalism == 1) HK_ContractorProfessionalism1.Checked = true;
                                    if (followUp.professionalism == 2) HK_ContractorProfessionalism2.Checked = true;
                                    if (followUp.professionalism == 3) HK_ContractorProfessionalism3.Checked = true;
                                    if (followUp.professionalism == 4) HK_ContractorProfessionalism4.Checked = true;
                                    if (followUp.professionalism == 5) HK_ContractorProfessionalism5.Checked = true;

                                    if (followUp.cleaningQuality == 1) HK_ContractorQuality1.Checked = true;
                                    if (followUp.cleaningQuality == 2) HK_ContractorQuality2.Checked = true;
                                    if (followUp.cleaningQuality == 3) HK_ContractorQuality3.Checked = true;
                                    if (followUp.cleaningQuality == 4) HK_ContractorQuality4.Checked = true;
                                    if (followUp.cleaningQuality == 5) HK_ContractorQuality5.Checked = true;

                                    HK_Better.Text = followUp.notes ?? "";
                                }
                            }

                            if (sameApp.appType == 2)
                            {
                                CarpetCleaningSection.Style["display"] = "table";
                                if (followUp.createdOn > DateTime.MinValue)
                                {
                                    if (followUp.schedulingSatisfaction == 1) CC_SchedulingSatisfaction1.Checked = true;
                                    if (followUp.schedulingSatisfaction == 2) CC_SchedulingSatisfaction2.Checked = true;
                                    if (followUp.schedulingSatisfaction == 3) CC_SchedulingSatisfaction3.Checked = true;
                                    if (followUp.schedulingSatisfaction == 4) CC_SchedulingSatisfaction4.Checked = true;
                                    if (followUp.schedulingSatisfaction == 5) CC_SchedulingSatisfaction5.Checked = true;

                                    if (followUp.timeManagement == 1) CC_ContractorTimeManagement1.Checked = true;
                                    if (followUp.timeManagement == 2) CC_ContractorTimeManagement2.Checked = true;
                                    if (followUp.timeManagement == 3) CC_ContractorTimeManagement3.Checked = true;
                                    if (followUp.timeManagement == 4) CC_ContractorTimeManagement4.Checked = true;
                                    if (followUp.timeManagement == 5) CC_ContractorTimeManagement5.Checked = true;

                                    if (followUp.professionalism == 1) CC_ContractorProfessionalism1.Checked = true;
                                    if (followUp.professionalism == 2) CC_ContractorProfessionalism2.Checked = true;
                                    if (followUp.professionalism == 3) CC_ContractorProfessionalism3.Checked = true;
                                    if (followUp.professionalism == 4) CC_ContractorProfessionalism4.Checked = true;
                                    if (followUp.professionalism == 5) CC_ContractorProfessionalism5.Checked = true;

                                    if (followUp.cleaningQuality == 1) CC_ContractorQuality1.Checked = true;
                                    if (followUp.cleaningQuality == 2) CC_ContractorQuality2.Checked = true;
                                    if (followUp.cleaningQuality == 3) CC_ContractorQuality3.Checked = true;
                                    if (followUp.cleaningQuality == 4) CC_ContractorQuality4.Checked = true;
                                    if (followUp.cleaningQuality == 5) CC_ContractorQuality5.Checked = true;

                                    CC_Better.Text = followUp.notes ?? "";
                                }
                            }

                            if (sameApp.appType == 3)
                            {
                                WindowWashingSection.Style["display"] = "table";
                                if (followUp.createdOn > DateTime.MinValue)
                                {
                                    if (followUp.schedulingSatisfaction == 1) WW_SchedulingSatisfaction1.Checked = true;
                                    if (followUp.schedulingSatisfaction == 2) WW_SchedulingSatisfaction2.Checked = true;
                                    if (followUp.schedulingSatisfaction == 3) WW_SchedulingSatisfaction3.Checked = true;
                                    if (followUp.schedulingSatisfaction == 4) WW_SchedulingSatisfaction4.Checked = true;
                                    if (followUp.schedulingSatisfaction == 5) WW_SchedulingSatisfaction5.Checked = true;

                                    if (followUp.timeManagement == 1) WW_ContractorTimeManagement1.Checked = true;
                                    if (followUp.timeManagement == 2) WW_ContractorTimeManagement2.Checked = true;
                                    if (followUp.timeManagement == 3) WW_ContractorTimeManagement3.Checked = true;
                                    if (followUp.timeManagement == 4) WW_ContractorTimeManagement4.Checked = true;
                                    if (followUp.timeManagement == 5) WW_ContractorTimeManagement5.Checked = true;

                                    if (followUp.professionalism == 1) WW_ContractorProfessionalism1.Checked = true;
                                    if (followUp.professionalism == 2) WW_ContractorProfessionalism2.Checked = true;
                                    if (followUp.professionalism == 3) WW_ContractorProfessionalism3.Checked = true;
                                    if (followUp.professionalism == 4) WW_ContractorProfessionalism4.Checked = true;
                                    if (followUp.professionalism == 5) WW_ContractorProfessionalism5.Checked = true;

                                    if (followUp.cleaningQuality == 1) WW_ContractorQuality1.Checked = true;
                                    if (followUp.cleaningQuality == 2) WW_ContractorQuality2.Checked = true;
                                    if (followUp.cleaningQuality == 3) WW_ContractorQuality3.Checked = true;
                                    if (followUp.cleaningQuality == 4) WW_ContractorQuality4.Checked = true;
                                    if (followUp.cleaningQuality == 5) WW_ContractorQuality5.Checked = true;

                                    WW_Better.Text = followUp.notes ?? "";
                                }
                            }

                            if (sameApp.appType == 4)
                            {
                                HomewatchSection.Style["display"] = "table";
                                if (followUp.createdOn > DateTime.MinValue)
                                {
                                    if (followUp.schedulingSatisfaction == 1) HW_SchedulingSatisfaction1.Checked = true;
                                    if (followUp.schedulingSatisfaction == 2) HW_SchedulingSatisfaction2.Checked = true;
                                    if (followUp.schedulingSatisfaction == 3) HW_SchedulingSatisfaction3.Checked = true;
                                    if (followUp.schedulingSatisfaction == 4) HW_SchedulingSatisfaction4.Checked = true;
                                    if (followUp.schedulingSatisfaction == 5) HW_SchedulingSatisfaction5.Checked = true;

                                    if (followUp.timeManagement == 1) HW_ContractorTimeManagement1.Checked = true;
                                    if (followUp.timeManagement == 2) HW_ContractorTimeManagement2.Checked = true;
                                    if (followUp.timeManagement == 3) HW_ContractorTimeManagement3.Checked = true;
                                    if (followUp.timeManagement == 4) HW_ContractorTimeManagement4.Checked = true;
                                    if (followUp.timeManagement == 5) HW_ContractorTimeManagement5.Checked = true;

                                    if (followUp.professionalism == 1) HW_ContractorProfessionalism1.Checked = true;
                                    if (followUp.professionalism == 2) HW_ContractorProfessionalism2.Checked = true;
                                    if (followUp.professionalism == 3) HW_ContractorProfessionalism3.Checked = true;
                                    if (followUp.professionalism == 4) HW_ContractorProfessionalism4.Checked = true;
                                    if (followUp.professionalism == 5) HW_ContractorProfessionalism5.Checked = true;

                                    if (followUp.cleaningQuality == 1) HW_ContractorQuality1.Checked = true;
                                    if (followUp.cleaningQuality == 2) HW_ContractorQuality2.Checked = true;
                                    if (followUp.cleaningQuality == 3) HW_ContractorQuality3.Checked = true;
                                    if (followUp.cleaningQuality == 4) HW_ContractorQuality4.Checked = true;
                                    if (followUp.cleaningQuality == 5) HW_ContractorQuality5.Checked = true;

                                    HW_Better.Text = followUp.notes ?? "";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Error Loading Appointment EX: " + ex.Message;
            }
        }

        protected void SubmitClick(object sender, EventArgs e)
        {
            try
            {
                AppStruct app;
                string error = Database.GetApointmentpByID(-1, Globals.SafeIntParse(Request["appID"]), out app);
                if (error != null)
                {
                    ErrorLabel.Text = "Error Loading Appointment Error: " + error;
                }
                else
                {
                    if (app.customerID != this.customerID)
                    {
                        Globals.LogoutUser(this);
                    }
                    else
                    {
                        FollowUpStruct HK_FollowUp = new FollowUpStruct();

                        HK_FollowUp.schedulingSatisfaction = 0;
                        if (HK_SchedulingSatisfaction1.Checked) HK_FollowUp.schedulingSatisfaction = 1;
                        if (HK_SchedulingSatisfaction2.Checked) HK_FollowUp.schedulingSatisfaction = 2;
                        if (HK_SchedulingSatisfaction3.Checked) HK_FollowUp.schedulingSatisfaction = 3;
                        if (HK_SchedulingSatisfaction4.Checked) HK_FollowUp.schedulingSatisfaction = 4;
                        if (HK_SchedulingSatisfaction5.Checked) HK_FollowUp.schedulingSatisfaction = 5;

                        HK_FollowUp.timeManagement = 0;
                        if (HK_ContractorTimeManagement1.Checked) HK_FollowUp.timeManagement = 1;
                        if (HK_ContractorTimeManagement2.Checked) HK_FollowUp.timeManagement = 2;
                        if (HK_ContractorTimeManagement3.Checked) HK_FollowUp.timeManagement = 3;
                        if (HK_ContractorTimeManagement4.Checked) HK_FollowUp.timeManagement = 4;
                        if (HK_ContractorTimeManagement5.Checked) HK_FollowUp.timeManagement = 5;

                        HK_FollowUp.professionalism = 0;
                        if (HK_ContractorProfessionalism1.Checked) HK_FollowUp.professionalism = 1;
                        if (HK_ContractorProfessionalism2.Checked) HK_FollowUp.professionalism = 2;
                        if (HK_ContractorProfessionalism3.Checked) HK_FollowUp.professionalism = 3;
                        if (HK_ContractorProfessionalism4.Checked) HK_FollowUp.professionalism = 4;
                        if (HK_ContractorProfessionalism5.Checked) HK_FollowUp.professionalism = 5;

                        HK_FollowUp.cleaningQuality = 0;
                        if (HK_ContractorQuality1.Checked) HK_FollowUp.cleaningQuality = 1;
                        if (HK_ContractorQuality2.Checked) HK_FollowUp.cleaningQuality = 2;
                        if (HK_ContractorQuality3.Checked) HK_FollowUp.cleaningQuality = 3;
                        if (HK_ContractorQuality4.Checked) HK_FollowUp.cleaningQuality = 4;
                        if (HK_ContractorQuality5.Checked) HK_FollowUp.cleaningQuality = 5;

                        HK_FollowUp.notes = HK_Better.Text;

                        FollowUpStruct CC_FollowUp = new FollowUpStruct();

                        CC_FollowUp.schedulingSatisfaction = 0;
                        if (CC_SchedulingSatisfaction1.Checked) CC_FollowUp.schedulingSatisfaction = 1;
                        if (CC_SchedulingSatisfaction2.Checked) CC_FollowUp.schedulingSatisfaction = 2;
                        if (CC_SchedulingSatisfaction3.Checked) CC_FollowUp.schedulingSatisfaction = 3;
                        if (CC_SchedulingSatisfaction4.Checked) CC_FollowUp.schedulingSatisfaction = 4;
                        if (CC_SchedulingSatisfaction5.Checked) CC_FollowUp.schedulingSatisfaction = 5;

                        CC_FollowUp.timeManagement = 0;
                        if (CC_ContractorTimeManagement1.Checked) CC_FollowUp.timeManagement = 1;
                        if (CC_ContractorTimeManagement2.Checked) CC_FollowUp.timeManagement = 2;
                        if (CC_ContractorTimeManagement3.Checked) CC_FollowUp.timeManagement = 3;
                        if (CC_ContractorTimeManagement4.Checked) CC_FollowUp.timeManagement = 4;
                        if (CC_ContractorTimeManagement5.Checked) CC_FollowUp.timeManagement = 5;

                        CC_FollowUp.professionalism = 0;
                        if (CC_ContractorProfessionalism1.Checked) CC_FollowUp.professionalism = 1;
                        if (CC_ContractorProfessionalism2.Checked) CC_FollowUp.professionalism = 2;
                        if (CC_ContractorProfessionalism3.Checked) CC_FollowUp.professionalism = 3;
                        if (CC_ContractorProfessionalism4.Checked) CC_FollowUp.professionalism = 4;
                        if (CC_ContractorProfessionalism5.Checked) CC_FollowUp.professionalism = 5;

                        CC_FollowUp.cleaningQuality = 0;
                        if (CC_ContractorQuality1.Checked) CC_FollowUp.cleaningQuality = 1;
                        if (CC_ContractorQuality2.Checked) CC_FollowUp.cleaningQuality = 2;
                        if (CC_ContractorQuality3.Checked) CC_FollowUp.cleaningQuality = 3;
                        if (CC_ContractorQuality4.Checked) CC_FollowUp.cleaningQuality = 4;
                        if (CC_ContractorQuality5.Checked) CC_FollowUp.cleaningQuality = 5;

                        CC_FollowUp.notes = CC_Better.Text;

                        FollowUpStruct WW_FollowUp = new FollowUpStruct();

                        WW_FollowUp.schedulingSatisfaction = 0;
                        if (WW_SchedulingSatisfaction1.Checked) WW_FollowUp.schedulingSatisfaction = 1;
                        if (WW_SchedulingSatisfaction2.Checked) WW_FollowUp.schedulingSatisfaction = 2;
                        if (WW_SchedulingSatisfaction3.Checked) WW_FollowUp.schedulingSatisfaction = 3;
                        if (WW_SchedulingSatisfaction4.Checked) WW_FollowUp.schedulingSatisfaction = 4;
                        if (WW_SchedulingSatisfaction5.Checked) WW_FollowUp.schedulingSatisfaction = 5;

                        WW_FollowUp.timeManagement = 0;
                        if (WW_ContractorTimeManagement1.Checked) WW_FollowUp.timeManagement = 1;
                        if (WW_ContractorTimeManagement2.Checked) WW_FollowUp.timeManagement = 2;
                        if (WW_ContractorTimeManagement3.Checked) WW_FollowUp.timeManagement = 3;
                        if (WW_ContractorTimeManagement4.Checked) WW_FollowUp.timeManagement = 4;
                        if (WW_ContractorTimeManagement5.Checked) WW_FollowUp.timeManagement = 5;

                        WW_FollowUp.professionalism = 0;
                        if (WW_ContractorProfessionalism1.Checked) WW_FollowUp.professionalism = 1;
                        if (WW_ContractorProfessionalism2.Checked) WW_FollowUp.professionalism = 2;
                        if (WW_ContractorProfessionalism3.Checked) WW_FollowUp.professionalism = 3;
                        if (WW_ContractorProfessionalism4.Checked) WW_FollowUp.professionalism = 4;
                        if (WW_ContractorProfessionalism5.Checked) WW_FollowUp.professionalism = 5;

                        WW_FollowUp.cleaningQuality = 0;
                        if (WW_ContractorQuality1.Checked) WW_FollowUp.cleaningQuality = 1;
                        if (WW_ContractorQuality2.Checked) WW_FollowUp.cleaningQuality = 2;
                        if (WW_ContractorQuality3.Checked) WW_FollowUp.cleaningQuality = 3;
                        if (WW_ContractorQuality4.Checked) WW_FollowUp.cleaningQuality = 4;
                        if (WW_ContractorQuality5.Checked) WW_FollowUp.cleaningQuality = 5;

                        WW_FollowUp.notes = WW_Better.Text;

                        FollowUpStruct HW_FollowUp = new FollowUpStruct();

                        HW_FollowUp.schedulingSatisfaction = 0;
                        if (HW_SchedulingSatisfaction1.Checked) HW_FollowUp.schedulingSatisfaction = 1;
                        if (HW_SchedulingSatisfaction2.Checked) HW_FollowUp.schedulingSatisfaction = 2;
                        if (HW_SchedulingSatisfaction3.Checked) HW_FollowUp.schedulingSatisfaction = 3;
                        if (HW_SchedulingSatisfaction4.Checked) HW_FollowUp.schedulingSatisfaction = 4;
                        if (HW_SchedulingSatisfaction5.Checked) HW_FollowUp.schedulingSatisfaction = 5;

                        HW_FollowUp.timeManagement = 0;
                        if (HW_ContractorTimeManagement1.Checked) HW_FollowUp.timeManagement = 1;
                        if (HW_ContractorTimeManagement2.Checked) HW_FollowUp.timeManagement = 2;
                        if (HW_ContractorTimeManagement3.Checked) HW_FollowUp.timeManagement = 3;
                        if (HW_ContractorTimeManagement4.Checked) HW_FollowUp.timeManagement = 4;
                        if (HW_ContractorTimeManagement5.Checked) HW_FollowUp.timeManagement = 5;

                        HW_FollowUp.professionalism = 0;
                        if (HW_ContractorProfessionalism1.Checked) HW_FollowUp.professionalism = 1;
                        if (HW_ContractorProfessionalism2.Checked) HW_FollowUp.professionalism = 2;
                        if (HW_ContractorProfessionalism3.Checked) HW_FollowUp.professionalism = 3;
                        if (HW_ContractorProfessionalism4.Checked) HW_FollowUp.professionalism = 4;
                        if (HW_ContractorProfessionalism5.Checked) HW_FollowUp.professionalism = 5;

                        HW_FollowUp.cleaningQuality = 0;
                        if (HW_ContractorQuality1.Checked) HW_FollowUp.cleaningQuality = 1;
                        if (HW_ContractorQuality2.Checked) HW_FollowUp.cleaningQuality = 2;
                        if (HW_ContractorQuality3.Checked) HW_FollowUp.cleaningQuality = 3;
                        if (HW_ContractorQuality4.Checked) HW_FollowUp.cleaningQuality = 4;
                        if (HW_ContractorQuality5.Checked) HW_FollowUp.cleaningQuality = 5;

                        HW_FollowUp.notes = HW_Better.Text;

                        List<AppStruct> sameApps = Database.GetSameApointments(app);
                        sameApps.Add(app);

                        foreach (AppStruct sameApp in sameApps)
                        {
                            FollowUpStruct followUp = new FollowUpStruct();
                            if (sameApp.appType == 1) followUp = HK_FollowUp;
                            if (sameApp.appType == 2) followUp = CC_FollowUp;
                            if (sameApp.appType == 3) followUp = WW_FollowUp;
                            if (sameApp.appType == 4) followUp = HW_FollowUp;

                            followUp.appointmentID = sameApp.appointmentID;
                            error = Database.SetFollowUp(followUp);
                            if (error != null)
                            {
                                ErrorLabel.Text = "Error Saving Review (ContractorID " + sameApp.contractorID + "): " + error;
                                return;
                            }

                            SendEmail.SendFollowUp(followUp.appointmentID);
                        }
                        CancelClick(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Submit Error EX: " + ex.Message;
            }
        }

        protected void CancelClick(object sender, EventArgs e)
        {
            try
            {
               Response.Redirect(Globals.GetPeviousPage(this, "PortalAppointments.aspx"));
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Error Cancel EX: " + ex.Message;
            }
        }
    }
}