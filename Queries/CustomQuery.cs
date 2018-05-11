using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Net;
using MonitorFolderActivity;
using System.Windows.Forms;
using System.Threading;
using System.Xml.Linq;

namespace ExchangeOnePassIdxGUI {

    public partial class CustomQuery : Form {

        public const int SELECT = 0;
        public const int UPDATE = 1;
        public const int DELETE = 2;
        public const int CUSTOM = 3;
        private string requestUrl;
        private string facetQuery;
        private SolrQuery solrQuery;
        private frmMain mainForm;
        private Dictionary<string, string> urlBuilder = new Dictionary<string, string>();

        public CustomQuery(SolrQuery query, frmMain mainForm) 
        {
            InitializeComponent();
            this.solrQuery = query;
            this.mainForm = mainForm;
            string[] requestTypes = new string[] { "select", "update", "delete", "facetupdate", "custom", "atomic update", "optimize", "commit"};

            String [] temp = { "journalmbx", "usermbx0", "usermbx1", "usermbx2", "usermbx3", "usermbx4", "usermbx5", "usermbx6", "usermbx7" };
            List<string> coreNames = temp.ToList();//Array();//solrQuery.coreNames;

            if (coreNames == null)
                return;

            coreNames.Insert(0, "usermbx"); //root virtual core

            this.requestTypeComboBox.Items.AddRange(requestTypes);
            this.coreNameComboBox.Items.AddRange(coreNames.ToArray());
            coreNames.RemoveAt(0);

            this.fullQueryText.Text = solrQuery.solrUrl;

            urlBuilder.Add("baseUrl", query.solrUrl);
            urlBuilder.Add("coreName", null);
            urlBuilder.Add("requestType", null);
            urlBuilder.Add("queryText", null);
        }

        private void coreNameComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            updateLiveUrlLabel("coreName", coreNameComboBox.SelectedItem.ToString());
            createUrl("coreName", coreNameComboBox.SelectedItem.ToString());
        }

        private void requestTypeComboBox_SelectedIndexChanged(object sender, EventArgs e) 
        {
            if (requestTypeComboBox.SelectedItem.ToString().Equals("update")) 
            {
                customQueryList.Visible = false;
                Add.Visible = false;
                coreNameLabel.Visible = true;
                coreNameComboBox.Visible = true;
                this.queryLabel.Text = "Condition:";

                this.newKeyLabel.Visible = true;
                this.newKeyLabel.Text = "Key to update:";

                this.newValueLabel.Visible = true;
                this.newValueLabel.Text = "New value for key:";

                this.newKeyText.Visible = true;
                this.newValueText.Visible = true;
                this.facetFiedlLabel.Visible = false;
                this.facetValueText.Visible = false;

                this.queryText.Height = 20;
            } 
            else if (requestTypeComboBox.SelectedItem.ToString().Equals("facetupdate"))
            {
                customQueryList.Visible = false;
                Add.Visible = false;
                coreNameLabel.Visible = true;
                coreNameComboBox.Visible = true;
                this.queryLabel.Text = "Condition:";

                this.newKeyLabel.Visible = true;
                this.newKeyLabel.Text = "Key to update:";

                this.newValueLabel.Visible = true;
                this.newValueLabel.Text = "New value for key:";

                this.newKeyText.Visible = true;
                this.newValueText.Visible = true;

                this.queryText.Height = 20;

                this.facetFiedlLabel.Visible = true;
                this.facetFiedlLabel.Text = "Faceting field:";

                this.facetValueText.Visible = true;
            }
            else if (requestTypeComboBox.SelectedItem.ToString().Equals("atomic update"))
            {
                customQueryList.Visible = false;
                Add.Visible = false;
                coreNameLabel.Visible = true;
                coreNameComboBox.Visible = true;
                this.queryLabel.Text = "Condition(s):";

                this.newKeyLabel.Visible = true;
                this.newKeyLabel.Text = "Key to update:";

                this.newValueLabel.Visible = true;
                this.newValueLabel.Text = "New value for key:";

                this.newKeyText.Visible = true;
                this.newValueText.Visible = true;

                this.queryText.Height = 21;

                this.facetFiedlLabel.Visible = true;
                this.facetFiedlLabel.Text = "UpdateRows: ";

                this.facetValueText.Text = mainForm.pageSize + "";
                this.facetValueText.Visible = true;
            }
            else if (queryText.Height == 20) 
            {
                customQueryList.Visible = false;
                Add.Visible = false;
                coreNameLabel.Visible = true;
                coreNameComboBox.Visible = true;
                this.queryLabel.Text = "Query body:";
                this.newKeyLabel.Visible = false;
                this.newValueLabel.Visible = false;
                this.newKeyText.Visible = false;
                this.newValueText.Visible = false;
                this.facetFiedlLabel.Visible = false;
                this.facetValueText.Visible = false;
                this.queryText.Height = 90;
            }
            else if (requestTypeComboBox.SelectedItem.ToString().Equals("select"))
            {
                customQueryList.Visible = false;
                Add.Visible = false;
                coreNameLabel.Visible = true;
                coreNameComboBox.Visible = true;
                this.queryLabel.Text = "Query Body:";
            }
            else if (requestTypeComboBox.SelectedItem.ToString().Equals("delete"))
            {
                customQueryList.Visible = false;
                Add.Visible = false;
                coreNameLabel.Visible = true;
                coreNameComboBox.Visible = true;
                this.queryLabel.Text = "Query Body:";
            }
            else if (requestTypeComboBox.SelectedItem.ToString().Equals("custom"))
            {
                customQueryList.Visible = true;
                Add.Visible = true;
                coreNameLabel.Visible = false;
                coreNameComboBox.Visible = false;
                this.queryLabel.Text = "Raw query:";
                this.facetFiedlLabel.Visible = false;
                this.facetValueText.Visible = false;
            }
            else if (requestTypeComboBox.SelectedItem.ToString().Equals("commit"))
            {
                coreNameLabel.Visible = true;
                coreNameComboBox.Visible = true;
                submitButton.Visible = true;
                this.facetFiedlLabel.Visible = false;
                this.facetValueText.Visible = false;
                customQueryList.Visible = false;
                Add.Visible = false;
                helpButton.Visible = false;
                queryText.Visible = false;
                queryLabel.Visible = false;
            }
            else if (requestTypeComboBox.SelectedItem.ToString().Equals("optimize"))
            {
                coreNameLabel.Visible = true;
                coreNameComboBox.Visible = true;
                submitButton.Visible = true;
                this.facetFiedlLabel.Visible = false;
                this.facetValueText.Visible = false;
                customQueryList.Visible = false;
                Add.Visible = false;
                helpButton.Visible = false;
                queryText.Visible = false;
                queryLabel.Visible = false;
            }
            updateLiveUrlLabel("requestType", requestTypeComboBox.SelectedItem.ToString()); 
            createUrl("requestType", requestTypeComboBox.SelectedItem.ToString()); 
        } 
         
        private void createUrl(string key, string value) {
            value = value.Trim();
            urlBuilder[key] = value;

            requestUrl = urlBuilder["baseUrl"] + "/" + urlBuilder["coreName"] + "/";

            foreach (KeyValuePair<string, string> part in urlBuilder) {
                if (part.Value == null) //url is incomplete
                    return;
            }

            //url is ready to submit

            switch (urlBuilder["requestType"]) {
                case "select":

                    requestUrl += "select?q=";
                    requestUrl += urlBuilder["queryText"];
                    requestUrl += "&wt=xml";

                    break;

                case "update":

                    string newKey = this.newKeyText.Text;
                    string newValue = this.newValueText.Text;

                    requestUrl += "atomic?commit=true&stream.body=<add><doc><query>";
                    requestUrl += urlBuilder["queryText"];
                    requestUrl += "</query><field name=\"" + newKey + "\" update=\"set\">" + newValue + "</field></doc></add>";

                    break;

                case "delete":

                    requestUrl += "update?stream.body=<delete><query>";
                    requestUrl += urlBuilder["queryText"];
                    requestUrl += "</query></delete>&commit=true";

                    break;

                case "facetupdate":
                    string facetField = this.facetValueText.Text;

                    facetQuery = urlBuilder["baseUrl"] + "/" + urlBuilder["coreName"] + "/";
                    facetQuery += "select?q=";
                    facetQuery += urlBuilder["queryText"];
                    facetQuery += "&facet=on&facet.field=";
                    facetQuery += facetField;
                    facetQuery += "&facet.limit=-1&facet.mincount=1";
                    facetQuery += "&rows=0&wt=xml";

                    break;

                case "custom":
                    requestUrl = queryText.Text;

                    break;

                default:
                    break;
            }
        }  

        private void submitRequest(string url)
        {
            XmlDocument doc = solrQuery.submitHttpRequest(url, frmMain.DEBUG_REQUEST);
            string prettyDoc = solrQuery.beautify(doc);
            mainForm.logToFile("Custom URL: " + url, frmMain.DEBUG_RESPONSE);
            mainForm.logToFile(prettyDoc, frmMain.DEBUG_RESPONSE);

            mainForm.AppendTextBox("Custom URL: " + url + "\n" + prettyDoc);
            mainForm.AppendTextBox("");
        } 

        private void updateLiveUrlLabel(string key, string value) {
            value = value.Trim();
            Dictionary<string, string> tempDict = urlBuilder;
            tempDict[key] = value;
            string tempUrl = "SOLR request url: ";

            tempUrl = tempDict["baseUrl"] + "/" + tempDict["coreName"] + "/";

            switch (tempDict["requestType"]) {
                case "select":

                    tempUrl += "select?q=";
                    tempUrl += tempDict["queryText"];
                    tempUrl += "&wt=xml";

                    break;

                case "update":

                    string newKey = this.newKeyText.Text;
                    string newValue = this.newValueText.Text;

                    tempUrl += "atomic?commit=true&stream.body=<add><doc><query>";
                    tempUrl += tempDict["queryText"];
                    tempUrl += "</query><field name=\"" + newKey + "\" update=\"set\">" + newValue + "</field></doc></add>";

                    break;

                case "delete":

                    tempUrl += "update?stream.body=<delete><query>";
                    tempUrl += tempDict["queryText"];
                    tempUrl += "</query></delete>&commit=true";

                    break;

                case "custom":

                    tempUrl = queryText.Text;

                    break;

                case "atomic update":
                    fullQueryText.Size = new System.Drawing.Size(598, 113);
                    fullQueryText.Location = new System.Drawing.Point(42, 260);
                    tempUrl += "atomic/?stream.body=<add><doc><query><![CDATA[";
                    tempUrl += queryText.Text;
                    tempUrl += "]]></query><field name=\"";
                    tempUrl += newKeyText.Text;
                    tempUrl += "\" update=\"set\" ><![CDATA[";
                    tempUrl += newValueText.Text;
                    tempUrl += "]]></field></doc></add>&softCommit=true&updateRows=" + facetValueText.Text;
                    break;

                case "commit":
                    tempUrl += "update?commit=true";
                    break;
                case "optimize":
                    tempUrl += "update?optimize=true&commit=true";
                    break;
                default:
                    break;
            }

            this.fullQueryText.Text = tempUrl;
            this.fullQueryText.WordWrap = true;
        } 

        private Boolean submitFacetUpdateRequest()
        {
            if (urlBuilder["requestType"] == "facetupdate")
            {
                XmlDocument doc = solrQuery.submitHttpRequest(facetQuery, frmMain.DEBUG_REQUEST);
                XDocument xDocument = XDocument.Parse(doc.OuterXml);

                if (facetValueText.Text == "jid")
                {
                    var jobIds = (from facetCount in xDocument.Elements("response").Elements("lst")
                                  where (string)facetCount.Attribute("name") == "facet_counts"
                                  from facetField in facetCount.Elements("lst")
                                  where (string)facetField.Attribute("name") == "facet_fields"
                                  from jid in facetField.Elements("lst")
                                  where (string)jid.Attribute("name") == "jid"
                                  from ele in jid.Elements()
                                  where Convert.ToInt32(ele.Value.ToString()) > 0
                                  select new
                                  {
                                      jobId = ele.Attribute("name").Value,
                                      count = ele.Value
                                  });
                    foreach (var jobId in jobIds)
                    {
                        string logStr = "jobId:" + jobId.jobId + " count:" + jobId.count;
                        mainForm.logToFile(DateTime.Now + ": JobId Facet" + logStr, frmMain.DEBUG_NORMAL);
                    }

                    foreach (var jobId in jobIds)
                    {
                        string tempUrl = requestUrl;
                        string newKey1 = this.newKeyText.Text;
                        string newValue1 = this.newValueText.Text;


                        tempUrl += "atomic?stream.body=<add><doc><query>";
                        tempUrl += urlBuilder["queryText"];
                        tempUrl += " AND jid:";
                        tempUrl += jobId.jobId;
                        tempUrl += "</query><field name=\"" + newKey1 + "\" update=\"set\">" + newValue1 + "</field></doc></add>";

                        XmlDocument doc1 = solrQuery.submitHttpRequest(tempUrl, frmMain.DEBUG_REQUEST);

                        string prettyDoc = solrQuery.beautify(doc1);
                    }
                }
                else if (facetValueText.Text == "afid")
                {
                    var afids = (from facetCount in xDocument.Elements("response").Elements("lst")
                                  where (string)facetCount.Attribute("name") == "facet_counts"
                                  from facetField in facetCount.Elements("lst")
                                  where (string)facetField.Attribute("name") == "facet_fields"
                                  from jid in facetField.Elements("lst")
                                 where (string)jid.Attribute("name") == "afid"
                                  from ele in jid.Elements()
                                  where Convert.ToInt32(ele.Value.ToString()) > 0
                                  select new
                                  {
                                      afid = ele.Attribute("name").Value,
                                      count = ele.Value
                                  });
                    foreach (var afid in afids)
                    {
                        string logStr = "afid:" + afid.afid + " count:" + afid.count;
                        mainForm.logToFile(DateTime.Now + ": Facet " + logStr, frmMain.DEBUG_NORMAL);
                    }

                    foreach (var afid in afids)
                    {
                        string tempUrl = requestUrl;
                        string newKey1 = this.newKeyText.Text;
                        string newValue1 = this.newValueText.Text;


                        tempUrl += "atomic?stream.body=<add><doc><query>";
                        tempUrl += urlBuilder["queryText"];
                        tempUrl += " AND afid:";
                        tempUrl += afid.afid;
                        tempUrl += "</query><field name=\"" + newKey1 + "\" update=\"set\">" + newValue1 + "</field></doc></add>";

                        XmlDocument doc1 = solrQuery.submitHttpRequest(tempUrl, frmMain.DEBUG_REQUEST);

                        string prettyDoc = solrQuery.beautify(doc1);
                    }
                }

                string commitURL = requestUrl + "atomic?commit=true";
                solrQuery.submitHttpRequest(commitURL, frmMain.DEBUG_REQUEST);

                return true;
            }

            return false;
        } 

        private void submitButton_Click(object sender, EventArgs e) {
            int numLoops = 1;
            try
            {
                if (requestTypeComboBox.SelectedItem.ToString().Equals("custom"))
                {
                    ThreadPool.SetMaxThreads(3, 3);
                    List<string> urls = new List<string>();
                    foreach (string query in customQueryList.Items)
                    {
                        urls.Add(query);
                    }
                    foreach (string url in urls)
                    {
                        if (url.IndexOf("usermbx/") > 0)
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                string realUrl = url.Substring(0, url.IndexOf("usermbx/") + 7) + i + url.Substring(url.IndexOf("usermbx/") + 7);
                                ThreadPool.QueueUserWorkItem(delegate { submitRequest(realUrl); });
                            }
                        }
                        else
                        {
                            submitRequest(url);
                        }
                        mainForm.logToFile("END custom user request", frmMain.DEBUG_RESPONSE);

                    }

                }
                else if (urlBuilder["coreName"].Equals("usermbx"))
                {
                    numLoops = 8;
                }

                if (requestTypeComboBox.SelectedItem.ToString().Equals("atomic update"))
                {
                    if (numLoops > 1)
                    {
                        String requrl = fullQueryText.Text;
                        int indexOfInsert = requrl.IndexOf("usermbx/");
                        for (int x = 0; x < 8; x++)
                        {
                            requrl = requrl.Insert(indexOfInsert + 7, x + "");
                            try
                            {
                                int updatedRows = Int32.Parse(facetValueText.Text);
                                while (updatedRows == Int32.Parse(facetValueText.Text))
                                {
                                    XmlDocument doc = solrQuery.submitHttpRequest(requrl);
                                    XMLParser parser = new XMLParser(doc);
                                    updatedRows = parser.updatedRows;
                                }
                                //String temp = requrl;
                                //temp = temp.Replace("softCommit=false", "softCommit=true");
                                //solrQuery.submitHttpRequest(temp);                                  
                            }
                            catch { MessageBox.Show("Atomic Update Failed"); break; }
                            requrl = fullQueryText.Text;
                        }
                        this.Close();
                        return;
                    }
                    String url = fullQueryText.Text;
                    try
                    {
                        int updatedRows = Int32.Parse(facetValueText.Text);
                        while (updatedRows == Int32.Parse(facetValueText.Text))
                        {
                            XmlDocument doc = solrQuery.submitHttpRequest(url);
                            XMLParser parser = new XMLParser(doc);
                            updatedRows = parser.updatedRows;
                        }
                        //String temp = url;
                        //temp = temp.Replace("softCommit=false", "softCommit=true");
                        //solrQuery.submitHttpRequest(temp);
                    }
                    catch { MessageBox.Show("Atomic Update Failed"); }
                    this.Close();
                    return;
                }

                if (requestTypeComboBox.SelectedItem.ToString().Equals("optimize"))
                {
                    if (numLoops > 1)
                    {
                        String requrl = fullQueryText.Text;
                        int indexOfInsert = requrl.IndexOf("usermbx/");
                        for (int x = 0; x < 8; x++)
                        {
                            requrl = requrl.Insert(indexOfInsert + 7, x + "");
                            try
                            {
                                solrQuery.submitHttpRequest(requrl);                                  
                            }
                            catch { MessageBox.Show("Optimize Failed"); break; }
                            requrl = fullQueryText.Text;
                        }
                        this.Close();
                        return;
                    }
                    else
                    {
                        String url = fullQueryText.Text;
                        solrQuery.submitHttpRequest(url);
                        this.Close();
                        return;
                    }
                }

                if (requestTypeComboBox.SelectedItem.ToString().Equals("commit"))
                {
                    if (numLoops > 1)
                    {
                        String requrl = fullQueryText.Text;
                        int indexOfInsert = requrl.IndexOf("usermbx/");
                        for (int x = 0; x < 8; x++)
                        {
                            requrl = requrl.Insert(indexOfInsert + 7, x + "");
                            try
                            {
                                solrQuery.submitHttpRequest(requrl);
                            }
                            catch { MessageBox.Show("Commit Failed"); break; }
                            requrl = fullQueryText.Text;
                        }
                        this.Close();
                        return;
                    }
                    else
                    {
                        String url = fullQueryText.Text;
                        solrQuery.submitHttpRequest(url);
                        this.Close();
                        return;
                    }
                }

                if (!requestTypeComboBox.SelectedItem.ToString().Equals("custom"))
                {
                    
                    for (int i = 0; i < numLoops; i++)
                    {
                        if (numLoops > 1)
                            urlBuilder["coreName"] = "usermbx" + i; //HARDCODE

                        createUrl("queryText", this.queryText.Text);
                        this.fullQueryText.Text = requestUrl;

                        mainForm.logToFile("START custom user request:", frmMain.DEBUG_RESPONSE);

                        if (submitFacetUpdateRequest())
                            continue;

                        ThreadPool.QueueUserWorkItem(delegate { submitRequest(requestUrl); });
                    }
                } 
                this.Close();
            }
            catch (NullReferenceException) { };
            
        } 

        private void queryText_TextChanged(object sender, EventArgs e) {
            updateLiveUrlLabel("queryText", queryText.Text);
        }

        private void newKeyText_TextChanged(object sender, EventArgs e) {
            updateLiveUrlLabel("newKey", newKeyText.Text);
        }

        private void newValueText_TextChanged(object sender, EventArgs e) {
            updateLiveUrlLabel("newValue", newValueText.Text);
        }

        private void pictureBox1_Click(object sender, EventArgs e) {
            Clipboard.SetText(fullQueryText.Text);
        }

        private void helpButton_Click(object sender, EventArgs e) {
            new CommandListForm().Show();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            customQueryList.Items.Add(queryText.Text);
            queryText.Text = "";
        }

        private void facetValueText_TextChanged(object sender, EventArgs e)
        {
            updateLiveUrlLabel("rows", facetValueText.Text);
        }
    }
}
