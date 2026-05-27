/*
 * Copyright(C) 2 2021 河南德朗智能科技有限公司
 * 文件名: Class_User.cs
 * 文件功能描述: 用户名称定义
 * 目的：用户名称定义
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  ClassLib_TestData
{
    /// <summary>
    /// 用户名类
    /// </summary>
    public class Class_User
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name = "";
        /// <summary>
        /// 密码
        /// </summary>
        public string Pass = "";
        /// <summary>
        /// 等级
        ///  0:超级用户 1：检定员 2：参观者
        /// </summary>
        public string strLevel = "";
    }
}
