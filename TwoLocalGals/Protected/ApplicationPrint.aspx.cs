using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nexus;

namespace TwoLocalGals.Protected
{
    public partial class ApplicationPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);
                int userAccess = Globals.GetUserAccess(this);

                if (userAccess < 5)
                    Globals.LogoutUser(this);

                if (!IsPostBack)
                {
                    DBRow contractor = Database.GetContractorDynamicByID(Globals.GetFranchiseMask(), Globals.SafeIntParse(Request["conID"]));

                    if (contractor != null)
                    {
                        FirstName.Text = contractor.GetString("firstName");
                        LastName.Text = contractor.GetString("lastName");
                        Address.Text = contractor.GetString("address");
                        City.Text = contractor.GetString("city");
                        State.Text = contractor.GetString("state");
                        Zip.Text = contractor.GetString("zip");
                        BestPhone.Text = Globals.FormatPhone(contractor.GetString("bestPhone"));
                        AlternatePhone.Text = Globals.FormatPhone(contractor.GetString("alternatePhone"));
                        Email.Text = contractor.GetString("email");
                        SSN.Text = contractor.GetString("ssn");
                        Birthday.Text = contractor.GetDate("birthday").ToString("d");

                        FindUs.Text = contractor.GetString("apFindUs");
                        WorkedBefore.Text = contractor.GetString("apWorkedBefore");
                        WorkedBeforeWhen.Text = contractor.GetString("apWorkedBeforeWhen");
                        HowLongAddress.Text = contractor.GetString("apHowLongAddress");
                        DriversLicense.Text = contractor.GetString("apDriversLicense");
                        DriversLicenseExpire.Text = contractor.GetString("apDriversLicenseExpire");
                        HaveCar.Text = contractor.GetString("apHaveCar");
                        DaysAvailable.Text = contractor.GetString("apDaysAvailable");
                        HighSchool.Text = contractor.GetString("apHighSchool");
                        College.Text = contractor.GetString("apCollege");
                        HighSchoolDiploma.Text = contractor.GetString("apHighSchoolDiploma");
                        Felony.Text = contractor.GetString("apFelony");
                        FelonyDescription.Text = contractor.GetString("apFelonyDescription");
                        RefOneName.Text = contractor.GetString("apRefOneName");
                        RefOnePosition.Text = contractor.GetString("apRefOnePosition");
                        RefOneCompany.Text = contractor.GetString("apRefOneCompany");
                        RefOneAddress.Text = contractor.GetString("apRefOneAddress");
                        RefOnePhoneNumber.Text = contractor.GetString("apRefOnePhoneNumber");
                        RefTwoName.Text = contractor.GetString("apRefTwoName");
                        RefTwoPosition.Text = contractor.GetString("apRefTwoPosition");
                        RefTwoCompany.Text = contractor.GetString("apRefTwoCompany");
                        RefTwoAddress.Text = contractor.GetString("apRefTwoAddress");
                        RefTwoPhoneNumber.Text = contractor.GetString("apRefTwoPhoneNumber");
                        EmpOneName.Text = contractor.GetString("apEmpOneName");
                        EmpOneAddress.Text = contractor.GetString("apEmpOneAddress");
                        EmpOnePhoneNumber.Text = contractor.GetString("apEmpOnePhoneNumber");
                        EmpOneSupervisor.Text = contractor.GetString("apEmpOneSupervisor");
                        EmpOneStartDate.Text = contractor.GetString("apEmpOneStartDate");
                        EmpOneEndDate.Text = contractor.GetString("apEmpOneEndDate");
                        EmpOneJobTitle.Text = contractor.GetString("apEmpOneJobTitle");
                        EmpOneReasonLeave.Text = contractor.GetString("apEmpOneReasonLeave");
                        EmpTwoName.Text = contractor.GetString("apEmpTwoName");
                        EmpTwoAddress.Text = contractor.GetString("apEmpTwoAddress");
                        EmpTwoPhoneNumber.Text = contractor.GetString("apEmpTwoPhoneNumber");
                        EmpTwoSupervisor.Text = contractor.GetString("apEmpTwoSupervisor");
                        EmpTwoStartDate.Text = contractor.GetString("apEmpTwoStartDate");
                        EmpTwoEndDate.Text = contractor.GetString("apEmpTwoEndDate");
                        EmpTwoJobTitle.Text = contractor.GetString("apEmpTwoJobTitle");
                        EmpTwoReasonLeave.Text = contractor.GetString("apEmpTwoReasonLeave");
                        EmpThreeName.Text = contractor.GetString("apEmpThreeName");
                        EmpThreeAddress.Text = contractor.GetString("apEmpThreeAddress");
                        EmpThreePhoneNumber.Text = contractor.GetString("apEmpThreePhoneNumber");
                        EmpThreeSupervisor.Text = contractor.GetString("apEmpThreeSupervisor");
                        EmpThreeStartDate.Text = contractor.GetString("apEmpThreeStartDate");
                        EmpThreeEndDate.Text = contractor.GetString("apEmpThreeEndDate");
                        EmpThreeJobTitle.Text = contractor.GetString("apEmpThreeJobTitle");
                        EmpThreeReasonLeave.Text = contractor.GetString("apEmpThreeReasonLeave");
                        ContactEmployer.Text = contractor.GetString("apContactEmployer");
                    }
                }
            }
            catch { }
        }
    }
}