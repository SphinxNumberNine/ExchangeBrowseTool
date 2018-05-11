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
using System.IO;

namespace ExchangeOnePassIdxGUI {

    public class SolrQuery {
        public frmMain mainForm;
        public String solrUrl;
        public List<String> coreNames;
        public List<String> numberOfDocsInEachCore;
        public List<XmlDocument> data;
        public List<XmlDocument> datatype5;
        public string apidList;
        public bool superUser;
        public LogInfo userCreds;
        public string webServerName;
        public string token;
        public int pageSize;

        public SolrQuery(String ip, int x, frmMain form, string mbxGUID = null) //used for all ambiguous queries
        {
            numberOfDocsInEachCore = new List<string>();
            mainForm = form;
            pageSize = form.pageSize;
            solrUrl = "http://" + ip + ":20000/solr";
            coreNames = getCoreNames(mbxGUID);
        }

        public SolrQuery(String ip, frmMain form) //used for initial search/to get mailboxes
        {
            superUser = form.superUser;
            numberOfDocsInEachCore = new List<string>();
            mainForm = form;
            pageSize = form.pageSize;
            userCreds = form.userCreds;
            webServerName = form.webServerName;
            token = form.token;
            solrUrl = "http://" + ip + ":20000/solr";
            coreNames = getCoreNames();
            try

            {
                Security temp = new Security(solrUrl, this, mainForm);
                apidList = temp.apidString();
            }
            catch (NullReferenceException) { }
            datatype5 = getMailboxes();
        }

        public SolrQuery(String ip, String parentGuid, frmMain form, string mbxGUID) //used for getting subfolders
        {
            numberOfDocsInEachCore = new List<string>();
            mainForm = form;
            pageSize = form.pageSize;
            solrUrl = "http://" + ip + ":20000/solr";
            coreNames = getCoreNames(mbxGUID);
            numberOfDocsInEachCore = getNumDocsPerCore();
            data = getSubfoldersForOneMailbox(mbxGUID, parentGuid);
            if (mbxGUID.Equals(""))
            {
                data = getSubfoldersForJournalMBX(parentGuid);
                Console.WriteLine();
            }
        }

        public SolrQuery(frmMain form, String ip)
        {
            numberOfDocsInEachCore = new List<string>();
            mainForm = form;
            pageSize = form.pageSize;
            solrUrl = "http://" + ip + ":20000/solr";
            //coreNames = getCoreNames(mbxGUID);
        }
       
        public List<String> getCoreNames(string mbxGUID = null) //sends a status request and gets the name of each core, which will be used when sending queries
        {
            try {
                List<String> coreNames = new List<String>();
                string requestURL;
                if(mbxGUID != null)
                    requestURL = solrUrl + "/admin/cores?action=STATUS&core=" + getCoreName(mbxGUID);
                else
                    requestURL = solrUrl + "/admin/cores?action=STATUS";

                XmlDocument statusDoc = submitHttpRequest(requestURL);

                XmlNode statusNode = statusDoc.SelectSingleNode("//lst[@name='status']");
                foreach (XmlNode childNodes in statusNode.ChildNodes)
                {

                    String coreLower = childNodes.FirstChild.InnerText.ToLower();
                    if (coreLower == "usermbx0" || coreLower == "usermbx1" || coreLower == "usermbx2" || coreLower == "usermbx3" || coreLower == "usermbx4" ||
                        coreLower == "usermbx5" || coreLower == "usermbx6" || coreLower == "usermbx7" || coreLower == "journalmbx")
                    {

                        coreNames.Add(childNodes.FirstChild.InnerText);
                        XmlNode docNode = childNodes.SelectSingleNode(".//int[@name='numDocs']");
                        if(docNode != null)
                            numberOfDocsInEachCore.Add(docNode.InnerText);
                    }
                }


                return coreNames;
            } catch {
                MessageBox.Show("The target machine does not have any valid SOLR cores.", 
                    "IP Address Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return null;
            }
        }

        public List<String> getNumDocsPerCore() //sends a status request to the server and gets the number of docs in each core, which will be used when sending queries
        {
            try
            {
                List<String> coreDocs = new List<String>();
                HttpWebRequest getStatus = (HttpWebRequest)(WebRequest.Create(solrUrl + "/admin/cores?action=STATUS"));
                HttpWebResponse status = (HttpWebResponse)(getStatus.GetResponse());
                var encode = ASCIIEncoding.ASCII;
                WebHeaderCollection header = status.Headers;
                var reader = new System.IO.StreamReader(status.GetResponseStream(), encode);
                String response = reader.ReadToEnd();
                String decodedStatus = System.Web.HttpUtility.HtmlDecode(response);
                XmlDocument statusDoc = new XmlDocument();
                statusDoc.LoadXml(decodedStatus);
                XmlNode statusNode = statusDoc.SelectSingleNode("//lst[@name='status']");
                XmlNodeList docNumList = statusDoc.SelectNodes("//int[@name='numDocs']");
                foreach (XmlElement doc in docNumList)
                {
                    coreDocs.Add(doc.InnerText);
                }
                return coreDocs;
            }
            catch
            {
                return null;
            }
        }

        private List<XmlDocument> getData() //gets the data from each core and adds it to individual XML documents to be parsed for information later
        {
            List<XmlDocument> results = new List<XmlDocument>();
            for (int x = 0; x < coreNames.Count; x++) {
                String requestUrl = solrUrl + "/" + coreNames[x] + "/select?indent=on&q=*:*&rows=" + numberOfDocsInEachCore[x] + "&wt=xml";
                results.Add(submitHttpRequest(requestUrl));
            }
            return results;
        }

        //kevin
        // return the value of cvowner for a given contentid
        public string getCvowner(string contentid) {
            for (int x = 0; x < coreNames.Count; x++) {
                String requesturl = solrUrl + "/" + coreNames[x] + "/select?q=contentid:" + contentid + "&rows=1&wt=xml";
                HttpWebRequest req = (HttpWebRequest)(WebRequest.Create(requesturl));
                HttpWebResponse resp = (HttpWebResponse)(req.GetResponse());
                var encode = ASCIIEncoding.ASCII;
                WebHeaderCollection header = resp.Headers;
                var reader = new System.IO.StreamReader(resp.GetResponseStream(), encode);
                String response = reader.ReadToEnd();
                String decodedResponse = System.Web.HttpUtility.HtmlDecode(response);

                if (!decodedResponse.Contains("numFound=\"0\"")) {
                    if (decodedResponse.Contains("&")) {
                        decodedResponse = decodedResponse.Replace("&", "&amp;");
                    }

                    XmlDocument newDoc = new XmlDocument();
                    newDoc.LoadXml(decodedResponse);
                    List<XmlDocument> list = new List<XmlDocument>();
                    list.Add(newDoc);
                    XMLParser parser = new XMLParser(list);
                    ServerResult result = parser.results[0];
                    return result.cvowner;
                } else {
                    //not in this core; skip to minimize latency
                }
            }

            return null; //contentid is invalid
        }

        private List<XmlDocument> getMailboxes() {
            if (coreNames == null)
                return null;

            List<XmlDocument> datatype5Docs = new List<XmlDocument>();
            List<String> numMailboxesPerCore = new List<String>();
            List<Tuple<String, XmlDocument>> list = new List<Tuple<String, XmlDocument>>();
            foreach (String core in coreNames) {
                String requestUrl;
                if (core.Equals("journalmbx"))
                {
                    requestUrl = solrUrl + "/" + core + "/select?q=datatype:5&start=0&rows=0";
                }
                else
                {
                    requestUrl = solrUrl + "/" + core + "/select?q=datatype:5 AND " + apidList + "&start=0&rows=0";
                }
               
                list.Add(Tuple.Create(requestUrl, submitHttpRequest(requestUrl)));
            }
            for (int i = 0; i < list.Count; i++) {
                Tuple<String, XmlDocument> newTup = list[i];
                XmlDocument doc = newTup.Item2;
                XmlNode node = doc.SelectSingleNode("//result");
                if (node != null) {
                    String num = doc.SelectSingleNode("//result").Attributes["numFound"].Value;
                    try {
                    numMailboxesPerCore.Add(num);
                    } catch { }
                } else {
                    mainForm.logToFile("ERROR: Null node exception: " + newTup.Item1, frmMain.DEBUG_HIGH);
                    mainForm.logToFile("ERROR: Null node exception: " + newTup.Item2, frmMain.DEBUG_HIGH);
                    mainForm.logToFile("" + newTup.Item2, frmMain.DEBUG_HIGH);

                }
            }
            for (int x = 0; x < coreNames.Count; x++) {
                String requestUrl = solrUrl + "/" + coreNames[x] + "/select?indent=on&q=datatype:5&rows=" + numMailboxesPerCore[x] + "&wt=xml";
                datatype5Docs.Add(submitHttpRequest(requestUrl));
            }
            if (mainForm.settings[1]) {
                mainForm.txtActivity.AppendText("q=datatype:5");
                mainForm.txtActivity.AppendText(Environment.NewLine);
            }

            return datatype5Docs;
        }

        public List<XmlDocument> getSubfoldersForOneMailbox(String parentguid) {
            try {
                List<XmlDocument> subfolders = new List<XmlDocument>();
                for (int x = 0; x < coreNames.Count; x++) {
                    String requestUrl = solrUrl + "/" + coreNames[x] + "/select?q=datatype:4 AND parentguid:" + parentguid + "&rows=" + numberOfDocsInEachCore[x] + "&wt=xml";
                    subfolders.Add(submitHttpRequest(requestUrl));
                }
                if (mainForm.settings[1]) {
                    mainForm.txtActivity.AppendText("q=datatype:4 AND parentguid:" + parentguid);
                    mainForm.txtActivity.AppendText(Environment.NewLine);
                }
                return subfolders;
            } catch {
                return null;
            }
        }

        public List<XmlDocument> getSubfoldersForOneMailbox(String mbxGUID, String parentguid)
        {
            try
            {
                List<XmlDocument> subfolders = new List<XmlDocument>();
                // for (int x = 0; x < coreNames.Count; x++)
                String requestUrl = solrUrl + "/" + getCoreName(mbxGUID) + "/select?q=datatype:4 AND cvowner:" + mbxGUID + " parentguid:" + parentguid + "&wt=xml&rows=1000";
                subfolders.Add(submitHttpRequest(requestUrl));
                if (mainForm.settings[1])
                {
                    mainForm.txtActivity.AppendText("q=datatype:4 AND parentguid:" + parentguid);
                    mainForm.txtActivity.AppendText(Environment.NewLine);
                }
                return subfolders;
            }
            catch
            {
                return null;
            }
        }

        public int pagesNeeded(String mbxGUID, String parentguid)
        {
            try
            {
                int numDocs = 0;
                List<XmlDocument> list = new List<XmlDocument>();
                //foreach (String core in coreNames)
                //{
                    String requestUrl = solrUrl + "/" + getCoreName(mbxGUID) + "/select?q=datatype:2 AND cvowner:" + mbxGUID +" parentguid:" + parentguid + "&start=0&rows=0&wt=xml";
                    list.Add(submitHttpRequest(requestUrl));
                //}

                foreach (XmlDocument doc in list)
                {
                    String num = doc.SelectSingleNode("//result").Attributes["numFound"].Value;
                    try
                    {
                        numDocs += Int32.Parse(num);
                    }
                    catch { }
                }
                int pages = numDocs / pageSize;
                return pages + 1;
            }
            catch
            {
                return -1;
            }
        }


        //used for paging with main form buttons sending new start value
        public List<XmlDocument> getMailsForOneSubfolder(String mbxGUID, String parentguid, int start, string jobID, String sort, String core = null)
        {
            try
            {
                List<XmlDocument> mails = new List<XmlDocument>();
                StringBuilder requestUrl = new StringBuilder();
                if (mbxGUID.Equals(""))
                {
                    requestUrl.Append(solrUrl + "/journalmbx/select?q=datatype:2 AND parentguid:" + parentguid);
                }
                else
                {
                    requestUrl.Append(solrUrl + "/" + getCoreName(mbxGUID) + "/select?q=datatype:2 AND cvowner:" + mbxGUID + " parentguid:" + parentguid);
                }
                if (jobID != null && jobID.Length > 0)
                    requestUrl.Append("AND jid:" + jobID);

                requestUrl.Append("&start=" + (start * pageSize) + "&rows=" + pageSize + "&sort=" + sort);
                if (sort.Equals("msgmodifiedtime"))
                {
                    requestUrl.Append(" desc&wt=xml");
                }
                else
                {
                    requestUrl.Append(" asc&wt=xml");
                }

                mails.Add(submitHttpRequest(requestUrl.ToString())); //at this stop, copy paste URL and check that number of returned messages is 1000
                if (mainForm.settings[1])
                {
                    mainForm.txtActivity.AppendText("q=datatype:2 AND parentguid:" + parentguid + "&jid" + jobID);
                    mainForm.txtActivity.AppendText(Environment.NewLine);
                }
                return mails;
            }
            catch
            {
                return getMailsForOneSubfolder(parentguid, start + 1, sort);
            }
        }

        //used for paging with main form buttons sending new start value
        public List<XmlDocument> getMailsForOneSubfolder(String parentguid, int start, String sort, String core = null) {
            try {
                List<XmlDocument> mails = new List<XmlDocument>();
                if (core == null)
                {
                    for (int x = 0; x < coreNames.Count; x++)
                    {
                        String requestUrl = solrUrl + "/" + coreNames[x] + "/select?q=datatype:2 AND parentguid:" + parentguid + "&start=" + (start * pageSize) + "&rows=" + pageSize + "&sort=" + sort;
                        if (sort.Equals("msgmodifiedtime"))
                        {
                            requestUrl += " desc&wt=xml";
                        }
                        else
                        {
                            requestUrl += " asc&wt=xml";
                        }
                        mails.Add(submitHttpRequest(requestUrl));
                    }
                }
                else
                {
                    String requestUrl = solrUrl + "/" + core + "/select?q=datatype:2 AND parentguid:" + parentguid + "&start=" + (start * pageSize) + "&rows=" + pageSize + "&sort=" + sort;
                    if (sort.Equals("msgmodifiedtime"))
                    {
                        requestUrl += " desc&wt=xml";
                    }
                    else
                    {
                        requestUrl += " asc&wt=xml";
                    }
                    mails.Add(submitHttpRequest(requestUrl));
                }
                if (mainForm.settings[1]) {
                    mainForm.txtActivity.AppendText("q=datatype:2 AND parentguid:" + parentguid);
                    mainForm.txtActivity.AppendText(Environment.NewLine);
                }
                return mails;
            } catch {
                return getMailsForOneSubfolder(parentguid, start + 1, sort);
            }
        }

        //mails for csv export, default value is 100 mails
        public List<XmlDocument> getMailsForOneSubfolder(String parentguid, bool y, String sort) {
            try {
                List<XmlDocument> mails = new List<XmlDocument>();
                for (int x = 0; x < coreNames.Count; x++) {
                    String requestUrl = solrUrl + "/" + coreNames[x] + "/select?q=datatype:2 AND parentguid:" + parentguid + "&rows=100" + "&sort=" + sort;
                    if (sort.Equals("msgmodifiedtime"))
                    {
                        requestUrl+= " desc&wt=xml";
                    }
                    else
                    {
                        requestUrl += " asc&wt=xml";
                    }
                    mails.Add(submitHttpRequest(requestUrl));
                }
                return mails;
            } catch {
                return null;
            }
        }

        //used for getting mails for a specific jobID
        public List<XmlDocument> getMailsForOneSubfolder(String parentguid, int start, String jobID, String sort, String core = null) 
        {
            try {
                List<XmlDocument> mails = new List<XmlDocument>();
                if (core == null)
                {
                    for (int x = 0; x < coreNames.Count; x++)
                    {
                        String requestUrl = solrUrl + "/" + coreNames[x] + "/select?q=datatype:2 AND parentguid:" + parentguid + "%0Ajid:" + jobID + "&start=" + (start * pageSize) + "&rows=" + pageSize + "&sort=" + sort;
                        if (sort.Equals("msgmodifiedtime"))
                        {
                            requestUrl += " desc&wt=xml";
                        }
                        else
                        {
                            requestUrl += " asc&wt=xml";
                        }
                        mails.Add(submitHttpRequest(requestUrl));
                    }
                }
                else
                {
                    String requestUrl = solrUrl + "/" + core + "/select?q=datatype:2 AND parentguid:" + parentguid + "%0Ajid" + jobID + "&start=" + (start * pageSize) + "&rows=" + pageSize + "&sort=" + sort;
                    if (sort.Equals("msgmodifiedtime"))
                    {
                        requestUrl += " desc&wt=xml";
                    }
                    else
                    {
                        requestUrl += " asc&wt=xml";
                    }
                    mails.Add(submitHttpRequest(requestUrl));
                }
                if (mainForm.settings[1]) {
                    mainForm.txtActivity.AppendText("q=datatype:2&parentguid:" + parentguid + "&jid" + jobID);
                    mainForm.txtActivity.AppendText(Environment.NewLine);
                }
                return mails;
            } catch {
                return getMailsForOneSubfolder(parentguid, start + 1, sort);
            }
        }

        //delete a single mail/document
        public void delete(string guid) {
            foreach (string core in coreNames) {
                String requestUrl = solrUrl + "/" + core + "/update?stream.body=<delete><query>contentid:" + guid + "</query></delete>&commit=true";
                submitHttpRequest(requestUrl);
            }

            if (mainForm.settings[1]) {
                mainForm.txtActivity.AppendText("<delete><query>contentid:" + guid + "</query></delete>");
                mainForm.txtActivity.AppendText(Environment.NewLine);
            }
        }

        //recursively delete any document contained within the deleted folder
        public void deleteChildren(string guid) {
            List<ServerResult> children = new XMLParser(getSubfoldersForOneMailbox(guid)).results;
            foreach (ServerResult result in children) {
                deleteChildren(result.documentId);
            }
            foreach (string core in coreNames) {
                string requestUrl = solrUrl + "/" + core + "/update?stream.body=<delete><query>parentguid:" + guid + "</query></delete>&commit=true";
                submitHttpRequest(requestUrl);
            }
        }

        //kevin
        // modularize http requests to SOLR Cloud
        public XmlDocument submitHttpRequest(string requestUrl, int logResponse = frmMain.DEBUG_RESPONSE)
        {
            try {
                string encodedUrl = requestUrl;
                //post request
                HttpWebRequest request = WebRequest.Create(encodedUrl) as HttpWebRequest;
                request.Timeout = Timeout.Infinite;
                request.KeepAlive = true;

                //log request
                mainForm.logToFile(DateTime.Now + ": " + "Sent request to " + encodedUrl, frmMain.DEBUG_NORMAL);

                //get response
                HttpWebResponse httpResponse = request.GetResponse() as HttpWebResponse;

                //log response
                mainForm.logToFile(DateTime.Now + ": " + "Recieved response from " + requestUrl, logResponse);
                mainForm.logToFile("", logResponse);
                mainForm.logToFile("", logResponse);
                mainForm.logToFile("", logResponse);

                //var header = httpResponse.GetResponseHeader();
                StreamReader reader = new StreamReader(httpResponse.GetResponseStream());
                string sr = reader.ReadToEnd();
                if (httpResponse != null)
                    httpResponse.Close();

                //create new XML document
                XmlDocument newDoc = new XmlDocument();

                newDoc.LoadXml(new StringBuilder(sr).Replace("\n", "").ToString());

                //log the new document
                mainForm.logToFile(beautify(newDoc), logResponse);
                mainForm.logToFile("", logResponse);
                mainForm.logToFile("", logResponse);
                mainForm.logToFile("", logResponse);

                //return
                return newDoc;
            } catch (WebException e) {
                mainForm.logToFile("ERROR: Web Exception: " + e.Message, frmMain.DEBUG_HIGH);
                mainForm.logToFile("    URL: " + requestUrl, frmMain.DEBUG_HIGH);
                mainForm.logToFile("", frmMain.DEBUG_HIGH);

                return null;
            }
        }
        
        //kevin
        // fix broken xml
        public String patchXml(string xml, int position) {
            int newPos = position;
            string illegalChar = "";

            while (true) {
                string suspect = xml.ElementAt(newPos).ToString();

                if (suspect.Equals(">")) {
                    illegalChar = ">";
                    break;
                } else if (suspect.Equals("<")) {
                    illegalChar = "<";
                    break;
                } else if (suspect.Equals("]")) {

                    if (xml.ElementAt(newPos - 1).Equals("]")) {

                        illegalChar = "]]";

                        if (xml.ElementAt(newPos - 2).Equals(">")) {
                            illegalChar = "]]>";
                        } else {
                            break;
                        }
                    } else {
                        break;
                    }

                    illegalChar = "]";
                }

                newPos--;
            }
            
            xml = xml.Remove(newPos, illegalChar.Length);

                if (illegalChar.Equals(">")) {
                    xml = xml.Insert(newPos, "&gt;");
                } else if (illegalChar.Equals("<")) {
                    xml = xml.Insert(newPos, "&lt;");
                }
            
            return xml;


        }

        //kevin
        // Update an index's parentGUID to a new (string) id
        // return the query time
        // Ex: http://172.24.50.90:20000/solr/usermbx0/atomic?commit=true&stream.body=<add><doc><query>contentid:78ce26af88d1abc49a5672874d5dce678819a6ea7231820298471b2580b00298</query><field name="parentguid" update="set">123test123</field></doc></add>
        public string updateParent(string documentId, string newParentId, string cvOwner) {
            String updateUrl = solrUrl +
              "/" + getCoreName(cvOwner) +
              "/atomic?commit=true&stream.body=<add><doc><query>contentid:" + 
              documentId + 
              "</query><field name=\"parentguid\" update=\"set\">" + 
              newParentId + 
              "</field></doc></add>";

            return "Query took: " + getQueryTime(submitHttpRequest(updateUrl));
        }

        //kevin
        // return a core name based on the cvowner
        public static string getCoreName(string cvOwner) {
            if (cvOwner.Equals(""))
            {
                return "journalmbx";
            }
            char[] splitId = cvOwner.ToCharArray();
            int addedHex = 0;

            foreach (char thisChar in splitId)
                addedHex += thisChar;

            string coreName = "usermbx" + (addedHex % 8);

            //Console.WriteLine("cvowner:" + cvOwner + " core:" + coreName);

            return coreName;
        }

        //kevin
        private string getCiStatusClause() {
            if (mainForm.settings[2]) {
                return "cistatus:[1 TO 2]";
            } else {
                return "cistatus:2";
            }
        }

        //kevin
        public Dictionary<int, long> getCIStateCount(string cvowner) {
            if (coreNames == null)
                return null;

            Dictionary<int, long> data = new Dictionary<int, long>();

            //initialize dictionary
            for (int i = 0; i <= 1; i++) {
                data.Add(i, 0);
            }

            for (int coreNum = 0; coreNum < coreNames.Count; coreNum++) {
                for (int i = 0; i <= 1; i++) {
                    String url;

                    if (cvowner.Equals("")) {
                        url = solrUrl + "/" + coreNames[coreNum] + "/select?q=cistate:" + i +
                            " AND datatype:2&rows=0&wt=xml";
                    } else {
                        url = solrUrl + "/" + coreNames[coreNum] + "/select?q=cistate:" + i + " AND cvowner:" + cvowner +
                            " AND datatype:2&rows=0&wt=xml";
                    }

                    XmlDocument doc = submitHttpRequest(url);
                    XmlNode node = doc.SelectSingleNode("//result");

                    if (node != null) {
                        String numFound = node.Attributes["numFound"].Value;

                        try {
                            int n = Int32.Parse(numFound);
                            data[i] += Int32.Parse(numFound);
                        } catch { }
                    }
                }
            }

            return data;
        }

        //kevin
        //Ex for cistatus
        //Key: 0        Value: 720937
        //Key: 1        Value: 0
        //Key: 2        Value: 93232320
        //Key: 3        Value: 6
        public Dictionary<int, long> getCIStatusCount(int floor, int ceiling, string cvowner) {
            if (coreNames == null)
                return null;

            Dictionary<int, long> data = new Dictionary<int, long>();

            //initialize dictionary
            for (int i = floor; i <= ceiling; i++) {
                data.Add(i, 0);
            }

            for (int coreNum = 0; coreNum < coreNames.Count; coreNum++) {
                List<XmlDocument> docs = new List<XmlDocument>(1);
                docs.Add(null);

                for (int i = floor; i <= ceiling; i++) {
                    String url;

                    if (cvowner.Equals("")) {
                        url = solrUrl + "/" + coreNames[coreNum] + "/select?q=cistatus:" + i +
                            " AND datatype:2&rows=0&wt=xml";
                    } else {
                        url = solrUrl + "/" + coreNames[coreNum] + "/select?q=cistatus:" + i + " AND cvowner:" + cvowner +
                        " AND datatype:2&rows=0&wt=xml";
                    }

                    XmlDocument doc = submitHttpRequest(url);
                    XmlNode node = doc.SelectSingleNode("//result");

                    if (node != null) {
                        String numFound = node.Attributes["numFound"].Value;
                        
                        try {
                            int n = Int32.Parse(numFound);
                            data[i] += n;
                        } catch { }
                    }
                }
            }

            return data;
        }

        //kevin
        // a method to fetch all core names
        // then find all of the mailboxes
        private String[,] getAllMailboxes(string filter) {
            List<XmlDocument> docs = new List<XmlDocument>();
            foreach (String core in coreNames) {
                String updateUrl = solrUrl + "/" + core + "/select?q=datatype:5&start=0&rows=999999";
                docs.Add(submitHttpRequest(updateUrl));
            }

            XMLParser parser = new XMLParser(docs);
            List<ServerResult> results = parser.results;

            //start a list to keep track of cvowners
            List<string> cvownerList = new List<string>();

            foreach (ServerResult thisResult in results) {
                if (thisResult.mailbox.ToUpper().Contains(filter.ToUpper())) {
                    if (!cvownerList.Contains(thisResult.cvowner)) {
                        cvownerList.Add(thisResult.cvowner);
                    }
                }
            }

            //an array containing the cvowner and the corename
            String[,] mailboxes = new String[cvownerList.Count, 2];

            for (int i = 0; i < mailboxes.GetLength(0); i++) {
                mailboxes[i, 0] = cvownerList[i];
                mailboxes[i, 1] = getCoreName(cvownerList[i]);
            }

            return mailboxes;
        }

        //kevin
        // sort mailboxes into batches (list of mailboxes) by userdefined amount of mailboxes per batch (param)
        private List<Tuple<String, List<String>>> batchByMailbox(String[,] mailboxes, int mailboxesPerBatch) {
            //a list of core names that are *IN USE*
            List<String> ourCoreNames = new List<String>();

            for (int i = 0; i < mailboxes.GetLength(0); i++) {
                if (!ourCoreNames.Contains(mailboxes[i, 1])) {
                    ourCoreNames.Add(mailboxes[i, 1]);
                }
            }

            //a way to store multiple lists containing every cvowner according to their batch
            //Tuple string is the core name; list is list of string cvowner (mailbox id)
            List<Tuple<String, List<String>>> batches = new List<Tuple<String, List<String>>>();

            //save the old core's name
            //we need this in order to check if we need to start a new batch
            //two conditions for a new batch:
            //      1. new core
            //      2. hit cap of mailboxesPerBatch
            String prevCore = ourCoreNames[0];
            int oldi = 0;
            //outter loop for each batch
            for (int i = 0; i < mailboxes.GetLength(0); i += mailboxesPerBatch) {
                List<String> cvowners = new List<String>();

                //inner loop for each mailbox in current batch
                for (int count = 0; count < mailboxesPerBatch; count++) {
                    try {
                        if (prevCore.Equals(mailboxes[i + count, 1])) { //check if this is still the same core
                            cvowners.Add(mailboxes[i + count, 0]);
                            if (mailboxes[i + count, 0].Equals("ea85871ex8aafx4091x8937x55a938ddedf9")) {
                                Console.Write("");
                            }
                            String added = mailboxes[i + count, 0];
                        } else {
                            prevCore = mailboxes[i + count, 1];

                            /**explaination:
                             * Save the 'oldi' for later use, since we are going to modify a crucial counter
                             * we need to modify 'i' because of remainers
                             * Ex: there are 11 datatype2s in a core, so we have a remainder of 11
                             * When we break this loop, we are automatically adding 50 to i.
                             * But we had a remainder, so we are missing 39 indices since we just
                             * added 50. This compensates for remainders.
                             */
                            oldi = i;
                            i = i - 50;
                            i = i + count;
                            //endexplain

                            break; //new core, start new batch
                        }
                    } catch (IndexOutOfRangeException) {
                        break; //catch this because we're adding by mailboxesPerBatch and we will go out of bounds eventually due to remainders
                    }

                    /**explaination:
                     * set the 'oldi' to the current 'i' simply because 'oldi' is referenced below. 
                     */ 
                    oldi = i;
                    //endexplain

                    prevCore = mailboxes[i + count, 1];
                }

                batches.Add(Tuple.Create(mailboxes[oldi, 1], cvowners));
            }

            return batches;
        }

        //kevin
        // recontent index all of the mailboxes in batches of 50
        public bool recontentIndexAll(String filter) {
            List<Tuple<String, List<String>>> batches = batchByMailbox(getAllMailboxes(filter), 50);

            int batchCounter = 1;
            //loop through each batch
            foreach (Tuple<String, List<String>> thisBatch in batches) {
                //Item1 is core name
                //Item2 is list of mailboxes in current batch

                String updateUrl = solrUrl + "/" + thisBatch.Item1;
                updateUrl += "/atomic?commit=true&stream.body=<add><doc><query>datatype:2 AND (";

                //loop through each mailbox (stored in Item2) in current batch
                foreach (String thisCvowner in thisBatch.Item2) {
                    updateUrl += "cvowner:" + thisCvowner + " OR ";
                }

                updateUrl = updateUrl.Substring(0, updateUrl.Length - 4); //remove last ' OR ' from the url
                updateUrl += ") AND " + getCiStatusClause() + "</query><field name=\"cistatus\" update=\"set\">" + frmMain.CI_STATUS_SUCCESS + "</field></doc></add>";

                try {
                    submitHttpRequest(updateUrl);
                } catch (Exception e) {
                    mainForm.logToFile("Encountered error while re-content indexing. " + e.Message, frmMain.DEBUG_HIGH);
                    mainForm.logToFile("        Url: " + updateUrl, frmMain.DEBUG_HIGH);
                    mainForm.logToFile("", frmMain.DEBUG_HIGH);
                }

                Console.WriteLine("Done with batch " + batchCounter + " of " + batches.Count);
                batchCounter++;
            }

            return true;
        }

        //kevin
        // updates cistatus for all mailboxes in a core
        //todo: apply filter
        public bool recontentIndexCore(string corename, string filter) {
            string updateUrl = solrUrl + "/" + corename;

            updateUrl += "/atomic?commit=true&stream.body=<add><doc><query>datatype:2 AND " + getCiStatusClause() + "</query><field name=\"cistatus\" update=\"set\">" + frmMain.CI_STATUS_SUCCESS + "</field></doc></add>";

            try {
                submitHttpRequest(updateUrl);
            } catch (Exception e) {
                mainForm.logToFile("Error re-content indexing. " + e.Message, frmMain.DEBUG_HIGH);
                return false;
            }

            return true;
        }

        //kevin
        // update a cistatus FOR DATATYPE 2 ONLY based on CVOWNER
        // used for mailbox recontent index
        // Ex: http://172.24.50.90:20000/solr/usermbx6/atomic?commit=true&stream.body=<add><doc><query>datatype:2 AND cistatus:[1 TO 3] AND cvowner:74c26ba2xe307x4d4fx9691xdedbb0ea5724</query><field name="cistatus" update="set">0</field></doc></add> 
        public bool updateCistatusMailbox(int newStatus, string cvOwner) {
            string coreName = getCoreName(cvOwner);

            String updateUrl = solrUrl + "/" + coreName;

            updateUrl += "/atomic?commit=true&stream.body=<add><doc><query>datatype:2 AND cvowner:" + cvOwner + " AND " + getCiStatusClause() + "</query><field name=\"cistatus\" update=\"set\">" + newStatus + "</field></doc></add>";

            try {
                submitHttpRequest(updateUrl);
            } catch (Exception e) {
                mainForm.logToFile("Error re-content indexing. " + e.Message, frmMain.DEBUG_HIGH);
                return false;
            }

            return true;
        }

        //kevin
        // update a cistatus FOR DATATYPE 2 ONLY based on parentId
        // used for folder recontent index
        // Ex: http://172.24.50.90:20000/solr/usermbx6/atomic?commit=true&stream.body=<add><doc><query>datatype:2 AND cistatus:[1 TO 3] AND parentguid:16f27b72db58b030504e7f6b0e0ecb3f497f5fa50d88f348ab81297bfd0814ae</query><field name="cistatus" update="set">0</field></doc></add> 
        public bool updateCistatusFolder(int newStatus, string cvOwner, string parentId) {
            string coreName = getCoreName(cvOwner);

            String updateUrl = solrUrl + "/" + coreName;

            updateUrl += "/atomic?commit=true&stream.body=<add><doc><query>datatype:2 AND parentguid:" + parentId + " AND " + getCiStatusClause() + "</query><field name=\"cistatus\" update=\"set\">" + newStatus + "</field></doc></add>";

            try {
                submitHttpRequest(updateUrl);
            } catch (Exception e) {
                mainForm.logToFile("Error re-content indexing. " + e.Message, frmMain.DEBUG_HIGH);
                return false;
            }

            return true;
        }

        private string getQueryTime(XmlDocument doc) {
            string timeInMs = null;

            List<XmlDocument> resultList = new List<XmlDocument>();

            resultList.Add(doc);

            XMLParser parser = new XMLParser(resultList);

            timeInMs = parser.timeForQuery + "ms";

            return timeInMs;
        }

        //send update request based on what the user has changed
        public void update(ServerResult result) {
            List<XmlDocument> docs = new List<XmlDocument>();
            foreach (String core in coreNames) {
                String requestUrl = solrUrl + "/" + core + "/select?q=contentid:" + result.documentId + "&start=0&rows=1&wt=xml";
                docs.Add(submitHttpRequest(requestUrl));
            }

            int index = 0;
            ServerResult oldResult = new ServerResult();

            foreach (XmlDocument doc in docs) {
                String num = doc.SelectSingleNode("//result").Attributes["numFound"].Value;
                try {
                    int Num = Int32.Parse(num);
                    if (Num > 0) {
                        index = docs.IndexOf(doc);
                        List<XmlDocument> list = new List<XmlDocument>();
                        list.Add(doc);
                        XMLParser parser = new XMLParser(list);
                        oldResult = parser.results[0];
                        break;
                    }
                } catch { }
            }

            String updateUrl = solrUrl + "/" + coreNames[index] + "/update?commit=true&stream.body=<add><doc><field name=\"contentid\">" + result.documentId + "</field>";

            if (!(result.AFID.Equals(oldResult.AFID))) {
                updateUrl += "<field name=\"afid\" update = \"set\">" + result.AFID + "</field>";
            }

            if (!(result.ccn.Equals(oldResult.ccn))) {
                updateUrl += "<field name=\"ccn\" update = \"set\">" + result.ccn + "</field>";
            }

            if (!(result.clid.Equals(oldResult.clid))) {
                updateUrl += "<field name=\"clid\" update = \"set\">" + result.clid + "</field>";
            }

            if (!(result.appGuid.Equals(oldResult.appGuid))) {
                updateUrl += "<field name=\"appGUID\" update = \"set\">" + result.appGuid + "</field>";
            }

            if (!(result.atyp.Equals(oldResult.atyp))) {
                updateUrl += "<field name=\"atyp\" update = \"set\">" + result.atyp + "</field>";
            }

            if (!(result.jid.Equals(oldResult.jid))) {
                updateUrl += "<field name=\"jid\" update = \"set\">" + result.jid + "</field>";
            }

            if (!(result.bktm.Equals(oldResult.bktm))) {
                updateUrl += "<field name=\"bktm\" update = \"set\">" + result.bktm + "</field>";
            }

            if (!(result.cvbkpendtm.Equals(oldResult.cvbkpendtm))) {
                updateUrl += "<field name=\"cvbkpendtm\" update = \"set\">" + result.cvbkpendtm + "</field>";
            }

            if (!(result.visibility.Equals(oldResult.visibility))) {
                updateUrl += "<field name=\"visible\" update = \"set\">" + result.visibility + "</field>";
            }

            if (!(result.size.Equals(oldResult.size))) {
                updateUrl += "<field name=\"size\" update = \"set\">" + result.size + "</field>";
            }

            if (!(result.modifiedTime.Equals(oldResult.modifiedTime))) {
                updateUrl += "<field name=\"msgmodifiedtime\" update = \"set\">" + result.modifiedTime + "</field>";
            }

            if (!(result.indexedAt.Equals(oldResult.indexedAt))) {
                updateUrl += "<field name=\"indexedat\" update = \"set\">" + result.indexedAt + "</field>";
            }

            if (!(result.cvStub.Equals(oldResult.cvStub))) {
                updateUrl += "<field name=\"cvstub\" update = \"set\">" + result.cvStub + "</field>";
            }

            if (!(result.AFOF.Equals(oldResult.AFOF))) {
                updateUrl += "<field name=\"afof\" update = \"set\">" + result.AFOF + "</field>";
            }

            if (!(result.apid.Equals(oldResult.apid))) {
                updateUrl += "<field name=\"apid\" update = \"set\">" + result.apid + "</field>";
            }

            if (!(result.mtm.Equals(oldResult.mtm))) {
                updateUrl += "<field name=\"mtm\" update = \"set\">" + result.mtm + "</field>";
            }

            if (!(result.links.Equals(oldResult.links))) {
                updateUrl += "<field name=\"lnks\" update = \"set\">" + result.links + "</field>";
            }

            if (!(result.cistate.Equals(oldResult.cistate))) {
                updateUrl += "<field name=\"cistate\" update = \"set\">" + result.cistate + "</field>";
            }

            if (!(result.objectEntryId.Equals(oldResult.objectEntryId))) {
                updateUrl += "<field name=\"objectEntryId\" update = \"set\">" + result.objectEntryId + "</field>";
            }

            if (!(result.subject.Equals(oldResult.subject))) {
                updateUrl += "<field name=\"conv\" update = \"set\">" + result.subject + "</field>";
            }

            if (!(result.msgClass.Equals(oldResult.msgClass))) {
                updateUrl += "<field name=\"msgclass\" update = \"set\">" + result.msgClass + "</field>";
            }

            if (!(result.sentFrom.Equals(oldResult.sentFrom))) {
                updateUrl += "<field name=\"fromdisp\" update = \"set\">" + result.sentFrom + "</field>";
            }

            if (!(result.sentTo.Equals(oldResult.sentTo))) {
                updateUrl += "<field name=\"todisp\" update = \"set\">" + result.sentTo + "</field>";
            }

            if (!(result.cvExChid.Equals(oldResult.cvExChid))) {
                updateUrl += "<field name=\"cvexchid\" update = \"set\">" + result.cvExChid + "</field>";
            }
            updateUrl += "</doc></add>";
            if (mainForm.settings[1]) {
                mainForm.txtActivity.AppendText(updateUrl.Substring(updateUrl.IndexOf("<doc>") + 5, updateUrl.IndexOf("</doc>")));
                mainForm.txtActivity.SelectionStart = mainForm.txtActivity.Text.Length;
                mainForm.txtActivity.ScrollToCaret();
            }

            submitHttpRequest(updateUrl);
        }

        //format XML document to write it in the log file
        public string beautify(XmlDocument doc) {
            if (doc == null)
                return "";

            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };
            using (XmlWriter writer = XmlWriter.Create(sb, settings)) {
                doc.Save(writer);
            }
            return sb.ToString();
        }

        //search for docs with a specific jobID
        public List<XmlDocument> jobIDSearch(String jobId, int start, String sort)
        {
            List<XmlDocument> docs = new List<XmlDocument>();
            foreach (String core in coreNames) {
                String requestUrl = solrUrl + "/" + core + "/select?q=jid:" + jobId + "&datatype:2" + "&start=" + (start * pageSize) + "&rows=" + pageSize + "&sort=" + sort;
                if (sort.Equals("msgmodifiedtime"))
                {
                    requestUrl += " desc&wt=xml";
                }
                else
                {
                    requestUrl += " asc&wt=xml";
                }
                docs.Add(submitHttpRequest(requestUrl));
            }

            return docs;
        }

        //perform full reconstruction
        public void reconstruct() {
            try {
                foreach (String core in coreNames) {
                    String requestUrl = solrUrl + "/" + core + "/atomic?stream.body=<add><doc><query increaseTick=\"false\">*:*</query><field name=\"reindix_s\" update=\"set\">6.1.0</field></doc></add>&commit=true";
                    mainForm.logToFile("Sent reconstruction request to" + requestUrl, frmMain.DEBUG_REQUEST);
                    submitHttpRequest(requestUrl);
                }
            } catch (Exception x) {
                MessageBox.Show("Reconstruct Failed. Error: " + x.Message);
                mainForm.logToFile("Reconstruction failed. Error: " + x.Message, frmMain.DEBUG_HIGH);
            }
        }

        //send request for importing CSV file
        public void add(String url) {
            try {
                HttpWebRequest req = (HttpWebRequest)(WebRequest.Create(url));
                mainForm.logToFile("Sent CSV Upload request to " + url, frmMain.DEBUG_REQUEST);
                HttpWebResponse resp = (HttpWebResponse)(req.GetResponse());
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        public ServerResult getSingleDocument(String contentid)
        {
            List<String> coreNames = getCoreNames();
            foreach (String core in coreNames)
            {
                String requestUrl = solrUrl + "/" + core + "/select?q=contentid:" + contentid + "&wt=xml";
                HttpWebRequest req = (HttpWebRequest)(WebRequest.Create(requestUrl));
                HttpWebResponse resp = (HttpWebResponse)(req.GetResponse());
                var encode = ASCIIEncoding.ASCII;
                WebHeaderCollection header = resp.Headers;
                var reader = new System.IO.StreamReader(resp.GetResponseStream(), encode);
                String response = reader.ReadToEnd();
                String decodedResponse = System.Web.HttpUtility.HtmlDecode(response);

                if (!decodedResponse.Contains("numFound=\"0\""))
                {
                    if (decodedResponse.Contains("&"))
                    {
                        decodedResponse = decodedResponse.Replace("&", "&amp;");
                    }

                    XmlDocument newDoc = new XmlDocument();
                    newDoc.LoadXml(decodedResponse);
                    List<XmlDocument> list = new List<XmlDocument>();
                    list.Add(newDoc);
                    XMLParser parser = new XMLParser(list);
                    ServerResult result = parser.results[0];
                    return result;
                }
                else
                {
                    //not in this core; skip to minimize latency
                }
            }
            return new ServerResult();
        }

        public List<XmlDocument> getSubfoldersForJournalMBX(String parentguid)
        {
            try
            {
                List<XmlDocument> subfolders = new List<XmlDocument>();
                String requestUrl = solrUrl + "/journalmbx" + "/select?q=datatype:4 AND parentguid:" + parentguid + "&rows=" + numberOfDocsInEachCore[0] + "&wt=xml";
                subfolders.Add(submitHttpRequest(requestUrl));
                if (mainForm.settings[1])
                {
                    mainForm.txtActivity.AppendText("q=datatype:4 AND parentguid:" + parentguid);
                    mainForm.txtActivity.AppendText(Environment.NewLine);
                }
                return subfolders;
            }
            catch
            {
                return null;
            }
        }

        public String atomicUpdate(List<String> parameters, List<String> updateFields, String core, int updateRows)
        {
            List<String> formattedParams = new List<string>();
            for (int x = 0; x < parameters.Count; x++ )
            {
                String line = parameters[x];
                if (line.Contains(" OR "))
                {
                    line = line.Insert(0, "(");
                    line = line.Insert(line.Length, ")");
                    int i = line.IndexOf(" OR ");
                    while (i >= 0)
                    {
                        line = line.Insert(i, ")");
                        line = line.Insert(i + 5, "(");
                        i = line.IndexOf(" OR ", i + 2);
                    }
                }
                line = line.Insert(0, "(");
                line = line.Insert(line.Length, ")");
                formattedParams.Add(line);
            }

            //create queryString

            String queryString = "<query><![CDATA[("; 
            foreach (String formattedParam in formattedParams)
            {
                queryString += formattedParam;
                if(!(formattedParams.IndexOf(formattedParam) == formattedParams.Count - 1)){
                    queryString += " AND ";
                }
                
            }
            queryString += ")]]></query>";

            //create field string(s)
            List<String> fieldStrings = new List<string>();
            foreach (String fields in updateFields)
            {
                String fieldString = "<field name=\"" + fields.Substring(0, fields.IndexOf(":")) + "\" update=\"set\" ><![CDATA[" + fields.Substring(fields.IndexOf(":") + 1) + "]]></field>";
                fieldStrings.Add(fieldString);
            }

            String fullUrl = solrUrl + "/" + core + "/atomic/?stream.body=<add><doc>" + queryString;

            foreach (String fieldString in fieldStrings)
            {
                fullUrl += fieldString;
            }

            fullUrl += "</doc></add>&softCommit=true&updateRows=" + updateRows + "&disableAtomicCheck=true&start=0&rows=10&q=&debugQuery=false";

            //submitHttpRequest(fullUrl);
            try
            {
                mainForm.logToFile("Sent atomic update request to" + fullUrl, frmMain.DEBUG_REQUEST);
            }
            catch{
                mainForm.logToFile("ERROR sending update request to " + fullUrl + " ;", frmMain.DEBUG_HIGH);
            }
            return "Query took: " + getQueryTime(submitHttpRequest(fullUrl));
        }

        public List<XmlDocument> segmentReport()
        {
            List<XmlDocument> reports = new List<XmlDocument>();
            foreach (String core in coreNames)
            {
                String url = solrUrl + "/" + core + "/admin/segments";
                reports.Add(submitHttpRequest(url));
            }
            return reports;
        }

        public List<XmlDocument> contentReport()
        {
            List<XmlDocument> docs = new List<XmlDocument>();
            foreach (String core in coreNames)
            {
                String url = solrUrl + "/admin/cores?action=STATUS&core=" + core + "&wt=xml";
                docs.Add(submitHttpRequest(url));
            }
            return docs;
        }

        public XmlDocument getSmartFolderMessages(int start, String fieldString, String core, String cvowner)
        {
            String url = solrUrl + "/" + core + "/select?q=" + fieldString + " AND cvowner:" + cvowner + "&sort=msgmodifiedtime desc&start=" + (start * pageSize) + "&rows=" + pageSize + "&wt=xml";
            return submitHttpRequest(url);
        }

        public List<String> getJids(String core, String cvowner)
        {
            List<String> jids = new List<string>();
            String url = solrUrl + "/" + core + "/select?q=cvowner:" + cvowner + " AND datatype:2&facet=on&facet.field=jid&facet.limit=-1&facet.mincount=1&rows=0&wt=xml";
            XmlDocument doc = submitHttpRequest(url);
            XmlNode jidsNode = doc.SelectSingleNode("//lst[@name='jid']");
            foreach (XmlNode child in jidsNode.ChildNodes)
            {
                jids.Add(child.Attributes[0].Value);
            }
            return jids;
        }

        public XmlDocument getJidSmartFolderMessages(String core, String cvowner, String jid, int start)
        {
            String url = solrUrl + "/" + core + "/select?q=cvowner:" + cvowner + " AND jid:" + jid + " AND datatype:2&sort=msgmodifiedtime desc&start=" + (start * pageSize) + "&rows=" + pageSize + "&wt=xml";
            return submitHttpRequest(url);
        }

        public XmlDocument customSelectionRequest(List<String> selections, String core, String cvowner, int start)
        {
            String url = solrUrl + "/" + core + "/select?q=cvowner:" + cvowner + " AND ";
            foreach (String selection in selections)
            {
                url += selection;
                if (selections.IndexOf(selection) != selections.Count - 1)
                {
                    url += " AND ";
                }
            }
            url += "&sort = msgmodifiedtime desc&start=" + (start * pageSize) + "&rows=" + pageSize + "&wt=xml";
            return submitHttpRequest(url);
        }

        public List<List<int>> CoreLevelReport()
        {
            List<int> backupCounts = new List<int>();
            foreach (String core in coreNames)
            {
                String url = solrUrl + "/" + core + "/select?q=cistate:1&wt=xml";
                XmlDocument doc = submitHttpRequest(url);
                //XmlNode numFoundNode = doc.SelectSingleNode("//int[@name='numFound']");
                XmlNode resultNode = doc.SelectSingleNode("//result");
                String numFound = resultNode.Attributes[1].Value;
                backupCounts.Add(Int32.Parse(numFound));
            }

            List<int> toBeContentIndexed = new List<int>();
            foreach (String core in coreNames)
            {
                String url = solrUrl + "/" + core + "/select?q=cistatus:0&wt=xml";
                XmlDocument doc = submitHttpRequest(url);
                XmlNode resultNode = doc.SelectSingleNode("//result");
                String numFound = resultNode.Attributes[1].Value;
                toBeContentIndexed.Add(Int32.Parse(numFound));
            }

            List<int> totalCISuccess = new List<int>();
            foreach (String core in coreNames)
            {
                String url = solrUrl + "/" + core + "/select?q=cistatus:1&wt=xml";
                XmlDocument doc = submitHttpRequest(url);
                XmlNode resultNode = doc.SelectSingleNode("//result");
                String numFound = resultNode.Attributes[1].Value;
                totalCISuccess.Add(Int32.Parse(numFound));
            }

            List<int> totalCIFailed = new List<int>();
            foreach (String core in coreNames)
            {
                String url = solrUrl + "/" + core + "/select?q=cistatus:2&wt=xml";
                XmlDocument doc = submitHttpRequest(url);
                XmlNode resultNode = doc.SelectSingleNode("//result");
                String numFound = resultNode.Attributes[1].Value;
                totalCIFailed.Add(Int32.Parse(numFound));
            }

            List<List<int>> temp = new List<List<int>>();
            temp.Add(backupCounts);
            temp.Add(toBeContentIndexed);
            temp.Add(totalCISuccess);
            temp.Add(totalCIFailed);

            return temp;
        }

        public ContentIndexedInfo MailboxLevelReport(String cvowner, String mailboxCore)
        {
            int backupCounts;
            String url = solrUrl + "/" + mailboxCore + "/select?q=cistate:1 AND cvowner: " + cvowner + "&wt=xml";
            XmlDocument doc = submitHttpRequest(url);
            XmlNode resultNode = doc.SelectSingleNode("//result");
            String numFound = resultNode.Attributes[1].Value;
            backupCounts = Int32.Parse(numFound);

            int toBeContentIndexed;
            url = solrUrl + "/" + mailboxCore + "/select?q=cistatus:0 AND cvowner: " + cvowner + "&wt=xml";
            doc = submitHttpRequest(url);
            resultNode = doc.SelectSingleNode("//result");
            numFound = resultNode.Attributes[1].Value;
            toBeContentIndexed = Int32.Parse(numFound);

            int totalCISuccess;
            url = solrUrl + "/" + mailboxCore + "/select?q=cistatus:1 AND cvowner: " + cvowner + "&wt=xml";
            doc = submitHttpRequest(url);
            resultNode = doc.SelectSingleNode("//result");
            numFound = resultNode.Attributes[1].Value;
            totalCISuccess = Int32.Parse(numFound);

            int totalCIFailed;
            url = solrUrl + "/" + mailboxCore + "/select?q=cistatus:2 AND cvowner: " + cvowner + "&wt=xml";
            doc = submitHttpRequest(url);
            resultNode = doc.SelectSingleNode("//result");
            numFound = resultNode.Attributes[1].Value;
            totalCIFailed = Int32.Parse(numFound);

            int lastBackupJobId;
            url = solrUrl + "/" + mailboxCore + "/select?q=cistate:1 AND cvowner: " + cvowner + "&sort=msgmodifiedtime desc&wt=xml";
            doc = submitHttpRequest(url);
            resultNode = doc.SelectSingleNode("//result");
            numFound = resultNode.Attributes[1].Value;
            int childNodeCount = Int32.Parse(numFound);
            if (childNodeCount > 0)
            {
                XmlNode lastBackedUpDoc = resultNode.FirstChild;
                XmlNode jidNode = lastBackedUpDoc.SelectSingleNode("//int[@name='jid']");
                Int32.TryParse(jidNode.InnerText, out lastBackupJobId); 
            }
            else
            {
                lastBackupJobId = -1;
            }

            int lastContentIndexedJobId;
            url = solrUrl + "/" + mailboxCore + "/select?q=cistatus:0 AND cvowner: " + cvowner + "&sort=msgmodifiedtime desc&wt=xml";
            doc = submitHttpRequest(url);
            resultNode = doc.SelectSingleNode("//result");
            numFound = resultNode.Attributes[1].Value;
            childNodeCount = Int32.Parse(numFound);
            if (childNodeCount > 0)
            {
                XmlNode lastBackedUpDoc = resultNode.FirstChild;
                XmlNode jidNode = lastBackedUpDoc.SelectSingleNode("//int[@name='jid']");
                Int32.TryParse(jidNode.InnerText, out lastContentIndexedJobId);
            }
            else
            {
                lastContentIndexedJobId = -1;
            }

            ContentIndexedInfo cii = new ContentIndexedInfo();
            cii.totalBackupCount = backupCounts;
            cii.totalToBeContentIndexed = toBeContentIndexed;
            cii.totalCISuccess = totalCISuccess;
            cii.totalCIFailed = totalCIFailed;
            cii.lastBackupJobId = lastBackupJobId;
            cii.lastContentIndexedJobId = lastContentIndexedJobId;

            return cii;
        }

        public List<Tuple<string, int>> coreLevelArchivingReport(String core, DateTime beginningTime, DateTime endTime, String unit, int value)
        {
            String url = solrUrl + "/" + core + "/select?q=*:*&rows=0&json.facet={ %22dateRange%22: { type: range, field: indexedat , start: %22NOW/SECOND-" + value;
            if (unit.Equals("hours"))
            {
                url += "HOUR%22";
            }
            else
            {
                url += "DAY%22";
            }
            url += ", end: %22NOW/SECOND%22, gap: %22%2B";
            if (unit.Equals("hours"))
            {
                url += "1HOUR%22";
            }
            else
            {
                url += "1DAY%22";
            }
            url += ", mincount: 0 } }";
            XmlDocument doc = submitHttpRequest(url);
            XmlNode bucketsNode = doc.SelectSingleNode("//arr[@name='buckets']");
            List<Tuple<string, int>> list = new List<Tuple<string, int>>();
            foreach (XmlNode lst in bucketsNode.ChildNodes)
            {
                Tuple<String, int> tuple = new Tuple<string, int>(lst.ChildNodes[0].InnerText, Int32.Parse(lst.ChildNodes[1].InnerText));
                list.Add(tuple);
            }
            return list;
        }

        public List<List<Tuple<string, int>>> coreLevelArchivingReport(List<String> cores, DateTime beginningTime, DateTime endTime, String unit, int value)
        {
            List<List<Tuple<string, int>>> var = new List<List<Tuple<string, int>>>();
            foreach (String core in cores)
            {
                String url = solrUrl + "/" + core + "/select?q=*:*&rows=0&json.facet={ %22dateRange%22: { type: range, field: indexedat , start: %22NOW/SECOND-" + value;
                if (unit.Equals("hours"))
                {
                    url += "HOUR%22";
                }
                else
                {
                    url += "DAY%22";
                }
                url += ", end: %22NOW/SECOND%22, gap: %22%2B";
                if (unit.Equals("hours"))
                {
                    url += "1HOUR%22";
                }
                else
                {
                    url += "1DAY%22";
                }
                url += ", mincount: 0 } }";
                XmlDocument doc = submitHttpRequest(url);
                XmlNode bucketsNode = doc.SelectSingleNode("//arr[@name='buckets']");
                List<Tuple<string, int>> list = new List<Tuple<string, int>>();
                foreach (XmlNode lst in bucketsNode.ChildNodes)
                {
                    Tuple<String, int> tuple = new Tuple<string, int>(lst.ChildNodes[0].InnerText, Int32.Parse(lst.ChildNodes[1].InnerText));
                    list.Add(tuple);
                }
                var.Add(list);
            }
            return var;
        }

        public List<Tuple<String, int>> mailboxLevelArchivingReport(String core, DateTime beginningTime, DateTime endTime, String unit, int value, String cvowner)
        {
            String url = solrUrl + "/" + core + "/select?q=cvowner:" + cvowner + "&rows=0&json.facet={ %22dateRange%22: { type: range, field: indexedat , start: %22NOW/SECOND-" + value;
            if (unit.Equals("hours"))
            {
                url += "HOUR%22";
            }
            else
            {
                url += "DAY%22";
            }
            url += ", end: %22NOW/SECOND%22, gap: %22%2B";
            if (unit.Equals("hours"))
            {
                url += "1HOUR%22";
            }
            else
            {
                url += "1DAY%22";
            }
            url += ", mincount: 0 } }";
            XmlDocument doc = submitHttpRequest(url);
            XmlNode bucketsNode = doc.SelectSingleNode("//arr[@name='buckets']");
            List<Tuple<string, int>> list = new List<Tuple<string, int>>();
            foreach (XmlNode lst in bucketsNode.ChildNodes)
            {
                Tuple<String, int> tuple = new Tuple<string, int>(lst.ChildNodes[0].InnerText, Int32.Parse(lst.ChildNodes[1].InnerText));
                list.Add(tuple);
            }
            return list;
        }

        public List<List<PerformanceInfo>> performanceCounterReport()
        {
            List<XmlDocument> docs = new List<XmlDocument>();
            foreach (String core in coreNames)
            {
                String url = solrUrl + "/" + core + "/atomic?perfcounter=true";
                XmlDocument doc = submitHttpRequest(url);
                docs.Add(doc);
            }
            XMLParser parser = new XMLParser(docs, this.mainForm, 0);
            List<List<PerformanceInfo>> llpi = parser.performanceInfo;
            return llpi;
        }

        public List<List<PerformanceInfo>> performanceCounterReport(String core)
        {
            List<XmlDocument> docs = new List<XmlDocument>();
            String url = solrUrl + "/" + core + "/atomic?perfcounter=true";
            XmlDocument doc = submitHttpRequest(url);
            docs.Add(doc);
            XMLParser parser = new XMLParser(docs, this.mainForm, 0);
            List<List<PerformanceInfo>> llpi = parser.performanceInfo;
            return llpi;
        }
    }
}