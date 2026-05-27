/*
 * Copyright(C) 2 2017 河南德朗智能科技有限公司
 * 文件名: Test_Parts.cs
 * 文件功能描述: 数据类型定义
 * 目的：检测部位（焊缝）记录定义
 * 创建标识: 陈大伟 2020-5
 * 修改标识: 
 * 修改描述:
 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib_TestData
{
    /// <summary>
    /// 焊缝主表信息
    /// </summary>
    public  class Class_Test_Parts
    {
        /// <summary>
        /// 主项目ID号:YYMMDDHHmm
        /// </summary>
        public string ID = "";
        
        /// <summary>
        /// 焊缝ID号:  MMDDHHmmss
        /// </summary>
        public string Sub_ID = "";
        /// <summary>
        /// 0:外壁  1：内壁
        /// </summary>
        public int DetectionSite = 0;
        

        /// <summary>
        /// 部位编号（B1_B2 起止码，以_为间隔,起码在前，止码在后）
        /// </summary>
        public  string Part_No = "";
       
      /// <summary>
        /// 壁厚
        /// </summary>
        public float  flThicknise =0;
       
        /// <summary>
        /// 探头间距
        /// </summary>
        public float ProbeSpacing = 0;
        /// <summary>
        /// 检测方向:检测编号起码（小数）在前、止码(大数)在后
        /// </summary>
        public   string  Detection_Direction = "";
       
        /// <summary>
        /// 声速
        /// </summary>
        public float  sPeed = 0;
        
        /// <summary>
        /// 延时
        /// </summary>
        public float T0 = 0;
       
        /// <summary>
        /// 先看哪些参数与计算有关 ","间隔
        /// 现在保存：m/X轴间隔   ---------------
        /// 通道参数   2 4 4 2 2   10   2 2 2 2 2 2    7  4 3 3 3 4  =24 *2 
        /// 10 切换当前要显示的通道，默认0  range(0)，显示为iChan+1;
        /// 11 设置当前通道增益，默认300   range(0,1100) ，界面单位dB，值=uData/10;
        /// 12 设置当前通道分频比，是范围对应的发码，默认200  范围， range(10,6000)，单位mm ;
        /// 13 设置当前通道零偏，默认0  延时, range(0,100)，单位us,
        /// 14 设置当前通道平移，默认0  延时, range(0,100)，单位us,
        /// 15 界面显示值=(uData-1)*5，单位ns; 
        /// 16 设置当前通道检波方式，默认3  0-3分别表示正检波，负检波，射频波，全检波
        /// 17 设置全局重复频率，默认2  0-7分别代表15Hz，30Hz，60Hz，100Hz，200Hz，
        /// 18 设置工作模式，默认1  0代表一发一收，1代表自发自收
        /// 19 设置带宽，默认0  0-3分别表示2-8M、0.5-4M、1-30M和5-15M
        /// 20 阻抗发码： 1  0代表48欧，1代表500欧
        /// 21 设置当前通道电压 ：3  0-4分别代表400V、200V和300V 
        /// </summary>
        public string TOFD_Para = "";
        /// <summary>
        /// 扩充1
        /// </summary>
        public string strOther1 = "";
      
        /// <summary>
        /// 扩充2
        /// </summary>
        public   string strOther2 = "";
      
        /// <summary>
        /// 扩充3
        /// </summary>
        public   int iOther1 = 0;
     
        /// <summary>
        /// 扩充4
        /// </summary>
        public   int iOther2 = 0;
       
        /// <summary>
        /// 备注
        /// </summary>
        public string ReMark = "";
     
    }
}
