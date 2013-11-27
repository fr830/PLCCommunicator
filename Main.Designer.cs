namespace PLCCommunicator
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblConnectionStatus = new System.Windows.Forms.Label();
            this.txtIPAddress = new System.Windows.Forms.TextBox();
            this.lblDetails = new System.Windows.Forms.Label();
            this.logDetails = new System.Windows.Forms.RichTextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblPlcIP = new System.Windows.Forms.Label();
            this.txtConnectionString = new System.Windows.Forms.RichTextBox();
            this.lblConnectionString = new System.Windows.Forms.Label();
            this.showDebug = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblDelay = new System.Windows.Forms.Label();
            this.txtDelay = new System.Windows.Forms.TextBox();
            this.lblMailServer = new System.Windows.Forms.Label();
            this.txtMailServer = new System.Windows.Forms.TextBox();
            this.btnEdit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(21, 18);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(79, 19);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Status:";
            // 
            // lblConnectionStatus
            // 
            this.lblConnectionStatus.BackColor = System.Drawing.SystemColors.Control;
            this.lblConnectionStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConnectionStatus.ForeColor = System.Drawing.Color.Red;
            this.lblConnectionStatus.Location = new System.Drawing.Point(87, 18);
            this.lblConnectionStatus.Name = "lblConnectionStatus";
            this.lblConnectionStatus.Size = new System.Drawing.Size(106, 19);
            this.lblConnectionStatus.TabIndex = 1;
            this.lblConnectionStatus.Text = "DISCONNECTED";
            // 
            // txtIPAddress
            // 
            this.txtIPAddress.Location = new System.Drawing.Point(88, 49);
            this.txtIPAddress.Name = "txtIPAddress";
            this.txtIPAddress.Size = new System.Drawing.Size(183, 20);
            this.txtIPAddress.TabIndex = 2;
            // 
            // lblDetails
            // 
            this.lblDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetails.Location = new System.Drawing.Point(21, 161);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.Size = new System.Drawing.Size(79, 17);
            this.lblDetails.TabIndex = 5;
            this.lblDetails.Text = "Messages:";
            // 
            // logDetails
            // 
            this.logDetails.Location = new System.Drawing.Point(22, 181);
            this.logDetails.Name = "logDetails";
            this.logDetails.ReadOnly = true;
            this.logDetails.Size = new System.Drawing.Size(548, 173);
            this.logDetails.TabIndex = 6;
            this.logDetails.Text = "";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(292, 44);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(59, 13);
            this.lblEmail.TabIndex = 7;
            this.lblEmail.Text = "Email Alert:";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(357, 44);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(213, 20);
            this.txtEmail.TabIndex = 8;
            // 
            // lblPlcIP
            // 
            this.lblPlcIP.AutoSize = true;
            this.lblPlcIP.Location = new System.Drawing.Point(19, 51);
            this.lblPlcIP.Name = "lblPlcIP";
            this.lblPlcIP.Size = new System.Drawing.Size(43, 13);
            this.lblPlcIP.TabIndex = 9;
            this.lblPlcIP.Text = "PLC IP:";
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Location = new System.Drawing.Point(357, 78);
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(213, 54);
            this.txtConnectionString.TabIndex = 10;
            this.txtConnectionString.Text = "";
            // 
            // lblConnectionString
            // 
            this.lblConnectionString.AutoSize = true;
            this.lblConnectionString.Location = new System.Drawing.Point(295, 78);
            this.lblConnectionString.Name = "lblConnectionString";
            this.lblConnectionString.Size = new System.Drawing.Size(56, 13);
            this.lblConnectionString.TabIndex = 11;
            this.lblConnectionString.Text = "Database:";
            // 
            // showDebug
            // 
            this.showDebug.AutoSize = true;
            this.showDebug.Location = new System.Drawing.Point(183, 115);
            this.showDebug.Name = "showDebug";
            this.showDebug.Size = new System.Drawing.Size(88, 17);
            this.showDebug.TabIndex = 12;
            this.showDebug.Text = "Show Debug";
            this.showDebug.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(478, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(92, 24);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "Save Settings";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblDelay
            // 
            this.lblDelay.AutoSize = true;
            this.lblDelay.Location = new System.Drawing.Point(19, 115);
            this.lblDelay.Name = "lblDelay";
            this.lblDelay.Size = new System.Drawing.Size(63, 13);
            this.lblDelay.TabIndex = 15;
            this.lblDelay.Text = "Delay (sec):";
            // 
            // txtDelay
            // 
            this.txtDelay.Location = new System.Drawing.Point(88, 112);
            this.txtDelay.Name = "txtDelay";
            this.txtDelay.Size = new System.Drawing.Size(39, 20);
            this.txtDelay.TabIndex = 16;
            // 
            // lblMailServer
            // 
            this.lblMailServer.AutoSize = true;
            this.lblMailServer.Location = new System.Drawing.Point(19, 85);
            this.lblMailServer.Name = "lblMailServer";
            this.lblMailServer.Size = new System.Drawing.Size(63, 13);
            this.lblMailServer.TabIndex = 17;
            this.lblMailServer.Text = "Mail Server:";
            // 
            // txtMailServer
            // 
            this.txtMailServer.Location = new System.Drawing.Point(88, 85);
            this.txtMailServer.Name = "txtMailServer";
            this.txtMailServer.Size = new System.Drawing.Size(183, 20);
            this.txtMailServer.TabIndex = 18;
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(478, 12);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(92, 24);
            this.btnEdit.TabIndex = 19;
            this.btnEdit.Text = "Edit Settings";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnAdmin_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.SteelBlue;
            this.label1.Location = new System.Drawing.Point(21, 372);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(455, 26);
            this.label1.TabIndex = 20;
            this.label1.Text = "NOTE: If the current status is DISCONNECTED. To reconnect the application, \r\nplea" +
                "se close and re-open the program.";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(593, 407);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.txtMailServer);
            this.Controls.Add(this.lblMailServer);
            this.Controls.Add(this.txtDelay);
            this.Controls.Add(this.lblDelay);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.showDebug);
            this.Controls.Add(this.lblConnectionString);
            this.Controls.Add(this.txtConnectionString);
            this.Controls.Add(this.lblPlcIP);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.logDetails);
            this.Controls.Add(this.lblDetails);
            this.Controls.Add(this.txtIPAddress);
            this.Controls.Add(this.lblConnectionStatus);
            this.Controls.Add(this.lblStatus);
            this.Name = "Main";
            this.Text = "PLC Communicator";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblConnectionStatus;
        private System.Windows.Forms.TextBox txtIPAddress;
        private System.Windows.Forms.Label lblDetails;
        private System.Windows.Forms.RichTextBox logDetails;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblPlcIP;
        private System.Windows.Forms.RichTextBox txtConnectionString;
        private System.Windows.Forms.Label lblConnectionString;
        private System.Windows.Forms.CheckBox showDebug;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblDelay;
        private System.Windows.Forms.TextBox txtDelay;
        private System.Windows.Forms.Label lblMailServer;
        private System.Windows.Forms.TextBox txtMailServer;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Label label1;
    }
}

