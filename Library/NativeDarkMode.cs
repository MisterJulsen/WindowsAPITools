using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAPITools
{
    public enum PreferredAppMode
    {
        Light = 3,
        Dark = 2,
        SystemCurrent = 1,
        Default = 0
    }

    public class NativeDarkMode
    {

        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);

        [DllImport("uxtheme.dll", EntryPoint = "#135")]
        private static extern bool SetPreferredAppMode(PreferredAppMode mode); // 1903 or later

        [DllImport("uxtheme.dll", EntryPoint = "#133")]
        private static extern bool AllowDarkModeForApp(bool allow); // 1803

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, string lParam);

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

        /// <summary>
        /// Whether the windows 10 native dark theme for the window border should be applied or not.
        /// IMPORTAND! YOU NEED TO HAVE A MANIFEST FILE WITH THE WINDOWS 10 SUPPORT ENTRY ENABLED!!!
        /// </summary>
        /// <param name="handle">The form's handle.</param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public static bool UseImmersiveDarkMode(IntPtr handle, bool enabled)
        {
            
            if (IsWindows10OrGreater(17763))
            {
                var attribute = DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1;
                if (IsWindows10OrGreater(18985))
                {
                    attribute = DWMWA_USE_IMMERSIVE_DARK_MODE;
                }
                int useImmersiveDarkMode = enabled ? 1 : 0;
                return DwmSetWindowAttribute(handle, (int)attribute, ref useImmersiveDarkMode, sizeof(int)) == 0;
            }
            Console.WriteLine("The OS does not support native dark mode.");
            return false;
        }

        /// <summary>
        /// Use the windows 10 native dark mode for MainMenu and ContextMenu control. Also the systemmenu and other context menus are supported. This method must be called before the first window is shown.
        /// IMPORTAND! YOU NEED TO HAVE A MANIFEST FILE WITH THE WINDOWS 10 SUPPORT ENTRY ENABLED!!!
        /// </summary>
        /// <returns></returns>
        public static bool EnableDarkMode()
        {
            if (IsWindows10OrGreater(17763))
            {
                if (IsWindows10OrGreater(18985))
                {
                    SetPreferredAppMode(PreferredAppMode.Dark);
                }
                else
                {
                    AllowDarkModeForApp(true);
                }
                return true;   
            }
            Console.WriteLine("The OS does not support native dark mode.");
            return false;
        }

        /// <summary>
        /// Set which preferred app mode should be used. Only for Windows 10 build 1903 or greater.
        /// IMPORTAND! YOU NEED TO HAVE A MANIFEST FILE WITH THE WINDOWS 10 SUPPORT ENTRY ENABLED!!!
        /// </summary>
        /// <param name="mode">The preferred app mode.</param>
        /// <returns>true, if sucessfull</returns>
        public static bool SetAppMode(PreferredAppMode mode)
        {
            if (IsWindows10OrGreater(17763))
            {
                if (IsWindows10OrGreater(18985))
                {
                    SetPreferredAppMode(mode);
                    return true;
                }
            }
            Console.WriteLine("The OS does not support these settings.");
            return false;
        }

        /// <summary>
        /// Disables the windows 10 native dark mode.
        /// IMPORTAND! YOU NEED TO HAVE A MANIFEST FILE WITH THE WINDOWS 10 SUPPORT ENTRY ENABLED!!!
        /// </summary>
        /// <returns></returns>
        public static bool DisableDarkMode()
        {
            if (IsWindows10OrGreater(17763))
            {
                if (IsWindows10OrGreater(18985))
                {
                    SetPreferredAppMode(PreferredAppMode.Default);
                }
                else
                {
                    AllowDarkModeForApp(false);
                }
                return true;
            }
            Console.WriteLine("The OS does not support native dark mode.");
            return false;
        }

        /// <summary>
        /// Checks if the dark mode is enabled on this system.
        /// IMPORTAND! YOU NEED TO HAVE A MANIFEST FILE WITH THE WINDOWS 10 SUPPORT ENTRY ENABLED!!!
        /// </summary>
        /// <returns></returns>
        public static bool IsSystemDarkModeEnabled()
        {
            return (int)Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", 1) == 0;
        }

        /// <summary>
        /// Checks if the installed system supports native dark mode.
        /// IMPORTAND! YOU NEED TO HAVE A MANIFEST FILE WITH THE WINDOWS 10 SUPPORT ENTRY ENABLED!!!
        /// </summary>
        /// <returns></returns>
        public static bool IsWin10DarkModeSupported()
        {
            return IsWindows10OrGreater(17763);
        }

        private static bool IsWindows10OrGreater(int build = -1)
        {
            return Environment.OSVersion.Version.Major >= 10 && Environment.OSVersion.Version.Build >= build;
        }

        /// <summary>
        /// Not every control is supported!
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="theme"></param>
        public static void SetControlTheme(IntPtr handle, ControlTheme theme)
        {
            SetWindowTheme(handle, TranslateTheme(theme), null);
        }

        protected static string TranslateTheme(ControlTheme theme)
        {
            switch (theme)
            {
                case ControlTheme.EXPLORER_DARK:
                    return "darkmode_explorer";
                case ControlTheme.EXPLORER_DEFAULT:
                    return "explorer";
                case ControlTheme.DARK_CFD:
                    return "darkmode_cfd";
                case ControlTheme.CFD:
                    return "cfd";
                case ControlTheme.NATIVE_LISTVIEW:
                    return "itemsview";
                case ControlTheme.NATIVE_LISTVIEW_DARK:
                    return "darkmode_itemsview";
                default:
                    return "";
            }
        }
    }

    public enum ControlTheme
    {
        EXPLORER_DEFAULT,
        EXPLORER_DARK,
        CFD,
        DARK_CFD,
        NATIVE_LISTVIEW,
        NATIVE_LISTVIEW_DARK

    }
}
