// Stephen Toub

using System;
using System.Runtime.InteropServices;

namespace SourcePreview
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  Enables preview handlers to pass keyboard shortcuts to the host. This interface retrieves a list of keyboard
    ///  shortcuts and directs the host to handle a keyboard shortcut.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("fec87aaf-35f9-447a-adb7-20234491401a")]
    public interface IPreviewHandlerFrame
    {

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Gets a list of the keyboard shortcuts for the preview host.
        /// </summary>
        /// <param name="pinfo"> A pointer to a PREVIEWHANDLERFRAMEINFO structure that receives accelerator table information.</param>
        /// <returns>
        ///  If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [PreserveSig]
        int GetWindowContext(PREVIEWHANDLERFRAMEINFO pinfo);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Directs the host to handle an keyboard shortcut passed from the preview handler.
        ///  f the keystroke message can be processed by the preview handler, the handler will process it and return
        ///  S_OK. If the preview handler cannot process the keystroke message, it will offer it to the host using
        ///  TranslateAccelerator. If the host processes the message, this method will return S_OK. If the host does not
        ///  process the message, this method will return S_FALSE.
        /// </summary>
        /// <param name="pmsg"> A pointer to a WM_COMMAND or WM_SYSCOMMAND window message that corresponds to a keyboard shortcut.</param>
        /// <returns>
        ///  If the keyboard shortcut is one that the host intends to handle, the host will process it and return S_OK;
        ///  otherwise, it returns S_FALSE.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [PreserveSig]
        int TranslateAccelerator(ref MSG pmsg);
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  Accelerator table structure. Used by IPreviewHandlerFrame::GetWindowContext.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [StructLayout(LayoutKind.Sequential)]
    public struct PREVIEWHANDLERFRAMEINFO
    {
        /// <summary>
        ///  The number of entries in the accelerator table.
        /// </summary>
        public uint cAccelEntries;

        /// <summary>
        ///  A handle to the accelerator table.
        /// </summary>
        public IntPtr haccel;
    }
}

