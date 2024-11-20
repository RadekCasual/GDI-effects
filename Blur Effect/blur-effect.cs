using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

class blur_effect
{
    // Import GDI functions
    const uint SRCCOPY = 0x00CC0020;

    [DllImport("user32.dll")]
    public static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("gdi32.dll")]
    public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

    [DllImport("gdi32.dll")]
    public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int width, int height);

    [DllImport("gdi32.dll")]
    public static extern IntPtr SelectObject(IntPtr hdc, IntPtr obj);

    [DllImport("gdi32.dll")]
    public static extern bool DeleteObject(IntPtr hObject);

    [DllImport("gdi32.dll")]
    public static extern bool DeleteDC(IntPtr hdc);

    [DllImport("gdi32.dll")]
    public static extern bool BitBlt(IntPtr hdcDest, int xDest, int yDest, int width, int height, IntPtr hdcSrc, int xSrc, int ySrc, uint rop);

    [DllImport("msimg32.dll")]
    public static extern bool AlphaBlend(IntPtr hdcDest, int xDest, int yDest, int width, int height, IntPtr hdcSrc, int xSrc, int ySrc, int srcWidth, int srcHeight, BLENDFUNCTION blend);

    // Structures for GDI functions
    [StructLayout(LayoutKind.Sequential)]
    public struct BLENDFUNCTION
    {
        public byte BlendOp;
        public byte BlendFlags;
        public byte SourceConstantAlpha;
        public byte AlphaFormat;
    }

    static void Main()
    {
        // Random for generating effects
        Random r = new Random();
        int x = Screen.PrimaryScreen.Bounds.Width;
        int y = Screen.PrimaryScreen.Bounds.Height;

        uint[] rndclr = { 0xF0FFFF };  

        while (true)
        {
            // Set up the device context
            IntPtr hdc = GetDC(IntPtr.Zero); // Get the screen DC
            IntPtr mhdc = CreateCompatibleDC(hdc);
            IntPtr hbit = CreateCompatibleBitmap(hdc, x, y); // Create a bitmap
            IntPtr holdbit = SelectObject(mhdc, hbit);

            // Copy screen into memory device context
            BitBlt(mhdc, 0, 0, x, y, hdc, 0, 0, SRCCOPY);

            // Perform AlphaBlend operation for a blur effect
            AlphaBlend(hdc, r.Next(-4, 4), r.Next(-4, 4), x, y, mhdc, 0, 0, x, y, new BLENDFUNCTION { SourceConstantAlpha = 70 });

            // Cleanup GDI objects
            SelectObject(mhdc, holdbit);
            DeleteObject(holdbit);
            DeleteObject(hbit);
            DeleteDC(mhdc);
            DeleteDC(hdc);

            // Delay for 50 milliseconds (this value can be changed to any time)
            Thread.Sleep(50);
        }
    }
}
