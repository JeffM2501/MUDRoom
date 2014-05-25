using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ParserTest.Parser;
using ParserTest.Describables;

namespace ParserTest.Language
{
    public static class CommonVerbs
    {
        public static void SetupLanugage()
        {
            CommandParser.Verbs.Add(new GetVerb());
        }
    }

    public class LookVerb : Verb
    {

    }

    public class GetVerb : Verb
    {
        public GetVerb()
        {
            Words.Add("get");
            Words.Add("take");
            Words.Add("grab");

            Arguments.Add(ParsedElement.MajorTypes.Noun);
        }

        public override void Act(string word, List<ParsedElement> Arguments)
        {
            base.Act(word, Arguments);
        }
    }

    public class GoVerb : Verb
    {
        public GoVerb()
        {
            Words.Add("go");
            Words.Add("walk");

            Arguments.Add(ParsedElement.MajorTypes.Name);
        }

        public override void Act(string word, List<ParsedElement> Arguments)
        {
            base.Act(word, Arguments);
        }

        protected void GoDirection(DescribedElementInstance.ElementLocations direction)
        {

        }
    }

    public class DirectionVerb : GoVerb
    {
        public DirectionVerb()
        {
            Words.Add("north");
            Words.Add("south");
            Words.Add("east");
            Words.Add("west");
        }

        public override void Act(string word, List<ParsedElement> Arguments)
        {
            base.Act(word, Arguments);
        }
    }
}
