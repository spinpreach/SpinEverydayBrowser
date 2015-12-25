using System;
using Microsoft.Win32;

namespace Spinpreach.MonsterGirlsPlayer
{
    internal class RegistryHelper
    {
        /// <summary>
        /// https://msdn.microsoft.com/ja-jp/library/ee330730(v=vs.85).aspx
        /// </summary>
        internal enum BROWSER_VERSION
        {
            IE7 = 7000,
            IE8 = 8000,
            IE8StandardsMode = 8888,
            IE9 = 9000,
            IE9StandardsMode = 9999,
            IE10 = 10000,
            IE10StandardsMode = 10001
        }

        /// <summary>
        /// [ WebBrowser Control Rendering Mode ] を設定するメソッドです。
        /// </summary>
        internal static void SetBrowserEmulation(BROWSER_VERSION value)
        {

            string subkey = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION";
            string exename = AppDomain.CurrentDomain.FriendlyName;

            try
            {
                using (var key = Registry.CurrentUser.CreateSubKey(subkey))
                {
                    key.SetValue(exename, value, RegistryValueKind.DWord);
                    key.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error : Browser Emulation", ex);
            }

        }

    }
}