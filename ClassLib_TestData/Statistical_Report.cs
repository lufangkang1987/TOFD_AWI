/*
 * Copyright(C) 2 2017 河南德朗智能科技有限公司
 * 文件名: Statistical_Report.cs
 * 文件功能描述: 统计报表：异常的开始结束位置
 * 目的：统计报表
 * 创建标识: 陈大伟 2020-5
 * 修改标识: 
 * 修改描述:
 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLib_TestData
{
    /// <summary>
    /// 统计报告
    /// </summary>
    public class Class_Test_Statistical_Report
    {
        /// <summary>
        /// 主项目ID号:YYMMDDHHmm
        /// </summary>
        public string _ID = "";
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        /// <summary>
        /// 焊缝ID号:  MMDDHHmmss
        /// </summary>
        public string _Sub_ID = "";
        public string Sub_ID
        {
            get { return _Sub_ID; }
            set { _Sub_ID = value; }
        }
        /// <summary>
        /// 焊缝编号
        /// </summary>
        public string _Part_No = "";
        public string Part_No
        {
            get { return _Part_No; }
            set { _Part_No = value; }
        }
        /// <summary>
        /// 开始位置
        /// </summary>
        public float _fl_Start_Distance = 0;
        public float fl_Start_Distance
        {
            get { return _fl_Start_Distance; }
            set { _fl_Start_Distance = value; }
        }
        /// <summary>
        /// 结束位置
        /// </summary>
        public float _fl_End_Distance = 0;
        public float fl_End_Distance
        {
            get { return _fl_End_Distance; }
            set { _fl_End_Distance = value; }
        }
        /// <summary>
        /// 缺陷长度
        /// </summary>
        public int _i_DefectLengt = 0;
        public int i_DefectLengt
        {
            get { return _i_DefectLengt; }
            set { _i_DefectLengt = value; }
        }
    }
}
