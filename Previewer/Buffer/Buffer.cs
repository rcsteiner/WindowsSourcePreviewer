////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: Base class for Fast character array based buffer, handling ASCII, UTF8, Big, Little Endian characters.
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
    ///  The Buffer Class definition.
    ///  Base class for Fast character array based buffer, handling ASCII, UTF8, Big, Little Endian characters.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public abstract class Buffer
    {
        /// <summary>
        ///  True if Offset is at the end of the buffer. Offset is buffer length.
        /// </summary>
        public abstract bool AtEnd { get; }

        /// <summary>
        ///  Get Current Char.
        /// </summary>
        public char CurrentChar { get; set; }

        /// <summary>
        ///  Get/Set Full Path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        ///  Get/Set the current Position, when set, sets the current character too.
        ///  Use only positions returned, since the position is not the character position. (utf8, unicode)
        /// </summary>
        public int Position { get { return _position; } set { Reset(value); } }

        /// <summary>
        ///  The position field.
        /// </summary>
        protected int _position;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Get Next Offset.
        /// </summary>
        /// <returns>
        ///  The char value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public abstract char MoveNext();

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Rewind the reader back to the start and returns the first character.
        /// </summary>
        /// <param name="position"> [optional=0] The position.</param>
        /// <returns>
        ///  The char value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public abstract char Reset(int position = 0);
    }
}
