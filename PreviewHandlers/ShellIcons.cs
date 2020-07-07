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
using System.Drawing;
using System.Runtime.InteropServices;

namespace SourcePreview
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Shell Icons Class definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static class ShellIcons
    {
        //Indicates that the function should not attempt to access the file specified by pszPath.
        //Rather, it should act as if the file specified by pszPath exists with the file attributes passed in dwFileAttributes.
        //This flag cannot be combined with the SHGFI_ATTRIBUTES, SHGFI_EXETYPE, or SHGFI_PIDL flags.

        private const uint SHGFI_ICON                         = 0x000000100;     // get icon
        private const uint SHGFI_DISPLAYNAME                  = 0x000000200;     // get display name
        private const uint SHGFI_TYPENAME                     = 0x000000400;     // get type name
        private const uint SHGFI_ATTRIBUTES                   = 0x000000800;     // get attributes
        private const uint SHGFI_ICONLOCATION                 = 0x000001000;     // get icon location
        private const uint SHGFI_EXETYPE                      = 0x000002000;     // return exe type
        private const uint SHGFI_SYSICONINDEX                 = 0x000004000;     // get system icon index
        private const uint SHGFI_LINKOVERLAY                  = 0x000008000;     // put a link overlay on icon
        private const uint SHGFI_SELECTED                     = 0x000010000;     // show icon in selected state
        private const uint SHGFI_ATTR_SPECIFIED               = 0x000020000;     // get only specified attributes
        private const uint SHGFI_LARGEICON                    = 0x000000000;     // get large icon
        private const uint SHGFI_SMALLICON                    = 0x000000001;     // get small icon
        private const uint SHGFI_OPENICON                     = 0x000000002;     // get open icon
        private const uint SHGFI_SHELLICONSIZE                = 0x000000004;     // get shell size icon
        private const uint SHGFI_PIDL                         = 0x000000008;     // pszPath is a pidl
        private const uint SHGFI_USEFILEATTRIBUTES            = 0x000000010;     // use passed dwFileAttribute

        private const uint FILE_ATTRIBUTE_READONLY            = 0x00000001;
        private const uint FILE_ATTRIBUTE_HIDDEN              = 0x00000002;
        private const uint FILE_ATTRIBUTE_SYSTEM              = 0x00000004;
        private const uint FILE_ATTRIBUTE_DIRECTORY           = 0x00000010;
        private const uint FILE_ATTRIBUTE_ARCHIVE             = 0x00000020;
        private const uint FILE_ATTRIBUTE_DEVICE              = 0x00000040;
        private const uint FILE_ATTRIBUTE_NORMAL              = 0x00000080;
        private const uint FILE_ATTRIBUTE_TEMPORARY           = 0x00000100;
        private const uint FILE_ATTRIBUTE_SPARSE_FILE         = 0x00000200;
        private const uint FILE_ATTRIBUTE_REPARSE_POINT       = 0x00000400;
        private const uint FILE_ATTRIBUTE_COMPRESSED          = 0x00000800;
        private const uint FILE_ATTRIBUTE_OFFLINE             = 0x00001000;
        private const uint FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x00002000;
        private const uint FILE_ATTRIBUTE_ENCRYPTED           = 0x00004000;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Get small icon image from an extension or a path. Uses only the extension to get the
        ///  Icon, the path does not have to be valid.
        /// </summary>
        /// <param name="aExtension"> extension.</param>
        /// <param name="aLarge">     if set to <c>true</param>
        /// <returns>
        ///  Icon
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static Icon GetIconFromExt(string aExtension, bool aLarge)
        {
            SHFILEINFO mInfo = new SHFILEINFO();
            uint oFlags = SHGFI_USEFILEATTRIBUTES | SHGFI_ICON | ((aLarge) ? SHGFI_LARGEICON : SHGFI_SMALLICON);

            SHGetFileInfo(aExtension, FILE_ATTRIBUTE_NORMAL, ref mInfo, (uint)Marshal.SizeOf(mInfo), oFlags);

            return Icon.FromHandle(mInfo.mIconHandle);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: SHGet File information.
        /// </summary>
        /// <param name="aPath">           The a Path.</param>
        /// <param name="aFileAttributes"> The a File Attributes.</param>
        /// <param name="aShellInfo">      [ref] The a Shell information.</param>
        /// <param name="aSizeFileInfo">   The a Size File information.</param>
        /// <param name="aFlags">          The a Flags.</param>
        /// <returns>
        ///  The System.IntPtr value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("shell32.dll")]
        private static extern IntPtr SHGetFileInfo(string aPath, uint aFileAttributes, ref SHFILEINFO aShellInfo, uint aSizeFileInfo, uint aFlags);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Windows structure used for Getting icons from the shell.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [StructLayout(LayoutKind.Sequential)]
        private struct SHFILEINFO
        {
            /// <summary>
            ///  The m Icon Handle field.
            /// </summary>
            public IntPtr mIconHandle;

            /// <summary>
            ///  The m Icon Index field.
            /// </summary>
            public IntPtr mIconIndex;

            /// <summary>
            ///  The m Attributes field.
            /// </summary>
            public uint mAttributes;

            /// <summary>
            ///  The m Display Name field.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]

            internal string mDisplayName;

            /// <summary>
            ///  The m Type Name field.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]


            internal string mTypeName;
        }

    }
}
