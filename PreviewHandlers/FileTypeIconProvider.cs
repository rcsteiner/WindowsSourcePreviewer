// Stephen Toub

using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SourcePreview
{
    internal sealed class FileTypeIconProvider : IDisposable
    {
        private ImageList _icons;
        private Dictionary<string, int> _extensionToIconIndex;

        public FileTypeIconProvider()
        {
            _icons = new ImageList();
            _extensionToIconIndex = new Dictionary<string, int>();
            _icons.Images.Add(PreviewHandlerResources.TreeView_XP_Explorer_ParentNode); // Folder index
        }

        public void Dispose()
        {
            if (_icons != null)
            {
                _icons.Dispose();
                _icons = null;
            }
        }

        public ImageList Icons { get { return _icons; } }

        public int FolderIndex { get { return 0; } }

        public int GetIconIndexForFile(string path)
        {
            // Get the extension.  This will work for either file paths or urls.
            if (path == null) return -1;
            string ext = Path.GetExtension(path.Trim());

            int index;
            if (!_extensionToIconIndex.TryGetValue(ext, out index))
            {
                // Get the icon for the file
                SHFILEINFO shinfo = new SHFILEINFO();
                const uint SHGFI_ICON = 0x100, SHGFI_SMALLICON = 0x1, SHGFI_USEFILEATTRIBUTES = 0x10;
                IntPtr rv = SHGetFileInfo(path, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo),
                    SHGFI_ICON | SHGFI_SMALLICON | SHGFI_USEFILEATTRIBUTES);
                if (rv == IntPtr.Zero) return -1;

                // The icon is returned in the hIcon member of the shinfo struct
                _icons.Images.Add(Icon.FromHandle(shinfo.hIcon));
                int pos = _icons.Images.Count - 1;
                _extensionToIconIndex[ext] = pos;
                index = pos;
            }

            // Return the mapped index.
            return index;
        }

        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        [DllImport("shell32.dll")]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);
    }
}
