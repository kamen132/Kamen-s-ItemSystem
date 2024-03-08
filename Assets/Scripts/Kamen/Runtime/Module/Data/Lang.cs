using System;

namespace KamenGameFramewrok
{
    public class Lang
    {
        public static LocalLanguage Language { get; private set; }

        public static void SetLanguage(LocalLanguage newLang)
        {
            Language = newLang;
        }

        public static string GetString(string key)
        {
            var table = KamenGame.Instance.Config.TbLang.Get(key);
            string res = "";
            switch (Language)
            {
                case LocalLanguage.ChineseCn:
                    res = table.CnN;
                    break;
                case LocalLanguage.English:
                    res = table.En;
                    break;
            }

            return res;
        }

        public static string Format(string format, params object[] paramArray)
        {
            try
            {
                return string.Format(format, paramArray);
            }
            catch (Exception ex)
            {
                
            }

            return format;
        }
    }

    public enum LocalLanguage
    {
        ChineseCn,
        English,
    }
}