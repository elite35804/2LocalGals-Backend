using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nexus;

namespace TwoLocalGals.Protected
{
    public partial class WebQuoteReply : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);
                if (Globals.GetUserAccess(this) < 5) Globals.LogoutUser(this);
                MaintainScrollPositionOnPostBack = true;

                if (!IsPostBack)
                {
                    if (Request.UrlReferrer == null) 
                        Session["PreviousPageUrl"] = null;
                    else if (!Request.UrlReferrer.ToString().Contains("WebQuoteReply.aspx"))
                        Session["PreviousPageUrl"] = Request.UrlReferrer.ToString();

                    LoadCustomerInfo();
                    LoadQuoteTemplates();
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Page_Load EX: " + ex.Message;
            }
        }

        public void SendClick(object sender, EventArgs e)
        {
            try
            {
                CustomerStruct customer;
                string error = Database.GetCustomerByID(Globals.GetFranchiseMask(), Globals.SafeIntParse(Request["custID"]), out customer);
                if (error != null)
                {
                    ErrorLabel.Text = "Error retrieving customer information: " + error;
                }
                else
                {
                    FranchiseStruct franchise = Globals.GetFranchiseByMask(customer.franchiseMask);
                    if (franchise.franchiseID == 0)
                    {
                        ErrorLabel.Text = "Could Not Identify Customer Franchise";
                    }
                    else
                    {
                        string emailBody = Globals.ReplaceEmailDefinedStrings(ReplyTextBox.Text.Replace("\0", ""), franchise, customer.firstName, customer.lastName);
                        if (null == (error = SendEmail.SendWebQuoteReply(emailBody, customer.email, franchise)))
                        {
                            if (null != (error = Database.SetCustomerQuoteReply(customer.customerID, ReplyTextBox.Text))) ErrorLabel.Text = error;
                            else CancelClick(sender, e);
                        }
                        else ErrorLabel.Text = error;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SendClick EX: " + ex.Message;
            }
        }

        public void CancelClick(object sender, EventArgs e)
        {
            try
            {
                if (Session["PreviousPageUrl"] != null)
                    Response.Redirect(Session["PreviousPageUrl"].ToString());
                else
                    Response.Redirect("Customers.aspx?custID=" + Globals.SafeIntParse(Request["custID"]));
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "CancelClick EX: " + ex.Message;
            }
        }

        public void SaveQuoteClick(object sender, EventArgs e)
        {
            try
            {
                if (QuoteTemplateName.Text != "(Sent Customer Quote)")
                {
                    QuoteTemplate template = new QuoteTemplate();
                    template.franchiseMask = Globals.GetFranchiseMask();
                    template.templateName = QuoteTemplateName.Text;
                    template.templateValue = ReplyTextBox.Text.Replace("\0", "");
                    string error = Database.SetQuoteTemplate(template);

                    if (error == null) 
                    {
                        string url = Globals.BuildQueryString("WebQuoteReply.aspx", "custID",  Globals.SafeIntParse(Request["custID"]));
                        url = Globals.BuildQueryString(url, "template", template.templateName);
                        Response.Redirect(url);
                    }
                    else ErrorLabel.Text = error;
                }
                else ErrorLabel.Text = "Inavlid template name (Sent Customer Quote)";
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SaveQuoteClick EX: " + ex.Message;
            }
        }

        public void DeleteQuoteClick(object sender, EventArgs e)
        {
            try
            {
                if (QuoteTemplateName.Text != "(Sent Customer Quote)")
                {
                    int franchiseMask = Globals.GetFranchiseMask();
                    if (Globals.GetUserAccess(this) >= 9) franchiseMask = -1;

                    string error = Database.DeleteQuoteTemplate(franchiseMask, QuoteTemplateName.Text);
                    if (error == null)
                    {
                        Response.Redirect("WebQuoteReply.aspx?custID=" + Globals.SafeIntParse(Request["custID"]));
                    }
                    else ErrorLabel.Text = error;
                }
                else ErrorLabel.Text = "Inavlid template name (Sent Customer Quote)";
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "DeleteQuoteClick EX: " + ex.Message;
            }
        }

        public void DeleteClick(object sender, EventArgs e)
        {
            try
            {
                string error = Database.DeleteCustomer(Globals.SafeIntParse(Request["custID"]));
                if (error == null) Response.Redirect("Customers.aspx");
                else ErrorLabel.Text = error;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "DeleteClick EX: " + ex.Message;
            }
        }

        public void LoadQuoteTemplates()
        {
            try
            {
                
                foreach (QuoteTemplate template in Database.GetQuoteTemplates(Globals.GetFranchiseMask()))
                {
                    Templates.Items.Add(new ListItem(template.templateName, Globals.Base64Encode(template.templateValue.Replace("\0", ""))));
                    if (template.templateName == Request["template"]) Templates.SelectedIndex = Templates.Items.Count - 1;
                }

                if (Templates.SelectedIndex > 0)
                {
                    ReplyTextBox.Text = Globals.Base64Decode(Templates.SelectedValue);
                    QuoteTemplateName.Text = Templates.SelectedItem.Text;
                }
                else if (Templates.SelectedItem != null && Templates.SelectedItem.Text == "(Sent Customer Quote)")
                {
                    ReplyTextBox.Text = Globals.Base64Decode(Templates.SelectedValue);
                }
                else
                {
                    Templates.Items.Insert(0, new ListItem("", ""));
                    Templates.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "LoadQuoteTemplates EX: " + ex.Message;
            }
        }

        public void LoadCustomerInfo()
        {
            try
            {
                CustomerStruct customer;
                string error = Database.GetCustomerByID(Globals.GetFranchiseMask(), Globals.SafeIntParse(Request["custID"]), out customer);
                if (error == null)
                {
                    CustomerTitleLabel.Text = @"<a href=""Customers.aspx?custID=" + customer.customerID + @""">" + customer.customerTitle + @"</a>";
                    SendButton.Text = "Send Quote (" + customer.email + ")";
                    if (customer.quoteReply) SendButton.Style["color"] = "Green";
                    if (!string.IsNullOrEmpty(customer.quoteValue)) Templates.Items.Add(new ListItem("(Sent Customer Quote)", Globals.Base64Encode(customer.quoteValue)));

                    BestPhone.Text = Globals.FormatPhone(customer.bestPhone);
                    AlternatePhone.Text = Globals.FormatPhone(customer.alternatePhoneOne);
                    Address.Text = customer.locationAddress;
                    City.Text = customer.locationCity;
                    State.Text = customer.locationState;
                    Zip.Text = customer.locationZip;
                    AccountType.Text = customer.accountType;
                    Advertisement.Text = customer.advertisement;
                    PreferredContact.Text = customer.preferredContact;
                    Frequency.Text = customer.NC_Frequency;
                    CleanRating.Text = customer.NC_CleanRating;
                    Bedrooms.Text = customer.NC_Bedrooms;
                    Bathrooms.Text = customer.NC_Bathrooms;
                    SquareFootage.Text = customer.NC_SquareFootage;
                    Pets.Text = customer.NC_Pets;

                    List<string> flooringList = new List<string>();
                    if (customer.NC_FlooringCarpet) flooringList.Add("Carpet");
                    if (customer.NC_FlooringHardwood) flooringList.Add("Hardwood");
                    if (customer.NC_FlooringTile) flooringList.Add("Tile");
                    if (customer.NC_FlooringLinoleum) flooringList.Add("Linoleum");
                    if (customer.NC_FlooringSlate) flooringList.Add("Slate");
                    if (customer.NC_FlooringMarble) flooringList.Add("Marble");
                    Flooring.Text = string.Join(", ", flooringList.ToArray());

                    RequestEcoCleaners.Text = customer.NC_RequestEcoCleaners ? "Yes" : "No";
                    DoDishes.Text = customer.NC_DoDishes ? "Yes" : "No";
                    ChangeBed.Text = customer.NC_ChangeBed ? "Yes" : "No";
                    Blinds.Text = customer.DC_Blinds ? "Yes " + customer.DC_BlindsAmount + " (" + customer.DC_BlindsCondition + ")" : "No";
                    Windows.Text = customer.DC_Windows ? "Yes " + customer.DC_WindowsAmount + " (" + (customer.DC_WindowsSills ? "Clean" : "Dont Clean") + " Sills)" : "No";
                    Walls.Text = customer.DC_Walls ? "Yes (" + customer.DC_WallsDetail + ")" : "No";
                    Fans.Text = customer.DC_CeilingFans ? "Yes " + customer.DC_CeilingFansAmount : "No";
                    KitchenCupboards.Text = customer.DC_KitchenCuboards ? "Yes (" + customer.DC_KitchenCuboardsDetail + ")" : "No";
                    BathroomCupboards.Text = customer.DC_BathroomCuboards ? "Yes (" + customer.DC_BathroomCuboardsDetail + ")" : "No";
                    Baseboards.Text = customer.DC_Baseboards ? "Yes" : "No";
                    DoorFrames.Text = customer.DC_DoorFrames ? "Yes" : "No";
                    VentCovers.Text = customer.DC_VentCovers ? "Yes" : "No";
                    InsideVents.Text = customer.DC_InsideVents ? "Yes" : "No";
                    Pantry.Text = customer.DC_Pantry ? "Yes" : "No";
                    LaundryRoom.Text = customer.DC_LaundryRoom ? "Yes" : "No";
                    LightSwitches.Text = customer.DC_LightSwitches ? "Yes" : "No";
                    LightFixtures.Text = customer.DC_LightFixtures ? "Yes" : "No";
                    Refrigerator.Text = customer.DC_Refrigerator ? "Yes" : "No";
                    Oven.Text = customer.DC_Oven ? "Yes" : "No";
                    NC_Details.Text = customer.NC_Details;
                    OtherOne.Text = customer.DC_OtherOne;
                    OtherTwo.Text = customer.DC_OtherTwo;
                    CC_SquareFootage.Text = customer.CC_SquareFootage;
                    CC_RoomCountSmall.Text = customer.CC_RoomCountSmall;
                    CC_RoomCountLarge.Text = customer.CC_RoomCountLarge;
                    CC_PetOdorAdditive.Text = customer.CC_PetOdorAdditive ? "Yes" : "No";
                    CC_Details.Text = customer.CC_Details;
                    WW_BuildingStyle.Text = customer.WW_BuildingStyle;
                    WW_BuildingLevels.Text = customer.WW_BuildingLevels;
                    WW_VaultedCeilings.Text = customer.WW_VaultedCeilings ? "Yes" : "No";
                    WW_PostConstruction.Text = customer.WW_PostConstruction ? "Yes" : "No";
                    WW_WindowCount.Text = customer.WW_WindowCount;
                    WW_WindowType.Text = customer.WW_WindowType;
                    WW_InsidesOutsides.Text = customer.WW_InsidesOutsides;
                    WW_RazorCount.Text = customer.WW_Razor ? "Yes (" + customer.WW_RazorCount + ")" : "No";
                    WW_HardWaterCount.Text = customer.WW_HardWater ? "Yes (" + customer.WW_HardWaterCount + ")" : "No";
                    WW_FrenchWindows.Text = customer.WW_FrenchWindows ? "Yes (" + customer.WW_FrenchWindowCount + ")" : "No";
                    WW_StormWindows.Text = customer.WW_StormWindows ? "Yes (" + customer.WW_StormWindowCount + ")" : "No";
                    WW_Screens.Text = customer.WW_Screens ? "Yes (" + customer.WW_ScreensCount + ")" : "No";
                    WW_Tracks.Text = customer.WW_Tracks ? "Yes (" + customer.WW_TracksCount + ")" : "No";
                    WW_Wells.Text = customer.WW_Wells ? "Yes (" + customer.WW_WellsCount + ")" : "No";
                    WW_Gutters.Text = customer.WW_Gutters ? "Yes (" + customer.WW_GuttersFeet + ")" : "No";
                    WW_Details.Text = customer.WW_Details;
                    HW_Frequency.Text = customer.HW_Frequency;
                    HW_StartDate.Text = customer.HW_StartDate;
                    HW_EndDate.Text = customer.HW_EndDate;
                    HW_GarbageCans.Text = customer.HW_GarbageCans ? "Yes (" + customer.HW_GarbageDay + ")" : "No";
                    HW_PlantsWatered.Text = customer.HW_PlantsWatered ? "Yes (" + customer.HW_PlantsWateredFrequency + ")" : "No";
                    HW_Thermostat.Text = customer.HW_Thermostat ? "Yes (" + customer.HW_ThermostatTemperature + ")" : "No";
                    HW_Breakers.Text = customer.HW_Breakers ? "Yes (" + customer.HW_BreakersLocation + ")" : "No";
                    HW_CleanBeforeReturn.Text = customer.HW_CleanBeforeReturn ? "Yes" : "No";
                    HW_Details.Text = customer.HW_Details;

                    if ((customer.sectionMask & 1) == 0) HousekeepingSection.Visible = false;
                    if ((customer.sectionMask & 2) == 0) CarpetCleaningSection.Visible = false;
                    if ((customer.sectionMask & 4) == 0) WindowWashingSection.Visible = false;
                    if ((customer.sectionMask & 8) == 0) HomewatchSection.Visible = false;
                }
                else ErrorLabel.Text = error;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "LoadCustomerInfo EX: " + ex.Message;
            }
        }
    }
}