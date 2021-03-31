using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

// Structure
public struct AD101DEVICEPARAMETER
{
    public int nRingOn;
    public int nRingOff;
    public int nHookOn;
    public int nHookOff;
    public int nStopCID;
    public int nNoLine;			// Add this parameter in new AD101(MCU Version is 6.0)


}

// Mod Caller Id
namespace ModCallerId
{
    public partial class frmCallerId : Form
    {
        public string Code;

        //public static int vTxtCustomerCode;
        [DllImport("AD101Device.dll", EntryPoint = "AD101_InitDevice")]
        public static extern int AD101_InitDevice(int hWnd);

        [DllImport("AD101Device.dll", EntryPoint = "AD101_GetDevice")]
        public static extern int AD101_GetDevice();

        [DllImport("AD101Device.dll", EntryPoint = "AD101_GetCPUVersion", CharSet = CharSet.Ansi)]
        public static extern int AD101_GetCPUVersion(int nLine, StringBuilder szCPUVersion);

        // Start reading cpu id of device 
        [DllImport("AD101Device.dll", EntryPoint = "AD101_ReadCPUID")]
        public static extern int AD101_ReadCPUID(int nLine);

        // Get readed cpu id of device 
        [DllImport("AD101Device.dll", EntryPoint = "AD101_GetCPUID", CharSet = CharSet.Ansi)]
        public static extern int AD101_GetCPUID(int nLine, StringBuilder szCPUID);

        // Get caller id number  
        [DllImport("AD101Device.dll", EntryPoint = "AD101_GetCallerID", CharSet = CharSet.Ansi)]
        public static extern int AD101_GetCallerID(int nLine, StringBuilder szCallerIDBuffer, StringBuilder szName, StringBuilder szTime);

        // Get dialed number 
        [DllImport("AD101Device.dll", EntryPoint = "AD101_GetDialDigit", CharSet = CharSet.Ansi)]
        public static extern int AD101_GetDialDigit(int nLine, StringBuilder szDialDigitBuffer);

        // Get collateral phone dialed number 
        [DllImport("AD101Device.dll", EntryPoint = "AD101_GetCollateralDialDigit", CharSet = CharSet.Ansi)]
        public static extern int AD101_GetCollateralDialDigit(int nLine, StringBuilder szDialDigitBuffer);

        // Get last line state 
        [DllImport("AD101Device.dll", EntryPoint = "AD101_GetState")]
        public static extern int AD101_GetState(int nLine);

        // Get ring count
        [DllImport("AD101Device.dll", EntryPoint = "AD101_GetRingIndex")]
        public static extern int AD101_GetRingIndex(int nLine);

        // Get talking time
        [DllImport("AD101Device.dll", EntryPoint = "AD101_GetTalkTime")]
        public static extern int AD101_GetTalkTime(int nLine);

        [DllImport("AD101Device.dll", EntryPoint = "AD101_GetParameter")]
        public static extern int AD101_GetParameter(int nLine, ref AD101DEVICEPARAMETER tagParameter);

        [DllImport("AD101Device.dll", EntryPoint = "AD101_ReadParameter")]
        public static extern int AD101_ReadParameter(int nLine);

        // Set systematic parameter  
        [DllImport("AD101Device.dll", EntryPoint = "AD101_SetParameter")]
        public static extern int AD101_SetParameter(int nLine, ref AD101DEVICEPARAMETER tagParameter);

        // Free devices 
        [DllImport("AD101Device.dll", EntryPoint = "AD101_FreeDevice")]
        public static extern void AD101_FreeDevice();

        // Get current AD101 device count
        [DllImport("AD101Device.dll", EntryPoint = "AD101_GetCurDevCount")]
        public static extern int AD101_GetCurDevCount();

        // Change handle of window that uses to receive message
        [DllImport("AD101Device.dll", EntryPoint = "AD101_ChangeWindowHandle")]
        public static extern int AD101_ChangeWindowHandle(int hWnd);

        // Show or don't show collateral phone dialed number
        [DllImport("AD101Device.dll", EntryPoint = "AD101_ShowCollateralPhoneDialed")]
        public static extern void AD101_ShowCollateralPhoneDialed(bool bShow);

        // Control led 
        [DllImport("AD101Device.dll", EntryPoint = "AD101_SetLED")]
        public static extern int AD101_SetLED(int nLine, int enumLed);

        // Control line connected with ad101 device to busy or idel
        [DllImport("AD101Device.dll", EntryPoint = "AD101_SetBusy")]
        public static extern int AD101_SetBusy(int nLine, int enumLineBusy);

        // Set line to start talking than start timer
        [DllImport("AD101Device.dll", EntryPoint = "AD101_SetLineStartTalk")]
        public static extern int AD101_SetLineStartTalk(int nLine);

        // Set time to start talking after dialed number 
        [DllImport("AD101Device.dll", EntryPoint = "AD101_SetDialOutStartTalkingTime")]
        public static extern int AD101_SetDialOutStartTalkingTime(int nSecond);

        // Set ring end time
        [DllImport("AD101Device.dll", EntryPoint = "AD101_SetRingOffTime")]
        public static extern int AD101_SetRingOffTime(int nSecond);

        public const int MCU_BACKID = 0x07;	// Return Device ID
        public const int MCU_BACKSTATE = 0x08;	// Return Device State
        public const int MCU_BACKCID = 0x09;		// Return Device CallerID
        public const int MCU_BACKDIGIT = 0x0A;	// Return Device Dial Digit
        public const int MCU_BACKDEVICE = 0x0B;	// Return Device Back Device ID
        public const int MCU_BACKPARAM = 0x0C;	// Return Device Paramter
        public const int MCU_BACKCPUID = 0x0D;	// Return Device CPU ID
        public const int MCU_BACKCOLLATERAL = 0x0E;		// Return Collateral phone dialed
        public const int MCU_BACKDISABLE = 0xFF;		// Return Device Init
        public const int MCU_BACKENABLE = 0xEE;
        public const int MCU_BACKMISSED = 0xAA;		// Missed call 
        public const int MCU_BACKTALK = 0xBB;		// Start Talk

        // LED Status 
        enum LEDTYPE
        {
            LED_CLOSE = 1,
            LED_RED,
            LED_GREEN,
            LED_YELLOW,
            LED_REDSLOW,
            LED_GREENSLOW,
            LED_YELLOWSLOW,
            LED_REDQUICK,
            LED_GREENQUICK,
            LED_YELLOWQUICK,
        };
        //////////////////////////////////////////////////////////////////////////////////////////////

        // Line Status 
        enum ENUMLINEBUSY
        {
            LINEBUSY = 0,
            LINEFREE,
        };

        public const int HKONSTATEPRA = 0x01; // hook on pr+  HOOKON_PRA
        public const int HKONSTATEPRB = 0x02;  // hook on pr-  HOOKON_PRR
        public const int HKONSTATENOPR = 0x03;  // have pr  HAVE_PR
        public const int HKOFFSTATEPRA = 0x04;   // hook off pr+  HOOKOFF_PRA
        public const int HKOFFSTATEPRB = 0x05;  // hook off pr-  HOOKOFF_PRR
        public const int NO_LINE = 0x06; // no line  NULL_LINE
        public const int RINGONSTATE = 0x07;  // ring on  RING_ON
        public const int RINGOFFSTATE = 0x08;  // ring off RING_OFF
        public const int NOHKPRA = 0x09; // NOHOOKPRA= 0x09, // no hook pr+
        public const int NOHKPRB = 0x0a; // NOHOOKPRR= 0x0a, // no hook pr-
        public const int NOHKNOPR = 0x0b; // NOHOOKNPR= 0x0b, // no hook no pr

        public const int WM_USBLINEMSG = 1024 + 180;
        ModConnection objModConnection = new ModConnection();
        appCode.dbCustomer objcustomer = new appCode.dbCustomer();

        // Caller Id
        public frmCallerId()
        {
            InitializeComponent();

            // Location
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width - this.Width, Screen.PrimaryScreen.Bounds.Height - (this.Height + 50) );

            // State
            this.WindowState = FormWindowState.Minimized;
        }
        
        // Form Caller Id Load
        private void frmCallerId_Load(object sender, EventArgs e)
        {
            //MessageBox.Show(AD101_InitDevice(Handle.ToInt32()).ToString());

            // Check Status
            if (AD101_InitDevice(Handle.ToInt32()) == 1)
            {
                MessageBox.Show("تم الإتصال بنجاح");
            }
            else { MessageBox.Show("حدث خطأ فى الإتصال"); return; }

            // Check 
            if (AD101_InitDevice(Handle.ToInt32()) == 0)
            {
                return;
            }

            // Get Device
            AD101_GetDevice();
            // Set Dial Out Starting Time
            AD101_SetDialOutStartTalkingTime(3);
            // Set Ring Off Time
            AD101_SetRingOffTime(7);
            // Form Caller Id Load
            funFormCallerIdLoad();
            // Active Control
            ActiveControl = btnReceive;
           
        }

        // On Device Msg
        private void OnDeviceMsg(IntPtr wParam, IntPtr Lparam)
        {
            int nMsg = new int();
            int nLine = new int();

            nMsg = wParam.ToInt32() % 65536;
            nLine = wParam.ToInt32() / 65536;

            switch (nMsg)
            {
                case MCU_BACKDISABLE:

                    break;

                case MCU_BACKENABLE:
                    break;

                case MCU_BACKID:
                    {
                        StringBuilder szCPUVersion = new StringBuilder(32);
                        AD101_GetCPUVersion(nLine, szCPUVersion);
                    }
                    break;

                case MCU_BACKCID:
                    {
                        StringBuilder szCallerID = new StringBuilder(128);
                        StringBuilder szName = new StringBuilder(128);
                        StringBuilder szTime = new StringBuilder(128);

                        int nLen = AD101_GetCallerID(nLine, szCallerID, szName, szTime);

                        txtPhone.Text = szCallerID.ToString();
                        funGetCustomerData();
                        this.BringToFront();
                        this.TopMost = true;
                        this.WindowState = FormWindowState.Normal;

                        ActiveControl = btnReceive;
                    }
                    break;

                case MCU_BACKSTATE:
                    {
                        switch (Lparam.ToInt32())
                        {
                            case HKONSTATEPRA:

                                break;

                            case HKONSTATEPRB:
                                break;

                            case HKONSTATENOPR:
                                break;

                            case HKOFFSTATEPRA:
                                {

                                }
                                break;

                            case HKOFFSTATEPRB:
                                {

                                }
                                break;

                            case NO_LINE:
                                {

                                }
                                break;

                            case RINGONSTATE:
                                {
                                    StringBuilder szCallerID = new StringBuilder(128);
                                    StringBuilder szName = new StringBuilder(128);
                                    StringBuilder szTime = new StringBuilder(128);

                                    int nLen = AD101_GetCallerID(nLine, szCallerID, szName, szTime);

                                    txtPhone.Text = szCallerID.ToString();
                                    funGetCustomerData();
                                    this.BringToFront();
                                    this.TopMost = true;
                                    this.WindowState = FormWindowState.Normal;

                                    ActiveControl = btnReceive;

                                    //string szRing = "Ring:" + string.Format("{0:D2}", AD101_GetRingIndex(nLine));
                                    ////label1.Text = szRing;
                                    //if (AD101_GetRingIndex(nLine) >= 0 && AD101_GetRingIndex(nLine) <= 3)
                                    //{
                                    //    tmr_customerphone.Start();
                                    //}
                                    //else if (AD101_GetRingIndex(nLine) > 3)
                                    //{
                                    //    tmr_customerphone.Stop();
                                    //    player.Stop();
                                    //}
                                }
                                break;

                            case RINGOFFSTATE:
                                break;

                            case NOHKPRA:

                                break;

                            case NOHKPRB:

                                break;

                            case NOHKNOPR:
                                {
                                }
                                break;


                            default:
                                break;
                        }
                    }
                    break;

                case MCU_BACKDIGIT:
                    {

                    }
                    break;


                case MCU_BACKCOLLATERAL:
                    {
                        StringBuilder szDialDigit = new StringBuilder(128);

                        AD101_GetCollateralDialDigit(nLine, szDialDigit);

                    }
                    break;

                case MCU_BACKDEVICE:
                    {
                        StringBuilder szCPUVersion = new StringBuilder(32);


                        AD101_GetCPUVersion(nLine, szCPUVersion);


                    }
                    break;

                case MCU_BACKPARAM:
                    {
                        AD101DEVICEPARAMETER tagParameter = new AD101DEVICEPARAMETER();

                        AD101_GetParameter(nLine, ref tagParameter);
                    }
                    break;

                case MCU_BACKCPUID:
                    {
                        StringBuilder szCPUID = new StringBuilder(4);

                        AD101_GetCPUID(nLine, szCPUID);


                    }
                    break;

                case MCU_BACKMISSED:
                    {
                    }
                    break;

                case MCU_BACKTALK:
                    {
                        string strTalk;
                        strTalk = string.Format("{0:D2}", Lparam) + "S";


                    }
                    break;

                default:
                    break;
            }
        }

        // Def
        protected override void DefWndProc(ref System.Windows.Forms.Message m)
        {
            switch (m.Msg)
            {
                case WM_USBLINEMSG: //´¦ہيدûد¢،،
                    OnDeviceMsg(m.WParam, m.LParam);
                    break;
                default:
                    base.DefWndProc(ref m);//µ÷سأ»ùہà؛¯ت‎´¦ہي·ا×ش¶¨زهدûد¢،£،،،،،،
                    break;
            }
        }

        // Get Customer Data
        private void funGetCustomerData()
        {

            funClearToOtherCall();
            // Declaration Dt Customer
            DataTable dtCustomerData = new DataTable();

            // Try To Get Data
            dtCustomerData = objcustomer.funCustomerInfoByPhone(txtPhone.Text);

            // Check If Dt Has Rows
            if (dtCustomerData.Rows.Count > 0)
            {
                // Get Data
                Code = "";
                Code = dtCustomerData.Rows[0]["TxtCustomerCode"].ToString();
                txtName.Text = dtCustomerData.Rows[0]["TxtCustomerName"].ToString();
                txtAddress.Text = dtCustomerData.Rows[0]["TxtCustomerAddress"].ToString();

                btnReceive.Enabled = true;
                btnCancel.Enabled = true;
            }
            else if (dtCustomerData.Rows.Count == 0)
            {

                btnReceive.Enabled = false;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;

                txtName.Focus();
            }

        }

        // Write To Text
        private void funFormCallerIdLoad()
        {
            // Write to Text
            funWriteToText();
        }

        // Read from Text
        private void funReadfromText()
        {
            string fileName = @"D:\TanmyaSW\Caller.txt"; 
            using (var reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Do stuff with your line here, it will be called for each 
                    // line of text in your file.
                    //MessageBox.Show(line);
                }
            }
        }

        // Write to Text
        private void funWriteToText()
        {
            if (string.IsNullOrEmpty(Code)) { Code = ""; }
            if (string.IsNullOrEmpty(txtName.Text)) { txtName.Text = " "; }
            if (string.IsNullOrEmpty(txtPhone.Text)) { txtPhone.Text = " "; }
            if (string.IsNullOrEmpty(txtAddress.Text)) { txtAddress.Text = " "; }
            string callerData = Code + Environment.NewLine + txtName.Text + Environment.NewLine + txtPhone.Text + Environment.NewLine + txtAddress.Text;
            string filePath = @"D:\TanmyaSW\Caller.txt";
            File.WriteAllText(filePath, callerData);
        }

        // Get Data By Phone
        private void funGetDataByPhone()
        { 
            
        }

        // Save and [Optional Recieve]
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Save
            funSave();

            // Chech If Receive or Not
            if (MessageBox.Show("تم الحفظ, هل تريد إستقبال المكالمة؟", "تم الحفظ", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
            {
                funWriteToText();
                funClear();

                btnReceive.Enabled = false;
                btnCancel.Enabled = false;
                btnSave.Enabled = false;

                this.WindowState = FormWindowState.Minimized;
            }
            else 
            {
                btnReceive.Enabled = true;
                btnCancel.Enabled = true;
                btnSave.Enabled = true;
            }
        }
        
        // Save
        private void funSave()
        {
            try
            {
               Code = objcustomer.funCustomerInsert(txtName.Text, txtPhone.Text, txtAddress.Text).ToString();
                //MessageBox.Show("Done !");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());                
            }
        }

        // Cancel
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Cancel
            funClear();

            btnReceive.Enabled = false;
            btnCancel.Enabled = false;
            btnSave.Enabled = false;

            this.TopMost = false;
            this.SendToBack();
            this.WindowState = FormWindowState.Minimized;
        }

        // Cancel
        private void funClear()
        {
            txtPhone.Clear();
            txtName.Clear();
            txtAddress.Clear();
        }
        // Other Calling
        private void funClearToOtherCall()
        {
            
            txtName.Clear();
            txtAddress.Clear();
        }

        // Close
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Receive
        private void btnReceive_Click(object sender, EventArgs e)
        {
            try
            {
                funWriteToText();
                funClear();

                btnReceive.Enabled = false;
                btnCancel.Enabled = false;
                btnSave.Enabled = false;

                this.TopMost = false;
                this.SendToBack();
                this.WindowState = FormWindowState.Minimized;

            }
            catch (Exception ex)
            { 
                
            }
        }
    }
}
