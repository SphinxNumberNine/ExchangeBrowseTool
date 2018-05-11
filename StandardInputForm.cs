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
    public partial class StandardInputForm : Form
    {
        public StandardInputForm()
        {
            InitializeComponent();
        }

        private void StandardInputForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter)
            {
                this.DialogResult = DialogResult.OK;
            }
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void Description_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
