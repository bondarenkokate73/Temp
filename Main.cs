using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Temp
{
    public partial class Main : Form
    {
        static readonly Random random = new Random();
        public Main()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await clickLeftOnPointAsync(new Point(900, 200));
            await clickLeftOnPointAsync(new Point(1000, 300));
            await clickLeftOnPointAsync(new Point(1100, 400));
        }

        public static Task Wait()
        {
            int sleep = random.Next(300, 600);
            return Task.Delay(sleep);
        }

        public async Task clickLeftOnPointAsync(Point point)
        {
            if (!point.IsEmpty)
            {
                int x = Convert.ToInt32(point.X / 1.25);
                int y = Convert.ToInt32(point.Y / 1.25);

                Point offsetPoint = new Point(x + 5, y + 5);

                await ClickLeftMouseButtonAsync(offsetPoint.X, offsetPoint.Y);
                await Wait();
            }
        }

        public static async Task ClickLeftMouseButtonAsync(int x, int y)
        {
            x = (x * 65536) / WinApi.GetSystemMetrics(WinApi.SystemMetric.SM_CXSCREEN);
            y = (y * 65536) / WinApi.GetSystemMetrics(WinApi.SystemMetric.SM_CYSCREEN);

            WinApi.INPUT[] inputs = new WinApi.INPUT[2];

            inputs[0] = new WinApi.INPUT
            {
                type = WinApi.SendInputEventType.InputMouse,
                mkhi = new WinApi.MouseKeybdhardwareInputUnion
                {
                    mi = new WinApi.MouseInputData
                    {
                        dx = x,
                        dy = y,
                        dwFlags = WinApi.MouseEventFlags.MOUSEEVENTF_MOVE | WinApi.MouseEventFlags.MOUSEEVENTF_ABSOLUTE
                    }
                }
            };

            inputs[1] = new WinApi.INPUT
            {
                type = WinApi.SendInputEventType.InputMouse,
                mkhi = new WinApi.MouseKeybdhardwareInputUnion
                {
                    mi = new WinApi.MouseInputData
                    {
                        dx = x,
                        dy = y,
                        dwFlags = WinApi.MouseEventFlags.MOUSEEVENTF_LEFTDOWN
                    }
                }
            };
            WinApi.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(WinApi.INPUT)));
            await Wait();

            WinApi.INPUT iNPUT = new WinApi.INPUT
            {
                type = WinApi.SendInputEventType.InputMouse,
                mkhi = new WinApi.MouseKeybdhardwareInputUnion
                {
                    mi = new WinApi.MouseInputData
                    {
                        dx = x,
                        dy = y,
                        dwFlags = WinApi.MouseEventFlags.MOUSEEVENTF_LEFTUP
                    }
                }
            };
            WinApi.SendInput(1, ref iNPUT, Marshal.SizeOf(typeof(WinApi.INPUT)));
        }
    }
}