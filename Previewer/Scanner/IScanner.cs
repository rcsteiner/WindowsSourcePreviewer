////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: Interface for simple scanner.
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
using System.Collections.Generic;
using SourcePreview;

namespace Scan
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Scanner interface definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public interface IScanner
    {
        char CurrentChar { get; }

        /// <summary>
        ///  Get At End.
        /// </summary>
        bool AtEnd { get; }

        /// <summary>
        ///  Get/Set Column.
        /// </summary>
        int Column { get; }

        /// <summary>
        ///  Get/Set Language.
        /// </summary>
        ILanguage Language { get; set; }

        /// <summary>
        ///  Get/Set Line Count.
        /// </summary>
        int LineCount { get; }

        /// <summary>
        ///  Get Line Number.
        /// </summary>
        int LineNumber { get; set; }

        /// <summary>
        ///  Get/Set maximum Line Length.
        /// </summary>
        int MaxLineLength { get; }

        /// <summary>
        ///  Get/Set the current Offset.
        /// </summary>
        int Position { get; set; }

        /// <summary>
        ///  Get/Set Start Line offset.
        /// </summary>
        int StartPositionOfLine { get; }

        /// <summary>
        ///  The Token field.
        /// </summary>
        Token Token { get; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Accept the character if it matches.
        /// </summary>
        /// <param name="c">               The char c.</param>
        /// <param name="flushWhitespace"> [optional=true] True if flush Whitespace.</param>
        /// <returns>
        ///  True if successful
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        bool Accept(char c, bool flushWhitespace=true);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Accept the text if it matches.
        /// </summary>
        /// <param name="text">             The text.</param>
        /// <param name="flushWhitespace"> [optional=true] True if flush Whitespace.</param>
        /// <returns>
        ///  True if successful
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        bool Accept(string text, bool flushWhitespace=true);


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Append Move Next.
        /// </summary>
        /// <param name="c"> The char c.</param>
        /// <returns>
        ///  The char value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        char AppendMoveNext(char c);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Count Lines and measure size of lines in characters.
        /// </summary>
        /// <returns>
        ///  The number of lines in the buffer.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        int CountLines();

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Get the position to the End Of Line.
        /// </summary>
        /// <returns>
        ///  Move to the end of the line (or buffer), then new position (after the eol)
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        int EndOfLine();

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Get Tokens, from the buffer one Token Reference at a time.
        /// </summary>
        /// <returns>
        ///  The next token in the token stream.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        IEnumerable<TokenType> GetTokens();

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Initialize the parser to use a StoreBuffer for it's source.
        /// </summary>
        /// <param name="buffer">   The buffer.</param>
        /// <param name="language"> The language.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        void Initialize(IBuffer buffer, ILanguage language);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Load.
        /// </summary>
        /// <param name="filePath"> The file Path.</param>
        /// <returns>
        ///  True if successful
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        bool Load(string filePath);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Move Next character
        /// </summary>
        /// <returns>
        ///  The char value or \0 if end.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        char MoveNext();

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Move To Line number, 1 based. Sets any block flags needed for comment scoping or range scoping.
        /// </summary>
        /// <param name="lineNumber"> The line Number to move to.</param>
        /// <returns>
        ///  The integer value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        int MoveToLine(int lineNumber);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Get Next Offset.
        /// </summary>
        /// <returns>
        ///  The PanelSourceView.Parser.IToken value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        TokenType NextToken();

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Peek Next Char.
        /// </summary>
        /// <returns>
        ///  The char value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        char PeekNextChar();

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Resets the parser to the start of the text source.
        /// </summary>
        /// <param name="position"> [optional=0] The position.</param>
        /// <returns>
        ///  The char value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        char Reset(int position = 0);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Flush a delimited block. Current character is also the end delimiter. Position is set to the position
        ///  following the end delimiter. Note that this scans to the end delimiter, carriage return, line feed or end
        ///  of buffer.
        /// </summary>
        /// <param name="c">      The char c.</param>
        /// <param name="type">   The token type to return.</param>
        /// <param name="escape"> The escape character (defaults to '\'.</param>
        /// <returns>
        ///  The Scan.TokenType value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        TokenType ScanDelimited(char c, TokenType type, char escape = '\\');

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Scan End of line.
        /// </summary>
        /// <returns>
        ///  The Scan.TokenType value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        TokenType ScanEol();

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Scan Name and check for keyword else variable.
        /// </summary>
        /// <param name="c"> The char c.</param>
        /// <returns>
        ///  The Scan.TokenType value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        TokenType ScanName(char c);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Scan Number.
        /// </summary>
        /// <param name="c">  The char c.</param>
        /// <returns>
        ///  The Scan.TokenType value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        TokenType ScanNumber(char c);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Scan Operators
        /// </summary>
        /// <param name="c"> The char c.</param>
        /// <returns>
        ///  The Scan.TokenType value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        TokenType ScanOperator(char c);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Scan Punctuation marks
        /// </summary>
        /// <param name="c"> The char c.</param>
        /// <returns>
        ///  The Scan.TokenType value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        TokenType ScanPunctuation(char c);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Scan Variable.
        /// </summary>
        /// <param name="c">  The char c.</param>
        /// <returns>
        ///  The Scan.TokenType value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        TokenType ScanVariable(char c);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Scan Whitespace.
        /// </summary>
        /// <param name="c"> The char c.</param>
        /// <returns>
        ///  The Scan.TokenType value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        TokenType ScanWhitespace(char c);
    }
}






