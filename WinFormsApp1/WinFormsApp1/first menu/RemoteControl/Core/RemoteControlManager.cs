using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WinFormsApp1.first_menu.RemoteControl
{
    /// <summary>
    /// 远程控制管理器 - 处理鼠标和键盘控制
    /// </summary>
    public class RemoteControlManager
    {
        // 鼠标事件标志
        private const int MOUSEEVENTF_MOVE = 0x0001;
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const int MOUSEEVENTF_LEFTUP = 0x0004;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const int MOUSEEVENTF_RIGHTUP = 0x0010;
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        private const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        private const int MOUSEEVENTF_WHEEL = 0x0800;
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;

        // 键盘事件标志
        private const int KEYEVENTF_KEYDOWN = 0x0000;
        private const int KEYEVENTF_KEYUP = 0x0002;
        private const int KEYEVENTF_UNICODE = 0x0004;
        private const int KEYEVENTF_SCANCODE = 0x0008;

        /// <summary>
        /// 移动鼠标到指定位置
        /// </summary>
        public static void MoveMouse(Point position)
        {
            WinAPI.SetCursorPos(position.X, position.Y);
        }

        /// <summary>
        /// 模拟鼠标按下
        /// </summary>
        public static void MouseDown(Point position, MouseButtons button)
        {
            WinAPI.SetCursorPos(position.X, position.Y);
            
            int flags = 0;
            switch (button)
            {
                case MouseButtons.Left:
                    flags = MOUSEEVENTF_LEFTDOWN;
                    break;
                case MouseButtons.Right:
                    flags = MOUSEEVENTF_RIGHTDOWN;
                    break;
                case MouseButtons.Middle:
                    flags = MOUSEEVENTF_MIDDLEDOWN;
                    break;
            }
            
            WinAPI.mouse_event(flags, position.X, position.Y, 0, 0);
        }

        /// <summary>
        /// 模拟鼠标弹起
        /// </summary>
        public static void MouseUp(Point position, MouseButtons button)
        {
            WinAPI.SetCursorPos(position.X, position.Y);
            
            int flags = 0;
            switch (button)
            {
                case MouseButtons.Left:
                    flags = MOUSEEVENTF_LEFTUP;
                    break;
                case MouseButtons.Right:
                    flags = MOUSEEVENTF_RIGHTUP;
                    break;
                case MouseButtons.Middle:
                    flags = MOUSEEVENTF_MIDDLEUP;
                    break;
            }
            
            WinAPI.mouse_event(flags, position.X, position.Y, 0, 0);
        }

        /// <summary>
        /// 模拟鼠标点击
        /// </summary>
        public static void MouseClick(Point position, MouseButtons button)
        {
            MouseDown(position, button);
            System.Threading.Thread.Sleep(50);
            MouseUp(position, button);
        }

        /// <summary>
        /// 模拟鼠标双击
        /// </summary>
        public static void MouseDoubleClick(Point position, MouseButtons button)
        {
            MouseClick(position, button);
            System.Threading.Thread.Sleep(50);
            MouseClick(position, button);
        }

        /// <summary>
        /// 模拟鼠标滚轮
        /// </summary>
        public static void MouseWheel(int delta)
        {
            WinAPI.mouse_event(MOUSEEVENTF_WHEEL, 0, 0, delta, 0);
        }

        /// <summary>
        /// 模拟按键按下
        /// </summary>
        public static void KeyDown(Keys key)
        {
            byte virtualKey = (byte)key;
            byte scanCode = (byte)WinAPI.MapVirtualKey(virtualKey, 0);
            WinAPI.keybd_event(virtualKey, scanCode, KEYEVENTF_KEYDOWN, 0);
        }

        /// <summary>
        /// 模拟按键弹起
        /// </summary>
        public static void KeyUp(Keys key)
        {
            byte virtualKey = (byte)key;
            byte scanCode = (byte)WinAPI.MapVirtualKey(virtualKey, 0);
            WinAPI.keybd_event(virtualKey, scanCode, KEYEVENTF_KEYUP, 0);
        }

        /// <summary>
        /// 模拟按键
        /// </summary>
        public static void KeyPress(Keys key)
        {
            KeyDown(key);
            System.Threading.Thread.Sleep(50);
            KeyUp(key);
        }

        /// <summary>
        /// 发送文本
        /// </summary>
        public static void SendText(string text)
        {
            SendKeys.SendWait(text);
        }

        /// <summary>
        /// 模拟组合键
        /// </summary>
        public static void SendKeyCombination(params Keys[] keys)
        {
            // 按下所有键
            foreach (var key in keys)
            {
                KeyDown(key);
                System.Threading.Thread.Sleep(10);
            }
            
            // 释放所有键（逆序）
            for (int i = keys.Length - 1; i >= 0; i--)
            {
                KeyUp(keys[i]);
                System.Threading.Thread.Sleep(10);
            }
        }

        /// <summary>
        /// 获取屏幕坐标的相对位置（0-65535）
        /// </summary>
        private static Point GetAbsolutePosition(Point position)
        {
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            
            int absoluteX = (position.X * 65535) / screenWidth;
            int absoluteY = (position.Y * 65535) / screenHeight;
            
            return new Point(absoluteX, absoluteY);
        }

        /// <summary>
        /// 模拟拖拽操作
        /// </summary>
        public static void DragDrop(Point startPosition, Point endPosition, MouseButtons button = MouseButtons.Left)
        {
            MoveMouse(startPosition);
            System.Threading.Thread.Sleep(100);
            MouseDown(startPosition, button);
            System.Threading.Thread.Sleep(100);
            
            // 平滑移动
            int steps = 10;
            int deltaX = (endPosition.X - startPosition.X) / steps;
            int deltaY = (endPosition.Y - startPosition.Y) / steps;
            
            for (int i = 1; i <= steps; i++)
            {
                Point currentPos = new Point(
                    startPosition.X + (deltaX * i),
                    startPosition.Y + (deltaY * i)
                );
                MoveMouse(currentPos);
                System.Threading.Thread.Sleep(20);
            }
            
            MouseUp(endPosition, button);
        }

        /// <summary>
        /// 锁定输入（禁用本地鼠标和键盘）
        /// </summary>
        public static bool BlockInput(bool block)
        {
            return WinAPI.BlockInput(block);
        }

        /// <summary>
        /// 获取当前鼠标位置
        /// </summary>
        public static Point GetCursorPosition()
        {
            return Cursor.Position;
        }

        /// <summary>
        /// 判断按键是否按下
        /// </summary>
        public static bool IsKeyPressed(Keys key)
        {
            return (WinAPI.GetAsyncKeyState((int)key) & 0x8000) != 0;
        }
    }
}
