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

            CommandParser.Verbs.Add(new PutVerb());
            CommandParser.Verbs.Add(new LookVerb());

            CommandParser.Verbs.Add(new InventoryVerb());
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
            Words.Add("whereami");
        }

        public override void Act(VerbInstance instance)
        {
            instance.PlayerContext.Output.OutputIOLine(string.Empty);
            instance.EnvironmnetContext.Describe(null, instance.PlayerContext.Output);
        }
    }

     public class InventoryVerb : Verb
     {
         public InventoryVerb()
         {
             Words.Add("inventory");
             Words.Add("inv");
         }

         public override void Act(VerbInstance instance)
         {
             instance.PlayerContext.Describe(null);
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
            if (instance.ParsedArguments.Count < 1)
                instance.PlayerContext.WriteLine("What do you want to get?");
            else
            {
                if (instance.ParsedArguments[0].Element == null)
                    instance.PlayerContext.WriteLine("You can't find a " + instance.ParsedArguments[0].Argument.Word + " anywhere");

                DescribedElementInstance element = instance.ParsedArguments[0].Element;

                DescribedElementInstance actual = instance.EnvironmnetContext.Get(element, -1);
                if (actual == null)
                    instance.PlayerContext.WriteLine("You can't get the " + element.ElementDefintion.CreateDescription(1));
                else
                { 
                    instance.PlayerContext.Put(actual);
                    instance.PlayerContext.WriteLine("You put " + actual.ElementDefintion.CreateDescription(actual.Quanity) + " in your pack");
                }
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
                instance.PlayerContext.WriteLine("What do you want to drop?");
            else
            {
                if (instance.ParsedArguments[0].Element == null)
                    instance.PlayerContext.WriteLine("You don't seem to have " + instance.ParsedArguments[0].Argument.Word + " on your person");

                DescribedElementInstance element = instance.ParsedArguments[0].Element;

                DescribedElementInstance actual = instance.PlayerContext.Get(element, -1);
                if (actual == null)
                    instance.PlayerContext.WriteLine("You can't get the " + element.ElementDefintion.CreateDescription(1));
                else
                {
                    actual.Location = DescribedElementInstance.ElementLocations.Middle;

                    instance.EnvironmnetContext.Put(actual);
                    instance.PlayerContext.WriteLine("You drop " + actual.ElementDefintion.CreateDescription(actual.Quanity) + " into on the ground");
                }
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
            if (instance.ParsedArguments.Count == 0)
                instance.PlayerContext.WriteLine("Where do you want to go?");
            else
            {
                VerbInstance.ParsedArgument arg = instance.ParsedArguments[0];
                if (arg.Argument.MajorType == ParsedElement.MajorTypes.Exit)
                    GoDirection(instance, arg.Location);
                else if (arg.Argument.MajorType == ParsedElement.MajorTypes.AnyItem)
                    instance.PlayerContext.WriteLine("You stand near " + arg.Element.ElementDefintion.CreateDescription(arg.Element.Quanity));
                else
                    instance.PlayerContext.WriteLine("You can't go to " + arg.Argument.Word);
            }
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
            DescribedElementInstance.ElementLocations location = DescribedElementInstance.ElementLocations.Middle;

            if (TextUtils.Language.IsLocation(instance.Word, ref location))
                GoDirection(instance,location);
        }
    }
}
