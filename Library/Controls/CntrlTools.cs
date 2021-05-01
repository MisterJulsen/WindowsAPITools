using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAPITools.Controls
{
    public class CntrlTools
    {
		/// <summary>
		/// Gets the system's hand mouse cursor, used for hyperlinks.
		/// The .NET framework only gives its internal cursor but not the one that the user has set in their profile.
		/// </summary>
		public static Cursor HandCursor
		{
			get
			{
				RegistryKey cursorsKey = Registry.CurrentUser.OpenSubKey(@"Control Panel\Cursors");
				if (cursorsKey != null)
				{
					object o = cursorsKey.GetValue("Hand");
					if (o is string)
					{
						IntPtr cursorHandle = NativeMethods.LoadCursorFromFile((string)o);
						return new Cursor(cursorHandle);
					}
				}
				return Cursors.Hand;
			}
		}

		/// <summary>
		/// Sets the font for a Form or Control and all its child controls, regarding different
		/// font style and size as set in the VS designer.
		/// </summary>
		/// <param name="ctl"></param>
		/// <param name="oldFont"></param>
		/// <param name="newFont"></param>
		public static void SetFont(Control ctl, Font oldFont, Font newFont, int recursion = 0)
		{
			if (oldFont != newFont)
			{
				ctl.SuspendLayout();
				foreach (Control subCtl in ctl.Controls)
				{
					SetFont(subCtl, oldFont, newFont, recursion + 1);
				}

				if (ctl.Font != oldFont)
				{
					if (ctl.Font.FontFamily.Name == oldFont.FontFamily.Name)
					{
						ctl.Font = new Font(newFont.FontFamily, ctl.Font.SizeInPoints * (newFont.SizeInPoints / oldFont.SizeInPoints), ctl.Font.Style);
					}
				}

				if (recursion == 0)
				{
					ctl.Font = newFont;
				}
				ctl.ResumeLayout();
			}
		}

		private static class NativeMethods
		{
			[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
			public static extern IntPtr LoadCursorFromFile(string path);			
		}
	}
}
