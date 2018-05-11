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
    public partial class AtomicUpdate : Form
    {
        List<String> parameterList = new List<String>();
        List<String> updateList = new List<String>();
        List<Tuple<String, Boolean>> parametersUsedList = new List<Tuple<string, bool>>();
        List<Tuple<String, Boolean>> updatedFieldsList = new List<Tuple<string, bool>>();
        MonitorFolderActivity.frmMain form;
        String ip;
        SolrQuery query;

        public AtomicUpdate()
        {
            InitializeComponent();
            populateParametersUsedList();
            populateUpdatedFieldsList();
            form = null;
            ip = null;
            query = null;
        }

        public AtomicUpdate(MonitorFolderActivity.frmMain form, String ip)
        {
            InitializeComponent();
            populateParametersUsedList();
            populateUpdatedFieldsList();
            this.form = form;
            this.ip = ip;
            this.query = new SolrQuery(ip, form);
            //coreComboBox.Items.Add("usermbx");
            try
            {
                coreComboBox.Items.AddRange(query.coreNames.ToArray());
            }
            catch(Exception e){
                form.logToFile(e.Message, MonitorFolderActivity.frmMain.DEBUG_HIGH);
            }
            if (coreComboBox.Items.Count == 0)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Abort;
                this.Close();
            }
        }

        private void addParameterButton_Click(object sender, EventArgs e)
        {
            String selectedField = fieldSelectionBox.Text;
            String selectedFieldValue = FieldValueBox.Text;
            int index = -1;
            bool found = false;
            foreach(Tuple<String, Boolean> parameter in parametersUsedList){
                if((selectedField.Equals(parameter.Item1)) && (parameter.Item2)){
                    index = parametersUsedList.IndexOf(parameter);
                    found = true;
                    break;
                }
            }

            if (found)
            {
                parametersUsedList[index] = new Tuple<string, bool>(parametersUsedList[index].Item1, true);

                for (int x = 0; x < selectionsBox.Lines.Length - 1; x++)
                {
                    String field = selectionsBox.Lines[x].Substring(0, selectionsBox.Lines[x].IndexOf(":"));
                    if (field.Equals(fieldSelectionBox.Text))
                    {
                        String[] lines = selectionsBox.Lines;
                        lines[x] = selectionsBox.Lines[x] + " OR " + selectedField + ":" + selectedFieldValue;
                        selectionsBox.Lines = lines;
                    }
                }
            }
            else
            {
                selectionsBox.AppendText(selectedField + ":" + selectedFieldValue);
                selectionsBox.AppendText(Environment.NewLine);
                Tuple<String, Boolean> temp = new Tuple<string, bool>(selectedField, false);
                index = parametersUsedList.IndexOf(temp);
                parametersUsedList[index] = new Tuple<string, bool>(selectedField, true);
            }
            fieldSelectionBox.Text = "";
            FieldValueBox.Text = "";
        }

        private void populateParametersUsedList()
        {
            parametersUsedList.Add(new Tuple<string, bool>("afid", false));
            parametersUsedList.Add(new Tuple<string, bool>("afof", false));
            parametersUsedList.Add(new Tuple<string, bool>("apid", false));
            parametersUsedList.Add(new Tuple<string, bool>("appGUID", false));
            parametersUsedList.Add(new Tuple<string, bool>("atyp", false));
            parametersUsedList.Add(new Tuple<string, bool>("bktm", false));
            parametersUsedList.Add(new Tuple<string, bool>("ccdisp", false));
            parametersUsedList.Add(new Tuple<string, bool>("ccn", false));
            parametersUsedList.Add(new Tuple<string, bool>("cijid", false));
            parametersUsedList.Add(new Tuple<string, bool>("cistate", false));
            parametersUsedList.Add(new Tuple<string, bool>("clid", false));
            parametersUsedList.Add(new Tuple<string, bool>("contentid", false));
            parametersUsedList.Add(new Tuple<string, bool>("conv", false));
            parametersUsedList.Add(new Tuple<string, bool>("cvbkpendtm", false));
            parametersUsedList.Add(new Tuple<string, bool>("cvexchid", false));
            parametersUsedList.Add(new Tuple<string, bool>("cvowner", false));
            parametersUsedList.Add(new Tuple<string, bool>("cvownerdisp", false));
            parametersUsedList.Add(new Tuple<string, bool>("cvstub", false));
            parametersUsedList.Add(new Tuple<string, bool>("datatype", false));
            parametersUsedList.Add(new Tuple<string, bool>("folder", false));
            parametersUsedList.Add(new Tuple<string, bool>("fromdisp", false));
            parametersUsedList.Add(new Tuple<string, bool>("hasAttach", false));
            parametersUsedList.Add(new Tuple<string, bool>("indexedat", false));
            parametersUsedList.Add(new Tuple<string, bool>("jid", false));
            parametersUsedList.Add(new Tuple<string, bool>("lnks", false));
            parametersUsedList.Add(new Tuple<string, bool>("msgclass", false));
            parametersUsedList.Add(new Tuple<string, bool>("msgmodifiedtime", false));
            parametersUsedList.Add(new Tuple<string, bool>("mtm", false));
            parametersUsedList.Add(new Tuple<string, bool>("parentguid", false));
            parametersUsedList.Add(new Tuple<string, bool>("retentionjid", false));
            parametersUsedList.Add(new Tuple<string, bool>("szkb", false));
            parametersUsedList.Add(new Tuple<string, bool>("todisp", false));
            parametersUsedList.Add(new Tuple<string, bool>("visible", false));
        }

        private void populateUpdatedFieldsList()
        {
            updatedFieldsList.Add(new Tuple<string, bool>("afid", false));
            updatedFieldsList.Add(new Tuple<string, bool>("afof", false));
            updatedFieldsList.Add(new Tuple<string, bool>("apid", false));
            updatedFieldsList.Add(new Tuple<string, bool>("appGUID", false));
            updatedFieldsList.Add(new Tuple<string, bool>("atyp", false));
            updatedFieldsList.Add(new Tuple<string, bool>("bktm", false));
            updatedFieldsList.Add(new Tuple<string, bool>("ccdisp", false));
            updatedFieldsList.Add(new Tuple<string, bool>("ccn", false));
            updatedFieldsList.Add(new Tuple<string, bool>("cijid", false));
            updatedFieldsList.Add(new Tuple<string, bool>("cistate", false));
            updatedFieldsList.Add(new Tuple<string, bool>("clid", false));
            updatedFieldsList.Add(new Tuple<string, bool>("contentid", false));
            updatedFieldsList.Add(new Tuple<string, bool>("conv", false));
            updatedFieldsList.Add(new Tuple<string, bool>("cvbkpendtm", false));
            updatedFieldsList.Add(new Tuple<string, bool>("cvexchid", false));
            updatedFieldsList.Add(new Tuple<string, bool>("cvowner", false));
            updatedFieldsList.Add(new Tuple<string, bool>("cvownerdisp", false));
            updatedFieldsList.Add(new Tuple<string, bool>("cvstub", false));
            updatedFieldsList.Add(new Tuple<string, bool>("datatype", false));
            updatedFieldsList.Add(new Tuple<string, bool>("folder", false));
            updatedFieldsList.Add(new Tuple<string, bool>("fromdisp", false));
            updatedFieldsList.Add(new Tuple<string, bool>("hasAttach", false));
            updatedFieldsList.Add(new Tuple<string, bool>("indexedat", false));
            updatedFieldsList.Add(new Tuple<string, bool>("jid", false));
            updatedFieldsList.Add(new Tuple<string, bool>("lnks", false));
            updatedFieldsList.Add(new Tuple<string, bool>("msgclass", false));
            updatedFieldsList.Add(new Tuple<string, bool>("msgmodifiedtime", false));
            updatedFieldsList.Add(new Tuple<string, bool>("mtm", false));
            updatedFieldsList.Add(new Tuple<string, bool>("parentguid", false));
            updatedFieldsList.Add(new Tuple<string, bool>("retentionjid", false));
            updatedFieldsList.Add(new Tuple<string, bool>("szkb", false));
            updatedFieldsList.Add(new Tuple<string, bool>("todisp", false));
            updatedFieldsList.Add(new Tuple<string, bool>("visible", false));
        }

        private void addFieldUpdateButton_Click(object sender, EventArgs e)
        {
            String selectedField = updatedFieldSelectionBox.Text;
            String selectedFieldValue = updatedFieldValueBox.Text;
            if ((!(selectedField.Equals(""))) && (!(selectedFieldValue.Equals(""))))
            {
                Tuple<String, Boolean> temp = new Tuple<string, bool>(selectedField, false);
                if (updatedFieldsList.IndexOf(temp) > -1)
                {
                    updatesBox.AppendText(selectedField + ":" + selectedFieldValue);
                    updatesBox.AppendText(Environment.NewLine);
                    Tuple<String, Boolean> temp2 = new Tuple<string, bool>(selectedField, true);
                    updatedFieldsList[updatedFieldsList.IndexOf(temp)] = temp2;
                    updatedFieldSelectionBox.Text = "";
                    updatedFieldValueBox.Text = "";
                }
                else
                {
                    MessageBox.Show("You already specified a value for that field.");
                }
            }
            else
            {
                MessageBox.Show("Invalid arguments.");
            }
            
        }

        private void submitAtomicUpdateButton_Click(object sender, EventArgs e)
        {
            List<String> parameters = new List<string>();
            foreach (String line in selectionsBox.Lines)
            {
                parameters.Add(line);
            }
            parameters.RemoveAt(parameters.Count - 1);

            List<String> updateFields = new List<string>();
            foreach (String line in updatesBox.Lines)
            {
                updateFields.Add(line);
            }
            updateFields.RemoveAt(updateFields.Count - 1);

            String core = coreComboBox.Text;
            int updateRows = 0;
            Int32.TryParse(updateRowsBox.Text, out updateRows);

            if (selectionsBox.Text == "")
            {
                selectionsBox.AppendText("*:*");
                selectionsBox.AppendText(Environment.NewLine);
            }

            if (updatesBox.Text == "")
            {
                MessageBox.Show("You didn't specify a field to update");
            }
            else
            {
                query.atomicUpdate(parameters, updateFields, core, updateRows);
            }
            this.Close();
        }
    }
}
