using System;

namespace AO.AutomationFramework.Core.BusinessLogic.Extensions
{
    public static class DotNetExtensions
    {
        public static bool ContainsWithAnyCase(this string stringA, string stringB)
        {
            return stringA.IndexOf(stringB, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}