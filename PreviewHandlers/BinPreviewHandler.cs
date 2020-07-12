// Stephen Toub

using System;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Drawing;
using System.Runtime.InteropServices;

namespace SourcePreview
{
    [PreviewHandler("MSDN Magazine Binary Preview Handler", ".bin;.dat;.class", "{FDFA5AAF-8243-415d-B5E5-AF551336BE7B}")]
    [ProgId("MsdnMag.BinaryPreviewHandler")]
    [Guid("DF9E65B0-7980-4053-9FCF-6E9AF953A9F4")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public sealed class BinaryPreviewHandler : FileBasedPreviewHandler
    {
        protected override PreviewHandlerControl CreatePreviewHandlerControl()
        {
            return new BinaryPreviewHandlerControl();
        }

        private sealed class BinaryPreviewHandlerControl : FileBasedPreviewHandlerControl
        {

            //TODO fix this.
            public override void Load(FileInfo file)
            {
                ByteViewer viewer = new ByteViewer();
                viewer.Dock = DockStyle.Fill;
 
                viewer.SetFile(file.FullName);
                viewer.BackColor = Color.White;
                viewer.ForeColor = Color.Black;
                viewer.SetDisplayMode(DisplayMode.Hexdump);
               Controls.Add(viewer);
            }
        }
    }
}