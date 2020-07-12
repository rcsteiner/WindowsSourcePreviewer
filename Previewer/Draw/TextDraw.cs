////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: Draws a source file based on language and palette to highlight it.
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
using System.Drawing;
using Scan;
using Win32;
using Color = Win32.Color;

namespace SourcePreview
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Source Draw Class definition. Draws a source file based on language and palette to highlight it.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class TextDraw
    {
        /// <summary>
        ///  The background field.
        /// </summary>
        public Color BackgroundColor { get { return  ((TokenType) (TokenType.Background)).Color; } }

        /// <summary>
        ///  Get/Set Row Column Size.
        /// </summary>
        public Size RowColSize { get; set; }

        /// <summary>
        ///  Get/Set Show Line Numbers.
        /// </summary>
        public bool ShowLineNumbers { get; set; }

        /// <summary>
        ///  Get/Set Tab Size.
        /// </summary>
        public int TabSize { get; set; }

        /// <summary>
        ///  The Font Height field.
        /// </summary>
        public int FontHeight;

        /// <summary>
        ///  The Font Width field.
        /// </summary>
        public int FontWidth;

        /// <summary>
        ///  The Line Pen field.
        /// </summary>
        public Pen LinePen;

        /// <summary>
        ///  The buffer field.
        /// </summary>
        private IScanner _scanner;

        /// <summary>
        ///  The zoom field.
        /// </summary>
        private float _zoom;

        /// <summary>
        ///  The Em Size field.
        /// </summary>
        private const float EmSize = 10;

        /// <summary>
        ///  The Family Name field.
        /// </summary>
        private const string FamilyName = "Consolas";

        /// <summary>
        ///  The fonts field.
        /// </summary>
        private Font[] fonts;

        /// <summary>
        ///  The Font Size field.
        /// </summary>
        private Size FontSize;

        /// <summary>
        ///  The Left Margin field.
        /// </summary>
        private Point LeftMargin = new Point(5, 5);

        /// <summary>
        ///  The start Point field.
        /// </summary>
        private Point startPoint;

        /// <summary>
        ///  The tc field.
        /// </summary>
        private TextContext tc = new TextContext();


        private const string HEX = "0123456789ABCDEF";

        private char[] _binChars;


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Default Constructor: For SourceDraw.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public TextDraw()
        {
            TabSize         = 4;
            ShowLineNumbers = true;
            RowColSize      = new Size(0, 0);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Create Fonts.
        /// </summary>
        /// <param name="emSize"> The em Size.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void CreateFonts(float emSize)
        {
            if (fonts != null)
            {
                foreach (var font in fonts)
                {
                    font.Dispose();
                }
            }
            emSize *= _zoom;
            fonts = new[]
            {
                new Font(FamilyName, emSize, FontStyle.Regular), // Regular
                new Font(FamilyName, emSize, FontStyle.Bold),    // Bold
                new Font(FamilyName, emSize, FontStyle.Italic)  // Italic
            //    new Font(FamilyName, emSize, FontStyle.Underline),  // Underline
            //    new Font(FamilyName, emSize, FontStyle.Strikeout),  // Strike-out
            };
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Draw.
        /// </summary>
        /// <param name="foreColor"> The foreground Color.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Draw(Color foreColor)
        {
            var text = _scanner.Token.Text;
            FontSize = tc.MeasureText(text, 0, _scanner.Token.Length);
            tc.DrawText(text, 0, _scanner.Token.Length, startPoint, foreColor);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Draw Line Numbers.
        /// </summary>
        /// <param name="start">  The start.</param>
        /// <param name="rows">   The rows.</param>
        /// <param name="height"> The height.</param>
        /// <returns>
        ///  The column x coordinate to start drawing text.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private int DrawLineNumbers(int start, int rows, int height)
        {
            if (LinePen == null)
            {
                LinePen = new Pen(((TokenType)(TokenType.LineNumber)).Color);
            }

            var c      = SetFont(TokenType.LineNumber);
            var w      = tc.MeasureText((start + rows).ToString()).Width + 3;
            var old    = tc.ChangeFormat(DrawFormat.DT_RIGHT);
            startPoint = new Point(w, 5);

            for (int i = 0; i < rows; ++i)
            {
                var line = (i + start);
                if (line > _scanner.LineCount) break;
                _scanner.Token.Clear();
                _scanner.Token.Append(line);
                Draw(c);
                startPoint.Y += FontHeight;
            }
            tc.ChangeFormat(old);

            w += 5;
            tc.SelectSolidPen(1, Color.DIM_GRAY_DIM_GREY);
            tc.DrawLine(w, 0, w, height);
            return w + 8;

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Draw the address numbers start is the starting line number.
        /// </summary>
        /// <param name="startAddr">  The start.</param>
        /// <param name="rows">   The rows.</param>
        /// <param name="height"> The height.</param>
        /// <returns>
        ///  The column x coordinate to start drawing text.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private int DrawAddressNumbers(int startAddr, int countWidth, int maxAddr, int rows, int height)
        {
            if (LinePen == null)
            {
                LinePen = new Pen(((TokenType)(TokenType.LineNumber)).Color);
            }

            var c = SetFont(TokenType.LineNumber);
            var w = tc.MeasureText("00000000").Width + 3;
            startPoint = new Point(3, 5);

            for (int i = 0; i < rows; i += countWidth)
            {
                var addr = (i + startAddr);
                if (addr > maxAddr) break;
                _scanner.Token.Clear();
                _scanner.Token.Append(addr);
                Draw(c);
                startPoint.Y += FontHeight;
            }

            w += 5;
            tc.SelectSolidPen(1, Color.DIM_GRAY_DIM_GREY);
            tc.DrawLine(w, 0, w, height);
            return w + 8;

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Set Font and gets the color.
        /// </summary>
        /// <param name="type"> The type.</param>
        /// <returns>
        ///  The System.Drawing.Color foreground color.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private Color SetFont(TokenType type)
        {
            var font      = fonts[(int)type.FontId];
            var foreColor = type.Color;
            tc.SelectFont(font);
            return foreColor;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Set Left Margin.
        /// </summary>
        /// <param name="left"> The left.</param>
        /// <param name="top">  The top.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetLeftMargin(int left, int top)
        {
            LeftMargin = new Point(left, top);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Show Buffer.
        /// </summary>
        /// <param name="dev">        The dev.</param>
        /// <param name="scanner">    The buffer.</param>
        /// <param name="clientSize"> The client Size.</param>
        /// <param name="rowStart">   The top.</param>
        /// <param name="colStart">   The horizontal Percent.</param>
        /// <param name="zoom">       The zoom.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void ShowBuffer(Graphics dev, IScanner scanner, Size clientSize, int rowStart, int colStart, float zoom)
        {
            if (scanner == null || scanner.LineCount == 0) return;

            _scanner = scanner;

            if (Math.Abs(_zoom - zoom) > .05)
            {
                _zoom = zoom;
                CreateFonts(EmSize);
            }

            tc.StartDrawing(dev, BackgroundColor, 4, fonts[0]);

            FontSize     = tc.MeasureText("M");
            FontWidth    = FontSize.Width;
            FontHeight   = FontSize.Height;
            var rows     = clientSize.Height / FontHeight;
            var cols     = clientSize.Width  / FontWidth;
            var leftSkip = colStart;

            // skip lines above view.
            _scanner.MoveToLine(rowStart);

            LeftMargin.X = DrawLineNumbers(scanner.LineNumber, rows, clientSize.Height);
            startPoint.X = LeftMargin.X - leftSkip * FontWidth;
            startPoint.Y = LeftMargin.Y;

            // restrict drawing to just the text part.
            tc.SelectClipRegion(LeftMargin.X - 1, LeftMargin.Y - 1, clientSize.Width, clientSize.Height);

            // todo fix end check here.
            while(!scanner.AtEnd)
            {
                var lineOffset = scanner.Position - scanner.StartPositionOfLine;
                var type = scanner.NextToken();

                // handle special cases of Eol and not visible
                if (type == TokenType.Eol)
                {
                    startPoint.X = LeftMargin.X - leftSkip * FontWidth;
                    startPoint.Y += FontHeight;
                    if (--rows <= 0)
                    {
                        break;
                    }

                    continue;
                }

                // check if visible, skip if not.
                if ((lineOffset + scanner.Token.Length) < leftSkip || lineOffset > cols)
                {
                    continue;
                }


                switch (type)
                {
                    case TokenType.WhiteSpace:
                        startPoint.X += scanner.Token.Length * FontWidth;
                        continue;

                    case TokenType.End: // EOD
                        break;

                    //case TokenType.Eol:
                    //    startPoint.X = LeftMargin.X;
                    //    startPoint.Y += FontHeight;
                    //    //int width = lineOffset;
                    //    //if (width > RowColSize.Width) RowColSize.Width = width;
                    //    --Rows;
                    //    continue;


                    default:
                        var foreColor = SetFont(type);
                        Draw(foreColor);
                        startPoint.X += FontSize.Width;
                        continue;
                }

                //Rectangle rect = new Rectangle(startPoint, size);
            } 
            tc.EndDrawing(dev);

            RowColSize = new Size(_scanner.MaxLineLength, _scanner.LineCount);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Show Buffer as binary data.
        /// </summary>
        /// <param name="dev">        The dev.</param>
        /// <param name="scanner">    The buffer.</param>
        /// <param name="buffer">      The buffer.</param>
        /// <param name="clientSize"> The client Size.</param>
        /// <param name="rowStart">   The top row 0 based.</param>
        /// <param name="colStart">   The horizontal Percent.</param>
        /// <param name="zoom">       The zoom.</param>
        /// <param name="colCount">   [optional=16] The col Count.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void ShowBinBuffer(Graphics dev, IScanner scanner, ReadBuffer buffer, Size clientSize, int rowStart,
            int colStart, float zoom, int colCount )
        {


            _scanner = scanner;

            if (_binChars == null) _binChars = new char[colCount];

            if (Math.Abs(_zoom - zoom) > .05)
            {
                _zoom = zoom;
                CreateFonts(EmSize);
            }

            tc.StartDrawing(dev, BackgroundColor, 4, fonts[0]);

            FontSize = tc.MeasureText("M");
            FontWidth = FontSize.Width;
            FontHeight = FontSize.Height;
            var rows = clientSize.Height / FontHeight;
            var cols = clientSize.Width / FontWidth;
            var leftSkip = colStart;

            // skip lines above view.
            _scanner.MoveToLine(rowStart);

            int startAddr = rowStart * colCount;
            LeftMargin.X = DrawAddressNumbers(startAddr, buffer.Length, colCount, rows, clientSize.Height);
            startPoint.X = LeftMargin.X - leftSkip * FontWidth;
            startPoint.Y = LeftMargin.Y;

            // restrict drawing to just the text part.
            tc.SelectClipRegion(LeftMargin.X - 1, LeftMargin.Y - 1, clientSize.Width, clientSize.Height);

            for (int r = 0; r < rows; ++r)
            {
                var lineOffset = 0;

                // format text rows
                var start = (r + rowStart) * colCount;
                for (int i = 0; i < colCount && i + start < buffer.Length; ++i)
                {
                    byte b = buffer.Text[i + start];
                    _scanner.Token.Append(HEX[(b >> 4) & 0xf]);
                    _scanner.Token.Append(HEX[b & 0xf]);
                    _scanner.Token.Append(' ');

                    _binChars[i] = b < 32 ? '.' : (char) b;
                }

                // check if visible, skip if not.
                if ((lineOffset + scanner.Token.Length) < leftSkip || lineOffset > cols)
                {
                    continue;
                }

                var foreColor = SetFont(TokenType.Punctuation);
                Draw(foreColor);
                startPoint.X += FontSize.Width;

                int w = startPoint.X;
                _scanner.Token.Clear();

                _scanner.Token.Append(' ');
                for (int i = 0; i < colCount; ++i)
                {
                    _scanner.Token.Append(_binChars[i]);
                }

                // check if visible, skip if not.
                if ((lineOffset + scanner.Token.Length) < leftSkip || lineOffset > cols)
                {
                    continue;
                }

                 foreColor = SetFont(TokenType.Operator);
                Draw(foreColor);
                startPoint.X += FontSize.Width;

                tc.SelectSolidPen(1, Color.DIM_GRAY_DIM_GREY);
                tc.DrawLine(w, 0, w, clientSize.Height);

            }

            tc.EndDrawing(dev);

            RowColSize = new Size(_scanner.MaxLineLength, _scanner.LineCount);
        }

    }
}
