using System.Linq;

namespace NTUBAdministrator.Utils
{
    public class StringUtility
    {
        public static string[] SplitAndTrim(string text, char separator)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }
            return text.Split(separator).Select(t => t.Trim()).ToArray();
        }

        public static string[] SplitAndTrim(string text, char[] separator)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }
            return text.Split(separator).Select(t => t.Trim()).ToArray();
        }
    }
}