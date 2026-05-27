using ClassLibrary_Interface;
using NewInspect.Services.Adapters;
using System;
using System.Threading;
using System.Drawing;

// ============================================================
// 文件: CameraService.cs
// 位置: NewInspect.Services/Services/
// 命名空间: NewInspect.Services
// 职责: 相机管理 — 多路相机切换、视频流控制
//       替代 Frm_Main_C.cs 中相机相关代码
// ============================================================

namespace NewInspect.Services
{
    public class CameraService
    {
        // 最多 4 路相机 (前后左右)
        private readonly ICameraDevice[] _cameras = new ICameraDevice[4];
        private int _activeCameraIndex = 0;
        private bool _isStreaming;

        /// <summary>当前活动帧</summary>
        public Bitmap CurrentFrame { get; private set; }

        /// <summary>帧更新事件 (供 UI 绑定)</summary>
        public event Action<Bitmap> FrameUpdated;

        public CameraService()
        {
            for (int i = 0; i < 4; i++)
            {
                _cameras[i] = new CameraAdapter();
            }
        }

        /// <summary>打开第 N 路相机</summary>
        public bool OpenCamera(int index, string ipAddress)
        {
            if (index < 0 || index >= 4) return false;
            return _cameras[index].Open(ipAddress);
        }

        /// <summary>关闭第 N 路相机</summary>
        public void CloseCamera(int index)
        {
            if (index < 0 || index >= 4) return;
            _cameras[index].Close();
        }

        /// <summary>切换到第 N 路相机</summary>
        public void SwitchCamera(int index)
        {
            if (index < 0 || index >= 4) return;
            _activeCameraIndex = index;
        }

        /// <summary>切换到大画面/小画面</summary>
        public void ToggleViewSize()
        {
            // 对应原 m_bl_Big 切换逻辑
        }

        /// <summary>抓拍当前画面</summary>
        public Bitmap Capture()
        {
            CurrentFrame = _cameras[_activeCameraIndex].Capture();
            return CurrentFrame;
        }

        /// <summary>开始视频流</summary>
        public void StartStream()
        {
            if (_isStreaming) return;
            _isStreaming = true;

            var cam = _cameras[_activeCameraIndex];
            var thread = new Thread(() =>
            {
                while (_isStreaming)
                {
                    try
                    {
                        var frame = cam.Capture();
                        if (frame != null)
                        {
                            CurrentFrame = frame;
                            FrameUpdated?.Invoke(frame);
                        }
                        Thread.Sleep(33); // 30fps
                    }
                    catch { break; }
                }
            })
            { IsBackground = true };
            thread.Start();
        }

        /// <summary>停止视频流</summary>
        public void StopStream()
        {
            _isStreaming = false;
        }

        /// <summary>释放所有相机</summary>
        public void Dispose()
        {
            _isStreaming = false;
            foreach (var cam in _cameras)
            {
                cam?.Close();
            }
        }
    }
}
