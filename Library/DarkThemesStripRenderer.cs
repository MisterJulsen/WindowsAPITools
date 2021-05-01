using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAPITools
{

    public class DarkThemesStripRenderer : ToolStripProfessionalRenderer
    {
        public DarkThemesStripRenderer(ProfessionalColorTable professionalColorTable) : base(professionalColorTable)
        {
            this.RoundedEdges = true;
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            e.ArrowColor = Color.LightGray;      
            
            base.OnRenderArrow(e);
        }

        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(this.ColorTable.CheckBackground), new Rectangle(e.Item.ContentRectangle.X + 1, e.Item.ContentRectangle.Y, e.Item.ContentRectangle.Height, e.Item.ContentRectangle.Height));
            e.Graphics.DrawImage(Properties.Resources.tick, new Point(e.ImageRectangle.X + e.ImageRectangle.Width / 2 - 6, e.ImageRectangle.Y + e.ImageRectangle.Height / 2 - 7));
        }


    }

    public class DarkThemedStripColorTable : ProfessionalColorTable
    {
        private Color border = Color.FromArgb(120, 120, 120);
        private Color inside = Color.FromArgb(65, 65, 65);
        private Color barButton = Color.FromArgb(80, 80, 80);
        private Color panelCol = Color.FromArgb(60, 60, 60);
        private Color baseCol = Color.FromArgb(43, 43, 43);

        public override Color ButtonSelectedGradientBegin
        {
            get { return barButton; }
        }

        public override Color ButtonSelectedGradientMiddle
        {
            get { return barButton; }
        }

        public override Color ButtonSelectedGradientEnd
        {
            get { return barButton; }
        }

        public override Color ButtonSelectedBorder
        {
            get { return border; }
        }

        public override Color StatusStripGradientBegin
        {
            get { return panelCol; }
        }

        public override Color StatusStripGradientEnd
        {
            get { return panelCol; }
        }

        public override Color ToolStripGradientBegin
        {
            get { return panelCol; }
        }

        public override Color ToolStripGradientMiddle
        {
            get { return panelCol; }
        }
        public override Color ToolStripGradientEnd
        {
            get { return panelCol; }
        }

        public override Color ButtonPressedGradientBegin
        {
            get { return Color.FromArgb(barButton.R + 40, barButton.G + 40, barButton.B + 40); }
        }

        public override Color ButtonPressedGradientMiddle
        {
            get { return Color.FromArgb(barButton.R + 40, barButton.G + 40, barButton.B + 40); }
        }

        public override Color ButtonPressedGradientEnd
        {
            get { return Color.FromArgb(barButton.R + 40, barButton.G + 40, barButton.B + 40); }
        }

        public override Color ButtonPressedBorder
        {
            get { return Color.FromArgb(border.R + 40, border.G + 40, border.B + 40); }
        }

        public override Color GripDark
        {
            get { return baseCol; }
        }

        public override Color GripLight
        {
            get { return inside; }
        }

        public override Color ToolStripPanelGradientBegin
        {
            get { return inside; }
        }

        public override Color ToolStripPanelGradientEnd
        {
            get { return inside; }
        }

        public override Color MenuBorder
        {
            get { return border; }
        }

        public override Color MenuStripGradientBegin
        {
            get { return baseCol; }
        }

        public override Color MenuStripGradientEnd
        {
            get { return baseCol; }
        }

        public override Color MenuItemBorder
        {
            get { return baseCol; }
        }

        public override Color MenuItemSelectedGradientBegin
        {
            get { return inside; }
        }

        public override Color MenuItemSelectedGradientEnd
        {
            get { return inside; }
        }

        public override Color MenuItemSelected
        {
            get { return inside; }
        }

        public override Color MenuItemPressedGradientBegin
        {
            get { return Color.FromArgb(inside.R + 40, inside.G + 40, inside.B + 40); }
        }

        public override Color MenuItemPressedGradientEnd
        {
            get { return Color.FromArgb(inside.R + 40, inside.G + 40, inside.B + 40); }
        }

        public override Color MenuItemPressedGradientMiddle
        {
            get { return Color.FromArgb(inside.R + 40, inside.G + 40, inside.B + 40); }
        }

        public override Color ToolStripDropDownBackground
        {
            get { return baseCol; }
        }

        public override Color ToolStripContentPanelGradientBegin
        {
            get { return baseCol; }
        }

        public override Color ToolStripContentPanelGradientEnd
        {
            get { return baseCol; }
        }

        public override Color ImageMarginGradientBegin
        {
            get { return Color.FromArgb(baseCol.R - 1, baseCol.G - 1, baseCol.B - 1); }
        }

        public override Color ImageMarginGradientMiddle
        {
            get { return Color.FromArgb(baseCol.R - 1, baseCol.G - 1, baseCol.B - 1); }
        }

        public override Color ImageMarginGradientEnd
        {
            get { return Color.FromArgb(baseCol.R - 1, baseCol.G - 1, baseCol.B - 1); }
        }

        public override Color SeparatorDark
        {
            get { return border; }
        }

        public override Color SeparatorLight
        {
            get { return baseCol; }
        }

        public override Color ToolStripBorder
        {
            get { return border; }
        }

        public override Color ButtonCheckedGradientBegin
        {
            get { return Color.FromArgb(barButton.R + 15, barButton.G + 15, barButton.B + 15); }
        }

        public override Color ButtonCheckedGradientEnd
        {
            get { return Color.FromArgb(barButton.R + 15, barButton.G + 15, barButton.B + 15); }
        }

        public override Color ButtonCheckedGradientMiddle
        {
            get { return Color.FromArgb(barButton.R + 15, barButton.G + 15, barButton.B + 15); }
        }

        public override Color ButtonCheckedHighlightBorder
        {
            get { return border; }
        }

        public override Color ButtonCheckedHighlight
        {
            get { return border; }
        }

        public override Color CheckBackground
        {
            get { return inside; }
        }

        public override Color CheckSelectedBackground
        {
            get { return Color.FromArgb(inside.R * 2, inside.G * 2, inside.B * 2); }
        }

        public override Color CheckPressedBackground
        {
            get { return Color.FromArgb(inside.R * 2, inside.G * 2, inside.B * 2); }
        }

        public override Color OverflowButtonGradientBegin
        {
            get { return border; }
        }

        public override Color OverflowButtonGradientMiddle
        {
            get { return border; }
        }

        public override Color OverflowButtonGradientEnd
        {
            get { return border; }
        }


    }
}
