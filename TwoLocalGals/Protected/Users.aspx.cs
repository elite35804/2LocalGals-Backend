using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Nexus.Protected
{
    public partial class Users : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);

                if (Globals.GetUserAccess(this) < 5)
                    Globals.LogoutUser(this);

                this.Page.Title = "2LG Users";

                if (!IsPostBack)
                {
                    int access = Globals.GetUserAccess(this);
                    int franchiseMask = Globals.GetUserAccess(this) < 9 ? Globals.GetFranchiseMask() : -1;

                    if (access >= 7) Access.Items.Add(new ListItem("Franchise Owner", "7"));
                    if (access == 9) Access.Items.Add(new ListItem("Administrator", "9"));

                    UserStruct user;
                    UserList.Items.Clear();
                    UserList.Items.Add(new ListItem("(New User)"));
                    UserList.Items.AddRange(Globals.GetUserList(Request["usr"], out user, franchiseMask, access));

                    WootEntry.Text = user.username;
                    BlahEntry.Text = "";
                    Access.SelectedValue = user.access.ToString();

                    ContractorStruct contractor;
                    Contractor.Items.Clear();
                    Contractor.Items.Add(new ListItem("", "0"));
                    Contractor.Items.AddRange(Globals.GetContractorList(Globals.GetFranchiseMask(), user.contractorID, 0, out contractor, true, true));

                    FranchiseList.Items.Clear();
                    FranchiseList.Items.AddRange(Globals.GetFranchiseList(franchiseMask, user.franchiseMask));
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Page_Load EX: " + ex.Message;
            }
        }

        public void UserChanged(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                {
                    if (UserList.SelectedIndex != 0)
                        Response.Redirect("Users.aspx?usr=" + UserList.SelectedItem.ToString());
                    else
                        Response.Redirect("Users.aspx");
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "UserChanged EX: " + ex.Message;
            }
        }

        public void NewClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                    Response.Redirect("Users.aspx");
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "NewClick EX: " + ex.Message;
            }
        }

        public void DeleteClick(object sender, EventArgs e)
        {
            try
            {
                string error = Database.DeleteUser(Request["usr"]);
                if (error == null)
                {
                    if (Request["usr"] == Globals.GetUsername())
                        Globals.LogoutUser(this);
                    Response.Redirect("Users.aspx");
                }
                else ErrorLabel.Text = error;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "DeleteClick EX: " + ex.Message;
            }
        }


        public void CancelClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Users.aspx?usr=" + Request["usr"]);
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "CancelClick EX: " + ex.Message;
            }
        }


        public void SaveClick(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                {
                    Response.Redirect("Users.aspx?usr=" + WootEntry.Text);
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SaveClick EX: " + ex.Message;
            }
        }

        public bool SaveChanges()
        {
            try
            {
                UserStruct user = new UserStruct();
                user.username = WootEntry.Text;
                user.password = BlahEntry.Text;
                user.access = Globals.SafeIntParse(Access.SelectedValue);
                user.contractorID = Globals.SafeIntParse(Contractor.SelectedValue);
                user.franchiseMask = 0;
                foreach (ListItem item in FranchiseList.Items)
                    if (item.Selected) user.franchiseMask |= Globals.IDToMask(Globals.SafeIntParse(item.Value));
                if (user.access == 2) user.franchiseMask = -1;
                if (string.IsNullOrEmpty(Request["usr"]) && string.IsNullOrEmpty(user.username)) return true;

                if (user.access > Globals.GetUserAccess(this))
                {
                    Globals.LogoutUser(this);
                    return false;
                }
                else
                {
                    string error = Database.SetUser(Request["usr"], user);
                    if (error != null)
                    {
                        ErrorLabel.Text = error;
                    }
                    else
                    {
                        if (user.username == Globals.GetUsername())
                            Globals.SetUserValues(user);
                    }
                    return error == null;
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SaveChanges EX: " + ex.Message;
                return false;
            }
        }
    }
}