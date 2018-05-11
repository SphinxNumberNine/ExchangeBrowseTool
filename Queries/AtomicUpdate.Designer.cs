namespace ExchangeOnePassIdxGUI
{
    partial class AtomicUpdate
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
            this.TitleLabel = new System.Windows.Forms.Label();
            this.selectionLabel = new System.Windows.Forms.Label();
            this.fieldSelectionBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.FieldValueBox = new System.Windows.Forms.TextBox();
            this.addParameterButton = new System.Windows.Forms.Button();
            this.dividerLine = new System.Windows.Forms.Label();
            this.selectionsBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.updatedFieldValueBox = new System.Windows.Forms.TextBox();
            this.updatedFieldSelectionBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.addFieldUpdateButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.updatesBox = new System.Windows.Forms.TextBox();
            this.submitAtomicUpdateButton = new System.Windows.Forms.Button();
            this.coreComboBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.updateRowsBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.Location = new System.Drawing.Point(13, 13);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(113, 17);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "Atomic Update";
            // 
            // selectionLabel
            // 
            this.selectionLabel.AutoSize = true;
            this.selectionLabel.Location = new System.Drawing.Point(12, 44);
            this.selectionLabel.Name = "selectionLabel";
            this.selectionLabel.Size = new System.Drawing.Size(54, 13);
            this.selectionLabel.TabIndex = 1;
            this.selectionLabel.Text = "Selection:";
            // 
            // fieldSelectionBox
            // 
            this.fieldSelectionBox.AutoCompleteCustomSource.AddRange(new string[] {
            "afid",
            "afof",
            "apid",
            "appGUID",
            "atyp",
            "bktm",
            "ccdisp",
            "ccn",
            "cijid",
            "cistate",
            "clid",
            "contentid",
            "conv",
            "cvbkpendtm",
            "cvexchid",
            "cvowner",
            "cvownerdisp",
            "cvstub",
            "datatype",
            "folder",
            "fromdisp",
            "hasAttach",
            "indexedat",
            "jid",
            "lnks",
            "msgclass",
            "msgmodifiedtime",
            "mtm",
            "parentguid",
            "retentionjid",
            "szkb",
            "todisp",
            "visible"});
            this.fieldSelectionBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.fieldSelectionBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.fieldSelectionBox.FormattingEnabled = true;
            this.fieldSelectionBox.Items.AddRange(new object[] {
            "afid",
            "afof",
            "apid",
            "appGUID",
            "atyp",
            "bktm",
            "ccdisp",
            "ccn",
            "cijid",
            "cistate",
            "clid",
            "contentid",
            "conv",
            "cvbkpendtm",
            "cvexchid",
            "cvowner",
            "cvownerdisp",
            "cvstub",
            "datatype",
            "folder",
            "fromdisp",
            "hasAttach",
            "indexedat",
            "jid",
            "lnks",
            "msgclass",
            "msgmodifiedtime",
            "mtm",
            "parentguid",
            "retentionjid",
            "szkb",
            "todisp",
            "visible",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""});
            this.fieldSelectionBox.Location = new System.Drawing.Point(72, 41);
            this.fieldSelectionBox.Name = "fieldSelectionBox";
            this.fieldSelectionBox.Size = new System.Drawing.Size(154, 21);
            this.fieldSelectionBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(232, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Value:";
            // 
            // FieldValueBox
            // 
            this.FieldValueBox.Location = new System.Drawing.Point(272, 41);
            this.FieldValueBox.Name = "FieldValueBox";
            this.FieldValueBox.Size = new System.Drawing.Size(160, 20);
            this.FieldValueBox.TabIndex = 4;
            // 
            // addParameterButton
            // 
            this.addParameterButton.Location = new System.Drawing.Point(438, 39);
            this.addParameterButton.Name = "addParameterButton";
            this.addParameterButton.Size = new System.Drawing.Size(57, 23);
            this.addParameterButton.TabIndex = 5;
            this.addParameterButton.Text = "Add";
            this.addParameterButton.UseVisualStyleBackColor = true;
            this.addParameterButton.Click += new System.EventHandler(this.addParameterButton_Click);
            // 
            // dividerLine
            // 
            this.dividerLine.AutoSize = true;
            this.dividerLine.Location = new System.Drawing.Point(-1, 168);
            this.dividerLine.Name = "dividerLine";
            this.dividerLine.Size = new System.Drawing.Size(505, 13);
            this.dividerLine.TabIndex = 6;
            this.dividerLine.Text = "_________________________________________________________________________________" +
    "__";
            // 
            // selectionsBox
            // 
            this.selectionsBox.Location = new System.Drawing.Point(16, 91);
            this.selectionsBox.Multiline = true;
            this.selectionsBox.Name = "selectionsBox";
            this.selectionsBox.ReadOnly = true;
            this.selectionsBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.selectionsBox.Size = new System.Drawing.Size(479, 77);
            this.selectionsBox.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(203, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Current Selection";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 202);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Update:";
            // 
            // updatedFieldValueBox
            // 
            this.updatedFieldValueBox.Location = new System.Drawing.Point(272, 199);
            this.updatedFieldValueBox.Name = "updatedFieldValueBox";
            this.updatedFieldValueBox.Size = new System.Drawing.Size(160, 20);
            this.updatedFieldValueBox.TabIndex = 10;
            // 
            // updatedFieldSelectionBox
            // 
            this.updatedFieldSelectionBox.AutoCompleteCustomSource.AddRange(new string[] {
            "afid",
            "afof",
            "apid",
            "appGUID",
            "atyp",
            "bktm",
            "ccdisp",
            "ccn",
            "cijid",
            "cistate",
            "clid",
            "conv",
            "cvbkpendtm",
            "cvexchid",
            "cvownerdisp",
            "cvstub",
            "folder",
            "fromdisp",
            "hasAttach",
            "indexedat",
            "jid",
            "lnks",
            "msgclass",
            "msgmodifiedtime",
            "mtm",
            "retentionjid",
            "szkb",
            "todisp",
            "visible"});
            this.updatedFieldSelectionBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.updatedFieldSelectionBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.updatedFieldSelectionBox.FormattingEnabled = true;
            this.updatedFieldSelectionBox.Items.AddRange(new object[] {
            "afid",
            "afof",
            "apid",
            "appGUID",
            "atyp",
            "bktm",
            "ccdisp",
            "ccn",
            "cijid",
            "cistate",
            "clid",
            "conv",
            "cvbkpendtm",
            "cvexchid",
            "cvowner",
            "cvownerdisp",
            "cvstub",
            "datatype",
            "folder",
            "fromdisp",
            "hasAttach",
            "indexedat",
            "jid",
            "lnks",
            "msgclass",
            "msgmodifiedtime",
            "mtm",
            "parentguid",
            "retentionjid",
            "szkb",
            "todisp",
            "visible"});
            this.updatedFieldSelectionBox.Location = new System.Drawing.Point(64, 199);
            this.updatedFieldSelectionBox.Name = "updatedFieldSelectionBox";
            this.updatedFieldSelectionBox.Size = new System.Drawing.Size(162, 21);
            this.updatedFieldSelectionBox.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(231, 202);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Value:";
            // 
            // addFieldUpdateButton
            // 
            this.addFieldUpdateButton.Location = new System.Drawing.Point(438, 197);
            this.addFieldUpdateButton.Name = "addFieldUpdateButton";
            this.addFieldUpdateButton.Size = new System.Drawing.Size(57, 23);
            this.addFieldUpdateButton.TabIndex = 13;
            this.addFieldUpdateButton.Text = "Add";
            this.addFieldUpdateButton.UseVisualStyleBackColor = true;
            this.addFieldUpdateButton.Click += new System.EventHandler(this.addFieldUpdateButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(197, 236);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Update Selections";
            // 
            // updatesBox
            // 
            this.updatesBox.Location = new System.Drawing.Point(15, 252);
            this.updatesBox.Multiline = true;
            this.updatesBox.Name = "updatesBox";
            this.updatesBox.ReadOnly = true;
            this.updatesBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.updatesBox.Size = new System.Drawing.Size(480, 115);
            this.updatesBox.TabIndex = 15;
            // 
            // submitAtomicUpdateButton
            // 
            this.submitAtomicUpdateButton.Location = new System.Drawing.Point(372, 373);
            this.submitAtomicUpdateButton.Name = "submitAtomicUpdateButton";
            this.submitAtomicUpdateButton.Size = new System.Drawing.Size(123, 23);
            this.submitAtomicUpdateButton.TabIndex = 16;
            this.submitAtomicUpdateButton.Text = "Submit Atomic Update";
            this.submitAtomicUpdateButton.UseVisualStyleBackColor = true;
            this.submitAtomicUpdateButton.Click += new System.EventHandler(this.submitAtomicUpdateButton_Click);
            // 
            // coreComboBox
            // 
            this.coreComboBox.FormattingEnabled = true;
            this.coreComboBox.Location = new System.Drawing.Point(364, 9);
            this.coreComboBox.Name = "coreComboBox";
            this.coreComboBox.Size = new System.Drawing.Size(131, 21);
            this.coreComboBox.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(329, 12);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Core";
            // 
            // updateRowsBox
            // 
            this.updateRowsBox.Location = new System.Drawing.Point(84, 375);
            this.updateRowsBox.Name = "updateRowsBox";
            this.updateRowsBox.Size = new System.Drawing.Size(142, 20);
            this.updateRowsBox.TabIndex = 19;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 378);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "updateRows:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(291, 373);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 21;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(234, 373);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(51, 23);
            this.button2.TabIndex = 22;
            this.button2.Text = "Help";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // AtomicUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 406);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.updateRowsBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.coreComboBox);
            this.Controls.Add(this.submitAtomicUpdateButton);
            this.Controls.Add(this.updatesBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.addFieldUpdateButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.updatedFieldSelectionBox);
            this.Controls.Add(this.updatedFieldValueBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.selectionsBox);
            this.Controls.Add(this.dividerLine);
            this.Controls.Add(this.addParameterButton);
            this.Controls.Add(this.FieldValueBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.fieldSelectionBox);
            this.Controls.Add(this.selectionLabel);
            this.Controls.Add(this.TitleLabel);
            this.Name = "AtomicUpdate";
            this.Text = "AtomicUpdate";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Label selectionLabel;
        private System.Windows.Forms.ComboBox fieldSelectionBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox FieldValueBox;
        private System.Windows.Forms.Button addParameterButton;
        private System.Windows.Forms.Label dividerLine;
        private System.Windows.Forms.TextBox selectionsBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox updatedFieldValueBox;
        private System.Windows.Forms.ComboBox updatedFieldSelectionBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button addFieldUpdateButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox updatesBox;
        private System.Windows.Forms.Button submitAtomicUpdateButton;
        private System.Windows.Forms.ComboBox coreComboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox updateRowsBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}