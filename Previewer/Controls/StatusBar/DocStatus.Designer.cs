using System.ComponentModel;
using System.Windows.Forms;

namespace Previewer.Controls
{
    partial class DocStatus
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelMessage = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.ScrollHorizontal = new Previewer.Controls.HScrollBar();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMessage
            // 
            this.panelMessage.AutoSize = true;
            this.panelMessage.BackColor = System.Drawing.Color.Transparent;
            this.panelMessage.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelMessage.Location = new System.Drawing.Point(0, 0);
            this.panelMessage.Name = "panelMessage";
            this.panelMessage.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.panelMessage.Size = new System.Drawing.Size(0, 15);
            this.panelMessage.TabIndex = 5;
            // 
            // statusStrip
            // 
            this.statusStrip.AutoSize = false;
            this.statusStrip.BackColor = System.Drawing.Color.Transparent;
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.Right;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusMessage});
            this.statusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.statusStrip.Location = new System.Drawing.Point(808, 0);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(198, 18);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 3;
            // 
            // statusMessage
            // 
            this.statusMessage.AutoSize = false;
            this.statusMessage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusMessage.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.statusMessage.Name = "statusMessage";
            this.statusMessage.Padding = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.statusMessage.Size = new System.Drawing.Size(196, 15);
            // 
            // ScrollHorizontal
            // 
            this.ScrollHorizontal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScrollHorizontal.LargeChange = 10;
            this.ScrollHorizontal.Location = new System.Drawing.Point(0, 0);
            this.ScrollHorizontal.Maximum = 100;
            this.ScrollHorizontal.Minimum = 0;
            this.ScrollHorizontal.Name = "ScrollHorizontal";
            this.ScrollHorizontal.Size = new System.Drawing.Size(808, 18);
            this.ScrollHorizontal.SmallChange = 1;
            this.ScrollHorizontal.TabIndex = 4;
            this.ScrollHorizontal.Value = 0;
            // 
            // DocStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.GrayText;
            this.Controls.Add(this.ScrollHorizontal);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.panelMessage);
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "DocStatus";
            this.Size = new System.Drawing.Size(1006, 18);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public HScrollBar ScrollHorizontal;
        private Label panelMessage;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusMessage;
    }
}
