using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLib_TestData
{
   
    public class Class_Test_AlarmArea
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
        /// 部位编号（B1_B2 起止码，以_为间隔,起码在前，止码在后）
        /// </summary>
        public string Part_No = "";
       /// <summary>
        /// 异常缺陷类型  点 、线、面
        /// </summary>
        public string  strType = "";

        /// <summary>
        /// 长度开始距离
        /// </summary>
        public float flLen_S = 0f;
        /// <summary>
        /// 长度结束距离
        /// </summary>
        public float flLen_E = 0f;
        /// <summary>
        /// 长度
        /// </summary>
        public float flLen = 0;

        /// <summary>
        /// D图X轴位置序号
        /// </summary>
        public int iX_No = 0;
        /// <summary>
        /// D图Y轴位置序号
        /// </summary>
        public int iY_No = 0;
        /// <summary>
        /// 高度开始时间
        /// </summary>
        public float flHeight_S = 0f;
        /// <summary>
        /// 高度结束时间
        /// </summary>
        public float flHeight_E = 0f;
        /// <summary>
        /// 高度
        /// </summary>
        public float flHeight = 0;

        /// <summary>
        /// 深度开始时间
        /// </summary>
        public float flDepth_S = 0f;
        /// <summary>
        /// 深度
        /// </summary>
        public float flDepth = 0;

    }
}
