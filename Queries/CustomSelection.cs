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
    public partial class CustomSelection : Form
    {

        public List<String> fieldstrings;

        public CustomSelection()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Boolean found = false;
            String fieldString = selectionComboBox.Text + ":" + ValueBox.Text;
            foreach (String line in currentSelection_Box.Lines)
            {
                if(line.Substring(0, line.IndexOf(":")).Equals(fieldString.Substring(0, fieldString.IndexOf(":"))))
                {
                    found = true;
                }
            }
            if (found != true)
            {
                List<String> lines = currentSelection_Box.Lines.ToList();
                lines.Add(fieldString);
                currentSelection_Box.Lines = lines.ToArray();
                selectionComboBox.Text = "";
                ValueBox.Text = "";
            }
            else
            {
                MessageBox.Show("You've already specified a selection for that field.");
            }
        }

        public void button1_Click(object sender, EventArgs e)
        {
            List<String> lines = currentSelection_Box.Lines.ToList();
            fieldstrings = lines;
            this.DialogResult = DialogResult.OK;
        }
    }
}
