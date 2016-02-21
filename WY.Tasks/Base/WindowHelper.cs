using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interop;

namespace WY.Tasks.Base {
    internal class WindowHelper {
        Window target;

        public Window Target {
            get { return target; }
            set { target = value; }
        }

        public WindowHelper()
            : this(null) {
        }

        public WindowHelper(Window window) {
            target = window;
        }
        //修复行为
        public void RepairBehavior() {
            if (target == null)
                return;

            this.target.SourceInitialized += delegate {
                IntPtr handle = (new WindowInteropHelper(target)).Handle;
                HwndSource hwndSource = HwndSource.FromHwnd(handle);
                if (hwndSource != null) {
                    hwndSource.AddHook(WindowProc);
                }
            };
        }

        //消息循环
        private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
            Console.Write(msg);
            return IntPtr.Zero;
        }
    }
}
