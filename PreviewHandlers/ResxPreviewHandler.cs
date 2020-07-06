// Stephen Toub

using System;
using System.IO;
using System.Resources;
using System.Collections;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SourcePreview
{
    [PreviewHandler("MSDN Magazine RESX Preview Handler", ".resx", "{A35B7E1D-C922-439d-81F4-6F9681D02F7F}")]
    [ProgId("MsdnMag.ResxPreviewHandler")]
    [Guid("F2D43DD7-1233-4d5f-9032-B82A570AEE81")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public sealed class ResxPreviewHandler : FileBasedPreviewHandler
    {
        protected override PreviewHandlerControl CreatePreviewHandlerControl()
        {
            return new ResxPreviewHandlerControl();
        }

        private sealed class ResxPreviewHandlerControl : StreamBasedPreviewHandlerControl
        {
            public override void Load(Stream previewStream)
            {
                ListView listView = new ListView();

                listView.Columns.Add("File Name", -2);
                listView.Columns.Add("Data Type", -2);
                listView.Columns.Add("Value", -2);

                listView.Dock = DockStyle.Fill;
                listView.BorderStyle = BorderStyle.None;
                listView.FullRowSelect = true;
                listView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
                listView.MultiSelect = false;
                listView.View = View.Details;

                Environment.CurrentDirectory = Path.GetDirectoryName(((FileStream)previewStream).Name);
                using (ResXResourceReader reader = new ResXResourceReader(previewStream))
                {
                    foreach (DictionaryEntry entry in reader)
                    {
                        ListViewItem item = new ListViewItem(new string[] { entry.Key.ToString(), entry.Value.GetType().ToString(), entry.Value.ToString() });
                        item.Tag = entry;
                        listView.Items.Add(item);
                    }
                }

                Controls.Add(listView);
            }
        }
    }
}