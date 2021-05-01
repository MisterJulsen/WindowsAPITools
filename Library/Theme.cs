using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinAPITools.Controls;

namespace WinAPITools
{
    public class Theme : DarkModeExtension
    {
       
        /// <summary>
        /// Simplest way to toggle between light and dark mode.
        /// </summary>
        /// <param name="theme">The theme which should be applied.</param>
        /// <param name="ctrl">The base control.</param>
        public static void SetTheme(ThemeType theme, Control ctrl, Action<ThemeType, Control> custom = null, bool formBackColor = true)
        {
            if (theme == ThemeType.DARK)
            {
                if (ctrl is Form) UseImmersiveDarkMode(((Form)ctrl).Handle, true);
                MakeAllDark(ctrl, custom, formBackColor);
                return;
            }

            DefaultCtrlData data = null;
            if (ctrl is Form) UseImmersiveDarkMode(((Form)ctrl).Handle, false);
            switch (ctrl)
            {
                case Form form:
                    if (!formBackColor) break;
                    if (ctrl.Tag is DefaultCtrlData) data = (DefaultCtrlData)ctrl.Tag;
                    if (data != null)
                    {
                        ctrl.BackColor = data.backcolor;
                        ctrl.ForeColor = data.forecolor;
                        ctrl.Tag = null;
                    }

                    if (form.Menu != null)
                    {
                        MainMenu mnu = form.Menu;
                        form.Menu = null;
                        for (int i = 0; i < mnu.MenuItems.Count; i++)
                        {
                            MenuItem itm = mnu.MenuItems[i];
                            itm.OwnerDraw = false;
                        }
                        form.Menu = mnu;
                    }
                    break;
                case MdiClient client:
                    if (!(client == null))
                    {
                        client.BackColor = SystemColors.AppWorkspace;
                    }
                    break;
                case ComboBox cbb:
                    cbb.FlatStyle = FlatStyle.System;
                    SetControlTheme(ctrl.Handle, ControlTheme.CFD);
                    
                    if (ctrl.Tag is DefaultCtrlData) data = (DefaultCtrlData)ctrl.Tag;
                    if (data != null)
                    {
                        ctrl.BackColor = data.backcolor;
                        ctrl.ForeColor = data.forecolor;
                        ctrl.Tag = null;
                    }                    
                    break;
                case Button btn:
                    if (btn.FlatStyle == FlatStyle.Standard || btn.FlatStyle == FlatStyle.System)
                    {
                        btn.FlatStyle = FlatStyle.System;
                        SetControlTheme(ctrl.Handle, ControlTheme.EXPLORER_DEFAULT);
                    }
                    break;
                case RadioButton rbtn:
                    if (rbtn.FlatStyle != FlatStyle.Standard && rbtn.FlatStyle != FlatStyle.System) break;
                    if (rbtn.Tag is DefaultCtrlData) data = (DefaultCtrlData)rbtn.Tag;
                    if (data != null)
                    {
                        rbtn.ForeColor = data.forecolor;
                        rbtn.FlatStyle = data.flatstyle;
                        rbtn.Tag = null;
                    }
                    break;
                case CheckBox chb:
                    if (chb.FlatStyle != FlatStyle.Standard && chb.FlatStyle != FlatStyle.System) break;
                    if (chb.Tag is DefaultCtrlData) data = (DefaultCtrlData)chb.Tag;
                    if (data != null)
                    {
                        chb.ForeColor = data.forecolor;
                        chb.FlatStyle = data.flatstyle;
                        chb.Tag = null;
                    }
                    break;
                case TextBox _:
                    SetControlTheme(ctrl.Handle, ControlTheme.CFD);
                    if (ctrl.Tag is DefaultCtrlData) data = (DefaultCtrlData)ctrl.Tag;
                    if (data != null)
                    {
                        ctrl.BackColor = data.backcolor;
                        ctrl.ForeColor = data.forecolor;
                        ctrl.Tag = null;
                    }
                    break;
                case MaskedTextBox _:
                    SetControlTheme(ctrl.Handle, ControlTheme.CFD);
                    if (ctrl.Tag is DefaultCtrlData) data = (DefaultCtrlData)ctrl.Tag;
                    if (data != null)
                    {
                        ctrl.BackColor = data.backcolor;
                        ctrl.ForeColor = data.forecolor;
                        ctrl.Tag = null;
                    }
                    break;
                case TreeView tv:
                    tv.HotTracking = true;
                    SetControlTheme(ctrl.Handle, ControlTheme.EXPLORER_DEFAULT);
                    if (ctrl.Tag is DefaultCtrlData) data = (DefaultCtrlData)ctrl.Tag;
                    if (data != null)
                    {
                        ctrl.BackColor = data.backcolor;
                        ctrl.ForeColor = data.forecolor;
                        ctrl.Tag = null;
                    }
                    break;
                case ListView lv:
                    SetControlTheme(ctrl.Handle, ControlTheme.NATIVE_LISTVIEW);
                    SendMessage(lv.Handle, 0x1000 + 54, 0x00010000, 0x00010000);
                    if (ctrl.Tag is DefaultCtrlData) data = (DefaultCtrlData)ctrl.Tag;
                    if (data != null)
                    {
                        ctrl.BackColor = data.backcolor;
                        ctrl.ForeColor = data.forecolor;
                        ctrl.Tag = null;
                    }
                    break;
                case ListBox _:
                    SetControlTheme(ctrl.Handle, ControlTheme.EXPLORER_DEFAULT);
                    if (ctrl.Tag is DefaultCtrlData) data = (DefaultCtrlData)ctrl.Tag;
                    if (data != null)
                    {
                        ctrl.BackColor = data.backcolor;
                        ctrl.ForeColor = data.forecolor;
                        ctrl.Tag = null;
                    }
                    break;
                case GroupBox _:
                    SetControlTheme(ctrl.Handle, ControlTheme.EXPLORER_DEFAULT);
                    if (ctrl.Tag is DefaultCtrlData) data = (DefaultCtrlData)ctrl.Tag;
                    if (data != null)
                    {
                        ctrl.ForeColor = data.forecolor;
                        ctrl.Tag = null;
                    }
                    break;
                case FlatTabControl tc:                    
                    foreach (TabPage page in tc.TabPages)
                    {
                        if (page.Tag is DefaultCtrlData) data = (DefaultCtrlData)page.Tag;
                        if (data != null)
                        {
                            page.BackColor = data.backcolor;
                            page.ForeColor = data.forecolor;
                            page.Tag = null;
                        }
                    }
                    break;
                case StatusStrip strip:
                    if (strip.RenderMode == ToolStripRenderMode.System)
                    {
                        SetControlTheme(ctrl.Handle, ControlTheme.EXPLORER_DEFAULT);
                        if (ctrl.Tag is DefaultCtrlData) data = (DefaultCtrlData)ctrl.Tag;
                        if (data != null)
                        {
                            ctrl.BackColor = data.backcolor;
                            ctrl.ForeColor = data.forecolor;
                            ctrl.Tag = null;
                        }
                    }
                    else
                    {
                        if (ctrl.Tag is DefaultCtrlData) data = (DefaultCtrlData)ctrl.Tag;
                        if (data != null)
                        {
                            ctrl.ForeColor = data.forecolor;
                            ctrl.Tag = null;
                        }
                        foreach (ToolStripItem itm in strip.Items)
                        {
                            ColorAllToolStripItems(itm, Color.Black);
                        }
                        strip.Renderer = LightToolStripRenderer();
                    }
                    break;
                case ToolStrip strip:
                    if (strip.RenderMode == ToolStripRenderMode.System)
                    {
                        SetControlTheme(ctrl.Handle, ControlTheme.EXPLORER_DEFAULT);
                        if (ctrl.Tag is DefaultCtrlData) data = (DefaultCtrlData)ctrl.Tag;
                        if (data != null)
                        {
                            ctrl.BackColor = data.backcolor;
                            ctrl.ForeColor = data.forecolor;
                            ctrl.Tag = null;
                        }
                    }
                    else
                    {
                        if (ctrl.Tag is DefaultCtrlData) data = (DefaultCtrlData)ctrl.Tag;
                        if (data != null)
                        {
                            ctrl.ForeColor = data.forecolor;
                            ctrl.Tag = null;
                        }
                        foreach (ToolStripItem itm in strip.Items)
                        {
                            ColorAllToolStripItems(itm, Color.Black);
                        }
                        strip.Renderer = LightToolStripRenderer();
                    }
                    break;               
                default:                    
                    SetControlTheme(ctrl.Handle, ControlTheme.EXPLORER_DEFAULT);
                    break;
            }
            
            foreach (Control c in ctrl.Controls)
            {
                SetTheme(theme, c, custom, formBackColor);
            }

            if (ctrl.ContextMenuStrip != null)
            {
                ContextMenuStrip strip = ctrl.ContextMenuStrip;
                if (strip.RenderMode == ToolStripRenderMode.System)
                {
                    SetControlTheme(ctrl.Handle, ControlTheme.EXPLORER_DEFAULT);
                    if (ctrl.Tag is DefaultCtrlData) data = (DefaultCtrlData)ctrl.Tag;
                    if (data != null)
                    {
                        ctrl.BackColor = data.backcolor;
                        ctrl.ForeColor = data.forecolor;
                        ctrl.Tag = null;
                    }
                }
                else
                {
                    if (ctrl.Tag is DefaultCtrlData) data = (DefaultCtrlData)ctrl.Tag;
                    if (data != null)
                    {
                        ctrl.ForeColor = data.forecolor;
                        ctrl.Tag = null;
                    }
                    foreach (ToolStripItem itm in strip.Items)
                    {
                        ColorAllToolStripItems(itm, Color.Black);
                    }
                    strip.Renderer = LightToolStripRenderer();
                }
            }

            if (custom != null)
            {
                custom(theme, ctrl);
            }

        }
    }

    public enum ThemeType
    {
        LIGHT,
        DARK
    }
}
