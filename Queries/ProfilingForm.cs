using MonitorFolderActivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExchangeOnePassIdxGUI {

    public partial class ProfilingForm : Form {

        int currentTimeMs;
        bool isRunning, stopProc, cancelled;
        string newTimeMs;
        string mailbox = "";
        Dictionary<string, string> cvowners = new Dictionary<string, string>();

        public ProfilingForm(int currentTimeMs, bool isPaused, TreeNodeCollection treeNodes) {
            InitializeComponent();

            this.currentTimeMs = currentTimeMs;
            this.isRunning = !isPaused;
            this.textBox1.Text = currentTimeMs.ToString();
            this.label2.AutoSize = false;
            this.label2.Width = this.textBox1.Width;
            this.label2.Height = 100;
            this.FormClosing += ProfilingForm_FormClosing;
            this.cancelled = false;

            if (isRunning) {
                this.start.Text = "Reset";
                this.stop.Text = "Stop";
                this.cancel.Visible = true;
            } else {
                this.start.Text = "Start";
                this.stop.Text = "Cancel";
                this.cancel.Visible = false;
            }

            List<String> topFolders = new List<string>();
            this.mailboxTextbox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.mailboxTextbox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            foreach (TreeNode node in treeNodes) {
                foreach (TreeNode childNode in node.Nodes) {
                    foreach (TreeNode grandChildNode in childNode.Nodes) {
                        if (grandChildNode.Tag != null) {
                            ServerResult tempNodeResults = (ServerResult)grandChildNode.Tag;
                            if (tempNodeResults.dataType.Equals("5")) {
                                topFolders.Add(grandChildNode.Text);
                                cvowners.Add(grandChildNode.Text.ToLower(), tempNodeResults.cvowner.ToLower());
                            }
                        }
                    }
                }
            }

            AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
            collection.AddRange(topFolders.ToArray());
            this.mailboxTextbox.AutoCompleteCustomSource = collection;

            this.textBox1.KeyDown += (sender, args) => {
                if (args.KeyCode == Keys.Return) {
                    this.start.PerformClick();
                }
            };
        }

        private void button1_Click(object sender, EventArgs e) {
            if (this.start.Text.Equals("Start")) {
                stopProc = false;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }

            if (isRunning) {
                this.start.Text = "Start";
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            if (this.stop.Text.Equals("Stop")) {
                stopProc = true;
                this.DialogResult = DialogResult.Abort;
                this.Close();
            } else if (this.stop.Text.Equals("Cancel")) {
                cancelled = true;
                stopProc = false;
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e) {
            if (this.cancel.Visible) {
                cancelled = true;
                stopProc = false;
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        public string getNewTime() {
            return newTimeMs;
        }

        public string getCvowner() {
            string cvowner = "";

            try {
                cvowner = cvowners[mailbox.ToLower()];
            } catch {

            }

            return cvowner;
        }

        public bool getStatus() {
            return stopProc;
        }

        public bool getCancelled() {
            return cancelled;
        }

        private void ProfilingForm_FormClosing(object sender, FormClosingEventArgs e) {
            newTimeMs = this.textBox1.Text;
            mailbox = this.mailboxTextbox.Text;
        }
    }
}
