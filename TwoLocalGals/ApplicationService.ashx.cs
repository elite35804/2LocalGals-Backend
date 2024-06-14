using Newtonsoft.Json;
using Nexus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using TwoLocalGals.Code;

namespace TwoLocalGals
{
    public class ApplicationService : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.StatusCode = 400;
            try
            {
                if (context.Request.ServerVariables["REQUEST_METHOD"] != "POST")
                {
                    context.Response.Write("Bad Request");
                    return;
                }

                string json = null;
                using (StreamReader streamReader = new StreamReader(context.Request.GetBufferedInputStream()))
                {
                    json = streamReader.ReadToEnd();
                }

                ApplicationModel application = JsonConvert.DeserializeObject<ApplicationModel>(json);

                if (application == null)
                {
                    context.Response.Write("Invalid JSON");
                    return;
                }


                if (string.IsNullOrEmpty(application.FirstName) || string.IsNullOrEmpty(application.LastName))
                {
                    context.Response.Write("First and Last Name cannot be blank");
                    return;
                }

                if (string.IsNullOrEmpty(application.Address) || string.IsNullOrEmpty(application.City) || string.IsNullOrEmpty(application.State) || string.IsNullOrEmpty(application.Zip))
                {
                    context.Response.Write("Required fields Address, City, State, and Zip");
                    return;
                }

                if (string.IsNullOrEmpty(application.BestPhone))
                {
                    context.Response.Write("Required field Phone Number");
                    return;
                }

                if (!Globals.ValidEmail(application.Email))
                {
                    context.Response.Write("Invalid Email Address");
                    return;
                }

                if (string.IsNullOrEmpty(application.SSN))
                {
                    context.Response.Write("Required field SSN (Social Security Number)");
                    return;
                }

                if (Globals.DateTimeParse(application.Birthday) == DateTime.MinValue)
                {
                    context.Response.Write("Invalid Birth Date");
                    return;
                }

                if (!string.IsNullOrWhiteSpace(application.CoverLetterFilename) && application.CoverLetterBytes != null)
                {
                    if (!GoodFileExtension(new FileInfo(application.CoverLetterFilename).Extension.ToLower()))
                    {
                        context.Response.Write("Invalid Cover Letter Extension");
                        return;
                    }
                    if (application.CoverLetterBytes.Length > 26214400)
                    {
                        context.Response.Write("Cover Letter must be smaller than 25 MB");
                        return;
                    }
                }

                if (!string.IsNullOrWhiteSpace(application.ResumeFilename) && application.ResumeBytes != null)
                {
                    if (!GoodFileExtension(new FileInfo(application.ResumeFilename).Extension.ToLower()))
                    {
                        context.Response.Write("Invalid Resume Extension");
                        return;
                    }
                    if (application.ResumeBytes.Length > 26214400)
                    {
                        context.Response.Write("Resume must be smaller than 25 MB");
                        return;
                    }
                }

                DBRow row = new DBRow();

                row.SetValue("applicant", true);
                row.SetValue("active", false);
                row.SetValue("franchiseMask", Globals.IDToMask(application.FranchiseID));
                row.SetValue("firstName", application.FirstName);
                row.SetValue("lastName", application.LastName);
                row.SetValue("address", application.Address);
                row.SetValue("city", application.City);
                row.SetValue("state", application.State);
                row.SetValue("zip", application.Zip);
                row.SetValue("bestPhone", Globals.FormatPhone(application.BestPhone));
                row.SetValue("alternatePhone", Globals.FormatPhone(application.AlternatePhone));
                row.SetValue("email", application.Email);
                row.SetValue("ssn", application.SSN);
                row.SetValue("birthday", Globals.DateTimeParseSql(application.Birthday));

                row.SetValue("apFindUs", application.FindUs);
                row.SetValue("apWorkedBefore", application.WorkedBefore);
                row.SetValue("apWorkedBeforeWhen", application.WorkedBeforeWhen);
                row.SetValue("apHowLongAddress", application.HowLongAddress);
                row.SetValue("apDriversLicense", application.DriversLicense);
                row.SetValue("apDriversLicenseExpire", application.DriversLicenseExpire);
                row.SetValue("apHaveCar", application.HaveCar);
                row.SetValue("apDaysAvailable", application.DaysAvailable);
                row.SetValue("apHighSchool", application.HighSchool);
                row.SetValue("apCollege", application.College);
                row.SetValue("apHighSchoolDiploma", application.HighSchoolDiploma);
                row.SetValue("apFelony", application.Felony);
                row.SetValue("apFelonyDescription", application.FelonyDescription);
                row.SetValue("apRefOneName", application.RefOneName);
                row.SetValue("apRefOnePosition", application.RefOnePosition);
                row.SetValue("apRefOneCompany", application.RefOneCompany);
                row.SetValue("apRefOneAddress", application.RefOneAddress);
                row.SetValue("apRefOnePhoneNumber", application.RefOnePhoneNumber);
                row.SetValue("apRefTwoName", application.RefTwoName);
                row.SetValue("apRefTwoPosition", application.RefTwoPosition);
                row.SetValue("apRefTwoCompany", application.RefTwoCompany);
                row.SetValue("apRefTwoAddress", application.RefTwoAddress);
                row.SetValue("apRefTwoPhoneNumber", application.RefTwoPhoneNumber);
                row.SetValue("apEmpOneName", application.EmpOneName);
                row.SetValue("apEmpOneAddress", application.EmpOneAddress);
                row.SetValue("apEmpOnePhoneNumber", application.EmpOnePhoneNumber);
                row.SetValue("apEmpOneSupervisor", application.EmpOneSupervisor);
                row.SetValue("apEmpOneStartDate", application.EmpOneStartDate);
                row.SetValue("apEmpOneEndDate", application.EmpOneEndDate);
                row.SetValue("apEmpOneJobTitle", application.EmpOneJobTitle);
                row.SetValue("apEmpOneReasonLeave", application.EmpOneReasonLeave);
                row.SetValue("apEmpTwoName", application.EmpTwoName);
                row.SetValue("apEmpTwoAddress", application.EmpTwoAddress);
                row.SetValue("apEmpTwoPhoneNumber", application.EmpTwoPhoneNumber);
                row.SetValue("apEmpTwoSupervisor", application.EmpTwoSupervisor);
                row.SetValue("apEmpTwoStartDate", application.EmpTwoStartDate);
                row.SetValue("apEmpTwoEndDate", application.EmpTwoEndDate);
                row.SetValue("apEmpTwoJobTitle", application.EmpTwoJobTitle);
                row.SetValue("apEmpTwoReasonLeave", application.EmpTwoReasonLeave);
                row.SetValue("apEmpThreeName", application.EmpThreeName);
                row.SetValue("apEmpThreeAddress", application.EmpThreeAddress);
                row.SetValue("apEmpThreePhoneNumber", application.EmpThreePhoneNumber);
                row.SetValue("apEmpThreeSupervisor", application.EmpThreeSupervisor);
                row.SetValue("apEmpThreeStartDate", application.EmpThreeStartDate);
                row.SetValue("apEmpThreeEndDate", application.EmpThreeEndDate);
                row.SetValue("apEmpThreeJobTitle", application.EmpThreeJobTitle);
                row.SetValue("apEmpThreeReasonLeave", application.EmpThreeReasonLeave);
                row.SetValue("apContactEmployer", application.ContactEmployer);

                int contractorID = 0;
                string error = Database.DynamicSetWithKeyInt("Contractors", "contractorID", ref contractorID, row);
                if (error != null)
                {
                    Database.LogThis("ApplicationService.SetCustomer: " + error, null);
                    context.Response.Write("Database Error");
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(application.CoverLetterFilename) && application.CoverLetterBytes != null)
                    {
                        string extension = new FileInfo(application.CoverLetterFilename).Extension.ToLower();
                        string uploadPath = Path.Combine(HttpContext.Current.Server.MapPath("~"), "Resume/CoverLetter_" + contractorID + extension);

                        using (FileStream fileStream = new FileStream(uploadPath, FileMode.Create))
                        {
                            fileStream.Write(application.CoverLetterBytes, 0, application.CoverLetterBytes.Length);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(application.ResumeFilename) && application.ResumeBytes != null)
                    {
                        string extension = new FileInfo(application.ResumeFilename).Extension.ToLower();
                        string uploadPath = Path.Combine(HttpContext.Current.Server.MapPath("~"), "Resume/Resume_" + contractorID + extension);

                        using (FileStream fileStream = new FileStream(uploadPath, FileMode.Create))
                        {
                            fileStream.Write(application.ResumeBytes, 0, application.ResumeBytes.Length);
                        }
                    }

                    context.Response.StatusCode = 200;
                    context.Response.Write("Success");
                }
            }
            catch (Exception ex)
            {
                Database.LogThis("ApplicationService", ex);
                context.Response.Write("Exception Caught");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private bool GoodFileExtension(string fileExtension)
        {
            try
            {
                switch (fileExtension)
                {
                    case ".zip": return true;
                    case ".pdf": return true;
                    case ".doc": return true;
                    case ".dot": return true;
                    case ".docx": return true;
                    case ".docm": return true;
                    case ".txt": return true;
                    case ".rar": return true;
                    case ".jpg": return true;
                    case ".xls": return true;
                    case ".wpd": return true;
                    case ".rtf": return true;
                    case ".odt": return true;
                    case ".ods": return true;
                    case ".odp": return true;
                }
            }
            catch { }
            return false;
        }
    }
}