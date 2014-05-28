using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ParserTest.Parser;
using ParserTest.Describables;
using ParserTest.Language;

namespace ParserTest.Parser.Actions
{
    public static class CommonVerbs
    {
        public static void Setup()
        {
            CommandParser.Verbs.Add(new GetVerb());
            CommandParser.Verbs.Add(new InspectVerb());
            CommandParser.Verbs.Add(new GoVerb());
            CommandParser.Verbs.Add(new DirectionVerb());
        }
    }

    public class InspectVerb : Verb
    {
        public InspectVerb()
        {
            Words.Add("inspect");
            Words.Add("view");

            Arguments.Add(ParsedElement.MajorTypes.AnyItem);
        }

        public override void Act(VerbInstance instance)
        {
            if (instance.ParsedArguments.Count != 1)
                instance.PlayerContext.WriteLine("What do you want to look at?");
            else
            {
                if (instance.ParsedArguments[0].Element == null)
                    instance.PlayerContext.WriteLine("You can't look at the " + instance.ParsedArguments[0].Argument.Word);

                instance.EnvironmnetContext.Describe(instance.ParsedArguments[0].Element, instance.PlayerContext.Output);
            }
        }
    }

     public class LookVerb : Verb
    {
        public LookVerb()
        {
            Words.Add("look");
        }

        public override void Act(VerbInstance instance)
        {
            instance.EnvironmnetContext.Describe(null, instance.PlayerContext.Output);
        }
    }

    public class GetVerb : Verb
    {
        public GetVerb()
        {
            Words.Add("get");
            Words.Add("take");
            Words.Add("grab");

            Arguments.Add(ParsedElement.MajorTypes.EnvironmentItem);
        }

        public override void Act(VerbInstance instance)
        {
            if (instance.ParsedArguments.Count != 1)
                instance.PlayerContext.WriteLine("What do you want to get?");
            else
            {
                if (instance.ParsedArguments[0].Element == null)
                    instance.PlayerContext.WriteLine("You can't find a " + instance.ParsedArguments[0].Argument.Word + " anywhere");

                DescribedElementInstance element = instance.ParsedArguments[0].Element;

                if (element.)
                instance.EnvironmnetContext.Describe(, instance.PlayerContext.Output);
            }
        }
    }

    public class PutVerb : Verb
    {
        public PutVerb()
        {
            Words.Add("put");
            Words.Add("drop");

            Arguments.Add(ParsedElement.MajorTypes.PersonalItem);
            Arguments.Add(ParsedElement.MajorTypes.Exit);
        }

        public override void Act(VerbInstance instance)
        {
            if (instance.ParsedArguments.Count != 1)
                instance.PlayerContext.WriteLine("What do you want to get?");
            else
            {
                if (instance.ParsedArguments[0].Element == null)
                    instance.PlayerContext.WriteLine("You can't find a " + instance.ParsedArguments[0].Argument.Word + " anywhere");

                instance.EnvironmnetContext.Describe(instance.ParsedArguments[0].Element, instance.PlayerContext.Output);
            }
        }
    }

    public class GoVerb : Verb
    {
        public class GoDirectionEventArgs : EventArgs
        {
            public VerbInstance VerbInst = null;
            public DescribedElementInstance.ElementLocations Direction = DescribedElementInstance.ElementLocations.Middle;

            public GoDirectionEventArgs(VerbInstance verb, DescribedElementInstance.ElementLocations dir)
            {
                Direction = dir;
                VerbInst = verb;
            }
        }

        public static EventHandler<GoDirectionEventArgs> GoDirectionEvent;

        public GoVerb()
        {
            Words.Add("go");
            Words.Add("walk");

            Arguments.Add(ParsedElement.MajorTypes.Noun);
        }

        public override void Act(VerbInstance instance)
        {
            base.Act(instance);
        }

        protected void GoDirection(VerbInstance instance, DescribedElementInstance.ElementLocations direction)
        {
            if (GoDirectionEvent == null)
                return;

            GoDirectionEvent(this, new GoDirectionEventArgs(instance, direction));
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
