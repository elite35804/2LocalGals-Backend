<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomeGuardContract.aspx.cs" Inherits="TwoLocalGals.HomeGuardContract" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>2LG Home Guard Contract</title>
    <link rel="shortcut icon" href="/favicon.ico" />
    <link href="/Styles/HomeGuardContract.css" rel="stylesheet" type="text/css" />
    <meta name="viewport" content="width=600" />
</head>
<body>
    <script language="javascript" type="text/javascript">
        function JsAgreeCheckboxChanged() {
            var checked = document.getElementById('<%=AgreeCheckbox.ClientID %>').checked;
            document.getElementById('SubmitButton').disabled = !checked;
            document.getElementById('SubmitButton').src = checked ? "2LG_SubmitBlue.png" : "2LG_SubmitGray.png";
        }

        function PrintPanel() {
            var panel = document.getElementById("<%=ContractPanel.ClientID %>");
            var printWindow = window.open('', '', 'height=400,width=800');
            printWindow.document.write('<html><head><title>DIV Contents</title>');
            printWindow.document.write('</head><body >');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
            return false;
        }

    </script>
    <form runat="server">
    <div class="Page">
        <img class="Logo" src="https://www.2localgals.com/images/img03.jpg" alt="None" />
        <div class="ErrorDiv" style="margin-top: 20px;">
            <asp:Label ID="ErrorLabel" runat="server" />
        </div>
        <div class="ContentHolder">
            <div class="Title">Home Guard Agreement</div>
            <h2>Customer Information:</h2>
            <p>
                <b>Name: </b><asp:Label ID="CustomerName" runat="server"></asp:Label><br />
                <b>Address: </b><asp:Label ID="CustomerAddress" runat="server"></asp:Label><br />
                <b>Telephone (Primary): </b><asp:Label ID="CustomerPhoneOne" runat="server"></asp:Label><br />
                <b>Telephone (Secondary): </b><asp:Label ID="CustomerPhoneTwo" runat="server"></asp:Label><br />
                <b>Email: </b><asp:Label ID="CustomerEmail" runat="server"></asp:Label>
            </p>
            <h2>Services Include:</h2>
            <p>
                <b>Exterior Checklist: </b> Check that all entrances are secure; visual check for evidence of forced entry, vandalism, theft or damage; check outside faucets and hoses for leaks; removal of newspapers, flyers, packages, mail and other evidence of non- occupancy; visual inspection of roof & gutters from the ground and visual inspection of yard/landscaping and winter checks for snow loads.
            </p>
            <p>
                <b>Interior Checklist: </b> Make periodic reasonable inspection:  (a) for signs of theft, vandalism, damage or other disturbance;  (b) that windows and entryways are secure;  (c) to determine if security system is set and working properly; (d) to note any unusual odors; (e)  of walls, ceilings, windows, tubs, showers for evidence of water damage, leakage;  (f)  check that thermostat is set at an appropriate temperature; (g)  check that freezers & refrigerators are working; (h) visual check of heating system and hot water heater.
            </p>
            <p>
                <b>Other Details: </b> <span id="OtherDetails" runat="server"></span>
            </p>
            <div style="float: right;"> <asp:Button ID="btnPrint" Text="Print Service Agreement" OnClientClick="return PrintPanel();" runat="server" /></div>
            <h2>
                Service Agreement:
            </h2>
            <div class="ContractHolder">
                <asp:Panel ID="ContractPanel" CssClass="ContractPanel" runat="server" Height="400px" ScrollBars="Vertical">
                    <div class="ContractHeader">2 LOCAL GALS</div>
                    <div class="ContractHeader">HOME GUARD SERVICE AGREEMENT</div>
                    <h3>Agreement</h3>
                    <p>This agreement is entered into by and between <%= this.fullCompanyName %> and <span id="ContractCustomerTitle" runat="server" style="text-decoration:underline;">NAME</span> (“CLIENT”) to, among other things, define the respective duties and obligations of <%= this.abbreviatedCompanyName %> and of the Client and services <%= this.abbreviatedCompanyName %> is to perform for Client hereunder as described in Schedule “A” to this Agreement. </p>
                    <h3>1. Inspection Reports</h3>
                    <p><%= this.abbreviatedCompanyName %> reports on the status of the Property to the owner while they are away. Each Property Patrol is documented by an electronic Property Patrol Report. This report is generated during the Patrol and is provided to the owner immediately after each patrol is completed to supply the owner with the status of their property at the time of the visit.</p>
                    <p><%= this.abbreviatedCompanyName %> Patrol Reporter arrives at the home to perform a complete Patrol of the property. There is a QR code brought by the Reporter. The Reporter uses a smartphone or tablet and scans the QR code. The reporter logs in to access the owner's site specific checklist. When the reporter logs in this automatically records the date, time & GPS position as proof that <%= this.abbreviatedCompanyName %> has been to the Client's property.</p>
                    <p>The reporter completes the property patrol while filling in the list of questions created by the owner & the <%= this.abbreviatedCompanyName %> representative custom to each property that may or may not include photos and comments. When the patrol has been completed and the reporter hits submit this then sends an e-mail to the Admin at <%= this.abbreviatedCompanyName %>. This lets them know that the reporter has completed the patrol. The Admin will then forward the report to the clients secure dashboard. The property owner will receive an automated email notifying them that they can now log into their dashboard to review their reports.</p>
                    <p>It is very important that you review the reports each time they are submitted as they identify any problems encountered or unfortunate issues that may have happened. This will give you the opportunity to work with <%= this.abbreviatedCompanyName %> to act on your behalf to minimize any further damages.</p>
                    <h3>2. Approvals and Authorization</h3>
                    <p>Client hereby contracts and agrees with <%= this.abbreviatedCompanyName %> to provide the services stated in this Agreement, and for other rendered services furnished to Client.  Client agrees to promptly pay for such services, including but not limited to those specified in Schedule “B” attached hereto.  Client hereby represents and warrants that:  (a) it is the legal and beneficial owner of the property that is the subject of this Agreement, or (b) it has full and complete authorization to enter into this Agreement on behalf of the property owner of said property and (c) it has further full authority to permit home and property access including all gated community access points.  Client agrees to inform all home and community security entities of this Agreement to have <%= this.abbreviatedCompanyName %> representatives enter the community and access the property.  Client assumes the affirmative duty to furnish true, accurate and complete information concerning said property and existing conditions as are necessary or convenient to <%= this.abbreviatedCompanyName %>’s performance of its duties hereunder.  Client waives any potential claim against <%= this.abbreviatedCompanyName %> for damages which result in whole or in part from its failure to fully disclose and/or to warn <%= this.abbreviatedCompanyName %> about actual or potential hazards or conditions.</p>
                    <h3>3. Indemnity</h3>
                    <p>The Client hereby agrees to indemnify and hold <%= this.abbreviatedCompanyName %>, its members, managers, officers, shareholders, directors, agents, attorneys, representatives and employees (“<b>INDEMNIFIED PARTIES</b>”) harmless from all liability for injuries to persons or property suffered or sustained by any person whomsoever, including but not limited to all liability, judgments, lawsuits, claims, damages, losses, expenses or insurance subrogation claims which may be asserted against or incurred by an Indemnified Party which is in any manner related to or results from providing the services described herein.</p>
                    <h3>4. Damages or Missing Items</h3>
                    <p>Client hereby waives any potential claims against <%= this.abbreviatedCompanyName %> for damage to its property, for items missing, switched out, lost, damaged or stolen under any circumstances including, including without in any way limiting the generality of the foregoing, damage or loss due to theft, vandalism, negligence of invited or uninvited individuals, acts of nature, etc., excepting only losses which are solely caused by <%= this.abbreviatedCompanyName %>’s gross negligence or willful misconduct.</p>
                    <h3>5. No Guarantees</h3>
                    <p>Client expressly acknowledges and agrees that the services that are the subject of this Agreement are limited to observation and reporting only.  <%= this.abbreviatedCompanyName %> is not responsible for and it does not guarantee against damage to or the condition of the property, including but not limited to damage from water, break‐in, vandalism, negligence, willful misconduct, acts of nature, etc.  The purpose of this Agreement is solely for scheduled observation and inspection.  If a false alarm occurs <%= this.abbreviatedCompanyName %> shall not be responsible for any resulting expenses or costs.</p>
                    <h3>6. Termination</h3>
                    <p>Either party may terminate this Agreement with 48 (forty eight) hours advance written notice.  Client shall pay all fees and other amounts owed to <%= this.abbreviatedCompanyName %> at the time of termination. On termination <%= this.abbreviatedCompanyName %> agrees to return all keys, codes, and any other means of property access to Client.</p>
                    <h3>7. Authorization and Approvals</h3>
                    <p>Client hereby expressly acknowledges, represents and warrants to <%= this.abbreviatedCompanyName %> that:</p>
                    <ol>
                        <li>The information provided by Client and set out in Schedule “A” attached hereto is accurate and complete.  Inaccurate or incomplete information therein is the responsibility of the Client and <%= this.abbreviatedCompanyName %> is hereby released from all damages, adequate responsibility and/or liability which in any manner results therefrom.  Any additional duties to be performed by <%= this.abbreviatedCompanyName %> shall be specified in a written agreement (addendum) signed by customer and <%= this.abbreviatedCompanyName %>.</li>
                        <li>By this Agreement Customer hereby grants access by <%= this.abbreviatedCompanyName %> to the Client’s property for property monitoring services, reporting and other services herein described.</li>
                        <li>In the event <%= this.abbreviatedCompanyName %> furnishes services without receiving a signed addendum or if it determines that additional services need to be performed for the care or protection of Customer’s property or rights, then <%= this.abbreviatedCompanyName %> may at its option provide said services which will be billed to and paid by Client on the same terms and conditions as herein provided for payment of charges and fees.</li>
                        <li>Client agrees to pay all costs, fees, time and expenses related to an emergency or urgency situation, which will be billed to and paid by Client on the same terms and conditions as herein provided for payment for charges and fees.</li>
                    </ol>
                    <h3>8. Service Agreement</h3>
                    <p>This Agreement must be signed by the Client before <%= this.abbreviatedCompanyName %> is obligated to begin to provide services. <%= this.abbreviatedCompanyName %>’s duties are limited to those specified herein or described in the exhibits attached hereto or added by addendum as herein provided.</p>
                    <h3>9. Billing, Late Fees & Attorney Fees</h3>
                    <p>Client will be charged for the services rendered by <%= this.abbreviatedCompanyName %> in advance of furnishing services hereunder with additional billing(s) should <%= this.abbreviatedCompanyName %> perform additional services. Late payments shall be assessed a late fee of ten percent (10%) of the late payment but not less than $25.  Dishonored check will be assessed a $25 fee or the fee charged by the bank, whichever is higher.  In the event Client fails to perform its obligations hereunder or to pay as agreed Client agrees to pay to <%= this.abbreviatedCompanyName %> its collection fees and expenses together with costs and reasonable attorney fees whether or not a lawsuit is filed.</p>
                    <div class="ContractHeader">SCHEDULE “A”</div>
                    <h3 style="text-decoration:underline;">CLIENT CONTACT INFORMATION</h3>
                    <p>
                        Name: <asp:Label ID="ContractCustomerName" runat="server"></asp:Label><br />
                        Address: <asp:Label ID="ContractCustomerAddress" runat="server"></asp:Label><br />
                        Telephone (Primary): <asp:Label ID="ContractPhoneOne" runat="server"></asp:Label><br />
                        Telephone (Secondary): <asp:Label ID="ContractPhoneTwo" runat="server"></asp:Label><br />
                        Email: <asp:Label ID="ContractEmail" runat="server"></asp:Label>
                    </p>
                    <h3 style="text-decoration:underline;">SERVICES</h3>
                    <h3>SERVICES INCLUDE:</h3>
                    <p><b>Exterior Checklist: </b> Check that all entrances are secure; visual check for evidence of forced entry, vandalism, theft or damage; check outside faucets and hoses for leaks; removal of newspapers, flyers, packages, mail and other evidence of non- occupancy; visual inspection of roof & gutters from the ground and visual inspection of yard/landscaping and winter checks for snow loads.</p>
                    <p><b>Interior Checklist: </b> Make periodic reasonable inspection:  (a) for signs of theft, vandalism, damage or other disturbance;  (b) that windows and entryways are secure;  (c) to determine if security system is set and working properly; (d) to note any unusual odors; (e)  of walls, ceilings, windows, tubs, showers for evidence of water damage, leakage;  (f)  check that thermostat is set at an appropriate temperature; (g)  check that freezers & refrigerators are working; (h) visual check of heating system and hot water heater.</p>
                </asp:Panel>
                
                <p>
                    <label><asp:CheckBox ID="AgreeCheckbox" OnClick="JsAgreeCheckboxChanged()" runat="server" />Check here to indicate that you have read and agree to the terms of the <span style="color:Blue;">2 LOCAL GALS HOME GUARD SERVICE AGREEMENT</span>. Client warrants and represents:  (a) that he has read this Agreement, (b) that he has sought independent counsel or obtained advice from person(s) of his choosing before signing this document, or (c) he has waived his right to do so and (d) that he has asked questions of <%= this.abbreviatedCompanyName %> concerning any provisions herein which is not fully understood and (e) that by signing this agreement you agree to and accept all of the terms and conditions herein.</label>
                </p>
            </div>
            <div class="SubmitDiv">
                <asp:ImageButton ID="SubmitButton" Enabled="false" ImageUrl="~/2LG_SubmitGray.png" OnClick="SubmitButton_Click" runat="server" />
                <asp:Label ID="AgreementSignedLabel" CssClass="AgreementSignedLabel" runat="server" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
