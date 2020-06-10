////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description:Interface for Text Drawing context. Wrappers around win32
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

using System.Drawing;
using System.Text;

namespace Win32
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Interface for Text Drawing context. Wrappers around win32
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public interface ITextContext
    {

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Draw Text.
        /// </summary>
        /// <param name="text">  The text.</param>
        /// <param name="point">  The point.</param>
        /// <param name="color"> The color.</param>
        /// <param name="font">  [optional=null] The font.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        void DrawText(string text, Point point, Color color, Font font = null);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Draw Text.
        /// </summary>
        /// <param name="text">   The text.</param>
        /// <param name="offset"> The offset.</param>
        /// <param name="length"> The length.</param>
        /// <param name="point">   The point.</param>
        /// <param name="color">  The color.</param>
        /// <param name="font">   [optional=null] The font.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        void DrawText(char[] text, int offset, int length, Point point, Color color, Font font = null);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Draw Text.
        /// </summary>
        /// <param name="text">   The text.</param>
        /// <param name="length"> The length.</param>
        /// <param name="point">   The point.</param>
        /// <param name="color">  The color.</param>
        /// <param name="font">   [optional=null] The font.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        void DrawText(StringBuilder text,  int length, Point point, Color color, Font font = null);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: End Text.
        /// </summary>
        /// <param name="graphics"> The Graphics g.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        void EndDrawing(Graphics graphics);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Measure Text.
        /// </summary>
        /// <param name="text"> The text.</param>
        /// <returns>
        ///  The System.Drawing.Size value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        Size MeasureText(string text);

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
        Size MeasureText(StringBuilder text,  int length);

 
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Select Font
        /// </summary>
        /// <param name="font"> The font.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        void SelectFont(Font font);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Start Drawing.
        /// </summary>
        /// <param name="graphics">   The graphics.</param>
        /// <param name="background">  The background.</param>
        /// <param name="tabSize">    The tab Size.</param>
        /// <param name="font">       The font.</param>
        /// <param name="format">     [optional=(DrawFormat.DT_NOPREFIX | DrawFormat.DT_NOCLIP)] The format.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        void StartDrawing(Graphics graphics, Color background, int tabSize, Font font, DrawFormat format = (DrawFormat.DT_NOPREFIX | DrawFormat.DT_NOCLIP));
    }
}