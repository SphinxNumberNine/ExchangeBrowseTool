using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Reflection;
using System.Security.Cryptography;
using System.Xml;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Linq;
using System.Runtime.Serialization.Json;
using ExchangeOnePassIdxGUI;
using System.Drawing;
using System.Threading;
using System.Xml.Linq;
//using System.Web.UI.WebControls;

namespace MonitorFolderActivity {
    //main structure containing all the fields from the SOLR
    public struct ServerResult { //these values are stored in node.Tag as ServerResult objects

        //displayed
        public string folder;
        public string mailbox;
        public string subject;
        public string fileName;
        public string sentTo;
        public string sentFrom;
        public string sentCC;
        public string size;
        public string modifiedTime;
        public string AFID;
        public string AFOF;
        public string documentId;
        public string parentGuid;
        public string dataType;

        //details menu
        public string ccn;
        public string clid;
        public string appGuid;
        public string atyp;
        public string jid;
        public string bktm;
        public string cvbkpendtm;
        public string cvowner;
        public string objectEntryId;
        public string msgClass;
        public string fmsmtp;
        public string tosmtp;
        public string version;
        public string indexedAt;
        public string cvStub;
        public string cvExChid;
        public string apid;
        public string mtm;
        public string links;
        public string cistate;
        public string visibility;

        //others
        public string ccsmtp;
        public string hasAttach;
        public string atts;
    }

    public struct SegmentInfo
    {
        public int numSegments;
        public String lastMergeDate;
        public int numberOfDocuments;
    }

    public struct ContentIndexedInfo
    {
        public int numberOfDocuments;

        //For core level reports and Mailbox Level Report

        public int totalBackupCount;
        public int totalToBeContentIndexed;
        public int totalCISuccess;
        public int totalCIFailed;

        //For Mailbox level report ONLY

        public int lastBackupJobId;
        public int lastContentIndexedJobId;
    }

    public struct PerformanceInfo
    {
        public String operationName;
        public int count;
        public long timeTaken;
        public long avgTimeTaken;
    }

    public partial class frmMain : Form {
        //String ip = "172.19.96.107";
        public int lastPage = -1;
        public String ip = "127.0.0.1";
        public String subclient;
        public String user;
        public String mailSort = "msgmodifiedtime";
        public List<ServerResult> serverResults = new List<ServerResult>();
        public List<ServerResult> currentPage = new List<ServerResult>();
        public Boolean[] settings = new Boolean[4];
        public static Boolean editMode = false;
        public int debugLevel = DEBUG_NORMAL;
        public int pageSize = 1000;
        public bool jidFilter = false;
        public String mailboxFilter = "";
        public String jid;
        public String updatePasscode = "indexing";
        public String selectedItemGuid;
        public int pageSelected;
        public String currentCore = "";
        public int start = 0;
        public List<String> mailboxes = new List<String>();
        public List<String> customSelection = new List<string>();
        public String filePath = Application.StartupPath + @"/log.txt";
        public static StreamWriter logFile;
        private Profiling profiling;
        public bool superUser;
        private string documentId = null;
        public LogInfo userCreds;
        public string webServerName;
        public string token;
        public ExchangeOnePassIdxGUI.PageHandler pageHandler;
        public ExchangeOnePassIdxGUI.SmartFolderHandler smartFolderHandler;
        public ExchangeOnePassIdxGUI.TreeViewHandler treeViewHandler;

        public const string DATATYPE_EMAIL = "2";
        public const string DATATYPE_FOLDER = "4";
        public const string DATATYPE_MAILBOX = "5";
        public const string DATATYPE_SMARTFOLDER = "6";
        public const string DATATYPE_SMARTFOLDER_CHILD = "7";

        public const int CI_STATUS_TOBE = 0;
        public const int CI_STATUS_SUCCESS = 1;
        public const int CI_STATUS_FAILED = 2;
        public const int CI_STATUS_SKIPPED = 3;

        public const int DEBUG_NORMAL = 0;
        public const int DEBUG_REQUEST = 1;
        public const int DEBUG_RESPONSE = 2;
        public const int DEBUG_HIGH = 3;

        public frmMain() {
            
            //HttpWebResponse oResponse = isSolrValid(webServerUrl);
            InitializeComponent();

            this.treeView1.HideSelection = false;
            this.treeView1.AllowDrop = true;

            // Set drag/drop event listeners
            this.listView1.MouseDown += new MouseEventHandler(listView1_MouseDown);
            this.listView1.MouseMove += new MouseEventHandler(listView1_MouseMove);
            this.listView1.DragOver += new DragEventHandler(listView1_DragOver);
            this.treeView1.DragEnter += new DragEventHandler(treeView1_DragEnter);
            this.treeView1.DragDrop += new DragEventHandler(treeView1_DragDrop);
            this.treeView1.ItemDrag += new ItemDragEventHandler(treeView1_ItemDrag);
            try
            {
                if (File.Exists(filePath))
                {
                    logFile = new StreamWriter(filePath, true);
                    logFile.AutoFlush = true; //autoflush so we get immediate logging in case of crash
                }
                else
                {
                    logFile = new StreamWriter(filePath, true);
                    logFile.AutoFlush = true; //autoflush so we get immediate logging in case of crash
                }
            }
            catch (System.IO.IOException)
            {
            }
            for (int x = 0; x < settings.Length; x++)
                settings[x] = false;

            currentMachineLabel.Text = getMachineNameFromIP(ip);

            // Initialize background thread
            profiling = new Profiling(this, ip);

            txtActivity.TextChanged += (sender, e) => {
                if (txtActivity.Visible) {
                    txtActivity.SelectionStart = txtActivity.TextLength;
                    txtActivity.ScrollToCaret();
                }
            };
            pageHandler = new PageHandler(this);
            smartFolderHandler = new SmartFolderHandler(this);
            treeViewHandler = new TreeViewHandler(this);
        }
            
        //kevin
        // Handle the user moving a directory's path through drag and drop
        private void treeView1_DragDrop(object sender, DragEventArgs e) {
            // Notify user that something is happening
            Cursor.Current = Cursors.WaitCursor;

            //Handle drag/drop for tree view items (folders)
            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false)) {
                Point pt = ((TreeView) sender).PointToClient(new Point(e.X, e.Y));
                TreeNode destinationNode = ((TreeView)sender).GetNodeAt(pt) ?? new TreeNode();
                TreeNode newNode = (TreeNode) e.Data.GetData("System.Windows.Forms.TreeNode");

                if (newNode.Tag == null) {
                    return; //trying to move to a new core, block action.
                }

                ServerResult newNodeResult = (ServerResult)newNode.Tag;
                TreeNode tempNode = destinationNode;

                if (destinationNode.Tag == null) {
                    return; //trying to move to a new core, block action.
                }

                ServerResult destinationNodeResults = (ServerResult)destinationNode.Tag;

                // prevent moving to a child
                while (tempNode.Tag != null) {
                    tempNode = tempNode.Parent;

                    if (tempNode.Tag != null) {
                        ServerResult tempResult = (ServerResult)tempNode.Tag;

                        if (tempResult.documentId == newNodeResult.documentId) {
                            MessageBox.Show("Warning! Unable to move folder to a child folder!");
                            return;
                        }
                    }
                }


                //block moving to another view, moving a mailbox, moving to the middle of nowhere in the treeview, or moving to different core
                if ((destinationNode.TreeView == newNode.TreeView) && !(newNodeResult.dataType.Equals(DATATYPE_MAILBOX)) && (destinationNode.Parent != null)) {
                    ServerResult destNodeResult = (ServerResult)destinationNode.Tag;

                    //move node
                    destinationNode.Nodes.Add((TreeNode) newNode.Clone());
                    destinationNode.Expand();
                    newNode.Remove();

                    //update Solr db
                    if (newNodeResult.documentId != destNodeResult.documentId) {
                        updateSolrParent(newNodeResult.documentId, destNodeResult.documentId, newNodeResult.cvowner);
                    }

                    //update view 
                    Cursor.Current = Cursors.Default;
                    treeView1_AfterSelect(null, null);

                    //finish
                    return;
                } else { // prevent user from dragging mailbox into another mailbox or dragging into anywhere but the treeview
                    return;
                }
            }

            // Handle drag/drop for list view items (emails)
            TreeNode nodeToDropIn = this.treeView1.GetNodeAt(this.treeView1.PointToClient(new Point(e.X, e.Y)));
            if (nodeToDropIn == null) { return; }

            // update Solr db
            if (nodeToDropIn.Tag == null)
                return; //prevent moving mail to different core

            ServerResult nodeToDropInResult = (ServerResult) nodeToDropIn.Tag;
            updateSolrParent(documentId, nodeToDropInResult.documentId, nodeToDropInResult.cvowner);

            // update view
            Cursor.Current = Cursors.Default;
            treeView1_AfterSelect(null, null);
        }

        //kevin
        // show effects when dragging an email
        private void listView1_DragOver(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.Move;
        }

        //kevin
        // show effects when dragging an email into treeview
        private void treeView1_DragEnter(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.Move;
        }

        //kevin
        // show effects when dragging a folder
        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e) {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        // store a public variable to access original location of email in MouseMove and MouseDown
        Point down;

        //kevin
        // store data when user clicks an email
        private void listView1_MouseDown(object sender, MouseEventArgs e) {
            down = new Point(e.X, e.Y);
        }

        //kevin
        // only perform the action when the user moves the mouse more than
        // 5 pixels. This prevents unwanted drag and drops.
        private void listView1_MouseMove(object sender, MouseEventArgs e) {
            Point current = new Point(e.X, e.Y);
            ListViewItem mSender = this.listView1.GetItemAt(down.X, down.Y);

            if (mSender != null) {
                try {
                    if (Math.Abs(current.X - down.X) >= 5 /*5 pixels*/ || Math.Abs(current.Y - down.Y) >= 5) {
                        documentId = mSender.SubItems[9].Text;
                        this.listView1.DoDragDrop(this.listView1.SelectedItems, DragDropEffects.Move);
                        current = down = new Point();
                    }
                } catch {
                    //user didn't select an email
                }
            }
        }

        //kevin
        // write new line to text box (txtActivity)
        public void AppendTextBox(string value) {
            try {
                if (InvokeRequired) {
                    this.Invoke(new Action<string>(AppendTextBox), new object[] { value });
                    return;
                }
            } catch { }

            txtActivity.Text += value;
            txtActivity.Text += Environment.NewLine;
        }

        //kevin
        // update textbox with information regarding update information
        private void updateSolrParent(string senderId, string targetId, string cvOwner) {
            SolrQuery solrQuery = new SolrQuery(ip, this);

            string resultData = solrQuery.updateParent(senderId, targetId, cvOwner);

            AppendTextBox(resultData);
        }

        public void logToFile(string content, int level) {
            if (level <= debugLevel) {
                try {
                    logFile.WriteLine(content);
                } catch (Exception) {

                }
            }
        }

        public void logToFileAndDisplay(string content)
        {
            try
            {
                logFile.WriteLine(content);
            }
            catch (Exception)
            {

            }

            AppendTextBox(content);

        }

        

        //Checks which node is selected. If mailbox, it gets the subfolders. If subfolder, it checks for any mails and adds them, and then checks for subfolders under that folder.
        public void treeView1_AfterSelect(object sender, TreeViewEventArgs e) 
        {
            treeViewHandler.afterSelect();
        }

        private void serverConfiguration(object sender, EventArgs e)
        {
            StandardInputForm inputBox = new StandardInputForm();
            inputBox.Description.Text = "Please give the IP of your desired SOLR server. Ex: 172.25.30.111";
            inputBox.Text = "Server Configuration";
            if (!File.Exists("SOLRServers.txt"))
            {
                var temp = File.Create("SOLRServers.txt");
                temp.Close();
            }
            String[] file1 = File.ReadAllLines("SOLRServers.txt");
            //bool checker = false;
            foreach (string ip in file1)
            {
                inputBox.searchBox.Items.Add(ip);
                if (inputBox.searchBox.Text == ip)
                {
                    //checker = true;
                }
            }
            inputBox.ShowDialog();
            if (inputBox.DialogResult == DialogResult.OK)
            {
                ip = inputBox.searchBox.Text;
                Console.WriteLine(ip);
            }
            //ip = Microsoft.VisualBasic.Interaction.InputBox("Please give the IP of your desired SOLR server. Ex: 172.25.30.111", "Server Configuration", "172.24.48.177");
            bool checkIP;
            IPAddress address;
            if (IPAddress.TryParse(ip, out address))
            {
                checkIP = true;
            }
            else
            {
                checkIP = false;
            }
            if (ip.Length < 7)
            {
                checkIP = false;
            }
            if (checkIP)
            {
                profiling.setIp(ip);
                StreamWriter writer = new StreamWriter("SOLRServers.txt");
                writer.WriteLine(ip);
                writer.Close();
            }
            
        }

        //poplutates the listview with mails
        public void writeMailsInList(List<ServerResult> mails)
        {
            try
            {
                txtActivity.Height = 90;
                //AppendTextBox("Now displaying page " + (start + 1).ToString());
                this.listView1.Items.Clear();
                this.listView1.SmallImageList = this.imageList1;
                TreeNode node = this.treeView1.SelectedNode;
                String mailbox = node.Parent.Text;
                String folder = node.Text;
                List<ServerResult> list = getMailsOnly(serverResults);
                List<ListViewItem> listViewItems = new List<ListViewItem>();
                foreach (ServerResult result in mails)
                {
                    String[] items = new String[13];
                    items[0] = result.fileName;
                    items[1] = result.subject;
                    items[2] = result.sentFrom;
                    items[3] = result.sentTo;
                    items[4] = result.sentCC;
                    items[5] = result.size;
                    items[6] = result.modifiedTime;
                    items[7] = result.AFID;
                    items[8] = result.AFOF;
                    items[9] = result.documentId;
                    items[10] = result.parentGuid;
                    items[11] = result.jid;
                    items[12] = result.apid;
                    ListViewItem newListItem = new ListViewItem(items);
                    newListItem.ImageIndex = 1;
                    this.listView1.Items.Add(newListItem);
                }
            }
            catch { }
        }

        public List<ServerResult> getMailsOnly(List<ServerResult> list)
        {
            List<ServerResult> mails = new List<ServerResult>();
            foreach (ServerResult x in list)
            {
                if (x.dataType.Equals(DATATYPE_EMAIL))
                {
                    mails.Add(x);
                }
            }
            return mails;
        }

        //refreshes treeview with any new folders/nodes
        private void writeResultsInTreeView(List<ServerResult> list)
        {
            treeViewHandler.writeResults(list);
        }
        
        private void search_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            //Thread searchThread = new Thread(delegate() {
                SolrQuery searchAll = new SolrQuery(ip, this);
                XMLParser parseSearch = new XMLParser(searchAll.datatype5);
                AppendTextBox("Query took" + parseSearch.timeForQuery + " ms");
                List<ServerResult> results2 = parseSearch.results;
                serverResults = results2;
                writeResultsInTreeView(results2);
                try
                {
                    moveJournalmbx();
                }
                catch { }

                Cursor.Current = Cursors.Default;
            //});

            //searchThread.IsBackground = true;
            //searchThread.Start();
        }

        private void delete_Click(object sender, EventArgs e)
        {
            ////////NOTE: CAN USE CUSTOM QUERY INSTEAD

            String deleteCode = Microsoft.VisualBasic.Interaction.InputBox("Please enter your authentication code.", "Deletion Code Required");
            deleteCode = deleteCode.ToLower();
            if (deleteCode.Equals(updatePasscode.ToLower()))
            {
                SolrQuery deleteQuery = new SolrQuery(ip, this);
                deleteQuery.delete(this.listView1.SelectedItems[0].SubItems[9].Text);
                this.listView1.Items.Remove(this.listView1.SelectedItems[0]);
            }
        }

        private void credentials_Click(object sender, EventArgs e)
        {
            DoubleInputForm form = new DoubleInputForm();
            form.ShowDialog();
            user = form.getUser();
            subclient = form.getSubclient();
        }

        private void pages_Click(object sender, EventArgs e)
        {

        }

        private void update_Click(object sender, EventArgs e)
        {
            EditForm inputBox = new EditForm();
            inputBox.Description.Text = "Please give your authentication code";
            inputBox.searchBox.UseSystemPasswordChar = true;
            inputBox.ShowDialog();
            String deleteCode = "";
            if (inputBox.DialogResult == DialogResult.OK) {
                deleteCode = inputBox.searchBox.Text;
            }

            deleteCode = deleteCode.ToLower();

            if (deleteCode.Equals(updatePasscode.ToLower())) {
                editMode = !editMode;

                if (editMode) {
                    editModeLabel.Text = "EDIT MODE";
                    this.updateEditToolStripMenuItem.Text = "Turn off Edit Mode";
                    this.listView1.ContextMenuStrip.Items[0].Text = "Update";
                    this.fullReconstructionToolStripMenuItem.Visible = true;
                } else {
                    editModeLabel.Text = "";
                    this.updateEditToolStripMenuItem.Text = "Update / Edit";
                    this.listView1.ContextMenuStrip.Items[0].Text = "Details";
                    this.fullReconstructionToolStripMenuItem.Visible = false;
                }

            }
        }

        private void menu_Click(object sender, EventArgs e)
        {

        }

        private void mailboxFilter_Click(object sender, EventArgs e)
        {
            StandardInputForm inputBox = new StandardInputForm();
            inputBox.Description.Text = "Enter a keyword for the mailbox filter";
            List<String> topFolders = new List<string>();
            inputBox.searchBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            inputBox.searchBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            foreach (TreeNode node in this.treeView1.Nodes) { //root node
                foreach (TreeNode childNode in node.Nodes) { //cores
                    foreach (TreeNode grandChildNode in childNode.Nodes) {
                        if (grandChildNode.Tag != null) {
                            ServerResult tempNodeResults = (ServerResult)grandChildNode.Tag;
                            if (tempNodeResults.dataType.Equals(DATATYPE_MAILBOX)) {
                                topFolders.Add(grandChildNode.Text);
                            }
                        }
                    }
                }
            }

            AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
            collection.AddRange(topFolders.ToArray());
            inputBox.searchBox.AutoCompleteCustomSource = collection;
            inputBox.ShowDialog();

            if (inputBox.DialogResult == DialogResult.OK)
                mailboxFilter = inputBox.searchBox.Text;
            
            List<TreeNode> mailboxList = new List<TreeNode>();
            TreeNodeCollection nodes = null;

            if (this.treeView1.SelectedNode == null) {
                nodes = this.treeView1.Nodes;
                Invoke(new Action(() => treeView1.Nodes.Clear()));
            } else {
                nodes = this.treeView1.Nodes;
                nodes.Clear();
            }

            foreach (ServerResult result in serverResults) {
                if (result.dataType.Equals(DATATYPE_MAILBOX))
                {
                    //////
                    if (result.mailbox.ToUpper().Contains(mailboxFilter.ToUpper()))
                    {
                        List<TreeNode> subNodes = new List<TreeNode>();
                        foreach (ServerResult result1 in serverResults)
                        {
                            if ((result1.dataType.Equals(DATATYPE_FOLDER)) && (result1.parentGuid.ToUpper().Equals(result.documentId.ToUpper())))
                            {
                                if (settings[0])
                                {
                                    List<ServerResult> mails = getMailsOnly(serverResults);
                                    int x = 0;
                                    foreach (ServerResult mail in mails)
                                    {
                                        if (result.folder.ToUpper().Equals(mail.folder.ToUpper()))
                                        {
                                            if (result.mailbox.ToUpper().Equals(mail.mailbox.ToUpper()))
                                            {
                                                x++;
                                            }
                                        }
                                    }
                                    if (x > 0)
                                    {
                                        subNodes.Add(new TreeNode(result1.subject + " - " + x));
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
                    }
                }
            }

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


            foreach (TreeNode node in mailboxList)
                treeView1.Nodes.Add(node);
        }

        private void options_Click(object sender, EventArgs e)
        {
            OptionsMenu options = new OptionsMenu(settings, debugLevel, pageSize);
            if (options.ShowDialog() == DialogResult.OK)
            {
                settings = options.settings;
                debugLevel = options.debuggerLevel;
                pageSize = options.pageSize;

                search_Click(searchToolStripMenuItem, new EventArgs());
                listView1.Items.Clear();
            }
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            if (editMode)
            {
                String updateChange = Microsoft.VisualBasic.Interaction.InputBox("Please enter the new value for the " + this.listView1.SelectedItems[0].SubItems[1].Text + "field", "Change " + this.listView1.SelectedItems[0].Text);
            }
        }

        private void details_Click(object sender, EventArgs e)
        {
            try
            {
                ServerResult selected = new ServerResult();
                foreach (ServerResult x in currentPage)
                {
                    if (this.listView1.SelectedItems[0].SubItems[9].Text.Equals(x.documentId)) //SubItems[] is an array of all the good data
                    {
                        selected = x;
                        break;
                    }
                }
                DetailsMenu details = new DetailsMenu(selected);
                details.Size = new System.Drawing.Size(798, 342);
                details.submitButton.Visible = false;
                if (editMode)
                {
                    details.submitButton.Visible = true;
                    foreach (Control x in details.Controls)
                    {
                        if (x is TextBox)
                        {
                            (x as TextBox).ReadOnly = false;
                        }
                    }
                    details.guidBox.ReadOnly = true;
                    details.parentGuidBox.ReadOnly = true;
                    details.folderBox.ReadOnly = true;
                    details.sizeBox.ReadOnly = true;
                    details.versionBox.ReadOnly = true;
                    details.cvownerBox.ReadOnly = true;
                    details.datatypeBox.ReadOnly = true;
                }
                details.ShowDialog();
                if ((editMode) && (details.DialogResult == DialogResult.OK))
                {
                    ServerResult newServerResult = details.returnNewServerResult();
                    SolrQuery updateQuery = new SolrQuery(ip, 0, this);
                    updateQuery.update(newServerResult);
                    List<XmlDocument> updatedMailDocs = updateQuery.getMailsForOneSubfolder(newServerResult.parentGuid, start, mailSort);
                    XMLParser parser = new XMLParser(updatedMailDocs);
                    List<ServerResult> updatedMails = parser.results;
                    lastPage = (parser.numFound / pageSize);
                    AppendTextBox("Query took " + parser.timeForQuery + " ms");
                    writeMailsInList(updatedMails);
                }
            }
            catch { }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            details_Click(sender, e);
        }

        private string getMachineNameFromIP(string ipAdress)
        {
            string machineName = string.Empty;
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(ipAdress);
                machineName = hostEntry.HostName;
            }
            catch (Exception)
            {
                machineName = "No valid ip";
                logToFile("ERROR: Invalid ip: " + ipAdress, DEBUG_HIGH);
            }
            return machineName;
        }

        public void sendPageRequest(object sender, EventArgs e)
        {
            pageSelected = Int32.Parse((sender as LinkLabel).Text);
        }

        //kevin
        // prompts user for authentication code
        public bool requestCredentials() {
            StandardInputForm inputBox = new StandardInputForm();
            inputBox.Description.Text = "Please give your authentication code";
            inputBox.ShowDialog();

            while (true) {
                String deleteCode = "";
                
                if (inputBox.DialogResult == DialogResult.OK) {
                    deleteCode = inputBox.searchBox.Text;
                }

                deleteCode = deleteCode.ToLower();

                if (deleteCode.Equals(updatePasscode.ToLower())) {
                    editMode = true;
                    return true;
                } else if (deleteCode.Equals("")) {
                    return false;
                } else {
                    inputBox.Description.Text = "Error: Wrong password.";
                    inputBox.ShowDialog();
                }
            }
        }

        public void nextPageLink(object sender, EventArgs e) 
        {
            pageHandler.nextPage();
        }

        public void previousPageLink(object sender, EventArgs e)
        {
            pageHandler.previousPage();
        }

        public void customPageLink(object sender, KeyPressEventArgs e)
        {
            pageHandler.customPage(sender, e);
        }

        private void jobIDSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            jidFilter = !jidFilter;
            if (jidFilter)
            {
                String jobId = Microsoft.VisualBasic.Interaction.InputBox("Please enter the desired JobId", "JID Search");
                jid = jobId;
                this.jobIDSearchToolStripMenuItem.Text = "Turn Off JobID Filter";
                JidFilterLabel.Show();
                jidLabel.Text = jid;
                jidLabel.Show();
            }
            else
            {
                jid = null;
                this.jobIDSearchToolStripMenuItem.Text = "JobID Search";
                jidLabel.Text = "";
                jidLabel.Hide();
            }
        }

        //todo: fix deletion of third level subfolders and below
        private void mailboxDelete(object sender, EventArgs e)
        {
            String password = Microsoft.VisualBasic.Interaction.InputBox("Password required for deletion", "Password");
            if (password.Equals(updatePasscode))
            {
                if (this.treeView1.SelectedNode != null)
                {
                    String guid = "";
                    if (this.treeView1.SelectedNode.Parent == null)
                    {
                        foreach (ServerResult result in serverResults)
                        {
                            if (result.mailbox.Equals(this.treeView1.SelectedNode.Text))
                            {
                                guid = result.documentId;
                                break;
                            }
                        }
                        if (!(guid.Equals("")))
                        {
                            SolrQuery deleteQuery = new SolrQuery(ip, 0, this);
                            deleteQuery.deleteChildren(guid);
                            deleteQuery.delete(guid);
                            search_Click(this.button1, new System.EventArgs());
                        } 
                    }
                    else if ((this.treeView1.SelectedNode.Parent != null) && (this.treeView1.SelectedNode.Parent.Parent == null))
                    {
                        foreach (ServerResult result in serverResults)
                        {
                            if (this.treeView1.SelectedNode.Parent.Text.Equals(result.mailbox))
                            {
                                if (this.treeView1.SelectedNode.Text.Equals(result.subject))
                                {
                                    guid = result.documentId;
                                    break;
                                }
                            }
                        }
                        if (!(guid.Equals("")))
                        {
                            SolrQuery deleteQuery = new SolrQuery(ip, 0, this);
                            deleteQuery.deleteChildren(guid);
                            deleteQuery.delete(guid);
                            search_Click(this.button1, new System.EventArgs());
                        }
                    }
                    else if ((this.treeView1.SelectedNode.Parent != null) && (this.treeView1.SelectedNode.Parent.Parent != null) && (this.treeView1.SelectedNode.Parent.Parent.Parent == null))
                    {
                        String subfolder = this.treeView1.SelectedNode.Text;
                        foreach (ServerResult result in serverResults)
                        {
                            if (result.links.Contains(subfolder) && (result.subject.Equals(this.treeView1.SelectedNode.Parent.Text)) && (result.dataType.Equals(DATATYPE_FOLDER)) && (this.treeView1.SelectedNode.Parent.Parent.Equals(result.mailbox)))
                            {
                                SolrQuery deleteQuery = new SolrQuery(ip, 0, this);
                                deleteQuery.deleteChildren(result.documentId);
                                deleteQuery.delete(result.documentId);
                                search_Click(this.button1, new System.EventArgs());
                            }
                        }
                    }
                }
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            mailSort = "msgmodifiedtime";
            if (e.Button == MouseButtons.Right)
            {
                this.treeView1.SelectedNode = e.Node;
                detailsToolStripMenuItem.Visible = true;
                if ((this.treeView1.SelectedNode.Text.Equals("SMART FOLDER: Custom Selection")) && (((ServerResult)this.treeView1.SelectedNode.Tag).dataType.Equals(DATATYPE_SMARTFOLDER)))
                {
                    this.selection.Visible = true;
                }
                else
                {
                    this.selection.Visible = false;
                }
            }
        }

        //kevin
        // Handle user clicking the 'Re-Content Index All' action in the menu
        private void reContentIndexAllToolStripMenuItem_Click(object sender, EventArgs e) {
            if (!editMode) {
                if (!requestCredentials()) {
                    return;
                }
            }

            Cursor.Current = Cursors.WaitCursor;
            long initialTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            SolrQuery query = new SolrQuery(ip, this);
            if (query.recontentIndexAll(mailboxFilter)) {
                AppendTextBox("Query took: " + ((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - initialTime) + " ms");

                Console.WriteLine("Success");
                logToFile(DateTime.Now + " Successfully recontent indexed all mail", DEBUG_HIGH);
            } else {
                Console.WriteLine("Failure");
                logToFile(DateTime.Now + " Error recontent indexing all mail", DEBUG_HIGH);
            }

            Cursor.Current = Cursors.Default;
        }

        private void selection_Click(object sender, EventArgs e)
        {
            CustomSelection customSelection = new CustomSelection();
            customSelection.ShowDialog();
            if (customSelection.DialogResult == DialogResult.OK)
            {
                List<String> fieldStrings = customSelection.fieldstrings;
                this.customSelection = fieldStrings;
            }
            this.treeView1_AfterSelect(treeView1, new TreeViewEventArgs(treeView1.SelectedNode));
        }

        //kevin
        // Handle user clicking the 'Re-Content Index' menu item after they right click
        // on a node in the treeview
        private void recontentIndexToolStripMenuItem_Click(object sender, EventArgs e) {
            if (!editMode) {
                if (!requestCredentials()) {
                    return;
                }
            }

            Cursor.Current = Cursors.WaitCursor;
            //List<TreeNode> startingFolder = new List<TreeNode>();
            //startingFolder.Add(this.treeView1.SelectedNode as TreeNode);

            if (recontentIndex(this.treeView1.SelectedNode as TreeNode)) {
                Console.WriteLine("Success");
            } else {
                Console.WriteLine("Failure");
            }

            Cursor.Current = Cursors.Default;
        }

        private bool recontentIndex(TreeNode node) {
            SolrQuery query = new SolrQuery(ip, this);

            if (node.Tag == null) {
                if (node.Text.Equals("usermbx")) { //root node
                    if (query.recontentIndexAll(mailboxFilter)) {
                        logToFile("Successfully updated cistatus for all mailboxes", DEBUG_NORMAL);
                        return true;
                    } else {
                        logToFile("Error updating cistatus", DEBUG_HIGH);
                        return false;
                    }
                } else { //core node TODO HERE
                    if (query.recontentIndexCore(node.Text, mailboxFilter)) {
                        logToFile("Successfully updated cistatus for core " + node.Text, DEBUG_NORMAL);
                        return true;
                    } else {
                        logToFile("Error updating cistatus for core " + node.Text, DEBUG_HIGH);
                        return false;
                    }
                }
            }

            ServerResult result = (ServerResult) node.Tag;

            if (result.dataType == DATATYPE_MAILBOX) {
                if (query.updateCistatusMailbox(CI_STATUS_SUCCESS, result.cvowner)) {
                    logToFile("Successfully updated cistatus for cvowner:" + result.cvowner, DEBUG_NORMAL);
                    return true;
                } else {
                    logToFile("Error updating cistatus", DEBUG_HIGH);
                }
            } else if (result.dataType == DATATYPE_FOLDER) {
                if (query.updateCistatusFolder(CI_STATUS_SUCCESS, result.cvowner, result.documentId)) {
                    logToFile("Successfully updated cistatus for emails with parentguid:" + result.documentId, DEBUG_NORMAL);
                    return true;
                } else {
                    logToFile("Error updating cistatus", DEBUG_HIGH);
                }
            }

            return false;
        }

        //kevin
        // A function that loops through EVERY node in the treeview
        // REGARDLESS OF wether or not the user loaded subnodes already
        // we will temporarily load subnodes for them
        [Obsolete("This method is no longer used. Please see recontentIndex(TreeNode node)", true)]
        private bool depRecontentIndex(List<TreeNode> itemsToRCI) {
            return false;
        }

        //kevin
        // Load subnodes because they otherwise wont be loaded until the user clicks
        // on a node in the treeview
        private TreeNode updateChildrenNodes(TreeNode node, SolrQuery query) {
            ServerResult result = (ServerResult) node.Tag;
            List<ServerResult> subfolders = getSubfolders(query, result.documentId);

            if (subfolders.Count > 0) {
                foreach (ServerResult res in subfolders) {
                    TreeNode newNode = new TreeNode(res.subject);
                    newNode.Tag = res;
                    node.Nodes.Add(newNode);
                }
            }

            return node;
        }

        //kevin
        // Return a list of subfolders
        private List<ServerResult> getSubfolders(SolrQuery query, string id) {
            List<ServerResult> subfolders = new List<ServerResult>();

            List<XmlDocument> results = query.getSubfoldersForOneMailbox(id);
            XMLParser parser = new XMLParser(results);
            subfolders = parser.results;

            return subfolders;
        }

        private void detailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DetailsMenu details = new DetailsMenu((ServerResult)treeView1.SelectedNode.Tag);
                if (((ServerResult)treeView1.SelectedNode.Tag).dataType.Equals(DATATYPE_MAILBOX))
                {
                    details.folderLabel.Text = "Subfolders";
                    details.folderBox.Text = this.treeView1.SelectedNode.Nodes.Count.ToString();
                    details.ShowDialog();
                }
                else if (((ServerResult)treeView1.SelectedNode.Tag).dataType.Equals(DATATYPE_FOLDER))
                {
                    details.folderLabel.Text = "Subfolders";
                    details.folderBox.Text = this.treeView1.SelectedNode.Nodes.Count.ToString();
                    details.ShowDialog();
                }
            }
            catch
            {
                MessageBox.Show("Details are not available for this item.");
            }
        }

        private void fullReconstructionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SolrQuery reconstructionQuery = new SolrQuery(ip, 0, this);
            try
            {
                reconstructionQuery.reconstruct();
                MessageBox.Show("Reconstruction succeeded");
            }
            catch
            {
                //MessageBox.Show("Reconstruction failed. Error message: " + ex.Message);
            }
        }

        private void exportToCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.treeView1.SelectedNode != null)
            {
                if (this.treeView1.SelectedNode.Parent == null)
                {
                    String csvName = Microsoft.VisualBasic.Interaction.InputBox("Please give the name for the exported CSV file");
                    ServerResult mailbox = new ServerResult();
                    foreach (ServerResult mailboxes in serverResults)
                    {
                        if (mailboxes.mailbox.Equals(this.treeView1.SelectedNode.Text))
                        {
                            if (mailboxes.dataType.Equals(DATATYPE_MAILBOX))
                            {
                                mailbox = mailboxes;
                            }
                        }
                    }
                    SolrQuery Query = new SolrQuery(ip, 0, this);
                    List<XmlDocument> subDocs = Query.getSubfoldersForOneMailbox(mailbox.documentId);
                    XMLParser parser = new XMLParser(subDocs);
                    List<ServerResult> subfolders = parser.results;
                    List<List<ServerResult>> mails = new List<List<ServerResult>>();
                    foreach (ServerResult subfolder in subfolders)
                    {
                        List<ServerResult> oneSubResults = new XMLParser(Query.getMailsForOneSubfolder(subfolder.documentId, true, mailSort)).results;
                        mails.Add(oneSubResults);
                    }
                    StringBuilder csvContent = new StringBuilder();
                    csvContent.AppendLine("parentguid,ccn,clid,appGUID,atyp,jid,bktm,cvbkpendtm,visible,szkb,cvowner,objectEntryId,conv,hasattach,atts,msgclass,msgmodifiedtime,folder,fromdisp,fmsmtp,todisp,tosmtp,ccdisp,ccsmtp,indexedat,cvstub,cvexchid,afof,datatype,apid,mtm,contentid,lnks,cistate,afid,cvownerdisp");
                    String mailboxLine = mailbox.parentGuid + "," + mailbox.ccn + "," + mailbox.clid + "," + mailbox.appGuid + "," + mailbox.atyp + "," + mailbox.jid + "," + mailbox.bktm + "," + mailbox.cvbkpendtm + "," + mailbox.visibility + "," + mailbox.size + "," + mailbox.cvowner + "," + mailbox.objectEntryId + "," + mailbox.subject + "," + mailbox.hasAttach + "," + mailbox.atts + "," + mailbox.msgClass + "," + mailbox.modifiedTime + "," + mailbox.folder + "," + mailbox.sentFrom + "," + mailbox.fmsmtp + "," + mailbox.sentTo + "," + mailbox.tosmtp + "," + mailbox.sentCC + "," + mailbox.ccsmtp + "," + mailbox.indexedAt + "," + mailbox.cvStub + "," + mailbox.cvExChid + "," + mailbox.AFOF + "," + mailbox.dataType + "," + mailbox.apid + "," + mailbox.mtm + "," + mailbox.documentId + "," + mailbox.links + "," + mailbox.cistate + "," + mailbox.AFID + "," + mailbox.mailbox;
                    if (!(mailboxLine.Split(',').Length > 36))
                    {
                        csvContent.AppendLine(mailboxLine);
                    }
                    foreach (ServerResult subfolder in subfolders)
                    {
                        List<ServerResult> subSubFolders = new XMLParser(Query.getSubfoldersForOneMailbox(subfolder.documentId)).results;
                        if (subSubFolders.Count > 0)
                        {
                            foreach (ServerResult subSubFolder in subSubFolders)
                            {
                                String subSubFolderLine = subSubFolder.parentGuid + "," + subSubFolder.ccn + "," + subSubFolder.clid + "," + subSubFolder.appGuid + "," + subSubFolder.atyp + "," + subSubFolder.jid + "," + subSubFolder.bktm + "," + subSubFolder.cvbkpendtm + "," + subSubFolder.visibility + "," + subSubFolder.size + "," + subSubFolder.cvowner + "," + subSubFolder.objectEntryId + "," + subSubFolder.subject + "," + subSubFolder.hasAttach + "," + subSubFolder.atts + "," + subSubFolder.msgClass + "," + subSubFolder.modifiedTime + "," + subSubFolder.folder + "," + subSubFolder.sentFrom + "," + subSubFolder.fmsmtp + "," + subSubFolder.sentTo + "," + subSubFolder.tosmtp + "," + subSubFolder.sentCC + "," + subSubFolder.ccsmtp + "," + subSubFolder.indexedAt + "," + subSubFolder.cvStub + "," + subSubFolder.cvExChid + "," + subSubFolder.AFOF + "," + subSubFolder.dataType + "," + subSubFolder.apid + "," + subSubFolder.mtm + "," + subSubFolder.documentId + "," + subSubFolder.links + "," + subSubFolder.cistate + "," + subSubFolder.AFID + "," + subSubFolder.mailbox;
                                if (!(subSubFolderLine.Split(',').Length > 36))
                                {
                                    csvContent.AppendLine(subSubFolderLine);
                                }
                                try
                                {
                                    List<ServerResult> subMails = new XMLParser(new SolrQuery(ip, 0, this).getMailsForOneSubfolder(subSubFolder.documentId, true, mailSort)).results;
                                    foreach (ServerResult subMail in subMails)
                                    {
                                        ServerResult mail = subMail;
                                        mail.subject = gedRidOfQuotesAndCommas(mail.subject);
                                        mail.sentFrom = gedRidOfQuotesAndCommas(mail.sentFrom);
                                        mail.sentTo = gedRidOfQuotesAndCommas(mail.sentTo);
                                        //mail.subject = gedRidOfQuotesAndCommas(mail.sentCC);
                                        String mailLine = mail.parentGuid + "," + mail.ccn + "," + mail.clid + "," + mail.appGuid + "," + mail.atyp + "," + mail.jid + "," + mail.bktm + "," + mail.cvbkpendtm + "," + mail.visibility + "," + mail.size + "," + mail.cvowner + "," + mail.objectEntryId + "," + mail.subject + "," + mail.hasAttach + "," + mail.atts + "," + mail.msgClass + "," + mail.modifiedTime + "," + mail.folder + "," + mail.sentFrom + "," + mail.fmsmtp + "," + mail.sentTo + "," + mail.tosmtp + "," + mail.sentCC + "," + mail.ccsmtp + "," + mail.indexedAt + "," + mail.cvStub + "," + mail.cvExChid + "," + mail.AFOF + "," + mail.dataType + "," + mail.apid + "," + mail.mtm + "," + mail.documentId + "," + mail.links + "," + mail.cistate + "," + mail.AFID + "," + mail.mailbox;
                                        if (!(mailLine.Split(',').Length > 36))
                                        {
                                            csvContent.AppendLine(mailLine);
                                        }
                                    }
                                }
                                catch { }
                            }
                        }
                        String subfolderLine = subfolder.parentGuid + "," + subfolder.ccn + "," + subfolder.clid + "," + subfolder.appGuid + "," + subfolder.atyp + "," + subfolder.jid + "," + subfolder.bktm + "," + subfolder.cvbkpendtm + "," + subfolder.visibility + "," + subfolder.size + "," + subfolder.cvowner + "," + subfolder.objectEntryId + "," + subfolder.subject + "," + subfolder.hasAttach + "," + subfolder.atts + "," + subfolder.msgClass + "," + subfolder.modifiedTime + "," + subfolder.folder + "," + subfolder.sentFrom + "," + subfolder.fmsmtp + "," + subfolder.sentTo + "," + subfolder.tosmtp + "," + subfolder.sentCC + "," + subfolder.ccsmtp + "," + subfolder.indexedAt + "," + subfolder.cvStub + "," + subfolder.cvExChid + "," + subfolder.AFOF + "," + subfolder.dataType + "," + subfolder.apid + "," + subfolder.mtm + "," + subfolder.documentId + "," + subfolder.links + "," + subfolder.cistate + "," + subfolder.AFID + "," + subfolder.mailbox;
                        if (!(subfolderLine.Split(',').Length > 36))
                        {
                            csvContent.AppendLine(subfolderLine);
                        }
                        int k = subfolders.IndexOf(subfolder);
                        try
                        {
                            foreach (ServerResult mail1 in mails[k])
                            {
                                ServerResult mail = mail1;
                                mail.subject = gedRidOfQuotesAndCommas(mail.subject);
                                mail.sentFrom = gedRidOfQuotesAndCommas(mail.sentFrom);
                                mail.sentTo = gedRidOfQuotesAndCommas(mail.sentTo);
                                //mail.subject = gedRidOfQuotesAndCommas(mail.sentCC);
                                String mailLine = mail.parentGuid + "," + mail.ccn + "," + mail.clid + "," + mail.appGuid + "," + mail.atyp + "," + mail.jid + "," + mail.bktm + "," + mail.cvbkpendtm + "," + mail.visibility + "," + mail.size + "," + mail.cvowner + "," + mail.objectEntryId + "," + mail.subject + "," + mail.hasAttach + "," + mail.atts + "," + mail.msgClass + "," + mail.modifiedTime + "," + mail.folder + "," + mail.sentFrom + "," + mail.fmsmtp + "," + mail.sentTo + "," + mail.tosmtp + "," + mail.sentCC + "," + mail.ccsmtp + "," + mail.indexedAt + "," + mail.cvStub + "," + mail.cvExChid + "," + mail.AFOF + "," + mail.dataType + "," + mail.apid + "," + mail.mtm + "," + mail.documentId + "," + mail.links + "," + mail.cistate + "," + mail.AFID + "," + mail.mailbox;
                                if (!(mailLine.Split(',').Length > 36))
                                {
                                    csvContent.AppendLine(mailLine);
                                }

                            }
                        }
                        catch { }
                    }
                    String csvPath = Application.StartupPath + "//" + csvName + ".csv";
                    File.WriteAllText(csvPath, string.Empty);
                    File.AppendAllText(csvPath, csvConvert(csvContent.ToString()));
                    MessageBox.Show("Export succeeded.");
                }
                else
                {
                    MessageBox.Show("You can only export a mailbox");
                }
            }
        }

        private void importCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //put File Explorer here later
            String path = Microsoft.VisualBasic.Interaction.InputBox("Please enter the full file path of the designated CSV file, including the extension.", "File Path");
            if (File.Exists(path))
            {
                FileInfo newFileInfo = new FileInfo(path);
                if (newFileInfo.Extension.ToUpper().Equals(".CSV"))
                {
                    StreamReader reader = new StreamReader(path);
                    String line;
                    List<String> lines = new List<string>();

                    while ((line = reader.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }

                    List<String> list = new List<string>();

                    foreach (String p in lines)
                    {
                        String[] arr = p.Split(',');
                        if (!(arr.Length > 36))
                        {
                            list.Add(p);
                        }
                    }
                    
                    //File.WriteAllText(path, String.Empty);
                    reader.Close();
                    File.Delete(path);
                    File.Create(path).Close();
                    using (System.IO.StreamWriter outfile = new StreamWriter(path))
                    {
                        outfile.Write(String.Join(System.Environment.NewLine, list.ToArray()));
                    }

                    SolrQuery query = new SolrQuery(ip, 0, this);
                    String core = "usermbx";
                    using (StreamReader getContentId = new StreamReader(path))
                    {
                        getContentId.ReadLine();
                        String cLine = getContentId.ReadLine();
                        String[] cLineArr = cLine.Split(',');
                        String contentid = cLineArr[31];
                        int sum = 0;
                        foreach (char ch in contentid)
                        {
                            try
                            {
                                int value = Convert.ToInt32(ch.ToString(), 16);
                                sum += value;
                            }
                            catch { }
                        }
                        sum = sum % 8;
                        core += sum.ToString();
                    }
                    //String core = query.coreNames[index];
                    String url = "http://" + ip + ":20000/solr/" + core + "/update/csv?stream.file=" + path + "&stream.contentType=text/plain;charset=utf-8&commit=true";
                    query.add(url);
                    search_Click(button1, new System.EventArgs());
                }
                else
                {
                    MessageBox.Show("File specified is in an unreadable format");
                }
            }
            else
            {
                MessageBox.Show("Path is invalid");
            }
            
        }

        public String csvConvert(String str)
        {
            Boolean x = (str.Contains(",") || str.Contains("\"") || str.Contains("\r") || str.Contains("\n"));

            if (x)
            {
                StringBuilder newString = new StringBuilder();
                //newString.Append("\"");
                foreach (char nextChar in str)
                {
                    if (nextChar == '"')
                    {

                    }
                    newString.Append(nextChar);
                    if (nextChar == '"')
                    {
                        newString.Append("\"");
                    }
                }
                //newString.Append("\"");
                return newString.ToString();
            }

            return str;
        }

        private String gedRidOfQuotesAndCommas(String x)
        {
            if ((x.Contains('"')) || (x.Contains(',')))
            {
                String newS = "";
                foreach (char h in x)
                {
                    if ((h != '"') && (h != ','))
                    {
                        newS += h;
                    }
                }
                x = newS;
            }

            return x;
        }

        private void txtActivity_TextChanged(object sender, EventArgs e) {

        }

        private void frmMain_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            logFile.Close();
            logFile.Flush();
        }

        //kevin
        private void profilingTimerToolStripMenuItem_Click(object sender, EventArgs e) {
            profiling.showProfilingForm(this.treeView1.Nodes);
        }

        //kevin
        private void customQueryToolStripMenuItem_Click(object sender, EventArgs e) {
            if (editMode) {
                new CustomQuery(new SolrQuery(this, ip), this).Show();
            } else {
                MessageBox.Show("Edit mode must be enabled to execute custom queries.");
            }
        }

        private void ColumnClick(object o, ColumnClickEventArgs e)
        {
            switch (e.Column)
            {
                case 1:
                    mailSort = "conv_sort";
                    break;
                case 2:
                    mailSort = "fromdisp_sort";
                    break;
                case 3:
                    mailSort = "todisp_sort";
                    break;
                case 6:
                    mailSort = "msgmodifiedtime";
                    break;
            }
            start = 0;
            treeView1_AfterSelect(this.treeView1.SelectedNode, new TreeViewEventArgs(this.treeView1.SelectedNode, TreeViewAction.ByMouse));
        }

        private void customPageBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                customPageLink(sender, e);
            }
        }

        private void atomicUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (editMode)
            {
                AtomicUpdate atomicUpdate = new AtomicUpdate(this, ip);
                try
                {
                    atomicUpdate.ShowDialog();
                }
                catch(Exception E){
                    logToFile(E.Message, DEBUG_HIGH);
                }
            }
            else
            {
                MessageBox.Show("You must be in edit mode in order to send atomic updates.");
            }
        }

        private void segmentReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SolrQuery query = new SolrQuery(ip, this);
            List<XmlDocument> segmentDocuments = query.segmentReport();
            XMLParser parser = new XMLParser(segmentDocuments, this);
            List<SegmentInfo> segmentInfo = parser.segmentInfo;
            List<String> numDocsPerCore = query.getNumDocsPerCore();
            foreach (String docsInCore in numDocsPerCore)
            {
                int x = numDocsPerCore.IndexOf(docsInCore);
                SegmentInfo temp = segmentInfo[x];
                temp.numberOfDocuments = Int32.Parse(docsInCore);
                segmentInfo[x] = temp;
            }
            int time = parser.timeForQuery;
            Report report = new Report(query.coreNames, segmentInfo);
            report.Show();
        }

        private void contentReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((sender as ToolStripMenuItem).Text.Equals("Core")) //core level report
            {
                SolrQuery query = new SolrQuery(ip, this);
                List<List<int>> data = query.CoreLevelReport();
                Report report = new Report(query.coreNames, data);
                report.Show();
            }
            else
            {
                if (this.treeView1.Nodes.Count < 1)
                {
                    search_Click(searchToolStripMenuItem, new EventArgs());
                }
                StandardInputForm inputBox = new StandardInputForm();
                inputBox.Description.Text = "Enter a keyword for the mailbox filter";
                List<String> topFolders = new List<string>();
                inputBox.searchBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                inputBox.searchBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                foreach (TreeNode node in this.treeView1.Nodes)
                { //root node
                    foreach (TreeNode childNode in node.Nodes)
                    { //cores
                        foreach (TreeNode grandChildNode in childNode.Nodes)
                        {
                            if (grandChildNode.Tag != null)
                            {
                                ServerResult tempNodeResults = (ServerResult)grandChildNode.Tag;
                                if (tempNodeResults.dataType.Equals(DATATYPE_MAILBOX))
                                {
                                    topFolders.Add(grandChildNode.Text);
                                }
                            }
                        }
                    }
                }

                AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
                collection.AddRange(topFolders.ToArray());
                inputBox.searchBox.AutoCompleteCustomSource = collection;
                inputBox.searchBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                inputBox.ShowDialog();

                String mailbox = "";

                if (inputBox.DialogResult == DialogResult.OK)
                {
                    mailbox = inputBox.searchBox.Text;
                }
                else
                {
                    return;
                }
                List<TreeNode> allNodes = new List<TreeNode>();
                foreach (TreeNode node in treeView1.Nodes[0].Nodes)
                {
                    List<TreeNode> treeNodes = node.Nodes
                                    .Cast<TreeNode>()
                                    .Where(r => r.Text.Equals(mailbox))
                                    .ToList();
                    allNodes.AddRange(treeNodes);
                }
                if (allNodes.Count == 0)
                {
                    foreach (TreeNode node in treeView1.Nodes[1].Nodes)
                    {
                        List<TreeNode> treeNodes = node.Nodes
                                        .Cast<TreeNode>()
                                        .Where(r => r.Text.Equals(mailbox))
                                        .ToList();
                        allNodes.AddRange(treeNodes);
                    }
                }

                String cvowner = ((ServerResult)(allNodes[0].Tag)).cvowner;
                String core = allNodes[0].Parent.Text;
                SolrQuery query = new SolrQuery(ip, this);
                ContentIndexedInfo cii = query.MailboxLevelReport(cvowner, core);
                Report report = new Report(mailbox, cii);
                report.Show();
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void moveJournalmbx()
        {
            treeViewHandler.moveJournalMbx();
        }

        public void addSmartFolders()
        {
            smartFolderHandler.addSmartFolders();
        }

        private void lastPageButton_Click(object sender, EventArgs e)
        {
            pageHandler.lastPage();
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            if (superUser)
            {
                editMode = true;
                editModeLabel.Text = "EDIT MODE";
                this.updateEditToolStripMenuItem.Text = "Turn off Edit Mode";
                this.listView1.ContextMenuStrip.Items[0].Text = "Update";
                this.fullReconstructionToolStripMenuItem.Visible = true;
            }
        }

        private void archiveReportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            String selection = "";
            switch ((sender as ToolStripMenuItem).Text)
            {
                case "Core":
                    /**StandardInputForm inputForm = new StandardInputForm();
                    inputForm.Description.Text = "Please select a core.";
                    inputForm.searchBox.Items.Clear();
                    inputForm.searchBox.Items.Add("journalmbx");
                    inputForm.searchBox.Items.Add("usermbx0");
                    inputForm.searchBox.Items.Add("usermbx1");
                    inputForm.searchBox.Items.Add("usermbx2");
                    inputForm.searchBox.Items.Add("usermbx3");
                    inputForm.searchBox.Items.Add("usermbx4");
                    inputForm.searchBox.Items.Add("usermbx5");
                    inputForm.searchBox.Items.Add("usermbx6");
                    inputForm.searchBox.Items.Add("usermbx7");
                    inputForm.ShowDialog();
                    if (inputForm.DialogResult == DialogResult.OK)
                    {
                        selection = inputForm.searchBox.Text;
                    }
                    else
                    {
                        break;
                    }*/
                    ArchivingSelectionForm selectionForm = new ArchivingSelectionForm();
                    selectionForm.ShowDialog();
                    if (selectionForm.DialogResult == DialogResult.No)
                    {
                        selection = "journalmbx";
                    }
                    else
                    {
                        selection = "usermbx";
                    }
                    ArchivingReportForm form = new ArchivingReportForm(this, new SolrQuery(this, ip), selection);
                    form.ShowDialog();
                    break;
                case "Mailbox":
                    StandardInputForm inputBox = new StandardInputForm();
                    inputBox.Description.Text = "Enter a keyword for the mailbox filter";
                    List<String> topFolders = new List<string>();
                    inputBox.searchBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    inputBox.searchBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    foreach (TreeNode node in this.treeView1.Nodes)
                    { //root node
                        foreach (TreeNode childNode in node.Nodes)
                        { //cores
                            foreach (TreeNode grandChildNode in childNode.Nodes)
                            {
                                if (grandChildNode.Tag != null)
                                {
                                    ServerResult tempNodeResults = (ServerResult)grandChildNode.Tag;
                                    if (tempNodeResults.dataType.Equals(DATATYPE_MAILBOX))
                                    {
                                        topFolders.Add(grandChildNode.Text);
                                    }
                                }
                            }
                        }
                    }

                    AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
                    collection.AddRange(topFolders.ToArray());
                    inputBox.searchBox.AutoCompleteCustomSource = collection;
                    inputBox.searchBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    inputBox.ShowDialog();

                    if (inputBox.DialogResult == DialogResult.OK)
                    {
                        selection = inputBox.searchBox.Text;
                    }
                    else
                    {
                        break;
                    }

                    String mailbox = selection;

                    List<TreeNode> allNodes = new List<TreeNode>();
                    foreach (TreeNode node in treeView1.Nodes[0].Nodes)
                    {
                        List<TreeNode> treeNodes = node.Nodes
                                        .Cast<TreeNode>()
                                        .Where(r => r.Text.Equals(mailbox))
                                        .ToList();
                        allNodes.AddRange(treeNodes);
                    }

                    if (allNodes.Count == 0)
                    {
                        foreach (TreeNode node in treeView1.Nodes[1].Nodes)
                        {
                            List<TreeNode> treeNodes = node.Nodes
                                            .Cast<TreeNode>()
                                            .Where(r => r.Text.Equals(mailbox))
                                            .ToList();
                            allNodes.AddRange(treeNodes);
                        }
                    }

                    String core = allNodes[0].Parent.Text;

                    ServerResult res = (ServerResult) allNodes[0].Tag;

                    String cvowner = res.cvowner;

                    form = new ArchivingReportForm(new SolrQuery(this, ip), this, cvowner, core);
                    form.ShowDialog();
                    break;
            }
            //ArchivingReportForm form = new ArchivingReportForm(this, new SolrQuery(this, ip));
            //form.ShowDialog();
        }

        private void performanceCountersReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String selection = "";

            StandardInputForm inputForm = new StandardInputForm();
            inputForm.Description.Text = "Please select a core.";
            inputForm.searchBox.Items.Clear();
            inputForm.searchBox.Items.Add("journalmbx");
            inputForm.searchBox.Items.Add("usermbx0");
            inputForm.searchBox.Items.Add("usermbx1");
            inputForm.searchBox.Items.Add("usermbx2");
            inputForm.searchBox.Items.Add("usermbx3");
            inputForm.searchBox.Items.Add("usermbx4");
            inputForm.searchBox.Items.Add("usermbx5");
            inputForm.searchBox.Items.Add("usermbx6");
            inputForm.searchBox.Items.Add("usermbx7");
            inputForm.ShowDialog();
            if (inputForm.DialogResult == DialogResult.OK)
            {
                selection = inputForm.searchBox.Text;
            }
            else
            {
                return;
            }

            List<List<PerformanceInfo>> performanceInfos = (new SolrQuery(ip, this)).performanceCounterReport(selection);
            Report report = new Report(selection, performanceInfos);
            report.Show();

            //add to report form later
        }

        private void checkCoreNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StandardInputForm inputForm = new StandardInputForm();
            inputForm.Description.Text = "Please enter the GUID of the mailbox you want to check the core of.";
            inputForm.ShowDialog();
            String mbxguid = "";
            if (inputForm.DialogResult == DialogResult.OK)
            {
                mbxguid = inputForm.searchBox.Text;
            }

            char[] splitId = mbxguid.ToCharArray();
            int addedHex = 0;

            foreach (char thisChar in splitId)
                addedHex += thisChar;

            string coreName = "usermbx" + (addedHex % 8);

            MessageBox.Show("The given mbxGUID would belong to the " + coreName + " core.");
        }
    }
}