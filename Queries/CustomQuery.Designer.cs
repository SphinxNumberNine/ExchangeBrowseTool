namespace ExchangeOnePassIdxGUI {
    partial class CustomQuery {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomQuery));
            this.titleLabel = new System.Windows.Forms.Label();
            this.coreNameComboBox = new System.Windows.Forms.ComboBox();
            this.coreNameLabel = new System.Windows.Forms.Label();
            this.requestTypeLabel = new System.Windows.Forms.Label();
            this.requestTypeComboBox = new System.Windows.Forms.ComboBox();
            this.queryText = new System.Windows.Forms.TextBox();
            this.queryLabel = new System.Windows.Forms.Label();
            this.submitButton = new System.Windows.Forms.Button();
            this.newKeyLabel = new System.Windows.Forms.Label();
            this.newKeyText = new System.Windows.Forms.TextBox();
            this.newValueText = new System.Windows.Forms.TextBox();
            this.newValueLabel = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.fullQueryText = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.helpButton = new System.Windows.Forms.Button();
            this.facetFiedlLabel = new System.Windows.Forms.Label();
            this.facetValueText = new System.Windows.Forms.TextBox();
            this.customQueryList = new System.Windows.Forms.ListBox();
            this.Add = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(12, 9);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(126, 22);
            this.titleLabel.TabIndex = 3;
            this.titleLabel.Text = "Custom Query";
            // 
            // coreNameComboBox
            // 
            this.coreNameComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.coreNameComboBox.FormattingEnabled = true;
            this.coreNameComboBox.Location = new System.Drawing.Point(0, 19);
            this.coreNameComboBox.Name = "coreNameComboBox";
            this.coreNameComboBox.Size = new System.Drawing.Size(369, 21);
            this.coreNameComboBox.TabIndex = 4;
            this.coreNameComboBox.SelectedIndexChanged += new System.EventHandler(this.coreNameComboBox_SelectedIndexChanged);
            // 
            // coreNameLabel
            // 
            this.coreNameLabel.AutoSize = true;
            this.coreNameLabel.Location = new System.Drawing.Point(0, 0);
            this.coreNameLabel.Name = "coreNameLabel";
            this.coreNameLabel.Size = new System.Drawing.Size(64, 13);
            this.coreNameLabel.TabIndex = 5;
            this.coreNameLabel.Text = "Core name: ";
            // 
            // requestTypeLabel
            // 
            this.requestTypeLabel.AutoSize = true;
            this.requestTypeLabel.Location = new System.Drawing.Point(3, 0);
            this.requestTypeLabel.Name = "requestTypeLabel";
            this.requestTypeLabel.Size = new System.Drawing.Size(73, 13);
            this.requestTypeLabel.TabIndex = 8;
            this.requestTypeLabel.Text = "Request type:";
            // 
            // requestTypeComboBox
            // 
            this.requestTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.requestTypeComboBox.FormattingEnabled = true;
            this.requestTypeComboBox.Location = new System.Drawing.Point(3, 19);
            this.requestTypeComboBox.Name = "requestTypeComboBox";
            this.requestTypeComboBox.Size = new System.Drawing.Size(237, 21);
            this.requestTypeComboBox.TabIndex = 7;
            this.requestTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.requestTypeComboBox_SelectedIndexChanged);
            // 
            // queryText
            // 
            this.queryText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.queryText.Location = new System.Drawing.Point(16, 108);
            this.queryText.Multiline = true;
            this.queryText.Name = "queryText";
            this.queryText.Size = new System.Drawing.Size(624, 253);
            this.queryText.TabIndex = 9;
            this.queryText.TextChanged += new System.EventHandler(this.queryText_TextChanged);
            // 
            // queryLabel
            // 
            this.queryLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.queryLabel.AutoSize = true;
            this.queryLabel.Location = new System.Drawing.Point(16, 92);
            this.queryLabel.Name = "queryLabel";
            this.queryLabel.Size = new System.Drawing.Size(64, 13);
            this.queryLabel.TabIndex = 10;
            this.queryLabel.Text = "Query body:";
            // 
            // submitButton
            // 
            this.submitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.submitButton.Location = new System.Drawing.Point(565, 399);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(75, 23);
            this.submitButton.TabIndex = 11;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            // 
            // newKeyLabel
            // 
            this.newKeyLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.newKeyLabel.AutoSize = true;
            this.newKeyLabel.Location = new System.Drawing.Point(16, 131);
            this.newKeyLabel.Name = "newKeyLabel";
            this.newKeyLabel.Size = new System.Drawing.Size(35, 13);
            this.newKeyLabel.TabIndex = 12;
            this.newKeyLabel.Text = "label1";
            this.newKeyLabel.Visible = false;
            // 
            // newKeyText
            // 
            this.newKeyText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.newKeyText.Location = new System.Drawing.Point(16, 147);
            this.newKeyText.Name = "newKeyText";
            this.newKeyText.Size = new System.Drawing.Size(624, 20);
            this.newKeyText.TabIndex = 13;
            this.newKeyText.Visible = false;
            this.newKeyText.TextChanged += new System.EventHandler(this.newKeyText_TextChanged);
            // 
            // newValueText
            // 
            this.newValueText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.newValueText.Location = new System.Drawing.Point(16, 186);
            this.newValueText.Name = "newValueText";
            this.newValueText.Size = new System.Drawing.Size(624, 20);
            this.newValueText.TabIndex = 15;
            this.newValueText.Visible = false;
            this.newValueText.TextChanged += new System.EventHandler(this.newValueText_TextChanged);
            // 
            // newValueLabel
            // 
            this.newValueLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.newValueLabel.AutoSize = true;
            this.newValueLabel.Location = new System.Drawing.Point(16, 170);
            this.newValueLabel.Name = "newValueLabel";
            this.newValueLabel.Size = new System.Drawing.Size(35, 13);
            this.newValueLabel.TabIndex = 14;
            this.newValueLabel.Text = "label1";
            this.newValueLabel.Visible = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(16, 41);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.coreNameComboBox);
            this.splitContainer1.Panel1.Controls.Add(this.coreNameLabel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.requestTypeComboBox);
            this.splitContainer1.Panel2.Controls.Add(this.requestTypeLabel);
            this.splitContainer1.Size = new System.Drawing.Size(624, 48);
            this.splitContainer1.SplitterDistance = 374;
            this.splitContainer1.TabIndex = 16;
            // 
            // fullQueryText
            // 
            this.fullQueryText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fullQueryText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.fullQueryText.Location = new System.Drawing.Point(42, 380);
            this.fullQueryText.Multiline = true;
            this.fullQueryText.Name = "fullQueryText";
            this.fullQueryText.ReadOnly = true;
            this.fullQueryText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.fullQueryText.Size = new System.Drawing.Size(598, 20);
            this.fullQueryText.TabIndex = 17;
            this.fullQueryText.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(16, 375);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(20, 20);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 18;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // helpButton
            // 
            this.helpButton.Location = new System.Drawing.Point(484, 399);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(75, 23);
            this.helpButton.TabIndex = 19;
            this.helpButton.Text = "Help";
            this.helpButton.UseVisualStyleBackColor = true;
            this.helpButton.Click += new System.EventHandler(this.helpButton_Click);
            // 
            // facetFiedlLabel
            // 
            this.facetFiedlLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.facetFiedlLabel.AutoSize = true;
            this.facetFiedlLabel.Location = new System.Drawing.Point(16, 209);
            this.facetFiedlLabel.Name = "facetFiedlLabel";
            this.facetFiedlLabel.Size = new System.Drawing.Size(35, 13);
            this.facetFiedlLabel.TabIndex = 20;
            this.facetFiedlLabel.Text = "label1";
            this.facetFiedlLabel.Visible = false;
            // 
            // facetValueText
            // 
            this.facetValueText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.facetValueText.Location = new System.Drawing.Point(16, 225);
            this.facetValueText.Name = "facetValueText";
            this.facetValueText.Size = new System.Drawing.Size(624, 20);
            this.facetValueText.TabIndex = 21;
            this.facetValueText.Visible = false;
            this.facetValueText.TextChanged += new System.EventHandler(this.facetValueText_TextChanged);
            // 
            // customQueryList
            // 
            this.customQueryList.FormattingEnabled = true;
            this.customQueryList.Location = new System.Drawing.Point(42, 367);
            this.customQueryList.Name = "customQueryList";
            this.customQueryList.Size = new System.Drawing.Size(436, 56);
            this.customQueryList.TabIndex = 22;
            this.customQueryList.Visible = false;
            // 
            // Add
            // 
            this.Add.Location = new System.Drawing.Point(484, 367);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(75, 23);
            this.Add.TabIndex = 23;
            this.Add.Text = "Add";
            this.Add.UseVisualStyleBackColor = true;
            this.Add.Visible = false;
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // CustomQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 443);
            this.Controls.Add(this.Add);
            this.Controls.Add(this.customQueryList);
            this.Controls.Add(this.facetValueText);
            this.Controls.Add(this.facetFiedlLabel);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.fullQueryText);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.newValueText);
            this.Controls.Add(this.newValueLabel);
            this.Controls.Add(this.newKeyText);
            this.Controls.Add(this.newKeyLabel);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.queryLabel);
            this.Controls.Add(this.queryText);
            this.Controls.Add(this.titleLabel);
            this.MinimumSize = new System.Drawing.Size(392, 318);
            this.Name = "CustomQuery";
            this.Text = "CustomQuery";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.ComboBox coreNameComboBox;
        private System.Windows.Forms.Label coreNameLabel;
        private System.Windows.Forms.Label requestTypeLabel;
        private System.Windows.Forms.ComboBox requestTypeComboBox;
        private System.Windows.Forms.TextBox queryText;
        private System.Windows.Forms.Label queryLabel;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.Label newKeyLabel;
        private System.Windows.Forms.TextBox newKeyText;
        private System.Windows.Forms.TextBox newValueText;
        private System.Windows.Forms.Label newValueLabel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox fullQueryText;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button helpButton;
        private System.Windows.Forms.Label facetFiedlLabel;
        private System.Windows.Forms.TextBox facetValueText;
        private System.Windows.Forms.ListBox customQueryList;
        private System.Windows.Forms.Button Add;
    }
}