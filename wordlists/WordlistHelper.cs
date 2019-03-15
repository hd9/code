using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace HildenCo.Code.Automation
{

    /// <summary>
    /// A Helper class to facilitate word/name/phrase random generation out of a wordlist
    /// <summary>
    public class WordlistHelper
    {

        #region Attributes
        private readonly static List<string> names, words;
        private readonly static int nLen, wLen;
        private readonly static Random r = new Random();
        #endregion

        #region ctor
        static ContentHelper()
        {
            // Importing wordlists from a resource file called TestResources
            // Wordlists:
            //      Wordlist_Names: Common English Names
            //      Wordlist_Words: Common English Words
            names = TestResources.Wordlist_Names.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            words = TestResources.Wordlist_Words.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            nLen = names.Count;
            wLen = words.Count;
        }
        #endregion

        /// <summary>
        /// Generates a Random Name out of the names wordlist
        /// <summary>
        public static string RandomName()
        {
            return names[RandomNumber(0, nLen)];
        }

        /// <summary>
        /// Generates a random sentence out of the names wordlist
        /// <summary>
        public static string RandomSentence(int maxWords = 5, bool camelCase = true)
        {
            var vals = new List<string>();
            for (var i = 0; i < maxWords; i++)
            {
                var w = words[RandomNumber(0, wLen)];
                if (camelCase) w = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(w);
                vals.Add(w);
            }

            return string.Join(" ", vals);
        }

        /// <summary>
        // Generates a random guid and return as string. Default size=8
        /// <summary>
        public static string RandomGuid(int size=8)
        {
            return Guid.NewGuid().ToString().Substring(0, size);
        }

        /// <summary>
        // Generates a random number between 10 and 100 by default
        /// <summary>
        public static int RandomNumber(int min=10, int max=100)
        {
            return r.Next(min, max);
        }
    }
}
