////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: Status bar with horizontal scroll built in.
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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Previewer.Controls
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Status bar with horizontal scroll built in.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public partial class DocStatus : UserControl
    {
        /// <summary>
        ///  Gets or sets the background color for the control. Causes foreground color to change to white or black.
        ///  Change foreground color after background to get foreground other than the default.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Category("Display")]
        [Description("Color of background.  Causes foreground color to change to white or black.")]
       public override Color BackColor
        {
            get => base.BackColor;
            set
            {
                base.BackColor                        = value;
                ForeColor                             = BackColor.IsDark() ? Color.White : Color.Black;
                ScrollHorizontal.BackColor = value;
                ScrollHorizontal.ForeColor            = ForeColor;
            }
        }

        /// <summary>
        ///  Get/Set foreground Color.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Category("Display")]
        [Description("Color of text (foreground)")]
        public override Color ForeColor
        {
            get => base.ForeColor;
            set
            {
                base.ForeColor             = value;
                base.ForeColor             = value;
                statusMessage.ForeColor    = value;
                panelMessage.ForeColor     = value;
                ScrollHorizontal.ForeColor = value;
            }
        }

        /// <summary>
        ///  Set information.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Category("Display")]
        [Description("Info panel text in status bar. (right side)")]
        public string Info { set { statusMessage.Text = value; Invalidate();} }

        /// <summary>
        ///  Set Message.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Category("Display")]
        [Description("Message panel text in status bar (left side).")]
        public string Message {  set { panelMessage.Text = value; Invalidate(); } }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Default Constructor: For DocStatus.
        ///  The color of the thumb has 3 levels, normal, hover, mouseDown
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public DocStatus()
        {
            InitializeComponent();
        }
    }
}
