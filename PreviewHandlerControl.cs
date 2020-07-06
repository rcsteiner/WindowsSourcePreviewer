// Stephen Toub

using System;
using System.IO;
using System.Windows.Forms;

namespace SourcePreview
{
    public abstract class PreviewHandlerControl : Form
    {
        protected PreviewHandlerControl()
        {
            FormBorderStyle = FormBorderStyle.None;
        }

        public new abstract void Load(FileInfo file);
        public new abstract void Load(Stream stream);

        public virtual void Unload()
        {
            foreach (Control c in Controls) c.Dispose();
            Controls.Clear();
        }

        protected static string CreateTempPath(string extension)
        {
            return Path.GetTempPath() + Guid.NewGuid().ToString("N") + extension;
        }
    }
}
