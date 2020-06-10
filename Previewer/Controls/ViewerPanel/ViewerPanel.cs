////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description: View panel for displaying source code.
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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Previewer.Controls;
using Scan;
using SourcePreview;
using HScrollBar = Previewer.Controls.HScrollBar;
using VScrollBar = Previewer.Controls.VScrollBar;

namespace PanelSourceView
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  The Source View Panel Class definition.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public partial class ViewerPanel : Control
    {

        /// <summary>
        ///  Get number of rows of text on Page.
        /// </summary>
        private int PageRows => ((Height - _scrollHorizontal.Height) / FontHeight) - 2;

        /// <summary>
        ///  The Horizonal field.
        /// </summary>
        public int HScrollValue;

        /// <summary>
        ///  The 0 based VScroll field, Top Line of page.
        /// </summary>
        public int VScrollValue;

        /// <summary>
        ///  Gets or sets the background color for the control.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Category("Display")]
        [Description("Set the background color off control")]

        public override Color BackColor
        {
            get { return base.BackColor;}
            set
            {
                _status.BackColor = value;
                base.BackColor = value;
            }
        }

        /// <summary>
        ///  The Zoom field.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DefaultValue(1)]
        [Category("Display")]
        [Description("Zoom factor (multiplier)")]

        public float ZoomValue { get; set; } = 1;

        /// <summary>
        ///  The buffer field.
        /// </summary>
        private IScanner _scanner;

        /// <summary>
        ///  The preview field.
        /// </summary>
        private TextDraw _preview;

        /// <summary>
        ///  The scroll Horizontal field.
        /// </summary>
        private HScrollBar _scrollHorizontal;

        /// <summary>
        ///  The scroll Vertical field.
        /// </summary>
        private VScrollBar _scrollVertical;

        /// <summary>
        ///  The  buffer field.
        /// </summary>
        private ReadBuffer _buffer;

        /// <summary>
        ///  The  status field.
        /// </summary>
        private DocStatus _status;

        /// <summary>
        ///  Get/Set Show Line Numbers.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DefaultValue(false)]
        [Category("Display")]
        [Description("Display Line numbers")]

        public bool ShowLineNumbers
        {
            get { return _preview.ShowLineNumbers; }
            set { _preview.ShowLineNumbers = value; }
        }

        /// <summary>
        ///  Get/Set  language.
        /// </summary>
        public ILanguage Language { get { return _scanner.Language; } set { _scanner.Language = value; } }


       // private static long _mem;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Default Constructor: For SourceViewPanel.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public ViewerPanel()
        {
            InitializeComponent();

            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            _buffer                 = new ReadBuffer();
            _scanner                = new Scanner(_buffer);
            _preview                = new TextDraw();

            _status                 = new DocStatus();
            _scrollVertical         = new VScrollBar();
            _scrollHorizontal       = _status.ScrollHorizontal;

            _scrollVertical.Dock    = DockStyle.Right;
            _status.Dock            = DockStyle.Bottom;

            Dock                    = DockStyle.Fill;

            _scrollVertical.Scroll   += ScrollVerticalOnScroll;
            _scrollHorizontal.Scroll += ScrollHorizontalOnScroll;

            Controls.Add(_scrollVertical);
            Controls.Add(_status);

           // Stopwatch.StartNew();

            _scrollVertical.SmallChange   = 3;
            _scrollHorizontal.SmallChange = 3;

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Form1 Load.
        /// </summary>
        /// <param name="filePath"> The file Path.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void LoadFile(string filePath)
        {
            _scanner.Load(filePath);
            SetLineAndColumn();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Raises the <see cref="E:System.Windows.Forms.Control.MouseWheel" /> event.
        /// </summary>
        /// <param name="e"> A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            int numberOfTextLinesToMove = e.Delta * SystemInformation.MouseWheelScrollLines / 60;
            if (numberOfTextLinesToMove == 0) return;

            if ((ModifierKeys & Keys.Control) == Keys.Control)
            {
                Zoom(numberOfTextLinesToMove);
                return;
            }

            SetVScrollValue(VScrollValue  - numberOfTextLinesToMove);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: On Paint.
        /// </summary>
        /// <param name="e"> The Paint Event arguments e.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (DesignMode) return;

            _preview.ShowBuffer(e.Graphics, _scanner, ClientSize, VScrollValue+1, HScrollValue, ZoomValue);
            SetScrollRange(_preview.RowColSize);
            //long totalMemory = GC.GetTotalMemory(false);
            //var after        = (totalMemory - _mem);
            //_mem             = totalMemory;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: On Resize.
        /// </summary>
        /// <param name="e"> The Event arguments e.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Refresh();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Scroll Horizontal On Scroll.
        /// </summary>
        /// <param name="sender">  The sender.</param>
        /// <param name="e">       The Scroll Event arguments e.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void ScrollHorizontalOnScroll(object sender, ScrollEventArgs e)
        {
            HScrollValue = e.NewValue;
            SetLineAndColumn();
            Refresh();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Scroll Vertical On Scroll.
        /// </summary>
        /// <param name="sender">  The sender.</param>
        /// <param name="e">       The Scroll Event arguments e.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void ScrollVerticalOnScroll(object sender, ScrollEventArgs e)
        {
            VScrollValue = e.NewValue;
            Refresh();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Set Scroll Range.
        /// </summary>
        /// <param name="rowCols"> The row Cols.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetScrollRange(Size rowCols)
        {
            var h                         = _preview.FontHeight;
            var w                         = _preview.FontWidth;
            var rows                      = (Height / (h==0?1:h));
            var cols                      = (Width  / (w==0?1:w));
            _scrollVertical.Maximum       = rowCols.Height;
            _scrollHorizontal.Maximum     = rowCols.Width;
            _scrollHorizontal.Visible     = HScrollValue > 0 || rowCols.Width > cols;
            _scrollVertical.Visible       = (rowCols.Height + 1 > rows);
            _scrollVertical.LargeChange   = rows > 3 ? rows - 2 : 1;
            _scrollHorizontal.LargeChange = cols / 5 + 1;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Zoom based on wheel movement.
        /// </summary>
        /// <param name="numberOfTextLinesToMove"> The number Of Text Lines To Move.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Zoom(int numberOfTextLinesToMove)
        {
            float z = ZoomValue * (numberOfTextLinesToMove > 0 ? 1.1F : .9F);

            if (z < .2) z = .2F;
            if (z > 3)  z = 3F;

            if (Math.Abs(z - ZoomValue) > .01)
            {
                ZoomValue = z;
                Refresh();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Set VScroll Value.
        /// </summary>
        /// <param name="value">  The value.</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetVScrollValue(int value)
        {
            var pageRows                   = PageRows;
            if (value < 0) value           = 0;
            if (value > VScrollValue && value > _scanner.LineCount - pageRows) value = _scanner.LineCount -  pageRows / 2;
            if (value != VScrollValue)
            {
                VScrollValue              = value;
                _scrollVertical.Value     = value;
                SetLineAndColumn();
                Refresh();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Set Line And Column.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SetLineAndColumn()
        {
            _status.Info = $"Line {VScrollValue} Column {HScrollValue}";
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Method: Process command Key.
        /// </summary>
        /// <param name="msg">     [ref] The message.</param>
        /// <param name="keyData">  The key Data.</param>
        /// <returns>
        ///  True if successful
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //var control = (ModifierKeys & Keys.Control) != 0;
            //var shift = (ModifierKeys & Keys.Shift) != 0;
            //var alt = (ModifierKeys & Keys.Alt) != 0;
            var pageRows = PageRows;

            var lastTopLine = _scanner.LineCount - pageRows / 2;
            switch (keyData)
            {
                case Keys.PageDown:
                    SetVScrollValue(VScrollValue + pageRows);
                    break;

                case Keys.PageUp:
                    SetVScrollValue(VScrollValue - pageRows);
                    break;

                case Keys.Escape:
                    break;
                case Keys.End:
                    SetVScrollValue(int.MaxValue);
                    break;

                case Keys.Home:
                    SetVScrollValue(0);
                    break;

                default:
                    return base.ProcessCmdKey(ref msg, keyData);

                    //case Keys.Left:
                    //    break;
                    //case Keys.Up:
                    //    break;
                    //case Keys.Right:
                    //    break;
                    //case Keys.Down:
                    //    break;
                    //case Keys.Insert:
                    //    break;
                    //case Keys.Delete:
                    //    break;
                    //case Keys.F1:
                    //case Keys.Help:
                    //    break;
                    //case Keys.F2:
                    //    break;
                    //case Keys.F3:
                    //    break;
                    //case Keys.F4:
                    //    break;
                    //case Keys.F5:
                    //    break;
                    //case Keys.F6:
                    //    break;
                    //case Keys.F7:
                    //    break;
                    //case Keys.F8:
                    //    break;
                    //case Keys.F9:
                    //    break;
                    //case Keys.F10:
                    //    break;
                    //case Keys.F11:
                    //    break;
                    //case Keys.F12:
                    //    break;

            }
            return true;
        }
    }
}


