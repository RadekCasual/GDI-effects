using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace radial_blur
{
    public class radial_blur
    {
        // Import GDI functions
        [DllImport("gdi32.dll")]
        static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        [DllImport("gdi32.dll")]
        static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);
        [DllImport("gdi32.dll")]
        static extern IntPtr SelectObject(IntPtr hdc, IntPtr h);
        [DllImport("gdi32.dll")]
        static extern bool DeleteObject(IntPtr hObject);
        [DllImport("gdi32.dll")]
        static extern bool DeleteDC(IntPtr hdc);
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern int ReleaseDC(IntPtr hWnd, IntPtr hdc);
        [DllImport("gdi32.dll")]
        static extern bool PlgBlt(IntPtr hdcDest, POINT[] lpPoint, IntPtr hdcSrc, int xSrc, int ySrc, int width, int height, IntPtr hbmMask, int xMask, int yMask);
        [DllImport("msimg32.dll")]
        static extern bool AlphaBlend(IntPtr hdcDest, int xOriginDest, int yOriginDest, int widthDest, int heightDest, IntPtr hdcSrc, int xOriginSrc, int yOriginSrc, int widthSrc, int heightSrc, BLENDFUNCTION blendFunction);

        // Structures for GDI functions
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;

            public BLENDFUNCTION(byte op, byte flags, byte alpha, byte format)
            {
                BlendOp = op;
                BlendFlags = flags;
                SourceConstantAlpha = alpha;
                AlphaFormat = format;
            }

            public static void Main()
            {
                // Random for generating effects
                Random r;
                int x = Screen.PrimaryScreen.Bounds.Width, y = Screen.PrimaryScreen.Bounds.Height;
                int left = Screen.PrimaryScreen.Bounds.Left, right = Screen.PrimaryScreen.Bounds.Right,
                    top = Screen.PrimaryScreen.Bounds.Top, bottom = Screen.PrimaryScreen.Bounds.Bottom;
                uint[] rndclr = { 0xFF0000, 0xFF00BC, 0x00FF33, 0xFFF700, 0x00FFEF };
                POINT[] lppoint = new POINT[3];
                while (true)
                {
                    r = new Random();
                    IntPtr hdc = GetDC(IntPtr.Zero);
                    IntPtr mhdc = CreateCompatibleDC(hdc);
                    IntPtr hbit = CreateCompatibleBitmap(hdc, x, y);
                    IntPtr holdbit = SelectObject(mhdc, hbit);
                    // Randomize the triangle points for `PlgBlt`
                    if (r.Next(2) == 1)
                    {
                        lppoint[0].X = (left + 30) + 0;
                        lppoint[0].Y = (top - 30) + 0;
                        lppoint[1].X = (right + 30) + 0;
                        lppoint[1].Y = (top + 30) + 0;
                        lppoint[2].X = (left - 30) + 0;
                        lppoint[2].Y = (bottom - 30) + 0;
                    }
                    else
                    {
                        lppoint[0].X = (left - 30) + 0;
                        lppoint[0].Y = (top + 30) + 0;
                        lppoint[1].X = (right - 30) + 0;
                        lppoint[1].Y = (top - 30) + 0;
                        lppoint[2].X = (left + 30) + 0;
                        lppoint[2].Y = (bottom + 30) + 0;
                    }
                    // Perform GDI operations
                    PlgBlt(mhdc, lppoint, hdc, left, top, (right - left), (bottom - top), IntPtr.Zero, 0, 0);
                    AlphaBlend(hdc, 0, 0, x, y, mhdc, 0, 0, x, y, new BLENDFUNCTION(0, 0, 50, 0));
                    // Cleanup GDI objects
                    SelectObject(mhdc, holdbit);
                    DeleteObject(holdbit);
                    DeleteObject(hbit);
                    DeleteDC(mhdc);
                    DeleteDC(hdc);
                    // Delay to control animation speed (this value can be changed to any time)
                    Thread.Sleep(30);

                }
            }
        }
    }
}
    


