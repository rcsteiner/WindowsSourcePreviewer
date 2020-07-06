////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: Delimiter language definitions.
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
    ///  The Delimiter structure definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public struct Delimiter
    {
        /// <summary>
        ///  The Name field.
        /// </summary>
        public string Name; 
        
        /// <summary>
        ///  The Start field.
        /// </summary>
        public string Start;

        /// <summary>
        ///  The Escape field.
        /// </summary>
        public string Escape;
   
        /// <summary>
        ///  The End field.
        /// </summary>
        public string End;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public Delimiter(string name, string start, string escape, string end)
        {
            Name = name;
            Start = start;
            Escape = escape;
            End = end;
        }
    }

}
