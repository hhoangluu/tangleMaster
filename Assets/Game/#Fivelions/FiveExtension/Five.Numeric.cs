using System.Globalization;

namespace Five.Numeric
{
    public static class NumbersFormat
    {
        public static string ToKMB(this decimal num, CultureInfo culture = null)
        {
            if (num > 999999999 || num < -999999999)
                return num.ToString("0,,,.###B", culture ?? CultureInfo.InvariantCulture);
            if (num > 999999 || num < -999999)
                return num.ToString("0,,.##M", culture ?? CultureInfo.InvariantCulture);
            if (num > 999 || num < -999)
                return num.ToString("0,.#K", culture ?? CultureInfo.InvariantCulture);
            return num.ToString(culture ?? CultureInfo.InvariantCulture);
        }
        public static string ToKMB(this double num, CultureInfo culture = null)
        {
            if (num > 999999999 || num < -999999999)
                return num.ToString("0,,,.###B", culture ?? CultureInfo.InvariantCulture);
            if (num > 999999 || num < -999999)
                return num.ToString("0,,.##M", culture ?? CultureInfo.InvariantCulture);
            if (num > 999 || num < -999)
                return num.ToString("0,.#K", culture ?? CultureInfo.InvariantCulture);
            return num.ToString(culture ?? CultureInfo.InvariantCulture);
        }
        //123.456.789 - 1,5/1,6
        public static string NumberFormatVN(this decimal num) { return num < 10m ? num.ToString() : num.ToString("0,0", CultureInfo.CreateSpecificCulture("el-GR")); }
        public static string NumberFormatVN(this double num) { return num < 10d ? num.ToString() : num.ToString("0,0", CultureInfo.CreateSpecificCulture("el-GR")); }

        public static string NumberAutoKMBFormatVN(this decimal num, int length)
        {
            return num.ToString().Length > length ? num.ToKMB(CultureInfo.CreateSpecificCulture("el-GR")) : num.NumberFormatVN();
        }

        public static string NumberAutoKMBFormatVN(this double num, int length)
        {
            return num.ToString().Length > length ? num.ToKMB(CultureInfo.CreateSpecificCulture("el-GR")) : num.NumberFormatVN();
        }
    }
}