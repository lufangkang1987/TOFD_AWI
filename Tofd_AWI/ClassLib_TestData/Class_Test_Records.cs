/*
 * Copyright(C) 2 2017 河南德朗智能科技有限公司
 * 文件名: Test_Records.cs
 * 文件功能描述: 数据类型定义
 * 目的：检测焊缝原始记录定义
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
    /// 焊缝原始记录
    /// </summary>
    public  class Class_Test_Records
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
        /// 当前距离
        /// </summary>
        public float flDistance_X = 0;
        /// <summary>
        /// 异常 正常0，异常“1”
        /// </summary>
        public string  strMarking = "0";
        /// <summary>
        /// 缺陷深度
        /// </summary>
        public int i_Defectdepth = 0;
        /// <summary>
        /// 缺陷长度
        /// </summary>
        public int i_DefectLengt = 0;
        /// <summary>
        /// 报文数据
        /// </summary>
        public string strData = "";
        /// <summary>
        /// 报文2数据
        /// </summary>
        public string strData_2 = "";
        /// <summary>
        /// 波峰序列每个值对应的时间序列，每个值乘以10ns为时间的采样时间
        /// </summary>
        public string strWave_Time = "";
        /// <summary>
        /// 扩充1
        /// </summary>
        public   string strOther1 = "";
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
        /// 运行过程修改的参数
        /// </summary>
        public string TOFD_Para = "";
       
    }
}
