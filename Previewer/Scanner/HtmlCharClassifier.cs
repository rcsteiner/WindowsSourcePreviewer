////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: Simple html character classifier.
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


namespace Scan
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Html Scanner Class definition. Handles looking for keywords in Tags and attribute names
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class HtmlCharClassifier : CharClassifier
    {
        /// <summary>
        ///  The  state field.
        /// </summary>
        private State _state;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  The State Enumeration definition.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private enum State
        {
            /// <summary>
            ///  The None enum value.
            /// </summary>
            None,

            /// <summary>
            ///  The Comment enum value.
            /// </summary>
            Comment,

            /// <summary>
            ///  The Tag enum value.
            /// </summary>
            Tag,

            /// <summary>
            ///  The Attribute enum value.
            /// </summary>
            Attribute,

            /// <summary>
            ///  The Body enum value.
            /// </summary>
            Body,
            Script,
            ScriptAttribute
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Initializes a new instance of the SimpleScanner class.
        /// </summary>
        /// <param name="scanner">  The scanner.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public HtmlCharClassifier(Scanner scanner) : base(scanner)
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Classify Character and see what kind of token it is. Override to add functionality.
        /// </summary>
        /// <param name="c"> The char c.</param>
        /// <returns>
        ///  The SourcePreview.TokenType value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override TokenType ClassifyCharacter(char c)
        {
            if (ScanEolAndWhitespace(c, out var classifyCharacter))
            {
                return classifyCharacter;
            }

            switch (_state)
                {
                    case State.Body:
                    case State.None:
                        return ClassifyPossibleTag(c);

                    case State.Comment:
                        return ScanComment(c);

                    case State.Tag:
                        return StateTag(c);

                    case State.Script:
                        return ScanScript(c);


                    case State.ScriptAttribute:
                    case State.Attribute:
                        return StateAttribute(c);

                    default:
                        return base.ClassifyCharacter(c);

                }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Classify Possible Tag.
        /// </summary>
        /// <param name="c">  The char c.</param>
        /// <returns>
        ///  The Scan.TokenType value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private TokenType ClassifyPossibleTag(char c)
        {
            while(c!= '\0')
            {
                switch (c)
                {

                    case '\n':
                        return Scanner.Token.Length > 0 ? (TokenType) TokenType.Default : Scanner.ScanEol();

                    case '<':
                        if (Scanner.Token.Length>0) return TokenType.Default;
                        
                        c = Scanner.AppendMoveNext(c);

                        if (c == '/')
                        {
                            // end tag
                            Scanner.AppendMoveNext(c);
                            _state = State.Tag;
                            return TokenType.Punctuation;
                        }
                        else if (c == '!')
                        {
                            // <!DOCTYPE xxxxxx >
                            // <!--  xxxxxxx -->
                            c = Scanner.AppendMoveNext(c);
                            if (c == '-' && Scanner.PeekNextChar() == '-')
                            {
                                // looking for ending comment, treat as comment
                                _state = State.Comment;
                                return ScanComment(c);
                            }
                            _state = State.Tag;
                            return TokenType.Punctuation;

                        }
                        else
                        {
                            // start tag
                            _state = State.Tag;
                            return TokenType.Punctuation;
                        }

                    default:
                        c = Scanner.AppendMoveNext(c);
                        break;
                }

            }
            return TokenType.End;
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Scan Comment. Either entering a comment or in a comment after Eol
        /// </summary>
        /// <returns>
        ///  The Scan.TokenType value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private TokenType ScanComment(char c)
        {
            while (c != '\0' && c != '\n')
            {
                if (Scanner.Accept("-->",false))
                {
                    _state = State.None;
                    break;
                }

                c = Scanner.AppendMoveNext(c);
            }

            return TokenType.Comment;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Scan Script.
        /// </summary>
        /// <param name="c">  The char c.</param>
        /// <returns>
        ///  The Scan.TokenType value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private TokenType ScanScript(char c)
        {
            while (c != '\0' && c != '\n')
            {
                if (c == '<')
                {
                    c = Scanner.AppendMoveNext(c);
                    if (c == '/')
                    {
                        c = Scanner.AppendMoveNext(c);
                        Scanner.ScanName(c);
                        c = Scanner.CurrentChar;
                        if (c == '>')
                        {
                            Scanner.AppendMoveNext(c);
                            _state = State.None;
                            break;
                        }
                    }
                }
                c = Scanner.AppendMoveNext(c);
            }
            return TokenType.Keyword3;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: State Attribute.
        /// </summary>
        /// <param name="c">  The char c.</param>
        /// <returns>
        ///  The Scan.TokenType value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private TokenType StateAttribute(char c)
            {
            switch (c)
            {
                case '>':
                    Scanner.AppendMoveNext(c);
                    _state = _state==State.ScriptAttribute?State.Script: State.Body;
                    return TokenType.Punctuation;

                case '/':
                    c = Scanner.AppendMoveNext(c);
                    if (c == '>')
                    {
                        Scanner.AppendMoveNext(c);
                        _state = State.None;
                    }
                    return TokenType.Punctuation;

                case '\'':
                case '"':
                    return Scanner.ScanDelimited(c, TokenType.String);

                case '\n':
                    return Scanner.ScanEol();

            }

            return char.IsLetter(c) ? Scanner.ScanName(c) : base.ClassifyCharacter(c);
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: State Tag.
        /// </summary>
        /// <param name="c">  The char c.</param>
        /// <returns>
        ///  The Scan.TokenType value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private TokenType StateTag(char c)
        {
            if (char.IsLetter(c))
            {
                _state = State.Attribute;
                var t = Scanner.ScanName(c);
                if (Scanner.Token.Compare("script") == 0)
                {
                    _state = State.ScriptAttribute;
                }

                return t;
            }
            return base.ClassifyCharacter(c);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Reset the classifier to start scanning at start of document.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Reset()
        {
            _state = State.None;
        }

    }
}
