////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: Fast character array based buffer, handling ASCII, UTF8, Big, Little Endian characters.
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SourcePreview;

namespace Scan
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  A simple reader buffer class. This is intended as a read-only utf8,
    ///  ascii, big endian and little endian files buffer.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class ReadBuffer : Buffer, IBuffer
    {
        /// <summary>
        ///  True if Offset is at the end of the buffer. Offset is buffer length.
        /// </summary>
        public override bool AtEnd { get { return Text == null || CurrentChar =='\0'; } }

        /// <summary>
        ///  Get/Set Is Unicode.
        /// </summary>
        public bool IsUnicode { get; set; }

        /// <summary>
        ///  Get/Set Is Utf8.
        /// </summary>
        public bool IsUtf8 { get; set; }

        /// <summary>
        ///  Get/Set Little Endian.
        /// </summary>
        public bool LittleEndian { get; set; }

        /// <summary>
        ///  The utf8 File Types field.
        /// </summary>
        private List<string> _utf8FileTypes = new List<string>() { ".html", ".xml" };

        /// <summary>
        ///  The Get Character field.
        /// </summary>
        private GetCharacterMethod _getCharacter;

        /// <summary>
        ///  length mapping table,
        ///  note: 10xxxxxx is a continue char,
        ///  0xxxxxxx is 1st of 1 byte code (ascii)
        ///  110xxxxx is 1st of 2 byte code
        ///  1110xxxx is 1st of 3 byte code
        /// </summary>
        private static readonly byte[] LENGTH = { 1, 1, 1, 1, 1, 1, 2, 3 };

        /// <summary>
        ///  The text field.
        /// </summary>
        protected byte[] Text;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Check Byte Order (BOM) of buffer and set flags and select GetCharacter method.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void CheckByteOrder()
        {
            IsUnicode    = false;
            IsUtf8       = false;
            LittleEndian = true;

            if (Text[0] == 0xef && Text[1] == 0xbb && Text[2] == 0xbf)
            {
                IsUtf8        = true;
                _position     = 3;
                _getCharacter = GetCharacterUtf8;
                return;

            }
            if (Text[0] == 0xff && Text[1] == 0xfe)
            {
                IsUnicode     = true;
                _position     = 1;
                _getCharacter = GetCharacterLittleEndian;
                return;
            }
            if (Text[0] == 0xfe && Text[1] == 0xff)
            {
                IsUnicode     = true;
                LittleEndian  = false;
                _position     = 1;
                _getCharacter = GetCharacterBigEndian;
                return;
            }

            if (!IsUtf8 && !IsUnicode && !string.IsNullOrEmpty(Path) && _utf8FileTypes.Contains(System.IO.Path.GetExtension(Path)))
            {
                IsUtf8        = true;
                _getCharacter = GetCharacterUtf8;
                return;
            }

            _getCharacter = GetCharacterAscii;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Sets the current character as ASCII
        /// </summary>
        /// <returns>
        ///  the current character or '\0' if EOF
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private char GetCharacterAscii()
        {
            return (_position + 1 <= Text.Length) ? (char)Text[_position++] : '\0';
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Get Character Big Endian.
        /// </summary>
        /// <returns>
        ///  The char value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private char GetCharacterBigEndian()
        {
            var c = (_position + 2 <= Text.Length) ? (char)(Text[_position] << 8 | Text[_position + 1]): '\0';
            _position += 2;
            return c;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Get Character Little Endian.
        /// </summary>
        /// <returns>
        ///  The char value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private char GetCharacterLittleEndian()
        {
            var c = (_position + 2 <= Text.Length) ? (char)(Text[_position] | Text[_position + 1] << 8): '\0';
            _position += 2;
            return c;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Get Character Utf8.
        /// </summary>
        /// <returns>
        ///  The char value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private char GetCharacterUtf8()
        {
            var c = '\0';
            if (_position < Text.Length)
            {
                var len = LENGTH[Text[_position] >> 5];
                if (len + _position <= Text.Length)
                {
                    switch (len)
                    {
                        case 1:
                            c = (char) Text[_position];
                            break;

                        case 2:
                            c = (char) (((Text[_position] & 0x001f) << 6) | (Text[_position + 1] & 0x003f));
                            break;

                        case 3:
                            c = (char) (((Text[_position] & 0x000f) << 12) | ((Text[_position + 1] & 0x003f) << 6) | (Text[_position + 2] & 0x003f));
                            break;
                    }
                }
                _position += len;
            }
            return c;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Initialize the buffer to an empty buffer of size.
        /// </summary>
        /// <param name="length"> The length.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual void Initialize(int length)
        {
            Text = new byte[length];
            Reset();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Load File, If the path is null, create a empty buffer.
        /// </summary>
        /// <param name="filePath"> [optional=null] The file Path. null for empty buffer.</param>
        /// <returns>
        ///  True if successful
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual bool Load(string filePath = null)
        {
            try
            {
                if (filePath != null)
                {
                    Path = System.IO.Path.GetFullPath(filePath);
                }
                if (File.Exists(Path))
                {
                    Text = File.ReadAllBytes(Path);
                    Reset();

                    return true;
                }
            }
            catch (Exception)
            {
            }
            Text = new byte[0];
            Reset();


            return false;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Get Next Offset.
        /// </summary>
        /// <returns>
        ///  The char value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override char MoveNext()
        {
            return  CurrentChar = _getCharacter();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Look ahead 1 character.
        /// </summary>
        /// <returns>
        ///  The char value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public char PeekNextChar()
        {
            int n     = _position;
            var c     = _position < Text.Length - 1 ? _getCharacter() : '\0';
            _position = n;
            return c;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Rewind the reader back to the start and returns the first character.
        /// </summary>
        /// <param name="position"> [optional=0] The position.</param>
        /// <returns>
        ///  The char value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override char Reset(int position = 0)
        {
            _position = position;
            if (position == 0)
            {
                CheckByteOrder();
            }
            return MoveNext();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Save the buffer in file path.
        /// </summary>
        /// <param name="filePath"> [optional=null] The file Path. if null, then use stored path.</param>
        /// <returns>
        ///  True if successful
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual bool Save(string filePath = null)
        {
            try
            {
                if (filePath != null)
                {
                    Path = System.IO.Path.GetFullPath(filePath);
                }
                if (File.Exists(Path))
                {
                    var f = new BinaryWriter(File.Open(Path, FileMode.Create));
                    f.Write(Text, 0, Text.Length);
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }
    }
}




