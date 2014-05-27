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

        public static class English
        {
			public const string Middle = "middle";
			public const string North = "north";
            public const string South = "south";
            public const string East = "east";
            public const string West = "west";
            public const string Up = "up";
            public const string Down = "down";
            
			static English()
			{
				AddSayings(Middle, new string[] { "In the middle", "Near the center", "In the center" });
				AddSayings(North, new string[] { "To the north", "Up to the north", "On the northern side" });
				AddSayings(South, new string[] { "To the south", "Down to the south","On the southern side" });
				AddSayings(West, new string[] { "To the west", "Over to the easts", "On the western side" });
				AddSayings(East, new string[] { "To the east", "Over to the east", "On the eastern side" });

				AddSayings(Up, new string[] { "Above", "Overhead"});
				AddSayings(Down, new string[] { "Below", "Underfoot", "On ground" });
			}

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

			private static Dictionary<string, List<string>> Terms = new Dictionary<string, List<string>>();

			public static void AddSaying(string key, string saying)
			{
				string lKey = key.ToLower();

				if(!Terms.ContainsKey(lKey))
					Terms.Add(lKey, new List<string>());
				Terms[lKey].Add(saying);
			}

			public static void AddSayings(string key, string[] sayings)
			{
				string lKey = key.ToLower();

				if(!Terms.ContainsKey(lKey))
					Terms.Add(lKey, new List<string>());
				Terms[lKey].AddRange(sayings);
			}

			public static string GetRandomSayingFast(string key)
			{
				if(Terms.ContainsKey(key))
					return GetRandomString(Terms[key]);

				return string.Empty;
			}

			public static string GetRandomSaying(string key)
			{
				return GetRandomSayingFast(key.ToLower());
			}

			public static string GetLocationDescription(DescribedElementInstance.ElementLocations location)
			{
				switch (location)
				{
					case DescribedElementInstance.ElementLocations.Middle:
						return GetRandomSayingFast(Middle);

					case DescribedElementInstance.ElementLocations.North:
						return GetRandomSayingFast(North);

					case DescribedElementInstance.ElementLocations.South:
						return GetRandomSayingFast(South);

					case DescribedElementInstance.ElementLocations.East:
						return GetRandomSayingFast(East);

					case DescribedElementInstance.ElementLocations.West:
						return GetRandomSayingFast(West);

					case DescribedElementInstance.ElementLocations.Up:
						return GetRandomSayingFast(Up);

					case DescribedElementInstance.ElementLocations.Down:
						return GetRandomSayingFast(Down);
				}

				return string.Empty;
			}

			public static void WriteElement(DescribedElementInstance element, bool followOpenChildren, IOutputInterface output)
			{
				StringBuilder lineBuilder = new StringBuilder();
			
				lineBuilder.Append(GetLocationDescription(element.Location));
			}
        }
    }
}
