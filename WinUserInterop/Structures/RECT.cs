using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Win32
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The RECT structure defines a rectangle by the coordinates of its upper-left and lower-right corners.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        /// <summary>
        ///  Specifies the x-coordinate of the upper-left corner of the rectangle.
        /// </summary>
        public int Left;

        /// <summary>
        ///  Specifies the y-coordinate of the upper-left corner of the rectangle.
        /// </summary>
        public int Top;

        /// <summary>
        ///  Specifies the x-coordinate of the lower-right corner of the rectangle.
        /// </summary>
        public int Right;

        /// <summary>
        ///  Specifies the y-coordinate of the lower-right corner of the rectangle.
        /// </summary>
        public int Bottom;


        /// <summary>
        ///  Get/Set Height.
        /// </summary>
        public int Height
        {
            get { return Bottom - Top; }
            set { Bottom = value + Top; }
        }

        /// <summary>
        ///  Get/Set Location.
        /// </summary>
        public Point Location
        {
            get { return new Point(Left, Top); }
            set { X = value.X; Y = value.Y; }
        }

        /// <summary>
        ///  Get/Set Size.
        /// </summary>
        public Size Size
        {
            get { return new Size(Width, Height); }
            set { Width = value.Width; Height = value.Height; }
        }

        /// <summary>
        ///  Get/Set Width.
        /// </summary>
        public int Width
        {
            get { return Right - Left; }
            set { Right = value + Left; }
        }

        /// <summary>
        ///  Get/Set x coordinate.
        /// </summary>
        public int X
        {
            get { return Left; }
            set { Right -= (Left - value); Left = value; }
        }

        /// <summary>
        ///  Get/Set y coordinate.
        /// </summary>
        public int Y
        {
            get { return Top; }
            set { Bottom -= (Top - value); Top = value; }
        }

        private const int BIG = Int32.MaxValue;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Constructor: For RECT.
        /// </summary>
        /// <param name="r">  The Rectangle r.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public RECT(Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom)
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Constructor: For RECT, default is the maximum box size.
        /// </summary>
        /// <param name="left">    The left.</param>
        /// <param name="top">     The top.</param>
        /// <param name="right">   The right.</param>
        /// <param name="bottom">  The bottom.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public RECT(int left=BIG, int top = BIG, int right = BIG, int bottom = BIG)
        {
            Left   = left;
            Top    = top;
            Right  = right;
            Bottom = bottom;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: From XYWH.
        /// </summary>
        /// <param name="x">       The integer x coordinate.</param>
        /// <param name="y">       The integer y coordinate.</param>
        /// <param name="width">   The width.</param>
        /// <param name="height">  The height.</param>
        /// <returns>
        ///  The Win32.RECT value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static RECT FromXYWH(int x, int y, int width, int height)
        {
            return new RECT(x, y, x + width, y + height);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Operator Overload: .
        /// </summary>
        /// <param name="r">  The rectangle r.</param>
        /// <returns>
        ///  The System.Drawing.Rectangle result of the System.Drawing.Rectangle operation
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static implicit operator Rectangle(RECT r)
        {
            return new Rectangle(r.Left, r.Top, r.Width, r.Height);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Operator Overload: .
        /// </summary>
        /// <param name="r">  The Rectangle r.</param>
        /// <returns>
        ///  The Win32.RECT result of the rectangle operation
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static implicit operator RECT(Rectangle r)
        {
            return new RECT(r);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Operator Overload: Inequality operator.
        /// </summary>
        /// <param name="r1">  The r1.</param>
        /// <param name="r2">  The r2.</param>
        /// <returns>
        ///  The bool result of the != operation
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static bool operator !=(RECT r1, RECT r2)
        {
            return !r1.Equals(r2);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Operator Overload: Equality operator.
        /// </summary>
        /// <param name="r1">  The r1.</param>
        /// <param name="r2">  The r2.</param>
        /// <returns>
        ///  The bool result of the == operation
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static bool operator ==(RECT r1, RECT r2)
        {
            return r1.Equals(r2);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Equals.
        /// </summary>
        /// <param name="obj">  The object.</param>
        /// <returns>
        ///  True if successful
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override bool Equals(object obj)
        {
            return obj is RECT ? Equals((RECT) obj) : obj is Rectangle && Equals(new RECT((Rectangle) obj));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Equals.
        /// </summary>
        /// <param name="r">  The rectangle r.</param>
        /// <returns>
        ///  True if successful
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool Equals(RECT r)
        {
            return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Equals.
        /// </summary>
        /// <param name="left">    The left.</param>
        /// <param name="top">     The top.</param>
        /// <param name="right">   The right.</param>
        /// <param name="bottom">  The bottom.</param>
        /// <returns>
        ///  True if successful
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool Equals(int left, int top, int right, int bottom)
        {
            return left == Left && top == Top && right == Right && bottom == Bottom;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Get Hash Code.
        /// </summary>
        /// <returns>
        ///  The integer value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override int GetHashCode()
        {
            return ((Rectangle)this).GetHashCode();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Get a string representation of RECT.
        /// </summary>
        /// <returns>
        ///  The string representation
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override string ToString()
        {
            return $"{{Left={Left},Top={Top},Right={Right},Bottom={Bottom}}}";
        }
    }
}
