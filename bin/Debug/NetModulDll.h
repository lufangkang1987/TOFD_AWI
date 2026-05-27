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

extern "C" _declspec(dllexport)	void	InitParam();			// 参数初始化
extern "C" _declspec(dllexport)	void	InitSendParam();		// 参数初始化硬件发码

extern "C" _declspec(dllexport)	void	SendCmdSampleStart(void);	// 开采样
extern "C" _declspec(dllexport)	void	SendCmdSampleClose(void);	// 关采样

extern "C" _declspec(dllexport) void	SendCmdCurrentChan(int iChan);	// 当前通道
extern "C" _declspec(dllexport) void	SendCmdServerClose(void);	// 关闭服务器

extern "C" _declspec(dllexport) void	SendCmdDB(int iData);		// 增益控制
extern "C" _declspec(dllexport) void	SendCmdFreqRatio(int iRange, int iSpeed);	// 范围声速-分频比
extern "C" _declspec(dllexport)	void	SendCmdZeroTime(int iData);	// 零偏
extern "C" _declspec(dllexport)	void	SendCmdParallel(int iData);	// 平移

extern "C" _declspec(dllexport) void	SendCmdPulWid(int iData);	// 脉冲宽度
extern "C" _declspec(dllexport) void	SendCmdWaveType(int iData);	// 检波方式
extern "C" _declspec(dllexport) void	SendCmdRepeatFreq(int iData);	// 重复频率
extern "C" _declspec(dllexport) void	SendCmdWorkMode(int iData);	// 工作模式

extern "C" _declspec(dllexport) void	SendCmdImpdance(int iData);	// 阻抗匹配
extern "C" _declspec(dllexport)	void	SendCmdHighVoltage(int iData);// 高压调节

extern "C" _declspec(dllexport)	void	SendCmdForword(int iData);	// 前放
extern "C" _declspec(dllexport)	void	InitEncoder(int iIndex,int iData);	// 编码器初始化

extern "C" _declspec(dllexport) void	SendCmdPulEnable(int iData);	// 脉冲使能
extern "C" _declspec(dllexport) void	SendCmdVoltEnable(int iData);	// 高压使能

extern "C" _declspec(dllexport) void    SendCmdGateStatus(int iSel,bool status);	//闸门是否显示
extern "C" _declspec(dllexport) void	SendCmdGateStart(int iSel, int iStart);		// 闸门起点
extern "C" _declspec(dllexport) void	SendCmdGateEnd(int iSel, int iEnd);			// 闸门终点
extern "C" _declspec(dllexport) void	SendCmdGateHigh(int iSel, int iHigh);		// 闸门高度

extern "C" _declspec(dllexport) float  getGateS(int iChan,int gate);//闸门内峰值声程
extern "C" _declspec(dllexport) float  getGateH(int iChan,int gate);//闸门内峰值高度

extern "C" _declspec(dllexport) void    GetRatioData(int iChan, U8 *pWaveDataBuf, U8 *pValueBuf, int *pTimeBuf);	//获取采样波形

extern "C" _declspec(dllexport) void    RealWave(int clientChan,int **pWaveDataBuf,int **pValueBuf,int **pTimeBuf);	//实时波形图

extern "C" _declspec(dllexport) unsigned int  getEncoderValue(int Client,int iIndex);//编码器值
