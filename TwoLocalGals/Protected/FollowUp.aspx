<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="FollowUp.aspx.cs" Inherits="TwoLocalGals.Protected.FollowUp" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script src="/star-rating/jquery.js" type="text/javascript"></script>
    <script src="/star-rating/jquery.form.js" type="text/javascript"></script>
    <script src="/star-rating/jquery.MetaData.js" type="text/javascript"></script>
    <script src="/star-rating/jquery.rating.js" type="text/javascript"></script>
    <link href="/star-rating/jquery.rating.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <!-- Menu Bar -->
    <div class="TopMenuButtons">
        <asp:Button ID="SubmitButton" OnClick="SubmitClick" Text="Submit" runat="server" />
        <asp:Button ID="CancelButton" OnClick="CancelClick" Text="Cancel" runat="server" />
    </div>
    <!-- Error Label -->
    <div class="errorDiv">
        <asp:Label ID="ErrorLabel" runat="server" CssClass="errorLabel" />
    </div>
    <!-- Page Title -->
    <div class="PageTitle">
        <asp:Label ID="CustomerTitleLabel" runat="server" />
    </div>
    <fieldset class="Entry" style="width: 900px;">
        <legend id="FollowUpLegend" runat="server">Follow Up</legend>
        <table id="HousekeepingSection" class="BidSheet" style="display:none;" runat="server">
            <tr>
                <td><h3>Housekeeping</h3></td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Scheduling Satisfaction
                </td>
                <td>
                    <input id="HK_SchedulingSatisfaction1" name="HK_star1" type="radio" class="star" runat="server" />
                    <input id="HK_SchedulingSatisfaction2" name="HK_star1" type="radio" class="star" runat="server" />
                    <input id="HK_SchedulingSatisfaction3" name="HK_star1" type="radio" class="star" runat="server" />
                    <input id="HK_SchedulingSatisfaction4" name="HK_star1" type="radio" class="star" runat="server" />
                    <input id="HK_SchedulingSatisfaction5" name="HK_star1" type="radio" class="star" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Contractor Time Management
                </td>
                <td>
                    <input id="HK_ContractorTimeManagement1" name="HK_star2" type="radio" class="star" runat="server" />
                    <input id="HK_ContractorTimeManagement2" name="HK_star2" type="radio" class="star" runat="server" />
                    <input id="HK_ContractorTimeManagement3" name="HK_star2" type="radio" class="star" runat="server" />
                    <input id="HK_ContractorTimeManagement4" name="HK_star2" type="radio" class="star" runat="server" />
                    <input id="HK_ContractorTimeManagement5" name="HK_star2" type="radio" class="star" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Contractor Professionalism
                </td>
                <td>
                    <input id="HK_ContractorProfessionalism1" name="HK_star3" type="radio" class="star" runat="server" />
                    <input id="HK_ContractorProfessionalism2" name="HK_star3" type="radio" class="star" runat="server" />
                    <input id="HK_ContractorProfessionalism3" name="HK_star3" type="radio" class="star" runat="server" />
                    <input id="HK_ContractorProfessionalism4" name="HK_star3" type="radio" class="star" runat="server" />
                    <input id="HK_ContractorProfessionalism5" name="HK_star3" type="radio" class="star" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Contractor Quality of Work
                </td>
                <td>
                    <input id="HK_ContractorQuality1" name="HK_star4" type="radio" class="star" runat="server" />
                    <input id="HK_ContractorQuality2" name="HK_star4" type="radio" class="star" runat="server" />
                    <input id="HK_ContractorQuality3" name="HK_star4" type="radio" class="star" runat="server" />
                    <input id="HK_ContractorQuality4" name="HK_star4" type="radio" class="star" runat="server" />
                    <input id="HK_ContractorQuality5" name="HK_star4" type="radio" class="star" runat="server" />
                </td>
            </tr>
             <tr>
                <td class="BidSheetHeader" colspan="2" style="padding-top:10px;">
                    Is there anything we could have done better for you?
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader" colspan="2">
                    <asp:TextBox ID="HK_Better" runat="server" Rows="4" TextMode="multiline" Width="480px" />
                </td>
            </tr>
        </table>
		<table id="CarpetCleaningSection" class="BidSheet" style="display:none;" runat="server">
            <tr>
                <td><h3>Carpet Cleaning</h3></td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Scheduling Satisfaction
                </td>
                <td>
                    <input id="CC_SchedulingSatisfaction1" name="CC_star1" type="radio" class="star" runat="server" />
                    <input id="CC_SchedulingSatisfaction2" name="CC_star1" type="radio" class="star" runat="server" />
                    <input id="CC_SchedulingSatisfaction3" name="CC_star1" type="radio" class="star" runat="server" />
                    <input id="CC_SchedulingSatisfaction4" name="CC_star1" type="radio" class="star" runat="server" />
                    <input id="CC_SchedulingSatisfaction5" name="CC_star1" type="radio" class="star" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Contractor Time Management
                </td>
                <td>
                    <input id="CC_ContractorTimeManagement1" name="CC_star2" type="radio" class="star" runat="server" />
                    <input id="CC_ContractorTimeManagement2" name="CC_star2" type="radio" class="star" runat="server" />
                    <input id="CC_ContractorTimeManagement3" name="CC_star2" type="radio" class="star" runat="server" />
                    <input id="CC_ContractorTimeManagement4" name="CC_star2" type="radio" class="star" runat="server" />
                    <input id="CC_ContractorTimeManagement5" name="CC_star2" type="radio" class="star" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Contractor Professionalism
                </td>
                <td>
                    <input id="CC_ContractorProfessionalism1" name="CC_star3" type="radio" class="star" runat="server" />
                    <input id="CC_ContractorProfessionalism2" name="CC_star3" type="radio" class="star" runat="server" />
                    <input id="CC_ContractorProfessionalism3" name="CC_star3" type="radio" class="star" runat="server" />
                    <input id="CC_ContractorProfessionalism4" name="CC_star3" type="radio" class="star" runat="server" />
                    <input id="CC_ContractorProfessionalism5" name="CC_star3" type="radio" class="star" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Contractor Quality of Work
                </td>
                <td>
                    <input id="CC_ContractorQuality1" name="CC_star4" type="radio" class="star" runat="server" />
                    <input id="CC_ContractorQuality2" name="CC_star4" type="radio" class="star" runat="server" />
                    <input id="CC_ContractorQuality3" name="CC_star4" type="radio" class="star" runat="server" />
                    <input id="CC_ContractorQuality4" name="CC_star4" type="radio" class="star" runat="server" />
                    <input id="CC_ContractorQuality5" name="CC_star4" type="radio" class="star" runat="server" />
                </td>
            </tr>
             <tr>
                <td class="BidSheetHeader" colspan="2" style="padding-top:10px;">
                    Is there anything we could have done better for you?
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader" colspan="2">
                    <asp:TextBox ID="CC_Better" runat="server" Rows="4" TextMode="multiline" Width="480px" />
                </td>
            </tr>
        </table>
		<table id="WindowWashingSection" class="BidSheet" style="display:none;" runat="server">
            <tr>
                <td><h3>Window Washing</h3></td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Scheduling Satisfaction
                </td>
                <td>
                    <input id="WW_SchedulingSatisfaction1" name="WW_star1" type="radio" class="star" runat="server" />
                    <input id="WW_SchedulingSatisfaction2" name="WW_star1" type="radio" class="star" runat="server" />
                    <input id="WW_SchedulingSatisfaction3" name="WW_star1" type="radio" class="star" runat="server" />
                    <input id="WW_SchedulingSatisfaction4" name="WW_star1" type="radio" class="star" runat="server" />
                    <input id="WW_SchedulingSatisfaction5" name="WW_star1" type="radio" class="star" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Contractor Time Management
                </td>
                <td>
                    <input id="WW_ContractorTimeManagement1" name="WW_star2" type="radio" class="star" runat="server" />
                    <input id="WW_ContractorTimeManagement2" name="WW_star2" type="radio" class="star" runat="server" />
                    <input id="WW_ContractorTimeManagement3" name="WW_star2" type="radio" class="star" runat="server" />
                    <input id="WW_ContractorTimeManagement4" name="WW_star2" type="radio" class="star" runat="server" />
                    <input id="WW_ContractorTimeManagement5" name="WW_star2" type="radio" class="star" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Contractor Professionalism
                </td>
                <td>
                    <input id="WW_ContractorProfessionalism1" name="WW_star3" type="radio" class="star" runat="server" />
                    <input id="WW_ContractorProfessionalism2" name="WW_star3" type="radio" class="star" runat="server" />
                    <input id="WW_ContractorProfessionalism3" name="WW_star3" type="radio" class="star" runat="server" />
                    <input id="WW_ContractorProfessionalism4" name="WW_star3" type="radio" class="star" runat="server" />
                    <input id="WW_ContractorProfessionalism5" name="WW_star3" type="radio" class="star" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Contractor Quality of Work
                </td>
                <td>
                    <input id="WW_ContractorQuality1" name="WW_star4" type="radio" class="star" runat="server" />
                    <input id="WW_ContractorQuality2" name="WW_star4" type="radio" class="star" runat="server" />
                    <input id="WW_ContractorQuality3" name="WW_star4" type="radio" class="star" runat="server" />
                    <input id="WW_ContractorQuality4" name="WW_star4" type="radio" class="star" runat="server" />
                    <input id="WW_ContractorQuality5" name="WW_star4" type="radio" class="star" runat="server" />
                </td>
            </tr>
             <tr>
                <td class="BidSheetHeader" colspan="2" style="padding-top:10px;">
                    Is there anything we could have done better for you?
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader" colspan="2">
                    <asp:TextBox ID="WW_Better" runat="server" Rows="4" TextMode="multiline" Width="480px" />
                </td>
            </tr>
        </table>
		<table id="HomewatchSection" class="BidSheet" style="display:none;" runat="server">
            <tr>
                <td><h3>Home Guard</h3></td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Scheduling Satisfaction
                </td>
                <td>
                    <input id="HW_SchedulingSatisfaction1" name="HW_star1" type="radio" class="star" runat="server" />
                    <input id="HW_SchedulingSatisfaction2" name="HW_star1" type="radio" class="star" runat="server" />
                    <input id="HW_SchedulingSatisfaction3" name="HW_star1" type="radio" class="star" runat="server" />
                    <input id="HW_SchedulingSatisfaction4" name="HW_star1" type="radio" class="star" runat="server" />
                    <input id="HW_SchedulingSatisfaction5" name="HW_star1" type="radio" class="star" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Contractor Time Management
                </td>
                <td>
                    <input id="HW_ContractorTimeManagement1" name="HW_star2" type="radio" class="star" runat="server" />
                    <input id="HW_ContractorTimeManagement2" name="HW_star2" type="radio" class="star" runat="server" />
                    <input id="HW_ContractorTimeManagement3" name="HW_star2" type="radio" class="star" runat="server" />
                    <input id="HW_ContractorTimeManagement4" name="HW_star2" type="radio" class="star" runat="server" />
                    <input id="HW_ContractorTimeManagement5" name="HW_star2" type="radio" class="star" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Contractor Professionalism
                </td>
                <td>
                    <input id="HW_ContractorProfessionalism1" name="HW_star3" type="radio" class="star" runat="server" />
                    <input id="HW_ContractorProfessionalism2" name="HW_star3" type="radio" class="star" runat="server" />
                    <input id="HW_ContractorProfessionalism3" name="HW_star3" type="radio" class="star" runat="server" />
                    <input id="HW_ContractorProfessionalism4" name="HW_star3" type="radio" class="star" runat="server" />
                    <input id="HW_ContractorProfessionalism5" name="HW_star3" type="radio" class="star" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader">
                    Contractor Quality of Work
                </td>
                <td>
                    <input id="HW_ContractorQuality1" name="HW_star4" type="radio" class="star" runat="server" />
                    <input id="HW_ContractorQuality2" name="HW_star4" type="radio" class="star" runat="server" />
                    <input id="HW_ContractorQuality3" name="HW_star4" type="radio" class="star" runat="server" />
                    <input id="HW_ContractorQuality4" name="HW_star4" type="radio" class="star" runat="server" />
                    <input id="HW_ContractorQuality5" name="HW_star4" type="radio" class="star" runat="server" />
                </td>
            </tr>
             <tr>
                <td class="BidSheetHeader" colspan="2" style="padding-top:10px;">
                    Is there anything we could have done better for you?
                </td>
            </tr>
            <tr>
                <td class="BidSheetHeader" colspan="2">
                    <asp:TextBox ID="HW_Better" runat="server" Rows="4" TextMode="multiline" Width="480px" />
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
