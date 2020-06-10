// Stephen Toub

using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;


namespace MsdnMag
{
    [PreviewHandler("MSDN Magazine ZIP Preview Handler", ".zip;.gadget", "{c0a64ec6-729b-442d-88ce-d76a9fc69e44}")]
    [ProgId("MsdnMag.ZipPreviewHandler")]
    [Guid("853f35e3-bd13-417b-b859-1df25be6c834")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public sealed class ZipPreviewHandler : FileBasedPreviewHandler
    {
        protected override PreviewHandlerControl CreatePreviewHandlerControl()
        {
            return new ZipPreviewHandlerControl();
        }

        private sealed class ZipPreviewHandlerControl : FileBasedPreviewHandlerControl
        {
            public override void Load(FileInfo file)
            {
                TreeView folderView = new TreeView();
                folderView.ImageList = _iconProvider.Icons;
                folderView.Dock = DockStyle.Fill;
                folderView.FullRowSelect = true;
                folderView.ShowLines = true;
                folderView.ShowPlusMinus = true;
                folderView.ShowRootLines = true;
                folderView.LabelEdit = false;
                folderView.BorderStyle = BorderStyle.None;
                folderView.TreeViewNodeSorter = new NodeSorter();

                List<string> filenames = new List<string>();

                using (ZipArchive zip = ZipFile.OpenRead(file.FullName))
                {

                    foreach (var entry in zip.Entries)
                    {
                        {
                            filenames.Add(entry.FullName);
                        }
                    }
                }

                TreeNode root = ConvertFilenamesToTreeRoot(file.Name, filenames);
                folderView.Nodes.Add(root);
                root.Expand();

                folderView.DoubleClick += delegate
                {
                    if (folderView.SelectedNode != null && folderView.SelectedNode.Tag as string != null)
                    {
                        TreeNode selectedNode = folderView.SelectedNode;
                        ZipArchive zipToOpen =ZipFile.OpenRead(file.FullName);
                        string tempPath = ExtractZipEntryToFile((string)selectedNode.Tag, zipToOpen);
                        zipToOpen.Dispose();
                        Process.Start(tempPath);
                    }
                };

                Controls.Add(folderView);
            }

            private class NodeSorter : IComparer
            {
                int IComparer.Compare(object x, object y)
                {
                    TreeNode node1 = (TreeNode)x;
                    TreeNode node2 = (TreeNode)y;

                    string tag1 = node1.Tag as string;
                    string tag2 = node2.Tag as string;

                    if (tag1 == null && tag2 == null) return StrCmpLogicalW(node1.Text, node2.Text);
                    else if (tag1 == tag2) return 0;
                    else if (tag1 == null) return -1;
                    else if (tag2 == null) return 1;
                    else return StrCmpLogicalW(tag1, tag2);
                }

                [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
                private static extern int StrCmpLogicalW(string strA, string strB);
            }

            private TreeNode ConvertFilenamesToTreeRoot(string rootName, IEnumerable<string> names)
            {
                TreeNode rootNode = new TreeNode(rootName);
                rootNode.ImageIndex = _iconProvider.FolderIndex;
                foreach (string name in names)
                {
                    string[] pathParts = name.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                    TreeNode parent = rootNode;
                    for(int i=0; i<pathParts.Length; i++)
                    {
                        string pathPart = pathParts[i];
                        TreeNode[] foundNodes = parent.Nodes.Find(pathPart, false);
                        TreeNode partNode;
                        if (foundNodes.Length == 0 || i == pathParts.Length-1)
                        {
                            partNode = new TreeNode(pathPart);
                            partNode.Name = pathPart;
                            parent.Nodes.Add(partNode);
                            if (i == pathParts.Length - 1)
                            {
                                partNode.ImageIndex = _iconProvider.GetIconIndexForFile(name);
                                partNode.Tag = name;
                            }
                            else partNode.ImageIndex = _iconProvider.FolderIndex;
                        }
                        else
                        {
                            partNode = foundNodes[0];
                        }
                        partNode.SelectedImageIndex = partNode.ImageIndex;
                        parent = partNode;
                    }
                }
                return rootNode;
            }

            private static string ExtractZipEntryToFile(string entryName, ZipArchive zip)
            {
                ZipArchiveEntry entry = zip.GetEntry(entryName);
                string tempPath = Path.GetTempPath() + Path.GetFileName(entry.Name);
                entry.ExtractToFile(tempPath);
                return tempPath;
            }

            private FileTypeIconProvider _iconProvider = new FileTypeIconProvider();

            protected override void Dispose(bool disposing)
            {
                if (disposing && _iconProvider != null)
                {
                    _iconProvider.Dispose();
                    _iconProvider = null;
                }
                base.Dispose(disposing);
            }
        }
    }
}