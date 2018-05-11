using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Net;
using System.IO;
using System.Web;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Reflection;

namespace MonitorFolderActivity
{
    public partial class loginPage : Form
    {
        String superUsername = "superAdmin";
        String superUserPassword = "indexing";
        bool superUser = false;
        public LogInfo userCreds;
        public string tokenTemp;
        public frmMain temp;
        public loginPage()
        {
            InitializeComponent();
            if (!File.Exists("SOLRWebservers.txt"))
            {
                var temp = File.Create("SOLRWebservers.txt");
                Console.WriteLine("webservers not detected");
                temp.Close();
            }
            string[] ips = File.ReadAllLines("SOLRWebservers.txt");
            foreach (var line in ips)
            {
                comboBox1.Items.Add(line);
            }

        }

        //returns session token, used to determine if the login attempt was successful
        public string GetSessionToken(string userName, string password, string service)
        {
            string token = string.Empty;
            string loginService = service + "Login";
            byte[] pwd = System.Text.Encoding.UTF8.GetBytes(password);
            String encodedPassword = Convert.ToBase64String(pwd, 0, pwd.Length, Base64FormattingOptions.None);
            string loginReq = string.Format("<DM2ContentIndexing_CheckCredentialReq mode=\"Webconsole\" username=\"{0}\" password=\"{1}\" />", userName, encodedPassword);
            HttpWebResponse resp = SendRequest(loginService, "POST", null, loginReq);
            //Check response code and check if the response has an attribute "token" set
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(resp.GetResponseStream());
                xmlDoc.Save("httpResponse.xml");
                if (xmlDoc.SelectSingleNode("/DM2ContentIndexing_CheckCredentialResp/@token") != null)
                    token = xmlDoc.SelectSingleNode("/DM2ContentIndexing_CheckCredentialResp/@token").Value;
            }
            else
            {
                Debug.WriteLine(string.Format("Login Failed. Status Code: {0}, Status Description: {1}", resp.StatusCode, resp.StatusDescription));
            }
            return token;
        }

        //sends login request to server
        public HttpWebResponse SendRequest(string serviceURL, string httpMethod, string token, string requestBody)
        {
            WebRequest req = WebRequest.Create(serviceURL);
            req.Method = httpMethod;
            req.ContentType = @"application/xml; charset=utf-8";
            //build headers with the received token
            if (!string.IsNullOrEmpty(token))
                req.Headers.Add("Authtoken", token);
            if(!String.IsNullOrEmpty(requestBody))
                WriteRequest(req, requestBody);
            try
            {
                return req.GetResponse() as HttpWebResponse;
            }
            catch (System.Net.WebException)
            {
                return null;
            }
        }

        private void WriteRequest(WebRequest req, string input)
        {
            req.ContentLength = Encoding.UTF8.GetByteCount(input);
            using (Stream stream = req.GetRequestStream())
            {
                stream.Write(Encoding.UTF8.GetBytes(input), 0, Encoding.UTF8.GetByteCount(input));
            }
        }

        //submits entered ip, username, and password to attempt to login, on a failed login creates error message, on a successful login starts Exchange browse software
        private void button1_Click(object sender, EventArgs  e)
        {
            string service = "http://" + comboBox1.Text + ":81/SearchSvc/CVWebService.svc/";
            errorMessage.Text = "";
            bool checkIP;
            IPAddress address;
            if (IPAddress.TryParse(comboBox1.Text, out address)) 
            {
                checkIP = true;
            } 
            else 
            {
                checkIP = false;
            }
            if (comboBox1.Text.Length < 7)
            {
                checkIP = false;
            }
            if (comboBox1.Text == "")
            {
                checkIP = false;
            }
            var auth = false;
            var suser1 = new LogInfo(superUsername, superUserPassword);
            userCreds = new LogInfo(this.Username.Text, this.Password.Text);
            temp = new frmMain();
            temp.logToFile(DateTime.Now + ": Login attempt Username: " + userCreds.username + " IP: " + comboBox1.Text, frmMain.DEBUG_NORMAL);
            LogInfo[] susers = {suser1};
            Console.WriteLine("Login attempt username {0}", userCreds.username);
            foreach (LogInfo supuser in susers)
            {
                if (supuser.username == userCreds.username && supuser.password == userCreds.password && comboBox1.Text == "")
                {
                    auth = true;
                    Console.WriteLine("Superuser confirmed");
                    temp.logToFile(DateTime.Now + ": Superuser confirmed", frmMain.DEBUG_NORMAL);
                    superUser = true;
                    break;

                }
            }
            if (!auth)
            {
                if ((!String.IsNullOrEmpty(userCreds.username)) && !(comboBox1.Text == "") && (checkIP || comboBox1.Text == "superUser"))
                {
                    string token = GetSessionToken(userCreds.username, userCreds.password, service);
                    tokenTemp = token;
                    if (!string.IsNullOrEmpty(token))
                    {
                        auth = true;

                    }
                }
            }
            if (auth)
            {
                Console.WriteLine("Login Confirmed");
                temp.logToFile(DateTime.Now + ": Login Successful", frmMain.DEBUG_NORMAL);
                String[] usedIPs = File.ReadAllLines("SOLRWebservers.txt");

                bool isUsed = false;
                foreach (string ip in usedIPs)
                {
                    if (comboBox1.Text == ip || comboBox1.Text == "superUser")
                    {
                        isUsed = true;
                    }
                }
                if (!isUsed && !String.IsNullOrEmpty(comboBox1.Text))
                {
                    System.IO.StreamWriter file1 = new System.IO.StreamWriter("SOLRWebservers.txt", true);
                    file1.WriteLine(comboBox1.Text);
                    file1.Close();
                }
                this.Hide();
                temp.superUser = superUser;
                temp.userCreds = userCreds;
                temp.webServerName = service;
                temp.token = tokenTemp;
                temp.Show();


            }
            if (!auth)
            {
                /**if (comboBox1.Text == "")
                {
                    errorMessage.Text = "No IP entered";
                }*/
                if (Username.Text == "")
                {
                    errorMessage.Text = "Please enter a username";
                }
                else if (!checkIP)
                {
                    errorMessage.Text = "Incorrect IP format";
                }
                else
                {
                    errorMessage.Text = "Incorrect username or password";
                }
                temp.logToFile(DateTime.Now + ": Login attempt failed", frmMain.DEBUG_NORMAL);
            }

        }

        //closes software
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //closes loginPage on frmMain close
        private void temp_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }

        private void Username_TextChanged(object sender, EventArgs e)
        {
            if (Username.Text.Equals("superAdmin"))
            {
                comboBox1.Enabled = false;
            }
        }
    }
    //stores login information
    public class LogInfo
    {
        public string username;
        public string password;

        public LogInfo(string user, string pass)
        {
            username = user;
            password = pass;
        }
    }
}
