/*文档说明
 * Copyright(C) 2 2017 郑州金润高科电子有限公司
 * 文件名: ClassHt200.cs
 * 文件功能描述: 2轮磁力爬行器通讯
 * 目的：操作设备通讯
 * 创建标识: 陈大伟 2017-11
 * 修改标识: 
 * 修改描述:
 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.IO.Ports;


namespace Cmm_PcPower
{
    public class ClasPcPower
    {
        /// <summary>
        /// 获得系统资源
        /// </summary>
        /// <param name="SysBuff"></param>
        public ClasPcPower()//, ref InterFace Msg_Plant)
        {
         
        }
        #region 变量
        /// <summary>
        /// 电池电量
        /// </summary>
        public string m_strPowerPC = "";
   
        /// <summary>
        /// 通讯口
        /// </summary>
        private SerialClass RS232 =new SerialClass  ();
        ///// <summary>
        ///// 通讯配置文件
        ///// </summary>
        //public string strConfigFileName = Application.StartupPath + "\\database\\CommConfig.ini";
        ///// <summary>
        ///// 日志文件
        ///// </summary>
        //public string strLogFileName = Application.StartupPath + "\\datalog\\PowerLog.ini";
        ///// <summary>
        ///// 操作文件类
        ///// </summary>
        //private csInterface m_csInter = new csInterface();
        /// <summary>
        /// 写日志 false：不写 true: 写日志
        /// </summary>
        public bool m_blWrLog = false;
        /// <summary>
        /// 文件锁
        /// </summary>
        private static object LockFile = new object();
        /// <summary>
        /// 连接状态
        /// </summary>
        public bool m_blNetLink = false;
        //报文
        /// <summary>
        /// 接收数据是否成功 true:成功 false:失败
        /// </summary>
        public bool m_blGetDat = false;
        /// <summary>
        /// 54:电量百分值，16进制  返回数据 AA FF 04 10 54 55 FF
        /// </summary>

        private static  string _strRetDat = "";
        public string m_strRetDat
        {
            get { return _strRetDat; }
            set { _strRetDat = value; }
        }

        /// <summary>
        /// 发送命令祯
        /// </summary>
        public string m_strSendCmd = "AA FF 04 10 55 FF";
        /// <summary>
        /// 波特率
        /// </summary>
        public string m_strComPara = "4800,n,8,1";
        #endregion


        #region 方法
        /// <summary>
        /// 串口初始化
        /// </summary>
        /// <returns></returns>
        public bool InitCom(string strPara="COM4,4800,n,8,1")
        {
          //  MessageBox.Show(strPara);
            bool _blRet = false;
            m_blNetLink = _blRet;

            m_strComPara = strPara;
            #region 1 串口参数设置
            string[] _sPara = strPara.Split(',');//格式 COM,波特率，奇偶校验(None/Odd/Even)，数据位，停止位：COM1,9600,1,8,1
            if (_sPara.Length == 5)
            {
                Parity parity = Parity.None;//无校验
                StopBits stopBits = StopBits.One;

                switch (_sPara[2].Substring(0, 1).ToUpper())
                {
                    case "O":
                        parity = Parity.Odd; break;
                    case "E":
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
                  //  MessageBox.Show("电源端口初始化：" +(_blRet?"1":"0"));
                }
                catch (Exception e)
                {
                   MessageBox.Show("电源端口初始化异常：" + e.Message);
                    _blRet = false;
                    return _blRet;
                }
                #region 调用数据解析
                RS232.DataReceived -= ParseDat_Power;
                RS232.DataReceived += ParseDat_Power;
                #endregion 数据解析

               // m_blNetLink = _blRet;
            }
            #endregion  串口参数设置
            return _blRet;
        }
        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns></returns>
        public bool CloseCom()
        {
            RS232.DataReceived -= ParseDat_Power;
            return RS232.closePort();
        }
        /// <summary>
        /// 发送数据：读电池容量
        /// </summary>
        /// <param name="strSendCmd"></param>
        /// <returns></returns>
        public bool SendData(string strSendCmd= "AA FF 04 10 55 FF")
        {
            bool _blRet = false;
             m_strSendCmd = strSendCmd;

            if (strSendCmd != "")
                strSendCmd = strSendCmd.Replace(" ", "");
            byte[] SendB = Encoding.Default.GetBytes(strSendCmd);
            m_blGetDat = false;//没有接收数据
            _blRet = RS232.WriteComm(strSendCmd);
            if (_blRet == false)
            {
                //CloseCom();
                //InitCom(m_strComPara);
                //_blRet = RS232.WriteComm(strSendCmd);
            }
           // WriteErrorLog(strLogFileName, m_strSendCmd, true);
            return _blRet;
        }
        /// <summary>
        /// 数据解析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="bits"></param>
        public void ParseDat_Power(object sender, SerialDataReceivedEventArgs e, byte[] bits)
        {
            string strCmdRev = SerialClass.byteToHexStr(bits);

            ParseDat_2(strCmdRev);
        }
        public void ParseDat_2(string strCmdRev)
        {
         //   WriteErrorLog(strLogFileName, strCmdRev, false);

           if(CheckDat(ref  strCmdRev))
            {
                string _strData = strCmdRev.Substring(8, 2);
                _strRetDat = Convert.ToInt16(_strData, 16).ToString ();
                m_strPowerPC = _strRetDat;
            }
        }
        /// 数据效验
        /// </summary>
        /// <param name="strCmdRev"></param>
        /// <returns></returns>
        private bool CheckDat(ref string strCmdRev)
        {
            bool _blRet = false;
            int iStar_0 = 0, iStar_1 = 0;
            //  string strHeard = "";
            strCmdRev = strCmdRev.Replace(" ", "");
            if (strCmdRev.Length >= 14)//AA FF 04 10 54 55 FF
            {
                //命令1 版本1 目标ID 2 源ID 2 指令1 指令信息1 长度1 X字节 效验1 命令尾1
                iStar_0 = strCmdRev.IndexOf("AAFF");
                if (iStar_0 > -1)
                {
                    if (iStar_0 > 0)
                        strCmdRev = strCmdRev.Substring(iStar_0);
                    if (strCmdRev.IndexOf("55FF") > 0)
                    {
                        _blRet = true;
                    }
                }
            }
            return _blRet;
        }
        /// <summary>
        /// 文件操作
        /// </summary>
        /// <param name="strPathFileName">包含路径的文件名称</param>
        /// <param name="strMsg">写入信息</param>
        /// <param name="blSendOrRec">true：发送 false:返回</param>
        public void WriteErrorLog(string strPathFileName, string strMsg, bool blSendOrRec)
        {
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
            catch { }
        }

        #endregion
    }
    /// <summary>
    /// 串口RS23通讯类
    /// </summary>
    public class SerialClass
    {
        private static  SerialPort _serialPort_Power = null;
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
        #region 获取串口名

        private string protName;
        /// <summary>
        /// 获取串口名
        /// </summary>
        public string PortName
        {
            get { return _serialPort_Power.PortName; }
            set
            {
                _serialPort_Power.PortName = value;
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
            get { return _serialPort_Power.BaudRate; }
            set
            {
                _serialPort_Power.BaudRate = value;
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
            _serialPort_Power = new SerialPort();
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
            _serialPort_Power = new SerialPort(comPortName);
            _serialPort_Power.BaudRate = 9600;

            switch (iVerify)
            {
                case 0://无校验
                    _serialPort_Power.Parity = Parity.None;
                    break;
                case 1://奇校验
                    _serialPort_Power.Parity = Parity.Odd;
                    break;
                case 2://偶校验
                    _serialPort_Power.Parity = Parity.Even;
                    break;
                default:
                    _serialPort_Power.Parity = Parity.Even;
                    break;
            }
            _serialPort_Power.DataBits = 8;
            _serialPort_Power.StopBits = StopBits.One;
            _serialPort_Power.Handshake = Handshake.None;
            _serialPort_Power.RtsEnable = true;
            _serialPort_Power.ReadTimeout = 2000;
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
            _serialPort_Power = new SerialPort(comPortName, baudRate, parity, dataBits, stopBits);
            _serialPort_Power.RtsEnable = true;  //自动请求
            _serialPort_Power.ReadTimeout = 3000;//超时
            setSerialPort();
        }
        #endregion

        #region 析构函数
        /// <summary>
        /// 析构函数，关闭串口
        /// </summary>
        ~SerialClass()
        {
            if (_serialPort_Power.IsOpen)
                _serialPort_Power.Close();
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
            if (_serialPort_Power.IsOpen)
                _serialPort_Power.Close();
            _serialPort_Power.PortName = comPortName;
            _serialPort_Power.BaudRate = baudRate;
            _serialPort_Power.Parity = parity;//Parity.None;
            _serialPort_Power.DataBits = dataBits;
            _serialPort_Power.StopBits = (StopBits)stopBits;
            //_serialPort_Power.Handshake = Handshake.None;
            //_serialPort_Power.RtsEnable = true;
            //_serialPort_Power.DtrEnable = true;
            //_serialPort_Power.ReadTimeout = 3000;
            //_serialPort_Power.NewLine = "/r/n";
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
            if (_serialPort_Power != null)
            {
                //设置触发DataReceived事件的字节数为1
                _serialPort_Power.ReceivedBytesThreshold = 1;
                //接收到一个字节时，也会触发DataReceived事件
                _serialPort_Power.DataReceived -= new SerialDataReceivedEventHandler(_serialPort_Power_DataReceived);
                //接收数据出错,触发事件
                _serialPort_Power.ErrorReceived -= new SerialErrorReceivedEventHandler(_serialPort_Power_ErrorReceived);

                _serialPort_Power.DataReceived += new SerialDataReceivedEventHandler(_serialPort_Power_DataReceived);
                //接收数据出错,触发事件
                _serialPort_Power.ErrorReceived += new SerialErrorReceivedEventHandler(_serialPort_Power_ErrorReceived);
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
            bool ok = false;
            //如果串口是打开的，先关闭
            if (_serialPort_Power.IsOpen)
                _serialPort_Power.Close();
            try
            {
                //打开串口
                _serialPort_Power.Open();
                ok = _serialPort_Power.IsOpen;
            }
            catch (Exception Ex)
            {
                System.Windows.Forms.MessageBox.Show(Ex.Message);
            }
            return ok;

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
                //  if (_serialPort_Power.IsOpen)
                {
                    _serialPort_Power.Close();
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
        void _serialPort_Power_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //禁止接收事件时直接退出
            if (ReceiveEventFlag)
            {
                return;
            }
            try
            {
                System.Threading.Thread.Sleep(20);
                byte[] _data = new byte[_serialPort_Power.BytesToRead];
                _serialPort_Power.Read(_data, 0, _data.Length);
                if (_data.Length == 0) { return; }
                if (DataReceived != null)
                {
                    DataReceived(sender, e, _data);
                }
                //_serialPort_Power.DiscardInBuffer();  //清空接收缓冲区 
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
        void _serialPort_Power_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
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
            if (_serialPort_Power.IsOpen)
            {
                _serialPort_Power.Write(data);
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
            if (_serialPort_Power == null)
                return false;
            byte[] bytesend = strToToHexByte(SendCMD);
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
                if (_serialPort_Power.IsOpen)
                {
                    //_serialPort_Power.DiscardInBuffer();//清空接收缓冲区
                    _serialPort_Power.Write(data, offset, count);
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
            if (_serialPort_Power.IsOpen)
            {
                try
                {
                    ReceiveEventFlag = true;        //关闭接收事件
                    _serialPort_Power.DiscardInBuffer();  //清空接收缓冲区 
                    _serialPort_Power.Write(SendData, 0, SendData.Length);
                    int num = 0, ret = 0;
                    System.Threading.Thread.Sleep(10);
                    ReceiveEventFlag = false;      //打开事件
                    while (num++ < Overtime)
                    {
                        if (_serialPort_Power.BytesToRead >= ReceiveData.Length)
                            break;
                        System.Threading.Thread.Sleep(10);
                    }
                    if (_serialPort_Power.BytesToRead >= ReceiveData.Length)
                    {
                        ret = _serialPort_Power.Read(ReceiveData, 0, ReceiveData.Length);
                    }
                    else
                    {
                        ret = _serialPort_Power.Read(ReceiveData, 0, _serialPort_Power.BytesToRead);
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
}
