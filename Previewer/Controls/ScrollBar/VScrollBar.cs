////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: The vertical scroll bar.
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
using System.Drawing;

namespace Previewer.Controls
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The VScroll Bar Class definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class VScrollBar : ScrollBar
    {
        /// <summary>
        ///  Get/Set Is Horizontal.
        /// </summary>
        protected override bool IsHorizontal => false;

        /// <summary>
        ///  The triangle Down field.
        /// </summary>
        private Point[] triDown = { new Point(8, 12), new Point(4, 4), new Point(12, 4) };

        /// <summary>
        ///  The triangle Up field.
        /// </summary>
        private Point[] triUp = { new Point(8, 4), new Point(12, 12), new Point(4, 12) };

        /// <summary>
        ///  The  string Format field.
        /// </summary>
        private StringFormat _stringFormat;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Default Constructor: For VScrollBar.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public VScrollBar()
        {
            _stringFormat = new StringFormat();
            _stringFormat.FormatFlags = StringFormatFlags.DirectionVertical;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Draw Arrow And Text.
        /// </summary>
        /// <param name="g">    The Graphics context.</param>
        /// <param name="text"> The text.</param>
        /// <param name="r">    The Rectangle to put text in.</param>
        /// <param name="sz">   The size of the text.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected override void DrawArrowAndText(Graphics g, string text, Rectangle r, SizeF sz)
        {
            // Draw Arrows
            DrawArrow(g, 0x0, 0x0, triUp);
            DrawArrow(g, 0, Height - ARROW_SIZE, triDown);
            if (text != null) g.DrawString(text, Font, Brushes.Black, 0, r.Top + r.Height / 2 - sz.Width / 2, _stringFormat);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Move Thumb.
        /// </summary>
        /// <param name="x">      The integer x coordinate.</param>
        /// <param name="y">      The integer y coordinate.</param>
        /// <param name="thumb">  The thumb.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected override void MoveThumb(int x, int y, Rectangle thumb)
        {
            if (y <= thumb.Top)
            {
                // large scroll up
                var w = thumb.Top - ARROW_SIZE;
                Value = (w <= 0) ? 0 : Value * (y - ARROW_SIZE) / w;
            }

            else if (y >= thumb.Bottom)
            {
                // large scroll down
                var w = Height - thumb.Bottom - ARROW_SIZE;
                Value += (w <= 0) ? 0 : Maximum * (y - thumb.Bottom) / w;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Scroll Change.
        /// </summary>
        /// <param name="point"> The point.</param>
        /// <param name="thumb"> The thumb.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected override void ScrollChange(Point point, Rectangle thumb)
        {
            var y = point.Y;

            if (y <= ARROW_SIZE)
            {
                // up scroll
                Value -= SmallChange;
                return;
            }

            if (y >= Height - ARROW_SIZE)
            {
                // down scroll
                Value += SmallChange;
                return;
            }

            if (y <= thumb.Top)
            {
                // large scroll up
                Value -= LargeChange;
                return;
            }

            if (y >= thumb.Bottom)
            {
                // large scroll down
                Value += LargeChange;
            }
        }
    }
}

