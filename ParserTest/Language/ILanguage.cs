using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ParserTest.Describables;
using ParserTest.IO;

namespace ParserTest.Language
{
    public interface ILanguage
    {
        string GetLanguageID();

        string GetQuantityDescritpion(int quanity, string firstWord, bool singleIsPair);

        void AddSaying(string key, string saying);
        void AddSayings(string key, string[] sayings);
        string GetRandomSaying(string key);

        string GetLocationDescription(DescribedElementInstance.ElementLocations location);
        void WriteElement(DescribedElementInstance element, bool followOpenChildren, IOutputInterface output);

        bool IsLocation(string word, ref DescribedElementInstance.ElementLocations location);
        bool IsFiller(string word);

        bool IsPluralOfNoun(string word, string noun);

        bool IsConnector(string word);
    }
}
