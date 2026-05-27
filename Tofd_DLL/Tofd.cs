/*
功能：TOFD数据读取操作
 创建：2022-3-19
2022-8-13 增加北京六维远光
 作者：陈大伟

 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using   Frame_Work;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using ClassLib_TestData;
using ClassLib_DataMang.DataBaseMang.OleDal;//数据库操作类
using EM_RIT;
using ECT_DLL;

using System.Net;
using AnyCardInterface;//AnyCardInterface;//北京超声探头
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Tofd_DLL
{
    /// <summary>
    /// TOFD数据操作：武汉中科
    /// </summary>
    public class CLTofd
    {
        public CLTofd(ref Frame_Work.ClassTofd_Buff SysBuff)
        {
            m_SysBuff = SysBuff;
        }
        #region 变量
        /// <summary>
        /// 日志文件
        /// </summary>
        public string strLogFileName = Application.StartupPath + "\\datalog\\CommLog.ini";
        ClassInterFace m_csInter = new ClassInterFace();
        //----
        /// <summary>
        /// 读取通道数据
        /// </summary>
        public  Thread Thread_Get_TofdData = null;

        /// <summary>
        /// 运行完成后才保存到数据库
        /// </summary>
        bool m_blRunEndTime_SaveDataBase = true;

        Frame_Work.ClassTofd_Buff m_SysBuff;
        #endregion

      

        #region 方法
        public void Thread_ReadUI()
        {
            if (Thread_Get_TofdData != null) Thread_Get_TofdData.Abort();
            Thread_Get_TofdData = new Thread(new ThreadStart(Read_TOFD));

            Thread_Get_TofdData.Name = "Thread_Read_Tofd";
            Thread_Get_TofdData.IsBackground = true;
            Thread_Get_TofdData.Start();
        }
        /// <summary>
        /// 运行耗时
        /// </summary>
       public double m_i_Runing = 0F;
        /// <summary>
        /// 获得TOFD数据进行处理   单调
        /// </summary>
        private void Read_TOFD()
        {
            DateTime dtStar = DateTime.Now;

            while (m_SysBuff.m_Tofd_DLL.m_iRun != 10)
            {
                try
                {
                    m_i_Runing = DateTime.Now.Subtract(dtStar).TotalMilliseconds;
                    dtStar = DateTime.Now;
                    //1 拿距离
                    GetDistan();
                    if (m_SysBuff.m_Tofd_DLL.m_iRun != 3)
                    {
                        if (m_SysBuff.m_Tofd_DLL.m_i_State == 0)
                        {
                            //2 读取数据
                            if (m_SysBuff.m_Tofd_DLL.blNetLink ||
                                 m_SysBuff.m_Tofd_DLL.m_pSparam[m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 2 ||
                                m_SysBuff.m_Tofd_DLL.m_bl_Ck_Wave)
                                m_SysBuff.m_Tofd_DLL.setPeakBuffer(m_SysBuff.m_Tofd_DLL.m_fWaveFramePerHeight,
                                                                   m_SysBuff.m_Tofd_DLL.m_fWaveFramePerWidth,
                                                                   m_SysBuff.m_Tofd_DLL.m_iRun);
                        }
                    }
                    //3 如果需要延时，就延时
                    if (m_SysBuff.m_Tofd_DLL.g_iWaitTime < 0)
                        WaitTime(0);
                    else if (m_SysBuff.m_Tofd_DLL.g_iWaitTime > 0)
                        WaitTime(m_SysBuff.m_Tofd_DLL.g_iWaitTime);
                }
                catch { }
             

            }
        }
        public  void GetData()
        {
            //1 拿距离
            GetDistan();
            if (m_SysBuff.m_Tofd_DLL.m_iRun != 3)
            {
                if (m_SysBuff.m_Tofd_DLL.m_i_State == 0)
                {
                    //2 读取数据
                    if (m_SysBuff.m_Tofd_DLL.blNetLink ||
                         m_SysBuff.m_Tofd_DLL.m_pSparam[m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 2 ||
                        m_SysBuff.m_Tofd_DLL.m_bl_Ck_Wave)
                        m_SysBuff.m_Tofd_DLL.setPeakBuffer(m_SysBuff.m_Tofd_DLL.m_fWaveFramePerHeight,
                                                           m_SysBuff.m_Tofd_DLL.m_fWaveFramePerWidth,
                                                           m_SysBuff.m_Tofd_DLL.m_iRun);
                }
            }
        }
        /// <summary>
        /// 以毫秒为单位延时
        /// </summary>
        /// <param name="dbWait">延时时间：ms</param>
        public static void WaitTime(float dbWait)
        {
            DateTime dtStar = DateTime.Now;
            while (true)
            {
                System.Threading.Thread.Sleep(3);

                if (DateTime.Now.Subtract(dtStar).TotalMilliseconds > dbWait) break;
                Application.DoEvents();
            }
        }
        /// <summary>
        /// 获得车体位置：单位是：m
        /// </summary>
        /// <returns></returns>
        public void GetDistan()
        {
            try
            {
                float _flDis = 0;
                int _iPul = 0;
                int _iA_B = 0;
                int _iNo = 0;
                float _fl_Dist = m_SysBuff.m_Tofd_DLL.flMaxDistance / 1000 - m_SysBuff.m_Tofd_DLL.Scree_iDotWithmm_X - m_SysBuff.m_Tofd_DLL.Scree_iDotWithmm_X;

                if (m_SysBuff.m_Tofd_DLL.m_pSparam[m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode != 1)
                {
                    #region 模拟距离
                    if (m_SysBuff.m_Tofd_DLL.m_iRun == 1 && m_SysBuff.m_Tofd_DLL.m_iRun_State == 0)
                    {
                        if (m_SysBuff.m_Tofd_DLL.m_pSparam[m_SysBuff.m_Tofd_DLL.m_icurChan].m_iEnPos == 1)
                            _flDis = (float)m_SysBuff.m_Climb.Trip + m_SysBuff.m_Tofd_DLL.Scree_iDotWithmm_X;//m
                        else
                            _flDis = (float)m_SysBuff.m_Climb.Trip - m_SysBuff.m_Tofd_DLL.Scree_iDotWithmm_X;
                    }
                    #endregion
                }
                else
                {
                    #region 实时采集编码器
                    _iNo = m_SysBuff.m_Tofd_DLL.m_icurChan;
                    _iA_B = m_SysBuff.m_Tofd_DLL.m_pSparam[_iNo].m_iCurEn;
                    //1 得到脉冲数
                    if (m_SysBuff.m_Tofd_DLL.m_pSparam[m_SysBuff.m_Tofd_DLL.m_icurChan].m_iEnPos == 1)
                    {
                        m_SysBuff.m_Tofd_DLL.m_pSparam_Real[_iNo].m_iEnPul[0] = Tofd.getEncoderValue(0, 0) - 0x800000;
                        m_SysBuff.m_Tofd_DLL.m_pSparam_Real[_iNo].m_iEnPul[1] = Tofd.getEncoderValue(0, 1) - 0x800000;
                    }
                    else
                    {
                        m_SysBuff.m_Tofd_DLL.m_pSparam_Real[_iNo].m_iEnPul[0] = 0x800000 - Tofd.getEncoderValue(0, 0);
                        m_SysBuff.m_Tofd_DLL.m_pSparam_Real[_iNo].m_iEnPul[1] = 0x800000 - Tofd.getEncoderValue(0, 1);
                    }
                    //2 计算实际距离
                    _iPul = m_SysBuff.m_Tofd_DLL.m_pSparam_Real[_iNo].m_iEnPul[_iA_B];

                    _flDis = m_SysBuff.m_Tofd_DLL.m_pSparam[_iNo].m_fEnRatio[_iA_B] * _iPul;
                    _flDis = (int)_flDis;//ms
                    m_SysBuff.m_Tofd_DLL.m_pSparam_Real[_iNo].m_fEnReal[_iA_B] = _flDis;
                    _flDis /= 1000;//m
                    #endregion
                }

                m_csInter.WriteErrorLog(strLogFileName, "脉冲数/校准系数/门限：" + _iPul.ToString ()+" / " + m_SysBuff.m_Tofd_DLL.m_pSparam[_iNo].m_fEnRatio[_iA_B] +" / " + _fl_Dist, false);
              
                if (_flDis < _fl_Dist)
                { //单位是：m
                    _flDis = float.Parse(_flDis.ToString("f3"));
                    if (m_SysBuff.m_Climb.iBmq_Type == 1)
                    {
                        _flDis = m_SysBuff.m_Climb.Trip_Com_mm * m_SysBuff.m_Tofd_DLL.fl4Car_JzXs ; _flDis /= 1000;//m
                    }
                    m_csInter.WriteErrorLog(strLogFileName, "距离" + _flDis.ToString("f3"), false);

                    m_SysBuff.m_Tofd_DLL.m_flDistanc_X = _flDis;
                    m_SysBuff.m_Climb.Trip = _flDis;
                    m_SysBuff.m_Climb.Trip_mm = _flDis;
                }
                else
                { }
            }
            catch { }
        }
        #endregion
    }

    /// <summary>m_i_Runing
    /// 北京六维远光
    /// </summary>
    public class CLTofd_BJ
    {
        public CLTofd_BJ(ref Frame_Work.ClassTofd_Buff SysBuff,int iChScanRange = 10)
        {
            m_ChScanRange = iChScanRange;
            dataLength =(int) m_ChScanRange * 100;

            m_SysBuff = SysBuff;
            m_TotalData = new byte[dataLength];
            m_TotalData_No = new int[dataLength];
            m_TotalData_Copy = new byte[dataLength];
        }
        #region 变量
        /// <summary>
        /// 运行耗时
        /// </summary>
        public double m_i_Runing = 0F;
        #region 编码器信息
        /// <summary>
        /// 显示1：距离  0：角度
        /// </summary>
        bool m_2_bl_JL1_JD0 = true;
        /// <summary>
        /// 编码器的距离值 单位mm
        /// </summary>
        public   int m_i_Trip = 0;
        double i_Trip = 0;
        /// <summary>
        /// 原始脉冲 个数
        /// </summary>
       public   double Trip_Original = 0;
        /// <summary>
        /// 清零脉冲 个数
        /// </summary>
        double Trip_Resetting = 0;
        /// <summary>
        /// 设定距离 单位mm
        /// </summary>
        int m_i_Trip_Init = 0;
        /// <summary>
        /// 编码器行程转换系数
        /// </summary>
        public double Trip_Ratio = 1;
        #endregion 编码器信息
        /// <summary>
        /// 系统缓存
        /// </summary>
        Frame_Work.ClassTofd_Buff m_SysBuff;
        /// <summary>
        /// 是否接收数据
        /// </summary>
        public bool m_bl_GetData = false;
        /// <summary>
        /// 接收探头数据
        /// </summary>
        byte[] m_TotalData;
        /// <summary>
        /// 接收探头数据
        /// </summary>
        public byte[] m_TotalData_Copy;
        /// <summary>
        /// 保存数据序号：数据序号，1us/100个点
        /// </summary>
        int[] m_TotalData_No;
        /// <summary>
        /// 保存数据序号：数据序号，1us/100个点
        /// </summary>
        public int[] m_TotalData_No_Copy;

        /// <summary>
        /// 探头接收数据量
        /// </summary>
        public   int dataLength = 3000;

        /// <summary>
        /// 通道号
        /// </summary>
        public int m_iTdNo = 0;
        /// <summary>
        /// 增益设置函数double 型参数。范围 0.0-110。精度 0.1dB
        /// </summary>
        public double m_ChGain = 20;
        /// <summary>
        /// 设置捕获起始位置，单位 us； 0
        /// </summary>
        public double m_ChScanStart = 0;
        /// <summary>
        /// 采集长度设置函数 范围受捕获起始、PRF限制。 30  采集时间长度
        /// </summary>
        public double m_ChScanRange = 30;
        /// <summary>
        /// 修改范围参数
        /// </summary>
        public bool m_bl_ModyFw = false;
        /// <summary>
        /// 当前选择通道号
        /// </summary>
        public int m_iTdNo_New = 0;
        /// <summary>
        /// Int 型参数。0：全波检波；1：正波检波；2：负波检波；3：RF 射频检波。
        /// </summary>
        public int m_i_Cmb_Bx_Select = 3;
        /// <summary>
        /// 范围
        /// </summary>
        public float  m_Num_UD_Fw = 400;
        /// <summary>
        /// 声速值
        /// </summary>
        public float m_Num_UD_Ss = 3000;
        /// <summary>
        /// 电压值
        /// </summary>
        public int m_Num_UD_DY = 400;
        /// <summary>
        /// 脉冲宽度
        /// </summary>
        public float m_Num_UD_Mckd = 50.0f;
        /// <summary>
        /// 探头频率（MHz）
        /// </summary>
        public int m_N_F = 1;


        /// <summary>
        /// 脉冲重复频率
        /// </summary>
        public int m_PRF = 60;
        /// <summary>
        /// 触发方式： 0:内部触发 1:手动触发 2:外部 3:Encoder0 4:Encoder1 5:Encoder2
        /// </summary>
        public int m_comboBoxTrigMode = 0;
        /// <summary>
        /// 滤波MHz 全通  0.5-2  1.0-5  2-6  4-9  7-15  10-20  >=15
        /// </summary>
        public int m_Cmb_Lb = 0;
        /// <summary>
        /// 接收到数据的压缩比例
        /// </summary>
       public   float m_fl_Ysbl = 0;


        #region 探头变量
        /// <summary>
        ///  Int 型参数。0：全波检波；1：正波检波；2：负波检波；3：RF 射频检波。
        /// </summary>
        public int m_i_Bx_Type = 0;
        /// <summary>
        /// 1 次 2 次  4 次  8 次  16 次 
        /// </summary>
        public int m_Cmb_Pj = 0;
        /// <summary>
        /// 阻尼电阻值  电阻 80          电阻 400
        /// </summary>
        public int m_Cmb_Zn = 0;
        #endregion 探头变量

        /// <summary>
        /// 采集线程是否停止
        /// </summary>
        bool m_RunRefreshData = true;
        /// <summary>
        /// 线程
        /// </summary>
        Task m_Thread;
        /// <summary>
        /// 读写文件
        /// </summary>
        ClassInterFace m_csInter = new ClassInterFace();
        /// <summary>
        /// 配置文件
        /// </summary>
        public  string HardFileName = Application.StartupPath + "\\database\\SysConfig.ini";

        /// <summary>
        /// 异常
        /// </summary>
        string m_strAlarmTitl = "异常";
        /// <summary>
        /// 正常
        /// </summary>
        string m_strAlarmTitl_Zc = "正常";

        string N_F = "";

        /// <summary>
        /// 屏幕每格数据个数
        /// </summary>
        float m_LineStep = 0;
        /// <summary>
        /// 屏幕高度每格数据
        /// </summary>
        float m_UnitY = 0;
        /// <summary>
        /// 声速
        /// </summary>
        string Num_UD_Ss = "";
        /// <summary>
        /// 查询
        /// </summary>
        bool m_bl_Ck_Query = false;

        /// <summary>
        /// 接收到的报文
        /// </summary>
       public   string m_strFram = "";
        /// <summary>
        /// 临界面位置线的门限对应缓存位置
        /// </summary>
        int m_iLimit_No = 0;
        /// <summary>
        /// 保存当前数据
        /// </summary>
        bool m_bl_Save = false;
        /// <summary>
        /// 联机是否成功
        /// </summary>
      // public  bool m_bl_Link = false;
        /// <summary>
        /// A扫描图画笔
        /// </summary>
        Color m_ColorL = Color.FromArgb(255, 0, 98, 0);
        /// <summary>
        /// 横轴间隔个数
        /// </summary>
         int m_iJgNum = 10;
        #endregion 变量

        #region 方法

        /// <summary>
        /// 计算距离
        /// </summary>
        /// <param name="iScreenNo"></param>
        /// <returns></returns>
        public float Cal_Thick(int iScreenNo, bool bl_JL1_Time0 = true)
        {
            float _Real = (float)iScreenNo * m_fl_Ysbl;
            int _No_Real = (int)(_Real);
            _Real = _No_Real / 100f;//ms
            if (bl_JL1_Time0)
                _Real *= m_Num_UD_Ss * 0.001f;
            return _Real; // 一发一收：100 自发自收：200
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="blVal">false:正向 true:反向</param>
        public unsafe void Zx0Fx1(bool blVal = false)
        {//encoderIndex		编码器编号，范围 0-3，最大支持 4 路编码器
         //  value  1为反向。

            CardInterface.AnyCardX_SetEncodeReversed(0, blVal == false ? 0 : 1);
        }
        /// <summary>
        /// 初始化探头
        /// </summary>
        public unsafe void InitParamter_Multi()
        {
            Zx0Fx1();
            Clear_Trip();

            CardInterface.AnyCardX_setGroupMax(0);//设置系统最大工作通道数 0： 一个工作通道；1：两个工作 通道。。
            CardInterface.AnyCardX_SetGain(m_iTdNo, m_ChGain);//	增益设置函数double 型参数。范围 0.0-110。精度 0.1dB
            CardInterface.AnyCardX_SetTimeStart(m_iTdNo, m_ChScanStart);//设置捕获起始位置，单位 us； 0
            CardInterface.AnyCardX_SetTimeRange(m_iTdNo, m_ChScanRange);//采集长度设置函数 范围受捕获起始、PRF限制。 30
            CardInterface.AnyCardX_setCreatePoint(m_iTdNo, dataLength);//3000 设置压缩上传点数量函数
            CardInterface.AnyCardX_setSignalSrcMode(m_iTdNo, m_iTdNo_New + 1);//ChannelIndex +1;
            CardInterface.AnyCardX_setCompensateStart(m_iTdNo, 0);//设置仪器的起始波在零点位置
            CardInterface.AnyCardX_SetWaveDetectMode(m_iTdNo, m_i_Cmb_Bx_Select);//Int 型参数。0：全波检波；1：正波检波；2：负波检波；3：RF 射频检波。
            CardInterface.AnyCardX_SetTxPulseSwitch(m_iTdNo, 1);//发射脉冲开关设置int 型参数。1：开启；0：关闭。
            CardInterface.AnyCardX_SetPulseReciveMode(m_iTdNo, 1);//自发自收(一发一收)模式设置 Int 型参数。0：自发自收；1：一发一收（对穿）。
            CardInterface.AnyCardX_PRF(m_PRF);// 脉冲重复频率 默认范围 10-9999。
            CardInterface.AnyCardX_setDataSwitch(0);//更改每包数据大小时，需调用此函数
            Get_Ysbl();
        }
        /// <summary>
        /// 获得压缩比例
        /// </summary>
        private void Get_Ysbl()
        {
            m_fl_Ysbl = (float)(m_ChScanRange * 100 / dataLength);
        }
        /// <summary>
        /// 设备联机:启动/关闭数据读取
        /// </summary>
        /// <param name="iType">0:联机 1：停机</param>
        public void Connection(int iType = 0)
        {
            if (iType == 0)
            {
                Init_Para(0);
                m_RunRefreshData = true;
                m_Thread = new Task(this.UpdateRefreshData);
                m_Thread.Start();

                CardInterface.AnyCardX_Start();
                InitParamter_Multi();
                CardInterface.AnyCardX_setCompensateStart(m_iTdNo, 0);
            }
            else
            {
                m_RunRefreshData = false;
                CardInterface.AnyCardX_Stop();
                Init_Para(1);
            }
        }
        /// <summary>
        /// 参数读写
        /// </summary>
        /// <param name="iType">0:读 1：写</param>
        public  void Init_Para(int iType = 0)
        {
            if (iType == 0)
            {
                m_ChScanRange = float.Parse(m_csInter.IniReadDefine("Parameter", "m_ChScanRange", "320", HardFileName));
                m_ChGain = float.Parse(m_csInter.IniReadDefine("Parameter", "m_ChGain", "20", HardFileName));
                m_iTdNo_New = int.Parse(m_csInter.IniReadDefine("Parameter", "m_iTdNo", "0", HardFileName));
                m_i_Cmb_Bx_Select = int.Parse(m_csInter.IniReadDefine("Parameter", "Cmb_Bx", "3", HardFileName));
                m_Num_UD_Fw = float.Parse(m_csInter.IniReadDefine("Parameter", "Num_UD_Fw", "400", HardFileName));
                m_Num_UD_Ss = float.Parse(m_csInter.IniReadDefine("Parameter", "Num_UD_Ss", "5900", HardFileName));
                m_Num_UD_DY = int.Parse(m_csInter.IniReadDefine("Parameter", "Num_UD_DY", "400", HardFileName));
                m_Num_UD_Mckd = float.Parse(m_csInter.IniReadDefine("Parameter", "Num_UD_Mckd", "500", HardFileName));
                m_N_F = int.Parse(m_csInter.IniReadDefine("Parameter", "N_F", "10", HardFileName));
                m_PRF = int.Parse(m_csInter.IniReadDefine("Parameter", "m_PRF", "60", HardFileName));
                m_comboBoxTrigMode = int.Parse(m_csInter.IniReadDefine("Parameter", "comboBoxTrigMode", "0", HardFileName));
                m_Cmb_Lb = int.Parse(m_csInter.IniReadDefine("Parameter", "Cmb_Lb", "2", HardFileName));
                m_i_Bx_Type = int.Parse(m_csInter.IniReadDefine("Parameter", "Cmb_Bx", "3", HardFileName));
                m_Cmb_Pj = int.Parse(m_csInter.IniReadDefine("Parameter", "Cmb_Pj", "0", HardFileName));
                m_Cmb_Zn = int.Parse(m_csInter.IniReadDefine("Parameter", "Cmb_Zn", "1", HardFileName));
                m_ChScanStart = float .Parse(m_csInter.IniReadDefine("Parameter", "numStartPos", "0", HardFileName));
                Trip_Ratio = float.Parse(m_csInter.IniReadDefine("Parameter", "Trip_Ratio", "1", HardFileName));
            }
            else
            {
                m_csInter.INIWriteValue("Parameter", "m_ChScanRange", m_ChScanRange.ToString(), HardFileName);//21 US
                m_csInter.INIWriteValue ("Parameter", "m_ChGain", m_ChGain.ToString (), HardFileName);//增益71
                m_csInter.INIWriteValue("Parameter", "m_iTdNo", m_iTdNo_New.ToString (), HardFileName);//通道0
               m_csInter.INIWriteValue("Parameter", "Cmb_Bx", m_i_Cmb_Bx_Select.ToString (), HardFileName);//射频 3
                m_csInter.INIWriteValue("Parameter", "Num_UD_Fw", m_Num_UD_Fw.ToString (), HardFileName);//范围123.9
                m_csInter.INIWriteValue("Parameter", "Num_UD_Ss", m_Num_UD_Ss.ToString (), HardFileName);//5900
                m_csInter.INIWriteValue("Parameter", "Num_UD_DY", m_Num_UD_DY.ToString (), HardFileName);//400
               m_csInter.INIWriteValue("Parameter", "Num_UD_Mckd", m_Num_UD_Mckd.ToString (), HardFileName);//100
                m_csInter.INIWriteValue("Parameter", "N_F", m_N_F.ToString (), HardFileName);//5
                m_csInter.INIWriteValue("Parameter", "m_PRF", m_PRF.ToString (), HardFileName);//60
                m_csInter.INIWriteValue("Parameter", "comboBoxTrigMode", m_comboBoxTrigMode.ToString (), HardFileName);//65536000
                m_csInter.INIWriteValue("Parameter", "Cmb_Lb", m_Cmb_Lb.ToString (), HardFileName);//2
          //      m_csInter.INIWriteValue("Parameter", "Cmb_Bx", m_i_Bx_Type.ToString (), HardFileName);//196608
                m_csInter.INIWriteValue("Parameter", "Cmb_Pj", m_Cmb_Pj.ToString (), HardFileName);//1
                m_csInter.INIWriteValue("Parameter", "Cmb_Zn", m_Cmb_Zn.ToString (), HardFileName);//1
                m_csInter.INIWriteValue("Parameter", "numStartPos", m_ChScanStart.ToString (), HardFileName);//0
                m_csInter.INIWriteValue("Parameter", "Trip_Ratio", Trip_Ratio.ToString(), HardFileName);//-107
            }
        }
        /// <summary>
        /// 距离清零
        /// </summary>
        public void Clear_Trip()
        {
            //encoderIndex		编码器编号，范围 0-3，最大支持 4 路编码器
            //   Value  0：		正常工作，1：复位。
            CardInterface.AnyCardX_SetEncodeReset(0, 1);
            Thread.Sleep(100);
            Trip_Resetting = Trip_Original;
        }

        public static void WaitTime(float dbWait)
        {
            DateTime dtStar = DateTime.Now;
            while (true)
            {
                System.Threading.Thread.Sleep(3);

                if (DateTime.Now.Subtract(dtStar).TotalMilliseconds > dbWait) break;
                Application.DoEvents();
            }
        }
        /// <summary>
        /// 采集数据
        /// </summary>
        void UpdateRefreshData()
        {
            byte[] m_AcqData = new byte[dataLength];
            CardInterface.Data_Head_Info m_CurrentUDPHeader = new CardInterface.Data_Head_Info();
            short group_id, groupId1, partIndex, partIndex1;
            byte _Dt = 0;
            var IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(m_TotalData, 0);
            string _strFram = "";
            float _flDis = 0;
            bool _blGetData = false;//采集到数据
           DateTime dtStar = DateTime.Now;
            while (m_RunRefreshData)
            {
                try
                {
                    
                    m_i_Runing = DateTime.Now.Subtract(dtStar).TotalMilliseconds;
                    dtStar = DateTime.Now;
                    int result = CardInterface.GetHardwareData(ref m_AcqData, ref m_CurrentUDPHeader);
                    if (-1 == result)
                    {

                    }
                    else if (-2 == result)
                    {
                        //无新数据                   
                    }
                    else
                    {
                        if (m_bl_Ck_Query == false && m_SysBuff.m_Tofd_DLL.m_bl_Ck_Wave == false )
                        {
                            group_id = (short)m_CurrentUDPHeader.ChannelIndex;
                            groupId1 = IPAddress.NetworkToHostOrder(group_id);

                            partIndex = (short)m_CurrentUDPHeader.PartIndex;
                            partIndex1 = IPAddress.NetworkToHostOrder(partIndex);
                            /*
                             距离编程思路：
                            接收编码器脉冲-->转换系数-->转换成实际距离
                             */
                            #region 1 距离数据转换

                            if (m_SysBuff.m_Tofd_DLL.m_pSparam[m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode != 1)
                            {
                                #region 模拟距离
                                if (m_SysBuff.m_Tofd_DLL.m_iRun == 1 && m_SysBuff.m_Tofd_DLL.m_iRun_State == 0)
                                {
                                    if (m_SysBuff.m_Tofd_DLL.m_pSparam[m_SysBuff.m_Tofd_DLL.m_icurChan].m_iEnPos == 1)
                                        _flDis = (float)m_SysBuff.m_Climb.Trip + m_SysBuff.m_Tofd_DLL.Scree_iDotWithmm_X;//m
                                    else
                                        _flDis = (float)m_SysBuff.m_Climb.Trip - m_SysBuff.m_Tofd_DLL.Scree_iDotWithmm_X;

                                    m_i_Trip =(int)( _flDis * 1000f);
                                    #endregion
                                }
                            }
                            else
                            {
                                Trip_Original = IPAddress.NetworkToHostOrder((int)m_CurrentUDPHeader.encodeX0) ;
                               
                                i_Trip = ((Trip_Original - Trip_Resetting) / (Trip_Ratio <= 0 ? 1 : Trip_Ratio) - m_i_Trip_Init);
                                m_i_Trip =(int)i_Trip;
                                _flDis =(float )(m_i_Trip / 1000f);

                            }
                            m_SysBuff.m_Tofd_DLL.m_flDistanc_X = _flDis;
                            m_SysBuff.m_Climb.Trip = _flDis;
                            m_SysBuff.m_Climb.Trip_mm = _flDis;

                            #endregion  1
                            _blGetData = false;

                            #region 2 获得采集数据
                            if (0 == partIndex1)
                            {
                                _strFram = "";
                                for (int k = 0; k < 1000; k++)
                                {
                                    _Dt = m_AcqData[k];
                                    m_TotalData[k] = _Dt;
                                    _strFram += (k == 0 ? "" : (k == m_iLimit_No ? "|||===>" : ",")) + _Dt.ToString();
                                    m_TotalData_No[k] = k;
                                }
                                if (dataLength <= 1000) _blGetData = true;
                            }
                            else if (1 == partIndex1)
                            {
                                for (int k = 0; k < 1000; k++)
                                {
                                    _Dt = m_AcqData[k];
                                    m_TotalData[1000 + k] = m_AcqData[k];
                                    _strFram += (k == 0 ? "" : (1000 + k == m_iLimit_No ? "|||===>" : ",")) + _Dt.ToString();

                                    m_TotalData_No[1000 + k] = 1000 + k;
                                }
                                if (dataLength > 1000 && dataLength <= 2000) _blGetData = true;
                            }
                            else if (2 == partIndex1)
                            {
                                for (int k = 0; k < 1000; k++)
                                {
                                    m_TotalData[2000 + k] = m_AcqData[k];
                                    _Dt = m_AcqData[k];
                                    _strFram += (k == 0 ? "" : (2000 + k == m_iLimit_No ? "|||===>" : ",")) + _Dt.ToString();
                                    m_TotalData_No[2000 + k] = 2000 + k;
                                }

                                //if (m_bl_Save)
                                //{
                                //    SaveData();
                                //    m_bl_Save = false;
                                //}
                                if (dataLength > 2000 && dataLength <= 3000) _blGetData = true;

                            }
                            #endregion 2

                            if (_blGetData)
                            {
                                m_strFram = _strFram;
                                IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(m_TotalData_Copy, 0);
                                Marshal.Copy(m_TotalData, 0, IntPtArr, m_TotalData.Length);
                                m_SysBuff.m_Tofd_DLL.blNetLink = true;// m_bl_Link = true;

                                #region 3 将采集数据转发给系统
                                if (m_SysBuff.m_Tofd_DLL.m_iRun != 10)
                                {
                                    if (m_SysBuff.m_Tofd_DLL.m_iRun != 3)
                                    {
                                        if (m_SysBuff.m_Tofd_DLL.m_i_State == 0)
                                        {
                                            //2 读取数据
                                            if (m_SysBuff.m_Tofd_DLL.blNetLink ||
                                                 m_SysBuff.m_Tofd_DLL.m_pSparam[m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 2 ||
                                                                               m_SysBuff.m_Tofd_DLL.m_bl_Ck_Wave)
                                                m_SysBuff.m_Tofd_DLL.setPeakBuffer(m_i_Trip,
                                                                                  m_SysBuff.m_Tofd_DLL.m_flDistanc_X,
                                                                                  dataLength,
                                                                                  m_TotalData);
                                        }
                                    }
                                }
                                #endregion 3
                            }
                            if ((groupId1 < 0) || (groupId1 >= 4))
                            {
                                MessageBox.Show("GroupID error");
                            }
                            _blGetData = false;
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }

                if (m_SysBuff.m_Tofd_DLL.g_iWaitTime < 0)
                    WaitTime(0);
                else if (m_SysBuff.m_Tofd_DLL.g_iWaitTime > 0)
                    WaitTime(m_SysBuff.m_Tofd_DLL.g_iWaitTime);
                //   Application.DoEvents();
                //   SysInfo.m_csInter.WaitTime(0.02f);
            }
        }
        /// <summary>
        /// 左边预留
        /// </summary>
        public float m_fl_Left_Add = 0;
        int i_X=0, i_Y = 0;
        /// <summary>
        /// 总测量距离
        /// </summary>
        public   float m_fl_Zc = 0;
        /// <summary>
        /// 画布宽度    
        /// </summary>
        int m_flPic_W = 0;
        /// <summary>
        /// 画布高度
        /// </summary>
        int m_flPic_H = 0;
        /// <summary>
        /// 横轴10个数据一个刻度
        /// </summary>
        int m_iKd_W_Num = 0;
        /// <summary>
        /// 纵轴10个数据一个刻度
        /// </summary>
        int m_iKd_H_Num = 0;
        /// <summary>
        /// 图像横向间隔总个数
        /// </summary>
        int m_i_Dac_With_AllNm =0;
        /// <summary>
        /// 1格占用距离  mm/单位
        /// </summary>
        float m_fl_1_mm = 0;
        /// <summary>
        /// 一个间隔占用几个像素
        /// </summary>
        public  int m_i_Pixel_mm = 5;

        /// <summary>
        /// 屏幕X轴位置
        /// </summary>
       public  int m_i_Screen_Posit_X = 0;
        /// <summary>
        /// Y轴位置
        /// </summary>
        public int m_i_Screen_Posit_Y = 0;
        /// <summary>
        /// 光标位置在数据缓存中对应序号
        /// </summary>
        public int m_LineStep_No = 0;
        /// <summary>
        /// D图上对应A扫描位置
        /// </summary>
        public int m_i_X_A = 0;

        public int m_i_Y_A = 0;
        /// <summary>
        /// 画波形
        /// </summary>
        List<PointF> m_DataList = new List<PointF>();
        /// <summary>
        /// 显示鼠标对应数据
        /// </summary>
        public bool m_bl_ShowMsg = false;
        public  void Get_Init(PictureBox PicA_Scan)
        { 
            Calcu_Left_Add();
            m_fl_Zc = Cal_Thick(dataLength);// + m_fl_Left_Add;//总长度
            m_flPic_W = PicA_Scan.Width;//画布宽度  965
            m_flPic_H = PicA_Scan.Height;//画布高度   364

            m_iKd_W_Num = int.Parse((m_flPic_W / m_iJgNum).ToString("f0"));//横轴10个数据一个刻度
            m_iKd_H_Num = int.Parse((m_flPic_H / m_iJgNum).ToString("f0"));//纵轴10个数据一个刻度

            m_i_Dac_With_AllNm = m_flPic_W / m_i_Pixel_mm;// m_SysBuff.m_Tofd_DLL.m_i_1_mm;//总个数
            m_fl_1_mm = m_fl_Zc / m_i_Dac_With_AllNm;//

            m_SysBuff.m_Tofd_DLL.m_fWaveFramePerWidth =1.0f * m_flPic_W / dataLength;
           
        }
        /// <summary>
        /// 计算左偏移
        /// </summary>
        public void Calcu_Left_Add()
        {
            m_fl_Left_Add = (float)m_ChScanStart * m_Num_UD_Ss * 0.001f;
        }
        /// <summary>
        /// 画波形图 单调
        /// </summary>
        /// <param name="pDC"></param>
        /// <param name="groupID"></param>
        public unsafe void DrawGraphics(PictureBox PicA_Scan)
        {
            #region 1 画A扫描图初始化
            m_bl_GetData = true;
            if (null == m_TotalData_Copy) return;

            Pen p_xy = new Pen(m_ColorL);// Brushes.LimeGreen CadetBlue);
                                         // 初始化画板，在内存中建立一块虚拟画布
            Bitmap image = new Bitmap(m_flPic_W, m_flPic_H);

            // 获取背景层
            Bitmap bg = (Bitmap)PicA_Scan.BackgroundImage;
            // 初始化整个画布
            Bitmap canvas = new Bitmap(m_flPic_W, m_flPic_H);
            // 初始化图形面板，获取这块内存画布的Graphics的引用
            Graphics g = Graphics.FromImage(image);
            Graphics gb = Graphics.FromImage(canvas);
            g.Clear(Color.Black);//
            g.SmoothingMode = SmoothingMode.AntiAlias;

            
                                            //   Cmb_JL.Text = m_fl_Zc.ToString();
            #endregion  初始化

            using (Font myFont = new Font(FontFamily.Families[2], 20))
            {
                #region 2 画横、纵轴分别10格的间隔线
                for (int i = 1; i < m_iJgNum; i++)
                {
                    i_X = i * m_iKd_W_Num;//竖线
                    g.DrawLine(p_xy, new Point(i_X, 0), new Point(i_X, (int)m_flPic_H));
                    i_Y = i * m_iKd_H_Num;//横线
                    g.DrawLine(p_xy, new Point(0, i_Y), new Point((int)m_flPic_W, i_Y));
                }

                Pen p_Kd = new Pen(Color.White);
                int Chart_Ruler_Y_Start = 0;// m_flPic_H - 15;//10
                int i_S_H = 0;//Chart_Ruler_Y_Start - 9;//6
                int i_S_L = 0;// Chart_Ruler_Y_Start - 3;//3
                if (m_i_Cmb_Bx_Select==3)
                {
                     Chart_Ruler_Y_Start = m_flPic_H - 15;//10
                     i_S_H = Chart_Ruler_Y_Start - 9;//6
                     i_S_L = Chart_Ruler_Y_Start - 3;//3
                }
                else
                {
                     Chart_Ruler_Y_Start = m_flPic_H - 10;//10
                     i_S_H = Chart_Ruler_Y_Start - 6;//6
                     i_S_L = Chart_Ruler_Y_Start - 3;//3
                }
                int _iGs = 10;
                int Chart_Ruler_Word_H = i_S_L - 2;
                PointF drawPoint = new PointF(i_X - 10, Chart_Ruler_Word_H);
                string strT = "";
                Font drawFont = new Font("Arial", (float)10);
                SolidBrush drawBrush = new SolidBrush(Color.White);
                double  _D = dataLength*1.0f / m_flPic_W;
                for (int _i = 1; _i < m_i_Dac_With_AllNm; _i++)
                {
                    i_X = _i * m_i_Pixel_mm;
                    if (_i % 5 != 0)
                        g.DrawLine(p_Kd, new Point(i_X, i_S_L), new Point(i_X, Chart_Ruler_Y_Start));
                    else
                        g.DrawLine(p_Kd, new Point(i_X, i_S_H), new Point(i_X, Chart_Ruler_Y_Start));

                    if (_i % _iGs == 0)
                    {
               //         strT = (_i * m_fl_1_mm + (m_ChScanStart > 0 ? m_fl_Left_Add : 0f)).ToString("f1");
             //           drawPoint = new PointF(i_X - 7, Chart_Ruler_Word_H - 20);//- Chart_Ruler_Word_X

                 //       g.DrawString(strT, drawFont, drawBrush, drawPoint);
                        if (m_i_Cmb_Bx_Select == 3)
                        {
                            drawPoint = new PointF(i_X - 13, Chart_Ruler_Word_H + 6);
                            strT = (Cal_Thick((int)(i_X * _D), false) + m_ChScanStart).ToString("f2");
                            g.DrawString(strT, drawFont, drawBrush, drawPoint);
                        }
                    }
                }
                drawPoint = new PointF(2, Chart_Ruler_Word_H - 15);
        //        g.DrawString("mm", drawFont, drawBrush, drawPoint);
                if (m_i_Cmb_Bx_Select == 3)
                {
                    drawPoint = new PointF(2, Chart_Ruler_Word_H + 6);
                    g.DrawString("us", drawFont, drawBrush, drawPoint);
                }
                #endregion 2
            }

            fixed (byte* ptempData = m_TotalData_Copy)
            {
                m_DataList.Clear();
                float tempY; int curLoc;
                byte _T = 0;

                bool _blAlarm = false;//是否有报警
                m_UnitY = m_flPic_H / 256f;
                m_LineStep = (float)dataLength / (float)m_flPic_W;
                for (int i = 0; i < m_flPic_W; i++)
                {
                    curLoc = (int)(i * m_LineStep);
                    if (curLoc >= dataLength) curLoc = dataLength - 1;

                    _T = ptempData[curLoc];

                    tempY = m_flPic_H - _T * m_UnitY;
                    PointF myPointF = new PointF((float)i, tempY);
                    m_DataList.Add(myPointF);

                }
                g.DrawLines(Pens.Yellow, m_DataList.ToArray());

                Font drawFont = new Font("黑体", 16);
                SolidBrush drawBrush = new SolidBrush(Color.White);
                PointF drawPoint = new PointF(m_LineStep_No > 2600 ? m_i_Screen_Posit_X - 110 : m_i_Screen_Posit_X, m_i_Screen_Posit_Y + 15);
                float _Real = (float)m_LineStep_No * m_fl_Ysbl;
                int _No_Real = (int)(_Real);
                float _Jl = Cal_Thick(m_LineStep_No);// _No_Real / 200f *  float .Parse (Num_UD_Ss.Value .ToString ());

                if (m_bl_ShowMsg)
                    g.DrawString(m_i_Screen_Posit_X.ToString() + "界面No:" + m_LineStep_No + "\r\n缓存No:" + _No_Real + "\r\n" +
                                  "距离:" + +_Jl + "mm\r\n幅度:" + ptempData[m_LineStep_No].ToString(), drawFont, drawBrush, drawPoint);

        //        Pen p_Y = new Pen(Color.Red);
      //          g.DrawLine(p_Y, new Point(m_i_Screen_Posit_X, 0), new Point(m_i_Screen_Posit_X, m_flPic_H));

            }

            #region 计算高度
            Pen g_Sz = new Pen(Brushes.Red);
            g_Sz.Width = 4;
            g.DrawLine(g_Sz, new Point(m_i_X_A - 5, m_i_Y_A),
                                              new Point(m_i_X_A + 5, m_i_Y_A));
            g_Sz.Width = 1;
            g.DrawLine(g_Sz, new Point(m_i_X_A, 0),
                                                 new Point(m_i_X_A, m_flPic_H));
            //float _Time = Tofd.m_pTimeBuf[SysInfo.m_Plant.m_i_X_No] * 0.01f;
            //float _f1 = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iN0].m_dSpeed / 1000f * 0.5f *
            //    (_Time - SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam_Real[_iN0].T0);
            //_f1 *= _f1;
            //float _f2 = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iN0].m_fPcsLen / 2f;
            //_f2 *= _f2;
            //double _d = Math.Sqrt((double)(_f1 - _f2));
            #endregion 计算高度




            Rectangle _Rect = new Rectangle(0, 0, m_flPic_W, m_flPic_H);

            if (bg != null)
                gb.DrawImage(bg, _Rect);// 先绘制背景层
            gb.DrawImage(image, _Rect); // 再绘制绘画层

            PicA_Scan.BackgroundImage = (Bitmap)canvas.Clone(); // 设置为背景层，就是在内存中建立了一张对应的画布，重新refresh就是再次的加载这个画布

            m_bl_GetData = false;
        }
        #endregion 方法
       
    }
    /// <summary>
    /// C扫描：捕获探头数据
    /// </summary>
    public class Cls_C_Scan
    {
        #region 变量
        public bool m_blLink = false ;
        /// <summary>
        /// 超声探头DLL类型 0：我们的电磁超声 1:武汉中科压电超声  2 德朗脉冲涡流
        /// </summary>
        public int m_i_UI_DLL_Type = -1;

        Class_C_Buff m_SysInfo_C;

        #region 设备
        /// <summary>
        /// 爬行器通讯口
        /// </summary>
        Clb_MT_Comm.MT_Comm m_Climb4;

        Frame_Work.ClassTofd_Buff m_SysBuff;

        /// <summary>
        /// Electromagnetic ultrasound module
        /// </summary>
        public EM_RIT.ClEM_RIT m_UT_My_DLL;
        /// <summary>
        /// 涂层测厚
        /// </summary>
        public Coating.Cl_Coating m_Coat_DLL;
        /// <summary>
        /// 脉冲涡流DLL
        /// </summary>
        public ECT_DLL.Cl_ECT m_ECT_My_DLL ;

        /// <summary>
        /// Tofd控件
        /// </summary>
      //  public Tofd m_Tofd_DLL = new Tofd();
        public Tofd_DLL.CLTofd m_Tofd;
        /// <summary>
        /// TOFD联机
        /// </summary>
        private Thread Thread_Tofd_Link = null;
        #endregion 设备
        /// <summary>
        /// 读取通道数据
        /// </summary>
        private Thread Thread_Get_Cscan_Data = null;

        MsgInterFace m_Inter_Face;
        #endregion 变量

        /// <summary>
        /// 获得系统资源
        /// </summary>
        /// <param name="Sys_C_Buff"></param>
        public Cls_C_Scan(ref Class_C_Buff Sys_C_Buff, ref Clb_MT_Comm.MT_Comm Climb4,
                          ref Frame_Work.ClassTofd_Buff SysBuff, MsgInterFace Out_Inter_Face,
                          int i_TOFD_0_Cscan_1_Mui_2)
        {
            m_i_TOFD_0_Cscan_1_Mui_2 = i_TOFD_0_Cscan_1_Mui_2;
            m_Inter_Face = Out_Inter_Face;
            m_SysInfo_C = Sys_C_Buff;
            
            m_Climb4 = Climb4;
            m_SysBuff = SysBuff;
            m_UT_My_DLL = new ClEM_RIT(ref m_Climb4,ref Sys_C_Buff);
        }
        /// <summary>
        /// TOFD:0  Cscan:1   M_Ui:2    Coat: 3
        /// </summary>
        int m_i_TOFD_0_Cscan_1_Mui_2 = 0;
        #region 方法
        /// <summary>
        /// 启动超声设备
        /// </summary>
        public void Init_UI()
        {
            m_blLink = false;
            if (m_i_TOFD_0_Cscan_1_Mui_2 == 3)//涂层测厚
            {
                if (m_Coat_DLL==null  || m_Coat_DLL.m_blNetTrue == false)
                {
                    m_Coat_DLL = new Coating.Cl_Coating(ref m_Inter_Face);
                    m_blLink = m_Coat_DLL.Usb_Init();
                }
                //是否有电磁超声设备
                ClassInterFace _Inface = new ClassInterFace();
                string _strT = _Inface.IniReadDefine("ClassUltrasGate", "Txt_Mul_Select", "", System.Windows.Forms.Application.StartupPath + "\\database\\HardConfig.ini");

                if (m_UT_My_DLL.m_blNetTrue==false && _strT.Length > 0 && _strT.IndexOf("1") > -1)
                    m_UT_My_DLL.Usb_Init();
            }
            else
            {
                switch (m_i_UI_DLL_Type)
                {
                    case 0://自己超声模块
                        m_blLink = m_UT_My_DLL.Usb_Init();
                        break;
                    case 1://武汉中科超声模块
                        if (Thread_Tofd_Link != null) Thread_Tofd_Link.Abort();
                        Thread_Tofd_Link = new Thread(new ThreadStart(Tofd_Link));

                        Thread_Tofd_Link.Name = "Thread_Tofd_Link";
                        Thread_Tofd_Link.IsBackground = true;
                        Thread_Tofd_Link.Start();
                        break;
                    case 2://脉冲涡流
                        m_ECT_My_DLL = new Cl_ECT(m_SysInfo_C.m_Plant_C, m_Inter_Face);
                        m_ECT_My_DLL.Item_Inidt();
                        
                        m_blLink = m_ECT_My_DLL.Link();
                        m_UT_My_DLL.m_iRomoteNum = Cl_ECT.m_iRomoteNum;
                        break;
                }
            }
        }
        private void Tofd_Link()
        {
            m_blLink = m_SysBuff.m_Tofd_DLL.Link();
            m_Tofd = new CLTofd(ref m_SysBuff);
            Sel_ZhaMen();
        }
        /// <summary>
        /// 设置闸门
        /// </summary>
        public  void Sel_ZhaMen()
        {

            if (m_blLink && m_i_UI_DLL_Type==1)
            {
                //设置第一闸门
                Tofd.SendCmdGateStatus (0,true );// 
                Tofd.SendCmdGateStart(0, m_SysBuff.m_Tofd_DLL.m_Gate_1.i_Gate_S);
                Tofd.SendCmdGateEnd(0, m_SysBuff.m_Tofd_DLL.m_Gate_1.i_Gate_E);
                if (m_SysBuff.m_Tofd_DLL.m_Data_Type == 2)
                {
                    Tofd.SendCmdGateStatus(1, true);
                    Tofd.SendCmdGateStart(1, m_SysBuff.m_Tofd_DLL.m_Gate_2.i_Gate_S);
                    Tofd.SendCmdGateEnd(1, m_SysBuff.m_Tofd_DLL.m_Gate_2.i_Gate_E);
                }
                else
                    Tofd.SendCmdGateStatus(1, false );
            }
        }
        /// <summary>
        /// 超声模块仪器关闭
        /// </summary>
        public void Close()
        {
            if (m_i_TOFD_0_Cscan_1_Mui_2 == 3)
            {//涂层测厚
                m_Coat_DLL.Usb_Close();
            }
            else
            {
                switch (m_i_UI_DLL_Type)
                {
                    case 0://自己超声模块
                        m_UT_My_DLL.Usb_Close();
                        break;
                    case 1://武汉中科超声模块
                        m_SysBuff.m_Tofd_DLL.Close_TOFD();
                        break;
                    case 2://脉冲涡流
                        m_ECT_My_DLL.UdpClose();
                        break;
                }
            }
            m_blLink = false;
        }
        /// <summary>
        /// 接收超声数据
        /// </summary>
        public   void Thread_ReadUI()
        {
            if (Thread_Get_Cscan_Data != null) Thread_Get_Cscan_Data.Abort();
            Thread_Get_Cscan_Data = new Thread(new ThreadStart(Read_Cscan_Data));

            Thread_Get_Cscan_Data.Name = "Thread_Read_Cscan_Data";
            Thread_Get_Cscan_Data.IsBackground = true;
            Thread_Get_Cscan_Data.Start();
        }
        public double m_i_Runing = 0F;
        /// <summary>
        /// 涂层信息
        /// </summary>
        public string m_str_Coat_Msg = "";
        public float  m_fl_A_T=0,m_fl_B_T=0;

        int i_Para_Wz = 0;
        /// <summary>
        /// 获得TOFD数据进行处理   单调
        /// </summary>
        private void Read_Cscan_Data()
        {

            float _fl_Thick = m_SysInfo_C.m_Plant_C.flNormal_Thickness;// * 1.2f;
            if (m_i_TOFD_0_Cscan_1_Mui_2 == 3)
                _fl_Thick = m_SysInfo_C.m_Plant_C.flThick_Max_CoatLimit + m_SysInfo_C.m_Plant_C.flThick_Pc_CoatLimit;
            byte[] _Arr_Byte = new byte[800];
            float _flWc = 0f;
            bool _blAlarm;
            float _flThick = 0;
            int _iNum_Coat = 0;

            DateTime dtStar = DateTime.Now;
            bool _bl_GetData = false;
            bool _bl_ShowMark_Coat = false;

            while (m_SysInfo_C.m_Ctrl.m_iRun != 10)
            {
                //1 拿距离
                m_i_Runing = DateTime.Now.Subtract(dtStar).TotalMilliseconds;
                dtStar = DateTime.Now;
                /*  if (m_SysBuff.m_Climb.iBmq_Type == 1)
                    {
                        float _flDis = m_SysBuff.m_Climb.Trip_Com_mm * m_SysBuff.m_Tofd_DLL.fl4Car_JzXs;
                        m_SysBuff.m_Climb.Trip_Com_mm = (int)_flDis;
                        _flDis /= 1000;//m
                        m_SysBuff.m_Climb.Trip = _flDis;
                    }
                */
                //
                if (m_SysInfo_C.m_Ctrl.m_iRun != 3 && m_SysInfo_C.m_Plant_C.m_Ck_His_Wave == false)
                {
                    //2 读取数据
                    switch (m_i_UI_DLL_Type)
                    {
                        case 0://电磁超声探头
                            if (m_UT_My_DLL.m_bl_Have)
                            {
                                if (m_i_TOFD_0_Cscan_1_Mui_2 == 2)//多通道功能
                                {
                                    Get_Data_Mul_UI(m_UT_My_DLL.m_iRomoteNum);
                                }
                                else if (m_UT_My_DLL.m_Arr_RemoteIpPort != null)
                                {
                                    Get_Data(m_SysInfo_C.m_Ctrl.m_bl_CS && m_UT_My_DLL.m_blNetTrue == false ? _fl_Thick : m_UT_My_DLL.m_Arr_RemoteIpPort[0].m_flThick,
                                           m_UT_My_DLL.m_Arr_RemoteIpPort[0].m_btArrWave, m_UT_My_DLL.m_Arr_RemoteIpPort[0].iGain, m_UT_My_DLL.m_iGain_Limit);
                                    //if (m_SysInfo_C.m_Ctrl.m_iRun == 1)
                                    //    m_Inter_Face.Fun_Show_C();
                                }
                            }
                            _bl_GetData = false;

                            if (m_i_TOFD_0_Cscan_1_Mui_2 == 3 && m_Coat_DLL.m_Arr_RemoteIpPort != null&&
                             (   m_UT_My_DLL.m_bl_Only_Coat && m_UT_My_DLL.m_bl_Have==false|| m_UT_My_DLL.m_bl_Only_Coat==false && m_UT_My_DLL.m_bl_Have))//&&
                            {
                                //等待多通道数据到齐
                                if (m_Coat_DLL.m_Arr_RemoteIpPort != null)
                                {
                                    if (m_SysInfo_C.m_Ctrl.m_bl_CS == false)//正常运行判断是否接收数据
                                    {
                                        _bl_GetData = true;
                                        for (int i = 0; i < m_Coat_DLL.m_iRomoteNum; i++)
                                        {
                                            if (m_Coat_DLL.m_Arr_RemoteIpPort[i].blUse)
                                            {
                                                if (m_Coat_DLL.m_Arr_RemoteIpPort[i].m_blGetData == false)
                                                {
                                                    _bl_GetData = false;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                        _bl_GetData = true;
                                    //计算超限、数据保存到缓存
                                    if (_bl_GetData)//接收到所有数据
                                    {
                                        i_Para_Wz = m_Climb4.m_SysBuf.i_Para_Wz;
                                        if (i_Para_Wz < 0) i_Para_Wz = 0;
                                        m_str_Coat_Msg = "接收数据：" + DateTime.Now.ToString("HH:mm:ss:fff") + "\r\n";// + " Y:" + i_Para_Wz.ToString() + "\r\n";
                                        if (m_Coat_DLL.i_C_UIDLL_Coat_iWaitTime > 0) WaitTime(m_Coat_DLL.i_C_UIDLL_Coat_iWaitTime);

                                        //   m_str_Coat_Msg +=  DateTime.Now.ToString("HH:mm:ss:fff") + " Y:" + m_Climb4.m_SysBuf.i_Para_Wz.ToString() + "\r\n";

                                        _flThick = 0;//多通道涂层，计算平均值
                                        _iNum_Coat = 0;
                                        for (int i = 0; i < m_Coat_DLL.m_iRomoteNum; i++)
                                        {
                                            try
                                            {
                                                if (m_Coat_DLL.m_Arr_RemoteIpPort!=null && i< m_Coat_DLL.m_Arr_RemoteIpPort.Length  && m_Coat_DLL.m_Arr_RemoteIpPort[i].blUse)
                                                {
                                                    _iNum_Coat++;
                                                    _flThick += m_Coat_DLL.m_Arr_RemoteIpPort[i].m_flThick;
                                                    m_Coat_DLL.m_Arr_RemoteIpPort[i].flRatio = (float)m_Climb4.m_SysBuf.Trip;//X轴位置
                                                    m_Coat_DLL.m_Arr_RemoteIpPort[i].iGain = i_Para_Wz;//Y轴当前位置
                                                }
                                            }
                                            catch (Exception E_coat)
                                            { }
                                        }
                                        _flThick /= _iNum_Coat;
                                        Get_Data_Coat(_flThick);

                                        //探头抬起动作


                                    }
                                }
                
                                m_Inter_Face.Fun_ShowCoat_C(_bl_GetData);

                                if (_bl_GetData) //结束拿厚度值，完成本次动作
                                {
                                    if (m_Coat_DLL.m_Arr_RemoteIpPort != null)
                                    {
                                        for (int i = 0; i < m_Coat_DLL.m_iRomoteNum; i++)
                                            m_Coat_DLL.m_Arr_RemoteIpPort[i].m_blGetData = false;
                                    }
                                }
                            }

                            break;
                        case 1://武汉中科探头
                            if (m_Tofd == null) continue;
                            m_Tofd.GetData();
                            try
                            {
                                if (m_SysBuff.m_Tofd_DLL.m_Data_Type == 2)
                                {
                                    //m_fl_A_T = Tofd.getGateT(0, 0);
                                    //m_fl_B_T = Tofd.getGateT(0, 1);
                                    m_SysBuff.m_Tofd_DLL.m_flThick = Tofd.getGateS(0, 1) - Tofd.getGateS(0, 0);
                                }
                                else if (m_SysBuff.m_Tofd_DLL.m_Data_Type == 1)
                                {
                                    //       m_fl_A_T = Tofd.getGateT(0, 0);
                                    m_SysBuff.m_Tofd_DLL.m_flThick = Tofd.getGateS(0, 0);
                                }
                            }
                            catch (Exception eget)
                            {
                            }
                            Get_Data(m_SysInfo_C.m_Ctrl.m_bl_CS ? _fl_Thick : m_SysBuff.m_Tofd_DLL.m_flThick, Tofd.m_pChannelBuf);
                            break;
                        case 2://脉冲涡流
                            if (m_i_TOFD_0_Cscan_1_Mui_2 == 2)
                                Get_Data_Mul_ECT(ECT_DLL.Cl_ECT.m_iRomoteNum);
                            break;
                    }

                    if (m_SysInfo_C.m_Ctrl.m_bl_CS &&
                        m_SysInfo_C.m_Ctrl.m_iRun == 1 && m_blLink == false)
                    {
                        _fl_Thick -= 0.01f;
                        _flWc = Math.Abs(_fl_Thick - m_SysInfo_C.m_Plant_C.flNormal_Thickness);//计算误差

                        _blAlarm = _flWc >= m_SysInfo_C.m_Plant_C.flstrThickAlarm;
                        if (_fl_Thick <= 2)
                        {
                            if (m_i_TOFD_0_Cscan_1_Mui_2 == 3)
                            { 
                                if (_fl_Thick < 0)
                                    _fl_Thick = m_SysInfo_C.m_Plant_C.flThick_Max_CoatLimit + m_SysInfo_C.m_Plant_C.flThick_Pc_CoatLimit;
                            }
                            else
                                _fl_Thick = m_SysInfo_C.m_Plant_C.flNormal_Thickness * 1.2f;
                        }
                        if (m_i_UI_DLL_Type == 0 && m_UT_My_DLL.m_bl_Have && m_UT_My_DLL.m_blNetTrue == false)
                            for (int _i = 0; _i < m_UT_My_DLL.m_iRomoteNum; _i++)
                                m_UT_My_DLL.m_Arr_RemoteIpPort[_i].m_flThick = _fl_Thick;
                        if(m_i_UI_DLL_Type == 2 && m_ECT_My_DLL.blNetTrue == false )
                        {
                            for (int i = 0; i < Cl_ECT.m_iRomoteNum; i++)
                            {
                                m_ECT_My_DLL.Set_CurrRun_Data(i, _fl_Thick, m_SysInfo_C.m_Ctrl.m_bl_CS);
                                m_ECT_My_DLL. Set_Buff(i);
                            }
                        }
                        if (m_i_TOFD_0_Cscan_1_Mui_2 == 3)
                            for (int i = 0; i < m_Coat_DLL.m_iRomoteNum; i++)
                                m_Coat_DLL.m_Arr_RemoteIpPort[i].m_flThick = _fl_Thick;
                    }
                }
                //3 如果需要延时，就延时
                // if(m_SysInfo_C.m_Ctrl.i_C_UIDLL_iWaitTime>0)
                WaitTime(m_SysInfo_C.m_Ctrl.i_C_UIDLL_iWaitTime);
                m_csInter.WriteErrorLog(strLogFileName, "距离" + m_Climb4.m_SysBuf.Trip_Com_mm, false);
            }
        }
        public string strLogFileName = Application.StartupPath + "\\datalog\\CommLog.ini";
        ClassInterFace m_csInter = new ClassInterFace();
        /// <summary>
        /// 返回颜色   i_Y_No光栅臂对应缓存序号
        /// </summary>
        int R = 0, G = 0, B = 0,i_Y_No, i_Trip_Com_mm;
        float m_flThickness = 0;
        /// <summary>
        /// 当前距离对应车体前进序号
        /// 多通道缓存：对应的是显示缓存链表的序号
        /// </summary>
        public    int m_i_X_Buff_No = 0;
        /// <summary>
        /// 新方法C扫描X轴位置
        /// </summary>
        public int m_i_C_X_Buff_No = 0;

        public int m_i_X_Buff_No_Coat = 0;
        #region 位置跟踪器
        /// <summary>
        /// 记录车体运行方向数据的老位置
        /// </summary>
        int m_P_i_X_Buff_No_Old = -1;
        /// <summary>
        /// 记录光栅臂运行方向数据的老位置
        /// </summary>
         int m_P_i_Y_ShowBuff_No_Old = -1;
        /// <summary>
        /// 记录长度
        /// </summary>
        public static  int m_P_i_Len = 2000;
        /// <summary>
        /// 新位置序号
        /// </summary>
        public int m_P_iNewPosit_No = 0;
        /// <summary>
        /// 界面显示数据缓存位置定位器
        /// </summary>
        public int m_P_i_Show_No = 0;

        /// <summary>
        /// 环形位置缓存：车体位置
        /// </summary>
        public int[] m_P_Arr_Posi_X = new int[m_P_i_Len];
        /// <summary>
        /// 环形位置缓存：光栅臂
        /// </summary>
        public int[] m_P_Arr_Posi_Y = new int[m_P_i_Len];
        /// <summary>
        /// 环形位置缓存：距离值mm
        /// </summary>
        public int[] m_P_Arr_Distanc = new int[m_P_i_Len];
        /// <summary>
        /// 保存当前间隔区间的显示的数据：厚度/颜色
        /// </summary>
        //public   Cls_EMAT_2[] m_Arr_C_Data;
        /// <summary>
        /// 涂层  厚度/颜色
        /// </summary>
        //public Cls_EMAT_2[] m_Arr_C_Data_Coat;
        #endregion 位置跟踪器

        /// <summary>
        /// 当前距离对应屏幕准备显示的列号
        /// </summary>
        public int m_i_X_Screen_No = 0;

        /// <summary>
        /// C扫描 当前距离对应屏幕准备显示的列号
        /// </summary>
        public int m_i_C_X_Screen_No = 0;
        /// <summary>
        /// 临时变量
        /// </summary>
        float m_flDat = 0,m_flWc=0;
        /// <summary>
        /// 光栅臂界面显示缓存序号
        /// </summary>
        public int m_i_Y_ShowBuff_No = 0;

        /// <summary>
        /// C扫描：光栅臂界面显示缓存序号
        /// </summary>
        public int m_i_ShowBuff_No_Y = -1;

        public bool m_bl_Begin = false;
        /// <summary>
        /// 初始化 0:左-右  1：右到左
        /// </summary>
        public int iGsb_RunFx_L_to_R = -1;
        /// <summary>
        /// C扫描：车体前进后退位置序号
        /// </summary>
        //      public int m_i_ShowBuff_No_X = -1;


        /// <summary>
        /// 涂层探头：光栅臂界面显示缓存序号
        /// </summary>
        public int m_i_Y_ShowBuff_No_Coat = 0;
        ///// <summary>
        ///// 
        ///// </summary>
        //public bool m_bl_ShowData_Coat = false;

        //public bool m_bl_ShowData_Coat_To_Mark = false;
        //public int m_i_Y_No_Coat = 0;

        //public int m_i_X_No_Coat = 0;
        /// <summary>
        /// Y轴显示缓存个数
        /// </summary>
        public int m_i_Y_BuffNum = 0;
        /// <summary>
        /// 是否报警
        /// </summary>
        bool blAlarm = false;
        /// <summary>
        /// 颜色
        /// </summary>
        string strColor = "";

        int m_iCurr_X = 0;
        int m_iCurr_Y = 0;


        public int m_i_Alarm_Num = -1;

        public int m_i_Alarm_Num_Coat = -1;
        /// <summary>
        /// 是否允许接收数据
        /// </summary>
        private bool m_bl_Allowed_To_Accept = false;
        /// <summary>
        /// 统计当前缺陷
        /// </summary>
     //  public  CL_AlarmData m_Alarm = new CL_AlarmData();

        /// <summary>
        /// 接收到厚度值的数据处理
        /// </summary>
        /// <param name="flThickness">厚度值</param>
        /// <param name="ArrWave">波形</param>
        private unsafe void Get_Data(float flThickness, byte[] ArrWave,int iGain=0,int iGain_Limit=0)
        {
            if (m_SysInfo_C.m_Ctrl.m_iRun != 1) return;

            int _iArr_Len = 0;
            if (ArrWave != null) _iArr_Len= ArrWave.Length;//波形数组长度

            if (m_SysInfo_C.m_Ctrl.m_bl_CS && m_i_TOFD_0_Cscan_1_Mui_2 !=3)
            {
                if (m_SysBuff.m_Climb.iRun_Gsb_Sc1_Zx0 == 0)
                {
                    if (m_Climb4.m_SysBuf.iRun_Gsb_Qj1_Ht0 == 1)
                        m_Climb4.m_SysBuf.Trip_Com_mm += m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X;
                    else
                        m_Climb4.m_SysBuf.Trip_Com_mm -= m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X;
                    m_Climb4.m_SysBuf.Trip = m_Climb4.m_SysBuf.Trip_Com_mm / 1000F;

                    m_Climb4.m_SysBuf.i_Para_Wz = m_SysBuff.m_Climb.i_Gsb_End_Pos;
                }
                else
                {
                    if (m_Climb4.m_SysBuf.blGsb_RunFx_L_to_R)
                        m_Climb4.m_SysBuf.i_Para_Wz++;
                    else
                        m_Climb4.m_SysBuf.i_Para_Wz--;

                    if (m_Climb4.m_SysBuf.i_Para_Wz >= m_Climb4.m_SysBuf .i_Gsb_End_Pos )    //.iGsb_Len)
                    {
                        m_Climb4.m_SysBuf.i_Para_Wz = m_SysBuff.m_Climb.i_Gsb_End_Pos;
                        m_Climb4.m_SysBuf.blGsb_RunFx_L_to_R = !m_Climb4.m_SysBuf.blGsb_RunFx_L_to_R;

                        if (m_Climb4.m_SysBuf.iRun_Gsb_Qj1_Ht0 == 1)
                            m_Climb4.m_SysBuf.Trip_Com_mm += m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X;
                        else
                            m_Climb4.m_SysBuf.Trip_Com_mm -= m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X;
                      //  m_Climb4.m_SysBuf.Trip = m_Climb4.m_SysBuf.Trip_Com_mm / 1000F;
                    }

                    if (m_Climb4.m_SysBuf.i_Para_Wz < m_Climb4.m_SysBuf.i_Gsb_Start_Pos )
                    {
                        m_Climb4.m_SysBuf.i_Para_Wz = m_Climb4.m_SysBuf.i_Gsb_Start_Pos;

                        m_Climb4.m_SysBuf.blGsb_RunFx_L_to_R = !m_Climb4.m_SysBuf.blGsb_RunFx_L_to_R;
                        if (m_Climb4.m_SysBuf.iRun_Gsb_Qj1_Ht0 == 1)
                            m_Climb4.m_SysBuf.Trip_Com_mm += m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X;
                        else
                            m_Climb4.m_SysBuf.Trip_Com_mm -= m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X;
                    }
                }
                m_Climb4.m_SysBuf.Trip = m_Climb4.m_SysBuf.Trip_Com_mm / 1000F;
            }
            if (m_i_TOFD_0_Cscan_1_Mui_2 != 3 && m_Climb4.m_SysBuf.m_UI_Gsb_Distan_Min > 0)
            {
                if (m_Climb4.m_SysBuf.blGsb_RunFx_L_to_R)
                {
                   if( m_Climb4.m_SysBuf.i_Para_Wz + 1>= m_Climb4.m_SysBuf.i_Gsb_End_Pos - m_Climb4.m_SysBuf.m_UI_Gsb_Distan_Min)
                    {
                        if(m_Climb4.m_SysBuf.i_Para_Wz-1 < m_Climb4.m_SysBuf.i_Gsb_End_Pos)
                            m_Climb4.m_SysBuf.i_Para_Wz++;
                    }
               
                }
                else
                {
                    if (m_Climb4.m_SysBuf.i_Para_Wz <= m_Climb4.m_SysBuf.i_Gsb_Start_Pos+ m_Climb4.m_SysBuf.m_UI_Gsb_Distan_Min)
                    {
                        if (m_Climb4.m_SysBuf.i_Para_Wz > m_Climb4.m_SysBuf.i_Gsb_Start_Pos)
                        m_Climb4.m_SysBuf.i_Para_Wz--;
                    }
              
                }
            }    
            if ( m_Climb4.m_SysBuf.i_Para_Wz >= m_Climb4.m_SysBuf.i_Gsb_End_Pos)
                        m_Climb4.m_SysBuf.i_Para_Wz = m_Climb4.m_SysBuf.i_Gsb_End_Pos-1;
            //if (m_Climb4.m_SysBuf.i_Para_Wz <= m_Climb4.m_SysBuf.i_Gsb_Start_Pos)
            //    m_Climb4.m_SysBuf.i_Para_Wz = m_Climb4.m_SysBuf.i_Gsb_Start_Pos +1;
            switch (m_i_UI_DLL_Type)
            {
                case 0://德朗 前进距离
                case 2://脉冲涡流
                    i_Trip_Com_mm = m_Climb4.m_SysBuf.Trip_Com_mm;
                    break;
                case 1://中科 前进距离
                    i_Trip_Com_mm =(int) m_SysBuff.m_Tofd_DLL.m_pSparam_Real
                                        [m_SysBuff.m_Tofd_DLL.m_icurChan].m_fEnReal
                                        [m_SysBuff.m_Tofd_DLL.m_pSparam
                                        [m_SysBuff.m_Tofd_DLL.m_icurChan].m_iCurEn]; //(int)(m_SysBuff.m_Tofd_DLL.m_flDistanc_X * 1000);
                    break;
            }
            //1.3 光栅臂对应位置
           //0-300  0 1 2 3 4    286 287 288 289 300
            i_Y_No = m_Climb4.m_SysBuf.i_Para_Wz;
            if (i_Y_No < 0) i_Y_No = 0;
            
            int _iLst_Buff_Len = m_SysInfo_C.m_C_One_Buff.Count;

            #region 1 计算车体位置对应的XY位置
            //    i_Trip_Com_mm = 39;
            //1.1 由实际点计算数据对应在缓存中开始序号

          //  if (i_Trip_Com_mm==0 || m_Climb4.m_SysBuf.blGsb_RunFx_L_to_R_Old != m_Climb4.m_SysBuf.blGsb_RunFx_L_to_R)
         //   {
                m_flDat = Math.Abs(i_Trip_Com_mm - m_Climb4.m_SysBuf.Trip_Com_Init_mm) * 1.0f / m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X;
                m_i_X_Buff_No = (int)m_flDat;//

         //   }


            //if (m_flDat - m_i_X_Buff_No >= 0.5) //防止超越一个间隔
            //    m_i_X_Buff_No++;

            //1.2 由实际点计算数据对应在当前屏幕中开始序号
            m_flDat = ((i_Trip_Com_mm- m_SysInfo_C.m_Plant_C.i_Screen_Start_Distance) * 1.0f / m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X);
            m_iCurr_X = (int)m_flDat;//
            //if (m_flDat - m_iCurr_X >= 0.5) 
            //    m_iCurr_X++;
            m_i_X_Screen_No = m_iCurr_X;

            //1.4 计算光栅臂对应显示位置
            m_flDat =i_Y_No / m_SysInfo_C.m_Plant_C.Scree_iDotHeightmm_Y;//Scree_iDotHeightmm_Y_Coat
            m_iCurr_Y = (int)m_flDat;
            
           //float  _fLen = 1.0f * m_SysInfo_C.m_Plant_C.iGsb_Len /
           //                     m_SysInfo_C.m_Plant_C.Scree_iDotHeightmm_Y;
           // int _iLen = (int)_fLen;
           if (m_flDat - m_iCurr_Y == 0 && m_iCurr_Y >= m_SysBuff.m_Climb.i_Gsb_Show_All_Num) m_iCurr_Y--;

            m_i_Y_ShowBuff_No = m_iCurr_Y;
            if (m_i_Y_ShowBuff_No < 0) m_i_Y_ShowBuff_No = 0;
            #endregion 1

            #region 2 添加新列数据
            if (m_i_X_Buff_No > m_SysInfo_C.m_Ctrl.i_Buff_Len)//数据前进不会跨区域
            {
                int _iTimes = m_i_X_Buff_No - m_SysInfo_C.m_Ctrl.i_Buff_Len;
                for (int _iTs = 0; _iTs < _iTimes; _iTs++)
                {
                    CLs_EMAT_Data _Cls_Data = new CLs_EMAT_Data(
                          m_SysInfo_C.m_Plant_C.iGsb_Len + 1, m_SysBuff.m_Climb.i_Gsb_Show_All_Num);
                    _Cls_Data.i_X_mm = i_Trip_Com_mm;//cdw 2025-5-2

                    for (int i = 0; i < _Cls_Data.Arr_C_Data.Count(); i++)
                        _Cls_Data.Arr_C_Data[i] = new Cls_EMAT_2();
                    for (int i = 0; i < _Cls_Data.Arr_Emat.Count(); i++)
                        _Cls_Data.Arr_Emat[i] = new Cls_EMAT_1(_iArr_Len);
                    m_SysInfo_C.m_C_One_Buff.Add(_Cls_Data);
                }
                m_SysInfo_C.m_Ctrl.i_Buff_Len = m_SysInfo_C.m_C_One_Buff.Count - 1;//比长度小1，目的是和序号对应0-N

            }
            #endregion 2
            #region 3 数据保存
            #region 3.1 原始缓存   数据存储在：车体位置对应X/Y方向对应的缓存列号中
            if (m_i_X_Buff_No >= m_SysInfo_C.m_C_One_Buff.Count())
                m_i_X_Buff_No= m_SysInfo_C.m_C_One_Buff.Count() - 1;

            #region 允许接收的缓存两头，都已经有数据时，车体在步进过程中，此时不需要采集数据
            m_bl_Allowed_To_Accept = m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_SysBuff.m_Climb.i_Gsb_Show_LeftBoundary].blUse &&
                                     m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_SysBuff.m_Climb.i_Gsb_Show_RightBoundary].blUse;
            if (m_bl_Allowed_To_Accept) return;//2025-5-2 add 
            #endregion 

            m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_Emat[i_Y_No].flThick = flThickness;
            m_flWc =  float.Parse(( m_SysInfo_C.m_Plant_C.flNormal_Thickness -flThickness ).ToString("f2"));//计算误差
            float _flBfz =Math .Abs ( m_flWc / m_SysInfo_C.m_Plant_C.flNormal_Thickness);
            float _flLimt = m_SysInfo_C.m_Plant_C.fl_Max_Limit / 100.0f;

            if (_flBfz <= _flLimt || m_SysInfo_C.m_Plant_C.i_Alarm == 0)
            {
                blAlarm = Math.Abs(m_flWc) >= m_SysInfo_C.m_Plant_C.flstrThickAlarm;
                if (m_flWc < 0 && blAlarm)
                {
                    blAlarm = false;//厚度超过公称厚度
                    m_flWc = 100;
                }

                m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_Emat[i_Y_No].blAlarm = blAlarm;

                if (m_flWc > 0)//减薄
                    m_SysInfo_C.m_Plant_C.CalculThickColor(m_flWc, out R, out G, out B, 0, m_SysInfo_C.m_Plant_C.Ck_No_Normal_Thickness);
                else//增厚
                    m_SysInfo_C.m_Plant_C.CalculThickColor_A(Math.Abs(m_flWc), out R, out G, out B, 0, m_SysInfo_C.m_Plant_C.Ck_No_Normal_Thickness);
            }
            else
            {
                blAlarm = false;
                m_flWc = 100;
                m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_Emat[i_Y_No].blAlarm = false;
                //   m_SysInfo_C.m_Plant_C.CalculThickColor(m_flWc, out R, out G, out B, 0, m_SysInfo_C.m_Plant_C.Ck_No_Normal_Thickness);
                R = m_SysInfo_C.m_Plant_C.i_Alarm_Out_R; //255;
                G = m_SysInfo_C.m_Plant_C.i_Alarm_Out_G; //245;
                B = m_SysInfo_C.m_Plant_C.i_Alarm_Out_B;//0
            }
            strColor = R.ToString() + "/" + G.ToString() + "/" + B.ToString();
            m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_Emat[i_Y_No].strColor = strColor;
            m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_Emat[i_Y_No].R = R;
            m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_Emat[i_Y_No].G = G;
            m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_Emat[i_Y_No].B = B;
            m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_Emat[i_Y_No].iGain = iGain;
            m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_Emat[i_Y_No].iGain_Limit = iGain_Limit;

            if (m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_Emat[i_Y_No].btArrWave == null)
                m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_Emat[i_Y_No].btArrWave = new byte[_iArr_Len];
            var IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_Emat[i_Y_No].btArrWave, 0);
           if(ArrWave!=null ) Marshal.Copy(ArrWave, 0, IntPtArr, _iArr_Len);//注意数据范围
            #endregion  3.1

            #region 3.1.2异常数据统计
            if(blAlarm )
            {
                if (m_SysInfo_C.m_One_Alarm_Buff.Count ==0)//  m_Alarm.i_X_S == -1)//第一个缺陷
                {
                    CL_AlarmData _Alarm = new CL_AlarmData();
                    _Alarm.i_X_S = i_Trip_Com_mm;
                    _Alarm.i_X_E = i_Trip_Com_mm;
                    _Alarm.fl_Wc = m_flWc;
                    _Alarm.i_Y_S = i_Y_No;
                    _Alarm.i_Y_X = i_Trip_Com_mm;

                    _Alarm.fl_Min_Thick = flThickness;
                    m_SysInfo_C.m_One_Alarm_Buff.Add(_Alarm);
                    m_i_Alarm_Num++;
                }
                else if (Math.Abs(i_Trip_Com_mm - m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num ].i_X_E) <= m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X)// 5) //只统计X轴方向
                {
                    if (i_Trip_Com_mm != m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_E)
                    {
                        m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_E = i_Trip_Com_mm;

                        if (m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].fl_Wc < m_flWc)
                        {
                            m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].fl_Wc = m_flWc;
                            m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].fl_Min_Thick = flThickness;
                            m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_Y_S = i_Y_No;
                            m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_Y_X = i_Trip_Com_mm;
                        }
                    }
                    else if (i_Trip_Com_mm == m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_E)
                    {
                        if (m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].fl_Wc < m_flWc)
                        {
                            m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].fl_Wc = m_flWc;
                            m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].fl_Min_Thick = flThickness;
                            m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_Y_S = i_Y_No;
                            m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_Y_X = i_Trip_Com_mm;
                        }
                    }
                }
                else //X方向大于5mm
                {
                    //数据整理
                    if (m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_S> m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_E)
                    {
                        int _id = m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_S;
                        m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_S = m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_E;
                        m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_E = _id;
                    }
                    //新区间
                    CL_AlarmData _Alarm = new CL_AlarmData();
                    _Alarm.i_X_S = i_Trip_Com_mm;
                    _Alarm.i_X_E = i_Trip_Com_mm;
                    _Alarm.fl_Wc = m_flWc;
                    _Alarm.i_Y_S = i_Y_No;
                    _Alarm.i_Y_X = i_Trip_Com_mm;
                    _Alarm.fl_Min_Thick = flThickness;

                    m_SysInfo_C.m_One_Alarm_Buff.Add(_Alarm);
                    m_i_Alarm_Num++;
                }
            }
            #endregion 3.1.2

            #region 3.2  计算区间显示值
            if (//m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].flWc <  m_flWc||
                      m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].flThick == -1 ||
             Math.Abs(m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].flWc) < Math.Abs(m_flWc)
                   )
            {
                m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].i_X_mm = i_Trip_Com_mm;//
                                                                               //      m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].m_i_X = i_Trip_Com_mm;
                m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].blUse = true;
                //   m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].m_i_X = m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].i_X_mm;
                m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].iGain_Limit = iGain_Limit;
                m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].iGain = iGain;
                m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].flThick = flThickness;
                m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].blAlarm = blAlarm;
                m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].strColor = strColor;
                m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].R = R;
                m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].G = G;
                m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].B = B;

                m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].flWc = m_flWc;
                m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].iArr_ShowNo = i_Y_No;

                if (m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].btArrWave == null)
                    m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].btArrWave = new byte[_iArr_Len];
                var IntPtArr_S = Marshal.UnsafeAddrOfPinnedArrayElement(m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].btArrWave, 0);
                if (ArrWave != null) Marshal.Copy(ArrWave, 0, IntPtArr_S, _iArr_Len);//注意数据范围
            }
            //else
            //{ 
            //if (m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].flThick==-1)
            //    { }
            //}
            #endregion 3.2

            //    Chang_Posit();

            #endregion 3
        }
        private unsafe void Get_Data_Coat(float flThickness)
        {
            if (m_SysInfo_C.m_Ctrl.m_iRun != 1) return;

            int _iArr_Len = 0;

            if (m_SysInfo_C.m_Ctrl.m_bl_CS)
            {
                if (m_SysBuff.m_Climb.iRun_Gsb_Sc1_Zx0 == 0)
                {
                    if (m_Climb4.m_SysBuf.iRun_Gsb_Qj1_Ht0 == 1)
                        m_Climb4.m_SysBuf.Trip_Com_mm += m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X;
                    else
                        m_Climb4.m_SysBuf.Trip_Com_mm -= m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X;
                    m_Climb4.m_SysBuf.Trip = m_Climb4.m_SysBuf.Trip_Com_mm / 1000F;

                    m_Climb4.m_SysBuf.i_Para_Wz = m_SysBuff.m_Climb.i_Gsb_End_Pos;
                }
                else
                {
                    if (m_i_TOFD_0_Cscan_1_Mui_2 == 3)
                    {
              //          if (m_Climb4.m_SysBuf.m_bl_Cs_Y)
                        {
                            m_Climb4.m_SysBuf.m_bl_Cs_Y = false;
                            //          m_Climb4.m_SysBuf.i_Para_Wz = 119;
                            if (m_Climb4.m_SysBuf.blGsb_RunFx_L_to_R)
                                m_Climb4.m_SysBuf.i_Para_Wz += 1;// m_Climb4.m_SysBuf.i_Coat_Interval;
                            else
                                m_Climb4.m_SysBuf.i_Para_Wz -= 1;// m_Climb4.m_SysBuf.i_Coat_Interval;
                        }
                    }
                    else
                    {
                        if (m_Climb4.m_SysBuf.blGsb_RunFx_L_to_R)
                            m_Climb4.m_SysBuf.i_Para_Wz++;
                        else
                            m_Climb4.m_SysBuf.i_Para_Wz--;
                    }
                    if (m_Climb4.m_SysBuf.i_Para_Wz > m_Climb4.m_SysBuf.iGsb_Len)
                    {
                        m_Climb4.m_SysBuf.i_Para_Wz = m_Climb4.m_SysBuf.iGsb_Len;
                        m_Climb4.m_SysBuf.blGsb_RunFx_L_to_R = !m_Climb4.m_SysBuf.blGsb_RunFx_L_to_R;

                        if (m_Climb4.m_SysBuf.iRun_Gsb_Qj1_Ht0 == 1)
                            m_Climb4.m_SysBuf.Trip_Com_mm += m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X;
                        else
                            m_Climb4.m_SysBuf.Trip_Com_mm -= m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X;
                        m_Climb4.m_SysBuf.Trip = m_Climb4.m_SysBuf.Trip_Com_mm / 1000F;
                    }

                    if (m_Climb4.m_SysBuf.i_Para_Wz < 0)
                    {
                        m_Climb4.m_SysBuf.i_Para_Wz = 0;

                        m_Climb4.m_SysBuf.blGsb_RunFx_L_to_R = !m_Climb4.m_SysBuf.blGsb_RunFx_L_to_R;
                        if (m_Climb4.m_SysBuf.iRun_Gsb_Qj1_Ht0 == 1)
                            m_Climb4.m_SysBuf.Trip_Com_mm += m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X;
                        else
                            m_Climb4.m_SysBuf.Trip_Com_mm -= m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X;
                    }
                }
                m_Climb4.m_SysBuf.Trip = m_Climb4.m_SysBuf.Trip_Com_mm / 1000F;
            }
      //      goto Cdww;
            i_Trip_Com_mm = m_Climb4.m_SysBuf.Trip_Com_mm;

            //1.3 光栅臂对应位置
            int _iLst_Buff_Len = m_SysInfo_C.m_C_One_Buff_Coat.Count;

            #region 1 计算车体位置对应的XY位置
            //1.1 由实际点计算数据对应在缓存中开始序号
            m_flDat = Math.Abs(i_Trip_Com_mm - m_Climb4.m_SysBuf.Trip_Com_Init_mm) * 1.0f / m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X;
            m_i_X_Buff_No_Coat = (int)m_flDat;//
            if (m_flDat - m_i_X_Buff_No_Coat >= 0.5)
                m_i_X_Buff_No_Coat++;

            //1.2 由实际点计算数据对应在当前屏幕中开始序号
            m_flDat = (1.0f * (i_Trip_Com_mm - m_SysInfo_C.m_Plant_C.i_Screen_Start_Distance) / m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X);
            m_iCurr_X = (int)m_flDat;//
            if (m_flDat - m_iCurr_X >= 0.5)
                m_iCurr_X++;
            m_i_X_Screen_No = m_iCurr_X;

            //1.4 计算光栅臂对应显示位置          
            //0-300  0 1 2 3 4    286 287 288 289 300
            i_Para_Wz=m_Climb4.m_SysBuf.i_Para_Wz;
            i_Y_No = i_Para_Wz;// 
            if (i_Y_No < 0) i_Y_No = 0;
            if (i_Y_No > m_SysInfo_C.m_Plant_C.iGsb_Len)
                i_Y_No = m_SysInfo_C.m_Plant_C.iGsb_Len;
            m_flDat = 1.0f * i_Y_No / m_SysInfo_C.m_Plant_C.Scree_iDotHeightmm_Y_Coat;
            m_iCurr_Y = (int)m_flDat;
            if (m_flDat - m_iCurr_Y >= 0.5) m_iCurr_Y++;
            float _ftLen = 1.0f * m_SysInfo_C.m_Plant_C.iGsb_Len /
                                 m_SysInfo_C.m_Plant_C.Scree_iDotHeightmm_Y_Coat;
            int _iLen = (int)_ftLen;
            m_i_Y_BuffNum = _iLen;
            if (m_flDat - m_iCurr_Y == 0 && m_iCurr_Y >= _iLen+2)
                m_iCurr_Y--;

            m_i_Y_ShowBuff_No_Coat = m_iCurr_Y;
            if (m_i_Y_ShowBuff_No_Coat < 0) m_i_Y_ShowBuff_No_Coat = 0;
            #endregion 1

            #region 2 添加新列数据
            if (m_i_X_Buff_No_Coat > m_SysInfo_C.m_Ctrl.i_Buff_Len_Coat)//数据前进不会跨区域
            {
                int _iTimes = m_i_X_Buff_No_Coat - m_SysInfo_C.m_Ctrl.i_Buff_Len_Coat;
                for (int _iTs = 0; _iTs < _iTimes; _iTs++)
                {
                    CLs_EMAT_Data _Cls_Data = new CLs_EMAT_Data(
                          m_SysInfo_C.m_Plant_C.iGsb_Len + 1, _iLen+2);
                    _Cls_Data.i_X_mm = i_Trip_Com_mm;

                    for (int i = 0; i < _Cls_Data.Arr_C_Data.Count(); i++)
                        _Cls_Data.Arr_C_Data[i] = new Cls_EMAT_2();
                    for (int i = 0; i < _Cls_Data.Arr_Emat.Count(); i++)
                        _Cls_Data.Arr_Emat[i] = new Cls_EMAT_1(_iArr_Len);
                    m_SysInfo_C.m_C_One_Buff_Coat.Add(_Cls_Data);
                }
                m_SysInfo_C.m_Ctrl.i_Buff_Len_Coat = m_SysInfo_C.m_C_One_Buff_Coat.Count - 1;//比长度小1，目的是和序号对应0-N
            }
            #endregion 2

            #region 3 数据保存
            #region 3.1 原始缓存   数据存储在：车体位置对应X/Y方向对应的缓存列号中
            if (m_i_X_Buff_No_Coat >= m_SysInfo_C.m_C_One_Buff_Coat.Count() && m_SysInfo_C.m_C_One_Buff_Coat.Count() > 0)
                m_i_X_Buff_No_Coat = m_SysInfo_C.m_C_One_Buff_Coat.Count() - 1;

            m_SysInfo_C.m_C_One_Buff_Coat[m_i_X_Buff_No_Coat].Arr_Emat[i_Y_No].flThick = flThickness;
            string _strMulThic = "";
            for (int i = 0; i < m_Coat_DLL.m_iRomoteNum; i++)
                _strMulThic += (i == 0 ? "" : "/") + m_Coat_DLL.m_Arr_RemoteIpPort[i].m_flThick;

            m_SysInfo_C.m_C_One_Buff_Coat[m_i_X_Buff_No_Coat].Arr_Emat[i_Y_No].str_Mul_Thick = _strMulThic;

            #region 误差计算 厚度有个区间 最小-->最大  误差偏差

            //1 独立数据

            float _fl_Wc = 0;
            m_flWc = 0;
            bool _bl_Alarm = false;
            for (int i = 0; i < m_Coat_DLL.m_iRomoteNum; i++)
            {
                m_Coat_DLL.m_Arr_RemoteIpPort[i].blAlarm = false;//合格
                //1 小于最小厚度值
                if (m_SysInfo_C.m_Plant_C.flThick_Min_CoatLimit > 0)
                {
                    _fl_Wc = m_SysInfo_C.m_Plant_C.flThick_Min_CoatLimit - m_Coat_DLL.m_Arr_RemoteIpPort[i].m_flThick;
                    if (_fl_Wc > 0)//减薄
                    {
                        m_Coat_DLL.m_Arr_RemoteIpPort[i].blAlarm = _fl_Wc > m_SysInfo_C.m_Plant_C.flThick_Pc_CoatLimit;

                    }
                    if (m_flWc < _fl_Wc) m_flWc = _fl_Wc;
                }
                //2 大于最大厚度值
                if (m_SysInfo_C.m_Plant_C.flThick_Max_CoatLimit > 0 && m_Coat_DLL.m_Arr_RemoteIpPort[i].blAlarm == false)
                {
                    _fl_Wc = m_Coat_DLL.m_Arr_RemoteIpPort[i].m_flThick - m_SysInfo_C.m_Plant_C.flThick_Max_CoatLimit;
                    if (_fl_Wc > 0)//增厚
                    {
                        m_Coat_DLL.m_Arr_RemoteIpPort[i].blAlarm = _fl_Wc > m_SysInfo_C.m_Plant_C.flThick_Pc_CoatLimit;

                    }
                    if (m_flWc < _fl_Wc) m_flWc = _fl_Wc;
                }
                //3 是否有不合格
                _bl_Alarm |= m_Coat_DLL.m_Arr_RemoteIpPort[i].blAlarm;
            }

            //2 平均数据
            if (m_Coat_DLL.m_iRomoteNum > 1 && _bl_Alarm == false)
            {
                //1 小于最小厚度值
                if (m_SysInfo_C.m_Plant_C.flThick_Min_CoatLimit > 0)
                {
                    _fl_Wc = m_SysInfo_C.m_Plant_C.flThick_Min_CoatLimit - flThickness;
                    if (_fl_Wc > 0)//减薄
                        _bl_Alarm |= _fl_Wc > m_SysInfo_C.m_Plant_C.flThick_Pc_CoatLimit;
                    if (m_flWc < _fl_Wc) m_flWc = _fl_Wc;
                }
                //2  大于最大厚度值
                if (m_SysInfo_C.m_Plant_C.flThick_Max_CoatLimit > 0 && _bl_Alarm == false)
                {
                    _fl_Wc = flThickness - m_SysInfo_C.m_Plant_C.flThick_Max_CoatLimit;
                    if (_fl_Wc > 0)//增厚
                        _bl_Alarm |= _fl_Wc > m_SysInfo_C.m_Plant_C.flThick_Pc_CoatLimit;
                    if (m_flWc < _fl_Wc) m_flWc = _fl_Wc;
                }
            }
            //3 颜色设置
            //待填写颜色
            if (flThickness < m_SysInfo_C.m_Plant_C.flThick_Min_CoatLimit)
                m_SysInfo_C.m_Plant_C.CalculThickColor_Coat_JB(flThickness, out R, out G, out B);
            else if (flThickness >= m_SysInfo_C.m_Plant_C.flThick_Min_CoatLimit &&
                    flThickness <= m_SysInfo_C.m_Plant_C.flThick_Max_CoatLimit)
                m_SysInfo_C.m_Plant_C.CalculThickColor_Coat_MD(flThickness, out R, out G, out B);
            else if (flThickness > m_SysInfo_C.m_Plant_C.flThick_Max_CoatLimit)
                m_SysInfo_C.m_Plant_C.CalculThickColor_Coat_ZH(flThickness, out R, out G, out B);

            #endregion 误差计算

            strColor = R.ToString() + "/" + G.ToString() + "/" + B.ToString();
            m_SysInfo_C.m_C_One_Buff_Coat[m_i_X_Buff_No_Coat].Arr_Emat[i_Y_No].blUse = true;
            m_SysInfo_C.m_C_One_Buff_Coat[m_i_X_Buff_No_Coat].i_X_mm = i_Trip_Com_mm;
            m_SysInfo_C.m_C_One_Buff_Coat[m_i_X_Buff_No_Coat].Arr_Emat[i_Y_No].strColor = strColor;
            m_SysInfo_C.m_C_One_Buff_Coat[m_i_X_Buff_No_Coat].Arr_Emat[i_Y_No].R = R;
            m_SysInfo_C.m_C_One_Buff_Coat[m_i_X_Buff_No_Coat].Arr_Emat[i_Y_No].G = G;
            m_SysInfo_C.m_C_One_Buff_Coat[m_i_X_Buff_No_Coat].Arr_Emat[i_Y_No].B = B;
            #endregion  3.1

            #region 3.1.2异常数据统计
            if (_bl_Alarm)
            {
                try
                {
                    if (m_SysInfo_C.m_One_Alarm_Buff_Coat.Count == 0)//  m_Alarm.i_X_S == -1)//第一个缺陷
                    {
                        CL_AlarmData _Alarm = new CL_AlarmData();
                        _Alarm.i_X_S = i_Trip_Com_mm;
                        _Alarm.i_X_E = i_Trip_Com_mm;
                        _Alarm.fl_Wc = m_flWc;
                        _Alarm.i_Y_S = i_Y_No;
                        _Alarm.i_Y_X = i_Trip_Com_mm;

                        _Alarm.fl_Min_Thick = flThickness;
                        m_SysInfo_C.m_One_Alarm_Buff_Coat.Add(_Alarm);
                        m_i_Alarm_Num_Coat++;
                    }
                    else if (
                        m_SysInfo_C.m_One_Alarm_Buff_Coat.Count > 0 &&
                                               Math.Abs(i_Trip_Com_mm - m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num_Coat].i_X_E) <=
                                               m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X)// 5) //只统计X轴方向
                    {
                        //   m_i_Alarm_Num_Coat = m_SysInfo_C.m_One_Alarm_Buff_Coat.Count - 1;
                        if (i_Trip_Com_mm != m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num_Coat].i_X_E)
                        {
                            m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num_Coat].i_X_E = i_Trip_Com_mm;

                            if (m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num_Coat].fl_Wc < m_flWc)
                            {
                                m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num_Coat].fl_Wc = m_flWc;
                                m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num_Coat].fl_Min_Thick = flThickness;
                                m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num_Coat].i_Y_S = i_Y_No;
                                m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num_Coat].i_Y_X = i_Trip_Com_mm;
                            }
                        }
                        else if (i_Trip_Com_mm == m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num_Coat].i_X_E)
                        {
                            if (m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num_Coat].fl_Wc < m_flWc)
                            {
                                m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num_Coat].fl_Wc = m_flWc;
                                m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num_Coat].fl_Min_Thick = flThickness;
                                m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num_Coat].i_Y_S = i_Y_No;
                                m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num_Coat].i_Y_X = i_Trip_Com_mm;
                            }
                        }
                    }
                    else //X方向大于5mm
                    {
                        //数据整理
                        if (m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num_Coat].i_X_S > m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num_Coat].i_X_E)
                        {
                            int _id = m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num_Coat].i_X_S;
                            m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num_Coat].i_X_S = m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num_Coat].i_X_E;
                            m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num_Coat].i_X_E = _id;
                        }
                        //新区间
                        CL_AlarmData _Alarm = new CL_AlarmData();
                        _Alarm.i_X_S = i_Trip_Com_mm;
                        _Alarm.i_X_E = i_Trip_Com_mm;
                        _Alarm.fl_Wc = m_flWc;
                        _Alarm.i_Y_S = i_Y_No;
                        _Alarm.i_Y_X = i_Trip_Com_mm;
                        _Alarm.fl_Min_Thick = flThickness;

                        m_SysInfo_C.m_One_Alarm_Buff_Coat.Add(_Alarm);
                        m_i_Alarm_Num_Coat++;
                    }
                }
                catch (Exception eAlarm)
                { }
            }
            #endregion 3.1.2

            #region 3.2  计算区间显示值  f10探头，距离超过50mmmm
            //  if (m_SysInfo_C.m_C_One_Buff_Coat[m_i_X_Buff_No_Coat].Arr_C_Data[m_i_Y_ShowBuff_No_Coat].flWc < m_flWc)
            {
                if (m_SysInfo_C.m_C_One_Buff_Coat[m_i_X_Buff_No_Coat].bl_Discharge == false && m_Climb4.m_SysBuf.bl_Discharge)
                    m_SysInfo_C.m_C_One_Buff_Coat[m_i_X_Buff_No_Coat].bl_Discharge = true ;
                m_Climb4.m_SysBuf.bl_Discharge = false;

                if (m_SysInfo_C.m_C_One_Buff_Coat[m_i_X_Buff_No_Coat].bl_Discharge_To_Mark == false && m_Climb4.m_SysBuf.bl_Discharge_To_Mark)
                    m_SysInfo_C.m_C_One_Buff_Coat[m_i_X_Buff_No_Coat].bl_Discharge_To_Mark = true ;
                m_Climb4.m_SysBuf.bl_Discharge_To_Mark = false;

                m_SysInfo_C.m_C_One_Buff_Coat[m_i_X_Buff_No_Coat].Arr_C_Data[m_i_Y_ShowBuff_No_Coat].m_i_X = i_Trip_Com_mm;
                m_SysInfo_C.m_C_One_Buff_Coat[m_i_X_Buff_No_Coat].Arr_C_Data[m_i_Y_ShowBuff_No_Coat].blUse = true;
                m_SysInfo_C.m_C_One_Buff_Coat[m_i_X_Buff_No_Coat].Arr_C_Data[m_i_Y_ShowBuff_No_Coat].flThick = flThickness;
                m_SysInfo_C.m_C_One_Buff_Coat[m_i_X_Buff_No_Coat].Arr_C_Data[m_i_Y_ShowBuff_No_Coat].str_Mul_Thick = _strMulThic;
                m_SysInfo_C.m_C_One_Buff_Coat[m_i_X_Buff_No_Coat].Arr_C_Data[m_i_Y_ShowBuff_No_Coat].blAlarm = _bl_Alarm;
                m_SysInfo_C.m_C_One_Buff_Coat[m_i_X_Buff_No_Coat].Arr_C_Data[m_i_Y_ShowBuff_No_Coat].strColor = strColor;

                m_SysInfo_C.m_C_One_Buff_Coat[m_i_X_Buff_No_Coat].Arr_C_Data[m_i_Y_ShowBuff_No_Coat].fl_Spark_Val = m_SysInfo_C.m_Ctrl.fl_Spark_Val;

                m_SysInfo_C.m_C_One_Buff_Coat[m_i_X_Buff_No_Coat].Arr_C_Data[m_i_Y_ShowBuff_No_Coat].R = R;
                m_SysInfo_C.m_C_One_Buff_Coat[m_i_X_Buff_No_Coat].Arr_C_Data[m_i_Y_ShowBuff_No_Coat].G = G;
                m_SysInfo_C.m_C_One_Buff_Coat[m_i_X_Buff_No_Coat].Arr_C_Data[m_i_Y_ShowBuff_No_Coat].B = B;

                m_SysInfo_C.m_C_One_Buff_Coat[m_i_X_Buff_No_Coat].Arr_C_Data[m_i_Y_ShowBuff_No_Coat].flWc = m_flWc;
                m_SysInfo_C.m_C_One_Buff_Coat[m_i_X_Buff_No_Coat].Arr_C_Data[m_i_Y_ShowBuff_No_Coat].iArr_ShowNo = i_Y_No;
            }
            #endregion 3.2
            #endregion 3

            Cdww:
            int dd = 0;
        }
        public static List<Color> GetSingleColorList(Color srcColor, Color desColor, int count)
        {
            List<Color> colorFactorList = new List<Color>();
            int redSpan = desColor.R - srcColor.R;
            int greenSpan = desColor.G - srcColor.G;
            int blueSpan = desColor.B - srcColor.B;
            for (int i = 0; i < count; i++)
            {
                Color color = Color.FromArgb(
                    srcColor.R + (int)((double)i / count * redSpan),
                    srcColor.G + (int)((double)i / count * greenSpan),
                    srcColor.B + (int)((double)i / count * blueSpan)
                );
                colorFactorList.Add(color);
            }
            return colorFactorList;
        }
//————————————————
//版权声明：本文为CSDN博主「羽木落凡」的原创文章，遵循CC 4.0 BY-SA版权协议，转载请附上原文出处链接及本声明。
//原文链接：https://blog.csdn.net/wsadcg/article/details/102860725
        /// <summary>
        /// 脉冲涡流数据
        /// </summary>
        /// <param name="flThickness"></param>
        /// <param name="ArrWave"></param>
        private unsafe void Get_Data(float flThickness, float [] ArrWave)
        {
            if (m_SysInfo_C.m_Ctrl.m_iRun != 1) return;

            int _iArr_Len = ArrWave.Length;//波形数组长度

            if (m_SysInfo_C.m_Ctrl.m_bl_CS)
            {
                if (m_Climb4.m_SysBuf.blGsb_RunFx_L_to_R)
                    m_Climb4.m_SysBuf.i_Para_Wz++;
                else
                    m_Climb4.m_SysBuf.i_Para_Wz--;

                if (m_Climb4.m_SysBuf.i_Para_Wz > m_Climb4.m_SysBuf.iGsb_Len)
                {
                    m_Climb4.m_SysBuf.i_Para_Wz = m_SysBuff.m_Climb.i_Gsb_End_Pos;
                    m_Climb4.m_SysBuf.blGsb_RunFx_L_to_R = !m_Climb4.m_SysBuf.blGsb_RunFx_L_to_R;
                    m_Climb4.m_SysBuf.Trip_Com_mm += m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X;
                    m_Climb4.m_SysBuf.Trip = m_Climb4.m_SysBuf.Trip_Com_mm / 1000F;
                }
                if (m_Climb4.m_SysBuf.i_Para_Wz == -1)
                {
                    m_Climb4.m_SysBuf.i_Para_Wz = 0;

                    m_Climb4.m_SysBuf.blGsb_RunFx_L_to_R = !m_Climb4.m_SysBuf.blGsb_RunFx_L_to_R;
                    m_Climb4.m_SysBuf.Trip_Com_mm += m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X;
                }
            }
            i_Trip_Com_mm = m_Climb4.m_SysBuf.Trip_Com_mm;
            //1.3 光栅臂对应位置
            i_Y_No = m_Climb4.m_SysBuf.i_Para_Wz;

            int _iLst_Buff_Len = m_SysInfo_C.m_C_One_Buff.Count;

            #region 1 计算车体位置对应的XY位置
            //1.1 由实际点计算数据对应在缓存中开始序号
            m_flDat = (i_Trip_Com_mm - m_Climb4.m_SysBuf.Trip_Com_Init_mm) / m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X;
            m_i_X_Buff_No = (int)m_flDat;//
            if (m_flDat - m_i_X_Buff_No >= 0.5) m_i_X_Buff_No++;

            //1.2 由实际点计算数据对应在当前屏幕中开始序号
            m_flDat = ((i_Trip_Com_mm - m_SysInfo_C.m_Plant_C.i_Screen_Start_Distance) / m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X);
            m_iCurr_X = (int)m_flDat;//
            if (m_flDat - m_iCurr_X >= 0.5) m_iCurr_X++;
            m_i_X_Screen_No = m_iCurr_X;

            //1.4 计算光栅臂对应显示位置
            m_flDat = 1.0f * i_Y_No / m_SysInfo_C.m_Plant_C.Scree_iDotHeightmm_Y;
            m_iCurr_Y = (int)m_flDat;
    
            //int _iLen = m_SysInfo_C.m_Plant_C.iGsb_Len /
            //                    m_SysInfo_C.m_Plant_C.Scree_iDotHeightmm_Y;

            float _ftLen = 1.0f * m_SysInfo_C.m_Plant_C.iGsb_Len /
                                m_SysInfo_C.m_Plant_C.Scree_iDotHeightmm_Y;
            int _iLen = (int)_ftLen;

            if (m_flDat - m_iCurr_Y == 0 && m_iCurr_Y >= _iLen) m_iCurr_Y--;

            m_i_Y_ShowBuff_No = m_iCurr_Y;
            #endregion 1

            #region 2 添加新列数据
            if (m_i_X_Buff_No > m_SysInfo_C.m_Ctrl.i_Buff_Len)//数据前进不会跨区域
            {
                CLs_EMAT_Data _Cls_Data = new CLs_EMAT_Data(
                                m_SysInfo_C.m_Plant_C.iGsb_Len + 1, _iLen+2);
                _Cls_Data.i_X_mm = i_Trip_Com_mm;

                for (int i = 0; i < _Cls_Data.Arr_C_Data.Count(); i++)
                    _Cls_Data.Arr_C_Data[i] = new Cls_EMAT_2();
                for (int i = 0; i < _Cls_Data.Arr_Emat.Count(); i++)
                {
                    _Cls_Data.Arr_Emat[i] = new Cls_EMAT_1(_iArr_Len);

                }
                m_SysInfo_C.m_C_One_Buff.Add(_Cls_Data);

                m_SysInfo_C.m_Ctrl.i_Buff_Len = m_SysInfo_C.m_C_One_Buff.Count - 1;//比长度小1，目的是和序号对应0-N
            }
            #endregion 2
            #region 3 数据保存
            #region 3.1 原始缓存   数据存储在：车体位置对应X/Y方向对应的缓存列号中
            m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_Emat[i_Y_No].flThick = flThickness;
            m_flWc = Math.Abs(float.Parse(Math.Abs(flThickness - m_SysInfo_C.m_Plant_C.flNormal_Thickness).ToString("f2")));//计算误差
            float _flBfz = m_flWc / m_SysInfo_C.m_Plant_C.flNormal_Thickness;
            float _flLimt = m_SysInfo_C.m_Plant_C.fl_Max_Limit / 100.0f;

            if (_flBfz <= _flLimt || m_SysInfo_C.m_Plant_C.i_Alarm == 0)
            {
                blAlarm = m_flWc >= m_SysInfo_C.m_Plant_C.flstrThickAlarm;
                m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_Emat[i_Y_No].blAlarm = blAlarm;

                m_SysInfo_C.m_Plant_C.CalculThickColor(m_flWc, out R, out G, out B, 0, m_SysInfo_C.m_Plant_C.Ck_No_Normal_Thickness);
            }
            else
            {
                blAlarm = false;
                m_flWc = 100;
                m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_Emat[i_Y_No].blAlarm = false;
                //   m_SysInfo_C.m_Plant_C.CalculThickColor(m_flWc, out R, out G, out B, 0, m_SysInfo_C.m_Plant_C.Ck_No_Normal_Thickness);
              //  R = 255; G = 245; B = 0;
                R = m_SysInfo_C.m_Plant_C.i_Alarm_Out_R; //255;
                G = m_SysInfo_C.m_Plant_C.i_Alarm_Out_G; //245;
                B = m_SysInfo_C.m_Plant_C.i_Alarm_Out_B;//0

            }
            strColor = R.ToString() + "/" + G.ToString() + "/" + B.ToString();
            m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_Emat[i_Y_No].strColor = strColor;
            m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_Emat[i_Y_No].R = R;
            m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_Emat[i_Y_No].G = G;
            m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_Emat[i_Y_No].B = B;

            if (m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_Emat[i_Y_No].flArrWave == null)
                m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_Emat[i_Y_No].flArrWave = new float [_iArr_Len];
            var IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_Emat[i_Y_No].flArrWave, 0);
            Marshal.Copy(ArrWave, 0, IntPtArr, _iArr_Len);//注意数据范围

            m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_Emat[i_Y_No].iWaveStandNo = m_ECT_My_DLL.m_ArrECT_Cali[0].iWaveStandNo;
            #endregion  3.1

            #region 3.1.2异常数据统计
            if (blAlarm)
            {
                if (m_SysInfo_C.m_One_Alarm_Buff.Count == 0)//  m_Alarm.i_X_S == -1)//第一个缺陷
                {
                    CL_AlarmData _Alarm = new CL_AlarmData();
                    _Alarm.i_X_S = i_Trip_Com_mm;
                    _Alarm.i_X_E = i_Trip_Com_mm;
                    _Alarm.fl_Wc = m_flWc;
                    _Alarm.i_Y_S = i_Y_No;
                    _Alarm.i_Y_X = i_Trip_Com_mm;

                    _Alarm.fl_Min_Thick = flThickness;
                    m_SysInfo_C.m_One_Alarm_Buff.Add(_Alarm);
                    m_i_Alarm_Num++;
                }
                else if (Math.Abs(i_Trip_Com_mm - m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_E) <= m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X)// 5) //只统计X轴方向
                {
                    if (i_Trip_Com_mm != m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_E)
                    {
                        m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_E = i_Trip_Com_mm;

                        if (m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].fl_Wc < m_flWc)
                        {
                            m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].fl_Wc = m_flWc;
                            m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].fl_Min_Thick = flThickness;
                            m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_Y_S = i_Y_No;
                            m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_Y_X = i_Trip_Com_mm;
                        }
                    }
                    else if (i_Trip_Com_mm == m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_E)
                    {
                        if (m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].fl_Wc < m_flWc)
                        {
                            m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].fl_Wc = m_flWc;
                            m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].fl_Min_Thick = flThickness;
                            m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_Y_S = i_Y_No;
                            m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_Y_X = i_Trip_Com_mm;
                        }
                    }
                }
                else //X方向大于5mm
                {
                    //数据整理
                    if (m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_S > m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_E)
                    {
                        int _id = m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_S;
                        m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_S = m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_E;
                        m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_E = _id;
                    }
                    //新区间
                    CL_AlarmData _Alarm = new CL_AlarmData();
                    _Alarm.i_X_S = i_Trip_Com_mm;
                    _Alarm.i_X_E = i_Trip_Com_mm;
                    _Alarm.fl_Wc = m_flWc;
                    _Alarm.i_Y_S = i_Y_No;
                    _Alarm.i_Y_X = i_Trip_Com_mm;
                    _Alarm.fl_Min_Thick = flThickness;

                    m_SysInfo_C.m_One_Alarm_Buff.Add(_Alarm);
                    m_i_Alarm_Num++;
                }
            }
            #endregion 3.1.2

            #region 3.2  计算区间显示值
            if (m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].flWc < m_flWc)
            {
                m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].flThick = flThickness;
                m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].blAlarm = blAlarm;
                m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].strColor = strColor;
                m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].R = R;
                m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].G = G;
                m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].B = B;

                m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].flWc = m_flWc;
                m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].iArr_ShowNo = i_Y_No;

                if (m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].flArrWave == null)
                    m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].flArrWave = new float [_iArr_Len];
                var IntPtArr_S = Marshal.UnsafeAddrOfPinnedArrayElement(m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].flArrWave, 0);
                Marshal.Copy(ArrWave, 0, IntPtArr_S, _iArr_Len);//注意数据范围
                m_SysInfo_C.m_C_One_Buff[m_i_X_Buff_No].Arr_C_Data[m_i_Y_ShowBuff_No].iWaveStandNo = m_ECT_My_DLL.m_ArrECT_Cali[0].iWaveStandNo;
            }
            #endregion 3.2

            //    Chang_Posit();

            #endregion 3
        }
        /// <summary>
        /// 多通道测量数据实时数据刷新
        /// </summary>
        /// <param name="iAll_Num"></param>
        /// <param name="_iArr_Len"></param>
        /// <param name="_Arr_In"></param>
        /// <param name="_Orig_Out"></param>
        private void Mody_Mul_CurrData(int iAll_Num,int _iArr_Len, IpEndPoint_Struct[] _Arr_In,bool blNew, ref Cls_EMAT_1[] Arr_Emat)
        {
            var IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_Arr_In[0].m_btArrWave, 0);

            for (int _iR = 0; _iR < iAll_Num; _iR++)//多通道序号
            {
                try
                {
                    if (blNew)
                        Arr_Emat[_iR] = new Cls_EMAT_1(_iArr_Len);

                    Arr_Emat[_iR].blUse = _Arr_In[_iR].blUse;
                    //如果当前通道使用
                    if (_Arr_In[_iR].blUse == false) continue;

                    Arr_Emat[_iR].flThick = _Arr_In[_iR].m_flThick;
                    Arr_Emat[_iR].iGain = _Arr_In[_iR].iGain;//通道增益值
                    Arr_Emat[_iR].blAlarm = _Arr_In[_iR].blAlarm;//报警

                    Arr_Emat[_iR].strColor = _Arr_In[_iR].strColor;//颜色值
                    Arr_Emat[_iR].R = _Arr_In[_iR].R;
                    Arr_Emat[_iR].G = _Arr_In[_iR].G;
                    Arr_Emat[_iR].B = _Arr_In[_iR].B;
                    IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(Arr_Emat[_iR].btArrWave, 0);
                    Marshal.Copy(_Arr_In[_iR].m_btArrWave, 0, IntPtArr, _iArr_Len);//注意数据范围

                    #region 间隔距离区间的需要在界面显示的最大误差数据
                    //Arr_Emat[_iR].Ori_flThick = m_Arr_C_Data[_iR].flThick;
                    //Arr_Emat[_iR].Ori_R = m_Arr_C_Data[_iR].R;
                    //Arr_Emat[_iR].Ori_G = m_Arr_C_Data[_iR].G;
                    //Arr_Emat[_iR].Ori_B = m_Arr_C_Data[_iR].B;
                    #endregion
                }
                catch (Exception ee)
                { }
            }
        }
        private void Mody_Mul_CurrData_ECT(int iAll_Num, bool blNew, ref Cls_EMAT_1[] Arr_Emat)
        {
            var IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(m_ECT_My_DLL.m_ArrECT_Buff[0].m_iArrWave, 0);

            for (int _iR = 0; _iR < iAll_Num; _iR++)//多通道序号
            {
                try
                {
                    if (blNew)
                        Arr_Emat[_iR] = new Cls_EMAT_1(m_ECT_My_DLL.m_iArr_Len);

                    Arr_Emat[_iR].blUse = m_SysInfo_C.m_Ctrl.m_bl_CS?true: m_ECT_My_DLL.m_ArrECT_Buff[_iR].blLink;
                    //如果当前通道使用
                    if (Arr_Emat[_iR].blUse == false && !m_SysInfo_C.m_Ctrl.m_bl_CS) continue;
                  
                    Arr_Emat[_iR].iWaveStandNo = m_ECT_My_DLL.m_ArrECT_Cali[0].iWaveStandNo;
                    Arr_Emat[_iR].flThick = m_ECT_My_DLL.m_ArrECT_Buff[_iR].flThick;

                    Arr_Emat[_iR].blAlarm = m_ECT_My_DLL.m_ArrECT_Buff[_iR].blAlarm;//报警

                    Arr_Emat[_iR].strColor = m_ECT_My_DLL.m_ArrECT_Buff[_iR].strColor;//颜色值
                    Arr_Emat[_iR].R = m_ECT_My_DLL.m_ArrECT_Buff[_iR].R;
                    Arr_Emat[_iR].G = m_ECT_My_DLL.m_ArrECT_Buff[_iR].G;
                    Arr_Emat[_iR].B = m_ECT_My_DLL.m_ArrECT_Buff[_iR].B;

                    IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(Arr_Emat[_iR].flArrWave, 0);
                    Marshal.Copy(m_ECT_My_DLL.m_ArrECT_Buff[_iR].m_iArrWave, 0, IntPtArr, m_ECT_My_DLL.m_iArr_Len);//注意数据范围

                    Arr_Emat[_iR].strArrWave = m_ECT_My_DLL.m_ArrECT_Buff[_iR].strWave;

                    #region 间隔距离区间的需要在界面显示的最大误差数据
                    //Arr_Emat[_iR].Ori_flThick = m_Arr_C_Data[_iR].flThick;
                    //Arr_Emat[_iR].Ori_R = m_Arr_C_Data[_iR].R;
                    //Arr_Emat[_iR].Ori_G = m_Arr_C_Data[_iR].G;
                    //Arr_Emat[_iR].Ori_B = m_Arr_C_Data[_iR].B;
                    //Arr_Emat[_iR].Ori_blAlarm = m_Arr_C_Data[_iR].blAlarm;
                    //Arr_Emat[_iR].Ori_flWc = m_Arr_C_Data[_iR].flWc;
                    //Arr_Emat[_iR].Ori_strArrWave = m_Arr_C_Data[_iR].strArrWave ;
                    #endregion
                }
                catch (Exception ee)
                { }
            }
        }

        private void Mody_Mul_CurrData_Coat(int iAll_Num, int _iArr_Len, IpEndPoint_Struct[] _Arr_In, bool blNew, ref Cls_EMAT_1[] Arr_Emat)
        {
            var IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_Arr_In[0].m_btArrWave, 0);

            for (int _iR = 0; _iR < iAll_Num; _iR++)//多通道序号
            {
                try
                {
                    if (blNew)
                        Arr_Emat[_iR] = new Cls_EMAT_1(_iArr_Len);

                    Arr_Emat[_iR].blUse = _Arr_In[_iR].blUse;
                    //如果当前通道使用
                    if (_Arr_In[_iR].blUse == false) continue;

                    Arr_Emat[_iR].flThick = _Arr_In[_iR].m_flThick;
                    Arr_Emat[_iR].iGain = _Arr_In[_iR].iGain;//通道增益值
                    Arr_Emat[_iR].blAlarm = _Arr_In[_iR].blAlarm;//报警

                    Arr_Emat[_iR].strColor = _Arr_In[_iR].strColor;//颜色值
                    Arr_Emat[_iR].R = _Arr_In[_iR].R;
                    Arr_Emat[_iR].G = _Arr_In[_iR].G;
                    Arr_Emat[_iR].B = _Arr_In[_iR].B;
                    IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(Arr_Emat[_iR].btArrWave, 0);
                    Marshal.Copy(_Arr_In[_iR].m_btArrWave, 0, IntPtArr, _iArr_Len);//注意数据范围

                    #region 间隔距离区间的需要在界面显示的最大误差数据
                    //Arr_Emat[_iR].Ori_flThick = m_Arr_C_Data_Coat[_iR].flThick;
                    //Arr_Emat[_iR].Ori_R = m_Arr_C_Data_Coat[_iR].R;
                    //Arr_Emat[_iR].Ori_G = m_Arr_C_Data_Coat[_iR].G;
                    //Arr_Emat[_iR].Ori_B = m_Arr_C_Data_Coat[_iR].B;
                    #endregion
                }
                catch (Exception ee)
                { }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public float m_fl_Max_Limit = 0;

        /// <summary>
        /// 多通道数据处理
        /// </summary>
        /// <param name="iAll_Num">通道数</param>
        private void Get_Data_Mul_UI(int iAll_Num)
        {
            if (m_SysInfo_C.m_Ctrl.m_iRun != 1) return;
            //     m_SysInfo_C.m_Ctrl.m_bl_CS = false ;
            #region 测试数据 
            int _iArr_Len = 1000;
            //        m_SysInfo_C.m_Ctrl.m_bl_CS = true;
            if (m_SysInfo_C.m_Ctrl.m_bl_CS)
            {
                #region 模拟距离
                if (m_Climb4.m_SysBuf.iRun_Gsb_Qj1_Ht0 == 1)
                    m_Climb4.m_SysBuf.Trip_Com_mm += m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X;
                else
                    m_Climb4.m_SysBuf.Trip_Com_mm -= m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X;
                if (m_Climb4.m_SysBuf.Trip_Com_mm < 0) m_Climb4.m_SysBuf.Trip_Com_mm = 0;

                m_Climb4.m_SysBuf.Trip = m_Climb4.m_SysBuf.Trip_Com_mm / 1000F;
                #endregion
            }
             //else

            #endregion 测试数据


     //      m_Climb4.m_SysBuf.Trip_Com_mm = (int)(m_Climb4.m_SysBuf.Trip * 1000f);
            i_Trip_Com_mm = m_Climb4.m_SysBuf.Trip_Com_mm;

            #region 1 拿多通道数据
            IpEndPoint_Struct[] _Arr_RemoteIpPort = new IpEndPoint_Struct[iAll_Num];
            var IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(m_UT_My_DLL.m_Arr_RemoteIpPort[0].m_btArrWave, 0);
            float _flBfz = 0, _fl_Channel_WcMax = 0;
            float _flThickness = 0;
            bool _blAlarm = false, _blT = false;
            int _i_Channel = -1;

            m_fl_Max_Limit = m_SysInfo_C.m_Plant_C.fl_Max_Limit / 100.0f;

            for (int i = 0; i < iAll_Num; i++)
            {
                _Arr_RemoteIpPort[i].blUse = m_UT_My_DLL.m_Arr_RemoteIpPort[i].blUse;
                if (_Arr_RemoteIpPort[i].blUse == false) continue;

                _Arr_RemoteIpPort[i].iGain = m_UT_My_DLL.m_Arr_RemoteIpPort[i].iGain;
                _Arr_RemoteIpPort[i].m_flThick = m_UT_My_DLL.m_Arr_RemoteIpPort[i].m_flThick;

                #region 误差计算

                //   m_flWc = float.Parse(Math.Abs(_Arr_RemoteIpPort[i].m_flThick - m_SysInfo_C.m_Plant_C.flNormal_Thickness).ToString("f2"));//计算误差
                m_flWc = float.Parse((m_SysInfo_C.m_Plant_C.flNormal_Thickness - _Arr_RemoteIpPort[i].m_flThick).ToString("f2"));//计算误差
                _flBfz = Math.Abs(m_flWc / m_SysInfo_C.m_Plant_C.flNormal_Thickness);

                if (_flBfz <= m_fl_Max_Limit)
                {
                    _blT = Math.Abs(m_flWc) >= m_SysInfo_C.m_Plant_C.flstrThickAlarm;

                    if (m_flWc < 0 && _blT)
                    {
                        _blT = false;
                        m_flWc = 100;
                    }

                    _Arr_RemoteIpPort[i].blAlarm = _blT;

                    if (_blT && Math.Abs(_fl_Channel_WcMax) < Math.Abs(m_flWc))
                    {
                        _fl_Channel_WcMax = m_flWc;
                        _i_Channel = i; _blAlarm = true;
                        _flThickness = _Arr_RemoteIpPort[i].m_flThick;
                    }
                    if (m_flWc > 0)//减薄
                        m_SysInfo_C.m_Plant_C.CalculThickColor(m_flWc, out R, out G, out B, 0, m_SysInfo_C.m_Plant_C.Ck_No_Normal_Thickness);
                    else//增厚
                        m_SysInfo_C.m_Plant_C.CalculThickColor_A(Math.Abs(m_flWc), out R, out G, out B, 0, m_SysInfo_C.m_Plant_C.Ck_No_Normal_Thickness);
                }
                else
                {
                    m_flWc = 100;
                    _Arr_RemoteIpPort[i].blAlarm = false;
                    R = m_SysInfo_C.m_Plant_C.i_Alarm_Out_R; //255;
                    G = m_SysInfo_C.m_Plant_C.i_Alarm_Out_G; //245;
                    B = m_SysInfo_C.m_Plant_C.i_Alarm_Out_B;//0
                }
                _Arr_RemoteIpPort[i].flWc = m_flWc;
                _Arr_RemoteIpPort[i].strColor = R.ToString() + "/" + G.ToString() + "/" + B.ToString();
                _Arr_RemoteIpPort[i].R = R;
                _Arr_RemoteIpPort[i].G = G;
                _Arr_RemoteIpPort[i].B = B;
                #endregion

                _Arr_RemoteIpPort[i].m_btArrWave = new byte[1000];

                if (m_UT_My_DLL.m_Arr_RemoteIpPort[i].m_btArrWave == null)
                    m_UT_My_DLL.m_Arr_RemoteIpPort[i].m_btArrWave = new byte[1000];
                if (m_UT_My_DLL.m_Arr_RemoteIpPort[i].m_iArrWave == null)
                    m_UT_My_DLL.m_Arr_RemoteIpPort[i].m_iArrWave = new int[1000];

                IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_Arr_RemoteIpPort[i].m_btArrWave, 0);//   
                Marshal.Copy(m_UT_My_DLL.m_Arr_RemoteIpPort[i].m_btArrWave, 0, IntPtArr, _iArr_Len);

                _Arr_RemoteIpPort[i].m_iArrWave = new int[1000];
                IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_Arr_RemoteIpPort[i].m_iArrWave, 0);//   
                Marshal.Copy(m_UT_My_DLL.m_Arr_RemoteIpPort[i].m_iArrWave, 0, IntPtArr, _iArr_Len);


                #region 3.1.2异常数据统计

                #endregion 3.1.2
            }
            #endregion 1

            if (_blAlarm)
            {
                if (m_SysInfo_C.m_One_Alarm_Buff.Count == 0)//  m_Alarm.i_X_S == -1)//第一个缺陷
                {
                    CL_AlarmData _Alarm = new CL_AlarmData();
                    _Alarm.i_X_S = i_Trip_Com_mm;
                    _Alarm.i_X_E = i_Trip_Com_mm;
                    _Alarm.fl_Wc = _fl_Channel_WcMax;
                    _Alarm.i_Y_S = _i_Channel;
                    _Alarm.i_Y_X = _i_Channel;

                    _Alarm.fl_Min_Thick = _flThickness;
                    m_SysInfo_C.m_One_Alarm_Buff.Add(_Alarm);
                    m_i_Alarm_Num++;
                }
                else if (Math.Abs(i_Trip_Com_mm - m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_E) <= m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X)
                {
                    m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_E = i_Trip_Com_mm;
                    m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].fl_Wc = _fl_Channel_WcMax;
                    m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_Y_S = _i_Channel;
                    m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].fl_Min_Thick = _flThickness;
                }
                else
                {
                    //数据整理
                    if (m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_S > m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_E)
                    {
                        int _id = m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_S;
                        m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_S = m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_E;
                        m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_E = _id;
                    }
                    //新区间
                    CL_AlarmData _Alarm = new CL_AlarmData();
                    _Alarm.i_X_S = i_Trip_Com_mm;
                    _Alarm.i_X_E = i_Trip_Com_mm;
                    _Alarm.fl_Wc = _fl_Channel_WcMax;
                    _Alarm.i_Y_S = _i_Channel;
                    _Alarm.i_Y_X = _i_Channel;
                    _Alarm.fl_Min_Thick = _flThickness;

                    m_SysInfo_C.m_One_Alarm_Buff.Add(_Alarm);
                    m_i_Alarm_Num++;
                }
            }
            #region 2 拿距离，计算显示位置
            //X轴线性增加（地址不变只覆盖数据），显示缓存计算位置

            //      m_flThickness = (float)m_Climb4.m_SysBuf.Trip;

            //1 计算显示缓存链表的位置
            //1.1 由实际点计算数据对应在缓存中开始序号

            m_flDat = Math.Abs((i_Trip_Com_mm - m_Climb4.m_SysBuf.Trip_Com_Init_mm) * 1.0f /(1.0f* m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X));
            m_i_X_Buff_No = (int)m_flDat;//
            if (m_flDat - m_i_X_Buff_No >= 0.5) m_i_X_Buff_No++;
            if (m_i_X_Buff_No < 0) m_i_X_Buff_No = 0;

            //1.2 由实际点计算数据对应在当前屏幕中开始序号
            m_flDat = (1.0f* (i_Trip_Com_mm - m_SysInfo_C.m_Plant_C.i_Screen_Start_Distance) /( m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X)*1.0f);
            m_iCurr_X = (int)m_flDat;//
            if (m_flDat - m_iCurr_X >= 0.5) m_iCurr_X++;
            m_i_X_Screen_No = m_iCurr_X;
            if (m_i_X_Screen_No < 0) m_i_X_Screen_No = 0;
            #endregion 2

            #region 2 添加新列数据
            //2.1 原始数据缓存没有了
            if (m_i_X_Buff_No >= m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff.Count)
            {
                //增加3个显示缓存
                for (int i = 0; i < 300; i++)//---cdw----
                {
                    Cls_Mul_ShowBuff _ShowBuff = new Cls_Mul_ShowBuff(iAll_Num);
                    for (int _iR = 0; _iR < iAll_Num; _iR++)
                        _ShowBuff.Arr_C_Data[_iR] = new Cls_EMAT_2();

                    m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff.Add(_ShowBuff);
                }
            }
            #endregion 2

            #region 3
            //3.0 计算显示值
            for (int _iR = 0; _iR < iAll_Num; _iR++)
            {
                m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].blUse = _Arr_RemoteIpPort[_iR].blUse;
                //m_Arr_C_Data[_iR].blUse = _Arr_RemoteIpPort[_iR].blUse;

                if (_Arr_RemoteIpPort[_iR].blUse == false) continue;
                if (Math .Abs ( m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].flWc) <= Math .Abs ( _Arr_RemoteIpPort[_iR].flWc))
                {
                    m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].m_i_X = i_Trip_Com_mm;
                    m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].bl_BuffUse = true;
                    m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].m_i_X = i_Trip_Com_mm;
                    m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].flThick = _Arr_RemoteIpPort[_iR].m_flThick;
                    m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].blAlarm = _Arr_RemoteIpPort[_iR].blAlarm;
                    m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].strColor = _Arr_RemoteIpPort[_iR].strColor;
                    m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].R = _Arr_RemoteIpPort[_iR].R;
                    m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].G = _Arr_RemoteIpPort[_iR].G;
                    m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].B = _Arr_RemoteIpPort[_iR].B;

                    m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].flWc = _Arr_RemoteIpPort[_iR].flWc;

                    if (m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].btArrWave == null)
                        m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].btArrWave = new byte[_iArr_Len];
                    var IntPtArr_S = Marshal.UnsafeAddrOfPinnedArrayElement(m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].btArrWave, 0);
                    Marshal.Copy(_Arr_RemoteIpPort[_iR].m_btArrWave, 0, IntPtArr_S, _iArr_Len);

                    //m_Arr_C_Data[_iR].flThick = _Arr_RemoteIpPort[_iR].m_flThick;
                    //m_Arr_C_Data[_iR].blAlarm = _Arr_RemoteIpPort[_iR].blAlarm;
                    //m_Arr_C_Data[_iR].strColor = _Arr_RemoteIpPort[_iR].strColor;
                    //m_Arr_C_Data[_iR].R = _Arr_RemoteIpPort[_iR].R;
                    //m_Arr_C_Data[_iR].G = _Arr_RemoteIpPort[_iR].G;
                    //m_Arr_C_Data[_iR].B = _Arr_RemoteIpPort[_iR].B;
                    //m_Arr_C_Data[_iR].flWc = _Arr_RemoteIpPort[_iR].flWc;

                    //if (m_Arr_C_Data[_iR].btArrWave == null)
                    //    m_Arr_C_Data[_iR].btArrWave = new byte[_iArr_Len];
                    //var IntPtArr_d = Marshal.UnsafeAddrOfPinnedArrayElement(m_Arr_C_Data[_iR].btArrWave, 0);
                    //Marshal.Copy(_Arr_RemoteIpPort[_iR].m_iArrWave, 0, IntPtArr_d, _iArr_Len);
                }
            }

            //3.1 原始记录判断是否距离没有变化，
            if (m_Climb4.m_SysBuf.iRun_Gsb_Qj1_Ht0 == 1 && m_SysInfo_C.m_Ctrl.m_i_Trip_Com_mm_Old < i_Trip_Com_mm ||
                m_Climb4.m_SysBuf.iRun_Gsb_Qj1_Ht0 == 0 && (m_SysInfo_C.m_Ctrl.m_i_Trip_Com_mm_Old > i_Trip_Com_mm || m_SysInfo_C.m_Ctrl.m_i_Trip_Com_mm_Old == -1))//新位置，或者前进或者后退
            {
                Cls_Mul_Orig _Orig = new Cls_Mul_Orig(iAll_Num);
                //填充数据
                // _Orig.bl_BuffUse = true;
                _Orig.i_X_mm = i_Trip_Com_mm;
                #region 显示缓存
                //  _Orig.i_Screen_Col = m_i_X_Screen_No;

                #endregion
                Mody_Mul_CurrData(iAll_Num, _iArr_Len, _Arr_RemoteIpPort, true, ref _Orig.Arr_Emat);

                m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Ori_Data.Add(_Orig);//始终添加到最后，这样做目的是为了显示界面正常显示车体实时运行轨迹
                m_P_i_Len++;
                m_SysInfo_C.m_Ctrl.m_i_Trip_Com_mm_Old = i_Trip_Com_mm;
            }
            else if(m_P_i_Len>-1)//位置不变，只刷新数据
            {
                Mody_Mul_CurrData(iAll_Num, _iArr_Len, _Arr_RemoteIpPort, false, ref m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Ori_Data[m_P_i_Len].Arr_Emat);
            }
            #endregion 3
        }
        /// <summary>
        /// 涂层测厚
        /// </summary>
        /// <param name="iAll_Num"></param>
        private void Get_Data_Mul_Coat(int iAll_Num)
        {
            if (m_SysInfo_C.m_Ctrl.m_iRun != 1) return;
            //     m_SysInfo_C.m_Ctrl.m_bl_CS = false ;
            #region 测试数据 
            int _iArr_Len = 1000;
            if (m_SysInfo_C.m_Ctrl.m_bl_CS)
            {
                #region 模拟距离
                if (m_Climb4.m_SysBuf.iRun_Gsb_Qj1_Ht0 == 1)
                    m_Climb4.m_SysBuf.Trip_Com_mm += m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X;
                else
                    m_Climb4.m_SysBuf.Trip_Com_mm -= m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X;
                if (m_Climb4.m_SysBuf.Trip_Com_mm < 0) m_Climb4.m_SysBuf.Trip_Com_mm = 0;

                m_Climb4.m_SysBuf.Trip = m_Climb4.m_SysBuf.Trip_Com_mm / 1000F;
                #endregion
            }
            #endregion 测试数据
          
            //光栅臂位置
            i_Y_No = m_Climb4.m_SysBuf.i_Para_Wz;
            if (i_Y_No < 0) i_Y_No = 0;
            //1.4 计算光栅臂对应显示位置
            m_flDat = 1.0f * i_Y_No / m_SysInfo_C.m_Plant_C.Scree_iDotHeightmm_Y;
            m_iCurr_Y = (int)m_flDat;

           // int _iLen = m_SysInfo_C.m_Plant_C.iGsb_Len / m_SysInfo_C.m_Plant_C.Scree_iDotHeightmm_Y;

            float _ftLen = 1.0f * m_SysInfo_C.m_Plant_C.iGsb_Len /
                                m_SysInfo_C.m_Plant_C.Scree_iDotHeightmm_Y;
            int _iLen = (int)_ftLen;


            if (m_flDat - m_iCurr_Y == 0 && m_iCurr_Y >= _iLen) m_iCurr_Y--;

            m_i_Y_ShowBuff_No_Coat = m_iCurr_Y;
            if (m_i_Y_ShowBuff_No_Coat < 0) m_i_Y_ShowBuff_No_Coat = 0;

            //X轴位置
            m_Climb4.m_SysBuf.Trip_Com_mm = (int)(m_Climb4.m_SysBuf.Trip * 1000f);
            i_Trip_Com_mm = m_Climb4.m_SysBuf.Trip_Com_mm;

            #region 1 拿多通道数据
            IpEndPoint_Struct[] _Arr_RemoteIpPort = new IpEndPoint_Struct[iAll_Num];
            var IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(m_Coat_DLL.m_Arr_RemoteIpPort[0].m_btArrWave, 0);
            float _flBfz = 0, _fl_Channel_WcMax = 0;
            float _flThickness = 0;
            bool _blAlarm = false, _blT = false;
            int _i_Channel = -1;

            m_fl_Max_Limit = m_SysInfo_C.m_Plant_C.fl_Max_Limit / 100.0f;

            for (int i = 0; i < iAll_Num; i++)
            {
                _Arr_RemoteIpPort[i].blUse = m_Coat_DLL.m_Arr_RemoteIpPort[i].blUse;
                if (_Arr_RemoteIpPort[i].blUse == false) continue;

                _Arr_RemoteIpPort[i].iGain = m_Coat_DLL.m_Arr_RemoteIpPort[i].iGain;
                _Arr_RemoteIpPort[i].m_flThick = m_Coat_DLL.m_Arr_RemoteIpPort[i].m_flThick;

                #region 误差计算

                //   m_flWc = float.Parse(Math.Abs(_Arr_RemoteIpPort[i].m_flThick - m_SysInfo_C.m_Plant_C.flNormal_Thickness).ToString("f2"));//计算误差
                m_flWc = float.Parse((m_SysInfo_C.m_Plant_C.flNormal_Thickness - _Arr_RemoteIpPort[i].m_flThick).ToString("f2"));//计算误差
                _flBfz = Math.Abs(m_flWc / m_SysInfo_C.m_Plant_C.flNormal_Thickness);

                if (_flBfz <= m_fl_Max_Limit)
                {
                    _blT = Math.Abs(m_flWc) >= m_SysInfo_C.m_Plant_C.flstrThickAlarm;

                    if (m_flWc < 0 && _blT)
                    {
                        _blT = false;
                        m_flWc = 100;
                    }

                    _Arr_RemoteIpPort[i].blAlarm = _blT;

                    if (_blT && Math.Abs(_fl_Channel_WcMax) < Math.Abs(m_flWc))
                    {
                        _fl_Channel_WcMax = m_flWc;
                        _i_Channel = i; _blAlarm = true;
                        _flThickness = _Arr_RemoteIpPort[i].m_flThick;
                    }
                    if (m_flWc > 0)//减薄
                        m_SysInfo_C.m_Plant_C.CalculThickColor(m_flWc, out R, out G, out B, 0, m_SysInfo_C.m_Plant_C.Ck_No_Normal_Thickness);
                    else//增厚
                        m_SysInfo_C.m_Plant_C.CalculThickColor_A(Math.Abs(m_flWc), out R, out G, out B, 0, m_SysInfo_C.m_Plant_C.Ck_No_Normal_Thickness);
                }
                else
                {
                    m_flWc = 100;
                    _Arr_RemoteIpPort[i].blAlarm = false;
                    R = m_SysInfo_C.m_Plant_C.i_Alarm_Out_R; //255;
                    G = m_SysInfo_C.m_Plant_C.i_Alarm_Out_G; //245;
                    B = m_SysInfo_C.m_Plant_C.i_Alarm_Out_B;//0
                }
                _Arr_RemoteIpPort[i].flWc = m_flWc;
                _Arr_RemoteIpPort[i].strColor = R.ToString() + "/" + G.ToString() + "/" + B.ToString();
                _Arr_RemoteIpPort[i].R = R;
                _Arr_RemoteIpPort[i].G = G;
                _Arr_RemoteIpPort[i].B = B;
                #endregion

                _Arr_RemoteIpPort[i].m_btArrWave = new byte[1000];

                if (m_Coat_DLL.m_Arr_RemoteIpPort[i].m_btArrWave == null)
                    m_Coat_DLL.m_Arr_RemoteIpPort[i].m_btArrWave = new byte[1000];
                if (m_Coat_DLL.m_Arr_RemoteIpPort[i].m_iArrWave == null)
                    m_Coat_DLL.m_Arr_RemoteIpPort[i].m_iArrWave = new int[1000];

                IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_Arr_RemoteIpPort[i].m_btArrWave, 0);//   
                Marshal.Copy(m_Coat_DLL.m_Arr_RemoteIpPort[i].m_btArrWave, 0, IntPtArr, _iArr_Len);

                _Arr_RemoteIpPort[i].m_iArrWave = new int[1000];
                IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_Arr_RemoteIpPort[i].m_iArrWave, 0);//   
                Marshal.Copy(m_Coat_DLL.m_Arr_RemoteIpPort[i].m_iArrWave, 0, IntPtArr, _iArr_Len);


                #region 3.1.2异常数据统计

                #endregion 3.1.2
            }
            #endregion 1

            if (_blAlarm)
            {
                if (m_SysInfo_C.m_One_Alarm_Buff_Coat.Count == 0)//  m_Alarm.i_X_S == -1)//第一个缺陷
                {
                    CL_AlarmData _Alarm = new CL_AlarmData();
                    _Alarm.i_X_S = i_Trip_Com_mm;
                    _Alarm.i_X_E = i_Trip_Com_mm;
                    _Alarm.fl_Wc = _fl_Channel_WcMax;
                    _Alarm.i_Y_S = _i_Channel;
                    _Alarm.i_Y_X = _i_Channel;

                    _Alarm.fl_Min_Thick = _flThickness;
                    m_SysInfo_C.m_One_Alarm_Buff_Coat.Add(_Alarm);
                    m_i_Alarm_Num++;
                }
                else if (Math.Abs(i_Trip_Com_mm - m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num].i_X_E) <= m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X)
                {
                    m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num].i_X_E = i_Trip_Com_mm;
                    m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num].fl_Wc = _fl_Channel_WcMax;
                    m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num].i_Y_S = _i_Channel;
                    m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num].fl_Min_Thick = _flThickness;
                }
                else
                {
                    //数据整理
                    if (m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num].i_X_S > m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num].i_X_E)
                    {
                        int _id = m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num].i_X_S;
                        m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num].i_X_S = m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num].i_X_E;
                        m_SysInfo_C.m_One_Alarm_Buff_Coat[m_i_Alarm_Num].i_X_E = _id;
                    }
                    //新区间
                    CL_AlarmData _Alarm = new CL_AlarmData();
                    _Alarm.i_X_S = i_Trip_Com_mm;
                    _Alarm.i_X_E = i_Trip_Com_mm;
                    _Alarm.fl_Wc = _fl_Channel_WcMax;
                    _Alarm.i_Y_S = _i_Channel;
                    _Alarm.i_Y_X = _i_Channel;
                    _Alarm.fl_Min_Thick = _flThickness;

                    m_SysInfo_C.m_One_Alarm_Buff_Coat.Add(_Alarm);
                    m_i_Alarm_Num++;
                }
            }
            #region 2 拿距离，计算显示位置
            //X轴线性增加（地址不变只覆盖数据），显示缓存计算位置

            //1 计算显示缓存链表的位置
            //1.1 由实际点计算数据对应在缓存中开始序号

            m_flDat = Math.Abs(i_Trip_Com_mm - m_Climb4.m_SysBuf.Trip_Com_Init_mm) / m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X;
            m_i_X_Buff_No = (int)m_flDat;//
            if (m_flDat - m_i_X_Buff_No >= 0.5) m_i_X_Buff_No++;
            if (m_i_X_Buff_No < 0) m_i_X_Buff_No = 0;

            //1.2 由实际点计算数据对应在当前屏幕中开始序号
            m_flDat = ((i_Trip_Com_mm - m_SysInfo_C.m_Plant_C.i_Screen_Start_Distance) / m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X);
            m_iCurr_X = (int)m_flDat;//
            if (m_flDat - m_iCurr_X >= 0.5) m_iCurr_X++;
            m_i_X_Screen_No = m_iCurr_X;
            if (m_i_X_Screen_No < 0) m_i_X_Screen_No = 0;
            #endregion 2

            #region 2 添加新列数据
            //2.1 原始数据缓存没有了
            if (m_i_X_Buff_No >= m_SysInfo_C.m_Lst_Mul_One_Buff_Coat.lst_Show_Buff.Count)
            {
                //增加3个显示缓存
                for (int i = 0; i < 300; i++)//---cdw----
                {
                    Cls_Mul_ShowBuff _ShowBuff = new Cls_Mul_ShowBuff(iAll_Num);
                    for (int _iR = 0; _iR < iAll_Num; _iR++)
                        _ShowBuff.Arr_C_Data[_iR] = new Cls_EMAT_2();

                    m_SysInfo_C.m_Lst_Mul_One_Buff_Coat.lst_Show_Buff.Add(_ShowBuff);
                }
            }
            #endregion 2

            #region 3
            //3.0 计算显示值
            for (int _iR = 0; _iR < iAll_Num; _iR++)
            {
                m_SysInfo_C.m_Lst_Mul_One_Buff_Coat.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].blUse = _Arr_RemoteIpPort[_iR].blUse;
                //m_Arr_C_Data_Coat[_iR].blUse = _Arr_RemoteIpPort[_iR].blUse;

                if (_Arr_RemoteIpPort[_iR].blUse == false) continue;
                if (m_SysInfo_C.m_Lst_Mul_One_Buff_Coat.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].flWc < _Arr_RemoteIpPort[_iR].flWc)
                {
                    m_SysInfo_C.m_Lst_Mul_One_Buff_Coat.lst_Show_Buff[m_i_X_Buff_No].m_i_X = i_Trip_Com_mm;
                    m_SysInfo_C.m_Lst_Mul_One_Buff_Coat.lst_Show_Buff[m_i_X_Buff_No].bl_BuffUse = true;
                    m_SysInfo_C.m_Lst_Mul_One_Buff_Coat.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].m_i_X = i_Trip_Com_mm;
                    m_SysInfo_C.m_Lst_Mul_One_Buff_Coat.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].flThick = _Arr_RemoteIpPort[_iR].m_flThick;
                    m_SysInfo_C.m_Lst_Mul_One_Buff_Coat.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].blAlarm = _Arr_RemoteIpPort[_iR].blAlarm;
                    m_SysInfo_C.m_Lst_Mul_One_Buff_Coat.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].strColor = _Arr_RemoteIpPort[_iR].strColor;
                    m_SysInfo_C.m_Lst_Mul_One_Buff_Coat.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].R = _Arr_RemoteIpPort[_iR].R;
                    m_SysInfo_C.m_Lst_Mul_One_Buff_Coat.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].G = _Arr_RemoteIpPort[_iR].G;
                    m_SysInfo_C.m_Lst_Mul_One_Buff_Coat.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].B = _Arr_RemoteIpPort[_iR].B;

                    m_SysInfo_C.m_Lst_Mul_One_Buff_Coat.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].flWc = _Arr_RemoteIpPort[_iR].flWc;

                    if (m_SysInfo_C.m_Lst_Mul_One_Buff_Coat.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].btArrWave == null)
                        m_SysInfo_C.m_Lst_Mul_One_Buff_Coat.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].btArrWave = new byte[_iArr_Len];
                    var IntPtArr_S = Marshal.UnsafeAddrOfPinnedArrayElement(m_SysInfo_C.m_Lst_Mul_One_Buff_Coat.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].btArrWave, 0);
                    Marshal.Copy(_Arr_RemoteIpPort[_iR].m_btArrWave, 0, IntPtArr_S, _iArr_Len);

                    //m_Arr_C_Data_Coat[_iR].flThick = _Arr_RemoteIpPort[_iR].m_flThick;
                    //m_Arr_C_Data_Coat[_iR].blAlarm = _Arr_RemoteIpPort[_iR].blAlarm;
                    //m_Arr_C_Data_Coat[_iR].strColor = _Arr_RemoteIpPort[_iR].strColor;
                    //m_Arr_C_Data_Coat[_iR].R = _Arr_RemoteIpPort[_iR].R;
                    //m_Arr_C_Data_Coat[_iR].G = _Arr_RemoteIpPort[_iR].G;
                    //m_Arr_C_Data_Coat[_iR].B = _Arr_RemoteIpPort[_iR].B;
                    //m_Arr_C_Data_Coat[_iR].flWc = _Arr_RemoteIpPort[_iR].flWc;

                }
            }

            //3.1 原始记录判断是否距离没有变化，
            if (m_Climb4.m_SysBuf.iRun_Gsb_Qj1_Ht0 == 1 && m_SysInfo_C.m_Ctrl.m_i_Trip_Com_mm_Old < i_Trip_Com_mm ||
                m_Climb4.m_SysBuf.iRun_Gsb_Qj1_Ht0 == 0 && (m_SysInfo_C.m_Ctrl.m_i_Trip_Com_mm_Old > i_Trip_Com_mm || m_SysInfo_C.m_Ctrl.m_i_Trip_Com_mm_Old == -1))//新位置，或者前进或者后退
            {
                Cls_Mul_Orig _Orig = new Cls_Mul_Orig(iAll_Num);
                //填充数据
                // _Orig.bl_BuffUse = true;
                _Orig.i_X_mm = i_Trip_Com_mm;
                #region 显示缓存
                //  _Orig.i_Screen_Col = m_i_X_Screen_No;

                #endregion
                Mody_Mul_CurrData_Coat(iAll_Num, _iArr_Len, _Arr_RemoteIpPort, true, ref _Orig.Arr_Emat);

                m_SysInfo_C.m_Lst_Mul_One_Buff_Coat.lst_Ori_Data.Add(_Orig);//始终添加到最后，这样做目的是为了显示界面正常显示车体实时运行轨迹
                m_P_i_Len++;
                m_SysInfo_C.m_Ctrl.m_i_Trip_Com_mm_Old = i_Trip_Com_mm;
            }
            else//位置不变，只刷新数据
            {
                Mody_Mul_CurrData_Coat(iAll_Num, _iArr_Len, _Arr_RemoteIpPort, false, ref m_SysInfo_C.m_Lst_Mul_One_Buff_Coat.lst_Ori_Data[m_P_i_Len].Arr_Emat);
            }
            #endregion 3
        }
        /// <summary>
        /// 多通道脉冲涡流
        /// </summary>
        /// <param name="iAll_Num"></param>
        private void Get_Data_Mul_ECT(int iAll_Num)
        {
            if (!m_SysInfo_C.m_Ctrl.m_bl_CS)
                m_Climb4.m_SysBuf.Trip_Com_mm = m_ECT_My_DLL.i_Trip_mm;
            if (m_SysInfo_C.m_Ctrl.m_iRun != 1) return;
            //     m_SysInfo_C.m_Ctrl.m_bl_CS = false ;
            #region 测试数据 
            //int _iArr_Len = 1000;
            //        m_SysInfo_C.m_Ctrl.m_bl_CS = true;
            if (m_SysInfo_C.m_Ctrl.m_bl_CS)
            {
                #region 模拟距离
                if (m_Climb4.m_SysBuf.iRun_Gsb_Qj1_Ht0 == 1)
                    m_Climb4.m_SysBuf.Trip_Com_mm += m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X;
                else
                    m_Climb4.m_SysBuf.Trip_Com_mm -= m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X;
                if (m_Climb4.m_SysBuf.Trip_Com_mm < 0) m_Climb4.m_SysBuf.Trip_Com_mm = 0;

                m_Climb4.m_SysBuf.Trip = m_Climb4.m_SysBuf.Trip_Com_mm / 1000F;
                #endregion
            }
            #endregion 测试数据

            i_Trip_Com_mm = m_Climb4.m_SysBuf.Trip_Com_mm;

            #region 1 拿多通道最大误差数据
            float _flThick = 0, _fl_Channel_WcMax = 0, _flT = 0, _flT_Curr = 0;
            float _flThickness = 0;
            bool _blAlarm = false, _blT = false;
            int _i_Channel = -1;

            m_fl_Max_Limit = m_SysInfo_C.m_Plant_C.fl_Max_Limit / 100.0f;
            //统计通道最大误差
            for (int i = 0; i < iAll_Num; i++)
            {
                if (m_ECT_My_DLL.m_ArrECT_Buff[i].blLink == false && !m_SysInfo_C.m_Ctrl.m_bl_CS) continue;
                _flThick = m_ECT_My_DLL.m_ArrECT_Buff[i].flThick;
                m_flWc = float.Parse((m_SysInfo_C.m_Plant_C.flNormal_Thickness - _flThick).ToString("f2"));//计算误差
                _blT &= m_ECT_My_DLL.m_ArrECT_Buff[i].blAlarm;

                if (m_flWc > 0 && _fl_Channel_WcMax < m_flWc)//m_ECT_My_DLL.m_ArrECT_Buff[i].blAlarm || 
                {
                    _fl_Channel_WcMax = m_flWc;
                    _i_Channel = i;
                    _blAlarm = true;
                    _flThickness = _flThick;
                }
            }
            #endregion 1

            if (_blAlarm)//统计减薄区域
            {
                if (m_SysInfo_C.m_One_Alarm_Buff.Count == 0)//  m_Alarm.i_X_S == -1)//第一个缺陷
                {
                    CL_AlarmData _Alarm = new CL_AlarmData();
                    _Alarm.i_X_S = i_Trip_Com_mm;
                    _Alarm.i_X_E = i_Trip_Com_mm;
                    _Alarm.fl_Wc = _fl_Channel_WcMax;
                    _Alarm.i_Y_S = _i_Channel;
                    _Alarm.i_Y_X = _i_Channel;

                    _Alarm.fl_Min_Thick = _flThickness;
                    m_SysInfo_C.m_One_Alarm_Buff.Add(_Alarm);
                    m_i_Alarm_Num++;
                }
                else if (Math.Abs(i_Trip_Com_mm - m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_E) <= m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X)
                {
                    if (_fl_Channel_WcMax > m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].fl_Wc)//区间误差最大就替换
                    {
                        m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_E = i_Trip_Com_mm;
                        m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].fl_Wc = _fl_Channel_WcMax;
                        m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_Y_S = _i_Channel;
                        m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].fl_Min_Thick = _flThickness;
                    }
                }
                else
                {
                    if (m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_S > m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_E)//倒着走
                    {
                        int _id = m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_S;
                        m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_S = m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_E;
                        m_SysInfo_C.m_One_Alarm_Buff[m_i_Alarm_Num].i_X_E = _id;
                    }
                    //产生新区间
                    CL_AlarmData _Alarm = new CL_AlarmData();
                    _Alarm.i_X_S = i_Trip_Com_mm;
                    _Alarm.i_X_E = i_Trip_Com_mm;
                    _Alarm.fl_Wc = _fl_Channel_WcMax;
                    _Alarm.i_Y_S = _i_Channel;
                    _Alarm.i_Y_X = _i_Channel;
                    _Alarm.fl_Min_Thick = _flThickness;

                    m_SysInfo_C.m_One_Alarm_Buff.Add(_Alarm);
                    m_i_Alarm_Num++;
                }
            }
            #region 2 拿距离，计算显示位置

            //1 计算显示缓存链表的位置
            //1.1 缓存位置：由实际点计算数据对应在缓存中开始序号
            m_flDat = Math.Abs((i_Trip_Com_mm - m_Climb4.m_SysBuf.Trip_Com_Init_mm) * 1.0f / (1.0f * m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X));
            m_i_X_Buff_No = (int)m_flDat;//
            if (m_flDat - m_i_X_Buff_No >= 0.5) m_i_X_Buff_No++;
            if (m_i_X_Buff_No < 0) m_i_X_Buff_No = 0;

            //1.2 屏幕位置：由实际点计算数据对应在当前屏幕中开始序号
            m_flDat = (1.0f * (i_Trip_Com_mm - m_SysInfo_C.m_Plant_C.i_Screen_Start_Distance) / (m_SysInfo_C.m_Plant_C.Scree_iDotWithmm_X) * 1.0f);
            m_iCurr_X = (int)m_flDat;//
            if (m_flDat - m_iCurr_X >= 0.5) m_iCurr_X++;
            m_i_X_Screen_No = m_iCurr_X;
            if (m_i_X_Screen_No < 0) m_i_X_Screen_No = 0;
            #endregion 2

            #region 2 添加缓存
            //2.1 原始数据缓存没有了
            if (m_i_X_Buff_No >= m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff.Count)
            {
                //增加3个显示缓存
                for (int i = 0; i < 300; i++)//---cdw----
                {
                    Cls_Mul_ShowBuff _ShowBuff = new Cls_Mul_ShowBuff(iAll_Num);
                    for (int _iR = 0; _iR < iAll_Num; _iR++)
                        _ShowBuff.Arr_C_Data[_iR] = new Cls_EMAT_2();

                    m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff.Add(_ShowBuff);
                }
            }
            #endregion 2

            #region 3
            //3.0 计算界面显示值（区间值）

            for (int _iR = 0; _iR < iAll_Num; _iR++)
            {
                _blT = m_ECT_My_DLL.m_ArrECT_Buff[_iR].blLink;
                m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].blUse = _blT;
                // m_Arr_C_Data[_iR].blUse = _blT;

                if (_blT == false && !m_SysInfo_C.m_Ctrl.m_bl_CS) continue;
                _flT = m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].flWc;
                _flThick = m_ECT_My_DLL.m_ArrECT_Buff[_iR].flThick;

                if (_flT == 0 || //数据为空
                    m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].blAlarm == false &&
                     m_ECT_My_DLL.m_ArrECT_Buff[_iR].blAlarm ||//老数据正常新数据报警
                    _flT < 0 && m_ECT_My_DLL.m_ArrECT_Buff[_iR].blAlarm == false ||//老的增厚新的减薄，就替换
                     _flT > 0 && _flThick > 0 && _flT < _flThick)//新数据误差大
                {
                    m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].m_i_X = i_Trip_Com_mm;
                    m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].iWaveStandNo = m_ECT_My_DLL.m_ArrECT_Cali[0].iWaveStandNo;
                    m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].iData_Type = 2;
                    m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].bl_BuffUse = true;
                    m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].blUse = true;
                    m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].m_i_X = i_Trip_Com_mm;
                    m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].flThick = m_ECT_My_DLL.m_ArrECT_Buff[_iR].flThick;
                    m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].blAlarm = m_ECT_My_DLL.m_ArrECT_Buff[_iR].blAlarm;
                    m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].strColor = m_ECT_My_DLL.m_ArrECT_Buff[_iR].strColor;
                    m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].R = m_ECT_My_DLL.m_ArrECT_Buff[_iR].R;
                    m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].G = m_ECT_My_DLL.m_ArrECT_Buff[_iR].G;
                    m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].B = m_ECT_My_DLL.m_ArrECT_Buff[_iR].B;

                    m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].flWc = m_ECT_My_DLL.m_ArrECT_Buff[_iR].flWc;

                    if (m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].flArrWave == null)
                        m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].flArrWave = new float[m_ECT_My_DLL.m_iArr_Len];
                    var IntPtArr_S = Marshal.UnsafeAddrOfPinnedArrayElement(m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Show_Buff[m_i_X_Buff_No].Arr_C_Data[_iR].flArrWave, 0);
                    Marshal.Copy(m_ECT_My_DLL.m_ArrECT_Buff[_iR].m_iArrWave, 0, IntPtArr_S, m_ECT_My_DLL.m_iArr_Len);

                    //m_Arr_C_Data[_iR].flThick = m_ECT_My_DLL.m_ArrECT_Buff[_iR].flThick;
                    //m_Arr_C_Data[_iR].blAlarm = m_ECT_My_DLL.m_ArrECT_Buff[_iR].blAlarm;
                    //m_Arr_C_Data[_iR].strColor = m_ECT_My_DLL.m_ArrECT_Buff[_iR].strColor;
                    //m_Arr_C_Data[_iR].R = m_ECT_My_DLL.m_ArrECT_Buff[_iR].R;
                    //m_Arr_C_Data[_iR].G = m_ECT_My_DLL.m_ArrECT_Buff[_iR].G;
                    //m_Arr_C_Data[_iR].B = m_ECT_My_DLL.m_ArrECT_Buff[_iR].B;
                    //m_Arr_C_Data[_iR].flWc = m_ECT_My_DLL.m_ArrECT_Buff[_iR].flWc;
                    //m_Arr_C_Data[_iR].strArrWave  = m_ECT_My_DLL.m_ArrECT_Buff[_iR].strWave ;
                }
            }

            //3.1 原始记录判断是否距离没有变化，
            if (m_Climb4.m_SysBuf.iRun_Gsb_Qj1_Ht0 == 1 && m_SysInfo_C.m_Ctrl.m_i_Trip_Com_mm_Old < i_Trip_Com_mm ||
                m_Climb4.m_SysBuf.iRun_Gsb_Qj1_Ht0 == 0 && (m_SysInfo_C.m_Ctrl.m_i_Trip_Com_mm_Old > i_Trip_Com_mm ||
                m_SysInfo_C.m_Ctrl.m_i_Trip_Com_mm_Old == -1))//新位置，或者前进或者后退
            {
                Cls_Mul_Orig _Orig = new Cls_Mul_Orig(iAll_Num);
                //填充数据
                _Orig.i_X_mm = i_Trip_Com_mm;
                #region 实时缓存
                //  _Orig.i_Screen_Col = m_i_X_Screen_No;
                Mody_Mul_CurrData_ECT(iAll_Num, true, ref _Orig.Arr_Emat);

                m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Ori_Data.Add(_Orig);//始终添加到最后，这样做目的是为了显示界面正常显示车体实时运行轨迹
                m_P_i_Len++;
                m_SysInfo_C.m_Ctrl.m_i_Trip_Com_mm_Old = i_Trip_Com_mm;
                #endregion
            }
            else if (m_P_i_Len > -1)//位置不变，只刷新数据
            {
                Mody_Mul_CurrData_ECT(iAll_Num, false,
                                      ref m_SysInfo_C.m_Lst_Mul_One_Buff.lst_Ori_Data[m_P_i_Len].Arr_Emat);
            }
            #endregion 3
        }
        /// <summary>
        /// 最新X/Y位置跟踪器
        /// </summary>
        private void Chang_Posit()
        {
            if (m_SysInfo_C.m_Ctrl.m_iRun != 1) return;
            #region 数据跟踪器   X轴或者Y轴有位置变化
            bool _bl_Chan_Posit = m_i_X_Buff_No != m_P_i_X_Buff_No_Old || m_i_Y_ShowBuff_No != m_P_i_Y_ShowBuff_No_Old;
            if (_bl_Chan_Posit)
            {
                if (m_P_iNewPosit_No < m_P_i_Len - 1)//0->1998 <  1999
                    m_P_iNewPosit_No++;
                else
                    m_P_iNewPosit_No = 0;

                m_P_Arr_Posi_X[m_P_iNewPosit_No] = m_i_X_Buff_No;
                m_P_Arr_Posi_Y[m_P_iNewPosit_No] = m_i_Y_ShowBuff_No;
                m_P_Arr_Distanc[m_P_iNewPosit_No] = i_Trip_Com_mm;
                m_P_i_X_Buff_No_Old = m_i_X_Buff_No;
                m_P_i_Y_ShowBuff_No_Old = m_i_Y_ShowBuff_No;
            }
            #endregion 数据跟踪器
        }
        /// <summary>
        /// 以毫秒为单位延时
        /// </summary>
        /// <param name="dbWait">延时时间：ms</param>
        public static void WaitTime(float dbWait)
        {
            DateTime dtStar = DateTime.Now;
            while (true)
            {
                System.Threading.Thread.Sleep(3);
                
                if (DateTime.Now.Subtract(dtStar).TotalMilliseconds > dbWait) break;
               Application.DoEvents();
            }
        }
        #endregion 方法

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
            if( IniReadDefine("CmmClimb", "LOG", "0", Application.StartupPath + "\\database\\SysConfig.ini") == "1"
            == false) return;

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
                    if (swTxt != null)
                    {
                        swTxt.WriteLine(strValue);
                        swTxt.Flush();
                        swTxt.Close();
                    }
                }
                catch (Exception e)
                {
                }
                swTxt = null;
            }
        }

        #endregion 文件操作
        /// <summary>
        /// 创建当前ID文件夹
        /// </summary>
        /// <param name="ID"></param>
        public void CreatCurrDir(string strPath,  string ID)
        {
            //1 判断路径是否存在
            //判断是否有Temp
          //  string strPath = SysInfo.m_W_i_FilePath + "\\";
            if (System.IO.Directory.Exists(strPath) == false)
            {
                System.IO.Directory.CreateDirectory(strPath);
            }
            strPath = strPath + ID + "\\";
            if (System.IO.Directory.Exists(strPath) == false)
            {
                System.IO.Directory.CreateDirectory(strPath);
            }
        }

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
