#pragma once
#pragma warning( disable: 4819 )

#include <stdint.h>
#include <stdbool.h>

#include <stdio.h>

//#include  "targetver.h"
//#include  "stdafx.h"
//
//#include  "hidapi.h"


#include <cstdlib>

//#include <highgui.h>

//
//using namespace System;
//using namespace System::Drawing;

#include<g:\opencv\build\include\opencv2\core.hpp>  
#include<g:\opencv\build\include\opencv2\highgui.hpp>  
#include<g:\opencv\build\include\opencv2\imgproc.hpp>
#include<g:\opencv\build\include\opencv2\opencv.hpp>
//class Delegate
//{
//public:
//	virtual ~Delegate(void) { }
//	virtual void operator()(void) = 0;
//};
//template<typename C>
//class DelegateImpl : public Delegate
//{
//private:
//	typedef void (C::*F)(void);
//	C* m_class;
//	F m_fun;
//public:
//	DelegateImpl(C* c, F f) { m_class = c; m_fun = f; }
//	virtual void operator()(void)
//	{
//		if (m_class)
//			return (m_class->*m_fun)();
//	}
//};

//_CRT_SECURE_NO_WARNINGS
//图像是否翻转  1：翻转 0：不翻转 / 相机类型0：正视 1：侧视 /  云图截取长宽尺寸，用于1280 * 720 分辨率    校正方法  是否保存数据
extern "C" _declspec(dllexport) void _stdcall   Change_Cam(int iType, int i_CamType, int i_Pic_Cut_Wh,int iMethod,int iSave);
//设置系统运行路径
extern "C" _declspec(dllexport) void _stdcall   SetPath(char * strPath);
//相机初始化  相机分辨率  注册表路径SOFTWARE\\ZzjrDLMainKey\\DE_DLSYS   注册表主键对应值
extern "C" _declspec(dllexport) int _stdcall  pair_image_capture(int camera_w, int camare_h, char * Str_In_Key_Value);//
extern "C" _declspec(dllexport) void  _stdcall  Img_SetFanSe(int i_FanSe);//是否反色 0：不 1：反色
extern "C" _declspec(dllexport) void _stdcall  Img_UpDown(double db_UpDownBs);//放大、缩小倍数 0.1 0.2...1 1.1 1.2 ...
//视频
extern "C" _declspec(dllexport) int _stdcall Video(unsigned char * imgbuffer1);//, unsigned char * imgbuffer2);
//视频牌照，然后匹配计算深度
extern "C" _declspec(dllexport) int _stdcall Video_Run(unsigned char * imgbuffer1, unsigned char * imgbuffer2,
	                                                   float  Out_Depth_Y[], float  Out_Depth_Z[], float Out_Depth_xx[]);
//设置工作状态
extern "C" _declspec(dllexport) void _stdcall  Set_Flag(int iType);//_stdcall
//拿外部文件，然后匹配计算深度
extern "C" _declspec(dllexport) int _stdcall StereoMatch( char * PathFile_L,   char * PathFile_R,
	                                                     float  Out_Depth_Y[], float  Out_Depth_Z[],  float Out_Depth_xx[]);
//寻找角点 角点图片路径，横向点个数，竖向点个数，输出XY坐标
extern "C" _declspec(dllexport) int _stdcall FindCornint( int In_boardWidth, int In_boardHeight,
                                                      	float  Out_Corner_X[], float  Out_Corner_Y[]);
//根据多个点拟合光平面  解奇异方程组 AX+BY+CZ+D=0
extern "C" _declspec(dllexport)  void _stdcall  GetHight(int iArrLen, double point_X[], double point_Y[], double point_Z[], float Out_ABCD[]);