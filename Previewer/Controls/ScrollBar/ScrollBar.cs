////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: Common code for scroll bar.
// 
//  Author:      Robert C. Steiner
// 
//=====================================================[ History ]=====================================================
//  Date        Who      What
//  ----------- ------   ----------------------------------------------------------------------------------------------
//  6/10/2020   RCS      Initial Code.
//====================================================[ Copyright ]====================================================
// 
//  Copyright 2020 Robert C. Steiner
//  
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software
//  and associated documentation files (the "Software"), to deal in the Software without restriction,
//  including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
//  and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
//  subject to the following conditions:
//  
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//  
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
//  INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE
//  AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES
//  OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//  CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Previewer.Controls
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  Delegate: Scroll Handler
    /// </summary>
    /// <param name="sender">  The sender.</param>
    /// <param name="e">       The Scroll Event arguments e.</param>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public delegate void ScrollHandler(object sender, ScrollEventArgs e);


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Scroll Bar Class definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public abstract partial class ScrollBar : UserControl
    {
        /// <summary>
        ///  Gets or sets the background color for the control. Causes foreground color to change to white or black.
        ///  Change foreground color after background to get foreground other than the default.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Category("Display")]
        [Description("Color of background.  Causes foreground color to change to white or black.")]
        public override Color BackColor
        {
            get => base.BackColor;
            set
            {
                base.BackColor = value;
                ForeColor = BackColor.IsDark() ? Color.White : Color.Black;
            }
        }

        /// <summary>
        ///  Get/Set Large Change.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("LargeChange normally a page worth of scroll")]
        public int LargeChange
        {
            get => _largeChange;
            set
            {
                _largeChange = value;
                Invalidate();
            }
        }

        /// <summary>
        ///  Get/Set Maximum.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("Maximum value")]
        public int Maximum
        {
            get => _maximum;
            set
            {
                _maximum = value;
                Invalidate();
            }
        }

        /// <summary>
        ///  Get/Set Minimum.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("Minimum value")]
        public int Minimum
        {
            get => _minimum;
            set
            {
                _minimum = value;
                Invalidate();
            }
        }

        /// <summary>
        ///  Get Range.
        /// </summary>
        public int Range
        {
            get { return Maximum - Minimum; }
        }

        /// <summary>
        ///  Get/Set Small Change.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("SmallChange")]
        public int SmallChange
        {
            get => _smallChange;
            set
            {
                _smallChange = value;
                Invalidate();
            }
        }

        /// <summary>
        ///  Get/Set Value.  Causes Scroll event to occur.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("Value")]
        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                if (_value < _minimum) _value = _minimum;
                if (_value > _maximum) _value = _maximum;
                Scroll?.Invoke(this, new ScrollEventArgs(ScrollEventType.First, _value));
            }
        }

        /// <summary>
        ///  The Scroll event.
        /// </summary>
        public new event ScrollHandler Scroll;

        /// <summary>
        ///  Raises the <see cref="E:System.Windows.Forms.Control.DoubleClick" /> event.
        /// </summary>
        private int BarLength => IsHorizontal ? Width : Height;

        /// <summary>
        ///  Get/Set foreground Brush.
        /// </summary>
        private Brush ForeBrush { get; set; }

        /// <summary>
        ///  Get/Set Is Horizontal.
        /// </summary>
        protected abstract bool IsHorizontal { get; }

        /// <summary>
        ///  The draw Points field.
        /// </summary>
        private Point[] _drawPoints = new Point[3];

        /// <summary>
        ///  The  hover field.
        /// </summary>
        private bool _hover;

        /// <summary>
        ///  The mo Large Change field.
        /// </summary>
        protected int _largeChange = 10;

        /// <summary>
        ///  The mo Maximum field.
        /// </summary>
        protected int _maximum = 100;

        /// <summary>
        ///  The mo Minimum field.
        /// </summary>
        protected int _minimum;

        /// <summary>
        ///  The mo Small Change field.
        /// </summary>
        protected int _smallChange = 1;

        /// <summary>
        ///  The mo Thumb Down field.
        /// </summary>
        private bool _thumbDown;

        /// <summary>
        ///  The mo Thumb Dragging field.
        /// </summary>
        private bool _thumbDragging;

        /// <summary>
        ///  The mo Value field.
        /// </summary>
        protected int _value;

        /// <summary>
        ///  The ARROW size field.
        /// </summary>
        protected const int ARROW_SIZE = 16;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Default Constructor: For CustomScrollbar.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public ScrollBar()
        {
            InitializeComponent();

            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            if (IsHorizontal)
            {
                Size = new Size(Size.Width, ARROW_SIZE);
                base.Dock = DockStyle.Bottom;
            }
            else
            {
                Size = new Size(ARROW_SIZE, Size.Height);
                base.Dock = DockStyle.Right;
            }

            Width = ARROW_SIZE;
            //   MinimumSize = new Size(ARROW_SIZE, ARROW_SIZE );
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Add points to the drawPoints array with an offset.
        /// </summary>
        /// <param name="x">      The integer x coordinate offset.</param>
        /// <param name="y">      The integer y coordinate offset.</param>
        /// <param name="points"> The points to add.</param>
        /// <returns>
        ///  The drawPoints array value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private Point[] Add(int x, int y, Point[] points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                _drawPoints[i] = new Point(x + points[i].X, y + points[i].Y);
            }
            return _drawPoints;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Draw Arrow.
        /// </summary>
        /// <param name="graphics"> The graphics.</param>
        /// <param name="x">        The integer x coordinate.</param>
        /// <param name="y">        The integer y coordinate.</param>
        /// <param name="dir">      The dir.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected void DrawArrow(Graphics graphics, int x, int y, Point[] dir)
        {
            graphics.FillPolygon(ForeBrush, Add(x, y, dir));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Draw Arrow And Text.
        /// </summary>
        /// <param name="g">    The Graphics context.</param>
        /// <param name="text"> The text to draw.</param>
        /// <param name="r">    The Rectangle to put it in.</param>
        /// <param name="sz">   The size.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected abstract void DrawArrowAndText(Graphics g, string text, Rectangle r, SizeF sz);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Get Thumb Color.
        /// </summary>
        /// <returns>
        ///  The System.Drawing.Color value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private Color GetThumbColor()
        {
            var c = ForeColor;
            var m = (c.IsDark()) ? 1 : -1;

            if (_thumbDown)
            {
                return c;
            }

            if (_hover)
            {
                return c.Lighten(m * .15);
            }

            return c.Lighten(m * .35);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Move Thumb.
        /// </summary>
        /// <param name="x"> The integer x coordinate.</param>
        /// <param name="y"> The integer y coordinate.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void MoveThumb(int x, int y)
        {
            MoveThumb(x, y, ThumbRectangle());
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Move Thumb.
        /// </summary>
        /// <param name="x">      The integer x coordinate.</param>
        /// <param name="y">      The integer y coordinate.</param>
        /// <param name="thumb">  The thumb.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected abstract void MoveThumb(int x, int y, Rectangle thumb);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Raises the <see cref="E:System.Windows.Forms.Control.MouseDown" /> event.
        /// </summary>
        /// <param name="e"> A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            var point = PointToClient(Cursor.Position);
            var thumb = ThumbRectangle();

            // check the slider box

            if (thumb.Contains(point))
            {
                // the thumb is being moved
                _thumbDown = true;
                Invalidate();
                return;
            }

            // check arrow
            ScrollChange(point, thumb);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Raises the <see cref="E:System.Windows.Forms.Control.MouseHover" /> event.
        /// </summary>
        /// <param name="e"> An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);

            var point = PointToClient(Cursor.Position);
            var thumb = ThumbRectangle();

            if (thumb.Contains(point))
            {
                _hover = true;
                Invalidate();
            }

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave" /> event.
        /// </summary>
        /// <param name="e"> An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (_hover)
            {
                _hover = false;
                Invalidate();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Raises the <see cref="E:System.Windows.Forms.Control.MouseMove" /> event.
        /// </summary>
        /// <param name="e"> A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_thumbDown)
            {
                _thumbDragging = true;
            }

            if (_thumbDragging)
            {
                MoveThumb(e.X, e.Y);
            }

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Raises the <see cref="E:System.Windows.Forms.Control.MouseUp" /> event.
        /// </summary>
        /// <param name="e"> A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            _thumbDragging = false;
            _thumbDown     = false;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Raises the <see cref="E:System.Windows.Forms.Control.MouseCaptureChanged" /> event.
        /// </summary>
        /// <param name="e"> An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected override void OnMouseCaptureChanged(EventArgs e)
        {
            base.OnMouseCaptureChanged(e);
             _thumbDragging = false;
            _thumbDown      = false;
            _hover          = false;
            Invalidate();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: On Paint.
        /// </summary>
        /// <param name="e"> The Paint Event arguments e.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected override void OnPaint(PaintEventArgs e)
        {
            var g               = e.Graphics;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.SmoothingMode     = SmoothingMode.AntiAlias;
            ForeBrush           = new SolidBrush(GetThumbColor());

            // Draw background 
            g.Clear(BackColor);

            // draw thumb
            var r = ThumbRectangle();
            g.FillRectangle(ForeBrush, r);
            ForeBrush.Dispose();


            // draw arrows and text
            ForeBrush = new SolidBrush(ForeColor);


            var sz = (! string.IsNullOrEmpty(Text) ) ? g.MeasureString(Text, Font) : SizeF.Empty;
            DrawArrowAndText(g, Text, r, sz);

            ForeBrush.Dispose();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Scroll Change.
        /// </summary>
        /// <param name="point"> The point.</param>
        /// <param name="thumb"> The thumb.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected abstract void ScrollChange(Point point, Rectangle thumb);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method:Compute Thumb Rectangle.
        /// </summary>
        /// <returns>
        ///  The System.Drawing.Rectangle value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected Rectangle ThumbRectangle()
        {
            //  +------------------------------------------------------------------+
            //  | <          [----------]                                       >  |
            //  +------------------------------------------------------------------+
            //  ^ h ^---x----^-----w----^                                      ^ h ^
            //      ^--------------------------o-------------------^-----w-----^

            var p = LargeChange / (float)Range;      // percent of bar for one large change.
            var b = BarLength - 2 * ARROW_SIZE;      // channel length
            var o = (int)(b / (p + 1));              // thumb left side
            var w = b - o;                           // adjusted thumb length (width)
            var x = ARROW_SIZE + o * Value / Range;  // thumb left or top position.
            var r = IsHorizontal
                ? new Rectangle(x, 0, w, Height)
                : new Rectangle(0, x, Width, w);
            return r;
        }
    }
}



