////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: Manage the palette and Token Type table.
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
using System.Collections.Generic;
using System.Drawing;
using SourcePreview;

namespace Scan
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Color Manager Class definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class ColorManager
    {
        /// <summary>
        ///  Get/Set Palette.
        /// </summary>
        public Palette Palette { get { return PaletteManager[PaletteId]; } }

        /// <summary>
        ///  Get/Set Palette identifier.  (The pallete to use now)
        /// </summary>
        public int PaletteId { get; set; }

        /// <summary>
        ///  Get Palette Manager.
        /// </summary>
        public List<Palette> PaletteManager { get; }

        /// <summary>
        ///  This is the names of the type table.
        /// </summary>
        public readonly List<string> TypeTable = new List<string>(32);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Initializes a new instance of the <see cref="T:System.Object" /> class.
        ///  Loads palettes and sets it to the first palette.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public ColorManager()
        {
            PaletteManager = new List<Palette>(4);
            var buffer = new StreamBuffer();
            foreach (var name in buffer.GetResourceStreams(".palette"))
            {
                var p = Load(buffer);
                PaletteManager.Add(p);
            }

            PaletteId = 0;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Load.
        ///  format:
        ///  Name="Pallete Name"
        ///  TypeName = 0xffffff FontStyle
        ///  
        ///  Styles are : Regular = 0,
        ///  Bold = 1,Italic = 2,Underline = 4, Strikeout = 8,
        /// </summary>
        /// <param name="buffer">       The buffer to read from.</param>
        /// <returns>
        ///  True if successful
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Palette Load(IBuffer buffer)
        {
            string typeName;
            var p = new Palette();

            var styles   = Enum.GetNames(typeof(FontStyle));
            var _scanner = new InfoScanner();

            _scanner.Initialize(buffer, null);

            // Get name

            p.FilePath = buffer.Path;
            p.Name = _scanner.ParseString("Name", true);
            int count = 0;

            _scanner.NextLine();

            // Get color pallete slot names and font style BGR order of bytes.
            //    Name       = "Default.palette"
            //    Default    = 0xffffff Regular
            //    Background = 0x000000 Regular
            //    LineNumber = 0xED9564 Regular
            //    End        = 0xD3D3D3 Regular
            //    EOL        = 0xD3D3D3 Regular
            //    Whitespace = 0xD3D3D3 Regular

            while ((typeName = _scanner.ParseText()) != null)
            {
                ++count;
                uint color = 0;
                if (_scanner.Accept('='))
                {
                    var ctxt = _scanner.ParseText();
                    int n = (ctxt.StartsWith("0x")) ? 2 : 0;

                    while (n < ctxt.Length)
                    {
                        color = color * 16 + (uint)Scanner.HEX_DIGITS.IndexOf(char.ToUpper(ctxt[n++]));
                    }

                }
                var style = _scanner.ParseText();
                _scanner.NextLine();
                var fontId = Array.IndexOf(styles, style);
                if (fontId < 0) fontId = 0;


                // now we have, color, typeName, and style.
                // see if name is in the TypeTable.
                int id;
                for (id = 0; id < TypeTable.Count; ++id)
                {
                    if (TypeTable[id].Equals(typeName, StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }
                }

                // id is the type id, so set the current palette color and font style for that id.
                if (id >= TypeTable.Count)
                {
                    TypeTable.Add(typeName);
                }

                p.Colors[id] = new ColorMap(id, color, fontId);
            }

            return p;
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>
        /////  Method: Create Default.
        ///// </summary>
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //private void CreateDefault()
        //{
        //    var p = new Palette("Default.palette", "Default.palette");
        //    // setup defaults.
        //    foreach (var name in TokenType.Names)
        //    {
        //        var tokenType = Add(name, 0x808080);
        //        p.Add(tokenType, Color.LIGHT_GRAY_LIGHT_GREY, FontStyle.Regular);
        //    }

        //    PaletteManager.Add(p);
        //    //  Palette.Save(@"h:\Colors\Default.palette");
        //}


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>
        /////  Method: Load.
        ///// </summary>
        ///// <param name="filePath"> The file Path.</param>
        ///// <returns>
        /////  True if successful
        ///// </returns>
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //public bool LoadFromFile(string filePath)
        //{
        //    // todo reuse buffer
        //    var buffer = new AsciiBuffer();
        //    buffer.Load(filePath);
        //    return Load(buffer);
        //}

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>
        /////  Method: Save.
        /////  Format: 'Keyword' TypeName { Name1 Name2 ... } can have multiple keyword groups.
        /////  'Operators' TypeName "+/*..."
        /////  'Delimiter' TypeName "\"" "\\" "\""
        /////  'Delimiter' TypeName "'" "\\" "'"
        /////  'Delimiter' TypeName "$\"" "\"\""
        /////  'Block' TypeName "{" "}"
        /////  'Comment' TypeName "//" "\n"
        /////  'Comment' TypeName "/*" "*/"
        ///// </summary>
        ///// <param name="filePath"> The file Path.</param>
        ///// <returns>
        /////  True if successful
        ///// </returns>
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //public bool Save(string filePath = null)
        //{
        //    var styles = Enum.GetNames(typeof(FontStyle));

        //    var dest = new InfoBuilder();
        //    dest.WriteProperty("Name", true, true, Name);
        //    int w = 0;
        //    foreach (var c in Colors)
        //    {
        //        var n = TokenType.Names[c.TypeId].Length;
        //        if (n > w) w = n;
        //    }

        //    foreach (var c in Colors)
        //    {
        //        var name = TokenType.Names[c.TypeId];
        //        var typeName = name + new string(' ', w - name.Length);
        //        dest.WriteProperty(typeName, false, true, $"0x{c.Color.Value:X6}", styles[(int)c.FontStyle]);
        //    }

        //    return dest.Save(filePath == null ? FilePath : filePath);
        //}

    }

}
