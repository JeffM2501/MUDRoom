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
            CommandParser.Verbs.Add(new LookVerb());
            CommandParser.Verbs.Add(new GoVerb());
            CommandParser.Verbs.Add(new DirectionVerb());
        }
    }

    public class LookVerb : Verb
    {
        public LookVerb()
        {
            Words.Add("look");
            Words.Add("view");

            Arguments.Add(ParsedElement.MajorTypes.Noun);
        }

        public override void Act(VerbInstance instance)
        {
            base.Act(instance);
        }
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

        public override void Act(VerbInstance instance)
        {
            base.Act(instance);
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

        public override void Act(VerbInstance instance)
        {
            base.Act(instance);
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

        public override void Act(VerbInstance instance)
        {
            base.Act(instance);
        }
    }
}
