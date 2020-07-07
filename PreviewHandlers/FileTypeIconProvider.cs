//   Copyright 2020 Robert C. Steiner
// 
//   Permission is hereby granted, free of charge, to any person obtaining a copy of this software
//   and associated documentation files (the "Software"), to deal in the Software without restriction,
//   including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
//   and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
//   subject to the following conditions:
// 
//   The above copyright notice and this permission notice shall be included in
//   all copies or substantial portions of the Software.
// 
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
//   INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE
//   AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES
//   OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//   CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SourcePreview
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The File Type Icon Provider Class definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public sealed class FileTypeIconProvider : IDisposable
    {
        /// <summary>
        ///  Get Folder Index.
        /// </summary>
        public int FolderIndex => 0;

        /// <summary>
        ///  Get Icons.
        /// </summary>
        public ImageList Icons { get; private set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Default Constructor: For FileTypeIconProvider.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public FileTypeIconProvider()
        {
            Trace.WriteLine("File Type Icon Constructor");

            Icons = new ImageList();
            Icons.Images.Add(FileExplorerPreview.Resources.SourcePreview.folderopen); // Folder index
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Dispose.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Dispose()
        {
            if (Icons != null)
            {
                Icons.Dispose();
                Icons = null;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Get Icon Index For File.
        /// </summary>
        /// <param name="path">  The path.</param>
        /// <returns>
        ///  The string value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public string GetIconIndexForFile(string path)
        {

            var ext = Path.GetExtension(path);

            // Check to see if the image collection contains an image
            // for this extension, using the extension as a key.
            if (!Icons.Images.ContainsKey(ext))
            {
                // If not, add the image to the image list.
                Icons.Images.Add(ext, ShellIcons.GetIconFromExt(ext, true));
            }

            return ext;
        }
    }
}

