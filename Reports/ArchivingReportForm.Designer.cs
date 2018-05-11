namespace ExchangeOnePassIdxGUI
{
    partial class ArchivingReportForm
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
            this.unitsLabel = new System.Windows.Forms.Label();
            this.customTimeBox = new System.Windows.Forms.TextBox();
            this.submitButton = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.dateColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.numberOfDocumentsIndexedColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.daysButton = new System.Windows.Forms.RadioButton();
            this.hoursButton = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Archiving Report";
            // 
            // unitsLabel
            // 
            this.unitsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.unitsLabel.AutoSize = true;
            this.unitsLabel.Location = new System.Drawing.Point(410, 19);
            this.unitsLabel.Name = "unitsLabel";
            this.unitsLabel.Size = new System.Drawing.Size(35, 13);
            this.unitsLabel.TabIndex = 3;
            this.unitsLabel.Text = "Hours";
            // 
            // customTimeBox
            // 
            this.customTimeBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.customTimeBox.Location = new System.Drawing.Point(304, 15);
            this.customTimeBox.Name = "customTimeBox";
            this.customTimeBox.Size = new System.Drawing.Size(100, 20);
            this.customTimeBox.TabIndex = 4;
            // 
            // submitButton
            // 
            this.submitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.submitButton.Location = new System.Drawing.Point(477, 14);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(54, 25);
            this.submitButton.TabIndex = 5;
            this.submitButton.Text = "Search";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.dateColumn,
            this.numberOfDocumentsIndexedColumn});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(15, 39);
            this.listView1.Name = "listView1";
            this.listView1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.listView1.Size = new System.Drawing.Size(518, 212);
            this.listView1.TabIndex = 6;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // dateColumn
            // 
            this.dateColumn.Text = "Date";
            this.dateColumn.Width = 250;
            // 
            // numberOfDocumentsIndexedColumn
            // 
            this.numberOfDocumentsIndexedColumn.Text = "Documents Indexed";
            this.numberOfDocumentsIndexedColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numberOfDocumentsIndexedColumn.Width = 250;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.daysButton);
            this.panel1.Controls.Add(this.hoursButton);
            this.panel1.Location = new System.Drawing.Point(140, 14);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(116, 24);
            this.panel1.TabIndex = 7;
            // 
            // daysButton
            // 
            this.daysButton.AutoSize = true;
            this.daysButton.Location = new System.Drawing.Point(63, 2);
            this.daysButton.Name = "daysButton";
            this.daysButton.Size = new System.Drawing.Size(49, 17);
            this.daysButton.TabIndex = 1;
            this.daysButton.TabStop = true;
            this.daysButton.Text = "Days";
            this.daysButton.UseVisualStyleBackColor = true;
            this.daysButton.CheckedChanged += new System.EventHandler(this.daysButton_CheckedChanged);
            // 
            // hoursButton
            // 
            this.hoursButton.AutoSize = true;
            this.hoursButton.Location = new System.Drawing.Point(4, 2);
            this.hoursButton.Name = "hoursButton";
            this.hoursButton.Size = new System.Drawing.Size(53, 17);
            this.hoursButton.TabIndex = 0;
            this.hoursButton.TabStop = true;
            this.hoursButton.Text = "Hours";
            this.hoursButton.UseVisualStyleBackColor = true;
            this.hoursButton.CheckedChanged += new System.EventHandler(this.hoursButton_CheckedChanged);
            // 
            // ArchivingReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 280);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.customTimeBox);
            this.Controls.Add(this.unitsLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimumSize = new System.Drawing.Size(559, 317);
            this.Name = "ArchivingReportForm";
            this.Text = "ArchivingReportForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label unitsLabel;
        private System.Windows.Forms.TextBox customTimeBox;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader dateColumn;
        private System.Windows.Forms.ColumnHeader numberOfDocumentsIndexedColumn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton daysButton;
        private System.Windows.Forms.RadioButton hoursButton;
    }
}