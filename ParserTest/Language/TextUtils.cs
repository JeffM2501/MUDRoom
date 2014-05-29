using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ParserTest.Describables;
using ParserTest.IO;

namespace ParserTest.Language
{
    public static class TextUtils
    {
		public static Random Rand = new Random();

		public static string GetRandomString(List<string> list)
		{
			return (list[Rand.Next(list.Count())]);
		}

        public static class SayingConstants
        {
            public const string Middle = "middle";
            public const string North = "north";
            public const string South = "south";
            public const string East = "east";
            public const string West = "west";
            public const string Up = "up";
            public const string Down = "down";
        }

        // global language, should actually save the language ID with viewer
   //     public static ILanguage Language = null;

        public static English EnglishLanguage = new English();
        // OTHER supported languages here

        static TextUtils()
        {
        }
    }
}
