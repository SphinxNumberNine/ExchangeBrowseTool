using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using MonitorFolderActivity;

namespace ExchangeOnePassIdxGUI {

    class XMLParser {
        public List<ServerResult> results;
        public List<SegmentInfo> segmentInfo;
        public List<ContentIndexedInfo> contentInfo;
        public List<List<PerformanceInfo>> performanceInfo;
        public int timeForQuery = 0;
        public int numFound = 0;
        public int updatedRows = -1;
        public XMLParser(List<XmlDocument> results) {
            this.results = readResultsFromXml(results);
        }

        public XMLParser(List<XmlDocument> segmentInfoList, frmMain form)
        {
            segmentInfo = readSegmentInfoFromXml(segmentInfoList);
        }

        public XMLParser(List<XmlDocument> contentInfoList, frmMain form, bool x)
        {
            contentInfo = readContentInfoFromXml(contentInfoList);
        }

        public XMLParser(List<XmlDocument> performanceInfoList, frmMain form, int i)
        {
            performanceInfo = readPerformanceInfoFromXml(performanceInfoList);
        }
        
        public XMLParser(XmlDocument doc)
        {
            updatedRows = getAtomicUpdatedRows(doc);
        }

        private List<ServerResult> readResultsFromXml(List<XmlDocument> docs) {
            try {
                List<ServerResult> results = new List<ServerResult>();
                foreach (XmlDocument doc in docs) {
                    XmlNode timeNode = doc.SelectSingleNode("//int[@name='QTime']");
                    String time = timeNode.InnerText;
                    timeForQuery += Int32.Parse(time);
                    XmlNode resultNode = doc.SelectSingleNode("//result");
                    numFound = Int32.Parse(resultNode.Attributes[1].Value);
                    foreach (XmlNode children in resultNode.ChildNodes) {
                        try {
                            ServerResult newResult = new ServerResult();
                            newResult.AFID = getInfoFromNode(children, "long[@name='afid']");
                            newResult.dataType = getInfoFromNode(children, "int[@name='datatype']"); 
                            newResult.documentId = getInfoFromNode(children, "str[@name='contentid']"); 
                            newResult.parentGuid = getInfoFromNode(children, "str[@name='parentguid']");
                            newResult.mailbox = getInfoFromNode(children, "str[@name='cvownerdisp']"); 
                            newResult.ccn = getInfoFromNode(children, "int[@name='ccn']"); 
                            newResult.clid = getInfoFromNode(children, "int[@name='clid']"); 
                            newResult.appGuid = getInfoFromNode(children, "str[@name='appGUID']");
                            newResult.atyp = getInfoFromNode(children, "int[@name='atyp']"); 
                            newResult.jid = getInfoFromNode(children, "int[@name='jid']");
                            newResult.bktm = getInfoFromNode(children, "date[@name='bktm']");
                            newResult.cvbkpendtm = getInfoFromNode(children, "date[@name='cvbkpendtm']");
                            newResult.visibility = getInfoFromNode(children, "bool[@name='visible']");
                            newResult.size = getInfoFromNode(children, "long[@name='szkb']");
                            newResult.cvowner = getInfoFromNode(children, "str[@name='cvowner']");
                            newResult.modifiedTime = getInfoFromNode(children, "date[@name='msgmodifiedtime']");
                            newResult.version = getInfoFromNode(children, "long[@name='_version_']");
                            newResult.indexedAt = getInfoFromNode(children, "date[@name='indexedat']");
                            newResult.cvStub = getInfoFromNode(children, "int[@name='cvstub']");
                            newResult.AFOF = getInfoFromNode(children, "long[@name='afof']");
                            newResult.apid = getInfoFromNode(children, "int[@name='apid']");
                            newResult.mtm = getInfoFromNode(children, "date[@name='mtm']");
                            newResult.links = getInfoFromNode(children, "str[@name='lnks']");
                            newResult.cistate = getInfoFromNode(children, "int[@name='cistate']");
                            if ((newResult.dataType.Equals("4")) || (newResult.dataType.Equals("2"))) 
                            {
                                newResult.objectEntryId = getInfoFromNode(children, "str[@name='objectEntryId']"); 
                                newResult.subject = getInfoFromNode(children, "str[@name='conv']");
                                if (newResult.dataType.Equals("2")) 
                                {
                                    newResult.msgClass = getInfoFromNode(children, "str[@name='msgclass']"); 
                                    newResult.folder = getInfoFromNode(children, "str[@name='folder']"); 
                                    newResult.sentFrom = getInfoFromNode(children, "str[@name='fromdisp']"); 
                                    newResult.fmsmtp = getInfoFromNode(children, "str[@name='fmsmtp']"); 
                                    newResult.sentTo = getInfoFromNode(children, "str[@name='todisp']");
                                    newResult.tosmtp = getInfoFromNode(children, "str[@name='tosmtp']"); 
                                    newResult.cvExChid = getInfoFromNode(children, "str[@name='cvexchid']"); 
                                    newResult.sentCC = getInfoFromNode(children, "str[@name='ccdisp']"); 
                                    newResult.ccsmtp = getInfoFromNode(children, "str[@name='ccsmtp']");
                                    newResult.hasAttach = getInfoFromNode(children, "bool[@name='hasattach']"); 
                                    newResult.atts = getInfoFromNode(children, "str[@name='atts']"); 
                                }
                            }
                            results.Add(newResult);
                        } 
                        catch(Exception e) {
                            Console.WriteLine(e.Message);
                        };
                    }
                }
                return results; //make sure results.count is 1000.
            } catch {
                return null;
            }
        }

        private List<SegmentInfo> readSegmentInfoFromXml(List<XmlDocument> docs)
        {
            try
            {
                List<SegmentInfo> segment_Info = new List<SegmentInfo>();
                foreach (XmlDocument doc in docs)
                {
                    SegmentInfo segment = new SegmentInfo();
                    XmlNode timeNode = doc.SelectSingleNode("//int[@name='QTime']");
                    String time = timeNode.InnerText;
                    XmlNode segmentsNode = doc.SelectSingleNode("//lst[@name='segments']");
                    segment.numSegments = segmentsNode.ChildNodes.Count;
                    timeForQuery += Int32.Parse(time);
                    foreach(XmlNode child in segmentsNode.ChildNodes)
                    {
                        if (child.ChildNodes[5].InnerText.Equals("merge"))
                        {
                            segment.lastMergeDate = child.ChildNodes[4].InnerText;
                            break;
                        }
                    }
                    segment_Info.Add(segment);
                }
                return segment_Info;
            }
            catch
            {
                return null;
            }
        }

        private List<ContentIndexedInfo> readContentInfoFromXml(List<XmlDocument> docs)
        {
            try
            {
                List<ContentIndexedInfo> contentIndexedInfoDocs = new List<ContentIndexedInfo>();
                foreach (XmlDocument doc in docs)
                {
                    ContentIndexedInfo temp = new ContentIndexedInfo();
                    XmlNode timeNode = doc.SelectSingleNode("//int[@name='QTime']");
                    String time = timeNode.InnerText;
                    timeForQuery += Int32.Parse(time);
                    XmlNode statusNode = doc.SelectSingleNode("//lst[@name='status']");
                    XmlNode coreNode = statusNode.FirstChild;
                    XmlNode indexNode = statusNode.SelectSingleNode("//lst[@name='index']");
                    XmlNode numDocsNode = indexNode.FirstChild;
                    int numDocs = Int32.Parse(numDocsNode.InnerText);
                    temp.numberOfDocuments = numDocs;
                    contentIndexedInfoDocs.Add(temp);
                }
                return contentIndexedInfoDocs;
            }
            catch
            {
                return null;
            }
        }

        private int getAtomicUpdatedRows(XmlDocument doc)
        {
            XmlNode updatedRowsNode = doc.SelectSingleNode("//int[@name='updatedRows']");
            updatedRows = Int32.Parse(updatedRowsNode.InnerText);
            return updatedRows;
        }

        public String getInfoFromNode(XmlNode node, String xpath)
        {
            try
            {
                return node.SelectSingleNode(xpath).InnerText;
            }
            catch
            {
                return null;
            }
        }

        public List<List<PerformanceInfo>> readPerformanceInfoFromXml(List<XmlDocument> docs)
        {
            List<List<PerformanceInfo>> llpi = new List<List<PerformanceInfo>>();
            foreach (XmlDocument doc in docs)
            {
                try
                {
                    List<PerformanceInfo> lpi = new List<PerformanceInfo>();
                    XmlNode perfCounterNode = doc.SelectSingleNode("//lst[@name='perfcounter']");
                    foreach (XmlNode childNodes in perfCounterNode)
                    {
                        PerformanceInfo pi = new PerformanceInfo();
                        pi.operationName = childNodes.Attributes[0].Value;
                        XmlNode countNode = childNodes.ChildNodes[0];
                        XmlNode timeTakenNode = childNodes.ChildNodes[1];
                        XmlNode avgTimeTakenNode = childNodes.ChildNodes[2];
                        pi.count = Int32.Parse(countNode.InnerText);
                        pi.timeTaken = long.Parse(timeTakenNode.InnerText);
                        long.TryParse(avgTimeTakenNode.InnerText, out pi.avgTimeTaken);
                        lpi.Add(pi);
                    }
                    llpi.Add(lpi);
                }
                catch
                {
                    return null;
                }
            }
            return llpi;
        }
    }
}