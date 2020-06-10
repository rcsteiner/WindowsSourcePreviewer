// Stephen Toub

using System;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MsdnMag
{
    [PreviewHandler("MSDN Magazine Internet Explorer Preview Handler", ".xml;.xps;.config;.psq;.html", "{88235ab2-bfce-4be8-9ed0-0408cd8da792}")]
    [ProgId("MsdnMag.InternetExplorerPreviewHandler")]
    [Guid("8fd75842-96ae-4ac9-a029-b57f7ef961a8")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public sealed class InternetExplorerPreviewHandler : FileBasedPreviewHandler
    {
        protected override PreviewHandlerControl CreatePreviewHandlerControl()
        {
            return new InternetExplorerPreviewHandlerControl();
        }

        private sealed class InternetExplorerPreviewHandlerControl : FileBasedPreviewHandlerControl
        {
            public override void Load(FileInfo file)
            {
                WebBrowser browser = new WebBrowser();
                browser.Dock = DockStyle.Fill;
                browser.Navigate(file.FullName);
                Controls.Add(browser);
            }
        }
    }
}