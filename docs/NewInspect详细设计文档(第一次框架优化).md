# NewInspect 详细设计文档

> **版本**: v1.0 | **日期**: 2026-05-29 | **作者**: 陈大伟
> 
> **项目**: 工业无损检测（NDT）TOFD / C-Scan 检测系统
> **技术栈**: C# WinForms + .NET Framework 4.8 + Access/SQL Server

---

## 目录

1. [系统概述](#1-系统概述)
2. [解决方案架构](#2-解决方案架构)
3. [分层架构设计](#3-分层架构设计)
4. [模块详细设计](#4-模块详细设计)
   - [4.1 接口层 ClassLibrary_Interface](#41-接口层-classlibrary_interface)
   - [4.2 数据模型层 ClassLib_TestData](#42-数据模型层-classlib_testdata)
   - [4.3 数据访问层 ClassLib_DataMang](#43-数据访问层-classlib_datamang)
   - [4.4 框架层 Frame_Work](#44-框架层-frame_work)
   - [4.5 服务层 NewInspect.Services](#45-服务层-newinspectservices)
   - [4.6 主程序 Tofd_AWI](#46-主程序-tofd_awi)
   - [4.7 硬件驱动层](#47-硬件驱动层)
5. [数据库设计](#5-数据库设计)
6. [核心业务流程](#6-核心业务流程)
7. [设计模式与架构决策](#7-设计模式与架构决策)
8. [配置管理](#8-配置管理)
9. [部署结构](#9-部署结构)

---

## 1. 系统概述

### 1.1 系统定位

NewInspect 是一套面向工业无损检测（NDT）的桌面应用系统，采用 TOFD（Time of Flight Diffraction，超声衍射时差法）和 C-Scan（C 扫描成像）两种技术，对管道焊缝等工业结构进行自动扫查、缺陷检测、数据记录和报告生成。

### 1.2 核心能力

| 能力 | 说明 |
|------|------|
| **TOFD 检测** | A/B/D 扫描波形实时采集与显示，增益/范围/闸门参数调节 |
| **C-Scan 成像** | 多通道 C 扫描图像采集与叠加 |
| **运动控制** | CAN/COM 双模式 4 轮车体控制：前进/后退/纠偏/打标 |
| **多路视频** | 最多 4 路工业相机 + USB 摄像头 + 爬行器视频同时显示 |
| **缺陷标注** | 检测过程中实时标注缺陷，记录位置/深度/长度 |
| **数据持久化** | 检测项目、焊缝、原始波形、缺陷标注全部入库 |
| **报表生成** | 自动生成检测报告，含波形截图和统计信息 |
| **用户管理** | 登录验证、权限分级 |

### 1.3 硬件环境

```
┌─────────────────────────────────────────────────┐
│                  工控机 (Windows)                  │
│                                                   │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐       │
│  │ TOFD 超声 │  │ CAN 总线 │  │ 工业相机  │       │
│  │ 采集模块  │  │ 控制卡   │  │ (×4路)   │       │
│  └────┬─────┘  └────┬─────┘  └────┬─────┘       │
│       │ USB/网口      │ PCI/CAN     │ GigE/RTSP   │
│  ┌────┴──────────────┴─────────────┴─────┐      │
│  │         NewInspect 软件系统              │      │
│  └────────────────────────────────────────┘      │
│                                                   │
│  外接设备: 爬行器(4轮车体)、激光轮廓仪、编码器      │
│  检测对象: 管道焊缝、压力容器焊缝等                 │
└─────────────────────────────────────────────────┘
```

---

## 2. 解决方案架构

### 2.1 项目一览

NewInspect.sln 包含 **10 个项目**，按功能分组：

```
NewInspect.sln
├── Tofd_AWI                  ← 主程序 (WinForms EXE)
│
├── NewInspect.Services       ← 新架构服务层
├── ClassLibrary_Interface     ← 接口 + 配置 + 消息总线
├── Frame_Work                ← 框架数据结构
├── ClassLib_TestData          ← 数据库 POCO 模型
│
├── [Driver 虚拟文件夹]
│   ├── Clb_MT_Comm           ← CAN/电机运动控制
│   ├── Clb_XmCam_DLL         ← 玄目工业相机
│   ├── MVSDK                 ← 迈德威视工业相机
│   ├── Video                 ← 通用摄像头
│   └── ClimbVideo            ← 爬行器视频采集
│
├── [外部 DLL 引用 (不在 .sln 中)]
│   ├── ClassLib_DataMang     ← 数据库访问层
│   ├── Tofd_DLL              ← TOFD 超声波驱动
│   ├── HD850_64              ← 激光轮廓仪
│   ├── Cmm_PcPower           ← 电源监控
│   ├── ReportDLL             ← 报表生成
│   └── clsExcel              ← Excel 导出
```

### 2.2 依赖关系图

```
                              Tofd_AWI (主程序)
                             ╱    │    │    │    ╲
                            ╱     │    │    │     ╲
              NewInspect.Services  │    │    │   ClassLib_DataMang (DAO)
                   │  │           │    │    │        │
                   │  └─────┬─────┘    │    │        │
                   ▼        ▼          │    │        ▼
           ClassLibrary    Frame      │  ClassLib   Tofd_DLL
           _Interface      _Work      │  _TestData  (硬件驱动)
               │             │        │     │
               │             └───┬────┘     │
               │                 ▼          │
               │           Clb_MT_Comm      │
               │                           │
               └───────────────────────────┘
                     (所有驱动层引用此接口)
```

### 2.3 依赖层级（从上到下）

| 层级 | 项目 | 内部依赖数 | 角色 |
|------|------|----------|------|
| 0（基础） | ClassLibrary_Interface | 0 | 接口定义 + 配置 + 消息 |
| 0（基础） | ClassLib_TestData | 0 | 数据 POCO 模型 |
| 0（基础） | Video / MVSDK / Clb_XmCam_DLL | 0 | 独立相机驱动 |
| 1 | Frame_Work | 依赖 TestData | 运行时数据结构 |
| 1 | Clb_MT_Comm | 依赖 TestData | 运动控制驱动 |
| 2 | ClimbVideo | 依赖 MVSDK + XmCam + Video | 多路视频聚合 |
| 2 | ClassLib_DataMang | 依赖 TestData + MT_Comm | 数据库访问层 |
| 3 | NewInspect.Services | 依赖 Interface + Frame + TestData + DataMang + 驱动 | 业务服务层 |
| 4 | Tofd_AWI | 依赖所有 | 主程序 |

---

## 3. 分层架构设计

### 3.1 新架构分层（目标）

```
┌──────────────────────────────────────────────────────────────┐
│                     UI 层 (From/)                             │
│  Frm_Main_C_New · Frm_TOFD · Frm_Move · Frm_Item · Frm_Bd   │
│  通过构造函数注入 Service 依赖，不直接访问硬件/数据库           │
├──────────────────────────────────────────────────────────────┤
│                  服务层 (NewInspect.Services)                  │
│  TofdService · MotionService · CameraService · DataService   │
│  纯粹业务逻辑，依赖接口而非具体实现                              │
├──────────────────────────────────────────────────────────────┤
│              适配器层 (NewInspect.Services/Adapters)           │
│  CanMotionControllerAdapter · TofdHardwareAdapter             │
│  薄封装 — 实现接口，桥接旧硬件 DLL ← 换硬件只改这层            │
├──────────────────────────────────────────────────────────────┤
│              接口层 (ClassLibrary_Interface)                   │
│  ITofdHardware · IMotionController · ICameraDevice · ...     │
│  7 个纯接口 + SystemConfig + MsgInterFace                     │
├──────────────────────────────────────────────────────────────┤
│              框架层 (Frame_Work)                              │
│  ClassTofd_Buff · Class_C_Buff · CLS_UltrasoundGate · ...   │
│  全系统共享的运行时数据结构定义                                  │
├──────────────────────────────────────────────────────────────┤
│           数据层 (ClassLib_DataMang)                          │
│  OleDbHelper(SQL Server) · 6 个 DAO (OleDal/)                │
│  Access / SQL Server 双模式数据库访问                           │
├──────────────────────────────────────────────────────────────┤
│           硬件驱动层 (8 个独立项目/DLL)                          │
│  Tofd_DLL · Clb_MT_Comm · MVSDK · Clb_XmCam_DLL · ...       │
│  各硬件厂商 SDK 的 .NET 封装                                   │
└──────────────────────────────────────────────────────────────┘
```

### 3.2 新旧架构对照

| 旧架构 | 新架构 | 改进点 |
|--------|--------|--------|
| `SysInfo.cs` (290KB, 全 static) | `NewInspect.Services/State/` (8 个状态类) | 单一职责、可测试 |
| 窗体直接 `new` 硬件对象 | 构造函数注入接口 | 可替换、可 Mock |
| 事件处理在 Form 中混写 | Service 中独立方法 | 逻辑与 UI 分离 |
| 全局 `using` 到处用 | 按需引用接口 | 编译期约束 |
| INI 散落各处读取 | `SystemConfig` 一次性加载 | 配置集中管理 |

---

## 4. 模块详细设计

### 4.1 接口层 ClassLibrary_Interface

**职责**: 定义系统宪法 — 硬件接口契约、统一配置、跨模块消息通讯

**文件结构**:
```
ClassLibrary_Interface/
├── IHardwareInterfaces.cs    ← 7 个硬件接口定义
├── SystemConfig.cs           ← INI 配置加载 (只读)
├── ClassInterFace.cs         ← INI 文件读写工具
├── ClsMsgInterFace.cs        ← 单例消息总线 (10+ 事件频道)
└── Models/
    └── InfrastructureModels.cs ← 基础设施模型
```

#### 4.1.1 硬件接口定义

| 接口 | 抽象设备 | 核心方法 |
|------|---------|---------|
| `ITofdHardware` | TOFD 超声模块 | `Connect()` `SetGain()` `AcquireAScan()` `AcquireDScan()` |
| `ICScanHardware` | C-Scan 采集 | `Connect()` `AcquireFrame()` `SetParameters()` |
| `IMotionController` | 运动电机 | `Connect()` `MoveForward()` `MoveBackward()` `Stop()` `MarkDefect()` |
| `ICameraDevice` | 工业相机 | `Open(ip)` `Capture()` `StartStream()` `StopStream()` |
| `ILaserProfiler` | 激光轮廓仪 | `Connect()` `GetProfile()` `GetTemperature()` |
| `IPowerMonitor` | 电源监控 | `GetBatteryPercent()` `IsCharging` |
| `IEddyCurrentDevice` | 涡流检测 | `Connect()` `ReadSignal()` `StartScan()` |

#### 4.1.2 消息总线 MsgInterFace

单例模式的消息中心，用于跨模块解耦通讯：

```
MsgInterFace (单例)
├── Event_ReceivedData        ← 收到报文数据
├── Event_RecivceState        ← 收到状态数据
├── Event_RecivceAlarm        ← 收到报警数据
├── Event_UpdateScreen        ← 更新画面
├── Event_PositionUpdate      ← 位置更新
├── Event_MarkCommand         ← 打标命令
├── ...                       ← 其他业务事件
```

#### 4.1.3 配置类 SystemConfig

从 `SysConfig.ini` / `HardConfig.ini` 一次性加载，不可变：

```csharp
public class SystemConfig
{
    int Language;           // 语言: 0=中文 1=English
    int WorkMode;           // 0=TOFD 1=CScan 2=M_UI 3=Coating
    int UltrasoundType;     // 超声探头厂家
    float SoundVelocity;    // TOFD 声速 m/s
    float ProbeCenterDistance;  // 探头中心距
    int CommMode;           // 通讯方式: 0=CAN 1=COM
    string CommPort;        // 串口号
    int SpeedPercent;       // 当前速度
    // ...
}
```

---

### 4.2 数据模型层 ClassLib_TestData

**职责**: 数据库表的 C# 映射模型（POCO），是 ClassLib_DataMang DAO 层的契约

**文件清单**:

| 文件 | 类 | 对应表 |
|------|----|-------|
| `Class_Test_Item.cs` | `Class_Test_Item` | `Test_Item_Info` (主库) |
| `Class_Test_Parts.cs` | `Class_Test_Parts` | `Part_{ID}` (子库) |
| `Class_Test_Records.cs` | `Class_Test_Records` | `Record_{ID}` (子库) |
| `Test_AlarmArea.cs` | `Class_Test_AlarmArea` | `Alarm_{ID}` (子库) |
| `Statistical_Report.cs` | `Class_Test_Statistical_Report` | `Test_Statistical_Report` |
| `Class_User.cs` | `Class_User` | `CngUser` (主库) |
| `StreamFile.cs` | `StreamFile` | 二进制流文件读写 (FileStream) |
| `SreamVideo.cs` | `clStreamVideo` | 视频流行程数据缓冲 |

**注意**: `StreamFile.cs` 不是纯 POCO — 它包含实际的 FileStream 读写逻辑，是数据库之外的另一条持久化路径。

---

### 4.3 数据访问层 ClassLib_DataMang

**职责**: 封装数据库 CRUD 操作，支持 Access 与 SQL Server 双引擎切换

**文件结构**:
```
ClassLib_DataMang/
└── DataBaseMang/
    ├── DBUtility/
    │   ├── OleDbHelper.cs      ← Access Jet OLEDB 引擎
    │   ├── SqlDbHelper.cs      ← SQL Server 引擎
    │   └── DBConvert.cs        ← 类型转换
    └── OleDal/
        ├── DbGlobal.cs          ← 全局 DAO 单例入口 ⭐
        ├── CreatDataBase.cs     ← 建库建表逻辑
        ├── Test_Item.cs         ← 工程信息 CRUD
        ├── Test_Parts.cs        ← 焊缝信息 CRUD
        ├── Test_Records.cs      ← 检测记录 CRUD
        ├── Test_Alarm.cs        ← 异常标注 CRUD
        ├── User.cs              ← 用户 CRUD
        ├── Test_Statistical_Report.cs ← 统计报告 CRUD
        └── Manage_Wave.cs       ← 波形管理
```

#### 4.3.1 DbGlobal 单例入口

```csharp
// 全局唯一的数据访问入口
DbGlobal.ImTest_Item        // → Test_Item DAO
DbGlobal.ImTest_Parts       // → Test_Parts DAO
DbGlobal.ImTest_Records     // → Test_Records DAO
DbGlobal.ImAlarm            // → Test_Alarm DAO
DbGlobal.ImUser             // → User DAO
DbGlobal.ImStatistical      // → Statistical_Report DAO
DbGlobal.ImCreatDataBase    // → CreatDataBase
```

#### 4.3.2 数据库切换

通过 `SysConfig.ini` 的 `[SqlDbHelper] > m_iDataBase_Type` 配置：
- `0` → Access `.mdb` (Microsoft.Jet.OLEDB.4.0)
- `1` → SQL Server (SqlConnection)

---

### 4.4 框架层 Frame_Work

**职责**: 定义全系统共享的运行时数据结构（历史遗留，计划随 SysInfo 退役后合并）

**核心文件**: `Struct.cs` (~10000 行，50+ 个类/结构体)

| 核心类 | 用途 |
|--------|------|
| `ClassTofd_Buff` | TOFD 超声全部运行时数据 (参数/波形/C-Scan) |
| `Class_C_Buff` | C-Scan 数据缓存 |
| `ClassSys_Buff` | 系统缓存 (以距离为键的 Dictionary) |
| `Class_C_ItemInfor` | 检测项目信息 (30+ 字段) |
| `CLS_UltrasoundGate` | 超声闸门参数 |
| `Cls_Report_P` | 报表参数 |
| `Tofd_Arr` | TOFD 阵列数据 |
| `ClAlarm` | 报警数据 |

**状态**: 历史债务。与 ClassLib_TestData 有大量重叠，等 SysInfo.cs 退役后可逐步迁移。

---

### 4.5 服务层 NewInspect.Services

**职责**: 纯粹业务逻辑层，面向接口编程，不直接依赖硬件 DLL

**文件结构**:
```
NewInspect.Services/
├── State/
│   └── RuntimeState.cs       ← 8 个状态类: TofdState, CScanState, MotionState, ...
├── Services/
│   ├── TofdService.cs        ← TOFD/C-Scan 检测流程
│   ├── MotionService.cs      ← 运动控制流程
│   ├── CameraService.cs      ← 4 路相机管理
│   ├── DataService.cs        ← 数据持久化封装
│   ├── VideoDisplayService.cs    ← 视频 PictureBox 绑定
│   └── WaveformDisplayService.cs ← A/B/C/D 波形渲染
├── Adapters/
│   └── HardwareAdapters.cs   ← 硬件适配器 (实现 IHardwareInterfaces)
└── Utilities/
    ├── SysUtility.cs          ← 声速/距离等物理量换算
    └── GridHelper.cs          ← DataGridView 辅助方法
```

#### 4.5.1 TofdService — TOFD 检测服务

```csharp
public class TofdService
{
    // 依赖注入 — 只依赖接口，不依赖具体硬件！
    TofdService(
        ITofdHardware hardware,       // ← 接口
        ICScanHardware cScanHardware,  // ← 接口
        TofdState tofdState,
        CScanState cScanState,
        SystemConfig config);
    
    // 核心方法
    bool Connect();                   // 连接 TOFD 设备
    void Disconnect();
    void SetGain(int db);             // 设置增益
    void SetSoundVelocity(float v);   // 设置声速
    void StartAScan();                // 启动 A 扫描采集
    void StopAScan();
    void StartDScan();                // 启动 D 扫描采集
}
```

#### 4.5.2 MotionService — 运动控制服务

```csharp
public class MotionService
{
    MotionService(IMotionController controller);  // ← 接口
    
    void GoForward(int speed);        // 前进
    void GoBackward(int speed);       // 后退
    void Stop();
    void MarkDefect();                // 缺陷打标
    void SetGratingArm(bool up);      // 光栅臂升降
    float GetDistance();              // 获取当前行程
    float GetSpeed();                 // 获取当前速度
}
```

#### 4.5.3 状态管理 — RuntimeState

将原来 SysInfo.cs 中的 160+ 个 static 字段拆分为 8 个职责单一的状态类：

| 状态类 | 管理内容 |
|--------|---------|
| `TofdState` | TOFD 连接状态/增益/曝光/相机模式/打标滞后 |
| `CScanState` | C-Scan 采集状态/帧率/通道参数 |
| `MotionState` | 车体运动状态/编码器读数/光栅臂位置 |
| `CameraState` | 4 路相机 IP/连接状态/录像状态 |
| `DataState` | 当前打开的项目/焊缝/记录 ID |
| `AlarmState` | 报警条件/缺陷标注列表 |
| `PrintState` | 报表参数/打印选项 |
| `UserState` | 当前用户/权限等级 |

#### 4.5.4 适配器模式

适配器是实现"换硬件不动服务"的关键：

```csharp
// 接口 — 永远不变
public interface IMotionController
{
    bool Connect(string portOrCan, bool isComNotCan);
    void MoveForward();
    void Stop();
    float GetDistance();
}

// 适配器 — 封装具体硬件 DLL
public class CanMotionControllerAdapter : IMotionController
{
    private Clb_MT_Comm.MT_Comm _mtComm;  // ← 具体硬件
    
    public bool Connect(string port, bool isCom)
    {
        _mtComm.m_blCom1_Can0 = isCom ? 1 : 0;
        _mtComm.InitCom(port);          // 封装旧 API
        return true;
    }
    
    public void MoveForward() => _mtComm.SendData(1, 1, 6, 0, "1");
    public void Stop()         => _mtComm.SendData(1, 1, 6, 0, "3");
}

// 服务层 — 换硬件这行代码一个字不改
public class MotionService
{
    private readonly IMotionController _ctrl;  // ← 依赖接口！
    
    public void GoForward(int speed) => _ctrl.MoveForward();
}
```

---

### 4.6 主程序 Tofd_AWI

**职责**: 程序入口、UI 窗体、组装所有依赖

#### 4.6.1 程序入口

```
Program.cs (旧) — 创建 SysInfo, 启动 Frm_Main
Program_New.cs (新) — 创建 Service, 注入 Frm_Main_C_New
```

#### 4.6.2 窗体清单

| 窗体 | 职责 | 大小 |
|------|------|------|
| `Frm_Main_C` | 旧主界面 (TOFD + C-Scan) | 8712 行 |
| `Frm_Main_C_New` | 新主界面 (DI 架构) | ~600 行 |
| `Frm_TOFD` | TOFD 工艺参数设置 | 1953 行 |
| `Frm_Move` | 车体运动控制 | 1053 行 |
| `Frm_Tofd_Calcu` | 探头校准计算器 | 599 行 |
| `Frm_Mul_Thick` | 多点测厚 | — |
| `Frm_Print` | 报表打印 | — |
| `Frm_Main_M_UI` | 磁粉检测界面 | — |

#### 4.6.3 新架构 DI 组装示例

```csharp
// Program_New.cs
var config = new SystemConfig();
var tofdAdapter = new TofdHardwareAdapter();       // 适配器
var motionAdapter = new CanMotionControllerAdapter();

var tofdService = new TofdService(tofdAdapter, ...);    // 服务
var motionService = new MotionService(motionAdapter);

var mainForm = new Frm_Main_C_New(                      // UI
    tofdService, motionService, cameraService, dataService);
```

---

### 4.7 硬件驱动层

#### 4.7.1 相机驱动（三套方案）

| 项目 | SDK | 接口 | 用途 |
|------|-----|------|------|
| `Clb_XmCam_DLL` | V_DLL.dll | RTSP | 玄目 4 路工业相机 |
| `MVSDK` | MvCameraControl.Net.dll | GigE/USB3 | 迈德威视工业相机 |
| `Video` | Emgu.CV (OpenCV) | USB/RTSP | 通用 USB/网络摄像头 |

#### 4.7.2 运动控制

| 文件 | SDK | 协议 | 功能 |
|------|-----|------|------|
| `CanCmd.cs` | CanCmd.dll (南京来可) | CAN 2.0B | CAN 总线收发 |
| `MT_Comm.cs` | 自研协议 | CAN/RS232 | 电机控制指令封装 |

CAN 指令速查：
```
前进:  SendData(1,1,6,0,"1")  →  帧 43 52 01 00 00 00 00 00
后退:  SendData(1,1,6,0,"2")  →  帧 43 52 02 ...
停止:  SendData(1,1,6,0,"3")  →  帧 43 52 03 ...
心跳:  SendData(1,3,0,0,"99")  →  每 1.5s 索取 MCU 状态
```

#### 4.7.3 其他硬件驱动

| 项目/DLL | 硬件 | 通讯方式 |
|-----------|------|---------|
| `Tofd_DLL` | TOFD 超声采集模块 | USB/网口 |
| `HD850_64` | 激光轮廓仪 (寻迹) | USB |
| `Cmm_PcPower` | 电源/电池监控 | — |
| `ECT_DLL` | 涡流检测模块 | — |

---

## 5. 数据库设计

### 5.1 物理架构

```
{程序运行目录}/
├── database/
│   ├── DellData.mdb          ← 主库 (所有项目共享)
│   └── SysConfig.ini         ← 数据库连接配置
├── SubData/
│   ├── {ID}_{项目名}.mdb     ← 子库 (每个检测项目一个独立文件)
│   └── Tmp/                  ← 临时库
└── HardConfig.ini            ← 硬件配置
```

### 5.2 主库结构 (DellData.mdb)

#### 表 `CngUser` — 用户

| 字段 | 类型 | 说明 |
|------|------|------|
| Name | varchar(100) | 用户名 |
| Pass | varchar(20) | 密码 |
| strLevel | varchar(5) | 权限等级 |

默认用户: `Dellon / 123 / 0`

#### 表 `Test_Item_Info` — 检测项目

| 字段 | 类型 | 说明 |
|------|------|------|
| ID | varchar(20) | yyMMddHHmmss 格式 |
| strDwmc | varchar(50) | 委托单位 |
| strItemName | varchar(250) | 工程/项目名称 |
| strSbbh | varchar(50) | 受检设备编号 |
| strMaterial | varchar(30) | 材质 |
| strWeldingtype | varchar(30) | 焊接类型 |
| strTest_Proportion | varchar(10) | 检测比例 |
| strTesting_Standard | varchar(25) | 检测标准 |
| ... | ... | 共 18+ 字段 |

### 5.3 子库结构 ({项目}.mdb)

每个检测项目对应一个独立 `.mdb` 文件，包含：

| 表名 | DAO | 内容 |
|------|-----|------|
| `Part_{ID}` | `Test_Parts` | 焊缝编号/壁厚/PCS/声速/T0 |
| `Record_{ID}` | `Test_Records` | 每个采样点的距离/波形数据/缺陷深度 |
| `Alarm_{ID}` | `Test_Alarm` | 缺陷标注: 起止位置/深度/高度/类型 |
| `Test_Statistical_Report` | `Statistical` | 缺陷统计: 位置/长度 |

### 5.4 数据库操作时机

```
程序启动
    │
    ├── Frm_Main.InitDataBase()
    │   ├── 检查 DellData.mdb 是否存在
    │   ├── 不存在 → 建库 + 建 CngUser 表 + 插默认用户
    │   └── 建 Test_Item_Info 表
    │
    ├── 用户登录
    │   └── DbGlobal.ImUser.GetData() → 查询 CngUser
    │
    ├── 新建项目
    │   └── DbGlobal.ImTest_Item.SaveData() → INSERT Test_Item_Info
    │
    ├── 开始检测
    │   ├── 创建 SubData/{ID}.mdb
    │   ├── 建 Record_{ID} 表
    │   └── 建 Alarm_{ID} 表
    │
    ├── 检测过程中 (每个采样周期)
    │   └── DbGlobal.ImTest_Records.SaveData() → INSERT 波形+距离
    │
    ├── 标注缺陷
    │   └── DbGlobal.ImAlarm.SaveData() → INSERT Alarm
    │
    ├── 历史回看
    │   └── DbGlobal.ImTest_Records.GetData() → SELECT 波形
    │
    └── 报表打印
        └── DbGlobal.ImAlarm.GetData() + ImStatistical.GetData()
```

---

## 6. 核心业务流程

### 6.1 TOFD 检测主流程

```
┌──────────┐    ┌──────────┐    ┌──────────┐    ┌──────────┐
│  Frm_Item │    │ Frm_Weld │    │ Frm_Main_C│    │ Frm_TOFD  │
│  工程信息  │    │  焊缝信息 │    │  主检测   │    │  工艺参数  │
└─────┬────┘    └─────┬────┘    └─────┬────┘    └─────┬────┘
      │               │               │               │
      ├─ 输入项目信息   │               │               │
      ├─ SaveData()    │               │               │
      │               │               │               │
      │               ├─ 输入焊缝参数   │               │
      │               ├─ SaveData()    │               │
      │               │               │               │
      │               │               ├─ 连接 TOFD     │
      │               │               ├─ 设置工艺参数   ├── 增益/范围/闸门
      │               │               ├─ 连接车体      │
      │               │               ├─ 连接相机      │
      │               │               │               │
      │               │               ├─ 开始扫查 ────┤
      │               │               │   │           │
      │               │               │   ├── 采集线程 │
      │               │               │   │  A扫: 50Hz │
      │               │               │   │  D扫: 按编码│
      │               │               │   │           │
      │               │               │   ├── 实时显示 │
      │               │               │   │  Pic_AScan │
      │               │               │   │  Pic_BScan │
      │               │               │   │  Pic_DScan │
      │               │               │   │           │
      │               │               │   ├── 实时存储 │
      │               │               │   │  SaveData()│
      │               │               │   │           │
      │               │               │   └── 缺陷判断 │
      │               │               │       → 报警   │
      │               │               │       → 打标   │
      │               │               │               │
      │               │               ├─ 停止扫查      │
      │               │               ├─ 生成报告      │
      │               │               └─ 打印          │
```

### 6.2 数据采集线程模型

```
主线程 (UI)                     工作线程
─────────                       ────────
Frm_Main_C_New                  Thread_A_Wave (A扫描采集)
    │                               │
    ├─ Bt_Start_Click()             │
    │   ├─ TofdService.StartAScan() │
    │   │   └─ _aScanThread.Start()─┤
    │   │                           ├─ while(_isRunning)
    │   │                           │   ├─ 硬件采集
    │   │                           │   │   _hardware.AcquireAScan()
    │   │                           │   │
    │   │                           │   ├─ UI 更新 (Invoke)
    │   │                           │   │   Pic_AScan.Invalidate()
    │   │                           │   │
    │   │                           │   ├─ 数据库写入
    │   │                           │   │   DbGlobal.SaveData()
    │   │                           │   │
    │   │                           │   └─ Thread.Sleep(20ms)
    │   │                           │
    │   │                           └─ end while
    │   │
    │   └─ TofdService.StartDScan() (同上，按编码器触发)
```

---

## 7. 设计模式与架构决策

### 7.1 已应用的设计模式

| 模式 | 应用位置 | 说明 |
|------|---------|------|
| **接口隔离** | `IHardwareInterfaces` | 7 个独立接口，客户端不依赖不需要的方法 |
| **适配器模式** | `Adapters/HardwareAdapters.cs` | 旧硬件 DLL → 新接口的薄封装 |
| **依赖注入** | 所有 Service 构造函数 | 通过接口注入，不 new 具体类 |
| **单例模式** | `MsgInterFace.GetInstance()` | 全局消息总线 |
| **单例入口** | `DbGlobal` | 所有 DAO 的统一入口 |
| **状态对象** | `State/RuntimeState.cs` | 替代 SysInfo 的 static 大杂烩 |
| **工厂方法** | `CreatDataBase.cs` | 根据配置创建 Access 或 SQL Server 数据库 |

### 7.2 架构决策记录

| 决策 | 原因 |
|------|------|
| **接口层不引用任何项目** | 保持最底层独立性，任何上层项目都可以依赖它 |
| **状态类与 Service 分离** | Service 持有业务逻辑，State 持有数据，单一职责 |
| **适配器是"薄壳"** | 不修改原有硬件 DLL 逻辑，只做接口适配，降低风险 |
| **数据库默认 Access** | 工控现场不一定有 SQL Server 环境，Access 免安装 |
| **子数据库独立文件** | 每个检测项目数据隔离，便于归档和迁移 |
| **PictureBox 归设计器管理** | UI 控件生命周期由 WinForms Designer 管理，Service 只做渲染 |

### 7.3 技术债务

| 问题 | 严重程度 | 建议 |
|------|---------|------|
| `SysInfo.cs` (290KB, 160+ static) | ⭐⭐⭐ | 用 NewInspect.Services.State 逐步替代 |
| `Frame_Work/Struct.cs` (10000 行) | ⭐⭐⭐ | 按领域拆到各使用方，等 SysInfo 退役后删除 |
| SQL 字符串拼接 (无参数化) | ⭐⭐ | 存在注入风险，建议改用参数化查询 |
| `Tofd_DLL` / `ClassLib_DataMang` 不在 .sln | ⭐⭐ | 作为预编译 DLL 引用，修改不便 |
| ClassLib_TestData 存在双副本 | ⭐⭐ | `Tofd_AWI/ClassLib_TestData/` 是遗留副本，应清理 |
| 旧窗体未迁移到 DI | ⭐⭐ | Frm_Move / Frm_TOFD 仍直接依赖 SysInfo |

---

## 8. 配置管理

### 8.1 配置文件

| 文件 | 位置 | 内容 |
|------|------|------|
| `SysConfig.ini` | `bin/Debug/DataBase/` | 系统参数 (486+ 行): 语言/模式/网络/运动/检测条件 |
| `HardConfig.ini` | `bin/Debug/DataBase/` | 硬件参数 (48 行): 探头厂家/采集时间/相机配置 |
| `Translate/*.ini` | `bin/Debug/DataBase/Translate/` | 中英文界面翻译 |

### 8.2 关键配置项

```ini
[SysConfig]
m_iLanguage=0            ; 0=中文 1=English
m_iWorkMode=0            ; 0=TOFD 1=CScan
m_iUltrasound_Type=0     ; 超声探头厂家
m_flSoundVelocity=5900   ; 声速 m/s
m_iCommMode=1            ; 通讯: 0=CAN 1=COM
m_strCommPort=COM3       ; 串口号
m_iSpeed=50              ; 速度百分比

[HardConfig]
m_iCScanRangeUs=10       ; C-Scan 采集时间
m_flDefaultGain=5        ; 默认增益

[SqlDbHelper]
m_iDataBase_Type=0       ; 数据库类型: 0=Access 1=SQL Server
m_Server=DESKTOP-A2L3AJS ; SQL Server 地址
m_DataBase=DellData      ; 数据库名
```

---

## 9. 部署结构

```
{部署目录}/
├── Tofd_AWI.exe              ← 主程序
├── NewInspect.Services.dll    ← 服务层
├── ClassLibrary_Interface.dll ← 接口层
├── Frame_Work.dll            ← 框架层
├── ClassLib_TestData.dll     ← 数据模型层
├── ClassLib_DataMang.dll     ← 数据访问层
├── Clb_MT_Comm.dll           ← 运动控制
├── Clb_XmCam_DLL.dll         ← 玄目相机
├── MVSDK.dll                 ← 迈德威视相机
├── Video.dll                 ← 通用摄像头
├── ClimbVideo.dll            ← 爬行器视频
├── Tofd_DLL.dll              ← TOFD 驱动
├── HD850_64.dll              ← 激光轮廓仪
├── ReportDLL.dll             ← 报表生成
├── clsExcel.dll              ← Excel 导出
│
├── database/
│   ├── DellData.mdb          ← 主数据库
│   ├── SysConfig.ini         ← 系统配置
│   └── HardConfig.ini        ← 硬件配置
│
├── SubData/                   ← 检测数据 (每个项目一个 .mdb)
│   └── {ID}_{项目名}.mdb
│
├── V_DLL.dll                 ← 玄目相机 SDK (黑盒)
├── CanCmd.dll                ← CAN 卡 SDK (黑盒)
├── MvCameraControl.Net.dll   ← 迈德威视 SDK
├── Cmm_PcPower.dll           ← 电源 SDK
├── ECT_DLL.dll               ← 涡流 SDK
├── AnyCardXSharp.dll         ← 采集卡 SDK
│
└── [Emgu.CV, OxyPlot, ...]   ← 第三方依赖
```

---

> **文档维护说明**: 本项目处于新旧架构过渡期。新架构（NewInspect.Services + 接口 + 适配器）正在逐步替代旧架构（SysInfo + 窗体直接调用硬件）。本文档以新架构为主要描述对象，旧架构标注为"待迁移"。

---

*文档结束*
