////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: A stream version of the buffer, used mostly for resources.
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

namespace Scan
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Stream Buffer Class definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class StreamBuffer : ReadBuffer
    {
        /// <summary>
        ///  The  stream field.
        /// </summary>
        private Stream _stream;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Initialize.
        /// </summary>
        /// <param name="stream"> The stream.</param>
        /// <param name="path">    The name path.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Initialize(Stream stream, string path)
        {
            _stream = stream;
            int len = (int)_stream.Length;
            Path = path;

            base.Initialize(len);

            _stream.Read(Text, 0, len);

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Get Resource Streams with a specific extension in the name.
        /// </summary>
        /// <param name="extension">  The extension.</param>
        /// <returns>
        ///  The stream name and the buffer is loaded.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public  IEnumerable<string> GetResourceStreams(string extension)
        {
            var assembly = typeof(StreamBuffer).Assembly;
            var names = assembly.GetManifestResourceNames();

            foreach (var name in names)
            {
                var s = System.IO.Path.GetExtension(name);
                if (s == extension)
                {
                    var stream = assembly.GetManifestResourceStream(name);
                    Initialize(stream, name);
                    stream.Close();
                    yield return name;
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Get Resource Stream with a specific name.
        /// </summary>
        /// <param name="filename">  The filename.  Note: names are in the general format of directory.name.ext</param>
        /// <returns>
        ///  The stream associated with this resource name. 
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Stream GetResourceStream(string filename)
        {
            var assembly = typeof(StreamBuffer).Assembly;
            var names = assembly.GetManifestResourceNames();
            filename = "." + filename;
            foreach (var name in names)
            {
                if (name.EndsWith(filename))
                {
                    return  assembly.GetManifestResourceStream(name);
                }
            }

            return null;
        }
    }
}
