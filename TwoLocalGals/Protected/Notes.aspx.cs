using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nexus;

namespace TwoLocalGals.Protected
{
    public partial class Notes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Globals.ForceSSL(this);

                if (Globals.GetUserAccess(this) < 5)
                    Globals.LogoutUser(this);

                this.Page.Title = "2LG Notes";

                int userMask = Globals.GetFranchiseMask();
                foreach (FranchiseStruct franchise in Database.GetFranchiseList())
                {
                    if (franchise.notesEnabled && (franchise.franchiseMask & userMask) != 0)
                    {
                        Table notesTable = new Table();
                        notesTable.ID = "FRAN" + franchise.franchiseID.ToString();
                        notesTable.CssClass = "NotesTable";
                        notesTable.Caption = "Notes";

                        TableRow generalRow = new TableRow();
                        generalRow.Cells.Add(Globals.FormatedTableHeaderCell("General Notes", 0));
                        notesTable.Rows.Add(generalRow);

                        TableRow generalNotesRow = new TableRow();
                        TableCell generalNotesCell = new TableCell();
                        TextBox generalNotesTextBox = new TextBox();
                        generalNotesTextBox.ID = "G" + franchise.franchiseID.ToString();
                        generalNotesTextBox.TextMode = TextBoxMode.MultiLine;
                        generalNotesTextBox.Rows = 10;
                        generalNotesTextBox.Width = 875;
                        generalNotesTextBox.Attributes.Add("onchange", "JsFormValueChanged(this)");
                        generalNotesTextBox.Text = franchise.notesGeneral;
                        generalNotesCell.Controls.Add(generalNotesTextBox);
                        generalNotesRow.Cells.Add(generalNotesCell);
                        notesTable.Rows.Add(generalNotesRow);

                        TableRow accountingRow = new TableRow();
                        accountingRow.Cells.Add(Globals.FormatedTableHeaderCell("Accounting Notes", 0));
                        notesTable.Rows.Add(accountingRow);

                        TableRow accountingNotesRow = new TableRow();
                        TableCell accountingNotesCell = new TableCell();
                        TextBox accountingNotesTextBox = new TextBox();
                        accountingNotesTextBox.ID = "A" + franchise.franchiseID.ToString();
                        accountingNotesTextBox.TextMode = TextBoxMode.MultiLine;
                        accountingNotesTextBox.Rows = 10;
                        accountingNotesTextBox.Width = 875;
                        accountingNotesTextBox.Attributes.Add("onchange", "JsFormValueChanged(this)");
                        accountingNotesTextBox.Text = franchise.notesAccounting;
                        accountingNotesCell.Controls.Add(accountingNotesTextBox);
                        accountingNotesRow.Cells.Add(accountingNotesCell);
                        notesTable.Rows.Add(accountingNotesRow);

                        NotesPanel.Controls.Add(notesTable);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Page_Load EX: " + ex.Message;
            }
        }

        public void CancelClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Notes.aspx");
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
                    Response.Redirect("Notes.aspx");
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
                foreach (Table table in NotesPanel.Controls)
                {
                    if (table.ID.StartsWith("FRAN"))
                    {
                        FranchiseStruct franchise = new FranchiseStruct();

                        franchise.franchiseID = Globals.SafeIntParse(table.ID.Substring(4));

                        TextBox generalNotes = (TextBox)table.FindControl("G" + franchise.franchiseID);
                        franchise.notesGeneral = generalNotes.Text;

                        TextBox accountingNotes = (TextBox)table.FindControl("A" + franchise.franchiseID);
                        franchise.notesAccounting = accountingNotes.Text;

                        string error = Database.UpdateFranchiseNotes(franchise);
                        if (error != null)
                        {
                            ErrorLabel.Text = error;
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "SaveChanges EX: " + ex.Message;
                return false;
            }
        }
    }
}