using BOMobile2.Services;
using BOMobile2.Services.Schema;
using BOMobile2.Util;
using Java.Util;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOMobile2
{
    public class Global
    {
        public static bool Letbit = true;
        public static bool IsTest = true;

        public static MobileDataService DataService;

        public static MemberLoginInfo MemberInfo = new MemberLoginInfo { Id = -1, Language = "tr" };

        public static string Language = "tr";

        public static Dictionary<int, string> Translates;

        public static IFtpProvider Ftp;
        public static IStringEncyrption Encrypt;

        public static string GetLanguage()
        {
            return CrossSettings.Current.GetValueOrDefault("UserLanguage", Locale.Default.Language.ToLower());
        }
    }
}
