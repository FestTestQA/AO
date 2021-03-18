using System;
using System.IO;
using System.Linq;

namespace AO.AutomationFramework.Core.BusinessLogic.Helpers
{
    public class StringHelper
    {
        private static readonly Random random = new Random();

        internal static string GenerateCSSByAttributeContains(string tag, string attribute, string attributeValueContains)
        {
            return $"{tag}[{attribute}*='{attributeValueContains}']";
        }

        internal static string GenerateCSSByAttributeEndsWith(string tag, string attribute, string attributeValueEnds)
        {
            return $"{tag}[{attribute}$='{attributeValueEnds}']";
        }

        internal static string GenerateCCSByAttributeStartsWith(string tag, string attribute, string attributeValueStart)
        {
            return $"{tag}[{attribute}^='{attributeValueStart}']";
        }

        public static string RandomString(int length, bool includeNumbers = true)
        {
            string chars = includeNumbers ? "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789" : "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Range(1, length).Select(_ => chars[random.Next(chars.Length)]).ToArray());
        }

        public static string RandomFriendlyString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPRSTUVWY0123456789";
            return new string(Enumerable.Range(1, length).Select(_ => chars[random.Next(chars.Length)]).ToArray());
        }

        public static string RandomFriendlyStringWithInt(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Range(1, length).Select(_ => chars[random.Next(chars.Length)]).ToArray());
        }

        public static string RandomFriendlyStringWithInt()
        {
            return new Random().Next(10, 99).ToString();
        }

        public static string SanitiseSpecialAndIllegalChars(string original)
        {
            string str = original;
            str = string.Join("", str.Split(Path.GetInvalidPathChars()));
            str = string.Join("", str.Split(Path.GetInvalidFileNameChars()));
            foreach (var item in str)
            {
                str = char.IsPunctuation(item) ? string.Join("", str.Split(item)) : str;
            }
            foreach (var item in str)
            {
                str = char.IsSymbol(item) ? string.Join("", str.Split(item)) : str;
            }
            return str;
        }
    }
}