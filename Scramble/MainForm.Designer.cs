namespace Scramble
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.textBoxPlainText = new System.Windows.Forms.TextBox();
            this.textBoxEncryptedText = new System.Windows.Forms.TextBox();
            this.labelPlainText = new System.Windows.Forms.Label();
            this.labelEncryptedText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxPlainText
            // 
            this.textBoxPlainText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPlainText.Location = new System.Drawing.Point(12, 28);
            this.textBoxPlainText.Multiline = true;
            this.textBoxPlainText.Name = "textBoxPlainText";
            this.textBoxPlainText.Size = new System.Drawing.Size(760, 250);
            this.textBoxPlainText.TabIndex = 0;
            this.textBoxPlainText.TextChanged += new System.EventHandler(this.textBoxPlainText_TextChanged);
            this.textBoxPlainText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPlainText_KeyPress);
            // 
            // textBoxEncryptedText
            // 
            this.textBoxEncryptedText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEncryptedText.Location = new System.Drawing.Point(12, 300);
            this.textBoxEncryptedText.Multiline = true;
            this.textBoxEncryptedText.Name = "textBoxEncryptedText";
            this.textBoxEncryptedText.Size = new System.Drawing.Size(760, 250);
            this.textBoxEncryptedText.TabIndex = 1;
            this.textBoxEncryptedText.TextChanged += new System.EventHandler(this.textBoxEncryptedText_TextChanged);
            this.textBoxEncryptedText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxEncryptedText_KeyPress);
            // 
            // labelPlainText
            // 
            this.labelPlainText.AutoSize = true;
            this.labelPlainText.Location = new System.Drawing.Point(12, 9);
            this.labelPlainText.Name = "labelPlainText";
            this.labelPlainText.Size = new System.Drawing.Size(67, 16);
            this.labelPlainText.TabIndex = 2;
            this.labelPlainText.Text = "Plain Text";
            // 
            // labelEncryptedText
            // 
            this.labelEncryptedText.AutoSize = true;
            this.labelEncryptedText.Location = new System.Drawing.Point(12, 281);
            this.labelEncryptedText.Name = "labelEncryptedText";
            this.labelEncryptedText.Size = new System.Drawing.Size(98, 16);
            this.labelEncryptedText.TabIndex = 3;
            this.labelEncryptedText.Text = "Encrypted Text";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.labelEncryptedText);
            this.Controls.Add(this.labelPlainText);
            this.Controls.Add(this.textBoxEncryptedText);
            this.Controls.Add(this.textBoxPlainText);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.Text = "Scramble";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxPlainText;
        private System.Windows.Forms.TextBox textBoxEncryptedText;
        private System.Windows.Forms.Label labelPlainText;
        private System.Windows.Forms.Label labelEncryptedText;
    }
}

