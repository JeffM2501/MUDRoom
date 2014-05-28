using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ParserTest.Describables;
using ParserTest.IO;

namespace ParserTest.Language
{
    public class English : ILanguage
    {
        public English()
        {
            AddSayings(TextUtils.SayingConstants.Middle, new string[] { "In the middle", "Near the center", "In the center" });
            AddSayings(TextUtils.SayingConstants.North, new string[] { "To the north", "Up to the north", "On the northern side" });
            AddSayings(TextUtils.SayingConstants.South, new string[] { "To the south", "Down to the south", "On the southern side" });
            AddSayings(TextUtils.SayingConstants.West, new string[] { "To the west", "Over to the easts", "On the western side" });
            AddSayings(TextUtils.SayingConstants.East, new string[] { "To the east", "Over to the east", "On the eastern side" });

            AddSayings(TextUtils.SayingConstants.Up, new string[] { "Above", "Overhead" });
            AddSayings(TextUtils.SayingConstants.Down, new string[] { "Below", "Underfoot", "On ground" });

            Fillers.Add("a");
            Fillers.Add("the");
            Fillers.Add("an");
            Fillers.Add("teh");
            Fillers.Add("that");
        }

        public static string SingularToBe = "is";
        public static string PluralToBe = "are";

        public static string Containing = "containing;";

        public List<string> Fillers = new List<string>();

        public string QuantyToBe(int quanity)
        {
            return quanity > 1 ? PluralToBe : SingularToBe;
        }

        public string GetLanguageID()
        {
            return "EN-US";
        }

        public bool IsVowel(char c)
        {
            return c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u';
        }

        public string GetQuantityDescritpion(int quanity, string firstWord, bool singleIsPair)
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

        private Dictionary<string, List<string>> Terms = new Dictionary<string, List<string>>();

        public void AddSaying(string key, string saying)
        {
            string lKey = key.ToLower();

            if (!Terms.ContainsKey(lKey))
                Terms.Add(lKey, new List<string>());
            Terms[lKey].Add(saying);
        }

        public void AddSayings(string key, string[] sayings)
        {
            string lKey = key.ToLower();

            if (!Terms.ContainsKey(lKey))
                Terms.Add(lKey, new List<string>());
            Terms[lKey].AddRange(sayings);
        }

        public string GetRandomSayingFast(string key)
        {
            if (Terms.ContainsKey(key))
                return TextUtils.GetRandomString(Terms[key]);

            return string.Empty;
        }

        public string GetRandomSaying(string key)
        {
            return GetRandomSayingFast(key.ToLower());
        }

        public string GetLocationDescription(DescribedElementInstance.ElementLocations location)
        {
            switch (location)
            {
                case DescribedElementInstance.ElementLocations.Middle:
                    return GetRandomSayingFast(TextUtils.SayingConstants.Middle);

                case DescribedElementInstance.ElementLocations.North:
                    return GetRandomSayingFast(TextUtils.SayingConstants.North);

                case DescribedElementInstance.ElementLocations.South:
                    return GetRandomSayingFast(TextUtils.SayingConstants.South);

                case DescribedElementInstance.ElementLocations.East:
                    return GetRandomSayingFast(TextUtils.SayingConstants.East);

                case DescribedElementInstance.ElementLocations.West:
                    return GetRandomSayingFast(TextUtils.SayingConstants.West);

                case DescribedElementInstance.ElementLocations.Up:
                    return GetRandomSayingFast(TextUtils.SayingConstants.Up);

                case DescribedElementInstance.ElementLocations.Down:
                    return GetRandomSayingFast(TextUtils.SayingConstants.Down);
            }

            return string.Empty;
        }
        
        public void WriteElement(DescribedElementInstance element, bool followOpenChildren, IOutputInterface output)
        {
            bool hasKids = followOpenChildren && element.DescribeChildren && element.Children.Count != 0;

            StringBuilder lineBuilder = new StringBuilder();

            lineBuilder.Append(GetLocationDescription(element.Location));

            lineBuilder.Append(" ");
            lineBuilder.Append(QuantyToBe(element.Quanity));

            lineBuilder.Append(" ");
            lineBuilder.Append(element.ElementDefintion.CreateDescription(element.Quanity));

            if (hasKids)
            {
                lineBuilder.AppendLine(" "); lineBuilder.Append(Containing);

                foreach (DescribedElementInstance child in element.Children)
                    lineBuilder.AppendLine(element.ElementDefintion.CreateDescription(element.Quanity));
            }

            output.OutputIOLine(lineBuilder.ToString());
        }

        public bool IsPluralOfNoun(string word, string noun)
        {
            char c = word[word.Length - 1];
            string sub = word.Substring(0, word.Length - 1);
            return word.Length > 1 &&  c == 's' &&  sub == noun;
        }

        public bool IsFiller(string word)
        {
            return Fillers.Contains(word);
        }

        public bool IsLocation (string word, ref DescribedElementInstance.ElementLocations location)
        {
            if (word == "north" || word == "n")
            {
                location = DescribedElementInstance.ElementLocations.North;
                return true;
            }

            if (word == "south" || word == "s")
            {
                location = DescribedElementInstance.ElementLocations.South;
                return true;
            }

            if (word == "east" || word == "ae")
            {
                location = DescribedElementInstance.ElementLocations.East;
                return true;
            }

            if (word == "west" || word == "w")
            {
                location = DescribedElementInstance.ElementLocations.West;
                return true;
            }

            if (word == "up" || word == "u")
            {
                location = DescribedElementInstance.ElementLocations.Up;
                return true;
            }

            if (word == "down" || word == "d")
            {
                location = DescribedElementInstance.ElementLocations.Down;
                return true;
            }

            if (word == "middle" || word == "center")
            {
                location = DescribedElementInstance.ElementLocations.Middle;
                return true;
            }

            return false;
        }
    }
}
