////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: Interface definition for a Buffer
// 
//  Author:      Robert C Steiner
// 
//=====================================================[ History ]=====================================================
//  Date        Who      What
//  ----------- ------   ----------------------------------------------------------------------------------------------
//  2/24/2020   RCS      Initial Code.
//====================================================[ Copyright ]====================================================
// 
//  BSD 3-Clause License
//  Copyright (c) 2020, Robert C. Steiner
//  All rights reserved.
//  Redistribution and use in source and binary forms, with or without
//  modification, are permitted provided that the following conditions are met:
//  1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following
//  disclaimer.
//  2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the
//  following disclaimer in the documentation and/or other materials provided with the distribution.
//  3. Neither the name of the copyright holder nor the names of its
//  contributors may be used to endorse or promote products derived from
//  this software without specific prior written permission.
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
//  INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//  DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
//  SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
//  SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
//  WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE
//  USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.Reserved.
// 
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace SourcePreview
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Store Buffer interface definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public interface IBuffer
    {
        /// <summary>
        ///  True if Offset is at the end of the buffer.  Offset is buffer length.
        /// </summary>
        bool AtEnd { get; }

        /// <summary>
        ///  Get/Set Full Path.
        /// </summary>
        string Path { get; set; }

        /// <summary>
        ///  Get Next Offset.
        /// </summary>
        char MoveNext();

        /// <summary>
        ///  Get Current Char.
        /// </summary>
        char CurrentChar { get; }

        /// <summary>
        ///  Get/Set the current Offset.
        /// </summary>
        int Position { get; set; }

        /// <summary>
        ///  Look ahead 1 character.
        /// </summary>
        char PeekNextChar();


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Load File, If the path is null, create a empty buffer.
        /// </summary>
        /// <param name="filePath"> [optional=null] The file Path. null for empty buffer.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        bool Load(string filePath = null);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Save the buffer in file path.
        /// </summary>
        /// <param name="filePath"> [optional=null] The file Path. if null, then use stored path.</param>
        /// <returns>
        ///  True if successful
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        bool Save(string filePath = null);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Rewind the reader back to the start.
        /// </summary>
        /// <param name="pos"> [optional=0] The position. (default reset to beginning. </param>
        /// <returns>
        ///  The first character.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        char Reset(int pos=0);


    }
}
