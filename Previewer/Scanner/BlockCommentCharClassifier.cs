////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: Character classifier for block comments.
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
namespace Scan
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Block Comment Char Classifier Class definition. Keeps track of comment blocks.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class BlockCommentCharClassifier : CharClassifier
    {
        /// <summary>
        ///  True if in a block comment.
        /// </summary>
        private bool _inBlockComment;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Constructor: For BlockCommentCharClassifier.
        /// </summary>
        /// <param name="scanner">  The scanner.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public BlockCommentCharClassifier(Scanner scanner) : base(scanner)
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Block Comment.
        /// </summary>
        /// <param name="c">  The char c.</param>
        /// <returns>
        ///  The SourcePreview.TokenType value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private TokenType BlockComment(char c)
        {
            while (c != '\0')
            {
                switch (c)
                {
                    case '\n':
                        if (Scanner.Token.Length > 0) return TokenType.Comment;
                        return Scanner.ScanEol();

                    case '\r':
                        c= Scanner.MoveNext();
                        break;

                    default:

                        if (Scanner.Accept(Scanner.Language.BlockComment[1],false))
                        {
                            _inBlockComment = false;
                            return TokenType.Comment;
                        }
                        c = Scanner.AppendMoveNext(c);
                        break;
                }
            }

            return TokenType.Comment;
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
          
            if (_inBlockComment)
            {
                return BlockComment(c);
            }

            var lang = Scanner.Language;

            if (lang != null)
            {
                
                if (!string.IsNullOrEmpty( lang.LineComment) && Scanner.Accept(lang.LineComment,false))
                {
                    Scanner.EndOfLine();
                    return TokenType.Comment;
                }

                if (lang.BlockComment!=null && !string.IsNullOrEmpty(lang.BlockComment[0]) && Scanner.Accept(Scanner.Language.BlockComment[0],false))
                {
                    _inBlockComment = true;
                    return BlockComment(Scanner.CurrentChar);
                }
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
            _inBlockComment = false;
        }
    }
}