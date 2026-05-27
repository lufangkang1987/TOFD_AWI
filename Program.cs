using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Tofd_AWI.Class;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using Frame_Work;
using Microsoft.VisualBasic;
        using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Tofd_AWI
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main1()
        {
            //1 获取欲启动进程名
            string strProcessName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            if (System.Diagnostics.Process.GetProcessesByName(strProcessName).Length > 1)
            {
                MessageBox.Show(strProcessName + "  Already running！", "news", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Application.Exit();
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            #region  有效期检查
        

            #endregion 
            //2 注册

            //if (CheckRegedit() == false)
            //    if (Regedit() == false)
            //    {
            //        MessageBox.Show("Registration failed！");
            //        //  m_blRegedit = false;
            //        return;
            //    }//注册失败 

          string _sT = SysInfo.csInter.IniReadDefine("System", "m_i_TOFD_0_Cscan_1", "0",
                                           Application.StartupPath + "\\database\\SysConfig.ini");
           int _C = int.Parse(_sT);
            SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 = _C;
            //3 界面加载
            //if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 0)
            //    Application.Run(new Frm_Main());
            //else if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1 ||
            //    SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2 ||
            //    SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
                try
                {
                    Application.Run(new Frm_Main_C());
                }
                catch (Exception ddd)
                { }
        }

        #region 注册

        ///<summary>
        /// 通过NetworkInterface读取网卡Mac
        ///</summary>
        ///<returns></returns>
        public static string GetMacByNetworkInterface(string strIN_Ip = "192.168.1.10")
        {
            string _strRet = "";

            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in nics)
            {
                if (adapter.NetworkInterfaceType.ToString().Equals("Ethernet"))
                {
                    IPInterfaceProperties ip = adapter.GetIPProperties();     //IP配置信息
                    for (int _i = 0; _i < ip.UnicastAddresses.Count(); _i++)
                    {
                        if (ip.UnicastAddresses[_i].Address.ToString() == strIN_Ip)
                        {
                            _strRet = adapter.GetPhysicalAddress().ToString();
                            break;
                        }
                    }
                }
            }
            if (_strRet == "")
            {
                MessageBox.Show(SysInfo.m_iLanguage == 0 ? "请设置有效网卡IP地址 : " : "Please set a valid network card IP address :" + strIN_Ip);
            }
            return _strRet;
        }
        /// <summary>
        /// 注册类
        /// </summary>
        static ClassRegister m_reg = new ClassRegister();
        private static bool CheckRegedit()
        {
            //        m_reg.IP = GetMacByNetworkInterface(SysInfo.csInter.IniReadDefine("IP", "IP", "192.168.1.240", SysInfo.HardFileName));
            return m_reg.Jg_Key();
        }
        private static bool Regedit()
        {
            bool blRegedit = false;
            m_reg.m_iPass_Num = 6;
            string strCalDat = "", strGetFacID = "";
            string strPass = SysInfo.csInter.IniReadDefine("RegEdidt", "strPass", "", SysInfo.HardFileName);

            if (strPass == "")
            {
                strPass = m_reg.Get_Pass();
                SysInfo.csInter.INIWriteValue("RegEdidt", "strPass", strPass, SysInfo.HardFileName);
            }
            string _1 = (SysInfo.m_iLanguage == 0 ? "请您现在将此序列号" : "Please send this serial number now") + ":   ";
            string _2 = (SysInfo.m_iLanguage == 0 ? "    发送给厂家，获得注册码" : "    Send it to the manufacturer for the registration code.");
            string _3 = (SysInfo.m_iLanguage == 0 ? "得到注册码了---继续;否---退出" : "Got the registration code -- continue; No - exit");
            string _4 = (SysInfo.m_iLanguage == 0 ? "警示" : "Warning");
            string _5 = (SysInfo.m_iLanguage == 0 ? "请输入注册码!" : "Please enter the registration code!");
            string _6 = (SysInfo.m_iLanguage == 0 ? "输入厂家注册码" : "Enter the manufacturer registration code");
            if (MessageBox.Show(_1 + strPass + _2 + "\r\n" + _3, _4, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                strCalDat = m_reg.Calcu_Pass(strPass);
                strGetFacID = Interaction.InputBox(_5, _6, "", -1, -1);
                string strMsg = "";

                //if (m_reg.m_Rsg == null)
                //    m_reg.Create();
                //strMsg = m_reg.Get_Key();

                // MessageBox.Show(strGetFacID + "/" + strCalDat + "/" + strMsg);
                if (strGetFacID == strCalDat)
                {
                    if (m_reg.m_Rsg == null)
                        m_reg.Create();
                    strMsg = m_reg.Get_Key();
                    if (m_reg.Set_Key(strMsg))//VX2000HR00622VD20F\u0017BXB\u0017H   VX2000PR048\u0017HT2R202RDD681455D 
                    {
                        MessageBox.Show("Registration success！");
                        SysInfo.csInter.INIWriteValue("RegEdidt", "strPass", "", SysInfo.HardFileName);
                        blRegedit = true;
                    }
                    blRegedit = true;
                }
            }

            return blRegedit;
        }



        #endregion  注册
    }


    public class ClockCryptoLock
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;
        private readonly TimeSpan _validityDuration;

        public ClockCryptoLock(string key, string iv, TimeSpan validityDuration)
        {
            _key = Encoding.UTF8.GetBytes(key);
            _iv = Encoding.UTF8.GetBytes(iv);
            _validityDuration = validityDuration;
        }

        public string Encrypt(string data)
        {
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = _key;
                aesAlg.IV = _iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(data);
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        }

        public string Decrypt(string encryptedData)
        {
            if (!IsLockedWithinValidityPeriod())
            {
                throw new InvalidOperationException("The lock is no longer valid.");
            }

            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = _key;
                aesAlg.IV = _iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                byte[] bytes = Convert.FromBase64String(encryptedData);

                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        private bool IsLockedWithinValidityPeriod()
        {
            DateTime now = DateTime.UtcNow;
            DateTime lockTime = new DateTime(long.Parse(Decrypt("YourEncryptedTime")));
            return now <= lockTime + _validityDuration;
        }
    }
}