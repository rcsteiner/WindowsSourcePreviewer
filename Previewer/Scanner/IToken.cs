////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: Interface for Token builder.
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
using System.Collections.Generic;

namespace Scan
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Token interface definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public interface IToken
    {
        /// <summary>
        ///  Get Length of the current token.
        /// </summary>
        int Length { get; }

        /// <summary>
        ///  Get Text buffer associated with the token.
        /// </summary>
        char[] Text { get; }

        /// <summary>
        ///  Get The syntax type of the token.
        /// </summary>
        TokenType Type { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Search Keyword array for this token text.
        /// </summary>
        /// <param name="keywords">  The keywords.</param>
        /// <returns>
        ///  The integer value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        int SearchKeyword(List<string> keywords);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Reset the token to empty.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        void Clear();

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Append a character to the token buffer.
        /// </summary>
        /// <param name="c"> The char  to append..</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        void Append(char c);


    }
}
