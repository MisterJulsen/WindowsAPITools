using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAPITools
{
    public class SystemMenu
    {
        /// <summary>
        /// The IntPtr of the SystemMenu.
        /// </summary>
        public IntPtr Menu { get; }

        /// <summary>
        /// The Form which contains the system menu.
        /// </summary>
        public Form OwnerForm { get; }

        private List<SystemMenuItem> Items;

        /// <summary>
        /// Create a new object to work with the form's system menu. To check the user input, put <code>CallEvents</code> in WndProc of the form.
        /// </summary>
        /// <param name="form">he form from which the menu is to be obtained</param>
        public SystemMenu(Form form)
        {
            Items = new List<SystemMenuItem>();
            OwnerForm = form;
            Menu = SysMenuAPI.GetSystemMenu(form.Handle);

            ReloadItems();
        }

        /// <summary>
        /// Refreshes the list of items, when the system menu has been modified by other functions that are not in this class. CAUTION! ALL ITEMS WILL LOSE THEIR CLICK EVENT! 
        /// </summary>
        public void ReloadItems()
        {
            Items.Clear();
            for (int i = 0; i < SysMenuAPI.GetMenuItemCount(Menu); i++)
            {
                Items.Add(new SystemMenuItem(this, Menu, SysMenuAPI.GetMenuItemID(Menu, i)));
            }
        }

        /// <summary>
        /// The method which handles the user click input for all custom menu items. Put a method call in the WndProc of your form.
        /// </summary>
        /// <param name="m"></param>
        public void CallEvents(Message m)
        {
            switch (m.Msg)
            {
                case SysMenuAPI.Flags.WM_SYSCOMMAND:
                    foreach (SystemMenuItem item in Items)
                    {
                        if ((int)m.WParam == item.ID)
                        {
                            item.ItemClick();
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Returns a list with all ids used by menu items in the system menu and all sub menus.
        /// </summary>
        /// <returns></returns>
        public List<int> GetUsedIDs()
        {
            List<int> usedIDs = new List<int>();
            foreach (SystemMenuItem itm in Items)
            {
                usedIDs.AddRange(RecursiveIDGetter(itm));
            }
            return usedIDs;
        }

        /// <summary>
        /// Returns the system menu item at the end of the index path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public SystemMenuItem GetItemByIndexPath(int[] path)
        {
            if (path.Length == 0) return null;
            else if (path.Length == 1) return Items[path[0]];
            else return RecursivePath(Items[path[0]], path, 1);
        }

        private SystemMenuItem RecursivePath(SystemMenuItem parent, int[] path, int pathIndex)
        {
            if (path.Length == pathIndex + 1) return parent.GetSubItems()[path[pathIndex]];
            else return RecursivePath(parent.GetSubItems()[path[pathIndex]], path, pathIndex + 1);
        }

        private List<int> RecursiveIDGetter(SystemMenuItem item)
        {
            List<int> usedIds = new List<int>();
            usedIds.Add(item.ID);
            foreach (SystemMenuItem itm in item.GetSubItems())
            {
                usedIds.AddRange(RecursiveIDGetter(itm));
            }
            return usedIds;
        }

        /// <summary>
        /// Add an item to the system menu.
        /// </summary>
        /// <param name="type">The type of the item.</param>
        /// <param name="text">The text of the item.</param>
        /// <returns>The item itself.</returns>
        public SystemMenuItem AddItem(SystemMenuItemType type, string text = "")
        {
            List<int> usedIDs = GetUsedIDs();
            int id = 0;
            while (usedIDs.Contains(id))
            {
                id++;
            }
            SystemMenuItem item = new SystemMenuItem(this, Menu, id);
            SysMenuAPI.AppendMenu(Menu, type == SystemMenuItemType.STRING ? SysMenuAPI.MenuTypes.MF_STRING : SysMenuAPI.MenuTypes.MF_SEPARATOR, id, text);
            Items.Add(item);
            return item;
        }

        /// <summary>
        /// Add an item at a certain index to the system menu.
        /// </summary>
        /// <param name="type">The type of the item.</param>
        /// <param name="index">The index the item should be added.</param>
        /// <param name="text">The text of the item.</param>
        /// <returns>The item itself.</returns>
        public SystemMenuItem InsertItem(SystemMenuItemType type, int index, string text = "")
        {
            List<int> usedIDs = GetUsedIDs();
            int id = 0;
            while (usedIDs.Contains(id))
            {
                id++;
            }
            SystemMenuItem item = new SystemMenuItem(this, Menu, id);
            SysMenuAPI.InsertMenu(Menu, index, SysMenuAPI.Flags.MF_BYPOSITION | (type == SystemMenuItemType.STRING ? SysMenuAPI.MenuTypes.MF_STRING : SysMenuAPI.MenuTypes.MF_SEPARATOR), id, text);
            Items.Insert(index, item);
            return item;
        }

        /// <summary>
        /// Remove an item from the system menu.
        /// </summary>
        /// <param name="index">The index of the item which should be removed.</param>
        public void RemoveItem(int index)
        {
            SysMenuAPI.RemoveMenu(Menu, Items[index].ID, SysMenuAPI.Flags.MF_BYCOMMAND);
            if (Items[index].HasSubItems())
            {
                Items[index].ClearSubMenu();
            }
            Items.RemoveAt(index);
        }

        /// <summary>
        /// Get the item of an index.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <returns>the item.</returns>
        public SystemMenuItem GetItem(int index)
        {
            return Items[index];
        }

        /// <summary>
        /// Get the count of items in the system menu.
        /// </summary>
        /// <returns>The item count.</returns>
        public int ItemCount()
        {
            return Items.Count;
        }

        /// <summary>
        /// Removes all elements (including menu items added by default) from the menu.
        /// </summary>
        public void ClearMenu()
        {
            for (int i = Items.Count - 1; i >= 0; i--)
            {
                SysMenuAPI.RemoveMenu(Menu, Items[i].ID, SysMenuAPI.Flags.MF_BYCOMMAND);
                Items.RemoveAt(i);
            }
        }




        /**
         *  PRESETS
         */
        public void UsePreset_ExtendedMenu(Action mainMenu = null, string mainmenu_Text = "Menu",
                                           Action help = null, string help_Text = "Help",
                                           Action topMost = null, string topMost_Text = "Window always on top",
                                           Action fullscreen = null, string fullscreen_Text = "Toggle fullscreen",
                                           Action info = null, string info_Text = "About",
                                           Action commandMenu = null, string commandMenu_Text = "Tools")
        {
            if (mainMenu != null)
            {
                SystemMenuItem item = InsertItem(SystemMenuItemType.STRING, 0, mainmenu_Text);
                item.SetSystemIcon(SysMenuAPI.MenuItemInfo_hItem.HBMMENU_SYSTEM);
                item.Click += (s1, e1) =>
                {
                    mainMenu();
                };
                InsertItem(SystemMenuItemType.SEPARATOR, 1);
            }

            if (help != null || topMost != null || fullscreen != null || info != null || commandMenu != null)
            {
                InsertItem(SystemMenuItemType.SEPARATOR, ItemCount() - 2);
            }

            if (help != null)
            {
                SystemMenuItem item = InsertItem(SystemMenuItemType.STRING, ItemCount() - 2, help_Text);
                item.Click += (s1, e1) =>
                {
                    help();
                };
            }

            if (info != null)
            {
                SystemMenuItem item = InsertItem(SystemMenuItemType.STRING, ItemCount() - 2, info_Text);
                item.Click += (s1, e1) =>
                {
                    info();
                };
            }

            if (topMost != null)
            {
                SystemMenuItem item = InsertItem(SystemMenuItemType.STRING, ItemCount() - 2, topMost_Text);
                item.Click += (s1, e1) =>
                {
                    topMost();
                };
            }

            if (fullscreen != null)
            {
                SystemMenuItem item = InsertItem(SystemMenuItemType.STRING, ItemCount() - 2, fullscreen_Text);
                item.Click += (s1, e1) =>
                {
                    fullscreen();
                };
            }

            if (commandMenu != null)
            {
                SystemMenuItem item = InsertItem(SystemMenuItemType.STRING, ItemCount() - 2, commandMenu_Text);
                item.Click += (s1, e1) =>
                {
                    commandMenu();
                };
            }
        }

        public void UsePreset_MoveCloseOnly()
        {
            RemoveItem(5);
            RemoveItem(4);
            RemoveItem(3);
            RemoveItem(2);
            RemoveItem(0);
        }

        public void UsePreset_MoveResizeCloseOnly()
        {
            RemoveItem(4);
            RemoveItem(3);
            RemoveItem(0);
        }

        public void UsePreset_NoMin()
        {
            RemoveItem(3);
        }

        public void UsePreset_NotResizable()
        {
            RemoveItem(4);
            RemoveItem(2);
        }

        public void UsePreset_NoClose()
        {
            RemoveItem(6);
            RemoveItem(5);
        }

        public void UsePreset_MoveOnly()
        {
            RemoveItem(6);
            RemoveItem(5);
            RemoveItem(4);
            RemoveItem(3);
            RemoveItem(2);
            RemoveItem(0);
        }

        public void DisableCloseButton(int closeButtonIndex)
        {
            Items[closeButtonIndex].Enabled = false;
        }

        public void Item_Click(SystemMenuItem item)
        {
            MessageBox.Show(item.ToString());
        }
    }


    public enum SystemMenuItemType
    {
        STRING,
        SEPARATOR
    }
}
