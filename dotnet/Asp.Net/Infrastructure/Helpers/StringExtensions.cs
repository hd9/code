using System.Collections.Generic;
using System.Linq;

namespace Web.Infrastructure.Helpers
{
    public static class StringExtensions
    {
        public static bool ContainsAny(this string str, IEnumerable<string> searchTerms)
        {
            return searchTerms.Any(searchTerm => str.ToLower().Contains(searchTerm.ToLower()));
        }

        public static bool ContainsAll(this string str, IEnumerable<string> searchTerms)
        {
            return searchTerms.All(searchTerm => str.ToLower().Contains(searchTerm.ToLower()));
        }
    }
}