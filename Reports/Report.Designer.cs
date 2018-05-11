namespace ExchangeOnePassIdxGUI
{
    partial class Report
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
            this.ReportTitle = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.coreName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.segmentNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lastMergeTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.numberOfDocuments = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // ReportTitle
            // 
            this.ReportTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ReportTitle.AutoSize = true;
            this.ReportTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReportTitle.Location = new System.Drawing.Point(12, 9);
            this.ReportTitle.Name = "ReportTitle";
            this.ReportTitle.Size = new System.Drawing.Size(125, 17);
            this.ReportTitle.TabIndex = 0;
            this.ReportTitle.Text = "Segment Report";
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.coreName,
            this.segmentNumber,
            this.lastMergeTime,
            this.numberOfDocuments});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(12, 29);
            this.listView1.MinimumSize = new System.Drawing.Size(405, 200);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(566, 200);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // coreName
            // 
            this.coreName.Text = "Core Name";
            this.coreName.Width = 100;
            // 
            // segmentNumber
            // 
            this.segmentNumber.Text = "Number of Segments";
            this.segmentNumber.Width = 135;
            // 
            // lastMergeTime
            // 
            this.lastMergeTime.Text = "Time of Last Merge";
            this.lastMergeTime.Width = 170;
            // 
            // numberOfDocuments
            // 
            this.numberOfDocuments.Text = "Number of Documents";
            // 
            // Report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 236);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.ReportTitle);
            this.MinimumSize = new System.Drawing.Size(606, 275);
            this.Name = "Report";
            this.Text = "SegmentReport";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ReportTitle;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader coreName;
        private System.Windows.Forms.ColumnHeader segmentNumber;
        private System.Windows.Forms.ColumnHeader lastMergeTime;
        private System.Windows.Forms.ColumnHeader numberOfDocuments;
    }
}