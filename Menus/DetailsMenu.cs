using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MonitorFolderActivity;

namespace ExchangeOnePassIdxGUI
{
    public partial class DetailsMenu : Form
    {
        ServerResult item;

        public DetailsMenu(ServerResult item)
        {
            InitializeComponent();
            this.item = item;
            this.msgclassBox.Text = item.msgClass;
            this.versionBox.Text = item.version;
            this.indexedBox.Text = item.indexedAt;
            this.cvstubBox.Text = item.cvStub;
            this.cvexchildBox.Text = item.cvExChid;
            this.datatypeBox.Text = item.dataType;
            this.apidBox.Text = item.apid;
            this.mtmBox.Text = item.mtm;
            this.linksBox.Text = item.links;
            this.cistateBox.Text = item.cistate;
            this.appGuidBox.Text = item.appGuid;
            this.ccnBox.Text = item.ccn;
            this.clidBox.Text = item.clid;
            this.atypBox.Text = item.atyp;
            this.jidBox.Text = item.jid;
            this.bktmBox.Text = item.bktm;
            this.cvpendtmBox.Text = item.cvbkpendtm;
            this.visibilityBox.Text = item.visibility;
            this.cvownerBox.Text = item.cvowner;
            this.objectEntryIdBox.Text = item.objectEntryId;
            this.guidBox.Text = item.documentId;
            this.parentGuidBox.Text = item.parentGuid;
            this.folderBox.Text = item.folder;
            this.fromBox.Text = item.sentFrom;
            this.toBox.Text = item.sentTo;
            this.subjectBox.Text = item.subject;
            this.sizeBox.Text = item.size;
            this.modifiedTimeBox.Text = item.modifiedTime;
            this.afidBox.Text = item.AFID;
            this.afofBox.Text = item.AFOF;
            this.hasAttachTextBox.Text = item.hasAttach;
            try
            {
                this.attachmentsTextBox.Text = item.atts;
            }
            catch{

            }
            if (!(frmMain.editMode))
            {
                this.Height = 315;
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                //this.MinimizeBox = false;
                this.MaximizeBox = false;
            }
        }

        public ServerResult returnNewServerResult()
        {
            ServerResult newResult = new ServerResult();
            newResult = item;
            if (!(item.msgClass.Equals(msgclassBox.Text)))
            {
                newResult.msgClass = msgclassBox.Text;
            }
            if (!(item.indexedAt.Equals(indexedBox.Text)))
            {
                newResult.indexedAt = indexedBox.Text;
            }
            if (!(item.cvStub.Equals(cvstubBox.Text)))
            {
                newResult.cvStub = cvstubBox.Text;
            }
            if(!(item.cvExChid.Equals(cvexchildBox.Text)))
            {
                newResult.cvExChid = cvexchildBox.Text;
            }
            if (!(item.apid.Equals(apidBox.Text)))
            {
                newResult.apid = apidBox.Text;
            }
            if (!(item.mtm.Equals(mtmBox.Text)))
            {
                newResult.mtm = mtmBox.Text;
            }
            if (!(item.links.Equals(linksBox.Text)))
            {
                newResult.links = linksBox.Text;
            }
            if (!(item.cistate.Equals(cistateBox.Text)))
            {
                newResult.cistate = cistateBox.Text;
            }
            if (!(item.appGuid.Equals(appGuidBox.Text)))
            {
                newResult.appGuid = appGuidBox.Text;
            }
            if (!(item.ccn.Equals(ccnBox.Text)))
            {
                newResult.ccn = ccnBox.Text;
            }
            if (!(item.clid.Equals(clidBox.Text)))
            {
                newResult.clid = clidBox.Text;
            }
            if (!(item.atyp.Equals(atypBox.Text)))
            {
                newResult.atyp = atypBox.Text;
            }
            if (!(item.jid.Equals(jidBox.Text)))
            {
                newResult.jid = jidBox.Text;
            }
            if (!(item.bktm.Equals(bktmBox.Text)))
            {
                newResult.bktm = bktmBox.Text;
            }
            if (!(item.cvbkpendtm.Equals(cvpendtmBox.Text)))
            {
                newResult.cvbkpendtm = cvpendtmBox.Text;
            }
            if (!(item.visibility.Equals(visibilityBox.Text)))
            {
                newResult.visibility = visibilityBox.Text;
            }
            if (!(item.objectEntryId.Equals(objectEntryIdBox.Text)))
            {
                newResult.objectEntryId = objectEntryIdBox.Text;
            }
            if (!(item.sentFrom.Equals(fromBox.Text)))
            {
                newResult.sentFrom = fromBox.Text;
            }
            if (!(item.sentTo.Equals(toBox.Text)))
            {
                newResult.sentTo = toBox.Text;
            }
            if (!(item.subject.Equals(subjectBox.Text)))
            {
                newResult.subject = subjectBox.Text;
            }
            if (!(item.size.Equals(sizeBox.Text)))
            {
                newResult.size = sizeBox.Text;
            }
            if (!(item.modifiedTime.Equals(modifiedTimeBox.Text)))
            {
                newResult.modifiedTime = modifiedTimeBox.Text;
            }
            if (!(item.AFID.Equals(afidBox.Text)))
            {
                newResult.AFID = afidBox.Text;
            }
            if (!(item.AFOF.Equals(afofBox.Text)))
            {
                newResult.AFOF = afofBox.Text;
            }
            return newResult;
        }
    }
}
