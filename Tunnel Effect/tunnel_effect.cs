using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace tunnel_effect
{
    public class tunnel_effect
    {

        // Import GDI functions
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr obj);

        [DllImport("gdi32.dll")]
        public static extern bool PlgBlt(IntPtr hdcDest, POINT[] lpPoints, IntPtr hdcSrc, int nXSrc, int nYSrc, int nWidth, int nHeight, IntPtr hbmMask, int xMask, int yMask);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hdc);

        // Structures for GDI functions
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        
        public static void Main()
        {
            // Random for generating effects
            Random r;
            int x = Screen.PrimaryScreen.Bounds.Width, y = Screen.PrimaryScreen.Bounds.Height;
            int left = Screen.PrimaryScreen.Bounds.Left, right = Screen.PrimaryScreen.Bounds.Right, 
                top = Screen.PrimaryScreen.Bounds.Top, bottom = Screen.PrimaryScreen.Bounds.Bottom;
            uint[] rndclr = { 0xF0FFFF };
            POINT[] lppoint = new POINT[3];
            while (true)
            {
                r = new Random();
            // Set up the device context
                IntPtr hdc = GetDC(IntPtr.Zero); 
                IntPtr mhdc = CreateCompatibleDC(hdc); 
                IntPtr hbit = CreateCompatibleBitmap(hdc, x, y); 
                IntPtr holdbit = SelectObject(mhdc, hbit); 
                // Randomize the points for `PlgBlt`
                lppoint[0].X = (left + 50) + 0;
                lppoint[0].Y = (top - 50) + 0;
                lppoint[1].X = (right + 50) + 0;
                lppoint[1].Y = (top + 50) + 0;
                lppoint[2].X = (left - 50) + 0;
                lppoint[2].Y = (bottom - 50) + 0;
                // Perform GDI operations
                PlgBlt(hdc, lppoint, hdc, left - 20, top - 20, (right - left) + 40, (bottom - top) + 40, 
                    IntPtr.Zero, 0, 0);
                // Cleanup GDI objects
                DeleteDC(hdc);
                // Delay to control animation speed (this value can be changed to any time)
                Thread.Sleep(50);

            }
        }
    }
}
