using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Configuration;

namespace PLCCommunicator
{
    public partial class Main : Form
    {
        Thread commThread;
        Thread uiThread;
        Communicator comm;

        delegate void SetTextCallback(string text);
        delegate void SetStatusCallback(bool status);

        public Main()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Main_FormClosing);
            adminAccess(false);
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            destroyThread();
        }

        private void adminAccess(bool b)
        {
            txtIPAddress.Enabled = b;
            lblEmail.Visible = b;
            txtEmail.Visible = b;
            lblMailServer.Visible = b;
            txtMailServer.Visible = b;
            lblDelay.Visible = b;
            txtDelay.Visible = b;
            showDebug.Visible = b;
            lblConnectionString.Visible = b;
            txtConnectionString.Visible = b;
            btnSave.Visible = b;
            btnEdit.Visible = !b;
        }
        private void Main_Load(object sender, EventArgs e)
        {
            txtIPAddress.Text = ConfigurationManager.AppSettings["plc_address"];
            txtEmail.Text = ConfigurationManager.AppSettings["email"];
            txtMailServer.Text = ConfigurationManager.AppSettings["mail_server"];
            txtDelay.Text = ConfigurationManager.AppSettings["delay"];
            showDebug.Checked = ConfigurationManager.AppSettings["show_debug"].ToUpper().Equals("FALSE") ? false : true;
            txtConnectionString.Text = ConfigurationManager.ConnectionStrings["PLCDatabaseConnection"].ConnectionString;
            lblConnectionStatus.ForeColor = System.Drawing.Color.Red;

            startService();
        }
        private void startUIThread()
        {
          uiThread = new Thread(delegate()
          {
              while (true)
              {
                  setLogText(comm.getLogMessages());
                  isActive(comm.isActive());
                  Thread.Sleep(3000);
              }
          });
          uiThread.Start();  
        }
        private void isActive(bool status)
        {
            if (lblConnectionStatus.InvokeRequired)
            {
                SetStatusCallback d = new SetStatusCallback(isActive);
                this.Invoke(d, new object[] { status });
            }
            else
            {
                if (status)
                {
                    lblConnectionStatus.ForeColor = System.Drawing.Color.Green;
                    lblConnectionStatus.Text = "CONNECTED";
                }
                else
                {
                    lblConnectionStatus.ForeColor = System.Drawing.Color.Red;
                    lblConnectionStatus.Text = "DISCONNECTED";
                    destroyThread();
                }
            }
        }
        private void setLogText(String text)
        {
            if (logDetails.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(setLogText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                logDetails.Text = text;
            }
        }

        private void startService()
        {
            startCommunicationThread();
            startUIThread();
        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            startService();
        }
        private void startCommunicationThread()
        {
            comm = new Communicator(txtIPAddress.Text, txtConnectionString.Text, showDebug.Checked, 
                                    Convert.ToInt32(txtDelay.Text), txtEmail.Text, txtMailServer.Text);
            commThread = new Thread(new ThreadStart(comm.threadStart));
            commThread.Start();
        }
        private void destroyThread()
        {
            if (uiThread != null)
            {
                uiThread.Abort();
                uiThread.Join();
            }
            if (commThread != null)
            {
                commThread.Abort();
                commThread.Join();
            }
        }
        private void stopService()
        {
            destroyThread();
            logDetails.Text = logDetails.Text + "PLC Communication Service was stopped at: " + DateTime.Now;
            //Console.WriteLine("PLC Communication Service was stopped at: " + DateTime.Now);
            lblConnectionStatus.ForeColor = System.Drawing.Color.Red;
            lblConnectionStatus.Text = "DISCONNECTED";
        }
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            stopService();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Open App.Config of executable
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            // Add an Application Setting.
            config.AppSettings.Settings.Remove("plc_address");
            config.AppSettings.Settings.Remove("email");
            config.AppSettings.Settings.Remove("show_debug");
            config.AppSettings.Settings.Remove("mail_server");
            config.AppSettings.Settings.Remove("delay");
            
            config.AppSettings.Settings.Add("plc_address", txtIPAddress.Text);
            config.AppSettings.Settings.Add("email", txtEmail.Text);
            config.AppSettings.Settings.Add("show_debug", showDebug.Checked ? "true" : "false");
            config.AppSettings.Settings.Add("mail_server", txtMailServer.Text);
            config.AppSettings.Settings.Add("delay", txtDelay.Text);

            ConnectionStringsSection connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            connectionStringsSection.ConnectionStrings["PLCDatabaseConnection"].ConnectionString = txtConnectionString.Text;

            // Save the configuration file.
            config.Save(ConfigurationSaveMode.Modified);
            // Force a reload of a changed section.
            ConfigurationManager.RefreshSection("appSettings");
            ConfigurationManager.RefreshSection("connectionStrings");
            MessageBox.Show("Changes have been saved. Please close and re-open the application", "Settings Change");
            stopService();
            adminAccess(false);
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            string sInput = Microsoft.VisualBasic.Interaction.InputBox("Authoization Required to edit settings.", "Authoization Required", "Enter Password:");
            if (sInput.ToUpper().Equals("SAFARI1985"))
                adminAccess(true);
            else
            {
                adminAccess(false);
                MessageBox.Show("Invalid password. Please contact the IT Department", "Authorized Access Only");
            }


        }

    }
}
