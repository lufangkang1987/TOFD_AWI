// ============================================================
// 文件: AppState.cs
// 命名空间: NewInspect.UI
// 职责: 全局共享状态 —— 连接状态、当前IP、事件通知
// ============================================================

using System;

namespace NewInspect.UI
{
    /// <summary>
    /// 全局共享状态 —— 所有窗体读写同一份状态
    /// </summary>
    public static class AppState
    {
        // ===== 连接状态 =====
        private static bool _isInstrumentConnected = false;
        private static string _currentIp = "192.168.0.51";

        /// <summary>
        /// 仪器是否已连接（双通道均正常）
        /// </summary>
        public static bool IsInstrumentConnected
        {
            get => _isInstrumentConnected;
            set
            {
                if (_isInstrumentConnected != value)
                {
                    _isInstrumentConnected = value;
                    ConnectionStateChanged?.Invoke(null, value);
                }
            }
        }

        /// <summary>
        /// 当前连接的仪器 IP
        /// </summary>
        public static string CurrentIp
        {
            get => _currentIp;
            set
            {
                if (_currentIp != value)
                {
                    _currentIp = value;
                }
            }
        }

        /// <summary>
        /// 连接状态改变时触发（参数：新的连接状态）
        /// Frm_NewInspect 订阅此事件来更新顶栏状态指示
        /// </summary>
        public static event EventHandler<bool> ConnectionStateChanged;

        // ===== 便捷方法 =====

        /// <summary>
        /// 标记为已连接，并触发事件
        /// </summary>
        public static void SetConnected(string ip)
        {
            CurrentIp = ip;
            IsInstrumentConnected = true;
        }

        /// <summary>
        /// 标记为断开，并触发事件
        /// </summary>
        public static void SetDisconnected()
        {
            IsInstrumentConnected = false;
        }
    }
}
