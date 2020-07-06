////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: Interface to simple language used for highlighting.
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
    ///  The Language interface definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public interface ILanguage
    {
        /// <summary>
        ///  Get/Set Block Comment.
        /// </summary>
        List<string> BlockComment { get;  }

        /// <summary>
        ///  Get/Set Line Comment.
        /// </summary>
        string LineComment { get; }

        /// <summary>
        ///  Get Character delimiter set.
        /// </summary>
        List<Delimiter> Delimiters { get; }

        /// <summary>
        ///  Get Extensions.
        /// </summary>
        List<string> Extensions { get; }

        /// <summary>
        ///  Get/Set Groups.
        /// </summary>
        List<string> Groups { get; set; }

        /// <summary>
        ///  Get/Set Key Continue.
        /// </summary>
        string KeyContinue { get; set; }

        /// <summary>
        ///  Get/Set Key Start.
        /// </summary>
        string KeyStart { get; set; }

        /// <summary>
        ///  Get Key Words and mapping table.
        /// </summary>
        List<IDMap> KeyWords { get; }

        /// <summary>
        ///  The Name field.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///  Get Operators characters.
        /// </summary>
        string Operators { get; }

        /// <summary>
        ///  Get Punctuation characters.
        /// </summary>
        string Punctuation { get; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Load.
        /// </summary>
        /// <param name="buffer"> The buffer.</param>
        /// <returns>
        ///  True if successful
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        bool Load(IBuffer buffer);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Save.
        /// </summary>
        /// <param name="filePath"> The file Path.</param>
        /// <returns>
        ///  True if successful
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
       //bool Save(string filePath);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: See if this language Supports a file extension.
        /// </summary>
        /// <param name="ext"> The extent.</param>
        /// <returns>
        ///  True if successful
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
       // bool Supports(string ext);
    }
}




