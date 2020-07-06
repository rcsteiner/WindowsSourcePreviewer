////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: Color map, maps id to font/color and can be set to dark.
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
using Color = Win32.Color;

namespace Scan
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Color map, maps id to font/color and can be set to dark.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public struct ColorMap
    {
        /// <summary>
        ///  Get/Set Name assigned to this map entry. Maps to Token Type Names.
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        ///  Get/Set Color for a white background.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        ///  Get/Set Font identifier.
        /// </summary>
        public int  FontId{ get; set; }

        /// <summary>
        ///  Get Dark background version of the color
        /// </summary>
        public Color Dark { get { return Color.Lighten(1.5); } }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="typeId">     The type identifier.</param>
        /// <param name="color">     The color.</param>
        /// <param name="fontStyle"> The font Style.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public ColorMap(int  typeId, Color color, int fontStyle)
        {
            TypeId    = typeId;
            Color     = color;
            FontId    = fontStyle;
        }


    }
}
