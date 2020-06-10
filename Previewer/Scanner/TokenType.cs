////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: Type of tokens.
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
    ///  The Token Type structure definition. Defines a byte value to mark the token information, from the
    ///  Token Info table. This makes the field a small cost in data structures.
    ///  A TokenType has
    ///  Color (color based on the palette)
    ///  FontStyle (Font style info)
    ///  Id (index into the TokenType Table)
    ///  Name (used for loading and saving)
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public struct TokenType
    {
        /// <summary>
        ///  The index field.  This is the only storage allocated for a token Type
        /// </summary>
        public byte Id { get; set; }

        /// <summary>
        ///  Get/Set Value as a Color.
        /// </summary>
        public Color Color { get { return ColorManager.Palette.GetColor(Id); } }

        /// <summary>
        ///  Get/Set Value as a Color.
        /// </summary>
        public int FontId { get { return ColorManager.Palette.GetFontId(Id); } }

        /// <summary>
        ///  Get Name, cannot be changed.
        /// </summary>
        public string Name { get { return ColorManager.TypeTable[Id]; } }

        /// <summary>
        ///  The Color Manager field.
        /// </summary>
        public static ColorManager ColorManager = new ColorManager();

        ///// <summary>
        /////  The Names field.
        ///// </summary>
        //public static string[] Names =
        //    {
        //        "Default","Background","LineNumber", "End", "EOL", "Whitespace", "Comment", "Operator", "String", "Character", "Number",
        //        "Punctuation","Preprocessor","Variable", "Keyword1","Keyword2","Keyword3","Keyword4","Keyword5","Keyword6" ,"Keyword7","Keyword8"
        //    };


        /// <summary>
        ///  The Default field.
        /// </summary>
        public const int Default = 0;

        /// <summary>
        ///  The Background field.
        /// </summary>
        public const int Background = 1;

        /// <summary>
        ///  The Line Number field.
        /// </summary>
        public const int LineNumber = 2;
        /// <summary>
        ///     The End enum value.
        /// </summary>
        public const int End = 3;

        /// <summary>
        ///     The Eol enum value.
        /// </summary>
        public const int Eol = 4;

        /// <summary>
        ///     The White Space enum value.
        /// </summary>
        public const int WhiteSpace = 5;

        /// <summary>
        ///     A Comment.
        /// </summary>
        public const int Comment = 6;

        /// <summary>
        ///     An operator
        /// </summary>
        public const int Operator = 7;

        /// <summary>
        ///     A String
        /// </summary>
        public const int String = 8;

        /// <summary>
        ///     A Char
        /// </summary>
        public const int Char = 9;

        /// <summary>
        ///     A number
        /// </summary>
        public const int Number = 10;

        /// <summary>
        ///     The Punctuation enum value.
        /// </summary>
        public const int Punctuation = 11;

        /// <summary>
        ///     The Preprocessor enum value.
        /// </summary>
        public const int Preprocessor = 12;

        /// <summary>
        ///     A Variable
        /// </summary>
        public const int Variable = 13;

        /// <summary>
        ///     AKeywords that are not types or control.
        /// </summary>
        public const int Keyword1 = 14;

        /// <summary>
        ///     AKeywords that are not types or control.
        /// </summary>
        public const int Keyword2 = 15;

        /// <summary>
        ///     AKeywords that are not types or control.
        /// </summary>
        public const int Keyword3 = 16;

        /// <summary>
        ///     AKeywords that are not types or control.
        /// </summary>
        public const int Keyword4 = 17;

        /// <summary>
        ///     AKeywords that are not types or control.
        /// </summary>
        public const int Keyword5 = 18;

        /// <summary>
        ///     AKeywords that are not types or control.
        /// </summary>
        public const int Keyword6 = 19;

        /// <summary>
        ///     AKeywords that are not types or control.
        /// </summary>
        public const int Keyword7 = 20;

        /// <summary>
        ///     AKeywords that are not types or control.
        /// </summary>
        public const int Keyword8 = 21;


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Constructor: For TokenType.
        /// </summary>
        /// <param name="value"> The value.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private TokenType(int value)
        {
            Id = (byte)value;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Operator Overload: .
        /// </summary>
        /// <param name="value"> The value.</param>
        /// <returns>
        ///  The PanelSourceView.Parser.TokenType result of the TokenType operation
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static implicit operator TokenType(int value) { return new TokenType(value); }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Operator Overload: .
        /// </summary>
        /// <param name="value"> The value.</param>
        /// <returns>
        ///  The integer result of the integer operation
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static implicit operator int(TokenType value) { return value.Id; }


        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>The fully qualified type name.</returns>
        public override string ToString()
        {
            return ColorManager?.TypeTable[Id];
        }
    }
}

