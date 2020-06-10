// Stephen Toub

using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace MsdnMag
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct RECT
    {
        public readonly int left;
        public readonly int top;
        public readonly int right;
        public readonly int bottom;
        public Rectangle ToRectangle() { return Rectangle.FromLTRB(left, top, right, bottom); }
    }
}
