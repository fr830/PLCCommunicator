using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Logix;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Data;

namespace PLCCommunicator 
{
    // TEST DESCRIPTIONS
    public struct type_DESC
    {
        public Int32 DescriptionInt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 82)]
        public char[] DescriptionString;
    };
    // HEADER RECORD STRUCTURE
    public struct type_PCB
    {
      public Int16 ID;
      public Int16 Passed;
      public Int16 Failed;
      public Int16 Year;
      public Int16 Month;
      public Int16 Day;
      public Int16 Hour;
      public Int16 Minute;
      public Int16 Second;
      public Int32 BarCodeInt;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 82)]
      public char[] BarCodeString;
    };
    // DETAIL RECORD STRUCTURE
    public struct type_PCB_DATA
    {
      public Int16 ID;
      public Int16 Units;
      public float VarData;
      public bool Passed;
      public bool Failed;
      public String TestDescription;
      public String UnitMeasure;
    };


    class Communicator
    {
        const int QSIZE = 55;
        const int PCBSIZE = 7;
        
        DTEncoding udtEnc = new DTEncoding();
        Controller ControlLogix = new Controller();
        Tag ControlLogixTag = new Tag();

        String ipAddress="";
        String cnstr = "";
        String mailServer = "";
        String emailAddress = "";
        bool showDebug = false;
        int SLEEPTIME = 20000; // DEFAULT 20 seconds
        //const string cnstr = @"Initial Catalog = FunctionalTest;Data Source = fshiftsql; User ID=isg; Password=isg";

        StringBuilder currentStatus = new StringBuilder();
        bool isConnected = true;

        public Communicator(String ip, String con, bool debug, int delay, String email, String smtpServer)
        {
            ipAddress = ip;
            cnstr = con;
            showDebug = debug;
            mailServer = smtpServer;
            emailAddress = email;
            if (delay > 10)
                SLEEPTIME = delay * 1000;
        }
        public Boolean isActive()
        {
            return isConnected;
        }
        public delegate void ObjectDelegate(object obj);

        public void sendAlertEmail()
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.To.Add(emailAddress);
            message.Subject = "IT Alert - Issue With PLC";
            message.From = new System.Net.Mail.MailAddress(emailAddress);
            message.Body = getLogMessages();
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(mailServer);
            smtp.Send(message);
            clearLogMessages();
            addToLog(" *** IT has been alerted with the issue ***", true);
            addToLog(" *** Please close the application and re-open to connect to the PLC ***", true);
        }
        public void threadStart()
        {
            addToLog("PLC Communication Service started at: " + DateTime.Now, true);
            while (testConnection())
            {
                if (isDataAvailable())
                {
                    var time = System.Diagnostics.Stopwatch.StartNew();
                    retreiveDataFromPLC();
                    var elapsed = time.Elapsed;
                    addToLog("Time to retreive and upload data: " + elapsed, showDebug);
                }

                Thread.Sleep(SLEEPTIME);
            }
            addToLog("PLC Communication Service lost communication at: " + DateTime.Now, true);
            isConnected = false;
            sendAlertEmail();
        }
        private void addToLog(String message, bool debug)
        {
            if (debug)
            {
                //Console.WriteLine(message);
                currentStatus.Append(message).Append("\n");
            }
        }
        public void clearLogMessages()
        {
            currentStatus.Length = 0;
        }
        public String getLogMessages()
        {
            return currentStatus.ToString();
        }
        private String[] getTestDescription()
        {
            String[] testDescription = new String[QSIZE];
            for (int i = 1; i < QSIZE; i++)
            {
                Tag udfTag = new Tag("Qdescriptions[" + i + "]");
                if (ControlLogix.ReadTag(udfTag) != ResultCode.E_SUCCESS)
                {
                    addToLog("ERROR: Unable to retreive the 'Qdescriptions[x]'", true);
                }
                if (ResultCode.QUAL_GOOD == udfTag.QualityCode)
                {
                    type_DESC desc = (type_DESC)udtEnc.ToType(udfTag, typeof(type_DESC));
                    Char[] descArray = desc.DescriptionString;
                    String description = new String(descArray);
                    testDescription[i] = description;
                }

            }
            return testDescription;        
        }
        private String[] getTestUnitMeasure()
        {
            String[] unitMeasure = new String[QSIZE];
            for (int i = 1; i < QSIZE; i++)
            {
                Tag udfTag = new Tag("Part[1].q[" + i + "].Units");
                if (ControlLogix.ReadTag(udfTag) != ResultCode.E_SUCCESS)
                {
                    addToLog("ERROR: Unable to retreive the 'Part[x].q[y].Units'", true);
                }
                if (ResultCode.QUAL_GOOD == udfTag.QualityCode)
                {
                    int measureCode = (Int16)udfTag.Value;
                    if (measureCode == 1) { unitMeasure[i] = "VOLTS"; }
                    else if (measureCode == 2) { unitMeasure[i] = "OHMS"; }
                    else if (measureCode == 3) { unitMeasure[i] = "AMPS"; }
                    else if (measureCode == 4) { unitMeasure[i] = "OHMS"; }
                    else { unitMeasure[i] = "N/A"; }
                }

            }
            return unitMeasure;
        }

        private Boolean isDataAvailable()
        {
            Boolean isDataAvailable = false;

            ControlLogixTag.Name = "New_Data_Available";

            if (ControlLogix.ReadTag(ControlLogixTag) != ResultCode.E_SUCCESS)
            {
                addToLog("ERROR: Unable to retreive the 'NEW DATA AVAILABLE' state", true);
            }
            if (ResultCode.QUAL_GOOD == ControlLogixTag.QualityCode)
            {
                isDataAvailable = (Boolean)ControlLogixTag.Value;
            }
    
            return isDataAvailable;
        }
        private void clearDataStatus()
        {
            ControlLogixTag.Name = "New_Data_Available";
            ControlLogixTag.Value = 0;
            if (ControlLogix.WriteTag(ControlLogixTag) != Logix.ResultCode.E_SUCCESS)
                addToLog("ERROR: Unable to WRITE the 'NEW DATA AVAILABLE' state", true);
            else
                addToLog("SUCCESS: WROTE to the 'NEW DATA AVAILABLE' state", showDebug);         
        }
        private void getHeaderData(String[] unitMeasure, String[] testDescription)
        {
            // GET PCB DATA
            for (int i = 1; i < PCBSIZE; i++)
            {
                /*
                type_PCB pcbData = createDummyType_PCB(i);
                printPCBHeader(pcbData);
                int headerID = insertHeaderData(pcbData);
                getDetailData(i, headerID);
                */
               
                Tag udfTag = new Tag("LastPartHeader" + i); // 1-7
                if (ControlLogix.ReadTag(udfTag) != Logix.ResultCode.E_SUCCESS)
                {
                    addToLog("ERROR: Unable to download PLC Header Data: " + ControlLogix.ErrorString + "--" + udfTag.ErrorCode, true);
                    return;
                }

                if (ResultCode.QUAL_GOOD == udfTag.QualityCode)
                {
                    type_PCB pcbData = (type_PCB)udtEnc.ToType(udfTag, typeof(type_PCB));
                    printPCBHeader(pcbData);
                    int passed = pcbData.Passed;
                    int failed = pcbData.Failed;

                    if (!(passed == 0 && failed == 0))
                    {
                        int headerID = insertHeaderData(pcbData);
                        //int headerID = i;
                        getDetailData(i, headerID, unitMeasure, testDescription);
                    }
                }
            }
        }
        private void getDetailData(int pcbID, int headerID, String[] unitMeasure, String[] testDescription)
        {
            String lastPartID = "LastPart[" + pcbID + "]";

            for (int i = 1; i < QSIZE; i++)
            {
                /*
                type_PCB_DATA pcbQData = createDummyType_PCB_DATA(i);
                printPCBDetail(pcbQData);
                insertPCBData(headerID, pcbQData);
                */ 
                Tag udfQTag = new Tag(lastPartID + ".Q[" + i + "]");

                if (ControlLogix.ReadTag(udfQTag) != Logix.ResultCode.E_SUCCESS)
                {
                    addToLog("ERROR: Unable to download PLC Detail Data: " + ControlLogix.ErrorString + "--" + udfQTag.ErrorCode, true);
                    return;
                }

                if (ResultCode.QUAL_GOOD == udfQTag.QualityCode)
                {
                    type_PCB_DATA pcbQData = (type_PCB_DATA)udtEnc.ToType(udfQTag, typeof(type_PCB_DATA));
                    bool passed = pcbQData.Passed;
                    bool failed = pcbQData.Failed;

                    if (passed || failed)
                    {
                        pcbQData.TestDescription = testDescription[i];
                        pcbQData.UnitMeasure = unitMeasure[i];

                        printPCBDetail(pcbQData);
                        insertPCBData(headerID, pcbQData);
                    }
                }
            }
        }
        private void printPCBHeader(type_PCB pcbData)
        {
            if (showDebug)
            {
                addToLog("ID: " + pcbData.ID, showDebug);
                addToLog("passed: " + pcbData.Passed, showDebug);
                addToLog("failed: " + pcbData.Failed, showDebug);
                addToLog("year: " + pcbData.Year, showDebug);
                addToLog("month: " + pcbData.Month, showDebug);
                addToLog("day: " + pcbData.Day, showDebug);
                addToLog("hour: " + pcbData.Hour, showDebug);
                addToLog("min: " + pcbData.Minute, showDebug);
                addToLog("sec: " + pcbData.Second, showDebug);
                Char[] barCodeArray = pcbData.BarCodeString;
                String barcode = new String(barCodeArray);
                addToLog("barcode: " + barcode.Trim(), showDebug);
                addToLog("-----------------", showDebug);
            }
        }
        private void printPCBDetail(type_PCB_DATA pcbQData)
        {
            if (showDebug)
            {
                addToLog("QID: " + pcbQData.ID, showDebug);
                addToLog("Units: " + pcbQData.Units, showDebug);
                addToLog("VarData: " + pcbQData.VarData, showDebug);
                addToLog("Passed: " + pcbQData.Passed, showDebug);
                addToLog("Failed: " + pcbQData.Failed, showDebug);
                addToLog("Test Description: " + pcbQData.TestDescription, showDebug);
                addToLog("Unit Measure: " + pcbQData.UnitMeasure, showDebug);
                addToLog("-----------------", showDebug);
            }
        }
        private void retreiveDataFromPLC()
        {
            String[] measureUnit = getTestUnitMeasure();
            String[] testDescription = getTestDescription();
            getHeaderData(measureUnit, testDescription); 
            // RESET THE FLAG
            clearDataStatus();
        }

        private type_PCB_DATA createDummyType_PCB_DATA(int id)
        {
            type_PCB_DATA qData = new type_PCB_DATA();
            qData.ID = (short)id;
            qData.Units = 2;
            qData.VarData = 0;
            qData.Failed = false;
            qData.Passed = true;
            return qData;
        }
        private type_PCB createDummyType_PCB(int id)
        {
            string strBarcode="13000000" + id;
            char[] barcode = strBarcode.ToCharArray();
          
            type_PCB pcb = new type_PCB();
            pcb.ID = (short)id;
            pcb.Passed = 1;
            pcb.Failed = 0;
            pcb.Year=2013;
            pcb.Month=11;
            pcb.Day=6;
            pcb.Hour=10;
            pcb.Minute = 56;
            pcb.Second = 0;
            pcb.BarCodeInt= 8;
            pcb.BarCodeString = barcode;

            return pcb;
        }

        private void insertPCBData(int headerID, type_PCB_DATA qData)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cnstr))
                {
                    String sql = "INSERT INTO plc_data (header_id, position, units, vardata, passed, failed, test_description, unit_measure) VALUES " +
                                "(@header_id, @position, @units, @vardata, @passed, @failed, @test_description, @unit_measure)";
                    
                    using (SqlCommand command = new SqlCommand(sql, con))
                    {
                      command.Parameters.Add("header_id", SqlDbType.Int).Value = headerID;
                      command.Parameters.Add("position", SqlDbType.Int).Value = qData.ID;
                      command.Parameters.Add("units", SqlDbType.Bit).Value = qData.Units;
                      command.Parameters.Add("vardata", SqlDbType.Real).Value = qData.VarData;
                      command.Parameters.Add("passed", SqlDbType.Bit).Value = qData.Passed;
                      command.Parameters.Add("failed", SqlDbType.Bit).Value = qData.Failed;
                      command.Parameters.Add("test_description", SqlDbType.VarChar, 500).Value = qData.TestDescription;
                      command.Parameters.Add("unit_measure", SqlDbType.VarChar, 25).Value = qData.UnitMeasure;
                      con.Open();
                      command.ExecuteNonQuery();
                      con.Close();
                    }
                }
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                // SEND EMAIL
                addToLog("PLC DB ERROR (insertPCBData): " + e.StackTrace, true);
            }
        }

        private int insertHeaderData(type_PCB pcb)
        {
            int ID = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(cnstr))
                {
                    string sql = "INSERT INTO plc_header (position, passed, failed, year, month, day, hour, minute, second, barcode) VALUES " +
                                 "(@position, @passed, @failed, @year, @month, @day, @hour, @minute, @second, @barcode)";
                    string sql2 = "SELECT @@Identity";
                    using (SqlCommand command = new SqlCommand(sql, con))
                    {
                        command.Parameters.Add("position", SqlDbType.Int).Value = pcb.ID;
                        command.Parameters.Add("passed", SqlDbType.Bit).Value = pcb.Passed;
                        command.Parameters.Add("failed", SqlDbType.Bit).Value = pcb.Failed;
                        command.Parameters.Add("year", SqlDbType.Int).Value = pcb.Year;
                        command.Parameters.Add("month", SqlDbType.Int).Value = pcb.Month;
                        command.Parameters.Add("day", SqlDbType.Int).Value = pcb.Day;
                        command.Parameters.Add("hour", SqlDbType.Int).Value = pcb.Hour;
                        command.Parameters.Add("minute", SqlDbType.Int).Value = pcb.Minute;
                        command.Parameters.Add("second", SqlDbType.Int).Value = pcb.Second;
                        command.Parameters.Add("barcode", SqlDbType.VarChar, 50).Value = new String(pcb.BarCodeString);
                        //command.Parameters.Add("date_saved", SqlDbType.DateTime).Value = DateTime.Now;
                        con.Open();
                        command.ExecuteNonQuery();
                        command.CommandText = sql2;
                        ID = System.Convert.ToInt32(command.ExecuteScalar());
                        con.Close();
                    }
                }
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                // SEND EMAIL
                addToLog("PLC DB ERROR (insertHeaderData): " + e.StackTrace, true);
            }

          return ID;
        }

        private Boolean testConnection()
        {
            try
            {
                ControlLogix.IPAddress = ipAddress;
                ControlLogix.Path = "0";
                ControlLogix.Timeout = 3000;

                if (ControlLogix.Connect() != ResultCode.E_SUCCESS)
                {
                    addToLog(ControlLogix.ErrorString, true);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (System.Exception ex) { addToLog(ex.Message, true); return false; }
        }
    }
}
