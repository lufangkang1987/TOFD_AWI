// ============================================================
// 文件: AppState.cs
// 命名空间: NewInspect.UI
// 职责: 全局共享状态 —— 连接状态、当前IP、事件通知
//       支持命令通道(51007)和数据通道(51005)独立状态跟踪
// ============================================================

using System;

namespace NewInspect.UI
{
    /// <summary>
    /// 通信通道类型
    /// </summary>
    public enum CommChannelType
    {
        Command,    // 命令通道 51007/JSON
        Data        // 数据通道 51005/二进制
    }

    /// <summary>
    /// 全局共享状态 —— 所有窗体读写同一份状态
    /// </summary>
    public static class AppState
    {
        // ===== 连接状态 =====
        private static bool _cmdConnected = false;
        private static bool _dataConnected = false;
        private static string _currentIp = "192.168.0.51";

        /// <summary>
        /// 仪器是否已连接（双通道均正常）
        /// 只读派生属性，由两个独立通道状态共同决定
        /// </summary>
        public static bool IsInstrumentConnected => _cmdConnected && _dataConnected;

        /// <summary>
        /// 命令通道(51007)是否已连接
        /// </summary>
        public static bool IsCommandChannelConnected => _cmdConnected;

        /// <summary>
        /// 数据通道(51005)是否已连接
        /// </summary>
        public static bool IsDataChannelConnected => _dataConnected;

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
        /// 连接状态改变时触发（参数：新的整体连接状态）
        /// 当命令通道和数据通道的组合结果变化时触发
        /// </summary>
        public static event EventHandler<bool> ConnectionStateChanged;

        /// <summary>
        /// 通道状态改变时触发（参数：通道类型, 新的连接状态）
        /// 用于UI独立显示命令通道和数据通道的状态
        /// </summary>
        public static event EventHandler<(CommChannelType Channel, bool Connected)> ChannelStateChanged;

        // ===== 便捷方法 =====

        /// <summary>
        /// 设置指定通道的连接状态，内部触发相应事件
        /// </summary>
        public static void SetChannelState(CommChannelType channel, bool connected)
        {
            bool oldOverall = IsInstrumentConnected;

            switch (channel)
            {
                case CommChannelType.Command:
                    if (_cmdConnected == connected) return;
                    _cmdConnected = connected;
                    break;
                case CommChannelType.Data:
                    if (_dataConnected == connected) return;
                    _dataConnected = connected;
                    break;
            }

            // 触发通道级别事件
            ChannelStateChanged?.Invoke(null, (channel, connected));

            // 如果整体状态发生变化，触发整体事件
            bool newOverall = IsInstrumentConnected;
            if (oldOverall != newOverall)
            {
                ConnectionStateChanged?.Invoke(null, newOverall);
            }
        }

        /// <summary>
        /// 标记为已连接（双通道均设为已连接），并触发事件
        /// </summary>
        public static void SetConnected(string ip)
        {
            CurrentIp = ip;
            SetChannelState(CommChannelType.Command, true);
            SetChannelState(CommChannelType.Data, true);
        }

        /// <summary>
        /// 标记为断开（双通道均设为断开），并触发事件
        /// </summary>
        public static void SetDisconnected()
        {
            SetChannelState(CommChannelType.Command, false);
            SetChannelState(CommChannelType.Data, false);
        }
    }
}
