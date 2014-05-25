using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserTest
{
    public static class TextUtils
    {
        public static class English
        {
            public static bool IsVowel(char c)
            {
                return c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u';
            }

            public static string GetQuantityDescritpion(int quanity, string firstWord, bool singleIsPair)
            {
                if (quanity < 1 || firstWord.Length == 0)
                    return string.Empty;

                if (quanity == 1)
                {
                    if (singleIsPair)
                        return "a pair of";

                    return IsVowel(firstWord[0]) ? "an" : "a";
                }
                else if (quanity == 2)
                {
                    return "a pair of";
                }
                else if (quanity < 4)
                    return "some";
                else if (quanity < 10)
                    return "a bunch of";
                
                return "a ton of";
            }
        }
    }
}
