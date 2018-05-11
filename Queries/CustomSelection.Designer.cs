namespace ExchangeOnePassIdxGUI
{
    partial class CustomSelection
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
            this.currentSelection_Box = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ValueBox = new System.Windows.Forms.TextBox();
            this.selectionComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Custom Selection";
            // 
            // currentSelection_Box
            // 
            this.currentSelection_Box.Location = new System.Drawing.Point(12, 80);
            this.currentSelection_Box.Multiline = true;
            this.currentSelection_Box.Name = "currentSelection_Box";
            this.currentSelection_Box.ReadOnly = true;
            this.currentSelection_Box.Size = new System.Drawing.Size(407, 128);
            this.currentSelection_Box.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(325, 214);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Submit Selection";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Selection: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(193, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Value: ";
            // 
            // ValueBox
            // 
            this.ValueBox.Location = new System.Drawing.Point(239, 32);
            this.ValueBox.Name = "ValueBox";
            this.ValueBox.Size = new System.Drawing.Size(119, 20);
            this.ValueBox.TabIndex = 6;
            // 
            // selectionComboBox
            // 
            this.selectionComboBox.AutoCompleteCustomSource.AddRange(new string[] {
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
            this.selectionComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.selectionComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.selectionComboBox.FormattingEnabled = true;
            this.selectionComboBox.Items.AddRange(new object[] {
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
            this.selectionComboBox.Location = new System.Drawing.Point(75, 32);
            this.selectionComboBox.Name = "selectionComboBox";
            this.selectionComboBox.Size = new System.Drawing.Size(112, 21);
            this.selectionComboBox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(170, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Current Selection";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(369, 30);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(50, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Add";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // CustomSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 242);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.selectionComboBox);
            this.Controls.Add(this.ValueBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.currentSelection_Box);
            this.Controls.Add(this.label1);
            this.Name = "CustomSelection";
            this.Text = "CustomSelection";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox currentSelection_Box;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ValueBox;
        private System.Windows.Forms.ComboBox selectionComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button2;
    }
}