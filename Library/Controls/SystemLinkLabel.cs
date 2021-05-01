using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAPITools.Controls
{
	public class SystemLinkLabel : LinkLabel
	{
		private const uint WM_SETCURSOR = 0x20;

		private Cursor handCursor;

		protected override void WndProc(ref Message msg)
		{
			if (msg.Msg == WM_SETCURSOR)
			{
				if (OverrideCursor == Cursors.Hand)
				{
					// Fetch the real hand cursor from the system settings and cache it
					if (handCursor == null)
					{
						handCursor = CntrlTools.HandCursor;
					}
					// Use the system's hand cursor instead of .NET's internal hand cursor
					OverrideCursor = handCursor;
				}
				else if (handCursor != null && OverrideCursor != handCursor)
				{
					// Forget the cached cursor
					handCursor.Dispose();
					handCursor = null;
				}
			}
			base.WndProc(ref msg);
		}
	}
}
