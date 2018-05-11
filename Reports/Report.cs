using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExchangeOnePassIdxGUI
{
    public partial class Report : Form
    {
        List<String> coreNames;
        List<MonitorFolderActivity.SegmentInfo> segmentInfoList;
        List<MonitorFolderActivity.ContentIndexedInfo> contentIndexedInfoList;
        List<List<MonitorFolderActivity.PerformanceInfo>> performanceInfo;
        List<List<int>> data;
        MonitorFolderActivity.ContentIndexedInfo cii;

        public Report()
        {
            InitializeComponent();
        }

        public Report(List<String> coreNames, List<MonitorFolderActivity.SegmentInfo> segmentInfoList) //user wants a segment report
        {
            InitializeComponent();
            this.coreNames = coreNames;
            this.segmentInfoList = segmentInfoList;
            this.listView1.Columns[3].Width = 157;
            populateList();
        }

        public Report(List<String> coreNames, List<MonitorFolderActivity.ContentIndexedInfo> contentIndexedInfoList) //user wants an indexed content report
        {
            InitializeComponent();
            this.coreNames = coreNames;
            this.contentIndexedInfoList = contentIndexedInfoList;
            populateList();
        }

        public Report(List<String> coreNames, List<List<int>> data) //core level content report
        {
            InitializeComponent();
            this.coreNames = coreNames;
            this.data = data;
            populateList();
        }

        public Report(String mailbox, MonitorFolderActivity.ContentIndexedInfo cii)
        {
            InitializeComponent();
            List<String> temp = new List<string>();
            temp.Add(mailbox);
            coreNames = temp;
            this.cii = cii;
            this.ReportTitle.Text = "Content Report";
            populateList();
        }

        public Report(String core, List<List<MonitorFolderActivity.PerformanceInfo>> performanceInfo)
        {
            InitializeComponent();
            //this.coreNames = coreNames;
            this.performanceInfo = performanceInfo;
            this.ReportTitle.Text = "Performance Counters Report for " + core;
            populateList();
        }

        private void populateList()
        {
            if (performanceInfo != null)
            {
                this.listView1.Columns.RemoveAt(3);
                this.listView1.Columns.RemoveAt(2);
                this.listView1.Columns.RemoveAt(1);
                this.listView1.Columns.RemoveAt(0);
                this.listView1.Columns.Add(new ColumnHeader("Operation"));
                this.listView1.Columns[0].Text = "Operation";
                this.listView1.Columns.Add(new ColumnHeader("Count"));
                this.listView1.Columns[1].Text = "Count";
                this.listView1.Columns.Add(new ColumnHeader("Time Taken"));
                this.listView1.Columns[2].Text = "Time Taken";
                this.listView1.Columns.Add(new ColumnHeader("Average Time Taken"));
                this.listView1.Columns[3].Text = "Average Time Taken";
                foreach (MonitorFolderActivity.PerformanceInfo performanceInfos in performanceInfo[0])
                {
                    String[] listitem = { performanceInfos.operationName, performanceInfos.count + "", performanceInfos.timeTaken + "", performanceInfos.avgTimeTaken + ""};
                    this.listView1.Items.Add(new ListViewItem(listitem));
                }

                return;
            }
            if (segmentInfoList != null)
            {
                for (int x = 0; x < coreNames.Count; x++)
                {
                    String[] listItem = { coreNames[x], segmentInfoList[x].numSegments + "", segmentInfoList[x].lastMergeDate, segmentInfoList[x].numberOfDocuments + ""};
                    this.listView1.Items.Add(new ListViewItem(listItem));
                }
            }
            if (contentIndexedInfoList != null)
            {
                this.listView1.Columns.RemoveAt(2);
                this.listView1.Columns.RemoveAt(1);
                this.listView1.Columns.Add(new ColumnHeader("Number of Documents"));
                this.listView1.Columns[1].Text = "Number of Documents";
                this.listView1.Columns[1].Width = 130;
                ReportTitle.Text = "Content Report";
                this.Text = "ContentReport";
                for (int x = 0; x < coreNames.Count; x++)
                {
                    String[] listItem = { coreNames[x], contentIndexedInfoList[x].numberOfDocuments + "" };
                    this.listView1.Items.Add(new ListViewItem(listItem));
                }
            }
            if(data != null)
            {
                this.listView1.Columns.RemoveAt(3);
                this.listView1.Columns.RemoveAt(2);
                this.listView1.Columns.RemoveAt(1);
                this.listView1.Columns.Add(new ColumnHeader("Number of Documents"));
                this.listView1.Columns[1].Text = "Number of Documents";
                this.listView1.Columns[1].Width = 130;
                this.listView1.Columns.Add(new ColumnHeader("Total Backup Count"));
                this.listView1.Columns[2].Text = "Total Backup Count";
                this.listView1.Columns[2].Width = 130;
                this.listView1.Columns.Add(new ColumnHeader("Total To Be Content Indexed"));
                this.listView1.Columns[3].Text = "Total To Be Content Indexed";
                this.listView1.Columns[3].Width = 130;
                this.listView1.Columns.Add(new ColumnHeader("Total CI Success"));
                this.listView1.Columns[4].Text = "Total CI Success";
                this.listView1.Columns[4].Width = 130;
                this.listView1.Columns.Add(new ColumnHeader("Total CI Failure"));
                this.listView1.Columns[5].Text = "Total CI Failure";
                this.listView1.Columns[5].Width = 130;
                this.listView1.Columns.RemoveAt(1);
                ReportTitle.Text = "Content Report";
                this.Text = "ContentReport";

                List<int> totalBackupCount = data[0];
                List<int> totalContentToBeIndexed = data[1];
                List<int> totalCiSuccess = data[2];
                List<int> totalCIFailed = data[3];

                for (int x = 0; x < coreNames.Count; x++)
                {
                    String[] listItem = { coreNames[x], totalBackupCount[x] + "", totalContentToBeIndexed[x] + "", totalCiSuccess[x] + "", totalCIFailed[x] + "" };
                    this.listView1.Items.Add(new ListViewItem(listItem));
                }
            }
            if (coreNames.Count == 1) //user submitted mailbox level report
            {
                this.listView1.Columns.RemoveAt(3);
                this.listView1.Columns.RemoveAt(2);
                this.listView1.Columns.RemoveAt(1);
                this.listView1.Columns.RemoveAt(0);
                this.listView1.Columns.Add(new ColumnHeader("Mailbox"));
                this.listView1.Columns[0].Text = "Mailbox";
                this.listView1.Columns[0].Width = 130;
                this.listView1.Columns.Add(new ColumnHeader("Total Backup Count"));
                this.listView1.Columns[1].Text = "Total Backup Count";
                this.listView1.Columns[1].Width = 130;
                this.listView1.Columns.Add(new ColumnHeader("Total To Be Content Indexed"));
                this.listView1.Columns[2].Text = "Total To Be Content Indexed";
                this.listView1.Columns[2].Width = 130;
                this.listView1.Columns.Add(new ColumnHeader("Total CI Success"));
                this.listView1.Columns[3].Text = "Total CI Success";
                this.listView1.Columns[3].Width = 130;
                this.listView1.Columns.Add(new ColumnHeader("Total CI Failed"));
                this.listView1.Columns[4].Text = "Total CI Failed";
                this.listView1.Columns[4].Width = 130;
                this.listView1.Columns.Add(new ColumnHeader("Last Backup JobID"));
                this.listView1.Columns[5].Text = "Last Backup JobID";
                this.listView1.Columns[5].Width = 130;
                this.listView1.Columns.Add(new ColumnHeader("Last Content Indexed JobID"));
                this.listView1.Columns[6].Text = "Last Content Indexed JobID";
                this.listView1.Columns[6].Width = 130;

                String[] listItem = { coreNames[0], cii.totalBackupCount + "", cii.totalToBeContentIndexed + "", cii.totalCISuccess + "", cii.totalCIFailed + "", cii.lastBackupJobId + "", cii.lastContentIndexedJobId + "" };
                this.listView1.Items.Add(new ListViewItem(listItem));
            }
        }
    }
}
