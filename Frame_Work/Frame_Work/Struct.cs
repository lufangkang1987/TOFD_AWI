using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using ClassLib_TestData;
using System.Drawing.Drawing2D;
using Microsoft.Win32;
using System.Management;
using System.Threading;
namespace Frame_Work
{
    /// <summary>
    /// 公共数据集合
    /// </summary>
    public class ClassSys_Buff
    {
        /// <summary>
        /// 以距离为主键的数据字典 距离转换成0.1mm小数变整数
        /// </summary>
        public Dictionary<int, Class_X_Data> g_Dic_Alarm = new Dictionary<int, Class_X_Data>();
    }
    #region C扫描数据
    /// <summary>
    /// C扫描项目信息
    /// </summary>
    public class Class_C_ItemInfor
    {
        /// <summary>
        /// ID号
        /// </summary>
        public string ID;
        /// <summary>
        /// 委托单位
        /// </summary>
        public string strWtdw;
        /// <summary>
        /// 工程名称
        /// </summary>
        public string StrGcmc;

        /// <summary>
        /// 工程名称:老的
        /// </summary>
        public string StrGcmc_Old = "";
        /// <summary>
        /// 工件名称
        /// </summary>
        public string strGjmc;
        /// <summary>
        /// 检测方法
        /// </summary>
        public string strJcff;
        /// <summary>
        /// 检测机构名称
        /// </summary>
        public string strJcjg;
        /// <summary>
        /// 检测机构地址
        /// </summary>
        public string strJgdz;
        /// <summary>
        /// 邮编
        /// </summary>
        public string strYb;
        /// <summary>
        /// 检 测 员
        /// </summary>
        public string strJcry;
        /// <summary>
        /// 审    批
        /// </summary>
        public string strSpry;
        /// <summary>
        /// 检测日期
        /// </summary>
        public string strJyrq;
        /// <summary>
        /// 报告编号
        /// </summary>
        public string strBgbh;
        /// <summary>
        /// 记录编号
        /// </summary>
        public string strJlbh;
        /// <summary>
        /// 检测部件
        /// </summary>
        public string strJcbj;
        /// <summary>
        /// 热处理状态
        /// </summary>
        public string strRclzt;
        /// <summary>
        /// 表面状态:
        /// </summary>
        public string strBmzt;
        /// <summary>
        /// 承压设备类别:
        /// </summary>
        public string strSblb;
        /// <summary>
        ///检测比例
        /// </summary>
        public string strJcbl;
        /// <summary>
        ///  检测标准
        /// </summary>
        public string strJcbz;
        /// <summary>
        ///  合格级别
        /// </summary>
        public string strHgjb;
        /// <summary>
        /// 技术等级
        /// </summary>
        public string strJsdj;
        /// <summary>
        /// 仪器名称
        /// </summary>
        public string strYqmc;
        /// <summary>
        /// 仪器型号
        /// </summary>
        public string strYqxh;
        /// <summary>
        /// 仪器编号
        /// </summary>
        public string strYqbh;
        /// <summary>
        /// 仪器有效期
        /// </summary>
        public string strYxq;
        /// <summary>
        /// 仪器精度
        /// </summary>
        public string strYqjd;



        /// <summary>
        /// 设备材质:
        /// </summary>
        public string strCaiZhi;
        /// <summary>
        /// 设备编号:
        /// </summary>
        public string strGjbh;
        /// <summary>
        /// 设备形式:
        /// </summary>
        public string strSbxs;
        /// <summary>
        /// 设备规格:
        /// </summary>
        public string strGg;
        /// <summary>
        /// 检测面:
        /// </summary>
        public string strJcm;
        /// <summary>
        /// 检测位置
        /// </summary>
        public string strJcwz;
        /// <summary>
        /// 检测准备 1：开始  2：结束  0：取消
        /// </summary>
        public int i_S1_E2_N0 = 0;
        /// <summary>
        /// 当前C扫描缓存总行数 N
        /// </summary>
        public int i_C_AllRows = 0;
        /// <summary>
        /// 当前检测行号0-N
        /// </summary>
        public int i_CurrRow_No = -1;
        /// <summary>
        /// 停止检测提醒数据保存
        /// </summary>
        public bool bl_Ask_if_Save_Data = true;
    }
    /// <summary>
    /// C扫描缓存
    /// </summary>
    public class Class_C_Buff
    {
        #region C扫描
        /// <summary>
        /// 文件名称
        /// </summary>
        public string m_FileName = "";
        /// <summary>
        /// C扫描：多行全部检测数据
        /// </summary>
        public List<List<CLs_EMAT_Data>> m_Lst_C_Buff = new List<List<CLs_EMAT_Data>>();
        /// <summary>
        /// C扫描：单行数据
        /// </summary>
        public List<CLs_EMAT_Data> m_C_One_Buff = new List<CLs_EMAT_Data>();
        /// <summary>
        /// 多行异常数据汇总
        /// </summary>
        public List<List<CL_AlarmData>> m_Lst_Alarm_Buff = new List<List<CL_AlarmData>>();
        /// <summary>
        /// 当前行异常数据
        /// </summary>
        public List<CL_AlarmData> m_One_Alarm_Buff = new List<CL_AlarmData>();


        /// <summary>
        /// 涂层C扫描：多行全部检测数据
        /// </summary>
        public List<List<CLs_EMAT_Data>> m_Lst_C_Buff_Coat = new List<List<CLs_EMAT_Data>>();
        /// <summary>
        /// 涂层C扫描：单行数据
        /// </summary>
        public List<CLs_EMAT_Data> m_C_One_Buff_Coat = new List<CLs_EMAT_Data>();
        /// <summary>
        /// 记录实时数据
        /// </summary>
        public List<Cls_Coat_Dis_Mark> m_C_One_Buff_Coat_ThickDisMark = new List<Cls_Coat_Dis_Mark>();
        /// <summary>
        /// 涂层多行异常数据汇总
        /// </summary>
        public List<List<CL_AlarmData>> m_Lst_Alarm_Buff_Coat = new List<List<CL_AlarmData>>();
        /// <summary>
        /// 涂层当前行异常数据
        /// </summary>
        public List<CL_AlarmData> m_One_Alarm_Buff_Coat = new List<CL_AlarmData>();
        #endregion C

        #region 多通道数据缓存
        /// <summary>
        /// 多通道数据缓存
        /// </summary>
        public Cls_Mul_UI_Data m_Lst_Mul_One_Buff = new Cls_Mul_UI_Data();
        /// <summary>
        /// 多通道：多行全部数据
        /// </summary>
        public List<Cls_Mul_UI_Data> m_Lst_Mul_All_Buff = new List<Cls_Mul_UI_Data>();

        /// <summary>
        /// 涂层 多通道数据缓存
        /// </summary>
        public Cls_Mul_UI_Data m_Lst_Mul_One_Buff_Coat = new Cls_Mul_UI_Data();
        /// <summary>
        /// 涂层 多通道：多行全部数据
        /// </summary>
        public List<Cls_Mul_UI_Data> m_Lst_Mul_All_Buff_Coat = new List<Cls_Mul_UI_Data>();
        #endregion 多通道

        /// <summary>
        /// C扫描: 控制信息
        /// </summary>
        public CLs_C_Ctrl m_Ctrl = new CLs_C_Ctrl();
        /// <summary>
        /// C扫描：图像处理类
        /// </summary>
        public Cls_Plant_C m_Plant_C = new Cls_Plant_C();

        #region 设备
        /// <summary>
        /// 4轮控制
        /// </summary>
     //   public  Clb_MT_Comm.MT_Comm m_Climb4;
        #endregion 设备
    }

    /// <summary>
    /// 上移曲线参数
    /// </summary>
    public class Cls_Upward_Curve
    {
        /// <summary>
        /// Y轴标准起始位置
        /// </summary>
        public int i_Y_S = -1;
        /// <summary>
        /// Y轴搜索结束位置
        /// </summary>
        public int i_Y_E = -1;
        /// <summary>
        /// X轴移动区间开始位置
        /// </summary>
        public int i_X_S = -1;
        /// <summary>
        /// X轴移动区间结束位置
        /// </summary>
        public int i_X_E = -1;

    }
    /// <summary>
    /// 向量
    /// </summary>
    public class Cls_Vector
    {
        /// <summary>
        /// 开始
        /// </summary>
        public int iNo_S = 0;
        /// <summary>
        /// 结束
        /// </summary>
        public int iNo_E = 0;
        /// <summary>
        /// 升：0   降: 1
        /// </summary>
        public int i_Up0_Down1 = -1;
        /// <summary>
        /// 高度
        /// </summary>
        public int iHeight_Max = 0;
    }
    public class Cls_Kd
    {
        /// <summary>
        /// 开始刻度 mm
        /// </summary>
        public int i_Start = -1;
        /// <summary>
        /// 结束刻度 mm
        /// </summary>
        public int i_End = -1;
    }
    /// <summary>
    /// 数据控制
    /// </summary>
    public class CLs_C_Ctrl
    {
        /// <summary>
        /// 是否测试
        /// </summary>
        public bool m_bl_CS = false;
        /// <summary>
        /// 负压控制
        /// </summary>
        public bool m_bl_FY = false;


        /// <summary>
        /// 文件读写
        /// </summary>
        ClassInterFace csInter = new ClassInterFace();
        /// <summary>
        /// 配置文件名
        /// </summary>
        string HardFileName = Application.StartupPath + "\\database\\SysConfig.ini";
        /// <summary>
        /// 读文件主键
        /// </summary>
        string m_strSection = "Class_Ctrl";

        /// <summary>
        /// 运行状态 1：开始 0：停止 2: 检定完成  10:退出程序
        /// </summary>
        public int m_iRun = 0;
        /// <summary>
        /// 超声：采集数据间隔时间
        /// </summary>
        public int i_C_UIDLL_iWaitTime = 0;

        
        /// <summary>
        /// 当前缓存的X轴位置信息
        /// </summary>
        public int i_X_Curr_Pos = -1;
        /// <summary>
        /// X轴间隔点左右偏差范围：mm
        /// </summary>
        public int i_X_Deviation_LR = 2;

        /// <summary>
        /// 当前缓存的X轴位置信息
        /// </summary>
        public int i_Y_Curr_Pos = -1;

        /// <summary>
        /// 放电值
        /// </summary>
        public float fl_Spark_Val = 1000f;

        /// <summary>
        /// 缓存0->N-1,N：缓存当前实时指针
        /// </summary>
        public int i_Buff_Len = -1;
        /// <summary>
        /// 缓存0->N-1,N：缓存当前实时指针
        /// </summary>
        public int i_Buff_Len_Coat = -1;
        /// <summary>
        /// 当前刷新界面Y轴位置指针
        /// </summary>
        public int i_C_Y = -1;

        #region 多通道使用变量
        /// <summary>
        /// 最后一次接收的距离值
        /// </summary>
        public int m_i_Trip_Com_mm_Old = -1;
        /// <summary>
        /// 多通道原始数据链表实时数据位置
        /// </summary>
        public int m_i_Mul_OrigLstLen = -1;

        #endregion 

        public CLs_C_Ctrl()
        {
            Init();
        }
        /// <summary>
        /// 读写配置文件
        /// </summary>
        public void Init(int iType = 0)
        {
            string strPath = System.Windows.Forms.Application.StartupPath + "\\database\\HardConfig.ini";
            if (iType == 0)
            {

                i_C_UIDLL_iWaitTime = int.Parse(csInter.IniReadDefine("ClassUltrasGate", "m_iTimeDelay", "0", strPath));

            }
            else
            {
                csInter.INIWriteValue("ClassUltrasGate", "m_iTimeDelay", i_C_UIDLL_iWaitTime.ToString(), strPath);
            }
        }
        /// <summary>
        /// 读主键对应值
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public string Read_One(string strKey)
        {
            return csInter.IniReadDefine(m_strSection, strKey, "", HardFileName);
        }
        /// <summary>
        /// 写主键对应值
        /// </summary>
        /// <param name="iType"></param>
        public void Write_One(string strKey, string strVal)
        {
            csInter.INIWriteValue(m_strSection, strKey, strVal, HardFileName);
        }
    }
    #region 多通道数据
    /// <summary>
    /// 多通道数据缓存
    /// </summary>
    public class Cls_Mul_UI_Data
    {
        /// <summary>
        /// 当前开始检测填写的设备编号（工件编号），多通道以"-"间隔
        /// </summary>
        public string strGjbh = "";
        /// <summary>
        /// 检测时间 yyyy-MM-dd HHmmss
        /// </summary>
        public string strTime = "";
        /// <summary>
        /// 原始数据缓存
        /// </summary>
        public List<Cls_Mul_Orig> lst_Ori_Data = new List<Cls_Mul_Orig>();
        /// <summary>
        /// 显示数据缓存
        /// </summary>
        public List<Cls_Mul_ShowBuff> lst_Show_Buff = new List<Cls_Mul_ShowBuff>();
        /// <summary>
        /// 终点距离
        /// </summary>
        public int Trip_Com_Init_mm = 0;
    }
    public class Cls_Mul_Orig
    {
        public Cls_Mul_Orig(int iNum)
        {
            Arr_Emat = new Cls_EMAT_1[iNum];
        }


        #region 实时运行时需要显示缓存的数据
        /// <summary>
        /// 原始数据缓存链表中，当前数据距离
        /// </summary>
        public int i_X_mm = 0;
        ///// <summary>
        ///// 原始数据缓存链表中，屏幕显示数据的列号
        ///// </summary>
        //public int i_Screen_Col = -1;


        #endregion 
        /// <summary>
        /// 光栅臂原始数据
        /// </summary>
        public Cls_EMAT_1[] Arr_Emat;
    }
    public class Cls_Mul_Tile
    {
        public List<Cls_Mul_One> lst_Mul_Titl = new List<Cls_Mul_One>();
    }
    public class Cls_Mul_One
    {
        public List<Cls_One_Data> lst_One_Row_Data = new List<Cls_One_Data>();
    }

    public class Cls_One_Data
    {
        /// <summary>
        /// 距离
        /// </summary>
        public int i_X = -1;
        /// <summary>
        /// 误差
        /// </summary>
        public float flWc = -1;
        /// <summary>
        /// 涂层一个平均点对应多个测量数据
        /// </summary>
        public string str_Mul_Thick = "";

        /// <summary>
        /// 当前点对应信息
        /// </summary>
        public float flThick = -1;
        /// <summary>
        /// 增益值
        /// </summary>
        public int iGain = 0;
        /// <summary>
        /// 增益门限
        /// </summary>
        public int iGain_Limit = 0;
        /// <summary>
        /// 管道编号
        /// </summary>
        public string strGdbh = "";
        /// <summary>
        /// 通道号
        /// </summary>
        public int i_ABC = 1;
        /// <summary>
        /// 波形数据
        /// </summary>
        public byte[] btArrWave;

        /// <summary>
        /// 波形数据
        /// </summary>
        public float [] flArrWave;
    }
    public class Cls_Mul_ShowBuff
    {
        public Cls_Mul_ShowBuff(int iNum)
        {
            Arr_C_Data = new Cls_EMAT_2[iNum];
        }
        /// <summary>
        /// 对应标定数据的序号
        /// </summary>
        public int iWaveStandNo = 0;
        /// <summary>
        /// 数据类型 0:电磁超声  2：脉冲涡流
        /// </summary>
        public int iData_Type = 0;
        /// <summary>
        /// 本列数据是否使用
        /// </summary>
        public bool bl_BuffUse = false;
        /// <summary>
        /// 显示有效点的数据
        /// </summary>
        public Cls_EMAT_2[] Arr_C_Data;
        /// <summary>
        /// 当前距离
        /// </summary>
        public int m_i_X = 0;
    }
    #endregion 多通道
    /// <summary>
    /// 统计C扫描检测过程异常数据类
    /// </summary>
    public class CL_AlarmData
    {
        /// <summary>
        /// 是否已经在报表中参与统计了
        /// </summary>
        public bool bl_Use = false;
        /// <summary>
        /// 检测列表的行号:1-N
        /// </summary>
        public int i_Row = 0;

        /// <summary>
        /// 面积
        /// </summary>
        public float flArea = 0;
        /// <summary>
        /// X轴开始位置
        /// </summary>
        public int i_X_S = -1;
        /// <summary>
        /// X轴结束位置
        /// </summary>
        public int i_X_E = -1;
        /// <summary>
        /// Y轴开始位置
        /// </summary>
        public int i_Y_S = -1;
        /// <summary>
        /// 当前对应的X位置
        /// </summary>
        public int i_Y_X = -1;
        /// <summary>
        /// 最小厚度
        /// </summary>
        public float fl_Min_Thick = 0;
        /// <summary>
        /// 误差
        /// </summary>
        public float fl_Wc = -1;
    }
    /// <summary>
    /// 完整C扫描图数据类
    /// </summary>
    public class Clas_C_One_Data
    {

        List<CLs_EMAT_Data> lst_All_C = new List<CLs_EMAT_Data>();
    }
    /// <summary>
    /// 全屏C扫描每行属性
    /// </summary>
    public class Cls_All_C_RowPro
    {
        /// <summary>
        /// 当前行数据个数
        /// </summary>
        public int i_lst_Num = 0;
        /// <summary>
        /// 当前屏幕开始距离对应的序号
        /// </summary>
        public int i_Start_No = 0;
    }
    /// <summary>
    /// 涂层、漏电、打标数据
    /// </summary>
    public class Cls_Coat_Dis_Mark
    {
        /// <summary>
        /// 数据块号
        /// </summary>
        public int i_CurrRow_No = 0;
        /// <summary>
        /// 时间
        /// </summary>
        public string strTime = "";
        /// <summary>
        /// 0：厚度 1：放电 2：打标
        /// </summary>
        public int iType = -1;
        /// <summary>
        /// X轴位置：车体运动方向
        /// </summary>
        public int i_X = 0;
        /// <summary>
        /// 光栅臂位置
        /// </summary>
        public int i_Y = 0;
        /// <summary>
        /// 厚度
        /// </summary>
        public float fl_Data = 0;


    }
    public class CLs_EMAT_Data
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iGsbLen">光栅臂长度mm</param>
        /// <param name="iCDataLen">显示数据数组长度</param>
        public CLs_EMAT_Data(int iGsbLen, int iCDataLen)
        {

            Arr_Emat = new Cls_EMAT_1[iGsbLen];

            Arr_C_Data = new Cls_EMAT_2[iCDataLen];
        }
        /// <summary>
        /// 是否添加了放电
        /// </summary>
        public bool bl_Add_Dis = false;
        /// <summary>
        /// 是否添加了打标
        /// </summary>
        public bool bl_Add_Mark = false;

        /// <summary>
        /// 当前车体位置：X轴的位置
        /// </summary>
        public int i_X_mm = 0;
        /// <summary>
        /// 光栅臂原始数据
        /// </summary>
        public Cls_EMAT_1[] Arr_Emat;
        /// <summary>
        /// C扫描显示有效点的数据
        /// </summary>
        public Cls_EMAT_2[] Arr_C_Data;
        /// <summary>
        /// 是否放电
        /// </summary>
        public bool bl_Discharge;
        /// <summary>
        /// 收到打标命令
        /// </summary>
        public bool bl_Discharge_To_Mark;
    }

    /// <summary>
    ///数据第一层： 电磁超声数据
    /// </summary>
    public class Cls_EMAT_1
    {
        public Cls_EMAT_1(int iArrLen)
        {
            btArrWave = new byte[iArrLen == 0 ? 10 : iArrLen];
        }
        /// <summary>
        /// 当前通道是否使用
        /// </summary>
        public bool blUse = false;
        /// <summary>
        /// 对应显示缓存的 R
        /// </summary>
        public int Ori_R = 0;
        /// <summary>
        /// 对应显示缓存的 G
        /// </summary>
        public int Ori_G = 0;
        /// <summary>
        /// 对应显示缓存的 B
        /// </summary>
        public int Ori_B = 0;
        /// <summary>
        /// 对应显示缓存的厚度值
        /// </summary>
        public float Ori_flThick = 0;
        /// <summary>
        /// 报警
        /// </summary>
        public bool Ori_blAlarm = false;
        /// <summary>
        /// 误差
        /// </summary>
        public float Ori_flWc = 0;
        /// <summary>
        /// 波形
        /// </summary>
        public string Ori_strArrWave = "";

        ///// <summary>
        ///// X轴距离：mm
        ///// </summary>
        //public int i_X_Distance = 0;
        ///// <summary>
        ///// Y轴，光栅臂位置：mm
        ///// </summary>
        //public int i_Y_Distance = 0;

        /// <summary>
        /// 当前数据是否报警
        /// </summary>
        public bool blAlarm;
        /// <summary>
        /// 厚度
        /// </summary>
        public float flThick;
        /// <summary>
        /// 多涂层探头厚度值
        /// </summary>
        public string str_Mul_Thick = "";
        /// <summary>
        /// 增益门限
        /// </summary>
        public int iGain_Limit = 0;
        /// <summary>
        /// 转换成数据保存的格式
        /// </summary>
        public string strArrWave;
        /// <summary>
        /// 波形数据
        /// </summary>
        public byte[] btArrWave;

        /// <summary>
        /// 波形数据
        /// </summary>
        public float[] flArrWave;
        /// <summary>
        /// 标定缓存的序号
        /// </summary>
        public int iWaveStandNo;
        /// <summary>
        /// 颜色
        /// </summary>
        public string strColor;

        public int R = 0;
        public int G = 0;
        public int B = 0;

        /// <summary>
        /// 增益值
        /// </summary>
        public int iGain;
    }
    /// <summary>
    /// 数据第二层：显示光栅臂检测区间数据
    /// </summary>
    public class Cls_EMAT_2
    {
        /// <summary>
        /// 此通道是否使用
        /// </summary>
        public bool blUse = false;
        /// <summary>
        /// 距离
        /// </summary>
        public int m_i_X = 0;
        /// <summary>
        /// 是否报警
        /// </summary>
        public bool blAlarm = false;
        /// <summary>
        /// 光栅臂显示数据点对应序号,目的：查询界面时调用误差
        /// </summary>
        public int iArr_ShowNo = -1;
        /// <summary>
        /// 标定缓存的序号
        /// </summary>
        public int iWaveStandNo = -1;

        /// <summary>
        /// 放电值
        /// </summary>
        public float fl_Spark_Val = 0;
        /// <summary>
        /// 显示的颜色
        /// </summary>
        public string strColor;

        public int R;
        public int G;
        public int B;
        /// <summary>
        /// 厚度值
        /// </summary>
        public float flThick = -1;
        /// <summary>
        /// 涂层的多通道数据
        /// </summary>
        public string str_Mul_Thick = "";
        /// <summary>
        /// 误差
        /// </summary>
        public float flWc = 0;
        /// <summary>
        /// 增益值
        /// </summary>
        public int iGain = 0;
        /// <summary>
        /// 增益门限
        /// </summary>
        public int iGain_Limit = 0;

        /// <summary>
        /// 波形数据
        /// </summary>
        public byte[] btArrWave;
        /// <summary>
        /// 波形
        /// </summary>
        public string strArrWave = "";
        /// <summary>
        /// 涡流波形
        /// </summary>
        public float[] flArrWave;
        /// <summary>
        /// 位置
        /// </summary>
        public Cls_Cell clsCell;
    }

    #region 平面
    /// <summary>
    /// 公共信息
    /// </summary>
    public class Class_Info
    {
        /// <summary>
        /// 公称厚度
        /// </summary>
        public float fl_Normal_Thickness = 10;
        /// <summary>
        /// 公称厚度像素个数 50
        /// </summary>
        public int i_Thick_Piex = 50;
        /// <summary>
        /// X轴间隔距离
        /// </summary>
        public int i_Xmm = 5;
        /// <summary>
        /// Y轴间隔距离
        /// </summary>
        public int i_Ymm = 5;
        /// <summary>
        /// X轴单元像素
        /// </summary>
        public int i_X_Piex = 5;
        /// <summary>
        /// Y轴单元像素
        /// </summary>
        public int i_Y_Piex = 5;

        /// <summary>
        /// 光栅臂长度
        /// </summary>
        public int iGsbLen = 300;
        /// <summary>
        /// 光栅臂显示数据长度
        /// </summary>
        public int ScreeShow_iAllRows_C = 0;
    }
    /// <summary>
    /// 三维坐标点
    /// </summary>
    public class Cls_Point
    {
        /// <summary>
        /// 点X值
        /// </summary>
        public double X = 0;
        /// <summary>
        /// 点Y值
        /// </summary>
        public double Y = 0;
        /// <summary>
        /// 点Z值
        /// </summary>
        public double Z = 0;
    }
    /// <summary>
    /// 构建面片：逆时针
    /// </summary>
    public class Cls_Cell
    {
        /// <summary>
        /// 左侧上
        /// </summary>
        public Cls_Point P1 = new Cls_Point();
        /// <summary>
        /// 左侧下
        /// </summary>
        public Cls_Point P2 = new Cls_Point();
        /// <summary>
        /// 左右侧下
        /// </summary>
        public Cls_Point P3 = new Cls_Point();
        /// <summary>
        /// 右侧上
        /// </summary>
        public Cls_Point P4 = new Cls_Point();
    }
    /// <summary>
    /// 以索引号为序号的属性表
    /// </summary>
    public class Cls_Cell_ThickColorPosit
    {
        /// <summary>
        /// 厚度值
        /// </summary>
        public float flThick = 10;
        /// <summary>
        /// 颜色值
        /// </summary>
        public byte[] ArrColor = { 170, 170, 170 };
        /// <summary>
        /// 位置
        /// </summary>
        public Cls_Cell Posit = new Cls_Cell();
    }
    #endregion 平面

    /// <summary>
    /// C扫描画图类
    /// </summary>
    public class Cls_Plant_C
    {
        #region 变量
        /// <summary>
        /// 中文0  英文1
        /// </summary>
        public int m_iLanguage = 0;
        /// <summary>
        /// 文件读写
        /// </summary>
        ClassInterFace csInter = new ClassInterFace();
        /// <summary>
        /// 配置文件名
        /// </summary>
        string HardFileName = Application.StartupPath + "\\database\\SysConfig.ini";
        /// <summary>
        /// 写配置文件的：主标记
        /// </summary>
        string m_strSection = "Class_Plant";

        #region 标准值
        /// <summary>
        /// 减薄标准颜色阶段表    自定义色标值，格式：自定义（0/1）|开始色(R/G/B)|中间开始色(R/G/B)|中间开始色(R/G/B)|结束色(R/G/B)|故障通道颜色:结束色1/开始色0
        /// </summary>
        public string str_Wc_Start_End_Col = "";//
        /// <summary>
        /// 增厚标准颜色阶段表：格式：自定义（0/1）|开始色(R/G/B)|中间开始色(R/G/B)|中间开始色(R/G/B)|结束色(R/G/B)|故障通道颜色:结束色1/开始色0
        /// </summary>
        public string str_Wc_Start_End_Col_A = "";//
        /// <summary>
        /// 使用系统原始超厚颜色
        /// </summary>
        public int i_Col_A_By_1 = 1;
        ///// <summary>
        ///// B扫描图对应标准颜色
        ///// </summary>
        //public static  Color[] m_Co_StandColor = new Color[256];
        /// <summary>
        /// 标准厚度
        /// </summary>
        public float flNormal_Thickness = 10;

        #region 涂层检测计算方法

        /// <summary>
        /// 厚度区间颜色平分个数
        /// </summary>
        public int i_Coat_M_Num = 100;
        /// <summary>
        /// 减薄增厚厚度区间颜色平分个数
        /// </summary>
        public int i_Coat_Jb_Zh_Num = 10;
        /// <summary>
        /// 涂层最小厚度、或者厚度设计值
        /// </summary>
        public float flThick_Min_CoatLimit = 1;
        /// <summary>
        /// 涂层最大厚度
        /// </summary>
        public float flThick_Max_CoatLimit = -1;
        /// <summary>
        /// 涂层厚度偏差
        /// </summary>
        public float flThick_Pc_CoatLimit = 0;
        /// <summary>
        /// 报警颜色门限
        /// </summary>
        public string strColor_Coat = "";
        #endregion
        /// <summary>
        /// 测量数据大于此百份值，不再计算
        /// </summary>
        public float fl_Max_Limit = 30;

        /// <summary>
        /// 超限不处理厚度颜色
        /// </summary>
        public int i_Alarm_Out_R = 0;
        /// <summary>
        /// 超限不处理厚度颜色
        /// </summary>
        public int i_Alarm_Out_G = 0;
        /// <summary>
        /// 超限不处理厚度颜色
        /// </summary>
        public int i_Alarm_Out_B = 0;

        /// <summary>
        /// 是否使用不显示大误差
        /// </summary>
        public int i_Alarm = 0;
        ///// <summary>
        ///// 误差颜色：标准基础分界颜色
        ///// </summary>
        //public Color[] m_ArrColor;
        /// <summary>
        /// 标准颜色序号记录
        /// </summary>
        int m_iLastNo = 0;

        #region 误差数据
        /// <summary>
        /// 误差百分值: 例如20%报警
        /// </summary>
        public string strWc_Bfz = "";
        /// <summary>
        /// 误差分辨率
        /// </summary>
        public string strWc_Fbl = "0.01";
        /// <summary>
        /// 过渡值比例
        /// </summary>
        public string strWc_Gds_bl = "0.4";

        public string Txt_Wc_Num = "200";
        #endregion 误差数据
        /// <summary>
        /// 报警门限：通过公称厚度和报警百分值计算获得
        /// </summary>
        public float flstrThickAlarm = 0;
        /// <summary>
        /// 单位类型：0：公制单位 1：英制单位
        /// </summary>
        public int iRad_Dw = 0;
        /// <summary>
        /// 多波形色标缓存
        /// </summary>
        System.Drawing.Color[] m_800Limit = null;
        #endregion 标准值
        /// <summary>
        /// 查看历史波形数据
        /// </summary>
        public bool m_Ck_His_Wave = false;
        /// <summary>
        /// 屏幕A扫描图形变量
        /// </summary>
        public System.Windows.Forms.PictureBox Pic_A = null;
        /// <summary>
        /// 屏幕B扫描图形变量
        /// </summary>
        public System.Windows.Forms.PictureBox Pic_B = null;
        /// <summary>
        /// 屏幕C扫描单行图形变量
        /// </summary>
        public System.Windows.Forms.PictureBox Pic_C = null;
        /// <summary>
        /// 涂层
        /// </summary>
        public System.Windows.Forms.PictureBox Pic_Coat_C = null;
        /// <summary>
        /// 屏幕整体C扫描单行图形变量
        /// </summary>
        public System.Windows.Forms.PictureBox Pic_C_All = null;
        /// <summary>
        /// 涂层整体C扫描
        /// </summary>
        public System.Windows.Forms.PictureBox Pic_Coat_All = null;
        /// <summary>
        /// 当前屏幕：开始位置
        /// </summary>
        public int i_Screen_Start_Distance = -1;
        /// <summary>
        /// 当前屏幕：结束位置
        /// </summary>
        public int i_Screen_End_Distance = -1;

        /// <summary>
        /// 图形坐标：横轴开始像素位置
        /// </summary>
        public int Chart_Ruler_X_Start = 10;

        /// <summary>
        /// 图形坐标：横轴开始像素位置
        /// </summary>
        public int Chart_Ruler_X_Start_C_All = 10;

        /// <summary>
        /// 图形坐标：纵轴开始像素位置
        /// </summary>
        public int Chart_Ruler_Y_Start = 10;
        /// <summary>
        /// 一个点宽度:像素个数
        /// </summary>
        public int Scree_iDotWith_X = 1;
        /// <summary>
        /// 多通道全C扫描图一个数据像素的高度
        /// </summary>
        public int Scree_iDotHeight_C_Mul = 5;
        /// <summary>
        /// C扫描一个点高度:像素个数  图像高度/数据个数
        /// </summary>
        public float  Scree_iDotHeight = 1;
        /// <summary>
        /// 涂层测厚纵轴：C扫描一个点高度:像素个数  图像高度/数据个数
        /// </summary>
        public float  Scree_iDotHeight_Coat = 1;
        /// <summary>
        /// 涂层测厚纵轴：C扫描一个点高度:像素个数  图像高度/数据个数
        /// </summary>
        public float Scree_iDotHeight_Coat_ALL = 1;
        /// <summary>
        /// 一个点代表距离  默认10毫米  单位mm
        /// </summary>
        public int Scree_iDotWithmm_X = 10;


        /// <summary>
        /// 横轴刻度字在刻度线上左偏移刻度量
        /// </summary>
        public int Chart_Ruler_Word_X = 2;
        /// <summary>
        /// 横轴纵轴刻度字下降量
        /// </summary>
        public int Chart_Ruler_Word_H = -2;
        /// <summary>
        /// 尺子字体大小
        /// </summary>
        public int Chart_Ruler_font = 8;



        /// <summary>
        /// 光栅臂长度mm
        /// </summary>
        public int iGsb_Len = 300;
        /// <summary>
        /// 保存光栅臂数据的长度
        /// </summary>
        public int i_Gsb_Arr_Len = 0;
        /// <summary>
        ///涂层保存光栅臂数据的长度
        /// </summary>
        public int i_Gsb_Arr_Len_Coat_ALL = 0;
        /// <summary>
        /// 一个点代表距离 Y轴 单位mm
        /// </summary>
        public float  Scree_iDotHeightmm_Y = 5.0f;

        /// <summary>
        /// 涂层测厚：一个点代表距离  默认10毫米  单位mm
        /// </summary>
        public int Scree_iDotHeightmm_Y_Coat = 2;
        /// <summary>
        /// C扫描：横轴列个数
        /// </summary>
        public int Scree_iAllCols_C = 0;
        /// <summary>
        /// 单行C扫描：纵轴行数
        /// </summary>
        public int Scree_iAllRows_C = 0;

        /// <summary>
        /// 涂层光栅臂测量点：单行C扫描：纵轴行数
        /// </summary>
        public int Scree_iAllRows_C_Coat = 0;

        /// <summary>
        /// 涂层光栅臂测量点：单行C扫描：纵轴行数
        /// </summary>
  //      public int Scree_iAllRows_C_Coat_ALL = 0;
        /// <summary>
        /// 多通道： 下一屏幕开始行号
        /// </summary>
        public int i_Row_Start_NextPage = 0;

        /// <summary>
        ///多通道C扫描：纵轴对应的：总通道数
        /// </summary>
        public int Scree_iAllRows_C_Mul = 0;
        /// <summary>
        /// B扫描图：1mm占用几个像素
        /// </summary>
        public float m_Scree_fl_DotHeight_B = 0;
        /// <summary>
        /// B扫描一个点宽度
        /// </summary>
        public int m_Scree_iDotWith_B = 0;
        /// <summary>
        /// 一屏标准长度 mm
        /// </summary>
        public int Scree_Stant_Distance = 0;

        /// <summary>
        /// 整体C扫描一屏标准长度 mm
        /// </summary>
        public int Scree_Stant_Distance_C_ALL = 0;
        /// <summary>
        /// 当前屏幕序号 0-N
        /// </summary>
        public int i_Screen_No = 0;
        /// <summary>
        /// 整体C图方位信息
        /// </summary>
        public CLS_C_ALL cls_C_All_Mark = new CLS_C_ALL();
        /// <summary>
        /// C扫描刻度表:记录每屏幕的开始/结束刻度
        /// </summary>
        public List<Cls_Kd> lst_Screenkd = new List<Cls_Kd>();

        /// <summary>
        /// C扫描刻度表:记录每屏幕的开始/结束刻度
        /// </summary>
        public List<Cls_Kd> lst_Screenkd_C_ALL = new List<Cls_Kd>();
        /// <summary>
        /// 最远检测距离-100000
        /// </summary>
        public float flMaxDistance = 0;
        /// <summary>
        /// 是否计算公称厚度  true:计算  false: 不计算
        /// </summary> 
        public bool Ck_No_Normal_Thickness = true;
        /// <summary>
        /// B扫纵轴一个点最大厚度比公称厚度多mm 默认5毫米
        /// </summary>
        public int Scree_iDotHeight_B_Y_Addmm = 5;

        /// <summary>
        /// C图像变量
        /// </summary>
        public Struct_G m_G_C = new Struct_G();
        /// <summary>
        /// 涂层C扫描
        /// </summary>
        public Struct_G m_Coat_C = new Struct_G();
        /// <summary>
        /// 整个C图像变量
        /// </summary>
        public Struct_G m_G_C_ALL = new Struct_G();
        /// <summary>
        /// 整个涂层C图像变量
        /// </summary>
        public Struct_G m_G_Coat_ALL = new Struct_G();
        /// <summary>
        /// B光栅臂
        /// </summary>
        public Struct_G m_G_B = new Struct_G();

        /// <summary>
        /// C扫描画笔
        /// </summary>
        Pen Ruler_p_One = new Pen(Brushes.Black);
        Font drawFont = new Font("黑体", (float)7.9, FontStyle.Bold);// new Font("Arial", (float)10);
                                                                   //   SolidBrush drawBrush = new SolidBrush(Color.Black);
        SolidBrush drawBrush = new SolidBrush(System.Drawing.Color.FromArgb(255, 21, 139, 207));

        /// <summary>
        /// 报警线
        /// </summary>
        Pen Ruler_p_Limit = new Pen(Brushes.Red);



        #region 多通道
        /// <summary>
        /// 一行B扫图数据高度
        /// </summary>
        public int m_iMul_OneB_Heigh = 1;
        /// <summary>
        /// 多通道1mm的像素高度
        /// </summary>
        public float m_iMul_B_OneDataHeitht = 0;
        #endregion

        #endregion 变量
        public Cls_Plant_C()
        {
            Init();
        }
        /// <summary>
        /// 读写配置文件
        /// </summary>
        public void Init(int iType = 0)
        {
            if (iType == 0)
            {
                #region 

                flThick_Min_CoatLimit = float.Parse(csInter.IniReadDefine(m_strSection, "flThick_Min_CoatLimit", "30", HardFileName));
                flThick_Max_CoatLimit = float.Parse(csInter.IniReadDefine(m_strSection, "flThick_Max_CoatLimit", "100", HardFileName));
                flThick_Pc_CoatLimit = float.Parse(csInter.IniReadDefine(m_strSection, "flThick_Pc_CoatLimit", "20", HardFileName));

                i_Alarm = int.Parse(csInter.IniReadDefine(m_strSection, "i_Alarm", "0", HardFileName));
                fl_Max_Limit = float.Parse(csInter.IniReadDefine(m_strSection, "fl_Max_Limit", "30", HardFileName));

                i_Alarm_Out_R = int.Parse(csInter.IniReadDefine(m_strSection, "i_Alarm_Out_R", "255", HardFileName));
                i_Alarm_Out_G = int.Parse(csInter.IniReadDefine(m_strSection, "i_Alarm_Out_G", "245", HardFileName));
                i_Alarm_Out_B = int.Parse(csInter.IniReadDefine(m_strSection, "i_Alarm_Out_B", "0", HardFileName));

                flNormal_Thickness = float.Parse(csInter.IniReadDefine(m_strSection, "flNormal_Thickness", "10", HardFileName));
                Chart_Ruler_X_Start = int.Parse(csInter.IniReadDefine(m_strSection, "Chart_Ruler_X_Start", "15", HardFileName));
                Chart_Ruler_X_Start_C_All = int.Parse(csInter.IniReadDefine(m_strSection, "Chart_Ruler_X_Start_C_All", "40", HardFileName));
                Chart_Ruler_Y_Start = int.Parse(csInter.IniReadDefine(m_strSection, "Chart_Ruler_Y_Start", "18", HardFileName));

                Scree_iDotWith_X = int.Parse(csInter.IniReadDefine(m_strSection, "Scree_iDotWith_X", "5", HardFileName));

                Scree_iDotHeightmm_Y = float .Parse(csInter.IniReadDefine(m_strSection, "Scree_iDotHeightmm_Y", "5", HardFileName));
                Scree_iDotHeight_C_Mul = int.Parse(csInter.IniReadDefine(m_strSection, "Scree_iDotHeight_C_Mul", "5", HardFileName));
                if (Scree_iDotHeight_C_Mul < 1) Scree_iDotHeight_C_Mul = 5;
                //     Scree_iDotWithmm_X = int.Parse(csInter.IniReadDefine(m_strSection, "Scree_iDotWithmm_X", "5", HardFileName));

                iGsb_Len = int.Parse(csInter.IniReadDefine(m_strSection, "iGsb_Len", "300", HardFileName));

                flMaxDistance = int.Parse(csInter.IniReadDefine(m_strSection, "flMaxDistance", "1000000", HardFileName));

                Chart_Ruler_font = int.Parse(csInter.IniReadDefine(m_strSection, "Chart_Ruler_font", "8", HardFileName));
                Chart_Ruler_Word_X = int.Parse(csInter.IniReadDefine(m_strSection, "Chart_Ruler_Word_X", "2", HardFileName));
                Chart_Ruler_Word_H = int.Parse(csInter.IniReadDefine(m_strSection, "Chart_Ruler_Word_H", "2", HardFileName));

                strWc_Bfz = csInter.IniReadDefine(m_strSection, "strWc_Bfz", "20", HardFileName);
                strWc_Fbl = csInter.IniReadDefine(m_strSection, "strWc_Fbl", "0.01", HardFileName);
                strWc_Gds_bl = csInter.IniReadDefine(m_strSection, "strWc_Gds_bl", "0.4", HardFileName);
                Txt_Wc_Num = csInter.IniReadDefine(m_strSection, "Txt_Wc_Num", "200", HardFileName);
                str_Wc_Start_End_Col = csInter.IniReadDefine(m_strSection, "str_Wc_Start_End_Col", "0/0/255|154/205/50|255/255/0|255/0/0", HardFileName);

                str_Wc_Start_End_Col_A = csInter.IniReadDefine(m_strSection, "str_Wc_Start_End_Col_A", "0/0/255|154/205/50|255/255/0|255/0/0", HardFileName);

                i_Col_A_By_1 = int.Parse(csInter.IniReadDefine(m_strSection, "i_Col_A_By_1", "1", HardFileName));
                #endregion

                Alarm_Init();
            }
            else
            {
                #region   i_Alarm
                csInter.INIWriteValue(m_strSection, "flThick_Min_CoatLimit", flThick_Min_CoatLimit.ToString(), HardFileName);
                csInter.INIWriteValue(m_strSection, "flThick_Max_CoatLimit", flThick_Max_CoatLimit.ToString(), HardFileName);
                csInter.INIWriteValue(m_strSection, "flThick_Pc_CoatLimit", flThick_Pc_CoatLimit.ToString(), HardFileName);

                csInter.INIWriteValue(m_strSection, "i_Alarm", i_Alarm.ToString(), HardFileName);
                csInter.INIWriteValue(m_strSection, "fl_Max_Limit", fl_Max_Limit.ToString(), HardFileName);
                csInter.INIWriteValue(m_strSection, "flNormal_Thickness", flNormal_Thickness.ToString(), HardFileName);
                csInter.INIWriteValue(m_strSection, "Chart_Ruler_X_Start", Chart_Ruler_X_Start.ToString(), HardFileName);
                csInter.INIWriteValue(m_strSection, "Chart_Ruler_Y_Start", Chart_Ruler_Y_Start.ToString(), HardFileName);
                csInter.INIWriteValue(m_strSection, "Scree_iDotWith_X", Scree_iDotWith_X.ToString(), HardFileName);
                csInter.INIWriteValue(m_strSection, "Scree_iDotHeightmm_Y", Scree_iDotHeightmm_Y.ToString(), HardFileName);
                csInter.INIWriteValue(m_strSection, "Scree_iDotWithmm_X", Scree_iDotWithmm_X.ToString(), HardFileName);
                csInter.INIWriteValue(m_strSection, "iGsb_Len", iGsb_Len.ToString(), HardFileName);

                csInter.INIWriteValue(m_strSection, "flMaxDistance", flMaxDistance.ToString(), HardFileName);

                csInter.INIWriteValue(m_strSection, "strWc_Bfz", strWc_Bfz, HardFileName);
                csInter.INIWriteValue(m_strSection, "strWc_Fbl", strWc_Fbl, HardFileName);
                csInter.INIWriteValue(m_strSection, "strWc_Gds_bl", strWc_Gds_bl, HardFileName);
                csInter.INIWriteValue(m_strSection, "Txt_Wc_Num", Txt_Wc_Num, HardFileName);

                csInter.INIWriteValue(m_strSection, "str_Wc_Start_End_Col", str_Wc_Start_End_Col, HardFileName);
                csInter.INIWriteValue(m_strSection, "str_Wc_Start_End_Col_A", str_Wc_Start_End_Col_A, HardFileName);

                #endregion
            }
            #region 1 A扫描

            #endregion 1 
            #region 2 B扫描

            #endregion 2
            #region 3 C扫描：单行

            #endregion  3
            #region 4 C扫描：整体

            #endregion 4
        }
        public string Read_One(string strKey)
        {
            return csInter.IniReadDefine(m_strSection, strKey, "", HardFileName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iType"></param>
        public void Write_One(string strKey, string strVal)
        {
            csInter.INIWriteValue(m_strSection, strKey, strVal, HardFileName);
        }
        public void Init_KD()
        {
            int _iNo = 0;
            lst_Screenkd.Clear();
         //0..   MessageBox.Show("eee-" + flMaxDistance+"    " + Scree_Stant_Distance.ToString ());
            while (true)
            {
                //1 拿数据
                Cls_Kd _kd = new Cls_Kd();
                try
                {
                    if (_iNo == 0)
                    {
                        _kd.i_Start = 0;
                        _kd.i_End = Scree_Stant_Distance;
                    }
                    else
                    {
                        _kd.i_Start = lst_Screenkd[_iNo - 1].i_End;
                        _kd.i_End = (_iNo + 1) * Scree_Stant_Distance;
                    }

                    //2 添加当前数据
                    lst_Screenkd.Add(_kd);
           //         MessageBox.Show("eee-" + _kd.i_End);
                    if (_kd.i_End > flMaxDistance) 
                        return;
                    //3 下一个数据
                    _iNo++;
                }
#pragma warning disable CS0168 // 声明了变量“e”，但从未使用过
                catch (Exception e)
#pragma warning restore CS0168 // 声明了变量“e”，但从未使用过
                { }
            }
        }
        public void Init_KD_C_ALL()
        {
            int _iNo = 0;
            lst_Screenkd_C_ALL.Clear();
            while (true)
            {
                //1 拿数据
                Cls_Kd _kd = new Cls_Kd();

                if (_iNo == 0)
                {
                    _kd.i_Start = 0;
                    _kd.i_End = Scree_Stant_Distance_C_ALL;
                }
                else
                {
                    _kd.i_Start = lst_Screenkd_C_ALL[_iNo - 1].i_End;
                    _kd.i_End = (_iNo + 1) * Scree_Stant_Distance_C_ALL;
                }
                //2 添加当前数据
                lst_Screenkd_C_ALL.Add(_kd);
                if (_kd.i_End > flMaxDistance) return;
                //3 下一个数据
                _iNo++;
            }
        }
        /// <summary>
        /// 距离寻找对应屏幕序号
        /// </summary>
        /// <param name="iDistance"></param>
        /// <returns></returns>
        public int Juge_ScreenNo_C(int iDistance)
        {
            int _iRet = 0;
            for (int i = 0; i < lst_Screenkd.Count; i++)
            {
                if (iDistance >= lst_Screenkd[i].i_Start && iDistance < lst_Screenkd[i].i_End)
                { _iRet = i; break; }
            }
            return _iRet;
        }
        /// <summary>
        /// 距离寻找对应屏幕序号
        /// </summary>
        /// <param name="iDistance"></param>
        /// <returns></returns>
        public int Juge_ScreenNo_C_ALL(int iDistance)
        {
            int _iRet = -1;
            for (int i = 0; i < lst_Screenkd_C_ALL.Count; i++)
            {
                if (iDistance >= lst_Screenkd_C_ALL[i].i_Start && iDistance < lst_Screenkd_C_ALL[i].i_End)
                { _iRet = i; break; }
            }
            return _iRet;
        }
        #region 误差颜色

        /// <summary>
        /// 减薄量厚度获得颜色
        /// </summary>
        /// <param name="dWT">标准值-被测值</param>
        /// <param name="R"></param>
        /// <param name="G"></param>
        /// <param name="B"></param>
        public int CalculThickColor(float dWT, out int R, out int G, out int B, int iIndex = 0, bool Ck_No_Normal_Thickness = true)
        {
            R = 0; G = 0; B = 0;
            int iDj = 0, iNum = 0, iAll = colorRange.Count();//预留误差等级
            string strDw;
            try
            {
                if (colorRange.Count > 0)
                {
                    if (Ck_No_Normal_Thickness == false)
                    {
                        R = colorRange[0].R;
                        G = colorRange[0].G;
                        B = colorRange[0].B;
                        if (iIndex > 0)
                        {
                            R = 85;
                            G = 85;
                        }
                        return 0;
                    }
                    dWT = Math.Abs(dWT);///求绝对值

                    strDw = dWT.ToString("0.000");
                    dWT = float.Parse(strDw);
                    iAll = colorRange.Count;

                    foreach (ColorRange cRange in colorRange)
                    {
                        iNum++;
                        if (dWT <= cRange.Max_Limit)//从小到大排列
                        {
                            R = cRange.R;
                            G = cRange.G;
                            B = cRange.B;
                            break;
                        }
                        else
                        {
                            if (iNum >= iAll)
                            {
                                R = cRange.R;
                                G = cRange.G;
                                B = cRange.B;
                            }
                        }
                    }
                }
            }
            catch { }
            return iDj;
        }
        /// <summary>
        /// 涂层减薄对应颜色
        /// </summary>
        /// <param name="dWT"></param>
        /// <param name="R"></param>
        /// <param name="G"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public int CalculThickColor_Coat_JB(float dWT, out int R, out int G, out int B)
        {
            R = 0; G = 0; B = 0;
            int iDj = 0, iNum = 0, iAll = m_Co_Coat_Jb.Count();//预留误差等级
            string strDw;
            try
            {
                if (m_Co_Coat_Jb.Count > 0)
                {
                 
                    dWT = Math.Abs(dWT);///求绝对值

                    strDw = dWT.ToString("0.000");
                    dWT = float.Parse(strDw);
                    iAll = m_Co_Coat_Jb.Count;

                    foreach (ColorRange cRange in m_Co_Coat_Jb)
                    {
                        iNum++;
                        if (dWT <= cRange.Max_Limit)//从小到大排列
                        {
                            R = cRange.R;
                            G = cRange.G;
                            B = cRange.B;
                            iDj = 1;
                            break;
                        }
                        else
                        {
                            if (iNum >= iAll)
                            {
                                R = cRange.R;
                                G = cRange.G;
                                B = cRange.B;
                            }
                        }
                    }
                }
            }
            catch { }
            return iDj;
        }
        /// <summary>
        /// 涂层厚度对应颜色
        /// </summary>
        /// <param name="dWT"></param>
        /// <param name="R"></param>
        /// <param name="G"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public int CalculThickColor_Coat_MD(float dWT, out int R, out int G, out int B)
        {
            R = 0; G = 0; B = 0;
            int iDj = 0, iNum = 0, iAll = m_Co_Coat_Md.Count();//预留误差等级
            string strDw;
            try
            {
                if (m_Co_Coat_Md.Count > 0)
                {

                    dWT = Math.Abs(dWT);///求绝对值

                    strDw = dWT.ToString("0.000");
                    dWT = float.Parse(strDw);
                    iAll = m_Co_Coat_Md.Count;

                    foreach (ColorRange cRange in m_Co_Coat_Md)
                    {
                        iNum++;
                        if (dWT <= cRange.Max_Limit)//从小到大排列
                        {
                            R = cRange.R;
                            G = cRange.G;
                            B = cRange.B;
                            iDj = 1;
                            break;
                        }
                        else
                        {
                            if (iNum >= iAll)
                            {
                                R = cRange.R;
                                G = cRange.G;
                                B = cRange.B;
                            }
                        }
                    }
                }
            }
            catch { }
            return iDj;
        }
        /// <summary>
        /// 涂层增厚对应颜色
        /// </summary>
        /// <param name="dWT"></param>
        /// <param name="R"></param>
        /// <param name="G"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public int CalculThickColor_Coat_ZH(float dWT, out int R, out int G, out int B)
        {
            R = 0; G = 0; B = 0;
            int iDj = 0, iNum = 0, iAll = m_Co_Coat_Zh.Count();//预留误差等级
            string strDw;
            try
            {
                if (m_Co_Coat_Zh.Count > 0)
                {

                    dWT = Math.Abs(dWT);///求绝对值

                    strDw = dWT.ToString("0.000");
                    dWT = float.Parse(strDw);
                    iAll = m_Co_Coat_Zh.Count;

                    foreach (ColorRange cRange in m_Co_Coat_Zh)
                    {
                        iNum++;
                        if (dWT <= cRange.Max_Limit)//从小到大排列
                        {
                            R = cRange.R;
                            G = cRange.G;
                            B = cRange.B;
                            iDj = 1;
                            break;
                        }
                        else
                        {
                            if (iNum >= iAll)
                            {
                                R = cRange.R;
                                G = cRange.G;
                                B = cRange.B;
                            }
                        }
                    }
                }
            }
            catch { }
            return iDj;
        }
        /// <summary>
        /// 增厚量活动对应颜色
        /// </summary>
        /// <param name="dWT"></param>
        /// <param name="R"></param>
        /// <param name="G"></param>
        /// <param name="B"></param>
        /// <param name="iIndex"></param>
        /// <param name="Ck_No_Normal_Thickness"></param>
        /// <returns></returns>
        public int CalculThickColor_A(float dWT, out int R, out int G, out int B, int iIndex = 0, bool Ck_No_Normal_Thickness = true)
        {
            R = 0; G = 0; B = 0;
            int iDj = 0, iNum = 0, iAll = colorRange_A.Count();//预留误差等级
            string strDw;
            try
            {
                if (colorRange_A.Count > 0)
                {
                    if (Ck_No_Normal_Thickness == false)
                    {
                        R = colorRange_A[0].R;
                        G = colorRange_A[0].G;
                        B = colorRange_A[0].B;
                        if (iIndex > 0)
                        {
                            R = 85;
                            G = 85;
                        }
                        return 0;
                    }
                    dWT = Math.Abs(dWT);///求绝对值

                    strDw = dWT.ToString("0.000");
                    dWT = float.Parse(strDw);
                    iAll = colorRange_A.Count;

                    foreach (ColorRange cRange in colorRange_A)
                    {
                        iNum++;
                        if (dWT <= cRange.Max_Limit)//从小到大排列
                        {
                            R = cRange.R;
                            G = cRange.G;
                            B = cRange.B;
                            break;
                        }
                        else
                        {
                            if (iNum >= iAll)
                            {
                                R = cRange.R;
                                G = cRange.G;
                                B = cRange.B;
                            }
                        }
                    }
                }
            }
            catch { }
            return iDj;
        }
        #region 脉冲涡流厚度异常颜色设置
        /// <summary>
        /// 起始厚度差值
        /// </summary>
        float m_flScale_Low = 0;//从50%-->100%
        float m_flJg_ECT = 0;
        public void Get_Stand_ECT_Limit(System.Windows.Forms.PictureBox PicArea)
        {
            Color[] ArrColor = new Color[2];
            colorRange_ECT.Clear();
            m_flJg_ECT = 530;

            m_flScale_Low = flNormal_Thickness * 0.5f;
            float _flLimit = m_flScale_Low / m_flJg_ECT;
            float _fl_LimitStart = m_flScale_Low;

            ArrColor[0] = System.Drawing.Color.FromArgb(91, 2, 0);
            ArrColor[1] = System.Drawing.Color.FromArgb(255, 0, 0);
            Add_StandECT_Limit(ArrColor, 210, PicArea, _flLimit, ref _fl_LimitStart);//50-70

            ArrColor[0] = System.Drawing.Color.FromArgb(255, 0, 0);
            ArrColor[1] = System.Drawing.Color.FromArgb(255, 255, 0);
            Add_StandECT_Limit(ArrColor, 210, PicArea, _flLimit, ref _fl_LimitStart);//70-90

            ArrColor[0] = System.Drawing.Color.FromArgb(255, 255, 0);
            ArrColor[1] = System.Drawing.Color.FromArgb(0, 255, 0);
            Add_StandECT_Limit(ArrColor, 110, PicArea, _flLimit, ref _fl_LimitStart);//90-100

            m_flJg_ECT = 310;
            m_flScale_Low = flNormal_Thickness * 0.3f;
            _flLimit = m_flScale_Low / m_flJg_ECT;
            _fl_LimitStart = m_flScale_Low;

            _fl_LimitStart = 0;
            ArrColor[0] = System.Drawing.Color.FromArgb(0, 255, 0);
            ArrColor[1] = System.Drawing.Color.FromArgb(0, 180, 124);
            Add_StandECT_Limit(ArrColor, 310, PicArea, _flLimit, ref _fl_LimitStart);//100-130
        }
        /// <summary>
        /// 厚度从50%到130% 公称厚度10，厚度差(工程厚度-实时厚度) 5->0-->-3.3  
        /// </summary>
        /// <param name="ArrColor"></param>
        /// <param name="iJG"></param>
        /// <param name="PicArea"></param>
        /// <param name="flLimitJG"></param>
        /// <param name="fl_LimitStart"></param>
        public void Add_StandECT_Limit(Color[] ArrColor, int iJG, System.Windows.Forms.PictureBox PicArea,
                                      float flLimitJG, ref float fl_LimitStart)
        {
            Bitmap _Bitmap_Out = new Bitmap(PicArea.ClientSize.Width, PicArea.ClientSize.Height);

            //1 指定颜色 91,2,0   255,0,0   255,255,0  0,255,0    0,180,124
            Color color = _Bitmap_Out.GetPixel(0, 0);
            float flLimit = fl_LimitStart;
            float flImageJG_Low = PicArea.ClientSize.Width / iJG;
            //3 解析
            PlanScheme_ColorLimit_JG(ArrColor, PicArea, iJG, ref _Bitmap_Out);
            //4 获得颜色，设置标准
            for (int i = 0; i < iJG; i++)
            {
                #region   1 获得色度值
                color = _Bitmap_Out.GetPixel((int)(i * (flImageJG_Low)), 0);
                #endregion 1
                #region 2 将误差限数据添加进系统
                ColorRange _clorRg = new ColorRange();
                _clorRg.R = color.R;
                _clorRg.G = color.G;
                _clorRg.B = color.B;
                _clorRg.Max_Limit = flLimit;
                colorRange_ECT.Add(_clorRg);
                #endregion 2

                #region  3 误差限色标添加 刻度累加
                flLimit -= flLimitJG;
                flLimit = float.Parse(flLimit.ToString("f3"));
                #endregion 3
            }
            fl_LimitStart = flLimit;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ArrColor"></param>
        /// <param name="PicArea"></param>
        /// <param name="iJG"></param>
        /// <param name="Rr_Bitmap"></param>
        public void PlanScheme_ColorLimit_JG(Color[] ArrColor, System.Windows.Forms.PictureBox PicArea, int iJG, ref Bitmap Rr_Bitmap)
        {
            try
            {
                float flPic_W = PicArea.Width;//画布宽度
                float flPic_H = PicArea.Height;//画布高度

                #region 1.2 准备画布
                // 初始化画板，在内存中建立一块虚拟画布
                Bitmap image = new Bitmap(PicArea.ClientSize.Width, PicArea.ClientSize.Height);

                // 获取背景层
                Bitmap bg = (Bitmap)PicArea.BackgroundImage;
                // 初始化整个画布
                Bitmap canvas = new Bitmap(PicArea.ClientSize.Width, PicArea.ClientSize.Height);
                // 初始化图形面板，获取这块内存画布的Graphics的引用
                Graphics g = Graphics.FromImage(image);
                Graphics gb = Graphics.FromImage(canvas);

                #endregion  准备画布
                g.Clear(Color.White);// g.Clear(Color.Teal);// CadetBlue);
                Rectangle rect = new Rectangle(0, 0, PicArea.ClientSize.Width, PicArea.ClientSize.Height);

                rect.Location = new Point(0, 0);
                LinearGradientBrush b3 = new LinearGradientBrush(rect, Color.Blue, Color.White, LinearGradientMode.Horizontal);
                if (ArrColor[0].R == 0 && ArrColor[0].G == 0 && ArrColor[0].B == 0)
                {
                    ArrColor[0] = Color.Blue; ArrColor[1] = Color.YellowGreen;
                    ArrColor[2] = Color.Yellow; ArrColor[3] = Color.Red;
                }

                b3 = new LinearGradientBrush(rect, ArrColor[0], ArrColor[1], LinearGradientMode.Horizontal);

                g.FillRectangle(b3, rect);

                Pen p = new Pen(Brushes.Green);//刻度笔

                int iBzNum = (int)((PicArea.Width) / iJG);
                if (iBzNum == 0) { iBzNum = PicArea.Width; }

              
                #region  4 刷新
                Rectangle _Rect = new Rectangle(0, 0, PicArea.Width, PicArea.Height);

                if (bg != null)
                    gb.DrawImage(bg, _Rect);// 先绘制背景层
                gb.DrawImage(image, _Rect); // 再绘制绘画层

                PicArea.BackgroundImage = (Bitmap)canvas.Clone();
                Rr_Bitmap = (Bitmap)canvas.Clone();


                g.Dispose(); g = null;
                image.Dispose();
                gb.Dispose();
                canvas.Dispose();

                PicArea.Refresh();
                #endregion 刷新

                canvas.Dispose();
                image.Dispose();
            }
            catch { }
        }
        #endregion 

        /// <summary>
        /// 脉冲涡流 依据厚度获得颜色
        /// </summary>
        /// <param name="dWT">标准值-被测值</param>
        /// <param name="R"></param>
        /// <param name="G"></param>
        /// <param name="B"></param>
        public int CalculThickColor_ECT(float dWT, out int R, out int G, out int B, int iIndex = 0, bool Ck_No_Normal_Thickness = true)
        {
            R = 0; G = 0; B = 0;
#pragma warning disable CS0219 // 变量“iNum”已被赋值，但从未使用过它的值
            int iDj = 0, iNum = 0, iAll = colorRange_ECT.Count();//预留误差等级
#pragma warning restore CS0219 // 变量“iNum”已被赋值，但从未使用过它的值
            string strDw;
            try
            {
                R = 20;
                G = 48;
                B = 244;
                if (colorRange_ECT.Count > 0)
                {
                    if (Ck_No_Normal_Thickness == false)
                    {
                        R = colorRange_ECT[0].R;
                        G = colorRange_ECT[0].G;
                        B = colorRange_ECT[0].B;
                        if (iIndex > 0)
                        {
                            R = 20;
                            G = 48;
                            B = 244;
                        }
                        return 0;
                    }
                    // dWT = Math.Abs(dWT);///求绝对值

                    strDw = dWT.ToString("0.000");
                    dWT = float.Parse(strDw);
                    iAll = colorRange_ECT.Count;

                    int _iAllNum = colorRange_ECT.Count - 1;
                    for (int i = 0; i <= _iAllNum; i++)//应道采用对半查找法
                    {
                        if (dWT >= colorRange_ECT[i].Max_Limit)
                        {
                            if (i > 0 && i < _iAllNum)
                            {
                                if (Math.Abs(dWT - colorRange_ECT[i - 1].Max_Limit) < Math.Abs(dWT - colorRange_ECT[i].Max_Limit))
                                {
                                    R = colorRange_ECT[i - 1].R;
                                    G = colorRange_ECT[i - 1].G;
                                    B = colorRange_ECT[i - 1].B;
                                }
                                else
                                {
                                    R = colorRange_ECT[i].R;
                                    G = colorRange_ECT[i].G;
                                    B = colorRange_ECT[i].B;
                                }
                                break;
                            }
                        }
                    }
                    //foreach (ColorRange cRange in colorRange_ECT)
                    //{
                    //    iNum++;
                    //    if (dWT >= cRange.Max_Limit)//从小到大排列
                    //    {
                    //        R = cRange.R;
                    //        G = cRange.G;
                    //        B = cRange.B;
                    //        break;
                    //    }
                    //    else
                    //    {
                    //        if (iNum == iAll)
                    //        {
                    //            R = cRange.R;
                    //            G = cRange.G;
                    //            B = cRange.B;
                    //        }
                    //    }
                    //}
                }
            }
            catch { }
            return iDj;
        }

        /// <summary>
        /// 计算厚度色标限
        /// </summary>
        /// <param name="strNum"></param>
        /// <param name="P_1"></param>
        /// <param name="P_Md"></param>
        /// <param name="P_2"></param>
        /// <param name="flMd_Bl"></param>
        public void InitLimitPic(string strNum, PictureBox P_1, PictureBox P_Md, PictureBox P_2,
                          System.Windows.Forms.GroupBox Grp_Stand_Color, float flMd_Bl = 0.4f)
        {
            string strType = strNum;
            Cls_Plant_C.colorRange.Clear(); m_iLastNo = 0;

            Bitmap Rr_Bitmap_1 = new Bitmap(P_1.ClientSize.Width, P_1.ClientSize.Height);
            Bitmap Rr_Bitmap_2 = new Bitmap(P_1.ClientSize.Width, P_1.ClientSize.Height);
            Bitmap Rr_Bitmap_Md = new Bitmap(P_1.ClientSize.Width, P_1.ClientSize.Height);
            Color color = Rr_Bitmap_1.GetPixel(0, 0);
            float flJG_Low = 0;
            float _flLimit = 0f;//刻度基数
            string _strXsw = "f2";// SysInfo.m_SysInfo.g_ScreenPlant.iRad_Dw == 1 ? "f4" : "f2";

            float iGetWith_Low = 0;//第一色标点个数 误差点个数
#pragma warning disable CS0219 // 变量“iLefMove”已被赋值，但从未使用过它的值
            int iLefMove = 0;
#pragma warning restore CS0219 // 变量“iLefMove”已被赋值，但从未使用过它的值
            string strT = "";

            //刻度最小单位
            float _flScale_Low = 0;//最小刻度值：由厚度和刻度点自动计算

            if (strType == "单色" || strType == "10")
            {
                //   Grp_100.Visible = false;
                if (strType == "10")
                    iGetWith_Low = 11;
                else
                    iGetWith_Low = 101;
                P_Md.Visible = false;
            }
            else
            {
                #region 100色
                //   Grp_100.Visible = true;
                //if (Ck_MyColor.Checked == false)
                //{
                //    Ck_MyColor.Checked = false;
                //    P_My_S.BackColor = Color.Blue;
                //    P_My_M_Start.BackColor = Color.YellowGreen;
                //    P_My_M_End.BackColor = Color.Yellow;
                //    P_My_E.BackColor = Color.Red;
                //}
                #endregion 100色
                P_Md.Visible = true;
                iGetWith_Low = float.Parse(strType);

                iGetWith_Low += 1;
            }
            _flScale_Low = flNormal_Thickness / (iGetWith_Low - 1);

            if (strType == "单色" || strType == "10")
            {
                /*
               思路：  用井壁厚度作为宽带参数，
                 */
                //    EnbColor(false);
                #region 单色
                //1 确定位置
                P_1.Width = Grp_Stand_Color.Width - 4;
                P_1.Left = 2; P_1.Visible = true;
                P_2.Visible = false; P_Md.Visible = false;
                //1.2 色标间隔
                if (strType == "10")
                    iGetWith_Low = 11;
                else
                    iGetWith_Low = 101;
                flJG_Low = (float)(P_1.ClientSize.Width / iGetWith_Low);
                //2 画色标图
                PlanScheme_ColorLimit(P_1, (int)flJG_Low, ref Rr_Bitmap_1, 2, 0);

                _flLimit = 0.0f;
                #region 1 统计色标值
                for (int i = 0; i < iGetWith_Low; i++)
                {
                    #region   1 获得色度值
                    color = Rr_Bitmap_1.GetPixel((int)(i * (flJG_Low - 0.01)), 0);
                    strT = color.R.ToString() + "," + color.G.ToString() + "," + color.B.ToString();
                    #endregion 1
                    #region 2 将误差限数据添加进系统
                    ColorRange _clorRg = new ColorRange();
                    _clorRg.R = color.R;
                    _clorRg.G = color.G;
                    _clorRg.B = color.B;
                    _clorRg.Max_Limit = _flLimit;
                    Cls_Plant_C.colorRange.Add(_clorRg);
                    #endregion 2
                    //    SetBatColor(i, (int)iGetWith_Low, color, 2);
                    #region  3  刻度值递增  
                    //   Txt_C.Text += (Txt_C.Text == "" ? "" : " ") + "(" + _flLimit.ToString("0.00") + ": " + strT + ")";
                    _flLimit += _flScale_Low;
                    #endregion 3
                }
                #endregion 1
                #endregion 单色
            }
            else
            {
                /*
                 思路：第一色标最大值1mm,后续递增
                 */
                #region 多色，正常测试
                P_Md.Width = (int)(Grp_Stand_Color.Width * flMd_Bl);// SysInfo.m_SysInfo.g_Gate.Num_Base_Color;
                P_1.Width = (Grp_Stand_Color.Width - P_Md.Width) / 2;// (Grp_Stand_Color.Width - P_Md.Width) / 2;
                P_2.Width = Grp_Stand_Color.Width - P_1.Width - P_Md.Width;
                P_1.Left = 1;
                P_Md.Left = P_1.Left + P_1.Width;
                P_2.Left = P_Md.Left + P_Md.Width;

                P_1.Visible = true; P_2.Visible = true; P_Md.Visible = true;
                flJG_Low = (float)((Grp_Stand_Color.Width) / iGetWith_Low);

                //图片2点数据量：由最小刻度倍数和第一图片间隔获得第二图片最小间隔，然后获得图片2点数量
                int iP_1_NumJg = (int)(((P_1.Width / (float)(Grp_Stand_Color.Width - 2)) * (float)iGetWith_Low));// (int)(P_2.ClientSize.Width / (flJG_Low * (_flScale_Height / _flScale_Low)));
                int iP_2_NumJg = (int)((P_2.Width / (float)(Grp_Stand_Color.Width - 2)) * (float)iGetWith_Low);// P_2.ClientSize.Width / iP_2_NumJg;// iGetWith_Height;
                int iP_Md_NumJg = (int)((P_Md.Width / (float)(Grp_Stand_Color.Width - 2)) * (float)iGetWith_Low);
                iP_Md_NumJg--;


                Color[] _ArrColor = GetStantCol(str_Wc_Start_End_Col);

                float flStart = 0;
                float fl_JG = flstrThickAlarm / (iGetWith_Low - 1);// float.Parse(strWc_Fbl);//  0.01f;// float.Parse(Txt_Wc_fbl.Text);// Ck_By_Gchd.Checked ? _flScale_Low : 0.01f;
                strWc_Fbl = fl_JG.ToString("f2");
                PlanScheme_ColorLimit_JG(_ArrColor, P_1, iP_1_NumJg, ref Rr_Bitmap_1, 0, 0, fl_JG, ref flStart, flNormal_Thickness);
                PlanScheme_ColorLimit_JG(_ArrColor, P_Md, iP_Md_NumJg, ref Rr_Bitmap_Md, 3, 0, fl_JG, ref flStart, flNormal_Thickness);
                PlanScheme_ColorLimit_JG(_ArrColor, P_2, iP_2_NumJg, ref Rr_Bitmap_2, 1, 0, fl_JG, ref flStart, flNormal_Thickness);

                _flLimit = 0.0f;

                #region 1
                Cls_Plant_C.colorRange.Clear();
                float _flLess = (iRad_Dw == 0 ? float.Parse(strWc_Fbl) : 0f);
                for (int i = 0; i < iP_1_NumJg; i++)
                {
                    #region   1 获得色度值
                    color = Rr_Bitmap_1.GetPixel((int)(i * (flJG_Low - _flLess)), 0);
                    strT = color.R.ToString() + "," + color.G.ToString() + "," + color.B.ToString();
                    //      SetBatColor(i, (int)iGetWith_Low, color, 0);
                    #endregion 1
                    #region 2 将误差限数据添加进系统
                    ColorRange _clorRg = new ColorRange();
                    _clorRg.R = color.R;
                    _clorRg.G = color.G;
                    _clorRg.B = color.B;
                    _clorRg.Max_Limit = _flLimit;
                    Cls_Plant_C.colorRange.Add(_clorRg);
                    #endregion 2

                    #region  3 误差限色标添加 刻度累加
                    _flLimit += float.Parse(strWc_Fbl); // _flScale_Low;
                    _flLimit = float.Parse(_flLimit.ToString(_strXsw));
                    #endregion 3
                }
                #endregion 1

                #region 中间过度色
                for (int i = 0; i < iP_Md_NumJg; i++)
                {
                    #region   1 获得色度值
                    color = Rr_Bitmap_Md.GetPixel((int)(i * (flJG_Low - _flLess)), 0);
                    strT = color.R.ToString() + "," + color.G.ToString() + "," + color.B.ToString();
                    #endregion 1
                    #region 2 将误差限数据添加进系统
                    ColorRange _clorRg = new ColorRange();
                    _clorRg.R = color.R;
                    _clorRg.G = color.G;
                    _clorRg.B = color.B;
                    _clorRg.Max_Limit = _flLimit;
                    Cls_Plant_C.colorRange.Add(_clorRg);
                    #endregion 2
                    #region  3 误差限色标添加 刻度累加
                    //  Txt_C.Text += (Txt_C.Text == "" ? "" : " ") + "(" + _flLimit + ": " + strT + ")";
                    _flLimit += float.Parse(strWc_Fbl); //_flLimit += _flScale_Low;
                    _flLimit = float.Parse(_flLimit.ToString(_strXsw));
                    #endregion 3
                }
                #endregion 中间过度色

                #region 2
                for (int i = 0; i < iP_2_NumJg; i++)
                {
                    try
                    {
                        #region   1 获得色度值
                        color = Rr_Bitmap_2.GetPixel((int)(i * (flJG_Low - _flLess)), 0); //(iMove_1 + i * iJG_Height, 0);
                        strT = color.R.ToString() + "," + color.G.ToString() + "," + color.B.ToString();
                        #endregion 1
                        //  SetBatColor(i, (int)iP_2_NumJg, color, 1);
                        #region 2 将误差限数据添加进系统
                        ColorRange _clorRg = new ColorRange();
                        _clorRg.R = color.R;
                        _clorRg.G = color.G;
                        _clorRg.B = color.B;
                        _clorRg.Max_Limit = _flLimit;
                        if (_flLimit == 2.9)
                        { }
                        Cls_Plant_C.colorRange.Add(_clorRg);
                        #endregion 2
                        #region  3 误差限色标添加 刻度累加
                        //    Txt_C.Text += (Txt_C.Text == "" ? "" : " ") + "(" + _flLimit + ": " + strT + ")";
                        _flLimit += float.Parse(strWc_Fbl); //_flLimit += _flScale_Low;
                        _flLimit = float.Parse(_flLimit.ToString(_strXsw));
                        #endregion 3
                    }
                    catch { }
                }
                #endregion 2
                #endregion 多色
            }
        }

        public void InitLimitPic_A(string strNum, PictureBox P_1, PictureBox P_Md, PictureBox P_2,
                        System.Windows.Forms.GroupBox Grp_Stand_Color, float flMd_Bl = 0.4f)
        {
            string strType = strNum;
            m_iLastNo = 0;

            Bitmap Rr_Bitmap_1 = new Bitmap(P_1.ClientSize.Width, P_1.ClientSize.Height);
            Bitmap Rr_Bitmap_2 = new Bitmap(P_1.ClientSize.Width, P_1.ClientSize.Height);
            Bitmap Rr_Bitmap_Md = new Bitmap(P_1.ClientSize.Width, P_1.ClientSize.Height);
            Color color = Rr_Bitmap_1.GetPixel(0, 0);
            float flJG_Low = 0;
            float _flLimit = 0f;//刻度基数
            string _strXsw = "f2";// SysInfo.m_SysInfo.g_ScreenPlant.iRad_Dw == 1 ? "f4" : "f2";

            float iGetWith_Low = 0;//第一色标点个数 误差点个数
#pragma warning disable CS0219 // 变量“iLefMove”已被赋值，但从未使用过它的值
            int iLefMove = 0;
#pragma warning restore CS0219 // 变量“iLefMove”已被赋值，但从未使用过它的值
            string strT = "";

            //刻度最小单位
            float _flScale_Low = 0;//最小刻度值：由厚度和刻度点自动计算

            if (strType == "单色" || strType == "10")
            {
                //   Grp_100.Visible = false;
                if (strType == "10")
                    iGetWith_Low = 11;
                else
                    iGetWith_Low = 101;
                P_Md.Visible = false;
            }
            else
            {
                P_Md.Visible = true;
                iGetWith_Low = float.Parse(strType);

                iGetWith_Low += 1;
            }
            _flScale_Low = flNormal_Thickness / (iGetWith_Low - 1);

            if (strType == "单色" || strType == "10")
            {
                /*
               思路：  用井壁厚度作为宽带参数，
                 */
                //    EnbColor(false);
                #region 单色
                //1 确定位置
                P_1.Width = Grp_Stand_Color.Width - 4;
                P_1.Left = 2; P_1.Visible = true;
                P_2.Visible = false; P_Md.Visible = false;
                //1.2 色标间隔
                if (strType == "10")
                    iGetWith_Low = 11;
                else
                    iGetWith_Low = 101;
                flJG_Low = (float)(P_1.ClientSize.Width / iGetWith_Low);
                //2 画色标图
                PlanScheme_ColorLimit(P_1, (int)flJG_Low, ref Rr_Bitmap_1, 2, 0);

                _flLimit = 0.0f;
                #region 1 统计色标值
                for (int i = 0; i < iGetWith_Low; i++)
                {
                    #region   1 获得色度值
                    color = Rr_Bitmap_1.GetPixel((int)(i * (flJG_Low - 0.01)), 0);
                    strT = color.R.ToString() + "," + color.G.ToString() + "," + color.B.ToString();
                    #endregion 1
                    #region 2 将误差限数据添加进系统
                    ColorRange _clorRg = new ColorRange();
                    _clorRg.R = color.R;
                    _clorRg.G = color.G;
                    _clorRg.B = color.B;
                    _clorRg.Max_Limit = _flLimit;
                    Cls_Plant_C.colorRange.Add(_clorRg);
                    #endregion 2
                    //    SetBatColor(i, (int)iGetWith_Low, color, 2);
                    #region  3  刻度值递增  
                    //   Txt_C.Text += (Txt_C.Text == "" ? "" : " ") + "(" + _flLimit.ToString("0.00") + ": " + strT + ")";
                    _flLimit += _flScale_Low;
                    #endregion 3
                }
                #endregion 1
                #endregion 单色
            }
            else
            {
                /*
                 思路：第一色标最大值1mm,后续递增
                 */
                #region 多色，正常测试
                P_Md.Width = (int)(Grp_Stand_Color.Width * flMd_Bl);// SysInfo.m_SysInfo.g_Gate.Num_Base_Color;
                P_1.Width = (Grp_Stand_Color.Width - P_Md.Width) / 2;// (Grp_Stand_Color.Width - P_Md.Width) / 2;
                P_2.Width = Grp_Stand_Color.Width - P_1.Width - P_Md.Width;
                P_1.Left = 1;
                P_Md.Left = P_1.Left + P_1.Width;
                P_2.Left = P_Md.Left + P_Md.Width;

                P_1.Visible = true; P_2.Visible = true; P_Md.Visible = true;
                flJG_Low = (float)((Grp_Stand_Color.Width) / iGetWith_Low);

                //图片2点数据量：由最小刻度倍数和第一图片间隔获得第二图片最小间隔，然后获得图片2点数量
                int iP_1_NumJg = (int)(((P_1.Width / (float)(Grp_Stand_Color.Width - 2)) * (float)iGetWith_Low));// (int)(P_2.ClientSize.Width / (flJG_Low * (_flScale_Height / _flScale_Low)));
                int iP_2_NumJg = (int)((P_2.Width / (float)(Grp_Stand_Color.Width - 2)) * (float)iGetWith_Low);// P_2.ClientSize.Width / iP_2_NumJg;// iGetWith_Height;
                int iP_Md_NumJg = (int)((P_Md.Width / (float)(Grp_Stand_Color.Width - 2)) * (float)iGetWith_Low);
                iP_Md_NumJg--;
                if (i_Col_A_By_1 == 1)
                    str_Wc_Start_End_Col_A = "0/0/255|0/0/160|0/0/64|64/0/0";
                Color[] _ArrColor = GetStantCol(str_Wc_Start_End_Col_A);

                float flStart = 0;
                float fl_JG = flstrThickAlarm / (iGetWith_Low - 1);// float.Parse(strWc_Fbl);//  0.01f;// float.Parse(Txt_Wc_fbl.Text);// Ck_By_Gchd.Checked ? _flScale_Low : 0.01f;
                strWc_Fbl = fl_JG.ToString("f2");
                PlanScheme_ColorLimit_JG(_ArrColor, P_1, iP_1_NumJg, ref Rr_Bitmap_1, 0, 0, fl_JG, ref flStart, flNormal_Thickness);
                PlanScheme_ColorLimit_JG(_ArrColor, P_Md, iP_Md_NumJg, ref Rr_Bitmap_Md, 3, 0, fl_JG, ref flStart, flNormal_Thickness);
                PlanScheme_ColorLimit_JG(_ArrColor, P_2, iP_2_NumJg, ref Rr_Bitmap_2, 1, 0, fl_JG, ref flStart, flNormal_Thickness);

                _flLimit = 0.0f;

                #region 1
                Cls_Plant_C.colorRange_A.Clear();
                float _flLess = (iRad_Dw == 0 ? float.Parse(strWc_Fbl) : 0f);
                for (int i = 0; i < iP_1_NumJg; i++)
                {
                    #region   1 获得色度值
                    color = Rr_Bitmap_1.GetPixel((int)(i * (flJG_Low - _flLess)), 0);
                    strT = color.R.ToString() + "," + color.G.ToString() + "," + color.B.ToString();
                    //      SetBatColor(i, (int)iGetWith_Low, color, 0);
                    #endregion 1
                    #region 2 将误差限数据添加进系统
                    ColorRange _clorRg = new ColorRange();
                    _clorRg.R = color.R;
                    _clorRg.G = color.G;
                    _clorRg.B = color.B;
                    _clorRg.Max_Limit = _flLimit;
                    Cls_Plant_C.colorRange_A.Add(_clorRg);
                    #endregion 2

                    #region  3 误差限色标添加 刻度累加
                    _flLimit += float.Parse(strWc_Fbl); // _flScale_Low;
                    _flLimit = float.Parse(_flLimit.ToString(_strXsw));
                    #endregion 3
                }
                #endregion 1

                #region 中间过度色
                for (int i = 0; i < iP_Md_NumJg; i++)
                {
                    #region   1 获得色度值
                    color = Rr_Bitmap_Md.GetPixel((int)(i * (flJG_Low - _flLess)), 0);
                    strT = color.R.ToString() + "," + color.G.ToString() + "," + color.B.ToString();
                    #endregion 1
                    #region 2 将误差限数据添加进系统
                    ColorRange _clorRg = new ColorRange();
                    _clorRg.R = color.R;
                    _clorRg.G = color.G;
                    _clorRg.B = color.B;
                    _clorRg.Max_Limit = _flLimit;
                    Cls_Plant_C.colorRange_A.Add(_clorRg);
                    #endregion 2
                    #region  3 误差限色标添加 刻度累加
                    //  Txt_C.Text += (Txt_C.Text == "" ? "" : " ") + "(" + _flLimit + ": " + strT + ")";
                    _flLimit += float.Parse(strWc_Fbl); //_flLimit += _flScale_Low;
                    _flLimit = float.Parse(_flLimit.ToString(_strXsw));
                    #endregion 3
                }
                #endregion 中间过度色

                #region 2
                for (int i = 0; i < iP_2_NumJg; i++)
                {
                    try
                    {
                        #region   1 获得色度值
                        color = Rr_Bitmap_2.GetPixel((int)(i * (flJG_Low - _flLess)), 0); //(iMove_1 + i * iJG_Height, 0);
                        strT = color.R.ToString() + "," + color.G.ToString() + "," + color.B.ToString();
                        #endregion 1
                        //  SetBatColor(i, (int)iP_2_NumJg, color, 1);
                        #region 2 将误差限数据添加进系统
                        ColorRange _clorRg = new ColorRange();
                        _clorRg.R = color.R;
                        _clorRg.G = color.G;
                        _clorRg.B = color.B;
                        _clorRg.Max_Limit = _flLimit;
                        if (_flLimit == 2.9)
                        { }
                        Cls_Plant_C.colorRange_A.Add(_clorRg);
                        #endregion 2
                        #region  3 误差限色标添加 刻度累加
                        //    Txt_C.Text += (Txt_C.Text == "" ? "" : " ") + "(" + _flLimit + ": " + strT + ")";
                        _flLimit += float.Parse(strWc_Fbl); //_flLimit += _flScale_Low;
                        _flLimit = float.Parse(_flLimit.ToString(_strXsw));
                        #endregion 3
                    }
                    catch { }
                }
                #endregion 2
                #endregion 多色
            }
        }
        /// <summary>
        /// 解析标准颜色
        /// </summary>
        /// <returns></returns>
        private System.Drawing.Color[] GetStantCol(string _str_Wc_Start_End_Col)
        {
            if (_str_Wc_Start_End_Col == "")
                _str_Wc_Start_End_Col = "0/0/255|154/205/50|255/255/0|255/0/0";
            System.Drawing.Color[] _RetArrColor = new System.Drawing.Color[4];

            string[] _sPara = "".Split('|');
            string[] _sPara_Sub = "".Split('|');
            int iR = 0, iG = 0, iB = 0;

            if (_str_Wc_Start_End_Col != "")
                _sPara = _str_Wc_Start_End_Col.Split('|');
            if (_sPara.Length >= 4)
            {
                #region 1 开始色
                _sPara_Sub = _sPara[0].Split('/');
                if (_sPara_Sub.Length == 3)
                {
                    if (_sPara_Sub[0] != "" && _sPara_Sub[1] != "" && _sPara_Sub[2] != "")
                    {
                        iR = int.Parse(_sPara_Sub[0]);
                        iG = int.Parse(_sPara_Sub[1]);
                        iB = int.Parse(_sPara_Sub[2]);
                        _RetArrColor[0] = System.Drawing.Color.FromArgb((byte)iR, (byte)iG, (byte)iB);
                    }
                }
                #endregion 1
                #region 2 中间开始色
                _sPara_Sub = _sPara[1].Split('/');
                if (_sPara_Sub.Length == 3)
                {
                    if (_sPara_Sub[0] != "" && _sPara_Sub[1] != "" && _sPara_Sub[2] != "")
                    {
                        iR = int.Parse(_sPara_Sub[0]);
                        iG = int.Parse(_sPara_Sub[1]);
                        iB = int.Parse(_sPara_Sub[2]);
                        _RetArrColor[1] = System.Drawing.Color.FromArgb((byte)iR, (byte)iG, (byte)iB);
                    }
                }
                #endregion 2
                #region 3 中间开始色
                _sPara_Sub = _sPara[2].Split('/');
                if (_sPara_Sub.Length == 3)
                {
                    if (_sPara_Sub[0] != "" && _sPara_Sub[1] != "" && _sPara_Sub[2] != "")
                    {
                        iR = int.Parse(_sPara_Sub[0]);
                        iG = int.Parse(_sPara_Sub[1]);
                        iB = int.Parse(_sPara_Sub[2]);
                        _RetArrColor[2] = System.Drawing.Color.FromArgb((byte)iR, (byte)iG, (byte)iB);
                    }
                }
                #endregion 3
                #region 4 结束色
                _sPara_Sub = _sPara[3].Split('/');
                if (_sPara_Sub.Length == 3)
                {
                    if (_sPara_Sub[0] != "" && _sPara_Sub[1] != "" && _sPara_Sub[2] != "")
                    {
                        iR = int.Parse(_sPara_Sub[0]);
                        iG = int.Parse(_sPara_Sub[1]);
                        iB = int.Parse(_sPara_Sub[2]);
                        _RetArrColor[3] = System.Drawing.Color.FromArgb((byte)iR, (byte)iG, (byte)iB);
                    }
                }
                #endregion 4
            }

            return _RetArrColor;
        }
        /// <summary>
        /// 画方案三图片画色标
        /// </summary>
        /// <param name="PicArea"></param>
        /// <param name="Rr_Bitmap"></param>
        /// <param name="iType"></param>
        /// <param name="iHorizontal_0"></param>
        /// <returns></returns>
        private int PlanScheme_ColorLimit(System.Windows.Forms.PictureBox PicArea, int iJG, ref Bitmap Rr_Bitmap, int iType, int iHorizontal_0)
        {

            int _iRet = 0;//画图返回
            try
            {
                float flPic_W = PicArea.Width;//画布宽度
                float flPic_H = PicArea.Height;//画布高度

                #region 1.2 准备画布
                // 初始化画板，在内存中建立一块虚拟画布
                Bitmap image = new Bitmap(PicArea.ClientSize.Width, PicArea.ClientSize.Height);

                // 获取背景层
                Bitmap bg = (Bitmap)PicArea.BackgroundImage;
                // 初始化整个画布
                Bitmap canvas = new Bitmap(PicArea.ClientSize.Width, PicArea.ClientSize.Height);
                // 初始化图形面板，获取这块内存画布的Graphics的引用
                Graphics g = Graphics.FromImage(image);
                Graphics gb = Graphics.FromImage(canvas);

                #endregion  准备画布
                g.Clear(Color.White);// g.Clear(Color.Teal);// CadetBlue);

                Rectangle rect = new Rectangle(0, 0, PicArea.ClientSize.Width, PicArea.ClientSize.Height);


                rect.Location = new Point(0, 0);
                LinearGradientBrush b3 = new LinearGradientBrush(rect, Color.Blue, Color.White, LinearGradientMode.Horizontal);

                if (iHorizontal_0 == 0)
                {
                    switch (iType)
                    {
                        case 0:

                            b3 = new LinearGradientBrush(rect, Color.Blue, Color.YellowGreen, LinearGradientMode.Horizontal);//WhiteSmoke
                            break;
                        case 2:
                            b3 = new LinearGradientBrush(rect, Color.Blue, Color.White, LinearGradientMode.Horizontal);
                            break;
                        case 1:
                            b3 = new LinearGradientBrush(rect, Color.Yellow, Color.Red, LinearGradientMode.Horizontal);
                            break;
                        case 3:
                            b3 = new LinearGradientBrush(rect, Color.YellowGreen, Color.Yellow, LinearGradientMode.Horizontal);
                            break;
                    }
                }
                else
                    switch (iType)
                    {
                        case 0:

                            b3 = new LinearGradientBrush(rect, Color.YellowGreen, Color.Blue, LinearGradientMode.Vertical);//WhiteSmoke
                            break;
                        case 2:
                            b3 = new LinearGradientBrush(rect, Color.Blue, Color.White, LinearGradientMode.Vertical);
                            break;
                        case 1:
                            b3 = new LinearGradientBrush(rect, Color.Red, Color.Yellow, LinearGradientMode.Vertical);
                            break;

                    }

                g.FillRectangle(b3, rect);

                if (iHorizontal_0 == 1)
                {
                    Pen p = new Pen(Brushes.Green);//刻度笔
#pragma warning disable CS0168 // 声明了变量“iH”，但从未使用过
                    int X, Y, iBzNum, iH, iXcale = 6;//位置
#pragma warning restore CS0168 // 声明了变量“iH”，但从未使用过
                    string strT = "";
                    Font drawFont = new Font("Arial", 8);//刻度字体定义
                    SolidBrush drawBrush = new SolidBrush(Color.Green);//刻度刷子

                    switch (iType)
                    {
                        case 0:
                            iBzNum = (PicArea.Height - 6) / iJG;
                            X = 0;
                            #region 1 画标准色标和刻度线
                            for (int i = 1; i <= iBzNum; i++)
                            {
                                //1 定位置和取颜色  画色标
                                Y = PicArea.Height - i * iJG;
                                try
                                {
                                    //if (i < iYsPort)
                                    p = new Pen(Brushes.White);//刻度笔

                                    //2 刻度
                                    g.DrawLine(p, new Point(X, Y), new Point(iXcale, Y));
                                    //3 画箭头
                                    //g.DrawLine(p, new Point(iW / 2 + 6, Y), new Point(iW / 2 + 6, Y + 6));
                                    //g.DrawLine(p, new Point(iW / 2 + 6 - 3, Y + 3), new Point(iW / 2 + 6, Y + 6));
                                    //g.DrawLine(p, new Point(iW / 2 + 6, Y + 6), new Point(iW / 2 + 6 + 3, Y + 3));
                                }
                                catch { }
                            }
                            #endregion 1
                            #region 2 画刻度值
                            X = iXcale + 2;
                            float _flJg = 0.0f;
                            for (int i = 1; i <= iBzNum; i += 3)
                            {
                                Y = PicArea.Height - i * iJG - 4; //(iBzNum - i) * iJG;
                                PointF drawPoint = new PointF(X, Y);

                                _flJg = (i - 1) * 0.1f;
                                strT = _flJg.ToString("0.0");
                                drawBrush = new SolidBrush(Color.White);//刻度刷子
                                g.DrawString(strT, drawFont, drawBrush, drawPoint);
                            }
                            // _iRet = _flJg;
                            #endregion 2
                            break;
                        case 1:
                            break;
                        case 2:
                            break;
                    }

                }

                #region  4 刷新
                Rectangle _Rect = new Rectangle(0, 0, PicArea.Width, PicArea.Height);

                if (bg != null)
                    gb.DrawImage(bg, _Rect);// 先绘制背景层
                gb.DrawImage(image, _Rect); // 再绘制绘画层

                PicArea.BackgroundImage = (Bitmap)canvas.Clone();// (Bitmap)canvas.Clone(); // 设置为背景层，就是在内存中建立了一张对应的画布，重新refresh就是再次的加载这个画布
                Rr_Bitmap = (Bitmap)canvas.Clone();

                g.Dispose(); g = null;
                image.Dispose();
                gb.Dispose();
                canvas.Dispose();

                PicArea.Refresh();
                #endregion 刷新

                canvas.Dispose();
                image.Dispose();


            }
            catch { }

            return _iRet;
        }

        private int PlanScheme_ColorLimit_MyColor(System.Windows.Forms.PictureBox PicArea, int iJG, ref Bitmap Rr_Bitmap, int iType, int iHorizontal_0, Color[] ArrColor)
        {

            int _iRet = 0;//画图返回
            try
            {
                float flPic_W = PicArea.Width;//画布宽度
                float flPic_H = PicArea.Height;//画布高度

                #region 1.2 准备画布
                // 初始化画板，在内存中建立一块虚拟画布
                Bitmap image = new Bitmap(PicArea.ClientSize.Width, PicArea.ClientSize.Height);

                // 获取背景层
                Bitmap bg = (Bitmap)PicArea.BackgroundImage;
                // 初始化整个画布
                Bitmap canvas = new Bitmap(PicArea.ClientSize.Width, PicArea.ClientSize.Height);
                // 初始化图形面板，获取这块内存画布的Graphics的引用
                Graphics g = Graphics.FromImage(image);
                Graphics gb = Graphics.FromImage(canvas);

                #endregion  准备画布
                g.Clear(Color.White);// g.Clear(Color.Teal);// CadetBlue);

                Rectangle rect = new Rectangle(0, 0, PicArea.ClientSize.Width, PicArea.ClientSize.Height);


                rect.Location = new Point(0, 0);
                LinearGradientBrush b3 = new LinearGradientBrush(rect, Color.Blue, Color.White, LinearGradientMode.Horizontal);

                if (iHorizontal_0 == 0)
                {
                    if (ArrColor.Length == 4)
                    {
                        switch (iType)
                        {
                            case 0:
                                b3 = new LinearGradientBrush(rect, ArrColor[0], ArrColor[1], LinearGradientMode.Horizontal);//WhiteSmoke
                                break;
                            case 2:
                                b3 = new LinearGradientBrush(rect, Color.Blue, Color.White, LinearGradientMode.Horizontal);
                                break;
                            case 1:
                                b3 = new LinearGradientBrush(rect, ArrColor[2], ArrColor[3], LinearGradientMode.Horizontal);
                                break;
                            case 3:
                                b3 = new LinearGradientBrush(rect, ArrColor[1], ArrColor[2], LinearGradientMode.Horizontal);
                                break;
                        }
                    }
                    else
                    {
                        switch (iType)
                        {
                            case 0:

                                b3 = new LinearGradientBrush(rect, Color.Blue, Color.YellowGreen, LinearGradientMode.Horizontal);//WhiteSmoke
                                break;
                            case 2:
                                b3 = new LinearGradientBrush(rect, Color.Blue, Color.White, LinearGradientMode.Horizontal);
                                break;
                            case 1:
                                b3 = new LinearGradientBrush(rect, Color.Yellow, Color.Red, LinearGradientMode.Horizontal);
                                break;
                            case 3:
                                b3 = new LinearGradientBrush(rect, Color.YellowGreen, Color.Yellow, LinearGradientMode.Horizontal);
                                break;
                        }
                    }
                }
                else
                    switch (iType)
                    {
                        case 0:

                            b3 = new LinearGradientBrush(rect, Color.YellowGreen, Color.Blue, LinearGradientMode.Vertical);//WhiteSmoke
                            break;
                        case 2:
                            b3 = new LinearGradientBrush(rect, Color.Blue, Color.White, LinearGradientMode.Vertical);
                            break;
                        case 1:
                            b3 = new LinearGradientBrush(rect, Color.Red, Color.Yellow, LinearGradientMode.Vertical);
                            break;

                    }

                g.FillRectangle(b3, rect);

                if (iHorizontal_0 == 1)
                {
                    Pen p = new Pen(Brushes.Green);//刻度笔
#pragma warning disable CS0168 // 声明了变量“iH”，但从未使用过
                    int X, Y, iBzNum, iH, iXcale = 6;//位置
#pragma warning restore CS0168 // 声明了变量“iH”，但从未使用过
                    string strT = "";
                    Font drawFont = new Font("Arial", 8);//刻度字体定义
                    SolidBrush drawBrush = new SolidBrush(Color.Green);//刻度刷子

                    switch (iType)
                    {
                        case 0:
                            iBzNum = (PicArea.Height - 6) / iJG;
                            X = 0;
                            #region 1 画标准色标和刻度线
                            for (int i = 1; i <= iBzNum; i++)
                            {
                                //1 定位置和取颜色  画色标
                                Y = PicArea.Height - i * iJG;
                                try
                                {
                                    //if (i < iYsPort)
                                    p = new Pen(Brushes.White);//刻度笔

                                    //2 刻度
                                    g.DrawLine(p, new Point(X, Y), new Point(iXcale, Y));
                                    //3 画箭头
                                    //g.DrawLine(p, new Point(iW / 2 + 6, Y), new Point(iW / 2 + 6, Y + 6));
                                    //g.DrawLine(p, new Point(iW / 2 + 6 - 3, Y + 3), new Point(iW / 2 + 6, Y + 6));
                                    //g.DrawLine(p, new Point(iW / 2 + 6, Y + 6), new Point(iW / 2 + 6 + 3, Y + 3));
                                }
                                catch { }
                            }
                            #endregion 1
                            #region 2 画刻度值
                            X = iXcale + 2;
                            float _flJg = 0.0f;
                            for (int i = 1; i <= iBzNum; i += 3)
                            {
                                Y = PicArea.Height - i * iJG - 4; //(iBzNum - i) * iJG;
                                PointF drawPoint = new PointF(X, Y);

                                _flJg = (i - 1) * 0.1f;
                                strT = _flJg.ToString("0.0");
                                drawBrush = new SolidBrush(Color.White);//刻度刷子
                                g.DrawString(strT, drawFont, drawBrush, drawPoint);
                            }
                            // _iRet = _flJg;
                            #endregion 2
                            break;
                        case 1:
                            break;
                        case 2:
                            break;
                    }

                }

                #region  4 刷新
                Rectangle _Rect = new Rectangle(0, 0, PicArea.Width, PicArea.Height);

                if (bg != null)
                    gb.DrawImage(bg, _Rect);// 先绘制背景层
                gb.DrawImage(image, _Rect); // 再绘制绘画层

                PicArea.BackgroundImage = (Bitmap)canvas.Clone();// (Bitmap)canvas.Clone(); // 设置为背景层，就是在内存中建立了一张对应的画布，重新refresh就是再次的加载这个画布
                Rr_Bitmap = (Bitmap)canvas.Clone();

                g.Dispose(); g = null;
                image.Dispose();
                gb.Dispose();
                canvas.Dispose();

                PicArea.Refresh();
                #endregion 刷新

                canvas.Dispose();
                image.Dispose();


            }
            catch { }

            return _iRet;
        }
        private float PlanScheme_ColorLimit_JG(Color[] ArrColor, System.Windows.Forms.PictureBox PicArea, int iJG, ref Bitmap Rr_Bitmap, int iType,
                                  int iHorizontal_0, float fl_JG, ref float flStart, float _flThickness_Wall = 10, int _iScale_Per = 0)
        {
#pragma warning disable CS0219 // 变量“_iRet”已被赋值，但从未使用过它的值
            int _iRet = 0;//画图返回
#pragma warning restore CS0219 // 变量“_iRet”已被赋值，但从未使用过它的值
            try
            {
                float flPic_W = PicArea.Width;//画布宽度
                float flPic_H = PicArea.Height;//画布高度

                #region 1.2 准备画布
                // 初始化画板，在内存中建立一块虚拟画布
                Bitmap image = new Bitmap(PicArea.ClientSize.Width, PicArea.ClientSize.Height);

                // 获取背景层
                Bitmap bg = (Bitmap)PicArea.BackgroundImage;
                // 初始化整个画布
                Bitmap canvas = new Bitmap(PicArea.ClientSize.Width, PicArea.ClientSize.Height);
                // 初始化图形面板，获取这块内存画布的Graphics的引用
                Graphics g = Graphics.FromImage(image);
                Graphics gb = Graphics.FromImage(canvas);

                #endregion  准备画布
                g.Clear(Color.White);// g.Clear(Color.Teal);// CadetBlue);
                Rectangle rect = new Rectangle(0, 0, PicArea.ClientSize.Width, PicArea.ClientSize.Height);

                rect.Location = new Point(0, 0);
                LinearGradientBrush b3 = new LinearGradientBrush(rect, Color.Blue, Color.White, LinearGradientMode.Horizontal);
                if (ArrColor[0].R == 0 && ArrColor[0].G == 0 && ArrColor[0].B == 0)
                {
                    ArrColor[0] = Color.Blue; ArrColor[1] = Color.YellowGreen;
                    ArrColor[2] = Color.Yellow; ArrColor[3] = Color.Red;
                }
                if (iHorizontal_0 == 0)
                {

                    switch (iType)
                    {
                        case 0:
                            b3 = new LinearGradientBrush(rect, ArrColor[0], ArrColor[1], LinearGradientMode.Horizontal); //Color.Blue, Color.YellowGreen, 
                            break;
                        case 2:
                            b3 = new LinearGradientBrush(rect, ArrColor[0], ArrColor[1], LinearGradientMode.Horizontal);//Color.Blue, Color.White,
                            break;
                        case 1:
                            b3 = new LinearGradientBrush(rect, ArrColor[2], ArrColor[3], LinearGradientMode.Horizontal);// Color.Yellow, Color.Red,
                            break;
                        case 3:
                            b3 = new LinearGradientBrush(rect, ArrColor[1], ArrColor[2], LinearGradientMode.Horizontal);// Color.YellowGreen, Color.Yellow,
                            break;
                    }
                }
                else
                    switch (iType)
                    {
                        case 0:
                            b3 = new LinearGradientBrush(rect, ArrColor[0], ArrColor[1], LinearGradientMode.Vertical); //Color.Blue, Color.YellowGreen, 
                            break;
                        case 2:
                            b3 = new LinearGradientBrush(rect, ArrColor[0], ArrColor[1], LinearGradientMode.Vertical);//Color.Blue, Color.White,
                            break;
                        case 1:
                            b3 = new LinearGradientBrush(rect, ArrColor[2], ArrColor[3], LinearGradientMode.Vertical);// Color.Yellow, Color.Red,
                            break;
                        case 3:
                            b3 = new LinearGradientBrush(rect, ArrColor[1], ArrColor[2], LinearGradientMode.Vertical);// Color.YellowGreen, Color.Yellow,
                            break;

                    }

                g.FillRectangle(b3, rect);

                Pen p = new Pen(Brushes.Green);//刻度笔
#pragma warning disable CS0168 // 声明了变量“iH”，但从未使用过
#pragma warning disable CS0219 // 变量“iXcale”已被赋值，但从未使用过它的值
                int X, Y, iBzNum, iH, iXcale = 6;//位置
#pragma warning restore CS0219 // 变量“iXcale”已被赋值，但从未使用过它的值
#pragma warning restore CS0168 // 声明了变量“iH”，但从未使用过
                string strT = "";
                Font drawFont = new Font("Arial", 8);//刻度字体定义
                SolidBrush drawBrush = new SolidBrush(iType != 0 ? Color.Black : Color.White);//刻度刷子

                if (iHorizontal_0 == 0)//横轴
                {
                    iBzNum = (int)((PicArea.Width) / iJG);
                    if (iBzNum == 0) { iBzNum = PicArea.Width; }
                }
                else
                {
                    iBzNum = (int)((PicArea.Height) / iJG);
                    if (iBzNum == 0) { iBzNum = PicArea.Height; }
                }

                if (iHorizontal_0 == 0)//横轴
                {
                    X = 0;
                    Y = PicArea.Height - 2;
                }
                else
                {
                    X = 2;
                    Y = 0;
                }
                int Y_Titl = PicArea.Height - 15;
                float _flJg = 0f;
                //;//实际值/百分值
                StringFormat StrF = new StringFormat();
                StrF.FormatFlags = StringFormatFlags.DirectionVertical; //StringFormatFlags.DirectionVertical; StringFormatFlags.DirectionVertical; // 竖排
                float fl_ScaleLeast = 0.01f;

                float _fl_Jg = (float)PicArea.Width / iJG;
                if (iHorizontal_0 == 1)//横轴
                    _fl_Jg = (float)PicArea.Height / iJG;
                int iPic_Jg = (int)_fl_Jg;
                float _fl_Add = _fl_Jg - iPic_Jg;
                int i10 = iJG > 500 ? 100 : (iRad_Dw == 0 ? (iJG > 6 ? 10 : 2) : (iJG > 190 ? 20 : (iJG > 3 ? 4 : 2)));
                string _strDw = iRad_Dw == 0 ? "mm" : "in.";
                string _strXsw = iRad_Dw == 0 ? "0.00" : "0.00000";
                for (int i = 1; i <= iJG; i++)//iBzNum
                {
                    //1 定位置和取颜色  画色标
                    if (iHorizontal_0 == 0)//横轴
                        X = (int)(i * (iPic_Jg + _fl_Add));
                    else
                        Y = (int)(i * (iPic_Jg + _fl_Add));

                    #region 1 画标准色标和刻度线
                    try
                    {
                        p = new Pen(iType != 0 ? Brushes.Black : Brushes.White);//    Brushes.White);//刻度笔
                        //2 刻度
                        if (iHorizontal_0 == 0)//横轴
                            g.DrawLine(p, new Point(X, (i % 5 == 0 ? Y - 2 : Y)), new Point(X, PicArea.Height));
                        else//竖轴
                            g.DrawLine(p, new Point(0, Y), new Point((i % 5 == 0 ? 4 : 2), Y));

                        #region 值
                        _flJg = flStart + (i) * fl_JG;

                        if (_iScale_Per == 1)
                        {
                            _flJg = (_flThickness_Wall - _flJg) / _flThickness_Wall * 100;
                            strT = _flJg.ToString("0") + "%";
                        }
                        else
                            strT = _flJg.ToString(_strXsw) + (iType == 0 && i == 10 ? _strDw : "");

                        #endregion
                        if (i % i10 == 0)
                        {
                            if (iHorizontal_0 == 0)//横轴
                                g.DrawString(strT, drawFont, drawBrush, X - 7, (iType == 0 && i == 10 ? 10 : 26), StrF);
                            else//竖轴
                                if ((i / i10) % 2 == 1)
                                g.DrawString(strT, drawFont, drawBrush, 4, Y - 10, StrF);
                        }
                    }
                    catch { }
                }
                flStart = _flJg + fl_ScaleLeast;
                #endregion 1
                #region  4 刷新
                Rectangle _Rect = new Rectangle(0, 0, PicArea.Width, PicArea.Height);

                if (bg != null)
                    gb.DrawImage(bg, _Rect);// 先绘制背景层
                gb.DrawImage(image, _Rect); // 再绘制绘画层

                PicArea.BackgroundImage = (Bitmap)canvas.Clone();// (Bitmap)canvas.Clone(); // 设置为背景层，就是在内存中建立了一张对应的画布，重新refresh就是再次的加载这个画布
                Rr_Bitmap = (Bitmap)canvas.Clone();

                if (m_800Limit == null)
                    m_800Limit = new Color[1000];
                int iOne_JG = (int)(Rr_Bitmap.Height / iJG);
                int iY = 0, iEnd = 0, iBase = m_iLastNo;
                if (iType == 0)
                {
                    m_iLastNo = 0;
                    for (int i = 0; i < iJG; i++)
                    {
                        iY = Rr_Bitmap.Height - i * iOne_JG - 2;
                        m_800Limit[i] = Rr_Bitmap.GetPixel(10, iY);
                        m_iLastNo++;
                    }
                }
                else if (iType == 3)
                {
                    iEnd = iBase + iJG;
                    for (int i = 0; i < iEnd; i++)
                    {
                        iY = Rr_Bitmap.Height - i * iOne_JG - 2;
                        m_800Limit[iBase + i] = Rr_Bitmap.GetPixel(10, iY);
                        m_iLastNo++;
                    }
                }
                else
                {
                    iEnd = iBase + iJG;
                    for (int i = 0; i < iEnd; i++)
                    {
                        iY = Rr_Bitmap.Height - i * iOne_JG - 2;
                        m_800Limit[iBase + i] = Rr_Bitmap.GetPixel(10, iY);
                        m_iLastNo++;
                    }
                }
                g.Dispose(); g = null;
                image.Dispose();
                gb.Dispose();
                canvas.Dispose();

                PicArea.Refresh();
                #endregion 刷新

                canvas.Dispose();
                image.Dispose();
            }
            catch { }

            return flStart;
        }

        /// <summary>
        /// 减薄厚度颜色对照表
        /// </summary>
        public static List<ColorRange> colorRange = new List<ColorRange>();
        public static List<ColorRange> colorRange_ECT = new List<ColorRange>();
        /// <summary>
        /// 增厚颜色误差对照表
        /// </summary>
        public static List<ColorRange> colorRange_A = new List<ColorRange>();


        #region 涂层厚度差颜色
        public static List<ColorRange> m_Co_Coat_Jb = new List<ColorRange>();

        public static List<ColorRange> m_Co_Coat_Md = new List<ColorRange>();

        public static List<ColorRange> m_Co_Coat_Zh = new List<ColorRange>();
      
        #endregion 
        #endregion 误差颜色
        /// <summary>
        /// 计算屏幕行列个数
        /// </summary>
        /// <param name="PicArea"></param>
        public void GetRulerPara_C()
        {
            if (Pic_C == null) return;
            #region 由一个点宽度高度，获得屏幕参数
            int iScreen_With = Pic_C.Width - Chart_Ruler_X_Start;
            int iScreen_Height = Pic_C.Height - Chart_Ruler_Y_Start;
        //    MessageBox.Show("33-1" + iScreen_With.ToString () + "  " + Scree_iDotWith_X);
            Scree_iAllCols_C = iScreen_With / Scree_iDotWith_X;
            #region Y轴光栅臂对应图像点宽度以及数据行数
            i_Gsb_Arr_Len = 60;
            Scree_iDotHeightmm_Y =1.0f* iGsb_Len / i_Gsb_Arr_Len;
         //   MessageBox.Show("33-2" + Scree_iDotHeightmm_Y.ToString());
            float _T = 1.0f * iGsb_Len / (Scree_iDotHeightmm_Y);// * 1000);///单位m
            i_Gsb_Arr_Len = (int)_T;
         //   MessageBox.Show("33-3" + _T.ToString());
            Scree_iDotHeight = (iScreen_Height / _T);
            if (Scree_iDotHeight == 0) Scree_iDotHeight = 1;
         //   MessageBox.Show("33-4" + Scree_iDotHeight.ToString());
            Scree_iAllRows_C =(int)( iScreen_Height / Scree_iDotHeight);

            #region 涂层光栅臂单调计算
         //   MessageBox.Show("33-4" + Scree_iDotHeightmm_Y_Coat.ToString());
            if (Scree_iDotHeightmm_Y_Coat <= 0) Scree_iDotHeightmm_Y_Coat = 2;
             _T = 1.0f * iGsb_Len / (Scree_iDotHeightmm_Y_Coat);// * 1000);///单位m
            i_Gsb_Arr_Len_Coat_ALL = (int)_T;
           Scree_iDotHeight_Coat =  (iScreen_Height / _T);
            if (Scree_iDotHeight_Coat == 0) Scree_iDotHeight_Coat = 1;
          //  MessageBox.Show("33-4-" + Scree_iDotHeight_Coat.ToString());
            Scree_iAllRows_C_Coat =(int)( iScreen_Height / Scree_iDotHeight_Coat);
            #endregion 

            #endregion 行数
            #endregion 一个点宽度高度
        //    MessageBox.Show("33-4-" + Scree_iAllCols_C.ToString() +"  " + Scree_iDotWithmm_X);
            Scree_Stant_Distance = Scree_iAllCols_C * Scree_iDotWithmm_X;
            Init_KD();

        }
       
        /// <summary>
        /// 图形清零
        /// </summary>
        public void Chart_Clear(ref Struct_G Pic)
        {
            if (Pic.g != null)
            {
                Pic.g.Dispose(); Pic.g = null;
                Pic.image.Dispose();
                Pic.gb.Dispose();
                Pic.canvas.Dispose();
                //         Application.DoEvents();
            }
        }
        /// <summary>
        /// C扫描单图像: 纵横刻度尺
        /// </summary>
        public void Plant_Ruler_C()
        {
            if (Pic_C == null) return;
            //  if (blPlantRuler == 4) return;
            int i_X = 0, i_Y = 0;
            int iDatNum = 0;
            string strT = "";
            Chart_Clear(ref m_G_C);
            System.Windows.Forms.PictureBox.CheckForIllegalCrossThreadCalls = false;
            #region 画刻度尺

            #region 1 画布准备:初始化、右边出图、满屏时
            m_G_C.image = new Bitmap(Pic_C.ClientSize.Width, Pic_C.ClientSize.Height);
            // 获取背景层
            m_G_C.bg = (Bitmap)Pic_C.BackgroundImage;
            // 初始化整个画布
            m_G_C.canvas = new Bitmap(Pic_C.ClientSize.Width, Pic_C.ClientSize.Height);
            // 初始化图形面板，获取这块内存画布的Graphics的引用
            m_G_C.g = Graphics.FromImage(m_G_C.image);
            m_G_C.gb = Graphics.FromImage(m_G_C.canvas);
            m_G_C.g.Clear(Color.White);
            m_G_C.Buff = new PointF[0];

            int i_S_H = 0, i_S_L = 0;
            float _flEndKd = 0f;

            int _iGs = 10;
            #region 1.2 画横轴刻度
            if (i_Screen_No < 0) i_Screen_No = 0;
            i_S_H = Chart_Ruler_Y_Start - 6; i_S_L = Chart_Ruler_Y_Start - 3;
            i_Screen_Start_Distance = lst_Screenkd[i_Screen_No].i_Start;
            i_Screen_End_Distance = lst_Screenkd[i_Screen_No].i_End;
#pragma warning disable CS0219 // 变量“_blZt_mm”已被赋值，但从未使用过它的值
            bool _blZt_mm = false;
#pragma warning restore CS0219 // 变量“_blZt_mm”已被赋值，但从未使用过它的值
            for (int i = 1; i <= Scree_iAllCols_C; i++)
            {
                i_X = (int)(Chart_Ruler_X_Start + i * Scree_iDotWith_X);
                _flEndKd = i_Screen_Start_Distance + (i * Scree_iDotWithmm_X);//Chart_Run_flStart_Distance

                //2 画刻度线
                if (iRad_Dw == 0 && i % 5 != 0 || iRad_Dw == 1 && i % 4 != 0)
                    m_G_C.g.DrawLine(Ruler_p_One, new Point(i_X, i_S_L), new Point(i_X, Chart_Ruler_Y_Start));
                else
                    m_G_C.g.DrawLine(Ruler_p_One, new Point(i_X, i_S_H), new Point(i_X, Chart_Ruler_Y_Start));
                #region 标记横轴刻度值
                if (i % _iGs == 0)
                {
                    //if (i == _iGs && _blZt_mm == false)
                    //{ drawFont = new Font("Arial", (float)10); _blZt_mm = true; }
                    //else
                    //    drawFont = new Font("黑体", (float)7.9, FontStyle.Bold);
                    strT = _flEndKd.ToString("f0") + (i == _iGs ? "mm" : "");
                    PointF drawPoint = new PointF(i_X - 15, Chart_Ruler_Word_H);
                    m_G_C.g.DrawString(strT, drawFont, drawBrush, drawPoint);
                }
                #endregion 标记横轴刻度值
                if (i_X > Pic_C.ClientSize.Width) break;
            }
            m_G_C.g.DrawLine(Ruler_p_One, new Point(Chart_Ruler_X_Start, Chart_Ruler_Y_Start), new Point((int)(Chart_Ruler_X_Start +
                                                    Scree_iAllCols_C * Scree_iDotWith_X), Chart_Ruler_Y_Start));
            #endregion 1.2 画横轴刻度

            #region  1.3 画纵轴刻度
            StringFormat StrF = new StringFormat();
            StrF.FormatFlags = StringFormatFlags.DirectionVertical; // 竖排
            int iLeft_H = Chart_Ruler_X_Start - (Chart_Ruler_X_Start > 16 ? 16 : Chart_Ruler_X_Start) - 2;
            i_S_H = Chart_Ruler_X_Start - 6; i_S_L = Chart_Ruler_X_Start - 3;
            int _iX_Row = 0;
            for (int i = 1; i <= Scree_iAllRows_C; i++)
            {
                i_Y = (int)(Chart_Ruler_Y_Start + i * Scree_iDotHeight);

                if (i % 5 != 0)
                {
                    m_G_C.g.DrawLine(Ruler_p_One, new Point(i_S_L, i_Y), new Point(Chart_Ruler_X_Start, i_Y));
                    if (i == 1)
                        m_G_C.g.DrawString(Scree_iDotHeightmm_Y.ToString("f0") + " mm", drawFont, drawBrush, iLeft_H, i_Y - 18, StrF);
                }
                else
                {
                    m_G_C.g.DrawLine(Ruler_p_One, new Point(i_S_H, i_Y), new Point(Chart_Ruler_X_Start, i_Y));
                    strT = (i * Scree_iDotHeightmm_Y).ToString("f0");
                    if (i == Scree_iAllRows_C)
                        m_G_C.g.DrawString(strT, drawFont, drawBrush, iLeft_H, i_Y - (_iX_Row > 9 ? 15 : 6), StrF);
                    else
                        m_G_C.g.DrawString(strT, drawFont, drawBrush, iLeft_H, i_Y - (_iX_Row > 9 ? 12 : 3), StrF);
                }
                iDatNum++;
                if (i_Y > Pic_C.ClientSize.Height) break;
            }
            m_G_C.g.DrawLine(Ruler_p_One, new Point(Chart_Ruler_X_Start, Chart_Ruler_Y_Start), new Point(Chart_Ruler_X_Start, Pic_C.ClientSize.Height));

            #endregion 1.3 画纵轴刻度

            Rectangle _Rect_Kd = new Rectangle(0, 0, Pic_C.Width, Pic_C.Height);
            if (m_G_C.bg != null)
            {
                try
                {
                    m_G_C.gb.DrawImage(m_G_C.bg, _Rect_Kd);// 先绘制背景层
                }
#pragma warning disable CS0168 // 声明了变量“de”，但从未使用过
                catch (Exception de)
#pragma warning restore CS0168 // 声明了变量“de”，但从未使用过
                { }
            }
            m_G_C.gb.DrawImage(m_G_C.image, _Rect_Kd); // 再绘制绘画层
            try
            {
                Pic_C.BackgroundImage = (Bitmap)m_G_C.canvas.Clone(); // 设置为背景层，就是在内存中建立了一张对应的画布，重新refresh就是再次的加载这个画布
                Pic_C.Refresh();
            }
            catch { }
            #endregion 1 画布准备
            #endregion 刻度尺

       //     Application.DoEvents();
        }
        /// <summary>
        /// 涂层画刻度初始化界面
        /// </summary>
        public void Plant_Ruler_Coat_C()
        {
            if (Pic_Coat_C == null) return;
            //  if (blPlantRuler == 4) return;
            int i_X = 0, i_Y = 0;
            int iDatNum = 0;
            string strT = "";
            Chart_Clear(ref m_Coat_C);
            System.Windows.Forms.PictureBox.CheckForIllegalCrossThreadCalls = false;
            #region 画刻度尺

            #region 1 画布准备:初始化、右边出图、满屏时
            m_Coat_C.image = new Bitmap(Pic_Coat_C.ClientSize.Width, Pic_Coat_C.ClientSize.Height);
            // 获取背景层
            m_Coat_C.bg = (Bitmap)Pic_Coat_C.BackgroundImage;
            // 初始化整个画布
            m_Coat_C.canvas = new Bitmap(Pic_Coat_C.ClientSize.Width, Pic_Coat_C.ClientSize.Height);
            // 初始化图形面板，获取这块内存画布的Graphics的引用
            m_Coat_C.g = Graphics.FromImage(m_Coat_C.image);
            m_Coat_C.gb = Graphics.FromImage(m_Coat_C.canvas);
            m_Coat_C.g.Clear(Color.White);
            m_Coat_C.Buff = new PointF[0];

            int i_S_H = 0, i_S_L = 0;
            float _flEndKd = 0f;

            int _iGs = 10;
            #region 1.2 画横轴刻度
            if (i_Screen_No < 0) i_Screen_No = 0;
            i_S_H = Chart_Ruler_Y_Start - 6; i_S_L = Chart_Ruler_Y_Start - 3;
            i_Screen_Start_Distance = lst_Screenkd[i_Screen_No].i_Start;
            i_Screen_End_Distance = lst_Screenkd[i_Screen_No].i_End;
#pragma warning disable CS0219 // 变量“_blZt_mm”已被赋值，但从未使用过它的值
            bool _blZt_mm = false;
#pragma warning restore CS0219 // 变量“_blZt_mm”已被赋值，但从未使用过它的值
            for (int i = 1; i <= Scree_iAllCols_C; i++)
            {
                i_X = (int)(Chart_Ruler_X_Start + i * Scree_iDotWith_X);
                _flEndKd = i_Screen_Start_Distance + (i * Scree_iDotWithmm_X);//Chart_Run_flStart_Distance

                //2 画刻度线
                if (iRad_Dw == 0 && i % 5 != 0 || iRad_Dw == 1 && i % 4 != 0)
                    m_Coat_C.g.DrawLine(Ruler_p_One, new Point(i_X, i_S_L), new Point(i_X, Chart_Ruler_Y_Start));
                else
                    m_Coat_C.g.DrawLine(Ruler_p_One, new Point(i_X, i_S_H), new Point(i_X, Chart_Ruler_Y_Start));
                #region 标记横轴刻度值
                if (i % _iGs == 0)
                {
                    //if (i == _iGs && _blZt_mm == false)
                    //{ drawFont = new Font("Arial", (float)10); _blZt_mm = true; }
                    //else
                    //    drawFont = new Font("黑体", (float)7.9, FontStyle.Bold);
                    strT = _flEndKd.ToString("f0") + (i == _iGs ? "mm" : "");
                    PointF drawPoint = new PointF(i_X - 15, Chart_Ruler_Word_H);
                    m_Coat_C.g.DrawString(strT, drawFont, drawBrush, drawPoint);
                }
                #endregion 标记横轴刻度值
                if (i_X > Pic_Coat_C.ClientSize.Width) break;
            }
            m_Coat_C.g.DrawLine(Ruler_p_One, new Point(Chart_Ruler_X_Start, Chart_Ruler_Y_Start), new Point((int)(Chart_Ruler_X_Start +
                                                    Scree_iAllCols_C * Scree_iDotWith_X), Chart_Ruler_Y_Start));
            #endregion 1.2 画横轴刻度

            #region  1.3 画纵轴刻度
            StringFormat StrF = new StringFormat();
            StrF.FormatFlags = StringFormatFlags.DirectionVertical; // 竖排
            int iLeft_H = Chart_Ruler_X_Start - (Chart_Ruler_X_Start > 16 ? 16 : Chart_Ruler_X_Start) - 2;
            i_S_H = Chart_Ruler_X_Start - 6; i_S_L = Chart_Ruler_X_Start - 3;
            int _iX_Row = 0;


            for (int i = 1; i <= Scree_iAllRows_C_Coat; i++)
            {
                i_Y = (int)(Chart_Ruler_Y_Start + i * Scree_iDotHeight_Coat);


                if (i > 1 && i % (Scree_iDotHeightmm_Y_Coat <= 20 ? 10 : 2) == 0)//刻度线
                {
                    m_Coat_C.g.DrawLine(Ruler_p_One, new Point(i_S_H, i_Y), new Point(Chart_Ruler_X_Start, i_Y));
                    strT = (i * Scree_iDotHeightmm_Y_Coat).ToString();
                    if (i == Scree_iAllRows_C_Coat)
                        m_Coat_C.g.DrawString(strT, drawFont, drawBrush, iLeft_H, i_Y - (_iX_Row > 9 ? 15 : 6), StrF);
                    else
                        m_Coat_C.g.DrawString(strT, drawFont, drawBrush, iLeft_H, i_Y - (_iX_Row > 9 ? 12 : 3), StrF);
                }
                if (i == 1)//行号
                {
                    m_Coat_C.g.DrawLine(Ruler_p_One, new Point(i_S_L, i_Y), new Point(Chart_Ruler_X_Start, i_Y));
                    m_Coat_C.g.DrawString(Scree_iDotHeightmm_Y_Coat.ToString() + " m m", drawFont, drawBrush, iLeft_H, i_Y - 18, StrF);
                 }
                //if (Scree_iDotHeightmm_Y_Coat < 20 &&  i % 10 != 0)
                //{
                //    m_Coat_C.g.DrawLine(Ruler_p_One, new Point(i_S_L, i_Y), new Point(Chart_Ruler_X_Start, i_Y));
                //    if (i == 1)
                //        m_Coat_C.g.DrawString(Scree_iDotHeightmm_Y_Coat.ToString() + " m m", drawFont, drawBrush, iLeft_H, i_Y - 18, StrF);
                //}
                //else if(i%2==1)
                //{
                //    m_Coat_C.g.DrawLine(Ruler_p_One, new Point(i_S_H, i_Y), new Point(Chart_Ruler_X_Start, i_Y));
                //    strT = (i * Scree_iDotHeightmm_Y_Coat).ToString();
                //    if (i == Scree_iAllRows_C)
                //        m_Coat_C.g.DrawString(strT, drawFont, drawBrush, iLeft_H, i_Y - (_iX_Row > 9 ? 15 : 6), StrF);
                //    else
                //        m_Coat_C.g.DrawString(strT, drawFont, drawBrush, iLeft_H, i_Y - (_iX_Row > 9 ? 12 : 3), StrF);
                //}
                iDatNum++;
                if (i_Y > Pic_Coat_C.ClientSize.Height) break;
            }
            m_Coat_C.g.DrawLine(Ruler_p_One, new Point(Chart_Ruler_X_Start, Chart_Ruler_Y_Start), new Point(Chart_Ruler_X_Start, Pic_Coat_C.ClientSize.Height));

            #endregion 1.3 画纵轴刻度

            Rectangle _Rect_Kd = new Rectangle(0, 0, Pic_Coat_C.Width, Pic_Coat_C.Height);
            if (m_Coat_C.bg != null)
            {
                try
                {
                    m_Coat_C.gb.DrawImage(m_Coat_C.bg, _Rect_Kd);// 先绘制背景层
                }
#pragma warning disable CS0168 // 声明了变量“de”，但从未使用过
                catch (Exception de)
#pragma warning restore CS0168 // 声明了变量“de”，但从未使用过
                { }
            }
            m_Coat_C.gb.DrawImage(m_Coat_C.image, _Rect_Kd); // 再绘制绘画层
            try
            {
                Pic_Coat_C.BackgroundImage = (Bitmap)m_Coat_C.canvas.Clone(); // 设置为背景层，就是在内存中建立了一张对应的画布，重新refresh就是再次的加载这个画布
                Pic_Coat_C.Refresh();
            }
            catch { }
            #endregion 1 画布准备
            #endregion 刻度尺

       //     Application.DoEvents();
        }
        /// <summary>
        /// 计算屏幕行列个数
        /// </summary>
        /// <param name="PicArea"></param>
        public void GetRulerPara_Mul(int iMul_Num = 2)
        {
            if (Pic_C == null) return;
            #region 由一个点宽度高度，获得屏幕参数
            int iScreen_With = Pic_C.Width - Chart_Ruler_X_Start;
            int iScreen_Height = Pic_C.Height - Chart_Ruler_Y_Start;

            Scree_iAllCols_C = iScreen_With / Scree_iDotWith_X;

            Scree_Stant_Distance = Scree_iAllCols_C * Scree_iDotWithmm_X;
            Init_KD();

            #region Y轴数据行数

            m_iMul_OneB_Heigh = (Pic_C.Height - Chart_Ruler_Y_Start - 1) / iMul_Num;//分成两半，中间加一个图像间隔4个像素的宽度
            float _flDataMax = flNormal_Thickness + Scree_iDotHeight_B_Y_Addmm;//显示距离
            m_iMul_B_OneDataHeitht = m_iMul_OneB_Heigh / _flDataMax;
            #endregion 行数
            #endregion 一个点宽度高度

            #region 画图

            Plant_Ruler_Mul(_flDataMax, iMul_Num);

            Mul_Titl_Init(iMul_Num);
            #endregion 画图
        }
        /// <summary>
        /// 画多通道B扫描横纵轴刻度
        /// </summary>
        /// <param name="iRow_OneB"></param>
        private void Plant_Ruler_Mul(float _flDataMax, int iMul_Num = 2)
        {
            if (Pic_C == null) return;
            int i_X = 0, i_Y = 0;
            string strT = "";
            Chart_Clear(ref m_G_C);
            System.Windows.Forms.PictureBox.CheckForIllegalCrossThreadCalls = false;
            #region 画刻度尺

            #region 1 画布准备:初始化、右边出图、满屏时
            m_G_C.image = new Bitmap(Pic_C.ClientSize.Width, Pic_C.ClientSize.Height);
            // 获取背景层
            m_G_C.bg = (Bitmap)Pic_C.BackgroundImage;
            // 初始化整个画布
            m_G_C.canvas = new Bitmap(Pic_C.ClientSize.Width, Pic_C.ClientSize.Height);
            // 初始化图形面板，获取这块内存画布的Graphics的引用
            m_G_C.g = Graphics.FromImage(m_G_C.image);
            m_G_C.gb = Graphics.FromImage(m_G_C.canvas);
            m_G_C.g.Clear(Color.White);
            m_G_C.Buff = new PointF[0];

            int i_S_H = 0, i_S_L = 0;
            float _flEndKd = 0f;

            int _iGs = 10;
            #region 1.2 画横轴刻度
            i_S_H = Chart_Ruler_Y_Start - 6; i_S_L = Chart_Ruler_Y_Start - 3;
            i_Screen_Start_Distance = lst_Screenkd[i_Screen_No].i_Start;
            i_Screen_End_Distance = lst_Screenkd[i_Screen_No].i_End;
#pragma warning disable CS0219 // 变量“_blZt_mm”已被赋值，但从未使用过它的值
            bool _blZt_mm = false;
#pragma warning restore CS0219 // 变量“_blZt_mm”已被赋值，但从未使用过它的值
            for (int i = 1; i <= Scree_iAllCols_C; i++)
            {
                i_X = (int)(Chart_Ruler_X_Start + i * Scree_iDotWith_X);
                _flEndKd = i_Screen_Start_Distance + (i * Scree_iDotWithmm_X);//Chart_Run_flStart_Distance

                //2 画刻度线
                if (iRad_Dw == 0 && i % 5 != 0 || iRad_Dw == 1 && i % 4 != 0)
                    m_G_C.g.DrawLine(Ruler_p_One, new Point(i_X, i_S_L), new Point(i_X, Chart_Ruler_Y_Start));
                else
                    m_G_C.g.DrawLine(Ruler_p_One, new Point(i_X, i_S_H), new Point(i_X, Chart_Ruler_Y_Start));
                #region 标记横轴刻度值
                if (i % _iGs == 0)
                {
                    //if (i == _iGs && _blZt_mm == false)
                    //{ drawFont = new Font("Arial", (float)10); _blZt_mm = true; }
                    //else
                    //    drawFont = new Font("黑体", (float)7.9, FontStyle.Bold);
                    strT = _flEndKd.ToString("f0") + (i == _iGs ? "mm" : "");
                    PointF drawPoint = new PointF(i_X - 15, Chart_Ruler_Word_H);
                    m_G_C.g.DrawString(strT, drawFont, drawBrush, drawPoint);
                }
                #endregion 标记横轴刻度值
                if (i_X > Pic_C.ClientSize.Width) break;
            }
            m_G_C.g.DrawLine(Ruler_p_One, new Point(Chart_Ruler_X_Start, Chart_Ruler_Y_Start), new Point((int)(Chart_Ruler_X_Start +
                                                    Scree_iAllCols_C * Scree_iDotWith_X), Chart_Ruler_Y_Start));
            #endregion 1.2 画横轴刻度

            #region  1.3 画纵轴刻度
            StringFormat StrF = new StringFormat();
            StrF.FormatFlags = StringFormatFlags.DirectionVertical; // 竖排
            int iLeft_H = Chart_Ruler_X_Start - (Chart_Ruler_X_Start > 16 ? 16 : Chart_Ruler_X_Start) - 2;
            i_S_H = Chart_Ruler_X_Start - 6; i_S_L = Chart_Ruler_X_Start - 3;

            int iRow = 1;//从实际的第一个间隔开始画

            for (int i = 1; i <= iMul_Num; i++)//通道数
            {
                for (int _iJg = 1; _iJg <= _flDataMax; _iJg++)//每行B扫描画刻度
                {
                    i_Y = (int)(Chart_Ruler_Y_Start + 1 + iRow++ * m_iMul_B_OneDataHeitht);//刻度位置

                    if (_iJg % 5 != 0)
                    {
                        m_G_C.g.DrawLine(Ruler_p_One, new Point(i_S_L, i_Y), new Point(Chart_Ruler_X_Start, i_Y));
                        if (i == 1 && _iJg == 1)
                            m_G_C.g.DrawString(" m m", drawFont, drawBrush, iLeft_H, i_Y - 8, StrF);
                    }
                    else
                    {
                        m_G_C.g.DrawLine(Ruler_p_One, new Point(i_S_H, i_Y), new Point(Chart_Ruler_X_Start, i_Y));

                        m_G_C.g.DrawString(_iJg.ToString(), drawFont, drawBrush, iLeft_H, i_Y - 8, StrF);
                    }
                    if (i_Y > Pic_C.ClientSize.Height) break;
                }
            }
            m_G_C.g.DrawLine(Ruler_p_One, new Point(Chart_Ruler_X_Start, Chart_Ruler_Y_Start), new Point(Chart_Ruler_X_Start, Pic_C.ClientSize.Height));

            #endregion 1.3 画纵轴刻度

            Rectangle _Rect_Kd = new Rectangle(0, 0, Pic_C.Width, Pic_C.Height);
            if (m_G_C.bg != null)
            {
                try
                {
                    m_G_C.gb.DrawImage(m_G_C.bg, _Rect_Kd);// 先绘制背景层
                }
#pragma warning disable CS0168 // 声明了变量“de”，但从未使用过
                catch (Exception de)
#pragma warning restore CS0168 // 声明了变量“de”，但从未使用过
                { }
            }
            m_G_C.gb.DrawImage(m_G_C.image, _Rect_Kd); // 再绘制绘画层
            try
            {
                Pic_C.BackgroundImage = (Bitmap)m_G_C.canvas.Clone(); // 设置为背景层，就是在内存中建立了一张对应的画布，重新refresh就是再次的加载这个画布
                Pic_C.Refresh();
            }
            catch { }
            #endregion 1 画布准备
            #endregion 刻度尺

            //       Application.DoEvents();
        }

        /// <summary>
        /// 多通道B扫描：实时数据
        /// </summary>
        /// <param name="i_X"></param>
        /// <param name="Data"></param>
        //public void Plant_B_Mul(int i_X, Cls_EMAT_1[] ArrData, int iType = 1)
        //{
        //    i_X = (int)(Chart_Ruler_X_Start + i_X * Scree_iDotWith_X) + 1;//由序号计算屏幕对应的像素位置
        //    int i_Y = 0;
        //    for (int i = 0; i < ArrData.Length; i++)
        //    {
        //        i_Y = (int)(Chart_Ruler_Y_Start + (i == 0 ? 1 : 0) + i * m_iMul_OneB_Heigh);
        //        Color color = Color.FromArgb(255, ArrData[i].Ori_R, ArrData[i].Ori_G, ArrData[i].Ori_B);

        //        if (i_X <= Pic_C.Width && i_Y <= Pic_C.Height)
        //        {
        //            InitColor(m_G_C.g, i_X, i_Y, Scree_iDotWith_X, ArrData[i].Ori_flThick * m_iMul_B_OneDataHeitht, color);
        //        }
        //    }
        //    if (iType == 1)
        //    {
        //        Rectangle _Rect = new Rectangle(0, 0, Pic_C.Width, Pic_C.Height);

        //        if (m_G_C.bg != null)
        //            m_G_C.gb.DrawImage(m_G_C.bg, _Rect);// 先绘制背景层

        //        m_G_C.gb.DrawImage(m_G_C.image, _Rect); // 再绘制绘画层
        //        Pic_C.BackgroundImage = (Bitmap)m_G_C.canvas.Clone();
        //    }
        //}
        /// <summary>
        /// 多通道图像、提示信息刷新
        /// </summary>
        /// <param name="i_X"></param>
        /// <param name="ArrData"></param>
        /// <param name="iType"></param>
        public void Plant_B_Mul(int i_X, Cls_EMAT_2[] ArrData, int iType = 1, string strGjbh = "",int iDataType=0)
        {
            int _iX_No = i_X;
            i_X = (int)(Chart_Ruler_X_Start + i_X * Scree_iDotWith_X) + 1;//由序号计算屏幕对应的像素位置
            int i_Y = 0;
            for (int i = 0; i < ArrData.Length; i++)
            {
                i_Y = (int)(Chart_Ruler_Y_Start + (i == 0 ? 1 : 0) + i * m_iMul_OneB_Heigh);
                Color color = Color.FromArgb(255, ArrData[i].R, ArrData[i].G, ArrData[i].B);

                if (i_X <= Pic_C.Width && i_Y <= Pic_C.Height)
                {
                    Mul_Titl_Add(i, _iX_No, ArrData[i], strGjbh, (i + 1),iDataType );
                    InitColor(m_G_C.g, i_X, i_Y, Scree_iDotWith_X, ArrData[i].flThick * m_iMul_B_OneDataHeitht, color);
                }
            }
            if (iType == 1)
            {
                Rectangle _Rect = new Rectangle(0, 0, Pic_C.Width, Pic_C.Height);

                if (m_G_C.bg != null)
                    m_G_C.gb.DrawImage(m_G_C.bg, _Rect);// 先绘制背景层

                m_G_C.gb.DrawImage(m_G_C.image, _Rect); // 再绘制绘画层
                Pic_C.BackgroundImage = (Bitmap)m_G_C.canvas.Clone();
            }
        }
        public void Plant_Mul(int i_X, Cls_EMAT_2 Data, int iType = 1)
        {
            if (Data.flThick < 0) return;

            i_X = (int)(Chart_Ruler_X_Start + i_X * m_Scree_iDotWith_B) + 1;//由序号计算屏幕对应的像素位置
            int i_Y = (int)(Chart_Ruler_Y_Start + Scree_iDotHeight);
            Color color = Color.FromArgb(255, Data.R, Data.G, Data.B);

            if (i_X <= Pic_B.Width && i_Y <= Pic_B.Height)
            {
                InitColor(m_G_B.g, i_X, i_Y, m_Scree_iDotWith_B, Data.flThick * m_Scree_fl_DotHeight_B, color);

                if (iType == 1)
                {
                    Rectangle _Rect = new Rectangle(0, 0, Pic_B.Width, Pic_B.Height);

                    if (m_G_B.bg != null)
                        m_G_B.gb.DrawImage(m_G_B.bg, _Rect);// 先绘制背景层

                    m_G_B.gb.DrawImage(m_G_B.image, _Rect); // 再绘制绘画层
                    Pic_B.BackgroundImage = (Bitmap)m_G_B.canvas.Clone();
                }
            }
        }
        public void Plant_C(int i_X, int i_Y, Cls_EMAT_2 Data, int iType = 1)
        {
            Mul_Titl_Add_A(i_Y, i_X, Data, "", (i_Y + 1));

            i_X = (int)(Chart_Ruler_X_Start + i_X * Scree_iDotWith_X + 1);//由序号计算屏幕对应的像素位置
            i_Y = (int)(Chart_Ruler_Y_Start + (i_Y * Scree_iDotHeight) + 1);
            Color color = Color.FromArgb(255, Data.R, Data.G, Data.B);

            if (i_X <= Pic_C.Width && i_Y <= Pic_C.Height)
            {

                InitColor(m_G_C.g, i_X, i_Y, Scree_iDotWith_X, Scree_iDotHeight, color);

                if (iType == 1)
                {
                    Rectangle _Rect = new Rectangle(0, 0, Pic_C.Width, Pic_C.Height);

                    if (m_G_C.bg != null)
                        m_G_C.gb.DrawImage(m_G_C.bg, _Rect);// 先绘制背景层

                    m_G_C.gb.DrawImage(m_G_C.image, _Rect); // 再绘制绘画层
                    Pic_C.BackgroundImage = (Bitmap)m_G_C.canvas.Clone();
                }
            }
        }

        public void Plant_Coat_C(int i_X, int i_Y, Cls_EMAT_2 Data, int iType = 1)
        {
            //     if (Data.blUse == false) return;
            Mul_Titl_Add_Coat_A(i_Y, i_X, Data, "", Data.iArr_ShowNo);// i_Y  * Scree_iDotHeightmm_Y_Coat);//(i_Y + 1) * Scree_iDotHeightmm_Y_Coat);

            i_X = (int)(Chart_Ruler_X_Start + i_X * Scree_iDotWith_X + 1);//由序号计算屏幕对应的像素位置
            i_Y = (int)(Chart_Ruler_Y_Start + (i_Y * Scree_iDotHeight_Coat) + 1);
            Color color = Color.FromArgb(255, Data.R, Data.G, Data.B);

            if (Data.flThick >=0 && i_X <= Pic_Coat_C.Width && i_Y <= Pic_Coat_C.Height)
            {
                InitColor(m_Coat_C.g, i_X, i_Y, Scree_iDotWith_X, Scree_iDotHeight_Coat-2, color);

                if (iType == 1)
                {
                    Rectangle _Rect = new Rectangle(0, 0, Pic_Coat_C.Width, Pic_Coat_C.Height);

                    if (m_Coat_C.bg != null)
                        m_Coat_C.gb.DrawImage(m_Coat_C.bg, _Rect);// 先绘制背景层

                    m_Coat_C.gb.DrawImage(m_Coat_C.image, _Rect); // 再绘制绘画层
                    Pic_Coat_C.BackgroundImage = (Bitmap)m_Coat_C.canvas.Clone();
                    Application.DoEvents();
                }
            }
        }
        /// <summary>
        /// 翻页：页面刷新
        /// </summary>
        /// <param name="iCurrRow">当前行号</param>
        /// <param name="i_Dist_S">开始距离</param>
        /// <param name="i_Dist_E">结束距离</param>
        /// <param name="Lst_Data">当前行数据</param>
        public void Plant_Screen_Coat_C(int iCurrRow, int i_Dist_S, int i_Dist_E, List<CLs_EMAT_Data> Lst_Data)
        {
            int _iDist = 0, _iCurr_Y;
            float _flDat = 0, _fl_Y_Dat;


            for (int i = 0; i < Lst_Data.Count; i++)
            {
                _iDist = Lst_Data[i].i_X_mm;
                if (_iDist >= i_Dist_S && _iDist <= i_Dist_E)
                {
                    _flDat =1.0f *(_iDist - i_Screen_Start_Distance) / Scree_iDotWithmm_X;
                    _iDist = (int)_flDat;//
                    if (_flDat - _iDist >= 0.5) _iDist++;

                    for (int _iR = 0; _iR < Lst_Data[i].Arr_C_Data.Length; _iR++)
                    {
                        try
                        {
                            Cls_EMAT_2 Data = Lst_Data[i].Arr_C_Data[_iR];
                            if (Data != null)
                            {
                                Data.m_i_X = Lst_Data[i].i_X_mm;
                               if (Data.flThick >= 0)
                                {

                          //          _fl_Y_Dat =1.0f * Lst_Data[i].Arr_C_Data[_iR].iArr_ShowNo / Scree_iDotHeightmm_Y_Coat;

                           //         _iCurr_Y = (int)(Chart_Ruler_Y_Start + _iR * Scree_iDotHeight_Coat);
                                    //_iCurr_Y = (int)_fl_Y_Dat;
                                    //if (_fl_Y_Dat - _iCurr_Y > 0.499) _iCurr_Y++;
                                    Plant_Coat_C(_iDist, _iR, Data, 0);

                                }
                            }
                        }
                        catch { }
                    }
                }
            }
            Rectangle _Rect = new Rectangle(0, 0, Pic_Coat_C.Width, Pic_Coat_C.Height);

            if (m_Coat_C.bg != null)
                m_Coat_C.gb.DrawImage(m_Coat_C.bg, _Rect);// 先绘制背景层

            m_Coat_C.gb.DrawImage(m_Coat_C.image, _Rect); // 再绘制绘画层
            Pic_Coat_C.BackgroundImage = (Bitmap)m_Coat_C.canvas.Clone();
        }
        /// <summary>
        /// 翻页：页面刷新
        /// </summary>
        /// <param name="iCurrRow">当前行号</param>
        /// <param name="i_Dist_S">开始距离</param>
        /// <param name="i_Dist_E">结束距离</param>
        /// <param name="Lst_Data">当前行数据</param>
        public void Plant_Screen_C(int iCurrRow, int i_Dist_S, int i_Dist_E, List<CLs_EMAT_Data> Lst_Data)
        {
            int _iDist = 0;
            float _flDat = 0;
            for (int i = 0; i < Lst_Data.Count; i++)
            {
                _iDist = Lst_Data[i].i_X_mm;
                if (_iDist >= i_Dist_S && _iDist <= i_Dist_E)
                {
                    _flDat = (_iDist - i_Screen_Start_Distance) / Scree_iDotWithmm_X;
                    _iDist = (int)_flDat;//
                    if (_flDat - _iDist >= 0.5) _iDist++;

                    for (int _iR = 0; _iR < Lst_Data[i].Arr_C_Data.Length; _iR++)
                    {
                        Cls_EMAT_2 Data = Lst_Data[i].Arr_C_Data[_iR];
                        Data.m_i_X = Lst_Data[i].i_X_mm;
                        if (Data.flThick >= 0)
                            Plant_C(_iDist, _iR, Data, 0);
                    }
                }
            }
            Rectangle _Rect = new Rectangle(0, 0, Pic_C.Width, Pic_C.Height);
            Application.DoEvents();
            if (m_G_C.bg != null)
                m_G_C.gb.DrawImage(m_G_C.bg, _Rect);// 先绘制背景层

            m_G_C.gb.DrawImage(m_G_C.image, _Rect); // 再绘制绘画层
            Pic_C.BackgroundImage = (Bitmap)m_G_C.canvas.Clone();
        }
        /// <summary>
        /// 刷新指定距离段的指定检测行的多通道数据
        /// </summary>
        /// <param name="i_Dist_S"></param>
        /// <param name="i_Dist_E"></param>
        /// <param name="lst_Show_Buff"></param>
        public void Plant_Screen_Mul(int i_Dist_S, int i_Dist_E, List<Cls_Mul_ShowBuff> lst_Show_Buff, int iStartNo, int i_DianDao)
        {
            //1 准备当前检测的开始结束缓存列号
            int _i_S_Col = Buff_By_Distan_mm_To_Col(i_Dist_S, i_DianDao > 0 ? 1 : 0) - iStartNo;
            int _i_E_Col = Buff_By_Distan_mm_To_Col(i_Dist_E, i_DianDao > 0 ? 1 : 0) - iStartNo;
            int _iNum = lst_Show_Buff != null ? lst_Show_Buff[0].Arr_C_Data.Length : 1;
            int i_X = 0, i_Y = 0, _i_X_No = Buff_By_Distan_mm_To_Col(i_Dist_S, 1);
            if (i_DianDao > 0)
            {
                _i_S_Col = i_DianDao - _i_S_Col;
                _i_E_Col = i_DianDao - _i_E_Col;
            }
            int _i_Lst_Count = lst_Show_Buff.Count;
            if (_i_E_Col > _i_Lst_Count) _i_E_Col = _i_Lst_Count;
            //2 数据刷新
            for (int _iCol = _i_S_Col; _iCol < _i_E_Col; _iCol++)
            {
                i_X = (int)(Chart_Ruler_X_Start + _i_X_No++ * Scree_iDotWith_X + 1);//由序号计算屏幕对应的像素位置
                for (int _iR = 0; _iR < _iNum; _iR++)
                {
                    i_Y = (int)(Chart_Ruler_Y_Start + (_iR == 0 ? 1 : 0) + _iR * m_iMul_OneB_Heigh);

                    Cls_EMAT_2 Data = lst_Show_Buff[_iCol].Arr_C_Data[_iR];

                    Color color = Color.FromArgb(255, Data.R, Data.G, Data.B);

                    if (i_X <= Pic_C.Width && i_Y <= Pic_C.Height)
                        InitColor(m_G_C.g, i_X, i_Y, Scree_iDotWith_X, Data.flThick * m_iMul_B_OneDataHeitht, color);
                }
            }
            //3 图像刷新
            Rectangle _Rect = new Rectangle(0, 0, Pic_C.Width, Pic_C.Height);

            if (m_G_C.bg != null)
                m_G_C.gb.DrawImage(m_G_C.bg, _Rect);// 先绘制背景层

            m_G_C.gb.DrawImage(m_G_C.image, _Rect); // 再绘制绘画层
            Pic_C.BackgroundImage = (Bitmap)m_G_C.canvas.Clone();
        }
        public void Mul_Titl_Init(int _iNum)
        {
            #region 准备缓存：提示信息
            m_Mul_Titl = new Cls_Mul_Tile();

            for (int i = 0; i < _iNum; i++)
            {
                Cls_Mul_One _One = new Cls_Mul_One();
                for (int _iCol = 0; _iCol < Scree_iAllCols_C + 5; _iCol++)
                {
                    Cls_One_Data _Data = new Cls_One_Data();
                    _One.lst_One_Row_Data.Add(_Data);

                }
                m_Mul_Titl.lst_Mul_Titl.Add(_One);
            }
            #endregion 提示信息
        }

        public void Mul_Titl_Init_Coat(int _iNum)
        {
            #region 准备缓存：提示信息
            m_Mul_Titl_Coat = new Cls_Mul_Tile();

            for (int i = 0; i < _iNum; i++)
            {
                Cls_Mul_One _One = new Cls_Mul_One();
                for (int _iCol = 0; _iCol < Scree_iAllCols_C + 5; _iCol++)
                {
                    Cls_One_Data _Data = new Cls_One_Data();
                    _One.lst_One_Row_Data.Add(_Data);

                }
                m_Mul_Titl_Coat.lst_Mul_Titl.Add(_One);
            }
            #endregion 提示信息
        }
        /// <summary>
        /// C扫描A界面提示信息缓存初始化
        /// </summary>
        /// <param name="_iNum"></param>
        public void Mul_Titl_Init_A(int _iNum)
        {
            #region 准备缓存：提示信息
            m_Mul_Titl_A = new Cls_Mul_Tile();
       //     m_Mul_Titl_Coat_A = new Cls_Mul_Tile();

            for (int i = 0; i < _iNum; i++)
            {
                Cls_Mul_One _One = new Cls_Mul_One();
                for (int _iCol = 0; _iCol < Scree_iAllCols_C + 5; _iCol++)
                {
                    Cls_One_Data _Data = new Cls_One_Data();
                    _One.lst_One_Row_Data.Add(_Data);
                }
                m_Mul_Titl_A.lst_Mul_Titl.Add(_One);
         //       m_Mul_Titl_Coat_A.lst_Mul_Titl.Add(_One);
            }
            #endregion 提示信息
        }

        public void Mul_Titl_Init_A_Coat(int _iNum)
        {
            #region 准备缓存：提示信息
            m_Mul_Titl_Coat_A = new Cls_Mul_Tile();

            for (int i = 0; i < _iNum; i++)
            {
                Cls_Mul_One _One = new Cls_Mul_One();
                for (int _iCol = 0; _iCol < Scree_iAllCols_C + 5; _iCol++)
                {
                    Cls_One_Data _Data = new Cls_One_Data();
                    _One.lst_One_Row_Data.Add(_Data);
                }
                m_Mul_Titl_Coat_A.lst_Mul_Titl.Add(_One);
            }
            #endregion 提示信息
        }
        /// <summary>
        /// 添加提示信息
        /// </summary>
        /// <param name="_iR">行号</param>
        /// <param name="_i_X_No">列号</param>
        /// <param name="Data">点对应数据</param>
        public void Mul_Titl_Add(int _iR, int _i_X_No, Cls_EMAT_2 Data, string strGdbh, int i_ABC,int iDataType=0)
        {
            try
            {
                int _iArrLen = 1000;
                m_Mul_Titl.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].i_ABC = i_ABC;
                m_Mul_Titl.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].strGdbh = strGdbh;
                m_Mul_Titl.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].i_X = Data.m_i_X;
                m_Mul_Titl.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].flWc = Data.flWc;
                m_Mul_Titl.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].flThick = Data.flThick;
                m_Mul_Titl.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].iGain = Data.iGain;
                m_Mul_Titl.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].iGain_Limit = Data.iGain_Limit;

                if (iDataType == 2)
                {
                    if (Data.flArrWave  == null)
                        _iArrLen = 400;
                    else
                        _iArrLen = Data.flArrWave.Length;
                    m_Mul_Titl.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].flArrWave  = new float [_iArrLen];
                    if (Data.flArrWave != null)
                    {
                        var IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(m_Mul_Titl.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].flArrWave, 0);//   
                        Marshal.Copy(Data.flArrWave , 0, IntPtArr, _iArrLen);
                    }
                }
                else
                {
                    if (Data.btArrWave == null)
                        _iArrLen = 1000;
                    else
                        _iArrLen = Data.btArrWave.Length;
                    m_Mul_Titl.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].btArrWave = new byte[_iArrLen];
                    if (Data.btArrWave != null)
                    {
                        var IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(m_Mul_Titl.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].btArrWave, 0);//   
                        Marshal.Copy(Data.btArrWave, 0, IntPtArr, _iArrLen);
                    }
                }
            }
#pragma warning disable CS0168 // 声明了变量“E”，但从未使用过
            catch (Exception E)
#pragma warning restore CS0168 // 声明了变量“E”，但从未使用过
            { }
        }
        public void Mul_Titl_Add_Coat(int _iR, int _i_X_No, Cls_EMAT_2 Data, string strGdbh, int i_ABC)
        {
            try
            {
                int _iArrLen = 1000;
                m_Mul_Titl_Coat.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].i_ABC = i_ABC;
                m_Mul_Titl_Coat.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].strGdbh = strGdbh;
                m_Mul_Titl_Coat.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].i_X = Data.m_i_X;
                m_Mul_Titl_Coat.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].flWc = Data.flWc;
                m_Mul_Titl_Coat.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].str_Mul_Thick = Data.str_Mul_Thick ;

                m_Mul_Titl_Coat.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].flThick = Data.flThick;
                m_Mul_Titl_Coat.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].iGain = Data.iGain;
                m_Mul_Titl_Coat.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].iGain_Limit = Data.iGain_Limit;

                if (Data.btArrWave == null)
                    _iArrLen = 1000;
                else
                    _iArrLen = Data.btArrWave.Length;
                m_Mul_Titl_Coat.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].btArrWave = new byte[_iArrLen];
                if (Data.btArrWave != null)
                {
                    var IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(m_Mul_Titl_Coat.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].btArrWave, 0);//   
                    Marshal.Copy(Data.btArrWave, 0, IntPtArr, _iArrLen);
                }
            }
#pragma warning disable CS0168 // 声明了变量“E”，但从未使用过
            catch (Exception E)
#pragma warning restore CS0168 // 声明了变量“E”，但从未使用过
            { }
        }

        /// <summary>
        /// 添加提示信息
        /// </summary>
        /// <param name="_iR">行号</param>
        /// <param name="_i_X_No">列号</param>
        /// <param name="Data">点对应数据</param>
        public void Mul_Titl_Add_A(int _iR, int _i_X_No, Cls_EMAT_2 Data, string strGdbh, int i_ABC)
        {
            try
            {
                int _iArrLen = 1000;
                m_Mul_Titl_A.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].i_ABC = i_ABC;
                m_Mul_Titl_A.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].strGdbh = strGdbh;
                m_Mul_Titl_A.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].i_X = Data.m_i_X;
                m_Mul_Titl_A.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].flWc = Data.flWc;
                m_Mul_Titl_A.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].flThick = Data.flThick;
                m_Mul_Titl_A.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].iGain = Data.iGain;
                m_Mul_Titl_A.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].iGain_Limit = Data.iGain_Limit;

                if (Data.btArrWave == null)
                    _iArrLen = 1000;
                else
                    _iArrLen = Data.btArrWave.Length;
                m_Mul_Titl_A.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].btArrWave = new byte[_iArrLen];
                if (Data.btArrWave != null)
                {
                    var IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(m_Mul_Titl_A.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].btArrWave, 0);//   
                    Marshal.Copy(Data.btArrWave, 0, IntPtArr, _iArrLen);
                }
            }
#pragma warning disable CS0168 // 声明了变量“E”，但从未使用过
            catch (Exception E)
#pragma warning restore CS0168 // 声明了变量“E”，但从未使用过
            { }
        }

        /// <summary>
        /// 添加提示信息
        /// </summary>
        /// <param name="_iR">行号</param>
        /// <param name="_i_X_No">列号</param>
        /// <param name="Data">点对应数据</param>
        public void Mul_Titl_Add_Coat_A(int _iR, int _i_X_No, Cls_EMAT_2 Data, string strGdbh, int i_ABC)
        {
            try
            {
                int _iArrLen = 1000;
                m_Mul_Titl_Coat_A.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].i_ABC = i_ABC;
                m_Mul_Titl_Coat_A.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].strGdbh = strGdbh;
                m_Mul_Titl_Coat_A.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].i_X = Data.m_i_X;
                m_Mul_Titl_Coat_A.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].flWc = Data.flWc;
                m_Mul_Titl_Coat_A.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].flThick = Data.flThick;
                m_Mul_Titl_Coat_A.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].iGain = Data.iGain;
                m_Mul_Titl_Coat_A.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].iGain_Limit = Data.iGain_Limit;

                if (Data.btArrWave == null)
                    _iArrLen = 1000;
                else
                    _iArrLen = Data.btArrWave.Length;
                m_Mul_Titl_Coat_A.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].btArrWave = new byte[_iArrLen];
                if (Data.btArrWave != null)
                {
                    var IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(m_Mul_Titl_Coat_A.lst_Mul_Titl[_iR].lst_One_Row_Data[_i_X_No].btArrWave, 0);//   
                    Marshal.Copy(Data.btArrWave, 0, IntPtArr, _iArrLen);
                }
            }
#pragma warning disable CS0168 // 声明了变量“E”，但从未使用过
            catch (Exception E)
#pragma warning restore CS0168 // 声明了变量“E”，但从未使用过
            { }
        }
        /// <summary>
        /// 多通道信息刷新
        /// </summary>
        public Cls_Mul_Tile m_Mul_Titl = new Cls_Mul_Tile();
        /// <summary>
        /// 整体C扫描信息刷新
        /// </summary>
        public Cls_Mul_Tile m_Mul_Titl_Coat = new Cls_Mul_Tile();
        /// <summary>
        /// 扫描过程多通道信息刷新
        /// </summary>
        public Cls_Mul_Tile m_Mul_Titl_A = new Cls_Mul_Tile();

        public Cls_Mul_Tile m_Mul_Titl_Coat_A = new Cls_Mul_Tile();
        /// <summary>
        /// 刷新多通道图像和提示信息
        /// </summary>
        /// <param name="i_Dist_S"></param>
        /// <param name="i_Dist_E"></param>
        /// <param name="lst_Show_Buff"></param>
        public void Plant_Screen_Mul_Dd(int i_Dist_S, int i_Dist_E, List<Cls_Mul_ShowBuff> lst_Show_Buff, string strGdbh)
        {
            int _iDist = 0, i_X = 0, i_Y = 0, _i_X_No = 0;
#pragma warning disable CS0219 // 变量“_flDat”已被赋值，但从未使用过它的值
            float _flDat = 0;
#pragma warning restore CS0219 // 变量“_flDat”已被赋值，但从未使用过它的值

            int _iNum = lst_Show_Buff != null ? lst_Show_Buff[0].Arr_C_Data.Length : 1;
#pragma warning disable CS0219 // 变量“_iArrLen”已被赋值，但从未使用过它的值
            int _iArrLen = 0;
#pragma warning restore CS0219 // 变量“_iArrLen”已被赋值，但从未使用过它的值

            Mul_Titl_Init(_iNum);

            //2 数据刷新
            bool _blCheng = false ;

            if(lst_Show_Buff.Count>5)
            {
                _blCheng= lst_Show_Buff[0].m_i_X > lst_Show_Buff[3].m_i_X;
            }
            for (int i = 0; i < lst_Show_Buff.Count; i++)
            {
                _iDist = lst_Show_Buff[i].m_i_X;
                if (_iDist >= i_Dist_S && _iDist <= i_Dist_E)
                {
                    _i_X_No = Buff_By_Distan_mm_To_Col(_iDist, 1, _blCheng);
                    i_X = (int)(Chart_Ruler_X_Start + _i_X_No * Scree_iDotWith_X + 1);//由序号计算屏幕对应的像素位置
                    for (int _iR = 0; _iR < _iNum; _iR++)
                    {
                        i_Y = (int)(Chart_Ruler_Y_Start + (_iR == 0 ? 1 : 0) + _iR * m_iMul_OneB_Heigh);

                        Cls_EMAT_2 Data = lst_Show_Buff[i].Arr_C_Data[_iR];

                        Color color = Color.FromArgb(255, Data.R, Data.G, Data.B);

                        Mul_Titl_Add(_iR, _i_X_No, Data, strGdbh, (_iR + 1));
                        if (i_X <= Pic_C.Width && i_Y <= Pic_C.Height)
                            InitColor(m_G_C.g, i_X, i_Y, Scree_iDotWith_X, Data.flThick * m_iMul_B_OneDataHeitht, color);
                    }
                }
            }
            //3 图像刷新
            Rectangle _Rect = new Rectangle(0, 0, Pic_C.Width, Pic_C.Height);

            if (m_G_C.bg != null)
                m_G_C.gb.DrawImage(m_G_C.bg, _Rect);// 先绘制背景层

            m_G_C.gb.DrawImage(m_G_C.image, _Rect); // 再绘制绘画层
            Pic_C.BackgroundImage = (Bitmap)m_G_C.canvas.Clone();
        }
        /// <summary>
        /// 距离计算缓存开始位置
        /// </summary>
        /// <param name="i_Trip_Com_mm"></param>
        /// <param name="iType">1:计算当前屏幕开始列号  0：链表开始列号</param>
        /// <returns></returns>
        public int Buff_By_Distan_mm_To_Col(int i_Trip_Com_mm, int iType = 0)
        {
            int _i_X_Buff_No = 0;
            float _flDat = (i_Trip_Com_mm - (iType == 1 ? i_Screen_Start_Distance : 0)) / (Scree_iDotWithmm_X);
            _i_X_Buff_No = (int)_flDat;//
            if (_flDat - _i_X_Buff_No > 0.5) _i_X_Buff_No++;



            return _i_X_Buff_No;
        }
        public int Buff_By_Distan_mm_To_Col(int i_Trip_Com_mm, int iType = 0, bool blCheng = false)
        {
            int _i_X_Buff_No = 0;
            float _flDat = 0;

            if (blCheng == false)
                _flDat = (i_Trip_Com_mm - (iType == 1 ? i_Screen_Start_Distance : 0)) / Scree_iDotWithmm_X;
            else
                _flDat = (i_Trip_Com_mm - (iType == 1 ? i_Screen_Start_Distance : 0)) / (1.0f * Scree_iDotWithmm_X);
            _i_X_Buff_No = (int)_flDat;//
            if (_flDat - _i_X_Buff_No > 0.5) _i_X_Buff_No++;


            return _i_X_Buff_No;
        }
        #region 整体C扫描处理
        /// <summary>
        /// 计算屏幕行列个数
        /// 3.1 计算屏幕高度按照720进行，横轴刻度占用20像素  纵轴分4行C扫描图，700/4=175 300长光栅臂5mm一个间隔
        ///     60格，175/60=2.9166像素:确定屏幕序号、开始/结束行
        /// 3.2 画纵横轴 纵轴为行号
        /// 3.3 画当前屏幕的光栅臂显示数据
        /// 3.4 上下、左右翻页：屏幕序号、开始/结束行
        /// 3.5 鼠标移动位置信息提示：XY行号位置、厚度，点击后显示波形
        /// </summary>
        /// <param name="PicArea"></param>
        private void GetRulerPara_C_All()
        {
            if (Pic_C_All == null) return;

            #region 1 列数
            int iScreen_With = Pic_C_All.Width - Chart_Ruler_X_Start_C_All;
            int iScreen_Height = Pic_C_All.Height - Chart_Ruler_Y_Start;

            Scree_iAllCols_C = iScreen_With / Scree_iDotWith_X;
            #endregion 1

            #region 2 行数
            float _flT = 1.0f * iGsb_Len / Scree_iDotHeightmm_Y;
            Scree_iAllRows_C =(int)_flT;//300/5单位mm
            cls_C_All_Mark.Scree_flDotHeight = (float)(iScreen_Height * 1.0f / cls_C_All_Mark.i_ScreenRows / Scree_iAllRows_C);//700 /4/60 2.9166
            #endregion 2

            //3 宽度
            Scree_Stant_Distance_C_ALL = Scree_iAllCols_C * Scree_iDotWithmm_X;

            //-----------------

           // #region 涂层光栅臂单调计算
           //float   _T = 1.0f * iGsb_Len / (Scree_iDotHeightmm_Y_Coat);// * 1000);///单位m
           // i_Gsb_Arr_Len_Coat_ALL = (int)_T;
           // Scree_iDotHeight_Coat = (iScreen_Height / _T);
           // if (Scree_iDotHeight_Coat == 0) Scree_iDotHeight_Coat = 1;

           // Scree_iAllRows_C_Coat = (int)(iScreen_Height / Scree_iDotHeight_Coat);
           // #endregion 


            //-----------
            //4 每屏幕刻度
            Init_KD_C_ALL();
        }
        private void GetRulerPara_C_All_Coat()
        {
            if (Pic_C_All == null) return;

            #region 1 列数
            int iScreen_With = Pic_C_All.Width - Chart_Ruler_X_Start_C_All;
            int iScreen_Height = Pic_C_All.Height - Chart_Ruler_Y_Start;

            Scree_iAllCols_C = iScreen_With / Scree_iDotWith_X;
            #endregion 1

            #region 2 行数
            float _flT = 1.0f * iGsb_Len / Scree_iDotHeightmm_Y_Coat;
            Scree_iAllRows_C_Coat = (int)_flT;//300/5单位mm
            cls_C_All_Mark.Scree_flDotHeight_Coat = (float)(iScreen_Height * 1.0f / cls_C_All_Mark.i_ScreenRows / (Scree_iAllRows_C_Coat+1));//700 /4/60 2.9166
            #endregion 2
            //3 宽度
            Scree_Stant_Distance_C_ALL = Scree_iAllCols_C * Scree_iDotWithmm_X;
            //4 每屏幕刻度
            Init_KD_C_ALL();
        }
        /// <summary>
        /// C扫描单图像: 横轴刻度、纵轴为行数
        /// </summary>
        private void Plant_Ruler_C_ALL()
        {
            if (Pic_C_All == null) return;
            int i_X = 0, i_Y = 0;
            int iDatNum = 0;
            string strT = "";

            Chart_Clear(ref m_G_C_ALL);
            System.Windows.Forms.PictureBox.CheckForIllegalCrossThreadCalls = false;
            #region 画刻度尺

            #region 1 画布准备:初始化、右边出图、满屏时
            m_G_C_ALL.image = new Bitmap(Pic_C_All.ClientSize.Width, Pic_C_All.ClientSize.Height);
            // 获取背景层
            m_G_C_ALL.bg = (Bitmap)Pic_C_All.BackgroundImage;
            // 初始化整个画布
            m_G_C_ALL.canvas = new Bitmap(Pic_C_All.ClientSize.Width, Pic_C_All.ClientSize.Height);
            // 初始化图形面板，获取这块内存画布的Graphics的引用
            m_G_C_ALL.g = Graphics.FromImage(m_G_C_ALL.image);
            m_G_C_ALL.gb = Graphics.FromImage(m_G_C_ALL.canvas);
            m_G_C_ALL.g.Clear(Color.White);
            m_G_C_ALL.Buff = new PointF[0];

            int i_S_H = 0, i_S_L = 0;
            float _flEndKd = 0f;

            int _iGs = 10;
            #region 1.2 画横轴刻度
            i_S_H = Chart_Ruler_Y_Start - 6; i_S_L = Chart_Ruler_Y_Start - 3;
            i_Screen_Start_Distance = lst_Screenkd_C_ALL[cls_C_All_Mark.i_Screen_No].i_Start;
            i_Screen_End_Distance = lst_Screenkd_C_ALL[cls_C_All_Mark.i_Screen_No].i_End;

            for (int i = 1; i <= Scree_iAllCols_C; i++)
            {
                i_X = (int)(Chart_Ruler_X_Start_C_All + i * Scree_iDotWith_X);
                _flEndKd = i_Screen_Start_Distance + (i * Scree_iDotWithmm_X);//Chart_Run_flStart_Distance

                //2 画刻度线
                if (iRad_Dw == 0 && i % 5 != 0 || iRad_Dw == 1 && i % 4 != 0)
                    m_G_C_ALL.g.DrawLine(Ruler_p_One, new Point(i_X, i_S_L), new Point(i_X, Chart_Ruler_Y_Start));
                else
                    m_G_C_ALL.g.DrawLine(Ruler_p_One, new Point(i_X, i_S_H), new Point(i_X, Chart_Ruler_Y_Start));
                #region 标记横轴刻度值
                if (i % _iGs == 0)
                {
                    //if (i == _iGs && _blZt_mm == false)
                    //{ drawFont = new Font("Arial", (float)10); _blZt_mm = true; }
                    //else
                    //    drawFont = new Font("黑体", (float)7.9, FontStyle.Bold);
                    strT = _flEndKd.ToString("f0") + (i == _iGs ? "mm" : "");
                    PointF drawPoint = new PointF(i_X - 15, Chart_Ruler_Word_H);
                    m_G_C_ALL.g.DrawString(strT, drawFont, drawBrush, drawPoint);
                }
                #endregion 标记横轴刻度值
                if (i_X > Pic_C_All.ClientSize.Width) break;
            }
            m_G_C_ALL.g.DrawLine(Ruler_p_One, new Point(Chart_Ruler_X_Start_C_All, Chart_Ruler_Y_Start), new Point((int)(Chart_Ruler_X_Start_C_All +
                                                    Scree_iAllCols_C * Scree_iDotWith_X), Chart_Ruler_Y_Start));
            #endregion 1.2 画横轴刻度

            #region  1.3 画纵轴刻度
            StringFormat StrF = new StringFormat();
            StrF.FormatFlags = StringFormatFlags.DisplayFormatControl;  //DirectionVertical; // 竖排
            int iLeft_H = Chart_Ruler_X_Start_C_All - (Chart_Ruler_X_Start_C_All > 16 ? 16 : Chart_Ruler_X_Start_C_All) - 4;
            i_S_H = Chart_Ruler_X_Start_C_All - 6; i_S_L = Chart_Ruler_X_Start_C_All - 3;
            int _iX_Row = 0;
            Font _drawFont = new Font("黑体", (float)18, FontStyle.Bold);//行号字体变大
            float _fl_Y = 0;
            for (int _iRow = 0; _iRow < cls_C_All_Mark.i_ScreenRows; _iRow++)
            {
                for (int i = 1; i <= Scree_iAllRows_C; i++)
                {
                    _fl_Y = (Chart_Ruler_Y_Start + (_iRow * Scree_iAllRows_C + i) * cls_C_All_Mark.Scree_flDotHeight) + 1;
                    i_Y = (int)_fl_Y;
                    if (_fl_Y - i_Y > 0.5) i_Y++;
                    if (i % 10 != 0)
                    {
                        m_G_C_ALL.g.DrawLine(Ruler_p_One, new Point(i_S_L, i_Y), new Point(Chart_Ruler_X_Start_C_All, i_Y));
                        if (i == 1)//Scree_iDotHeightmm_Y.ToString() + 
                            m_G_C_ALL.g.DrawString((_iRow == 0 ? "mm" : ""), drawFont, drawBrush, iLeft_H, i_Y - 5, StrF);
                    }
                    else//刻度线
                    {
                        m_G_C_ALL.g.DrawLine(Ruler_p_One, new Point(i_S_H, i_Y), new Point(Chart_Ruler_X_Start_C_All, i_Y));
                        strT = (i * Scree_iDotHeightmm_Y).ToString("f0");
                        if (i == Scree_iAllRows_C)
                            m_G_C_ALL.g.DrawString(strT, drawFont, drawBrush, iLeft_H, i_Y - (_iX_Row > 9 ? 15 : 6), StrF);
                        else
                            m_G_C_ALL.g.DrawString(strT, drawFont, drawBrush, iLeft_H, i_Y - (_iX_Row > 9 ? 12 : 3), StrF);
                    }
                    if (i == 1)//行号
                    {
                        if (cls_C_All_Mark.bl_R1_L0)//1：从上往下     0：从下往上
                            strT = "" + (cls_C_All_Mark.i_Row_Start + _iRow);
                        else
                            strT = "" + (cls_C_All_Mark.i_Row_Start + cls_C_All_Mark.i_ScreenRows - 1 - _iRow);
                        m_G_C_ALL.g.DrawString(strT, _drawFont, drawBrush, -4, i_Y - (_iX_Row > 9 ? 12 : 3), StrF);
                    }
                    iDatNum++;
                    if (i_Y > Pic_C_All.ClientSize.Height)
                        break;
                }
            }
            m_G_C_ALL.g.DrawLine(Ruler_p_One, new Point(Chart_Ruler_X_Start_C_All, Chart_Ruler_Y_Start), new Point(Chart_Ruler_X_Start_C_All, Pic_C_All.ClientSize.Height));

            #endregion 1.3 画纵轴刻度

            Rectangle _Rect_Kd = new Rectangle(0, 0, Pic_C_All.Width, Pic_C_All.Height);
            if (m_G_C_ALL.bg != null)
            {
                try
                {
                    m_G_C_ALL.gb.DrawImage(m_G_C_ALL.bg, _Rect_Kd);// 先绘制背景层
                }
#pragma warning disable CS0168 // 声明了变量“de”，但从未使用过
                catch (Exception de)
#pragma warning restore CS0168 // 声明了变量“de”，但从未使用过
                { }
            }
            m_G_C_ALL.gb.DrawImage(m_G_C_ALL.image, _Rect_Kd); // 再绘制绘画层
            try
            {
                Pic_C_All.BackgroundImage = (Bitmap)m_G_C_ALL.canvas.Clone(); // 设置为背景层，就是在内存中建立了一张对应的画布，重新refresh就是再次的加载这个画布
                Pic_C_All.Refresh();
            }
            catch { }
            #endregion 1 画布准备
            #endregion 刻度尺

            Application.DoEvents();
        }
        /// <summary>
        /// 涂层C扫描图像初始化
        /// </summary>
        private void Plant_Ruler_Coat_ALL()
        {
            if (Pic_Coat_All == null) return;
            int i_X = 0, i_Y = 0;
            int iDatNum = 0;
            string strT = "";

            Chart_Clear(ref m_G_Coat_ALL);
            System.Windows.Forms.PictureBox.CheckForIllegalCrossThreadCalls = false;
            #region 画刻度尺

            #region 1 画布准备:初始化、右边出图、满屏时
            m_G_Coat_ALL.image = new Bitmap(Pic_Coat_All.ClientSize.Width, Pic_Coat_All.ClientSize.Height);
            // 获取背景层
            m_G_Coat_ALL.bg = (Bitmap)Pic_Coat_All.BackgroundImage;
            // 初始化整个画布
            m_G_Coat_ALL.canvas = new Bitmap(Pic_Coat_All.ClientSize.Width, Pic_Coat_All.ClientSize.Height);
            // 初始化图形面板，获取这块内存画布的Graphics的引用
            m_G_Coat_ALL.g = Graphics.FromImage(m_G_Coat_ALL.image);
            m_G_Coat_ALL.gb = Graphics.FromImage(m_G_Coat_ALL.canvas);
            m_G_Coat_ALL.g.Clear(Color.White);
            m_G_Coat_ALL.Buff = new PointF[0];

            int i_S_H = 0, i_S_L = 0;
            float _flEndKd = 0f;

            int _iGs = 10;
            #region 1.2 画横轴刻度
            i_S_H = Chart_Ruler_Y_Start - 6; i_S_L = Chart_Ruler_Y_Start - 3;
            i_Screen_Start_Distance = lst_Screenkd_C_ALL[cls_C_All_Mark.i_Screen_No].i_Start;
            i_Screen_End_Distance = lst_Screenkd_C_ALL[cls_C_All_Mark.i_Screen_No].i_End;

            for (int i = 1; i <= Scree_iAllCols_C; i++)
            {
                i_X = (int)(Chart_Ruler_X_Start_C_All + i * Scree_iDotWith_X);
                _flEndKd = i_Screen_Start_Distance + (i * Scree_iDotWithmm_X);//Chart_Run_flStart_Distance

                //2 画刻度线
                if (iRad_Dw == 0 && i % 5 != 0 || iRad_Dw == 1 && i % 4 != 0)
                    m_G_Coat_ALL.g.DrawLine(Ruler_p_One, new Point(i_X, i_S_L), new Point(i_X, Chart_Ruler_Y_Start));
                else
                    m_G_Coat_ALL.g.DrawLine(Ruler_p_One, new Point(i_X, i_S_H), new Point(i_X, Chart_Ruler_Y_Start));
                #region 标记横轴刻度值
                if (i % _iGs == 0)
                {
                    //if (i == _iGs && _blZt_mm == false)
                    //{ drawFont = new Font("Arial", (float)10); _blZt_mm = true; }
                    //else
                    //    drawFont = new Font("黑体", (float)7.9, FontStyle.Bold);
                    strT = _flEndKd.ToString("f0") + (i == _iGs ? "mm" : "");
                    PointF drawPoint = new PointF(i_X - 15, Chart_Ruler_Word_H);
                    m_G_Coat_ALL.g.DrawString(strT, drawFont, drawBrush, drawPoint);
                }
                #endregion 标记横轴刻度值
                if (i_X > Pic_Coat_All.ClientSize.Width) break;
            }
            m_G_Coat_ALL.g.DrawLine(Ruler_p_One, new Point(Chart_Ruler_X_Start_C_All, Chart_Ruler_Y_Start), new Point((int)(Chart_Ruler_X_Start_C_All +
                                                    Scree_iAllCols_C * Scree_iDotWith_X), Chart_Ruler_Y_Start));
            #endregion 1.2 画横轴刻度

            #region  1.3 画纵轴刻度
            StringFormat StrF = new StringFormat();
            StrF.FormatFlags = StringFormatFlags.DisplayFormatControl;  //DirectionVertical; // 竖排
            int iLeft_H = Chart_Ruler_X_Start_C_All - (Chart_Ruler_X_Start_C_All > 16 ? 16 : Chart_Ruler_X_Start_C_All) - 4;
            i_S_H = Chart_Ruler_X_Start_C_All - 6; i_S_L = Chart_Ruler_X_Start_C_All - 3;
            int _iX_Row = 0;
            Font _drawFont = new Font("黑体", (float)18, FontStyle.Bold);//行号字体变大
            float _fl_Y = 0;
            int _i_Scree_iAllRows_C_Coat = Scree_iAllRows_C_Coat + 1;
            for (int _iRow = 0; _iRow < cls_C_All_Mark.i_ScreenRows; _iRow++)
            {
                for (int i = 1; i <= _i_Scree_iAllRows_C_Coat; i++)
                {
                    _fl_Y = (Chart_Ruler_Y_Start + (_iRow * _i_Scree_iAllRows_C_Coat + i) * cls_C_All_Mark.Scree_flDotHeight_Coat) + 1;
                    i_Y = (int)_fl_Y;
                    if (_fl_Y - i_Y > 0.5) i_Y++;
              //      if (Scree_iDotHeightmm_Y_Coat < 20 && i % 10 != 0)
                    {
               //         m_G_Coat_ALL.g.DrawLine(Ruler_p_One, new Point(i_S_L, i_Y), new Point(Chart_Ruler_X_Start_C_All, i_Y));
                        //if (i == 1)//Scree_iDotHeightmm_Y.ToString() + 
                        //    m_G_Coat_ALL.g.DrawString((_iRow == 0 ? "mm" : ""), drawFont, drawBrush, iLeft_H, i_Y - 5, StrF);
                }
             //       else
                    if (i>1 && i % (Scree_iDotHeightmm_Y_Coat <= 20?10:2) == 0)//刻度线
                    {
                        m_G_Coat_ALL.g.DrawLine(Ruler_p_One, new Point(i_S_H, i_Y), new Point(Chart_Ruler_X_Start_C_All, i_Y));
                        strT = (i * Scree_iDotHeightmm_Y_Coat).ToString();
                        if (i == _i_Scree_iAllRows_C_Coat)
                            m_G_Coat_ALL.g.DrawString(strT, drawFont, drawBrush, iLeft_H, i_Y - (_iX_Row > 9 ? 15 : 6), StrF);
                        else
                            m_G_Coat_ALL.g.DrawString(strT, drawFont, drawBrush, iLeft_H, i_Y - (_iX_Row > 9 ? 12 : 3), StrF);
                    }
                    if (i == 1)//行号
                    {                       
                        _fl_Y = (Chart_Ruler_Y_Start + (_iRow * _i_Scree_iAllRows_C_Coat + i-1) * cls_C_All_Mark.Scree_flDotHeight_Coat);
                        i_Y = (int)_fl_Y;
                        m_G_Coat_ALL.g.DrawString((_iRow == 0 ? "mm" : ""), drawFont, drawBrush, iLeft_H-5, i_Y , StrF);
                        m_G_Coat_ALL.g.DrawLine(Ruler_p_One, new Point(i_S_H, i_Y), new Point(Chart_Ruler_X_Start_C_All, i_Y));

                        if (cls_C_All_Mark.bl_R1_L0)//1：从上往下     0：从下往上
                            strT = "" + (cls_C_All_Mark.i_Row_Start + _iRow);
                        else
                            strT = "" + (cls_C_All_Mark.i_Row_Start + cls_C_All_Mark.i_ScreenRows - 1 - _iRow);


                        m_G_Coat_ALL.g.DrawString(strT, _drawFont, drawBrush, -4, _fl_Y , StrF);//+ (_iX_Row > 9 ? 12 : 3)
                    }
                    iDatNum++;
                    if (i_Y > Pic_Coat_All.ClientSize.Height)
                        break;
                }
            }
            m_G_Coat_ALL.g.DrawLine(Ruler_p_One, new Point(Chart_Ruler_X_Start_C_All, Chart_Ruler_Y_Start), new Point(Chart_Ruler_X_Start_C_All, Pic_Coat_All.ClientSize.Height));

            #endregion 1.3 画纵轴刻度

            Rectangle _Rect_Kd = new Rectangle(0, 0, Pic_Coat_All.Width, Pic_Coat_All.Height);
            if (m_G_Coat_ALL.bg != null)
            {
                try
                {
                    m_G_Coat_ALL.gb.DrawImage(m_G_Coat_ALL.bg, _Rect_Kd);// 先绘制背景层
                }
#pragma warning disable CS0168 // 声明了变量“de”，但从未使用过
                catch (Exception de)
#pragma warning restore CS0168 // 声明了变量“de”，但从未使用过
                { }
            }
            m_G_Coat_ALL.gb.DrawImage(m_G_Coat_ALL.image, _Rect_Kd); // 再绘制绘画层
            try
            {
                Pic_Coat_All.BackgroundImage = (Bitmap)m_G_Coat_ALL.canvas.Clone(); // 设置为背景层，就是在内存中建立了一张对应的画布，重新refresh就是再次的加载这个画布
                Pic_Coat_All.Refresh();
            }
            catch { }
            #endregion 1 画布准备
            #endregion 刻度尺

            Application.DoEvents();
        }
        /// <summary>
        /// 获得当前行的屏幕对应开始结束序号
        /// </summary>
        /// <param name="iS_E">0:开始  1：结束</param>
        /// <param name="_lstRowRecord">当前行记录</param>
        /// <returns></returns>
        public int Get_S_E_No_C_ALL(int iS_E, List<CLs_EMAT_Data> _lstRowRecord)
        {
            int _iRet = -1;
            int _iX_mm = 0;

            bool _blDd = false;
            int _iLen = _lstRowRecord.Count - 1;
            if (_iLen > 2)
            {
                _blDd = _lstRowRecord[1].i_X_mm > _lstRowRecord[_iLen - 2].i_X_mm;
            }

            if (iS_E == 0)//开始
            {
                if (_blDd)//颠倒
                {
                    for (int _iC = 0; _iC < _lstRowRecord.Count; _iC++)
                    {
                        if (_lstRowRecord[_iC].i_X_mm < cls_C_All_Mark.fl_Dist_E)
                        {

                            if (_lstRowRecord.Count > 3 && Math.Abs(_lstRowRecord[_iC].i_X_mm - _lstRowRecord[_iC + 1].i_X_mm) <= Scree_iDotWithmm_X)
                            {
                                _iRet = _iC; break;
                            }
                        }
                    }
                }
                else
                {
                    for (int _iC = 0; _iC < _lstRowRecord.Count; _iC++)
                    {
                        if (_lstRowRecord[_iC].i_X_mm >= cls_C_All_Mark.fl_Dist_S)
                        {
                            _iRet = _iC; break;
                        }
                    }
                }
            }
            else//结束
            {
                if (_blDd)//颠倒
                {
                    for (int _iC = 1; _iC < _lstRowRecord.Count; _iC++)
                    {
                        _iX_mm = _lstRowRecord[_iC].i_X_mm;
                        if (_iX_mm <= cls_C_All_Mark.fl_Dist_E)
                        {
                            if (_iX_mm >= cls_C_All_Mark.fl_Dist_S)
                            {
                                _iRet = _iC;
                            }
                            else
                                break;
                        }
                    }
                }
                else//增加
                {
                    for (int _iC = 0; _iC < _lstRowRecord.Count; _iC++)
                    {
                        _iX_mm = _lstRowRecord[_iC].i_X_mm;
                        if (_iX_mm >= cls_C_All_Mark.fl_Dist_S)
                        {
                            if (_iX_mm < cls_C_All_Mark.fl_Dist_E)
                            {
                                _iRet = _iC;
                            }
                            else
                                break;
                        }
                    }
                }

            }
            return _iRet;
        }
        public int Get_S_E_No_C_ALL(int iS_E, List<Cls_Mul_ShowBuff> _lstRowRecord)
        {
            int _iRet = -1;
            int _iX_mm = 0;

            bool _blDd = false;
            int _iLen = _lstRowRecord.Count - 1;
            if (_iLen > 2)
            {
                _blDd = _lstRowRecord[1].m_i_X > _lstRowRecord[_iLen - 2].m_i_X;
            }

            if (iS_E == 0)//开始
            {
                if (_blDd)//颠倒
                {
                    for (int _iC = 0; _iC < _lstRowRecord.Count; _iC++)
                    {
                        if (_lstRowRecord[_iC].m_i_X < cls_C_All_Mark.fl_Dist_E)
                        {

                            if (_lstRowRecord.Count > 10 && Math.Abs(_lstRowRecord[_iC].m_i_X - _lstRowRecord[_iC + 1].m_i_X) <= Scree_iDotWithmm_X)
                            {
                                _iRet = _iC; break;
                            }
                        }
                    }
                }
                else
                {
                    for (int _iC = 0; _iC < _lstRowRecord.Count; _iC++)
                    {
                        if (_lstRowRecord[_iC].m_i_X >= cls_C_All_Mark.fl_Dist_S)
                        {

                            if (_lstRowRecord.Count > 10 && Math.Abs(_lstRowRecord[_iC].m_i_X - _lstRowRecord[_iC + 1].m_i_X) <= Scree_iDotWithmm_X)
                            {
                                _iRet = _iC; break;
                            }
                        }
                    }
                }
            }
            else//结束
            {
                if (_blDd)//颠倒
                {
                    for (int _iC = 1; _iC < _lstRowRecord.Count; _iC++)
                    {
                        _iX_mm = _lstRowRecord[_iC].m_i_X;
                        if (_iX_mm <= cls_C_All_Mark.fl_Dist_E)
                        {
                            if (_iX_mm >= cls_C_All_Mark.fl_Dist_S)
                            {
                                _iRet = _iC;
                            }
                            else
                                break;
                        }
                    }
                }
                else//增加
                {
                    for (int _iC = 0; _iC < _lstRowRecord.Count; _iC++)
                    {
                        _iX_mm = _lstRowRecord[_iC].m_i_X;
                        if (_iX_mm >= cls_C_All_Mark.fl_Dist_S)
                        {
                            if (_iX_mm < cls_C_All_Mark.fl_Dist_E)
                            {
                                _iRet = _iC;
                            }
                            else
                                break;
                        }
                    }
                }

            }
            return _iRet;
        }
        /// <summary>
        /// 全部C扫描每行的属性值
        /// </summary>
       // public List<Cls_All_C_RowPro> m_Lst_C_All_Pro, m_Lst_Coat_All_Pro;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bl_R1_L0">未检区域在   1：右边 0：左边</param>
        /// <param name="iScreenNo">0-N</param>
        /// <param name="iRow_Start">1</param>
        /// <param name="iRow_End">4</param>
        /// <param name="m_Lst_C_Buff"></param>
        /// <returns></returns>
        public int Plant_C_ALL(bool bl_R1_L0, int iScreenNo, int iRow_Start, int iRow_End, List<List<CLs_EMAT_Data>> m_Lst_C_Buff)
        {
            int _iRet = 0;
            int _iAllNum = m_Lst_C_Buff.Count;
            if (_iAllNum == 0) return 2;

            //0 参数检查
            if (iRow_End > _iAllNum)
                iRow_End = _iAllNum;
            if (iRow_Start > iRow_End)
                iRow_Start = iRow_End;

            if (iRow_Start < 1 || iRow_Start > _iAllNum || iRow_End < 1 || iRow_End > _iAllNum)
            {
                _iRet = 3; return _iRet;
            }
            if (i_Screen_No < 0)
            {
                _iRet = 4; return _iRet;
            }
            cls_C_All_Mark.i_Screen_No = iScreenNo;
            //2 界面初始化
            GetRulerPara_C_All();
            Plant_Ruler_C_ALL();
            Mul_Titl_Init(Scree_iAllRows_C * 4);

            //1 参数初始化
            cls_C_All_Mark.bl_R1_L0 = bl_R1_L0;

            cls_C_All_Mark.fl_Dist_S = lst_Screenkd_C_ALL[cls_C_All_Mark.i_Screen_No].i_Start;
            cls_C_All_Mark.fl_Dist_E = lst_Screenkd_C_ALL[cls_C_All_Mark.i_Screen_No].i_End;
            cls_C_All_Mark.i_Row_Start = iRow_Start;
            cls_C_All_Mark.i_Row_End = iRow_End;
            //      cls_C_All_Mark.i_ScreenRows = Scree_iAllRows_C;

            //3 拿指定数据画当前界面
            int _iCol_Start = 0, _iCol_End = 0, _iX = 0, _i_Y = 0, _iCurr_Row = 1;
            int _iAllPiex = cls_C_All_Mark.i_ScreenRows * Scree_iAllRows_C - 1;
        //    m_Lst_C_All_Pro = new List<Cls_All_C_RowPro>();

            int _i_Jl_S = 0, _i_Jl_E = 0;
            bool _bl_Dd = false;
#pragma warning disable CS0219 // 变量“_strTitl”已被赋值，但从未使用过它的值
            string _strTitl = "";
#pragma warning restore CS0219 // 变量“_strTitl”已被赋值，但从未使用过它的值

            int _iRowAdd = 0;
            #region 画超声C图
            for (int _iRow = iRow_Start - 1; _iRow < iRow_End; _iRow++)
            {
                _iRowAdd++;
                if (_iRowAdd > 4) break;
                _iCurr_Row = _iRow - (iRow_Start - 1);//0-N
                //3.1拿当前行数据
                List<CLs_EMAT_Data> _lstRowRecord = m_Lst_C_Buff[_iRow];

                //   _strTitl = _lstRowRecord
                //3.2 计算当前行当前屏幕的开始/结束位置
                _iCol_Start = Get_S_E_No_C_ALL(0, _lstRowRecord);
                _iCol_End = Get_S_E_No_C_ALL(1, _lstRowRecord);

                //3.3 刷新当前行、当前区域数据
                if (_iCol_Start > -1 && _iCol_End > -1)
                {
                    _i_Jl_S = _lstRowRecord[_iCol_Start].i_X_mm;
                    _i_Jl_E = _lstRowRecord[_iCol_End].i_X_mm;
                    _bl_Dd = false;
                    if (_i_Jl_S > _i_Jl_E)
                    {
                        _bl_Dd = true;
                        int _i_D = _i_Jl_S;
                        _i_Jl_S = _i_Jl_E;
                        _i_Jl_E = _i_D;
                    }
                    //1 添加每行数据位置属性，防止倒着检测
                    Cls_All_C_RowPro _C_Pro = new Cls_All_C_RowPro();

                    float _fl = (_i_Jl_S - i_Screen_Start_Distance) / Scree_iDotWithmm_X;
                    _C_Pro.i_Start_No = (int)_fl;
                    if (_fl - _C_Pro.i_Start_No >= 0.5) _C_Pro.i_Start_No++;
                    _C_Pro.i_lst_Num = _bl_Dd ? _lstRowRecord.Count - 1 : 0;
             //       m_Lst_C_All_Pro.Add(_C_Pro);

                    for (int _iCol = _iCol_Start; _iCol <= _iCol_End; _iCol++)
                    {
                        //3.4 拿数据
                        for (int _iR_C = 0; _iR_C < Scree_iAllRows_C; _iR_C++)
                        {
                            _iX = ((_lstRowRecord[_iCol].i_X_mm - i_Screen_Start_Distance) / Scree_iDotWithmm_X);
                            Cls_EMAT_2 _C_Data = _lstRowRecord[_iCol].Arr_C_Data[_iR_C];
                            _C_Data.m_i_X = _lstRowRecord[_iCol].i_X_mm;
                            //3.5 计算Y位置
                            if (cls_C_All_Mark.bl_R1_L0)
                                _i_Y = _iR_C + _iCurr_Row * Scree_iAllRows_C;
                            else
                                _i_Y = (cls_C_All_Mark.i_ScreenRows - 1 - _iCurr_Row) * Scree_iAllRows_C + _iR_C;
                            //3.6 画图
                            if (_C_Data.flThick >= 0)
                                Plant_C_ALL(_iX, _i_Y, _C_Data, m_iLanguage == 0 ? "第" + (_iRow + 1).ToString() + "行" : "Line: " + (_iRow + 1).ToString(),(int)( _iR_C * Scree_iDotHeightmm_Y), 1);//(_iR_C + 1) * 5, 1);


                        }
                    }
                }
            } Application.DoEvents();
            Rectangle _Rect = new Rectangle(0, 0, Pic_C_All.Width, Pic_C_All.Height);

            if (m_G_C_ALL.bg != null)
                m_G_C_ALL.gb.DrawImage(m_G_C_ALL.bg, _Rect);// 先绘制背景层

            m_G_C_ALL.gb.DrawImage(m_G_C_ALL.image, _Rect); // 再绘制绘画层
            Pic_C_All.BackgroundImage = (Bitmap)m_G_C_ALL.canvas.Clone();
            #endregion 超声C图

            //4 退出
            _iRet = 1;
            return _iRet;
        }

        public int Plant_C_ALL_Coat(bool bl_R1_L0, int iScreenNo, int iRow_Start, int iRow_End, List<List<CLs_EMAT_Data>> m_Lst_C_Buff)
        {
            int _iRet = 0;
            int _iAllNum = m_Lst_C_Buff.Count;
            if (_iAllNum == 0) return 2;

            //0 参数检查
            if (iRow_End > _iAllNum)
                iRow_End = _iAllNum;
            if (iRow_Start > iRow_End)
                iRow_Start = iRow_End;

            if (iRow_Start < 1 || iRow_Start > _iAllNum || iRow_End < 1 || iRow_End > _iAllNum)
            {
                _iRet = 3; return _iRet;
            }
            if (i_Screen_No < 0)
            {
                _iRet = 4; return _iRet;
            }
            cls_C_All_Mark.i_Screen_No = iScreenNo;
            //2 界面初始化
            GetRulerPara_C_All_Coat();
            Plant_Ruler_Coat_ALL();
            Mul_Titl_Init_Coat((Scree_iAllRows_C_Coat+1) * 4);

            //1 参数初始化
            cls_C_All_Mark.bl_R1_L0 = bl_R1_L0;

            cls_C_All_Mark.fl_Dist_S = lst_Screenkd_C_ALL[cls_C_All_Mark.i_Screen_No].i_Start;
            cls_C_All_Mark.fl_Dist_E = lst_Screenkd_C_ALL[cls_C_All_Mark.i_Screen_No].i_End;
            cls_C_All_Mark.i_Row_Start = iRow_Start;
            cls_C_All_Mark.i_Row_End = iRow_End;
            //      cls_C_All_Mark.i_ScreenRows = Scree_iAllRows_C;

            //3 拿指定数据画当前界面
            int _iCol_Start = 0, _iCol_End = 0, _iX = 0, _i_Y = 0, _iCurr_Row = 1;
     //       int _iAllPiex = cls_C_All_Mark.i_ScreenRows * Scree_iAllRows_C_Coat - 1;
      //      m_Lst_Coat_All_Pro = new List<Cls_All_C_RowPro>();

            int _i_Jl_S = 0, _i_Jl_E = 0;
            bool _bl_Dd = false;
            string _strTitl = "";

            int _iRowAdd = 0, _i_Scree_iAllRows_C_Coat= Scree_iAllRows_C_Coat+1;
           
            #region 画超声C图
            for (int _iRow = iRow_Start - 1; _iRow < iRow_End; _iRow++)
            {
                _iRowAdd++;
                if (_iRowAdd > 4) break;
                _iCurr_Row = _iRow - (iRow_Start - 1);//0-N
                //3.1拿当前行数据
                List<CLs_EMAT_Data> _lstRowRecord = m_Lst_C_Buff[_iRow];

                //   _strTitl = _lstRowRecord
                //3.2 计算当前行当前屏幕的开始/结束位置
                _iCol_Start = Get_S_E_No_C_ALL(0, _lstRowRecord);
                _iCol_End = Get_S_E_No_C_ALL(1, _lstRowRecord);

                //3.3 刷新当前行、当前区域数据
                if (_iCol_Start > -1 && _iCol_End > -1)
                {
                    _i_Jl_S = _lstRowRecord[_iCol_Start].i_X_mm;
                    _i_Jl_E = _lstRowRecord[_iCol_End].i_X_mm;
                    _bl_Dd = false;
                    if (_i_Jl_S > _i_Jl_E)
                    {
                        _bl_Dd = true;
                        int _i_D = _i_Jl_S;
                        _i_Jl_S = _i_Jl_E;
                        _i_Jl_E = _i_D;
                    }
                    //1 添加每行数据位置属性，防止倒着检测
                    Cls_All_C_RowPro _C_Pro = new Cls_All_C_RowPro();

                    float _fl = (_i_Jl_S - i_Screen_Start_Distance) / Scree_iDotWithmm_X;
                    _C_Pro.i_Start_No = (int)_fl;
                    if (_fl - _C_Pro.i_Start_No >= 0.5) _C_Pro.i_Start_No++;
                    _C_Pro.i_lst_Num = _bl_Dd ? _lstRowRecord.Count - 1 : 0;
                    //    m_Lst_Coat_All_Pro.Add(_C_Pro);
                    int _i_Fd = -1;

                    int _iNo_Coat = 0;
                    if (cls_C_All_Mark.bl_R1_L0)
                        _iNo_Coat = 0 + _iCurr_Row * _i_Scree_iAllRows_C_Coat;
                    else
                        _iNo_Coat = (cls_C_All_Mark.i_ScreenRows - 1 - _iCurr_Row) * _i_Scree_iAllRows_C_Coat + 0;

                    bool _bl_Dis = false, _bl_Mark = false;
                    float f_Y=0;
                    int i_X = 0;
                    for (int _iCol = _iCol_Start; _iCol <= _iCol_End; _iCol++)
                    {
                        _i_Fd = -1;

                        //3.4 拿数据
                        for (int _iR_C = 0; _iR_C < _i_Scree_iAllRows_C_Coat; _iR_C++)
                        {
                            _iX = ((_lstRowRecord[_iCol].i_X_mm - i_Screen_Start_Distance) / Scree_iDotWithmm_X);
                            Cls_EMAT_2 _C_Data = _lstRowRecord[_iCol].Arr_C_Data[_iR_C];
                            _C_Data.m_i_X = _lstRowRecord[_iCol].i_X_mm;
                            //3.5 计算Y位置
                            if (cls_C_All_Mark.bl_R1_L0)
                                _i_Y = _iR_C + _iCurr_Row * _i_Scree_iAllRows_C_Coat;
                            else
                                _i_Y = (cls_C_All_Mark.i_ScreenRows - 1 - _iCurr_Row) * _i_Scree_iAllRows_C_Coat + _iR_C;
                            //3.6 画图
                            if (_C_Data.flThick >= 0)
                            {
                                _i_Fd++;
                                _bl_Dis = _lstRowRecord[_iCol].bl_Discharge;
                                _bl_Mark = _lstRowRecord[_iCol].bl_Discharge_To_Mark;

                                _strTitl = m_iLanguage == 0 ? "第" + (_iRow + 1).ToString() + "行" : "Line: " + (_iRow + 1).ToString();
                                _strTitl += m_iLanguage == 0 ?
                                   (_bl_Dis?" 放电":"") + (_bl_Mark?" 打标":"") : (_bl_Dis ? " Discharge" : "") + (_bl_Mark ? " Mark" : "");
                                    
                                Plant_C_ALL_Coat(_iX, _i_Y, _C_Data, _strTitl , _C_Data.iArr_ShowNo, 1); //(_iR_C + 1) * Scree_iDotHeightmm_Y_Coat, 1);
                                                                                                                             //      if (_i_Fd == 0)
                                {
                                    if (_bl_Dis || _bl_Mark)
                                    {
                                        bool _blNew = true   ;
                                       
                                        int _i_Add = 2;
                                        f_Y = (Chart_Ruler_Y_Start + (_iNo_Coat * cls_C_All_Mark.Scree_flDotHeight_Coat)) + 15;
                                        i_X = Chart_Ruler_X_Start_C_All + _iX * Scree_iDotWith_X +
                                         (  _blNew==false ?(Scree_iDotWith_X / 2 - 7):0);
                                        _i_Y = (int)f_Y;
                                        if (_bl_Dis)
                                        {
                                            if (_blNew)
                                            {
                                                Pen _pen = new Pen(Color.Red, 1.5f);
                                                _i_Y -= 15;
                                                m_G_Coat_ALL.g.DrawLine(_pen, new Point(i_X - _i_Add, _i_Y + 8), new Point(i_X + _i_Add, _i_Y));
                                                m_G_Coat_ALL.g.DrawLine(_pen, new Point(i_X - _i_Add, _i_Y + 8), new Point(i_X + _i_Add + _i_Add, _i_Y + 8));
                                                m_G_Coat_ALL.g.DrawLine(_pen, new Point(i_X + _i_Add, _i_Y), new Point(i_X + _i_Add + _i_Add, _i_Y + 8));
                                            }
                                            else
                                               Fd_All_Add(_iX, _iNo_Coat, _strTitl);
                                        }
                                        if (_bl_Mark)
                                        {
                                            if (_blNew)
                                            { Pen _pen = new Pen(Color.Red, 2);
                                              if(_bl_Dis)     _i_Y += 15;

                                                m_G_Coat_ALL.g.DrawEllipse(_pen, i_X, _i_Y, 5, 5);
                                            }
                                            else
                                               Fd_To_Mark_All_Add(_iX, _iNo_Coat, _strTitl);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            } 
            Rectangle _Rect = new Rectangle(0, 0, Pic_Coat_All.Width, Pic_Coat_All.Height);

            if (m_G_Coat_ALL.bg != null)
                m_G_Coat_ALL.gb.DrawImage(m_G_Coat_ALL.bg, _Rect);// 先绘制背景层

            m_G_Coat_ALL.gb.DrawImage(m_G_Coat_ALL.image, _Rect); // 再绘制绘画层
            Pic_Coat_All.BackgroundImage = (Bitmap)m_G_Coat_ALL.canvas.Clone();
            #endregion 超声C图
Application.DoEvents();
            //4 退出
            _iRet = 1;
            return _iRet;
        }
        #region 在涂层整体C扫描图上放置放电图标
        /// <summary>
        /// 涂层整体C扫描放电图标
        /// </summary>
        public List<Button> m_lst_Coat_All_Fd=new List<Button> ();

        /// <summary>
        /// 涂层整体C扫描收到打标命令显示打标图标
        /// </summary>
        public List<Button> m_lst_Coat_All_Fd_To_Mark = new List<Button>();
        /// <summary>
        /// 放电图标
        /// </summary>
        public Button m_bt_Coat_All_Fd;

        /// <summary>
        /// 打标图标
        /// </summary>
        public Button m_bt_Coat_All_Fd_To_Mark;
        /// <summary>
        /// 图标个数
        /// </summary>
        public int m_Coat_All_Fd_Num = 0;
        public void Fd_All_Clear()
        {
            m_Coat_All_Fd_Num = 0;

            if (m_lst_Coat_All_Fd != null)
            {
                for (int i = 0; i < m_lst_Coat_All_Fd.Count; i++)
                {
                    m_lst_Coat_All_Fd[i].Visible = false;
                    Pic_Coat_All.Controls.Remove(m_lst_Coat_All_Fd[i]);
                }
                m_lst_Coat_All_Fd.Clear();
            }

            if (m_lst_Coat_All_Fd_To_Mark != null)
            {
                for (int i = 0; i < m_lst_Coat_All_Fd_To_Mark.Count; i++)
                {
                    m_lst_Coat_All_Fd_To_Mark[i].Visible = false;
                    Pic_Coat_All.Controls.Remove(m_lst_Coat_All_Fd_To_Mark[i]);
                }
                m_lst_Coat_All_Fd_To_Mark.Clear();
            }
        }
        private void Fd_All_Add(int iX, int iY, string strTitl, int iAdd_1 = 1)
        {
            int _iWith = 10, _Height = 15; //m_bt_Coat_All_Fd.Height;// m_bt_Coat_All_Fd.Width

            Button _bt = new Button();
            _bt.BackColor = Color.Transparent;
            _bt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            _bt.FlatStyle = FlatStyle.Popup;
            _bt.Name = "Bt_Mul_DisCharge" + "_" + m_Coat_All_Fd_Num++;
            _bt.Width = _iWith;
            _bt.Height = _Height;
            _bt.Visible = true;

            float f_Y = (Chart_Ruler_Y_Start + (iY * cls_C_All_Mark.Scree_flDotHeight_Coat)) + (iAdd_1 == 1 ? 1 : 0);
            int i_X = Chart_Ruler_X_Start_C_All + iX * Scree_iDotWith_X +
                      Scree_iDotWith_X / 2 - _iWith / 2;
            _bt.Location = new Point(i_X, (int)f_Y);
            string _strT = "放电：" + strTitl + " " + (i_Screen_Start_Distance + iX * Scree_iDotWithmm_X).ToString() + "mm";
            Tool_Tip(_bt, _strT);

            _bt.BackgroundImage = m_bt_Coat_All_Fd.BackgroundImage;
            if (m_lst_Coat_All_Fd == null)
                m_lst_Coat_All_Fd = new List<Button>();
            m_lst_Coat_All_Fd.Add(_bt);

            SetBtnStyle(_bt);
            SetTran(Pic_Coat_All, _bt);
            Pic_Coat_All.Controls.Add(_bt);
        }
        private void Fd_To_Mark_All_Add(int iX, int iY, string strTitl, int iAdd_1 = 1)
        {
            int _iWith = 10, _Height = 10;// m_bt_Coat_All_Fd_To_Mark.Height;

            Button _bt = new Button();
            _bt.BackColor = Color.Transparent;
            _bt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            _bt.FlatStyle = FlatStyle.Popup;
            _bt.Name = "Bt_Mul_DisCharge_Mark" + "_" + m_Coat_All_Fd_Num++;
            _bt.Width = _iWith;
            _bt.Height = _Height;
            _bt.Visible = true;

            float f_Y = (Chart_Ruler_Y_Start + (iY * cls_C_All_Mark.Scree_flDotHeight_Coat)) + (iAdd_1 == 1 ? 15 : 0);
            int i_X = Chart_Ruler_X_Start_C_All + iX * Scree_iDotWith_X +
                      Scree_iDotWith_X / 2 - _iWith / 2;
            _bt.Location = new Point(i_X, (int)f_Y);
            string _strT = "打标：" + strTitl + " " + (i_Screen_Start_Distance + iX * Scree_iDotWithmm_X).ToString() + "mm";
            Tool_Tip(_bt, _strT);

            _bt.BackgroundImage = m_bt_Coat_All_Fd_To_Mark.BackgroundImage;
            if (m_lst_Coat_All_Fd_To_Mark == null)
                m_lst_Coat_All_Fd_To_Mark = new List<Button>();
            m_lst_Coat_All_Fd_To_Mark.Add(_bt);

            SetBtnStyle(_bt);
            SetTran(Pic_Coat_All, _bt);
            Pic_Coat_All.Controls.Add(_bt);
        }

        private   void Tool_Tip(System.Windows.Forms.Control ctTiTl, string strVal)
        {
            ToolTip tooltip2 = new ToolTip();
            //设置提示框显示时间，默认5000，最大为32767，超过此数，将以默认5000显示           
            tooltip2.AutoPopDelay = 6000;
            //设置要显示提示框的控件 button1按钮
            tooltip2.SetToolTip(ctTiTl, strVal);
        }
        private void SetBtnStyle(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;//样式
            btn.ForeColor = Color.Transparent;//前景
            btn.BackColor = Color.Transparent;//去背景
            btn.FlatAppearance.BorderSize = 0;//去边线
            btn.FlatAppearance.MouseOverBackColor = Color.Transparent;//鼠标经过
            btn.FlatAppearance.MouseDownBackColor = Color.Transparent;//鼠标按下

        }
        private void SetTran(PictureBox pic_Parents, Button pic_Son)
        {
            pic_Parents.SendToBack();
            pic_Son.BackColor = Color.Transparent;
            pic_Son.Parent = pic_Parents;
            pic_Son.BringToFront();
        }
        #endregion 放置放电图标
        /// <summary>
        /// 多通道全C扫描图界面计算 
        /// </summary>
        /// <param name="m_iRomoteNum">探头个数</param>
        private void GetRulerPara_C_All_Mul(int m_iRomoteNum = 1)
        {
            if (Pic_C_All == null) return;
            if (m_iRomoteNum < 0) m_iRomoteNum = 1;

            #region 1 列数
            int iScreen_With = Pic_C_All.Width - Chart_Ruler_X_Start_C_All;
            int iScreen_Height = Pic_C_All.Height - Chart_Ruler_Y_Start;

            Scree_iAllCols_C = iScreen_With / Scree_iDotWith_X;
            #endregion 1

            #region 2 行数
            cls_C_All_Mark.Scree_flDotHeight = Scree_iDotHeight_C_Mul;
            int _iT = (int)(iScreen_Height / Scree_iDotHeight_C_Mul);//700 /4/60 2.9166

            Scree_iAllRows_C_Mul = _iT / m_iRomoteNum;
            Scree_iAllRows_C = Scree_iAllRows_C_Mul * m_iRomoteNum;
            #endregion 2

            //3 宽度
            Scree_Stant_Distance_C_ALL = Scree_iAllCols_C * Scree_iDotWithmm_X;
            //4 每屏幕刻度
            Init_KD_C_ALL();
        }
        /// <summary>
        /// 多通道C扫描单图像: 横轴刻度、纵轴为行数
        /// </summary>
        private void Plant_Ruler_C_ALL_Mul()
        {
            if (Pic_C_All == null) return;
            int i_X = 0, i_Y = 0;
            int iDatNum = 0;
            string strT = "";

            Chart_Clear(ref m_G_C_ALL);
            System.Windows.Forms.PictureBox.CheckForIllegalCrossThreadCalls = false;
            #region 画刻度尺

            #region 1 画布准备:初始化、右边出图、满屏时
            m_G_C_ALL.image = new Bitmap(Pic_C_All.ClientSize.Width, Pic_C_All.ClientSize.Height);
            // 获取背景层
            m_G_C_ALL.bg = (Bitmap)Pic_C_All.BackgroundImage;
            // 初始化整个画布
            m_G_C_ALL.canvas = new Bitmap(Pic_C_All.ClientSize.Width, Pic_C_All.ClientSize.Height);
            // 初始化图形面板，获取这块内存画布的Graphics的引用
            m_G_C_ALL.g = Graphics.FromImage(m_G_C_ALL.image);
            m_G_C_ALL.gb = Graphics.FromImage(m_G_C_ALL.canvas);
            m_G_C_ALL.g.Clear(Color.White);
            m_G_C_ALL.Buff = new PointF[0];

            int i_S_H = 0, i_S_L = 0;
            float _flEndKd = 0f;

            int _iGs = 10;
            #region 1.2 画横轴刻度
            i_S_H = Chart_Ruler_Y_Start - 6; i_S_L = Chart_Ruler_Y_Start - 3;
            i_Screen_Start_Distance = lst_Screenkd_C_ALL[cls_C_All_Mark.i_Screen_No].i_Start;
            i_Screen_End_Distance = lst_Screenkd_C_ALL[cls_C_All_Mark.i_Screen_No].i_End;

            for (int i = 1; i <= Scree_iAllCols_C; i++)
            {
                i_X = (int)(Chart_Ruler_X_Start_C_All + i * Scree_iDotWith_X);
                _flEndKd = i_Screen_Start_Distance + (i * Scree_iDotWithmm_X);//Chart_Run_flStart_Distance

                //2 画刻度线
                if (iRad_Dw == 0 && i % 5 != 0 || iRad_Dw == 1 && i % 4 != 0)
                    m_G_C_ALL.g.DrawLine(Ruler_p_One, new Point(i_X, i_S_L), new Point(i_X, Chart_Ruler_Y_Start));
                else
                    m_G_C_ALL.g.DrawLine(Ruler_p_One, new Point(i_X, i_S_H), new Point(i_X, Chart_Ruler_Y_Start));
                #region 标记横轴刻度值
                if (i % _iGs == 0)
                {
                    strT = _flEndKd.ToString("f0") + (i == _iGs ? "mm" : "");
                    PointF drawPoint = new PointF(i_X - 15, Chart_Ruler_Word_H);
                    m_G_C_ALL.g.DrawString(strT, drawFont, drawBrush, drawPoint);
                }
                #endregion 标记横轴刻度值
                if (i_X > Pic_C_All.ClientSize.Width) break;
            }
            m_G_C_ALL.g.DrawLine(Ruler_p_One, new Point(Chart_Ruler_X_Start_C_All, Chart_Ruler_Y_Start), new Point((int)(Chart_Ruler_X_Start_C_All +
                                                    Scree_iAllCols_C * Scree_iDotWith_X), Chart_Ruler_Y_Start));
            #endregion 1.2 画横轴刻度

            #region  1.3 画纵轴刻度
            StringFormat StrF = new StringFormat();
            StrF.FormatFlags = StringFormatFlags.DisplayFormatControl;  //DirectionVertical; // 竖排
            int iLeft_H = Chart_Ruler_X_Start_C_All - (Chart_Ruler_X_Start_C_All > 16 ? 16 : Chart_Ruler_X_Start_C_All) - 4;
            i_S_H = Chart_Ruler_X_Start_C_All - 6; i_S_L = Chart_Ruler_X_Start_C_All - 3;
            int _iX_Row = 0;
            Font _drawFont = new Font("黑体", (float)18, FontStyle.Bold);//行号字体变大
            float _fl_Y = 0;

            int _iCurr_No = 0;
            for (int i = 1; i <= Scree_iAllRows_C; i++)
            {
                //1 纵轴坐标位置
                if (cls_C_All_Mark.bl_R1_L0)
                    _fl_Y = (Chart_Ruler_Y_Start + i * cls_C_All_Mark.Scree_flDotHeight) + 1;
                else
                    _fl_Y = (Chart_Ruler_Y_Start + (cls_C_All_Mark.i_ScreenRows - i) * cls_C_All_Mark.Scree_flDotHeight) + 1;
                i_Y = (int)_fl_Y;
                if (_fl_Y - i_Y > 0.5) i_Y++;

                if (i % 5 != 0 || i == 1)
                {
                    m_G_C_ALL.g.DrawLine(Ruler_p_One, new Point(i_S_L, i_Y), new Point(Chart_Ruler_X_Start_C_All, i_Y));
                    //if (i == 1)//Scree_iDotHeightmm_Y.ToString() + 
                    //    m_G_C_ALL.g.DrawString("1", drawFont, drawBrush, iLeft_H, i_Y - 5, StrF);
                }
                else//刻度线
                {
                    //1 画线
                    m_G_C_ALL.g.DrawLine(Ruler_p_One, new Point(i_S_H, i_Y), new Point(Chart_Ruler_X_Start_C_All, i_Y));
                    //2 线号
                    //     if (cls_C_All_Mark.bl_R1_L0)//1：从上往下     0：从下往上
                    _iCurr_No = (cls_C_All_Mark.i_Screen_Row_Start + i - 1);
                    //      else
                    //          _iCurr_No = cls_C_All_Mark.i_Row_Start + cls_C_All_Mark.i_ScreenRows - i+1 ;

                    m_G_C_ALL.g.DrawString(_iCurr_No.ToString(), drawFont, drawBrush, 15, i_Y - (_iX_Row > 9 ? 12 : 3), StrF);
                }
                iDatNum++;
                if (i_Y > Pic_C_All.ClientSize.Height) break;
            }
            cls_C_All_Mark.i_Row_End = _iCurr_No;//下一行

            m_G_C_ALL.g.DrawLine(Ruler_p_One, new Point(Chart_Ruler_X_Start_C_All, Chart_Ruler_Y_Start), new Point(Chart_Ruler_X_Start_C_All, Pic_C_All.ClientSize.Height));

            #endregion 1.3 画纵轴刻度

            Rectangle _Rect_Kd = new Rectangle(0, 0, Pic_C_All.Width, Pic_C_All.Height);
            if (m_G_C_ALL.bg != null)
            {
                try
                {
                    m_G_C_ALL.gb.DrawImage(m_G_C_ALL.bg, _Rect_Kd);// 先绘制背景层
                }
#pragma warning disable CS0168 // 声明了变量“de”，但从未使用过
                catch (Exception de)
#pragma warning restore CS0168 // 声明了变量“de”，但从未使用过
                { }
            }
            m_G_C_ALL.gb.DrawImage(m_G_C_ALL.image, _Rect_Kd); // 再绘制绘画层
            try
            {
                Pic_C_All.BackgroundImage = (Bitmap)m_G_C_ALL.canvas.Clone(); // 设置为背景层，就是在内存中建立了一张对应的画布，重新refresh就是再次的加载这个画布
                Pic_C_All.Refresh();
            }
            catch { }
            #endregion 1 画布准备
            #endregion 刻度尺

            Application.DoEvents();
        }
        public int Plant_C_ALL_Mul(bool bl_R1_L0, int iScreenNo, int iRow_Start, int iRow_End,
                                   List<Cls_Mul_UI_Data> m_Lst_C_Mul_Buff, int iRomoteNum = 1)
        {
            int _iRet = 0;
            //检查是否有数据
            int _iAllNum = m_Lst_C_Mul_Buff.Count;
            if (_iAllNum == 0) return 2;

            //0 参数检查
            if (iRow_Start > iRow_End)
                iRow_Start = iRow_End;

            //if (iRow_Start < 1 || iRow_Start > _iAllNum || iRow_End < 1 || iRow_End > _iAllNum)
            //{
            //    _iRet = 3; return _iRet;
            //}
            if (i_Screen_No < 0)
            {
                _iRet = 4; return _iRet;
            }
            
            cls_C_All_Mark.i_Screen_No = iScreenNo;
            
            //2 界面初始化
            GetRulerPara_C_All_Mul();
            Plant_Ruler_C_ALL_Mul();
            Mul_Titl_Init(Scree_iAllRows_C);

            //1 参数初始化
            cls_C_All_Mark.bl_R1_L0 = bl_R1_L0;

            cls_C_All_Mark.fl_Dist_S = lst_Screenkd_C_ALL[cls_C_All_Mark.i_Screen_No].i_Start;
            cls_C_All_Mark.fl_Dist_E = lst_Screenkd_C_ALL[cls_C_All_Mark.i_Screen_No].i_End;
            cls_C_All_Mark.i_Row_Start = iRow_Start;//列表开始行
            cls_C_All_Mark.i_Row_End = iRow_End;//列表结束列
            cls_C_All_Mark.i_ScreenRows = Scree_iAllRows_C;

            //3 拿指定数据画当前界面
            int _iCol_Start = 0, _iCol_End = 0, _iX = 0, _i_Y = 0, _iCurr_Row = 1;
            int _iAllPiex = cls_C_All_Mark.i_ScreenRows * Scree_iAllRows_C - 1;
         //   m_Lst_C_All_Pro = new List<Cls_All_C_RowPro>();

            int _i_Jl_S = 0, _i_Jl_E = 0, _iCurr_Row_Num_Z = 0, _iCurr_Row_Num_F = 0;
#pragma warning disable CS0219 // 变量“_bl_Dd”已被赋值，但从未使用过它的值
            bool _bl_Dd = false;
#pragma warning restore CS0219 // 变量“_bl_Dd”已被赋值，但从未使用过它的值
            string _strGdbh = "";

            for (int _iRow = iRow_Start - 1; _iRow < iRow_End; _iRow++)
            {
                _iCurr_Row = _iRow - (iRow_Start - 1);//0-N
                _iCurr_Row_Num_Z = _iCurr_Row * iRomoteNum;
                //   _iCurr_Row_Num_F = (cls_C_All_Mark.i_ScreenRows -1 - _iCurr_Row) * iRomoteNum;
                _iCurr_Row_Num_F = cls_C_All_Mark.i_ScreenRows - 1 - _iCurr_Row * iRomoteNum;

                //3.1拿当前行数据
                List<Cls_Mul_ShowBuff> _lstRowRecord = m_Lst_C_Mul_Buff[_iRow].lst_Show_Buff;

                if (m_iLanguage == 0)
                    _strGdbh = "Number:" + m_Lst_C_Mul_Buff[_iRow].strGjbh + " Line:" + (_iRow + 1);// + "行" ;
                else
                    _strGdbh = "第" + m_Lst_C_Mul_Buff[_iRow].strGjbh + " Line:" + (_iRow + 1) + "行";

                //3.2 计算当前行当前屏幕的开始/结束位置
                _iCol_Start = Get_S_E_No_C_ALL(0, _lstRowRecord);
                _iCol_End = Get_S_E_No_C_ALL(1, _lstRowRecord);

                //3.3 刷新当前行、当前区域数据
                if (_iCol_Start > -1 && _iCol_End > -1)
                {
                    //拿当前屏幕开始/结束距离值
                    _i_Jl_S = _lstRowRecord[_iCol_Start].m_i_X;
                    _i_Jl_E = _lstRowRecord[_iCol_End].m_i_X;
                    _bl_Dd = false;
                    if (_i_Jl_S > _i_Jl_E)
                    {
                        _bl_Dd = true;
                        int _i_D = _i_Jl_S;
                        _i_Jl_S = _i_Jl_E;
                        _i_Jl_E = _i_D;
                    }
                    if (_iCol_Start > _iCol_End)
                    {
                        int _i_D = _iCol_Start;
                        _iCol_Start = _iCol_End;
                        _iCol_End = _i_D;
                    }

                    for (int _iCol = _iCol_Start; _iCol <= _iCol_End; _iCol++)
                    {
                        //3.4 拿各个通道数据
                        for (int _iR_C = 0; _iR_C < iRomoteNum; _iR_C++)
                        {
                            try
                            {
                                _iX = ((_lstRowRecord[_iCol].m_i_X - i_Screen_Start_Distance) / Scree_iDotWithmm_X);
                                Cls_EMAT_2 _C_Data = _lstRowRecord[_iCol].Arr_C_Data[_iR_C];

                                //3.5 计算Y位置
                                if (cls_C_All_Mark.bl_R1_L0)
                                    _i_Y = _iCurr_Row_Num_Z + _iR_C;
                                else
                                    _i_Y = _iCurr_Row_Num_F - _iR_C;

                                //3.6 画图
                                if (_C_Data.flThick >= 0)
                                    Plant_C_ALL(_iX, _i_Y, _C_Data, _strGdbh, (_iR_C + 1), 1, 1);
                            }
                            catch (Exception e1)
                            { }
                            Application.DoEvents();
                        }
                    }
                }
            }
            Rectangle _Rect = new Rectangle(0, 0, Pic_C_All.Width, Pic_C_All.Height);

            if (m_G_C_ALL.bg != null)
                m_G_C_ALL.gb.DrawImage(m_G_C_ALL.bg, _Rect);// 先绘制背景层

            m_G_C_ALL.gb.DrawImage(m_G_C_ALL.image, _Rect); // 再绘制绘画层
            Pic_C_All.BackgroundImage = (Bitmap)m_G_C_ALL.canvas.Clone();
            Application.DoEvents();
            //4 退出
            _iRet = 1;
            return _iRet;
        }
        /// <summary>
        /// 全C扫描界面，添加一个指定行/列的点的数据
        /// </summary>
        /// <param name="i_X"></param>
        /// <param name="i_Y"></param>
        /// <param name="Data"></param>
        /// <param name="iType"></param>
        private void Plant_C_ALL(int i_X, int i_Y, Cls_EMAT_2 Data, string strBh, int i_ABC, int iType = 0, int iAdd_1 = 1)
        {
            int _i_X_No = i_X;
            i_X = (int)(Chart_Ruler_X_Start_C_All + i_X * Scree_iDotWith_X) + (iAdd_1 == 1 ? 1 : 0);//由序号计算屏幕对应的像素位置
            float f_Y = (Chart_Ruler_Y_Start + (i_Y * cls_C_All_Mark.Scree_flDotHeight)) + (iAdd_1 == 1 ? 1 : 0);
            Color color = Color.FromArgb(255, Data.R, Data.G, Data.B);

            if (iType == 1) Mul_Titl_Add(i_Y, _i_X_No, Data, strBh, i_ABC);

            if (i_X <= Pic_C_All.Width && f_Y <= Pic_C_All.Height + 1)
                InitColor(m_G_C_ALL.g, i_X, f_Y, Scree_iDotWith_X, cls_C_All_Mark.Scree_flDotHeight, color);
        }
        private void Plant_C_ALL_Coat(int i_X, int i_Y, Cls_EMAT_2 Data, string strBh, int i_ABC, int iType = 0, int iAdd_1 = 1)
        {
            int _i_X_No = i_X;
            i_X = (int)(Chart_Ruler_X_Start_C_All + i_X * Scree_iDotWith_X) + (iAdd_1 == 1 ? 1 : 0);//由序号计算屏幕对应的像素位置
            float f_Y = (Chart_Ruler_Y_Start + (i_Y * cls_C_All_Mark.Scree_flDotHeight_Coat)) + (iAdd_1 == 1 ? 1 : 0);
            Color color = Color.FromArgb(255, Data.R, Data.G, Data.B);

            if (iType == 1) Mul_Titl_Add_Coat(i_Y, _i_X_No, Data, strBh, i_ABC);

            if (i_X <= Pic_Coat_All.Width && f_Y <= Pic_Coat_All.Height + 1)
                InitColor(m_G_Coat_ALL.g, i_X, f_Y, Scree_iDotWith_X, cls_C_All_Mark.Scree_flDotHeight_Coat/2, color);
        }

        #endregion 整体C扫描处理
        /// <summary>
        /// 画C扫图运行时的B扫（光栅臂）
        /// </summary>
        /// <param name="iLangege"></param>
        public void Plant_Ruler_B(int m_iLanguage = 0,int iDoEvents=0)
        {
            try
            {
                if (Pic_B == null) return;
                int i_X = 0, i_Y = 0;
                string strT = "";
                Chart_Clear(ref m_G_B);
                m_Scree_iDotWith_B = Pic_B.ClientSize.Width / Scree_iAllRows_C;
                int iLine_X = (int)(Chart_Ruler_X_Start + Scree_iAllRows_C * m_Scree_iDotWith_B);
                System.Windows.Forms.PictureBox.CheckForIllegalCrossThreadCalls = false;

                #region 画刻度尺
                #region 0 画布准备:初始化、右边出图、满屏时
                // 初始化画板，在内存中建立一块虚拟画布
                m_G_B.image = new Bitmap(Pic_B.ClientSize.Width, Pic_B.ClientSize.Height);
                // 获取背景层
                m_G_B.bg = (Bitmap)Pic_B.BackgroundImage;
                // 初始化整个画布
                m_G_B.canvas = new Bitmap(Pic_B.ClientSize.Width, Pic_B.ClientSize.Height);
                // 初始化图形面板，获取这块内存画布的Graphics的引用
                m_G_B.g = Graphics.FromImage(m_G_B.image);
                m_G_B.gb = Graphics.FromImage(m_G_B.canvas);
                m_G_B.g.Clear(Color.White);
                m_G_B.Buff = new PointF[0];
                float iLeft = Ruler_p_One.Width;
                int iTop = Pic_B.Top;

                int i_S_H = 0, i_S_L = 0;
                float _flEndKd = 0f;
                #endregion 0 画布准备

                #region 1 画横轴刻度
                i_S_H = Chart_Ruler_Y_Start - 6; i_S_L = Chart_Ruler_Y_Start - 3;
                int _iGs = 5;
                int _iJg_C = (int)Scree_iDotHeightmm_Y;// int.Parse(Scree_iDotHeightmm_Y.ToString());// * 1000f
                for (int i = 1; i <= Scree_iAllRows_C; i++)//和C扫描（单行）相同光栅臂距离
                {
                    i_X = (int)(Chart_Ruler_X_Start + i * m_Scree_iDotWith_B);
                    _flEndKd = (i * Scree_iDotHeightmm_Y);

                    if (i % 5 != 0)
                        m_G_B.g.DrawLine(Ruler_p_One, new Point(i_X, i_S_L), new Point(i_X, Chart_Ruler_Y_Start));
                    else
                        m_G_B.g.DrawLine(Ruler_p_One, new Point(i_X, i_S_H), new Point(i_X, Chart_Ruler_Y_Start));
                    if (i % _iGs == 0)
                    {
                        #region 标记横轴刻度值
                        //if (_blZt_mm == false)
                        //{ drawFont = new Font("Arial", (float)10); _blZt_mm = true; }
                        //else
                        drawFont = new Font("黑体", (float)7.9, FontStyle.Bold);
                        strT = _flEndKd.ToString("f0") + (i == _iGs ? "m m" : "");

                        PointF drawPoint = new PointF(i_X - 15, Chart_Ruler_Word_H);//- Chart_Ruler_Word_X
                        m_G_B.g.DrawString(strT, drawFont, drawBrush, drawPoint);
                        #endregion 标记横轴刻度值
                    }
                    if (i_X > Pic_B.ClientSize.Width)
                        break;
                }

                m_G_B.g.DrawLine(Ruler_p_One, new Point(Chart_Ruler_X_Start, Chart_Ruler_Y_Start), new Point(iLine_X, Chart_Ruler_Y_Start));

                //  Font drawFont_t = new Font("Arial", (float)Chart_Ruler_font);
                m_G_B.g.DrawString(m_iLanguage == 0 ? "距离" : "Dist", drawFont, drawBrush, -2, 1);
                #endregion 1 画横轴刻度

                #region  2 画纵轴刻度
                StringFormat StrF = new StringFormat();
                StrF.FormatFlags = StringFormatFlags.DirectionVertical;// StringFormatFlags.DisplayFormatControl ; // 竖排
                int iLeft_H = Chart_Ruler_X_Start - (Chart_Ruler_X_Start > 16 ? 16 : Chart_Ruler_X_Start) + 1;

                i_S_H = Chart_Ruler_X_Start - 6; i_S_L = Chart_Ruler_X_Start - 3;
                float _flTmp = 0;
                int iScreen_Height = Pic_B.Height - Chart_Ruler_Y_Start -(int) Scree_iDotHeight;
                float _flMax = (flNormal_Thickness + Scree_iDotHeight_B_Y_Addmm);
                int i_Dat = 0;
                m_Scree_fl_DotHeight_B = iScreen_Height / _flMax;
                float _fl_HdVal_Jg = _flMax / 7f;
                string _strTitl = "";
                int _iJgNum = 5;
                int _iNum = 6;
                for (int iKd = 0; iKd < 200; iKd += 5)
                {
                    #region 2.1画一条数据纵轴刻度
                    for (int i = 1; i < _iNum; i++)
                    {
                        i_Dat = (iKd + i);
                        _flTmp = Chart_Ruler_Y_Start + Scree_iDotHeight + m_Scree_fl_DotHeight_B * i_Dat;
                        i_Y = (int)_flTmp;
                        i_Y = i_Y < _flTmp ? ++i_Y : i_Y;

                        if (i_Y > iScreen_Height) break;

                        _strTitl = i_Dat.ToString();
                        if (i % _iJgNum != 0)
                        {
                            m_G_B.g.DrawLine(Ruler_p_One, new Point(i_S_L, i_Y), new Point(Chart_Ruler_X_Start, i_Y));
                            if (i_Dat == 1)
                                m_G_B.g.DrawString((iKd == 0 && i == 1 ? "m m" : ""), drawFont, drawBrush, iLeft_H - 2, i_Y - 1, StrF);
                        }
                        else
                        {
                            m_G_B.g.DrawLine(Ruler_p_One, new Point(i_S_H, i_Y), new Point(Chart_Ruler_X_Start, i_Y));
                            m_G_B.g.DrawString(_strTitl, drawFont, drawBrush, (iLeft_H - 5 + (i_Dat <= 10 ? 1 : -1)), i_Y - (i_Dat <= 10 ? 8 : 4), StrF);
                        }
                    }
                    if (i_Y > iScreen_Height) break;
                    #endregion 2.1
                }
                m_G_B.g.DrawLine(Ruler_p_One, new Point(Chart_Ruler_X_Start, Chart_Ruler_Y_Start), new Point(Chart_Ruler_X_Start, Pic_B.ClientSize.Height));
                StrF.FormatFlags = StringFormatFlags.DirectionVertical;
                m_G_B.g.DrawString(m_iLanguage == 0 ? "厚度" : "Thick", drawFont, drawBrush, -2, 14, StrF);
                #endregion 2 画纵轴刻度

                #region 3 报警线
                if (Ck_No_Normal_Thickness)
                {
                    _flTmp = Chart_Ruler_Y_Start + Scree_iDotHeight + m_Scree_fl_DotHeight_B * (flNormal_Thickness - flstrThickAlarm);
                    i_Y = (int)_flTmp;
                    i_Y = i_Y < _flTmp ? ++i_Y : i_Y;
                    m_G_B.g.DrawLine(Ruler_p_Limit, new Point(Chart_Ruler_X_Start, i_Y), new Point(iLine_X, i_Y));

                    _flTmp = Chart_Ruler_Y_Start + Scree_iDotHeight + m_Scree_fl_DotHeight_B * (flNormal_Thickness);
                    i_Y = (int)_flTmp;
                    i_Y = i_Y < _flTmp ? ++i_Y : i_Y;
                    Pen Ruler_p_Limit_G = new Pen(Brushes.Green);
                    m_G_B.g.DrawLine(Ruler_p_Limit_G, new Point(Chart_Ruler_X_Start, i_Y), new Point(iLine_X, i_Y));
                }
                #endregion 3
                #region 3 刷新
                Rectangle _Rect_Kd = new Rectangle(0, 0, Pic_B.Width, Pic_B.Height);
                if (m_G_B.bg != null)
                {
                    try
                    {
                        m_G_B.gb.DrawImage(m_G_B.bg, _Rect_Kd);// 先绘制背景层
                    }
#pragma warning disable CS0168 // 声明了变量“de”，但从未使用过
                    catch (Exception de)
#pragma warning restore CS0168 // 声明了变量“de”，但从未使用过
                    { }
                }
                m_G_B.gb.DrawImage(m_G_B.image, _Rect_Kd); // 再绘制绘画层
                System.Windows.Forms.PictureBox.CheckForIllegalCrossThreadCalls = false;
                try
                {
                    Pic_B.BackgroundImage = (Bitmap)m_G_B.canvas.Clone(); // 设置为背景层，就是在内存中建立了一张对应的画布，重新refresh就是再次的加载这个画布
                    Pic_B.Refresh();
                }
#pragma warning disable CS0168 // 声明了变量“e2”，但从未使用过
                catch (Exception e2)
#pragma warning restore CS0168 // 声明了变量“e2”，但从未使用过
                { }
                #endregion 3
                #endregion 刻度尺
                if(iDoEvents==0)
                Application.DoEvents();
            }
            catch { }
        }
        /// <summary>
        /// B扫描：实时数据
        /// </summary>
        /// <param name="i_X">光栅臂显示缓存位置序号</param>
        /// <param name="Data"></param>
        public void Plant_B(int i_X, Cls_EMAT_2 Data, int iType = 1)
        {
            if (Data.flThick < 0) return;

            i_X = (int)(Chart_Ruler_X_Start + i_X * m_Scree_iDotWith_B) + 1;//由序号计算屏幕对应的像素位置
            int i_Y = (int)(Chart_Ruler_Y_Start + Scree_iDotHeight);
            Color color = Color.FromArgb(255, Data.R, Data.G, Data.B);

            if (i_X <= Pic_B.Width && i_Y <= Pic_B.Height)
            {
                InitColor(m_G_B.g, i_X, i_Y, m_Scree_iDotWith_B, Data.flThick * m_Scree_fl_DotHeight_B, color);

                if (iType == 1)
                {
                    Rectangle _Rect = new Rectangle(0, 0, Pic_B.Width, Pic_B.Height);

                    if (m_G_B.bg != null)
                        m_G_B.gb.DrawImage(m_G_B.bg, _Rect);// 先绘制背景层

                    m_G_B.gb.DrawImage(m_G_B.image, _Rect); // 再绘制绘画层
                    Pic_B.BackgroundImage = (Bitmap)m_G_B.canvas.Clone();
                }
            }
        }
        public void Plant_B_Brush()
        {
            Rectangle _Rect = new Rectangle(0, 0, Pic_B.Width, Pic_B.Height);

            if (m_G_B.bg != null)
                m_G_B.gb.DrawImage(m_G_B.bg, _Rect);// 先绘制背景层

            m_G_B.gb.DrawImage(m_G_B.image, _Rect); // 再绘制绘画层
            Pic_B.BackgroundImage = (Bitmap)m_G_B.canvas.Clone();
        }

        /// <summary>
        /// 告警数据初始化
        /// </summary>
        public void Alarm_Init()
        {
            if (flNormal_Thickness <= 0) return;
            float _flBfz = 0;
            //1 获得百分值
            try
            {
          //      strWc_Bfz = "20";
                _flBfz = float.Parse(strWc_Bfz);
            }
            catch { strWc_Bfz = "20"; }

            //2 误差厚度
            flstrThickAlarm = flNormal_Thickness * _flBfz / 100;

            //3 色标个数   
            if (iRad_Dw == 1)
                strWc_Fbl = (1f / 32f / 10f).ToString();
            else
                strWc_Fbl = "0.01";
            float _flfbl = float.Parse(strWc_Fbl);


            //4 色标个数
            int iSbgs = (int)(flstrThickAlarm / _flfbl);
            Txt_Wc_Num = iSbgs.ToString();

            Write_One("strWc_Bfz", strWc_Bfz.ToString());
           Write_One("Txt_Wc_Num", iSbgs.ToString());
        }

        /// <summary>
        /// 画色块
        /// </summary>
        /// <param name="G">画布</param>
        /// <param name="intStar_X">起点X</param>
        /// <param name="intStar_Y">起点Y</param>
        /// <param name="iWith">宽度</param>
        /// <param name="iHeight">高度</param>
        /// <param name="color">颜色</param>
        public void InitColor(Graphics G, int intStar_X, int intStar_Y, int iWith, int iHeight, Color color)
        {
            SolidBrush mybrush = new SolidBrush(color);
            try
            {
                if (G != null)
                    G.FillRectangle(mybrush, intStar_X, intStar_Y, iWith, iHeight);
            }
#pragma warning disable CS0168 // 声明了变量“e”，但从未使用过
            catch (Exception e)
#pragma warning restore CS0168 // 声明了变量“e”，但从未使用过
            { }
        }
        /// <summary>
        /// 画色块
        /// </summary>
        /// <param name="G">画布</param>
        /// <param name="flStar_X">起点X</param>
        /// <param name="flStar_Y">起点Y</param>
        /// <param name="iWith">宽度</param>
        /// <param name="iHeight">高度</param>
        /// <param name="color">颜色</param>
        public void InitColor(Graphics G, float flStar_X, float flStar_Y, float flWith, float flHeight, Color color)
        {
            SolidBrush mybrush = new SolidBrush(color);
            try
            {
                if (G != null)
                    G.FillRectangle(mybrush, flStar_X, flStar_Y, flWith, flHeight);
            }
#pragma warning disable CS0168 // 声明了变量“e”，但从未使用过
            catch (Exception e)
#pragma warning restore CS0168 // 声明了变量“e”，但从未使用过
            { }
        }
    }
    public struct ColorRange
    {
        /// <summary>
        /// 最大厚度误差 方案中存储方式：从小到大排列
        /// </summary>
        public float Max_Limit;
        /// <summary>
        /// 色彩R
        /// </summary>
        public int R;
        /// <summary>
        /// 色彩G
        /// </summary>
        public int G;
        /// <summary>
        /// 色彩B
        /// </summary>
        public int B;
    }
    #endregion C扫描
    /// <summary>
    /// TOFD类缓存
    /// </summary>
    public class ClassTofd_Buff
    {
        public int Txt_Wc_Num = 0;
        /// <summary>
        /// Tofd控件
        /// </summary>
        public Tofd m_Tofd_DLL = new Tofd();
        /// <summary>
        /// 距离信息
        /// </summary>
        public ClassLib_TestData.clStreamVideo m_Climb = new ClassLib_TestData.clStreamVideo();

        /// <summary>
        /// 测量原始记录
        /// </summary>
        public Class_Test_Records m_Test_Record = new Class_Test_Records();
        /// <summary>
        /// 数据列表,检测完成保存数据库
        /// </summary>
      //  public  List<Class_Test_Records> m_Lst_Test_Record = new List<Class_Test_Records>();
        /// <summary>
        /// 打标 报警数据
        /// </summary>
        public List<Class_Test_AlarmArea> m_Lst_Mark_Record = new List<Class_Test_AlarmArea>();
    }
    public class FrameTime
    {

        /// <summary>
        /// 帧序号
        /// </summary>
        public int iFrameNo;
        /// <summary>
        /// 时间ID DateTime.Now.ToString("HHmmssf")
        /// </summary>
        public string strTimeID;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]//设定对齐粒度为一个字节
    [System.Serializable]
    public struct SEmatChanParam
    {
        /// <summary>
        /// 8 增益 0-1100，显示0-110.0dB   
        /// </summary>
        public Int32 m_idB;
        /// <summary>
        /// 7 范围 10-1000mm   
        /// </summary>
        public Int32 m_iRange;
        /// <summary>
        /// 9 零偏 0-100us    
        /// </summary>
        public Int32 m_iZeroTime;
        /// <summary>
        /// 10 平移 0-100us     
        /// </summary>
        public Int32 m_iParallelTime;

        /// <summary>
        ///11  脉冲个数 1-101，显示(data-1)*5ns
        /// </summary>
        public System.UInt16 m_iPulWidthCode;
        /// <summary>
        /// 1 检波方式	正/负/射/全         
        /// </summary>
        public System.Byte m_iDemodulation_Flag;
        /// <summary>
        /// 重复频率 15/30/60/100/200/300/400/500HZ
        /// </summary>
        public System.Byte m_iRepeatFreq;           //13 重复频率 15/30/60/100/200/300/400/500HZ    


        /// <summary>
        /// 工作模式 自发自收/一发一收   
        /// </summary>
        public System.Byte m_iWorkMode;         // 4 工作模式 自发自收/一发一收   
        /// <summary>
        ///  2 带宽选择2-8M、0.5-4M、1-30M和5-15M 
        /// </summary>
        public System.Byte m_iSecBandWidthF;        // 2 带宽选择 2-8M、0.5-4M、1-30M和5-15M
        /// <summary>
        /// 3 阻抗匹配 48/500欧       
        /// </summary>
        public System.Byte m_iImpedanceF;           // 3 阻抗匹配 48/500欧       
        /// <summary>
        /// 12 高压调节 400/200/300V  
        /// </summary>
        public System.Byte m_iVolt;             // 12 高压调节 400/200/300V  
        /// <summary>
        /// 声速
        /// </summary>
        public int m_dSpeed;               // 6 声速   
        /// <summary>
        /// 前放开关
        /// </summary>
        public System.Byte m_iForword;              //5 前放开关    
         /// <summary>
        /// 0-类型一，1-类型2
        /// </summary>
        public byte m_iPcsType;
        /// <summary>
        /// 0-平板，1-圆弧外壁，2-圆弧内壁
        /// </summary>
        public byte m_iPcsMode;     
         /// <summary>
        /// 定位方式：0-模拟，1-编码器 2：测试
        /// </summary>
        public byte m_BScanMode;
        /// <summary>
        /// 当前编码器：0-A，1-B
        /// </summary>
        public byte m_iCurEn;
        /// <summary>
        /// 编码器方向：0-反向，1-正向
        /// </summary>
        public byte m_iEnPos;
        /// <summary>
        /// 相关---
        /// </summary>
        public byte m_iInterfix;
        /// <summary>
        /// 通道是能---
        /// </summary>
        public byte m_iChanInit;
        /// <summary>
        /// 脉冲使能
        /// </summary>
        public System.Byte m_iPulEnable;        
        /// <summary>
        /// 高压使能
        /// </summary>
        public System.Byte m_iVoltEnable;//
        /// <summary>
        /// 扫查是否有效---
        /// </summary>
        public byte m_iSActive;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 307)]//
        public System.Byte[] u8bk;//=new byte[318];//371
        /// <summary>
        /// 分析线-------
        /// </summary>
        public System.UInt16 m_iPcsLt;
        /// <summary>
        /// 探头前沿------
        /// </summary>
        public float m_probefront;
        /// <summary>
        /// Tofd记录长度
        /// </summary>
        public int m_BRecordLen;

        public System.UInt16 m_iPcsLW;
        public System.UInt16 m_iPcsBW;

        /// <summary>
        /// 探头中心间距
        /// </summary>
        public float m_fPcsLen;
        /// <summary>
        /// 弧长
        /// </summary>
        public float m_fPcsArc;
        /// <summary>
        /// 弦高
        /// </summary>
        public float m_fPcsChord;

        /// <summary>
        /// 外壁的外径或内壁的内径
        /// </summary>
        public float m_fPcsDia;
        /// <summary>
        /// 楔块角度
        /// </summary>
        public float m_fPcsAngle;
        /// <summary>
        /// 分层起点
        /// </summary>
        public float m_fPcsStart;
        /// <summary>
        /// 分层终点
        /// </summary>
        public float m_fPcsEnd;
        /// <summary>
        /// 步进精度
        /// </summary>
        public float m_fEnStep;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        /// <summary>
        /// 编码器精度 0:A编码器 1：B编码器
        /// </summary>
        public float[] m_fEnRatio;

        //---------------------------
        ///// <summary>
        ///// 编码器读值
        ///// </summary>
        //public int [] m_iEnPul;        
        ///// <summary>
        ///// 实际位置
        ///// </summary>
        //public float [] m_fEnReal;
        ///// <summary>
        ///// 楔块探头延时时间 2t0
        ///// </summary>
        //public float T0;
        ///// <summary>
        ///// 直通波在楔块运行时间 L0
        ///// </summary>
        //public float L0;

        ///// <summary>
        ///// 直通波在楔块距离
        ///// </summary>
        //public float L0_Distan;
    }
    public struct Emat_Real
    {
        /// <summary>
        /// 编码器读值
        /// </summary>
        public int[] m_iEnPul;
        /// <summary>
        /// 实际位置
        /// </summary>
        public float[] m_fEnReal;
        /// <summary>
        /// 楔块探头延时时间 2t0
        /// </summary>
        public float T0;
        /// <summary>
        /// 直通波在楔块运行时间 L0
        /// </summary>
        public float L0;

        /// <summary>
        /// 直通波在楔块距离
        /// </summary>
        public float L0_Distan;
    }
    /// <summary>
    /// 
    /// </summary>
    public class CL_BiaoZ_S
    {
        /// <summary>
        /// 数据类型0：长度 1：高度 2：深度
        /// </summary>
        public int iDataType = 0;
        /// <summary>
        /// 0:开始  1：结束
        /// </summary>
        public int i_S0_E1 = 0;
    }
    /// <summary>
    /// 标注数据类型
    /// </summary>
    public class ClBiaoZhu
    {
        /// <summary>
        /// 标注数据类型，0：长度 1：高度  2：深度
        /// </summary>
        public int iSelectAddType = -1;
        /// <summary>
        /// 屏幕序号
        /// 此数据由当前屏幕实时计算，
        /// 保存后再现时也按照实时计算方法
        /// </summary>
        public int iScreenNo = 0;
        /// <summary>
        /// D图X轴位置序号
        /// </summary>
        public int iX_No = 0;
        /// <summary>
        /// D图Y轴位置序号
        /// </summary>
        public int iY_No = 0;

        /// <summary>
        /// 长度开始距离
        /// </summary>
        public float flLen_S = 0f;
        /// <summary>
        /// 长度结束距离
        /// </summary>
        public float flLen_E = 0f;
        /// <summary>
        /// 长度
        /// </summary>
        public float flLen = 0;

        /// <summary>
        /// 高度开始时间
        /// </summary>
        public float flHeight_S = 0f;
        /// <summary>
        /// 高度结束时间
        /// </summary>
        public float flHeight_E = 0f;
        /// <summary>
        /// 高度
        /// </summary>
        public float flHeight = 0;

        /// <summary>
        /// 深度开始时间
        /// </summary>
        public float flDepth_S = 0f;
        /// <summary>
        /// 深度
        /// </summary>
        public float flDepth = 0;

        /// <summary>
        /// 直通波屏幕位置
        /// </summary>
        public int i_LW_X = 0;
        /// <summary>
        /// 直通波的全部时间
        /// </summary>
        public float flTLW_us = 0;
        /// <summary>
        /// 直通波的全部距离
        /// </summary>
        public float flTLW_mm = -1;

        /// <summary>
        /// 底波屏幕位置
        /// </summary>
        public int i_BW_X = 0;
        /// <summary>
        /// 底波的全部时间
        /// </summary>
        public float flTBW_us = 0;
        /// <summary>
        /// 底波的全部距离
        /// </summary>
        public float flTBW_mm = 0;
    }
    /// <summary>
    /// TOFD数据显示
    /// </summary>
    public class Tofd_Show_Data
    {
        public Tofd_Show_Data(int iType, int iDataLenNum = 3000)
        {
            if (iType == 0)//武汉中科
            {
                m_ArrWave = new string[UTS_DATA_WIDTH];
                m_pChannelBuf = new byte[UTS_DATA_WIDTH];
                m_pValueBuf = new byte[UTS_DATA_WIDTH];
                m_pTimeBuf = new int[UTS_DATA_WIDTH];
            }
            else if (iType == 1)//北京探头
            {
                m_ArrWave = new string[iDataLenNum];
                m_pChannelBuf = new byte[iDataLenNum];
                m_pValueBuf = new byte[iDataLenNum];
                m_pTimeBuf = new int[iDataLenNum];
            }
        }
        /// <summary>
        /// 当前距离
        /// </summary>
        public float m_flDistanc_X = 0;
        /// <summary>
        /// 显示宽度，总采样点数 512
        /// </summary>
        public const int UTS_DATA_WIDTH = 512;
        ///// <summary>
        ///// 正/负/全检波数据
        ///// </summary>
        //public PointF[] m_points = new PointF[UTS_DATA_WIDTH];
        ///// <summary>
        ///// 射频波数据
        ///// </summary>
        //public PointF[] m_points_2 = new PointF[UTS_DATA_WIDTH * 2];
        /// <summary>
        /// 波形数
        /// </summary>
        public string[] m_ArrWave = new string[UTS_DATA_WIDTH];


        /// <summary>
        /// 波形数据
        /// </summary>
        public byte[] m_pChannelBuf = new byte[UTS_DATA_WIDTH];
        /// <summary>
        /// 波谷序列
        /// </summary>
        public byte[] m_pValueBuf = new byte[UTS_DATA_WIDTH];

        /// <summary>
        /// 波峰序列每个值对应的时间序列，每个值乘以10ns为时间的采样时间
        /// </summary>
        public int[] m_pTimeBuf = new int[UTS_DATA_WIDTH];
    }
    /// <summary>
    /// 闸门计算波峰
    /// </summary>
    public class PeakDat
    {
        /// <summary>
        /// 当前开始点对应缓存位置
        /// </summary>
        public int i_Gate_S = 0;
        /// <summary>
        /// 开始
        /// </summary>
        public int i_S_X = 0;
        /// <summary>
        /// 结束
        /// </summary>
        public int i_E_X = 0;
        /// <summary>
        /// 高度
        /// </summary>
        public int i_Y = 0;
        /// <summary>
        /// 当前结束点对应缓存位置
        /// </summary>
        public int i_Gate_E = 0;
        /// <summary>
        /// 之前波峰走势 1：升  -1：降
        /// </summary>
        public int i_K = 0;
        /// <summary>
        /// 当前闸门波峰位置序号
        /// </summary>
        public int i_Gate_P = -1;
        /// <summary>
        /// 波峰值
        /// </summary>
        public float fl_Peak_Data = -999;
    }
    /// <summary>
    /// TOFD控制类
    /// </summary>
    public class Tofd
    {
        #region TOFD的Dll调用
        /// <summary>
        /// 相关个数
        /// </summary>
        public int m_i_XiangGuan_Num = 4;
        /// <summary>
        /// 是否相关
        /// </summary>
        public bool m_bl_XiangGuan = false;
        /// <summary>
        /// 是否平均
        /// </summary>
        public bool m_bl_PingJun = false;
        /// <summary>
        /// 厚度值
        /// </summary>
        public float m_flThick = 0;

        /// <summary>
        /// 写激光日志
        /// </summary>
        public bool m_bl_Xj_Write = false;
        /// <summary>
        /// 超声探头类型 0:武汉中科  1：北京
        /// </summary>
        public int i_Ul_Probe_Type = 0;

        public string m_strLinkMsg = "";
        /// <summary>
        /// 闸门类型  1:1个闸门  2:2个闸门  0：TOFD,不使用闸门
        /// </summary>
        public int m_Data_Type = 1;
        /// <summary>
        /// 闸门1
        /// </summary>
        public PeakDat m_Gate_1 = new PeakDat();
        /// <summary>
        /// 闸门2
        /// </summary>
        public PeakDat m_Gate_2 = new PeakDat();

        /// <summary>
        /// 0:暂停  1：继续
        /// </summary>
        public int m_iRun_State = 0;
        /// <summary>
        /// 已经运行时长
        /// </summary>
        public int m_iHaveRunTime = 0;
        /// <summary>
        /// 联机是否成功
        /// </summary>
        public bool blNetLink = false;
        /// <summary>
        /// 0:正常运行  1：校准
        /// </summary>
        public int m_i_State = 0;

        /// <summary>
        /// 当时数据是否有异常
        /// </summary>
        public bool blAlarm = false;
        /// <summary>
        /// 报文数据 波形类型 | 波形数据,','间隔512个数据
        /// </summary>
        public string strWave = "";
        /// <summary>
        /// 报文数据 波形类型 | 波形数据,','间隔512个数据
        /// </summary>
        public string strWave_2 = "";
        /// <summary>
        /// 采样时间 波峰序列每个值对应的时间序列，每个值乘以10ns为时间的采样时间
        /// </summary>
        public string strWave_Time = "";
        /// <summary>
        /// 数据处理循环间隔时间，单位ms
        /// </summary>
        public int g_iWaitTime = 10;
        /// <summary>
        /// 数据处理循环间隔时间，单位ms
        /// </summary>
     //   public float flWaitTime = 10f;
        /// <summary>
        /// 通道数量
        /// </summary>
        public const int HSD_CLIENT = 1;
        /// <summary>
        /// 探头连接 true:成功 false:失败
        /// </summary>
        public bool[] bl_ArrNetTrue = new bool[HSD_CLIENT];

        /// <summary>
        /// 工艺文件夹名称
        /// </summary>
        public string m_strSection = "";
        /// <summary>
        /// 通讯参数
        /// </summary>
        public SEmatChanParam[] m_pSparam;
        /// <summary>
        /// 4轮车距离校准系数
        /// </summary>
        public float  fl4Car_JzXs = 1.0f;
        /// <summary>
        /// 实际读数
        /// </summary>
        public Emat_Real[] m_pSparam_Real;
        /// <summary>
        /// 全数据最大显示范围
        /// </summary>
        public int[] m_iMinRange = new int[CHAN_OF_CLIENT];
        /// <summary>
        /// 全数据最大显示范围
        /// </summary>
        public int[] m_iMAxRange = new int[CHAN_OF_CLIENT];
        /// <summary>
        /// 联机状态
        /// </summary>
        public bool[] m_connectStatus = new bool[HSD_CLIENT];
        /// <summary>
        /// 显示宽度，总采样点数 512
        /// </summary>
        public const int UTS_DATA_WIDTH = 512;
        /// <summary>
        /// 一个格子的时间间隔
        /// </summary>
        public float m_fl_Time_JG = 0;
        /// <summary>
        /// 显示高度
        /// </summary>
        public const int UTS_DATA_HEIGHT = 255;
        /// <summary>
        /// 波形图后面的格子的总数
        /// </summary>
        public const int CHAN_BACK_LINE = 10;
        /// <summary>
        /// 
        /// </summary>
        public const int TEXTLENGTH = 40;
        /*
         TOFD检测模块动态库接口文档
        整体调用流程：
        1.网络连接
        InitModule->CreateHostSocket->GetConnectStatus
        2.通道数据下发(N个通道就循环下发N次，该模块只有一个通道)
        SendCmdCurrentChan->SendCmdDB->SendCmdFreqRatio->
        SendCmdZeroTime->SendCmdParallel->SendCmdPulWid->
        SendCmdWaveType->SendCmdRepeatFreq->SendCmdWorkMode->
        SendCmdBandWidth ->SendCmdImpdance->SendCmdHighVoltage->
        SendCmdForword
        3.	全局数据下发 InitEncoder
        4.	设置当前通道，设置波形数据指针，开启采样
        SendCmdCurrentChan(curChan)-> RealWave->SendCmdSampleStart
        5.关闭程序ExitModule

        关于初始化参数问题，InitParam()初始化了链接库的所有参数，可以在调用网络相关函数之前调用；
        InitSendParam()包含所有初始参数的发码，连接成功后可以调用。硬件初始化就全部处理了，此时可
        以只处理自己想要改变的硬件发码。后文对各个函数详细说明。
         */

        #region  
        [DllImport("NetModulDll.dll", EntryPoint = "InitModule", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool InitModule(System.UInt32 ChannelSum, System.UInt32 BoardSum);
        /*
         //初始化模块    1.	BOOL		InitModule(ULONG ChannelSum, ULONG BoardSum);
        ChannelSum：通道数，此模块通道数为1
        BoardSum：客户端数，此模块客户端数为1
        说明：
        ①判断ChannelSum和BoardSum是否都不为0，否则返回false;判断ChannelSum和BoardSum是否超过最大通道数和最大客户端数，超过则使用最大值;
        ②初始化每个模块连接状态和客户端套接字;
        ③初始化线程句柄，创建接收数据的线程;
         */
        #endregion

        #region  2 创建网络连接
        [DllImport("NetModulDll.dll", EntryPoint = "CreateHostSocket", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CreateHostSocket();
        /*
       //本机IP:192.168.1.240    PORT 8765   创建网络连接  2.	BOOL 		CreateHostSocket(); 
        说明：
        ①创建连接套接字，设置服务器地址信息，端口号为8765，绑定服务器地址和端口，
        失败返回false，对服务器的socket进行监听，失败返回false，全部成功则返回true。
         */
        #endregion 2

        #region  3 监听线程中调用    3.	BOOL  	GetConnectStatus(USHORT* pConnectNo); 
        [DllImport("NetModulDll.dll", EntryPoint = "GetConnectStatus", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetConnectStatus(System.UInt16[] pConnectNo);
        /*
         pConnectNo = 最大模块数+1；
        说明：传入模块数量，监听服务器套接字socket，有新的连接连入保存为新的客户端套接字，获取客户端的IP地址，如果包含在客户机地址列表(见文档末尾)中则保存当前客户端套接字及连接状态，客户端连接数++，pConnectNo指针指向的值为当前客户端的index,判断客户端连接数是否等于客户端数，如果相等则代表所有指定客户端都已连接，停止监听线程;
        上层判断pConnectNo >= 0 && pConnectNo < 客户端数,则关闭等待连接窗口进入主界面;
        网络连接成功后，接下来就是初始化每个通道数据并下发给仪器获取初始化波形数据。
         */
        #endregion 3

        #region  4 说明：退出程序时调用，关闭动态库中相关接口，返回true
        [DllImport("NetModulDll.dll", EntryPoint = "ExitModule", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ExitModule();
        #endregion 4

        #region  5 说明：初始化全局参数，可在调用网络连接相关函数之前调用
        [DllImport("NetModulDll.dll", EntryPoint = "InitParam", CallingConvention = CallingConvention.Cdecl)]
        public static extern void InitParam();
        #endregion 5

        #region  6 说明：初始化硬件发码，可在网络连接成功后调用
        [DllImport("NetModulDll.dll", EntryPoint = "InitSendParam", CallingConvention = CallingConvention.Cdecl)]
        public static extern void InitSendParam();
        #endregion 6

        #region 7 开启采样
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdSampleStart", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdSampleStart();
        #endregion 7

        #region  8  关闭采样
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdSampleClose", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdSampleClose();
        #endregion 8

        #region  9 关闭服务器
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdServerClose", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdServerClose();
        #endregion 9

        #region  10 当前通道发码
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdCurrentChan", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdCurrentChan(int iData);
        /*
        iChan：range(0)，显示为iChan+1;
        说明：切换当前要显示的通道，此模块只有一个通道，该值为0
        */
        #endregion 10

        #region  11 增益控制发码
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdDB", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdDB(int iData);
        /*
        iData: range(0,1100) ，界面单位dB，值=iData/10;
        说明：设置当前通道增益，默认300，即30.0dB
        */
        #endregion

        #region   12 分频比发码
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdFreqRatio", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdFreqRatio(int iRange, int iSpeed);
        /*
        iRange: 范围，range(10,1000)，单位mm ;
        iSpeed:声速，range(2500,3500)，单位m/s;
        说明：设置当前通道分频比，是范围和声速对应的发码，
        默认200mm、3240m/s
        */
        #endregion 12

        #region  13 零偏发码
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdZeroTime", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdZeroTime(int iData);
        /*
        iData: 延时, range(0,3000)，界面单位us，值=iData/100;
        说明：设置当前通道零偏，默认0
        */
        #endregion 13

        #region  14 平移发码
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdParallel", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdParallel(int iData);
        /*
        iData: 延时, range(0,3000)，界面单位us，值=iData/100;
        说明：设置当前通道平移，默认0
        */
        #endregion

        #region  15 脉冲宽度发码

        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdPulEnable", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdPulEnable(int iData);//脉冲使能
                                                              //iData: 为1，脉冲宽度发麻之前发

        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdPulEnable", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdVoltEnable(int iData);//高压使能
                                                               //iData: 为1，高压调节发码之前发

        [DllImport("NetModulDll.dll", EntryPoint = "SaveTbuf", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SaveTbuf(int iData);



        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdPulWid", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdPulWid(int iData);
        /*
        iData: range(1,101)  界面显示值=(iData-1)*5，单位ns;
        说明：设置当前通道脉冲宽度，默认80
        */
        #endregion 15

        #region   16 检波方式发码
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdWaveType", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdWaveType(int iData);
        /*
         iData: range(0,3)  0-3分别表示正检波，负检波，射频波，全检波
        说明：设置当前通道检波方式，默认3
        */
        #endregion 16

        #region  17 重复频率发码
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdRepeatFreq", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdRepeatFreq(int iData);
        /*
        iData：range(0,7)  0-8分别代表15Hz，30Hz，60Hz，100Hz，200Hz，300Hz，400Hz，500Hz，1kHz
        说明:设置全局重复频率，默认2
        */
        #endregion 17


        #region  18 工作模式发码
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdWorkMode", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdWorkMode(int iData);
        /*
         iData：range(0,1)，0代表自发自收，1代表一发一收
        说明:设置工作模式，默认0
        */
        #endregion 18
        #region 19   带宽发码
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdBandWidth", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdBandWidth(int iData);
        /*
        iData：range(0,3) ，0-3分别表示2-8M、1-30M、0.5-4M和5-15M
        说明:设置带宽，默认0
        */
        #endregion 19

        #region 20 阻抗发码
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdImpdance", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdImpdance(int iData);
        /*
        iData：range(0,1)，0代表48欧，1代表500欧
        说明:设置阻抗，默认1
        */
        #endregion 20

        #region 21 高压调节发码
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdHighVoltage", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdHighVoltage(int iData);
        /*
         iData:  range(0,2)  0-2分别代表400V、200V和300V
         说明：设置当前通道电压，默认0
        */
        #endregion 21


        #region 22 前放
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdForword", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdForword(int iData);
        //iData： range(0,1)，0-关，1-开
        #endregion 22


        #region 23 编码器初始化
        [DllImport("NetModulDll.dll", EntryPoint = "InitEncoder", CallingConvention = CallingConvention.Cdecl)]
        public static extern void InitEncoder(int iIndex, int iData);
        //iIndex：range(0,1)，0-编码器A，1-编码器B
        //iData:  0
        #endregion 23

        #region 24 RealWave 实时采样数据和最大值对应的序列，通道数为1，采样点数为512
        /// <summary>
        /// 波形数
        /// </summary>
        public string[] m_ArrWave;
        /// <summary>
        /// 波峰序列每个值对应的时间序列，每个值乘以10ns为时间的采样时间
        /// </summary>
        public string[] m_ArrWave_Time;
        /// <summary>
        /// 波峰序列
        /// </summary>
        public static byte[] m_pChannelBuf = new byte[UTS_DATA_WIDTH];
        /// <summary>
        /// 波谷序列
        /// </summary>
        public static byte[] m_pValueBuf = new byte[UTS_DATA_WIDTH];

        /// <summary>
        /// 检测数据缓存
        /// </summary>
        public static List<Tofd_Arr> m_lstTofd = new List<Tofd_Arr>();
        /// <summary>
        /// 100000  100米
        /// </summary>
        public static int m_iArrLen = 100000;
        /// <summary>
        /// 显示用的数据链表,在函数setPeakBuffer中增加数据
        /// </summary>
        public static Tofd_Show_Data[] m_Arr_Tofd_Show = new Tofd_Show_Data[m_iArrLen];
        /// 数据个数
        /// </summary>
        public static int m_i_Run_AllNum = 0;
        /// <summary>
        /// 当前刷新D图序号
        /// </summary>
        public static int m_i_Run_i_No_Brush = 0;
        /// <summary>
        /// 波峰序列每个值对应的时间序列，每个值乘以10ns为时间的采样时间
        /// </summary>
        public static int[] m_pTimeBuf = new int[UTS_DATA_WIDTH];
        [DllImport("NetModulDll.dll", EntryPoint = "RealWave", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void RealWave(int clientChan, byte*[] pWaveDataBuf, byte*[] pValueBuf, int*[] pTimeBuf);
        /*void RealWave(int clientChan, int **pWaveDataBuf, int **pValueBuf ,  int **pTimeBuf);	
         pWaveDataBuf：波峰指针，
        例如：unsigned char m_pPeakBuf[通道数][采样点数]
        pValueBuf：波谷指针，
        例如：unsigned char m_pDestBuf[通道数][采样点数]
        pValueBuf：TimeBuf指针，波峰序列每个值对应的时间序列，每个值乘以10ns为时间的采样时间
        例如：int  m_pDestBuf[通道数][采样点数]
        说明：实时采样数据和最大值对应的序列，通道数为1，采样点数为512
         */
        [DllImport("NetModulDll.dll", EntryPoint = "GetRatioData", CallingConvention = CallingConvention.Cdecl)]

        public static extern void GetRatioData(int iChan, byte[] pWaveDataBuf, byte[] pValueBuf, int[] pTimeBuf); //获取采样值 
        public static void DLLTofd_RealWave_(int clientChan, ref byte[,] WaveDataBuf, ref byte[,] ValueBuf, ref int[,] TimeBuf)
        {
            int row = WaveDataBuf.GetUpperBound(0) + 1;
            int col = WaveDataBuf.GetUpperBound(1) + 1;
            unsafe
            {
                byte*[] iArr_1 = new byte*[row];
                byte*[] iArr_2 = new byte*[row];
                int*[] iArr_3 = new int*[row];

                for (int iNo = 0; iNo <= clientChan; iNo++)
                {
                    iArr_1 = new byte*[row];
                    iArr_2 = new byte*[row];
                    iArr_3 = new int*[row];
                    fixed (byte* fp_1 = WaveDataBuf)
                    {
                        for (int i = 0; i < row; i++)
                            iArr_1[i] = fp_1 + i * col;
                    }
                    fixed (byte* fp_2 = ValueBuf)
                    {
                        for (int i = 0; i < row; i++)
                            iArr_2[i] = fp_2 + i * col;
                    }
                    fixed (int* fp_3 = TimeBuf)
                    {
                        for (int i = 0; i < row; i++)
                            iArr_3[i] = fp_3 + i * col;
                    }

                    RealWave(iNo, iArr_1, iArr_2, iArr_3);
                }
            }
            return;
        }


        #endregion 24 RealWave

        #region 25．unsigned int  getEncoderValue(int Client,int iIndex);  //编码器值
        //Client和iIndex均为0，返回值为当前编码器的脉冲个数。
        //说明：实际编码器值和编码器方向相关
        //如果是反向，编码器值 = 0x800000 – 返回值；
        //如果是正向，编码器值 = 返回值 – 0x800000。

        [DllImport("NetModulDll.dll", EntryPoint = "getEncoderValue", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getEncoderValue(int Client, int iIndex);

        #endregion 25
        #region 26 保存数据
        [DllImport("NetModulDll.dll", EntryPoint = "SaveData", CallingConvention = CallingConvention.Cdecl)]

        public static extern bool SaveData(int iChan);    //保存数据
        #endregion 26


        /* 2023-11-15 增加如下函数，闸门计算厚度
         iSel:  range(0,1)  0、1分别代表闸门A和闸门B
        status:  range(0,1)  0、1分别代表不显示和显示
        说明：设置某个闸门是否显示
         */

        /// <summary>
        ///iSel:  range(0,1)  0、1分别代表闸门A和闸门B
        ///status:  range(0,1)  0、1分别代表不显示和显示
        ///说明：设置某个闸门是否显示
        /// </summary>
        /// <param name="iSel"></param>
        /// <param name="status"></param>
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdGateStatus", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdGateStatus(int iSel, bool status);

        /*
          iSel:  range(0,1)  0、1分别代表闸门A和闸门B
        iStart:  range(0,512)，表示闸门起点
        说明：设置某个闸门的起点

  */
        /// <returns></returns>
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdGateStart", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdGateStart(int iSel, int iStart);

        /*
         iSel:  range(0,1)  0、1分别代表闸门A和闸门B
        iEnd:  range(0,512)，表示闸门的终点
        说明：设置某个闸门的终点

  */
        /// <returns></returns>
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdGateEnd", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdGateEnd(int iSel, int iEnd);

        /*
          iSel:  range(0,1)  0、1分别代表闸门A和闸门B
        iHigh:  range(0,255)，表示闸门的高度
        说明：设置某个闸门的高度

  */
        /// <returns></returns>
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdGateHigh", CallingConvention = CallingConvention.Cdecl)]
        public static extern void  SendCmdGateHigh(int iSel, int iHigh);
        //---

        /*
          iChan:  range(0,1)  0、1分别代表通道1和通道2
          gate:  range(0,1)  0、1分别代表闸门A和闸门B
          返回值：通道iChan中闸门gate内的峰值的声程 
 */
        /// <returns></returns>
        [DllImport("NetModulDll.dll", EntryPoint = "getGateS", CallingConvention = CallingConvention.Cdecl)]
        public static extern float  getGateS(int iChan, int gate);
        /*
       iChan:  range(0,1)  0、1分别代表通道1和通道2
        iSel:  range(0,1)  0、1分别代表闸门A和闸门B
        返回值：通道iChan中闸门gate内的峰值的高度 

 */
        /// <returns></returns>
        [DllImport("NetModulDll.dll", EntryPoint = "getGateH", CallingConvention = CallingConvention.Cdecl)]
        public static extern float getGateH(int iChan, int gate);

        /*
        iChan:  range(0,1)  0、1分别代表通道1和通道2
        iSel:  range(0,1)  0、1分别代表闸门A和闸门B
        返回值：通道iChan中闸门gate内的峰值的声时，单位us 

*/
        /// <returns></returns>
        [DllImport("NetModulDll.dll", EntryPoint = "getGateT", CallingConvention = CallingConvention.Cdecl)]
        public static extern float getGateT(int iChan, int gate);
        /*
          iSel:  range(0,1)  0、1分别代表闸门A和闸门B
          iHigh:  range(0,255)，表示闸门的高度
          说明：设置某个闸门的高度

*/
     
        /// <summary>
        /// 联机
        /// </summary>
        /// <returns></returns>
        public bool Link()
        {
            try
            {
                if (m_pSparam == null)
                    m_pSparam = new SEmatChanParam[CHAN_OF_CLIENT];
                if (blNetLink)
                    Close_TOFD();
                Init_Param();
                blNetLink = initNetWork();
                if (blNetLink)
                {
                    blNetLink = false;
                    if (m_pSparam[m_icurChan].m_BScanMode == 2) goto CDW;
                    run();
                    Set_Cannl_Param();
                }
            }
#pragma warning disable CS0168 // 声明了变量“Linke”，但从未使用过
            catch (Exception Linke)
#pragma warning restore CS0168 // 声明了变量“Linke”，但从未使用过
            { }
        CDW:
            if (blNetLink == false)
            {
                m_strLinkMsg = "TOFD 联机失败";

            }

            return blNetLink;
        }
        /// <summary>
        /// 测试使用：保存数据
        /// </summary>
        /// <param name="iChan"></param>
        /// <returns></returns>
        public bool Save_Data(int iChan)
        {
            bool _blRet = SaveData(iChan);
            return _blRet;
        }
        /// <summary>
        /// 1 参数初始化
        /// </summary>
        public void Init_Param()
        {
            InitParam();
        }
        /// <summary>
        /// 显示数据初始化
        /// </summary>
        public void Init_ShowData(int i_UI_Type, int iChScanRange = 10)
        {
            i_Ul_Probe_Type = i_UI_Type;
            Tofd.m_Arr_Tofd_Show = new Tofd_Show_Data[Tofd.m_iArrLen];
            int _iRange = iChScanRange * 100;
            for (int i = 0; i < Tofd.m_iArrLen; i++)
                Tofd.m_Arr_Tofd_Show[i] = new Tofd_Show_Data(i_Ul_Probe_Type, _iRange);
        
            //for(int i=0;i<50;i++)
            //    ThreadPool.QueueUserWorkItem(new WaitCallback(SaveOneWave), (object)i);
        }
        /// <summary>
        /// 车体前进true/后退false
        /// </summary>
        //public bool m_blF_W = false;

        /// <summary>
        /// 联机
        /// </summary>
        public bool initNetWork()
        {
            bool blRet = false;
            m_connectStatus = new bool[HSD_CLIENT];

            blRet = InitModule(CHAN_OF_CLIENT, HSD_CLIENT);// blRet = InitModule(CHAN_OF_CLIENT, HSD_CLIENT);
            if (blRet == false)
            {
                MessageBox.Show("Network settings error!");
                return blRet;
            }
            blRet = CreateHostSocket();
            if (blRet == false)
                MessageBox.Show("Local network settings failed!");
            return blRet;
        }
        public void connectStatues(int i)
        {
            if (i >= 0 && i < HSD_CLIENT)
                m_connectStatus[i] = true;
        }
        private bool stopped = false;
        public void run()
        {
            UInt16[] curUtsCardOk = new UInt16[1];
            curUtsCardOk[0] = HSD_CLIENT + 1;
            stopped = false;
            while (!stopped)
            {
                if (GetConnectStatus(curUtsCardOk))
                {
                    stopped = false;
                }
                if (curUtsCardOk[0] < HSD_CLIENT && curUtsCardOk[0] >= 0)
                {
                    m_pChannelBuf = new byte[UTS_DATA_WIDTH];
                    m_pValueBuf = new byte[UTS_DATA_WIDTH];
                    m_pTimeBuf = new int[UTS_DATA_WIDTH];

                    connectStatues(curUtsCardOk[0]);
                    stopped = true;
                    blNetLink = true;
                }
                System.Threading.Thread.Sleep(10);
            }
        }

        public void Set_Cannl_Param()
        {
            InitSendParam();
            if (m_1_2 == 1)
                Demo_SendParam_1();
            else if (m_1_2 == 2)
                Demo_SendParam_2();
            setPeakBuffer(m_fWaveFramePerHeight, m_fWaveFramePerWidth);
            SendCmdSampleStart(); //增加开关采样，不再默认开采样
            SendCmdSampleStart();
        }
        /// <summary>
        /// 关闭TOFD
        /// </summary>
        public void Close_TOFD()
        {
            blNetLink = false;
            //     if (blNetLink)    
            ExitModule();
        }
        /// <summary>
        /// 当前通道 0-N   单通道：0  双通道 ：1
        /// </summary>
        public int m_icurChan = 1;
        /// <summary>
        /// 单通道还是双通道1：单 2：双
        /// </summary>
        public int m_1_2 = 2;
        /// <summary>
        /// 每块通道数 双通道：2 单通道：1
        /// </summary>
        public const int CHAN_OF_CLIENT = 2;//2
        /// <summary>
        /// 一个数据点高度
        /// </summary>
        public float m_fWaveFramePerHeight;
        /// <summary>
        /// 一个数据点宽度
        /// </summary>
        public float m_fWaveFramePerWidth;

        /// <summary>
        /// 报警门限值
        /// </summary>
        public int m_i_Alarm_Limit = 20;
        /// <summary>
        /// 报警左起点
        /// </summary>
        public int m_i_Alarm_Left_S = 0;
        /// <summary>
        /// 报警右终点
        /// </summary>
        public int m_i_Alarm_Left_E = 0;
        /// <summary>
        /// 是否启动报警
        /// </summary>
        public bool m_bl_Alarm = false;

        /// <summary>
        /// 图形左边起点
        /// </summary>
        float m_iWaveLeft = 0;
        /// <summary>
        /// 每个点的高度与宽度
        /// </summary>
        /// <param name="iWidth"></param>
        /// <param name="iHeight"></param>
        /// <param name="iLeft"></param>
        public void SetH_W(int iWidth, int iHeight, int iLeft)
        {
            m_fWaveFramePerHeight = iHeight / ((float)UTS_DATA_HEIGHT);
            m_fWaveFramePerWidth = iWidth / ((float)UTS_DATA_WIDTH);
            m_iWaveLeft = iLeft;
        }
        /// <summary>
        /// 正/负/全检波数据
        /// </summary>
        public PointF[] m_points = new PointF[UTS_DATA_WIDTH];
        /// <summary>
        /// 射频波数据
        /// </summary>
        public PointF[] m_points_2 = new PointF[UTS_DATA_WIDTH * 2];
        /// <summary>
        /// B扫描图对应标准颜色
        /// </summary>
        public Color[] m_Co_StandColor = new Color[256];


        /// <summary>
        /// 画图：0:D图 1：B图
        /// </summary>
        public int m_i_Plant_B1_D0 = 0;


        /// 开口方向
        /// </summary>
        public int m_Pbl_Zf = -1;
        /// <summary>
        /// 波峰位置统计
        /// </summary>
        public Tofd_Peak m_PeakPosi = new Tofd_Peak();

        /// <summary>
        /// 波峰位置统计
        /// </summary>
        public struct Tofd_Peak
        {
            /// <summary>
            /// 波峰门限
            /// </summary>
            public int iPeakLimit;
            /// <summary>
            /// 表面波标准 首波 ：开始位置
            /// </summary>
            public int i_Stand_Start_L;
            /// <summary>
            /// 表面波标准 首波 ：终止位置
            /// </summary>
            public int i_Stand_End_L;
            /// <summary>
            /// 超出范围
            /// </summary>
            public int i_Ou_of_Range;

            /// <summary>
            /// 表面波标准 底波 ：开始位置
            /// </summary>
            public int i_Stand_Start_R;
            /// <summary>
            /// 表面波标准 底波 ：终止位置
            /// </summary>
            public int i_Stand_End_R;
        }


        public class CliPeak
        {
            /// <summary>
            /// 波形开始
            /// </summary>
            public int iPeak_Start = -1;
            /// <summary>
            ///波形结束
            /// </summary>
            public int iPeak_End = -1;
        }
        int _iLenStart = UTS_DATA_WIDTH / 3;
        int _iLenEnd = 2 * UTS_DATA_WIDTH / 3;
        /// <summary>
        /// 当前数据波形数据
        /// </summary>
#pragma warning disable CS0649 // 从未对字段“Tofd.m_LstPeakData”赋值，字段将一直保持其默认值 null
        List<CliPeak> m_LstPeakData;
#pragma warning restore CS0649 // 从未对字段“Tofd.m_LstPeakData”赋值，字段将一直保持其默认值 null
        /// <summary>
        /// 图像中一个代表距离mm
        /// </summary>
        public int m_i_1_mm = 1;
        /// <summary>
        /// 采集数据
        /// </summary>
        /// <param name="clientChan"></param>
        public void DLLTofd_RealWave_1(int clientChan)
        {
            if (m_bl_Ck_Wave == false)
            {
                GetRatioData(m_icurChan, m_pChannelBuf, m_pValueBuf, m_pTimeBuf);
                //     bool _bl=  SaveTbuf(m_icurChan);
                if (m_pSparam[m_icurChan].m_BScanMode == 2)
                {

                    bool _blOk = true;
#pragma warning disable CS0162 // 检测到无法访问的代码
                    for (int i = 0; i < 50; i++)
#pragma warning restore CS0162 // 检测到无法访问的代码
                    { if (m_pChannelBuf[i] > 0) _blOk = false; break; }
                    if (_blOk)
                    {
                        for (int i = 0; i < UTS_DATA_WIDTH; i++)
                        {
                            m_pValueBuf[i] = 127; m_pChannelBuf[i] = 127;
                        }
                        int _iNo = 0, _iNo_L = 0;
                        m_pChannelBuf[_iNo++] = 127; m_pChannelBuf[_iNo++] = 132; m_pChannelBuf[_iNo++] = 125;
                        m_pChannelBuf[_iNo++] = 128; m_pChannelBuf[_iNo++] = 127; m_pChannelBuf[_iNo++] = 105;
                        m_pChannelBuf[_iNo++] = 0; m_pChannelBuf[_iNo++] = 0; m_pChannelBuf[_iNo++] = 132;
                        m_pChannelBuf[_iNo++] = 255; m_pChannelBuf[_iNo++] = 255; m_pChannelBuf[_iNo++] = 255;
                        m_pChannelBuf[_iNo++] = 255; m_pChannelBuf[_iNo++] = 217; m_pChannelBuf[_iNo++] = 54;
                        m_pChannelBuf[_iNo++] = 1; m_pChannelBuf[_iNo++] = 0; m_pChannelBuf[_iNo++] = 0;
                        m_pChannelBuf[_iNo++] = 0; m_pChannelBuf[_iNo++] = 17; m_pChannelBuf[_iNo++] = 92;
                        m_pChannelBuf[_iNo++] = 150; m_pChannelBuf[_iNo++] = 180; m_pChannelBuf[_iNo++] = 197;
                        m_pChannelBuf[_iNo++] = 205; m_pChannelBuf[_iNo++] = 206; m_pChannelBuf[_iNo++] = 203;
                        m_pChannelBuf[_iNo++] = 196; m_pChannelBuf[_iNo++] = 189; m_pChannelBuf[_iNo++] = 178;
                        m_pChannelBuf[_iNo++] = 162; m_pChannelBuf[_iNo++] = 152; m_pChannelBuf[_iNo++] = 143;
                        m_pChannelBuf[_iNo++] = 137; m_pChannelBuf[_iNo++] = 130; m_pChannelBuf[_iNo++] = 125;

                        m_pValueBuf[_iNo_L++] = 127; m_pValueBuf[_iNo_L++] = 122; m_pValueBuf[_iNo_L++] = 125;
                        m_pValueBuf[_iNo_L++] = 126; m_pValueBuf[_iNo_L++] = 118; m_pValueBuf[_iNo_L++] = 0;
                        m_pValueBuf[_iNo_L++] = 0; m_pValueBuf[_iNo_L++] = 0; m_pValueBuf[_iNo_L++] = 12;
                        m_pValueBuf[_iNo_L++] = 196; m_pValueBuf[_iNo_L++] = 255; m_pValueBuf[_iNo_L++] = 255;
                        m_pValueBuf[_iNo_L++] = 254; m_pValueBuf[_iNo_L++] = 72; m_pValueBuf[_iNo_L++] = 11;
                        m_pValueBuf[_iNo_L++] = 0; m_pValueBuf[_iNo_L++] = 0; m_pValueBuf[_iNo_L++] = 0;
                        m_pValueBuf[_iNo_L++] = 0; m_pValueBuf[_iNo_L++] = 0; m_pValueBuf[_iNo_L++] = 34;
                        m_pValueBuf[_iNo_L++] = 104; m_pValueBuf[_iNo_L++] = 158; m_pValueBuf[_iNo_L++] = 198;
                        m_pValueBuf[_iNo_L++] = 200; m_pValueBuf[_iNo_L++] = 204; m_pValueBuf[_iNo_L++] = 212;
                        m_pValueBuf[_iNo_L++] = 190; m_pValueBuf[_iNo_L++] = 180; m_pValueBuf[_iNo_L++] = 164;
                        m_pValueBuf[_iNo_L++] = 154; m_pValueBuf[_iNo_L++] = 145; m_pValueBuf[_iNo_L++] = 138;
                        m_pValueBuf[_iNo_L++] = 131; m_pValueBuf[_iNo_L++] = 126; m_pValueBuf[_iNo_L++] = 123;
                    }
                }
            }
        }
        /// <summary>
        /// 北京六维远光探头采集数据
        /// </summary>
        /// <param name="i_Trip">mm单位的距离</param>
        /// <param name="flDistanc_X">m单位的距离</param>
        /// <param name="iDatLen">数据长度</param>
        /// <param name="RecevData">数据</param>
        public void setPeakBuffer(int i_Trip, float flDistanc_X, int iDatLen, byte[] RecevData)
        {
            byte _iData = 0;
            int _i_Run_i_No_Brush = 0;

            if (m_iRun == 1 && m_iRun_State == 0 && i_Trip >= 0)
            {
                #region 1 确定当前距离对应缓存的位置
                if (m_i_1_mm == 0)
                {
                    float _fmm = (Scree_iDotWithmm_X * 1000);
                    m_i_1_mm = int.Parse(_fmm.ToString("f0"));
                }
                float _fX = i_Trip / m_i_1_mm;
                _i_Run_i_No_Brush = int.Parse(_fX.ToString("f0"));


                Tofd.m_Arr_Tofd_Show[_i_Run_i_No_Brush].m_flDistanc_X = flDistanc_X;

                if (flDistanc_X > m_flDistanc_X_Old)
                {
                    m_flDistanc_X_Old = flDistanc_X;

                    _fX = i_Trip / m_i_1_mm;
                    Tofd.m_i_Run_AllNum = int.Parse(_fX.ToString("f0"));
                }
                #endregion 1 

                #region 2 测量数据转储到缓存
                for (int j = 0; j < iDatLen; j++)
                {
                    #region 2.1 报警判断
                    if (j > m_i_Alarm_Left_S && j < m_i_Alarm_Left_E)
                    {
                        if (m_bl_Alarm == false)
                        {
                            m_bl_Alarm = (_iData > m_i_Alarm_Limit);
                        }
                    }
                    #endregion 2.1 

                    #region 2.2 波形数据保存到检测缓存
                    _iData = RecevData[j];
                    Tofd.m_Arr_Tofd_Show[_i_Run_i_No_Brush].m_ArrWave[j] = _iData.ToString();
                    Tofd.m_Arr_Tofd_Show[_i_Run_i_No_Brush].m_pChannelBuf[j] = _iData;
                    //  Tofd.m_Arr_Tofd_Show[_i_Run_i_No_Brush].m_pTimeBuf[j] = _iTime;
                    #endregion 2.2
                }
                #endregion 2
            }

        }
        /// <summary>
        /// 获得距离对应位置序号
        /// </summary>
        /// <param name="_Scree_iDotWithmm_X"></param>
        /// <param name="_m_flDistanc_X"></param>
        /// <returns></returns>
        private int Get_Distan_mm_No(int _imm, float  _m_flDistanc_X)
        {
            int _iRet = -1;
            //float _fmm = (_Scree_iDotWithmm_X * 1000);
            //int _imm = int.Parse(_fmm.ToString("f0"));

            float _fX = (_m_flDistanc_X * 1000);
            int _iX = int.Parse(_fX.ToString("f0"));
            _fX = _iX / _imm;
            _iRet = int.Parse(_fX.ToString("f0"));
            if (_iRet > m_iArrLen) _iRet = m_iArrLen - 1;
            return _iRet;
        }

        /// <summary>
        /// 获得TOFD数据
        /// </summary>
        public void setPeakBuffer(float fWaveFramePerHeight, float fWaveFramePerWidth, int iRun = 0, bool blCollection = true)
        {
            try
            {
                if (Tofd.m_Arr_Tofd_Show == null) return;


                blAlarm = false;//当前数据没有异常
                                //2024-5-7 del    strWave = ""; strWave_2 = ""; strWave_Time = "";
                int _iTime = 0;

                //   m_LstPeakData = new List<CliPeak>();
                //  CliPeak _Peak = new CliPeak();

                float y = 0, x = 0, y1 = 0, y_old = -999;
                if (m_Data_Type != 0)
                {
                    m_Gate_1.i_K = 0;
                    m_Gate_1.fl_Peak_Data = -999;
                    m_Gate_1.i_Gate_P = -1;

                    m_Gate_2.i_K = 0;
                    m_Gate_2.fl_Peak_Data = -999;
                    m_Gate_2.i_Gate_P = -1;
                }
                //  for (int i = 0; i < 1; ++i)

                //    int i = m_icurChan;
                DLLTofd_RealWave_1(m_icurChan);
                if (blCollection)//手动时，实时显示实际位置
                {
                    m_PeakPosi = new Tofd_Peak();
                    m_PeakPosi.iPeakLimit = 127;
                    m_PeakPosi.i_Ou_of_Range = 10;
                    m_PeakPosi.i_Stand_Start_L = -1;
                    m_PeakPosi.i_Stand_End_L = -1;
                    m_PeakPosi.i_Stand_Start_R = -1;
                    m_PeakPosi.i_Stand_End_R = -1;
                }

                #region 显示缓存

                int _i_Run_i_No_Brush = 0;
                m_bl_Alarm = false;
                byte _iData = 0, _iData_2 = 0;
                var IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(Tofd.m_Arr_Tofd_Show[0].m_pChannelBuf, 0);

                if (m_iRun == 1 && m_iRun_State == 0)
                {
                    float _fmm = (Scree_iDotWithmm_X * 1000);
                    int _imm = int.Parse(_fmm.ToString("f0"));

                    #region 如果距离不连续，就拿上一个数补充空缺位置,此处只补充一个
                    float _fl = m_flDistanc_X - m_flDistanc_X_Old;
                    _fl *= 1000;
                    int _i_Err = (int)_fl;
                    _fl = m_flDistanc_X_Old * 1000;
                    int _i_X_Old = (int)_fl;

                    if (m_bl_AddDataByHis && _i_X_Old > 0 && _i_Err > _imm)
                    {
                        //1 填充位置
                        int _iDstNo_Old = Get_Distan_mm_No(_imm, m_flDistanc_X_Old);
                        int _iDstNo_Add = _iDstNo_Old + 1;
                        //2 距离填充
                        Tofd.m_Arr_Tofd_Show[_iDstNo_Add].m_flDistanc_X = m_flDistanc_X_Old + Scree_iDotWithmm_X;

                        //3 数值填充
                        //    波峰
                        IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(Tofd.m_Arr_Tofd_Show[_iDstNo_Add].m_pChannelBuf, 0);
                        Marshal.Copy(Tofd.m_Arr_Tofd_Show[_iDstNo_Old].m_pChannelBuf, 0, IntPtArr, Tofd.UTS_DATA_WIDTH);
                        //    波谷
                        IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(Tofd.m_Arr_Tofd_Show[_iDstNo_Add].m_pValueBuf, 0);
                        Marshal.Copy(Tofd.m_Arr_Tofd_Show[_iDstNo_Old].m_pValueBuf, 0, IntPtArr, Tofd.UTS_DATA_WIDTH);
                        //    时间
                        IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(Tofd.m_Arr_Tofd_Show[_iDstNo_Add].m_pTimeBuf, 0);
                        Marshal.Copy(Tofd.m_Arr_Tofd_Show[_iDstNo_Old].m_pTimeBuf, 0, IntPtArr, Tofd.UTS_DATA_WIDTH);
                    }
                    #endregion
                    if (m_flDistanc_X >= 0)
                    {
                        //float _fX = (m_flDistanc_X * 1000);
                        //int _iX = int.Parse(_fX.ToString("f0"));
                        //_fX = _iX / _imm;
                        _i_Run_i_No_Brush = Get_Distan_mm_No(_imm, m_flDistanc_X);

                        if (_i_Run_i_No_Brush < m_iArrLen)
                            Tofd.m_Arr_Tofd_Show[_i_Run_i_No_Brush].m_flDistanc_X = m_flDistanc_X;

                        if (m_flDistanc_X > m_flDistanc_X_Old)
                        {
                            m_flDistanc_X_Old = m_flDistanc_X;
                            Tofd.m_i_Run_AllNum = Get_Distan_mm_No(_imm, m_flDistanc_X_Old);// int.Parse(_fX.ToString("f0"));
                        }
                    }
                }
                #endregion 显示缓存

                if (m_pSparam[m_icurChan].m_iDemodulation_Flag != 2)//不是射频波
                {
                    PointF[] points = new PointF[UTS_DATA_WIDTH];
                    for (int j = 0; j < UTS_DATA_WIDTH; ++j)
                    {

                        _iData = m_pChannelBuf[j];
                        //2024-5-7 del        strWave += (j == 0 ? "" : ",") + _iData.ToString();
                        _iTime = m_pTimeBuf[j];
                        #region 报警判断
                        if (j > m_i_Alarm_Left_S && j < m_i_Alarm_Left_E)
                        {
                            if (m_bl_Alarm == false)
                            {
                                m_bl_Alarm = (_iData > m_i_Alarm_Limit);
                            }
                        }
                        #endregion
                        if (m_iRun == 1 && m_iRun_State == 0 && m_flDistanc_X >= 0)
                        {
                            //       Tofd.m_Arr_Tofd_Show[_i_Run_i_No_Brush].m_ArrWave[j] = _iData.ToString();
                            Tofd.m_Arr_Tofd_Show[_i_Run_i_No_Brush].m_pChannelBuf[j] = _iData;
                            Tofd.m_Arr_Tofd_Show[_i_Run_i_No_Brush].m_pTimeBuf[j] = _iTime;
                        }
                        //2024-5-7 del      strWave_Time += (j == 0 ? "" : ",") + _iTime.ToString();
                        y = (float)((UTS_DATA_HEIGHT - _iData) * m_fWaveFramePerHeight);
                        // float y = (float)(_iData * fWaveFramePerHeight);

                        #region 寻找闸门内最大波峰位置
                        switch (m_Data_Type)
                        {
                            case 1:
                                FindPeak(j, y, ref y_old, ref m_Gate_1);
                                break;
                            case 2:
                                FindPeak(j, y, ref y_old, ref m_Gate_1);
                                FindPeak(j, y, ref y_old, ref m_Gate_2);
                                break;
                        }
                        #endregion 闸门内找波峰
                        x = (float)(j * fWaveFramePerWidth) + m_iWaveLeft;
                        points[j] = new PointF(x, y);

                        #region 伤点判断
                        //if (blCollection)
                        //    Juge_limit(j, _iData);
                        //if (j > 3 && j < UTS_DATA_WIDTH - 3)
                        //    Juge_Peak(ref _Peak, j, m_pChannelBuf[j - 1], _iData, m_pChannelBuf[j + 1], ref m_LstPeakData);

                        #endregion  伤点判断
                    }
                    m_points = (PointF[])points.Clone();
                }
                else//射频波
                {
                    int _iNum = UTS_DATA_WIDTH * 2;
                    PointF[] points_2 = new PointF[_iNum];

                    if (m_iRun == 1 && m_iRun_State == 0 && m_flDistanc_X >= 0)
                    {
                        IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(Tofd.m_Arr_Tofd_Show[_i_Run_i_No_Brush].m_pChannelBuf, 0);
                        Marshal.Copy(m_pChannelBuf, 0, IntPtArr, Tofd.UTS_DATA_WIDTH);
                        IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(Tofd.m_Arr_Tofd_Show[_i_Run_i_No_Brush].m_pValueBuf, 0);
                        Marshal.Copy(m_pValueBuf, 0, IntPtArr, Tofd.UTS_DATA_WIDTH);
                        IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(Tofd.m_Arr_Tofd_Show[_i_Run_i_No_Brush].m_pTimeBuf, 0);
                        Marshal.Copy(m_pTimeBuf, 0, IntPtArr, Tofd.UTS_DATA_WIDTH);
                    }

                    for (int j = 0; j < UTS_DATA_WIDTH; ++j)
                    {
                        _iData = m_pChannelBuf[j];
                        _iData_2 = m_pValueBuf[j];
                        _iTime = m_pTimeBuf[j];

                        #region 报警判断
                        if (j > m_i_Alarm_Left_S && j < m_i_Alarm_Left_E)
                        {
                            if (m_bl_Alarm == false)
                            {
                                m_bl_Alarm = (Math.Abs(_iData - 127) > m_i_Alarm_Limit);
                                m_bl_Alarm = (Math.Abs(_iData_2 - 127) > m_i_Alarm_Limit);
                            }
                        }
                        #endregion
                        //if (m_iRun == 1 && m_iRun_State == 0 && m_flDistanc_X >= 0)
                        //{
                        //  //  Tofd.m_Arr_Tofd_Show[_i_Run_i_No_Brush].m_ArrWave[j] = _iData.ToString();
                        //    Tofd.m_Arr_Tofd_Show[_i_Run_i_No_Brush].m_pChannelBuf[j] = _iData;
                        //    Tofd.m_Arr_Tofd_Show[_i_Run_i_No_Brush].m_pValueBuf[j] = _iData_2;
                        //    Tofd.m_Arr_Tofd_Show[_i_Run_i_No_Brush].m_pTimeBuf[j] = _iTime;
                        //}
                        //2024-5-7 del         strWave += (j == 0 ? "" : ",") + _iData.ToString();
                        //2024-5-7 del      strWave_Time += (j == 0 ? "" : ",") + _iTime.ToString();

                        x = (float)(j * fWaveFramePerWidth) + m_iWaveLeft;
                        //float y = (float)((UTS_DATA_HEIGHT - _iData) * m_fWaveFramePerHeight);
                        //float y1 = (float)((UTS_DATA_HEIGHT - _iData_2) * m_fWaveFramePerHeight);
                        y = (float)((_iData) * fWaveFramePerHeight);
                        y1 = (float)((_iData_2) * fWaveFramePerHeight);

                        #region 寻找闸门内最大波峰位置 26
                        switch (m_Data_Type)
                        {
                            case 1:
                                FindPeak(j, y, ref y_old, ref m_Gate_1);
                                break;
                            case 2:
                                FindPeak(j, y, ref y_old, ref m_Gate_1);
                                FindPeak(j, y, ref y_old, ref m_Gate_2);
                                break;
                        }
                        #endregion 闸门内找波峰

                        //2024-5-7 del        strWave_2 += (j == 0 ? "" : ",") + _iData_2.ToString();

                        points_2[j * 2] = new PointF(x, y);
                        points_2[j * 2 + 1] = new PointF(x, y1);

                        #region 伤点判断
                        //if (blCollection)
                        //    Juge_limit(j, _iData);
                        //if (j > 3 && j < UTS_DATA_WIDTH - 3)
                        //    Juge_Peak(ref _Peak, j, m_pChannelBuf[j - 1], _iData, m_pChannelBuf[j + 1], ref m_LstPeakData);
                        #endregion  伤点判断
                    }
                    #region 计算厚度值
                    float _Time_1 = 0, _Time_2 = 0;
                    switch (m_Data_Type)
                    {
                        case 1:
                            if (m_Gate_1.i_Gate_P > 0)
                            {
                                _Time_1 = Tofd.m_pTimeBuf[m_Gate_1.i_Gate_P] * 0.01f;

                                m_flThick = m_pSparam[m_icurChan].m_dSpeed * _Time_1 / 1000f;
                            }
                            break;
                        case 2:
                            if (m_Gate_1.i_Gate_P > 0)
                            {
                                _Time_1 = Tofd.m_pTimeBuf[m_Gate_1.i_Gate_P] * 0.01f;
                            }
                            if (m_Gate_2.i_Gate_P > 0)
                            {
                                _Time_2 = Tofd.m_pTimeBuf[m_Gate_2.i_Gate_P] * 0.01f;
                            }

                            m_flThick = m_pSparam[m_icurChan].m_dSpeed * (_Time_2 - _Time_1) / 1000f;
                            break;
                    }
                    #endregion 计算厚度
                    m_points_2 = (PointF[])points_2.Clone();
                    //2024-5-7 del     strWave_2 = m_pSparam[m_icurChan].m_iDemodulation_Flag + "|" + strWave_2;
                }
                //2024-5-7 del    strWave = m_pSparam[m_icurChan].m_iDemodulation_Flag + "|" + strWave;
                //if (m_iRun == 1 && m_iRun_State == 0 && m_flDistanc_X >= 0)  26
                //{
                //    //波峰
                //    var IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(Tofd.m_Arr_Tofd_Show[_i_Run_i_No_Brush].m_pChannelBuf, 0);
                //    Marshal.Copy(Tofd.m_pChannelBuf, 0, IntPtArr, Tofd.UTS_DATA_WIDTH);
                //    //波谷
                //    IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(Tofd.m_Arr_Tofd_Show[_i_Run_i_No_Brush].m_pValueBuf, 0);
                //    Marshal.Copy(Tofd.m_pValueBuf, 0, IntPtArr, Tofd.UTS_DATA_WIDTH);
                //    //时间
                //  //  IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(Tofd.m_Arr_Tofd_Show[_i_Run_i_No_Brush].m_pTimeBuf, 0);
                //  //  Marshal.Copy(Tofd.m_pTimeBuf, 0, IntPtArr, Tofd.UTS_DATA_WIDTH);
                //}

                #region  波形类型确认
                //      Juge_m_LstPeakData();
                #endregion
            }
            catch { }
        }
        private void FindPeak(int j, float y, ref float y_old, ref PeakDat Gate)
        {
            if (j >= Gate.i_Gate_S && j <= Gate.i_Gate_E && Gate.i_Gate_S < Gate.i_Gate_E)
            {
                if (y_old == -999)
                    y_old = y;
                else
                {
                    if (y_old > y)//降低 
                    {
                        switch (Gate.i_K)
                        {
                            case 0://新数据
                                Gate.i_K = -1;//下降
                                break;
                            case 1://之前趋势为上升
                                   //之前那个点为波峰
                                if (Gate.fl_Peak_Data < y_old)
                                {
                                    Gate.fl_Peak_Data = y_old;
                                    Gate.i_Gate_P = j - 1;
                                }
                                break;
                            case -1://之前趋势为下降
                                Gate.i_K = -1;//仍然下降
                                break;
                        }
                    }
                    else if (y_old < y)//爬升
                    {
                        switch (Gate.i_K)
                        {
                            case 0://新数据
                                Gate.i_K = 1;
                                break;
                            case 1://之前趋势为上升
                                Gate.i_K = 1;//仍然上升
                                break;
                            case -1://之前趋势为下降
                                Gate.i_K = 1;
                                break;
                        }
                    }
                    //当前数据刷新老数据
                    y_old = y;
                }
            }
        }
        /// <summary>
        /// 缺陷定性
        /// </summary>
        private void Juge_m_LstPeakData()
        {
            int _iLimit_L_R = 5;//直通波和底波波峰左右偏移误差

            ClAlarm _Alarm = new ClAlarm();
            int _iLastNo = m_LstPeakData.Count - 1;

            try
            {
                for (int i = 0; i < m_LstPeakData.Count; i++)
                {
                    //依据标准波形的直通波和底波的开始结束区间为界限:总共7种类型
                    // 1 直通波没有
                    if (i == 0 && m_LstPeakData[i].iPeak_Start - m_PeakPosi.i_Stand_End_L > _iLimit_L_R &&
                       m_LstPeakData[i].iPeak_End - m_PeakPosi.i_Stand_Start_R > _iLimit_L_R)
                        _Alarm.lstType.Add("1");
                    //2 直通波滞后
                    if (i == 0 && m_LstPeakData[i].iPeak_Start - m_PeakPosi.i_Stand_Start_L > _iLimit_L_R &&
                        m_LstPeakData[i].iPeak_End - m_PeakPosi.i_Stand_End_L > _iLimit_L_R)
                        _Alarm.lstType.Add("2");
                    //3 没有底波  ，只有直通波和底波上端点
                    if (i == _iLastNo && m_LstPeakData[i].iPeak_Start - m_PeakPosi.i_Stand_End_L > _iLimit_L_R &&
                      m_LstPeakData[i].iPeak_End > m_PeakPosi.i_Stand_Start_R)
                        _Alarm.lstType.Add("3");
                    //4 有底波  ，底波滞后
                    if (i == _iLastNo && m_LstPeakData[i].iPeak_Start - m_PeakPosi.i_Stand_Start_R > _iLimit_L_R &&
                      m_LstPeakData[i].iPeak_End - m_PeakPosi.i_Stand_End_R > _iLimit_L_R)
                        _Alarm.lstType.Add("4");
                    //5 、7 有直通波和底波
                    if (i > 0 && i < _iLastNo && m_LstPeakData[i].iPeak_Start - m_PeakPosi.i_Stand_End_L > _iLimit_L_R &&
                    m_LstPeakData[i].iPeak_End > m_PeakPosi.i_Stand_Start_R)
                        _Alarm.lstType.Add("5");
                    //6 直通波有尾巴
                    if (i == 0 && m_LstPeakData[i].iPeak_Start - m_PeakPosi.i_Stand_Start_L < _iLimit_L_R &&
                      m_LstPeakData[i].iPeak_End - m_PeakPosi.i_Stand_End_L > _iLimit_L_R)
                        _Alarm.lstType.Add("6");
                }
                if (_Alarm.lstType.Count > 0)//有异常报警
                {
                    blAlarm = true;//当前数据有异常
                    int _iNum = m_lstAlarm.Count;
                    if (_iNum == 0)
                    {
                        _Alarm.flDistanc_X_Start = m_flDistanc_X;
                        _Alarm.flDistanc_X_End = m_flDistanc_X;
                        m_lstAlarm.Add(_Alarm);
                    }
                    else
                    {
                        if (m_flDistanc_X - m_lstAlarm[_iNum - 1].flDistanc_X_End > Scree_iDotWithmm_X)
                        {
                            //新增异常点
                            _Alarm.flDistanc_X_Start = m_flDistanc_X;
                            _Alarm.flDistanc_X_End = m_flDistanc_X;
                            m_lstAlarm.Add(_Alarm);
                        }
                        else// if(m_lstAlarm[_iNum - 1]. //同一种类型
                            m_lstAlarm[_iNum - 1].flDistanc_X_End = m_flDistanc_X;
                    }
                    string _strType = "";
                    for (int i = 0; i < _iNum; i++)
                        _strType += (i == 0 ? "" : ",") + _Alarm.lstType[i];
                    m_lstAlarm[_iNum - 1].strType = _strType;
                }
            }
#pragma warning disable CS0168 // 声明了变量“e”，但从未使用过
            catch (Exception e)
#pragma warning restore CS0168 // 声明了变量“e”，但从未使用过
            { }
        }
        /// <summary>
        /// tofd缺陷统计
        /// </summary>
        public List<ClAlarm> m_lstAlarm = new List<ClAlarm>();
        /// <summary>
        /// 图点间隔
        /// </summary>
        public float Scree_iDotWithmm_X = 5;
        /// <summary>
        /// 当前距离----90
        /// </summary>
        public float m_flDistanc_X = 0;
        /// <summary>
        /// 最远距离
        /// </summary>
        public int flMaxDistance = 0;
        /// <summary>
        /// 1：开始 0：停止 2: 检定完成  10:退出程序
        /// </summary>
        public int m_iRun = 0;

        /// <summary>
        /// 是否查看历史数据对应的波形 true:查看历史数据  false:实时数据
        /// </summary>
        public bool m_bl_Ck_Wave = false;
        /// <summary>
        /// 上次距离
        /// </summary>
        public float m_flDistanc_X_Old = 0;
        /// <summary>
        /// 距离跳变数据允许填充
        /// </summary>
        public bool m_bl_AddDataByHis = false;
        /// <summary>
        /// TOFD异常统计
        /// </summary>
        public class ClAlarm
        {
            /// <summary>
            /// 类型
            /// </summary>
            public List<string> lstType = new List<string>();
            /// <summary>
            /// 类型
            /// </summary>
            public string strType = "";
            /// <summary>
            /// 开始位置
            /// </summary>
            public float flDistanc_X_Start = -1;
            /// <summary>
            /// 开始位置
            /// </summary>
            public float flDistanc_X_End = -1;

        }

        /// <summary>
        /// 寻找直通波与底波界限
        /// </summary>
        /// <param name="j">数据X轴位置</param>
        /// <param name="blCollection">是否采集直通波和底波边界</param>
        /// <param name="_iData">当前峰值</param>
        private void Juge_limit(int j, int _iData)
        {
            #region 伤点判断 在X轴上判断Y数据异常：中心的数值为127

            //1 起点和终端位置判断
            //波峰位置

            if (j < _iLenStart)
            {
                if (m_PeakPosi.i_Stand_Start_L == -1)
                    if (Math.Abs(_iData - m_PeakPosi.iPeakLimit) >= m_PeakPosi.i_Ou_of_Range)
                        m_PeakPosi.i_Stand_Start_L = _iData;
                if (m_PeakPosi.i_Stand_Start_L != -1)
                    if (Math.Abs(_iData - m_PeakPosi.iPeakLimit) < m_PeakPosi.i_Ou_of_Range)
                        m_PeakPosi.i_Stand_End_L = _iData;
            }
            if (j > _iLenEnd)
            {
                if ((m_PeakPosi.i_Stand_Start_L != -1 && m_PeakPosi.i_Stand_End_L != -1) &&
                        m_PeakPosi.i_Stand_Start_R == -1)
                {
                    if (Math.Abs(_iData - m_PeakPosi.iPeakLimit) >= m_PeakPosi.i_Ou_of_Range)
                        m_PeakPosi.i_Stand_Start_R = _iData;
                }
                if (m_PeakPosi.i_Stand_Start_R != -1)
                {
                    if (Math.Abs(_iData - m_PeakPosi.iPeakLimit) < m_PeakPosi.i_Ou_of_Range)
                        m_PeakPosi.i_Stand_End_L = _iData;
                }
            }


            //2 在起点和终端中间有异常
            //2.1 有上端点
            //2.2 有下端点
            //2.3 短小缺陷时，上下端的距离很小
            //2.4 单个气孔：由于几何尺寸小，没有明显分离的上下端的回波，
            //形成一个独立的小月牙状，相位无法分辨



            //3 直通波被阻断：上/下表面开口裂纹
            //3.1 没有起点/终点
            //3.2 起点和终点之间有下/上端点
            //4 表面开口深度非常小，直通波没有断开，但明显滞后
            //4.1 起点滞后
            //4.2 有下/上端点

            #endregion  伤点判断

        }
        /// <summary>
        /// 中轴线
        /// </summary>
        public int m_iPeakLmit = 127;
        /// <summary>
        /// 波峰最低值限度
        /// </summary>
        public int m_iLimit = 5;
        /// <summary>
        /// 三个点判断波峰开始和结束
        /// </summary>
        /// <param name="iDat1"></param>
        /// <param name="iDat2"></param>
        /// <param name="iDat3"></param>
        /// <param name="_LstPeakData"></param>
        private void Juge_Peak(ref CliPeak _iCurPeak, int j, int iDat1, int iDat2, int iDat3, ref List<CliPeak> _LstPeakData)
        {
            if (_iCurPeak.iPeak_Start == -1)
            {
                if (Math.Abs(iDat3 - m_iPeakLmit) > m_iLimit &&
                  (iDat1 <= iDat2 && iDat2 < iDat3 || iDat1 >= iDat2 && iDat2 > iDat3))
                {
                    _iCurPeak.iPeak_Start = j;//波峰起点
                }
            }
            else
            {
                if (Math.Abs(iDat1 - m_iPeakLmit) < m_iLimit &&
                   Math.Abs(iDat2 - m_iPeakLmit) < m_iLimit &&
                   Math.Abs(iDat3 - m_iPeakLmit) < m_iLimit)
                {
                    _iCurPeak.iPeak_End = j;//波峰回落点

                    _LstPeakData.Add(_iCurPeak);
                    _iCurPeak = new CliPeak();
                }
            }
        }
        public void Demo_SendParam_Cann2()
        {
            SendCmdRepeatFreq((int)m_pSparam[0].m_iRepeatFreq);
            SendCmdVoltEnable(m_pSparam[0].m_iVoltEnable);//   SendCmdPulEnable(m_pSparam[i].m_iPulEnable); 陈大伟2023-2-27 新命令
            SendCmdHighVoltage((int)m_pSparam[0].m_iVolt);
            SendCmdForword(m_pSparam[0].m_iForword);
        }
        public void Demo_SendCurrentChanParam(int i)
        {
            SendCmdCurrentChan(i);

            SendCmdDB(m_pSparam[i].m_idB);
            SendCmdFreqRatio(m_pSparam[i].m_iRange, m_pSparam[i].m_dSpeed);
            SendCmdZeroTime(m_pSparam[i].m_iZeroTime);
            SendCmdParallel(m_pSparam[i].m_iParallelTime);

            // SendCmdForword(m_pSparam[i].m_iForword);

            //闸门发码
            SendCmdPulEnable(m_pSparam[i].m_iPulEnable);//陈大伟2023-2-27 新命令
            SendCmdPulWid(m_pSparam[i].m_iPulWidthCode);
            SendCmdWaveType((int)m_pSparam[i].m_iDemodulation_Flag);
            SendCmdWorkMode((int)m_pSparam[i].m_iWorkMode);
            SendCmdImpdance((int)m_pSparam[i].m_iImpedanceF);
        }
        public void Demo_SendParam_1()
        {

            for (int i = 0; i < CHAN_OF_CLIENT; i++)
            {
                SendCmdCurrentChan(i);

                SendCmdDB(m_pSparam[i].m_idB);
                SendCmdFreqRatio(m_pSparam[i].m_iRange, m_pSparam[i].m_dSpeed);
                SendCmdZeroTime(m_pSparam[i].m_iZeroTime);
                SendCmdParallel(m_pSparam[i].m_iParallelTime);

                SendCmdForword(m_pSparam[i].m_iForword);

                SendCmdPulWid(m_pSparam[i].m_iPulWidthCode);
                SendCmdWaveType((int)m_pSparam[i].m_iDemodulation_Flag);

                SendCmdRepeatFreq((int)m_pSparam[i].m_iRepeatFreq);
                SendCmdWorkMode((int)m_pSparam[i].m_iWorkMode);

                SendCmdBandWidth((int)m_pSparam[i].m_iSecBandWidthF);
                SendCmdImpdance((int)m_pSparam[i].m_iImpedanceF);
                SendCmdHighVoltage((int)m_pSparam[i].m_iVolt);
            }

            SendCmdCurrentChan(0);
            InitEncoder(0, 0);
            InitEncoder(1, 0);
        }
        public void Demo_SendParam_2()
        {
            Demo_SendParam_Cann2();

            for (int i = 0; i < 2; i++)
                Demo_SendCurrentChanParam(i);
            Demo_SendCurrentChanParam(m_icurChan);

            if (1 == 0)
            {
#pragma warning disable CS0162 // 检测到无法访问的代码
                for (int i = 0; i < CHAN_OF_CLIENT; i++)
#pragma warning restore CS0162 // 检测到无法访问的代码
                {
                    SendCmdCurrentChan(i);

                    SendCmdDB(m_pSparam[i].m_idB);
                    SendCmdFreqRatio(m_pSparam[i].m_iRange, m_pSparam[i].m_dSpeed);
                    SendCmdZeroTime(m_pSparam[i].m_iZeroTime);
                    SendCmdParallel(m_pSparam[i].m_iParallelTime);

                    SendCmdForword(m_pSparam[i].m_iForword);

                    SendCmdPulWid(m_pSparam[i].m_iPulWidthCode);
                    SendCmdWaveType((int)m_pSparam[i].m_iDemodulation_Flag);
                    SendCmdRepeatFreq((int)m_pSparam[i].m_iRepeatFreq);
                    SendCmdWorkMode((int)m_pSparam[i].m_iWorkMode);

                    SendCmdBandWidth((int)m_pSparam[i].m_iSecBandWidthF);

                    SendCmdImpdance((int)m_pSparam[i].m_iImpedanceF);
                    SendCmdHighVoltage((int)m_pSparam[i].m_iVolt);
                }
            }
            SendCmdCurrentChan(m_icurChan);
            InitEncoder(0, 0);
            InitEncoder(1, 0);
        }
        /// <summary>
        /// 说明：设置当前通道分频比，是范围和声速对应的发码
        /// </summary>
        public void SendCmdFreqRatio()
        {
            SendCmdFreqRatio(m_pSparam[m_icurChan].m_iRange, m_pSparam[m_icurChan].m_dSpeed);
        }
        /// <summary>
        /// 说明：设置当前通道平移，默认0
        /// </summary>
        public void SendCmdParallel()
        {
            SendCmdParallel(m_pSparam[m_icurChan].m_iParallelTime);
        }
        public void Init_Encoder()
        {
            InitEncoder(m_pSparam[m_icurChan].m_iCurEn, 0);
        }
        #endregion 方法
    }

    public class Tofd_Arr
    {
        /// <summary>
        /// 显示宽度，总采样点数 512
        /// </summary>
        public const int UTS_DATA_WIDTH = 512;
        /// <summary>
        /// 距离信息
        /// </summary>
        public float m_flDistanc_X = 0f;
        /// <summary>
        /// 波峰序列
        /// </summary>
        public byte[] ChannelBuf = new byte[UTS_DATA_WIDTH];
        /// <summary>
        /// 波谷序列
        /// </summary>
        public byte[] ValueBuf = new byte[UTS_DATA_WIDTH];
    }
    /// <summary>
    /// 测量数据
    /// </summary>
    public class Class_X_Data
    {
        /// <summary>
        /// 母材厚度
        /// </summary>
        public float m_flThick = 0.5f;
        /// <summary>
        /// 结论true:合格 false:不合格
        /// </summary>
        public bool blJL = true;
        /// <summary>
        /// 焊缝正面还是背面  true:正面 false:背面
        /// </summary>
        public bool m_blZm1_Bm0 = true;
        /// <summary>
        /// 对接：false  角焊：true
        /// </summary>
        public bool m_blDuijie0_JiaoHan1 = false;
        /// <summary>
        /// 凸面个数
        /// </summary>
        public int m_iBall = 0;
        /// <summary>
        /// 是否有缺陷（凸面）
        /// </summary>
        public bool m_blBall = false;
        /// <summary>
        /// 是否有缺陷（凹陷）
        /// </summary>
        public bool m_blAlarm = false;
        /// <summary>
        /// 一帧数据长度
        /// </summary>
        public int m_iprofileCnt = 0;
        /// <summary>
        /// 母材厚度:统计的最低值
        /// </summary>
        public double m_dbL_Min_H = 0;
        /// <summary>
        /// 最大值
        /// </summary>
        public double m_dbL_Max_H = 0;

        /// <summary>
        /// 焊缝
        /// </summary>
        public Class_Weld m_clsWeld = new Class_Weld();
        /// <summary>
        /// 当前帧对应的单轮廓的X轴位置数组
        /// </summary>
        public double[] m_Arr_profileX = new double[1000];

        /// <summary>
        /// 当前帧对应的单轮廓的Z轴方向深度数组
        /// </summary>
        public double[] m_Arr_profileZ = new double[1000];

        /// <summary>
        /// 当前帧数据对应的缺陷（凹陷）
        /// </summary>
        public List<Class_Alarm> m_lstAlarm = new List<Class_Alarm>();
        /// <summary>
        /// 凹陷个数
        /// </summary>
        public int iDownNum = 0;


    }

    public class Class_Weld
    {
        /// <summary>
        /// 是否有焊缝
        /// </summary>
        public bool m_blHave = false;
        /// <summary>
        /// 
        /// </summary>
        public int iType = 0;
        /// <summary>
        /// 焊缝与母材左边交点 X值
        /// </summary>
        public float m_i_W_Start_X = -1;
        /// <summary>
        /// 焊缝与母材左边交点  Y值
        /// </summary>
        public double m_i_W_Start_Y = -1;
        /// <summary>
        /// 焊缝与母材右边交点 X值
        /// </summary>
        public float m_i_W_End_X = -1;
        /// <summary>
        /// 焊缝与母材右边交点 Y值
        /// </summary>
        public double m_i_W_End_Y = -1;
        /// <summary>
        /// 两条母材边延长线相交的X点
        /// </summary>
        public double db_Line_LR_X = -1;
        /// <summary>
        /// 两条母材边延长线相交的Y点
        /// </summary>
        public double db_Line_LR_Y = -1;
        /// <summary>
        /// 母材角点与此角平分线与母材焊缝顶面交点距离
        /// </summary>
        public double db_Line_3_Mucai_H = -1;
        /// <summary>
        /// 计算最短距离的应一边的交点  X点
        /// </summary>
        public int i_MinDist_X = -1;
        /// <summary>
        /// 计算最短距离的应一边的交点  Y点
        /// </summary>
        public int i_MinDist_Y = -1;
        /// <summary>
        /// 焊缝余高:与焊缝面高度比较， 内角测量点到线短，外角测量点到线则长
        /// </summary>
        public double m_dbCentWeld_H = -1;


        /// <summary>
        /// 焊缝宽度
        /// </summary>
        public double m_dbCentWeld_W = -1;
        /// <summary>
        /// 点到直线的距离
        /// </summary>
        public double m_dbPoint_Line4 = -1;
        /// <summary>
        /// 焊瘤点 X
        /// </summary>
        public double m_Point_4_x = -1;//
        /// <summary>
        ///  焊瘤点 Y
        /// </summary>
        public double m_Point_4_y = -1;//
        /// <summary>
        /// 角焊接过渡部队称时正常厚度a（没有余高）
        /// </summary>
        public double m_Point_4_a = -1;
        /// <summary>
        /// 角焊接过渡部队称时焊瘤长度h
        /// </summary>
        public double m_Point_4_h = -1;
        /// <summary>
        /// 结论true:合格 false:不合格
        /// </summary>
        public bool blJL = true;
        /// <summary>
        /// 结论
        /// </summary>
        public List<csJL> m_lstJL = new List<csJL>();
    }
    /// <summary>
    /// 结论类
    /// </summary>
    public class csJL
    {
        /// <summary>
        /// 检查内容
        /// </summary>
        public string strType = "";
        /// <summary>
        /// 缺陷编号
        /// </summary>
        public int iType = -1;
        /// <summary>
        ///  根部凹入 结论 -1 1：合格：0：不合格
        /// </summary>
        public int iJL = -1;
        /// <summary>
        /// 结论内容
        /// </summary>
        public string strJL = "";
    }
    /// <summary>
    /// INI文件操作类
    /// </summary>
    public class csInterface
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
            if (strPathFileName == "") return;
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
#pragma warning disable CS0168 // 声明了变量“e”，但从未使用过
                catch (Exception e)
#pragma warning restore CS0168 // 声明了变量“e”，但从未使用过
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
#pragma warning disable CS0168 // 声明了变量“e”，但从未使用过
                catch (Exception e)
#pragma warning restore CS0168 // 声明了变量“e”，但从未使用过
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
        public void WaitTime(double dbWait, bool m_blCloseApp = false)
        {
            DateTime dtStar = DateTime.Now;
            while (true)
            {
                try
                {
                    if (m_blCloseApp) break;
                    Application.DoEvents();
                    if (DateTime.Now.Subtract(dtStar).TotalSeconds > dbWait) break;
                    System.Threading.Thread.Sleep(10);
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
        public bool FileCopy(string OldPathFile, string NewPathFile)
        {
            bool blRet = false;
            try
            {
                System.IO.File.Copy(OldPathFile, NewPathFile, true);
                blRet = System.IO.File.Exists(NewPathFile);
            }
#pragma warning disable CS0168 // 声明了变量“e”，但从未使用过
            catch (Exception e)
#pragma warning restore CS0168 // 声明了变量“e”，但从未使用过
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
#pragma warning disable CS0168 // 声明了变量“e”，但从未使用过
            catch (Exception e)
#pragma warning restore CS0168 // 声明了变量“e”，但从未使用过
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
        public void DeleFile(string strPathFile, int iType = 0)
        {
            try
            {
                if (strPathFile == "") return;
                string strPath = "";
                int iS = strPathFile.IndexOf('\\');
                if (iS == 0)
                    strPath = Application.StartupPath;// AppDomain.CurrentDomain.BaseDirectory;
                else
                    strPath = AppDomain.CurrentDomain.BaseDirectory;

                string _strPathFile = strPath + strPathFile;
                if (iType == 1 && strPathFile != "")
                    _strPathFile = strPathFile;
                System.IO.File.Delete(_strPathFile);
            }
            catch (Exception Err)
            {
                MessageBox.Show("Delete Files：" + strPathFile + " error！" + Err.Message);
            }
        }

    }
    /// <summary>
    /// 凸起物
    /// </summary>
    public class Class_ball
    {
        /// <summary>
        /// 统计降落时个数
        /// </summary>
        public int iDownNum = 0;
        /// <summary>
        /// 起点值
        /// </summary>
        public double dbStartVal = 0;
        /// <summary>
        /// 开始扬起点
        /// </summary>
        public int iStart = -1;
        /// <summary>
        /// 回落点
        /// </summary>
        public int iEnd = -1;
        /// <summary>
        /// 高度
        /// </summary>
        public double flHeight = 0;
        /// <summary>
        /// 高度位置
        /// </summary>
        public int iHeightPoint = -1;

    }

    /// <summary>
    /// 焊缝位置
    /// </summary>
    public class Class_Weld_Position
    {
        public bool m_blAlarm = false;
        /// <summary>
        /// 缺陷X轴开始位置：数组序号，是实际值/10
        /// </summary>
        public int i_Start = -1;
        /// <summary>
        /// 缺陷X轴结束位置
        /// </summary>
        public int i_End = -1;
        /// <summary>
        /// 缺陷X轴最深处深度
        /// </summary>
        public double dbDepth = -1;
        /// <summary>
        /// 焊缝位置
        /// </summary>
        public int i_Cent = -1;
        /// <summary>
        /// 斜率
        /// </summary>
        public float f_K = -99;
    }
    /// <summary>
    /// 一帧数据中缺陷数据
    /// </summary>
    public class Class_Alarm
    {
        /// <summary>
        /// 缺陷类型 0：母材上凹陷 1：凸起  2：咬边 3 母材上凹陷 -1:取消此缺陷
        /// </summary>
        public int i_Type = -1;
        /// <summary>
        /// 缺陷X轴开始位置：数组序号，是实际值/10
        /// </summary>
        public int i_Start = -1;
        /// <summary>
        /// 缺陷X轴结束位置
        /// </summary>
        public int i_End = -1;
        /// <summary>
        /// 缺陷X轴最深处深度
        /// </summary>
        public double dbDepth = -1;
        /// <summary>
        /// 起点深度
        /// </summary>
        public double dbDepth_S = -1;
        /// <summary>
        /// 是否已经刷新图
        /// </summary>
        public bool bl_Show = false;


        /// <summary>
        /// 结论true:合格 false:不合格
        /// </summary>
        public bool blJL = true;
        /// <summary>
        /// 结论
        /// </summary>
        public List<csJL> m_lstJL = new List<csJL>();
    }
    /// <summary>
    /// C整体图方位变量
    /// </summary>
    public class CLS_C_ALL
    {
        /// <summary>
        /// 后续检测在当前位置的  左边：0 false右边：1 true
        /// </summary>
        public bool bl_R1_L0 = true;
        /// <summary>
        /// 单行C扫描的行数        n   
        /// </summary>
        public int Scree_iAllRows_C = 0;
        /// <summary>
        /// C扫描一屏幕显示行数 n
        /// </summary>
        public int i_ScreenRows = 4;

        /// <summary>
        /// 一个点的高度
        /// </summary>
        public float Scree_flDotHeight = 3;

        /// <summary>
        /// 一个点的高度
        /// </summary>
        public float Scree_flDotHeight_Coat = 3;
        /// <summary>
        /// 当前整体C扫描图屏幕序号 0-N
        /// </summary>
        public int i_Screen_No = -1;

        /// <summary>
        /// 多通道模式时：记录当前屏幕开始序号 1-N
        /// </summary>
        public int i_Screen_Row_Start = 1;
        /// <summary>
        /// 当前页面对应的：开始行号 1-N
        /// </summary>
        public int i_Row_Start = 1;
        /// <summary>
        /// 当前页面对应的：结束行号 1-M
        /// </summary>
        public int i_Row_End = 4;
        /// <summary>
        /// 本页面开始距离
        /// </summary>
        public float fl_Dist_S = 0;
        /// <summary>
        /// 本页面结束距离
        /// </summary>
        public float fl_Dist_E = 0;
    }
    #region 脉冲涡流
    /// <summary>
    /// 单探头数据
    /// </summary>
    public class CLECT_Data
    {
        /// <summary>
        /// 探头IP 192.168.1.101/ 101-255
        /// </summary>
        //public string Ip = "192.168.1.101";
        /// <summary>
        /// Ip第四位
        /// </summary>
        public int i_Ip = 0;
        /// <summary>
        /// 数据是否保存
        /// </summary>
        public int m_i_SaveData = 0;
        /// <summary>
        /// 联机状态
        /// </summary>
        public bool blLink = false;
        /// <summary>
        /// 当前通讯
        /// </summary>
        public bool blCurrCmm = false;

        /// <summary>
        /// 当前对应时分秒毫秒（100毫秒）
        /// </summary>
        public string strCurrtTime = "";// DateTime.Now.ToString("HHmmssf");

        /// <summary>
        /// X轴距离
        /// </summary>
        public float flDistance_X = 0;
        /// <summary>
        /// Y轴位置
        /// </summary>
        public float flDistance_Y = 0;

        /// <summary>
        /// 波形类型 0：标定数据 1：通道数据
        /// </summary>
        public int iWaveType = 1;
        /// <summary>
        /// 数据标定序号，用此数据方便实时涡流数据与标定数据一一对应
        /// </summary>
        public int iWaveStandNo = -1;
        /// <summary>
        /// 采集厚度值
        /// </summary>
        public float flThick = 0;
        /// <summary>
        /// 误差
        /// </summary>
        public float flWc = 0;
        /// <summary>
        /// 厚度百分值
        /// </summary>
        public float Per_Thick = 0;

        public int m_iArr_Len = 201;
        /// <summary>
        /// 波形原始数据 实时数据0：百分值 后面为实时电压幅度数据
        /// 标定数据：没有百分值，全部为实时电压数据
        /// </summary>
        public float[] m_iArrWave = new float[201];
        /// <summary>
        /// 数据保存波形，以","间隔数据
        /// </summary>
        public string strWave = "";

        /// <summary>
        /// 数据是否报警 1：报警 0：正常
        /// </summary>
        public bool  blAlarm = false ;
        /// <summary>
        /// 数据对应颜色值
        /// </summary>
        public string strColor = "";

        public int R = 0;
        public int G = 0;
        public int B = 0;
        /// <summary>
        /// Flag对应含义
        /// </summary>
        public string strFlagMsg = "";

    }
    /// <summary>
    /// 标定数据
    /// </summary>
    public class CL_ECT_Bd
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int ID;
        /// <summary>
        /// 标定的报文
        /// </summary>
        public float[] m_iArrWave = new float[201];
    }
    #endregion 脉冲涡流

    #region 图像
    public struct Struct_G
    {
        public Bitmap image;
        // 获取背景层
        public Bitmap bg;
        // 初始化整个画布
        public Bitmap canvas;
        // 初始化图形面板，获取这块内存画布的Graphics的引用
        public Graphics g;
        public Graphics gb;

        public int iCs_Num;
        /// <summary>
        /// 数据缓存：根据X、Y填写测量点数据
        /// </summary>
        public PointF[] Buff;
    }


    /// <summary>
    /// 图像控制
    /// </summary>
    public class Class_Plant
    {
        public float[] m_flArr_Rul_S = new float[900];
        public float[] m_flArr_Rul_E = new float[900];
        public void Cal_S_E()
        {
            m_flArr_Rul_S = new float[900];
            m_flArr_Rul_E = new float[900];

            for (int i = 0; i < m_flArr_Rul_S.Count(); i++)
            {
                if (i == 0)
                {
                    m_flArr_Rul_S[i] = float.Parse((i * (Scree_Stant_Distance + Scree_iDotWithmm_X)).ToString("f3"));
                    m_flArr_Rul_E[i] = float.Parse((i * (Scree_Stant_Distance + Scree_iDotWithmm_X) + Scree_Stant_Distance).ToString("f3")); // + Scree_iDotWithmm_X-0.001).ToString("f3"));
                }
                else
                {
                    m_flArr_Rul_S[i] = m_flArr_Rul_E[i - 1];
                    m_flArr_Rul_E[i] = float.Parse((i * (Scree_Stant_Distance) + Scree_Stant_Distance).ToString("f3"));
                }
            }
        }
        public int Get_No(float flDistance)
        {
            //  Cal_S_E();
            flDistance = float.Parse(flDistance.ToString("f1"));
            int iNo = 0;
            if (m_flArr_Rul_S[0] == 0 && m_flArr_Rul_E[0] == 0)
                return 0;
            //    Cal_S_E();
            for (int i = 0; i < m_flArr_Rul_S.Count(); i++)
            {
                if (flDistance >= m_flArr_Rul_S[i] && (flDistance <= m_flArr_Rul_E[i]))//|| flDistance <= m_flArr_Rul_S[i+1]))
                {
                    iNo = i;
                    break;
                }
                if (m_flArr_Rul_S[i] == 0 && m_flArr_Rul_E[i] == 0)
                {
                    iNo = i;
                    break;
                }
            }
            return iNo;
        }
        /// <summary>
        /// 是否计算公称厚度  true:计算  false: 不计算
        /// </summary> 
        public bool Ck_No_Normal_Thickness = true;
        /// <summary>
        /// 是否退出
        /// </summary>
        public bool m_blOut = false;
        /// <summary>
        /// 锁
        /// </summary>
        public object lockValue_Pic = new object();
        /// <summary>
        /// 通道个数
        /// </summary>
        public int m_iRomoteNum = 1;

        /// <summary>
        /// 是否浏览回放
        /// </summary>
        public bool m_blBrowse = false;
        /// <summary>
        /// 当前运行时对应队列屏号
        /// </summary>
        public int m_iCurRuning_No = 0;
        /// <summary>
        /// 画尺子后列个数
        /// </summary>
        public int m_iCol_HeardNum = 0;
        /// <summary>
        /// 用于B扫多行鼠标移动查看时用
        /// true:程序运行  false:没有运行，显示数据为初始化程序时调用数据库数据
        /// </summary>
        public bool m_blHaveRun = true;
        ///// <summary>
        ///// 当前点运行屏幕序号
        ///// </summary>
        //public int iCurrRunRulerNo = 0;
        /// <summary>
        /// 画图距离：可以最远距离给，基本和测量差距不大，类似梯形图上下边的最长边距离；也可以由上一行数据给，基本是矩形框
        /// </summary>
        public float Check_iDistance_Plant = 0;
        /// <summary>
        /// 幅度、dB刻度切换 0：幅度 1：dB
        /// </summary>
        public int m_Per_dB = 0;
        /// <summary>
        /// 相位切换 使用平板和台体  0:台体 1：平板
        /// </summary>
        public int m_iAsus = 0;
        /// <summary>
        /// 幅度不对应时，调整基准
        /// </summary>
        public int m_iHigh = 100;

        /// <summary>
        /// 上次填充位置
        /// </summary>
        public int m_iLastNo = 0;
        /// <summary>
        /// 多波形色标缓存
        /// </summary>
        public System.Drawing.Color[] m_800Limit = null;
        /// <summary>
        /// 运行位置标记
        /// </summary>
       // public RunMark m_RunMark = new RunMark();
        /// <summary>
        /// 不行步长
        /// </summary>
        public int m_iWave_Step_Type = 2;
        /// <summary>
        /// 是否可以开始绘波形图
        /// </summary>
        public bool m_blCanPlantWave = true;
        /// <summary>
        /// 重绘尺子
        /// </summary>
        public bool blRepPlantRuler = false;
        /// <summary>
        /// 重绘屏内容类型 0：磁爬从起始行开始 1：当前行右边出界开始  2：直线有返回的从磁爬换行最右开始 3：直线有返回从左边出界
        /// </summary>
        public int iRepPlantType = 0;
        /// <summary>
        /// 0、1正在画刻度尺  2、3画灰度图  4：行超出后，画新屏刻度、画指定区域图
        /// </summary>
        public int blPlantRuler = 0;
        /// <summary>
        /// 画图方式 0：探头事件驱动  1：循环画图
        /// </summary>
        public int iPlant_Event = 0;
        /// <summary>
        /// 配置文件名
        /// </summary>
        public string HardFileName = "";
        /// <summary>
        /// 屏幕B图形变量
        /// </summary>
        public System.Windows.Forms.PictureBox PicArea = null;
        /// <summary>
        /// 屏幕多图类似C图形变量
        /// </summary>
        public System.Windows.Forms.PictureBox PicArea_TowChannel = null;
        /// <summary>
        /// 屏幕B1图波形变量
        /// </summary>
        public System.Windows.Forms.PictureBox PicArea_B_Wave = null;
        /// <summary>
        /// 屏幕B1图多次波形变量
        /// </summary>
        public System.Windows.Forms.PictureBox PicArea_B_Wave_Mul = null;
        /// <summary>
        /// 屏幕B1图形变量
        /// </summary>
        public System.Windows.Forms.PictureBox PicArea_B = null;

        /// <summary>
        /// 屏幕C图形变量
        /// </summary>
        public System.Windows.Forms.PictureBox PicArea_C = null;



        /// <summary>
        /// 图像变量
        /// </summary>
        public Struct_G m_G = new Struct_G();

        /// <summary>
        /// C图像变量
        /// </summary>
        public Struct_G m_G_C = new Struct_G();

        /// <summary>
        /// 双通道刷屏次数
        /// </summary>
        public int m_i_Num_C_TowChannel = 0;
        /// <summary>
        /// 2个通道C图像变量
        /// </summary>
        public Struct_G m_G_C_TowChannel = new Struct_G();
        /// <summary>
        /// 1张B图变量
        /// </summary>
        public Struct_G m_G_B_One = new Struct_G();


        /// <summary>
        /// 1张B图变量
        /// </summary>
        public Struct_G m_G_B_One_Wave = new Struct_G();

        /// <summary>
        /// 1张B图多次波形图变量
        /// </summary>
        public Struct_G m_G_PicArea_B = new Struct_G();

        #region 浏览变量
        /// <summary>
        /// 500个数据的灰度图
        /// </summary>
        Struct_G m_G_PicArea_C = new Struct_G();
        /// <summary>
        /// B图全部图
        /// </summary>
#pragma warning disable CS0414 // 字段“Class_Plant.m_G_Browse_B”已被赋值，但从未使用过它的值
        Struct_G m_G_Browse_B = new Struct_G();
#pragma warning restore CS0414 // 字段“Class_Plant.m_G_Browse_B”已被赋值，但从未使用过它的值
        /// <summary>
        /// 浏览C图
        /// </summary>
#pragma warning disable CS0414 // 字段“Class_Plant.m_G_Browse_C”已被赋值，但从未使用过它的值
        Struct_G m_G_Browse_C = new Struct_G();
#pragma warning restore CS0414 // 字段“Class_Plant.m_G_Browse_C”已被赋值，但从未使用过它的值
        #endregion 浏览变量

        #region  屏幕尺子
        /// <summary>
        /// 波形幅度调整
        /// </summary>
        public int iWave_Adjust_range_filter = 25;
        /// <summary>
        /// 1张B时图片占界面的比例 百分制 默认63%
        /// </summary>
        public int Ratio_Pic_Grup = 63;
        /// <summary>
        /// 画图类型 0：C扫图 1：B扫图
        /// </summary>
        public int Scree_Chart_B_c = 0;
        /// <summary>
        /// 屏幕宽度
        /// </summary>
        public int Scree_iScreenWith = 0;
        /// <summary>
        /// 屏幕高度
        /// </summary>
        public int Scree_iScreenHeight = 0;
        /// <summary>
        /// true;//以1毫米为数据间隔  false:以列表距离间隔为依据画列表
        /// </summary>
        public bool blGrdShowJgType = false;
        /// <summary>
        /// 一个点代表距离  默认0.5毫米  单位m
        /// </summary>
        public float Scree_iDotWithmm_X = 0.5f;
        /// <summary>
        /// 应用磁爬控制速度：一个点代表距离  默认10毫米  单位m
        /// </summary>
        public float Scree_iDotWithmm_X_Climb = 10;
        /// <summary>
        /// 一个点宽度
        /// </summary>
        public int Scree_iDotWith_X = 1;
        /// <summary>
        /// 一个点高度
        /// </summary>
        public int Scree_iDotHeight = 1;


        /// <summary>
        /// B扫纵轴一个点代表距离  默认1毫米
        /// </summary>
        public int Scree_iDotHeightmm_B_Y = 1;
        /// <summary>
        /// B扫纵轴一个点高度
        /// </summary>
        public int Scree_iDotHeight_B_Y = 1;
        /// <summary>
        /// B扫纵轴一个点最大厚度比公称厚度多mm 默认5毫米
        /// </summary>
        public int Scree_iDotHeight_B_Y_Addmm = 5;
        /// <summary>
        /// 公称厚度 画图使用
        /// </summary>
        public float flNormal_Thickness = 0;

        /// <summary>
        /// 双通道公称厚度
        /// </summary>
        public float flNormal_Thickness_2 = 0;
        /// <summary>
        /// 报警线
        /// </summary>
        public float flstrThickAlarm = 0;
        /// <summary>
        /// 一条数据占最大行数
        /// </summary>
        public int Scree_iDataMaxHeight = 0;
        /// <summary>
        /// B图时一条数据占最大行数
        /// </summary>
        public int Scree_iDataMaxHeight_B_One = 0;
        /// <summary>
        /// B扫时一屏显示总数据个数
        /// </summary>
        public int Scree_iAllDataNum_B = 0;

        /// <summary>
        /// 屏幕总行数 ：由屏幕高度和一个点高度得到   B扫图：数据行数
        /// </summary>
        public int Scree_iAllRows = 0;

        public int Scree_iAllRows_C = 0;
        /// <summary>
        /// B1屏幕总行数 ：由屏幕高度和一个点高度得到   B扫图：数据行数
        /// </summary>
        public int Scree_iAllRows_B_One = 0;
        /// <summary>
        /// 屏幕总列数：由屏幕宽度和一个点宽度得到
        /// </summary>
        public int Scree_iAllCols = 0;
        /// <summary>
        /// B图屏幕总列数：1
        /// </summary>
        public int Scree_iAllCols_B_One = 0;
        /// <summary>
        /// 一屏标准长度
        /// </summary>
        public float Scree_Stant_Distance = 0;
        /// <summary>
        /// 刻度按照 true:米 false:毫米
        /// </summary>
        public bool m_blM_mm = false;
        #endregion 屏幕尺子

        #region 图形运行变量
        /// <summary>
        /// 图形左起开始位置坐标 单位 m
        /// </summary>
        public float Chart_Run_flStart_Distance = 0;
        /// <summary>
        /// 图形右边结束位置坐标 单位 m
        /// </summary>
        public float Chart_Run_flEnd_Distance = 0;
        /// <summary>
        /// 重绘图形
        /// </summary>
        public bool m_blRepPlant = false;
        /// <summary>
        /// 图形左起开始位置坐标 单位 m
        /// </summary>
        public float Chart_Run_flStart_Distance_B_One = 0;
        /// <summary>
        /// 图形右边结束位置坐标 单位 m
        /// </summary>
        public float Chart_Run_flEnd_Distance_B_One = 0;


        /// <summary>
        /// 查询图形左起开始位置坐标 单位 m  
        /// 0：B一张图 1：B全图  2：C图
        /// </summary>
        public float[] Chart_Run_flStart_Distance_Browse = new float[5];
        /// <summary>
        /// 查询图形右边结束位置坐标 单位 m
        /// 0：B一张图 1：B全图  2：C图
        /// </summary>
        public float[] Chart_Run_flEnd_Distance_Browse = new float[5];


        /// <summary>
        /// 查询图形开始行号
        /// 0：B一张图 1：B全图  2：C图
        /// </summary>
        public int[] Chart_Run_iStart_Row_Browse = new int[5];
        /// <summary>
        /// 查询图形右边结束结束行号
        /// 0：B一张图 1：B全图  2：C图
        /// </summary>
        public int[] Chart_Run_iEnd_Row_Browse = new int[5];


        /// <summary>
        /// 查询屏幕列数  0：B一张图 1：B全图  2：C图
        /// </summary>
        public int[] Scree_iAllCols_Browse = new int[5];
        /// <summary>
        /// 查询屏幕宽度  0：B一张图 1：B全图  2：C图
        /// </summary>
        public float[] Scree_Stant_Distance_Browse = new float[5];
        /// <summary>
        /// 查询屏幕数据行数  0：B一张图 1：B全图  2：C图
        /// </summary>
        public int[] Scree_iAllRows_Browse = new int[5];
        public bool[] Scree_iAllRows_Browse_Bl = new bool[5];
        /// <summary>
        /// 查询一条数据占多少行数
        /// </summary>
        public int Scree_iDataMaxHeight_Browse_B = 0;


        /// <summary>
        /// 过山车有过原点方式，坐标两侧距离都会有数据 0: 初始距离数据 1: 过原点厚度距离数据
        /// </summary>
        public int m_iFx_Gsc = 0;

        /// <summary>
        /// 图形对应缓存开始行号
        /// </summary>
        public int Chart_Run_iStartRow_Buff = 0;
        /// <summary>
        /// 2通道换屏开始行号
        /// </summary>
        public int Chart_Run_iStartRow_Buff_TowChannel = 0;
        /// <summary>
        /// B1张图图形对应缓存开始行号
        /// </summary>
        public int Chart_Run_iStartRow_Buff_B_One = 0;
        /// <summary>
        /// 图形对应缓存当前运行行号
        /// 
        /// 1 增加规则：接收到磁爬完成命令了，并且接收到有效数据后，Chart_Run_iEndRow_Buff=iBuff_Rows赋值
        /// 2 同时判断：当 Chart_Run_iEndRow_Buff -Chart_Run_iStartRow_Buff +1 >iScree_AllRows 时，Chart_Run_iStartRow_Buff=Chart_Run_iEndRow_Buff
        /// </summary>
        public int Chart_Run_iEndRow_Buff = 0;
        /// <summary>
        /// B1张图
        /// </summary>
        public int Chart_Run_iEndRow_Buff_B_One = 0;
        /// <summary>
        /// 语言0：中文  1：英文
        /// </summary>
        public int m_iLanguage = 0;
        /// <summary>
        /// 当前图形是否出右边，用于换行时画屏 true:当前屏幕有过右出界 false:没有出界
        /// </summary>
        public bool Chart_blRightOut = false;
        #endregion 图形运行变量

        #region 图形查询变量:运行时记录当前图形对应位置
        /// <summary>
        /// 奇数行缓存计算右超屏刻度 1-N
        /// </summary>
        public float Chart_B_One_flEnd_Distance = 0;
        /// <summary>
        /// 偶数行缓存计算左超屏刻度 1-N
        /// </summary>
        public float Chart_B_One_flStart_Distance = 0;
        /// <summary>
        /// 查询图形左起开始位置坐标
        /// </summary>
        public float Chart_Brows_B_One_flStart_Distance = 0;
        /// <summary>
        /// 查询图形右边结束位置坐标
        /// </summary>
       // public float Chart_Brows_B_One_flEnd_Distance = 0;
        /// <summary>
        /// 查询图形左起开始缓存序号
        /// </summary>
        public int Chart_Brows_B_One_iStart_ColNo = 0;
        /// <summary>
        /// 查询图形右边结束缓存序号
        /// </summary>
        public int Chart_Brows_B_One_iEnd_ColNo = 0;
        /// <summary>
        /// 是否过山车 0：不是1：是
        /// </summary>
        public int Ck_Gsc = 0;
        /// <summary>
        /// 查询图形对应缓存行号
        /// </summary>
        public int Chart_Brows_B_One_Buff_iCurr_Row = 0;
        #endregion 图形查询变量

        #region 画图中字体大小
        /// <summary>
        /// 尺子字体大小
        /// </summary>
        public int Chart_Ruler_font = 8;
        ///// <summary>
        ///// 字体颜色
        ///// </summary>
        //public Color Chart_Ruler_BrushColor = Color.Wheat;
        Font drawFont = new Font("Arial", (float)10);
        SolidBrush drawBrush = new SolidBrush(Color.Black);

        SolidBrush drawBrush_One = new SolidBrush(Color.Black);
        /// <summary>
        /// 横轴刻度字在刻度线上左偏移刻度量
        /// </summary>
        public int Chart_Ruler_Word_X = 2;
        /// <summary>
        /// 横轴纵轴刻度字下降量
        /// </summary>
        public int Chart_Ruler_Word_H = 2;
        /// <summary>
        /// 线段颜色
        /// </summary>
        Pen Ruler_p = new Pen(Brushes.Black);

        Pen Ruler_p_One = new Pen(Brushes.Black);
        /// <summary>
        /// 报警线
        /// </summary>
        Pen Ruler_p_Limit = new Pen(Brushes.Red);
        /// <summary>
        /// 图形坐标：横轴开始像素位置
        /// </summary>
        public int Chart_Ruler_X_Start = 10;
        /// <summary>
        /// 左偏移量
        /// </summary>
        public int Plant_Offset = 2;
        /// <summary>
        /// 图形坐标：纵轴开始像素位置
        /// </summary>
        public int Chart_Ruler_Y_Start = 10;
        /// <summary>
        /// B扫双通道时起始位置
        /// </summary>
        public int Chart_Ruler_Y_Start_TowChannel = 0;
        /// <summary>
        /// 实际数据的高度，分成两半，中间加一个像素的宽度
        /// </summary>
      // public  int iScreen_Height = 0;//
        /// <summary>
        /// 图形坐标：刻度小数位
        /// </summary>
        public int Chart_Ruler_Xsw = 3;
        #endregion 画图中字体大小

        #region 数据缓存行号
        /// <summary>
        /// 0-N  增加规则：接收到磁爬完成命令了，并且接收到有效数据后，加1
        /// </summary>
        public int Buff_iRows = 0;

        #endregion 数据缓存行号
        /// <summary>
        /// 图像类参数读写 0：读 1：写
        /// </summary>
        /// <param name="iType"></param>
        public bool Init(int iType = 0)
        {
            bool _blRet = false;
            csInterface csInter = new csInterface();
            if (HardFileName == "") HardFileName = Application.StartupPath + "\\database\\SysConfig.ini";

            if (iType == 0)
            {
                Ck_No_Normal_Thickness = int.Parse(csInter.IniReadDefine("Class_Plant", "Ck_No_Normal_Thickness", "1", HardFileName)) == 1;
                blGrdShowJgType = int.Parse(csInter.IniReadDefine("Class_Plant", "blGrdShowJgType", "1", HardFileName)) == 1;
                m_blM_mm = int.Parse(csInter.IniReadDefine("Class_Plant", "m_blM_mm", "1", HardFileName)) == 1;
                m_iHigh = int.Parse(csInter.IniReadDefine("Class_Plant", "m_iHigh", "127", HardFileName));
                m_iAsus = int.Parse(csInter.IniReadDefine("Class_Plant", "m_iAsus", "1", HardFileName));
                m_iWave_Step_Type = int.Parse(csInter.IniReadDefine("Class_Plant", "m_iWave_Step_Type", "2", HardFileName));
                iWave_Adjust_range_filter = int.Parse(csInter.IniReadDefine("Class_Plant", "iWave_Adjust_range_filter", "25", HardFileName));
                Ratio_Pic_Grup = int.Parse(csInter.IniReadDefine("Class_Plant", "Ratio_Pic_Grup", "63", HardFileName));
                Scree_Chart_B_c = int.Parse(csInter.IniReadDefine("Class_Plant", "Scree_Chart_B_c", "1", HardFileName));

                Scree_iDotHeight_B_Y_Addmm = int.Parse(csInter.IniReadDefine("Class_Plant", "Scree_iDotHeight_B_Y_Addmm", "0", HardFileName));
                Scree_iDotHeightmm_B_Y = int.Parse(csInter.IniReadDefine("Class_Plant", "Scree_iDotHeightmm_B_Y", "0", HardFileName));
                Scree_iDotHeight_B_Y = int.Parse(csInter.IniReadDefine("Class_Plant", "Scree_iDotHeight_B_Y", "0", HardFileName));

                iPlant_Event = int.Parse(csInter.IniReadDefine("Class_Plant", "iPlant_Event", "0", HardFileName));
                Scree_iDotWithmm_X = float.Parse(csInter.IniReadDefine("Class_Plant", "Scree_iDotWithmm_X", "0.5", HardFileName));
                //Scree_iDotWithmm_X *= 0.001F;
                //              Scree_iDotWithmm_X = float.Parse(Scree_iDotWithmm_X.ToString("f3"));
                Scree_iDotWithmm_X_Climb = int.Parse(csInter.IniReadDefine("Class_Plant", "Scree_iDotWithmm_X_Climb", "10", HardFileName));
                Scree_iDotWithmm_X_Climb *= 0.001F;
                Scree_iDotWithmm_X_Climb = float.Parse(Scree_iDotWithmm_X_Climb.ToString("f3"));

                Scree_iDotWith_X = int.Parse(csInter.IniReadDefine("Class_Plant", "Scree_iDotWith_X", "1", HardFileName));
                Scree_iDotHeight = int.Parse(csInter.IniReadDefine("Class_Plant", "Scree_iDotHeight", "1", HardFileName));

                Chart_Run_flStart_Distance = float.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Run_flStart_Distance", "0", HardFileName));
                Chart_Run_flEnd_Distance = float.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Run_flEnd_Distance", "10", HardFileName));
                Chart_Run_iStartRow_Buff = int.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Run_iStartRow_Buff", "0", HardFileName));
                Chart_Run_iEndRow_Buff = int.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Run_iEndRow_Buff", "10", HardFileName));

                Chart_Run_flStart_Distance_B_One = float.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Run_flStart_Distance_B_One", "0", HardFileName));
                Chart_Run_flEnd_Distance_B_One = float.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Run_flEnd_Distance_B_One", "10", HardFileName));
                Chart_Run_iStartRow_Buff_B_One = int.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Run_iStartRow_Buff_B_One", "0", HardFileName));
                Chart_Run_iEndRow_Buff_B_One = int.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Run_iEndRow_Buff_B_One", "10", HardFileName));

                Chart_Ruler_font = int.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Ruler_font", "8", HardFileName));
                Chart_Ruler_Word_X = int.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Ruler_Word_X", "2", HardFileName));
                Chart_Ruler_Word_H = int.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Ruler_Word_H", "2", HardFileName));
                Chart_Ruler_X_Start = int.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Ruler_X_Start", "10", HardFileName));
                Chart_Ruler_Y_Start = int.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Ruler_Y_Start", "0", HardFileName));
                Chart_Ruler_Xsw = int.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Ruler_Xsw", "2", HardFileName));
            }
            else
            {
                csInter.INIWriteValue("Class_Plant", "Ck_No_Normal_Thickness", (Ck_No_Normal_Thickness ? "1" : "0"), HardFileName);
                csInter.INIWriteValue("Class_Plant", "blGrdShowJgType", (blGrdShowJgType ? "1" : "0"), HardFileName);
                csInter.INIWriteValue("Class_Plant", "m_blM_mm", (m_blM_mm ? "1" : "0"), HardFileName);
                csInter.INIWriteValue("Class_Plant", "m_blM_mm", (m_blM_mm ? "1" : "0"), HardFileName);
                csInter.INIWriteValue("Class_Plant", "m_iHigh", m_iHigh.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "m_iAsus", m_iAsus.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "m_iWave_Step_Type", m_iWave_Step_Type.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "iWave_Adjust_range_filter", iWave_Adjust_range_filter.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Ratio_Pic_Grup", Ratio_Pic_Grup.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "iPlant_Event", iPlant_Event.ToString(), HardFileName);
                float iW = (float)(Scree_iDotWithmm_X * 1000f);
                csInter.INIWriteValue("Class_Plant", "Scree_Chart_B_c", Scree_Chart_B_c.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Scree_iDotHeight_B_Y_Addmm", Scree_iDotHeight_B_Y_Addmm.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Scree_iDotHeightmm_B_Y", Scree_iDotHeightmm_B_Y.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Scree_iDotHeight_B_Y", Scree_iDotHeight_B_Y.ToString(), HardFileName);

                csInter.INIWriteValue("Class_Plant", "Scree_iDotWithmm_X", iW.ToString("f0"), HardFileName);
                iW = (float)(Scree_iDotWithmm_X_Climb * 1000f);
                csInter.INIWriteValue("Class_Plant", "Scree_iDotWithmm_X_Climb", iW.ToString("f0"), HardFileName);

                csInter.INIWriteValue("Class_Plant", "Scree_iDotWith_X", Scree_iDotWith_X.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Scree_iDotHeight", Scree_iDotHeight.ToString(), HardFileName);

                csInter.INIWriteValue("Class_Plant", "Chart_Run_flStart_Distance", Chart_Run_flStart_Distance.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Chart_Run_flEnd_Distance", Chart_Run_flEnd_Distance.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Chart_Run_iStartRow_Buff", Chart_Run_iStartRow_Buff.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Chart_Run_iEndRow_Buff", Chart_Run_iEndRow_Buff.ToString(), HardFileName);

                csInter.INIWriteValue("Class_Plant", "Chart_Run_flStart_Distance_B_One", Chart_Run_flStart_Distance_B_One.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Chart_Run_flEnd_Distance_B_One", Chart_Run_flEnd_Distance_B_One.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Chart_Run_iStartRow_Buff_B_One", Chart_Run_iStartRow_Buff_B_One.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Chart_Run_iEndRow_Buff_B_One", Chart_Run_iEndRow_Buff_B_One.ToString(), HardFileName);

                csInter.INIWriteValue("Class_Plant", "Chart_Ruler_font", Chart_Ruler_font.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Chart_Ruler_Word_X", Chart_Ruler_Word_X.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Chart_Ruler_Word_H", Chart_Ruler_Word_H.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Chart_Ruler_X_Start", Chart_Ruler_X_Start.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Chart_Ruler_Y_Start", Chart_Ruler_Y_Start.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Chart_Ruler_Xsw", Chart_Ruler_Xsw.ToString(), HardFileName);
            }
            return _blRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iType"></param>
        public void Write_One(string strKey, string strVal)
        {
            csInterface csInter = new csInterface();
            if (HardFileName == "") HardFileName = Application.StartupPath + "\\database\\HardConfig.ini";

            csInter.INIWriteValue("Class_Plant", strKey, strVal, HardFileName);
        }
        ///// <summary>
        ///// 计算横轴点个数
        ///// </summary>
        //public void GetRulerPara_B_One()
        //{
        //    if (PicArea_B == null) return;  //
        //    #region 由一个点宽度高度，获得屏幕参数
        //    int iScreen_With = PicArea_B.Width - Chart_Ruler_X_Start;
        //    int iScreen_Height = PicArea_B.Height - Chart_Ruler_Y_Start;

        //    Scree_iAllCols = iScreen_With / Scree_iDotWith_X;
        //    Scree_iAllRows_B_One = 1;
        //    #endregion 一个点宽度高度

        //    Scree_Stant_Distance = Scree_iAllCols * Scree_iDotWithmm_X;
        //}
        /// <summary>
        /// 画B1曲线的刻度
        /// </summary>
        public void Plant_Ruler_H()
        {
            if (PicArea_B == null) return;
#pragma warning disable CS0219 // 变量“i_Y”已被赋值，但从未使用过它的值
            int i_X = 0, i_Y = 0;
#pragma warning restore CS0219 // 变量“i_Y”已被赋值，但从未使用过它的值
            string strT = "";
            int iLine_X = (int)(Chart_Ruler_X_Start + Scree_iAllCols * Scree_iDotWith_X);
            blPlantRuler = 0;
            Chart_Clear(m_G_PicArea_B);
            System.Windows.Forms.PictureBox.CheckForIllegalCrossThreadCalls = false;
            SolidBrush _drawBrush_One = new SolidBrush(Color.White);
            Pen _Ruler_p_One = new Pen(Brushes.White);
            PointF drawPoint;

            #region 画刻度尺
            #region 0 画布准备:初始化、右边出图、满屏时
            // 初始化画板，在内存中建立一块虚拟画布
            m_G_PicArea_B.image = new Bitmap(PicArea_B.ClientSize.Width, PicArea_B.ClientSize.Height);
            // 获取背景层
            m_G_PicArea_B.bg = (Bitmap)PicArea_B.BackgroundImage;
            // 初始化整个画布
            m_G_PicArea_B.canvas = new Bitmap(PicArea_B.ClientSize.Width, PicArea_B.ClientSize.Height);
            // 初始化图形面板，获取这块内存画布的Graphics的引用
            m_G_PicArea_B.g = Graphics.FromImage(m_G_PicArea_B.image);
            m_G_PicArea_B.gb = Graphics.FromImage(m_G_PicArea_B.canvas);
            m_G_PicArea_B.g.Clear(Color.FromArgb(255, 30, 30, 30));// Color.Gray); ;
            m_G_PicArea_B.Buff = new PointF[0];
            float iLeft = Ruler_p_One.Width;
            int iTop = PicArea_B.Top;

            int i_S_H = 0, i_S_L = 0;
            float _flEndKd = 0f;
            #endregion 0 画布准备

            #region 1 画横轴刻度
            i_S_H = Chart_Ruler_Y_Start - 6; i_S_L = Chart_Ruler_Y_Start - 3;
            for (int i = 1; i <= Scree_iAllCols; i++)
            {
                i_X = (int)(Chart_Ruler_X_Start + i * Scree_iDotWith_X);
                if (i_X > PicArea_B.ClientSize.Width) break;

                _flEndKd = Chart_Run_flStart_Distance_B_One + (i * Scree_iDotWithmm_X);

                if (i % 5 != 0)//if (i % 5 != 0)
                    m_G_PicArea_B.g.DrawLine(_Ruler_p_One, new Point(i_X, i_S_L), new Point(i_X, Chart_Ruler_Y_Start));
                else
                    m_G_PicArea_B.g.DrawLine(_Ruler_p_One, new Point(i_X, i_S_H), new Point(i_X, Chart_Ruler_Y_Start));
                if (i % 10 == 0)
                {
                    #region 标记横轴刻度值
                    if (m_blM_mm == false)
                    {
                        //     _flEndKd *= 1000;
                        strT = _flEndKd.ToString("f0") + (i == 10 ? "mm" : "");
                    }
                    else
                        strT = _flEndKd.ToString("f2") + (i == 10 ? "m" : "");

                    drawPoint = new PointF(i_X - 9, Chart_Ruler_Word_H);//- Chart_Ruler_Word_X
                    m_G_PicArea_B.g.DrawString(strT, drawFont, _drawBrush_One, drawPoint);
                    #endregion 标记横轴刻度值
                }
            }
            int _iNo = Get_No(Chart_Run_flStart_Distance_B_One);
            m_flArr_Rul_S[_iNo] = Chart_Run_flStart_Distance_B_One;
            m_flArr_Rul_E[_iNo] = _flEndKd;
            Chart_Run_flEnd_Distance_B_One = _flEndKd;
            //  Chart_Brows_B_One_flEnd_Distance = _flEndKd;

            Chart_Brows_B_One_iEnd_ColNo = Chart_Brows_B_One_iStart_ColNo + Scree_iAllCols - 2;
            m_G_PicArea_B.g.DrawLine(_Ruler_p_One, new Point(Chart_Ruler_X_Start, Chart_Ruler_Y_Start), new Point(iLine_X, Chart_Ruler_Y_Start));
            #endregion 1 画横轴刻度

            #region 3 刷新
            Rectangle _Rect_Kd = new Rectangle(0, 0, PicArea_B.Width, PicArea_B.Height);
            if (m_G_PicArea_B.bg != null)
            {
                try
                {
                    m_G_PicArea_B.gb.DrawImage(m_G_PicArea_B.bg, _Rect_Kd);// 先绘制背景层
                }
#pragma warning disable CS0168 // 声明了变量“de”，但从未使用过
                catch (Exception de)
#pragma warning restore CS0168 // 声明了变量“de”，但从未使用过
                { }
            }
            m_G_PicArea_B.gb.DrawImage(m_G_PicArea_B.image, _Rect_Kd); // 再绘制绘画层
            System.Windows.Forms.PictureBox.CheckForIllegalCrossThreadCalls = false;
            try
            {
                PicArea_B.BackgroundImage = (Bitmap)m_G_PicArea_B.canvas.Clone(); // 设置为背景层，就是在内存中建立了一张对应的画布，重新refresh就是再次的加载这个画布
                PicArea_B.Refresh();
            }
#pragma warning disable CS0168 // 声明了变量“e2”，但从未使用过
            catch (Exception e2)
#pragma warning restore CS0168 // 声明了变量“e2”，但从未使用过
            { }
            #endregion 3
            #endregion 刻度尺

            Application.DoEvents();
            blPlantRuler = 1;
        }
        public void Plant_Ruler_Z()
        {
            if (PicArea_C == null) return;
#pragma warning disable CS0219 // 变量“i_X”已被赋值，但从未使用过它的值
            int i_X = 0, i_Y = 0;
#pragma warning restore CS0219 // 变量“i_X”已被赋值，但从未使用过它的值
#pragma warning disable CS0219 // 变量“strT”已被赋值，但从未使用过它的值
            string strT = "";
#pragma warning restore CS0219 // 变量“strT”已被赋值，但从未使用过它的值
            int iLine_X = (int)(Chart_Ruler_X_Start + Scree_iAllCols * Scree_iDotWith_X);
            blPlantRuler = 0;
            Chart_Clear(m_G_PicArea_C);
            System.Windows.Forms.PictureBox.CheckForIllegalCrossThreadCalls = false;
            SolidBrush _drawBrush_One = new SolidBrush(Color.White);
            Pen _Ruler_p_One = new Pen(Brushes.White);

            #region 画刻度尺
            #region 0 画布准备:初始化、右边出图、满屏时
            // 初始化画板，在内存中建立一块虚拟画布
            m_G_PicArea_C.image = new Bitmap(PicArea_C.ClientSize.Width, PicArea_C.ClientSize.Height);
            // 获取背景层
            m_G_PicArea_C.bg = (Bitmap)PicArea_C.BackgroundImage;
            // 初始化整个画布
            m_G_PicArea_C.canvas = new Bitmap(PicArea_C.ClientSize.Width, PicArea_C.ClientSize.Height);
            // 初始化图形面板，获取这块内存画布的Graphics的引用
            m_G_PicArea_C.g = Graphics.FromImage(m_G_PicArea_C.image);
            m_G_PicArea_C.gb = Graphics.FromImage(m_G_PicArea_C.canvas);
            m_G_PicArea_C.g.Clear(Color.FromArgb(255, 30, 30, 30));// Color.Gray); ;
            m_G_PicArea_C.Buff = new PointF[0];
            float iLeft = Ruler_p_One.Width;
            int iTop = PicArea_C.Top;

            int i_S_H = 0, i_S_L = 0;
            #endregion 0 画布准备

            #region 1.5 画纵坐标
            float _i_X = 5;

            float flD = 0.0f, fl_D2 = 0.0f;
            string strD = "";
            float flJg_Fz_X = PicArea_C.Height / 10;
            Pen p_xy = new Pen(Brushes.CadetBlue);
            p_xy.Width = 1;
            PointF drawPoint_W = new PointF(10, 10);
            Font drawFont_TiTl_XY = new Font("Arial", 10);
            int _Chart_Ruler_X_Start = Chart_Ruler_X_Start - 3;
            i_S_H = _Chart_Ruler_X_Start - 6; i_S_L = _Chart_Ruler_X_Start - 3;
            SolidBrush drawBrush_TiTl_XY = new SolidBrush(Color.White);
            StringFormat StrF = new StringFormat();
            StrF.FormatFlags = StringFormatFlags.DirectionVertical; // 竖排

            for (int i = 1; i < 10; i++)
            {
                i_Y = (int)(i * flJg_Fz_X);//画刻度线Chart_Ruler_Y_Start 

                flD = (i * _i_X);
                fl_D2 = (int)flD;
                strD = flD.ToString(flD == fl_D2 ? "f0" : "f1") + (i == 9 ? " mm" : "");

                m_G_PicArea_C.g.DrawLine(_Ruler_p_One, new Point(i_S_H, i_Y), new Point(_Chart_Ruler_X_Start, i_Y));
                m_G_PicArea_C.g.DrawString(strD, drawFont, _drawBrush_One, -3, i_Y - (i == 1 ? 10 : 18), StrF);
            }
            m_G_PicArea_C.g.DrawLine(_Ruler_p_One, new Point(_Chart_Ruler_X_Start, 0), new Point(_Chart_Ruler_X_Start, PicArea_C.ClientSize.Height));//Chart_Ruler_Y_Start

            #endregion 1.5
            #region 3 刷新
            Rectangle _Rect_Kd = new Rectangle(0, 0, PicArea_C.Width, PicArea_C.Height);
            if (m_G_PicArea_C.bg != null)
            {
                try
                {
                    m_G_PicArea_C.gb.DrawImage(m_G_PicArea_C.bg, _Rect_Kd);// 先绘制背景层
                }
#pragma warning disable CS0168 // 声明了变量“de”，但从未使用过
                catch (Exception de)
#pragma warning restore CS0168 // 声明了变量“de”，但从未使用过
                { }
            }
            m_G_PicArea_C.gb.DrawImage(m_G_PicArea_C.image, _Rect_Kd); // 再绘制绘画层
            System.Windows.Forms.PictureBox.CheckForIllegalCrossThreadCalls = false;
            try
            {
                PicArea_C.BackgroundImage = (Bitmap)m_G_PicArea_C.canvas.Clone(); // 设置为背景层，就是在内存中建立了一张对应的画布，重新refresh就是再次的加载这个画布
                PicArea_C.Refresh();
            }
#pragma warning disable CS0168 // 声明了变量“e2”，但从未使用过
            catch (Exception e2)
#pragma warning restore CS0168 // 声明了变量“e2”，但从未使用过
            { }
            #endregion 3
            #endregion 刻度尺

            Application.DoEvents();
            blPlantRuler = 1;
        }
        /// <summary>
        /// 绘制当前距离数据
        /// </summary>
        /// <param name="iKey_Dist">1-N：0.1mm-N/10mm</param>
        /// <param name="x_Data">报文数据</param>
        public void Plant_CurrDist(int iKey_Dist, Class_X_Data x_Data)
        {
            try
            {
                //1 判断当前点是否在图形刻度尺范围
                float _flKey_Dist = iKey_Dist / 10f;//转换成实际距离

                int _i_BeginPlan = -1;//准备画初始图  0:左图 1：右图
                int i_X = 0, i_Y = 0;
                float flJg_H = 0;
                int iDat = 0;//测量高度数据
                int _fl_One_H = 1;

                if (x_Data.m_iprofileCnt > 0)
                {
                    flJg_H = (PicArea_C.Height) / ((float)x_Data.m_iprofileCnt);// - (Chart_Ruler_Y_Start + Scree_iDotHeight_B_Y)
                                                                                //      _fl_One_H = int.Parse(flJg_H.ToString());
                }

                if (_flKey_Dist > Chart_Run_flEnd_Distance_B_One) _i_BeginPlan = 1;
                if (_flKey_Dist < Chart_Run_flStart_Distance_B_One) _i_BeginPlan = 0;
                #region  2 不在就重新绘制刻度尺
                if (_i_BeginPlan == 1)
                {
                    m_blRepPlant = true;
                    Chart_Run_flStart_Distance_B_One = Chart_Run_flEnd_Distance_B_One + Scree_iDotWithmm_X;
                    Plant_Ruler_H();
                    Plant_Ruler_Z();
                }
                if (_i_BeginPlan == 0)
                {
                    m_blRepPlant = true;
                    int _iNo = Get_No(_flKey_Dist);
                    Chart_Run_flStart_Distance_B_One = m_flArr_Rul_S[_iNo];
                    Plant_Ruler_H();
                    Plant_Ruler_Z();
                }
                m_blRepPlant = false;
                #endregion  2

                #region  3 绘制当前点对于灰度图
                i_X = GetPointByDistance(_flKey_Dist);
                i_Y = (int)(Chart_Ruler_Y_Start + Scree_iDotHeight_B_Y);
                #region 3.1 显示 如果有缺陷，就画报警
                if (i_X <= PicArea_B.Width && i_Y <= PicArea_B.Height)
                {
                    InitColor(m_G_PicArea_B.g, i_X, i_Y, Scree_iDotWith_X, Scree_iDotWith_X + 3,
                        x_Data.iDownNum > 0 ? Color.FromArgb(255, 255, 0, 0) : Color.FromArgb(255, 30, 30, 30));

                    Rectangle _Rect = new Rectangle(0, 0, PicArea_B.Width, PicArea_B.Height);
                    try
                    {
                        if (m_G_PicArea_B.bg != null)
                            try
                            {
                                m_G_PicArea_B.gb.DrawImage(m_G_PicArea_B.bg, _Rect);// 先绘制背景层
                            }
#pragma warning disable CS0168 // 声明了变量“ebg”，但从未使用过
                            catch (Exception ebg)//GDI+ 中发生一般性错误。
#pragma warning restore CS0168 // 声明了变量“ebg”，但从未使用过
                            {
                            }
                        m_G_PicArea_B.gb.DrawImage(m_G_PicArea_B.image, _Rect); // 再绘制绘画层
                        PicArea_B.BackgroundImage = (Bitmap)m_G_PicArea_B.canvas.Clone();
                    }
#pragma warning disable CS0168 // 声明了变量“e”，但从未使用过
                    catch (Exception e)//GDI+ 中发生一般性错误。
#pragma warning restore CS0168 // 声明了变量“e”，但从未使用过
                    {
                    }
                }
                #endregion 3.1

                #region 3.2
                if (x_Data.m_iprofileCnt > 0)
                {
                    //int _iStart = x_Data.m_i_W_End - 100;
                    //_iStart = _iStart < 0 ? 0 : _iStart;
                    //int _iEnd = x_Data.m_i_W_End + 170;
                    //_iEnd = _iEnd > x_Data.m_iprofileCnt ? x_Data.m_iprofileCnt: _iEnd;
                    int[] _Arr = new int[500];
                    for (int iNo = 0; iNo < x_Data.m_iprofileCnt; iNo++)
                    {
                        iDat = int.Parse(((x_Data.m_Arr_profileZ[iNo] - x_Data.m_dbL_Min_H) * 10).ToString("f0"));
                        iDat += 100;

                        if (iDat > 255)
                            iDat = 255;
                        _Arr[iNo] = iDat;
                        if (iDat > 0)
                        {
                            i_Y = (int)(iNo * flJg_H);//iStartChart_Ruler_Y_Start
                            InitColor(m_G_PicArea_C.g, i_X, i_Y, Scree_iDotWith_X, _fl_One_H, System.Drawing.Color.FromArgb((byte)iDat, (byte)iDat, (byte)iDat));
                        }
                    }

                    Rectangle _Rect = new Rectangle(0, 0, PicArea_C.Width, PicArea_C.Height);

                    if (m_G_PicArea_C.bg != null)
                        m_G_PicArea_C.gb.DrawImage(m_G_PicArea_C.bg, _Rect);// 先绘制背景层

                    m_G_PicArea_C.gb.DrawImage(m_G_PicArea_C.image, _Rect); // 再绘制绘画层
                    PicArea_C.BackgroundImage = (Bitmap)m_G_PicArea_C.canvas.Clone();
                }
                #endregion 3.2 一帧数据的灰度图
                #endregion  3
            }
#pragma warning disable CS0168 // 声明了变量“e”，但从未使用过
            catch (Exception e)
#pragma warning restore CS0168 // 声明了变量“e”，但从未使用过
            {

            }
        }
        /// <summary>
        /// 画色块
        /// </summary>
        /// <param name="G">画布</param>
        /// <param name="intStar_X">起点X</param>
        /// <param name="intStar_Y">起点Y</param>
        /// <param name="iWith">宽度</param>
        /// <param name="iHeight">高度</param>
        /// <param name="color">颜色</param>
        public void InitColor(Graphics G, int intStar_X, int intStar_Y, int iWith, int iHeight, Color color)
        {
            SolidBrush mybrush = new SolidBrush(color);
            try
            {
                G.FillRectangle(mybrush, intStar_X, intStar_Y, iWith, iHeight);
            }
#pragma warning disable CS0168 // 声明了变量“e”，但从未使用过
            catch (Exception e)
#pragma warning restore CS0168 // 声明了变量“e”，但从未使用过
            { }
        }
        public int GetPointByDistance(float _flDistance_X)//, ref bool blX)
        {
            //1 转换成整数
            int _i_X = 0;
            float _flDat = (_flDistance_X - Chart_Run_flStart_Distance_B_One) / Scree_iDotWithmm_X;//单位m
            _flDat = float.Parse(_flDat.ToString("f1"));
            _i_X = (int)_flDat;//由实际点计算屏幕开始序号
                               //  blX = _i_X == 0;
            if (_flDat - _i_X >= 0.5) _i_X++;
            //2 转换成屏幕列位置
            _i_X = (int)(Chart_Ruler_X_Start + _i_X * Scree_iDotWith_X);// + (blX ? 1 : 0);//由序号计算屏幕对应的像素位置

            return _i_X;
        }

        /// <summary>
        /// 图形清零
        /// </summary>
        public void Chart_Clear_C()
        {
            if (m_G_C.g != null)
            {
                m_G_C.g.Dispose(); m_G_C.g = null;
                m_G_C.image.Dispose();
                m_G_C.gb.Dispose();
                m_G_C.canvas.Dispose();
                Application.DoEvents();
            }
        }
        public void Chart_Clear_C_TowChannel()
        {
            if (m_G_C_TowChannel.g != null)
            {
                m_G_C_TowChannel.g.Dispose(); m_G_C_TowChannel.g = null;
                m_G_C_TowChannel.image.Dispose();
                m_G_C_TowChannel.gb.Dispose();
                m_G_C_TowChannel.canvas.Dispose();
                Application.DoEvents();
            }
        }
        public void Chart_Clear_B_One()
        {
            if (m_G_B_One.g != null)
            {
                m_G_B_One.g.Dispose(); m_G_B_One.g = null;
                m_G_B_One.image.Dispose();
                m_G_B_One.gb.Dispose();
                m_G_B_One.canvas.Dispose();
                Application.DoEvents();
            }
        }
        public void Chart_Clear(Struct_G mg)
        {
            if (mg.g != null)
            {
                mg.g.Dispose(); mg.g = null;
                mg.image.Dispose();
                mg.gb.Dispose();
                mg.canvas.Dispose();
                Application.DoEvents();
            }
        }

        /// <summary>
        /// 清理图缓存
        /// </summary>
        /// <param name="G"></param>
        public void Chart_Clear(ref Struct_G G)
        {
            if (G.g != null)
            {
                G.g.Dispose(); G.g = null;
                G.image.Dispose();
                G.gb.Dispose();
                G.canvas.Dispose();
                Application.DoEvents();
            }
        }

    }

    #endregion

    #region 注册
    /// <summary>
    /// 注册类
    /// </summary>
    public class ClassRegister
    {
        /// <summary>
        /// 注册变量
        /// </summary>
        public RegistryKey m_Rsg = null;

        /// <summary>
        /// 使用截止日期
        /// </summary>
        public string m_Key_LastDat = "TO_LastDate_Key";
        /// <summary>
        /// 主键名称
        /// </summary>
        public string m_Key_Name = "TO_DLSYS_Key";
        /// <summary>
        /// 子主键
        /// </summary>
        public string m_Son_Key = "TO_DLSYS";

        /// <summary>
        /// 公司主键
        /// </summary>
        public string m_Parent_Key = "SzeDLongKun";
        /// <summary>
        /// 注册码
        /// </summary>
        private SoftReg m_softReg = new SoftReg();
        /// <summary>
        /// 当前电脑IP物理地址
        /// </summary>
        public string IP = "";
        /// <summary>
        /// 口令长度
        /// </summary>
        public int m_iPass_Num = 6;
        #region 注册基本函数
        /// <summary>
        /// 创建注册
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public bool Create()
        {
            bool _blRet = false;

            try
            {
                m_Rsg = Registry.CurrentUser.OpenSubKey("Software", true).CreateSubKey(m_Parent_Key).CreateSubKey(m_Son_Key);
                _blRet = true;
            }
            catch { _blRet = false; }
            return _blRet;
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="strSetVal">设定值</param>
        /// <returns>是否成功</returns>
        public bool Set_Key(string strSetVal)
        {
            bool _blRet = false;
            if (m_Rsg != null)
            {
                if (m_Key_Name != "" && strSetVal != null)
                {
                    m_Rsg.SetValue(m_Key_Name, strSetVal);
                    System.Threading.Thread.Sleep(10);
                    m_Rsg.SetValue(m_Key_Name, strSetVal);
                    m_Rsg.Close();
                    _blRet = true;
                }
            }
            return _blRet;
        }
        /// <summary>
        /// 判断机器是否注册
        /// </summary>
        /// <returns></returns>
        public bool Jg_Key()
        {
            bool _blRet = false;
            string strKey = "";
            if (m_Rsg == null)
                Create();

            if (m_softReg != null)
                if (Read(ref strKey))// foreach (string strKey in m_Rsg.GetSubKeyNames())
                {// VX2000PR048\u0017HT2R202RDD681455D
                    m_softReg.IPMac = IP;
                    if (strKey == m_softReg.GetRNum())// Njtjtjf\u0017Lpbvh\u0017Hjvn\u0017ZVD\u0017Hjrjfj681455D   DLJDLDLL000603F66J8H5FDJ   VX2000HR00622VD20F\u0017BXB\u0017H
                        _blRet = true;  //此软件已注册
                }

            return _blRet;
        }
        /// <summary>
        /// 获得注册码
        /// </summary>
        /// <returns></returns>
        public string Get_Key()
        {
            return m_softReg.GetRNum();
        }
        /// <summary>
        /// 读主键值
        /// </summary>
        /// <param name="strReadVal">读到值</param>
        /// <returns>是否成功</returns>

        public bool Read(ref string strReadVal)
        {
            bool _blRet = false;
            strReadVal = "";
            if (m_Rsg != null)
            {
                if (m_Key_Name != "")
                {
                    object _objVal;
                    for (int i = 0; i < 3; i++)
                    {
                        _objVal = m_Rsg.GetValue(m_Key_Name);
                        if (_objVal != null)
                        {
                            strReadVal = _objVal.ToString();
                            _blRet = true;
                            break;
                        }
                    }
                }
                // m_Rsg.Close();
            }
            return _blRet;
        }
        #endregion 注册基本函数

        #region 口令类函数
        /// <summary>
        /// 获取指定位数的随机数
        /// </summary>
        /// <returns></returns>
        public string Get_Pass()
        {
            m_iPass_Num = (m_iPass_Num < 1 || m_iPass_Num > 10) ? 4 : m_iPass_Num;
            string _strRet = "";
            string strT = "";
            //1 取0.1-1之间随机数
            //2 转变成01-10数值
            //3 取个位数 
            Random _Rd = new Random();
            for (int i = 0; i < m_iPass_Num; i++)
            {
                strT = ((int)(_Rd.NextDouble() * 10)).ToString().PadLeft(2, '0');
                _strRet += strT.Substring(1, 1);
            }

            return _strRet;
        }
        /// <summary>
        /// 计算口令
        /// </summary>
        /// <param name="strPass"></param>
        /// <returns></returns>
        public string Calcu_Pass(string strPass)
        {
            m_iPass_Num = (m_iPass_Num < 4 || m_iPass_Num > 10) ? 4 : m_iPass_Num;
            string _strRet = "";
            string[] strArrDate = DateTime.Now.ToString("yyyy-MM-dd").Split('-');
            int iDat = 0;

            int i_1 = (int.Parse(strArrDate[0]) - 19 + int.Parse(strArrDate[1]) + 12 + int.Parse(strArrDate[2]) + 1040); //(int.Parse(strArrDate[0]) - 89 + int.Parse(strArrDate[1]) + 2 + int.Parse(strArrDate[2]) + 1000);
            int i_2 = int.Parse(strPass.Substring(3)) + int.Parse(strPass.Substring(0, 3));
            iDat = i_1 + i_2;
            _strRet = iDat.ToString().PadLeft(m_iPass_Num, '0');
            _strRet = _strRet.Substring(0, m_iPass_Num);

            return _strRet;
        }
        #endregion 口令类函数
    }
    /// <summary>
    /// TOFD历史数据显示位置
    /// </summary>
    public class Cls_BrushPoint
    {
        /// <summary>
        /// 显示的位置序号
        /// </summary>
        public int iCurrPoint = 0;
        /// <summary>
        /// 显示的距离位置
        /// </summary>
        public float flCurrPoint = 0;
    }
    /// <summary>
    /// 以机器的硬件为注册码
    /// </summary>
    public class SoftReg
    {
        /// <summary>
        /// 网卡物理地址
        /// </summary>
        public string IPMac = "";
        ///<summary>
        /// 获取硬盘卷标号
        ///</summary>
        ///<returns></returns>
        public string GetDiskVolumeSerialNumber()
        {
            ManagementClass mc = new ManagementClass("win32_NetworkAdapterConfiguration");
            ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
            disk.Get();
            return disk.GetPropertyValue("VolumeSerialNumber").ToString();
        }

        ///<summary>
        /// 获取CPU序列号
        ///</summary>
        ///<returns></returns>
        public string GetCpu()
        {
            string strCpu = null;
            ManagementClass myCpu = new ManagementClass("win32_Processor");
            ManagementObjectCollection myCpuCollection = myCpu.GetInstances();
            foreach (ManagementObject myObject in myCpuCollection)
            {
                strCpu = myObject.Properties["Processorid"].Value.ToString();
            }
            return strCpu;
        }
        /// <summary>  
        /// 获取硬盘序号  
        /// </summary>  
        /// <returns>硬盘序号</returns>  
        public static string GetDiskID()
        {
            try
            {
                string strDiskID = string.Empty;
                ManagementClass mc = new ManagementClass("Win32_DiskDrive");
                ManagementObjectCollection moc = mc.GetInstances();
                string _strT = "";
                foreach (ManagementObject mo in moc)
                {
                    _strT = mo.Properties["Model"].Value.ToString();//ST1000LM024 HN-M101MBB
                    if (_strT != "")
                    {
                        if (_strT.ToUpper().IndexOf("USB") < 0)
                            strDiskID = _strT;
                    }
                }
                moc = null;
                mc = null;
                return strDiskID;
            }
            catch
            {
                return "unknown";
            }
        }
        public int[] intCode = new int[127];    //存储密钥
        public char[] charCode = new char[25];  //存储ASCII码
        public int[] intNumber = new int[25];   //存储ASCII码值
        public int m_iLen = 0;//生成机器码的长度
        ///<summary>
        /// 生成机器码
        ///</summary>
        ///<returns></returns>
        public string GetMNum()
        {
            string strNum = GetDiskID() + IPMac + GetDiskVolumeSerialNumber(); //GetIPAddress  GetCpu() GetDiskID  ST1000DM003-1SB10C ATA Device220C0803
                                                                               //  strNum = GetDiskID() + GetMacAddress() + GetDiskVolumeSerialNumber();
            m_iLen = strNum.Length;
            string strMNum = strNum.Substring(0, m_iLen);    //Generic Flash Disk USB Device3E4D7CBE 截取前24位作为机器码  ST1000DM003-1SB10C ATA Device3E4D7CBE
            return strMNum;
        }

        ///
        /// 获取网卡硬件地址
        ///
        ///
        public string GetMacAddress()
        {
            string mac = "";
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                {
                    mac = mo["MacAddress"].ToString();
                    break;
                }
            }





            return mac;
        }


        ///
        /// 获取IP地址
        ///
        ///
        public string GetIPAddress()
        {
            string st = "";
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                {
                    //st=mo["IpAddress"].ToString();
                    System.Array ar;
                    ar = (System.Array)(mo.Properties["IpAddress"].Value);
                    if (ar.GetValue(0).ToString() == "192.168.1.10")
                    {
                        st = ar.GetValue(1).ToString();
                        break;
                    }
                }
            }
            //  st = st.Replace('.', '');
            return st;
        }
        //初始化密钥
        public void SetIntCode()
        {
            intCode = new int[127];
            for (int i = 1; i < intCode.Length; i++)
            {
                intCode[i] = i % 8;
            }
        }
        ///<summary>
        /// 生成注册码
        ///</summary>
        ///<returns></returns>
        public string GetRNum(string strMNum)
        {
            SetIntCode();
            charCode = new char[m_iLen];
            intNumber = new int[m_iLen];

            for (int i = 1; i < charCode.Length; i++)   //存储机器码
                charCode[i] = Convert.ToChar(strMNum.Substring(i - 1, 1));
            for (int j = 1; j < intNumber.Length; j++)  //改变ASCII码值
                intNumber[j] = Convert.ToInt32(charCode[j]) + intCode[Convert.ToInt32(charCode[j])];
            string strAsciiName = "";   //注册码
            for (int k = 1; k < intNumber.Length; k++)  //生成注册码
            {
                if ((intNumber[k] >= 48 && intNumber[k] <= 57) || (intNumber[k] >= 65 && intNumber[k]
                    <= 90) || (intNumber[k] >= 97 && intNumber[k] <= 122))  //判断如果在0-9、A-Z、a-z之间
                {
                    strAsciiName += Convert.ToChar(intNumber[k]).ToString();
                }
                else if (intNumber[k] > 122)  //判断如果大于z
                    strAsciiName += Convert.ToChar(intNumber[k] - 10).ToString();
                else
                    strAsciiName += Convert.ToChar(intNumber[k] - 9).ToString();
            }
            return strAsciiName;
        }
        public string GetRNum()
        {
            string strMNum = GetMNum();//Generic Flash Disk USB Device3E4D7CBE

            SetIntCode();
            charCode = new char[m_iLen];
            intNumber = new int[m_iLen];
            for (int i = 1; i < charCode.Length; i++)   //存储机器码
                charCode[i] = Convert.ToChar(strMNum.Substring(i - 1, 1));
            for (int j = 1; j < intNumber.Length; j++)  //改变ASCII码值
                intNumber[j] = Convert.ToInt32(charCode[j]) + intCode[Convert.ToInt32(charCode[j])] + 22;
            string strAsciiName = "";   //注册码
            for (int k = 1; k < intNumber.Length; k++)  //生成注册码
            {
                if ((intNumber[k] >= 48 && intNumber[k] <= 57) || (intNumber[k] >= 65 && intNumber[k]
                    <= 90) || (intNumber[k] >= 97 && intNumber[k] <= 122))  //判断如果在0-9、A-Z、a-z之间
                    strAsciiName += Convert.ToChar(intNumber[k]).ToString();
                else if (intNumber[k] > 122)  //判断如果大于z
                    strAsciiName += Convert.ToChar(intNumber[k] - 10).ToString();
                else
                    strAsciiName += Convert.ToChar(intNumber[k] - 9).ToString();
            }
            return strAsciiName;//Njtjtjf\u0017Lpbvh\u0017Hjvn\u0017ZVD\u0017Hjrjfj6J8H5FD      VX2000HR00622VD20F\u0017BXB\u0017H
        }
    }
    #endregion 注册
}