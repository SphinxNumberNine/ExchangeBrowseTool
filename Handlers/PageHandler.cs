using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MonitorFolderActivity;

namespace ExchangeOnePassIdxGUI
{
    public class PageHandler
    {
        frmMain formMain;
        public PageHandler(frmMain formMain)
        {
            this.formMain = formMain;
        }

        public void nextPage()
        {
            if (formMain.listView1.Items.Count < formMain.pageSize)
            {
                return;
            }
            if (((ServerResult)formMain.treeView1.SelectedNode.Tag).dataType.Equals("6"))
            {
                formMain.start += 1;
                formMain.treeView1_AfterSelect(formMain.treeView1, new TreeViewEventArgs(formMain.treeView1.SelectedNode));
                return;
            }
            else if (((ServerResult)formMain.treeView1.SelectedNode.Tag).dataType.Equals("7"))
            {
                formMain.start += 1;
                formMain.treeView1_AfterSelect(formMain.treeView1, new TreeViewEventArgs(formMain.treeView1.SelectedNode));
                return;
            }
            formMain.listView1.Items.Clear();
            formMain.start += 1;
            SolrQuery newQuery = new SolrQuery(formMain.ip, formMain);
            List<ServerResult> mails;
            if (formMain.jidFilter)
            {
                mails = new XMLParser(newQuery.getMailsForOneSubfolder(formMain.selectedItemGuid, formMain.start, formMain.jid, formMain.mailSort, formMain.currentCore)).results;
            }
            else
            {
                mails = new XMLParser(newQuery.getMailsForOneSubfolder(formMain.selectedItemGuid, formMain.start, formMain.mailSort, formMain.currentCore)).results;
            }
            formMain.currentPage = mails;
            formMain.listView1.Items.Clear();
            formMain.writeMailsInList(mails);
            formMain.AppendTextBox("Now displaying page " + (formMain.start + 1).ToString());
        }

        public void previousPage()
        {
            if (formMain.start > 0)
            {
                if (((ServerResult)formMain.treeView1.SelectedNode.Tag).dataType.Equals("6"))
                {
                    formMain.start -= 1;
                    formMain.treeView1_AfterSelect(formMain.treeView1, new TreeViewEventArgs(formMain.treeView1.SelectedNode));
                    return;
                }
                else if (((ServerResult)formMain.treeView1.SelectedNode.Tag).dataType.Equals("7"))
                {
                    formMain.start -= 1;
                    formMain.treeView1_AfterSelect(formMain.treeView1, new TreeViewEventArgs(formMain.treeView1.SelectedNode));
                    return;
                }
                formMain.listView1.Items.Clear();
                formMain.start -= 1;
                //treeView1_AfterSelect(this.treeView1.SelectedNode, new TreeViewEventArgs(this.treeView1.SelectedNode));
                SolrQuery newQuery = new SolrQuery(formMain.ip, formMain);
                List<ServerResult> mails;
                if (formMain.jidFilter)
                {
                    mails = new XMLParser(newQuery.getMailsForOneSubfolder(formMain.selectedItemGuid, formMain.start, formMain.jid, formMain.mailSort)).results;
                }
                else
                {
                    mails = new XMLParser(newQuery.getMailsForOneSubfolder(formMain.selectedItemGuid, formMain.start, formMain.mailSort)).results;
                }
                formMain.currentPage = mails;
                formMain.listView1.Items.Clear();
                formMain.writeMailsInList(mails);
                formMain.AppendTextBox("Now displaying page " + (formMain.start + 1).ToString());
            }
        }

        public void customPage(object sender, KeyPressEventArgs e)
        {
            formMain.listView1.Items.Clear();
            try
            {
                formMain.start = Int32.Parse((sender as TextBox).Text) - 1;
            }
            catch
            {
                MessageBox.Show("Invalid Page Number");
                formMain.customPageLink(sender, e);
            }
            SolrQuery newQuery = new SolrQuery(formMain.ip, formMain);
            List<ServerResult> mails;
            if (formMain.jidFilter)
            {
                mails = new XMLParser(newQuery.getMailsForOneSubfolder(formMain.selectedItemGuid, formMain.start, formMain.jid)).results;
            }
            else
            {
                mails = new XMLParser(newQuery.getMailsForOneSubfolder(formMain.selectedItemGuid, formMain.start, formMain.mailSort)).results;
            }
            formMain.currentPage = mails;
            formMain.listView1.Items.Clear();
            formMain.writeMailsInList(mails);
            if ((mails.Count == 0) || (mails == null))
            {
                MessageBox.Show("No messages found on page " + (formMain.start + 1));
            }
            else
            {
                try
                {
                    formMain.AppendTextBox("Now displaying page " + formMain.start);
                }
                catch (Exception ex)
                {

                }
            }
        }

        public void lastPage()
        {
            try
            {
                formMain.start = formMain.lastPage;
                if (formMain.listView1.Items.Count < formMain.pageSize)
                {
                    return;
                }
                if (((ServerResult)formMain.treeView1.SelectedNode.Tag).dataType.Equals("6"))
                {
                    formMain.treeView1_AfterSelect(formMain.treeView1, new TreeViewEventArgs(formMain.treeView1.SelectedNode));
                    return;
                }
                else if (((ServerResult)formMain.treeView1.SelectedNode.Tag).dataType.Equals("7"))
                {
                    formMain.treeView1_AfterSelect(formMain.treeView1, new TreeViewEventArgs(formMain.treeView1.SelectedNode));
                    return;
                }
                formMain.listView1.Items.Clear();
                SolrQuery newQuery = new SolrQuery(formMain.ip, formMain);
                List<ServerResult> mails;
                if (formMain.jidFilter)
                {
                    mails = new XMLParser(newQuery.getMailsForOneSubfolder(formMain.selectedItemGuid, formMain.start, formMain.jid, formMain.mailSort, formMain.currentCore)).results;
                }
                else
                {
                    mails = new XMLParser(newQuery.getMailsForOneSubfolder(formMain.selectedItemGuid, formMain.start, formMain.mailSort, formMain.currentCore)).results;
                }
                formMain.currentPage = mails;
                formMain.listView1.Items.Clear();
                formMain.writeMailsInList(mails);
                formMain.AppendTextBox("Now displaying page " + (formMain.start + 1).ToString());
            }
            catch { }
        }
    }
}
