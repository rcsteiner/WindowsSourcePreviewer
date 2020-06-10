////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: Simple test of preview window
// 
//  Author:      Robert C. Steiner
// 
//=====================================================[ History ]=====================================================
//  Date        Who      What
//  ----------- ------   ----------------------------------------------------------------------------------------------
//  6/10/2020   RCS      Initial Code.
//====================================================[ Copyright ]====================================================
// 
//  Copyright 2020 Robert C. Steiner
//  
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software
//  and associated documentation files (the "Software"), to deal in the Software without restriction,
//  including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
//  and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
//  subject to the following conditions:
//  
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//  
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
//  INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE
//  AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES
//  OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//  CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Windows.Forms;
using Scan;

namespace TestViewer
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Form1 Class definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public partial class Main : Form
    {


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Default Constructor: For Form1.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Main()
        {
            InitializeComponent();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: open Tool Strip Menu Item Click.
        /// </summary>
        /// <param name="sender">  The sender.</param>
        /// <param name="e">       The Event arguments e.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            if (d.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var fileName = d.FileName;
                    viewer.LoadFile(fileName);
                    viewer.Refresh();
                    // viewer.Language = lang;
                }
                catch (Exception)
                {
                }

            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: rebuild Language Map Tool Strip Menu Item Click.
        /// </summary>
        /// <param name="sender">  The sender.</param>
        /// <param name="e">       The Event arguments e.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void rebuildLanguageMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lm = new LanguageManager();
            lm.RebuildDictionary();
        }
    }
}

