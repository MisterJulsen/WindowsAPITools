using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAPITools.Controls
{
    public class ModernLinkLabel : Label
    {
        private Color currentColor = Color.Black;
        private Font currentFont;

        public ModernLinkLabel()
        {

        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.currentFont = Font;
            currentFont = new Font(Font, FontStyle.Regular);
            this.currentColor = Color.FromKnownColor(KnownColor.Highlight);
            try
            {
                Cursor = CntrlTools.HandCursor;
            }
            catch (Exception)
            {
                Cursor = Cursors.Hand;
            }
            this.Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            string measureString = this.Text;
            TextRenderer.DrawText(e.Graphics, measureString, currentFont, new Point(0, 0), currentColor);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            currentFont = new Font(Font, FontStyle.Underline);
            this.currentColor = Color.FromKnownColor(KnownColor.Highlight);
            this.Refresh();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            currentFont = new Font(Font, FontStyle.Regular);
            this.currentColor = Color.FromKnownColor(KnownColor.Highlight);
            this.Refresh();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            currentFont = new Font(Font, FontStyle.Underline);
            this.currentColor = Color.Red;
            this.Refresh();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            currentFont = new Font(Font, FontStyle.Underline);
            this.currentColor = Color.FromKnownColor(KnownColor.Highlight);
            this.Refresh();
        }
    }
}
