using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
/**创建人: 苏州博智慧达
**时间: 20190731
**------------修订历史记录----------------------------------------------------------------------------
** 修改人: 苏州博智慧达
** 版  本: 1.0.0
** 日　期: 20190131
** 描　述: 版本建立
** 版  本: 1.0.1
** 日　期: 20190508
** 描　述: 版本更新
** 版  本: 1.0.2
** 日　期: 20190701
** 描　述: 版本更新
** 版  本: 1.0.3
** 日　期: 20190716
** 描　述: 版本更新：增加配置文件绝对路径输入,增加平均高度计算
** 版  本: 1.0.4
** 日　期: 20190821
** 描　述: 版本更新：无效点处理优化
** 版  本: 1.0.5
** 日　期: 20190826
** 描　述: 版本更新：增加原图数据转轮廓数据功能,设置阈值功能
** 版  本: 1.0.6
** 日　期: 20190926
** 描　述: 版本更新：增加0100W传感器适配，增加传感器温度监视功能
** 版  本: 1.0.7
** 日　期: 20191029
** 描　述: 版本更新：增加像素坐标转物理坐标功能
** 版  本: 1.0.8
** 日　期: 20191121
** 描　述: 版本更新：函数库增加了命名空间“LshdProfile”,最大连接数量6提高到8
 ** 版  本: 1.0.9
** 日　期: 20200316
** 描　述: 版本更新：增加100DB传感器适配,优化内部算法
**------------------------------------------------------------------------------------------------------
/********************************************************************************************************/
namespace LSHDPROFILE
{
      class LS_HD6
    {
        /***轮廓仪采集到单个轮廓的回调函数（低速，每帧一次回调）
        @param profileX   单个轮廓的X轴数据指针
        @param profileZ   单个轮廓的Z轴数据指针
        @param profileZ   单个轮廓的点数，一般为750~1000
        @param pImage   原始图像数据指针
        @param imgWidth   原始图像宽度（单位：像素）
        @param imgHeight   原始图像高度（单位：像素）
        注意：一般情况下用户使用回调参数为前三项以获取轮廓物理坐标。
        高级开发者如需要获取原始图像数据需要在LSHD6_SetSingleCallBackMode函数中进行设置以获取原始图像数据信息
        ***/
        [System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public  unsafe delegate  void pCallbackSingleProfile(double* bufferX, double* bufferZ, uint profileCnt, byte* pImageData, UInt32 imageWidth, UInt32 imageHeight);             
        /***轮廓仪采集到批量轮廓的回调函数（高速，全部帧结束后回调）
        @param profileZ   批量轮廓的Z轴数据指针
        @param profileX   批量轮廓的X轴数据指针(每个单个轮廓都是一样的）
        @param xcount	  批量轮廓中单个轮廓的数据点数，一般为750~1000(每个单个轮廓都是一样的）
        @param ycount	  批量轮廓的总数量，一般为20000以下
        ***/
        [System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public unsafe delegate void pCallbackBatchProfile(double** profileZ, double* profileX, int xcount, int ycount);        
        /*************************************************************
        说明：获取开发库版本号
        输入：无
        输出：无
        返回值：开发库版本号
        *************************************************************/
        [DllImport("LS_HD6.dll")]
        public static extern double LSHD6_GetVersion();
         /*************************************************************
        说明：获取可用轮廓仪数量
        输入：无
        输出：无
        返回值：可用轮廓仪数量
        *************************************************************/
        [DllImport("LS_HD6.dll")]
        public static extern UInt32 LSHD6_GetEnableCamereNumber();
        /*************************************************************
        说明：加载轮廓仪配置文件
        输入：
        @param cameraIndex 轮廓仪序列，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推,最大值6
        @param cameraDataFileName 轮廓仪配置文件名，如 “SN6-0020-1143989”，配置文件需放入工程目录或可执行程序同级目录下
        输出：无
        返回值：载入成功返回true，载入失败返回false
        *************************************************************/
        [DllImport("LS_HD6.dll")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool LSHD6_SetCameraParameter(UInt32 cameraIndex, string cameraDataFileName);
        /*************************************************************
        说明：加载轮廓仪配置文件（绝对路径）
        输入：
        @param cameraIndex 轮廓仪序列，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推,最大值6
        @param cameraDataFileName 轮廓仪配置文件名加绝对路径，如 “C:\\Laser\\SN6-0020-1143989”，配置文件可放在指定路径的文件夹下
        输出：无
        返回值：载入成功返回true，载入失败返回false
        *************************************************************/
        [DllImport("LS_HD6.dll")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool LSHD6_SetCameraParameterAbsolutePath(UInt32 cameraIndex, string cameraDataFileName);
        /*************************************************************
        说明：设置轮廓仪动态库及配置文件的绝对路径（如果必须为绝对路径时在加载参数前调用）
        输入：
        @param filePath 文件存放的路径，格式为“D:\\...\\...\\...”例如“D:\\HDLaser\\filePath”,不能含中文字符
        输出：无
        返回值：无
        *************************************************************/
        [DllImport("LS_HD6.dll")]
        public static extern void LSHD6_SetSysFilePath(string filePath);        
        /*************************************************************
        说明：初始化轮廓仪不带轮廓显示功能
        输入：
        @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
        @param cameraSerialNumber 轮廓仪序列号后7位数字前加“S”,例如序列号为SN6-0020-1134543的轮廓仪设置“S1134543”
        @param ExposureTime:曝光时间（单位：微秒，一般使用100~3000)
        @param AcquisitionFrameRate:帧速（单位：帧/秒，一般使用1~400,不同型号轮廓仪可设置的最大值不同，300mm量程以下一般全幅面200帧/秒，300mm量程以上可全幅面400帧/秒)
        @param SpeedOrPrecise:增益（高速模式或者是高精度模式，如果100帧/秒以下高精度，可设置false，如需要高速扫描则设置true）
        @param TriggerMode：0软件触发采集轮廓，1外部IO低频率触发（100帧/秒以下），2编码器高速触发
        返回值：初始化成功返回0，其他参看错误码
        *************************************************************/
        [DllImport("LS_HD6.dll")]
        public static extern UInt32 LSHD6_InitialCameraWithoutUI(UInt32 cameraIndex, string cameraSerialNumber,
        double ExposureTime, double AcquisitionFrameRate, bool SpeedOrPrecise, UInt32 TriggerMode);
        /*************************************************************
        说明：销毁轮廓仪，程序退出前需执行，避免内存泄漏
        输入：
        @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
        输出：无
        返回值：停止成功返回true，失败返回false
        *************************************************************/
        [DllImport("LS_HD6.dll")]
        public static extern void LSHD6_DestroyCamera(UInt32 cameraIndex);

        /*************************************************************
        说明：设置轮廓仪Bining模式（合并相邻的两个像素，可以降低精度提高速度），如需此功能，必须在轮廓仪加载配置文件之前设置，使用过程中不可修改
        输入：
        @param cameraIndex 轮廓仪序列，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推,最大值6
        @param HBiningOpen 水平方向binning，true则开启，false关闭，默认关闭状态
        @param VBiningOpen 垂直方向binning，true则开启，false关闭，默认关闭状态
        输出：无
        返回值：成功返回true，失败返回false
        *************************************************************/
        [DllImport("LS_HD6.dll")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool LSHD6_SetCameraBining(UInt32 cameraIndex, bool HBiningOpen, bool VBiningOpen);

        /*************************************************************
        说明：开启激光
        输入：
        @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
        输出：无
        返回值：无
        *************************************************************/
         [DllImport("LS_HD6.dll")]
         [return: MarshalAs(UnmanagedType.I1)]
         public static extern void  LSHD6_SetLaserOn(UInt32 cameraIndex);
         /*************************************************************
        说明：关闭激光
        输入：
        @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
        输出：无
        返回值：无
        *************************************************************/
         [DllImport("LS_HD6.dll")]
         [return: MarshalAs(UnmanagedType.I1)]
         public static extern void LSHD6_SetLaserOff(UInt32 cameraIndex);
       /*************************************************************
        说明：内部软件触发方式进行连续取像，一般用来预览和调试，固定帧速20pps
        输入：
        @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
        @param frequency 轮廓仪低速帧速（1~100帧/秒）
        输出：无
        返回值：成功返回true，失败返回false
        *************************************************************/
        [DllImport("LS_HD6.dll")]
         [return: MarshalAs(UnmanagedType.I1)]
         public static extern bool LSHD6_LowSpeedGrab(UInt32 cameraIndex,double frequency);

    /*************************************************************
    说明：停止轮廓仪工作，进入待机状态
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    输出：无
    返回值：无
    *************************************************************/
     [DllImport("LS_HD6.dll")]
     [return: MarshalAs(UnmanagedType.I1)]
     public static extern bool LSHD6_Stop(UInt32 cameraIndex);

        /*************************************************************
    说明：设置单轮廓回调函数
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    @param func 用户回调函数
    输出：无
    返回值：无
    *************************************************************/
    [DllImport("LS_HD6.dll")]
     public  static extern void LSHD6_SetSingleCallBack(UInt32 cameraIndex, pCallbackSingleProfile func);
    /*************************************************************
    说明：设置批处理轮廓回调函数
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    @param func 用户回调函数
    输出：无
    返回值：无
    *************************************************************/
    [DllImport("LS_HD6.dll")]
    public  static extern void LSHD6_SetBatchCallBack(UInt32 cameraIndex, pCallbackBatchProfile func);
     /*************************************************************
    说明：控制是否显示轮廓曲线或者原始图像，一般用来预览和调试时设置显示，运行时设置为不显示
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    @param showUI 是否显示轮廓曲线或者原始图像，true为显示，false为不显示（C#动态库该参数未使用）
    @param showRawImageOrProfileCurve 显示轮廓曲线或者原始灰度图像，true为显示原始图像，false为显示轮廓曲线
    输出：无
    返回值：无
    *************************************************************/
     [DllImport("LS_HD6.dll")]
    public static extern void LSHD6_SetshowUI(UInt32 cameraIndex, bool showUI, bool showRawImageOrProfileCurve);

     /*************************************************************
     说明：设置从原始图像提取轮廓像素坐标的亮度阈值
     输入：
     @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
     @param threshold 亮度阈值（10~250）
     输出：无
     返回值：无
     *************************************************************/
     [DllImport("LS_HD6.dll")]
     public static extern void LSHD6_SetRawImageThreshold(UInt32 cameraIndex, int threshold);
    /*************************************************************
    说明：获取轮廓尺寸信息
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    @param profileXMax 轮廓X方向最大值
    @param profileZMax 轮廓Z方向最大值
    输出：无
    返回值：无
    *************************************************************/
    [DllImport("LS_HD6.dll")]
    public static extern  void  LSHD6_GetProfileSize(UInt32 cameraIndex, ref double  profileXMax,ref double profileZMax);

    /*************************************************************
    说明：设置单轮廓回调模式
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    @param ifImageDataDirect 如需获取原始图像数据指针设置为true（C#只可设置为true，C++可设置为false)
    输出：无
    返回值：无
    *************************************************************/
    [DllImport("LS_HD6.dll")]
    public static extern void LSHD6_SetSingleCallBackMode(UInt32 cameraIndex, bool ifImageDataDirect);
    /*************************************************************
    说明：设置轮廓仪曝光
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    @param value 曝光时间（单位微秒，一般使用100~3000)
    输出：无
    返回值：成功返回true，失败返回false
    *************************************************************/
    [DllImport("LS_HD6.dll")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool LSHD6_SetExposureTime(UInt32 cameraIndex, double value);
    /*************************************************************
    说明：设置轮廓仪增益
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    @param speedOrPrecise 轮廓仪增益模式（高速模式或者是高精度模式，如果100帧/秒以下高精度，可设置false，如需要高速扫描则设置true）
    输出：无
    返回值：成功返回true，失败返回false
    *************************************************************/
    [DllImport("LS_HD6.dll")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool LSHD6_SetGain(UInt32 cameraIndex, bool speedOrPrecise);
    /*************************************************************
    说明：设置轮廓仪帧速
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    @param value 轮廓仪帧速（1~400帧/秒,不同型号轮廓仪可设置的最大值不同，300mm量程以下一般全幅面200帧/秒，300mm量程以上可全幅面400帧/秒)
    输出：无
    返回值：成功返回true，失败返回false
    *************************************************************/
    [DllImport("LS_HD6.dll")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool LSHD6_SetFrameRate(UInt32 cameraIndex, double value);
     /*************************************************************
    说明：获取轮廓仪帧速，必须在触发模式设置为软件触发时使用
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    @param value 轮廓仪实际帧速
    输出：无
    返回值：轮廓仪帧速，-99代表获取失败
    *************************************************************/
    [DllImport("LS_HD6.dll")]
    public static extern double LSHD6_GetFrameRate(UInt32 cameraIndex);
    /*************************************************************
    说明：设置轮廓仪触发模式，需在轮廓仪停止状态下
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    @param value：触发模式，0软件触发采集轮廓，1外部IO低频率触发（一般100帧/秒以下，每触发一次采集一帧，并执行一次回调），2编码器高速触发
    输出：无
    返回值：成功返回true，失败返回false
    *************************************************************/
    [DllImport("LS_HD6.dll")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool LSHD6_SetTriggerMode(UInt32 cameraIndex, int value);
    /*************************************************************
    说明：外部触发方式进行连续取像，外部每到来一个上升沿信号则触发取像一次
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    输出：无
    返回值：成功返回true，失败返回false
    *************************************************************/
    [DllImport("LS_HD6.dll")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool LSHD6_LowSpeedGrabOutter(UInt32 cameraIndex);
    /*************************************************************
    说明：无效点填补
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    @param type 补间类型，0不补间，1垂直补间，2直线补间
    @param compansationCnt ,补间距离阈值，连续无效点数量小于该值则转换为有效点
    输出：无
    返回值：无
    *************************************************************/
    [DllImport("LS_HD6.dll")]
    public static extern void LSHD6_SetCompansation(UInt32 cameraIndex,int type, int compansationCnt);
    /*************************************************************
    说明：轮廓滤波
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    @param type 滤波类型，0不滤波，1均值滤波，2中值滤波，3均值滤波+中值滤波
    @param SmoothNumber 均值滤波值 
    @param SmoothTimes 均值滤波次数
    @param MidNumber 中值滤波值
    输出：无
    返回值：无
    *************************************************************/
    [DllImport("LS_HD6.dll")]
    public static extern void LSHD6_SetFilter(UInt32 cameraIndex, int type, int SmoothNumber, int SmoothTimes, int MidNumber);


    /*************************************************************
    说明：内部软件触发方式进行固定帧数取像，每次执行前需要执行LSHD6_Stop
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    @param snapNumber 触发次数，最少为1
    输出：无
    返回值：成功返回true，失败返回false
    *************************************************************/
    [DllImport("LS_HD6.dll")]
    public static extern bool LSHD6_Snap(UInt32 cameraIndex, UInt32 snapNumber);


    /*************************************************************
    说明：申请批处理内存
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    @param ProfileCnt：轮廓个数，最大不超过20000
    输出：无
    返回值：成功返回true，失败返回false
    *************************************************************/
    [DllImport("LS_HD6.dll")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool LSHD6_SetBatchProfileBuffer(UInt32 cameraIndex, int ProfileCnt);
    /*************************************************************
    说明：相机高速扫描--固定帧数内部触发高速扫描
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    @param snapNumber：轮廓个数，最大不超过申请内存的轮廓个数
    输出：无
    返回值：成功返回true，失败返回false
    *************************************************************/
    [DllImport("LS_HD6.dll")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool LSHD6_InnerTriggerHighSpeedScan(UInt32 cameraIndex, int snapNumber);
    /*************************************************************
    说明：相机高速扫描--固定帧数编码器触发高速扫描
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    @param snapNumber：轮廓个数，最大不超过申请内存的轮廓个数
    输出：无
    返回值：成功返回true，失败返回false
        *************************************************************/
    [DllImport("LS_HD6.dll")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool LSHD6_EncoderTriggerHighSpeedScan(UInt32 cameraIndex, int snapNumber);
    /*************************************************************
    说明：相机高速扫描--内部触发高速循环扫描，不固定帧数，200帧以下可长时间工作，200帧以上内存循环存满后自动停止
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    输出：无
    返回值：成功返回true，失败返回false
    *************************************************************/
    [DllImport("LS_HD6.dll")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool LSHD6_InnerTriggerHighSpeedConstantScan(UInt32 cameraIndex);
    /*************************************************************
    说明：相机高速扫描--编码器触发高速循环扫描，不固定帧数，200帧以下可长时间工作，200帧以上内存循环存满后自动停止
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    输出：无
    返回值：成功返回true，失败返回false
    *************************************************************/
    [DllImport("LS_HD6.dll")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool LSHD6_EncoderTriggerHighSpeedConstantScan(UInt32 cameraIndex);

    /*************************************************************
    说明：保存当前轮廓
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    @param fileName为文件名，不需要加文件扩展名。系统会在当前目录下创建文件夹 MeasureData并保存txt文本文件
    输出：无
    返回值：成功返回true，失败返回false
    *************************************************************/
     [DllImport("LS_HD6.dll")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool LSHD6_SaveCurrentSingleProfile(UInt32 cameraIndex,string fileName);
    /*************************************************************
    说明：保存批处理轮廓
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    @param fileName为文件名，不需要加文件扩展名。系统会在当前目录下创建文件夹 MeasureData并保存csv文本文件
    输出：无
    返回值：成功返回true，失败返回false
    *************************************************************/
     [DllImport("LS_HD6.dll")]
     [return: MarshalAs(UnmanagedType.I1)]
     public static extern bool LSHD6_SaveBatchProfile(UInt32 cameraIndex, string fileName);
    /*************************************************************
    说明：保存高度图
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    @param fileName为文件名，不需要加文件扩展名。系统会在当前目录下创建文件夹 Picture并保存bmp8位灰度图
    @param heightUpper 高度上限，映射灰度255
    @param heightLower 高度下限，映射灰度0，中间高度等比例映射
    输出：无
    返回值：成功返回true，失败返回false
    *************************************************************/
     [DllImport("LS_HD6.dll")]
     [return: MarshalAs(UnmanagedType.I1)]
     public static extern bool LSHD6_SaveHeightPicture(UInt32 cameraIndex,string fileName, double heightUpper, double heightLower);
     /*************************************************************
     说明：创建UDP服务器，在连续高速扫描时不间断的通过本地端口向目标端口发送轮廓数据
     输入：
     @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
     输入：
     @param LocalIp0 本地ip第1段
     @param LocalIp1 本地ip第2段
     @param LocalIp2 本地ip第3段
     @param LocalIp3 本地ip第4段
     @param LocalPortNumer 本地端口号
     @param TargetIp0 目标ip第1段
     @param TargetIp1 目标ip第2段
     @param TargetIp2 目标ip第3段
     @param TargetIp3 目标ip第4段
     @param TargetPortNumber 目标端口号
     输出：无
     返回值：成功返回true，失败返回false
     *************************************************************/
     [DllImport("LS_HD6.dll")]
     [return: MarshalAs(UnmanagedType.I1)]
     public static extern bool LSHD6_CreateUDPserver(UInt32 cameraIndex, byte LocalIp0, byte LocalIp1, byte LocalIp2, byte LocalIp3, UInt32 LocalPortNumer,
    byte TargetIp0, byte TargetIp1, byte TargetIp2, byte TargetIp3, UInt32 TargetPortNumber);

     /*************************************************************
     说明：获取轮廓数据，此方法使用查询的方式，每次轮廓仪被触发5~25ms后可以读取到当前的轮廓，适合labview等不能使用回调机制的开发环境,不适合高速场合
     输入：
     @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
     输出：
     @param profileX 轮廓X数组
     @param profileZ 轮廓Z数组
     @param profileCnt 轮廓点数
     返回值：成功返回true，失败返回false
     *************************************************************/
    [DllImport("LS_HD6.dll")]
    public static extern void LSHD6_GetProfileData(UInt32 cameraIndex, double[] profileX, double[] profileZ, ref int profileCnt);

    /*************************************************************
    说明：是否启用UDP发送数据
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    @param udpsendOrNot 如需UDP发送数据则设置true，否则设置为false
    输出：无
    返回值：成功返回true，失败返回false
    *************************************************************/
     [DllImport("LS_HD6.dll")]
     [return: MarshalAs(UnmanagedType.I1)]
     public static extern bool LSHD6_UDPEnable(UInt32 cameraIndex, bool udpsendOrNot);
    /*************************************************************
    说明：销毁UDP服务器
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    输出：无
    返回值：成功返回true，失败返回false
    *************************************************************/
     [DllImport("LS_HD6.dll")]
     [return: MarshalAs(UnmanagedType.I1)]
     public static extern bool LSHD6_UDPDestroy(UInt32 cameraIndex);
    /*************************************************************
    说明：UDP发送数据
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    @param UdpMessage发送内容
    @param length发送字节数
    输出：无
    返回值：无
    *************************************************************/
    [DllImport("LS_HD6.dll")]
    public unsafe static extern void LSHD6_SendUDPbroadcast(UInt32 cameraIndex, Byte* UdpMessage, int length);
    /*************************************************************
    说明：设置轮廓仪取像区域及显示区域，轮廓仪需要在初始化完成后，且在停止状态
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    @param ROIleft 轮廓仪取像区域左上列坐标（0~1200，16的倍数）
    @param ROItop 轮廓仪取像区域左上行坐标（0~1024，1的倍数）
    @param ROIWidthSize 轮廓仪取像区域宽度（64~1200，16的倍数）
    @param ROIHeightSize 轮廓仪取像区域高度（1~1023，1的倍数）
    @param imgLeft：如需要显示原始图像或轮廓曲线，在此处传入显示控件区域左上角行坐标（单位pixel）C#不用设置该参数
    @param imgTop：如需要显示原始图像或轮廓曲线，在此处传入显示控件区域左上角行坐标（单位pixel）C#不用设置该参数
    @param imgWidth:如需要显示原始图像或轮廓曲线，在此处传入显示控件区域宽度（单位pixel）C#不用设置该参数
    @param imgHeight:如需要显示原始图像或轮廓曲线，在此处传入显示控件区域高度（单位pixel）C#不用设置该参数
    输出：无
    返回值：成功返回true，失败返回false
    *************************************************************/
    [DllImport("LS_HD6.dll")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool  LSHD6_SetCameraROI(UInt32 cameraIndex, UInt32 ROIleft, UInt32 ROItop, UInt32 ROIWidthSize, UInt32 ROIHeightSize,
    UInt32 imgLeft, UInt32 imgTop, UInt32 imgWidth, UInt32 imgHeight);
    /*************************************************************
    说明：获取轮廓仪取像区域
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    输出：
    @param ROIleft 轮廓仪取像区域左上列坐标（0~1200，16的倍数）
    @param ROItop 轮廓仪取像区域左上行坐标（0~1024，1的倍数）
    @param ROIWidthSize 轮廓仪取像区域宽度（64~1200，16的倍数）
    @param ROIHeightSize 轮廓仪取像区域高度（1~1023，1的倍数）
    返回值：成功返回true，失败返回false
    *************************************************************/
    [DllImport("LS_HD6.dll")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool LSHD6_GetCameraROI(UInt32 cameraIndex, ref UInt32 ROIleft, ref UInt32 ROItop, ref UInt32 ROIWidthSize, ref UInt32 ROIHeightSize);

    /*************************************************************
    说明：计算平均高度
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    @param LineStart 计算角度起始端
    @param LineStart 计算角度结束端
    输出：无
    返回值：区域平均高度
    *************************************************************/
    [DllImport("LS_HD6.dll")]
    public static extern  double  LSHD6_CalPlaneAverageHeight(UInt32 cameraIndex, double LineStart, double LineEnd);

    /*************************************************************
    说明：计算安装角度
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    @param LineStart 计算角度起始端
    @param LineStart 计算角度结束端
    输出：无
    返回值：根据标定物计算出的设备安装角度（单位：弧度），标定物要尽可能的水平以提高系统综合精度
    *************************************************************/
    [DllImport("LS_HD6.dll")]
    public static extern double LSHD6_CalDeviceAngle(UInt32 cameraIndex, double LineStart, double LineEnd);

    /*************************************************************
    说明：设备安装高度补偿，一般在多台并用时使用
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    @param heightOffset 高度补偿
    输出：无
    返回值：无
    *************************************************************/
    [DllImport("LS_HD6.dll")]
    public static extern void LSHD6_SetDeviceHeightOffset(UInt32 cameraIndex, double heightOffset);

    /*************************************************************
    说明：安装角度补偿
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    @param deviceAngle 安装角度
    输出：无
    返回值：无
    *************************************************************/
    [DllImport("LS_HD6.dll")]
    public static extern void LSHD6_SetDeviceAngle(UInt32 cameraIndex, double deviceAngle);

    /*************************************************************
    说明：原始图像转轮廓数据。适合特殊场合原始图像受到多重反射或镜面反射或其他干扰无法获取良好的轮廓，可以先获取
    原始图像数据，用户自行编写算法过滤干扰后再使用该函数获取轮廓数据，可以获取质量较好的轮廓
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    @param ImageData 图片数据指针
    输出：
    @param profileX 轮廓坐标X数组
    @param profileZ 轮廓坐标Z数组
    @param profileCnt 轮廓点数
    返回值：无
    *************************************************************/
    [DllImport("LS_HD6.dll")]
    public  unsafe static extern void LSHD6_GetProfileFromRawImage(UInt32 cameraIndex,  byte* ImageData, double[] profileX, double[] profileZ, ref int profileCnt);
    /*************************************************************
    说明：像素(数组）坐标转轮廓数据。适合特殊场合原始图像受到多重反射或镜面反射或其他干扰无法获取良好的轮廓，可以先获取
    原始图像数据，用户自行编写算法过滤干扰并提取像素坐标后再使用该函数获取轮廓数据，可以获取质量较好的轮廓
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    @param pixelX 像素坐标X数组
    @param pixelZ 像素坐标X数组
    @param pixelCnt 像素点数
    输出：
    @param profileX 轮廓坐标X数组
    @param profileZ 轮廓坐标Z数组
    @param profileCnt 轮廓点数
    返回值：无
    *************************************************************/
    [DllImport("LS_HD6.dll")]
    public  unsafe static extern void LSHD6_GetProfileFromPixelArray(UInt32 cameraIndex, double[] pixelX, double[] pixelZ, int pixelCnt, double[] profileX, double[] profileZ, ref int profileCnt);
    /*************************************************************
    说明：像素（单个）坐标转轮廓数据。适合特殊场合原始图像受到多重反射或镜面反射或其他干扰无法获取良好的轮廓，可以先获取
    原始图像数据，用户自行编写算法定位单点像素坐标后再使用该函数获取轮廓数据，可以获取稳定的特征点
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    @param pixelX 像素坐标X
    @param pixelZ 像素坐标Z
    输出：
    @param profileX 轮廓坐标X
    @param profileZ 轮廓坐标Z
    返回值：无
    *************************************************************/
    [DllImport("LS_HD6.dll")]
    public  unsafe static extern void LSHD6_GetProfileFromSinglePixel(UInt32 cameraIndex, double pixelX, double pixelZ,  ref double profileX, ref double profileZ);

	/*************************************************************
	说明：获取轮廓仪温度
	输入：
	@param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
	输出：无
	返回值：轮廓仪温度，-99为无效值
	*************************************************************/
    [DllImport("LS_HD6.dll")]
	public  static extern double LSHD6_GetTemperature(UInt32 cameraIndex);

    /*************************************************************
    说明：保存原始图像
    输入：
    @param cameraIndex 轮廓仪编号，如果为连接的第一台轮廓仪则设置1，第二台设置为2，以此类推，最大值6
    输出：无
    返回值：成功返回true，失败返回false。系统在程序根目录下创建Picture文件夹，文件名为LSXRawImage.bmp，X表示轮廓仪序号
    *************************************************************/
    [DllImport("LS_HD6.dll")]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool LSHD6_SaveRawImage(UInt32 cameraIndex);


    }

}
