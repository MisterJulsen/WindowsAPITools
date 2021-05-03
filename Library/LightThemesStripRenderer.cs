using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAPITools
{
    public class LightThemesStripRenderer : ToolStripProfessionalRenderer
    {
        public LightThemesStripRenderer(ProfessionalColorTable professionalColorTable) : base(professionalColorTable)
        {
            this.RoundedEdges = true;
        }

        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {            
            e.Graphics.FillRectangle(new SolidBrush(this.ColorTable.CheckBackground), new Rectangle(e.Item.ContentRectangle.X + 1, e.Item.ContentRectangle.Y, e.Item.ContentRectangle.Height, e.Item.ContentRectangle.Height));
            e.Graphics.DrawImage(Properties.Resources.black_tick, new Point(e.ImageRectangle.X + e.ImageRectangle.Width / 2 - 6, e.ImageRectangle.Y + e.ImageRectangle.Height / 2 - 7));
        }


    }

    public class LightThemedStripColorTable : ProfessionalColorTable
    {
        private Color panelBorder = Color.FromArgb(204, 204, 204);
        private Color panelInside = Color.FromArgb(242, 242, 242);

        private Color selButtonBorder = Color.FromArgb(100, 0, 140, 255);
        private Color selButtonInside = Color.FromArgb(50, 0, 140, 255);

        private Color pressedButtonBorder = Color.FromArgb(100, 0, 140, 255);
        private Color pressedButtonInside = Color.FromArgb(80, 0, 140, 255);

        private Color selMenuItem = Color.FromArgb(110, 0, 140, 255);

        public override Color ButtonSelectedGradientBegin
        {
            get { return selButtonInside; }
        }

        public override Color ButtonSelectedGradientMiddle
        {
            get { return selButtonInside; }
        }

        public override Color ButtonSelectedGradientEnd
        {
            get { return selButtonInside; }
        }

        public override Color ButtonSelectedBorder
        {
            get { return selButtonBorder; }
        }

        public override Color StatusStripGradientBegin
        {
            get { return panelInside; }
        }

        public override Color StatusStripGradientEnd
        {
            get { return panelInside; }
        }

        public override Color ToolStripGradientBegin
        {
            get { return panelInside; }
        }

        public override Color ToolStripGradientMiddle
        {
            get { return panelInside; }
        }
        public override Color ToolStripGradientEnd
        {
            get { return panelInside; }
        }

        public override Color ButtonPressedGradientBegin
        {
            get { return pressedButtonInside; }
        }

        public override Color ButtonPressedGradientMiddle
        {
            get { return pressedButtonInside; }
        }

        public override Color ButtonPressedGradientEnd
        {
            get { return pressedButtonInside; }
        }

        public override Color ButtonPressedBorder
        {
            get { return pressedButtonBorder; }
        }

        public override Color GripDark
        {
            get { return panelBorder; }
        }

        public override Color GripLight
        {
            get { return panelInside; }
        }

        public override Color ToolStripPanelGradientBegin
        {
            get { return panelInside; }
        }

        public override Color ToolStripPanelGradientEnd
        {
            get { return panelInside; }
        }

        public override Color MenuBorder
        {
            get { return panelBorder; }
        }

        public override Color MenuStripGradientBegin
        {
            get { return panelInside; }
        }

        public override Color MenuStripGradientEnd
        {
            get { return panelInside; }
        }

        public override Color MenuItemBorder
        {
            get { return panelInside; }
        }

        public override Color MenuItemSelectedGradientBegin
        {
            get { return selMenuItem; }
        }

        public override Color MenuItemSelectedGradientEnd
        {
            get { return selMenuItem; }
        }

        public override Color MenuItemSelected
        {
            get { return selMenuItem; }
        }

        public override Color MenuItemPressedGradientBegin
        {
            get { return selMenuItem; }
        }

        public override Color MenuItemPressedGradientEnd
        {
            get { return selMenuItem; }
        }

        public override Color MenuItemPressedGradientMiddle
        {
            get { return selMenuItem; }
        }

        public override Color ToolStripDropDownBackground
        {
            get { return panelInside; }
        }

        public override Color ToolStripContentPanelGradientBegin
        {
            get { return panelInside; }
        }

        public override Color ToolStripContentPanelGradientEnd
        {
            get { return panelInside; }
        }

        public override Color ImageMarginGradientBegin
        {
            get { return Color.FromArgb(panelInside.R - 1, panelInside.G - 1, panelInside.B - 1); }
        }

        public override Color ImageMarginGradientMiddle
        {
            get { return Color.FromArgb(panelInside.R - 1, panelInside.G - 1, panelInside.B - 1); }
        }

        public override Color ImageMarginGradientEnd
        {
            get { return Color.FromArgb(panelInside.R - 1, panelInside.G - 1, panelInside.B - 1); }
        }

        public override Color SeparatorDark
        {
            get { return panelBorder; }
        }

        public override Color SeparatorLight
        {
            get { return panelInside; }
        }

        public override Color ToolStripBorder
        {
            get { return panelBorder; }
        }

        public override Color ButtonCheckedGradientBegin
        {
            get { return pressedButtonInside; }
        }

        public override Color ButtonCheckedGradientEnd
        {
            get { return pressedButtonInside; }
        }

        public override Color ButtonCheckedGradientMiddle
        {
            get { return pressedButtonInside; }
        }

        public override Color ButtonCheckedHighlightBorder
        {
            get { return pressedButtonBorder; }
        }

        public override Color ButtonCheckedHighlight
        {
            get { return pressedButtonInside; }
        }

        public override Color CheckBackground
        {
            get { return selMenuItem; }
        }

        public override Color CheckSelectedBackground
        {
            get { return selMenuItem; }
        }

        public override Color CheckPressedBackground
        {
            get { return selMenuItem; }
        }

        public override Color OverflowButtonGradientBegin
        {
            get { return panelBorder; }
        }

        public override Color OverflowButtonGradientMiddle
        {
            get { return panelBorder; }
        }

        public override Color OverflowButtonGradientEnd
        {
            get { return panelBorder; }
        }


    }
}
