////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: Simple config info file builder.
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
using System.IO;
using System.Text;

namespace Scan
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Builder Class definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    class InfoBuilder
    {
        /// <summary>
        ///  The destination field.
        /// </summary>
        private StringBuilder dest = new StringBuilder(2000);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Save.
        /// </summary>
        /// <param name="filePath"> The file Path.</param>
        /// <returns>
        ///  True if successful
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool Save(string filePath)
        {
            File.WriteAllText(filePath, dest.ToString());
            dest.Clear();
            return true;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Write Property.
        /// </summary>
        /// <param name="name">      The name.</param>
        /// <param name="useQuotes"> True if use Quotes.</param>
        /// <param name="lineFeed">   True if line Feed.</param>
        /// <param name="value">     The value.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void WriteProperty(string name, bool useQuotes, bool lineFeed, IEnumerable<string> value)
        {
           if (name!=null) dest.AppendFormat("{0} =", name);
            foreach (var v in value)
            {
                dest.Append(' ');
                if (useQuotes) dest.Append('"');
                dest.Append(v);
                if (useQuotes) dest.Append('"');
            }
            if (lineFeed) dest.AppendLine();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Write Property.
        /// </summary>
        /// <param name="name">      The name.</param>
        /// <param name="useQuotes"> True if use Quotes.</param>
        /// <param name="lineFeed">   True if line Feed.</param>
        /// <param name="value">     The value.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void WriteProperty(string name, bool useQuotes, bool lineFeed, params string[] value)
        {
            WriteProperty(name, useQuotes, lineFeed, (IEnumerable<string>)value);
        }
    }
}

