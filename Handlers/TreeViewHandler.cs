using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonitorFolderActivity;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;

namespace ExchangeOnePassIdxGUI
{
    public class TreeViewHandler
    {
        frmMain formMain;

        public TreeViewHandler(frmMain formMain)
        {
            this.formMain = formMain;
        }

        public void afterSelect()
        {
            String mbxGUID = "";
            formMain.start = 0;

            if (formMain.treeView1.SelectedNode.Tag == null)
            {
                return;
            }

            String parentguid = "";
            ServerResult myres = (ServerResult)formMain.treeView1.SelectedNode.Tag;
            parentguid = myres.documentId;
            if (myres.cvStub == null)
            {
                mbxGUID = "";
            }
            else
            {
                mbxGUID = myres.cvowner;
            }
            formMain.selectedItemGuid = parentguid;

            if (myres.dataType.Equals("5")) //if true, the selected node is a mailbox
            {
                formMain.listView1.Items.Clear();
                formMain.currentCore = formMain.treeView1.SelectedNode.Parent.Text;
                formMain.start = 0;

                SolrQuery newQuery = new SolrQuery(formMain.ip, parentguid, formMain, mbxGUID);
                XMLParser newParser = new XMLParser(newQuery.data);

                formMain.AppendTextBox("Query took" + newParser.timeForQuery + " ms");

                var subfolders = newParser.results.OrderBy(n => n.subject);

                foreach (ServerResult subfolder in subfolders)
                    formMain.serverResults.Add(subfolder);

                formMain.treeView1.SelectedNode.Nodes.Clear();

                foreach (ServerResult result in subfolders)
                {
                    TreeNode node = new TreeNode(result.subject);
                    node.Tag = result;
                    formMain.treeView1.SelectedNode.Nodes.Add(node);
                }
                //treeView1.SelectedNode.Expand();

                if (formMain.settings[3])
                {
                    formMain.addSmartFolders();
                }
            }
            else if (myres.dataType.Equals("6")) //the selected node is a smart folder
            {
                formMain.treeView1.SelectedNode.Nodes.Clear();
                formMain.smartFolderHandler.smartFolderClick();
            }
            else if (myres.dataType.Equals("7"))//the selected node is the child of a smart folder
            {
                formMain.smartFolderHandler.smartFolderChildClick();
            }
            else //the selected node is a subfolder
            {
                formMain.AppendTextBox("Now displaying page " + (formMain.start + 1).ToString());

                formMain.treeView1.SelectedNode.Nodes.Clear();

                if (((ServerResult)formMain.treeView1.SelectedNode.Tag).cvStub == null)
                {
                    mbxGUID = "";
                }
                SolrQuery newQuery = new SolrQuery(formMain.ip, 0, formMain, mbxGUID);
                if (newQuery.pagesNeeded(mbxGUID, parentguid) > 1)
                {/*kevin
                    txtActivity.Height = 75;
                    txtActivity.SelectionStart = txtActivity.Text.Length;
                    txtActivity.ScrollToCaret();*/

                    LinkLabel previousPage = new LinkLabel();
                    previousPage.Height = 15;
                    previousPage.Text = "Previous Page";
                    previousPage.Width = 30;
                    previousPage.Location = new Point(275, 639);
                    previousPage.Click += new System.EventHandler(formMain.previousPageLink);
                    formMain.Controls.Add(previousPage);
                    previousPage.Show();

                    LinkLabel nextPage = new LinkLabel();
                    nextPage.Height = 15;
                    nextPage.Text = "Next Page";
                    nextPage.Location = new Point(310, 639);
                    nextPage.Width = 30;
                    nextPage.Click += new System.EventHandler(formMain.nextPageLink);
                    formMain.Controls.Add(nextPage);
                    nextPage.Show();
                }
                List<ServerResult> subfolders = new List<ServerResult>();
                try
                {
                    List<XmlDocument> results;
                    if (((ServerResult)formMain.treeView1.SelectedNode.Tag).cvStub == null)
                    {
                        results = newQuery.getSubfoldersForJournalMBX(parentguid);
                    }
                    else
                    {
                        results = newQuery.getSubfoldersForOneMailbox(mbxGUID, parentguid);
                    }
                    XMLParser parser = new XMLParser(results);
                    formMain.AppendTextBox("Query took" + parser.timeForQuery + " ms");
                    subfolders = parser.results;
                }
                catch
                {
                    //List<ServerResult> subfolders = new List<ServerResult>();
                }
                //if so ^ add subnodes

                if (subfolders == null)
                    return;

                var subfolders1 = subfolders.OrderBy(n => n.subject);

                if (subfolders.Count > 0)
                {
                    foreach (ServerResult subfolder in subfolders1)
                    {
                        formMain.serverResults.Add(subfolder);
                        if (subfolder.links.IndexOf(subfolder.subject + "\\") > -1)
                        {
                            TreeNode node = new TreeNode(subfolder.links.Substring(subfolder.links.IndexOf("\\" + subfolder.subject + "\\") + subfolder.subject.Length + 2));
                            node.Tag = subfolder;
                            formMain.treeView1.SelectedNode.Nodes.Add(node);
                        }
                        else
                        {
                            TreeNode node = new TreeNode(subfolder.subject);
                            node.Tag = subfolder;
                            formMain.treeView1.SelectedNode.Nodes.Add(node);
                        }
                    }
                }
                //check if the selected subfolder has any datatype2 children
                XMLParser parser1 = new XMLParser(newQuery.getMailsForOneSubfolder(mbxGUID, formMain.selectedItemGuid, formMain.start, formMain.jid, formMain.mailSort));
                List<ServerResult> mails = parser1.results;
                formMain.lastPage = (parser1.numFound / formMain.pageSize);
                formMain.currentPage = mails;
                formMain.listView1.Items.Clear();
                if (mails != null)
                {
                    formMain.writeMailsInList(mails);
                }
            }

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

        public void moveJournalMbx()
        {
            TreeNode journalNode = formMain.treeView1.Nodes[0].FirstNode;
            if (journalNode.Text.Equals("journalmbx"))
            {
                formMain.treeView1.Nodes[0].Nodes.Remove(journalNode);
                formMain.treeView1.Nodes.Add(journalNode);
            }
        }

        public void writeResults(List<ServerResult> list)
        {
            if (list == null)
                return;

            List<TreeNode> coreList = new List<TreeNode>();
            List<TreeNode> mailboxList = new List<TreeNode>();

            foreach (ServerResult result in list)
            {
                List<TreeNode> subNodeMbx = new List<TreeNode>();

                if (result.dataType.Equals("5"))
                {
                    List<TreeNode> subNodes = new List<TreeNode>();
                    foreach (ServerResult result1 in list)
                    {
                        if ((result1.dataType.Equals("4")) && (result1.parentGuid.ToUpper().Equals(result.documentId.ToUpper())))
                        {
                            if (formMain.settings[0])
                            {
                                List<ServerResult> mails = formMain.getMailsOnly(formMain.serverResults);
                                int x = 0;
                                foreach (ServerResult mail in mails)
                                {
                                    if (result1.subject != null)
                                    {
                                        if (result1.subject.ToUpper().Equals(mail.folder.ToUpper()))
                                        {
                                            if (result1.mailbox.ToUpper().Equals(mail.mailbox.ToUpper()))
                                            {
                                                x++;
                                            }
                                        }
                                    }
                                }
                                if (x > 0)
                                {
                                    subNodes.Add(new TreeNode(result1.subject + " " + x));
                                }
                                else
                                {
                                    subNodes.Add(new TreeNode(result1.subject));
                                }
                            }
                            else
                            {
                                subNodes.Add(new TreeNode(result1.subject));
                            }
                        }
                    }
                    for (int x = 0; x < subNodes.Count; x++)
                    {
                        int index = x;
                        for (int y = x + 1; y < subNodes.Count; y++)
                        {
                            if (subNodes[y].Text.ToUpper().CompareTo(subNodes[index].Text.ToUpper()) < 0)
                            {
                                index = y;
                            }
                            TreeNode temp = new TreeNode();
                            temp = subNodes[x];
                            subNodes[x] = subNodes[index];
                            subNodes[index] = temp;
                        }
                    }
                    TreeNode[] nodeArray = subNodes.ToArray();
                    TreeNode treeNode = new TreeNode(result.mailbox, nodeArray);
                    treeNode.Tag = result;
                    mailboxList.Add(treeNode);

                    Console.WriteLine("test");
                    //turn mailboxList into node array
                    //save that array as child array in corelist
                }
            }

            //sort
            for (int x = 0; x < mailboxList.Count; x++)
            {
                int index = x;
                for (int y = x + 1; y < mailboxList.Count; y++)
                {
                    if (mailboxList[y].Text.ToUpper().CompareTo(mailboxList[index].Text.ToUpper()) < 0)
                    {
                        index = y;
                    }
                }
                TreeNode temp = new TreeNode();
                temp = mailboxList[x];
                mailboxList[x] = mailboxList[index];
                mailboxList[index] = temp;
            }
            //end sort


            //////START KEVIN
            TreeNode[] mailboxArray = mailboxList.ToArray();
            List<Tuple<string, TreeNode>> cmpltMbx = new List<Tuple<string, TreeNode>>();

            foreach (TreeNode node in mailboxArray)
            {
                ServerResult result = (ServerResult)node.Tag;
                string cvowner = result.cvowner;
                if (result.cvStub == null)
                {
                    cvowner = "";
                }
                Tuple<string, TreeNode> newTuple = new Tuple<string, TreeNode>(SolrQuery.getCoreName(cvowner), node);
                cmpltMbx.Add(newTuple);
            }

            foreach (Tuple<string, TreeNode> mbxData in cmpltMbx)
            {
                string coreName = mbxData.Item1;
                TreeNode mbxTreeNode = mbxData.Item2;

                bool found = false;

                foreach (TreeNode coreNode in coreList)
                {
                    if (coreNode.Text.Equals(coreName))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    coreList.Add(new TreeNode(coreName));
                }

            }

            List<TreeNode> topLevelCores = new List<TreeNode>();

            foreach (TreeNode coreNode in coreList)
            {
                List<TreeNode> mbxsInCurrentCore = new List<TreeNode>();

                foreach (Tuple<string, TreeNode> mbxData in cmpltMbx)
                {
                    if (mbxData.Item1.Equals(coreNode.Text))
                    {
                        mbxsInCurrentCore.Add(mbxData.Item2);
                    }
                }

                TreeNode[] mbxsincore = mbxsInCurrentCore.ToArray();
                topLevelCores.Add(new TreeNode(coreNode.Text, mbxsincore));
            }

            formMain.treeView1.ImageList = formMain.imageList1;

            //sort
            for (int x = 0; x < topLevelCores.Count; x++)
            {
                int index = x;
                for (int y = x + 1; y < topLevelCores.Count; y++)
                {
                    if (topLevelCores[y].Text.ToUpper().CompareTo(topLevelCores[index].Text.ToUpper()) < 0)
                    {
                        index = y;
                    }
                }
                TreeNode temp = new TreeNode();
                temp = topLevelCores[x];
                topLevelCores[x] = topLevelCores[index];
                topLevelCores[index] = temp;
            }
            //endsort

            //////END KEVIN


            TreeNodeCollection nodes = null;

            if (formMain.treeView1.SelectedNode == null)
            {
                nodes = formMain.treeView1.Nodes;
                formMain.Invoke(new Action(() => formMain.treeView1.Nodes.Clear()));
            }
            else
            {
                nodes = formMain.treeView1.Nodes;
                nodes.Clear();
            }

            /*
            foreach (TreeNode node in mailboxList)
                treeView1.Nodes.Add(node);

            foreach (TreeNode node in mailboxList)
                mailboxes.Add(node.Text);*/

            TreeNode topNode = new TreeNode("usermbx", topLevelCores.ToArray());

            /**
            foreach (TreeNode node in topLevelCores)
                treeView1.Nodes.Add(node);**/

            formMain.treeView1.Nodes.Add(topNode);
        }
    }
}
