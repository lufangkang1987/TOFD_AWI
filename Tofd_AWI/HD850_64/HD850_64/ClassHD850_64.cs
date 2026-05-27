/*
 * Copyright(C) 2 2017 郑州金润高科电子有限公司
 * 文件名: HD850_64
 * 文件功能描述:测量生成一字高度数据
 * 目的：用江苏博智慧达HD8-0050W一字激光轮廓传感器仪器产生200帧/秒数据，每帧500个高度数据，用于测量焊缝位置、绘制3D图等用途 
 * 创建标识: 陈大伟 2020-8-19
 * 修改标识: 
 * 修改描述:
 * 版本：V1.0


 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

using System.IO;
using System.Drawing.Imaging;
using LSHDPROFILE;
using Frame_Work;
using Emgu;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;

namespace HD850_64
{
    public class Cl_HD850_64
    {
        public Cl_HD850_64(ref ClassSys_Buff   SysBuff)
        {
            m_Dic_SysBuff = SysBuff;
        }
        public Cl_HD850_64()
        {
           
        }
        #region 变量
        static byte[] m_ArrBt_Z = new byte[2];
        int m_iBt_Z = 0;
        /// <summary>
        /// 计算线的类
        /// </summary>
        static ClassLine m_CalLin = new ClassLine();
        /// <summary>
        /// 第一条线
        /// </summary>
       static   LineK m_Line_1 = new LineK();
        /// <summary>
        /// 第二条线
        /// </summary>
        static LineK m_Line_2 = new LineK();
        /// <summary>
        /// 第三条线 ：交点平分线
        /// </summary>
        static LineK m_Line_3 = new LineK();
        /// <summary>
        /// 两条线的角度点坐标
        /// </summary>
        static Lien_Line_Point m_Point_JiaoDian = new Lien_Line_Point();

        /// <summary>
        /// 带缺陷的测量数据字典
        /// </summary>
        static  Frame_Work.ClassSys_Buff m_Dic_SysBuff;
        /******用户自定义变量*******/
        private static uint m_lsNumber = 1;
        private double m_maxX = 50;//轮廓线X最大范围
        private double m_maxZ = 80;//轮廓线Z最大范围
        private double m_scaleX = 5;//显示轮廓的X刻度
        private double m_scaleZ = 8;//显示轮廓的Z刻度

        /// <summary>
        /// 母材厚度
        /// </summary>
        private static float m_flThick = 1;//
        /// <summary>
        /// 母材厚度
        /// </summary>
        public float g_flThick
        {
            get { return m_flThick; }
            set { m_flThick = value; }
        }
        //---------
        /// <summary>
        /// 左入口门槛
        /// </summary>
        private static float _fl_Limit_L = 0.45f;//
        /// <summary>
        /// 左入口门槛
        /// </summary>
        public float g_fl_Limit_L
        {
            get { return _fl_Limit_L; }
            set { _fl_Limit_L = value; }
        }
        /// <summary>
        /// 右入口门槛
        /// </summary>
        private static float _fl_Limit_R = 1;//
        /// <summary>
        /// 右入口门槛
        /// </summary>
        public float g_fl_Limit_R
        {
            get { return _fl_Limit_R; }
            set { _fl_Limit_R = value; }
        }
        //---




        /// <summary>
        /// 缺陷高度落差标准 默认0.4mm
        /// </summary>
        private static  double m_Limit_Alarm_H = 0.4;//
        public double  g_Limit_Alarm_H
        {
            get { return m_Limit_Alarm_H; }
            set { m_Limit_Alarm_H = value; }
        }
        /// <summary>
        /// 原始图
        /// </summary>
        public System.Windows.Forms.Label Lb_YuanShiTu;
        /// <summary>
        /// 轮廓图
        /// </summary>
        public System.Windows.Forms.Label Lb_LunKuoTu;

        /// <summary>
        /// 焊缝位置
        /// </summary>
        private static Class_Weld_Position _WeldPosition = new Class_Weld_Position();// null;
        /// <summary>
        /// 焊缝位置
        /// </summary>
        public  Class_Weld_Position g_WeldPosition
        {
            get { return _WeldPosition; }
            set { _WeldPosition = value; }
        }

        #region 3 输出数据
        /// <summary>
        /// 接收报文距离信息，单位0.5mm
        /// </summary>
        private static  int  m_iKey_Dist_Frame = 0;
        /// <summary>
        /// 接收报文距离信息，单位0.5mm
        /// </summary>
        public int g_iKey_Dist_Frame
        {
            get { return m_iKey_Dist_Frame; }
            set { m_iKey_Dist_Frame = value; }
        }

        /// <summary>
        /// 报文显示
        /// </summary>
        public TextBox Txt_Data = new TextBox();
        /// <summary>
        /// 中心点位置
        /// </summary>
        public TextBox Txt_CentWelb = new TextBox();
        /// <summary>
        /// 焊缝类型0：对接焊缝 1：对角内角 2： 对角外角  3 ：搭接
        /// </summary>
        private static int  m_Weld_Type = 0;
        /// <summary>
        /// 焊缝类型0：对接焊缝 1：对角内角 2： 对角外角  3 ：搭接
        /// </summary>
        public int g_Weld_Type
        {
            get { return m_Weld_Type; }
            set { m_Weld_Type = value; }

        }
        /// <summary>
        /// 焊缝面是正面还是背面 true:正面 false:背面
        /// </summary>
        private static bool  m_blZm1_Bm0 = true ;
        /// <summary>
        /// 焊缝面是正面还是背面 true:正面 false:背面
        /// </summary>
        public bool g_blZm1_Bm0
        {
            get { return m_blZm1_Bm0; }
            set { m_blZm1_Bm0 = value; }
        }

        /// <summary>
        ///触发方式0：内部触发 1：编码器触发
        /// </summary>
        private static int m_ChuFa_Time0_Bmq1 = 0;
        /// <summary>
        /// 触发方式0：内部触发 1：编码器触发
        /// </summary>
        public int g_ChuFa_Time0_Bmq1
        {
            get { return m_ChuFa_Time0_Bmq1; }
            set { m_ChuFa_Time0_Bmq1 = value; }

        }
        //--
        /// <summary>
        /// 是否已经拿数据进行发送
        /// </summary>
        private static int m_iGet_ArrZ = 0 ;
        /// <summary>
        /// 是否已经拿数据进行发送
        /// </summary>
        public int g_iGet_ArrZ
        {
            get { return m_iGet_ArrZ; }
            set { m_iGet_ArrZ = value; }
        }
        /// <summary>
        /// 将一个高度*100转换成16进制整数进行网络传输，地位在前 高位在后
        /// </summary>
        private static byte [] m_Z_Out = new byte[1008+3+26];//单轮廓的X数组
        public byte[] g_Z_Out
        {
            get { return m_Z_Out; }
            set { m_Z_Out = value; }

        }
        //--
        private  static double[] m_profileX = new double[1000];//单轮廓的X数组
        public double[] g_profileX
        {
            get { return m_profileX; }
            set { m_profileX = value; }

        }
        private  static double[] m_profileZ = new double[1000];//单轮廓的Z数组
        public  double[] g_profileZ
        {
            get { return m_profileZ; }
            set { m_profileZ = value; }
        }
        private static uint m_profileCnt = 0;//单轮廓的点数
        public uint g_profileCnt
        {
            get { return m_profileCnt; }
            set { m_profileCnt = value; }
        }
        private static int m_ImageWidth = 0;//原始图片的宽度
        public int g_ImageWidth
        {
            get { return m_ImageWidth; }
            set { m_ImageWidth = value; }
        }
        private static int m_ImageHeight = 0;//原始图片的高度
        public int g_ImageHeight
        {
            get { return m_ImageHeight; }
            set { m_ImageHeight = value; }
        }
        private static unsafe byte* m_PImageData = null;//原始图片的数据指针
        public unsafe byte*  g_PImageData
        {
            get { return m_PImageData; }
            set { m_PImageData = value; }
        }
        public  int m_showUIFlag = 1;//UI显示状态：1显示轮廓 2显示原图 其他不显示
        public volatile bool m_showProfileBusy = false;//显示轮廓busy状态
        public volatile bool m_showRawImageBusy = false;//显示图像busy状态

        public  static string m_strGetDat = "";//接收到数据
        public string g_strGetDat
        {
            get { return m_strGetDat; }
            set { m_strGetDat = value; }
        }
        private   static double m_dbOutCentWeld = 0;//焊缝中心点
        public double g_dbOutCentWeld
        {
            get { return m_dbOutCentWeld; }
            set { m_dbOutCentWeld = value; }
        }
        /// <summary>
        /// 仪器温度
        /// </summary>
        private  static string m_strTemperature = "";
        /// <summary>
        /// 仪器温度
        /// </summary>
        public string g_strTemperature
        {
            get { return m_strTemperature; }
            set { m_strTemperature = value; }
        }
        /// <summary>
        /// 焊缝高度
        /// </summary>
        public static double m_dbOutCentWeld_H = 0;//焊缝中心点高度
        public double g_dbOutCentWeld_H
        {
            get { return m_dbOutCentWeld_H; }
            set { m_dbOutCentWeld_H = value; }
        }


        /// <summary>
        /// 焊缝宽度
        /// </summary>
        public static double m_dbOutCentWeld_W = 0;//焊缝中心点宽度
        public double g_dbOutCentWeld_W
        {
            get { return m_dbOutCentWeld_W; }
            set { m_dbOutCentWeld_W = value; }

        }




        /// <summary>
        /// 数据头
        /// </summary>
        public static double m_dbStart = 0;//焊缝中心点高度
        public double g_dbStart
        {
            get { return m_dbStart; }
            set { m_dbStart = value; }
        }
        /// <summary>
        /// 数据尾
        /// </summary>
        public static double m_dbEnd = 0;//焊缝中心点高度
        public double g_dbEnd
        {
            get { return m_dbEnd; }
            set { m_dbEnd = value; }
        }
        #endregion 输出数据
        private static double m_dbT = 0;//临时
        /// <summary>
        /// 寻找焊缝数据间隔
        /// </summary>
        public static   double m_dbLimit_FindWeld = 0.32;//
        /// <summary>
        /// 水平角度
        /// </summary>
        public   double m_dbAngle = 0;
        /// <summary>
        /// 水平是否校准：校准后可以搜索缺陷
        /// </summary>
        public static bool m_blAngle_JZ =false ;//水平角度是否
        /// <summary>
        /// 水平是否校准：校准后可以搜索缺陷
        /// </summary>
        public bool g_blAngle_JZ
        {
            get { return m_blAngle_JZ; }
            set { m_blAngle_JZ = value; }

        }
        #endregion 变量

        #region 输出方法
        /// <summary>
        /// 1 参数初始化
        /// </summary>
        public void  Init_Data()
        {


        }
        /// <summary>
        /// 2 打开设备
        /// </summary>
        /// <param name="cameraParamFile"></param>
        /// <param name="iTrigger"></param>
        public bool Run_Cam( string cameraParamFile = "SN8-0050W-1207839",string _strHead = "H",double freq = 20, int iTrigger = 0)
        {
            //Console.WriteLine("系统版本:" + LS_HD6.LSHD6_GetVersion());
            //Console.WriteLine("发现相机:" + LS_HD6.LSHD6_GetEnableCamereNumber());

            #region 打开文件
            bool _blRet = OpenFile(cameraParamFile);//Application.StartupPath +"\\"+ 
            string[] _Spara = cameraParamFile.Split('-');
            #endregion
            #region 相机初始化
            _blRet &= InitCam(_Spara[2], _strHead);
            #endregion

            #region 触发低速
            Trigger(iTrigger == 0 ? 0 : 1);
            #endregion

            LS_HD6.LSHD6_SetCompansation(m_lsNumber, 1, 9);
  //        LS_HD6.LSHD6_SetFilter(m_lsNumber, 2, 0, 0, 9);
            return _blRet;
        }
        /// <summary>
        ///    无效点填补
        /// </summary>
        /// <param name="iType">补间类型，0不补间，1垂直补间，2直线补间</param>
        /// <param name="iDataLimt">补间距离阈值，连续无效点数量小于该值则转换为有效点</param>
        public void Set_Compansation(int iType, int iDataLimt)
        {
            #region 间断点补齐方法
            LS_HD6.LSHD6_SetCompansation(m_lsNumber, iType, iDataLimt);
            #endregion
        }
        /// <summary>
        /// 4.1 绘制原始图像
        /// </summary>
        /// <param name="hWnd"></param>
        public void Draw_LunKuoTu( ref Label hWnd ,ref List<Class_Alarm> m_lstAlarm)//  ref label2)
        {
            try
            {
                DrawImageProflie(ref hWnd, ref m_maxX, ref m_maxZ,
                               ref m_scaleX, ref m_scaleZ, ref m_profileX, ref m_profileZ, ref m_profileCnt,
                               ref   m_lstAlarm);
            }
            catch { }
        }
        public void Draw_LunKuoTu(ref Label hWnd, ref Frame_Work.Class_X_Data X_Data)//  ref label2)
        {
            try
            {
                DrawImageProflie(ref hWnd, ref m_maxX, ref m_maxZ,
                               ref m_scaleX, ref m_scaleZ, ref m_profileX, ref m_profileZ, ref m_profileCnt,
                               ref X_Data);
            }
            catch { }
        }
        //Frame_Work.Class_X_Data _X_Data

        //绘制轮廓
        private unsafe void DrawImageProflie(ref Label hWnd, ref double MaxX, ref double MaxZ,
                                            ref double ScaleX, ref double ScaleZ, ref double[] Axis_X, ref double[] Axis_Z, ref uint AxisCount,
                                            ref Frame_Work.Class_X_Data X_Data)
        {
            m_showProfileBusy = true;
            Bitmap bmp = new Bitmap((int)hWnd.Width, (int)hWnd.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.FromArgb(0, 0, 0));// g.Clear(Color.FromArgb(0, 125, 125));
            Color colorGreen = Color.FromArgb(0, 200, 0);//颜色为绿色
            Color colorB = Color.FromArgb(255, 0, 0);//颜色为绿色
            Color colorRed = Color.FromArgb(255, 0, 0);//颜色为红色
            Color colorGray = Color.FromArgb(200, 200, 200);//颜色为白色
            Color colorBlack = Color.FromArgb(30, 30, 30);//颜色为黑色
            Color colorYellow = Color.FromArgb(200, 200, 0);//颜色为黑色
            Pen PGreen = new Pen(colorGreen, 2);//创建一个画笔对象,该画笔的颜色为绿色，笔触大小为2个像素
            Pen pRed = new Pen(colorB, 4);//创建一个画笔对象,该画笔的颜色为黄色，笔触大小为1个像素
            Pen pGray = new Pen(Color.FromArgb(200, 200, 200), 1);//创建一个画笔对象,该画笔的颜色为灰色，笔触大小为1个像素

            Pen pGray_R = new Pen(Color.FromArgb(0, 0, 255), 1);//创建一个画笔对象,该画笔的颜色为灰色，笔触大小为1个像素
            Pen pBalck = new Pen(colorBlack, 3);//创建一个画笔对象,该画笔的颜色为白色，笔触大小为3个像素
            Pen pYellow = new Pen(colorYellow, 3);//创建一个画笔对象,该画笔的颜色为白色，笔触大小为1个像素
            pGray.DashStyle = DashStyle.Dot;
            Font font = new Font("Adobe Gothic Std", 9f, FontStyle.Bold);
            Brush brush = new SolidBrush(Color.FromArgb(255, 255, 255)); //Brush brush = new SolidBrush(Color.FromArgb(0, 0, 0));
            Brush brush_R = new SolidBrush(Color.FromArgb(255, 0, 0));
            uint axisInfo_top = 20;
            uint axisInfo_right = (uint)hWnd.Width - 20;
            uint axisInfo_left = 20;
            uint axisInfo_bottom = (uint)hWnd.Height - 30;
            g.DrawLine(pBalck, axisInfo_left, axisInfo_bottom, axisInfo_left, axisInfo_top);
            g.DrawLine(pBalck, axisInfo_left, axisInfo_bottom, axisInfo_right, axisInfo_bottom);
            //需设置一个页面的X及Z的显示范围及分度格数
            uint xNum, zNum;
            xNum = (uint)(MaxX / ScaleX);//x方向格子总数量
            zNum = (uint)(MaxZ / ScaleZ);//z方向格子总数量
            double XZoom, ZZoom;
            XZoom = (axisInfo_right - axisInfo_left) / MaxX;
            ZZoom = (axisInfo_bottom - axisInfo_top) / MaxZ;
            float scaleXlen = (float)(ScaleX * XZoom);
            float scaleZlen = (float)(ScaleZ * ZZoom);
            string str;
            bool _blShow = false;
            //绘制刻度
            for (uint i = 1; i <= zNum; i++)
            {
                g.DrawLine(pGray, axisInfo_left, axisInfo_bottom - scaleZlen * i, axisInfo_right, axisInfo_bottom - scaleZlen * i);
                str = (ScaleZ * i).ToString();
                g.DrawString(str, font, brush, axisInfo_left - 10, axisInfo_bottom - scaleZlen * i);
            }
            g.DrawString("采集区域 ：" + "0 - 5 45-50", font, brush, 50, axisInfo_bottom + 15);//(m_Weld_Type == 0 ? "0 - 15" : 
            for (uint j = 1; j <= xNum; j++)
            {
                if (j == 1 || j == 9 )//&& m_Weld_Type == 1)
                    g.DrawLine(pYellow, axisInfo_left + scaleXlen * j, axisInfo_bottom, axisInfo_left + scaleXlen * j, axisInfo_top);
                else
                    g.DrawLine(pGray, axisInfo_left + scaleXlen * j, axisInfo_bottom, axisInfo_left + scaleXlen * j, axisInfo_top);

                //       g.DrawLine(j == 5 ? pGray_R:  pGray, axisInfo_left + scaleXlen * j, axisInfo_bottom, axisInfo_left + scaleXlen * j, axisInfo_top);
                str = (ScaleX * j).ToString();

                g.DrawString(str, font, brush, axisInfo_left - 20 + scaleXlen * j, axisInfo_bottom);
            }
            //绘制轮廓线
            if (AxisCount > 10)
            {
                int _ddd = (int)(m_dbOutCentWeld * 10);
                int _iLstNum = X_Data.m_lstAlarm.Count();


                for (uint k = 0; k < AxisCount - 1; k++)
                {
                    if (k == _WeldPosition.i_Cent)
                    {
                        g.DrawString("余高：" + _WeldPosition.dbDepth.ToString("f2") + "mm", font, brush, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(-25 + axisInfo_bottom - ZZoom * Axis_Z[k]));
                        g.DrawString("位置：" +( _WeldPosition.i_Cent/10f).ToString("f1") + "mm", font, brush, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(-48 + axisInfo_bottom - ZZoom * Axis_Z[k]));


                        g.DrawLine(pRed, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(-6 + axisInfo_bottom - ZZoom * Axis_Z[k]),
                                  (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(6 + axisInfo_bottom - ZZoom * Axis_Z[k + 0]));
                    }
              //      if(_blShow)
                    if (X_Data.m_lstAlarm.Count > 0 || X_Data.m_clsWeld.m_blHave)
                    {
                        int _iShowNo = -1;
                        int _iShowNo_Ao = -1;
                        if (X_Data.m_clsWeld.m_blHave)//焊缝 Convert.ToString(166, 16)
                        {
                            if (k == (int)(X_Data.m_clsWeld.m_i_W_Start_X * 10 + (X_Data.m_clsWeld.m_i_W_End_X - X_Data.m_clsWeld.m_i_W_Start_X) * 10 / 2.0f))
                            {
                                g.DrawString("焊缝余高：" + X_Data.m_clsWeld.m_dbCentWeld_H.ToString("f2") + "mm", font, brush, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(15 + axisInfo_bottom - ZZoom * Axis_Z[k]));
                                g.DrawString("宽：" + X_Data.m_clsWeld.m_dbCentWeld_W.ToString("f2") + "mm", font, brush, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(35 + axisInfo_bottom - ZZoom * Axis_Z[k]));
                                g.DrawString("总高：" + X_Data.m_clsWeld.db_Line_3_Mucai_H.ToString("f2") + "mm", font, brush, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(55 + axisInfo_bottom - ZZoom * Axis_Z[k]));
                                if (X_Data.m_clsWeld.m_lstJL.Count > 0)
                                {
                                    string _strT = "";
                                    for (int _i = 0; _i < X_Data.m_clsWeld.m_lstJL.Count; _i++)
                                    {
                                        _strT += X_Data.m_clsWeld.m_lstJL[_i].strType + "" + "\r\n" + X_Data.m_clsWeld.m_lstJL[_i].strJL + "\r\n";
                                        g.DrawString(_strT, font, X_Data.m_clsWeld.m_lstJL[_i].iJL == 1 ? brush : brush_R, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(75 + axisInfo_bottom - ZZoom * Axis_Z[k]));
                                    }
                                }


                            }
                        if (k >= (int)(X_Data.m_clsWeld.m_i_W_Start_X * 10f) && k <= (int)(X_Data.m_clsWeld.m_i_W_End_X * 10f))
                                _iShowNo = 1; //
                        if(k>= _WeldPosition.i_Start && k<= _WeldPosition.i_End )
                                _iShowNo = 1; //  // 
                        }
                        if (k >= _WeldPosition.i_Start && k <= _WeldPosition.i_End && _WeldPosition.m_blAlarm)
                            _iShowNo_Ao = 1;
                        if (X_Data.m_blAlarm && false )
                        {
                            for (int _iNo = 0; _iNo < X_Data.m_lstAlarm.Count; _iNo++)
                            {
                                try
                                {
                                    if (k >= X_Data.m_lstAlarm[_iNo].i_Start && k <= X_Data.m_lstAlarm[_iNo].i_End)
                                    {
                                        if (k == X_Data.m_lstAlarm[_iNo].i_Start)
                                        {
                                            float _w = (X_Data.m_lstAlarm[_iNo].i_End - X_Data.m_lstAlarm[_iNo].i_Start) / 10f;

                                            g.DrawString((X_Data.m_lstAlarm[_iNo].i_Type != 1 ? "凹" : "凸") + " h：" + X_Data.m_lstAlarm[_iNo].dbDepth.ToString("f2") + "mm", font, brush, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(-55 + axisInfo_bottom - ZZoom * Axis_Z[k]));
                                            g.DrawString("b：" + _w.ToString("f2") + "mm", font, brush, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(-40 + axisInfo_bottom - ZZoom * Axis_Z[k]));

                                            if (X_Data.m_lstAlarm[_iNo].i_Type == 1)
                                                g.DrawString(Calcu_DuiJie_Tu(X_Data.m_lstAlarm[_iNo].dbDepth, _w, X_Data.m_blZm1_Bm0, X_Data.m_blDuijie0_JiaoHan1),
                                                    font, brush, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(-25 + axisInfo_bottom - ZZoom * Axis_Z[k]));
                                            else if (X_Data.m_lstAlarm[_iNo].i_Type > 1)
                                                g.DrawString(Calcu_DuiJie_Ao(X_Data.m_lstAlarm[_iNo].dbDepth, X_Data.m_flThick, X_Data.m_blZm1_Bm0, X_Data.m_lstAlarm[_iNo].i_Type),
                                                   font, brush, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(-25 + axisInfo_bottom - ZZoom * Axis_Z[k]));

                                        }
                                        _iShowNo_Ao = 1;
                                        break;
                                    }
                                }
                                catch (Exception e2)
                                { }
                            }
                        }

                        if (_iShowNo > -1)
                        {
                            g.DrawLine(X_Data.m_clsWeld.m_blHave ? pYellow : pRed, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(axisInfo_bottom - ZZoom * Axis_Z[k]),
                                      (float)(axisInfo_left + XZoom * Axis_X[k + 1]), (float)(axisInfo_bottom - ZZoom * Axis_Z[k + 1]));
                        }
                        else
                            g.DrawLine(m_dbOutCentWeld > 0 && k == m_dbOutCentWeld * 10 ? pRed : PGreen, (float)(axisInfo_left + XZoom * Axis_X[k]),
                                      (float)(axisInfo_bottom - ZZoom * Axis_Z[k]), (float)(axisInfo_left + XZoom * Axis_X[k + 1]),
                                      (float)(axisInfo_bottom - ZZoom * Axis_Z[k + 1]));
                        if (_iShowNo_Ao > 0)
                            g.DrawLine(pRed, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(axisInfo_bottom - ZZoom * Axis_Z[k]),
                                       (float)(axisInfo_left + XZoom * Axis_X[k + 1]), (float)(axisInfo_bottom - ZZoom * Axis_Z[k + 1]));

                    }
                    else
                        g.DrawLine(m_dbOutCentWeld > 0 && k == m_dbOutCentWeld * 10 ? pRed : PGreen, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(axisInfo_bottom - ZZoom * Axis_Z[k]), (float)(axisInfo_left + XZoom * Axis_X[k + 1]),
                                  (float)(axisInfo_bottom - ZZoom * Axis_Z[k + 1]));


                }
            }
            hWnd.CreateGraphics().DrawImage(bmp, 0, 0);
            pRed.Dispose(); PGreen.Dispose(); pYellow.Dispose();
            pGray.Dispose(); pBalck.Dispose(); font.Dispose();
            brush.Dispose(); bmp.Dispose(); g.Dispose();
            pGray_R.Dispose();
            m_showProfileBusy = false;
        }
        /// <summary>
        /// 焊缝凸面计算  补强  过渡渗透
        /// </summary>
        /// <param name="H"></param>
        /// <param name="W"></param>
        /// <param name="blZm1_Bm0">焊缝正面还是背面  true:正面 false:背面</param>
        /// <param name="blDuiJie_JiaoHan">对接：false  角焊：true</param>
        /// <returns></returns>
        private string Calcu_DuiJie_Tu(double  H,float W,bool blZm1_Bm0,bool blDuiJie_JiaoHan)
        {
            string _strR = "";
            if (blZm1_Bm0)//补强
            {
                if (H <= (1.0 + 0.1f * W) && H <= 5)
                    _strR = "B级，h<=1+0.1 * b h <= 5: " + "Yes";
                else if (H <= (1.0 + 0.15f * W) && H <= 7)
                    _strR = "C级，h<=1+0.15 * b  h <= 7: " + "Yes";
                else if (H <= (1.0 + 0.25f * W) && H <= 10)
                    _strR = "D级，h<=1+0.25 * b  h <= 10: " + "Yes";
                else
                    _strR = "h > 1 + 0.25 * b: " + "No";
            }
            else
            {
                //过渡渗透
                if (blDuiJie_JiaoHan == false)
                {
                    if (H <= (1.0 + 0.1f * W))
                        _strR = "B级，h<=1+0.1 * b: " + "Yes";
                    else if (H <= (1.0 + 0.3f * W))
                        _strR = "C级，h<=1+0.3 * b: " + "Yes";
                    else if (H <= (1.0 + 0.6f * W))
                        _strR = "D级，h<=1+0.6 * b: " + "Yes";
                    else
                        _strR = "h > 1 + 0.6 * b: " + "No";
                }
                else
                {
                    if (H <= (1.0 + 0.2f * W) && H<=3)
                        _strR = "B级，h<=1+0.2 * b h<=3: " + "Yes";
                    else if (H <= (1.0 + 0.6f * W) && H <= 4)
                        _strR = "C级，h<=1+0.6 * b h <= 4: " + "Yes";
                    else if (H <= (1.0 +  W) && H <= 5)
                        _strR = "D级，h<=1+  b h <= 5: " + "Yes";
                    else
                        _strR = "h > 1 + 1.0 * b: " + "No";
                }
            }
            return _strR;
        }

        /// <summary>
        /// 焊缝凸面计算  补强  过渡渗透
        /// </summary>
        /// <param name="H"></param>
        /// <param name="W"></param>
        /// <param name="blZm1_Bm0">焊缝正面还是背面  true:正面 false:背面</param>
        /// <param name="iType">缺陷类型 0：母材上凹陷 1：凸起  2：咬边 3 母材上凹陷 -1:取消此缺陷</param>
        /// <returns></returns>
        private string Calcu_DuiJie_Ao(double H, float t, bool blZm1_Bm0, int iType=2)
        {
            string _strR = "";
            if (blZm1_Bm0)//正面：咬边 下沉
            {
                if (iType == 2)//咬边
                {
                    if (t > 3)
                    {
                        if (H <= 0.05 * t && H <= 0.5)
                            _strR = "B级，h<= 0.05* t h <= 0.5: " + "Yes";
                        else if (H <= 0.1 * t && H <= 0.5)
                            _strR = "C级，h<= 0.1 * t  h <= 0.5: " + "Yes";
                        else if (H <= 0.2 * t && H <= 1)
                            _strR = "D级，h<=0.2 * t  h <= 1: " + "Yes";
                        else
                            _strR = "h > 0.2 * t  h >= 1: " + "No";
                    }
                    else
                    {
                        if (H <= 0.1 * t)
                            _strR = "C级，h<= 0.1 * t : " + "Yes";
                        else if (H <= 0.2 * t)
                            _strR = "D级，h<=0.2 * t: " + "Yes";
                        else
                            _strR = "h > 0.2 * t : " + "No";
                    }
                }
                else if (iType == 3)//下沉
                {
                    if (t > 3)
                    {
                        if (H <= 0.05 * t && H <= 0.5)
                            _strR = "B级，h<= 0.05* t h <= 0.5: " + "Yes";
                        else if (H <= 0.1 * t && H <= 1)
                            _strR = "C级，h<= 0.1 * t  h <=1: " + "Yes";
                        else if (H <= 0.25 * t && H <= 2)
                            _strR = "D级，h<=0.25 * t  h <= 2: " + "Yes";
                        else
                            _strR = "h > 0.25 * t  h >= 1: " + "No";
                    }
                    else
                    {
                        if (H <= 0.1 * t)
                            _strR = "C级，h<= 0.1 * t : " + "Yes";
                        else if (H <= 0.25 * t)
                            _strR = "D级，h<=0.25 * t: " + "Yes";
                        else
                            _strR = "h > 0.25 * t : " + "No";
                    }
                }
            }
            else//背面
            {
                if (t > 3)
                {
                    if (H <= 0.05 * t && H <= 0.5)
                        _strR = "B级，h<= 0.05* t h <= 0.5: " + "Yes";
                    else if (H <= 0.1 * t && H <= 1)
                        _strR = "C级，h<= 0.1 * t  h <=1: " + "Yes";
                    else if (H <= 0.2 * t && H <= 2)
                        _strR = "D级，h<=0.2 * t  h <= 2: " + "Yes";
                    else
                        _strR = "h > 0.2 * t  h >= 1: " + "No";
                }
                else
                {
                    if (H <= 0.1 * t)
                        _strR = "C级，h<= 0.1 * t : " + "Yes";
                    else if (H <= 0.1 * t+0.2)
                        _strR = "D级，h<=0.1 * t+0.2: " + "Yes";
                    else
                        _strR = "h > 0.1 * t +0.2: " + "No";
                }
            }
            return _strR;
        }

        private unsafe void DrawImageProflie(ref Label hWnd, ref double MaxX, ref double MaxZ,
          ref double ScaleX, ref double ScaleZ, ref double[] Axis_X, ref double[] Axis_Z, ref uint AxisCount,
          ref List<Class_Alarm> m_lstAlarm)
        {
            m_showProfileBusy = true;
            Bitmap bmp = new Bitmap((int)hWnd.Width, (int)hWnd.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.FromArgb(0, 0, 0));// g.Clear(Color.FromArgb(0, 125, 125));
            Color colorGreen = Color.FromArgb(0, 200, 0);//颜色为绿色
            Color colorB = Color.FromArgb(255, 0, 0);//颜色为绿色
            Color colorRed = Color.FromArgb(255, 0, 0);//颜色为红色
            Color colorGray = Color.FromArgb(200, 200, 200);//颜色为白色
            Color colorBlack = Color.FromArgb(30, 30, 30);//颜色为黑色
            Color colorYellow = Color.FromArgb(200, 200, 0);//颜色为黑色
            Pen PGreen = new Pen(colorGreen, 2);//创建一个画笔对象,该画笔的颜色为绿色，笔触大小为2个像素
            Pen pRed = new Pen(colorB, 4);//创建一个画笔对象,该画笔的颜色为黄色，笔触大小为1个像素
            Pen pGray = new Pen(Color.FromArgb(200, 200, 200), 1);//创建一个画笔对象,该画笔的颜色为灰色，笔触大小为1个像素

            Pen pGray_R = new Pen(Color.FromArgb(0, 0, 255), 1);//创建一个画笔对象,该画笔的颜色为灰色，笔触大小为1个像素
            Pen pBalck = new Pen(colorBlack, 3);//创建一个画笔对象,该画笔的颜色为白色，笔触大小为3个像素
            Pen pYellow = new Pen(colorYellow, 3);//创建一个画笔对象,该画笔的颜色为白色，笔触大小为1个像素
            pGray.DashStyle = DashStyle.Dot;
            Font font = new Font("Adobe Gothic Std", 9f, FontStyle.Bold);
            Brush brush = new SolidBrush(Color.FromArgb(255, 255, 255)); //Brush brush = new SolidBrush(Color.FromArgb(0, 0, 0));
            Brush brush_R = new SolidBrush(Color.FromArgb(255, 0, 0));
            uint axisInfo_top = 20;
            uint axisInfo_right = (uint)hWnd.Width - 20;
            uint axisInfo_left = 20;
            uint axisInfo_bottom = (uint)hWnd.Height - 30;
            g.DrawLine(pBalck, axisInfo_left, axisInfo_bottom, axisInfo_left, axisInfo_top);
            g.DrawLine(pBalck, axisInfo_left, axisInfo_bottom, axisInfo_right, axisInfo_bottom);
            //需设置一个页面的X及Z的显示范围及分度格数
            uint xNum, zNum;
            xNum = (uint)(MaxX / ScaleX);//x方向格子总数量
            zNum = (uint)(MaxZ / ScaleZ);//z方向格子总数量
            double XZoom, ZZoom;
            XZoom = (axisInfo_right - axisInfo_left) / MaxX;
            ZZoom = (axisInfo_bottom - axisInfo_top) / MaxZ;
            float scaleXlen = (float)(ScaleX * XZoom);
            float scaleZlen = (float)(ScaleZ * ZZoom);
            string str;
            //绘制刻度
            for (uint i = 1; i <= zNum; i++)
            {
                g.DrawLine(pGray, axisInfo_left, axisInfo_bottom - scaleZlen * i, axisInfo_right, axisInfo_bottom - scaleZlen * i);
                str = (ScaleZ * i).ToString();
                g.DrawString(str, font, brush, axisInfo_left - 10, axisInfo_bottom - scaleZlen * i);
            }
            g.DrawString("测量区域 ： 10-40", font, brush, 10, 0);
            for (uint j = 1; j <= xNum; j++)
            {
                switch (j)
                {
                    case 2:
                    case 8://pYellow
                        g.DrawLine(pYellow, axisInfo_left + scaleXlen * j, axisInfo_bottom, axisInfo_left + scaleXlen * j, axisInfo_top);
                        break;
                    case 5:
                        g.DrawLine(pGray_R, axisInfo_left + scaleXlen * j, axisInfo_bottom, axisInfo_left + scaleXlen * j, axisInfo_top);
                        break;
                    default:
                        g.DrawLine(pGray, axisInfo_left + scaleXlen * j, axisInfo_bottom, axisInfo_left + scaleXlen * j, axisInfo_top);
                        break;
                }
                g.DrawLine(j == 5 ? pGray_R : pGray, axisInfo_left + scaleXlen * j, axisInfo_bottom, axisInfo_left + scaleXlen * j, axisInfo_top);
                str = (ScaleX * j).ToString();

                g.DrawString(str, font, brush, axisInfo_left - 20 + scaleXlen * j, axisInfo_bottom);
            }
            //绘制轮廓线
            if (AxisCount > 10)
            {
                int _ddd = (int)(m_dbOutCentWeld * 10);
                int _iLstNum = m_lstAlarm.Count();

                for (uint k = 0; k < AxisCount - 1; k++)
                {
                    if (_ddd > 0 && k == _ddd)
                    {
                        g.DrawString("余高：" + m_dbOutCentWeld_H.ToString("f2") + "mm", font, brush, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(-48 + axisInfo_bottom - ZZoom * Axis_Z[k]));
                        g.DrawString("宽度：" + m_dbOutCentWeld_W.ToString("f2") + "mm", font, brush, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(-25 + axisInfo_bottom - ZZoom * Axis_Z[k]));


                        g.DrawLine(pRed, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(-6 + axisInfo_bottom - ZZoom * Axis_Z[k]),
                                  (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(6 + axisInfo_bottom - ZZoom * Axis_Z[k + 0]));
                    }
                    else if (m_lstAlarm.Count > 0)
                    {
                        int _iShowNo = -1;
                        for (int _iNo = 0; _iNo < m_lstAlarm.Count; _iNo++)
                        {
                            try
                            {
                                if (k >= m_lstAlarm[_iNo].i_Start && k <= m_lstAlarm[_iNo].i_End)
                                {
                                    if (k == m_lstAlarm[_iNo].i_Start)
                                    {
                                        g.DrawString((m_lstAlarm[_iNo].i_Type == 0 ? "凹" : "凸") + "宽：" + ((m_lstAlarm[_iNo].i_End - m_lstAlarm[_iNo].i_Start) / 10f).ToString("f2") + "mm", font, brush, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(35 + axisInfo_bottom - ZZoom * Axis_Z[k]));
                                        g.DrawString("高：" + m_lstAlarm[_iNo].dbDepth.ToString("f2") + "mm", font, brush, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(55 + axisInfo_bottom - ZZoom * Axis_Z[k]));
                                    }
                                    _iShowNo = _iNo;
                                    break;
                                }
                            }
                            catch (Exception e2)
                            { }
                        }
                        if (_iShowNo > -1)
                        {

                            g.DrawLine(pRed, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(axisInfo_bottom - ZZoom * Axis_Z[k]),
                                      (float)(axisInfo_left + XZoom * Axis_X[k + 1]), (float)(axisInfo_bottom - ZZoom * Axis_Z[k + 1]));
                        }
                        else
                            g.DrawLine(m_dbOutCentWeld > 0 && k == m_dbOutCentWeld * 10 ? pRed : PGreen, (float)(axisInfo_left + XZoom * Axis_X[k]),
                                      (float)(axisInfo_bottom - ZZoom * Axis_Z[k]), (float)(axisInfo_left + XZoom * Axis_X[k + 1]),
                                      (float)(axisInfo_bottom - ZZoom * Axis_Z[k + 1]));

                    }
                    else
                        g.DrawLine(m_dbOutCentWeld > 0 && k == m_dbOutCentWeld * 10 ? pRed : PGreen, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(axisInfo_bottom - ZZoom * Axis_Z[k]), (float)(axisInfo_left + XZoom * Axis_X[k + 1]),
                                  (float)(axisInfo_bottom - ZZoom * Axis_Z[k + 1]));
                }
            }
            hWnd.CreateGraphics().DrawImage(bmp, 0, 0);
            pRed.Dispose(); PGreen.Dispose(); pYellow.Dispose();
            pGray.Dispose(); pBalck.Dispose(); font.Dispose();
            brush.Dispose(); bmp.Dispose(); g.Dispose();
            pGray_R.Dispose();
            m_showProfileBusy = false;
        }

        /// <summary>
        /// 4.2 绘制轮廓图
        /// </summary>
        /// <param name="hWnd"></param>
        public unsafe void Draw_YuanShiTu(ref Label hWnd)
        {

            DrawRawImage(ref hWnd, m_PImageData, m_ImageWidth, m_ImageHeight);
        }
        /// <summary>
        /// 6 焊缝中心点
        /// </summary>
        /// <returns></returns>
        public double Center_weld()
        {
            return m_dbOutCentWeld;
        }
        /// <summary>
        /// 5 销毁轮廓仪:程序退出前需执行，避免内存泄漏
        /// </summary>
        public void DestroyCamera()
        {
            LS_HD6.LSHD6_DestroyCamera(1);

        }
        /// <summary>
        /// 7 激光线左右边缘高度校正
        /// </summary>
        /// <param name="LineStart">计算角度起始端位置</param>
        /// <param name="LineEnd">计算角度结束端位置</param>
        public void LeftAdRightHorizontal_correction(double LineStart = 1, double LineEnd = 25)
        {
            if (LineStart > -1 && LineStart < LineEnd && LineEnd < 1280)
            {
                m_dbAngle = LS_HD6.LSHD6_CalDeviceAngle(m_lsNumber, LineStart, LineEnd);
                LS_HD6.LSHD6_SetDeviceAngle(m_lsNumber, -1 * m_dbAngle);
            }
        }
        public void LeftAdRightHorizontal_correction_Get(double LineStart = 1, double LineEnd = 25)
        {
            if (LineStart > -1 && LineStart < LineEnd && LineEnd < 1280)
            {
                m_dbAngle = LS_HD6.LSHD6_CalDeviceAngle(m_lsNumber, LineStart, LineEnd);
            }
        }
        public void LeftAdRightHorizontal_correction_Set()
        {
            LS_HD6.LSHD6_SetDeviceAngle(m_lsNumber, -1 * m_dbAngle);
        }
        #region 其他函数
        /// <summary>
        /// 1 打开关闭激光器  
        /// 关闭激光器，轮廓数据为-99.999 
        /// 打开激光器，轮廓数据恢复正常值
        /// </summary>
        /// <param name="blVal"></param>
        public void SetLaser(bool blVal)
        {
            if (blVal)
            {
                LS_HD6.LSHD6_SetLaserOn(m_lsNumber);
                Console.WriteLine("轮廓仪" + m_lsNumber + "打开激光");
            }
            else
            {
                LS_HD6.LSHD6_SetLaserOff(m_lsNumber);
                Console.WriteLine("轮廓仪" + m_lsNumber + "关闭激光");
            }
        }
        /// <summary>
        ///  2 无效点填补
        /// </summary>
        /// <param name="type">补间类型，0不补间，1垂直补间，2直线补间</param>
        /// <param name="compansationCnt">补间距离阈值，连续无效点数量小于该值则转换为有效点</param>
        public void  SetCompansation(int type, int compansationCnt)
        {
            switch (type )
            {
                case 0:
                    LS_HD6.LSHD6_SetCompansation(m_lsNumber, 0, 0);
                    break;
                case 1:
                    LS_HD6.LSHD6_SetCompansation(m_lsNumber, 1, compansationCnt);
                    break;
                case 2:
                    LS_HD6.LSHD6_SetCompansation(m_lsNumber, 2, compansationCnt);
                    break;
            }
        }
        /// <summary>
        /// 4 曝光时间
        /// </summary>
        /// <param name="dbVal">dbVal 曝光时间（单位微秒，一般使用100~3000)</param>
        /// <returns></returns>
        public bool SetExposureTime(double dbVal)
        {
            bool ret = LS_HD6.LSHD6_SetExposureTime(m_lsNumber, dbVal);
            if (ret)
                Console.WriteLine("轮廓仪" + m_lsNumber + "曝光值已修改");
            return ret;
        }
        /// <summary>
        /// 5 获得帧率
        /// </summary>
        /// <returns></returns>
        public double GetFrameRate()
        {
            return LS_HD6.LSHD6_GetFrameRate(m_lsNumber);
        }
        /// <summary>
        ///  6 设置滤波
        /// </summary>
        /// <param name="type"> type 滤波类型，0不滤波，1均值滤波，2中值滤波，3均值滤波+中值滤波</param>
        /// <param name="SmoothNumber">均值滤波值: 奇数值</param>
        /// <param name="SmoothTimes">均值滤波次数</param>
        /// <param name="MidNumber">中值滤波值</param>
        public void  SetFilter(int type, int SmoothNumber, int SmoothTimes, int MidNumber)
        {
            switch (type )
            {
                case 0:
                    LS_HD6.LSHD6_SetFilter(m_lsNumber, 0, 0, 0, 0);
                    break;
                case 1:
                    LS_HD6.LSHD6_SetFilter(m_lsNumber, 1, SmoothNumber, SmoothTimes, MidNumber);// 9, 1, 9);
                    break;
                case 2:
                    LS_HD6.LSHD6_SetFilter(m_lsNumber, 2, SmoothNumber, SmoothTimes, MidNumber); //0, 0, 9);
                    break;
                case 3:
                    LS_HD6.LSHD6_SetFilter(m_lsNumber, 3, SmoothNumber, SmoothTimes, MidNumber);// 9, 1, 9);
                    break;
            }
        }
        /// <summary>
        /// 7 显示原始图
        /// </summary>
        public void ShowYuanShiTu()
        {
            LS_HD6.LSHD6_SetshowUI(m_lsNumber, false, true);
            LS_HD6.LSHD6_SetSingleCallBackMode(m_lsNumber, true);
            m_showUIFlag = 2;

          //  Txt_Data.Visible = true;
          //  ShowFrame();
        }
        /// <summary>
        /// 8 显示轮廓图
        /// </summary>
        public void ShowLunKuoTu()
        {
            LS_HD6.LSHD6_SetshowUI(m_lsNumber, false, false);
            m_showUIFlag = 1;

           // Txt_Data.Visible = false;
          //  ShowFrame();
        }
        private void ShowFrame()
        {
            if (Txt_Data.Visible == false)
            {
            //    Txt_Data.Visible = true;
              //  button41.Text = "报文隐藏";
            }
            else
            {
         //       Txt_Data.Visible = false;
            //    button41.Text = "报文显示";
            }
        }
        #endregion 其他函数
        /// <summary>
        /// 打开标定文件
        /// </summary>
        /// <param name="cameraParamFile"></param>
        /// <returns></returns>
        private bool OpenFile(string cameraParamFile = "SN8-0050W-1207839")
        {
            bool ret = false;
            // string cameraParamFile = "SN8-0050W-1207839";
            ret = LS_HD6.LSHD6_SetCameraParameter(m_lsNumber, cameraParamFile);
            if (ret == true)
            {
                Console.WriteLine("轮廓仪" + m_lsNumber + "配置文件:" + cameraParamFile + "加载成功");
            }
            else
            {
                Console.WriteLine("轮廓仪" + m_lsNumber + "配置文件加载失败");
            }
            return ret;
        }
        /// <summary>
        /// 相机初始化
        /// </summary>
        /// <returns></returns>
        private bool InitCam( string strSearial,string strHead="H")
        {
            bool _blRet = false;

            string cameraSerial = strHead + strSearial;// "S1207839";//SN8-0050W-1207839      S1207839
            UInt32 ret = LS_HD6.LSHD6_InitialCameraWithoutUI(m_lsNumber, cameraSerial, 10, 200, true, 0);//m_lsNumber, cameraSerial,100, 30, true, 0);
            if (ret == 0)
            {
                Console.WriteLine("轮廓仪" + m_lsNumber + "序列号" + cameraSerial + "初始化成功");
                LS_HD6.LSHD6_SetshowUI(m_lsNumber, false, false);
                LS_HD6.LSHD6_SetSingleCallBack(m_lsNumber, m_callBackSingle);
                LS_HD6.LSHD6_SetBatchCallBack(m_lsNumber, m_callBackBatch);
                //LS_HD6.LSHD6_GetProfileSize(m_lsNumber, ref m_maxX, ref m_maxZ);
                uint A, B, C, D;
                A = B = C = D = 0;
                _blRet = LS_HD6.LSHD6_GetCameraROI(1, ref A, ref B, ref C, ref D);
                Console.WriteLine(A);
                Console.WriteLine(B);
                Console.WriteLine(C);
                Console.WriteLine(D);
                _blRet &= LS_HD6.LSHD6_SetCameraROI(1, 96, 300, 1088, 600, 0, 0, 0, 0);
                m_scaleX = m_maxX / 10;
                m_scaleZ = m_maxZ / 10;
            }
            else
            {
                Console.WriteLine("轮廓仪" + cameraSerial + "初始化失败，错误代码：" + ret);
            }
            return _blRet;
        }
        /// <summary>
        /// 3 触发方式：软件触发0/编码器1/
        /// </summary>
        /// <param name="iType"> 0：软件触发 1：编码器触发</param>
        /// <returns></returns>
        public bool Trigger(int iType = 0, double freq = 20)
        {
            bool _blRet = false;
            if (iType == 0)
                _blRet &= Set_LowSpeedGrab(freq);
            else if (iType == 1)
                _blRet &= EncoderTrigger();
            return _blRet;
        }
        /// <summary>
        /// 低速内部触发
        /// </summary>
        private bool Set_LowSpeedGrab(double freq=20)
        {
            bool ret = LS_HD6.LSHD6_LowSpeedGrab(m_lsNumber, freq);
            if (ret == true)
            {
                Console.WriteLine("轮廓仪" + m_lsNumber + "低速连续取像开始");
             //   Time_Video.Start();
            }
            else
            {
                Console.WriteLine("轮廓仪" + m_lsNumber + "低速连续取像失败");
            }
            return ret;
        }
        /// <summary>
        /// 编码器触发
        /// </summary>
        private bool EncoderTrigger()
        {
            bool ret = LS_HD6.LSHD6_EncoderTriggerHighSpeedConstantScan(m_lsNumber);
            if (ret)
            {
                Console.WriteLine("轮廓仪" + m_lsNumber + "连续编码器触发高速扫描开始");
            }
            else
            {
                Console.WriteLine("轮廓仪" + m_lsNumber + "连续编码器触发高速扫描失败");
            }
            return ret;
        }
        #region 输出显示报文、中心点、原始图、轮廓图
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="Value"></param>
        delegate void Delg_ShowTiTl();
        private void MsgShow()
        {
            Txt_Data.Text = m_strGetDat;
            Txt_CentWelb.Text = m_dbOutCentWeld.ToString("f1");
        }
       // private unsafe void Time_ShowRun(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    #region 显示中心点
        ////    MsgShow();
          
        //    #endregion 显示中心点
        //    #region 画图
        //    switch (m_showUIFlag)
        //    {
        //        case 1:
        //            if (m_showProfileBusy == false)
        //            {
        //                DrawImageProflie(ref Lb_YuanShiTu);
        //            }
        //            break;
        //        case 2:
        //            if (m_showRawImageBusy == false && m_PImageData != null)
        //            {
        //                DrawRawImage(ref Lb_LunKuoTu);
        //            }
        //            break;
        //    }
        //    #endregion 画图
        //}
        ///// <summary>
        ///// 画图
        ///// </summary>
        ///// <param name="YuanShiTu"></param>
        ///// <param name="LunKuoTu"></param>
        //public unsafe void  PlantVideo(ref System.Windows.Forms.Label YuanShiTu, ref System.Windows.Forms.Label LunKuoTu)
        //{
        //    switch (m_showUIFlag)
        //    {
        //        case 1:
        //            if (m_showProfileBusy == false)
        //            {
        //                DrawImageProflie(ref YuanShiTu);
        //            }
        //            break;
        //        case 2:
        //            if (m_showRawImageBusy == false && m_PImageData != null)
        //            {
        //                DrawRawImage(ref LunKuoTu);
        //            }
        //            break;
        //    }
        //}
        #endregion 

        //绘制轮廓
        private unsafe void DrawRawImage(ref Label hWnd, byte* pImage, int width, int height)
        {
            if (width == 0 || height == 0) return;
            m_showRawImageBusy = true;
            IntPtr p = new IntPtr(pImage);
            Bitmap m_bitmap = new Bitmap(width, height, width, PixelFormat.Format8bppIndexed, p);
            ColorPalette palette = m_bitmap.Palette;
            for (int i = 0; i < 256; i++)
            {
                palette.Entries[i] = Color.FromArgb(i, i, i);
            }
            m_bitmap.Palette = palette;
            Rectangle src = new Rectangle(0, 0, width, height);
            Rectangle dst = new Rectangle(0, 0, hWnd.Width, hWnd.Height);
            hWnd.CreateGraphics().DrawImage(m_bitmap, dst, src, GraphicsUnit.Pixel);
            m_bitmap.Dispose();
            m_showRawImageBusy = false;
        }


        #region 回调函数
        private static   byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");//FE1001050103A000D
            if ((hexString.Length % 2) != 0)
                hexString = "0" + hexString;
            byte[] returnBytes = new byte[hexString.Length / 2];
            try
            {
                for (int i = 0; i < returnBytes.Length; i++)
                    returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);//Convert.ToString(166, 16)
            }
            catch { }
            return returnBytes;
        }
        /******用户自定义回调函数*******/
        private unsafe LS_HD6.pCallbackSingleProfile m_callBackSingle = new LS_HD6.pCallbackSingleProfile(HD6_singleCallBack);
        /// <summary>
        /// 单轮廓回调函数  陈大伟 计算
        /// </summary>
        /// <param name="bufferX"></param>
        /// <param name="bufferZ"></param>
        /// <param name="profileCnt"></param>
        /// <param name="pImageData"></param>
        /// <param name="imageWidth"></param>
        /// <param name="imageHeight"></param>
        public unsafe static void HD6_singleCallBack(double* bufferX, double* bufferZ, uint profileCnt, byte* pImageData, UInt32 imageWidth, UInt32 imageHeight)
        {
            //  bool _blOldWeld = false;
            //存储轮廓数据
            m_strGetDat = "";
            double _dbStartDat = bufferZ[0];
            int _iArrNum = 20;//采集数组个数
            double[] _ArrFind_Data = new double[_iArrNum];//通过30个点的有效数值，
            int[] _ArrPositio_L = new int[_iArrNum];//发现30个点的左边有效位置，
            int[] _ArrPositio_R = new int[_iArrNum];//发现30个点的有效右边位置，
            Class_Alarm _AlarmData = new Class_Alarm();//临时缺陷特征数据

            double _dbLval = 0, _dbL_MuCai_H = 0;
            double _dbLowDep = 900, _dbMaxDep = 0;//最小大值
            int _iL_Num = 0, _iFindStart = 0, _iNo_W = -1;
            bool _blMcHd = false, _blOk = false;//, _blFindAlarm = false;//母材厚度是否计算,是否缺陷
            Class_X_Data _X_Data = new Class_X_Data();
            _X_Data.m_flThick = m_flThick;

            _X_Data.m_blZm1_Bm0 = m_blZm1_Bm0;// , 
            double[] _ArrY_Err_L = new double[profileCnt];//左侧直线X对应计算点与实际值的误差
            double[] _ArrY_Err_R = new double[profileCnt];//右侧侧直线X对应计算点与实际值的误差
            Double _k1 = m_Line_1.K / 10f;
            Double _k2 = m_Line_2.K / 10f;
            double _dbVal_L = 0, _dbVal_R = 0;//俩差值
            uint _profileCnt = profileCnt - 1;
            Class_ball _clBall = new Class_ball();//统计凸起
            Lien_Line_Point _Point_JiaoDian = new Lien_Line_Point();
            double _Y = 0f, _Y2 = 999;//计算处和焊缝的焦点
            double _T = 0;//临时变量

            #region 0-5  45-50范围是计算两头线斜率区域
            if (profileCnt > 0)
            {
                
                int _Weld_iStart = -1, _Weld_iEnd = -1;
                for (int i = 0; i < 100; i++)
                {
                    if (bufferZ[i] > 0 && _Weld_iStart == -1)
                        _Weld_iStart = i;
                    if (bufferZ[500 - 1 - i] > 0 && _Weld_iEnd == -1)
                        _Weld_iEnd = 500 - 1 - i;
                    if (_Weld_iEnd > 0 && _Weld_iStart > 0) break;
                }
                if (_Weld_iEnd == 0) _Weld_iEnd = 500;

                m_Line_1 = m_CalLin.CalCu_Fit_Line(bufferX, bufferZ, profileCnt, _Weld_iStart, _Weld_iStart > 50 ? _Weld_iStart + 30 : 50);//150
           //     m_Line_2 = m_CalLin.CalCu_Fit_Line(bufferX, bufferZ, profileCnt, _Weld_iEnd < 350 ? _Weld_iEnd - 50 : 350, _Weld_iEnd);
                m_Line_2 = m_CalLin.CalCu_Fit_Line(bufferX, bufferZ, profileCnt, _Weld_iEnd < 350 ? _Weld_iEnd - 30 : 450, _Weld_iEnd);
                if (Math.Abs(m_Line_1.K) < 0.5 && Math.Abs(m_Line_2.K) < 0.7)
                {
                    m_Line_3.blOk = false;
                    m_Weld_Type = 0;
                }
                else
                {
                    m_Weld_Type = 1;
                    m_Point_JiaoDian = m_CalLin.L_L_P(m_Line_1, m_Line_2);
                    m_Line_3 = m_CalLin.Arct_Line(m_CalLin.Arct_K(m_Line_1, m_Line_2), m_Point_JiaoDian);
                    m_Line_3.blOk = (m_Point_JiaoDian.x > 15 && m_Point_JiaoDian.x < 450);
                }
                _X_Data.m_blDuijie0_JiaoHan1 = m_Weld_Type == 1;
                _k1 = m_Line_1.K / 10f;
                _k2 = m_Line_2.K / 10f;
            }
            #endregion 
            m_profileCnt = profileCnt;
            _WeldPosition.m_blAlarm = false;
            _WeldPosition.i_Start = -1;
            _WeldPosition.i_Cent = -1;
            if (m_iGet_ArrZ == 0)
            {
                m_Z_Out = new byte[1008+3+26];//1037
                m_iGet_ArrZ = 1;
            }
            
            for (int i = 0; i < profileCnt; i++)
            {
                #region 1 计算数据基础
                m_dbT = bufferZ[i];
                if (m_iGet_ArrZ == 1)
                {
                    m_ArrBt_Z = new byte[2];
                    m_ArrBt_Z[0] = 0;
                    m_ArrBt_Z[1] = 0;
                    m_ArrBt_Z = strToToHexByte((m_dbT * 100).ToString("f0"));
                    m_Z_Out[i * 2] = m_ArrBt_Z[0];//低位
                    if(m_ArrBt_Z.Length ==2)
                    m_Z_Out[i * 2 + 1] = m_ArrBt_Z[1];//高位
                }
                m_profileX[i] =bufferX[i];
                m_profileZ[i] = m_dbT;

                //当前数据与左右直线的误差
                _dbVal_L = m_dbT - (_k1 * i + m_Line_1.b);
                _ArrY_Err_L[i] = _dbVal_L;
                _dbVal_R = m_dbT - (_k2 * i + m_Line_2.b);
                _ArrY_Err_R[i] = _dbVal_R;
                #endregion  1
                if (m_Line_3.blOk)//角接焊缝
                {
                    #region 1 寻找两条线和焊缝的左右正向交点
                    if (_dbVal_L > 0.6 && _X_Data.m_clsWeld.m_i_W_Start_X == -1)//离开 0.2
                    {
                        _blOk = true;
                        for (int _iL = i + 1; _iL < i + 4 && _iL < profileCnt; _iL++)
                        {
                            if (bufferZ[_iL] - (_k1 * _iL + m_Line_1.b) < 0.55)//0.15
                            {
                                _blOk = false; break;
                            }
                        }
                        if (_blOk)
                        {
                            _X_Data.m_clsWeld.m_i_W_Start_X = i / 10f;
                            _X_Data.m_clsWeld.m_i_W_Start_Y = m_dbT;
                        }
                    }
                    if (_X_Data.m_clsWeld.m_i_W_Start_X > 0 &&
                        _dbVal_R < 0.5 && _X_Data.m_clsWeld.m_i_W_End_X == -1)//回来  0.2
                    {
                        _blOk = true;
                        for (int _iL = i + 1; _iL < i + 4 && _iL < profileCnt; _iL++)
                        {
                            _T = bufferZ[_iL] - (_k1 * _iL + m_Line_1.b);
                            if (_T <= 0.45)//= 0.15
                            {
                                _blOk = false; break;
                            }
                        }
                        if (_blOk)
                        {
                            _X_Data.m_clsWeld.m_i_W_End_X = i / 10f;
                            _X_Data.m_clsWeld.m_i_W_End_Y = m_dbT;
                        }
                    }
                    #endregion  

                    #region 2 平分线和焊缝面的交点
                    if (_X_Data.m_clsWeld.m_i_W_Start_X > 0)//找到第一交点后再找平分线交点
                    {
                        _Y = Math.Abs(m_dbT - (m_Line_3.K * i / 10f + m_Line_3.b));
                        if (_Y < _Y2)
                        {
                            _Y2 = _Y;
                            _Point_JiaoDian.x = i / 10;
                            _Point_JiaoDian.y = m_dbT;
                        }
                    }
                    #endregion 寻找经过交点的第三条线和焊缝面的交点
                }

                if (m_dbT >= 0 && m_dbT < _dbLowDep) _dbLowDep = m_dbT;//搜索最小值
                if (m_dbT >= 0 && _dbMaxDep < m_dbT) _dbMaxDep = m_dbT;//搜索最大

                m_strGetDat += (i == 0 ? "" : " , ") + m_dbT.ToString("f3");

                if (m_dbT > 0)//水平线已校正并有效数据m_blAngle_JZ
                {
                    #region 1 计算母材平均高度
                    if (i < 30 && m_dbT > 0)
                    {
                        _iL_Num++;
                        _dbL_MuCai_H += m_dbT;
                    }
                    else if (_blMcHd == false && i == 30 && _iL_Num > 0)
                    {
                        _blMcHd = true;
                        _dbL_MuCai_H /= _iL_Num;
                    }
                    #endregion 1 计算母材平均高度
                 
                    #region 2 寻找凹陷的缺陷
                    #region 2.1 咬边凹陷、母材上的凹陷 特征：低于母材 先找与母材0.4的落差点，然后往后找母材起始点，
                    //再继续找最深，最后找与母材同等深度的回升点，完成初次
                    //此宽度不能大于焊缝宽度
                    //2.1.1 回头找起点
                    if (_AlarmData.i_Start == -1 && i + 3 < profileCnt &&
                        (m_dbT - bufferZ[i + 3] > m_Limit_Alarm_H &&
                        m_dbT - bufferZ[i + 2] > m_Limit_Alarm_H &&
                        m_dbT - bufferZ[i + 1] > m_Limit_Alarm_H
                        ))
                    {
                        _iFindStart = i;// -1;
                        for (int _iNo = i - 1; _iNo > i - 5; _iNo--)//往回找与母材交汇的起点
                        {
                            if (_iNo > 0)
                            {
                                _dbLval = bufferZ[_iNo] - bufferZ[_iNo + 1];
                                if (!(_dbLval > 0 && _dbLval > 0.05))
                                {
                                    _iFindStart = _iNo;
                                    break;
                                }
                            }
                        }
                        if (i - _iFindStart < 10)
                        {
                            i += 3;
                            _AlarmData.i_Type = 0;
                            _AlarmData.dbDepth = m_dbT;
                            _AlarmData.i_Start = _iFindStart;
                            _AlarmData.dbDepth_S = m_dbT;
                        }
                    }
                    //2.1.2 找最深处
                    if (m_dbT > 0 && _AlarmData.i_Type == 0 && _AlarmData.i_Start > 0 && _AlarmData.dbDepth > m_dbT)//_AlarmData.i_End == -1 &&
                    {
                        _AlarmData.dbDepth = m_dbT;
                    }
                    //2.1.3 找回升终点
                    if (_AlarmData.i_Type == 0 && _AlarmData.i_Start > 0 && _AlarmData.dbDepth_S > 0 &&//前提
                              (m_dbT - _AlarmData.dbDepth_S > 0 ||//右边高
                              _AlarmData.dbDepth_S - m_dbT > 0 && //左边高
                              (m_dbT - _AlarmData.dbDepth > m_Limit_Alarm_H)))// Math.Abs ( m_dbT - _AlarmData.dbDepth_S )<= 0.1)
                    {
                        if (m_dbT - _dbL_MuCai_H < _dbL_MuCai_H - bufferZ[i - 1])
                            _AlarmData.i_End = i;
                        else
                            _AlarmData.i_End = i - 1;
                        if (_AlarmData.i_End - _AlarmData.i_Start > 2 &&
                            _AlarmData.dbDepth_S - _AlarmData.dbDepth > m_Limit_Alarm_H)
                        {
                            if (_AlarmData.dbDepth_S - m_dbT > 0)//左边高时调整起点
                            {
                                _AlarmData.dbDepth_S = bufferZ[_AlarmData.i_End];//返回找
                                _AlarmData.dbDepth = 900;
                                for (int _iN = _AlarmData.i_End - 1; _iN > _AlarmData.i_Start; _iN--)
                                {
                                    if (bufferZ[_iN] > 0 && _AlarmData.dbDepth > bufferZ[_iN])
                                        _AlarmData.dbDepth = bufferZ[_iN];//找最低处
                                    if (bufferZ[_iN] > _AlarmData.dbDepth_S)
                                    {
                                        _AlarmData.i_Start = _iN;
                                        break;
                                    }
                                }
                            }
                            //终止当前缺陷
                            Class_Alarm _NewAlarm = new Class_Alarm();
                            _NewAlarm.i_Type = 0;
                            _NewAlarm.i_Start = _AlarmData.i_Start;
                            _NewAlarm.i_End = _AlarmData.i_End;
                            _NewAlarm.dbDepth = _AlarmData.dbDepth_S - _AlarmData.dbDepth;

                            if (_NewAlarm.i_End - _NewAlarm.i_Start > 3 && _NewAlarm.dbDepth > m_Limit_Alarm_H)
                            {
                                _X_Data.m_lstAlarm.Add(_NewAlarm);
                                _X_Data.iDownNum++;
                            }

                            _AlarmData = new Class_Alarm(); //开始下一个
                        }
                    }
                    #endregion  2.1 咬边凹陷
                    #endregion  2 选择缺陷
                }

                #region 3 寻找焊缝与母材左右交点位置 焊缝位置
                if (_clBall.iStart == -1 && (m_Line_3.blOk == false || m_Line_3.blOk && i < m_Point_JiaoDian.x) && _dbVal_L >= 0.1)//
                {
                    _blOk = false ;
                    double _db1 = _dbVal_L, _db2 = 0;
                    int _iNum = 0;
                    if (i > 50)
                    {
                        for (int _iNo = i + 3; _iNo < i + 17 && _iNo + 16 < profileCnt; _iNo++)// for (int _iNo = i + 3; _iNo < i + 7 && _iNo + 4 < profileCnt; _iNo++)
                        {
                            _db2 = bufferZ[_iNo] - (_k1 * _iNo + m_Line_1.b);
                            if (Math.Abs(_db2)> _fl_Limit_L)// if (  _db2 - _dbVal_L < _fl_Limit_L )//0.04
                            {
                                _iNum++;
                                if (_iNum > 10)
                                {
                                    _blOk = true;
                                    break;
                                }
                            }
                            _db1 = _db2;
                        }
                    }
                    if (_blOk)
                    {
                        _clBall.dbStartVal = _dbVal_L;
                        _clBall.iStart = i;
                        _clBall.flHeight = _dbVal_L;
                        _clBall.iHeightPoint = i;
                    }
                }
                if (_clBall.iStart > 0)//找终点位置
                {
                    if (_clBall.flHeight < _dbVal_L)
                    {
                        _clBall.flHeight = _dbVal_L;
                        _clBall.iHeightPoint = i;
                    }

                    //---右边降落判断-

                    _blOk = false ;
                    double _db1 = _dbVal_L, _db2 = 0, _dbK = 0;
                    int _iNum = 0;
                    for (int _iNo = i + 6; _iNo < i + 16 && _iNo + 15 < profileCnt; _iNo++)// for (int _iNo = i + 3; _iNo < i + 7 && _iNo + 4 < profileCnt; _iNo++)
                    {
                        _db1 = bufferZ[_iNo];
                        _dbK = (_k2 * _iNo + m_Line_2.b);
                        _db2 = _db1 - _dbK;
                        if (Math .Abs (  _db2 ) < _fl_Limit_R)// if (_db2 - _dbVal_L < _fl_Limit_R)//0.04
                        {
                            _iNum++;
                            if (_iNum > 9)
                            {
                                _blOk = true;
                                break;
                            }
                        }
                    }
                    //---


                    if (_clBall.iStart > 0 && _blOk&& // _dbVal_L > -0.2 && _dbVal_L < 0.2 && 
                         _clBall.flHeight > 0.4 && i - _clBall.iStart > 50)//焊缝高度    焊缝最小宽度
                    {

                        if (_AlarmData.i_Start == -1 || _AlarmData.i_Start > 0)// && i < _AlarmData.i_Start)
                        {//不能因为直线右上扬出现凹坑
                            _clBall.iDownNum++;
                            if (_clBall.iDownNum > 0 )//保证下降数据阶段平稳了
                            {
                                _clBall.iEnd = i;//结束

                                //终止当前缺陷
                                Class_Alarm _NewAlarm = new Class_Alarm();
                                _NewAlarm.i_Type = 1;
                                _NewAlarm.i_Start = _clBall.iStart;
                                _NewAlarm.i_End = _clBall.iEnd;
                                _NewAlarm.dbDepth = _clBall.flHeight;
                                _NewAlarm.dbDepth_S = _clBall.dbStartVal;

                                if (_WeldPosition.i_Start ==-1)
                                {
                                    _WeldPosition.i_Start = _clBall.iStart;
                                    _WeldPosition.i_End = _clBall.iEnd;
                                    _WeldPosition.dbDepth = _clBall.flHeight;
                                    _WeldPosition.i_Cent = _clBall.iStart+(int)(( _clBall.iEnd- _clBall.iStart)/2);
                                    _WeldPosition.f_K =(float)_k1;
                                }
                                else
                                {
                                    if(_WeldPosition.i_Start + _WeldPosition.i_End- _WeldPosition.i_Start >
                                        _clBall.iStart + _clBall.iEnd - _clBall.iStart)
                                    {
                                        _WeldPosition.i_Start = _clBall.iStart;
                                        _WeldPosition.i_End = _clBall.iEnd;
                                        _WeldPosition.dbDepth = _clBall.flHeight;
                                        _WeldPosition.i_Cent = _clBall.iStart + (int)((_clBall.iEnd - _clBall.iStart) / 2);
                                        _WeldPosition.f_K = (float)_k1;
                                    }
                                }
                                _WeldPosition.m_blAlarm = true ;
                                _X_Data.m_lstAlarm.Add(_NewAlarm);

                                _clBall = new Class_ball();//开始下一个
                            }
                        }
                    }
                }

                #endregion  3 搜索焊缝左右位置
            }

            if (m_iGet_ArrZ == 1)
            {
                m_ArrBt_Z = new byte[2];
                m_ArrBt_Z[0] = 0;
                m_ArrBt_Z[1] = 0;
                m_ArrBt_Z = strToToHexByte((_WeldPosition.dbDepth * 100).ToString("f0"));//余高
                m_Z_Out[1000] = m_ArrBt_Z[0];
                if (m_ArrBt_Z.Length > 1)
                    m_Z_Out[1001] = m_ArrBt_Z[1];

                m_ArrBt_Z = new byte[2];
                m_ArrBt_Z[0] = 0;
                m_ArrBt_Z[1] = 0;
                m_ArrBt_Z = strToToHexByte((_WeldPosition.i_Cent).ToString("f0"));//中心点
                m_Z_Out[1002] = m_ArrBt_Z[0];
                if (m_ArrBt_Z.Length > 1) m_Z_Out[1003] = m_ArrBt_Z[1];

                m_ArrBt_Z = new byte[2];
                m_ArrBt_Z[0] = 0;
                m_ArrBt_Z[1] = 0;
                m_ArrBt_Z = strToToHexByte((_WeldPosition.i_Start).ToString("f0"));//焊缝开始
                m_Z_Out[1004] = m_ArrBt_Z[0];
                if (m_ArrBt_Z.Length > 1) m_Z_Out[1005] = m_ArrBt_Z[1];

                m_ArrBt_Z = new byte[2];
                m_ArrBt_Z[0] = 0;
                m_ArrBt_Z[1] = 0;
                m_ArrBt_Z = strToToHexByte((_WeldPosition.i_End).ToString("f0"));//焊缝结束
                m_Z_Out[1006] = m_ArrBt_Z[0];
                if (m_ArrBt_Z.Length > 1) m_Z_Out[1007] = m_ArrBt_Z[1];

                m_Z_Out[1034] = 0xFE;
                m_Z_Out[1035] = 0xFE;
                m_Z_Out[1036] = 0xFE;
                //m_ArrBt_Z[0] = 0;
                //m_ArrBt_Z[1] = 0;
                //m_ArrBt_Z = strToToHexByte((_X_Data.m_clsWeld.m_dbCentWeld_W * 10).ToString("f0"));//焊缝宽
                //m_Z_Out[1006] = m_ArrBt_Z[0];
                //m_Z_Out[1007] = m_ArrBt_Z[1];

                //m_ArrBt_Z[0] = 0;
                //m_ArrBt_Z[1] = 0;
                //m_ArrBt_Z = strToToHexByte((_X_Data.m_clsWeld.db_Line_3_Mucai_H * 10).ToString("f0"));//焊缝总高
                //m_Z_Out[1008] = m_ArrBt_Z[0];
                //m_Z_Out[1009] = m_ArrBt_Z[1];

                //---位置
                //m_ArrBt_Z[0] = 0;
                //m_ArrBt_Z[1] = 0;
                //m_ArrBt_Z = strToToHexByte((_X_Data.m_clsWeld.db_Line_3_Mucai_H * 10).ToString("f0"));//焊缝总高
                //m_Z_Out[1008] = m_ArrBt_Z[0];
                //m_Z_Out[1009] = m_ArrBt_Z[1];
                m_iGet_ArrZ = 2;
            }
            #region 5 添加缺陷
            
            #region 5.1 过滤
            if (_X_Data.m_clsWeld.m_i_W_Start_X > 5 && _X_Data.m_clsWeld.m_i_W_End_X > 5)// AV型升降明显母材：计算宽度和余高
            {
                Lien_Line_Point _P_L = new Lien_Line_Point();
                Lien_Line_Point _P_R = new Lien_Line_Point();
                _P_L.x = _X_Data.m_clsWeld.m_i_W_Start_X;
                _P_L.y = _X_Data.m_clsWeld.m_i_W_Start_Y;
                _P_R.x = _X_Data.m_clsWeld.m_i_W_End_X;
                _P_R.y = _X_Data.m_clsWeld.m_i_W_End_Y;


                #region 5.1.1 计算余高
                _X_Data.m_clsWeld.db_Line_3_Mucai_H = m_CalLin.Distanc_Towpoint(_Point_JiaoDian, m_Point_JiaoDian);

                _X_Data.m_clsWeld.m_dbCentWeld_W = m_CalLin.Distanc_Towpoint(_P_L, _P_R);
                //点到直线距离
                _X_Data.m_clsWeld.db_Line_LR_X = m_Point_JiaoDian.x;
                _X_Data.m_clsWeld.db_Line_LR_Y = m_Point_JiaoDian.y;
                _X_Data.m_clsWeld.m_dbPoint_Line4 = m_CalLin.Distanc_Point_Line(_P_L, _P_R, m_Point_JiaoDian.x, m_Point_JiaoDian.y);
                _X_Data.m_clsWeld.m_dbCentWeld_H = Math.Abs(_X_Data.m_clsWeld.db_Line_3_Mucai_H -
                                                            _X_Data.m_clsWeld.m_dbPoint_Line4);
                if (_X_Data.m_clsWeld.m_dbCentWeld_H > 0) _X_Data.m_clsWeld.m_blHave = true;

                //余高过低和过高都不合格，但标准是什么

                #endregion  5.1.1 计算余高

                #region 5.1.2判断焊瘤位置是否合格
                #region 5.1.2.1 找母材交点边的长边

                double _L_z = m_CalLin.Distanc_Towpoint(_P_L, m_Point_JiaoDian);//母材左点与交点距离
                double _L_y = m_CalLin.Distanc_Towpoint(m_Point_JiaoDian, _P_R);//母材右点与交点距离
                Lien_Line_Point _Point_4 = new Lien_Line_Point();//新点坐标
                double h = 0, w = 0;//高、宽

                #endregion 5.1.2.1

                #region 5.1.2.2 根据两条线的斜率  与要计算交点左右边，计算新交点位置
                if (_L_z < _L_y)//求X1右边点
                {
                    w = _L_z / Math.Sqrt(1 + m_Line_2.K * m_Line_2.K);
                    h = Math.Abs(m_Line_2.K) * w;
                    if (m_Line_2.K > 0)
                    {
                        _Point_4.x = m_Point_JiaoDian.x + w;
                        _Point_4.y = m_Point_JiaoDian.y + h;
                    }
                    else
                    {
                        _Point_4.x = m_Point_JiaoDian.x + w;
                        _Point_4.y = m_Point_JiaoDian.y - h;
                    }
                    _X_Data.m_clsWeld.m_Point_4_a = m_CalLin.Distanc_Point_Line(_Point_4, _P_R, m_Point_JiaoDian.x, m_Point_JiaoDian.y);
                    _X_Data.m_clsWeld.m_Point_4_h = m_CalLin.Distanc_Towpoint(_P_R, _Point_4);
                }
                else//左边
                {
                    w = _L_y / Math.Sqrt(1 + m_Line_1.K * m_Line_1.K);
                    h = Math.Abs(m_Line_1.K) * w;
                    if (m_Line_1.K > 0)
                    {
                        _Point_4.x = m_Point_JiaoDian.x - w;
                        _Point_4.y = m_Point_JiaoDian.y - h;
                    }
                    else
                    {
                        _Point_4.x = m_Point_JiaoDian.x - w;
                        _Point_4.y = m_Point_JiaoDian.y + h;
                    }
                    _X_Data.m_clsWeld.m_Point_4_a = m_CalLin.Distanc_Point_Line(_P_L, _Point_4, m_Point_JiaoDian.x, m_Point_JiaoDian.y);
                    _X_Data.m_clsWeld.m_Point_4_h = m_CalLin.Distanc_Towpoint(_P_L, _Point_4);
                }
                _X_Data.m_clsWeld.m_Point_4_x = _Point_4.x;
                _X_Data.m_clsWeld.m_Point_4_y = _Point_4.y;

                csJL _JL = new csJL();
                _JL.iType = 512;
                _JL.strType = "角焊过渡不对称";
                if (_X_Data.m_clsWeld.m_Point_4_h <= (1.5 + 0.15 * _X_Data.m_clsWeld.m_Point_4_h))
                {
                    _JL.iJL = 1;
                    _JL.strJL = "B级 h <= (1.5+0.15*a :Yes";
                }
                else if (_X_Data.m_clsWeld.m_Point_4_h <= (0.2 + 0.15 * _X_Data.m_clsWeld.m_Point_4_h))
                {
                    _JL.iJL = 1;
                    _JL.strJL = "C级 h <= (0.2 + 0.15 *a :Yes";
                }
                else if (_X_Data.m_clsWeld.m_Point_4_h <= (0.2 + 0.2 * _X_Data.m_clsWeld.m_Point_4_h))
                {
                    _JL.iJL = 1;
                    _JL.strJL = "C级 h <= (0.2 + 0.2 *a :Yes";
                }
                else
                {
                    _JL.iJL = 0;
                    _JL.strJL = "h > (0.2 + 0.2 *a ";
                }
                _X_Data.m_clsWeld.m_lstJL.Add(_JL);

                _X_Data.m_clsWeld.blJL &= (_JL.iJL == 1);
                _X_Data.blJL &= _X_Data.m_clsWeld.blJL;
                #endregion 5.1.2.2
                #endregion  5.1.2

                #region 5.1.3 焊缝凹陷类型判断
                for (int _i = 0; _i < _X_Data.m_lstAlarm.Count; _i++)
                {
                    if (_X_Data.m_lstAlarm[_i].i_Type == 0 && 
                        (_X_Data.m_lstAlarm[_i].i_End <= _P_L.x ||//在母材左边：咬边 得验证，要低于母材
                        _X_Data.m_lstAlarm[_i].i_Start >= _P_R.x))
                    {
                        _blOk = false ;
                        for (int _No = _X_Data.m_lstAlarm[_i].i_Start; _No <= _X_Data.m_lstAlarm[_i].i_End; _No++)
                            if (_ArrY_Err_L[_No] > 0.5)
                                _blOk = true;
                        _X_Data.m_lstAlarm[_i].i_Type = _blOk == false?-1: 2;
                    }

                    if (_X_Data.m_lstAlarm[_i].i_Type == 0 &&
                        _X_Data.m_lstAlarm[_i].i_Start > _P_L.x + 3 &&//在母材上面
                        _X_Data.m_lstAlarm[_i].i_End < _P_R.x + 3)
                        _X_Data.m_lstAlarm[_i].i_Type = 3;
                }
                #endregion  5.1.3
            }
            #endregion 5.1 过滤

            #region 5.2 拿缺陷
            if (m_Dic_SysBuff.g_Dic_Alarm.ContainsKey(m_iKey_Dist_Frame))//同距离数据就修改
            {
                m_Dic_SysBuff.g_Dic_Alarm[m_iKey_Dist_Frame].m_blBall = _X_Data.m_blBall;
                m_Dic_SysBuff.g_Dic_Alarm[m_iKey_Dist_Frame].m_blAlarm = false;
                m_Dic_SysBuff.g_Dic_Alarm[m_iKey_Dist_Frame].m_iprofileCnt = (int)profileCnt;
                m_Dic_SysBuff.g_Dic_Alarm[m_iKey_Dist_Frame].m_dbL_Min_H = _dbLowDep;
                m_Dic_SysBuff.g_Dic_Alarm[m_iKey_Dist_Frame].m_dbL_Max_H = _dbMaxDep;
                var IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(m_Dic_SysBuff.g_Dic_Alarm[m_iKey_Dist_Frame].m_Arr_profileX, 0);
                Marshal.Copy(m_profileX, 0, IntPtArr, (int)profileCnt);
                IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(m_Dic_SysBuff.g_Dic_Alarm[m_iKey_Dist_Frame].m_Arr_profileZ, 0);
                Marshal.Copy(m_profileZ, 0, IntPtArr, (int)profileCnt);

                #region 伤点
                m_Dic_SysBuff.g_Dic_Alarm[m_iKey_Dist_Frame].m_lstAlarm.Clear();
                for (int _iNo = 0; _iNo < _X_Data.m_lstAlarm.Count; _iNo++)
                {
                    Class_Alarm _NewAlarm = new Class_Alarm();
                    _NewAlarm.i_Type = _X_Data.m_lstAlarm[_iNo].i_Type;
                    _NewAlarm.i_Start = _X_Data.m_lstAlarm[_iNo].i_Start;
                    _NewAlarm.i_End = _X_Data.m_lstAlarm[_iNo].i_End;
                    _NewAlarm.dbDepth = _X_Data.m_lstAlarm[_iNo].dbDepth;
                    _NewAlarm.dbDepth_S = _X_Data.m_lstAlarm[_iNo].dbDepth_S;

                    m_Dic_SysBuff.g_Dic_Alarm[m_iKey_Dist_Frame].m_lstAlarm.Add(_NewAlarm);
                }
                m_Dic_SysBuff.g_Dic_Alarm[m_iKey_Dist_Frame].m_blAlarm = _X_Data.m_lstAlarm.Count > 0;
                #endregion 伤点
                #region 焊缝
                m_Dic_SysBuff.g_Dic_Alarm[m_iKey_Dist_Frame].m_clsWeld.m_blHave = _X_Data.m_clsWeld.m_blHave;
                m_Dic_SysBuff.g_Dic_Alarm[m_iKey_Dist_Frame].m_clsWeld.iType = _X_Data.m_clsWeld.iType;
                m_Dic_SysBuff.g_Dic_Alarm[m_iKey_Dist_Frame].m_clsWeld.m_i_W_Start_X = _X_Data.m_clsWeld.m_i_W_Start_X;
                m_Dic_SysBuff.g_Dic_Alarm[m_iKey_Dist_Frame].m_clsWeld.m_i_W_Start_Y = _X_Data.m_clsWeld.m_i_W_Start_Y;
                m_Dic_SysBuff.g_Dic_Alarm[m_iKey_Dist_Frame].m_clsWeld.m_i_W_End_X = _X_Data.m_clsWeld.m_i_W_End_X;
                m_Dic_SysBuff.g_Dic_Alarm[m_iKey_Dist_Frame].m_clsWeld.m_i_W_End_Y = _X_Data.m_clsWeld.m_i_W_End_Y;
                m_Dic_SysBuff.g_Dic_Alarm[m_iKey_Dist_Frame].m_clsWeld.db_Line_LR_X = _X_Data.m_clsWeld.db_Line_LR_X;
                m_Dic_SysBuff.g_Dic_Alarm[m_iKey_Dist_Frame].m_clsWeld.db_Line_LR_Y = _X_Data.m_clsWeld.db_Line_LR_Y;
                m_Dic_SysBuff.g_Dic_Alarm[m_iKey_Dist_Frame].m_clsWeld.db_Line_3_Mucai_H = _X_Data.m_clsWeld.db_Line_3_Mucai_H;
                m_Dic_SysBuff.g_Dic_Alarm[m_iKey_Dist_Frame].m_clsWeld.i_MinDist_X = _X_Data.m_clsWeld.i_MinDist_X;
                m_Dic_SysBuff.g_Dic_Alarm[m_iKey_Dist_Frame].m_clsWeld.i_MinDist_Y = _X_Data.m_clsWeld.i_MinDist_Y;
                m_Dic_SysBuff.g_Dic_Alarm[m_iKey_Dist_Frame].m_clsWeld.m_dbCentWeld_H = _X_Data.m_clsWeld.m_dbCentWeld_H;
                m_Dic_SysBuff.g_Dic_Alarm[m_iKey_Dist_Frame].m_clsWeld.m_dbCentWeld_W = _X_Data.m_clsWeld.m_dbCentWeld_W;
                m_Dic_SysBuff.g_Dic_Alarm[m_iKey_Dist_Frame].m_clsWeld.m_dbPoint_Line4 = _X_Data.m_clsWeld.m_dbPoint_Line4;
          
                m_Dic_SysBuff.g_Dic_Alarm[m_iKey_Dist_Frame].m_clsWeld.m_lstJL.Clear();
                for (int _i = 0; _i < _X_Data.m_clsWeld.m_lstJL.Count; _i++)
                {
                    csJL _JL = new csJL();
                    _JL.iType = _X_Data.m_clsWeld.m_lstJL[_i].iType;
                    _JL.strType = _X_Data.m_clsWeld.m_lstJL[_i].strType;
                    _JL.strJL = _X_Data.m_clsWeld.m_lstJL[_i].strJL;
                    _JL.iJL = _X_Data.m_clsWeld.m_lstJL[_i].iJL;

                    m_Dic_SysBuff.g_Dic_Alarm[m_iKey_Dist_Frame].m_clsWeld.m_lstJL.Add(_JL);
                }
                m_Dic_SysBuff.g_Dic_Alarm[m_iKey_Dist_Frame].m_clsWeld.blJL = _X_Data.m_clsWeld.blJL;
                m_Dic_SysBuff.g_Dic_Alarm[m_iKey_Dist_Frame].blJL = _X_Data.blJL;
                #endregion 焊缝
            }
            else
            {
                _X_Data.m_blAlarm = _X_Data.m_lstAlarm.Count > 0;
                _X_Data.m_iprofileCnt = (int)profileCnt;
                _X_Data.m_dbL_Min_H = _dbLowDep;
                _X_Data.m_dbL_Max_H = _dbMaxDep;

                var IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_X_Data.m_Arr_profileX, 0);
                Marshal.Copy(m_profileX, 0, IntPtArr, (int)profileCnt);
                IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_X_Data.m_Arr_profileZ, 0);
                Marshal.Copy(m_profileZ, 0, IntPtArr, (int)profileCnt);

                m_Dic_SysBuff.g_Dic_Alarm.Add(m_iKey_Dist_Frame, _X_Data);
            }
            #endregion 5.2 拿缺陷

            #endregion 5 添加缺陷
            m_dbStart = _ArrY_Err_L[0];
            m_dbEnd = _ArrY_Err_L[490];
            //存储图像数据
            m_PImageData = pImageData;
            m_ImageWidth = (int)imageWidth;
            m_ImageHeight = (int)imageHeight;
            m_strTemperature = LS_HD6.LSHD6_GetTemperature(1).ToString("f2");
        }
        //private  byte Chg_10_TO_Byte(int iData)
        //{
        //    byte _RetByte = 0;
        //    _RetByte= Convert.ToByte( Convert.ToString(iData, 16).ToString (), 16);
        //    return _RetByte;
        //}
        public unsafe static void HD6_singleCallBack_Old(double* bufferX, double* bufferZ, uint profileCnt, byte* pImageData, UInt32 imageWidth, UInt32 imageHeight)
        {
            //存储轮廓数据
            m_strGetDat = "";
            double _dbStartDat = bufferZ[0];
            //double _dbEnd = bufferZ[profileCnt - 3];
            //if (profileCnt > 0 && _dbEnd>0 && _dbStartDat>0) _dbStartDat = _dbStartDat < _dbEnd?_dbStartDat: _dbEnd;
            int _i_L = 0, _i_R = 0, _i_LastPosition = 0;//存储左右数组序号
            int _iArrNum = 20;//采集数组个数
            float _flLimit = 0.25f;//采集数据间隔
            double[] _ArrFind_Data = new double[_iArrNum];//通过30个点的有效数值，
            int[] _ArrPositio_L = new int[_iArrNum];//发现30个点的左边有效位置，
            int[] _ArrPositio_R = new int[_iArrNum];//发现30个点的有效右边位置，
            double _dbWc = 0, _dbWc_Next = 0;
            bool _blDown = false;
            Class_Alarm _AlarmData = new Class_Alarm();//临时缺陷特征数据

            double _dbLval = 0, _dbL_MuCai_H = 0;
            double _dbLowDep = 900,_dbMaxDep=0;//最小大值
            int _iL_Num = 0, _iFindStart = 0, _iNo_W = -1;
            bool _blMcHd = false;//母材厚度是否计算,是否缺陷
            Class_X_Data _X_Data = new Class_X_Data();

            #region 0-5  45-50范围是计算两头线斜率区域

            #endregion 
            m_profileCnt = profileCnt;
            for (int i = 0; i < profileCnt; i++)
            {
                m_dbT = bufferZ[i];
                m_profileX[i] = bufferX[i]; 
                m_profileZ[i] = m_dbT;
             
                if (m_dbT >= 0 && m_dbT < _dbLowDep) _dbLowDep = m_dbT;//搜索最小值
                if (m_dbT >= 0 && _dbMaxDep < m_dbT) _dbMaxDep = m_dbT;//搜索最大

                m_strGetDat += (i == 0 ? "" : " , ") + m_dbT.ToString("f3");

                #region 缺陷寻找 最好用斜率来计算，因为适应对角焊缝

                #endregion  缺陷寻找

                if (  m_dbT > 0)//水平线已校正并有效数据m_blAngle_JZ
                {
                    #region 1 计算母材平均高度
                    if (i < 50 && m_dbT > 0)
                    {
                        _iL_Num++;
                        _dbL_MuCai_H += m_dbT;
                    }
                    else if (_blMcHd == false && i == 50 && _iL_Num > 0)
                    {
                        _blMcHd = true;
                        _dbL_MuCai_H /= _iL_Num;
                    }
                    #endregion 1 计算母材平均高度
                    #region 2 寻找缺陷
             //       if (_blMcHd)
                    {
                        #region 2.1 咬边凹陷、母材上的凹陷 特征：低于母材 先找与母材0.4的落差点，然后往后找母材起始点，
                        //再继续找最深，最后找与母材同等深度的回升点，完成初次
                        //此宽度不能大于焊缝宽度
                        //2.1.1 回头找起点
                        if (_AlarmData.i_Start == -1 && i+3 < profileCnt &&
                            (m_dbT- bufferZ[i+3]   > m_Limit_Alarm_H&&
                            m_dbT - bufferZ[i + 2] > m_Limit_Alarm_H&&
                            m_dbT - bufferZ[i + 1] > m_Limit_Alarm_H
                            ))
                        {
                            _iFindStart = i;// -1;
                            for (int _iNo = i - 1; _iNo > i - 5; _iNo--)//往回找与母材交汇的起点
                            {
                                if (_iNo > 0)
                                {
                                    _dbLval = bufferZ[_iNo] - bufferZ[_iNo + 1];
                                    if (! (_dbLval>0 && _dbLval > 0.05))
                                    {
                                       _iFindStart = _iNo;
                                        break;
                                    }
                                }
                            }
                            if (i- _iFindStart < 10)
                            {
                                _AlarmData.i_Type = 0;
                                _AlarmData.dbDepth = m_dbT;
                                _AlarmData.i_Start = _iFindStart;
                                _AlarmData.dbDepth_S = m_dbT;
                            }
                        }
                        //2.1.2 找最深处
                        if (m_dbT>0 && _AlarmData.i_Type == 0 && _AlarmData.i_Start > 0 &&  _AlarmData.dbDepth > m_dbT)//_AlarmData.i_End == -1 &&
                        {
                            _AlarmData.dbDepth = m_dbT;
                        }
                        //2.1.3 找回升终点
                        if (_AlarmData.i_Type==0 && _AlarmData.i_Start > 0 && _AlarmData.dbDepth_S>0 &&//前提
                            (m_dbT - _AlarmData.dbDepth_S >=0 ||//右边高
                        _AlarmData.dbDepth_S  -  m_dbT  > 0&& //左边高
                        (m_dbT - _AlarmData.dbDepth > 0.4) ))// Math.Abs ( m_dbT - _AlarmData.dbDepth_S )<= 0.1)
                        {
                            if (m_dbT - _dbL_MuCai_H < _dbL_MuCai_H - bufferZ[i - 1])
                                _AlarmData.i_End = i;
                            else
                                _AlarmData.i_End = i - 1;
                            if (_AlarmData.i_End - _AlarmData.i_Start > 2 && 
                                _AlarmData.dbDepth_S - _AlarmData.dbDepth > 0.4)
                            {
                                if (_AlarmData.dbDepth_S - m_dbT > 0)//左边高时调整起点
                                {
                                    _AlarmData.dbDepth_S = bufferZ[_AlarmData.i_End];//返回找
                                    _AlarmData.dbDepth = 900;
                                    for (int _iN = _AlarmData.i_End -1; _iN > _AlarmData.i_Start; _iN--)
                                    {
                                        if (bufferZ[_iN]>0 &&  _AlarmData.dbDepth > bufferZ[_iN])
                                            _AlarmData.dbDepth = bufferZ[_iN];//找最低处
                                        if (bufferZ[_iN] > _AlarmData.dbDepth_S)
                                        {
                                            _AlarmData.i_Start = _iN;
                                            break;
                                        }
                                    }
                                }
                                //终止当前缺陷
                                Class_Alarm _NewAlarm = new Class_Alarm();
                                _NewAlarm.i_Start = _AlarmData.i_Start;
                                _NewAlarm.i_End = _AlarmData.i_End;
                                _NewAlarm.dbDepth = _AlarmData.dbDepth_S - _AlarmData.dbDepth ;

                                if(_NewAlarm.i_End - _NewAlarm.i_Start>4 && _NewAlarm.dbDepth >0.4)
                         //       _X_Data.m_lstAlarm.Add(_NewAlarm);
                                _AlarmData = new Class_Alarm(); //开始下一个
                            }
                        }
                        #endregion  2.1 咬边凹陷
                    }
                    #endregion  2 选择缺陷
                }

                #region 3 寻找焊缝左右位置
                if (_i_L < _iArrNum)
                {
                    if (_blDown == false && bufferZ[i] - _dbStartDat >= m_dbLimit_FindWeld)
                    {//上升阶段：找到间隔0.2的点
                        _dbStartDat = bufferZ[i];
                        _i_R = _i_L;//保存最后一个上升点位置

                        _ArrPositio_L[_i_L] = i;//当前位置
                        _ArrFind_Data[_i_L++] = _dbStartDat;//保存当前上升点的值

                    }
                    _dbWc = _dbStartDat - bufferZ[i];
                    if (_blDown == false && _i_R > 0 && _dbWc > m_dbLimit_FindWeld / 2)//提前判断，防止漏掉左边最后一个点
                    {
                        _blDown = true;
                        _i_R--;
                        _dbStartDat = _ArrFind_Data[_i_R];
                        _dbWc = 0;
                    }
                    if (_blDown && _i_R >= 0 && _dbWc > 0)//提前判断，防止漏掉左边最后一个点
                    {
                        //1 下降阶段：寻找和上升同样值的点，和两个点进行比较，寻找误差小的点
                        if (i + 1 < profileCnt)//和两个点进行比较
                        {
                            _dbWc_Next = _dbStartDat - bufferZ[i + 1];

                            //_dbWc = Math.Abs(_dbWc - m_dbLimit_FindWeld);
                            //_dbWc_Next = Math.Abs(_dbWc_Next - m_dbLimit_FindWeld);

                            if (_dbWc < _flLimit && _dbWc < _dbWc_Next)
                            {
                                _i_LastPosition = _i_R;
                                _ArrPositio_R[_i_R] = i;//保存当前下降点的位置
                            }

                            if (_dbWc_Next < _flLimit && _dbWc_Next < _dbWc)
                            {
                                _i_LastPosition = _i_R;
                                _ArrPositio_R[_i_R] = i + 1;//保存当前下降点的位置
                                i++;//直接下一个
                            }
                        }
                        //2 超出范围，找下一个点
                        //      if (_dbWc > m_dbLimit_FindWeld + 0.1)
                        {
                            _i_R--;//最终有可能下降部分会比上升阶段少两头两个点
                            if (_i_R >= 0)
                                _dbStartDat = _ArrFind_Data[_i_R];//下降一个点
                        }
                    }
                }
                #endregion  3 搜索焊缝左右位置
            }
            #region 4 根据焊缝左右位置确定焊缝中心位置
            if (_i_LastPosition >= 0)
            {
                int _iNum = 0, _iNoNum = -1, _iNo = -1;
                double _dbOutCentWeld = 0;
                for (int i = _i_LastPosition; i < _iArrNum; i++)
                {
                    if (_ArrPositio_L[i] > 0 && _ArrPositio_R[i] > 0)
                    {
                        if (_iNoNum == -1)
                            _iNoNum = 1;
                        else
                            _iNoNum++;
                        if (_iNoNum == 2)
                            _iNo = i;//寻找第二个
                        _iNum++;
                        _dbOutCentWeld += (_ArrPositio_R[i] - _ArrPositio_L[i]) / 10 / 2 + _ArrPositio_L[i] / 10;
                    }
                }
                m_dbOutCentWeld = 0;
                m_dbOutCentWeld_H = 0;
                m_dbOutCentWeld_W = 0;
         //       m_profileCnt = 0;
                if (_iNum >= 2 && _iNo > 0)
                {
                    m_dbOutCentWeld = _dbOutCentWeld / _iNum;
                    m_dbOutCentWeld_H = bufferZ[(int)(m_dbOutCentWeld * 10)];
                    m_dbOutCentWeld_H = m_dbOutCentWeld_H - ((bufferZ[_ArrPositio_L[_iNo]] > bufferZ[_ArrPositio_R[_iNo]] ?
                          bufferZ[_ArrPositio_R[_iNo]] : bufferZ[_ArrPositio_L[_iNo]]) - m_dbLimit_FindWeld * _iNo);
                    m_dbOutCentWeld_W = (_ArrPositio_R[_iNo] - _ArrPositio_L[_iNo]) / 10;
                   
                    _iNo_W = _iNo;
                }
            }
            #endregion 4 寻找焊缝中心位置

            #region 5 缺陷过滤
            if (_blMcHd)// profileCnt >0)//母材厚度确定_blMcHd
            {
                #region 5.1 过滤

                #endregion 5.1

                #region 5.2 拿缺陷
                // m_dbKey_FrameNo
                //整理当前帧
                _X_Data.m_blAlarm = _X_Data.m_lstAlarm.Count > 0;
                _X_Data.m_iprofileCnt = (int)profileCnt;
                //_X_Data.m_dbCentHeight = m_dbOutCentWeld_H;
                //if (_iNo_W > -1)
                //{
                //    _X_Data.m_i_W_Start = _ArrPositio_R[_iNo_W];
                //    _X_Data.m_i_W_End = _ArrPositio_L[_iNo_W];
                //    _X_Data.m_dbL_Min_H = _dbLowDep;
                //    _X_Data.m_dbL_Max_H = _dbMaxDep;
                //}
             var    IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_X_Data.m_Arr_profileX, 0);
                Marshal.Copy(m_profileX, 0, IntPtArr, (int)profileCnt);
                IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_X_Data.m_Arr_profileZ, 0);
                Marshal.Copy(m_profileZ, 0, IntPtArr, (int)profileCnt);
                //当前帧加入字典
                if (m_Dic_SysBuff.g_Dic_Alarm.ContainsKey(m_iKey_Dist_Frame))//同距离数据就修改
                    m_Dic_SysBuff.g_Dic_Alarm.Remove(m_iKey_Dist_Frame);
                m_Dic_SysBuff.g_Dic_Alarm.Add(m_iKey_Dist_Frame, _X_Data);

                if (m_ChuFa_Time0_Bmq1 == 0)
                {
                    // m_iKey_Dist_Frame+=5;
                }
                #endregion 5.2
            }
            //1 拷贝报文

            //2 拷贝缺陷
            #endregion 5
            //存储图像数据
            m_PImageData = pImageData;
            m_ImageWidth = (int)imageWidth;
            m_ImageHeight = (int)imageHeight;
            //Console.WriteLine("轮廓仪当前温度："+LS_HD6.LSHD6_GetTemperature(1));
        }
        private unsafe LS_HD6.pCallbackBatchProfile m_callBackBatch = new LS_HD6.pCallbackBatchProfile(HD6_batchCallBack);
        public unsafe static void HD6_batchCallBack(double** profileZ, double* profileX, int xcount, int ycount)
        {
            Console.WriteLine("轮廓仪" + m_lsNumber + "批量扫描结束");
            Console.WriteLine("轮廓总数量" + ycount);
            Console.WriteLine("第100条轮廓的第200个点X坐标：" + profileX[199]);
            Console.WriteLine("第100条轮廓的第200个点Z坐标：" + profileZ[99][199]);
        }

        #endregion 回调函数
        #endregion 输出方法
    }
    #region  直线类
    /// <summary>
    /// 拟合直线类
    /// </summary>
    public class ClassLine
    {
        /// <summary>
        /// Emgu拟合直线
        /// </summary>
        /// <param name="bufferX"></param>
        /// <param name="bufferZ"></param>
        /// <param name="profileCnt"></param>
        /// <param name="iStart"></param>
        /// <param name="iEnd"></param>
        /// <returns></returns>
        public unsafe LineK CalCu_Fit_Line(double* bufferX, double* bufferZ, uint profileCnt,
                                     int iStart, int iEnd)
        {
            LineK _Line = new LineK();
            PointF p1, p2;
            int _iNum = iEnd - iStart + (iStart==0?0: 1);
            if (_iNum <= 0) return _Line;

            PointF[] vp2 = new PointF[_iNum];
            for (int i = 0; i < _iNum; i++)
            {
                vp2[i].X = (float)bufferX[iStart];
                vp2[i].Y = (float)bufferZ[iStart++];
            }
            CvInvoke.FitLine(vp2, out p1, out p2, Emgu .CV.CvEnum . DistType.L1, 0, 0.01, 0.01);

            double k = p1.Y / p1.X;
            double c = p2.Y - k * p2.X;
            _Line.K = k;
            _Line.b = c;

            return _Line;
        }
        /// <summary>
        /// 求两条线的平分线
        /// </summary>
        /// <param name="k"></param>
        /// <param name="PointX"></param>
        /// <returns></returns>
        public LineK Arct_Line(double k, Lien_Line_Point PointX)
        {
            double _b = PointX.y - PointX.x * k;
            LineK _Line = new LineK();
            _Line.K = k;
            _Line.b = _b;
            return _Line;
        }
        /// <summary>
        /// 求两条母材线的平分线的斜率
        /// </summary>
        /// <param name="Line_1"></param>
        /// <param name="Line_2"></param>
        /// <returns></returns>
        public double Arct_K(LineK Line_1, LineK Line_2)
        {
            double _dbRet_1 = Math.Atan(Line_1.K) * 180f / Math.PI;
            double _dbRet_2 = Math.Atan(Line_2.K) * 180f / Math.PI;
            if (_dbRet_1 < 0) _dbRet_1 = 180 + _dbRet_1;
            if (_dbRet_2 < 0) _dbRet_2 = 180 + _dbRet_2;

            _dbRet_1 = Math.Abs(_dbRet_1 - _dbRet_2) / 2.0f + _dbRet_2;
            _dbRet_1 = _dbRet_1 * Math.PI / 180f;
            _dbRet_1 = Math.Tan(_dbRet_1);
            return _dbRet_1 ;
        }
        /// <summary>
        /// 两点间距离
        /// </summary>
        /// <param name="Point_1"></param>
        /// <param name="Point_2"></param>
        /// <returns></returns>
        public double Distanc_Towpoint(Lien_Line_Point Point_1, Lien_Line_Point Point_2)
        {
            double _dbRet = 0;
            _dbRet = Math.Sqrt((Point_1.x - Point_2.x)* (Point_1.x - Point_2.x) + (Point_1.y- Point_2.y )* (Point_1.y - Point_2.y));
            return _dbRet;
        }

        /// <summary>
        /// 已知两点求直线方程
        /// </summary>
        /// <param name="Point_1">点1</param>
        /// <param name="Point_2">点2</param>
        /// <returns></returns>
        public LineK Line_Point_1_2(Lien_Line_Point Point_1, Lien_Line_Point Point_2)
        {
            LineK _Line = new LineK();
            _Line.K = (Point_1.y - Point_2.y) / (Point_1.x - Point_2.x);
            _Line.b = Point_1.y - _Line.K * Point_1.x;

            return _Line;
        }

        /// <summary>
        /// 点到直线距离
        /// </summary>
        /// <param name="Point_1">直线点1</param>
        /// <param name="Point_2">直线点2</param>
        /// <param name="_x_3">直线外点 X</param>
        /// <param name="_y_3">直线外点 Y</param>
        /// <returns></returns>
        public double Distanc_Point_Line(Lien_Line_Point Point_1, Lien_Line_Point Point_2, double _x_3, double _y_3)
        {
            //直线方程系数  (Y2-Y1) x + （X1-X2）y +(X2-X1  )Y1 -(Y2-Y1) X1=0
            //A =(Y2-Y1)   B= （X1-X2）  C=(X2-X1  )Y1 -(Y2-Y1) X1
            //double _A = (_y_2 - _y_1);// / ( - );
            //double _B = _x_1 - _x_2;
            //double _C = (_x_2 - _x_1) * _y_1 - (_y_2 - _y_1) * _x_1;
            //点到直线距离 = | Ax+By+C|/Math.Sqrt(_A * _A + _B * _B)
            double _A = (Point_2.y - Point_1.y);// / ( - );
            double _B = Point_1.x - Point_2.x;
            double _C = (Point_2.x - Point_1.x) * Point_1.y - (Point_2.y - Point_1.y) * Point_1.x;

            return Math.Abs(_A * _x_3 + _B * _y_3 + _C) / Math.Sqrt(_A * _A + _B * _B);
        }
        /// <summary>
        /// 两条直线交点坐
        /// </summary>
        /// <param name="Line_1">直线1</param>
        /// <param name="Line_2">直线2</param>
        /// <returns></returns>
        public Lien_Line_Point L_L_P(LineK Line_1, LineK Line_2)
        {
            Lien_Line_Point _Point = new Lien_Line_Point();
            _Point.x = (Line_1.b - Line_2.b) / (Line_2.K - Line_1.K );
            _Point.y = Line_1.K * _Point.x + Line_1.b;
            return _Point;
        }
        /// <summary>
        /// 计算直线  Y= k x  +  b
        /// </summary>
        /// <param name="bufferX">x轴数据</param>
        /// <param name="bufferZ">y轴数据</param>
        /// <param name="profileCnt">数组总数据个数</param>
        /// <param name="iStart">直线的开始点</param>
        /// <param name="iEnd">直线的结束点</param>
        /// <returns></returns>
        public unsafe LineK CalCu_K_b(double* bufferX ,double* bufferZ, uint profileCnt,
                                     int iStart,int iEnd)
        {
            LineK _Line = new LineK();
            double _A = 0f;//x平方和
            double _B = 0f;//x和
            double _C = 0f;//x * y和
            double _D = 0f;//y和
            int iNum =  iEnd - iStart + (iStart==0?0: 1);//个数

            if (profileCnt <= 0) return _Line;
            if (iStart > profileCnt || iEnd > profileCnt) return _Line;
            
            for(int i=iStart;i<=iEnd;i++)
            {

                _A += bufferX[i] * bufferX[i];
                _B += bufferX[i] ;
                _C += bufferX[i] * bufferZ[i];
                _D += bufferZ[i];
            }
            _Line.K = (_C * iNum - _B * _D) / (_A * iNum - _B * _B);
            _Line.b  = (_A * _D  - _C  * _B ) / (_A * iNum - _B * _B);

            return _Line;
        }

        public unsafe LineK CalCu_K_b_Start_End(double* bufferX, double* bufferZ, uint profileCnt,
                                  int iStart, int iEnd,int iStart_2, int iEnd_2)
        {
            LineK _Line = new LineK();
            double _A = 0f;//x平方和
            double _B = 0f;//x和
            double _C = 0f;//x * y和
            double _D = 0f;//y和
            int iNum = iEnd - iStart + 1;//个数
            int iNum_2 = iEnd_2 - iStart_2 + 1;//个数

            if (profileCnt <= 0) return _Line;
            if (iStart > profileCnt || iEnd > profileCnt) return _Line;
            if (iStart_2 > profileCnt || iEnd_2 > profileCnt) return _Line;

            for (int i = iStart; i <= iEnd; i++)
            {

                _A += bufferX[i] * bufferX[i];
                _B += bufferX[i];
                _C += bufferX[i] * bufferZ[i];
                _D += bufferZ[i];
            }
            for (int i = iStart_2; i <= iEnd_2; i++)
            {

                _A += bufferX[i] * bufferX[i];
                _B += bufferX[i];
                _C += bufferX[i] * bufferZ[i];
                _D += bufferZ[i];
            }
            iNum += iNum_2;

            _Line.K = (_C * iNum - _B * _D) / (_A * iNum - _B * _B);
            _Line.b = (_A * _D - _C * _B) / (_A * iNum - _B * _B);

            return _Line;
        }
    }
    /// <summary>
    /// 直线方程 y=kx +b
    /// </summary>
    public class LineK
    {
        /// <summary>
        /// 直线是否拟合成功
        /// </summary>
     public    bool blOk = false;
        /// <summary>
        /// 斜率
        /// </summary>
        public double  K = 0.0f;
        /// <summary>
        /// 与Y的焦点
        /// </summary>
        public double b = 0.0f;
    }
    /// <summary>
    /// 两条直线交点坐标
    /// </summary>
    public class Lien_Line_Point
    {
        /// <summary>
        /// 点是否确定
        /// </summary>
        public bool  blOk = false;
        /// <summary>
        /// x
        /// </summary>
        public double x = 0.0f;
        /// <summary>
        /// y
        /// </summary>
        public double y = 0.0f;
    }

    #endregion  直线类
}
