////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: Classify a character
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

using System.Linq.Expressions;

namespace Scan
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Char Classifier Class definition.  Basic character classifier.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class CharClassifier : ICharClassifier
    {
        /// <summary>
        ///  Get/Set Scanner.
        /// </summary>
        public IScanner Scanner { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="scanner">  The scanner.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public CharClassifier(Scanner scanner)
        {
            Scanner = scanner;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Scan for either Eol or Whitespace.
        /// </summary>
        /// <param name="c">                  The char c.</param>
        /// <param name="classifyCharacter"> [out] The classify Character.</param>
        /// <returns>
        ///  True if successful
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected bool ScanEolAndWhitespace(char c, out TokenType classifyCharacter)
        {
            if (c == '\n')
            {
                classifyCharacter = Scanner.ScanEol();
                return true;
            }

            if (char.IsWhiteSpace(c))
            {
                classifyCharacter = Scanner.ScanWhitespace(c);
                return true;
            }

            classifyCharacter = TokenType.Default;
            return false;
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
        public virtual TokenType ClassifyCharacter(char c)
        {
            if (c == '\n')
            {
                return Scanner.ScanEol();
            }

            if (char.IsWhiteSpace(c))
            {
                return Scanner.ScanWhitespace(c);
            }


            if (Scanner.Language.Delimiters != null)
            {
                for (var index = 0; index < Scanner.Language.Delimiters.Count; index++)
                {
                    var delimiter = Scanner.Language.Delimiters[index];
                    if (c == delimiter.Start[0])
                    {
                        return Scanner.ScanDelimited(c, TokenType.String + index, delimiter);
                    }
                }
            }

            if (Scanner.Language?.KeyStart?.IndexOf(c) >= 0)
            {
                return Scanner.ScanName(c);
            }

            if (char.IsNumber(c))
            {
                return Scanner.ScanNumber(c);
            }

            if (char.IsLetter(c) || c == '_')
            {
                return Scanner.ScanVariable(c);
            }

            if (Scanner.Language?.Punctuation.IndexOf(c) >= 0)
            {
                return Scanner.ScanPunctuation(c);
            }

            if (Scanner.Language?.Operators.IndexOf(c) >= 0)
            {
                return Scanner.ScanOperator(c);
            }

            Scanner.AppendMoveNext(c);

            return TokenType.Default;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Reset the classifier to start scanning at start of document.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual void Reset()
        {
        }
    }
}
