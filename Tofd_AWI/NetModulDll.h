// 下列 ifdef 块是创建使从 DLL 导出更简单的
// 宏的标准方法。此 DLL 中的所有文件都是用命令行上定义的 NETMODULDLL_EXPORTS
// 符号编译的。在使用此 DLL 的
// 任何其他项目上不应定义此符号。这样，源文件中包含此文件的任何其他项目都会将
// extern "C" _declspec(dllexport) 函数视为是从 DLL 导入的，而此 DLL 则将用此宏定义的
// 符号视为是被导出的。
#pragma once
extern "C" _declspec(dllexport) BOOL  InitModule(ULONG ChannelSum, ULONG BoardSum);
extern "C" _declspec(dllexport) BOOL  CreateHostSocket(void);
extern "C" _declspec(dllexport) BOOL  GetConnectStatus(USHORT* pConnectNo);
extern "C" _declspec(dllexport) BOOL  ExitModule(void);

extern "C" _declspec(dllexport)	void	InitParam();					// 参数初始化
extern "C" _declspec(dllexport)	void	InitSendParam();				// 参数初始化硬件发码

extern "C" _declspec(dllexport)	void	SendCmdSampleStart(void);		// 开采样
extern "C" _declspec(dllexport)	void	SendCmdSampleClose(void);		// 关采样

extern "C" _declspec(dllexport) void	SendCmdCurrentChan(int iChan);	// 当前通道
extern "C" _declspec(dllexport) void	SendCmdServerClose(void);		// 关闭服务器

extern "C" _declspec(dllexport) void	SendCmdDB(U32 u32Data);			// 增益控制
extern "C" _declspec(dllexport) void	SendCmdFreqRatio(U32 u32Data);	// 范围-分频比
extern "C" _declspec(dllexport)	void	SendCmdZeroTime(U32 u32Data);	// 零偏
extern "C" _declspec(dllexport)	void	SendCmdParallel(U32 u32Data);	// 平移

extern "C" _declspec(dllexport) void	SendCmdPulWid(U32 u32Data);		// 脉冲宽度
extern "C" _declspec(dllexport) void	SendCmdWaveType(U32 u32Data);	// 检波方式
extern "C" _declspec(dllexport) void	SendCmdRepeatFreq(U32 u32Data);	// 重复频率
extern "C" _declspec(dllexport) void	SendCmdWorkMode(U32 u32Data);	// 工作模式

extern "C" _declspec(dllexport) void	SendCmdBandWidth(U32 u32Data);	// 带宽设置
extern "C" _declspec(dllexport) void	SendCmdImpdance(U32 u32Data);	// 阻抗匹配
extern "C" _declspec(dllexport)	void	SendCmdHighVoltage(U32 u32Data);// 高压调节
extern "C" _declspec(dllexport)	void	InitEncoder(U32 u32Data);		// 编码器初始化

extern "C" _declspec(dllexport) void    RealWave(int clientChan,int **pWaveDataBuf,int **pValueBuf,U32* pEncoderValue,U8* pBEncoderSignal);//实时波形图
