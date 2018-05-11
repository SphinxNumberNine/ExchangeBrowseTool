using MonitorFolderActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ExchangeOnePassIdxGUI {

    //kevin
    class Profiling {
        private bool isPaused;
        private int pauseTimeMs;
        private long beginTimeMinutes;
        private string ip;
        private string cvowner = "";
        private DateTime unixConstant;
        private Thread currentThread;
        private frmMain mainForm;

        public Profiling(frmMain mainForm, string ip) {
            this.isPaused = true;
            this.mainForm = mainForm;
            this.ip = ip;
            this.unixConstant = new DateTime(1970, 1, 1);
            
            ThreadStart newThread = new ThreadStart(QueryCIStatus);
            currentThread = new Thread(newThread);
            currentThread.IsBackground = true; //will quit thread on application exit
        }

        public void setIp(string newIp) {
            this.ip = newIp;
        }

        //display Dialog form for user to pick pause time and/or reset/stop/start timer
        public void showProfilingForm(TreeNodeCollection treeNodes) {
            ProfilingForm profilingForm = new ProfilingForm(pauseTimeMs / 1000, isPaused, treeNodes);

            var result = profilingForm.ShowDialog();

            if (result == DialogResult.OK) {

                string input = profilingForm.getNewTime();
                cvowner = profilingForm.getCvowner();

                try {
                    isPaused = false;
                    currentThread.Start();
                    //currentThread.SetApartmentState(ApartmentState.STA);
                    beginTimeMinutes = (Int64)(DateTime.UtcNow.Subtract(unixConstant)).TotalMinutes;
                } catch (ThreadStateException) {
                    // Thread already running; User clicked reset and wants to just change the pause time.
                    try {
                        currentThread.Resume();
                    } catch { }
                }

                try {
                    pauseTimeMs = Int32.Parse(input) * 1000; //convert to ms
                } catch {
                    try {
                        pauseTimeMs = (Int32)Math.Round(Double.Parse(input) * 1000); //convert to ms
                    } catch { }
                }
            
            } else if (result == DialogResult.Abort) {
                mainForm.AppendTextBox("Profiling stopped.");
                isPaused = true;
                currentThread.Suspend();
            } else if (result == DialogResult.Cancel) {

            }
        }

        // prints total status of every datatype 2 cistatus
        public void QueryCIStatus() {
            SolrQuery query = new SolrQuery(ip, mainForm);
            long lastCICount = 0;
            long lastBackupCount = 0;
            long totalCIFromStart = 0;
            long totalBackupFromStart = 0;
            long lastQueryTimeSeconds = (Int64)(DateTime.UtcNow.Subtract(unixConstant)).TotalSeconds; 

            while (true) {

                try
                {
                    long currentTimeMinutes = (Int64)(DateTime.UtcNow.Subtract(unixConstant)).TotalMinutes;
                    if (currentTimeMinutes - beginTimeMinutes >= 2*24*60)
                    {
                        mainForm.logToFileAndDisplay("Profiling auto-stopped after " + (currentTimeMinutes - beginTimeMinutes) + " minutes");
                        isPaused = true;
                        currentThread.Suspend();
                        
                        //return;
                    }

                    Dictionary<int, long> statusData = new Dictionary<int, long>();
                    statusData = query.getCIStatusCount(frmMain.CI_STATUS_TOBE, frmMain.CI_STATUS_SKIPPED, cvowner);
                    Dictionary<int, long> stateData = new Dictionary<int, long>();
                    stateData = query.getCIStateCount(cvowner);

                    if (statusData != null)
                    {
                        mainForm.logToFileAndDisplay("");
                        mainForm.logToFileAndDisplay(System.DateTime.Now.ToString() + ". Next report query filed in " + (pauseTimeMs / 1000) + " seconds.");

                        if (!cvowner.Equals(""))
                        {
                            mainForm.logToFileAndDisplay("Showing results for mailbox with cvowner: " + cvowner);
                        }

                        mainForm.logToFileAndDisplay("CI Status Results:\t\t\t\t\t\tCI State Results:");
                        mainForm.logToFileAndDisplay("CI Status To Be (" + frmMain.CI_STATUS_TOBE + "): \t" + statusData[frmMain.CI_STATUS_TOBE] +
                            "\t\t\t\t" + "CI State 0: " + stateData[0]);
                        mainForm.logToFileAndDisplay("CI Status Success (" + frmMain.CI_STATUS_SUCCESS + "): \t" + statusData[frmMain.CI_STATUS_SUCCESS] +
                            "\t\t\t\t" + "CI State 1: " + stateData[1]);
                        mainForm.logToFileAndDisplay("CI Status Failed (" + frmMain.CI_STATUS_FAILED + "): \t\t" + statusData[frmMain.CI_STATUS_FAILED]);
                        mainForm.logToFileAndDisplay("CI Status Skipped (" + frmMain.CI_STATUS_SKIPPED + "): \t" + statusData[frmMain.CI_STATUS_SKIPPED]);
                        long currentTimeinSeconds = (Int64)(DateTime.UtcNow.Subtract(unixConstant)).TotalSeconds;

                        mainForm.logToFileAndDisplay(System.DateTime.Now.ToString() + ". CI Status No. of object contentIndexed: " + (statusData[frmMain.CI_STATUS_SUCCESS] - lastCICount).ToString() + ". Took:" + ((Int64)(currentTimeinSeconds - lastQueryTimeSeconds)).ToString() + " seconds.");
                        mainForm.logToFileAndDisplay(System.DateTime.Now.ToString() + ". CI Status No. of objects backedup: " + (stateData[1] - lastBackupCount).ToString());

                        lastQueryTimeSeconds = currentTimeinSeconds;
                        lastCICount = statusData[frmMain.CI_STATUS_SUCCESS];
                        lastBackupCount = stateData[1];
                        totalCIFromStart += lastCICount;
                        totalBackupFromStart += lastBackupCount;

                        mainForm.logToFileAndDisplay(System.DateTime.Now.ToString() + ". CI Status Total CI count:" + totalCIFromStart.ToString() + "Total Backup count:" + totalBackupFromStart.ToString());
                    }
                    else
                    {
                        isPaused = true;
                        currentThread.Suspend();
                        //return;
                    }
                }
                catch (Exception)
                {
                    mainForm.logToFileAndDisplay("Exception occurred. Retrying...");
                }

                Thread.Sleep(pauseTimeMs);
            }
        }
    }
}
