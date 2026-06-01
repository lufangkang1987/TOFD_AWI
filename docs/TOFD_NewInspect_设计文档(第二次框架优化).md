# TOFD / NewInspect 程序详细设计文档

> **版本**: 1.0 | **日期**: 2026-05-29 | **目标框架**: .NET 10.0-windows  
> **项目**: 超声衍射时差法 (TOFD) 无损检测系统

---

## 目录

1. [项目概述](#1-项目概述)
2. [整体架构](#2-整体架构)
3. [分层设计](#3-分层设计)
4. [接口层 (ClassLibrary_Interface)](#4-接口层-classlibrary_interface)
5. [框架层 (Frame_Work)](#5-框架层-frame_work)
6. [适配器层 (Adapters)](#6-适配器层-adapters)
7. [硬件层 (Hardware)](#7-硬件层-hardware)
8. [互操作层 (Interop)](#8-互操作层-interop)
9. [业务服务层 (NewInspect.Services)](#9-业务服务层-newinspectservices)
10. [数据管理层 (ClassLib_DataMang)](#10-数据管理层-classlib_datamang)
11. [测试数据层 (TestData)](#11-测试数据层-testdata)
12. [UI 层设计 (TOFD)](#12-ui-层设计-tofd)
13. [依赖注入与启动流](#13-依赖注入与启动流)
14. [消息总线设计](#14-消息总线设计)
15. [编译与部署](#15-编译与部署)

---

## 1. 项目概述

### 1.1 业务背景

TOFD (Time of Flight Diffraction) 超声衍射时差法是一种高精度工业无损检测技术。本软件系统通过超声采集卡、工业相机、四轮运动平台（小车）的协同控制，实现对焊缝/工件的自动化 TOFD 扫描检测。

### 1.2 核心功能

| 功能模块 | 说明 |
|----------|------|
| **超声采集** | 控制采集卡进行 A-Scan 信号采样，支持 1-8 通道同步采集 |
| **波形显示** | 实时 A-Scan 波形、B-Scan 截面图、C-Scan 俯视热图、TOFD 灰度图谱 |
| **视频监控** | 双工业相机：焊缝跟踪（TOFD 探头同轴）+ 全局监控（俯视） |
| **运动控制** | 四轮差速/麦克纳姆底盘：方向控制、精确位移、里程计、急停 |
| **TOFD 参数** | PCS 计算、DAC/TCG 曲线生成、声程/深度/时间换算 |
| **报警监控** | 实时阈值报警 + 设备状态监控（温度、限位、电压） |
| **报表导出** | 生成 PDF/Word/CSV/Excel 格式检测报告 |
| **数据存储** | JSON 文件存储检测记录、校准数据、系统配置 |

### 1.3 技术栈

| 类别 | 技术选型 |
|------|----------|
| 运行时 | .NET 10.0 |
| UI 框架 | Windows Forms |
| DI 容器 | Microsoft.Extensions.DependencyInjection 10.0.0-preview |
| 波形绘图 | OxyPlot 2.2.0 (WindowsForms) |
| 机器视觉 | Emgu.CV 4.13.0.5924 (OpenCV .NET 绑定) |
| 硬件通信 | P/Invoke (Cdecl)，3 套 Native DLL |
| 数据存储 | JSON 文件 (System.Text.Json) |

---

## 2. 整体架构

### 2.1 分层架构图

```
┌─────────────────────────────────────────────────────────────────────┐
│                        TOFD (WinForms UI)                           │
│  Frm_Main / Frm_* / Controls / DarkTheme                            │
├─────────────────────────────────────────────────────────────────────┤
│                    NewInspect.Services (业务服务层)                   │
│  InspectionService / CameraService / RobotMotionService             │
│  AlarmService / CalibrationService / ReportExportService ...        │
├──────────────┬──────────────────────┬───────────────────────────────┤
│   Adapters   │    ClassLib_DataMang │      TestData                 │
│  (路由层)     │    (JSON 持久化)      │     (模拟数据)                  │
├──────┬───────┼──────────────────────┼───────────────────────────────┤
│Hardware│Interop│  Frame_Work (数据结构+运动学+消息定义)                │
│(硬件抽象)│(P/Invoke)│                                                │
├──────┴───────┴──────────────────────────────────────────────────────┤
│              ClassLibrary_Interface (接口契约层)                      │
│           13个接口 + MessageBus + 50+ DTO/枚举/结构体                 │
└─────────────────────────────────────────────────────────────────────┘
```

### 2.2 项目依赖关系

```
TOFD (WinExe, net10.0-windows)
  ├── ClassLibrary_Interface    ← 接口契约 (0 依赖)
  ├── Frame_Work                → ClassLibrary_Interface
  ├── ClassLib_DataMang         → Frame_Work, TestData
  ├── TestData                  → Frame_Work
  ├── NewInspect.Services       → ClassLibrary_Interface, Frame_Work, Interop
  │     NuGet: Emgu.CV
  ├── Adapters                  → ClassLibrary_Interface, Frame_Work, Hardware, Interop
  ├── Hardware                  ← 0 依赖
  └── Interop                   ← 0 依赖 (AllowUnsafeBlocks)
```

### 2.3 解决方案配置

| 文件 | 说明 |
|------|------|
| `TOFD.sln` | VS 格式解决方案 (Format 12.00) |
| `TOFD.slnx` | 新版 XML 方案格式 |
| `Directory.Build.props` | 统一 MSBuild 属性 |
| `nuget.config` | NuGet 源配置 (仅 nuget.org) |

---

## 3. 分层设计

### 3.1 七层架构职责

| 层 | 项目 | 职责 | 依赖 |
|----|------|------|------|
| **L1 接口** | ClassLibrary_Interface | 定义全部系统契约 (13 接口 + 消息总线) | 无 |
| **L2 框架** | Frame_Work | 数据结构、运动学模型、22 种消息定义 | L1 |
| **L3 硬件抽象** | Hardware | 超声信号物理仿真 (高斯脉冲) | 无 |
| **L3 互操作** | Interop | 3 套 Native DLL P/Invoke 声明 | 无 |
| **L4 适配器** | Adapters | Simulated/Real 双模式路由 | L1, L2, L3 |
| **L5 服务** | NewInspect.Services | 11 个业务服务实现 | L1, L2, L3 |
| **L6 数据** | ClassLib_DataMang | JSON 文件持久化 (DAO 模式) | L2, TestData |
| **L6 测试数据** | TestData | 模拟检测记录/缺陷/波形生成 | L2 |
| **L7 UI** | TOFD | WinForms 界面，DI 组装一切 | 全部 |

### 3.2 设计模式

| 模式 | 应用位置 | 说明 |
|------|----------|------|
| **策略模式** | Adapters 层 | Simulated/Real 双实现，运行时通过配置字符串切换 |
| **观察者模式** | MessageBus | 类型安全的发布-订阅消息总线 |
| **适配器模式** | Adapters 层 | 将 Native DLL 适配为项目接口 |
| **依赖注入** | 全局 | Microsoft.Extensions.DI 组装 |
| **DAO 模式** | ClassLib_DataMang | 4 个 DAO 类封装数据访问 |
| **工厂方法** | Program.cs | 硬件适配器工厂函数 |

---

## 4. 接口层 (ClassLibrary_Interface)

**位置**: `ClassLibrary_Interface/` | **文件数**: 15 个 | **无外部依赖**

### 4.1 接口清单

#### 4.1.1 IAcquisitionCard — 超声采集卡

```csharp
public interface IAcquisitionCard : IDisposable
{
    bool Initialize(string configPath);
    bool StartAcquisition();
    bool StopAcquisition();
    short[] ReadData(int channelIndex, int sampleCount);
    AcquisitionStatus GetStatus();
    event EventHandler<short[]>? OnDataReady;
}

public class AcquisitionStatus
{
    public bool IsRunning { get; set; }
    public int SampleRate { get; set; }
    public int ActiveChannels { get; set; }
    public double Temperature { get; set; }
}
```

#### 4.1.2 IMotionController — 扫描轴运动控制

```csharp
public interface IMotionController : IDisposable
{
    bool Connect(string portName, int baudRate);
    bool Disconnect();
    bool MoveToPosition(int axis, double position, double speed);
    bool MoveContinuous(int axis, bool forward, double speed);
    bool StopAll();
    double GetPosition(int axis);
    MotionStatus GetStatus();
    event EventHandler<MotionEventArgs>? OnPositionChanged;
}
```

关联类型: `MotionEventArgs` (Axis, Position, MotionState), `MotionStatus` (IsConnected, ActiveAxisCount, double[] Positions), `MotionState` 枚举 (Idle/Moving/Error/Homed)

#### 4.1.3 IProbeController — 探头控制

```csharp
public interface IProbeController : IDisposable
{
    bool SelectProbe(int probeIndex);
    bool SetGain(double gainDb);          // 0~80 dB
    bool SetPulseWidth(double widthNs);   // 20~500 ns
    double GetGain();
    ProbeInfo GetProbeInfo(int probeIndex);
}
```

`ProbeInfo`: Index, Name, Frequency(MHz), ElementSize(mm), ElementCount, Angle(deg)

#### 4.1.4 IRobotMotionController — 四轮机器人运动

```csharp
public interface IRobotMotionController : IDisposable
{
    // CAN 通信
    bool ConnectCan(string canInterface, int baudRate);
    bool DisconnectCan();
    bool IsConnected { get; }

    // 车轮独立控制
    bool SetWheelSpeed(int wheelIndex, double rpm);
    bool SetWheelSpeeds(double[] rpms);
    bool SetWheelTorque(int wheelIndex, double torqueNm);
    bool StopAllWheels();
    bool EmergencyStop();

    // 底盘高级指令
    bool MoveVelocity(double vx, double vy, double omega);
    bool MoveToPosition(double x, double y, double theta, double speed);
    bool Rotate(double angle, double angularSpeed);

    // 反馈
    double GetWheelPosition(int wheelIndex);
    double GetWheelVelocity(int wheelIndex);
    RobotOdometry GetOdometry();
    WheelEncoderData[] GetAllEncoderData();

    // 状态
    RobotMotionStatus GetRobotStatus();
    bool IsEmergencyStopped { get; }

    // 事件
    event EventHandler<RobotOdometryEventArgs>? OnOdometryUpdated;
    event EventHandler<EmergencyStopEventArgs>? OnEmergencyStop;
}
```

关联类型: `RobotOdometry` (X,Y,Theta,Vx,Vy,Omega), `WheelEncoderData` (Position,Velocity,Current,Temperature), `ChassisType` 枚举 (Differential/Mecanum/Ackermann)

#### 4.1.5 ICameraService — 双摄像头服务

```csharp
public interface ICameraService : IDisposable
{
    bool Initialize(int cameraCount = 2);
    CameraFrame? CaptureFrame(int cameraIndex);
    bool StartContinuousCapture(CameraType cameraType, int fps);
    bool StopContinuousCapture(int cameraIndex);
    bool SetExposure(int cameraIndex, double exposureMs);
    bool SetGain(int cameraIndex, double gainDb);
    bool SetResolution(int cameraIndex, int width, int height);
    CameraInfo GetCameraInfo(int cameraIndex);
    WeldPosition? DetectWeldPosition(CameraFrame frame);
    bool StartWeldTracking(int fps);
    bool StopWeldTracking();
    event EventHandler<CameraFrame>? OnFrameCaptured;
    event EventHandler<WeldPosition>? OnWeldDetected;
    CameraStatus GetStatus(int cameraIndex);
}
```

关联类型: `CameraType` 枚举 (WeldTracking=0/GlobalMonitor=1), `CameraFrame` (BGR byte[], Width, Height, Timestamp), `WeldPosition` (OffsetX/Y, Angle, Confidence 0~1), `CameraInfo` (Model, Serial, MaxFps)

#### 4.1.6 IAlarmService — 报警服务

```csharp
public interface IAlarmService
{
    void CheckThreshold(short[] data, double threshold);
    void SubscribeAlarm(Action<AlarmInfo> handler);
    List<AlarmInfo> GetActiveAlarms();
    List<AlarmInfo> GetAlarmHistory();
    void ClearAlarm(string alarmId);
    void AcknowledgeAlarm(string alarmId);
}
```

`AlarmInfo`: Id(Guid), Severity(Info/Warning/Error/Critical), Message, Timestamp, Acknowledged

#### 4.1.7 ICalibrationService — 校准服务

```csharp
public interface ICalibrationService
{
    bool CalibrateVelocity(double knownThickness, out double velocity);
    bool CalibrateDelay(double[] wedgeDistance, out double delay);
    bool CalibrateSensitivity(double[] referenceEcho);
    CalibrationResult GetCurrentCalibration();
    bool LoadCalibration(string filePath);
    bool SaveCalibration(string filePath);
}
```

`CalibrationResult`: Velocity(m/s), ProbeDelay(us), Sensitivity(dB), CalibratedAt

#### 4.1.8 ITofdParameterService — TOFD 参数服务

```csharp
public interface ITofdParameterService
{
    TofdProbeConfig GetProbeConfig();
    void SetProbeConfig(TofdProbeConfig config);
    TofdWedgeConfig GetWedgeConfig();
    void SetWedgeConfig(TofdWedgeConfig config);
    TofdGateConfig GetGateConfig();
    void SetGateConfig(TofdGateConfig config);
    TofdScanPlan GetScanPlan();
    void SetScanPlan(TofdScanPlan plan);

    PcsCalculationResult CalculatePcs(double thickness, double probeAngle, double wedgeDelay);
    DacTcgCurve GenerateDacCurve(double[] holeDepths, double[] amplitudes);
    DacTcgCurve GenerateTcgCurve(double[] holeDepths);
    double SoundPathToDepth(double soundPath, double pcs, double velocity);
    double DepthToTime(double depth, double pcs, double velocity, double wedgeDelay);
    double TimeToDepth(double time, double pcs, double velocity, double wedgeDelay);
    bool SaveParameters(string filePath);
    bool LoadParameters(string filePath);
}
```

关键 DTO: `TofdProbeConfig` (Frequency, Angle, ElementSize), `TofdWedgeConfig` (Angle, Velocity, Delay), `TofdGateConfig` (GateStart/End, Threshold), `PcsCalculationResult` (PCS, SoundPath, BeamSpreadAngle, TimeRange), `DacTcgCurve` (Type, List\<CurvePoint\>)

#### 4.1.9 IWaveformDisplayService — 波形显示服务

```csharp
public interface IWaveformDisplayService
{
    void PushAScanData(int channelIndex, short[] data, double sampleRate);
    AScanBuffer GetAScanBuffer(int channelIndex);
    AScanBuffer[] GetAllAScanBuffers();
    BScanImage BuildBScanImage(int channelIndex, double gateStart, double gateEnd);
    CScanImage BuildCScanImage(double gateStart, double gateEnd, int gridRes);
    TofdGrayScaleImage BuildTofdImage(int channelIndex);
    short[] GetPeakHoldData(int channelIndex);
    void ResetPeakHold();
    void SetDisplayRange(int channelIndex, double timeStart, double timeEnd);
    void SetColorMap(string colorMap);  // "Gray"/"Jet"/"Hot"/"Cool"
    event EventHandler<WaveformUpdateEventArgs>? OnAScanUpdated;
    event EventHandler<BScanImage>? OnBScanUpdated;
    event EventHandler<CScanImage>? OnCScanUpdated;
    event EventHandler<TofdGrayScaleImage>? OnTofdUpdated;
}
```

关联数据: `AScanBuffer` (DataQueue 容量 500, PeakHold), `BScanImage` (Width/Height, byte[] PixelData), `CScanImage` (AmplitudeMap + DepthMap), `TofdGrayScaleImage` (GrayData, PCS, Velocity)

#### 4.1.10 IDeviceStatusService — 设备状态服务

```csharp
public interface IDeviceStatusService
{
    AcquisitionCardStatus GetAcquisitionCardStatus();
    AxisStatus[] GetAxisStatus();
    CameraDeviceStatus[] GetCameraStatus();
    SystemHealthStatus GetSystemHealthStatus();
    DeviceStatusSnapshot GetFullSnapshot();
    List<DeviceStatusSnapshot> GetStatusHistory(int lastMinutes = 10);
    LimitSwitchStatus[] GetLimitSwitchStatus();
    TemperatureStatus GetTemperatureStatus();
    void StartMonitoring();
    void StopMonitoring();
    event EventHandler<DeviceStatusSnapshot>? OnStatusUpdated;
    event EventHandler<LimitSwitchTriggeredEventArgs>? OnLimitTriggered;
    event EventHandler<OvertempEventArgs>? OnOvertemperature;
}
```

关键 DTO: `DeviceStatusSnapshot` (聚合全部硬件状态), `TemperatureStatus` (CardTemp, MotorTemps[], AmbientTemp, Warning=70°C, Critical=85°C), `SystemHealthStatus` (HealthLevel, CpuUsage, MemoryUsage, Uptime)

#### 4.1.11 ISystemConfig — 系统配置

```csharp
public interface ISystemConfig
{
    SystemConfiguration Load();
    void Save(SystemConfiguration config);
    T GetValue<T>(string key, T defaultValue);
    void SetValue<T>(string key, T value);
}
```

`SystemConfiguration`: 5 个硬件类型路由字段 (AcquisitionCardType/MotionControllerType/... 均为 "Simulated"/"Real"), 数据路径, 默认采集参数 (100MHz/2048点/2通道), 扫描参数 (300mm×100mm, 步进1mm), 报警阈值 80%

#### 4.1.12 IDataStorage — 数据存储

```csharp
public interface IDataStorage
{
    bool SaveInspectionData(InspectionRecordDto record, string? filePath = null);
    InspectionRecordDto? LoadInspectionData(string filePath);
    bool DeleteInspectionData(string filePath);
    List<InspectionSummaryDto> QueryRecords(DateTime from, DateTime to);
    List<DefectInfoDto> GetDefects();
    string GetStoragePath();
}
```

DTO: `InspectionRecordDto` (Id, OperatorName, WeldId, Defects[], JsonData), `InspectionSummaryDto` (RecordId, DefectCount, HasCriticalDefect), `DefectInfoDto` (Id, Depth, Length, Height, Classification, Severity)

#### 4.1.13 IReportExportService — 报表导出

```csharp
public interface IReportExportService
{
    string GeneratePdfReport(InspectionRecordDto record, object? tofdParameters, string outputPath);
    string GenerateWordReport(InspectionRecordDto record, object? tofdParameters, string outputPath);
    string BatchExportPdf(List<InspectionRecordDto> records, string outputDir);
    string ExportDefectsToCsv(List<DefectInfoDto> defects, string outputPath);
    string ExportScanDataToCsv(List<short[]> scanData, double sampleRate, string outputPath);
    string ExportDefectsToExcel(List<DefectInfoDto> defects, string outputPath);
    string ExportInspectionSummaryToExcel(List<InspectionSummaryDto> summaries, string outputPath);
    DefectStatistics GenerateDefectStatistics(List<DefectInfoDto> defects);
    bool SetReportTemplate(string templatePath);
    string GetReportTemplate();
}
```

`DefectStatistics`: TotalCount, ByType/BySeverity 字典, AverageDepth/Length/Height, MaxAmplitude, DefectDensity

### 4.2 消息总线

```csharp
public interface IMessageBus
{
    void Subscribe<T>(Action<T> handler) where T : MessageBase;
    void Unsubscribe<T>(Action<T> handler) where T : MessageBase;
    void Publish<T>(T message) where T : MessageBase;
}

public abstract class MessageBase
{
    public DateTime Timestamp { get; set; }
    public string Source { get; set; }
}
```

**实现** (`MessageBus`): Dictionary\<Type, List\<Delegate\>\> + lock 同步，Publish 时快照机制防止迭代中修改异常。

---

## 5. 框架层 (Frame_Work)

**位置**: `Frame_Work/` | **文件数**: 10 个 | **依赖**: ClassLibrary_Interface

### 5.1 核心数据结构 (`Struct.cs`)

| 类型 | 类别 | 关键属性 |
|------|------|----------|
| `Point3D` (struct) | 三维坐标 | X, Y, Z (double) |
| `AScanData` (struct) | A-Scan | ChannelIndex, ProbeIndex, Point3D Position, short[] RawData, SampleRate, Timestamp |
| `BScanData` (struct) | B-Scan | Width/Height, byte[] AmplitudeData, StartPosition/EndPosition, SoundPathMin/Max |
| `CScanData` (struct) | C-Scan | Width/Height, byte[] AmplitudeMap+DepthMap, GateStart/End |
| `TofdImageData` (struct) | TOFD 图谱 | ScanLength, DepthPoints, byte[] GrayScaleData, LateralWaveTime, BackwallTime, PcsValue |
| `ScanState` (enum) | 扫描状态 | Idle / Scanning / Paused / Stopped / Error |
| `ScanMode` (enum) | 扫描模式 | AScan / BScan / CScan / Tofd / PhasedArray |
| `ProbeType` (enum) | 探头类型 | SingleElement / DualElement / PhasedArray / Tofd |

### 5.2 业务实体

| 类 | 说明 |
|----|------|
| `DefectInfo` | 缺陷信息: Id, Point3D Location, Depth/Length/Height, Amplitude, Classification, Severity |
| `InspectionRecord` | 完整检测记录聚合: ScanParameters + MaterialInfo + List\<AScanData\> + TofdImageData + List\<DefectInfo\> |
| `InspectionSummary` | 记录摘要: RecordId, WeldId, DefectCount, HasCriticalDefect, ScanLength |
| `ScanParameters` | 扫描参数: ScanLength/Width, StepSize, ProbeCount/Frequency, SampleRate(100MHz)/Points(2048), Gain |
| `MaterialInfo` | 材料信息: Name, Grade, WaveVelocity(5920m/s), ShearVelocity(3230m/s), Density(7800kg/m³), Thickness |

### 5.3 运动学引擎 (`RobotKinematics.cs`)

```csharp
public class RobotKinematics
{
    // 参数
    public double WheelRadius { get; set; } = 50;     // mm
    public double WheelBase { get; set; } = 300;      // mm
    public double TrackWidth { get; set; } = 250;     // mm
    public double GearRatio { get; set; } = 1.0 / 30;
    public ChassisType ChassisType { get; set; } = ChassisType.Differential;

    // 逆运动学: 机器人速度 → 4轮 RPM
    public double[] InverseKinematics(double vx, double vy, double omega);

    // 正运动学: 4轮 RPM → 机器人速度
    public RobotVelocity ForwardKinematics(double[] wheelRpms);

    // 里程计航迹推算 (中值积分)
    public RobotOdometry UpdateOdometry(RobotOdometry prev, double[] wheelRpms, double dt);
}
```

支持 **差速 (Differential)** 和 **麦克纳姆轮 (Mecanum)** 两种底盘解算。

**PID 控制器**:

```csharp
public class PositionPIDController
{
    public double Kp { get; set; } = 1.0;
    public double Ki { get; set; } = 0.0;
    public double Kd { get; set; } = 0.1;
    public double IntegralMax { get; set; } = 100;
    public double OutputMax { get; set; } = 3000;

    public double Compute(double setpoint, double measurement, double dt);
    public void Reset();
}
```

### 5.4 应用消息定义 (`AppMessages.cs`)

全部继承 `MessageBase`，共 **21 个消息类**，分 6 组:

| 分组 | 消息类 | 关键属性 |
|------|--------|----------|
| **扫描** | `ScanStateChangedMessage` | OldState, NewState |
| | `AlarmTriggeredMessage` | Alarm |
| | `DataAvailableMessage` | AScanData |
| | `CalibrationCompletedMessage` | Velocity, Delay |
| | `PositionUpdateMessage` | Axis, Position |
| | `ErrorMessage` | ErrorCode, Description, Exception |
| | `TofdParametersChangedMessage` | ParameterName, TofdParameterSet |
| | `EmergencyStopMessage` | Reason |
| | `LimitSwitchTriggeredMessage` | AxisIndex, LimitName |
| **相机** | `CameraFrameCapturedMessage` | CameraIndex, Width, Height, Timestamp |
| | `WeldDetectedMessage` | OffsetX, OffsetY, Angle, Confidence |
| | `CameraStatusChangedMessage` | CameraIndex, IsConnected, CurrentFps |
| **波形** | `WaveformUpdatedMessage` | ChannelIndex, SampleCount |
| | `BScanUpdatedMessage` | ChannelIndex, ImageWidth/Height |
| | `CScanUpdatedMessage` | GridResolution, GateStart/End |
| | `TofdImageUpdatedMessage` | ChannelIndex, TimeMin/Max |
| **报表** | `ReportGeneratedMessage` | ReportType, OutputPath |
| | `DefectStatisticsUpdatedMessage` | TotalCount, AverageDepth |
| **设备** | `DeviceStatusUpdatedMessage` | Component, Status |
| | `OvertemperatureMessage` | Component, Temperature, Threshold |
| | `SystemHealthChangedMessage` | Level, CpuUsage, MemoryUsage |

### 5.5 TOFD 参数聚合 (`TofdParameters.cs`)

```csharp
public class TofdParameterSet
{
    public string Name { get; set; } = "默认";
    public TofdProbeConfig Probe { get; set; }
    public TofdWedgeConfig Wedge { get; set; }
    public TofdGateConfig Gate { get; set; }
    public TofdScanPlan ScanPlan { get; set; }
    public DacTcgCurve DacCurve { get; set; }
    public DacTcgCurve TcgCurve { get; set; }
    public MaterialInfo Material { get; set; }
    public string StandardCode { get; set; } = "NB/T 47013.10-2015";
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}
```

---

## 6. 适配器层 (Adapters)

**位置**: `Adapters/` | **文件数**: 13 个 | **依赖**: ClassLibrary_Interface, Frame_Work, Hardware, Interop

### 6.1 设计模式

每个适配器读取 `SystemConfiguration` 中的硬件类型字段 (`"Simulated"` / `"Real"`)，运行时构造对应实现:

```
config.CameraType == "Real"  → RealCameraService (P/Invoke)
config.CameraType == "Simulated" → SimulatedCameraService (合成图案)
```

### 6.2 适配器总览

| 适配器 | 实现接口 | 构造函数依赖 |
|--------|----------|-------------|
| `AcquisitionCardAdapter` | `IAcquisitionCard` | `SystemConfiguration` |
| `MotionControllerAdapter` | `IMotionController` | `SystemConfiguration` |
| `ProbeControllerAdapter` | `IProbeController` | `SystemConfiguration` |
| `RobotMotionControllerAdapter` | `IRobotMotionController` | `SystemConfiguration, IMessageBus, ISystemConfig` |
| `CameraAdapter` | `ICameraService` | `SystemConfiguration, IMessageBus` |
| `DataStorageAdapter` | `IDataStorage` | `SystemConfiguration` |
| `CanBusAdapter` | 独立 IDisposable | 无 |

### 6.3 Simulated 子层

| 实现类 | 关键特性 |
|--------|----------|
| `SimulatedAcquisitionCard` | 后台 Task 以 20Hz 生成合成超声波信号 (主脉冲+回波+噪声) |
| `SimulatedMotionController` | 3 轴位置 ±0.005mm 抖动模拟 |
| `SimulatedProbeController` | 4 个预定义探头 (TOFD 5/10MHz, PA)，Gain 0~80dB |
| `SimulatedRobotMotionController` | 完整 4 轮编码器仿真，50ms 差速里程计，急停事件 |
| `SimulatedCameraService` | 相机0=激光线灰度图，相机1=彩色渐变图，模拟焊缝偏移检测 |

### 6.4 Real 子层

| 实现类 | 状态 | 说明 |
|--------|------|------|
| `RealAcquisitionCard` | 桩 (throw) | TODO: 对接 Tofd_DLL |
| `RealMotionController` | 桩 (throw) | TODO: 对接 CAN/串口 |
| `RealRobotMotionController` | **已实现** | P/Invoke `CanMotionControllerNative`，注册编码器/错误回调 |
| `RealCameraService` | **已实现** | P/Invoke `CameraSDKNative`，非托管内存帧捕获 |

---

## 7. 硬件层 (Hardware)

**位置**: `Hardware/` | **文件数**: 6 个 | **依赖**: 无

### 7.1 Simulated 子层 (功能完整)

| 类 | 说明 |
|----|------|
| `SimulatedHardwareBase` (abstract) | 基类: Random Rng + Initialize/Shutdown + Noise() 高斯噪声 |
| `SimulatedADConverter` | 模拟 ADC: 100MHz 采样率, 16bit 分辨率, 量化噪声 |
| `SimulatedPulserReceiver` | 声学高斯脉冲仿真: 200V 脉冲电压, 100ns 脉宽, 40dB 增益, 1~20MHz 带宽 |

### 7.2 Real 子层 (桩)

| 类 | 说明 |
|----|------|
| `RealHardwareBase` (abstract) | 空桩 — "TODO: DLL加载、设备枚举、连接管理" |
| `RealADConverter` | 空桩 — "TODO: P/Invoke 调用ADC驱动" |
| `RealPulserReceiver` | 空桩 — "TODO: P/Invoke Tofd_DLL" |

---

## 8. 互操作层 (Interop)

**位置**: `Interop/` | **文件数**: 5 个 | **依赖**: 无 | **AllowUnsafeBlocks**

### 8.1 P/Invoke 汇总 (57 个 DllImport)

| DLL | 函数数 | 用途 |
|-----|--------|------|
| `CanMotionController.dll` | 18 个 | 电机控制: 速度/转矩/编码器/CAN帧/回调 |
| `TofdHardware.dll` | 20 个 | 超声采集卡: 采集/A-Scan读取/增益/脉宽/DAC/TCG/回调 |
| `CameraSDK.dll` | 19 个 | 工业相机: 枚举/打开/采集/曝光/增益/分辨率/帧回调 |

### 8.2 通用工具

#### NativeErrorCodes (`Common/NativeErrorCodes.cs`)

统一错误码定义: Success(0), ErrorDeviceNotFound(-2), ErrorTimeout(-4), ErrorInvalidParameter(-5) 等 + `GetErrorMessage()` 中文描述 + `ThrowOnError()`

#### NativeMemoryHelper (`Common/NativeMemoryHelper.cs`)

安全非托管内存操作:
- `ToManagedShortArray(IntPtr, int)` — IntPtr → short[]
- `PtrToArray<T>(IntPtr, int)` — 泛型转换
- `AllocBuffer/FreeBuffer` — 分配/释放
- `UsingBuffer(int, Action<IntPtr>)` — using 模式安全包装

### 8.3 回调委托示例

```csharp
// CAN 编码器回调
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
delegate void EncoderDataCallback(
    [In] MotorEncoderDataNative[] data, int count, IntPtr userData);

// 相机帧回调
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
delegate void FrameCallback(
    IntPtr imageData, int width, int height, int stride,
    long frameNumber, int pixelFormat, IntPtr userData);

// 采集卡数据回调
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
delegate void DataReadyCallback(
    int channel, IntPtr dataPtr, int sampleCount, double sampleRate, IntPtr userData);
```

---

## 9. 业务服务层 (NewInspect.Services)

**位置**: `NewInspect.Services/` | **文件数**: 11 个 | **依赖**: ClassLibrary_Interface, Frame_Work, Interop

### 9.1 服务清单

| 服务 | 注入接口 | 关键功能 |
|------|----------|----------|
| **InspectionService** | 具体类 (Singleton) | 扫描流程控制: Start/Stop/Pause/Resume，协调采集卡+运动+探头，AScanBuffer 管理 |
| **AlarmService** | `IAlarmService` | 阈值检测，活跃/历史报警管理，Acknowledge/Clear |
| **CalibrationService** | `ICalibrationService` | 声速/延迟/灵敏度校准，JSON 加载/保存 |
| **ConfigService** | `ISystemConfig` | JSON 文件加载/保存 `SystemConfiguration`，反射 GetValue/SetValue |
| **DataProcessingService** | 具体类 (Singleton) | **占位** — "TODO: 带通滤波、Hilbert 包络、缺陷自动识别" |
| **RobotMotionService** | 具体类 (Singleton) | 封装 `RobotKinematics` + `PositionPIDController`，里程计更新 |
| **WaveformDisplayService** | `IWaveformDisplayService` | 多通道 A-Scan 缓冲 (容量 500)，峰值保持，B/C/TOFD 图像构建 |
| **TofdParameterService** | `ITofdParameterService` | PCS 计算 (含波束扩散角物理公式)，DAC/TCG 曲线生成，声程/深度/时间互转 |
| **CameraService** | 具体类 (Singleton) | **Emgu.CV 实现**: VideoCapture 帧捕获，焊缝位置检测 (阈值分割+轮廓查找) |
| **ReportExportService** | `IReportExportService` | HTML 格式 PDF/Word 报告，CSV/Excel 导出，缺陷统计 |
| **DeviceStatusService** | `IDeviceStatusService` | 500ms 轮询所有硬件状态，温度/限位告警，History 快照记录 |

### 9.2 关键服务实现细节

#### InspectionService (核心编排)

```
StartScan(ScanParameters)
  ├── _acquisitionCard.Initialize()         // 配置采集卡
  ├── _paramService.SetScanPlan(plan)       // 应用参数
  ├── _acquisitionCard.StartAcquisition()    // 启动采集
  ├── _motionController.MoveContinuous()    // 启动扫描运动
  ├── Subscribe OnDataReady                 // 每帧: PushAScanData → 发布 DataAvailableMessage
  └── _messageBus.Publish(ScanStateChanged) // 通知 UI 状态变化
```

#### CameraService (Emgu.CV 直接实现)

不通过适配器层，直接用 Emgu.CV 封装:
- `VideoCapture` 打开相机 → 连续帧采集 (Task + CancellationToken)
- `DetectWeldPosition()`: 阈值分割 + 轮廓查找 → 焊缝中心像素 → 世界坐标
- `CameraFrame` ↔ `Mat` 互转

#### TofdParameterService (物理计算)

```csharp
// PCS 计算公式 (含波束扩散角):
public PcsCalculationResult CalculatePcs(double thickness, double probeAngle, double wedgeDelay)
{
    double wedgeSoundPath = wedgeDelay * wedgeVelocity / 2;
    double beamSpread = Math.Asin(0.51 * velocity / (frequency * elementSize)) * 180 / Math.PI;
    double halfBeam = Math.Tan((probeAngle + beamSpread) * Math.PI / 180) * thickness;
    double pcs = 2 * (halfBeam + wedgeSoundPath * Math.Sin(probeAngle * Math.PI / 180));
    double soundPath = Math.Sqrt(pcs * pcs / 4 + thickness * thickness);
    // ... 计算时间窗口、深度覆盖范围
}
```

---

## 10. 数据管理层 (ClassLib_DataMang)

**位置**: `ClassLib_DataMang/` | **文件数**: 5 个 | **依赖**: Frame_Work, TestData

### 10.1 存储结构

```
./Data/
  inspections/      检测记录 JSON (文件名 = record.Id)
  calibrations/     校准数据 JSON
  configs/          系统配置 JSON
```

### 10.2 类层次

| 类 | 职责 |
|----|------|
| `DataContext` | 根上下文: 自动 CreateDirectory，SaveInspectionAsync/LoadInspectionAsync/DeleteInspection |
| `InspectionDao` | 检测记录 CRUD: InsertAsync/GetByIdAsync/QueryAsync(时间范围)/Delete |
| `DefectDao` | 缺陷查询: GetByRecordAsync/GetBySeverityAsync |
| `CalibrationDao` | 泛型校准存取: SaveAsync\<T\>/LoadAsync\<T\> |
| `ConfigDao` | 系统配置: LoadAsync/SaveAsync |

---

## 11. 测试数据层 (TestData)

**位置**: `TestData/` | **文件数**: 3 个 | **依赖**: Frame_Work

| 静态类 | 说明 |
|--------|------|
| `MockDefectData` | 随机缺陷生成: 6 种类型 (气孔/夹渣/未熔合/裂纹/咬边/根部未焊透)，4 级严重等级 |
| `MockInspectionRecords` | 懒加载 20 条模拟检测记录，按 WeldId 查找 |
| `SeedDataGenerator` | 完整检测记录工厂: CreateMockRecord/CreateMockRecords/GenerateMockWaveform |

---

## 12. UI 层设计 (TOFD)

**位置**: `TOFD/` | **文件数**: 36 个 .cs | **依赖**: 全部 8 个项目

### 12.1 Form 清单 (8 个在用)

| Form | 注入服务 | 布局方式 | 说明 |
|------|----------|----------|------|
| **Frm_Main** | 14 个服务 | 纯代码 | **当前主窗体**: Sidebar + ViewContainer 布局，暗色主题 |
| `Frm_Main_C_New` | 13 个服务 | Designer | **备用主窗体**: TabControl 7页+右侧参数面板+顶部工具栏 |
| `Frm_Config` | `ISystemConfig` | Designer | 硬件类型选择 + 数据路径 + 采集参数 |
| `Frm_Calibration` | `ICalibrationService` | 纯代码 | 声速/延迟/灵敏度校准 |
| `Frm_AlarmList` | `IAlarmService` | 纯代码 | 报警列表 (500ms 自动刷新) |
| `Frm_ReportExport` | `IReportExportService, IDataStorage` | 纯代码 | 5 种导出格式 + DataGridView 缺陷预览 |
| `Frm_FloatingMotion` | `IRobotMotionController` | 纯代码 | 浮动无边框运动控制 Overlay |
| `Frm_*` (4个弹窗) | 各1个服务 | 纯代码 | DeviceStatus/RobotControl/TofdSetup — 纯封装对应面板 |

### 12.2 Control 清单 (18 个)

#### 核心视图控件

| 控件 | 依赖服务 | 说明 |
|------|----------|------|
| `InspectorGrid` | ICameraService, IWaveformDisplayService, ITofdParameterService | **3×3 检测监控网格**: CamA(0,0) + Waveform(0,1,rowspan2) + Params(0,2,rowspan2) + CamB(1,0) + Metrics(2,0-2) |
| `MultiChannelAScanView` | IWaveformDisplayService | 1-8 通道 A-Scan 实时波形 (OxyPlot)，峰值保持+通道切换 |
| `BScanViewControl` | 无 (数据驱动) | 灰度 B-Scan 截面图 + 闸门覆盖层 |
| `CScanViewControl` | 无 (数据驱动) | Jet 色彩映射热图: 蓝(0)→青(64)→绿(128)→黄(192)→红(255) |
| `TofdImageViewControl` | 无 (数据驱动) | TOFD 灰度图谱 + 直通波/底面波标注 + 缺陷标记 |

#### 相机控件

| 控件 | 依赖服务 | 说明 |
|------|----------|------|
| `DualCameraPanel` | ICameraService | 左右并列双 CameraPreviewControl + 工具栏 |
| `CameraPreviewControl` | ICameraService | PictureBox 渲染单路视频 + 十字准心 + 帧信息覆盖层 |

#### 参数与状态控件

| 控件 | 依赖服务 | 说明 |
|------|----------|------|
| `TofdParameterPanel` | ITofdParameterService | 5 Tab: 探头/楔块/PCS计算器/闸门/扫描计划 |
| `DeviceStatusPanel` | IDeviceStatusService | 8 个 StatusIndicator 网格: 采集卡/4轴/2相机/系统 |
| `StatusIndicator` | 无 | 自绘圆形指示灯 + 标签 + 数值 + 错误计数气泡 (Critical 闪烁) |
| `StatusBarControl` | 无 | 底部 24px 状态栏: CH1/CH2 + Gain + PRF + Mode + 时钟 |
| `MetricsPanel` | 无 | 6 列指标卡: 深度/长度/幅度/SNR/位置/耦合 |

#### 运动控件

| 控件 | 依赖服务 | 说明 |
|------|----------|------|
| `RobotControlPanel` | IRobotMotionController | 3×3 方向按钮 + 速度/角速度滑块 + 里程计 + 急停 |

#### 布局控件

| 控件 | 说明 |
|------|------|
| `NavigationSidebar` | 64px 宽侧边栏，5 个图标导航: 检测/数据/报告/操控/设置 |
| `ViewContainer` | 视图容器: RegisterView("key", Control) + SwitchTo("key") |
| `TitleBar` | 32px 标题栏: "NewInspect" Logo + 系统状态 + 时钟 |
| `DarkTheme` (静态类) | 暗色主题: 9 色常量 + `ApplyToForm(Form)` 递归应用 + 控件工厂方法 |

### 12.3 暗色主题规范

```csharp
public static class DarkTheme
{
    // 背景色
    public static readonly Color BgRoot     = Color.FromArgb(0x0A, 0x0E, 0x14);  // 最深
    public static readonly Color Surface    = Color.FromArgb(0x13, 0x18, 0x20);  // 面板
    public static readonly Color Elevated   = Color.FromArgb(0x1C, 0x23, 0x30);  // 高亮面板

    // 文字色
    public static readonly Color TextPrimary   = Color.FromArgb(0xC9, 0xD1, 0xD9);
    public static readonly Color TextSecondary = Color.FromArgb(0x8B, 0x94, 0x9E);

    // 功能色
    public static readonly Color Accent    = Color.FromArgb(0x2D, 0xD4, 0xBF);  // Teal
    public static readonly Color Border    = Color.FromArgb(0x30, 0x38, 0x48);
    public static readonly Color Success   = Color.FromArgb(0x23, 0xC5, 0x5E);
    public static readonly Color Warning   = Color.FromArgb(0xF5, 0xA6, 0x23);
    public static readonly Color Error     = Color.FromArgb(0xEF, 0x44, 0x44);

    public static Font FontUi()   => new("Segoe UI", 9f);
    public static Font FontMono() => new("Consolas", 9f);

    public static void ApplyToForm(Form form);  // 递归遍历控件树应用主题
    public static void ApplySingleControl(Control ctrl);  // 按类型应用: Button/TextBox/Label/Panel/...
    public static Button CreateAccentButton(string text);  // 工厂
    public static Button CreateToolbarButton(string text);  // 工厂
}
```

### 12.4 Frm_Main 布局结构

```
┌─ TitleBar (32px) ────────────────────────────────────┐
│ "NewInspect"                   状态: System Ready │ 14:32 │
├─ NavigationSidebar (64px) ──┬─ ViewContainer ──────────┤
│ 🔍 检测   (active)           │ InspectorGrid            │
│ 📁 数据                      │ ┌───────┬───────┬─────┐ │
│ 📊 报告                      │ │ Cam A │Waveform│参数│ │
│ ─────────                    │ ├───────┤       │面板 │ │
│ 🎮 操控 (toggle overlay)     │ │ Cam B │       │     │ │
│ ⚙  设置                      │ ├───────┴───────┴─────┤ │
│                              │ │ 指标:深度|长度|幅度... │ │
├──────────────────────────────┴─ StatusBar (24px) ──────┤
│ CH1:● CH2:● │ Gain:42dB PRF:2000Hz TOFD │ 14:32       │
└────────────────────────────────────────────────────────┘
```

---

## 13. 依赖注入与启动流

### 13.1 Program.cs 注册一览

| 注册方式 | 接口/类型 | 实现类 | 生命周期 |
|----------|-----------|--------|----------|
| 直接 | `IMessageBus` | `MessageBus` | Singleton |
| 工厂 | `ISystemConfig` | `ConfigService` | Singleton |
| 工厂 | `IAcquisitionCard` | `AcquisitionCardAdapter` | Singleton |
| 工厂 | `IMotionController` | `MotionControllerAdapter` | Singleton |
| 工厂 | `IProbeController` | `ProbeControllerAdapter` | Singleton |
| 工厂 | `IDataStorage` | `DataStorageAdapter` | Singleton |
| 工厂 | `IRobotMotionController` | `RobotMotionControllerAdapter` | Singleton |
| 工厂 | `ICameraService` | `CameraAdapter` | Singleton |
| 直接 | `IAlarmService` | `AlarmService` | Singleton |
| 直接 | `ICalibrationService` | `CalibrationService` | Singleton |
| 直接 | `ITofdParameterService` | `TofdParameterService` | Singleton |
| 直接 | `IWaveformDisplayService` | `WaveformDisplayService` | Singleton |
| 直接 | `IReportExportService` | `ReportExportService` | Singleton |
| 直接 | `IDeviceStatusService` | `DeviceStatusService` | Singleton |
| 直接 | `DataProcessingService` | 同左 | Singleton |
| 直接 | `InspectionService` | 同左 | Singleton |
| 直接 | `CameraService` | 同左 | Singleton |
| 直接 | `RobotMotionService` | 同左 | Singleton |

Form 全部 Transient 注册: Frm_Main, Frm_Config, Frm_Calibration, Frm_AlarmList, Frm_ReportExport, Frm_TofdSetup, Frm_RobotControl, Frm_DeviceStatus

### 13.2 硬件适配器工厂函数

```csharp
// 示例: 采集卡适配器
IAcquisitionCard acqAdapter = config.AcquisitionCardType == "Real"
    ? new RealAcquisitionCard()
    : new SimulatedAcquisitionCard();
```

所有适配器工厂遵循相同模式: 读取 `SystemConfiguration` 中对应字段，构造 `Real*/Simulated*` 实现。

---

## 14. 消息总线设计

### 14.1 实现原理

```csharp
public class MessageBus : IMessageBus
{
    private readonly Dictionary<Type, List<Delegate>> _handlers = new();
    private readonly object _lock = new();

    public void Subscribe<T>(Action<T> handler) where T : MessageBase
    {
        lock (_lock)
        {
            var type = typeof(T);
            if (!_handlers.ContainsKey(type))
                _handlers[type] = new List<Delegate>();
            _handlers[type].Add(handler);
        }
    }

    public void Publish<T>(T message) where T : MessageBase
    {
        List<Delegate>? handlers;
        lock (_lock)
        {
            if (!_handlers.TryGetValue(typeof(T), out handlers)) return;
            handlers = new List<Delegate>(handlers);  // 快照 — 防并发修改
        }

        foreach (var handler in handlers.Cast<Action<T>>())
        {
            try { handler(message); }
            catch (Exception ex) { /* 吞异常，记录日志 */ }
        }
    }
}
```

### 14.2 典型消息流

```
InspectionService.StartScan()
  ├── _acquisitionCard.StartAcquisition()
  ├── _messageBus.Publish(new ScanStateChangedMessage { ... })
  │     └── Frm_Main.OnScanStateChanged()    → 更新标题栏状态
  │     └── StatusBarControl.SetScanState()  → 更新状态栏颜色
  │
  └── _acquisitionCard.OnDataReady += (data) =>
      ├── _waveformService.PushAScanData(ch, data, sampleRate)
      ├── _alarmService.CheckThreshold(data, threshold)
      └── _messageBus.Publish(new DataAvailableMessage { ... })
            └── Frm_Main.OnDataAvailable()   → 更新 MetricsPanel
```

---

## 15. 编译与部署

### 15.1 编译命令

```bash
# 正常编译 (VS / dotnet CLI 均可用)
dotnet build TOFD.sln

# 绕过 NuGet restore bug (.NET 10.0.300 SDK 特定问题)
dotnet build TOFD/TOFD.csproj --no-restore
```

### 15.2 已知问题

| 问题 | 原因 | 解决方案 |
|------|------|----------|
| .NET 10.0.300 SDK NuGet crash | `ProgramData` 环境变量缺失 → `Environment.GetFolderPath` 返 null | `dotnet build --no-restore` 或 `Directory.Build.props` 设 `RestoreRootConfigDirectory` |
| `Timer` 歧义编译错误 | `using System.Windows.Forms` 和 `using System.Threading` 冲突 | 完全限定 `System.Windows.Forms.Timer` |

### 15.3 项目统计

| 指标 | 数量 |
|------|------|
| 项目数 | 9 |
| 接口数 | 13 |
| 服务实现 | 11 |
| 适配器 | 7 路由 + 5 Simulated + 4 Real |
| Form | 8 在用 |
| UserControl | 18 |
| P/Invoke 函数 | 57 |
| 消息类型 | 21 |
| DTO/结构体/枚举 | ~100 |
| 源文件总计 | ~80 |

---

> **文档维护**: 随项目架构变更更新。最后一次同步: 2026-05-29
