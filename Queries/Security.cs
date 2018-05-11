using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonitorFolderActivity;
using System.Windows.Forms;
using System.Threading;
using System.Xml.Linq;
using System.Web;
using System.Xml;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Net;

namespace ExchangeOnePassIdxGUI
{
    public class Security
    {
        public frmMain mainForm;
        private SolrQuery querier;
        private string address;
        public bool superUser;
        public LogInfo userCreds;
        public string webServerName;
        loginPage httpRequester = new loginPage();
        public string token;
        public List<string> apids;
        public List<String> coreNames;


        public Security(string address1, SolrQuery querier1, frmMain form1)
        {
            superUser = querier1.superUser;
            address = address1;
            querier = querier1;
            mainForm = form1;
            userCreds = querier.userCreds;
            webServerName = querier1.webServerName;
            token = querier1.token;
            coreNames = querier.coreNames;
            getAPIDs(address);
            if (!superUser)
            {
                getValidApids();
            }
            
        }

        //Returns list of apids on solr server as a list of ints
        public List<string> getAPIDs(String address)
        {
            apids = new List<string>();
            //OLD WAY OF OBTAINING APIDs

            /**foreach (String core in coreNames)
            {
                List<string> temp = new List<string>();
                String URL = address + "/" + core + "/select?q=*:*&facet=on&facet.field=apid&facet.limit=-1&facet.mincount=1&rows=0&wt=xml";
                XmlDocument doc = querier.submitHttpRequest(URL, frmMain.DEBUG_REQUEST);
                XDocument xDocument = XDocument.Parse(doc.OuterXml);
                var apidList = (from facetCount in xDocument.Elements("response").Elements("lst")
                                where (string)facetCount.Attribute("name") == "facet_counts"
                                from facetField in facetCount.Elements("lst")
                                where (string)facetField.Attribute("name") == "facet_fields"
                                from apid in facetField.Elements("lst")
                                where (string)apid.Attribute("name") == "apid"
                                from ele in apid.Elements()
                                where Convert.ToInt32(ele.Value.ToString()) > 0
                                select new
                                {
                                    apid = ele.Attribute("name").Value,
                                    count = ele.Value
                                }
                              );
                foreach (var apid in apidList)
                {
                    /**var tempo = apid.ToString();
                    char holder = ',';
                    var pos = tempo.IndexOf(holder);
                    tempo = tempo.Substring(pos - 4, 4);
                    temp.Add(tempo);
                    temp.Add(apid.apid + "");
                }
                foreach (string apid in temp)
                {
                    Console.WriteLine(apid);
                }
                foreach (String x in temp)
                {
                    apids.Add(x);
                }
            }*/

            //NEW WAY OF OBTAINING APIDs
            String url = address + "/" + "usermbx0" + "/select?indent=on&q=*:*&wt=xml&shards=";
            foreach (String core in coreNames)
            {
                url += address + "/" + core;
                if (!(coreNames.IndexOf(core) == coreNames.Count - 1))
                {
                    url += ",";
                }
            }
            url += "&fl=cvownerdisp,indexedat,[shard]&rows=0&facet=on&facet.field=apid";
            XmlDocument doc = querier.submitHttpRequest(url, frmMain.DEBUG_REQUEST);
            XDocument xDocument = XDocument.Parse(doc.OuterXml);
            var apidList = (from facetCount in xDocument.Elements("response").Elements("lst")
                where (string)facetCount.Attribute("name") == "facet_counts"
                from facetField in facetCount.Elements("lst")
                where (string)facetField.Attribute("name") == "facet_fields"
                from apid in facetField.Elements("lst")
                where (string)apid.Attribute("name") == "apid"
                from ele in apid.Elements()
                where Convert.ToInt32(ele.Value.ToString()) > 0
                select new
                {
                    apid = ele.Attribute("name").Value,
                    count = ele.Value
                }
                );
            foreach (var apid in apidList)
            {
                var tempo = apid.ToString();
                char holder = ',';
                var pos = tempo.IndexOf(holder);
                tempo = tempo.Substring(pos - 4, 4);
                //apids.Add(tempo); //old way, sometimes returns APIDs with '=' in the string, causing exception when connecting to SOLR
                apids.Add(apid.apid + "");
            }
            for (int x = 0; x < apids.Count; x++)
            {
                apids[x] = apids[x].Trim();
            }
            try
            {
                apids = apids.Distinct().ToList();
            }
            catch { }
            return apids;
        }

        //Returns list of apids that the user has access to
        public List<string> getValidApids()
        {
            List<string> retList = new List<string>();
            foreach (string apid in apids)
            {
                WebRequest req = WebRequest.Create(webServerName + "CheckPermissionAssocToEntity?permId=13&entityType=7&entityId=" + apid);
                req.Method = "POST";
                req.ContentType = @"application/xml; charset=utf-8";
                req.Headers.Add("Authtoken", token);
                req.ContentLength = 0;
                var resp = req.GetResponse() as HttpWebResponse;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(resp.GetResponseStream());
                xmlDoc.Save("httpResponse2.xml");
                StreamReader reader = new StreamReader("httpResponse2.xml");
                if (reader.ReadLine().IndexOf("1") > 0)
                {
                    retList.Add(apid);
                }
                reader.Close();
            }
            apids = retList;
            return retList;
        }

        //creates XML request format apid list
        public string apidString()
        {
            string retValue = "";
            List<string> validAPIDs = apids;
            for (int i = 0; i < validAPIDs.Count(); i++)
            {
                if (i == 0)
                {
                    retValue += ("(apid:" + validAPIDs[0]);
                }
                else
                {
                    retValue += (" OR apid:" + validAPIDs[i]);
                }
            }
            retValue += ")";
            return retValue;
        }
    }
}
