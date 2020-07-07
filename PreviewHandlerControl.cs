// Stephen Toub

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SourcePreview
{
    public abstract class PreviewHandlerControl : Form
    {
        protected PreviewHandlerControl()
        {
            Trace.WriteLine("PreviewHandlerControl constructor End");

            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.Black;
            ForeColor = Color.White;
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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // PreviewHandlerControl
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "PreviewHandlerControl";
            this.ResumeLayout(false);

        }
    }
}
