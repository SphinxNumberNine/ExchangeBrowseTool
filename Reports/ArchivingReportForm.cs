using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MonitorFolderActivity;
namespace ExchangeOnePassIdxGUI
{
    public partial class ArchivingReportForm : Form
    {
        int defaultHours = 24;
        int defaultDays = 7;
        frmMain frm;
        SolrQuery query;
        String core;
        String reportType;
        String cvowner;
        Boolean initialSubmit = false;

        public ArchivingReportForm()
        {
            InitializeComponent();
            hoursButton.Select();
            customTimeBox.Text = defaultHours + "";
        }

        public ArchivingReportForm(frmMain frm, SolrQuery query, String core)
        {
            InitializeComponent();
            hoursButton.Select();
            customTimeBox.Text = defaultHours + "";
            listView1.Columns[1].TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.frm = frm;
            this.query = query;
            this.core = core;
            this.reportType = "CORE";
        }

        public ArchivingReportForm(SolrQuery query, frmMain frm, String cvowner, String core)
        {
            InitializeComponent();
            hoursButton.Select();
            customTimeBox.Text = defaultHours + "";
            listView1.Columns[1].TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.frm = frm;
            this.query = query;
            this.cvowner = cvowner;
            this.core = core;
            this.reportType = "MAILBOX";
        }

        private void hoursButton_CheckedChanged(object sender, EventArgs e)
        {
            if (hoursButton.Checked)
            {
                unitsLabel.Text = "Hours";
                customTimeBox.Text = defaultHours + "";
            }
        }

        private void daysButton_CheckedChanged(object sender, EventArgs e)
        {
            if (daysButton.Checked)
            {
                unitsLabel.Text = "Days";
                customTimeBox.Text = defaultDays + "";
            }
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            try
            {
                if ((sender as Button).Text.Equals("Search"))
                {
                    initialSubmit = true;
                }
            }
            catch { }
            this.listView1.Items.Clear();
            String unit = unitsLabel.Text;
            int value = 0;
            Int32.TryParse(customTimeBox.Text, out value);
            DateTime currentTime = DateTime.Now;
            int currentYear = currentTime.Year;
            int currentMonth = currentTime.Month;
            int currentDay = currentTime.Day;
            int currentHour = currentTime.Hour;
            int currentMinute = currentTime.Minute;
            int currentSecond = currentTime.Second;
            DateTime beginningTime;

            if (unit.Equals("Days")) //days
            {
                beginningTime = currentTime.AddDays(-1 * value);
            }
            else //hours
            {
                beginningTime = currentTime.AddHours(-1 * value);
            }

            List<Tuple<String, int>> group;
            List<List<Tuple<String, int>>> group1;

            if (reportType.Equals("CORE"))
            {
                if (core.Equals("journalmbx"))
                {
                    group = query.coreLevelArchivingReport(core, beginningTime, currentTime, unit.ToLower(), value);
                    foreach (Tuple<String, int> t in group)
                    {
                        String[] item = { t.Item1, t.Item2 + "" };
                        var additem = new ListViewItem(item);
                        listView1.Items.Add(additem);
                    }
                }
                else
                {
                    group1 = query.coreLevelArchivingReport(query.getCoreNames().GetRange(1, 8), beginningTime, currentTime, unit.ToLower(), value);
                    listView1.Columns.RemoveAt(1);
                    ColumnHeader usermbx0 = new ColumnHeader();
                    usermbx0.Text = "usermbx0";
                    ColumnHeader usermbx1 = new ColumnHeader();
                    usermbx1.Text = "usermbx1";
                    ColumnHeader usermbx2 = new ColumnHeader();
                    usermbx2.Text = "usermbx2";
                    ColumnHeader usermbx3 = new ColumnHeader();
                    usermbx3.Text = "usermbx3";
                    ColumnHeader usermbx4 = new ColumnHeader();
                    usermbx4.Text = "usermbx4";
                    ColumnHeader usermbx5 = new ColumnHeader();
                    usermbx5.Text = "usermbx5";
                    ColumnHeader usermbx6 = new ColumnHeader();
                    usermbx6.Text = "usermbx6";
                    ColumnHeader usermbx7 = new ColumnHeader();
                    usermbx7.Text = "usermbx7";

                    if (listView1.Columns.Count > 2)
                    {
                        while (listView1.Columns.Count > 1)
                        {
                            listView1.Columns.RemoveAt(listView1.Columns.Count - 1);
                        }
                    }

                    ColumnHeader[] newColumns = { usermbx0, usermbx1, usermbx2, usermbx3, usermbx4, usermbx5, usermbx6, usermbx7 };
                    listView1.Columns.AddRange(newColumns);
                    

                    Tuple<String, int>[][] arrays = group1.Select(a => a.ToArray()).ToArray();

                    for (int x = 0; x < arrays[0].Length; x++)
                    {
                        String date = arrays[0][x].Item1;
                        List<string> totals = new List<String>();
                        for (int y = 0; y < arrays.Length; y++)
                        {
                            totals.Add(arrays[y][x].Item2 + "");
                        }
                        List<String> listItem = new List<string>();
                        listItem.Add(date);
                        listItem.AddRange(totals);
                        String[] listitem = listItem.ToArray();
                        listView1.Items.Add(new ListViewItem(listitem));
                    }

                }
            }
            else
            {
                group =  query.mailboxLevelArchivingReport(core, beginningTime, currentTime, unit.ToLower(), value, cvowner);
                foreach (Tuple<String, int> t in group)
                {
                    String[] item = { t.Item1, t.Item2 + "" };
                    var additem = new ListViewItem(item);
                    listView1.Items.Add(additem);
                }
            }
        }
    }
}