// Stephen Toub

using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Microsoft.Win32;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Diagnostics;

namespace SourcePreview
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Preview Handler Class definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public abstract class PreviewHandler : IPreviewHandler, IPreviewHandlerVisuals, IOleWindow, IObjectWithSite
    {
        /// <summary>
        ///  The  frame field.
        /// </summary>
        private IPreviewHandlerFrame _frame;

        /// <summary>
        ///  The  parent Hwnd field.
        /// </summary>
        private IntPtr _parentHwnd;

        /// <summary>
        ///  The  preview Control field.
        /// </summary>
        private PreviewHandlerControl _previewControl;

        /// <summary>
        ///  The  show Preview field.
        /// </summary>
        private bool _showPreview;

        /// <summary>
        ///  The  unk Site field.
        /// </summary>
        private object _unkSite;

        /// <summary>
        ///  The  window Bounds field.
        /// </summary>
        private Rectangle _windowBounds;

        /// <summary>
        ///  The S FAIL field.
        /// </summary>
        private const int S_FAIL = 1;

        /// <summary>
        ///  The S OK field.
        /// </summary>
        private const int S_OK = 0;

        public const int E_NOTIMPL = unchecked((int)0x80004001);


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Default Constructor: For PreviewHandler.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected PreviewHandler()
        {
            Trace.WriteLine("constructor start");
            _previewControl = CreatePreviewHandlerControl(); // NOTE: shouldn't call virtual function from constructor; see article for more information
            IntPtr forceCreation = _previewControl.Handle;
            _previewControl.BackColor = SystemColors.Window;
            Trace.WriteLine("constructor end");

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Create Preview Handler Control.
        /// </summary>
        /// <returns>
        ///  The SourcePreview.PreviewHandlerControl value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected abstract PreviewHandlerControl CreatePreviewHandlerControl();


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Invoke On Preview Thread.
        /// </summary>
        /// <param name="d">  The Method Invoker d.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void InvokeOnPreviewThread(MethodInvoker d)
        {
            _previewControl.Invoke(d);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Load.
        /// </summary>
        /// <param name="c">  The Preview Handler Control c.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected abstract void Load(PreviewHandlerControl c);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Register.
        /// </summary>
        /// <param name="t">  The Type template type.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [ComRegisterFunction]
        public static void Register(Type t)
        {
            if (t != null && t.IsSubclassOf(typeof(PreviewHandler)))
            {
                object[] attrs = (object[])t.GetCustomAttributes(typeof(PreviewHandlerAttribute), true);
                try
                {
                    if (attrs != null && attrs.Length == 1)
                    {
                        PreviewHandlerAttribute attr = attrs[0] as PreviewHandlerAttribute;
                        RegisterPreviewHandler(attr.Name, attr.Extension, t.GUID.ToString("B"), attr.AppId);
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Register Preview Handler.
        ///  at: HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\PreviewHandlers
        /// </summary>
        /// <param name="name">          The name.</param>
        /// <param name="extensions">    The extensions.</param>
        /// <param name="previewerGuid"> The previewer unique identifier.</param>
        /// <param name="appId">         The application identifier.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected static void RegisterPreviewHandler(string name, string extensions, string previewerGuid, string appId)
        {
            // Create a new prevhost AppID so that this always runs in its own isolated process
            using (RegistryKey appIdsKey = Registry.ClassesRoot.OpenSubKey("AppID", true))
            using (RegistryKey appIdKey = appIdsKey.CreateSubKey(appId))
            {
                appIdKey.SetValue("DllSurrogate", @"%SystemRoot%\system32\prevhost.exe", RegistryValueKind.ExpandString);
            }

            // Add preview handler to preview handler list
            using (RegistryKey handlersKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\PreviewHandlers", true))
            {
                handlersKey.SetValue(previewerGuid, name, RegistryValueKind.String);
            }

            // Modify preview handler registration
            using (RegistryKey clsidKey = Registry.ClassesRoot.OpenSubKey("CLSID"))
            using (RegistryKey idKey = clsidKey.OpenSubKey(previewerGuid, true))
            {
                idKey.SetValue("DisplayName", name, RegistryValueKind.String);
                idKey.SetValue("AppID", appId, RegistryValueKind.String);
                idKey.SetValue("DisableLowILProcessIsolation", 1, RegistryValueKind.DWord); // optional, depending on what preview handler needs to be able to do
            }

            foreach (string extension in extensions.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                Trace.WriteLine("Registering extension '" + extension + "' with previewer '" + previewerGuid + "'");

                // Set preview handler for specific extension
                using (RegistryKey extensionKey = Registry.ClassesRoot.CreateSubKey(extension))
                using (RegistryKey shellexKey = extensionKey.CreateSubKey("shellex"))
                using (RegistryKey previewKey = shellexKey.CreateSubKey("{8895b1c6-b41f-4c1c-a562-0d564250836f}"))
                {
                    previewKey.SetValue(null, previewerGuid, RegistryValueKind.String);
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Retrieves the latest site passed using SetSite.
        /// </summary>
        /// <param name="riid">    The IID of the interface pointer that should be returned in ppvSite.</param>
        /// <param name="ppvSite">
        ///                        Address of pointer variable that receives the interface pointer requested in riid.
        ///                        Upon successful return, *ppvSite contains the requested interface pointer to the site
        ///                        last seen in SetSite. The specific interface returned depends on the riid argument—in
        ///                        essence, the two arguments act identically to those in QueryInterface. If the
        ///                        appropriate interface pointer is available, the object must call AddRef on that
        ///                        pointer before returning successfully. If no site is available, or the requested
        ///                        interface is not supported, this method must *ppvSite to NULL and return a failure
        ///                        code.
        ///</param>
        /// <returns>
        ///  This method returns S_OK on success.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        int IObjectWithSite.GetSite(ref Guid riid, out object ppvSite)
        {
            ppvSite = _unkSite;
            return S_OK;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: IObject With Site. Set Site.
        /// </summary>
        /// <param name="pUnkSite"> The p Unk Site.</param>
        /// <returns>This method returns S_OK on success.</returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        int IObjectWithSite.SetSite(object pUnkSite)
        {
            _unkSite = pUnkSite;
            _frame = _unkSite as IPreviewHandlerFrame;
            return S_OK;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Determines whether context-sensitive help mode should be entered during an in-place activation session.
        /// </summary>
        /// <param name="fEnterMode"> TRUE if help mode should be entered; FALSE if it should be exited.</param>
        /// <returns>
        ///  This method returns S_OK if the help mode was entered or exited successfully, depending on the value passed
        ///  in fEnterMode.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        int IOleWindow.ContextSensitiveHelp(bool fEnterMode)
        {
            return E_NOTIMPL;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Retrieves a handle to one of the windows participating in in-place activation (frame, document, parent, or
        ///  in-place object window).
        /// </summary>
        /// <param name="phwnd"> A pointer to a variable that receives the window handle.</param>
        /// <returns>
        ///  This method returns S_OK on success.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        int IOleWindow.GetWindow(out IntPtr phwnd)
        {
            phwnd = IntPtr.Zero;
            phwnd = _previewControl.Handle;
            return S_OK;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Directs the preview handler to load data from the source specified in an earlier Initialize method call,
        ///  and to begin rendering to the previewer window.
        /// </summary>
        /// <returns>
        ///  This method can return one of these values.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        int IPreviewHandler.DoPreview()
        {
            _showPreview = true;
            InvokeOnPreviewThread(delegate ()
            {
                try
                {
                    Load(_previewControl);
                }
                catch (Exception exc)
                {
                    _previewControl.Controls.Clear();
                    TextBox text = new TextBox();
                    text.ReadOnly = true;
                    text.Multiline = true;
                    text.Dock = DockStyle.Fill;
                    text.Text = exc.ToString();
                    _previewControl.Controls.Add(text);
                }
                UpdateWindowBounds();
            });
            return S_OK;

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Directs the preview handler to return the HWND from calling the GetFocus Function.
        /// </summary>
        /// <param name="phwnd">
        ///                      When this method returns, contains a pointer to the HWND returned from calling the
        ///                      GetFocus Function from the preview handler's foreground thread.
        ///</param>
        /// <returns>
        ///  The integer value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        int IPreviewHandler.QueryFocus(out IntPtr phwnd)
        {

            //  Default focus will be nothing.
            //IntPtr focusHandle = IntPtr.Zero;

            //  If we have a preview handler, focus it.
            //if (_previewControl != null)
            //{
            //    InvokeOnPreviewThread(() =>
            //    {
            //        var focusedControl = _previewControl.FindFocusedControl();
            //        focusHandle = focusedControl != null ? focusedControl.Handle : _previewControl.Handle;
            //    });
            //}

            //phwnd = focusHandle;

            IntPtr result = IntPtr.Zero;
            InvokeOnPreviewThread(delegate () { result = GetFocus(); });
            phwnd = result;
            if (phwnd == IntPtr.Zero)  phwnd = _previewControl.Handle;
            return S_OK;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Directs the preview handler to set focus to itself.
        /// </summary>
        /// <returns>
        ///  If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        int IPreviewHandler.SetFocus()
        {
            InvokeOnPreviewThread(delegate () { _previewControl.Focus(); });
            return S_OK;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: IPreview Handler. Set rectangle.
        /// </summary>
        /// <param name="rect"> [ref] The rectangle.</param>
        /// <returns>
        ///  The integer value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        int IPreviewHandler.SetRect(ref RECT rect)
        {
            _windowBounds = rect.ToRectangle();
            UpdateWindowBounds();
            return S_OK;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: IPreview Handler. Set Window.
        /// </summary>
        /// <param name="hwnd">  The hwnd.</param>
        /// <param name="rect"> [ref] The rectangle.</param>
        /// <returns>
        ///  The integer value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        int IPreviewHandler.SetWindow(IntPtr hwnd, ref RECT rect)
        {
            _parentHwnd = hwnd;
            _windowBounds = rect.ToRectangle();
            UpdateWindowBounds();
            return S_OK;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Directs the preview handler to handle a keystroke passed up from the message pump of the process in which
        ///  the preview handler is running.
        /// </summary>
        /// <param name="pmsg"> A pointer to a window message.</param>
        /// <returns>
        ///  If the keystroke message can be processed by the preview handler, the handler will process it and return
        ///  S_OK. If the preview handler cannot process the keystroke message, it will offer it to the host using
        ///  TranslateAccelerator. If the host processes the message, this method will return S_OK. If the host does not
        ///  process the message, this method will return S_FALSE.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        int IPreviewHandler.TranslateAccelerator(ref MSG pmsg)
        {

            if (_frame != null)
            {
                var r = _frame.TranslateAccelerator(pmsg);
                Trace.WriteLine($"TranslateAccelerator {pmsg.wParam} ret={r}");
                return r;
            }

           Trace.WriteLine($"TranslateAccelerator {pmsg.wParam} NO Frame");
           return S_OK;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Directs the preview handler to cease rendering a preview and to release all resources that have been
        ///  allocated based on the item passed in during the initialization.
        /// </summary>
        /// <returns>
        ///  If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        int IPreviewHandler.Unload()
        {
            _showPreview = false;
            InvokeOnPreviewThread(delegate ()
            {
                _previewControl.Visible = false;
                _previewControl.Unload();
            });
            return S_OK;

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Sets the background color of the preview handler.
        /// </summary>
        /// <param name="color"> A value of type COLORREF to use for the preview handler background.</param>
        /// <returns>
        ///  If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        int IPreviewHandlerVisuals.SetBackgroundColor(COLORREF color)
        {
            Color c = color.Color;
            InvokeOnPreviewThread(delegate () { _previewControl.BackColor = c; });
            return S_OK;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Sets the color of the text within the preview handler.
        /// </summary>
        /// <param name="plf"> [ref] The plf.</param>
        /// <returns>
        ///  If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error cod
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        int IPreviewHandlerVisuals.SetFont(ref LOGFONT plf)
        {
            Font f = Font.FromLogFont(plf);
            InvokeOnPreviewThread(delegate () { _previewControl.Font = f; });
            return S_OK;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Sets the font attributes to be used for text within the preview handler.
        /// </summary>
        /// <param name="color">  The color.</param>
        /// <returns>
        ///  If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        int IPreviewHandlerVisuals.SetTextColor(COLORREF color)
        {
            Color c = color.Color;
            InvokeOnPreviewThread(delegate () { _previewControl.ForeColor = c; });
            return S_OK;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Unregister.
        /// </summary>
        /// <param name="t">  The Type template type.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [ComUnregisterFunction]
        public static void Unregister(Type t)
        {
            if (t != null && t.IsSubclassOf(typeof(PreviewHandler)))
            {
                object[] attrs = (object[])t.GetCustomAttributes(typeof(PreviewHandlerAttribute), true);
                try
                {
                    if (attrs != null && attrs.Length == 1)
                    {
                        PreviewHandlerAttribute attr = attrs[0] as PreviewHandlerAttribute;
                        UnregisterPreviewHandler(attr.Extension, t.GUID.ToString("B"), attr.AppId);
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Unregister Preview Handler.
        /// </summary>
        /// <param name="extensions">     The extensions.</param>
        /// <param name="previewerGuid">  The previewer unique identifier.</param>
        /// <param name="appId">          The application identifier.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected static void UnregisterPreviewHandler(string extensions, string previewerGuid, string appId)
        {
            foreach (string extension in extensions.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                Trace.WriteLine($"Unregistering extension '{extension}' with previewer '{previewerGuid}'");
                try
                {
                    using (RegistryKey shellexKey = Registry.ClassesRoot.OpenSubKey(extension + "\\shellex", true))
                    {
                        Trace.WriteLine($"Removing subkey '{extension}' with previewer '{previewerGuid}' on shellexKey={shellexKey}");
                        shellexKey.DeleteSubKey("{8895b1c6-b41f-4c1c-a562-0d564250836f}");
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                }
            }

            try
            {
                using (RegistryKey appIdsKey = Registry.ClassesRoot.OpenSubKey("AppID", true))
                {
                    appIdsKey.DeleteSubKey(appId);
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }

            using (RegistryKey classesKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\PreviewHandlers", true))
            {
                classesKey.DeleteValue(previewerGuid);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Update Window Bounds.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void UpdateWindowBounds()
        {
            if (_showPreview)
            {
                InvokeOnPreviewThread(delegate ()
                {
                    SetParent(_previewControl.Handle, _parentHwnd);
                    _previewControl.Bounds = _windowBounds;
                    _previewControl.Visible = true;
                });
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Get Focus.
        /// </summary>
        /// <returns>
        ///  The System.IntPtr value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetFocus();

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Set Parent.
        /// </summary>
        /// <param name="hWndChild">      The h window Child.</param>
        /// <param name="hWndNewParent">  The h window New Parent.</param>
        /// <returns>
        ///  The System.IntPtr value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

    }
}
