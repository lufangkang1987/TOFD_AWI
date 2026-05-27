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

using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

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

    /// <summary>
    /// 检测报告参数
    /// </summary>
    public class Cls_Report_P
    {
        /// <summary>
        /// 文件读写
        /// </summary>
        public static ClassInterFace csInter = new ClassInterFace();
        public static string HardFileName = Application.StartupPath + "\\database\\SysConfig.ini";
        public void Init(int iType)
        {
            if (iType == 0)
            {
                Txt_Wtdw = csInter.IniReadDefine("Cls_Report_P", "Txt_Wtdw", "", HardFileName);
                Txt_Gcmc = csInter.IniReadDefine("Cls_Report_P", "Txt_Gcmc", "", HardFileName);
                Txt_Gjmc = csInter.IniReadDefine("Cls_Report_P", "Txt_Gjmc", "", HardFileName);
                Txt_Gjbh = csInter.IniReadDefine("Cls_Report_P", "Txt_Gjbh", "", HardFileName);
                Txt_Jcff = csInter.IniReadDefine("Cls_Report_P", "Txt_Jcff", "", HardFileName);
                Txt_Jcrq = csInter.IniReadDefine("Cls_Report_P", "Txt_Jcrq", "", HardFileName);
                Txt_Jcjgmc = csInter.IniReadDefine("Cls_Report_P", "Txt_Jcjgmc", "", HardFileName);
                Txt_Jcjgdz = csInter.IniReadDefine("Cls_Report_P", "Txt_Jcjgdz", "", HardFileName);
                Txt_Yb = csInter.IniReadDefine("Cls_Report_P", "Txt_Yb", "", HardFileName);

                Txt_Bg_Bgbh = csInter.IniReadDefine("Cls_Report_P", "Txt_Bg_Bgbh", "", HardFileName);
                Txt_Bz = csInter.IniReadDefine("Cls_Report_P", "Txt_Bz", "", HardFileName);

                Txt_Bg_Jlbh = csInter.IniReadDefine("Cls_Report_P", "Txt_Bg_Jlbh", "", HardFileName);
                Txt_Jcr = csInter.IniReadDefine("Cls_Report_P", "Txt_Jcr", "", HardFileName);
                Txt_Shr = csInter.IniReadDefine("Cls_Report_P", "Txt_Shr", "", HardFileName);
                Txt_Gg = csInter.IniReadDefine("Cls_Report_P", "Txt_Gg", "", HardFileName);
                Txt_CL = csInter.IniReadDefine("Cls_Report_P", "Txt_CL", "", HardFileName);

                Txt_Hjff = csInter.IniReadDefine("Cls_Report_P", "Txt_Hjff", "", HardFileName);
                Txt_Pkxs = csInter.IniReadDefine("Cls_Report_P", "Txt_Pkxs", "", HardFileName);
                Txt_Rclzt = csInter.IniReadDefine("Cls_Report_P", "Txt_Rclzt", "", HardFileName);
                Txt_Bmzt = csInter.IniReadDefine("Cls_Report_P", "Txt_Bmzt", "", HardFileName);
                Txt_Jcbw = csInter.IniReadDefine("Cls_Report_P", "Txt_Jcbw", "", HardFileName);
                Txt_Jcsj = csInter.IniReadDefine("Cls_Report_P", "Txt_Jcsj", "", HardFileName);
                Txt_Bmwd = csInter.IniReadDefine("Cls_Report_P", "Txt_Bmwd", "", HardFileName);
                Txt_Cysblb = csInter.IniReadDefine("Cls_Report_P", "Txt_Cysblb", "", HardFileName);
                Txt_Jcbl = csInter.IniReadDefine("Cls_Report_P", "Txt_Jcbl", "", HardFileName);

                Txt_Jcbz = csInter.IniReadDefine("Cls_Report_P", "Txt_Jcbz", "", HardFileName);
                Txt_Hgjb = csInter.IniReadDefine("Cls_Report_P", "Txt_Hgjb", "", HardFileName);
                Txt_Jsdj = csInter.IniReadDefine("Cls_Report_P", "Txt_Jsdj", "", HardFileName);
                Txt_CzZdsbh = csInter.IniReadDefine("Cls_Report_P", "Txt_CzZdsbh", "", HardFileName);

                Txt_Yqmc = csInter.IniReadDefine("Cls_Report_P", "Txt_Yqmc", "DAUT200自动化TOFD焊缝检测系统", HardFileName);
                Txt_Yqxh = csInter.IniReadDefine("Cls_Report_P", "Txt_Yqxh", "DAUT200", HardFileName);
                Txt_Yqbh = csInter.IniReadDefine("Cls_Report_P", "Txt_Yqbh", "", HardFileName);
                Txt_Sczz = csInter.IniReadDefine("Cls_Report_P", "Txt_Sczz", "", HardFileName);
                Txt_Sk = csInter.IniReadDefine("Cls_Report_P", "Txt_Sk", "", HardFileName);
                Txt_Ohj = csInter.IniReadDefine("Cls_Report_P", "Txt_Ohj", "", HardFileName);

                Txt_Wd = csInter.IniReadDefine("Cls_Report_P", "Txt_Wd", "", HardFileName);
                Txt_Jcm = csInter.IniReadDefine("Cls_Report_P", "Txt_Jcm", "", HardFileName);
                Txt_Jcqy = csInter.IniReadDefine("Cls_Report_P", "Txt_Jcqy", "", HardFileName);

                Txt_Tt_Td = csInter.IniReadDefine("Cls_Report_P", "Txt_Tt_Td", "", HardFileName);
                Txt_Tt_Xh = csInter.IniReadDefine("Cls_Report_P", "Txt_Tt_Xh", "", HardFileName);
                Txt_Tt_Bh = csInter.IniReadDefine("Cls_Report_P", "Txt_Tt_Bh", "", HardFileName);
                Txt_Tt_Lmd = csInter.IniReadDefine("Cls_Report_P", "Txt_Tt_Lmd", "", HardFileName);
                Txt_Tt_Sjck = csInter.IniReadDefine("Cls_Report_P", "Txt_Tt_Sjck", "", HardFileName);
            }
            else
            {
                csInter.INIWriteValue("Cls_Report_P", "Txt_Wtdw", Txt_Wtdw, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Gcmc", Txt_Gcmc, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Gjmc", Txt_Gjmc, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Gjbh", Txt_Gjbh, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Jcff", Txt_Jcff, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Jcrq", Txt_Jcrq, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Jcjgmc", Txt_Jcjgmc, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Jcjgdz", Txt_Jcjgdz, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Yb", Txt_Yb, HardFileName);

                csInter.INIWriteValue("Cls_Report_P", "Txt_Bg_Bgbh", Txt_Bg_Bgbh, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Bz", Txt_Bz, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Bg_Jlbh", Txt_Bg_Jlbh, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Jcr", Txt_Jcr, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Shr", Txt_Shr, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Gg", Txt_Gg, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_CL", Txt_CL, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Hjff", Txt_Hjff, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Pkxs", Txt_Pkxs, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Rclzt", Txt_Rclzt, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Bmzt", Txt_Bmzt, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Jcbw", Txt_Jcbw, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Jcsj", Txt_Jcsj, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Bmwd", Txt_Bmwd, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Cysblb", Txt_Cysblb, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Jcbl", Txt_Jcbl, HardFileName);

                csInter.INIWriteValue("Cls_Report_P", "Txt_Jcbz", Txt_Jcbz, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Hgjb", Txt_Hgjb, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Jsdj", Txt_Jsdj, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_CzZdsbh", Txt_CzZdsbh, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Yqmc", Txt_Yqmc, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Yqxh", Txt_Yqxh, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Yqbh", Txt_Yqbh, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Sczz", Txt_Sczz, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Sk", Txt_Sk, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Ohj", Txt_Ohj, HardFileName);

                csInter.INIWriteValue("Cls_Report_P", "Txt_Wd", Txt_Wd, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Jcm", Txt_Jcm, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Jcqy", Txt_Jcqy, HardFileName);

                csInter.INIWriteValue("Cls_Report_P", "Txt_Tt_Td", Txt_Tt_Td, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Tt_Xh", Txt_Tt_Xh, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Tt_Bh", Txt_Tt_Bh, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Tt_Lmd", Txt_Tt_Lmd, HardFileName);
                csInter.INIWriteValue("Cls_Report_P", "Txt_Tt_Sjck", Txt_Tt_Sjck, HardFileName);
            }
        }
        /// <summary>
        /// 委托单位
        /// </summary>
        public string Txt_Wtdw = "";
        /// <summary>
        /// 工程名称:
        /// </summary>
        public string Txt_Gcmc = "";
        /// <summary>
        /// 工件名称:
        /// </summary>
        public string Txt_Gjmc = "";
        /// <summary>
        /// 工件编号:
        /// </summary>
        public string Txt_Gjbh = "";
        /// <summary>
        /// 检测方法:
        /// </summary>
        public string Txt_Jcff = "";
        /// <summary>
        /// 检测日期: yyyy-MM-dd
        /// </summary>
        public string Txt_Jcrq = "";
        /// <summary>
        /// 检测机构名称:
        /// </summary>
        public string Txt_Jcjgmc = "";
        /// <summary>
        /// 检测机构地址
        /// </summary>
        public string Txt_Jcjgdz = "";
        /// <summary>
        /// 邮编:
        /// </summary>
        public string Txt_Yb = "";


        /// <summary>
        /// 报告编号:
        /// </summary>
        public string Txt_Bg_Bgbh = "";
        /// <summary>
        /// 记录编号:
        /// </summary>
        public string Txt_Bg_Jlbh = "";
        /// <summary>
        /// 检测人:
        /// </summary>
        public string Txt_Jcr = "";
        /// <summary>
        /// 审核人:
        /// </summary>
        public string Txt_Shr = "";
        /// <summary>
        /// 备注:
        /// </summary>
        public string Txt_Bz = "";

        //---------工作参数
        /// <summary>
        /// 规格::
        /// </summary>
        public string Txt_Gg = "";
        /// <summary>
        /// 材料:
        /// </summary>
        public string Txt_CL = "";
        /// <summary>
        /// 焊接方法:
        /// </summary>
        public string Txt_Hjff = "";
        /// <summary>
        /// 坡口型式:
        /// </summary>
        public string Txt_Pkxs = "";
        /// <summary>
        /// 热处理状态
        /// </summary>
        public string Txt_Rclzt = "";
        /// <summary>
        /// 表面状态
        /// </summary>
        public string Txt_Bmzt = "";
        /// <summary>
        /// 检测部位:
        /// </summary>
        public string Txt_Jcbw = "";
        /// <summary>
        /// 检测时机:
        /// </summary>
        public string Txt_Jcsj = "";
        /// <summary>
        /// 表面温度:
        /// </summary>
        public string Txt_Bmwd = "";
        /// <summary>
        /// 承压设备类别:
        /// </summary>
        public string Txt_Cysblb = "";
        /// <summary>
        /// 检测比例:
        /// </summary>
        public string Txt_Jcbl = "";
        /// <summary>
        /// 检测标准:
        /// </summary>
        public string Txt_Jcbz = "";
        /// <summary>
        /// 合格级别:
        /// </summary>
        public string Txt_Hgjb = "";
        /// <summary>
        /// 技术等级:
        /// </summary>
        public string Txt_Jsdj = "";
        /// <summary>
        /// 操作指导书编号
        /// </summary>
        public string Txt_CzZdsbh = "";
        /// <summary>
        /// 仪器名称
        /// </summary>
        public string Txt_Yqmc = "";
        /// <summary>
        /// 仪器型号
        /// </summary>
        public string Txt_Yqxh = "";
        /// <summary>
        /// 仪器编号
        /// </summary>
        public string Txt_Yqbh = "";
        /// <summary>
        /// 扫查装置
        /// </summary>
        public string Txt_Sczz = "";
        /// <summary>
        /// 试块
        /// </summary>
        public string Txt_Sk = "";
        /// <summary>
        /// 耦合剂
        /// </summary>
        public string Txt_Ohj = "";
        /// <summary>
        /// 检测温度
        /// </summary>
        public string Txt_Wd = "";
        /// <summary>
        /// 检测面
        /// </summary>
        public string Txt_Jcm = "";
        /// <summary>
        /// 检测区域
        /// </summary>
        public string Txt_Jcqy = "";
        /// <summary>
        /// 通道
        /// </summary>
        public string Txt_Tt_Td = "";
        /// <summary>
        /// 探头型号
        /// </summary>
        public string Txt_Tt_Xh = "";
        /// <summary>
        /// 探头编号
        /// </summary>
        public string Txt_Tt_Bh = "";
        /// <summary>
        /// 灵敏度设置
        /// </summary>
        public string Txt_Tt_Lmd = "";
        /// <summary>
        /// 时间窗口设置:
        /// </summary>
        public string Txt_Tt_Sjck = "";
        /// <summary>
        /// 频率
        /// </summary>
        public string Txt_PL = "";
        /// <summary>
        /// 镜片尺寸
        /// </summary>
        public string Txt_Jpcc = "";
        /// <summary>
        /// 楔块角度
        /// </summary>
        public string Txt_Xkjd = "";
        /// <summary>
        /// PCS
        /// </summary>
        public string Txt_PCS = "";
        /// <summary>
        /// 扫查步进
        /// </summary>
        public string Txt_Scbj = "";
        /// <summary>
        /// 扫查方式
        /// </summary>
        public string Txt_Scfs = "";


        /// <summary>
        /// 保存文件路径
        /// </summary>
        public  string m_Save_FilePath = "";
        /// <summary>
        /// 保存文件对应的文件名称
        /// </summary>
       public   string m_Save_FileName = "";
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

        /// <summary>
        /// 16进制字节变字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string HexToStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    if (i == 0)
                        returnStr += int.Parse(bytes[i].ToString("X2")).ToString();
                    else
                        returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
      

        /// <summary>
        /// 当前文件对应数据个数
        /// </summary>
        public int m_iNum_FileData = 0;
           /// <summary>
        /// 字符串转字节数组
        /// </summary>
        /// <param name="strHex"></param>
        /// <returns></returns>
        public static byte[] StrToHex(string strHex)
        {
            //清空格
            strHex = strHex.Replace(" ", "");
            if ((strHex.Length % 2) != 0)
                strHex = strHex.Insert(0, "0");
            //数组
            byte[] returnBytes = new byte[strHex.Length / 2];
            //转换
            try
            {
                for (int i = 0; i < returnBytes.Length; i++)
                    returnBytes[i] = Convert.ToByte(strHex.Substring(i * 2, 2), 16);
            }
            catch (Exception e)
            {
                return new byte[strHex.Length / 2];
                //throw e;
            }
            return returnBytes;
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

    /// <summary>
    /// 报告声明页
    /// </summary>
    public class ClassPage
    {
        /// <summary>
        /// 工件名称
        /// </summary>
        public string Gjmc = "";
        /// <summary>
        /// 报告编号
        /// </summary>
        public string Bgbh = "";
        /// <summary>
        /// 页数
        /// </summary>
        public int iPages = 0;
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark = "";
        /// <summary>
        /// 批准人
        /// </summary>
        public string Pzr = "";
        /// <summary>
        /// 日期
        /// </summary>
        public string Date = "";
    }

  
}
