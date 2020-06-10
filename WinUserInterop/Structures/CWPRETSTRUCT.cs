using System;
using System.Runtime.InteropServices;

namespace Win32
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The CWPRETSTRUCT structure definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [StructLayout(LayoutKind.Sequential)]
    public struct CWPRETSTRUCT
    {
        /// <summary>
        ///  The l Result field.
        /// </summary>
        public IntPtr lResult;

        /// <summary>
        ///  The l parameter field.
        /// </summary>
        public IntPtr lParam;

        /// <summary>
        ///  The message field.
        /// </summary>
        public uint message;

        /// <summary>
        ///  The w parameter field.
        /// </summary>
        public IntPtr wParam;

        /// <summary>
        ///  The h window field.
        /// </summary>
        public IntPtr hWnd;

    }
}
