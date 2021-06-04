using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAPITools.Controls
{
    public class ModernLinkLabel : Control
    {
        private Color currentColor = Color.Black;
        private Font currentFont;

        private const uint WM_SETCURSOR = 0x20;
        private Cursor handCursor;

        

        public ModernLinkLabel()            
        {
            TabStop = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Selectable | ControlStyles.SupportsTransparentBackColor, true);
        }

        protected override void WndProc(ref Message msg)
        {
            if (msg.Msg == WM_SETCURSOR)
            {
                if (handCursor == null)
                {
                    // Fetch the real hand cursor from the system settings and cache it
                    if (handCursor == null)
                    {
                        handCursor = CntrlTools.HandCursor;
                    }
                    // Use the system's hand cursor instead of .NET's internal hand cursor
                    Cursor = handCursor;
                }
                else if (handCursor != null && Cursor != handCursor)
                {
                    // Forget the cached cursor
                    handCursor.Dispose();
                    handCursor = null;
                }
            }
            base.WndProc(ref msg);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.currentFont = Font;
            currentFont = new Font(Font, FontStyle.Regular);
            this.currentColor = Color.FromKnownColor(KnownColor.Highlight);
            /*
            try
            {
                Cursor = CntrlTools.HandCursor;
            }
            catch (Exception)
            {
                Cursor = Cursors.Hand;
            }
            */

            this.Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            string measureString = this.Text;
            TextRenderer.DrawText(e.Graphics, measureString, currentFont, new Point(0, 0), currentColor);
            if (Focused) ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.Black, ButtonBorderStyle.Dashed);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            currentFont = new Font(Font, FontStyle.Underline);
            this.currentColor = Color.FromKnownColor(KnownColor.Highlight);
            this.Refresh();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Enter)
            {
                base.OnClick(EventArgs.Empty);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Refresh();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            currentFont = new Font(Font, FontStyle.Regular);
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
            currentFont = new Font(Font, FontStyle.Regular);
            this.currentColor = Color.FromKnownColor(KnownColor.Highlight);
            this.Refresh();
        }


    }
}
