/*
* Copyright(C) 2 2017 郑州金润高科电子有限公司
* 文件名: MT_Comm.xaml.cs
* 文件功能描述: 打磨机通讯DLL
* 目的：打磨机通讯
* 创建标识: 陈大伟 2020-5
* 修改标识: 
* 修改描述:

*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using System.IO.Ports;
using System.Windows.Forms;

using ClassLib_TestData;

using System.Runtime.InteropServices;
using Laike.Can.CanCmd;

namespace Clb_MT_Comm
{
    public class MT_Comm
    {
        /// <summary>
        /// 获得系统资源
        /// </summary>
        /// <param name="SysBuff"></param>
        public MT_Comm(ref clStreamVideo SysBuff, ref MsgInterFace Msg_Plant, int iTime_Waite)// MsgInterFace Msg_Plant)
        {
            m_SysBuf = SysBuff;
            g_iTime_Waite = iTime_Waite;
        }
        /// <summary>
        /// 消息接口
        /// </summary>
        MsgInterFace g_Msg_InterFace = null;
        int g_iTime_Waite = 20;
        /// <summary>
        /// 接收距离处理方法 0：准确距离处理
        /// </summary>
        int m_iGetDist_Type = 0;
        /// <summary>
        /// 判断光栅臂运行方向距离
        /// </summary>
        float  m_iJugeDist = 0.015f;
        ///// <summary>
        ///// 是否可以运行计算了
        ///// </summary>
        //bool m_blCanCalcu = false;
        /// <summary>
        /// 纵轴临时距离
        /// </summary>
        float flDistance_Y = 0;
        /// <summary>
        /// 通讯类型 0false：CAN 1true：COM
        /// </summary>
        public bool m_blComType = false;
        /// <summary>
        /// 循环
        /// </summary>
        bool m_blWhile = false;
        /// <summary>
        /// 通讯方式  COM 1： Can: 0
        /// </summary>
        public int m_blCom1_Can0 = 0;
        #region Can通讯变量
        /// <summary>
        /// 读取通道数据
        /// </summary>
        public System.Threading.Thread Trd_GetCanData = null;
        /// <summary>
        /// 前进1 后退2
        /// </summary>
        public static int m_blQin1_Hou2 = 0;
        /// <summary>
        /// 类型
        /// </summary>
        static UInt32 m_devtype = 1;
        /// <summary>
        /// 设备指针
        /// </summary>
        static UInt32 m_DeviceHandle = 0;
        /// <summary>
        /// 终端电阻 1：使用 0：不使用
        /// </summary>
        public int checkBox1_Checked = 0;
        /// <summary>
        /// 打开设备
        /// </summary>
        UInt32 m_bOpen = 0;
        /// <summary>
        /// 索引号
        /// </summary>
        UInt32 m_devind = 0;
        /// <summary>
        /// 第几路CAN
        /// </summary>
        UInt32 m_dwChannel = 0;

        CAN_DataFrame[] m_recobj = new CAN_DataFrame[50];

        UInt32[] m_arrdevtype = new UInt32[20];

        System.Timers.Timer timer_rec;

        #region 选择参数
        /// <summary>
        /// 类型
        /// </summary>
        public int comboBox_devtype_SelectedIndex = 0;
        /// <summary>
        /// 选择Can 索引号
        /// </summary>
        public int comboBox_DevIndex_SelectedIndex = 0;
        /// <summary>
        /// 第几路Can
        /// </summary>
        public int comboBox_CANIndex_SelectedIndex = 0;
        #endregion 选择参数
        #region 初始化CAN参数
        /// <summary>
        /// 验收码
        /// </summary>
        public string textBox_AccCode_Text = "00000000";
        /// <summary>
        /// 屏蔽码
        /// </summary>
        public string textBox_AccMask_Text = "FFFFFFFF";
        /// <summary>
        /// 定时器0
        /// </summary>
        public string textBox_Time0_Text = "03";//波特率125
        /// <summary>
        /// 定时器1
        /// </summary>
        public string textBox_Time1_Text = "1C";
        /// <summary>
        /// 滤波方式
        /// </summary>
        public int comboBox_Filter_SelectedIndex = 0;
        /// <summary>
        /// 模式
        /// </summary>
        public int comboBox_Mode_SelectedIndex = 0;
        #endregion 初始化参数
        #region 发送数据帧
        /// <summary>
        /// 发送格式
        /// </summary>
        public int comboBox_SendType_SelectedIndex = 2;
        /// <summary>
        /// 帧格式
        /// </summary>
        public int comboBox_FrameFormat_SelectedIndex = 0;
        /// <summary>
        /// 帧类型
        /// </summary>
        public int comboBox_FrameType_SelectedIndex = 0;
        /// <summary>
        /// 帧ID
        /// </summary>
        public string textBox_ID_Text = "00000123";
        /// <summary>
        /// 发送数据
        /// </summary>
        public string textBox_Data_Text = "00 01 02 03 04 05 06 07 ";


        #endregion 选择参数


        #endregion  Can变量
        #region Com变量
        /// <summary>
        /// 程序是否退出
        /// </summary>
        public bool m_blExit = false;
       
        /// <summary>
        /// 车体系统缓存
        /// </summary>
        public clStreamVideo m_SysBuf;
      

        /// <summary>
        /// 通讯口
        /// </summary>
        private SerialClass RS232 = new SerialClass();
        /// <summary>
        /// 系统配置文件
        /// </summary>
        public string m_SysFileName = Application.StartupPath + "\\DataBase\\SysInfo.ini";//
        /// <summary>
        /// 日志文件
        /// </summary>
        public string strLogFileName = Application.StartupPath + "\\datalog\\CommLog.ini";
        /// <summary>
        /// 读写文件
        /// </summary>
        private ClassInterFace m_csInter = new ClassInterFace();
        /// <summary>
        /// 连接状态
        /// </summary>
        public bool m_blLink = false;
        /// <summary>
        /// 通讯报文是否写日志
        /// </summary>
        public bool m_blWrLog = false;
        /// <summary>
        /// 串口参数
        /// </summary>
        private string m_strComPara = "";
        /// <summary>
        /// 文件互斥锁
        /// </summary>
        private object LockFile = new object();
        /// <summary>
        /// 串口发送互斥锁
        /// </summary>
        private object m_LockCom = new object();
        #endregion

        #region 方法
        /// <summary>
        /// 串口初始化
        /// </summary>
        /// <returns></returns>
        public bool InitCom(string Comm, string strPara = "38400,0,8,1")
        {
            bool _blRet = false;
            if (Comm == "") return _blRet;
            m_blLink = false;

            m_strComPara = Comm + "," + strPara;
            #region 1 串口参数设置
            string[] _sPara = m_strComPara.Split(',');//格式 COM,波特率，奇偶校验(None/Odd/Even)，数据位，停止位：COM1,9600,1,8,1
            if (_sPara.Length == 5)
            {
                Parity parity = Parity.None;//无校验
                StopBits stopBits = StopBits.One;

                switch (_sPara[2].Substring(0, 1).ToUpper())
                {
                    case "1":
                        parity = Parity.Odd; break;
                    case "2":
                        parity = Parity.Even; break;
                }
                switch (int.Parse(_sPara[4]))
                {
                    case 1:
                        stopBits = StopBits.One; break;
                    case 2:
                        stopBits = StopBits.Two; break;
                    default:
                        stopBits = StopBits.None; break;
                }
                try
                {
                    _blRet = RS232.setSerialPort(_sPara[0], int.Parse(_sPara[1]), parity, int.Parse(_sPara[3]), stopBits);
                }
                catch (Exception e)
                {
                    MessageBox.Show("磁爬端口初始化异常：" + e.Message);
                    _blRet = false;
                    return _blRet;
                }
                #region 调用数据解析
                RS232.DataReceived -= ParseDat;
                RS232.DataReceived += ParseDat;
                #endregion 数据解析

                m_blLink = false;
            }
            #endregion  串口参数设置

            return _blRet;
        }

        #region Can方法
        /*
        CAN 通道波特率 dwBtr[0] dwBtr[1]
5Kbps 0xBF 0xFF
10Kbps 0x31 0x1C
20Kbps 0x18 0x1C
50Kbps 0x09 0x1C
100Kbps 0x04 0x1C
125Kbps 0x03 0x1C
250Kbps 0x01 0x1C
500Kbps 0x00 0x1C
800Kbps 0x00 0x16
1000Kbps 0x00 0x14
         */
        /// <summary>
        /// can通讯
        /// </summary>
        /// <param name="iBtl">波特率 默认125 BF FF</param>
        ///  <param name="iCurrType">设备类型 4：USBCAN_1CH</param>
        public void InitCan(int iBtl = 125, int iCurrType = 5)
        {
            Int32 curindex = 0;
            comboBox_devtype_SelectedIndex = iCurrType;
            #region 波特率设置
            switch (iBtl)//波特率
            {
                case 5:
                    textBox_Time0_Text = "03";
                    textBox_Time1_Text = "1C";
                    break;
                case 10:
                    textBox_Time0_Text = "31";
                    textBox_Time1_Text = "1C";
                    break;
                case 20:
                    textBox_Time0_Text = "18";
                    textBox_Time1_Text = "1C";
                    break;
                case 50:
                    textBox_Time0_Text = "09";
                    textBox_Time1_Text = "1C";
                    break;
                case 100:
                    textBox_Time0_Text = "04";
                    textBox_Time1_Text = "1C";
                    break;
                case 125:
                    textBox_Time0_Text = "03";
                    textBox_Time1_Text = "1C";
                    break;
                case 250:
                    textBox_Time0_Text = "01";
                    textBox_Time1_Text = "1C";
                    break;
                case 500:
                    textBox_Time0_Text = "00";
                    textBox_Time1_Text = "1C";
                    break;
                case 800:
                    textBox_Time0_Text = "00";
                    textBox_Time1_Text = "16";
                    break;
                case 1000:
                    textBox_Time0_Text = "00";
                    textBox_Time1_Text = "14";
                    break;
            }
            #endregion 波特率设置

            m_arrdevtype[curindex++] = CanCmd.LCUSB_131B;
            m_arrdevtype[curindex++] = CanCmd.LCUSB_131B;
            m_arrdevtype[curindex++] = CanCmd.LCPCI_252;
            m_arrdevtype[curindex++] = CanCmd.LCMiniPcie_431;
            m_arrdevtype[curindex++] = CanCmd.LCMiniPcie_432;
            m_arrdevtype[curindex++] = CanCmd.USBCAN_1CH;//当前选中的型号
            m_arrdevtype[curindex++] = CanCmd.USBCAN_C_1CH;
            m_arrdevtype[curindex++] = CanCmd.USBCAN_E_1CH;
            m_arrdevtype[curindex++] = CanCmd.USBCAN_E_2CH;
            m_arrdevtype[curindex++] = CanCmd.MPCIeCAN_1CH;
            m_arrdevtype[curindex++] = CanCmd.MPCIeCAN_2CH;

            Can_Open();
            m_blLink = Can_Start();
        }
        /// <summary>
        /// 终端电阻 1：使用 0：不使用
        /// </summary>
        public void Can_Checked()
        {
            checkBox1_Checked = ((m_arrdevtype[comboBox_devtype_SelectedIndex] == CanCmd.USBCAN_1CH) ||
                                (m_arrdevtype[comboBox_devtype_SelectedIndex] == CanCmd.USBCAN_C_1CH)) ? 1 : 0;
        }
        /// <summary>
        /// 打开指定Can 
        /// </summary>
        public void Can_Open()
        {
            if (m_bOpen == 1)
            {
                CanCmd.CAN_DeviceClose(m_DeviceHandle);
                m_bOpen = 0;
                timer_rec.Enabled = false;
                //button_StartCAN.Enabled = false;
                //button_StopCAN.Enabled = false;
            }
            else
            {
                m_devtype = m_arrdevtype[comboBox_devtype_SelectedIndex];
                char arg = '0';
                m_devind = (UInt32)comboBox_DevIndex_SelectedIndex;
                m_dwChannel = (UInt32)comboBox_CANIndex_SelectedIndex;
                m_DeviceHandle = CanCmd.CAN_DeviceOpen(m_devtype, m_devind, ref arg);
                if (m_DeviceHandle == 0)
                {
                    //MessageBox.Show("打开设备失败,请检查设备类型和设备索引号是否正确", "错误",
                    //        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                //   button_StartCAN.Enabled = true;
                m_bOpen = 1;
                //   if (checkBox1_Enabled == 1)   // 支持控制内置终端电阻
                {
                    byte val = 1;
                    CanCmd.CAN_WriteRegister(m_DeviceHandle, m_dwChannel, 0xfe, ref val, 1);
                    Can_Checked();
                    if (checkBox1_Checked == 1)
                        val = 1;
                    else
                        val = 0;
                    CanCmd.CAN_WriteRegister(m_DeviceHandle, m_dwChannel, 0xff, ref val, 1);
                }
                m_blWhile = false   ;
                if (m_blWhile == false)
                {
                    if (timer_rec == null)
                        timer_rec = new System.Timers.Timer();
                    timer_rec.Elapsed -= timer_rec_Tick;
                    timer_rec.Elapsed += timer_rec_Tick;
                    timer_rec.Interval = g_iTime_Waite;
                }
            }

            //  buttonConnect.Text = m_bOpen == 1 ? "关闭设备" : "打开设备";
        }
        /// <summary>
        /// 启动Can
        /// </summary>
        /// <returns></returns>
        public bool Can_Start()
        {
            bool _blRet = false;
            if (m_bOpen == 0)
                return _blRet;
            CAN_InitConfig config = new CAN_InitConfig();
            config.dwAccCode = System.Convert.ToUInt32("0x" + textBox_AccCode_Text, 16);
            config.dwAccMask = System.Convert.ToUInt32("0x" + textBox_AccMask_Text, 16);
            config.nBtrType = 1;   // 位定时参数模式(1表示SJA1000,0表示LPC21XX)
            // 1M ： dwBtr0=00 dwBtr1=0x14  Other: 0014 -1M 0016-800K 001C-500K 011C-250K 031C-125K 041C-100K 091C-50K 181C-20K 311C-10K BFFF-5K
            config.dwBtr0 = System.Convert.ToByte("0x" + textBox_Time0_Text, 16);
            config.dwBtr1 = System.Convert.ToByte("0x" + textBox_Time1_Text, 16);
            config.nFilter = (Byte)comboBox_Filter_SelectedIndex;
            config.bMode = (Byte)comboBox_Mode_SelectedIndex;
            if (CanCmd.CAN_RESULT_OK != CanCmd.CAN_ChannelStart(m_DeviceHandle, m_dwChannel, ref config))
            {
                //   button_StopCAN_Enabled = false;
                MessageBox.Show("启动失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return _blRet;
            }
            _blRet = true  ;
            if (m_blWhile)
                Run_GetData_Can();
            else
                timer_rec.Enabled = true;
            return _blRet;
        }
        private void Run_GetData_Can()
        {
            if (Trd_GetCanData != null) Trd_GetCanData.Abort();
            Trd_GetCanData = new System.Threading.Thread(new System.Threading.ThreadStart(ReadCan));
            // Trd_GetCanData.Priority = ThreadPriority.AboveNormal;
            Trd_GetCanData.Name = "Thread_ReadUI";
            Trd_GetCanData.IsBackground = true;
            Trd_GetCanData.Start();
        }
        private void ReadCan()
        {
            while (m_blExit==false )
            {
                Read_CanBuff_Time();
                Application.DoEvents();
                System.Threading.Thread.Sleep(5);
            }
        }
        /// <summary>
        /// 停止CAN
        /// </summary>
        public void Can_Stop()
        {
            if (m_bOpen == 0)
                return;
            timer_rec.Enabled = false;
            CanCmd.CAN_ChannelStop(m_DeviceHandle, m_dwChannel);

        }
        /// <summary>
        /// 发送帧
        /// </summary>
        unsafe public bool Can_Send(String strdata)
        {
            bool _blRet = false;
            if (m_bOpen == 0)
                return _blRet;

            CAN_DataFrame sendobj = new CAN_DataFrame();
            sendobj.nSendType = (byte)comboBox_SendType_SelectedIndex;
            sendobj.bRemoteFlag = (byte)comboBox_FrameFormat_SelectedIndex;
            sendobj.bExternFlag = (byte)comboBox_FrameType_SelectedIndex;
            sendobj.uID = System.Convert.ToUInt32("0x" + textBox_ID_Text, 16);
            int len = (strdata.Length) / 2;
            sendobj.nDataLen = System.Convert.ToByte(len);
            //       String strdata = textBox_Data_Text;

            for (int i = 0; i < 8; i++)
                sendobj.arryData[i] = System.Convert.ToByte("0x" + strdata.Substring(i * 2, 2), 16);


            if (CanCmd.CAN_ChannelSend(m_DeviceHandle, m_dwChannel, ref sendobj, 1) == 0)
            {
                //MessageBox.Show("发送失败", "错误",
                //        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
                _blRet = true;
            return _blRet;
        }
        unsafe private void timer_rec_Tick(object sender, EventArgs e)
        {//单调
         //          DateTime dtStar = DateTime.Now;
            Read_CanBuff_Time();
        }
        unsafe private void Read_CanBuff_Time()
        {
            UInt32 res = new UInt32();
            res = CanCmd.CAN_GetReceiveCount(m_DeviceHandle, m_dwChannel);
            if (res == 0)
                return;

            /////////////////////////////////////
            UInt32 con_maxlen = 50;
            IntPtr pt = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(CAN_DataFrame)) * (Int32)con_maxlen);

            res = CanCmd.CAN_ChannelReceive(m_DeviceHandle, m_dwChannel, pt, con_maxlen, 100);// res = CanCmd.CAN_ChannelReceive(m_DeviceHandle, m_dwChannel, pt, con_maxlen, 100);
            ////////////////////////////////////////////////////////
            if (res == 0)
            { // 读取错误信息
                CAN_ErrorInformation err = new CAN_ErrorInformation();
                // 必须调用此函数
                if (CanCmd.CAN_GetErrorInfo(m_DeviceHandle, m_dwChannel, ref err) == CanCmd.CAN_RESULT_OK)
                {  // CAN通讯有错误
                    // 处理错误信息
                }
                else
                {  // 没有收到CAN数据

                }
            }
            else
            {
                // WriteErrorLog(strLogFileName, ("抓取数据个数：") + res.ToString (), false);
                String str = "";
                string _strRetData = "";//接收数据报文
                byte[] _ArrData = new byte[8];//接收数据
                int iLen = 0;
                for (UInt32 i = 0; i < res; i++)
                {
                    try
                    {
                        // iLen = Marshal.SizeOf(typeof(CAN_DataFrame));
                        CAN_DataFrame obj = (CAN_DataFrame)Marshal.PtrToStructure((IntPtr)((UInt64)pt + (UInt64)(i * Marshal.SizeOf(typeof(CAN_DataFrame)))), typeof(CAN_DataFrame));

                        str = "接收到数据: ";
                        str += "  帧ID:0x" + System.Convert.ToString((Int32)obj.uID, 16);
                        str += "  帧格式:";
                        if (obj.bRemoteFlag == 0)
                            str += "数据帧 ";
                        else
                            str += "远程帧 ";
                        if (obj.bExternFlag == 0)
                            str += "标准帧 ";
                        else
                            str += "扩展帧 ";

                        //////////////////////////////////////////
                        if (obj.bRemoteFlag == 0)
                        {
                            str += "数据: ";
                            byte len = (byte)(obj.nDataLen % 9);
                            byte j = 0;
                            if (j++ < len)

                                str += " " + System.Convert.ToString(obj.arryData[0], 16);
                            if (j++ < len)
                                str += " " + System.Convert.ToString(obj.arryData[1], 16);
                            if (j++ < len)
                                str += " " + System.Convert.ToString(obj.arryData[2], 16);
                            if (j++ < len)
                                str += " " + System.Convert.ToString(obj.arryData[3], 16);
                            if (j++ < len)
                                str += " " + System.Convert.ToString(obj.arryData[4], 16);
                            if (j++ < len)
                                str += " " + System.Convert.ToString(obj.arryData[5], 16);
                            if (j++ < len)
                                str += " " + System.Convert.ToString(obj.arryData[6], 16);
                            if (j++ < len)
                                str += " " + System.Convert.ToString(obj.arryData[7], 16);
                            for (int iNo = 0; iNo < 8; iNo++)
                                _ArrData[iNo] = obj.arryData[iNo];// _strRetData += System.Convert.ToString(obj.arryData[iNo], 16);

                            ParseDat_Can(_ArrData);
                        }
                    }
                    catch (Exception e55)
                    { }

                    //listBox_Info.Items.Add(str);
                    //listBox_Info.SelectedIndex = listBox_Info.Items.Count - 1;
                }
            }
            Marshal.FreeHGlobal(pt);
            //       double _dWait=   DateTime.Now.Subtract(dtStar).TotalMilliseconds ;
        }
        /// <summary>
        /// 关闭通讯
        /// </summary>
        public bool Can_Close()
        {
            if (m_bOpen == 1)
            {
                CanCmd.CAN_ChannelStop(m_DeviceHandle, m_dwChannel);

                CanCmd.CAN_DeviceClose(m_DeviceHandle);
                m_bOpen = 0;
                timer_rec.Enabled = false;
            }
            return m_bOpen == 0;
        }
        #endregion Can方法
        /*
        /*
 //十进制转二进制  1！@123ZZZZ!@#123
Console.WriteLine("十进制166的二进制表示: "+Convert.ToString(166, 2));
//十进制转八进制
Console.WriteLine("十进制166的八进制表示: "+Convert.ToString(166, 8));
//十进制转十六进制
Console.WriteLine("十进制166的十六进制表示: "+Convert.ToString(166, 16));

//二进制转十进制
Console.WriteLine("二进制 111101 的十进制表示: "+Convert.ToInt32("111101", 2));
//八进制转十进制
Console.WriteLine("八进制 44 的十进制表示: "+Convert.ToInt32("44", 8));
//十六进制转十进制
Console.WriteLine("十六进制 CC的十进制表示: "+Convert.ToInt32("CC", 16));
 */
        /// <summary>
        /// 校验和 累加校验和
        /// </summary>
        /// <param name="memorySpage"></param>
        /// <returns></returns>
        public string checksumEx(string StrCData)
        {
            StrCData = StrCData.Replace(" ", "");
            string strTmp = string.Empty;
            int iSum = 0;
            for (int i = 0; i < StrCData.Length; i += 2)
                iSum += Convert.ToInt16(StrCData.Substring(i, 2), 16);
            strTmp = StrToHex(iSum.ToString());
            if (strTmp.Length > 2)
                strTmp = strTmp.Substring(strTmp.Length - 2, 2);
            if (strTmp.Length == 1)
                strTmp = "0" + strTmp;
            int _D = Convert.ToInt16(strTmp, 16) % 256;
            strTmp = _D.ToString();
            return StrToHex(strTmp).ToString().ToUpper();
        }
        /// <summary>
        /// 十进制字符串转十六进制字符串
        /// </summary>
        /// <param name="Cmd"></param>
        /// <returns></returns>}
        public static string StrToHex(string str)
        {
            if (str.Length == 0)
                return string.Empty;
            return Convert.ToString(Convert.ToInt64(str), 16);
        }
        /// <summary>
        /// 数据解析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="bits"></param>
        public void ParseDat(object sender, SerialDataReceivedEventArgs e, byte[] btOutArrGet)
        {
            byte[] _btArrGet = new byte[8];//实际报文数据
            bool _blVal = true;
            string strCmdRev = SerialClass.byteToHexStr(btOutArrGet);//可能包含帧头帧尾的接收报文转换字符串
            m_SysBuf.iDatIndex = -1;
            string _strCmd_Type = "";
            //1 判断是否有帧头帧尾和校验码
            #region 帧头帧尾判断
            #region  三、与PC通讯时的协议更改
            /*01	FE	12	A0	4A	      56	00	00	00	20	00	22    	82	0D	0A
            01 FE为帧头用于识别数据的开始
            12为数据长度，表示从第4-15字节的字节个数
            A0在DR中表示运动模式，此处无用；5-12字节为上面8字节的数据报文。
            从第6-12字节为CAN数据，是我们真正要传输的有用数据
            第13字节为校验和
            14-15字节为帧尾，表示数据帧结束 
            */
            #endregion
            /*
            if (btOutArrGet.Length >= 15)
                if (btOutArrGet[0] == 0x01 && btOutArrGet[1] == 0xFE && btOutArrGet[13] == 0x0D && btOutArrGet[14] == 0x0A)//btOutArrGet[13] == 0x01 && btOutArrGet[14] == 0xFE)
                {
                    //判断校验码
                    // if (btArrGet[12].ToString("X2") == checksumEx(strCmdRev))
                    {
                        Array.Copy(btOutArrGet, 4, _btArrGet, 0, _btArrGet.Length);
                        _blVal = true;
                    }
                }
                else if (btOutArrGet.Length == 8)
                {
                    Array.Copy(btOutArrGet, 0, _btArrGet, 0, _btArrGet.Length);
                    _blVal = true;
                }
            */
            #endregion 帧头帧尾

            if (_blVal)
            {
                m_blLink = true;
                strCmdRev = SerialClass.byteToHexStr(_btArrGet);
                #region   1、动作指令
                if (_btArrGet[0] == 0x43 && _btArrGet[1] == 0x52)
                {
                    m_SysBuf.iDatIndex = 10;
                    m_SysBuf.i_Auto_Run = Convert.ToInt32(strCmdRev.Substring(4, 2), 16);
                    m_SysBuf.i_Xz = Convert.ToInt32(strCmdRev.Substring(6, 2), 16);
                    m_SysBuf.i_Sj_Up = Convert.ToInt32(strCmdRev.Substring(8, 2), 16);
                    m_SysBuf.i_Run = Convert.ToInt32(strCmdRev.Substring(10, 2), 16);
                    m_SysBuf.i_Gsb_Hs = Convert.ToInt32(strCmdRev.Substring(12, 2), 16);
                    m_SysBuf.i_Jp = Convert.ToInt32(strCmdRev.Substring(14, 2), 16);
                    _strCmd_Type = "打磨机回传运动状态:";
                    goto JumpPosition;
                    /*
                // 2、打磨机回传运动状态 0x43	0x52	 0xXX	 0xXX	 0xXX	 0xXX	 0xXX	 0xXX
                第三字节 01：自动模式启动 
                  02：自动模式关闭
         第四字节 01：磨头旋转
                  02：旋转停止
         第五字节 01：升降压下
                  02：升降抬起
         第六字节 01：前进
                  02：后退
                  03：停止
                  04：前进左转
                  05：前进右转
                  06：后退左转
                  07：后退右转
         第七字节 01: 横扫启动
                  02：横扫停止
                  03：左扫
                  04：右扫
                  05：满行程左扫（用于设定横扫区间值）
                  06：满行程右扫（用于设定横扫区间值）
         第八字节 01：左纠偏
                  02：右纠偏
                  03：停止纠偏
                */
                }
                #endregion

                #region 二、参数指令
                if (_btArrGet[0] == 0x42 && _btArrGet[1] == 0x4C)
                {
                    m_SysBuf.iDatIndex = 21;
                    m_SysBuf.i_Para_Zxw = Convert.ToInt32(strCmdRev.Substring(8, 4), 16);
                    m_SysBuf.i_Para_Yxw = Convert.ToInt32(strCmdRev.Substring(12, 4), 16);
                    _strCmd_Type = "滑台参数:";
                    goto JumpPosition;
                    // return;
                    //  1、滑台参数0x42  0x4C     0x00    0x00    0x00    0x00    0x00    0x00
                    /*
                      前两字节是指令的针头，用于识别指令的类型；
              第三字节空
              第四字节：00：向打磨机写入参数
                        01：向打磨机读取参数
              第五字节:左限位参数高位
              第六字节；左限位参数低位  不同光栅臂读写数据范围不同，例如0-200
              第七字节：右限位参数高位
              第八字节：右限位参数低位
                     */
                }
                if (_btArrGet[0] == 0x4A && _btArrGet[1] == 0x56)
                {
                    m_SysBuf.iDatIndex = 22;
                    m_SysBuf.Speed = double.Parse(Convert.ToInt32(strCmdRev.Substring(8, 4), 16).ToString());
                    m_SysBuf.i_Para_Speed_Gsb = Convert.ToInt32(strCmdRev.Substring(12, 4), 16);
                    _strCmd_Type = "速度参数:";
                    goto JumpPosition;
                    // return;
                    // 2、速度参数 0x4A	0x56	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
                    /*
                    前两字节是指令的针头，用于识别指令的类型；	 
             第三字节空
             第四字节：00：向打磨机写入参数
                       01：向打磨机读取参数
             第五字节:车体速度参数高位
             第六字节；车体速度参数低位 设定速度例如：0-300 实际速度也基本为此速度
             第七字节：滑台参数高位
             第八字节：滑台参数低位
                    */
                }
                if (_btArrGet[0] == 0x43 && _btArrGet[1] == 0x59)
                {
                    m_SysBuf.iDatIndex = 23;
                    double _dbJuli = double.Parse(Convert.ToInt32(strCmdRev.Substring(8, 8), 16).ToString());
                    m_SysBuf.Trip = _dbJuli;
                    //double _flInd = double.Parse((Math.Abs(_dbJuli) / m_MainBuff.g_Climb.Interval).ToString("f0"));
                    //m_MainBuff.g_Gate.flDistance_X = (float)_dbJuli / 1000f;
                    _strCmd_Type = "车体总行程:" + m_SysBuf.Trip/1000f;
                    goto JumpPosition;
                    // return;
                    //3、车体总行程设定参数
                    /* 0x43	0x59	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
                     前两字节是指令的针头，用于识别指令的类型；	 
              第三字节空
              第四字节：00：向打磨机写入参数 只是回传位置mm
                        01：向打磨机读取参数
             第五六七八字节分别是总行程的高位至低位(因为单位是mm所以参数需要使用四个字节来表达一个参数)

                    */
                }
                if (_btArrGet[0] == 0x4F && _btArrGet[1] == 0x53)
                {
                    m_SysBuf.iDatIndex = 24;
                    m_SysBuf.i_Para_Dbjg = Convert.ToInt32(strCmdRev.Substring(12, 4), 16);
                    _strCmd_Type = "单步间距:" + m_SysBuf.i_Para_Dbjg;
                    goto JumpPosition;
                    return;
                    //4、车体单次行程参数
                    /*0x4F	0x53	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
                     前两字节是指令的针头，用于识别指令的类型；	
             第三字节空
             第四字节：00：向打磨机写入参数 单步间距 mm
                       01：向打磨机读取参数
             第五六字节空
             第七字节：单步参数高位
             第八字节：单步参数低位

                    */
                }
                if (_btArrGet[0] == 0x43 && _btArrGet[1] == 0x4F)
                {
                    m_SysBuf.iDatIndex = 25;
                    m_SysBuf.i_Para_Jp = Convert.ToInt32(strCmdRev.Substring(12, 4), 16);
                    _strCmd_Type = "纠偏参数:" + m_SysBuf.i_Para_Jp;
                    goto JumpPosition;
                    // return;
                    //5、纠偏参数  设置系数，暂时没使用
                    /*0x43	0x4F	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
                    前两字节是指令的针头，用于识别指令的类型；	
             第三字节空
             第四字节：00：向打磨机写入参数
                       01：向打磨机读取参数
             第五六字节空
             第七字节：纠偏参数高位
             第八字节：纠偏参数低位
                    */
                }

                //6、心跳指令
                /*0x48	0x42	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
                    每1.5秒发送一次，用于车体检测通讯是否正常，发送模式为，其他指令成功传输后1.5内没有其他指令发送，则发送一条心跳指令，来维持车体正常运转。
                */
                if (_btArrGet[0] == 0x43 && _btArrGet[1] == 0x58)
                {
                    m_SysBuf.iDatIndex = 27;
                    m_SysBuf.i_Para_Wz = Convert.ToInt32(strCmdRev.Substring(8, 8), 16);

                  //  m_MainBuff.g_Gate.blGsb_RunFx_L_to_R = Convert.ToInt32(strCmdRev.Substring(4, 2), 16) == 1;//1:远离原点 2：向原点运动
                    //  m_blCanCalcu = false;

                    //当前光栅臂位置
                    flDistance_Y = m_SysBuf.i_Para_Wz / 1000f;
                 /*   flDistance_Y = Math.Abs(flDistance_Y);
                    if (m_MainBuff.g_Climb.iRun_Gsb_Sc1_Zx0 == 1)
                    {
                        switch (m_MainBuff.g_Climb.m_iGetDist_Type)
                        {
                            case 0:
                                #region 0
                                if (m_MainBuff.g_Climb.m_iRun == 1)
                                {
                                    #region 判断运行方向  在开始运行头20mm内判断运行方向
                                    //if (flDistance_Y < m_iJugeDist && m_MainBuff.g_Gate.flDistance_Y < m_iJugeDist)
                                    //{
                                    //    // m_blCanCalcu = true;
                                    //    m_MainBuff.g_Gate.blGsb_RunFx_L_to_R = flDistance_Y > m_MainBuff.g_Gate.flDistance_Y;
                                    //}
                                    //else if (flDistance_Y > m_MainBuff.g_ScreenPlant.iGsb_Len - m_iJugeDist && m_MainBuff.g_Gate.flDistance_Y > m_MainBuff.g_ScreenPlant.iGsb_Len - m_iJugeDist)// m_MainBuff.g_Gate.m_UI_StartDistanc)
                                    //{
                                    //    // m_blCanCalcu = true;
                                    //    m_MainBuff.g_Gate.blGsb_RunFx_L_to_R = !(flDistance_Y < m_MainBuff.g_Gate.flDistance_Y);
                                    //}
                                    #endregion  判断运行方向

                                    if (m_MainBuff.g_Gate.blGsb_RunFx_L_to_R && flDistance_Y >= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min && m_MainBuff.g_Gate.flDistance_Y >= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min ||
                                       m_MainBuff.g_Gate.blGsb_RunFx_L_to_R == false && flDistance_Y <= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Max && m_MainBuff.g_Gate.flDistance_Y <= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Max)
                                    {
                                        #region 超声延时数据处理 2021-03-22 1942
                                        if (m_MainBuff.g_Gate.blGsb_RunFx_L_to_R)//光栅臂   从左往右
                                        {
                                            //1 开始测量时延时接收数据
                                            m_MainBuff.g_Gate.m_UI_Start_Bool = flDistance_Y >= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min - m_MainBuff.g_Climb.m_UI_Gsb_Add;

                                            if (m_MainBuff.g_Gate.m_UI_Start_Bool)
                                            {
                                                //2 结束后，再接收20个数据补足剩余20mm距离
                                                if (m_MainBuff.g_Gate.m_UI_End_Bool == false && m_MainBuff.g_Gate.m_UI_EndBuff_Cs != 0) m_MainBuff.g_Gate.m_UI_EndBuff_Cs = 0;
                                                m_MainBuff.g_Gate.m_UI_End_Bool = flDistance_Y >= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Max;

                                                //  if(m_MainBuff.g_Gate.m_UI_StartDistanc > m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min)
                                                flDistance_Y -= (m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min);// +0.04f;
                                                if (flDistance_Y < 0) return;
                                                //else
                                                //    flDistance_Y -= m_MainBuff.g_Gate.m_UI_StartDistanc;
                                            }
                                        }
                                        else //if (m_MainBuff.g_ScreenPlant.iCurrBuffRows % 2 == 1)//光栅臂   从右往左
                                        {
                                            //1 开始测量时延时接收数据
                                            m_MainBuff.g_Gate.m_UI_Start_Bool = flDistance_Y <= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Max + m_MainBuff.g_Climb.m_UI_Gsb_Add;
                                            if (m_MainBuff.g_Gate.m_UI_Start_Bool)
                                            {
                                                //2 结束后，再接收20个数据补足剩余20mm距离
                                                if (m_MainBuff.g_Gate.m_UI_End_Bool == false && m_MainBuff.g_Gate.m_UI_EndBuff_Cs != 0) m_MainBuff.g_Gate.m_UI_EndBuff_Cs = 0;
                                                m_MainBuff.g_Gate.m_UI_End_Bool = flDistance_Y <= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min;

                                                //      flDistance_Y -= m_MainBuff.g_Gate.m_UI_StartDistanc;// + 0.04f;
                                                //   if (m_MainBuff.g_Gate.m_UI_StartDistanc > m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min)
                                                flDistance_Y += m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min;// +0.04f;
                                                if (flDistance_Y > m_MainBuff.g_ScreenPlant.iGsb_Len / 1000f) return;
                                                //else
                                                //    flDistance_Y += m_MainBuff.g_Gate.m_UI_StartDistanc;
                                            }
                                        }
                                        #endregion 超声延时数据处理
                                    }
                                }
                                #endregion  0
                                break;
                            case 1:
                                #region 1
                                #region 超声延时数据处理 2021-03-22 1942
                                if (m_MainBuff.g_Gate.blGsb_RunFx_L_to_R)//光栅臂   从左往右
                                {
                                    //1 开始测量时延时接收数据
                                    flDistance_Y -= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min;// +0.04f;
                                    if (flDistance_Y < m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min) return;
                                }
                                else //光栅臂   从右往左
                                {
                                    //1 开始测量时延时接收数据
                                    flDistance_Y += m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min;// +0.04f;
                                    if (flDistance_Y > m_MainBuff.g_Climb.m_UI_Gsb_Distan_Max) return;
                                }
                                #endregion 超声延时数据处理
                                #endregion 1
                                break;
                            case 2:
                                #region 1
                                #region 超声延时数据处理 2021-03-22 1942
                                if (m_MainBuff.g_Gate.blGsb_RunFx_L_to_R)//光栅臂   从左往右
                                {

                                    //1 开始测量时延时接收数据
                                    flDistance_Y -= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min;
                                    if (flDistance_Y < 0) return;
                                }
                                else //光栅臂   从右往左
                                {
                                    //1 开始测量时延时接收数据
                                    flDistance_Y += m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min;
                                    if (flDistance_Y > m_MainBuff.g_ScreenPlant.iGsb_Len / 1000f) return;
                                }
                                #endregion 超声延时数据处理
                                #endregion 1
                                break;
                        }
                    }
                    flDistance_Y = float.Parse(flDistance_Y.ToString("f3"));
                    m_MainBuff.g_Gate.flDistance_Y = flDistance_Y;
                    */
                    _strCmd_Type = "滑台实时位置:" + m_SysBuf.i_Para_Wz + " " + flDistance_Y;
                   
                    goto JumpPosition;
                    //#region 适应电磁超声延时
                    //flDistance_Y = m_SysBuf.i_Para_Wz / 1000f;
                    //flDistance_Y = Math.Abs(flDistance_Y);

                    //#region 1
                    //#region 超声延时数据处理 2021-03-22 1942
                    //if (m_MainBuff.g_Gate.blGsb_RunFx_L_to_R)//光栅臂   从左往右
                    //{
                    //    //1 开始测量时延时接收数据
                    //    flDistance_Y -= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min;
                    //    if (flDistance_Y < 0) return;
                    //}
                    //else //光栅臂   从右往左
                    //{
                    //    //1 开始测量时延时接收数据
                    //    flDistance_Y += m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min;
                    //    if (flDistance_Y > m_MainBuff.g_ScreenPlant.iGsb_Len / 1000f) return;
                    //}
                    //flDistance_Y = float.Parse(flDistance_Y.ToString("f3"));
                    //m_MainBuff.g_Gate.flDistance_Y = flDistance_Y;
                    //_strCmd_Type = "滑台实时位置:" + m_SysBuf.i_Para_Wz + " " + m_MainBuff.g_Gate.flDistance_Y;
                    //goto JumpPosition;

                    ////    _strCmd_Type = "滑台实时位置:" + m_SysBuf.i_Para_Wz + " " + m_MainBuff.g_Gate.flDistance_Y;
                    //#endregion 超声延时数据处理
                    //#endregion 1
                    //#endregion
                    return;

                    //7、滑台实时位置 （CY）
                    /*0x43	0x58	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
                     前两字节是指令的针头，用于识别指令的类型；	 
              第三字节空
              第四字节：00：向打磨机写入参数
                        01：向打磨机读取参数
              第五六七八字节分别是滑台数据的高位至低位

                    */
                }
                #endregion 二

                #region 三 附加指令
                if (_btArrGet[0] == 0x47 && _btArrGet[1] == 0x59)
                {
                    m_SysBuf.iDatIndex = 28;
                    m_SysBuf.Z = (Convert.ToInt16(strCmdRev.Substring(4, 4), 16) / 100F).ToString("f2");
                    m_SysBuf.Y = (Convert.ToInt16(strCmdRev.Substring(8, 4), 16) / 100F).ToString("f2");
                    m_SysBuf.X = (Convert.ToInt16(strCmdRev.Substring(12, 4), 16) / 100F).ToString("f2");
                    _strCmd_Type = "陀螺仪数据:" + m_SysBuf.X + "," + m_SysBuf.Y + "," + m_SysBuf.Z;
                    goto JumpPosition;
                    /*1、陀螺仪数据 gyroscopic
                    字节	1	2	3	4	5	6	7	8
                     指令示例：	0x47	0x59	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
			                    偏航角（z轴）*100	俯仰角（y轴）*100	滚转角（x轴）*100
                    前两字节是指令的针头，用于识别指令的类型；	 
	                      第三四字节：偏航角（z轴）*100的高位至低位
	                      第三四字节：俯仰角（y轴）*100的高位至低位
                    第三四字节：滚转角（x轴）*100的高位至低位
                    */
                }
                if (_btArrGet[0] == 0x43 && _btArrGet[1] == 0x49)
                {
                    m_SysBuf.iDatIndex = 29;

                    if (Convert.ToInt16(strCmdRev.Substring(4, 2), 16) == 1)//拍照
                        g_Msg_InterFace.Fun_Photo();
                    if (Convert.ToInt16(strCmdRev.Substring(6, 2), 16) == 1)//1开始录像
                        g_Msg_InterFace.Fun_Video(1);
                    if (Convert.ToInt16(strCmdRev.Substring(6, 2), 16) == 2)//2停止录像
                        g_Msg_InterFace.Fun_Video(2);
                    _strCmd_Type = "相机指令  " + "拍照：" + Convert.ToInt16(strCmdRev.Substring(4, 2), 16) + ", 录像：" + Convert.ToInt16(strCmdRev.Substring(6, 2), 16);
                    goto JumpPosition;
                    /*1、相机指令  Camera instruction
                    字节	1	2	3	4	5	6	7	8
                     指令示例：	0x43	0x49	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
                    第三字节  01：拍照 
			  
	                     第四字节 01：开始录像
	                              02：停止录像
                    */
                }
                if (_btArrGet[0] == 0x41 && _btArrGet[1] == 0x4E)
                {
                    m_SysBuf.iDatIndex = 30;

                    m_SysBuf.flAnle = Convert.ToInt16(strCmdRev.Substring(12, 4), 16) / 100f;
                    _strCmd_Type = "陀螺仪校正角度值：  " + m_SysBuf.flAnle;
                    goto JumpPosition;
                    /*陀螺仪矫正 angle  PC iComType_1==1 case 1://陀螺仪矫正 angle
                    字节	1	2	3	4	5	6	7	8
                     指令示例：	0x41	0x4E	 0x00	 0x00	 0x00	 0x00	    0x00	 0x00
                    前两字节是指令的针头，用于识别指令的类型；	 
	                      第三字节空
	                      第四字节：00：向车体写入参数
	                                01：向车体读取参数
	                      第七八字节分别是数据*100的高位至低位

                    */
                }
                #endregion  附加指令
                //if (m_SysBuf.blWinOpen)
                //    g_Msg_InterFace.Fun_UI_ClimbUpData(strCmdRev);
            }
        JumpPosition:
            WriteErrorLog(strLogFileName, (_strCmd_Type + " 报文：") + strCmdRev, false);

        }


        public void ParseDat_Old(object sender, SerialDataReceivedEventArgs e, byte[] btOutArrGet)
        {
            byte[] _btArrGet = new byte[8];//实际报文数据
            bool _blVal = false;
            string strCmdRev = SerialClass.byteToHexStr(btOutArrGet);//可能包含帧头帧尾的接收报文转换字符串
            m_SysBuf.iDatIndex = -1;
            //1 判断是否有帧头帧尾和校验码
            #region 帧头帧尾判断
            #region  三、与PC通讯时的协议更改
            /*01	FE	12	A0	4A	      56	00	00	00	20	00	22    	82	0D	0A
            01 FE为帧头用于识别数据的开始
            12为数据长度，表示从第4-15字节的字节个数
            A0在DR中表示运动模式，此处无用；5-12字节为上面8字节的数据报文。
            从第6-12字节为CAN数据，是我们真正要传输的有用数据
            第13字节为校验和
            14-15字节为帧尾，表示数据帧结束 
            */
            #endregion
            if (btOutArrGet.Length >= 15)
                if (btOutArrGet[0] == 0x01 && btOutArrGet[1] == 0xFE && btOutArrGet[13] == 0x0D && btOutArrGet[14] == 0x0A)//btOutArrGet[13] == 0x01 && btOutArrGet[14] == 0xFE)
                {
                    //判断校验码
                    // if (btArrGet[12].ToString("X2") == checksumEx(strCmdRev))
                    {
                        Array.Copy(btOutArrGet, 4, _btArrGet, 0, _btArrGet.Length);
                        _blVal = true;
                    }
                }
                else if (btOutArrGet.Length == 8)
                {
                    Array.Copy(btOutArrGet, 0, _btArrGet, 0, _btArrGet.Length);
                    _blVal = true;
                }
            #endregion 帧头帧尾

            if (_blVal)
            {
                m_blLink = true;
                strCmdRev = SerialClass.byteToHexStr(_btArrGet);
                #region   1、动作指令
                if (_btArrGet[0] == 0x43 && _btArrGet[1] == 0x52)
                {
                    m_SysBuf.iDatIndex = 10;
                    m_SysBuf.i_Auto_Run = Convert.ToInt32(strCmdRev.Substring(4, 2), 16);
                    m_SysBuf.i_Xz = Convert.ToInt32(strCmdRev.Substring(6, 2), 16);
                    m_SysBuf.i_Sj_Up = Convert.ToInt32(strCmdRev.Substring(8, 2), 16);
                    m_SysBuf.i_Run = Convert.ToInt32(strCmdRev.Substring(10, 2), 16);
                    m_SysBuf.i_Gsb_Hs = Convert.ToInt32(strCmdRev.Substring(12, 2), 16);
                    m_SysBuf.i_Jp = Convert.ToInt32(strCmdRev.Substring(14, 2), 16);
                    return;
                    /*
                // 2、打磨机回传运动状态 0x43	0x52	 0xXX	 0xXX	 0xXX	 0xXX	 0xXX	 0xXX
                第三字节 01：自动模式启动 
                  02：自动模式关闭
         第四字节 01：磨头旋转
                  02：旋转停止
         第五字节 01：升降压下
                  02：升降抬起
         第六字节 01：前进
                  02：后退
                  03：停止
                  04：前进左转
                  05：前进右转
                  06：后退左转
                  07：后退右转
         第七字节 01: 横扫启动
                  02：横扫停止
                  03：左扫
                  04：右扫
                  05：满行程左扫（用于设定横扫区间值）
                  06：满行程右扫（用于设定横扫区间值）
         第八字节 01：左纠偏
                  02：右纠偏
                  03：停止纠偏
                */
                }
                #endregion

                #region 二、参数指令
                if (_btArrGet[0] == 0x42 && _btArrGet[1] == 0x4C)
                {
                    m_SysBuf.iDatIndex = 21;
                    m_SysBuf.i_Para_Zxw = Convert.ToInt32(strCmdRev.Substring(8, 4), 16);
                    m_SysBuf.i_Para_Yxw = Convert.ToInt32(strCmdRev.Substring(12, 4), 16);
                    return;
                    //  1、滑台参数0x42  0x4C     0x00    0x00    0x00    0x00    0x00    0x00
                    /*
                      前两字节是指令的针头，用于识别指令的类型；
              第三字节空
              第四字节：00：向打磨机写入参数
                        01：向打磨机读取参数
              第五字节:左限位参数高位
              第六字节；左限位参数低位  不同光栅臂读写数据范围不同，例如0-200
              第七字节：右限位参数高位
              第八字节：右限位参数低位
                     */
                }
                if (_btArrGet[0] == 0x4A && _btArrGet[1] == 0x56)
                {
                    m_SysBuf.iDatIndex = 22;
                    m_SysBuf.Speed = double.Parse(Convert.ToInt32(strCmdRev.Substring(8, 4), 16).ToString());
                    m_SysBuf.i_Para_Speed_Gsb = Convert.ToInt32(strCmdRev.Substring(12, 4), 16);
                    return;
                    // 2、速度参数 0x4A	0x56	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
                    /*
                    前两字节是指令的针头，用于识别指令的类型；	 
             第三字节空
             第四字节：00：向打磨机写入参数
                       01：向打磨机读取参数
             第五字节:车体速度参数高位
             第六字节；车体速度参数低位 设定速度例如：0-300 实际速度也基本为此速度
             第七字节：滑台参数高位
             第八字节：滑台参数低位
                    */
                }
                if (_btArrGet[0] == 0x43 && _btArrGet[1] == 0x59)
                {
                    m_SysBuf.iDatIndex = 23;
                    m_SysBuf.Trip = double.Parse(Convert.ToInt32(strCmdRev.Substring(8, 8), 16).ToString());
                    return;
                    //3、车体总行程设定参数
                    /* 0x43	0x59	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
                     前两字节是指令的针头，用于识别指令的类型；	 
              第三字节空
              第四字节：00：向打磨机写入参数 只是回传位置mm
                        01：向打磨机读取参数
             第五六七八字节分别是总行程的高位至低位(因为单位是mm所以参数需要使用四个字节来表达一个参数)

                    */
                }
                if (_btArrGet[0] == 0x4F && _btArrGet[1] == 0x53)
                {
                    m_SysBuf.iDatIndex = 24;
                    m_SysBuf.i_Para_Dbjg = Convert.ToInt32(strCmdRev.Substring(12, 4), 16);
                    return;
                    //4、车体单次行程参数
                    /*0x4F	0x53	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
                     前两字节是指令的针头，用于识别指令的类型；	
             第三字节空
             第四字节：00：向打磨机写入参数 单步间距 mm
                       01：向打磨机读取参数
             第五六字节空
             第七字节：单步参数高位
             第八字节：单步参数低位

                    */
                }
                if (_btArrGet[0] == 0x43 && _btArrGet[1] == 0x4F)
                {
                    m_SysBuf.iDatIndex = 25;
                    m_SysBuf.i_Para_Jp = Convert.ToInt32(strCmdRev.Substring(12, 4), 16);
                    return;
                    //5、纠偏参数  设置系数，暂时没使用
                    /*0x43	0x4F	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
                    前两字节是指令的针头，用于识别指令的类型；	
             第三字节空
             第四字节：00：向打磨机写入参数
                       01：向打磨机读取参数
             第五六字节空
             第七字节：纠偏参数高位
             第八字节：纠偏参数低位
                    */
                }

                //6、心跳指令
                /*0x48	0x42	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
                    每1.5秒发送一次，用于车体检测通讯是否正常，发送模式为，其他指令成功传输后1.5内没有其他指令发送，则发送一条心跳指令，来维持车体正常运转。
                */
                if (_btArrGet[0] == 0x43 && _btArrGet[1] == 0x58)
                {
                    m_SysBuf.iDatIndex = 27;
                    m_SysBuf.i_Para_Wz = Convert.ToInt32(strCmdRev.Substring(8, 8), 16);
                    return;

                    //7、滑台实时位置 （CY）
                    /*0x43	0x58	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
                     前两字节是指令的针头，用于识别指令的类型；	 
              第三字节空
              第四字节：00：向打磨机写入参数
                        01：向打磨机读取参数
              第五六七八字节分别是滑台数据的高位至低位

                    */
                }
                #endregion 二
                //if (m_SysBuf.blWinOpen)
                //    g_Msg_InterFace.Fun_UI_ClimbUpData(strCmdRev);
            }
        }
        /// <summary>
        /// /解析接收报文
        /// </summary>
        /// <param name="_btArrGet"></param>
        public void ParseDat_Can(byte[] _btArrGet) 
        {
            //  啊  byte[] _btArrGet = new byte[8];//实际报文数据
            bool _blVal = true;
            string strCmdRev = "";// SerialClass.byteToHexStr(btOutArrGet);//可能包含帧头帧尾的接收报文转换字符串
            m_SysBuf.iDatIndex = -1;
            string _strCmd_Type = "";
            //1 判断是否有帧头帧尾和校验码
            #region 帧头帧尾判断
            #region  三、与PC通讯时的协议更改
            /*01	FE	12	A0	4A	      56	00	00	00	20	00	22    	82	0D	0A
            01 FE为帧头用于识别数据的开始
            12为数据长度，表示从第4-15字节的字节个数
            A0在DR中表示运动模式，此处无用；5-12字节为上面8字节的数据报文。
            从第6-12字节为CAN数据，是我们真正要传输的有用数据
            第13字节为校验和
            14-15字节为帧尾，表示数据帧结束 
            */
            #endregion
            //if (btOutArrGet.Length >= 15)
            //    if (btOutArrGet[0] == 0x01 && btOutArrGet[1] == 0xFE && btOutArrGet[13] == 0x0D && btOutArrGet[14] == 0x0A)//btOutArrGet[13] == 0x01 && btOutArrGet[14] == 0xFE)
            //    {
            //        //判断校验码
            //        // if (btArrGet[12].ToString("X2") == checksumEx(strCmdRev))
            //        {
            //            Array.Copy(btOutArrGet, 4, _btArrGet, 0, _btArrGet.Length);
            //            _blVal = true;
            //        }
            //    }
            //    else if (btOutArrGet.Length == 8)
            //    {
            //        Array.Copy(btOutArrGet, 0, _btArrGet, 0, _btArrGet.Length);
            //        _blVal = true;
            //    }
            #endregion 帧头帧尾

            if (_blVal)
            {
                m_blLink = true;
                strCmdRev = SerialClass.byteToHexStr(_btArrGet);
                #region   1、动作指令
                if (_btArrGet[0] == 0x43 && _btArrGet[1] == 0x52)
                {
                    m_SysBuf.iDatIndex = 10;
                    m_SysBuf.i_Auto_Run = Convert.ToInt32(strCmdRev.Substring(4, 2), 16);
                    m_SysBuf.i_Xz = Convert.ToInt32(strCmdRev.Substring(6, 2), 16);
                    m_SysBuf.i_Sj_Up = Convert.ToInt32(strCmdRev.Substring(8, 2), 16);
                    m_SysBuf.i_Run = Convert.ToInt32(strCmdRev.Substring(10, 2), 16);
                    m_SysBuf.i_Gsb_Hs = Convert.ToInt32(strCmdRev.Substring(12, 2), 16);
                    m_SysBuf.i_Jp = Convert.ToInt32(strCmdRev.Substring(14, 2), 16);
                    _strCmd_Type = "打磨机回传运动状态:";
                    goto JumpPosition;
                    /*
                // 2、打磨机回传运动状态 0x43	0x52	 0xXX	 0xXX	 0xXX	 0xXX	 0xXX	 0xXX
                第三字节 01：自动模式启动 
                  02：自动模式关闭
         第四字节 01：磨头旋转
                  02：旋转停止
         第五字节 01：升降压下
                  02：升降抬起
         第六字节 01：前进
                  02：后退
                  03：停止
                  04：前进左转
                  05：前进右转
                  06：后退左转
                  07：后退右转
         第七字节 01: 横扫启动
                  02：横扫停止
                  03：左扫
                  04：右扫
                  05：满行程左扫（用于设定横扫区间值）
                  06：满行程右扫（用于设定横扫区间值）
         第八字节 01：左纠偏
                  02：右纠偏
                  03：停止纠偏
                */
                }
                #endregion

                #region 二、参数指令
                if (_btArrGet[0] == 0x42 && _btArrGet[1] == 0x4C)
                {
                    m_SysBuf.iDatIndex = 21;
                    m_SysBuf.i_Para_Zxw = Convert.ToInt32(strCmdRev.Substring(8, 4), 16);
                    m_SysBuf.i_Para_Yxw = Convert.ToInt32(strCmdRev.Substring(12, 4), 16);
                    _strCmd_Type = "滑台参数:";
                    goto JumpPosition;
                    //  1、滑台参数0x42  0x4C     0x00    0x00    0x00    0x00    0x00    0x00
                    /*
                      前两字节是指令的针头，用于识别指令的类型；
              第三字节空
              第四字节：00：向打磨机写入参数
                        01：向打磨机读取参数
              第五字节:左限位参数高位
              第六字节；左限位参数低位  不同光栅臂读写数据范围不同，例如0-200
              第七字节：右限位参数高位
              第八字节：右限位参数低位
                     */
                }
                if (_btArrGet[0] == 0x4A && _btArrGet[1] == 0x56)
                {
                    m_SysBuf.iDatIndex = 22;
                    m_SysBuf.Speed = double.Parse(Convert.ToInt32(strCmdRev.Substring(8, 4), 16).ToString());
                    m_SysBuf.i_Para_Speed_Gsb = Convert.ToInt32(strCmdRev.Substring(12, 4), 16);
                    _strCmd_Type = "速度参数:";
                    goto JumpPosition;
                    // 2、速度参数 0x4A	0x56	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
                    /*
                    前两字节是指令的针头，用于识别指令的类型；	 
             第三字节空
             第四字节：00：向打磨机写入参数
                       01：向打磨机读取参数
             第五字节:车体速度参数高位
             第六字节；车体速度参数低位 设定速度例如：0-300 实际速度也基本为此速度
             第七字节：滑台参数高位
             第八字节：滑台参数低位
                    */
                }
                if (_btArrGet[0] == 0x43 && _btArrGet[1] == 0x59)
                {
                    m_SysBuf.iDatIndex = 23;
                    double _dbJuli=double.Parse(Convert.ToInt32(strCmdRev.Substring(8, 8), 16).ToString());
                    m_SysBuf.Trip_Com_mm = (float)_dbJuli;
                    //m_SysBuf.Trip = _dbJuli / 1000f;
                    //double  _flInd =double .Parse ( (Math.Abs(_dbJuli) / m_MainBuff.g_Climb.Interval).ToString ("f0"));

                    //m_MainBuff.g_Gate.blGsb_RunFx_L_to_R = (int)(_flInd) % 2 == 0;

                    //m_MainBuff.g_Gate.flDistance_X = (float)_dbJuli/1000f;
                    _strCmd_Type = "车体总行程:" + m_SysBuf.Trip;
                    goto JumpPosition;
                    //3、车体总行程设定参数
                    /* 0x43	0x59	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
                     前两字节是指令的针头，用于识别指令的类型；	 
              第三字节空
              第四字节：00：向打磨机写入参数 只是回传位置mm
                        01：向打磨机读取参数
             第五六七八字节分别是总行程的高位至低位(因为单位是mm所以参数需要使用四个字节来表达一个参数)

                    */
                }
                if (_btArrGet[0] == 0x4F && _btArrGet[1] == 0x53)
                {
                    m_SysBuf.iDatIndex = 24;
                    m_SysBuf.i_Para_Dbjg = Convert.ToInt32(strCmdRev.Substring(12, 4), 16);
                    _strCmd_Type = "单步间距:"+ m_SysBuf.i_Para_Dbjg;
                    goto JumpPosition;
                    //4、车体单次行程参数
                    /*0x4F	0x53	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
                     前两字节是指令的针头，用于识别指令的类型；	
             第三字节空
             第四字节：00：向打磨机写入参数 单步间距 mm
                       01：向打磨机读取参数
             第五六字节空
             第七字节：单步参数高位
             第八字节：单步参数低位

                    */
                }
                if (_btArrGet[0] == 0x43 && _btArrGet[1] == 0x4F)
                {
                    m_SysBuf.iDatIndex = 25;
                    m_SysBuf.i_Para_Jp = Convert.ToInt32(strCmdRev.Substring(12, 4), 16);
                    _strCmd_Type = "纠偏参数:" + m_SysBuf.i_Para_Jp;
                    goto JumpPosition;
                    //5、纠偏参数  设置系数，暂时没使用
                    /*0x43	0x4F	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
                    前两字节是指令的针头，用于识别指令的类型；	
             第三字节空
             第四字节：00：向打磨机写入参数
                       01：向打磨机读取参数
             第五六字节空
             第七字节：纠偏参数高位
             第八字节：纠偏参数低位
                    */
                }

                //6、心跳指令
                /*0x48	0x42	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
                    每1.5秒发送一次，用于车体检测通讯是否正常，发送模式为，其他指令成功传输后1.5内没有其他指令发送，则发送一条心跳指令，来维持车体正常运转。
                */
                if (_btArrGet[0] == 0x43 && _btArrGet[1] == 0x58)
                {
                    m_SysBuf.iDatIndex = 27;
                    m_SysBuf.i_Para_Wz = Convert.ToInt32(strCmdRev.Substring(8, 8), 16);
                  //  m_blCanCalcu = false;
                   
                    //当前光栅臂位置
                    //flDistance_Y = m_SysBuf.i_Para_Wz / 1000f;
                    //flDistance_Y = Math.Abs(flDistance_Y);
                    //switch (m_MainBuff.g_Climb.m_iGetDist_Type)
                    //{
                    //    case 0:
                    //        #region 0
                    //        if (m_MainBuff.g_Climb.m_iRun == 1)
                    //        {
                    //            #region 判断运行方向  在开始运行头20mm内判断运行方向
                    //            //if (flDistance_Y < m_iJugeDist && m_MainBuff.g_Gate.flDistance_Y < m_iJugeDist)
                    //            //{
                    //            //    // m_blCanCalcu = true;
                    //            //    m_MainBuff.g_Gate.blGsb_RunFx_L_to_R = flDistance_Y > m_MainBuff.g_Gate.flDistance_Y;
                    //            //}
                    //            //else if (flDistance_Y > m_MainBuff.g_ScreenPlant.iGsb_Len - m_iJugeDist && m_MainBuff.g_Gate.flDistance_Y > m_MainBuff.g_ScreenPlant.iGsb_Len - m_iJugeDist)// m_MainBuff.g_Gate.m_UI_StartDistanc)
                    //            //{
                    //            //    // m_blCanCalcu = true;
                    //            //    m_MainBuff.g_Gate.blGsb_RunFx_L_to_R = !(flDistance_Y < m_MainBuff.g_Gate.flDistance_Y);
                    //            //}
                    //            #endregion  判断运行方向

                    //            if (m_MainBuff.g_Gate.blGsb_RunFx_L_to_R && flDistance_Y >= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min && m_MainBuff.g_Gate.flDistance_Y >= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min ||
                    //               m_MainBuff.g_Gate.blGsb_RunFx_L_to_R == false && flDistance_Y <= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Max && m_MainBuff.g_Gate.flDistance_Y <= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Max)
                    //            {
                    //                #region 超声延时数据处理 2021-03-22 1942
                    //                if (m_MainBuff.g_Gate.blGsb_RunFx_L_to_R)//光栅臂   从左往右
                    //                {
                    //                    //1 开始测量时延时接收数据
                    //                    m_MainBuff.g_Gate.m_UI_Start_Bool = flDistance_Y >= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min - m_MainBuff.g_Climb.m_UI_Gsb_Add;

                    //                    if (m_MainBuff.g_Gate.m_UI_Start_Bool)
                    //                    {
                    //                        //2 结束后，再接收20个数据补足剩余20mm距离
                    //                        if (m_MainBuff.g_Gate.m_UI_End_Bool == false && m_MainBuff.g_Gate.m_UI_EndBuff_Cs != 0) m_MainBuff.g_Gate.m_UI_EndBuff_Cs = 0;
                    //                        m_MainBuff.g_Gate.m_UI_End_Bool = flDistance_Y >= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Max;

                    //                        //  if(m_MainBuff.g_Gate.m_UI_StartDistanc > m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min)
                    //                        flDistance_Y -= (m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min);// +0.04f;
                    //                        if (flDistance_Y < 0) return;
                    //                        //else
                    //                        //    flDistance_Y -= m_MainBuff.g_Gate.m_UI_StartDistanc;
                    //                    }
                    //                }
                    //                else //if (m_MainBuff.g_ScreenPlant.iCurrBuffRows % 2 == 1)//光栅臂   从右往左
                    //                {
                    //                    //1 开始测量时延时接收数据
                    //                    m_MainBuff.g_Gate.m_UI_Start_Bool = flDistance_Y <= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Max + m_MainBuff.g_Climb.m_UI_Gsb_Add;
                    //                    if (m_MainBuff.g_Gate.m_UI_Start_Bool)
                    //                    {
                    //                        //2 结束后，再接收20个数据补足剩余20mm距离
                    //                        if (m_MainBuff.g_Gate.m_UI_End_Bool == false && m_MainBuff.g_Gate.m_UI_EndBuff_Cs != 0) m_MainBuff.g_Gate.m_UI_EndBuff_Cs = 0;
                    //                        m_MainBuff.g_Gate.m_UI_End_Bool = flDistance_Y <= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min;

                    //                        //      flDistance_Y -= m_MainBuff.g_Gate.m_UI_StartDistanc;// + 0.04f;
                    //                        //   if (m_MainBuff.g_Gate.m_UI_StartDistanc > m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min)
                    //                        flDistance_Y += m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min;// +0.04f;
                    //                        if (flDistance_Y > m_MainBuff.g_ScreenPlant.iGsb_Len / 1000f) return;
                    //                        //else
                    //                        //    flDistance_Y += m_MainBuff.g_Gate.m_UI_StartDistanc;
                    //                    }
                    //                }
                    //                #endregion 超声延时数据处理
                    //            }
                    //        }
                    //        #endregion  0
                    //        break;
                    //    case 1:
                    //        #region 1
                    //        #region 超声延时数据处理 2021-03-22 1942
                    //        if (m_MainBuff.g_Gate.blGsb_RunFx_L_to_R)//光栅臂   从左往右
                    //        {
                    //            //1 开始测量时延时接收数据
                    //            flDistance_Y -= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min;// +0.04f;
                    //            if (flDistance_Y < m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min) return;
                    //        }
                    //        else //光栅臂   从右往左
                    //        {
                    //            //1 开始测量时延时接收数据
                    //            flDistance_Y += m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min;// +0.04f;
                    //            if (flDistance_Y > m_MainBuff.g_Climb.m_UI_Gsb_Distan_Max) return;
                    //        }
                    //        #endregion 超声延时数据处理
                    //        #endregion 1
                    //        break;
                    //    case 2:
                    //        #region 1
                    //        #region 超声延时数据处理 2021-03-22 1942
                    //        if (m_MainBuff.g_Gate.blGsb_RunFx_L_to_R)//光栅臂   从左往右
                    //        {
                    //            //1 开始测量时延时接收数据
                    //            flDistance_Y -= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min;// +0.04f;
                    //            if (flDistance_Y < 0) return;
                    //            if (flDistance_Y >= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Max-0.001f)
                    //            {
                    //                //启动接收剩余数据 >280
                    //                m_MainBuff.g_Gate.m_UI_EndTime = DateTime.Now;

                    //            }
                    //        }
                    //        else //光栅臂   从右往左
                    //        {
                    //            //1 开始测量时延时接收数据
                    //            flDistance_Y += m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min;// +0.04f;
                    //            if (flDistance_Y > m_MainBuff.g_ScreenPlant.iGsb_Len/1000f) return;
                    //            if (flDistance_Y <= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min-0.001f)
                    //            {
                    //                //启动接收剩余数据 <20
                    //                m_MainBuff.g_Gate.m_UI_EndTime = DateTime.Now;
                    //            }
                    //        }
                    //        #endregion 超声延时数据处理
                    //        #endregion 1
                    //        break;
                    //}
                    //flDistance_Y =float .Parse ( flDistance_Y.ToString("f3"));
                    //m_MainBuff.g_Gate.flDistance_Y = flDistance_Y;
                    //_strCmd_Type = "滑台实时位置:" + m_SysBuf.i_Para_Wz + " " + m_MainBuff.g_Gate.flDistance_Y;
                    goto JumpPosition;

                    //7、滑台实时位置 （CY）
                    /*0x43	0x58	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
                     前两字节是指令的针头，用于识别指令的类型；	 
              第三字节空
              第四字节：00：向打磨机写入参数
                        01：向打磨机读取参数
              第五六七八字节分别是滑台数据的高位至低位

                    */
                }
                #endregion 二
                #region 三 附加指令
                if (_btArrGet[0] == 0x47 && _btArrGet[1] == 0x59)
                {
                    m_SysBuf.iDatIndex = 28;
                    m_SysBuf.Z = (Convert.ToInt16(strCmdRev.Substring(4, 4), 16) / 100F).ToString("f2");
                    m_SysBuf.Y = (Convert.ToInt16(strCmdRev.Substring(8, 4), 16) / 100F).ToString("f2");
                    m_SysBuf.X = (Convert.ToInt16(strCmdRev.Substring(12, 4), 16) / 100F).ToString("f2");
                    _strCmd_Type = "陀螺仪数据:" + m_SysBuf.X + "," + m_SysBuf.Y + "," + m_SysBuf.Z;
                    goto JumpPosition;
                    /*1、陀螺仪数据 gyroscopic
                    字节	1	2	3	4	5	6	7	8
                     指令示例：	0x47	0x59	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
			                    偏航角（z轴）*100	俯仰角（y轴）*100	滚转角（x轴）*100
                    前两字节是指令的针头，用于识别指令的类型；	 
	                      第三四字节：偏航角（z轴）*100的高位至低位
	                      第三四字节：俯仰角（y轴）*100的高位至低位
                    第三四字节：滚转角（x轴）*100的高位至低位
                    */
                }
                if (_btArrGet[0] == 0x43 && _btArrGet[1] == 0x49)
                {
                    m_SysBuf.iDatIndex = 29;

                    if (Convert.ToInt16(strCmdRev.Substring(4, 2), 16) == 1)//拍照
                        g_Msg_InterFace.Fun_Photo();
                    if (Convert.ToInt16(strCmdRev.Substring(6, 2), 16) == 1)//1开始录像
                        g_Msg_InterFace.Fun_Video(1);
                    if (Convert.ToInt16(strCmdRev.Substring(6, 2), 16) == 2)//2停止录像
                        g_Msg_InterFace.Fun_Video(2);
                    _strCmd_Type = "相机指令  " + "拍照：" + Convert.ToInt16(strCmdRev.Substring(4, 2), 16) + ", 录像：" + Convert.ToInt16(strCmdRev.Substring(6, 2), 16);
                    goto JumpPosition;
                    /*1、相机指令  Camera instruction
                    字节	1	2	3	4	5	6	7	8
                     指令示例：	0x43	0x49	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
                    第三字节  01：拍照 
			  
	                     第四字节 01：开始录像
	                              02：停止录像
                    */
                }
                if (_btArrGet[0] == 0x41 && _btArrGet[1] == 0x4E)
                {
                    m_SysBuf.iDatIndex = 30;

                    m_SysBuf.flAnle = Convert.ToInt16(strCmdRev.Substring(12, 4), 16) / 100f;
                    _strCmd_Type = "陀螺仪校正角度值：  " + m_SysBuf.flAnle;
                    goto JumpPosition;
                    /*陀螺仪矫正 angle  PC iComType_1==1 case 1://陀螺仪矫正 angle
                    字节	1	2	3	4	5	6	7	8
                     指令示例：	0x41	0x4E	 0x00	 0x00	 0x00	 0x00	    0x00	 0x00
                    前两字节是指令的针头，用于识别指令的类型；	 
	                      第三字节空
	                      第四字节：00：向车体写入参数
	                                01：向车体读取参数
	                      第七八字节分别是数据*100的高位至低位

                    */
                }
                #endregion  附加指令
                //if (m_SysBuf.blWinOpen)
                //    g_Msg_InterFace.Fun_UI_ClimbUpData(strCmdRev);
            }
        JumpPosition:
            WriteErrorLog(strLogFileName, (_strCmd_Type + " 报文：") + strCmdRev, false);
        }

        /// <summary>
        /// 打磨机CAN指令
        /// </summary>
        /// <param name="iComType_0">一、动作类指令 二、参数指令 四、磁粉项目增加指令</param>
        /// <param name="iComType_1">命令类型</param>
        /// <param name="iIndex">单字节命令：字节序号3-8</param>
        /// <param name="iRw">1读0写</param>
        /// <param name="strVal">值，多个数据以'，'间隔</param>
        /// <returns></returns>
        public string SendData(int iComType_0, int iComType_1, int iIndex = 3, int iRw = 1, string strVal = "")
        {

            if (m_blLink == false) return "";

            bool _blRet = false;
            byte[] _btArrSend = new byte[8];
            int _iNo = 0;//报文指针
            int _iT = 0;//临时
            bool _blVal = true;//数据是否有效
            string[] _sPara = "".Split(',');
            #region 组织报文帧
            switch (iComType_0)
            {
                case 1://一、动作类指令
                    #region 
                    _btArrSend[_iNo++] = 0x43; _btArrSend[_iNo++] = 0x52;
                    switch (iComType_1)
                    {
                        case 1://1、动作指令
                            #region 第3-8字节
                            switch (iIndex)//字节位置
                            {
                                case 3:
                                    switch (int.Parse(strVal))
                                    {
                                        case 1://前进
                                            _btArrSend[--iIndex] = 0x01;//自动模式前进启动
                                            break;
                                        case 2:
                                            _btArrSend[--iIndex] = 0x02;//自动模式关闭
                                            break;
                                        case 3://后退
                                            _btArrSend[--iIndex] = 0x03;//自动模式后退启动 2021-03-10增加
                                            break;
                                        case 6://前进
                                            _btArrSend[--iIndex] = 0x06;//光栅臂到达指定位置的前进自动模式启动 2021-04-30增加
                                            break;
                                        case 7://后退
                                            _btArrSend[--iIndex] = 0x07;//光栅臂到达指定位置的后退自动模式启动 2021-04-30增加
                                            break;
                                    }
                                    break;
                                case 4:
                                    if (int.Parse(strVal) == 1)
                                        _btArrSend[--iIndex] = 0x01;//磨头旋转
                                    else
                                        _btArrSend[--iIndex] = 0x02;//旋转停止
                                    break;
                                case 5:
                                    if (int.Parse(strVal) == 1)
                                        _btArrSend[--iIndex] = 0x01;//升降压下
                                    else
                                        _btArrSend[--iIndex] = 0x02;//升降抬起
                                    break;
                                case 6://01：前进    02：后退      03：停止 04：前进左转 
                                       //05：前进右转 06：后退左转 07：后退右转
                                    _iT = int.Parse(strVal);

                                    if (_iT > 0 && _iT < 8)
                                        _btArrSend[--iIndex] = (byte)_iT;
                                    else
                                        _blVal = false;
                                    break;
                                case 7://01：横扫启动  02：横扫停止 03：左扫
                                       //04：右扫  05：满行程左扫（用于设定横扫区间值）
                                       //06：满行程右扫（用于设定横扫区间值）
                                       //10: 行走到指定位置1（C扫光栅臂到达指定位置）
                                       //11: 行走到指定位置2（C扫光栅臂到达指定位置）
                                    _iT = int.Parse(strVal);

                                    if (_iT > 0 && _iT < 7)
                                        _btArrSend[--iIndex] = (byte)_iT;
                                    else
                                        _blVal = false;
                                    break;
                                case 8://第八字节 01：左纠偏
                                       //02：右纠偏
                                       //03：停止纠偏
                                    _iT = int.Parse(strVal);

                                    if (_iT > 0 && _iT < 4)
                                        _btArrSend[--iIndex] = (byte)_iT;
                                    else
                                        _blVal = false;
                                    break;
                            }

                            #endregion
                            break;
                        case 3://3、向打磨机索取状态指令
                            #region 
                            _btArrSend[3] = 0x99;
                            #endregion
                            break;
                    }
                    #endregion
                    break;
                case 2://二、参数指令
                    #region 
                    switch (iComType_1)
                    {
                        case 1://1、滑台参数
                            _btArrSend[_iNo++] = 0x42; _btArrSend[_iNo++] = 0x4C;
                            #region 
                            if (iRw == 0)
                                _btArrSend[3] = 0x0;// 00：向打磨机写入参数 
                            else
                                _btArrSend[3] = 0x01;//01：向打磨机读取参数
                            _sPara = strVal.Split(',');//车体速度参数,滑台参数
                            if (_sPara.Length == 2)
                            {
                                string _str16 = Convert.ToString(int.Parse(_sPara[0]), 16);
                                _str16 = strAddLen_Num(_str16);
                                _btArrSend[4] = Convert.ToByte(_str16.Substring(0, 2), 16); //第五字节:左限位参数高位
                                _btArrSend[5] = Convert.ToByte(_str16.Substring(2, 2), 16); ;//第六字节；左限位参数低位  不同光栅臂读写数据范围不同，例如0-200

                                _str16 = Convert.ToString(int.Parse(_sPara[1]), 16);
                                _str16 = strAddLen_Num(_str16);
                                _btArrSend[6] = Convert.ToByte(_str16.Substring(0, 2), 16);//第七字节：右限位参数高位
                                _btArrSend[7] = Convert.ToByte(_str16.Substring(2, 2), 16);//第八字节：右限位参数低位
                            }
                            else
                                _blVal = false;

                            #endregion
                            break;
                        case 2://2、速度参数
                            _btArrSend[_iNo++] = 0x4A; _btArrSend[_iNo++] = 0x56;
                            #region 
                            if (iRw == 0)
                                _btArrSend[3] = 0x0;// 00：向打磨机写入参数 
                            else
                                _btArrSend[3] = 0x01;//01：向打磨机读取参数
                            _sPara = strVal.Split(',');//车体速度参数,滑台参数
                            if (_sPara.Length == 2)
                            {
                                string _str16 = Convert.ToString(int.Parse(_sPara[0]), 16);
                                _str16 = strAddLen_Num(_str16);
                                _btArrSend[4] = Convert.ToByte(_str16.Substring(0, 2), 16); //第五字节:车体速度参数高位
                                _btArrSend[5] = Convert.ToByte(_str16.Substring(2, 2), 16); ;//第六字节；车体速度参数低位

                                _str16 = Convert.ToString(int.Parse(_sPara[1]), 16);
                                _str16 = strAddLen_Num(_str16);
                                _btArrSend[6] = Convert.ToByte(_str16.Substring(0, 2), 16);//第七字节：滑台参数高位
                                _btArrSend[7] = Convert.ToByte(_str16.Substring(2, 2), 16);//第八字节：滑台参数低位
                            }
                            else
                                _blVal = false;

                            #endregion
                            break;
                        case 3://3、车体总行程设定参数
                            _btArrSend[_iNo++] = 0x43; _btArrSend[_iNo++] = 0x59;
                            #region 
                            if (iRw == 0)
                                _btArrSend[3] = 0x0;// 00：向打磨机写入参数 
                            else
                                _btArrSend[3] = 0x01;//01：向打磨机读取参数

                            strVal = Convert.ToString(int.Parse(strVal), 16);
                            strVal = strAddLen_Num(strVal, 8);
                            _btArrSend[4] = Convert.ToByte(strVal.Substring(0, 2), 16); //第五六七八字节分别是总行程的
                            _btArrSend[5] = Convert.ToByte(strVal.Substring(2, 2), 16); //高位至低位(因为单位是mm所以
                            _btArrSend[6] = Convert.ToByte(strVal.Substring(4, 2), 16); //参数需要使用四个字节来表达一个参数)
                            _btArrSend[7] = Convert.ToByte(strVal.Substring(6, 2), 16); //
                            #endregion
                            break;
                        case 4://4、车体单次行程参数
                            _btArrSend[_iNo++] = 0x4F; _btArrSend[_iNo++] = 0x53;
                            #region 
                            if (iRw == 0)
                                _btArrSend[3] = 0x0;// 00：向打磨机写入参数  单步间距 mm
                            else
                                _btArrSend[3] = 0x01;//01：向打磨机读取参数

                            strVal = Convert.ToString(int.Parse(strVal), 16);
                            strVal = strAddLen_Num(strVal);
                            _btArrSend[6] = Convert.ToByte(strVal.Substring(0, 2), 16); // 第七字节：单步参数高位
                            _btArrSend[7] = Convert.ToByte(strVal.Substring(2, 2), 16); //第八字节：单步参数低位
                            #endregion
                            break;
                        case 5://5、纠偏参数  设置系数，暂时没使用
                            _btArrSend[_iNo++] = 0x43; _btArrSend[_iNo++] = 0x4F;
                            #region 
                            if (iRw == 0)
                                _btArrSend[3] = 0x0;// 00：向打磨机写入参数 
                            else
                                _btArrSend[3] = 0x01;//01：向打磨机读取参数

                            strVal = Convert.ToString(int.Parse(strVal), 16);
                            strVal = strAddLen_Num(strVal);
                            _btArrSend[6] = Convert.ToByte(strVal.Substring(0, 2), 16); //第七字节：纠偏参数高位
                            _btArrSend[7] = Convert.ToByte(strVal.Substring(2, 2), 16); //第八字节：纠偏参数低位
                            #endregion
                            break;
                        case 6://6、心跳指令
                            //每1.5秒发送一次，用于车体检测通讯是否正常，发送模式为，其他指令成功传输后1.5内没有其他指令发送，则发送一条心跳指令，来维持车体正常运转。
                            _btArrSend[_iNo++] = 0x48; _btArrSend[_iNo++] = 0x42;
                            break;
                        case 7://7、滑台实时位置 （CY）
                            _btArrSend[_iNo++] = 0x43; _btArrSend[_iNo++] = 0x58;
                            #region 
                            if (iRw == 0)
                                _btArrSend[3] = 0x0;// 00：向打磨机写入参数 
                            else
                                _btArrSend[3] = 0x01;//01：向打磨机读取参数

                            strVal = Convert.ToString(int.Parse(strVal), 16);
                            strVal = strAddLen_Num(strVal, 8);
                            _btArrSend[4] = Convert.ToByte(strVal.Substring(0, 2), 16); //第五六七八字节分别是滑台数据的高位至低位
                            _btArrSend[5] = Convert.ToByte(strVal.Substring(2, 2), 16);
                            _btArrSend[6] = Convert.ToByte(strVal.Substring(4, 2), 16);
                            _btArrSend[7] = Convert.ToByte(strVal.Substring(6, 2), 16);
                            #endregion
                            break;
                    }
                    break;
                #endregion
                case 4://四、磁粉项目增加指令
                    #region 
                    _btArrSend[_iNo++] = 0x4D; _btArrSend[_iNo++] = 0x54;
                    switch (iComType_1)
                    {
                        case 1://1、动作指令
                            #region 第3-8字节
                            switch (iIndex)
                            {
                                case 3://第三字节 01：前后灯光级别设置，
                                    //01代表关灯。
                                    //0A:灯亮度达到最高，端口PWM输出电压达到巅峰。
                                    strVal = Convert.ToString(int.Parse(strVal), 16);
                                    strVal = strAddLen_Num(strVal, 2);
                                    _btArrSend[--iIndex] = Convert.ToByte(strVal.Substring(0, 2), 16);
                                    break;
                                case 4://第四字节 01：左右灯光级别设置，01代表关灯。
                                    //01代表关灯。
                                    //0A:灯亮度达到最高，端口PWM输出电压达到巅峰。
                                    strVal = Convert.ToString(int.Parse(strVal), 16);
                                    strVal = strAddLen_Num(strVal, 2);
                                    _btArrSend[--iIndex] = Convert.ToByte(strVal.Substring(0, 2), 16);
                                    break;
                                case 5://第五字节
                                    if (int.Parse(strVal) == 1)
                                        _btArrSend[3] = 0x1;// 磁化 
                                    else
                                        _btArrSend[3] = 0x02;//解除磁化
                                    break;
                                case 6://第六字节 01：喷磁悬液
                                    if (int.Parse(strVal) == 1)
                                        _btArrSend[3] = 0x1;// 喷磁悬液 
                                    else
                                        _btArrSend[3] = 0x02;//停止喷磁悬液
                                    break;
                            }
                            #endregion
                            break;
                        case 2://二、手动速度参数
                            #region 
                            /*
                            备注：因为演示使用过程中手动模式车体速度与小车自动模式速度差异较大，
                            所以增加本条指令用于区分，原速度指令用于自动模式的速度，
                            本条指令用于手动模式的速度。  */
                            _btArrSend[_iNo++] = 0x4D; _btArrSend[_iNo++] = 0x83;
                            _sPara = strVal.Split(',');//车体速度参数
                            strVal = Convert.ToString(int.Parse(_sPara[0]), 16);
                            strVal = strAddLen_Num(strVal, 2);
                            _btArrSend[6] = Convert.ToByte(strVal.Substring(0, 2), 16); //第七字节：手动速度高位
                            _btArrSend[7] = Convert.ToByte(strVal.Substring(2, 2), 16); //第八字节：手动速度低位
                            #endregion
                            break;
                        case 3://二、自动速度参数
                            #region   备注
                            /*
                            备注：因为演示使用过程中手动模式车体速度与小车自动模式速度差异较大，
                            所以增加本条指令用于区分，原速度指令用于自动模式的速度，
                            本条指令用于手动模式的速度。 
                            前两字节是指令的针头，用于识别指令的类型；
                             指令示例：	0x4D	0x53	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
	                         第三字节:空
	                         第四字节:空
                             第五字节: 车体自动速度高位
	                         第六字节: 车体自动速度低位
	                         第七字节：滑台自动速度高位
	                         第八字节：滑台自动速度低位
                             */
                            _btArrSend[_iNo++] = 0x4D; _btArrSend[_iNo++] = 0x53;
                            #endregion 备注
                            #region  命令
                            if (iRw == 0)
                                _btArrSend[3] = 0x0;// 00：向打磨机写入参数 
                            else
                                _btArrSend[3] = 0x01;//01：向打磨机读取参数
                            _sPara = strVal.Split(',');//车体速度参数,滑台参数
                            if (_sPara.Length == 2)
                            {
                                string _str16 = Convert.ToString(int.Parse(_sPara[0]), 16);
                                _str16 = strAddLen_Num(_str16);
                                _btArrSend[4] = Convert.ToByte(_str16.Substring(0, 2), 16); //第五字节:车体速度参数高位
                                _btArrSend[5] = Convert.ToByte(_str16.Substring(2, 2), 16); ;//第六字节；车体速度参数低位

                                _str16 = Convert.ToString(int.Parse(_sPara[1]), 16);
                                _str16 = strAddLen_Num(_str16);
                                _btArrSend[6] = Convert.ToByte(_str16.Substring(0, 2), 16);//第七字节：滑台参数高位
                                _btArrSend[7] = Convert.ToByte(_str16.Substring(2, 2), 16);//第八字节：滑台参数低位
                            }
                            else
                                _blVal = false;

                            #endregion     命令  
                            break;
                    }
                    #endregion
                    break;
                case 5://五、TOFD磁爬车增加指令
                    #region 
                    switch (iComType_1)
                    {
                        case 1://1 探架升降UD   （up down）
                            /*
                         指令示例：	0x55	0x44	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
                        前两字节是指令的针头，用于识别指令的类型；	 
	                          第三字节空
	                          第四字节：00：向车体写入参数
	                                    01：向车体读取参数
	  
                              第八字节：01：DOWN
	                                    02：UP
                             */
                            _btArrSend[_iNo++] = 0x55; _btArrSend[_iNo++] = 0x44;
                            if (iRw == 0)
                                _btArrSend[3] = 0x0;// 00：向车体写入参数
                            else
                                _btArrSend[3] = 0x01;//01：向车体读取参数
                            if (int.Parse(strVal) == 1)
                                _btArrSend[7] = 0x1;// DOWN  落下
                            else
                                _btArrSend[7] = 0x02;//UP   抬起
                            break;
                        case 2://2、缺陷打标MK   (mark)
                            /*
                             指令示例：	0x4D	0x4B	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
                            前两字节是指令的针头，用于识别指令的类型；	 
	                              第三字节空
	                              第四字节：00：向车体写入参数
	                                        01：向车体读取参数
	  
                                  第八字节: 01：打标记
	                                       02：停止打标记
                             */
                            _btArrSend[_iNo++] = 0x4D; _btArrSend[_iNo++] = 0x4B;
                            if (iRw == 0)
                                _btArrSend[3] = 0x0;// 00：向车体写入参数
                            else
                                _btArrSend[3] = 0x01;//01：向车体读取参数
                            if (int.Parse(strVal) == 1)
                                _btArrSend[7] = 0x1;// 打标记
                            else
                                _btArrSend[7] = 0x02;//停止打标记
                            break;
                        case 3:// 3、寻迹命令：数据长度、中心点位置
                            _btArrSend[_iNo++] = 0x4D; _btArrSend[_iNo++] = 0x4B;
                            if (iRw == 0)
                                _btArrSend[3] = 0x0;// 00：向车体写入参数
                            else
                                _btArrSend[3] = 0x01;//01：向车体读取参数

                            _sPara = strVal.Split(',');//数据宽度50,100（实际值对应500，1000），中心的位置，斜率
                            if (_sPara.Length == 3)//这里只使用数据宽度、中心点位置
                            {
                                strVal = Convert.ToString(int.Parse(_sPara[0]), 16);//数据宽度50,100（实际值对应500，1000）
                                strVal = strAddLen_Num(strVal, 2);
                                _btArrSend[4] = Convert.ToByte(strVal.Substring(0, 2), 16); //第4字节存放数据宽度 50/100
                                
                                strVal = Convert.ToString(int.Parse(_sPara[1]), 16);//第5、6字节存放中心的位置,数据范围500，1000
                                strVal = strAddLen_Num(strVal, 4);
                                _btArrSend[5] = Convert.ToByte(strVal.Substring(0, 2), 16);
                                _btArrSend[6] = Convert.ToByte(strVal.Substring(2, 2), 16);
                            }
                            break;
                    }
                    #endregion
                    break;

            }
            #endregion

            #region 发送
            string _strSend = SerialClass.byteToHexStr(_btArrSend);
            WriteErrorLog(strLogFileName,  "发送 报文：" + _strSend, true );

            if (m_blCom1_Can0 == 1)
            {
                if (_blVal)
                {
                    lock (m_LockCom)
                    {
                        _blRet = RS232.WriteComm(_btArrSend);
                        m_SysBuf.dtSendStar = DateTime.Now;
                    }
                    if (_blRet == false)
                    {
                        //CloseCom();
                        //InitCom(m_strComPara);
                        lock (m_LockCom)
                            _blRet = RS232.WriteComm(_btArrSend);
                    }
                    _strSend += "/" + (_blRet ? "1" : "0");
                }
            }
            else
                _strSend += "/" + (Can_Send(_strSend) ? "1" : "0");
            WaitTime(10);
            #endregion

            return _blVal ? _strSend : "";
        }
        public void WaitTime(double dbWait)
        {
            try
            {
                dbWait = dbWait/1000 ;
                DateTime dtStar = DateTime.Now;
                while (true)
                {
                    Application.DoEvents();
                    if (DateTime.Now.Subtract(dtStar).TotalSeconds > dbWait) break;
                    System.Threading.Thread.Sleep(1);
                }
            }
            catch { }
        }
        /// <summary>
        /// 增加到指定长度
        /// </summary>
        /// <param name="strDat"></param>
        /// <param name="iNum"></param>
        /// <returns></returns>
        private string strAddLen_Num(string strDat, int iNum = 4)
        {
            string _strRet = strDat;
            int _iLen = strDat.Length;

            for (int i = 0; i < iNum - _iLen; i++)
                _strRet = "0" + _strRet;
            return _strRet;
        }
        /// <summary>
        /// 关闭设备
        /// </summary>
        /// <returns></returns>
        public bool CloseSet()
        {
            bool blRet = false;
            if (m_blCom1_Can0 == 1)
                blRet = CloseCom();
            else
                Can_Close();
            return blRet;
        }
        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns></returns>
        public bool CloseCom()
        {
            RS232.DataReceived -= ParseDat;

            return RS232.closePort();
        }
        /// <summary>
        /// 文件操作
        /// </summary>
        /// <param name="strPathFileName">包含路径的文件名称</param>
        /// <param name="strMsg">写入信息</param>
        /// <param name="blSendOrRec">true：发送 false:返回</param>
        public void WriteErrorLog(string strPathFileName, string strMsg, bool blSendOrRec, string _strCmdName = "", string strSendData = "")
        {
            m_blWrLog = m_csInter.IniReadDefine("CmmClimb", "LOG", "0", Application.StartupPath + "\\database\\SysConfig.ini") == "1";
            if (m_blWrLog == false) return;
            try
            {
                lock (LockFile)
                {
                    strMsg = strMsg.Trim();
                    if (strMsg == "") return;

                    strMsg += "\r\n";
                    System.IO.StreamWriter swTxt = null;

                    //1 判断路径是否存在
                    string strPath = strPathFileName;
                    if (strPathFileName.IndexOf("datalog") > 0)
                    {
                        strPath = System.Windows.Forms.Application.StartupPath + "\\datalog\\";
                    }
                    else if (strPathFileName.IndexOf("Log") > 0)
                    {
                        strPath = System.Windows.Forms.Application.StartupPath + "\\log\\";
                    }

                    if (System.IO.Directory.Exists(strPath) == false)
                    {
                        System.IO.Directory.CreateDirectory(strPath);
                    }
                    //2 创建文件
                    try
                    {
                        swTxt = System.IO.File.AppendText(strPathFileName);
                    }
                    catch (Exception e)
                    {
                    }
                    //3 写入数据
                    string strValue = "";
                    strValue = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + (blSendOrRec ? "->" : "<-");

                    strValue += strMsg;
                    try
                    {
                        swTxt.WriteLine(strValue);
                        swTxt.Flush();
                        swTxt.Close();
                    }
                    catch (Exception e)
                    {
                    }
                    swTxt = null;
                }
            }
            catch { }
        }

        #endregion
    }
    /// <summary>
    /// 串口RS23通讯类
    /// </summary>
    public class SerialClass
    {
        private SerialPort _serialPort = null;
        /// <summary>
        /// 定义通讯委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="bits"></param>
        public delegate void SerialPortDataReceiveEventArgs(object sender, SerialDataReceivedEventArgs e, byte[] bits);
        /// <summary>
        /// 定义接收数据事件
        /// </summary>
        public event SerialPortDataReceiveEventArgs DataReceived;
        //定义接收错误事件
        //public event SerialErrorReceivedEventHandler Error;
        /// <summary>
        /// 接收事件是否有效 false表示有效
        /// </summary>
        public bool ReceiveEventFlag = false;
        /// <summary>
        /// 是否打开
        /// </summary>
        public bool m_blOpen = false;
        #region 获取串口名

        private string protName;
        /// <summary>
        /// 获取串口名
        /// </summary>
        public string PortName
        {
            get { return _serialPort.PortName; }
            set
            {
                _serialPort.PortName = value;
                protName = value;
            }

        }
        #endregion

        #region 获取比特率
        private int baudRate;
        /// <summary>
        /// 获取比特率
        /// </summary>
        public int BaudRate
        {
            get { return _serialPort.BaudRate; }
            set
            {
                _serialPort.BaudRate = value;
                baudRate = value;
            }
        }
        #endregion

        #region 默认构造函数
        /// <summary>
        /// 默认构造函数，操作COM1，速度为9600，没有奇偶校验，8位字节，停止位为1 "COM1", 9600, Parity.None, 8, StopBits.One
        /// </summary>
        public SerialClass()
        {
            _serialPort = new SerialPort();
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="comPortName">串口名COM1</param>
        /// <param name="iVerify">奇偶校验 0：无 1：奇 2：偶</param>
        public SerialClass(string comPortName, int iVerify)
        {
            _serialPort = new SerialPort(comPortName);
            _serialPort.BaudRate = 9600;

            switch (iVerify)
            {
                case 0://无校验
                    _serialPort.Parity = Parity.None;
                    break;
                case 1://奇校验
                    _serialPort.Parity = Parity.Odd;
                    break;
                case 2://偶校验
                    _serialPort.Parity = Parity.Even;
                    break;
                default:
                    _serialPort.Parity = Parity.Even;
                    break;
            }
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;
            _serialPort.RtsEnable = true;
            _serialPort.ReadTimeout = 2000;
            setSerialPort();
        }
        #endregion

        #region 构造函数,可以自定义串口的初始化参数
        /// <summary>
        /// 构造函数,可以自定义串口的初始化参数
        /// </summary>
        /// <param name="comPortName">需要操作的COM口名称</param>
        /// <param name="baudRate">COM的速度</param>
        /// <param name="parity">奇偶校验位</param>
        /// <param name="dataBits">数据长度</param>
        /// <param name="stopBits">停止位</param>
        public SerialClass(string comPortName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            _serialPort = new SerialPort(comPortName, baudRate, parity, dataBits, stopBits);
            _serialPort.RtsEnable = true;  //自动请求
            _serialPort.ReadTimeout = 3000;//超时
            setSerialPort();
        }
        #endregion

        #region 析构函数
        /// <summary>
        /// 析构函数，关闭串口
        /// </summary>
        ~SerialClass()
        {
            if (_serialPort.IsOpen)
                _serialPort.Close();
        }
        #endregion

        #region 设置串口参数
        /// <summary>
        /// 设置串口参数
        /// </summary>
        /// <param name="comPortName">需要操作的COM口名称</param>
        /// <param name="baudRate">COM的速度</param>
        /// <param name="dataBits">数据长度</param>
        /// <param name="stopBits">停止位</param>
        public bool setSerialPort(string comPortName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            if (_serialPort.IsOpen)
                _serialPort.Close();
            _serialPort.PortName = comPortName;
            _serialPort.BaudRate = baudRate;
            _serialPort.Parity = parity;//Parity.None;
            _serialPort.DataBits = dataBits;
            _serialPort.StopBits = (StopBits)stopBits;
            //_serialPort.Handshake = Handshake.None;
            //_serialPort.RtsEnable = true;
            //_serialPort.DtrEnable = true;
            //_serialPort.ReadTimeout = 3000;
            //_serialPort.NewLine = "/r/n";
            setSerialPort();
            return openPort();
        }
        #endregion

        #region 设置接收函数
        /// <summary>
        /// 设置串口资源,还需重载多个设置串口的函数
        /// </summary>
        public void setSerialPort()
        {
            if (_serialPort != null)
            {
                //设置触发DataReceived事件的字节数为1
                _serialPort.ReceivedBytesThreshold = 1;
                //接收到一个字节时，也会触发DataReceived事件
                _serialPort.DataReceived -= new SerialDataReceivedEventHandler(_serialPort_DataReceived);
                //接收数据出错,触发事件
                _serialPort.ErrorReceived -= new SerialErrorReceivedEventHandler(_serialPort_ErrorReceived);

                _serialPort.DataReceived += new SerialDataReceivedEventHandler(_serialPort_DataReceived);
                //接收数据出错,触发事件
                _serialPort.ErrorReceived += new SerialErrorReceivedEventHandler(_serialPort_ErrorReceived);
                //打开串口
                //openPort();
            }

        }
        #endregion

        #region 打开串口资源
        /// <summary>
        /// 打开串口资源
        /// <returns>返回bool类型</returns>
        /// </summary>
        public bool openPort()
        {
            //  bool ok = false;
            m_blOpen = false;
            //如果串口是打开的，先关闭
            if (_serialPort.IsOpen)
                _serialPort.Close();
            try
            {
                //打开串口
                _serialPort.Open();
                m_blOpen = _serialPort.IsOpen;
            }
            catch (Exception Ex)
            {
                //   System.Windows.Forms.MessageBox.Show(Ex.Message);
            }
            return m_blOpen;

        }
        #endregion

        #region 关闭串口
        /// <summary>
        /// 关闭串口资源,操作完成后,一定要关闭串口
        /// </summary>
        public bool closePort()
        {
            bool _blRet = false;
            try
            {
                //如果串口处于打开状态,则关闭
                //  if (_serialPort.IsOpen)
                {
                    _serialPort.Close();
                    _blRet = true;
                }
            }
            catch { }
            return _blRet;
        }
        #endregion

        #region 接收串口数据事件
        /// <summary>
        /// 接收串口数据事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //禁止接收事件时直接退出
            if (ReceiveEventFlag)
            {
                return;
            }
            try
            {
                System.Threading.Thread.Sleep(20);
                byte[] _data = new byte[_serialPort.BytesToRead];
                _serialPort.Read(_data, 0, _data.Length);
                if (_data.Length == 0) { return; }
                if (DataReceived != null)
                {
                    DataReceived(sender, e, _data);
                }
                //_serialPort.DiscardInBuffer();  //清空接收缓冲区 
            }
            catch (Exception ex)
            {
                //  throw ex;
            }
        }
        #endregion

        #region 接收数据出错事件
        /// <summary>
        /// 接收数据出错事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _serialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {

        }
        #endregion

        #region 发送数据string类型
        public void SendData(string data)
        {
            //发送数据
            //禁止接收事件时直接退出
            if (ReceiveEventFlag)
            {
                return;
            }
            if (_serialPort.IsOpen)
            {
                _serialPort.Write(data);
            }
        }
        #endregion

        /// <summary>
        /// 将十六进制字符串转写入端口 
        /// </summary>
        /// <param name="SendCMD"></param>
        /// <returns></returns>
        public bool WriteComm(string SendCMD)
        {
            bool blRet = false;
            if (_serialPort == null)
                return false;
            byte[] bytesend = strToToHexByte(SendCMD);
            blRet = SendData(bytesend, 0, bytesend.Length);//往端口写入数据
            return blRet;
        }
        /// <summary>
        /// 发送数组
        /// </summary>
        /// <param name="bytesend"></param>
        /// <returns></returns>
        public bool WriteComm(byte[] bytesend)
        {
            bool blRet = false;
            if (_serialPort == null)
                return false;
            if (bytesend == null) return false;
            if (bytesend.Length <= 1) return false;

            blRet = SendData(bytesend, 0, bytesend.Length);//往端口写入数据
            return blRet;
        }
        #region 发送数据byte类型
        /// <summary>
        /// 数据发送
        /// </summary>
        /// <param name="data">要发送的数据字节</param>
        public bool SendData(byte[] data, int offset, int count)
        {
            bool _blRet = false;
            //禁止接收事件时直接退出
            if (ReceiveEventFlag)
            {
                return _blRet;
            }
            try
            {
                if (_serialPort.IsOpen)
                {
                    //_serialPort.DiscardInBuffer();//清空接收缓冲区
                    _serialPort.Write(data, offset, count);
                    _blRet = true;
                }
                else
                    _blRet = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _blRet;
        }
        #endregion

        #region 发送命令
        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="SendData">发送数据</param>
        /// <param name="ReceiveData">接收数据</param>
        /// <param name="Overtime">超时时间</param>
        /// <returns></returns>
        public int SendCommand(byte[] SendData, ref byte[] ReceiveData, int Overtime)
        {
            if (_serialPort.IsOpen)
            {
                try
                {
                    ReceiveEventFlag = true;        //关闭接收事件
                    _serialPort.DiscardInBuffer();  //清空接收缓冲区 
                    _serialPort.Write(SendData, 0, SendData.Length);
                    int num = 0, ret = 0;
                    System.Threading.Thread.Sleep(10);
                    ReceiveEventFlag = false;      //打开事件
                    while (num++ < Overtime)
                    {
                        if (_serialPort.BytesToRead >= ReceiveData.Length)
                            break;
                        System.Threading.Thread.Sleep(10);
                    }
                    if (_serialPort.BytesToRead >= ReceiveData.Length)
                    {
                        ret = _serialPort.Read(ReceiveData, 0, ReceiveData.Length);
                    }
                    else
                    {
                        ret = _serialPort.Read(ReceiveData, 0, _serialPort.BytesToRead);
                    }
                    ReceiveEventFlag = false;      //打开事件
                    return ret;
                }
                catch (Exception ex)
                {
                    ReceiveEventFlag = false;
                    throw ex;
                }
            }
            return -1;
        }
        #endregion

        #region 获取串口
        /// <summary>
        /// 获取所有已连接短信猫设备的串口
        /// </summary>
        /// <returns></returns>
        public string[] serialsIsConnected()
        {
            List<string> lists = new List<string>();
            string[] seriallist = getSerials();
            foreach (string s in seriallist)
            {
            }
            return lists.ToArray();
        }
        #endregion

        #region 获取当前全部串口资源
        /// <summary>
        /// 获得当前电脑上的所有串口资源
        /// </summary>
        /// <returns></returns>
        public string[] getSerials()
        {
            return SerialPort.GetPortNames();
        }
        #endregion


        #region 字节型转换16
        /// <summary>
        /// 把字节型转换成十六进制字符串
        /// </summary>
        /// <param name="InBytes"></param>
        /// <returns></returns>
        public static string ByteToString(byte[] InBytes)
        {
            string StringOut = "";
            foreach (byte InByte in InBytes)
            {
                StringOut = StringOut + String.Format("{0:X2} ", InByte);
            }
            return StringOut;
        }
        #endregion

        #region 十六进制字符串转字节型
        /// <summary>
        /// 把十六进制字符串转换成字节型(方法1)
        /// </summary>
        /// <param name="InString"></param>
        /// <returns></returns>
        public static byte[] StringToByte(string InString)
        {
            string[] ByteStrings;
            ByteStrings = InString.Split(" ".ToCharArray());
            byte[] ByteOut;
            ByteOut = new byte[ByteStrings.Length];
            for (int i = 0; i <= ByteStrings.Length - 1; i++)
            {
                //ByteOut[i] = System.Text.Encoding.ASCII.GetBytes(ByteStrings[i]);
                ByteOut[i] = Byte.Parse(ByteStrings[i], System.Globalization.NumberStyles.HexNumber);
                //ByteOut[i] =Convert.ToByte("0x" + ByteStrings[i]);
            }
            return ByteOut;
        }
        #endregion

        #region 十六进制字符串转字节型
        /// <summary>
        /// 字符串转16进制字节数组(方法2)
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");//FE1001050103A000D
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            try
            {
                for (int i = 0; i < returnBytes.Length; i++)
                    returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            catch { }
            return returnBytes;
        }
        #endregion

        #region 字节型转十六进制字符串
        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
        #endregion
    }
    public class ClassInterFace
    {
        #region  1 INI文件操作方法
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        private static object LockFile = new object();

        /// <summary>
        /// 日志文件
        /// </summary>
        public string strErrFileName = Application.StartupPath + "\\datalog\\Err.ini";
        /// <summary>
        /// void EraseSection        删除指定[Section]的全部内容
        /// string Section           [Section]
        /// string strFileName       配置文件名称
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="strFileName"></param>
        public void EraseSection(string Section, string strFileName)
        {
            WritePrivateProfileString(Section, null, null, strFileName);
        }
        /// <summary>
        /// void EraseSectionOneItem    删除指定[Section]的Key值内容
        /// string Section           [Section]
        /// string strKey            strKey=?
        /// string strFileName       配置文件名称
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="strKey"></param>
        /// <param name="strFileName"></param>
        public void EraseSectionOneItem(string Section, string strKey, string strFileName)
        {
            WritePrivateProfileString(Section, strKey, null, strFileName);
        }
        /// <summary>
        /// INIReadValue           从INI文件里读数据
        /// string Section     配置文件[]里的值
        /// string strKey         []下标记名
        /// string strValue       标记名的默认值
        /// string strFileName    配置文件名称
        /// </summary>
        public string INIReadValue(string Section, string strKey, string strValue, string strFileName)
        {   //从ini配置文件读取-----------
            StringBuilder sbTemp = new StringBuilder(1024);
            int i = GetPrivateProfileString(Section, strKey, strValue, sbTemp, 1024, strFileName);
            string strTmp = "";
            strTmp = sbTemp.ToString().Trim();
            if (strTmp.Length == 0) strTmp = strValue;//给出默认值
            return strTmp;
        }
        /// <summary>
        /// INIWriteValue         从INI文件里写数据
        /// string Section     配置文件[]里的值
        /// string strKey         []下标记名
        /// string strValue       标记名的值
        /// string strFileName    配置文件名称
        /// </summary>
        public void INIWriteValue(string Section, string strKey, string strValue, string strFileName)
        {   //写入ini配置文件-----------
            string strTmp = "";
            if (strValue != null)
            {
                strTmp = strValue.Trim();
                strValue.Replace("/n", "");		//替代回车换行
                long n = WritePrivateProfileString(Section, strKey, strTmp, strFileName);
            }
        }
        /// <summary>
        /// 给配置初始化默认参数
        /// string strValue就是默认参数
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="strKey"></param>
        /// <param name="strValue"></param>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public string IniReadDefine(string Section, string strKey, string strValue, string strFileName)
        {
            string strTmp = INIReadValue(Section, strKey, "", strFileName);
            if (strTmp.Length == 0)
            {
                strTmp = strValue;
                INIWriteValue(Section, strKey, strTmp, strFileName);
            }
            return strTmp;
        }

        /// <summary>
        /// 文件操作
        /// </summary>
        /// <param name="strPathFileName">包含路径的文件名称</param>
        /// <param name="strMsg">写入信息</param>
        /// <param name="blSendOrRec">true：发送 false:返回</param>
        public void WriteErrorLog(string strPathFileName, string strMsg, bool blSendOrRec)
        {
            lock (LockFile)
            {
                strMsg = strMsg.Trim();
                if (strMsg == "") return;

                strMsg += "\r\n";
                System.IO.StreamWriter swTxt = null;

                //1 判断路径是否存在
                string strPath = strPathFileName;
                if (strPathFileName.IndexOf("datalog") > 0)
                {
                    strPath = System.Windows.Forms.Application.StartupPath + "\\datalog\\";
                }
                else if (strPathFileName.IndexOf("Log") > 0)
                {
                    strPath = System.Windows.Forms.Application.StartupPath + "\\log\\";
                }

                if (System.IO.Directory.Exists(strPath) == false)
                {
                    System.IO.Directory.CreateDirectory(strPath);
                }
                //2 创建文件
                try
                {
                    swTxt = System.IO.File.AppendText(strPathFileName);
                }
                catch (Exception e)
                {

                }
                //3 写入数据
                string strValue = "";
                strValue = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + (blSendOrRec ? "->" : "<-") + strMsg;
                //strValue = srTxt.ReadLine();
                try
                {
                    swTxt.WriteLine(strValue);
                    swTxt.Flush();
                    swTxt.Close();
                }
                catch (Exception e)
                {
                }
                swTxt = null;
            }
        }

        #endregion 文件操作

        /// <summary>
        /// 等待指定时间（秒）
        /// </summary>
        /// <param name="dbWait"></param>
        public void WaitTime(double dbWait, ref bool m_blApp)
        {
            DateTime dtStar = DateTime.Now;
            while (true)
            {
                try
                {
                    if (m_blApp) break;
                    Application.DoEvents();
                    if (DateTime.Now.Subtract(dtStar).TotalSeconds > dbWait) break;
                    Thread.Sleep(10);
                }
                catch { break; }
            }
        }
        /// <summary>
        /// 等待制定时间
        /// </summary>
        /// <param name="dbWait">秒</param>
        public void WaitTime(double dbWait)
        {
            DateTime dtStar = DateTime.Now;
            while (true)
            {
                try
                {

                    Application.DoEvents();
                    if (DateTime.Now.Subtract(dtStar).TotalSeconds > dbWait) break;
                    Thread.Sleep(5);
                }
                catch { break; }
            }
        }

        public bool IsNum(string strDat)
        {
            bool _blRet = false;

            try
            {
                float _flDat = float.Parse(strDat);
                _blRet = true;

            }
            catch { }

            return _blRet;
        }
        /*
         1，C#追加文件
　　　　StreamWriter sw = File.AppendText(Server.MapPath(".")+"\\myText.txt");
　　　　sw.WriteLine("追逐理想");
　　　　sw.WriteLine("kzlll");
　　　　sw.WriteLine(".NET笔记");
　　　　sw.Flush();
　　　　sw.Close();,
2，C#拷贝文件
　　　　string OrignFile,NewFile;
　　　　OrignFile = Server.MapPath(".")+"\\myText.txt";
　　　　NewFile = Server.MapPath(".")+"\\myTextCopy.txt";
　　　　File.Copy(OrignFile,NewFile,true);
3，C#删除文件
　　　　string delFile = Server.MapPath(".")+"\\myTextCopy.txt";
　　　　File.Delete(delFile);
4，C#移动文件
　　　　string OrignFile,NewFile;
　　　　OrignFile = Server.MapPath(".")+"\\myText.txt";
　　　　NewFile = Server.MapPath(".")+"\\myTextCopy.txt";
　　　　File.Move(OrignFile,NewFile);
5，C#创建目录
// 创建目录c:\sixAge
　　　　DirectoryInfo d=Directory.CreateDirectory("c:\\sixAge");
// d1指向c:\sixAge\sixAge1
　　　　DirectoryInfo d1=d.CreateSubdirectory("sixAge1");
// d2指向c:\sixAge\sixAge1\sixAge1_1
　　　　DirectoryInfo d2=d1.CreateSubdirectory("sixAge1_1");
// 将当前目录设为c:\sixAge
　　　　Directory.SetCurrentDirectory("c:\\sixAge");
// 创建目录c:\sixAge\sixAge2
　　　　Directory.CreateDirectory("sixAge2");
// 创建目录c:\sixAge\sixAge2\sixAge2_1
　　　　Directory.CreateDirectory("sixAge2\\sixAge2_1");
         */
        /// <summary>
        /// 文件拷贝
        /// </summary>
        /// <param name="OldPathFile"></param>
        /// <param name="NewPathFile"></param>
        /// <returns></returns>
        public bool FileCopy(string OldPathFile, string NewPathFile)
        {
            bool blRet = false;
            try
            {
                System.IO.File.Copy(OldPathFile, NewPathFile, true);
                blRet = System.IO.File.Exists(NewPathFile);
            }
            catch (Exception e)
            { }
            return blRet;
        }
        /// <summary>
        /// 修改文件名字
        /// </summary>
        /// <param name="sourceFileName">带路径的老文件名</param>
        /// <param name="destFileName">带路径的新文件名</param>
        public bool Change_FileName(string sourceFileName, string destFileName)
        {
            bool _blRet = false;
            //string[] strDirs_S = System.IO.Directory.GetDirectories(sourceFileName);
            //if (System.IO.Directory.Exists(sourceFileName))
            //{
            //    string[] strDirs_d = System.IO.Directory.GetDirectories(destFileName);
            //    if (strDirs_S[0] == strDirs_d[0])
            //    {
            try
            {
                System.IO.File.Move(sourceFileName, destFileName); _blRet = true;
            }
            catch (Exception e)
            { }
            //    }
            //}
            return _blRet;
        }
        public string[] GetLatestFiles(string Path, int count)
        {
            string[] strArr = new string[1];

            string strPath = Application.StartupPath;// AppDomain.CurrentDomain.BaseDirectory;
            Path = strPath + "\\" + Path;
            if (System.IO.Directory.Exists(Path))
            {
                var query = (from f in System.IO.Directory.GetFiles(Path, "*.mdb")
                             let fi = new System.IO.FileInfo(f)
                             orderby fi.CreationTime descending
                             select fi.FullName).Take(count);
                strArr = query.ToArray();
                // return query.ToArray();
            }
            if (strArr != null)
            {
                if (strArr.Count() > 0)
                {
                    string[] sPara = "".Split(',');

                    for (int i = 0; i < strArr.Count(); i++)
                    {
                        if (strArr[i] != "")
                            sPara = strArr[i].Split('\\');
                        if (sPara.Count() > 0)
                            strArr[i] = sPara[sPara.Count() - 1];
                    }
                }
            }
            return strArr;
        }

        /// <summary>
        /// 清空文件夹下
        /// </summary>
        /// <param name="strDir">目录地址:文件夹名字</param>
        public void DeleteFiles(string strDir)
        {
            try
            {
                string strPath = Application.StartupPath;// AppDomain.CurrentDomain.BaseDirectory;
                strDir = strPath + "\\" + strDir;
                if (System.IO.Directory.Exists(strDir))
                {
                    string[] strDirs = System.IO.Directory.GetDirectories(strDir);
                    string[] strFiles = System.IO.Directory.GetFiles(strDir);
                    foreach (string strFile in strFiles)
                    {
                        System.IO.File.Delete(strFile);
                    }

                    //foreach (string strdir in strDirs)
                    //{
                    //    Directory.Delete(strdir, true);
                    //}
                    Console.WriteLine("删除成功！");
                }
                else
                {
                    Console.WriteLine("此目录不存在！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除文件夹保存：" + ex.Message);
            }
        }

        /// <summary>
        /// 删除当前路径下文件夹下某文件(例如：\\datalog\\GWJMRunMsg.ini)或者当前路径下文件  陈大伟 
        /// </summary>
        /// <param name="strPathFile">格式：\\datalog\\GWJMRunMsg.ini</param>
        public bool DeleFile(string strPathFile, int iType = 0)
        {
            bool _blRet = true;
            try
            {
                if (strPathFile == "") return _blRet;
                string strPath = "";
                int iS = strPathFile.IndexOf('\\');
                if (iS == 0)
                    strPath = Application.StartupPath;// AppDomain.CurrentDomain.BaseDirectory;
                else
                    strPath = AppDomain.CurrentDomain.BaseDirectory;

                string _strPathFile = strPath + strPathFile;
                if (iType == 1 && strPathFile != "")
                    _strPathFile = strPathFile;
                if (System.IO.File.Exists(_strPathFile))
                    System.IO.File.Delete(_strPathFile);
            }
            catch (Exception Err)
            {
                _blRet = false;
                //       MessageBox.Show("删除文件：" + strPathFile + "出错！" + Err.Message);
            }
            return _blRet;
        }
    }
}
