using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AeroLike; 

public class ScreenResolution {
    [DllImport("gdi32.dll")]
    private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

    [DllImport("user32.dll")]
    private static extern IntPtr GetDC(IntPtr ptr);

    public static int Height { get => GetDeviceCaps(GetDC(IntPtr.Zero), 117/*DESKTOPVERTRES*/); }
    public static int Width { get => GetDeviceCaps(GetDC(IntPtr.Zero), 118/*DESKTOPVERTRES*/); }
}
