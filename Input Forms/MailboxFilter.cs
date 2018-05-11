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
    public partial class MailboxFilter : Form
    {
        public MailboxFilter(List<String> mailboxes)
        {
            InitializeComponent();
            var source = new AutoCompleteStringCollection();
            source.AddRange(mailboxes.ToArray());
            textBox1.AutoCompleteCustomSource = source;
            textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }
    }
}
