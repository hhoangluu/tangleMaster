using UnityEngine;

namespace Five.String
{
    public static class StripVietnameseAccentExtensions
    {
        //Matrix, latin word in first line, replace following line by return to index of first line
        private static readonly string[] VietnameseSigns = new string[]
        {
        "aAeEoOuUiIdDyY",
        "áàạảãâấầậẩẫăắằặẳẵ",
        "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
        "éèẹẻẽêếềệểễ",
        "ÉÈẸẺẼÊẾỀỆỂỄ",
        "óòọỏõôốồộổỗơớờợởỡ",
        "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
        "úùụủũưứừựửữ",
        "ÚÙỤỦŨƯỨỪỰỬỮ",
        "íìịỉĩ",
        "ÍÌỊỈĨ",
        "đ",
        "Đ",
        "ýỳỵỷỹ",
        "ÝỲỴỶỸ"
        };

        /// <summary>
        /// Remove all accent in Vietnamese and convert to normal text
        /// </summary>
        public static string StripVietnameseAccent(this string str)
        {
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return str;
        }
    }

    public static class StringColor
    {
        public static string Color(this string stringInput, Color color)
        {
            byte maxBytes = 255;
            string hex = (color.r * maxBytes).RoundToInt().ToHex() + (color.g * maxBytes).RoundToInt().ToHex() + (color.b * maxBytes).RoundToInt().ToHex() + (color.a * maxBytes).RoundToInt().ToHex();
            return string.Format("<color=#{1}>{0}</color>", stringInput, hex);
        }

        public static string Color(this string stringInput, Color32 color)
        {
            string hex = color.r.ToInt().ToHex() + color.g.ToInt().ToHex() + color.b.ToInt().ToHex() + color.a.ToInt().ToHex();
            return string.Format("<color=#{1}>{0}</color>", stringInput, hex);
        }

        private static string GetHexString(this int stringInput, bool doubleZero = false)
        {
            if (doubleZero && stringInput == 0)
                return "00";
            var hexChar = "abcdef";
            return stringInput <= 9 ? "0" + stringInput.ToString() : hexChar[(hexChar.Length - 1) - (15 - stringInput)].ToString();
        }

        private static string ToHex(this int @this, string result = "")
        {
            if (@this < 16)
                return result.InsertBefore(@this.GetHexString(true));

            var odd = (@this % 16f).RoundToInt();
            var newValue = ((@this - odd) / 16f).RoundToInt();
            return ToHex(newValue, result.InsertBefore(odd.GetHexString()));
        }

        private static int RoundToInt(this float @this) { return Mathf.RoundToInt(@this); }

        private static int ToInt(this byte @this) { return @this; }

        private static string InsertBefore(this string @this, string insertor) { return insertor + @this; }
    }
}
