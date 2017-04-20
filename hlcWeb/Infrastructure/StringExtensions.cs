using System.Text.RegularExpressions;

namespace hlcWeb.Infrastructure
{
    public static class StringExtensions
    {
        public static string SplitCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";

            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );
        }
    }
}