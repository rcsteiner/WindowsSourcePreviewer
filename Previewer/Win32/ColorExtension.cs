////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: Extension to System.Color
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
namespace System.Drawing
{
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>
///  The Color Extension Class definition.
/// </summary>
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
public static class ColorExtension
    {

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Test if this color is Dark
        /// </summary>
        /// <param name="color">  The color.</param>
        /// <returns>
        ///  True if successful
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static bool IsDark(this Color color)
        {
            return color.R * 0.299 + color.G * 0.587 + color.B * 0.114 < 186;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Lighten the color by an amount.
        /// </summary>
        /// <param name="color">     The color.</param>
        /// <param name="inAmount"> The in Amount.</param>
        /// <returns>
        ///  The Win32.Color value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static Color Lighten(this Color color, double inAmount)
        {
            return Color.FromArgb(255, SatMultiply(color.R, inAmount), SatMultiply(color.G, inAmount),
                SatMultiply(color.B, inAmount));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Support for saturated Multiply Add.
        /// </summary>
        /// <param name="component"> color component</param>
        /// <param name="inAmount">  The in Amount.</param>
        /// <returns>
        ///  The integer value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static int SatMultiply(uint component, double inAmount)
        {
            return (int)Math.Min(255, component + 255 * inAmount);
        }
    }
}
