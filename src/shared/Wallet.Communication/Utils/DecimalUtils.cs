using System.Globalization;

namespace Wallet.Communication.Utils
{
    public static class DecimalUtils
    {
        public static decimal DecimalPrecision(this decimal value)
        {
            return Math.Round(value, 2);
        }
        public static decimal StringToDecimalCurrency(this string value)
        {
            return decimal.Parse(value, CultureInfo.InvariantCulture);
        }

        public static string DecimalToStringCurrency(this decimal value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
