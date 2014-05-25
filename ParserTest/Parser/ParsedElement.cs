using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserTest.Parser
{
    public class ParsedElement
    {
        public enum MajorTypes
        {
            Verb,
            Noun,
            Adjetive,
            Name,
            SpecificItem,
            GenericItem,
            Filler,
            Exit,
            Unknown,
        }

        public MajorTypes MajorType;
        public string Subtype;

        public string Word = string.Empty;

        public List<string> Descriptors = new List<string>();
    }
}
