/*
 * Copyright(C) 2 2021 河南德朗智能科技有限公司
 * 文件名: Struct.cs
 * 文件功能描述: 数据类型定义
 * 目的：检测项目记录定义
 * 
 * 文件格式：
 *   每次产生一个数据库，数据库名称结构：单位名称前2位_受检设备编号前2位_ID
 *   Test_Item：保存主表信息
 *   Test_Parts：保存检测焊缝信息
 *   Test_Records：保存每条焊缝检测的原始信息
 *   Statistical_Report：检测完成后，统计当前焊缝异常
 *   
 * 创建标识: 陈大伟 2021-5
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
    /// 项目记录
    /// </summary>
    public class Class_Test_Item
    {
        /// <summary>
        /// 主项目ID号:YYMMDDHHmm   
        /// </summary>
        public string _ID = "";
        /// <summary>
        /// 主项目ID号:YYMMDDHHmm  
        /// </summary>
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public string _Dwmc = "";
        /// <summary>
        /// 单位名称
        /// </summary>
        public string Dwmc
        {
            get { return _Dwmc; }
            set { _Dwmc = value; }
        }
        public string _ItemName = "";
        /// <summary>
        /// 工程名称
        /// </summary>
        public string ItemName
        {
            get { return _ItemName; }
            set { _ItemName = value; }
        }
        public string _Sbbh = "";
        /// <summary>
        /// 检测设备编号
        /// </summary>
        public string Sbbh
        {
            get { return  _Sbbh ; }
            set {_Sbbh  = value; }
        }
       
        /// <summary>
        /// 表面状况（清理《12.5um、打磨、毛坯）
        /// </summary>
        public string _Surface_condition = "";
        /// <summary>
        /// 表面状况（清理《12.5um、打磨、毛坯）
        /// </summary>
        public string Surface_condition
        {
            get { return _Surface_condition; }
            set { _Surface_condition = value; }
        }
        /// <summary>
        /// 材质（Q345B）、
        /// </summary>
        public string _Material = "";
        /// <summary>
        /// 材质（Q345B）、
        /// </summary>
        public string Material
        {
            get { return _Material; }
            set { _Material = value; }
        }
        /// <summary>
        /// 坡口形式
        /// </summary>
        public string _strPkxs = "";
        /// <summary>
        /// 坡口形式
        /// </summary>
        public string strPkxs
        {
            get { return _strPkxs; }
            set { _strPkxs = value; }
        }
        /// <summary>
        /// 焊接类型
        /// </summary>
        public string _Weldingtype = "";
        /// <summary>
        /// 焊接类型
        /// </summary>
        public string Weldingtype
        {
            get { return _Weldingtype; }
            set { _Weldingtype = value; }
        }


        /// <summary>
        /// 检测设备名称
        /// </summary>
        public string _JcSbName = "";
        /// <summary>
        /// 检测设备名称
        /// </summary>
        public string JcSbName
        {
            get { return _JcSbName; }
            set { _JcSbName = value; }
        }

        /// <summary>
        /// 检测仪器编号
        /// </summary>
        public string _Serial_number = "";
        /// <summary>
        /// 检测仪器编号
        /// </summary>
        public string Serial_number
        {
            get { return _Serial_number; }
            set { _Serial_number = value; }
        }
        /// <summary>
        /// 检测设备型号
        /// </summary>
        public string _Model = "";
        /// <summary>
        /// 检测设备型号
        /// </summary>
        public string Model
        {
            get { return _Model; }
            set { _Model = value; }
        }

        /// <summary>
        /// 试块：CSK-1A
        /// </summary>
        public string _Testblock = "";
        /// <summary>
        /// 试块：CSK-1A
        /// </summary>
        public string Testblock
        {
            get { return _Testblock; }
            set { _Testblock = value; }
        }

        /// <summary>
        /// 检测比例（100%）
        /// </summary>
        public string _Test_Proportion = "";
        /// <summary>
        /// 检测比例（100%）
        /// </summary>
        public string Test_Proportion
        {
            get { return _Test_Proportion; }
            set { _Test_Proportion = value; }
        }
        /// <summary>
        /// 检测标准（NB/T4730-10-2015）
        /// </summary>
        public string _Testing_Standard = "";
        /// <summary>
        /// 检测标准（NB/T4730-10-2015）
        /// </summary>
        public string Testing_Standard
        {
            get { return _Testing_Standard; }
            set { _Testing_Standard = value; }
        }

        /// <summary>
        /// 工艺文件编号
        /// </summary>
        public string _Process_Doc_Number = "";
        /// <summary>
        /// 工艺文件编号
        /// </summary>
        public string Process_Doc_Number
        {
            get { return _Process_Doc_Number; }
            set { _Process_Doc_Number = value; }
        }

        /// <summary>
        /// 监理单位单位
        /// </summary>
        public string _strJLdw = "";
        /// <summary>
        /// 监理单位单位
        /// </summary>
        public string strJLdw
        {
            get { return _strJLdw; }
            set { _strJLdw = value; }
        }
        /// <summary>
        /// 制造单位
        /// </summary>
        public string _strZzdw = "";
        /// <summary>
        /// 制造单位
        /// </summary>
        public string strZzdw
        {
            get { return _strZzdw; }
            set { _strZzdw = value; }
        }
        /// <summary>
        /// 报告编号
        /// </summary>
        public string _strBgbh = "";
        /// <summary>
        /// 报告编号
        /// </summary>
        public string strBgbh
        {
            get { return _strBgbh; }
            set { _strBgbh = value; }
        }
        /// <summary>
        /// 报告人以及资质UT(TOFD)-II
        /// </summary>
        public string _strJyy = "";
        /// <summary>
        /// 报告人以及资质UT(TOFD)-II
        /// </summary>
        public string strJyy
        {
            get { return _strJyy; }
            set { _strJyy = value; }
        }
        /// <summary>
        /// 审核人 以及资质UT(TOFD)-II
        /// </summary>
        public string _strHyy = "";
        /// <summary>
        /// 审核人 以及资质UT(TOFD)-II
        /// </summary>
        public string strHyy
        {
            get { return _strHyy; }
            set { _strHyy = value; }
        }
        /// <summary>
        /// 监理人
        /// </summary>
        public string _strSp = "";
        /// <summary>
        /// 监理人
        /// </summary>
        public string strSp
        {
            get { return _strSp; }
            set { _strSp = value; }
        }


        /// <summary>
        /// 扩充1
        /// </summary>
        public string _strOther1 = "";
        public string strOther1
        {
            get { return _strOther1; }
            set { _strOther1 = value; }
        }
        /// <summary>
        /// 扩充2
        /// </summary>
        public string _strOther2 = "";
        public string strOther2
        {
            get { return _strOther2; }
            set { _strOther2 = value; }
        }
        /// <summary>
        /// 扩充3
        /// </summary>
        public int _iOther1 = 0;
        public int iOther1
        {
            get { return _iOther1; }
            set { _iOther1 = value; }
        }
        /// <summary>
        /// 扩充4
        /// </summary>
        public int _iOther2 = 0;
        public int iOther2
        {
            get { return _iOther2; }
            set { _iOther2 = value; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string _ReMark = "";
        public string ReMark
        {
            get { return _ReMark; }
            set { _ReMark = value; }
        }
    }
    /// <summary>
    /// 屏幕刻度
    /// </summary>
    public class Class_Screen_Kd
    {
        /// <summary>
        /// 屏幕刻度尺：起始刻度位置mm
        /// </summary>
        public float Chart_Run_flStart_Distance = -1;
        /// <summary>
        /// 屏幕刻度尺：结束刻度位置mm
        /// </summary>
        public float Chart_Run_flEnd_Distance = -1;
    }

}
