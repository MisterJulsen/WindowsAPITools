using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAPITools
{
    public class SystemMenuItem
    {

        public delegate void ItemClickEventHandler (object sender, SystemItemEventArgs e);
        public event ItemClickEventHandler Click;
        private List<SystemMenuItem> SubItems;

        /// <summary>
        /// A handle to the menu bar, drop-down menu, submenu, or shortcut menu.
        /// </summary>
        public IntPtr HMenu { get; }
        /// <summary>
        /// The system menu object, which contains this item.
        /// </summary>
        public SystemMenu SysMenu { get; }
        /// <summary>
        /// The id of the menu item. Cannot be changed.
        /// </summary>
        public int ID { get; }

        /// <summary>
        /// Wether the item is checked or not.
        /// </summary>
        public bool Checked
        {
            get
            {
                return SysMenuAPI.GetMenuState(HMenu, ID, SysMenuAPI.Flags.MF_BYCOMMAND) == SysMenuAPI.Flags.MF_CHECKED;
            }
            set
            {
                SysMenuAPI.CheckMenuItem(HMenu, ID, SysMenuAPI.Flags.MF_BYCOMMAND | (value ? SysMenuAPI.Flags.MF_CHECKED : SysMenuAPI.Flags.MF_UNCHECKED));
            }
        }

        /// <summary>
        /// Wether the item is enabled or not.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return SysMenuAPI.GetMenuState(HMenu, ID, SysMenuAPI.Flags.MF_BYCOMMAND) == SysMenuAPI.Flags.MF_ENABLED;
            }
            set
            {
                SysMenuAPI.EnableMenuItem(HMenu, ID, value ? SysMenuAPI.Flags.MF_ENABLED : SysMenuAPI.Flags.MF_DISABLED);
            }
        }
                
        /// <summary>
        /// Set or get the text of the menu item.
        /// </summary>
        public string Text
        {
            get
            {
                string caption = "";
                MENUITEMINFO mif = new MENUITEMINFO();
                mif.fMask = SysMenuAPI.FMask.MIIM_STRING;
                mif.fType = SysMenuAPI.FType.MFT_STRING;

                mif.dwTypeData = IntPtr.Zero;
                bool res = SysMenuAPI.GetMenuItemInfo(HMenu, ID, false, mif);
                if (!res)
                    return "";
                mif.cch++;
                mif.dwTypeData = Marshal.AllocHGlobal((IntPtr)(mif.cch * 2));
                try
                {
                    res = SysMenuAPI.GetMenuItemInfo(HMenu, ID, false, mif);
                    if (!res)
                        return "";
                    caption = Marshal.PtrToStringUni(mif.dwTypeData);
                }
                finally
                {
                    Marshal.FreeHGlobal(mif.dwTypeData);
                }
                return caption;
            }
            set
            {
                MenuItemInfo mInfo = new MenuItemInfo()
                {
                    cbSize = Marshal.SizeOf(typeof(MenuItemInfo)),
                    fMask = SysMenuAPI.FMask.MIIM_STRING,
                    fType = SysMenuAPI.FType.MFT_STRING,
                    fState = SysMenuAPI.FState.MFS_DISABLED,                    
                    wID = ID,
                    hbmpChecked = IntPtr.Zero,
                    hbmpUnchecked = IntPtr.Zero,
                    dwTypeData = Marshal.StringToHGlobalAuto(value),
                    dwItemData = IntPtr.Zero,
                    hSubMenu = IntPtr.Zero,
                    cch = Text.Length,
                };
                SysMenuAPI.SetMenuItemInfo(HMenu, ID, false, ref mInfo);
            }
        }

        /// <summary>
        /// Get the hMenu for the sub menu or set the hMenu for a new menu. To create a submenu please use the methods.
        /// </summary>
        public IntPtr SubMenu
        {
            get
            {
                IntPtr submenu;
                MENUITEMINFO mif = new MENUITEMINFO();
                mif.fMask = SysMenuAPI.FMask.MIIM_SUBMENU;
                mif.fType = SysMenuAPI.FType.MFT_MENUBREAK;

                mif.hSubMenu = IntPtr.Zero;
                bool res = SysMenuAPI.GetMenuItemInfo(HMenu, ID, false, mif);
                if (!res)
                    throw new Win32Exception();
                mif.cch++;
                mif.hSubMenu = Marshal.AllocHGlobal((IntPtr)(mif.cch * 2));
                try
                {
                    res = SysMenuAPI.GetMenuItemInfo(HMenu, ID, false, mif);
                    if (!res)
                        throw new Win32Exception();
                    submenu = mif.hSubMenu;
                }
                finally
                {
                    //Marshal.FreeHGlobal(mif.hSubMenu);
                }
                return submenu;
            }
            set
            {
                MenuItemInfo mInfo = new MenuItemInfo()
                {
                    cbSize = Marshal.SizeOf(typeof(MenuItemInfo)),
                    fMask = SysMenuAPI.FMask.MIIM_SUBMENU,
                    fState = SysMenuAPI.FState.MFS_ENABLED,
                    wID = ID,
                    hbmpChecked = IntPtr.Zero,
                    hbmpUnchecked = IntPtr.Zero,
                    dwTypeData = IntPtr.Zero,
                    dwItemData = IntPtr.Zero,
                    hSubMenu = value,
                    cch = Text.Length,
                };
                SysMenuAPI.SetMenuItemInfo(HMenu, ID, false, ref mInfo);
            }
        }

        /// <summary>
        /// Set or get the icn of the menu icon displayed next to the text.
        /// </summary>
        public Bitmap ItemIcon
        {
            get
            {
                Bitmap bmp = new Bitmap(1, 1);
                MENUITEMINFO mif = new MENUITEMINFO();
                mif.fMask = SysMenuAPI.FMask.MIIM_BITMAP;
                mif.fType = SysMenuAPI.FType.MFT_BITMAP;

                mif.hbmpItem = IntPtr.Zero;
                bool res = SysMenuAPI.GetMenuItemInfo(HMenu, ID, false, mif);
                if (!res)
                    throw new Win32Exception();
                mif.cch++;
                mif.hbmpItem = Marshal.AllocHGlobal((IntPtr)(mif.cch * 2));
                try
                {
                    res = SysMenuAPI.GetMenuItemInfo(HMenu, ID, false, mif);
                    if (!res)
                        throw new Win32Exception();
                    bmp = Image.FromHbitmap(mif.hbmpItem);
                }
                finally
                {
                    Marshal.FreeHGlobal(mif.dwTypeData);
                }
                return bmp;
            }
            set
            {
                IntPtr bmp = value.GetHbitmap();
                Size s = new Size(value.Width, value.Height);
                using (Bitmap renderBmp = new Bitmap(s.Width, s.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb))
                {
                    using (Graphics g = Graphics.FromImage(renderBmp))
                    {
                        g.DrawImage(value, 0, 0, s.Width, s.Height);
                    }
                    bmp = renderBmp.GetHbitmap(Color.FromArgb(0, 0, 0, 0));
                }
                MenuItemInfo mInfo = new MenuItemInfo()
                {
                    cbSize = Marshal.SizeOf(typeof(MenuItemInfo)),
                    fMask = SysMenuAPI.FMask.MIIM_STRING | SysMenuAPI.FMask.MIIM_BITMAP | SysMenuAPI.FMask.MIIM_FTYPE | SysMenuAPI.FMask.MIIM_STATE | SysMenuAPI.FMask.MIIM_ID,
                    fType = SysMenuAPI.FType.MFT_STRING,
                    fState = SysMenuAPI.FState.MFS_ENABLED,
                    wID = ID,
                    hbmpItem = bmp,
                    hbmpChecked = IntPtr.Zero,
                    hbmpUnchecked = IntPtr.Zero,
                    dwTypeData = Marshal.StringToHGlobalAuto(Text),
                    dwItemData = IntPtr.Zero,
                    hSubMenu = IntPtr.Zero,
                    cch = Text.Length,
                };
                SysMenuAPI.SetMenuItemInfo(HMenu, ID, false, ref mInfo);
            }
        }

        /// <summary>
        /// Set or get the icn of the menu icon displayed next to the text, if it is not checked.
        /// </summary>
        public Bitmap UncheckedIcon
        {
            get
            {
                Bitmap bmp = new Bitmap(1, 1);
                MENUITEMINFO mif = new MENUITEMINFO();
                mif.fMask = SysMenuAPI.FMask.MIIM_BITMAP;
                mif.fType = SysMenuAPI.FType.MFT_BITMAP;

                mif.hbmpItem = IntPtr.Zero;
                bool res = SysMenuAPI.GetMenuItemInfo(HMenu, ID, false, mif);
                if (!res)
                    throw new Win32Exception();
                mif.cch++;
                mif.hbmpItem = Marshal.AllocHGlobal((IntPtr)(mif.cch * 2));
                try
                {
                    res = SysMenuAPI.GetMenuItemInfo(HMenu, ID, false, mif);
                    if (!res)
                        throw new Win32Exception();
                    bmp = Image.FromHbitmap(mif.hbmpItem);
                }
                finally
                {
                    Marshal.FreeHGlobal(mif.dwTypeData);
                }
                return bmp;
            }
            set
            {
                IntPtr bmp = value.GetHbitmap();
                Size s = new Size(value.Width, value.Height);
                using (Bitmap renderBmp = new Bitmap(s.Width, s.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb))
                {
                    using (Graphics g = Graphics.FromImage(renderBmp))
                    {
                        g.DrawImage(value, 0, 0, s.Width, s.Height);
                    }
                    bmp = renderBmp.GetHbitmap(Color.FromArgb(0, 0, 0, 0));
                }
                MenuItemInfo mInfo = new MenuItemInfo()
                {
                    cbSize = Marshal.SizeOf(typeof(MenuItemInfo)),
                    fMask = SysMenuAPI.FMask.MIIM_STRING | SysMenuAPI.FMask.MIIM_BITMAP | SysMenuAPI.FMask.MIIM_FTYPE | SysMenuAPI.FMask.MIIM_STATE | SysMenuAPI.FMask.MIIM_ID,
                    fType = SysMenuAPI.FType.MFT_STRING,
                    fState = SysMenuAPI.FState.MFS_ENABLED,
                    wID = ID,
                    hbmpItem = bmp,
                    hbmpChecked = IntPtr.Zero,
                    hbmpUnchecked = IntPtr.Zero,
                    dwTypeData = Marshal.StringToHGlobalAuto(Text),
                    dwItemData = IntPtr.Zero,
                    hSubMenu = IntPtr.Zero,
                    cch = Text.Length,
                };
                SysMenuAPI.SetMenuItemInfo(HMenu, ID, false, ref mInfo);
            }
        }

        /// <summary>
        /// Set or get the icn of the menu icon displayed next to the text, if it is checked.
        /// </summary>
        public Bitmap CheckedIcon
        {
            get
            {
                Bitmap bmp = new Bitmap(1, 1);
                MENUITEMINFO mif = new MENUITEMINFO();
                mif.fMask = SysMenuAPI.FMask.MIIM_BITMAP;
                mif.fType = SysMenuAPI.FType.MFT_BITMAP;

                mif.hbmpItem = IntPtr.Zero;
                bool res = SysMenuAPI.GetMenuItemInfo(HMenu, ID, false, mif);
                if (!res)
                    throw new Win32Exception();
                mif.cch++;
                mif.hbmpItem = Marshal.AllocHGlobal((IntPtr)(mif.cch * 2));
                try
                {
                    res = SysMenuAPI.GetMenuItemInfo(HMenu, ID, false, mif);
                    if (!res)
                        throw new Win32Exception();
                    bmp = Image.FromHbitmap(mif.hbmpItem);
                }
                finally
                {
                    Marshal.FreeHGlobal(mif.dwTypeData);
                }
                return bmp;
            }
            set
            {
                IntPtr bmp = value.GetHbitmap();
                Size s = new Size(value.Width, value.Height);
                using (Bitmap renderBmp = new Bitmap(s.Width, s.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb))
                {
                    using (Graphics g = Graphics.FromImage(renderBmp))
                    {
                        g.DrawImage(value, 0, 0, s.Width, s.Height);
                    }
                    bmp = renderBmp.GetHbitmap(Color.FromArgb(0, 0, 0, 0));
                }
                MenuItemInfo mInfo = new MenuItemInfo()
                {
                    cbSize = Marshal.SizeOf(typeof(MenuItemInfo)),
                    fMask = SysMenuAPI.FMask.MIIM_STRING | SysMenuAPI.FMask.MIIM_BITMAP | SysMenuAPI.FMask.MIIM_FTYPE | SysMenuAPI.FMask.MIIM_STATE | SysMenuAPI.FMask.MIIM_ID,
                    fType = SysMenuAPI.FType.MFT_STRING,
                    fState = SysMenuAPI.FState.MFS_ENABLED,
                    wID = ID,
                    hbmpItem = bmp,
                    hbmpChecked = IntPtr.Zero,
                    hbmpUnchecked = IntPtr.Zero,
                    dwTypeData = Marshal.StringToHGlobalAuto(Text),
                    dwItemData = IntPtr.Zero,
                    hSubMenu = IntPtr.Zero,
                    cch = Text.Length,
                };
                SysMenuAPI.SetMenuItemInfo(HMenu, ID, false, ref mInfo);
            }
        }


        /// <summary>
        /// Create a new instance of an SystemMenuItem. The constructor won't create a real menu item!
        /// </summary>
        /// <param name="menu">The system menu object which contains all items ans sub menus.</param>
        /// <param name="hMenu">The menu the entry should be assigned to.</param>
        /// <param name="id">The id of the item. Should not be used by another object.</param>
        public SystemMenuItem(SystemMenu menu, IntPtr hMenu, int id)
        {
            HMenu = hMenu;
            SysMenu = menu;
            ID = id;
            SubItems = new List<SystemMenuItem>();
        }

        /// <summary>
        /// Add a item to a sub menu of this item. This method will create a submenu if there is no one present.
        /// </summary>
        /// <param name="type">The type of the item.</param>
        /// <param name="text">The text of the item.</param>
        /// <returns>The item itself.</returns>
        public SystemMenuItem AddSubItem(SystemMenuItemType type, string text = "")
        {
            List<int> usedIDs = SysMenu.GetUsedIDs();
            int id = 0;
            while (usedIDs.Contains(id))
            {
                id++;
            }

            IntPtr p = SubMenu;
            if (p == IntPtr.Zero)
            {
                p = SysMenuAPI.CreateMenu();
                SubMenu = p;
            }

            SystemMenuItem itm = new SystemMenuItem(SysMenu, p, id);
            SubItems.Add(itm);
            SysMenuAPI.AppendMenu(p, type == SystemMenuItemType.STRING ? SysMenuAPI.MenuTypes.MF_STRING : SysMenuAPI.MenuTypes.MF_SEPARATOR, id, text);
            return itm;
        }

        /// <summary>
        /// Add a item at a certain position to a sub menu of this item. This method will create a submenu if there is no one present.
        /// </summary>        
        /// <param name="index">The index in the menu list the item should be added.</param>
        /// <param name="type">The type of the item.</param>
        /// <param name="text">The text of the item.</param>
        /// <returns>The item itself.</returns>
        public SystemMenuItem InsertSubItem(int index, SystemMenuItemType type, string text = "")
        {
            List<int> usedIDs = SysMenu.GetUsedIDs();
            int id = 0;
            while (usedIDs.Contains(id))
            {
                id++;
            }

            IntPtr p = SubMenu;
            if (p == IntPtr.Zero)
            {
                p = SysMenuAPI.CreateMenu();
                SubMenu = p;
            }

            SystemMenuItem itm = new SystemMenuItem(SysMenu, p, id);
            SubItems.Insert(index, itm);
            SysMenuAPI.InsertMenu(p, index, SysMenuAPI.Flags.MF_BYPOSITION | (type == SystemMenuItemType.STRING ? SysMenuAPI.MenuTypes.MF_STRING : SysMenuAPI.MenuTypes.MF_SEPARATOR), id, text);
            return itm;
        }

        /// <summary>
        /// Remove an item from the sub menu.
        /// </summary>
        /// <param name="index">The index of the item which should be removed.</param>
        public void RemoveSubItem(int index)
        {
            SystemMenuItem itm = SubItems[index];
            SysMenuAPI.RemoveMenu(itm.HMenu, itm.ID, SysMenuAPI.Flags.MF_BYCOMMAND);
            if (itm.HasSubItems())
            {
                itm.ClearSubMenu();
            }
            SubItems.RemoveAt(index);
            if (SubItems.Count == 0)
            {
                SubMenu = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Removes all sub items.
        /// </summary>
        public void ClearSubMenu()
        {
            for (int i = SubItems.Count - 1; i >= 0; i--)
            {
                if (SubItems[i].HasSubItems())
                {
                    SubItems[i].ClearSubMenu();
                }
                SysMenuAPI.RemoveMenu(SubItems[i].HMenu, SubItems[i].ID, SysMenuAPI.Flags.MF_BYCOMMAND);
                SubItems.RemoveAt(i);
            }
            if (SubItems.Count == 0)
            {
                SubMenu = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Returns a list with all sub items.
        /// </summary>
        /// <returns>List with all sub items.</returns>
        public List<SystemMenuItem> GetSubItems()
        {
            return SubItems;
        }

        /// <summary>
        /// Check if there are subitems.
        /// </summary>
        /// <returns>true, if there are sub items, false otherwise.</returns>
        public bool HasSubItems()
        {
            return SubItems.Count > 0;
        }

        /// <summary>
        /// Count the number of sub items.
        /// </summary>
        /// <returns>The count of sub items.</returns>
        public int GetSubItemsCount()
        {
            return SubItems.Count;
        }

        /// <summary>
        /// Set an system icon as default bitmap icon.
        /// </summary>
        /// <param name="icon">The icon type.</param>
        public void SetSystemIcon(SysMenuAPI.MenuItemInfo_hItem icon)
        {
            MenuItemInfo mInfo = new MenuItemInfo()
            {
                cbSize = Marshal.SizeOf(typeof(MenuItemInfo)),
                fMask = SysMenuAPI.FMask.MIIM_STRING | SysMenuAPI.FMask.MIIM_BITMAP | SysMenuAPI.FMask.MIIM_FTYPE | SysMenuAPI.FMask.MIIM_STATE | SysMenuAPI.FMask.MIIM_ID,
                fType = SysMenuAPI.FType.MFT_STRING,
                fState = SysMenuAPI.FState.MFS_ENABLED,
                wID = ID,
                hbmpItem = (IntPtr)icon,
                hbmpChecked = IntPtr.Zero,
                hbmpUnchecked = IntPtr.Zero,
                dwTypeData = Marshal.StringToHGlobalAuto(Text),
                dwItemData = IntPtr.Zero,
                hSubMenu = IntPtr.Zero,
                cch = Text.Length,
            };
            SysMenuAPI.SetMenuItemInfo(HMenu, ID, false, ref mInfo);
        }

        internal void ItemClick()
        {
            Click?.Invoke(this, new SystemItemEventArgs(this));
        }

        public override string ToString()
        {
            return Text;
        }
    }
    /*
     typedef struct tagMENUITEMINFOA {
  UINT      cbSize;
  UINT      fMask;
  UINT      fType;
  UINT      fState;
  UINT      wID;
  HMENU     hSubMenu;
  HBITMAP   hbmpChecked;
  HBITMAP   hbmpUnchecked;
  ULONG_PTR dwItemData;
  LPSTR     dwTypeData;
  UINT      cch;
  HBITMAP   hbmpItem;
} MENUITEMINFOA, *LPMENUITEMINFOA;
     */
    [StructLayout(LayoutKind.Sequential)]
    public class MENUITEMINFO
    {
        public int cbSize;
        public uint fMask;
        public uint fType;
        public uint fState;
        public uint wID;
        public IntPtr hSubMenu;
        public IntPtr hbmpChecked;
        public IntPtr hbmpUnchecked;
        public IntPtr dwItemData;
        public IntPtr dwTypeData;
        public uint cch;
        public IntPtr hbmpItem;

        public MENUITEMINFO()
        {
            cbSize = Marshal.SizeOf(typeof(MENUITEMINFO));
        }
    }

    public struct MenuItemInfo
    {
        public int cbSize;                 //The size of the structure, in bytes. The caller must set this member to sizeof(MENUITEMINFO). 
        public int fMask;    //See MenuItemInfo_fMask
        public int fType;    //See MenuItemInfo_fType
        public int fState;  //See MenuItemInfo_fState
        public int wID;                    //An application-defined value that identifies the menu item. Set fMask to MIIM_ID to use wID.
        public IntPtr hSubMenu;             //A handle to the drop-down menu or submenu associated with the menu item. If the menu item is not an item that opens a drop-down menu or submenu, this member is NULL. Set fMask to MIIM_SUBMENU to use hSubMenu.
        public IntPtr hbmpChecked;          //A handle  to the bitmap to display next to the item if it is selected. If this member is NULL, a default bitmap is used. If the MFT_RADIOCHECK type value is specified, the default bitmap is a bullet. Otherwise, it is a check mark. Set fMask to MIIM_CHECKMARKS to use hbmpChecked.
        public IntPtr hbmpUnchecked;        //A handle to the bitmap to display next to the item if it is not selected. If this member is NULL, no bitmap is used. Set fMask to MIIM_CHECKMARKS to use hbmpUnchecked. 
        public IntPtr dwItemData;           //An application-defined value associated with the menu item. Set fMask to MIIM_DATA to use dwItemData.
        public IntPtr dwTypeData;           //The contents of the menu item. The meaning of this member depends on the value of fType and is used only if the MIIM_TYPE flag is set in the fMask member.
                                            //To retrieve a menu item of type MFT_STRING, first find the size of the string by setting the dwTypeData member of MENUITEMINFO to NULL and then calling GetMenuItemInfo. The value of cch+1 is the size needed. Then allocate a buffer of this size, place the pointer to the buffer in dwTypeData, increment cch, and call GetMenuItemInfo once again to fill the buffer with the string. If the retrieved menu item is of some other type, then GetMenuItemInfo sets the dwTypeData member to a value whose type is specified by the fType member.
                                            //When using with the SetMenuItemInfo function, this member should contain a value whose type is specified by the fType member.
                                            //dwTypeData is used only if the MIIM_STRING flag is set in the fMask member 
        public int cch;                    //The length of the menu item text, in characters, when information is received about a menu item of the MFT_STRING type. However, cch is used only if the MIIM_TYPE flag is set in the fMask member and is zero otherwise. Also, cch is ignored when the content of a menu item is set by calling SetMenuItemInfo.
                                            //Note that, before calling GetMenuItemInfo, the application must set cch to the length of the buffer pointed to by the dwTypeData member. If the retrieved menu item is of type MFT_STRING (as indicated by the fType member), then GetMenuItemInfo changes cch to the length of the menu item text. If the retrieved menu item is of some other type, GetMenuItemInfo sets the cch field to zero.
                                            //The cch member is used when the MIIM_STRING flag is set in the fMask member.

        public IntPtr hbmpItem;             //A handle to the bitmap to be displayed, or it can be one of the values MenuItemInfo_hItem Enum. It is used when the MIIM_BITMAP flag is set in the fMask member. 
    }

    public class SystemItemEventArgs : EventArgs
    {
        /// <summary>
        /// The item that was clicked on.
        /// </summary>
        public SystemMenuItem Item { get; set; }

        public SystemItemEventArgs(SystemMenuItem item)
        {
            Item = item;
        }
    }
    
}
