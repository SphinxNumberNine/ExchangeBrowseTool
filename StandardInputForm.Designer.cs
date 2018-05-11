namespace ExchangeOnePassIdxGUI
{
    partial class StandardInputForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        public System.ComponentModel.IContainer components = null;

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
        public void InitializeComponent()
        {
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.Description = new System.Windows.Forms.TextBox();
            this.searchBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // searchBox
            // 
            this.searchBox.Location = new System.Drawing.Point(12, 72);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(287, 20);
            this.searchBox.TabIndex = 0;
            this.searchBox.TextChanged += new System.EventHandler(this.searchBox_TextChanged);
            this.searchBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.StandardInputForm_KeyPress);
            // 
            // OKButton
            // 
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKButton.Location = new System.Drawing.Point(224, 12);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 1;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            // 
            // CancelButton
            // 
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(224, 41);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 2;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // Description
            // 
            this.Description.Location = new System.Drawing.Point(13, 12);
            this.Description.Multiline = true;
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            this.Description.Size = new System.Drawing.Size(205, 52);
            this.Description.TabIndex = 3;
            this.Description.TextChanged += new System.EventHandler(this.Description_TextChanged);
            // 
            // searchBox
            // 
            this.searchBox.FormattingEnabled = true;
            this.searchBox.Location = new System.Drawing.Point(13, 71);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(286, 21);
            this.searchBox.TabIndex = 4;
            // 
            // StandardInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(311, 104);
            this.Controls.Add(this.searchBox);
            this.Controls.Add(this.Description);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OKButton);
            this.Name = "StandardInputForm";
            this.Text = "StandardInputForm";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.StandardInputForm_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button OKButton;
        public System.Windows.Forms.Button CancelButton;
        public System.Windows.Forms.TextBox Description;
        public System.Windows.Forms.ComboBox searchBox;
    }
}