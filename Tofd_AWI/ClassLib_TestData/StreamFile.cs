using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;//FileStream

namespace ClassLib_TestData
{
    public  class StreamFile
    {
        /// <summary>
        /// 故障信息
        /// </summary>
        public string strErrMsg = "";
        /// <summary>
        /// 行程流文件
        /// </summary>
        public FileStream m_Stream_Trp;
        /// <summary>
        /// 检测项目记录
        /// </summary>
        public Test_Item m_TestItem = new Test_Item();
        /// <summary>
        /// 检测部件记录
        /// </summary>
        public Test_Parts m_TestParts = new Test_Parts();
        /// <summary>
        /// 伤点记录
        /// </summary>
        public Test_Records m_Record = new Test_Records();

        /// <summary>
        /// 流文件名
        /// </summary>
        public string FileName_Stream_Trp = "";


        #region 流文件操作
        public bool Write_File_One(string strFileName,int iType)
        {
            bool _blRet = false;
            try
            {
                File.Delete(strFileName);
                FileStream Stream_Tmp = new FileStream(strFileName, FileMode.CreateNew);

                switch (iType)
                {
                    case 0://检测项目记录
                        #region 

                        #endregion
                        break;
                    case 1://检测项目部件记录
                        #region 

                        #endregion
                        break;
                    case 2://检测伤点记录
                        #region 

                        #endregion
                        break;
                }
                Stream_Tmp.Close();
                Stream_Tmp.Dispose();

            }
            catch (Exception e)
            {
                strErrMsg = e.Message;
            }
            return _blRet;
        }
        public bool Read_File_One(string strFileName, int iType)
        {
            bool _blRet = false;
            try
            {
                FileStream Stream_Tmp = new FileStream(strFileName, FileMode.Open, FileAccess.ReadWrite);

                switch (iType)
                {
                    case 0://检测项目记录
                        #region 

                        #endregion
                        break;
                    case 1://检测项目部件记录
                        #region 

                        #endregion
                        break;
                    case 2://检测伤点记录
                        #region 

                        #endregion
                        break;
                }
                Stream_Tmp.Close();
                Stream_Tmp.Dispose();

            }
            catch (Exception e)
            {
                strErrMsg = e.Message;
            }

            return _blRet;
        }
        /// <summary>
        /// 打开流文件
        /// </summary>
        /// <returns></returns>
        public bool Open_Trp_File()
        {
            bool _blRet = false;
            try
            {
                #region 打开流文件
                try
                {
                    File.Delete(FileName_Stream_Trp);
                    m_Stream_Trp = new FileStream(FileName_Stream_Trp, FileMode.CreateNew);
                    _blRet = true;
                }
                catch (Exception e)
                {
                    strErrMsg = e.Message;
                }
                #endregion
            }
            catch { }
            return _blRet;
        }
        /// <summary>
        /// 关闭流文件
        /// </summary>
        /// <returns></returns>
        public bool Close_Trp_File()
        {
            bool _blRet = false;
            try
            {
                m_Stream_Trp.Close();
                m_Stream_Trp.Dispose();
            }
            catch (Exception e)
            {
                strErrMsg = e.Message;
            }
            return _blRet;
        }



        /// <summary>
        /// 写运行记录文件
        /// </summary>
        /// <param name="strFileName"></param>
        /// <param name="csInVideoData"></param>
        public int Save_Trp_Data(clStreamVideo csInVideoData)
        {
            int _iRet = -1;
            try
            {
       //         if (iFile_DoType != 1) return _iRet;

                byte[] ls1 = new byte[8];
                //1 视频帧序号 4
                ls1 = BitConverter.GetBytes(csInVideoData.iFrameNo); m_Stream_Trp.Write(ls1, 0, ls1.Length );
                //2 行程  8
                byte[] ls2 = new byte[8];
                ls2 = BitConverter.GetBytes(csInVideoData.Trip); m_Stream_Trp.Write(ls2, 0, ls2.Length );
                //3 车体速度 8
                byte[] ls3 = new byte[8];
                ls3 = BitConverter.GetBytes(csInVideoData.Speed); m_Stream_Trp.Write(ls3, 0, ls3.Length );
                _iRet = 0;
            }
            catch (Exception e)
            { strErrMsg = "Stream_Save：" + e.Message; }

            return _iRet;
        }

        /// <summary>
        /// 读流文件对应的运行记录
        /// </summary>
        /// <returns>数据保存到缓存列表</returns>
        public List<clStreamVideo> Read_Trp()
        {
            List<clStreamVideo> _Lst_RetData = new List<clStreamVideo>();
            int _iReadNum = 0;//读出数据个数

            try
            {
                //0 打开文件
                FileStream save = new FileStream(FileName_Stream_Trp, FileMode.Open, FileAccess.ReadWrite);

                byte[] lsbyte = new byte[4];
                while (true)
                {
                    clStreamVideo _Data = new clStreamVideo();
                    //1 视频帧序号 4
                    lsbyte = new byte[4];
                    _iReadNum = save.Read(lsbyte, 0, lsbyte.Length);
                    if (_iReadNum == 0)
                    {
                        break;
                    }

                    _Data.iFrameNo = BitConverter.ToInt32(lsbyte, 0);
                    //2 行程  8
                    lsbyte = new byte[8];
                    save.Read(lsbyte, 0, lsbyte.Length);
                    _Data.Trip = BitConverter.ToDouble(lsbyte, 0);
                    //3 车体速度  8
                    lsbyte = new byte[8];
                    save.Read(lsbyte, 0, lsbyte.Length);
                    _Data.Speed = BitConverter.ToDouble(lsbyte, 0);

                    //4 添加进列表
                    _Lst_RetData.Add(_Data);
                }
                //5 关闭文件
                save.Close();
            }
            catch (Exception e)
            { strErrMsg = "Stream_Read：" + e.Message; }

            return _Lst_RetData;
        }

        #endregion 流文件操作
    }

  
}
