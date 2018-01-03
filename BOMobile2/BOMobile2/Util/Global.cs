using BOMobile2.Services;
using BOMobile2.Services.Schema;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOMobile2
{
    public class Global
    {
        public static MobileDataService DataService;

        public static MemberLoginInfo MemberInfo = new MemberLoginInfo { Id = -1, Language = "tr" };

        public static string Language = "tr";

        public static Dictionary<int, string> Translates;

        public static Util.IFtpProvider Ftp;

        public static string GetLanguage()
        {
            return Locale.Default.Language.ToLower();
        }
    }
}
