using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nexus;
using System.Threading;
using System.Web.Security;
using System.Drawing;
using System.Data.SqlClient;

namespace TwoLocalGals
{
    public partial class Login : System.Web.UI.Page
    {
        private bool IsValidUsername(string username)
        {
            try
            {
                if (!string.IsNullOrEmpty(username) && username.Length > 2)
                {
                    foreach (char c in username)
                    {
                        if ((byte)c >= (byte)'A' && (byte)c <= (byte)'Z') continue;
                        if ((byte)c >= (byte)'a' && (byte)c <= (byte)'z') continue;
                        if ((byte)c >= (byte)'0' && (byte)c <= (byte)'9') continue;
                        switch (c)
                        {
                            case '@': continue;
                            case '.': continue;
                            case '!': continue;
                            case '#': continue;
                            case '$': continue;
                            case '&': continue;
                            case '*': continue;
                            case '+': continue;
                            case '-': continue;
                            case '/': continue;
                            case '=': continue;
                            case '?': continue;
                            case '^': continue;
                            case '_': continue;
                            case '|': continue;
                            case '~': continue;
                        }
                        return false;
                    }
                    return true;
                }
            }
            catch { }
            return false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);
                if (Request["logout"] == "true")
                {
                    Globals.LogoutUser(this);
                }
                else if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (Globals.GetUserAccess(this) > 0)
                    {
                        Page.Response.Redirect("~/Protected/Schedule.aspx");
                    }
                    if (Globals.GetPortalCustomerID(this) > 0)
                    {
                        Page.Response.Redirect("~/Protected/PortalAppointments.aspx");
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Request["A"]) && !string.IsNullOrEmpty(Request["B"]))
                    {
                        Username.Text = Globals.Decrypt(Request["A"]);
                        Password.Text = Globals.Decrypt(Request["B"]);
                        Logon_Click(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Page Load Error: " + ex.Message;
            }
        }

        public void Forgot_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLabel.ForeColor = Color.Red;
                if (!IsValidUsername(Username.Text.Trim()) || !Globals.ValidEmail(Username.Text.Trim()) || Username.Text.Trim().Length < 7)
                {
                    ErrorLabel.Text = "Valid Email Required";
                }
                else
                {
                    Thread.Sleep(1500);
                    List<CustomerStruct> customers = Database.GetCustomers(-1, "LOWER(email) LIKE @username", "customerID", new SqlParameter[] { new SqlParameter("username", Username.Text.Trim().ToLower() + "%") });
                    foreach (CustomerStruct customer in customers)
                    {
                        SendEmail.SendCustomerLoginInfo(customer);
                    }
                    ErrorLabel.ForeColor = Color.Black;
                    ErrorLabel.Text = "A login email has been generated.<br>Please check your inbox.";
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Forgot_Click EX: " + ex.Message;
            }
        }

        protected void Logon_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorLabel.ForeColor = Color.Red;
                Globals.CleanAllCookies();

                if (!IsValidUsername(Username.Text.Trim()))
                {
                    ErrorLabel.Text = "Login Failed. Invalid Username";
                }
                else
                {
                    UserStruct user = Database.ValidateUser(Username.Text.Trim(), Password.Text ?? "");
                    if (user.access > 0)
                    {
                        Globals.SetUserValues(user);
                        //Recurring.StartTask("Start", null, System.Web.Caching.CacheItemRemovedReason.Expired);
                        FormsAuthentication.RedirectFromLoginPage(user.username, RememberMe.Checked);
                    }
                    else
                    {
                        string passphrase = Password.Text;
                        int customerID = Globals.PassphraseToCustomerID(passphrase);
                        if (customerID <= 0)
                        {
                            Thread.Sleep(2000);
                            ErrorLabel.Text = "Login Failed. Invalid Credentials";
                        }
                        else
                        {
                            CustomerStruct customer;
                            string error = Database.GetCustomerByID(-1, customerID, out customer);
                            if (error != null)
                            {
                                Thread.Sleep(2000);
                                ErrorLabel.Text = "Account Lookup Error: " + error;
                            }
                            else
                            {
                                if (Globals.SplitEmail(customer.email)[0].ToLower() != Username.Text.Trim().ToLower())
                                {
                                    Thread.Sleep(2000);
                                    ErrorLabel.Text = "Login Failed. Invalid Credentials";
                                }
                                else
                                {
                                    Globals.SetPortalValues(customer);
                                    FormsAuthentication.RedirectFromLoginPage(Globals.FormatFullName(customer.firstName, customer.lastName, "Customer"), RememberMe.Checked);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Thread.Sleep(2000);
                ErrorLabel.Text = "Logon Error: " + ex.Message;
            }
        }
    }
}