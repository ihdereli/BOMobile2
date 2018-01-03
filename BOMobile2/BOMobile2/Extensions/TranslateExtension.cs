using BOMobile2;
using BOMobile2.Services;
using BOMobile2.Services.Schema;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Extensions
{
    [ContentProperty("Key")]
    public class TranslateExtension : IMarkupExtension
    {
        public int Key { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Global.Translates == null) return "";

            string val = "";

            if (Global.Translates.TryGetValue(Key, out val))
            {
                return val;
            }

            return Key;
        }

        public static string Translate(int key)
        {
            if (Global.Translates == null) return "";

            string val = "";

            if (Global.Translates.TryGetValue(key, out val))
            {
                return val;
            }

            return key.ToString();
        }
    }
}
