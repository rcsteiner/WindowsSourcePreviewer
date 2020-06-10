////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: Info text scanner for resource and information based on x = y parsing.
// 
//  Author:      Robert C. Steiner
// 
//=====================================================[ History ]=====================================================
//  Date        Who      What
//  ----------- ------   ----------------------------------------------------------------------------------------------
//  6/7/2020    RCS      Initial Code.
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
using System.Collections.Generic;
using SourcePreview;

namespace Scan
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The information Scanner Class definition. Used to parse .lang and .palette files. 
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    internal class InfoScanner : Scanner
    {

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Constructor: For InfoScanner.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public InfoScanner() : base(null)
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Constructor: For InfoScanner.
        /// </summary>
        /// <param name="buffer"> The  Buffer to use for input.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public InfoScanner(IBuffer buffer) : base(buffer)
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Parse Array. (must be single line)
        ///  format: Name = { x y z }
        /// </summary>
        /// <param name="name"> [optional=null] The name. if No name, then just parses { x y z}</param>
        /// <returns>
        ///  The string[] value or null, the token is set to the name (if any).
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public List<string> ParseArray(string name = null)
        {
            var data = new List<string>();

            FlushWhitespace();
            if ((name == null) || (Accept(name) && Accept('=')))
            {
                while (!AtEnd)
                {
                    FlushWhitespace();
                    var  c = CurrentChar;
                    if (c == '\n')
                    {
                        return data;
                    }

                    if (c == '"')
                    {
                        ScanDelimited(c,TokenType.String);
                        data.Add(Token.ToString(1,Token.Length-2));
                    }
                    else
                    {
                        ScanWhileNot(c, WHITESPACE);
                        data.Add(Token.ToString());
                    }
                }
            }

            return data;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Parse Keywords.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public List<string> ParseMap(out List<short> map)
        {
            var data = new List<string>(64);
            map = new List<short>(64);

            FlushWhitespace();
                while (!AtEnd)
                {
                    FlushWhitespace();
                    if (CurrentChar == '\n')
                    {
                        MoveNext();
                        return data;
                    }

                    Token.Clear();
                    ScanWhileNot(CurrentChar, WHITESPACE);
                    data.Add(Token.ToString());
                    FlushWhitespace();
                    Token.Clear();
                    ScanWhile(CurrentChar,DIGITS);
                    map.Add((short)Token.ToLong());
                }

            return data;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Parse String.
        /// </summary>
        /// <param name="trimEnds"> [optional=false] True if trim Ends.</param>
        /// <returns>
        ///  The string value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public string ParseString(bool trimEnds = false)
        {
            FlushWhitespace();
            ScanDelimited(CurrentChar,TokenType.String);
            return trimEnds ? Token.ToString(1, Token.Length-2 ) : Token.ToString();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Parse an double quote delimited string
        ///  format: Name = "text of value"
        /// </summary>
        /// <param name="name">     The name.</param>
        /// <param name="trimEnds"> [optional=false] True if trim Ends.</param>
        /// <returns>
        ///  The string value or null if not found.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public string ParseString(string name, bool trimEnds = false)
        {
            if ((Accept(name) && Accept('=')))
            {
                FlushWhitespace();
                return ParseString(trimEnds);
            }

            return null;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Parse Text property.
        ///  Format: name = text
        /// </summary>
        /// <param name="name"> [optional=null] The name, if null then just parse token..</param>
        /// <returns>
        ///  The string value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public string ParseText(string name = null)
        {
            Token.Clear();
            if (!AtEnd && ((name == null) || (Accept(name) && Accept('='))))
            {
                FlushWhitespace();
                ScanWhile(CurrentChar, LETTERS_DIGITS);
                return Token.ToString();
            }

            return null;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Parse Type = of a command line.
        /// </summary>
        /// <returns>
        ///  The string line type and the parser is left after the =, blanks are flushed.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public string ParseType()
        {
            string type = null;

            FlushWhitespace();
            Token.Clear();
            if (!AtEnd)
            {
                FlushWhitespace();
                ScanWhile(CurrentChar, LETTERS);
                type = Token.ToString();
                Token.Clear();
                Accept('=');
                FlushWhitespace();
            }

            return type;
        }
    }
}

