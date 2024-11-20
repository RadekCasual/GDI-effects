using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

public class inverted_colors
{
    // Constants for GDI functions
    const int SRCCOPY = 0x00CC0020;
    const int PATINVERT = 0x005A0049;

    // Import GDI functions
    [DllImport("user32.dll")]
    public static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDc);

    [DllImport("gdi32.dll")]
    public static extern IntPtr CreateSolidBrush(uint crColor);

    [DllImport("gdi32.dll")]
    public static extern int SelectObject(IntPtr hdc, IntPtr h);

    [DllImport("gdi32.dll")]
    public static extern bool PatBlt(IntPtr hdc, int x, int y, int w, int h, int rop);

    [DllImport("gdi32.dll")]
    public static extern bool DeleteObject(IntPtr hObject);

    [DllImport("gdi32.dll")]
    public static extern bool DeleteDC(IntPtr hdc);

    public static void Main()
    {
        //Random for generating effects
        Random r = new Random();
        int x = Screen.PrimaryScreen.Bounds.Width, y = Screen.PrimaryScreen.Bounds.Height;
        uint[] rndclr = { 0xF0FFFF }; 

        while (true)
        {
            // Set up the device context
            IntPtr hdc = GetDC(IntPtr.Zero); 
            IntPtr brush = CreateSolidBrush(rndclr[r.Next(rndclr.Length)]); 
            SelectObject(hdc, brush); 

            // Perform a drawing operation (invert the screen)
            PatBlt(hdc, 0, 0, x, y, PATINVERT);

            // Clean up resources
            DeleteObject(brush);
            DeleteDC(hdc);

            // Delay to control animation speed (this value can be changed to any time)
            Thread.Sleep(500);
        }
    }
}
