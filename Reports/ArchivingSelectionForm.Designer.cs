namespace ExchangeOnePassIdxGUI
{
    partial class ArchivingSelectionForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.journalmbxButton = new System.Windows.Forms.Button();
            this.usermbxButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(61, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select a core:";
            // 
            // journalmbxButton
            // 
            this.journalmbxButton.Location = new System.Drawing.Point(12, 35);
            this.journalmbxButton.Name = "journalmbxButton";
            this.journalmbxButton.Size = new System.Drawing.Size(75, 23);
            this.journalmbxButton.TabIndex = 1;
            this.journalmbxButton.Text = "journalmbx";
            this.journalmbxButton.UseVisualStyleBackColor = true;
            this.journalmbxButton.Click += new System.EventHandler(this.journalmbxButton_Click);
            // 
            // usermbxButton
            // 
            this.usermbxButton.Location = new System.Drawing.Point(103, 35);
            this.usermbxButton.Name = "usermbxButton";
            this.usermbxButton.Size = new System.Drawing.Size(75, 23);
            this.usermbxButton.TabIndex = 2;
            this.usermbxButton.Text = "usermbx";
            this.usermbxButton.UseVisualStyleBackColor = true;
            this.usermbxButton.Click += new System.EventHandler(this.usermbxButton_Click);
            // 
            // ArchivingSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(190, 70);
            this.Controls.Add(this.usermbxButton);
            this.Controls.Add(this.journalmbxButton);
            this.Controls.Add(this.label1);
            this.Name = "ArchivingSelectionForm";
            this.Text = "ArchivingSelectionForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button journalmbxButton;
        private System.Windows.Forms.Button usermbxButton;
    }
}