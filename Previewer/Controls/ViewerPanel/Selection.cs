////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: Selection
// 
//  Author:      Robert C. Steiner
// 
//=====================================================[ History ]=====================================================
//  Date        Who      What
//  ----------- ------   ----------------------------------------------------------------------------------------------
//  7/18/2020   RCS      Initial Code.
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

using Scan;
using Win32;

namespace UI.Controls
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Selection Class definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class Selection
    {
        /// <summary>
        ///  Get Text.
        /// </summary>
        public Token Text { get { return Scanner.ScanToken(Start, End); } }

        /// <summary>
        ///  Get Length.
        /// </summary>
        public int Length    { get { return End - Start; } }

        /// <summary>
        ///  The Buffer field.
        /// </summary>
        public IScanner Scanner;

        /// <summary>
        ///  The Length field.
        /// </summary>
        public int End;

        /// <summary>
        ///  The Position field.
        /// </summary>
        public int Start;


        private int _startMarker;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Constructor: For Selection.
        /// </summary>
        /// <param name="scanner">  The scanner.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Selection(IScanner scanner)
        {
            Scanner = scanner;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Expand.
        /// </summary>
        /// <param name="pos">  The position.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Expand(in int pos)
        {
            if (pos < _startMarker)
            {
                End = _startMarker+1;
                Start = pos;
            }
            else
            {
                Start = _startMarker;
                End = pos+1;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Set Start.
        /// </summary>
        /// <param name="pos">  The position.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetStart(int pos)
        {
            _startMarker = pos;
            Start =  pos;
            End = Start + 1;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Clear this object to its initial state..
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Clear()
        {
            Start = End = 0;
        }
    }
}
