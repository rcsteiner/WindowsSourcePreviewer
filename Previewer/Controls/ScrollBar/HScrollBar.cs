////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: The horizontal scroll bar.
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
    ///  The HScroll Bar Class definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class HScrollBar : ScrollBar
    {
        /// <summary>
        ///  Get/Set Is Horizontal.
        /// </summary>
        protected override bool IsHorizontal => true;

        /// <summary>
        ///  The triangle Left field.
        /// </summary>
        private Point[] triLeft = { new Point(4, 8), new Point(12, 4), new Point(12, 12) };

        /// <summary>
        ///  The triangle Right field.
        /// </summary>
        private Point[] triRight = { new Point(12, 8), new Point(4, 12), new Point(4, 4) };

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
            DrawArrow(g, 0x0, 0x0, triLeft);
            DrawArrow(g, Width - ARROW_SIZE, 0, triRight);
           if (text != null) g.DrawString(text, Font, Brushes.Black, r.X + r.Width / 2, 3);
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
            if (x <= thumb.Left)
            {
                // large scroll left
                var w = thumb.Left - ARROW_SIZE;
                Value = (w <= 0) ? 0 : Value * (x - ARROW_SIZE) / w;
            }
            else if (x >= thumb.Right)
            {
                // large scroll right
                var w = Width - thumb.Right - ARROW_SIZE;
                Value += (w <= 0) ? 0 : Maximum * (x - thumb.Right) / w;
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
            var x = point.X;

            if (x <= ARROW_SIZE)
            {
                // left scroll
                Value -= SmallChange;
                return;
            }

            if (x >= Width - ARROW_SIZE)
            {
                // right scroll
                Value += SmallChange;
                return;
            }

            if (x <= thumb.Left)
            {
                // large scroll left
                Value -= LargeChange;
                return;
            }

            if (x >= thumb.Right)
            {
                // large scroll right
                Value += LargeChange;
            }
        }
    }
}

