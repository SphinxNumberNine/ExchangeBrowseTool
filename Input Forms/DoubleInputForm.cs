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
    public partial class DoubleInputForm : Form
    {
        public struct Credentials
        {
            public String user;
            public String subclient;
        }

        public Credentials creds;

        public DoubleInputForm()
        {
            InitializeComponent();
        }

        public void button1_Click(object sender, EventArgs e)
        {
            creds.user = richTextBox1.Text;
            creds.subclient = textBox1.Text;

            this.Hide();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }

        public String getUser()
        {
            return creds.user;
        }

        public String getSubclient()
        {
            return creds.subclient;
        }
    }
}
