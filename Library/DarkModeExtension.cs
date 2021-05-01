using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinAPITools.Controls;

namespace WinAPITools
{
    public class DarkModeExtension : NativeDarkMode
    {
        
        /// <summary>
        /// This method applies a dark theme (system dark theme or custom theme) for alle controls in the given control.
        /// </summary>
        /// <param name="ctrl">A form or container control which should be changed.</param>
        public static void MakeAllDark(Control ctrl, Action<ThemeType, Control> custom = null, bool formBackColor = true)
        {
            
            switch (ctrl)
            {
                case ComboBox cbb:
                    cbb.FlatStyle = FlatStyle.System;
                    SetControlTheme(ctrl.Handle, ControlTheme.DARK_CFD);
                    if (ctrl.Tag == null) ctrl.Tag = new DefaultCtrlData() { backcolor = ctrl.BackColor, forecolor = ctrl.ForeColor };
                    ctrl.BackColor = Color.FromArgb(56, 56, 56);
                    ctrl.ForeColor = Color.LightGray;
                    break;
                case Button btn:
                    if (btn.FlatStyle == FlatStyle.Standard || btn.FlatStyle == FlatStyle.System)
                    {
                        btn.FlatStyle = FlatStyle.System;
                        SetControlTheme(ctrl.Handle, ControlTheme.EXPLORER_DARK);
                    }
                    break;
                case RadioButton rbtn:
                    if (rbtn.FlatStyle != FlatStyle.Standard && rbtn.FlatStyle != FlatStyle.System) break;
                    if (rbtn.Tag == null) ctrl.Tag = new DefaultCtrlData() { backcolor = rbtn.BackColor, forecolor = rbtn.ForeColor, flatstyle = rbtn.FlatStyle };
                    rbtn.FlatStyle = FlatStyle.Standard;
                    rbtn.ForeColor = Color.LightGray;
                    break;
                case CheckBox chb:
                    if (chb.FlatStyle != FlatStyle.Standard && chb.FlatStyle != FlatStyle.System) break;
                    if (chb.Tag == null) ctrl.Tag = new DefaultCtrlData() { backcolor = chb.BackColor, forecolor = chb.ForeColor, flatstyle = chb.FlatStyle };
                    chb.FlatStyle = FlatStyle.Standard;
                    chb.ForeColor = Color.LightGray;
                    break;
                case TextBox _:
                    SetControlTheme(ctrl.Handle, ControlTheme.DARK_CFD);
                    if (ctrl.Tag == null) ctrl.Tag = new DefaultCtrlData() { backcolor = ctrl.BackColor, forecolor = ctrl.ForeColor };
                    ctrl.BackColor = Color.FromArgb(56, 56, 56);
                    ctrl.ForeColor = Color.LightGray;
                    break;
                case MaskedTextBox _:
                    SetControlTheme(ctrl.Handle, ControlTheme.DARK_CFD);
                    if (ctrl.Tag == null) ctrl.Tag = new DefaultCtrlData() { backcolor = ctrl.BackColor, forecolor = ctrl.ForeColor };
                    ctrl.BackColor = Color.FromArgb(56, 56, 56);
                    ctrl.ForeColor = Color.LightGray;
                    break;
                case TreeView tv:
                    tv.HotTracking = true;
                    SetControlTheme(ctrl.Handle, ControlTheme.EXPLORER_DARK);
                    if (ctrl.Tag == null) ctrl.Tag = new DefaultCtrlData() { backcolor = ctrl.BackColor, forecolor = ctrl.ForeColor };
                    ctrl.BackColor = Color.FromArgb(45, 45, 45);
                    ctrl.ForeColor = Color.LightGray;
                    break;
                case ListView lv:
                    SetControlTheme(ctrl.Handle, ControlTheme.NATIVE_LISTVIEW_DARK);
                    SendMessage(lv.Handle, 0x1000 + 54, 0x00010000, 0x00010000);
                    if (ctrl.Tag == null) ctrl.Tag = new DefaultCtrlData() { backcolor = ctrl.BackColor, forecolor = ctrl.ForeColor };
                    ctrl.BackColor = Color.FromArgb(45, 45, 45);
                    ctrl.ForeColor = Color.LightGray;
                    break;
                case ListBox _:
                    SetControlTheme(ctrl.Handle, ControlTheme.EXPLORER_DARK);
                    if (ctrl.Tag == null) ctrl.Tag = new DefaultCtrlData() { backcolor = ctrl.BackColor, forecolor = ctrl.ForeColor };
                    ctrl.BackColor = Color.FromArgb(45, 45, 45);
                    ctrl.ForeColor = Color.LightGray;
                    break;
                case GroupBox _:
                    SetControlTheme(ctrl.Handle, ControlTheme.EXPLORER_DARK);
                    if (ctrl.Tag == null) ctrl.Tag = new DefaultCtrlData() { forecolor = ctrl.ForeColor };
                    ctrl.ForeColor = Color.LightGray;
                    break;
                case FlatTabControl tc:
                   foreach (TabPage page in tc.TabPages)
                    {
                        if (page.Tag == null) page.Tag = new DefaultCtrlData() { backcolor = page.BackColor, forecolor = page.ForeColor };
                        page.BackColor = Color.FromArgb(50, 50, 52);
                        page.ForeColor = Color.LightGray;
                    }
                    break;
                case StatusStrip strip:
                    if (strip.RenderMode == ToolStripRenderMode.System)
                    {
                        SetControlTheme(ctrl.Handle, ControlTheme.EXPLORER_DARK);
                        if (ctrl.Tag == null) ctrl.Tag = new DefaultCtrlData() { backcolor = ctrl.BackColor, forecolor = ctrl.ForeColor };
                        ctrl.ForeColor = Color.LightGray;
                        ctrl.BackColor = Color.FromArgb(45, 45, 45);
                    }
                    else
                    {
                        if (ctrl.Tag == null) ctrl.Tag = new DefaultCtrlData() { backcolor = ctrl.BackColor, forecolor = ctrl.ForeColor };
                        ctrl.ForeColor = Color.LightGray;
                        foreach (ToolStripItem itm in strip.Items)
                        {
                            ColorAllToolStripItems(itm, Color.LightGray);
                        }
                        strip.Renderer = DarkToolStripRenderer();
                    }
                    break;
                case ToolStrip strip:
                    if (strip.RenderMode == ToolStripRenderMode.System)
                    {
                        SetControlTheme(ctrl.Handle, ControlTheme.EXPLORER_DARK);
                        if (ctrl.Tag == null) ctrl.Tag = new DefaultCtrlData() { backcolor = ctrl.BackColor, forecolor = ctrl.ForeColor };
                        ctrl.ForeColor = Color.LightGray;
                        ctrl.BackColor = Color.FromArgb(45, 45, 45);
                    }
                    else
                    {
                        if (ctrl.Tag == null) ctrl.Tag = new DefaultCtrlData() { backcolor = ctrl.BackColor, forecolor = ctrl.ForeColor };
                        ctrl.ForeColor = Color.White;
                        foreach (ToolStripItem itm in strip.Items)
                        {
                            ColorAllToolStripItems(itm, Color.LightGray);
                        }
                        strip.Renderer = DarkToolStripRenderer();
                    }
                    break;
                default:                    
                    SetControlTheme(ctrl.Handle, ControlTheme.EXPLORER_DARK);
                    break;
            }

            foreach (Control c in ctrl.Controls)
            {
                MakeAllDark(c, custom, formBackColor);
            }

            if (ctrl.ContextMenuStrip != null)
            {
                ContextMenuStrip strip = ctrl.ContextMenuStrip;
                if (strip.RenderMode == ToolStripRenderMode.System)
                {
                    SetControlTheme(ctrl.Handle, ControlTheme.EXPLORER_DARK);
                    if (ctrl.Tag == null) ctrl.Tag = new DefaultCtrlData() { backcolor = ctrl.BackColor, forecolor = ctrl.ForeColor };
                    ctrl.ForeColor = Color.LightGray;
                    ctrl.BackColor = Color.FromArgb(45, 45, 45);
                }
                else
                {
                    if (ctrl.Tag == null) ctrl.Tag = new DefaultCtrlData() { backcolor = ctrl.BackColor, forecolor = ctrl.ForeColor };
                    ctrl.ForeColor = Color.LightGray;
                    foreach (ToolStripItem itm in strip.Items)
                    {
                        ColorAllToolStripItems(itm, Color.LightGray);
                    }
                    strip.Renderer = DarkToolStripRenderer();
                }
            }

            if (ctrl is Form)
            {
                if (formBackColor)
                {
                    Form form = (Form)ctrl;
                    if (ctrl.Tag == null) ctrl.Tag = new DefaultCtrlData() { backcolor = ctrl.BackColor, forecolor = ctrl.ForeColor };
                    ctrl.BackColor = Color.FromArgb(35, 35, 35);
                    ctrl.ForeColor = Color.LightGray;
                }
            }
            else if (ctrl is MdiClient)
            {
                if (formBackColor)
                {
                    MdiClient client = ctrl as MdiClient;
                    if (!(client == null))
                    {
                        client.BackColor = Color.FromArgb(90, 90, 90);
                    }
                }
            }

            if (custom != null)
            {
                custom(ThemeType.DARK, ctrl);
            }

        }

        /// <summary>
        /// Simpliefied method to apply dark theme for a form. This method will change the window border if supported.
        /// </summary>
        /// <param name="form">The form the dark theme should be applied to.</param>
        public static void Dark(Form form)
        {
            UseImmersiveDarkMode(form.Handle, true);
            MakeAllDark(form);
        }

        /// <summary>
        /// This function applies a customdark theme for the main menu of a form (not menu stirp!). This function is not included in the MakeAllDark function because every time this method is called a new item renderer is applied. Please only use the function once per window.
        /// </summary>
        /// <param name="frm">The form which contains the menu which should be changed.</param>
        public static void DarkMainMenu(Form frm)
        {
            MainMenu mnu = frm.Menu;
            if (mnu == null) return;
            frm.Menu = null;
            for (int i = 0; i < mnu.MenuItems.Count; i++)
            {
                MenuItem itm = mnu.MenuItems[i];
                itm.OwnerDraw = true;

                MeasureItemEventHandler mi = new MeasureItemEventHandler(MeasureItem);
                DrawItemEventHandler di1 = null;
                DrawItemEventHandler di2 = new DrawItemEventHandler(DrawItem);

                if (i == mnu.MenuItems.Count - 1)
                {
                    di1 = (s1, e1) =>
                    {
                        Rectangle area = new Rectangle(e1.Bounds.X, e1.Bounds.Y, frm.ClientRectangle.Right, 19);
                        e1.Graphics.FillRectangle(new SolidBrush(baseCol), area);
                        e1.DrawBackground();
                    };
                }
                else if (i == 0)
                {
                    di1 = (s1, e1) =>
                    {
                        if (frm.IsMdiContainer)
                        {
                            if (frm.ActiveMdiChild != null)
                            {
                                Rectangle area = new Rectangle(-100, e1.Bounds.Y, e1.Bounds.Width + 100, 19);
                                e1.Graphics.FillRectangle(new SolidBrush(baseCol), area);
                                e1.Graphics.DrawIcon(frm.ActiveMdiChild.Icon, new Rectangle(e1.Bounds.X - 16, e1.Bounds.Y, 16, 16));
                            }
                        }
                    };
                }

                itm.MeasureItem += mi;
                itm.DrawItem += di1;
                itm.DrawItem += di2;
            }
            frm.Menu = mnu;
        }

        /// <summary>
        /// This function will set the color of all sub items of the given item to the given color.
        /// </summary>
        /// <param name="itm">The base item.</param>
        /// <param name="color">The color.</param>
        public static void ColorAllToolStripItems(ToolStripItem itm, Color color)
        {
            itm.ForeColor = color;
            switch (itm)
            {
                case ToolStripDropDownButton btn:
                    foreach (ToolStripItem i in btn.DropDownItems)
                    {
                        ColorAllToolStripItems(i, color);
                    }
                    break;
                case ToolStripSplitButton btn:
                    foreach (ToolStripItem i in btn.DropDownItems)
                    {
                        ColorAllToolStripItems(i, color);
                    }
                    break;
                case ToolStripMenuItem btn:
                    foreach (ToolStripItem i in btn.DropDownItems)
                    {
                        ColorAllToolStripItems(i, color);
                    }
                    break;
                default:
                    break;
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Dark themed strip renderer.</returns>
        public static ToolStripProfessionalRenderer DarkToolStripRenderer()
        {
            return new DarkThemesStripRenderer(new DarkThemedStripColorTable());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Windows 10 like light themed strip renderer.</returns>
        public static ToolStripProfessionalRenderer LightToolStripRenderer()
        {
            return new LightThemesStripRenderer(new LightThemedStripColorTable());
        }

        private static Color border = Color.FromArgb(120, 120, 120);
        private static Color inside = Color.FromArgb(60, 60, 60);
        private static Color baseCol = Color.FromArgb(43, 43, 43);

        private static void DrawItem(object sender, DrawItemEventArgs e)
        {
            Rectangle rc = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            Rectangle borrc = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
            Font aFont = new Font("Segoe UI", 9, FontStyle.Regular, GraphicsUnit.Point);
            MenuItem customItem = (MenuItem)sender;

            e.Graphics.FillRectangle(new SolidBrush(baseCol), rc);
            TextRenderer.DrawText(e.Graphics, customItem.Text, aFont, rc, Color.White);

            if (e.State == (DrawItemState.NoAccelerator | DrawItemState.HotLight))
            {
                e.Graphics.FillRectangle(new SolidBrush(inside), rc);
                e.Graphics.DrawRectangle(new Pen(border), borrc);

                TextRenderer.DrawText(e.Graphics, customItem.Text, aFont, rc, Color.White);
            }
            else
            {
                if (e.State == (DrawItemState.NoAccelerator | DrawItemState.Selected))
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(inside.R + 40, inside.G + 40, inside.B + 40)), rc);
                    e.Graphics.DrawRectangle(new Pen(Color.FromArgb(border.R + 40, border.G + 40, border.B + 40)), borrc);

                    TextRenderer.DrawText(e.Graphics, customItem.Text, aFont, rc, Color.White);
                }
            }            
            e.DrawFocusRectangle();
        }

        private static void MeasureItem(object sender, MeasureItemEventArgs e)
        {
            MenuItem customItem = (MenuItem)sender;
            Font aFont = new Font("Segoe UI", 9, FontStyle.Regular, GraphicsUnit.Point);
            SizeF stringSize = e.Graphics.MeasureString(customItem.Text, aFont);

            e.ItemWidth = Convert.ToInt32(stringSize.Width);
            e.ItemHeight = 19;
        }
    }
}
