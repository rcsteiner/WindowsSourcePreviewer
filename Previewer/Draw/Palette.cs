////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: Defines a Palette to use for text based on Token Type
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
    ///  The Pallete Class definition.  Defines a Palette to use for text based on Token Type. 
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class Palette
    {
        /// <summary>
        ///  Get/Set Pallete.
        /// </summary>
        public ColorMap[] Colors { get;  }

        /// <summary>
        ///  Get/Set File Path.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        ///  Get/Set Name.
        /// </summary>
        public string Name { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="filePath"> The file Path.</param>
        /// <param name="name">     The name.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Palette(string filePath = null, string name = null)
        {
            FilePath = filePath;
            Name     = name;
            Colors   = new ColorMap[32];
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>
        /////  Method: Add.
        ///// </summary>
        ///// <param name="typeId"> The type identifier.</param>
        ///// <param name="color">  The color.</param>
        ///// <param name="style">  The style.</param>
        ///// <returns>
        /////  The integer value.
        ///// </returns>
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //public int Add(int typeId, Color color, FontStyle style)
        //{
        //    for (int i = Colors.Length; i <= typeId; ++i)
        //    {
        //        Colors.Add(new ColorMap(typeId, color, style));
        //    }
        //    Colors[typeId] = new ColorMap(typeId, color, style);
        //    return typeId;
        //}

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Get Color based on pallete index.
        /// </summary>
        /// <param name="id"> The identifier.</param>
        /// <returns>
        ///  The Win32.Color value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Color GetColor(int id)
        {
            if (id >= Colors.Length) id = 0;
            return Colors[id].Color;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Get Font Style.
        /// </summary>
        /// <param name="id">  The identifier.</param>
        /// <returns>
        ///  The System.Drawing.FontStyle value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public int GetFontId(int id)
        {
            if (id >= Colors.Length) id = 0;
            return Colors[id].FontId;
        }

    }
}



