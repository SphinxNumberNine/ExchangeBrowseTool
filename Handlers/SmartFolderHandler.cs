using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using MonitorFolderActivity;

namespace ExchangeOnePassIdxGUI
{
    public class SmartFolderHandler
    {
        frmMain formMain;

        public SmartFolderHandler(frmMain formMain)
        {
            this.formMain = formMain;
        }

        public void addSmartFolders()
        {
            if (((ServerResult)formMain.treeView1.SelectedNode.Tag).dataType.Equals("5"))
            {
                ServerResult smartFolderSR = new ServerResult();
                smartFolderSR.dataType = "6";
                TreeNode toRecipientSmartFolder = new TreeNode("SMART FOLDER: To Recipient");
                toRecipientSmartFolder.Tag = smartFolderSR;
                TreeNode anyRecipientSmartFolder = new TreeNode("SMART FOLDER: Any Recipient");
                anyRecipientSmartFolder.Tag = smartFolderSR;
                TreeNode fromRecipientSmartFolder = new TreeNode("SMART FOLDER: From Recipient");
                fromRecipientSmartFolder.Tag = smartFolderSR;
                TreeNode hasAttachSmartFolder = new TreeNode("SMART FOLDER: Has Attachment");
                hasAttachSmartFolder.Tag = smartFolderSR;
                TreeNode importanceSmartFolder = new TreeNode("SMART FOLDER: Important Messages");
                importanceSmartFolder.Tag = smartFolderSR;
                TreeNode stubbedSmartFolder = new TreeNode("SMART FOLDER: Stubbed Messages");
                stubbedSmartFolder.Tag = smartFolderSR;
                TreeNode jidSmartFolder = new TreeNode("SMART FOLDER: JIDs");
                jidSmartFolder.Tag = smartFolderSR;
                TreeNode customSmartFolder = new TreeNode("SMART FOLDER: Custom Selection");
                customSmartFolder.Tag = smartFolderSR;

                try
                {
                    formMain.treeView1.Nodes.Remove(toRecipientSmartFolder);
                }
                catch { }
                try
                {
                    formMain.treeView1.Nodes.Remove(anyRecipientSmartFolder);
                }
                catch { }
                try
                {
                    formMain.treeView1.Nodes.Remove(fromRecipientSmartFolder);
                }
                catch { }
                try
                {
                    formMain.treeView1.Nodes.Remove(hasAttachSmartFolder);
                }
                catch { }
                try
                {
                    formMain.treeView1.Nodes.Remove(importanceSmartFolder);
                }
                catch { }
                try
                {
                    formMain.treeView1.Nodes.Remove(stubbedSmartFolder);
                }
                catch { }
                try
                {
                    formMain.treeView1.Nodes.Remove(jidSmartFolder);
                }
                catch { }
                try
                {
                    formMain.treeView1.Nodes.Remove(customSmartFolder);
                }
                catch { }

                formMain.treeView1.SelectedNode.Nodes.Add(toRecipientSmartFolder);
                formMain.treeView1.SelectedNode.Nodes.Add(anyRecipientSmartFolder);
                formMain.treeView1.SelectedNode.Nodes.Add(fromRecipientSmartFolder);
                formMain.treeView1.SelectedNode.Nodes.Add(hasAttachSmartFolder);
                formMain.treeView1.SelectedNode.Nodes.Add(importanceSmartFolder);
                formMain.treeView1.SelectedNode.Nodes.Add(stubbedSmartFolder);
                formMain.treeView1.SelectedNode.Nodes.Add(jidSmartFolder);
                formMain.treeView1.SelectedNode.Nodes.Add(customSmartFolder);
            }
        }

        public void smartFolderClick()
        {
            ServerResult mailboxRes = (ServerResult)formMain.treeView1.SelectedNode.Parent.Tag;
            String cvowner = mailboxRes.cvowner;
            String field = "";
            String core = formMain.treeView1.SelectedNode.Parent.Parent.Text;
            switch (formMain.treeView1.SelectedNode.Text)
            {
                case "SMART FOLDER: To Recipient":
                    field = "todisp:" + mailboxRes.mailbox;
                    break;
                case "SMART FOLDER: Any Recipient":
                    field = "ccdisp:" + mailboxRes.mailbox;
                    break;
                case "SMART FOLDER: From Recipient":
                    field = "fromdisp:" + mailboxRes.mailbox;
                    break;
                case "SMART FOLDER: Important Messages":
                    field = "msgimportance: 1";
                    break;
                case "SMART FOLDER: Has Attachment":
                    field = "hasattach:true";
                    break;
                case "SMART FOLDER: Stubbed Messages":
                    field = "cvstub:1";
                    break;
                case "SMART FOLDER: JIDs":
                    SolrQuery query1 = new SolrQuery(formMain.ip, formMain);
                    List<String> jids = query1.getJids(core, cvowner);
                    foreach (String jid in jids)
                    {
                        TreeNode node = new TreeNode(jid);
                        ServerResult result = new ServerResult();
                        result.dataType = "7";
                        node.Tag = result;
                        try
                        {
                            formMain.treeView1.SelectedNode.Nodes.Remove(node);
                        }
                        catch { }
                        formMain.treeView1.SelectedNode.Nodes.Add(node);
                    }
                    //this.treeView1.SelectedNode.Expand();
                    break;
                case "SMART FOLDER: Custom Selection":
                    if (formMain.customSelection.Count > 0)
                    {
                        SolrQuery query2 = new SolrQuery(formMain.ip, formMain);
                        XmlDocument doc = query2.customSelectionRequest(formMain.customSelection, core, cvowner, formMain.start);
                        List<XmlDocument> docs = new List<XmlDocument>();
                        docs.Add(doc);
                        XMLParser parser = new XMLParser(docs);
                        List<ServerResult> mails = parser.results;
                        formMain.currentPage = mails;
                        formMain.lastPage = (parser.numFound / formMain.pageSize);
                        formMain.writeMailsInList(mails);
                        if (formMain.listView1.Items.Count == 0)
                        {
                            formMain.nextButton.Visible = false;
                            formMain.previousButton.Visible = false;
                            formMain.customPageBox.Visible = false;
                            formMain.label1.Visible = false;
                            formMain.lastPageButton.Visible = false;
                        }
                        else
                        {
                            formMain.nextButton.Visible = true;
                            formMain.previousButton.Visible = true;
                            formMain.customPageBox.Visible = true;
                            formMain.label1.Visible = true;
                            formMain.lastPageButton.Visible = true;
                        }
                        return;
                    }
                    else
                    {
                        MessageBox.Show("You have not set up a selection.");
                    }
                    break;
            }
            SolrQuery query = new SolrQuery(formMain.ip, formMain);
            List<XmlDocument> list = new List<XmlDocument>();
            list.Add(query.getSmartFolderMessages(formMain.start, field, core, cvowner));
            XMLParser newParse = new XMLParser(list);
            List<ServerResult> results = newParse.results;
            formMain.lastPage = (newParse.numFound / formMain.pageSize);
            formMain.currentPage = results;
            formMain.writeMailsInList(results);
            if (formMain.listView1.Items.Count == 0)
            {
                formMain.nextButton.Visible = false;
                formMain.previousButton.Visible = false;
                formMain.customPageBox.Visible = false;
                formMain.label1.Visible = false;
                formMain.lastPageButton.Visible = false;
            }
            else
            {
                formMain.nextButton.Visible = true;
                formMain.previousButton.Visible = true;
                formMain.customPageBox.Visible = true;
                formMain.label1.Visible = true;
                formMain.lastPageButton.Visible = true;
            }
        }

        public void smartFolderChildClick()
        {
            ServerResult mailboxRes = (ServerResult)formMain.treeView1.SelectedNode.Parent.Parent.Tag;
            String cvowner = mailboxRes.cvowner;
            String core = formMain.treeView1.SelectedNode.Parent.Parent.Parent.Text;
            SolrQuery query = new SolrQuery(formMain.ip, formMain);
            String jid1 = formMain.treeView1.SelectedNode.Text;
            XmlDocument doc = query.getJidSmartFolderMessages(core, cvowner, jid1, formMain.start);
            List<XmlDocument> docs = new List<XmlDocument>();
            docs.Add(doc);
            XMLParser parser = new XMLParser(docs);
            List<ServerResult> results = parser.results;
            formMain.currentPage = results;
            formMain.lastPage = (parser.numFound / formMain.pageSize);
            formMain.writeMailsInList(formMain.currentPage);
            if (formMain.listView1.Items.Count == 0)
            {
                formMain.nextButton.Visible = false;
                formMain.previousButton.Visible = false;
                formMain.customPageBox.Visible = false;
                formMain.label1.Visible = false;
                formMain.lastPageButton.Visible = false;
            }
            else
            {
                formMain.nextButton.Visible = true;
                formMain.previousButton.Visible = true;
                formMain.customPageBox.Visible = true;
                formMain.label1.Visible = true;
                formMain.lastPageButton.Visible = true;
            }
        }
    }
}
