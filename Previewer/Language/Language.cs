////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: Simple Language use for highlighting.
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
using SourcePreview;

namespace Scan
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Language Class definition.  Simple Language use for highlighting.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [Serializable]
    public class Language : ILanguage
    {
        /// <summary>
        ///  Get/Set Block Comment.
        /// </summary>
        public List<string> BlockComment { get; set; }

        /// <summary>
        ///  Get Character.
        /// </summary>
        public List<Delimiter> Delimiters { get; set; }

        /// <summary>
        ///  Get Extensions.
        /// </summary>
        public List<string> Extensions { get; set; }

        /// <summary>
        ///  Get/Set File Path.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        ///  Get Key Words.  Sorted list of keywords
        /// </summary>
        public List<IDMap> KeyWords { get; set; }

        /// <summary>
        ///  Get/Set Groups.
        /// </summary>
        public List<string> Groups { get; set; }

        /// <summary>
        ///  Get/Set Line Comment.
        /// </summary>
        public string LineComment { get; set; }

        /// <summary>
        ///  The Name field.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///  Get Operators.
        /// </summary>
        public string Operators { get; set; }

        /// <summary>
        ///  Get Punctuation.
        /// </summary>
        public string Punctuation { get; set; }

        /// <summary>
        ///  Get/Set Keyword Chars.
        /// </summary>
        public string KeyContinue { get; set; }

        /// <summary>
        ///  Get/Set Key Start.
        /// </summary>
        public string KeyStart { get; set; }


        /// <summary>
        ///  The Manager field.
        /// </summary>
        public static LanguageManager Manager = new LanguageManager();

        /// <summary>
        ///  The scanner field.
        /// </summary>
        private InfoScanner _scanner;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="buffer"> The buffer.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Language(IBuffer buffer)
        {
            Load(buffer);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="filePath"> The file Path.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Language(string filePath)
        {
            FilePath = filePath;
            LoadFromFile(FilePath);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Initializes a new instance of the <see cref="T:System.Object" /> class. Used for default and simple
        ///  languages.
        /// </summary>
        /// <param name="name">        The name.</param>
        /// <param name="operators">   The operators.</param>
        /// <param name="punctuation"> The punctuation.</param>
        /// <param name="delimiters">   The delimiters.</param>
        /// <param name="extensions">  The extensions.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Language(string name, string operators, string punctuation, List<Delimiter> delimiters, params string[] extensions)
        {
            Name        = name;
            Extensions  = new List<string>(extensions);
            Operators   = operators;
            Punctuation = punctuation;
            Delimiters = delimiters;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Load.
        /// </summary>
        /// <param name="buffer"> The buffer to read from.</param>
        /// <returns>
        ///  True if successful
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool Load(IBuffer buffer)
        {
            if (_scanner == null)
            {
                _scanner = new InfoScanner(buffer);
            }

            _scanner.Initialize(buffer, this);

            FilePath = buffer.Path;

            string type;
            while ((type = _scanner.ParseType()) != null)
            {
                switch (type)
                {
                    case "KeyStart":
                        KeyStart = _scanner.ParseString(true);
                        break;
                   case "KeyContinue":
                        KeyContinue = _scanner.ParseString(true);
                        break;

                    case "Name":
                        Name = _scanner.ParseString(true);
                        break;
                    case "Ext":
                        Extensions = _scanner.ParseArray();
                        break;
                    case "Delimiter":
                        {
                            if (Delimiters == null)
                            {
                                Delimiters = new List<Delimiter>();
                            }

                            var d = new Delimiter();
                            d.Name = _scanner.ParseText();
                            d.Start = _scanner.ParseString(true);
                            d.Escape= _scanner.ParseString(true);
                            d.End = _scanner.ParseString(true);
                            if (string.IsNullOrEmpty(d.Escape)) d.Escape = "\0";
                           Delimiters.Add(d);
                        }
                        break;
                    case "Operators":
                        Operators = _scanner.ParseString(true);
                        break;
                    case "Punctuation":
                        Punctuation = _scanner.ParseString(true);
                        break;
                    case "LineComment":
                        LineComment = _scanner.ParseString(true);
                        break;
                    case "BlockComment":
                        BlockComment = _scanner.ParseArray();
                        break;
                    case "Keyword":

                        KeyWords =_scanner.ParseMap();
                        break;

                    case "Group":
                        Groups = _scanner.ParseArray();
                        break;
                    default:
                        break;
                }
                _scanner.NextLine();
            }

            if (KeyWords != null)
            {
                KeyWords.Sort((x, y) => x.Text.CompareTo(y.Text));
            }

            return true;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Load.
        /// </summary>
        /// <param name="filePath"> The file Path.</param>
        /// <returns>
        ///  True if successful
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool LoadFromFile(string filePath)
        {
            // todo reuse buffer
            var buffer = new ReadBuffer();
            buffer.Load(filePath);
            return Load(buffer);
        }

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
        //    var dest = new InfoBuilder();
        //    dest.WriteProperty("Name", true, true, Name);
        //    dest.WriteProperty("Ext", false, true, Extensions);
        //    if (Delimiters != null)
        //    {
        //        foreach (var delimiter in Delimiters)
        //        {
        //            dest.WriteProperty("Delimiter", true, true, delimiter.Name, delimiter.Start, delimiter.Escape,
        //                delimiter.End);
        //        }
        //    }

        //    dest.WriteProperty("Operators", true, true, Operators);
        //    dest.WriteProperty("Punctuation", true, true, Punctuation);

        //    for (var i = TokenType.Keyword; i < TokenType.Info.Count; i++)
        //    {
        //        var tokenType = (TokenType)i;
        //        dest.WriteProperty("Group", false, false, tokenType.Name);
        //        dest.WriteProperty(null, false, true, KeywordByType(tokenType));
        //    }

        //    return dest.Save(filePath ?? FilePath);
        //}

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>
        /////  Method: Sort.
        ///// </summary>
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //private void Sort()
        //{
        //    KeyWords.Sort(delegate (KeyWord x, KeyWord y) { return x.Text.CompareTo(y.Text); });
        //}

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: See if this language Supports a file extension.
        /// </summary>
        /// <param name="ext"> The extent.</param>
        /// <returns>
        ///  True if it does.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //public bool Supports(string ext)
        //{
        //    foreach (var extension in Extensions)
        //    {
        //        if (extension.Equals(ext, StringComparison.CurrentCultureIgnoreCase))
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}
    }
}
