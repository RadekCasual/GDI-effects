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

        // Import necessary WinAPI functions
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

        // Define the POINT structure for PlgBlt
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        
        public static void Main()
        {
            Random r;
            // Get the screen width and height
            int x = Screen.PrimaryScreen.Bounds.Width, y = Screen.PrimaryScreen.Bounds.Height;
            int left = Screen.PrimaryScreen.Bounds.Left, right = Screen.PrimaryScreen.Bounds.Right, 
                top = Screen.PrimaryScreen.Bounds.Top, bottom = Screen.PrimaryScreen.Bounds.Bottom;
            uint[] rndclr = { 0xF0FFFF };
            // Create an array for the points that will define the screen polygon
            POINT[] lppoint = new POINT[3];
            // Infinite loop to create the tunnel effect
            while (true)
            {
                r = new Random();

                IntPtr hdc = GetDC(IntPtr.Zero); // Get the device context for the screen
                IntPtr mhdc = CreateCompatibleDC(hdc); // Create a memory device context
                IntPtr hbit = CreateCompatibleBitmap(hdc, x, y); // Create a compatible bitmap
                IntPtr holdbit = SelectObject(mhdc, hbit); // Select the bitmap into the memory DC
                // Set the points for the tunnel effect
                lppoint[0].X = (left + 50) + 0;
                lppoint[0].Y = (top - 50) + 0;
                lppoint[1].X = (right + 50) + 0;
                lppoint[1].Y = (top + 50) + 0;
                lppoint[2].X = (left - 50) + 0;
                lppoint[2].Y = (bottom - 50) + 0;
                // Use PlgBlt to draw the tunnel effect
                PlgBlt(hdc, lppoint, hdc, left - 20, top - 20, (right - left) + 40, (bottom - top) + 40, 
                    IntPtr.Zero, 0, 0);
                // Clean up
                DeleteDC(hdc);
                // Sleep for 50 milliseconds (this value can be changed to any time)
                Thread.Sleep(50);

            }
        }
    }
}
