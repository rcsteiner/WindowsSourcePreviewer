using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SourcePreview
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Zip Preview Handler Class definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [PreviewHandler("RCS ZIP Preview Handler", ".zip;.jar;.gadget", "{D694DE84-EE56-46E2-8EEC-69B44C86BDA6}")]
    [ProgId("RCS.ZipPreviewHandler")]
    [Guid("AC73332B-6C6F-4250-957A-2B2813CC3FD2")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public  class ZipPreviewHandler : FileBasedPreviewHandler
    {

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Create Preview Handler Control.
        /// </summary>
        /// <returns>
        ///  The MsdnMag.PreviewHandlerControl value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected override PreviewHandlerControl CreatePreviewHandlerControl()
        {
            Trace.WriteLine("ZIP Control Create");

            try
            {
                return new ZipPreviewHandlerControl();
            }
            catch (Exception e)
            {
                Trace.WriteLine($"PreviewHandlerControl exception {e}");
                throw;
            }
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Zip Preview Handler Control Class definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class ZipPreviewHandlerControl : FileBasedPreviewHandlerControl
    {
        private FileTypeIconProvider _iconProvider = new FileTypeIconProvider();

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Load.
        /// </summary>
        /// <param name="file">  The file.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Load(FileInfo file)
        {
            Trace.WriteLine("PreviewHandler Load Start");

            var folderView                = new TreeView();
            folderView.ImageList          = _iconProvider.Icons;
            folderView.Dock               = DockStyle.Fill;
            folderView.FullRowSelect      = true;
            folderView.ShowLines          = true;
            folderView.ShowPlusMinus      = true;
            folderView.ShowRootLines      = true;
            folderView.LabelEdit          = false;
            folderView.BorderStyle        = BorderStyle.None;
            folderView.TreeViewNodeSorter = new NodeSorter();
            folderView.BackColor          = Color.Black;
            folderView.ForeColor          = Color.White;

            var filenames = new List<string>();

            using (var zip = ZipFile.OpenRead(file.FullName))
            {
                foreach (var entry in zip.Entries)
                {
                    {
                        filenames.Add(entry.FullName);
                    }
                }
            }

            var root = ConvertFilenamesToTreeRoot(file.Name, filenames);
            folderView.Nodes.Add(root);
            root.Expand();

            folderView.DoubleClick += delegate
            {
                if (folderView.SelectedNode != null && folderView.SelectedNode.Tag as string != null)
                {
                    var selectedNode = folderView.SelectedNode;
                    var zipToOpen = ZipFile.OpenRead(file.FullName);
                    var tempPath = ExtractZipEntryToFile((string)selectedNode.Tag, zipToOpen);
                    zipToOpen.Dispose();
                    Process.Start(tempPath);
                }
            };

            Controls.Add(folderView);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Convert Filenames To Tree Root.
        /// </summary>
        /// <param name="rootName">  The root Name.</param>
        /// <param name="names">     The names.</param>
        /// <returns>
        ///  The System.Windows.Forms.TreeNode value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private TreeNode ConvertFilenamesToTreeRoot(string rootName, IEnumerable<string> names)
        {
            var rootNode = new TreeNode(rootName);
            rootNode.ImageIndex = _iconProvider.FolderIndex;
            foreach (var name in names)
            {
                var pathParts = name.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                var parent = rootNode;
                for (var i = 0; i < pathParts.Length; i++)
                {
                    var pathPart = pathParts[i];
                    var foundNodes = parent.Nodes.Find(pathPart, false);
                    TreeNode partNode;
                    if (foundNodes.Length == 0 || i == pathParts.Length - 1)
                    {
                        partNode = new TreeNode(pathPart);
                        partNode.Name = pathPart;
                        parent.Nodes.Add(partNode);
                        if (i == pathParts.Length - 1)
                        {
                            partNode.ImageKey = _iconProvider.GetIconIndexForFile(name);
                            partNode.Tag = name;
                        }
                        else
                        {
                            partNode.ImageIndex = _iconProvider.FolderIndex;
                        }
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

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Extract Zip Entry To File.
        /// </summary>
        /// <param name="entryName">  The entry Name.</param>
        /// <param name="zip">        The zip.</param>
        /// <returns>
        ///  The string value.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static string ExtractZipEntryToFile(string entryName, ZipArchive zip)
        {
            var entry = zip.GetEntry(entryName);
            var tempPath = Path.GetTempPath() + Path.GetFileName(entry.Name);
            entry.ExtractToFile(tempPath);
            return tempPath;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Dispose.
        /// </summary>
        /// <param name="disposing">  True if disposing.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected override void Dispose(bool disposing)
        {
            if (disposing && _iconProvider != null)
            {
                _iconProvider.Dispose();
                _iconProvider = null;
            }

            base.Dispose(disposing);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  The Node Sorter Class definition.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public class NodeSorter : IComparer
        {
            int IComparer.Compare(object x, object y)
            {
                var node1 = (TreeNode)x;
                var node2 = (TreeNode)y;

                var tag1 = node1.Tag as string;
                var tag2 = node2.Tag as string;

                if (tag1 == null && tag2 == null)
                {
                    return StrCmpLogicalW(node1.Text, node2.Text);
                }

                if (tag1 == tag2)
                {
                    return 0;
                }

                if (tag1 == null)
                {
                    return -1;
                }

                if (tag2 == null)
                {
                    return 1;
                }

                return StrCmpLogicalW(tag1, tag2);
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// <summary>
            ///  Method: string compare Logical W.
            /// </summary>
            /// <param name="strA">  The string A.</param>
            /// <param name="strB">  The string B.</param>
            /// <returns>
            ///  The integer value.
            /// </returns>
            ////////////////////////////////////////////////////////////////////////////////////////////////////////
            [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
            private static extern int StrCmpLogicalW(string strA, string strB);
        }
    }

}