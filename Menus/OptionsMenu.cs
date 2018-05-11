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
    public partial class OptionsMenu : Form
    {
        public Boolean[] settings;
        public int debuggerLevel;
        public int pageSize;

        public OptionsMenu(int currentLevel)
        {
            InitializeComponent();

            debuggerLevel = currentLevel;

            settings = new Boolean[this.checkedListBox1.Items.Count];
            for (int x = 0; x < settings.Length; x++)
            {
                settings[x] = false;
            }
            fillSettings();
        }

        public OptionsMenu(Boolean[] settings, int currentLevel, int currentPageSize)
        {
            InitializeComponent();

            debuggerLevel = currentLevel;

            this.settings = settings;

            this.pageSize = currentPageSize;
            fillSettings();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < settings.Length; x++)
            {
                if (this.checkedListBox1.GetItemCheckState(x) == CheckState.Checked)
                {
                    settings[x] = true;
                }
                else
                {
                    settings[x] = false;
                }
            }
            pageSize = Int32.Parse(pageSizeBox.Text);
            this.Hide();
            //return settings;
        }
        private void fillSettings()
        {
            for (int x = 0; x < settings.Length; x++)
            {
                if (settings[x] == true)
                {
                    this.checkedListBox1.SetItemCheckState(x, CheckState.Checked);
                }
                else
                {
                    this.checkedListBox1.SetItemCheckState(x, CheckState.Unchecked);
                }
            }

            pageSizeBox.Text = pageSize + "";

        }

        private void button3_Click(object sender, EventArgs e) {
            DebugSelect frm = new DebugSelect(debuggerLevel);
            if (frm.ShowDialog() == DialogResult.OK) {
                debuggerLevel = frm.debuggingLevel;
            }
        }
    }
}
