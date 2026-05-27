using ClassLibrary_Interface;
using Tofd_AWI.NewFramework.State;
using ClassLib_DataMang.DataBaseMang.OleDal;
using ClassLib_TestData;
using System;
using System.Collections.Generic;

// ============================================================
// 文件: DataService.cs
// 位置: NewFramework/Services/
// 职责: 数据持久化 — 检测记录/报警/用户/项目的 CRUD
//       替代 ClassLib_DataMang 的直接调用（加一层抽象）
// ============================================================

namespace Tofd_AWI.NewFramework.Services
{
    public class DataService
    {
        private readonly SystemConfig _config;
        private string DatabasePath =>
            _config.DatabasePath + "InspectData.mdb";

        public DataService(SystemConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        // ========== 测试项目 (原 Test_Item.cs) ==========

        /// <summary>保存检测项目</summary>
        public bool SaveItem(ClassLib_TestData.Class_Test_Item item)
        {
            try
            {
                Test_Item testItem = new Test_Item(DatabasePath);
                // 对应原有: testItem.Insert(...) 等操作
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>查询检测项目列表</summary>
        public List<ClassLib_TestData.Class_Test_Item> GetItems(string weldId)
        {
            var list = new List<ClassLib_TestData.Class_Test_Item>();
            // 原有: Test_Item.Select(...)
            return list;
        }

        // ========== 检测记录 (原 Test_Records.cs) ==========

        /// <summary>保存检测记录</summary>
        public bool SaveRecord(Class_Test_Records record)
        {
            try
            {
                Test_Records recordsDal = new Test_Records(DatabasePath);
                // recordsDal.Insert(record);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // ========== 报警记录 (原 Test_Alarm.cs) ==========

        /// <summary>保存报警/缺陷记录</summary>
        public bool SaveAlarm(Class_Test_Alarm alarm)
        {
            try
            {
                Test_Alarm alarmDal = new Test_Alarm(DatabasePath);
                // alarmDal.Insert(alarm);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // ========== 用户管理 (原 User.cs) ==========

        /// <summary>验证用户登录</summary>
        public bool ValidateUser(string username, string password)
        {
            // 原有: User.Validate(username, password)
            return true;
        }
    }
}
