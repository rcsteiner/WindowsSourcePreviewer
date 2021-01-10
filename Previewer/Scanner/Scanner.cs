////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: Scanner base class
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
using System.IO;
using System.Runtime.InteropServices;
using SourcePreview;

namespace Scan
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The basic text scanner.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class Scanner : IScanner
    {
        /// <summary>
        ///  Get At End.
        /// </summary>
        public bool AtEnd { get { return Buffer.AtEnd; } }

        /// <summary>
        ///  Get/Set Column.
        /// </summary>
        public int Column { get; private set; }

        /// <summary>
        ///  Get Current Char.
        /// </summary>
        public char CurrentChar { get { return Buffer.CurrentChar; } }

        /// <summary>
        ///  Get/Set Language.
        /// </summary>
        public ILanguage Language { get; set; }

        /// <summary>
        ///  Get Line Count.
        /// </summary>
        public int LineCount { get { return _lineCount >= 0 ? _lineCount : CountLines(); } }

        /// <summary>
        ///  Get Line Number.
        /// </summary>
        public int LineNumber { get; set; }

        /// <summary>
        ///  Get/Set maximum Line Length.
        /// </summary>
        public int MaxLineLength { get; set; }

        /// <summary>
        ///  Get/Set Offset.
        /// </summary>
        public int Position { get { return Buffer.Position; } set { Buffer.Position = value; } }

        /// <summary>
        ///  Get/Set Start position of Line.
        /// </summary>
        public int StartPositionOfLine { get; private set; }

        /// <summary>
        ///  Get/Set Tab Size.
        /// </summary>
        public int TabSize { get; set; }

        /// <summary>
        ///  The token field.
        /// </summary>
        public Token Token { get; }

        /// <summary>
        ///  The Char Classifier field.
        /// </summary>
        public ICharClassifier CharClassifier;

        /// <summary>
        ///  digits s.
        /// </summary>
        public const string DIGITS = "0123456789";

        /// <summary>
        ///  Hex digits in both cases.
        /// </summary>
        public const string HEX_DIGITS = "0123456789ABCDEFabcdef";

        /// <summary>
        ///  The LETTERS field.
        /// </summary>
        public const string LETTERS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        ///  The LETTER and digits field.
        /// </summary>
        public const string LETTERS_DIGITS = LETTERS + DIGITS + "_@{}.?/\\*%$#-+><=!:;";

        /// <summary>
        ///  The line Count field.
        /// </summary>
        private int _lineCount = -1;

        /// <summary>
        ///  The buffer containing the raw text.
        /// </summary>
        protected IBuffer Buffer;

        /// <summary>
        ///  The WHITESPACE field.
        /// </summary>
        protected const string WHITESPACE = " \t\r";

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Constructor: For StoreParser.
        /// </summary>
        /// <param name="buffer"> The buffer.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Scanner(IBuffer buffer)
        {
            Buffer = buffer;
            Token = new Token();
            Column = 0;
            TabSize = 4;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Accept the text if it matches, flushes any whitespace.
        /// </summary>
        /// <param name="c">               The char c.</param>
        /// <param name="flushWhitespace"> [optional=true] True if flush Whitespace.</param>
        /// <returns>
        ///  True if successful
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool Accept(char c, bool flushWhitespace = true)
        {
            if (flushWhitespace) ScanWhitespace(CurrentChar);
            Token.Clear();
            // and match the text string.
            if (CurrentChar != c)
            {
                return false;
            }

            MoveNext();

            return true;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Accept the text if it matches, flushes any whitespace.
        /// </summary>
        /// <param name="text">            The text.</param>
        /// <param name="flushWhitespace"> [optional=true] True if flush Whitespace.</param>
        /// <returns>
        ///  True if successful
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool Accept(string text, bool flushWhitespace = true)
        {
            if (flushWhitespace) ScanWhitespace(CurrentChar);

            if (CurrentChar != text[0]) return false;       // fast check!

            // save position
            // and match the text string.
            var pos = Buffer.Position - 1;
            var b = Token.Length;

            char cc = AppendMoveNext(CurrentChar);


            for (var index = 1; index < text.Length; index++)
            {
                var c = text[index];
                if (cc != c)
                {
                    // if not matched then restore position and 
                    // return false
                    Buffer.Reset(pos);
                    Token.Length = b;
                    return false;
                }
                cc = AppendMoveNext(cc);

            }

            return true;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Append to token buffer and Move Next.
        /// </summary>
        /// <param name="c"> The char c.</param>
        /// <returns>
        ///  The char value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public char AppendMoveNext(char c)
        {
            Token.Append(c);
            return MoveNext();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Count Lines and measure size of lines in characters.
        /// </summary>
        /// <returns>
        ///  The number of lines in the buffer.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public int CountLines()
        {
            if (AtEnd) return 0;

            _lineCount = 0;
            MaxLineLength = 0;

            int start = 0;
            int old = Position;

            Position = 0;
            bool hasText = false;
            for (int i = 0; !AtEnd; ++i)
            {
                if (MoveNext() == '\n')
                {
                    hasText = false;
                    ++_lineCount;
                    Column = 0;
                    var len = i - start;
                    if (len > MaxLineLength) MaxLineLength = len;
                    start = i + 1;
                    continue;
                }

                hasText = true;
            }

            if (hasText) ++_lineCount;

            // reset the position and current character.
            Reset(old);
            return _lineCount;

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Get the offset to the End Of Line.
        /// </summary>
        /// <returns>
        ///  Position of end of the line (or buffer)
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public int EndOfLine()
        {
            //char c=Buffer.CurrentChar;
            //while (c != '\n' && (c = MoveNext()) != '\0')
            char c = CurrentChar;
            while (c != '\0' && c != '\n')
            {
                ++Column;
                c = AppendMoveNext(c);
            }

            return Position;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Flush Whitespace and clears the token.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void FlushWhitespace()
        {
            ScanWhitespace(CurrentChar);
            Token.Clear();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Get Classifier.
        /// </summary>
        /// <param name="language"> The language.</param>
        /// <returns>
        ///  The Scan.ICharClassifier value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private ICharClassifier GetClassifier(ILanguage language)
        {
            if (language != null && !string.IsNullOrEmpty(language.Name))
            {
                if (Language.Name == "html" || Language.Name == "xml")
                {
                    return new HtmlCharClassifier(this);
                }

                if ((language.BlockComment != null || language.LineComment != null))
                {
                    return new BlockCommentCharClassifier(this);
                }
            }

            return new CharClassifier(this);

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Get Position from the row and column.
        /// </summary>
        /// <param name="row">    The row in the buffer (line number).</param>
        /// <param name="column"> The column in the row adjusted for left side clipping.</param>
        /// <returns>
        ///  The integer value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public int GetPosition(int row, int column)
        {
            MoveToLine(row);

            // move back to start of line and scan to position

            while (Column < column)
            {
                if (CurrentChar == '\t')
                {
                    Column += TabSize - (Column % TabSize);
                    if (Column > column)
                    {
                        break;
                    }
                }
                MoveNext();
            }

            return Position - 1;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Get Tokens.
        /// </summary>
        /// <returns>
        ///  Fills out the token information on the next token.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public IEnumerable<TokenType> GetTokens()
        {
            Reset();
            while (!AtEnd)
            {
                yield return NextToken();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Initialize.
        /// </summary>
        /// <param name="buffer">   The buffer.</param>
        /// <param name="language"> The language.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Initialize(IBuffer buffer, ILanguage language)
        {
            this.Buffer = buffer;
            Language = language;
            _lineCount = -1;
            CharClassifier = GetClassifier(language);
            Reset();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Load a file and Initialize the scanner.
        /// </summary>
        /// <param name="filePath"> The file Path.</param>
        /// <returns>
        ///  True if successful
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool Load(string filePath)
        {
            if (Buffer.Load(filePath))
            {
                Initialize(Buffer, Scan.Language.Manager[Path.GetExtension(filePath)]);
                return true;
            }

            return false;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Move Next character.
        /// </summary>
        /// <returns>
        ///  The char value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public char MoveNext()
        {
            ++Column;
            return Buffer.MoveNext();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Move To Line. Scan only for multiline blocks like block comments etc.
        /// </summary>
        /// <param name="lineNumber"> The line Number.</param>
        /// <returns>
        ///  The new line number.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public int MoveToLine(int lineNumber)
        {
            StartPositionOfLine = 0;
            LineNumber = 1;
            Buffer.Reset();
            CharClassifier.Reset();

            while (lineNumber > LineNumber && !AtEnd)
            {
                char c = CurrentChar;
                while (c != '\0' && CharClassifier.ClassifyCharacter(c) != TokenType.Eol)
                {
                    Token.Clear();              // throw  away the line
                    c = CurrentChar;
                }
            }

            StartPositionOfLine = Position;
            Column = 0;
            return LineNumber;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Move to start of Next Line.
        ///  Moves over eol character to first character of next line, clears token.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void NextLine()
        {
            EndOfLine();
            if (CurrentChar == '\n') MoveNext();
            Token.Clear();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Next Token.
        /// </summary>
        /// <returns>
        ///  The SourcePreview.TokenType value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public TokenType NextToken()
        {
            Token.Clear();

            // todo fix end check here
            return CurrentChar == '\0' ? (TokenType)TokenType.End : (TokenType)CharClassifier.ClassifyCharacter(CurrentChar);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Look ahead 1 character.
        /// </summary>
        /// <returns>
        ///  The char value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public char PeekNextChar()
        {
            return Buffer.PeekNextChar();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Reset to begin processing the buffer. Normally called after changing the buffer source.
        /// </summary>
        /// <param name="pos"> [optional=0] The position.</param>
        /// <returns>
        ///  The char value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public char Reset(int pos = 0)
        {
            Token.Clear();
            Column = 0;
            StartPositionOfLine = 0;
            LineNumber = 1;
            return Buffer.Reset(pos);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Flush a delimited block. Position is set to the position
        ///  following the end delimiter. Note that this scans to the end delimiter, carriage return, line feed or end
        ///  of buffer.
        /// </summary>
        /// <param name="c">         The char c.</param>
        /// <param name="type">      The token type to return.</param>
        /// <param name="delimiter"> The delimiter.</param>
        /// <returns>
        ///  The Scan.TokenType value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public TokenType ScanDelimited(char c, TokenType type, Delimiter delimiter)
        {
            //var start = delimiter.Start[0];
            //var end = delimiter.End[0];
            var esc = string.IsNullOrEmpty(delimiter.Escape) ? '\0' : delimiter.Escape[0];
            return ScanDelimited(c, type, esc);
        }

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
        public TokenType ScanDelimited(char c, TokenType type, char escape = '\\')
        {
            if (!AtEnd)
            {
                var end = c;
                c = AppendMoveNext(c);       // add delimiters so caller can take them off if trim is true.

                while (!AtEnd && c != '\r' && c != '\n' && c != end)
                {
                    if (c == escape)          // always accept charactter after delimiter.
                    {
                       c = AppendMoveNext(c);
                    }
                    c = AppendMoveNext(c);

                }
                if (c == end)
                {
                    c = AppendMoveNext(c);
                }
            }
            return type;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Flush a string of digits only.
        /// </summary>
        /// <param name="c"> The char c.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void ScanDigits(char c)
        {
            ScanWhile(c, DIGITS);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Scan Eol.
        /// </summary>
        /// <returns>
        ///  The Scan.TokenType value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public TokenType ScanEol()
        {
            Column = 0;
            ++LineNumber;
            MoveNext();
            StartPositionOfLine = Position;
            return TokenType.Eol;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Flush a string of hex digits only.
        /// </summary>
        /// <param name="c"> The char c.</param>
        /// <returns>
        ///  The Scan.TokenType value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public TokenType ScanHex(char c)
        {
            // scan prefix
            c = AppendMoveNext(c);
            c = AppendMoveNext(c);
            ScanWhile(c, HEX_DIGITS);
            return TokenType.Number;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Name.
        /// </summary>
        /// <param name="c"> The char c.</param>
        /// <returns>
        ///  The SourcePreview.TokenType value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public TokenType ScanName(char c)
        {
            while (!AtEnd)
            {
                c = AppendMoveNext(c);
                if (Language.KeyContinue.IndexOf(c) < 0) break;
            }

            // check for keywords

            int id = Token.SearchKeyword(Language.KeyWords);
            if (id != -1)
            {
                return Language.KeyWords[id].Value + TokenType.Keyword1;
            }

            return ScanVariable(c);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Parse a Number. (possible scientific notation.
        /// </summary>
        /// <param name="c"> The char c.</param>
        /// <returns>
        ///  The SourcePreview.TokenType value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public TokenType ScanNumber(char c)
        {
            if (PeekNextChar() == 'x')
            {
                return ScanHex(c);
            }
            bool hasDp = false;
            bool hasExp = false;
            while (!AtEnd && char.IsNumber(c))
            {
                c = AppendMoveNext(c);
                if (c == '.')
                {
                    if (hasDp) break;
                    hasDp = true;
                }
                else if (c == 'e' || c == 'E')
                {
                    if (hasExp) break;
                    hasExp = true;
                    c = AppendMoveNext(c);
                    if (c == '-' || c == '+')
                    {
                        c = AppendMoveNext(c);
                    }
                }
            }
            return TokenType.Number;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Parse a Number. (possible scientific notation.
        /// </summary>
        /// <param name="c"> The char c.</param>
        /// <returns>
        ///  The SourcePreview.TokenType value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected TokenType ScanNumber2(char c)
        {
            if (PeekNextChar() == 'x')
            {
                return ScanHex(c);
            }

            ScanDigits(c);
            if (c == '.')
            {
                c = AppendMoveNext(c);
                ScanDigits(c);
            }

            if (c == 'e' || c == 'E')
            {
                c = AppendMoveNext(c);
                if (c == '-' || c == '+')
                {
                    c = AppendMoveNext(c);
                }

                ScanDigits(c);
            }

            return TokenType.Number;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Punctuation.
        /// </summary>
        /// <param name="c"> The char c.</param>
        /// <returns>
        ///  The SourcePreview.TokenType value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public TokenType ScanOperator(char c)
        {
            int i = Position;
            while (!AtEnd)
            {
                c = AppendMoveNext(c);
                if (Language.Operators.IndexOf(c) < 0) break;
            }

            return TokenType.Operator;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Punctuation.
        /// </summary>
        /// <param name="c"> The char c.</param>
        /// <returns>
        ///  The SourcePreview.TokenType value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public TokenType ScanPunctuation(char c)
        {
            //TODO merge this pattern of punct and ops
            int i = Position;
            while (!AtEnd)
            {
                c = AppendMoveNext(c);
                if (Language.Punctuation.IndexOf(c) < 0) break;
            }

            return TokenType.Punctuation;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Scan Token at a position and for a length.
        /// </summary>
        /// <param name="position"> The position.</param>
        /// <param name="end">       The end.</param>
        /// <returns>
        ///  The Scan.Token value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Token ScanToken(int position, int end)
        {
            Buffer.Reset(position);
            CharClassifier.Reset();

            Token.Clear();

            while (Buffer.Position < end)
            {
                //TODO keep tabs for now, if want to expand then do this.
                //else if (CurrentChar == '\t')
                //{
                //    int n = TabSize - (Column % TabSize);
                //    for (int i = 0; i < n; ++i)
                //    {
                //        Token.Append(' ');
                //    }

                //    MoveNext();
                //    continue;
                //}
                AppendMoveNext(CurrentChar);

            }

            return Token;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Scan Variable.
        /// </summary>
        /// <param name="c"> The char c.</param>
        /// <returns>
        ///  The Scan.TokenType value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public TokenType ScanVariable(char c)
        {
            //TODO this can be a variable continue too.
            while (!AtEnd && (char.IsLetterOrDigit(c) || c == '_'))
            {
                c = AppendMoveNext(c);
            }
            return TokenType.Variable;

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Scans a while.
        /// </summary>
        /// <param name="c">     The char c.</param>
        /// <param name="valid"> string containing ASCII characters to scan the char Text.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void ScanWhile(char c, string valid)
        {
            while (!AtEnd && valid.IndexOf(c) >= 0)
            {
                c = AppendMoveNext(c);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Scans up to the next occurrence of any of the characters in the valid string.
        /// </summary>
        /// <param name="c">       The char c.</param>
        /// <param name="invalid"> string containing ASCII characters to stop On.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void ScanWhileNot(char c, string invalid)
        {
            while (!AtEnd && c != '\n' && invalid.IndexOf(c) < 0)
            {
                c = AppendMoveNext(c);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Scan Whitespace.
        /// </summary>
        /// <param name="c"> The char c.</param>
        /// <returns>
        ///  True if successful
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public TokenType ScanWhitespace(char c)
        {
            while (!AtEnd && c != '\n' && char.IsWhiteSpace(c))
            {
                if (c == '\r')
                {
                    c = MoveNext();
                    continue;
                }
                if (c == '\t')
                {
                    int n = TabSize - (Column % TabSize);
                    for (int i = 0; i < n; ++i)
                    {
                        Token.Append(' ');
                    }
                    c = MoveNext();
                    break;
                }
                c = AppendMoveNext(c);
            }

            return TokenType.WhiteSpace;
        }
    }
}

