////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: TExt context, manages the drawing of text 
// 
//  Author:      Robert C Steiner
// 
//=====================================================[ History ]=====================================================
//  Date        Who      What
//  ----------- ------   ----------------------------------------------------------------------------------------------
//  2/24/2020   RCS      Initial Code.
//====================================================[ Copyright ]====================================================
// 
//  BSD 3-Clause License
//  Copyright (c) 2020, Robert C. Steiner
//  All rights reserved.
//  Redistribution and use in source and binary forms, with or without
//  modification, are permitted provided that the following conditions are met:
//  1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following
//  disclaimer.
//  2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the
//  following disclaimer in the documentation and/or other materials provided with the distribution.
//  3. Neither the name of the copyright holder nor the names of its
//  contributors may be used to endorse or promote products derived from
//  this software without specific prior written permission.
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
//  INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//  DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
//  SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
//  SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
//  WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE
//  USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.Reserved.
// 
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace Win32
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Text Context Class definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class TextContext : ITextContext
    {
        /// <summary>
        ///  The device-context field.
        /// </summary>
        private IntPtr _dc;

        /// <summary>
        ///  Get/Set Current Font.
        /// </summary>
        private Font _currentFont;

        /// <summary>
        ///  The Format field.
        /// </summary>
        private DrawFormat _format;

        /// <summary>
        ///  The Font field.
        /// </summary>
        private IntPtr _oldFont;

        /// <summary>
        ///  The Text parameters field.
        /// </summary>
        private DRAWTEXTPARAMS _textParams;

        /// <summary>
        ///  The current text bounds
        /// </summary>
        private RECT _bounds;

        /// <summary>
        ///  The current clipping rectangle.
        /// </summary>
        private RECT _clip;

        /// <summary>
        ///  The clip Region Handle field.
        /// </summary>
        private IntPtr _clipRegionHandle;

        /// <summary>
        ///  The Font Handle field.
        /// </summary>
        private IntPtr _hFont;

        /// <summary>
        ///  Get Size Bounds rectangle.
        /// </summary>
        private static RECT SIZE_BOUNDS => new RECT(0, 0);


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Change Format of Drawing text.
        /// </summary>
        /// <param name="format"> The format.</param>
        /// <returns>
        ///  The old format.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public DrawFormat ChangeFormat(DrawFormat format)
        {
            var old = _format;
            _format = format;
            return old;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Draw a Line from Start to End.
        /// </summary>
        /// <param name="start"> The start point.</param>
        /// <param name="end">   The end point.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void DrawLine(Point start, Point end)
        {
            MoveTo(start);
            LineTo(end);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Draw Line using descrete x,y values.
        /// </summary>
        /// <param name="x">  The integer x coordinate.</param>
        /// <param name="y">  The integer y coordinate.</param>
        /// <param name="x2"> The x2.</param>
        /// <param name="y2"> The y2.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void DrawLine(int x, int y, int x2, int y2)
        {
            MoveToEx(_dc, x, y, IntPtr.Zero);
            LineTo(_dc, x2, y2);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Draw Text at a point in a color and font. Resets the text color to previous text color after
        ///  drawing.
        /// </summary>
        /// <param name="text">  The text.</param>
        /// <param name="point"> The point.</param>
        /// <param name="color"> The color.</param>
        /// <param name="font">  [optional=null] The font if not null (if null, use the current font).</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public  void DrawText(string text, Point point, Color color, Font font = null)
        {
            SelectFont(font);
            SetBounds(point);
           // var oldColor = SetTextColor(_dc, color);
            SetTextColor(_dc, color);
            DrawTextExW(_dc, text, text.Length, ref _bounds, (int)(_format), _textParams);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Draw Text.
        /// </summary>
        /// <param name="text">   The text.</param>
        /// <param name="length"> The length.</param>
        /// <param name="point">  The point.</param>
        /// <param name="color">  The color.</param>
        /// <param name="font">   [optional=null] The font.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void DrawText(StringBuilder text, int length, Point point, Color color, Font font = null)
        {
            SelectFont(font);
            SetBounds(point);
            SetTextColor(_dc, color);
            DrawTextExW(_dc, text, length, ref _bounds, (int)(_format), _textParams);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Draw Text.
        /// </summary>
        /// <param name="text">   The text.</param>
        /// <param name="offset"> The offset.</param>
        /// <param name="length"> The length.</param>
        /// <param name="point">  The point.</param>
        /// <param name="color">  The color.</param>
        /// <param name="font">   [optional=null] The font.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public  void DrawText(char[] text, int offset, int length, Point point, Color color, Font font = null)
        {
            SelectFont(font);
            SetBounds(point);
            SetTextColor(_dc, color);
            DrawTextExW(text, offset, length, (int)(_format));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Draw Text Ex W.
        /// </summary>
        /// <param name="charArray">       The char Array.</param>
        /// <param name="offset">          The offset.</param>
        /// <param name="count">           The count.</param>
        /// <param name="format">          The format.</param>
        /// <returns>
        ///  The integer value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public int DrawTextExW(char[] charArray, int offset, int count, int format)
        {
            unsafe
            {
                fixed (char* p = &charArray[offset])
                {
                    return DrawTextExW(_dc, (IntPtr)p, count, ref _bounds, format, _textParams);
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: End the drawing, freeing resources.  Restores old font (before StartDrawing() was called)
        /// </summary>
        /// <param name="graphics"> The Graphics object from form.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void EndDrawing(Graphics graphics)
        {
            if (_oldFont != IntPtr.Zero)
            {
                SelectObject(_dc, _oldFont); 
                _oldFont = IntPtr.Zero;
            }

            if (_hFont != IntPtr.Zero)
            {
                DeleteObject( _hFont);
                _currentFont = null;
            }

            graphics.ReleaseHdc(_dc);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Line To.
        /// </summary>
        /// <param name="newPoint"> The new Point.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void LineTo(Point newPoint)
        {
            LineTo(_dc, newPoint.X, newPoint.Y);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Measure Text.
        /// </summary>
        /// <param name="text"> The text.</param>
        /// <returns>
        ///  The System.Drawing.Size value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Size MeasureText(string text)
        {
            if (text.Length == 0) return Size.Empty;

            var bounds = SIZE_BOUNDS;
            DrawTextExW(_dc, text, text.Length, ref bounds, (int)_format | (int)TextFormatFlags.CalculateRectangle, _textParams);

            return bounds.Size;
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Measure Text.
        /// </summary>
        /// <param name="text">   The text.</param>
        /// <param name="length"> The length.</param>
        /// <returns>
        ///  The System.Drawing.Size value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public  Size MeasureText(StringBuilder text, int length)
        {
            if (text.Length == 0) return Size.Empty;

            _bounds = SIZE_BOUNDS;
            DrawTextExW(_dc, text, length, ref _bounds, (int)_format | (int)TextFormatFlags.CalculateRectangle, _textParams);

            return _bounds.Size;
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Measure Text.
        /// </summary>
        /// <param name="text">   The text.</param>
        /// <param name="offset"> The offset.</param>
        /// <param name="length"> The length.</param>
        /// <returns>
        ///  The System.Drawing.Size value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Size MeasureText(char[] text, int offset, int length)
        {
            if (text.Length == 0) return Size.Empty;

            _bounds = SIZE_BOUNDS;
            DrawTextExW(text, offset, length, (int)_format | (int)TextFormatFlags.CalculateRectangle);

            return _bounds.Size;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Move To.
        /// </summary>
        /// <param name="newPoint"> The new Point.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void MoveTo(Point newPoint)
        {
            MoveToEx(_dc, newPoint.X, newPoint.Y, IntPtr.Zero);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Select Clip Region.
        /// </summary>
        /// <param name="left">    The left.</param>
        /// <param name="top">     The top.</param>
        /// <param name="right">   The right.</param>
        /// <param name="bottom">  The bottom.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SelectClipRegion(int left,int top, int right, int bottom )
        {
            if (_clip.Equals(left, top, right, bottom))
            {
                SelectClipRgn(_dc, _clipRegionHandle);
                return;
            }

            if (_clipRegionHandle != IntPtr.Zero)
            {
                DeleteObject(_clipRegionHandle);
            }

            _clip             = new RECT(left,top,right,bottom);
            _clipRegionHandle = CreateRectRgn(left,top, right, bottom);
            SelectClipRgn(_dc, _clipRegionHandle);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Select Font
        /// </summary>
        /// <param name="font"> The font.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SelectFont(Font font)
        {
            if (font == null || font.Equals(_currentFont)) return;

            
            if (_hFont != IntPtr.Zero)
            {
                DeleteObject(_hFont);
            }

            _currentFont   = font;
            _hFont         = font.ToHfont();
            SelectObject(_dc, _hFont);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Create Pen.
        /// </summary>
        /// <param name="width"> The width.</param>
        /// <param name="color"> The color.</param>
        /// <returns>
        ///  The System.IntPtr value. The pen.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public IntPtr SelectSolidPen(int width, Color color)
        {
            var p   =  CreatePen(PenStyle.PS_SOLID, width, color);
            var old =  SelectObject(_dc, p);
            if (old != IntPtr.Zero) DeleteObject(old);

            return p;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Set Bounds.
        /// </summary>
        /// <param name="point"> The point.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetBounds(Point point)
        {
            if ((_format & DrawFormat.DT_RIGHT) != 0)
            {
                _bounds.X      = 0;
                _bounds.Y      = point.Y;
                _bounds.Right  = point.X;
                _bounds.Bottom = int.MaxValue;
            }
            else
            {
                _bounds.X      = point.X;
                _bounds.Y      = point.Y;
                _bounds.Right  = int.MaxValue;
                _bounds.Bottom = int.MaxValue;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Start Drawing, sets the background color and clears the clip region, set tab and basic draw format.
        /// </summary>
        /// <param name="graphics">   The graphics.</param>
        /// <param name="background"> The background.</param>
        /// <param name="tabSize">    The tab Size.</param>
        /// <param name="font">       The font.</param>
        /// <param name="format">     [optional=(DrawFormat.DT_NOPREFIX | DrawFormat.DT_NOCLIP)] The format.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void StartDrawing(Graphics graphics, Color background, int tabSize, Font font, DrawFormat format = (DrawFormat.DT_NOPREFIX | DrawFormat.DT_NOCLIP))
        {
            graphics.Clear(background);
            _dc         = graphics.GetHdc();
            _textParams = new DRAWTEXTPARAMS(tabSize);
            _format     = format;
            SetBkMode(_dc, BkMode.TRANSPARENT);
            SelectFont(font);
        }

        #region Static Windows Functions

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  The block Mode Enumeration definition.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private enum BkMode
        {
            /// <summary>
            ///  The TRANSPARENT enum value.
            /// </summary>
            TRANSPARENT = 1,

            /// <summary>
            ///  The OPAQUE enum value.
            /// </summary>
            OPAQUE = 2
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Delete device-context.
        /// </summary>
        /// <param name="hDC"> The h device-context.</param>
        /// <returns>
        ///  True if successful
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("gdi32.dll", EntryPoint = "DeleteDC", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool DeleteDC(IntPtr hDC);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Deletes a logical pen, brush, font, bitmap, region, or palette, freeing all system resources associated
        ///  with the object. After the object is deleted, the specified handle is no longer valid.
        /// </summary>
        /// <param name="hObject"> A handle to a logical pen, brush, font, bitmap, region, or palette.</param>
        /// <returns>
        ///  <para>If the function succeeds, the return value is nonzero.</para>
        ///  <para>If the specified handle is not valid or is currently selected into a DC, the return value is
        ///  zero.</para>
        /// </returns>
        /// <remarks>
        ///  <para>Do not delete a drawing object (pen or brush) while it is still selected into a DC.</para>
        ///  <para>When a pattern brush is deleted, the bitmap associated with the brush is not deleted. The bitmap must
        ///  be deleted independently.</para>
        /// </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteObject([In] IntPtr hObject);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Select Object.
        /// </summary>
        /// <param name="hdc">     The hdc.</param>
        /// <param name="hgdiobj"> The hgdiobj.</param>
        /// <returns>
        ///  The System.IntPtr value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("gdi32.dll", EntryPoint = "SelectObject", SetLastError = true)]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Set block Color.
        /// </summary>
        /// <param name="hDC"> The h device-context.</param>
        /// <param name="clr"> The clear.</param>
        /// <returns>
        ///  The integer value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int SetBkColor(IntPtr hDC, int clr);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Set block Mode.
        /// </summary>
        /// <param name="hdc">  The hdc.</param>
        /// <param name="mode"> The mode.</param>
        /// <returns>
        ///  The integer value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("gdi32.dll", EntryPoint = "SetBkMode", SetLastError = true)]
        private static extern int SetBkMode(IntPtr hdc, BkMode mode);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Set Text Color.
        /// </summary>
        /// <param name="hDC">   The h device-context.</param>
        /// <param name="color"> The color.</param>
        /// <returns>
        ///  The uint value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern uint SetTextColor(IntPtr hDC, uint color);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Release device-context.
        /// </summary>
        /// <param name="hWnd"> The h window.</param>
        /// <param name="hDC">  The h device-context.</param>
        /// <returns>
        ///  The integer value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("user32.dll", EntryPoint = "ReleaseDC", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Draw Text Ex A.
        /// </summary>
        /// <param name="hDC">        The h device-context.</param>
        /// <param name="lpszString"> The lpsz String.</param>
        /// <param name="nCount">     The n Count.</param>
        /// <param name="lpRect">     [ref] The pointer to a rectangle.</param>
        /// <param name="nFormat">    The n Format.</param>
        /// <param name="lpDTParams"> The pointer to a DTParams.</param>
        /// <returns>
        ///  If the function succeeds, the return value is the height of the text in logical units.
        ///  If DT_VCENTER or DT_BOTTOM is specified, the return value is the offset from lpRect->top to the bottom of
        ///  the drawn text
        ///  If the function fails, the return value is zero
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int DrawTextExW(IntPtr hDC, StringBuilder lpszString, int nCount, ref RECT lpRect, int nFormat, [In, Out] DRAWTEXTPARAMS lpDTParams);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Draw Text Ex W.
        /// </summary>
        /// <param name="hDC">        The h device-context.</param>
        /// <param name="lpszString"> The lpsz String.</param>
        /// <param name="nCount">     The n Count.</param>
        /// <param name="lpRect">     [ref] The pointer to a rectangle.</param>
        /// <param name="nFormat">    The n Format.</param>
        /// <param name="lpDTParams"> The pointer to a DTParams.</param>
        /// <returns>
        ///  The integer value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("user32.dll", EntryPoint = "DrawTextExW", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int DrawTextExW(IntPtr hDC, string lpszString, int nCount, ref RECT lpRect, int nFormat, [In, Out] DRAWTEXTPARAMS lpDTParams);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Draw Text Ex W.
        /// </summary>
        /// <param name="hDC">        The h device-context.</param>
        /// <param name="lpszString"> The lpsz String.</param>
        /// <param name="nCount">     The n Count.</param>
        /// <param name="lpRect">     [ref] The pointer to a rectangle.</param>
        /// <param name="nFormat">    The n Format.</param>
        /// <param name="lpDTParams"> The pointer to a DTParams.</param>
        /// <returns>
        ///  The integer value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int DrawTextExW(IntPtr hDC, IntPtr lpszString, int nCount, ref RECT lpRect, int nFormat, [In, Out] DRAWTEXTPARAMS lpDTParams);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Line To.
        /// </summary>
        /// <param name="hdc">    The hdc.</param>
        /// <param name="nXEnd">  The n XEnd.</param>
        /// <param name="nYEnd">  The n YEnd.</param>
        /// <returns>
        ///  True if successful
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool LineTo(IntPtr hdc, int nXEnd, int nYEnd);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Move To Ex.
        /// </summary>
        /// <param name="hdc">      The hdc.</param>
        /// <param name="X">        The integer x coordinate.</param>
        /// <param name="Y">        The integer y coordinate.</param>
        /// <param name="lpPoint">  The pointer to a Point.</param>
        /// <returns>
        ///  True if successful
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool MoveToEx(IntPtr hdc, int X, int Y, IntPtr lpPoint);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Create Pen.
        /// </summary>
        /// <param name="fnPenStyle">  The function Pen Style.</param>
        /// <param name="nWidth">      The n Width.</param>
        /// <param name="crColor">     The cr Color.</param>
        /// <returns>
        ///  The System.IntPtr value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CreatePen(PenStyle fnPenStyle, int nWidth, uint crColor);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Select Clip region.
        /// </summary>
        /// <param name="hdc">   The hdc.</param>
        /// <param name="hrgn">  The hrgn.</param>
        /// <returns>
        ///  The integer value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int SelectClipRgn(IntPtr hdc, IntPtr hrgn);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Create rectangle region.
        /// </summary>
        /// <param name="nLeftRect">    The n Left rectangle.</param>
        /// <param name="nTopRect">     The n Top rectangle.</param>
        /// <param name="nRightRect">   The n Right rectangle.</param>
        /// <param name="nBottomRect">  The n Bottom rectangle.</param>
        /// <returns>
        ///  The System.IntPtr value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        #endregion

    }
}
