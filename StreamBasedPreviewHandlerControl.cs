// Stephen Toub

using System;
using System.IO;

namespace SourcePreview
{
    public abstract class StreamBasedPreviewHandlerControl : PreviewHandlerControl
    {
        public sealed override void Load(FileInfo file)
        {
            using (FileStream fs = new FileStream(file.FullName,
                FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.ReadWrite))
            {
                Load(fs);
            }
        }
    }
}
