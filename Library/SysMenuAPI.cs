using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAPITools
{
    public class SysMenuAPI
    {

        /**
         * MICROSOFT DOCUMENTATION: https://docs.microsoft.com/en-us/windows/win32/api/winuser/
         */

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIDNewItem, string lpNewItem);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InsertMenu(IntPtr hMenu, int uPosition, int uFlags, int uIDNewItem, string lpNewItem);

        [DllImport("User32")]
        public static extern int GetMenuItemCount(IntPtr hWnd);

        [DllImport("User32")]
        public static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32")]
        public static extern bool EnableMenuItem(IntPtr hMenu, int uIDEnableItem, int uEnable);

        [DllImport("user32")]
        public static extern int GetMenuState(IntPtr hMenu, int uId, int uFlags);

        [DllImport("user32")]
        public static extern int SetMenuItemBitmaps(IntPtr hMenu, int uPosition, int uFlags, IntPtr uBitmapUnchecked, IntPtr uBitmaapChecked);

        [DllImport("user32")]
        public static extern bool SetCaretBlinkTime(int uMSeconds);

        [DllImport("user32")]
        public static extern bool SetMenuItemInfoA(IntPtr hMenu, int item, bool fByPosition, MenuItemInfo lpmii);

        [DllImport("user32", SetLastError = true)]
        public static extern bool SetMenu(IntPtr hWnd, IntPtr hMenu);

        [DllImport("user32", SetLastError = true)]
        public static extern bool DestroyMenu(IntPtr hMenu);

        [DllImport("user32", SetLastError = true)]
        public static extern int CheckMenuItem(IntPtr hMenu, int uIDCheckItem, int uCheck);

        [DllImport("user32", SetLastError = true)]
        public static extern int GetMenuItemID(IntPtr hMenu, int uPos);

        [DllImport("user32", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool GetMenuItemInfo(IntPtr hMenu, int item, bool fByPosition, MENUITEMINFO lpmii);

        [DllImport("user32", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool GetMenuItemInfo(IntPtr hMenu, int item, bool fByPosition, MenuItemInfo lpmii);

        [DllImport("user32", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool SetMenuItemInfo(IntPtr hMenu, int item, bool fByPosition, ref MenuItemInfo lpmii);

        [DllImport("user32", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr GetSubMenu(IntPtr hMenu, int uPos);

        [DllImport("user32", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr CreateMenu();


        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();


        public static IntPtr GetSystemMenu(IntPtr handle)
        {
            return GetSystemMenu(handle, false);
        }

        public class Flags
        {
            public const int MF_BYPOSITION = 0x400;
            public const int MF_BYCOMMAND = 0x000;

            public const int MF_BITMAP = 0x004;
            public const int MF_CHECKED = 0x008;
            public const int MF_DISABLED = 0x001;
            public const int MF_ENABLED = 0x000;
            public const int MF_GRAYED = 0x001;
            public const int MF_MENUBARBREAK = 0x020;
            public const int MF_MENUBREAK = 0x040;
            public const int MF_OWNERDRAW = 0x100;
            public const int MF_POPUP = 0x010;
            public const int MF_UNCHECKED = 0x000;

            public const int WM_SYSCOMMAND = 0x112;
        }

        public class MenuTypes
        {
            public const int MF_STRING = 0x0;
            public const int MF_SEPARATOR = 0x800;
        }

        public class FMask
        {
            /// <summary>
            /// Retrieves or sets the hbmpItem member.
            /// </summary>
            public const int MIIM_BITMAP = 0x080;
            /// <summary>
            /// Retrieves or sets the hbmpChecked and hbmpUnchecked members.
            /// </summary>
            public const int MIIM_CHECKMARKS = 0x008;
            /// <summary>
            /// Retrieves or sets the dwItemData member.
            /// </summary>
            public const int MIIM_DATA = 0x020;
            /// <summary>
            /// Retrieves or sets the fType member.
            /// </summary>
            public const int MIIM_FTYPE = 0x100;
            /// <summary>
            /// Retrieves or sets the wID member.
            /// </summary>
            public const int MIIM_ID = 0x002;
            /// <summary>
            /// Retrieves or sets the fState member.
            /// </summary>
            public const int MIIM_STATE = 0x001;
            /// <summary>
            /// Retrieves or sets the dwTypeData member.
            /// </summary>
            public const int MIIM_STRING = 0x040;
            /// <summary>
            /// Retrieves or sets the hSubMenu member.
            /// </summary>
            public const int MIIM_SUBMENU = 0x004;
            /// <summary>
            /// Retrieves or sets the fType and dwTypeData members. MIIM_TYPE is replaced by MIIM_BITMAP, MIIM_FTYPE, and MIIM_STRING.
            /// </summary>
            public const int MIIM_TYPE = 0x010;
        }

        public class FType
        {
            /// <summary>
            /// Displays the menu item using a bitmap. The low-order word of the dwTypeData member is the bitmap handle, and the cch member is ignored. MFT_BITMAP is replaced by MIIM_BITMAP and hbmpItem.
            /// </summary>
            public const int MFT_BITMAP = 0x004;
            /// <summary>
            /// Places the menu item on a new line (for a menu bar) or in a new column (for a drop-down menu, submenu, or shortcut menu). For a drop-down menu, submenu, or shortcut menu, a vertical line separates the new column from the old.
            /// </summary>
            public const int MFT_MENUBARBREAK = 0x020;
            /// <summary>
            /// Places the menu item on a new line (for a menu bar) or in a new column (for a drop-down menu, submenu, or shortcut menu). For a drop-down menu, submenu, or shortcut menu, the columns are not separated by a vertical line.
            /// </summary>
            public const int MFT_MENUBREAK = 0x040;
            /// <summary>
            /// Assigns responsibility for drawing the menu item to the window that owns the menu. The window receives a WM_MEASUREITEM message before the menu is displayed for the first time, and a WM_DRAWITEM message whenever the appearance of the menu item must be updated. If this value is specified, the dwTypeData member contains an application-defined value.
            /// </summary>
            public const int MFT_OWNERDRAW = 0x100;
            /// <summary>
            /// Displays selected menu items using a radio-button mark instead of a check mark if the hbmpChecked member is NULL.
            /// </summary>
            public const int MFT_RADIOCHECK = 0x200;
            /// <summary>
            /// Right-justifies the menu item and any subsequent items. This value is valid only if the menu item is in a menu bar.
            /// </summary>
            public const int MFT_RIGHTJUSTIFY = 0x4000;
            /// <summary>
            /// Specifies that menus cascade right-to-left (the default is left-to-right). This is used to support right-to-left languages, such as Arabic and Hebrew.
            /// </summary>
            public const int MFT_RIGHTBORDER = 0x2000;
            /// <summary>
            /// Specifies that the menu item is a separator. A menu item separator appears as a horizontal dividing line. The dwTypeData and cch members are ignored. This value is valid only in a drop-down menu, submenu, or shortcut menu.
            /// </summary>
            public const int MFT_SEPARATOR = 0x800;
            /// <summary>
            /// Displays the menu item using a text string. The dwTypeData member is the pointer to a null-terminated string, and the cch member is the length of the string. MFT_STRING is replaced by MIIM_STRING.
            /// </summary>
            public const int MFT_STRING = 0x000;
        }

        public class FState
        {
            /// <summary>
            /// Checks the menu item. For more information about selected menu items, see the hbmpChecked member.
            /// </summary>
            public const int MFS_CHECKED = 0x008;
            /// <summary>
            /// Specifies that the menu item is the default. A menu can contain only one default menu item, which is displayed in bold.
            /// </summary>
            public const int MFS_DEFAULT = 0x1000;
            /// <summary>
            /// Disables the menu item and grays it so that it cannot be selected. This is equivalent to MFS_GRAYED.
            /// </summary>
            public const int MFS_DISABLED = 0x003;
            /// <summary>
            /// Enables the menu item so that it can be selected. This is the default state.
            /// </summary>
            public const int MFS_ENABLED = 0x000;
            /// <summary>
            /// Disables the menu item and grays it so that it cannot be selected. This is equivalent to MFS_DISABLED.
            /// </summary>
            public const int MFS_GRAYED = 0x003;
            /// <summary>
            /// Highlights the menu item.
            /// </summary>
            public const int MFS_HILITE = 0x080;
            /// <summary>
            /// Unchecks the menu item. For more information about clear menu items, see the hbmpChecked member.
            /// </summary>
            public const int MFS_UNCHECKED = 0x000;
            /// <summary>
            /// Removes the highlight from the menu item. This is the default state.
            /// </summary>
            public const int MFS_UNHILITE = 0x000;
        }

        //A handle to the bitmap to be displayed, or it can be one of the values in the following Enum. 
        //It is used when the MIIM_BITMAP flag is set in the fMask member. (ex. (hBitmap)HBMMENU_SYSTEM)
        public enum MenuItemInfo_hItem
        {
            HBMMENU_CALLBACK = -1,              //A bitmap that is drawn by the window that owns the menu. The application must process the WM_MEASUREITEM and WM_DRAWITEM messages.
            HBMMENU_MBAR_CLOSE = 5,             //Close button for the menu bar.
            HBMMENU_MBAR_CLOSE_D = 6,           //Disabled close button for the menu bar.
            HBMMENU_MBAR_MINIMIZE = 3,          //Minimize button for the menu bar.
            HBMMENU_MBAR_MINIMIZE_D = 7,        //Disabled minimize button for the menu bar.
            HBMMENU_MBAR_RESTORE = 2,           //Restore button for the menu bar.
            HBMMENU_POPUP_CLOSE = 8,            //Close button for the submenu.
            HBMMENU_POPUP_MAXIMIZE = 10,        //Maximize button for the submenu.
            HBMMENU_POPUP_MINIMIZE = 11,        //Minimize button for the submenu.
            HBMMENU_POPUP_RESTORE = 9,          //Restore button for the submenu.
            HBMMENU_SYSTEM = 1,                 //Windows icon or the icon of the window specified in dwItemData.
        }

    }
}
