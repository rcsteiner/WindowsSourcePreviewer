////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: Token used to build elements.
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

namespace Scan
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Token Reference Class definition, fits in 64 bits.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class Token : IToken
    {
        /// <summary>
        ///  Get Length of the token.
        /// </summary>
        public int Length { get;  set; }

        /// <summary>
        ///  Get Text char[] of the token.
        /// </summary>
        public char[] Text { get; }

        /// <summary>
        ///  Get Type of token.
        /// </summary>
        public TokenType Type { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="maxLength"> [optional=1000] The maximum Length.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Token(int maxLength = 2000)
        {
            Text = new char[maxLength];
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Append.
        /// </summary>
        /// <param name="c"> The char c.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Append(char c)
        {
            if (c!= '\0' && Length < Text.Length)
            {
                Text[Length++] = c;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Binary Search.
        /// </summary>
        /// <param name="wordList">  The word List.</param>
        /// <returns>
        ///  The integer value or -1 if not found.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private int BinarySearch(List<IDMap> wordList)
        {
            if (wordList == null) return -1;

            int min = 0;
            int max = wordList.Count - 1;
            int d;

            while (min <= max)
            {
                int mid = (min + max) / 2;
                d = Compare(wordList[mid].Text);
                if (d == 0)
                {
                    return mid;
                }

                if (d < 0)
                {
                    max = mid - 1;
                }
                else
                {
                    min = mid + 1;
                }
            }
            return -1;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Reset the token to empty.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Clear()
        {
            Length = 0;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Determines whether the specified text is equal at the current read position. Must be a valid
        ///  token and case-sensitive.
        /// </summary>
        /// <param name="text"> The text enumerator.</param>
        /// <returns>
        ///  <c>true</c> if the specified text is equal, adjusts the position by text length; otherwise, <c>false</c>.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public int Compare(string text)
        {
            int d;

            for (var index = 0; index < text.Length && index < Length; index++)
            {
                d = Text[index] - text[index];
                if (d != 0) return d;
            }

            return Length - text.Length;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Search Keyword.
        /// </summary>
        /// <param name="keywords"> The keywords.</param>
        /// <returns>
        ///  The integer value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public int SearchKeyword(List<IDMap> keywords)
        {
            return  BinarySearch(keywords);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Set.
        /// </summary>
        /// <param name="integer">  The integer.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Append(int integer)
        {
            if (integer < 0)
            {
                Append('-');
                integer = -integer;
            }
            RecursiveNumToString(integer);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Set Number.
        /// </summary>
        /// <param name="number">  The number.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void RecursiveNumToString(int number)
        {
            if (number > 10)
            {
                RecursiveNumToString(number / 10);
            }
            Append((char)((number % 10) + '0'));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: To Bool.
        /// </summary>
        /// <returns>
        ///  True if successful
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool ToBool()
        {
            return Compare("true") == 0 || Compare("false") == 0;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: To Double.
        /// </summary>
        /// <returns>
        ///  The double value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public double ToDouble()
        {
            double v = 0;
            double div = 1;
            int exp = 0;

            bool inFract = false;
            for (int i = 0; i < Length; ++i)
            {
                var c = Text[i];
                if (char.IsDigit(c))
                {
                    v = v * 10 + c - '0';
                    if (inFract)
                    {
                        div = div * 10;
                    }
                }
                else if (c == '.')
                {
                    inFract = true;
                }
                else // exponent  e or E
                {
                    inFract = false;
                    ++i;
                    c = Text[i];
                    var sign = c == '-';
                    if (sign || c == '+')
                    {
                        ++i;
                    }
                    for (; i < Length; ++i)
                    {
                        exp = exp * 10 + Text[i] - '0';
                    }

                    if (sign) exp = -exp;
                }
            }

            // compute actual value

            v = v / div * Math.Pow(10, exp);

            return v;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: To Long.
        /// </summary>
        /// <returns>
        ///  The long value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public long ToLong()
        {
            long v = 0;
            for (int i = 0; i < Length; ++i)
            {
                v = v * 10 + Text[i] - '0';
            }

            return v;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Get a string representation of Token.
        /// </summary>
        /// <param name="start">   The start.</param>
        /// <param name="length">  The length.</param>
        /// <returns>
        ///  The string representation
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public string ToString(int start, int length)
        {
            return new string(Text, start, length);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Get a string representation of Token.
        /// </summary>
        /// <returns>
        ///  The string representation
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override string ToString()
        {
            return new string(Text, 0, Length);
        }
    }
}
