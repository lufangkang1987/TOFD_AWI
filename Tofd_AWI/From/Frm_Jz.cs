using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;
using Tofd_AWI.Class;
namespace Tofd_AWI.From
{
    public partial class Frm_Jz : Form
    {
        #region 变量

        /// <summary>
        /// 一个数据点高度
        /// </summary>
        public float m_fWaveFramePerHeight;
        /// <summary>
        /// 一个数据点宽度
        /// </summary>
        public float m_fWaveFramePerWidth;
        /// <summary>
        /// 直通波开始时间
        /// </summary>
        float m_flT1 = -1;
        /// <summary>
        /// 底面波开始时间
        /// </summary>
        float m_flTb = -1;
        /// <summary>
        /// 读取通道数据
        /// </summary>
        private Thread Thread_Get_TofdData = null;
        /// <summary>
        /// 画波形
        /// </summary>
        delegate void Delg_PlantAllWave();
        #endregion
        public Frm_Jz()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 计算声速和探头初始时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_T_Ok_Click(object sender, EventArgs e)
        {
            Txt_C_Cz.Text = ""; Txt_Speed.Text = ""; Txt_T0.Text = "";

            //弧长计算h:高b:弦长   double _L =Math .Sqrt (b2 + 16 * h2) / 2 + b2 * ln((4 * h + sqrt(b2 + 16 * h2)) / b) / (8 * h)
            float _flPcs = 0, _flThick = 0;
            if (Txt_Pcs.Text == "") { MessageBox.Show("PCS无效"); return; }
            _flPcs = float.Parse(Txt_Pcs.Text) / 2;
            if (_flPcs <= 0) { MessageBox.Show("PCS无效"); return; }

            if (Txt_Sk_Thick.Text == "") { MessageBox.Show("厚度值无效"); return; }

            _flThick = float.Parse(Txt_Sk_Thick.Text);
            if (_flThick <= 0) { MessageBox.Show("厚度值无效"); return; }

            m_flT1 = float.Parse(Txt_T1.Text);
            m_flTb = float.Parse(Txt_Tb.Text);
            if (m_flT1 <= 0) { MessageBox.Show("直通波时间无效"); return; }
            if (m_flTb <= 0) { MessageBox.Show("底面波时间无效"); return; }
            if (m_flTb < m_flT1) { MessageBox.Show("底面波时间 >直通波时间"); return; }

            //计算速度
            double _db_C = 0;
            double _db_C_Cz = m_flTb - m_flT1;//底波 - 直通波时间
            double _db_Sql = Math.Sqrt(_flPcs * _flPcs + _flThick * _flThick);
            _db_C = 2 * (_db_Sql - _flPcs) / _db_C_Cz;
            //计算T0
            double _db_T0 = 0;
            _db_T0 = m_flTb - 2 * _db_Sql / _db_C;

            //显示结果
            Txt_Speed.Text = _db_C.ToString("f4");
            Txt_T0.Text = _db_T0.ToString("f3");
            Txt_C_Cz.Text = _db_C_Cz.ToString("f3");

            int _iCurrChan = SysInfo.m_Tofd_DLL.m_icurChan;
            SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_dSpeed = (int)(_db_C * 1000);
            SysInfo.m_Tofd_DLL.m_pSparam_Real[_iCurrChan].T0 = (float)_db_T0;

            SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsLen = float.Parse(Txt_Pcs.Text);
        }
        /// <summary>
        /// 参数读写
        /// </summary>
        /// <param name="iType"></param>
        private void Init()
        {
            #region 读参数
            Txt_T0.Text = SysInfo.m_Tofd_DLL.m_pSparam_Real[SysInfo.m_Tofd_DLL.m_icurChan].T0.ToString();

                //SysInfo.csInter.IniReadDefine(SysInfo.m_Tofd_DLL.m_strSection, "T0",
                //        SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].T0.ToString(), SysInfo.HardFileName);


            Txt_L0.Text = SysInfo.csInter.IniReadDefine(SysInfo.m_Tofd_DLL.m_strSection, "L0",
                        SysInfo.m_Tofd_DLL.m_pSparam_Real[SysInfo.m_Tofd_DLL.m_icurChan].L0.ToString(), SysInfo.HardFileName);

            Txt_L0_Distan.Text = SysInfo.m_Tofd_DLL.m_pSparam_Real[SysInfo.m_Tofd_DLL.m_icurChan].L0_Distan.ToString("f0");

            Txt_Speed.Text = SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_dSpeed.ToString();
            #endregion
        }
        private void Frm_Jz_Load(object sender, EventArgs e)
        {
            SysInfo.m_blFrm_TOFD_Open[1] = true;
            Init();
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            this.Left = 0;
            this.Top = 0;
            m_fWaveFramePerHeight = Pic_A.Height / ((float)Tofd.UTS_DATA_HEIGHT);
            m_fWaveFramePerWidth = Pic_A.Width / ((float)Tofd.UTS_DATA_WIDTH);
            //  SysInfo.m_Plant.m_iKd_W_512Num = Pic_A.Width / SysInfo.m_Plant.m_iJgNum;

            SysInfo.m_Tofd_DLL.m_i_State = 1;
            Thread_ReadUI();
        }
        /// <summary>
        /// 启动TOFD数据处理线程
        /// </summary>
        private void Thread_ReadUI()
        {
            if (Thread_Get_TofdData != null) Thread_Get_TofdData.Abort();
            Thread_Get_TofdData = new Thread(new ThreadStart(Read_TOFD));

            Thread_Get_TofdData.Name = "Thread_JZ";
            Thread_Get_TofdData.IsBackground = true;
            Thread_Get_TofdData.Start();
        }
        private void Read_TOFD()
        {
            while (SysInfo.m_iRun != 10)
            {
                #region 画图
                if (this.InvokeRequired == true)
                {
                    try
                    {
                        Delg_PlantAllWave ms = new Delg_PlantAllWave(PlantWave);
                        this.Invoke(ms, new object[] { });
                    }
                    catch
                    { }
                }
                else
                {
                    PlantWave();
                }

                #endregion
            }
        }
        private void PlantWave()
        {
            #region A扫图
     //       if (SysInfo.m_Tofd_DLL.blNetLink ||
     //           SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_BScanMode == 2)
                SysInfo.m_Tofd_DLL.setPeakBuffer(m_fWaveFramePerHeight,
                                                 m_fWaveFramePerWidth,
                                                 SysInfo.m_iRun);

            SysInfo.m_Plant.Plant_A(Pic_A, SysInfo.m_Tofd_DLL);
            SysInfo.WaitTime(0.01f);
            Application.DoEvents();
            #endregion A 扫
        }
        /// <summary>
        /// 工件中声速边界角度计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_Calcu_Click(object sender, EventArgs e)
        {
            try
            {
                #region 1 选择金属的声束中心角， 计算楔块中入射角度θ sinθ P = sinθ L C p/ C L 
                double _Zxj = double.Parse(Txt_Zxj.Text);//工件折射中心角度
                double _Cp = double.Parse(Txt_Xk_Ss.Text);//楔块声速
                double _CL = double.Parse(Txt_Gj_Ss.Text);//工件中纵波声速
                double _Hd_Jd = 180f / Math.PI;
                double _Jd_Hd = Math.PI / 180f;
                double _Q = Math.Asin(Math.Sin(_Zxj * _Jd_Hd) * _Cp / _CL) * _Hd_Jd;
                #endregion

                #region 2 计算楔块中声束扩散角γ sinγ=Fλ/D=FC p /Df
                double _F = double.Parse(Txt_F.Text);
                double _D = double.Parse(Txt_D.Text);
                double _f = double.Parse(Txt_Ph.Text);
                double _γ = Math.Asin(_F * _Cp / (_D * _f)) * _Hd_Jd;
                #endregion

                #region 3 求出楔块中扩散的上下边界角 γ 上 = θ p ＋γ 和 γ 下 = θ p －γ
                double _Xk_Q_S = _Q + _γ;
                double _Xk_Q_X = _Q - _γ;
                #endregion

                #region 4 用 Ｓｎｅｌｌ定律分别求出工件中声束边界角度
                //sinγ L上 = sin γ上 C L / C P
                //sinγ L下 = sin γ下 C L / C P
                double _C_BL = _CL / _Cp;
                //     double __Gj_Q_S = Math.Asin(Math .Sin (_Xk_Q_S* _Jd_Hd) * _C_BL)* _Hd_Jd;

                double __Gj_Q_S = (Math.Sin(_Xk_Q_S * _Jd_Hd) * _C_BL);
                if (__Gj_Q_S > 1) __Gj_Q_S = 1;
                __Gj_Q_S = Math.Asin(__Gj_Q_S) * _Hd_Jd;
                double __Gj_Q_x = Math.Asin(Math.Sin(_Xk_Q_X * _Jd_Hd) * _C_BL) * _Hd_Jd;
                Txt_Sbj.Text = __Gj_Q_S.ToString("f1");
                Txt_Xbj.Text = __Gj_Q_x.ToString("f2");
                #endregion
            }
            catch (Exception e2)
            {
                MessageBox.Show(e2.Message);
            }
        }

        private void Frm_Jz_ResizeEnd(object sender, EventArgs e)
        {
            m_fWaveFramePerHeight = Pic_A.Height / ((float)Tofd.UTS_DATA_HEIGHT);
            m_fWaveFramePerWidth = Pic_A.Width / ((float)Tofd.UTS_DATA_WIDTH);
            //    SysInfo.m_Plant.m_iKd_W_Num = Pic_A.Width / SysInfo.m_Plant.m_iJgNum;
        }

        private void Frm_Jz_FormClosed(object sender, FormClosedEventArgs e)
        {
            Thread_Get_TofdData.Abort();
            SysInfo.m_Tofd_DLL.m_i_State = 0;
            SysInfo.m_blFrm_TOFD_Open[1] = false;

            SysInfo.Init_Jz();
        }

        private void Pic_A_Click(object sender, EventArgs e)
        {
            float  _iTime = Tofd.m_pTimeBuf[SysInfo.m_Plant.m_i_X_No]  * 0.01f;
            if (Rd_1.Checked)
                if (_iTime >= 0)
                    Txt_T1.Text = _iTime.ToString();
            if (Rd_2.Checked)
                if (_iTime >= 0)
                    Txt_Tb.Text = _iTime.ToString();

            if (Rd_4.Checked)
                if (_iTime >= 0)
                {
                    Txt_T0.Text = _iTime.ToString();
                    SysInfo.m_Tofd_DLL.m_pSparam_Real[SysInfo.m_Tofd_DLL.m_icurChan].T0 = _iTime;
               
                }
            if (Rad_L0.Checked)
                if (_iTime >= 0)
                {
                    Txt_L0.Text = _iTime.ToString();
                    SysInfo.m_Tofd_DLL.m_pSparam_Real[SysInfo.m_Tofd_DLL.m_icurChan].L0 = _iTime;
                }
        }

        private void Pic_A_MouseMove(object sender, MouseEventArgs e)
        {
            float _fld = e.X / m_fWaveFramePerWidth;
            SysInfo.m_Plant.m_i_X_No = (int)_fld;

            SysInfo.m_Plant.m_i_X_A = e.X;
            SysInfo.m_Plant.m_i_Y_A = e.Y;
        }

        private void Rd_4_MouseHover(object sender, EventArgs e)
        {
          string _strT=  "探头对接后，此时屏幕上最左侧为始脉冲，而始脉冲之后第一个回波，" +
                "即是发射探头发出经由楔块到按收探头收到的声波，该波的传播时间即为声波在楔块中的延迟时间，" +
                "该波形的第一个脉冲的半周期波峰对齐,即为：楔块延时";
            _strT = SysInfo.csInter.IniReadDefine ("MsgTiTl", "2TO", _strT, SysInfo.HardFileName);
            ShowMsg(_strT);
        }
        private void ShowMsg( string strD,int iType=0)
        {
            if (iType == 0)
            {
                Pic_To.Visible = true;
                Txt_Msg.Visible = true;
                Txt_Msg.Text = strD;
            }
            else if(iType ==1)
            {
                Pic_L0.Visible = true;
                Txt_Msg.Visible = true;
                Txt_Msg.Text = strD;

            }
        }

        private void Rd_4_MouseLeave(object sender, EventArgs e)
        {
            Txt_Msg.Visible = false ;
            Pic_To.Visible = false;
        }

        private void Rad_L0_MouseMove(object sender, MouseEventArgs e)
        {
            string _strT = "探头前沿距离计算：探头前沿的测量方法与普通脉冲反射法不一样，由于采用的是大扩散角探头因此声束的指向性不强，最高反射回波不易获取，因此采用下面的方法来进行前沿的测定，将探头放置在平面的工件或试块上，相对放置，如图：";
            _strT = SysInfo.csInter.IniReadDefine("MsgTiTl", "L0", _strT, SysInfo.HardFileName);
            ShowMsg(_strT,1);
        }

        private void Rad_L0_MouseLeave(object sender, EventArgs e)
        {
            Txt_Msg.Visible = false;
            Pic_L0.Visible = false;
        }

        private void Bt_L0_Click(object sender, EventArgs e)
        {
            try
            {
                Txt_L0_Distan.Text = "";
                  int _iN0 = SysInfo.m_Tofd_DLL.m_icurChan;
                float _L0 = float.Parse(Txt_L0.Text);
                float _C = SysInfo.m_Tofd_DLL.m_pSparam[_iN0].m_dSpeed;
                float _t0 = float.Parse(Txt_T0.Text);
                if (_C <= 1000) { MessageBox.Show("声速值不能小于1000"); return; }
                if (_L0 <= _t0) { MessageBox.Show("前沿时间或者楔块延时不正确"); return; }

                float _L = (_L0 - _t0) / 2 * _C;
                Txt_L0_Distan.Text = _L.ToString("f0");
                SysInfo.m_Tofd_DLL.m_pSparam_Real[_iN0].L0_Distan  = _L;
            }
            catch { }
        }
    }
}
