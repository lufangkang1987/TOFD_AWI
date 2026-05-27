using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tofd_AWI.Class;

namespace Tofd_AWI.From
{
    public partial class Frm_TOFD : Form
    {
        /// <summary>
        /// 工艺文件个数
        /// </summary>
        int m_Craft_iNum = 0;
        /// <summary>
        /// 窗体是否激活
        /// </summary>
        bool m_blActive = false;
      
        public Frm_TOFD()
        {
            InitializeComponent();
        }

        private void Bt_Link_Click(object sender, EventArgs e)
        {
            if (SysInfo.Thread_Get_TofdData != null) SysInfo.Thread_Get_TofdData.Abort();
            Lb_Send.Text = "联机TOFD...";
            bool _blRet = SysInfo.m_Tofd_DLL.Link();
            Lb_Send.Text = "联机TOFD: 结束";
            Lb_Link.Text = _blRet ? "成功" : "失败";
            Lb_Link.ForeColor = _blRet ? Color.Green : Color.Red;
        }

        private void Frm_TOFD_Load(object sender, EventArgs e)
        {
            InitFrm();
            Lb_Send.Text = "联机TOFD: 结束";
            Lb_Link.Text = SysInfo.m_Tofd_DLL.blNetLink ? "成功" : "失败";
            Lb_Link.ForeColor = SysInfo.m_Tofd_DLL.blNetLink ? Color.Green : Color.Red;

            SysInfo.m_blFrmOpen[0] = true;
            m_blActive = true;
        }

        private void VoltChange()
        {
            switch (SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iVolt)
            {
                case 0:
                    Txt_Gytj.Text = "400V";
                    break;
                case 1:
                    Txt_Gytj.Text = "200V";
                    break;
                case 2:
                    Txt_Gytj.Text = "300V";
                    break;
            }
        }
        /// <summary>
        /// 频率值刷新界面
        /// </summary>
        private void FreqChange()
        {
            switch ((int)SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iRepeatFreq)
            {
                case 0:
                    Txt_Cfpl.Text = "15Hz"; break;
                case 1:
                    Txt_Cfpl.Text = "30Hz"; break;
                case 2:
                    Txt_Cfpl.Text = "60Hz"; break;
                case 3:
                    Txt_Cfpl.Text = "100Hz"; break;
                case 4:
                    Txt_Cfpl.Text = "200Hz"; break;
                case 5:
                    Txt_Cfpl.Text = "300Hz"; break;
                case 6:
                    Txt_Cfpl.Text = "400Hz"; break;
                case 7:
                    Txt_Cfpl.Text = "500Hz"; break;
                case 8:
                    Txt_Cfpl.Text = "1kHz"; break;

            }
        }
        private void Bt_Ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// 界面初始化
        /// </summary>
        private void InitFrm(int iType = 0)
        {
            int i = SysInfo.m_Tofd_DLL.m_icurChan;
            //1 增益
            Track_Zy.Value = SysInfo.m_Tofd_DLL.m_pSparam[i].m_idB;
            Txt_Zy.Text = (SysInfo.m_Tofd_DLL.m_pSparam[i].m_idB / 10).ToString("f1") + "dB";
            //2 范围
            Track_Fw.Value = SysInfo.m_Tofd_DLL.m_pSparam[i].m_iRange;
            Txt_Fw.Text = (SysInfo.m_Tofd_DLL.m_pSparam[i].m_iRange).ToString() + "mm";
            //3 零偏
            Track_Lp.Value = SysInfo.m_Tofd_DLL.m_pSparam[i].m_iZeroTime;
            float _T = SysInfo.m_Tofd_DLL.m_pSparam[i].m_iZeroTime / 100f;
            Txt_Lp.Text = _T.ToString("f2") + "us";
            SysInfo.m_Tofd_DLL.m_pSparam_Real[SysInfo.m_Tofd_DLL.m_icurChan].T0 = _T;
            //4 平移
            Track_Py.Value = SysInfo.m_Tofd_DLL.m_pSparam[i].m_iParallelTime;
            Txt_Py.Text = (SysInfo.m_Tofd_DLL.m_pSparam[i].m_iParallelTime / 100).ToString("f2") + "us";

            //5 脉冲个数 待命
            Track_Mckd.Value = SysInfo.m_Tofd_DLL.m_pSparam[i].m_iPulWidthCode;
            Txt_Mckd.Text = ((SysInfo.m_Tofd_DLL.m_pSparam[i].m_iPulWidthCode - 1) * 5).ToString() + "ns";
            //6 检波方式
            Cmb_Jbfs.SelectedIndex = SysInfo.m_Tofd_DLL.m_pSparam[i].m_iDemodulation_Flag;
            //7  重复频率
            Track_Cfpl.Value = SysInfo.m_Tofd_DLL.m_pSparam[i].m_iRepeatFreq;
            FreqChange();
            //7 工作方式
            Cmb_Gzfs.SelectedIndex = SysInfo.m_Tofd_DLL.m_pSparam[i].m_iWorkMode;

            //8 宽带选择
            Cmb_Kdxz.SelectedIndex = SysInfo.m_Tofd_DLL.m_pSparam[i].m_iSecBandWidthF;
            //9 阻抗匹配
            Cmb_Zkpp.SelectedIndex = SysInfo.m_Tofd_DLL.m_pSparam[i].m_iImpedanceF;
            //10 高压调节
            Track_Gytj.Value = SysInfo.m_Tofd_DLL.m_pSparam[i].m_iVolt;
            VoltChange();
            //11 速度
            if (SysInfo.m_Tofd_DLL.m_pSparam[i].m_dSpeed < Track_Sd.Minimum)
                SysInfo.m_Tofd_DLL.m_pSparam[i].m_dSpeed = Track_Sd.Minimum;
            Track_Sd.Value = SysInfo.m_Tofd_DLL.m_pSparam[i].m_dSpeed;
            Txt_Sd.Text = (SysInfo.m_Tofd_DLL.m_pSparam[i].m_dSpeed).ToString() + "m/s";


            //14 前放开关
            Cmb_Qf.SelectedIndex = SysInfo.m_Tofd_DLL.m_pSparam[i].m_iForword;
         
            //重新调用不刷新工艺文件列表
            if (iType == 1) return;

            #region 工艺文件列表刷新
            SysInfo.m_blFrm_TOFD_Open[0] = false;
            SysInfo.m_blFrm_TOFD_Open[1] = false;

            string _strT = SysInfo.csInter.IniReadDefine("Tofd_Craft", "CurrCraft_Name", "", SysInfo.HardFileName);
            m_Craft_iNum = int.Parse(SysInfo.csInter.IniReadDefine("Tofd_Craft", "Num", "0", SysInfo.HardFileName));
            Cmb_Gywj.Items.Clear();
            if (_strT != "" && m_Craft_iNum > 0)
            {
                Txt_Gywjmc.Text = _strT;

                for (int _iNo = 0; _iNo < m_Craft_iNum; _iNo++)
                {
                    _strT = SysInfo.csInter.INIReadValue("Tofd_Craft", (_iNo + 1).ToString(), "", SysInfo.HardFileName);
                    if (_strT != "")
                        Cmb_Gywj.Items.Add(_strT);
                }
            }
            #endregion
        }
        private void Track_Zy_Scroll(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_idB = Track_Zy.Value;
            Tofd .SendCmdDB(SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_idB);
           
            Txt_Zy.Text = (SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_idB / 10f).ToString("f1") + "dB";
            Lb_Send.Text = "增益: " + Txt_Zy.Text;
        }

        private void Track_Fw_Scroll(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iRange = Track_Fw.Value;
            Txt_Fw.Text = (SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iRange ).ToString("") + "mm";

            Tofd.SendCmdFreqRatio(SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iRange, SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_dSpeed);
            Lb_Send.Text = "范围: " + Txt_Fw.Text;
        }
        private void Track_Lp_Scroll(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iZeroTime = Track_Lp.Value;
            Tofd.SendCmdZeroTime(SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iZeroTime);
           
            Txt_Lp.Text = (SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iZeroTime /100f).ToString("f2") + "us";
            Lb_Send.Text = "零偏: " + Txt_Lp.Text;
        }

        private void Track_Py_Scroll(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iParallelTime = Track_Py.Value;
            Tofd.SendCmdParallel(SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iParallelTime);
          
            Txt_Py.Text = (SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iParallelTime / 100f).ToString("f2") + "us";
            Lb_Send.Text = "平移: " + Txt_Py.Text;
        }

        private void Cmb_Jbfs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iDemodulation_Flag = (byte)Cmb_Jbfs.SelectedIndex;
            Tofd.SendCmdWaveType((int)SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iDemodulation_Flag);
            Lb_Send.Text = "检波方式：" + Cmb_Jbfs.Text ;
        }

        private void Track_Cfpl_Scroll(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iRepeatFreq = (byte)Track_Cfpl.Value;
            Tofd.SendCmdRepeatFreq((int)SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iRepeatFreq);
         
            FreqChange();
            Lb_Send.Text = "重复频率:: " + Txt_Cfpl.Text;
        }

        private void Cmb_Gzfs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iWorkMode = (byte)Cmb_Gzfs.SelectedIndex;
            Tofd.SendCmdWorkMode((int)SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iWorkMode);
            Lb_Send.Text = "工作方式:: " + Cmb_Gzfs.Text;
        }

        private void Cmb_Kdxz_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iSecBandWidthF = (byte)Cmb_Kdxz.SelectedIndex;
            Tofd.SendCmdBandWidth((int)SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iSecBandWidthF);
            Lb_Send.Text = "宽带选择：" + Cmb_Kdxz.Text;
        }

        private void Cmb_Zkpp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iImpedanceF = (byte)Cmb_Zkpp.SelectedIndex;
            Tofd.SendCmdImpdance((int)SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iImpedanceF);

            Lb_Send.Text = "阻抗匹配：" + Cmb_Zkpp.Text;
        }

        private void Track_Gytj_Scroll(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iVolt = (byte)Track_Gytj.Value;
            Tofd.SendCmdHighVoltage((int)SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iVolt);
          
            VoltChange();
            Lb_Send.Text = "高压调节:：" + Txt_Gytj.Text;
        }

        private void Track_Sd_Scroll(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_dSpeed = Track_Sd.Value;
            Txt_Sd.Text = (SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_dSpeed).ToString() + "m/s";
          
            Tofd.SendCmdFreqRatio((int)SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iRange, SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_dSpeed);
            Lb_Send.Text = "声速: " + Txt_Sd.Text;
        }

        private void Cmb_Qf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iForword = (byte)Cmb_Qf.SelectedIndex;
            Tofd.SendCmdForword((int)SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iForword);
            Lb_Send.Text = "前放开关: " + Cmb_Qf.Text;
        }

        private void Track_Mckd_Scroll(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
             SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iPulWidthCode =(ushort) Track_Mckd.Value ;
            Txt_Mckd.Text = ((SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iPulWidthCode - 1) * 5).ToString() + "ns";
            Tofd.SendCmdPulWid((int)SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iPulWidthCode);
            Lb_Send.Text = "脉冲个数: " + Txt_Mckd.Text;
        }

        private void Bt_CalCu_Click(object sender, EventArgs e)
        {
            Bt_CalCu.Enabled = false;
            Frm_Tofd_Calcu _flFrm = new Frm_Tofd_Calcu();
            _flFrm.Show ();
            Bt_CalCu.Enabled = true;
        }
        /// <summary>
        /// 探头校准
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_Jz_Click(object sender, EventArgs e)
        {
            Bt_Jz.Enabled = false;
            Frm_Jz   _flFrm = new Frm_Jz();
            _flFrm.Show  ();
            Bt_Jz.Enabled = true;
        }

        private void Frm_TOFD_FormClosing(object sender, FormClosingEventArgs e)
        {
            SysInfo.m_blFrmOpen[0] = false;
            SysInfo.Init(1);
            
        }
        /// <summary>
        /// 工艺文件保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_Mody_Click(object sender, EventArgs e)
        {
            //1 文件名检测
            if (SysInfo.m_blFrm_TOFD_Open[0] || SysInfo.m_blFrm_TOFD_Open[1])
            {
                MessageBox.Show("请先关闭参数计算或者校准界面!");
                return;
            }
            if (Txt_Gywjmc.Text == "")
            {
                //组织文件名
                Txt_Gywjmc.Text = GetCurr_CraftFimeName();

                MessageBox.Show("新文件名：请确认!");
                return;
            }
            //2 更新系统文件名
            SysInfo.m_Tofd_DLL.m_strSection = Txt_Gywjmc.Text;

            //1 寻找是否有重名
            bool _blOk = false;
            for(int i=0;i<Cmb_Gywj .Items .Count;i++)
            {
                if(Cmb_Gywj .Text == SysInfo.m_Tofd_DLL.m_strSection)
                {
                    _blOk = true;
                    break;
                }
            }

            if (_blOk == false)
            {
                m_Craft_iNum++;
                SysInfo.csInter.INIWriteValue("Tofd_Craft", "Num", m_Craft_iNum.ToString(), SysInfo.HardFileName);
                SysInfo.csInter.INIWriteValue("Tofd_Craft", "CurrCraft_Name", SysInfo.m_Tofd_DLL.m_strSection, SysInfo.HardFileName);
                SysInfo.csInter.INIWriteValue("Tofd_Craft", m_Craft_iNum.ToString(), SysInfo.m_Tofd_DLL.m_strSection, SysInfo.HardFileName);

                Cmb_Gywj.Items.Add(SysInfo.m_Tofd_DLL.m_strSection);
            }

            // 3 新文件保存对应参数
            //3.1 探头参数保存
            SysInfo.Init(1);
            //3.2 计算参数
            SysInfo.IniIt_Calcu();
            //3.3 校准参数
            SysInfo.Init_Jz();
        }
        /// <summary>
        /// 得到当前文件名
        /// </summary>
        private string  GetCurr_CraftFimeName()
        {
            string _strRet = "平板";
            int _iCurrChan = SysInfo.m_Tofd_DLL.m_icurChan;
            //1 类型
            int _iT = SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iPcsMode;
            switch (_iT)
            {
                case 0://平板
                    _strRet = "平板";
                    break;
                case 1://圆弧外壁
                    _strRet = "外壁";
                    break;
                case 2://圆弧内壁
                    _strRet = "内壁";
                    break;

            }
            //2 板子厚度
            float _T = SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsEnd - SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsStart;
            _strRet += "厚度" + _T.ToString ("f0");
            //3 楔块角度
            string _Jd = SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsAngle.ToString("f0");
            _strRet += "角度" + _Jd;
            return _strRet;
        }
        private void Bt_Dell_Click(object sender, EventArgs e)
        {
            if (Cmb_Gywj.Text == "")
            {
                MessageBox.Show("列表的文件名不能为空!");
                return;
            }
            if(m_Craft_iNum>0 && Cmb_Gywj.SelectedIndex>-1)
            {
                //2 文件删除
                SysInfo.csInter.EraseSection(Cmb_Gywj.SelectedItem .ToString (), SysInfo.HardFileName);
                SysInfo.csInter.EraseSection("Tofd_Craft", SysInfo.HardFileName);
                SysInfo.WaitTime(0.1f);
                //3 重新组织列表文件        
                //1 下拉框删除
                Cmb_Gywj.Items.RemoveAt(Cmb_Gywj.SelectedIndex);
                for(int i=0;i< Cmb_Gywj.Items.Count;i++)
                    SysInfo.csInter.INIWriteValue("Tofd_Craft", (i+1).ToString (), Cmb_Gywj.Items[i].ToString (), SysInfo.HardFileName);
                SysInfo.csInter.INIWriteValue("Tofd_Craft", "Num", Cmb_Gywj.Items.Count.ToString(), SysInfo.HardFileName);
                SysInfo.csInter.INIWriteValue("Tofd_Craft", "CurrCraft_Name", "", SysInfo.HardFileName);
                m_Craft_iNum = Cmb_Gywj.Items.Count;
                //4 刷新当前文件名
                Cmb_Gywj.Text = "";
                Txt_Gywjmc.Text = "";
            }
        }

        private void Txt_Gywjmc_Click(object sender, EventArgs e)
        {
            if (SysInfo.m_blFrm_TOFD_Open[0] || SysInfo.m_blFrm_TOFD_Open[1])
            {
                MessageBox.Show("请先关闭参数计算或者校准界面!");
                return;
            }
            if (Txt_Gywjmc.Text == "")
            {
                //组织文件名
                Txt_Gywjmc.Text = GetCurr_CraftFimeName();

                MessageBox.Show("新文件名：请确认!");
                return;
            }
        }

        private void Cmb_Gywj_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            if (SysInfo.m_blFrm_TOFD_Open[0] || SysInfo.m_blFrm_TOFD_Open[1])
            {
                MessageBox.Show("请先关闭参数计算或者校准界面!");
                return;
            }
            #region 添加新的工艺文件

            //1 文件主键
            SysInfo.m_Tofd_DLL.m_strSection = Cmb_Gywj.SelectedItem .ToString ();
            SysInfo.csInter.INIWriteValue("Tofd_Craft", "CurrCraft_Name", SysInfo.m_Tofd_DLL.m_strSection, SysInfo.HardFileName);
            //2 调用数据
            SysInfo.Init();
            //3 刷新界面
            InitFrm(1);
            Txt_Gywjmc.Text = SysInfo.m_Tofd_DLL.m_strSection;
            #endregion
        }

        private void Bt_Gywjm_Brsh_Click(object sender, EventArgs e)
        {
            Txt_Gywjmc.Text = GetCurr_CraftFimeName();
        }
    }
}
