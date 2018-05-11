using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExchangeOnePassIdxGUI {
    public partial class DebugSelect : Form {
        public int debuggingLevel = 0;

        public DebugSelect(int currentLevel) {
            InitializeComponent();

            debuggingLevel = currentLevel;

            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;

            switch (currentLevel) {
                case 0:
                    radioButton1.Checked = true;
                    break;
                case 1:
                    radioButton2.Checked = true;
                    break;
                case 2:
                    radioButton3.Checked = true;
                    break;
                case 3:
                    radioButton4.Checked = true;
                    break;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e) {
            if (this.radioButton1.Checked) {
                debuggingLevel = 0;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
                radioButton4.Checked = false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e) {
            if (this.radioButton2.Checked) {
                debuggingLevel = 1;
                radioButton1.Checked = false;
                radioButton3.Checked = false;
                radioButton4.Checked = false;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e) {
            if (this.radioButton3.Checked) {
                debuggingLevel = 2;
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton4.Checked = false;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e) {
            if (this.radioButton4.Checked) {
                debuggingLevel = 3;
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
            }
        }
    }
}
