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
            CommandParser.Verbs.Add(new HelpVerb());

            CommandParser.Verbs.Add(new TellVerb());
        }
    }

    public class InspectVerb : Verb
    {
        public InspectVerb()
        {
            Words.Add("inspect");
            Words.Add("view");

            Arguments.Add(VerbArgumentTypes.AnyItem);
        }

        public override bool Act(VerbInstance instance)
        {
            if (instance.Arguments.Count != 1)
                instance.PlayerContext.WriteLine("What do you want to look at?");
            else
            {
                VerbElementArgument elemnt = instance.Arguments[0] as VerbElementArgument;
                if (elemnt == null)
                    instance.PlayerContext.WriteLine("You can't look at the " + instance.Arguments[0].Text);
                else
                {
                    instance.EnvironmnetContext.Describe(elemnt.Element, instance.PlayerContext);
                    instance.PlayerContext.InspectedElement = elemnt.Element;
                }
            }

            return true;
        }
    }

     public class LookVerb : Verb
    {
        public LookVerb()
        {
            Words.Add("look");
            Words.Add("whereami");
        }

        public override bool Act(VerbInstance instance)
        {
            instance.PlayerContext.Output.OutputIOLine(string.Empty);
            instance.EnvironmnetContext.Describe(null, instance.PlayerContext);
            return true;
        }
    }

     public class InventoryVerb : Verb
     {
         public InventoryVerb()
         {
             Words.Add("inventory");
             Words.Add("inv");
         }

         public override bool Act(VerbInstance instance)
         {
             instance.PlayerContext.Describe(null);
             return true;
         }
     }

    public class GetVerb : Verb
    {
        public GetVerb()
        {
            Words.Add("get");
            Words.Add("take");
            Words.Add("grab");

            Arguments.Add(VerbArgumentTypes.EnvironmentItem);
        }

        public override bool Act(VerbInstance instance)
        {
            if (instance.Arguments.Count < 1)
            {
                instance.PlayerContext.WriteLine("What do you want to get?");
                return false;
            }
            else
            {
                VerbElementArgument elemnt = instance.Arguments[0] as VerbElementArgument;
                if (elemnt == null)
                    instance.PlayerContext.WriteLine("You can't find a " + instance.Arguments[0].Text + " anywhere");

                DescribedElementInstance actual = instance.EnvironmnetContext.Get(elemnt.Element, -1);
                if (actual == null)
                    instance.PlayerContext.WriteLine("You can't get the " + instance.PlayerContext.Language.CreateDescription(1,elemnt.Element.ElementDefintion));
                else
                {
                    instance.PlayerContext.Put(actual);
                    instance.PlayerContext.WriteLine("You put " + instance.PlayerContext.Language.CreateDescription(actual.Quanity,actual.ElementDefintion) + " in your pack");
                }
            }

            return true;
        }
    }

    public class PutVerb : Verb
    {
        public PutVerb()
        {
            Words.Add("put");
            Words.Add("drop");

            Arguments.Add(VerbArgumentTypes.PersonalItem);
            Arguments.Add(VerbArgumentTypes.Exit);
        }

        public override bool Act(VerbInstance instance)
        {
            if (instance.Arguments.Count != 1)
            {
                instance.PlayerContext.WriteLine("What do you want to drop?");
                return false;
            }
            else
            {
                VerbElementArgument elemnt = instance.Arguments[0] as VerbElementArgument;
                if (elemnt == null)
                     instance.PlayerContext.WriteLine("You don't seem to have " + instance.Arguments[0].Text+ " on your person");

                DescribedElementInstance actual = instance.PlayerContext.Get(elemnt.Element, -1);
                if (actual == null)
                    instance.PlayerContext.WriteLine("You can't get the " + instance.PlayerContext.Language.CreateDescription(1,elemnt.Element.ElementDefintion));
                else
                {
                    actual.Location = DescribedElementInstance.ElementLocations.Middle;

                    instance.EnvironmnetContext.Put(actual);
                    instance.PlayerContext.WriteLine("You drop " + instance.PlayerContext.Language.CreateDescription(actual.Quanity, actual.ElementDefintion) + " into on the ground");
                }
            }

            return true;
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

            Arguments.Add(VerbArgumentTypes.Noun);
        }

        public override bool Act(VerbInstance instance)
        {
            instance.PlayerContext.InspectedElement = null;

            if (instance.Arguments.Count == 0)
            {
                instance.PlayerContext.WriteLine("Where do you want to go?");
                return false;
            }
            else
            {
                VerbLocationArgument locArg = instance.Arguments[0] as VerbLocationArgument;
                VerbElementArgument elementArg = instance.Arguments[0] as VerbElementArgument;

                if (locArg != null)
                    GoDirection(instance, locArg.Location);
                else if (elementArg != null)
                    instance.PlayerContext.WriteLine("You stand near " + instance.PlayerContext.Language.CreateDescription(elementArg.Element.Quanity,elementArg.Element.ElementDefintion));
                else
                    instance.PlayerContext.WriteLine("You can't go to " + instance.Arguments[0].Text);
            }
            return true;
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
            Words.Clear();

            Words.Add("north");
            Words.Add("south");
            Words.Add("east");
            Words.Add("west");

            Words.Add("n");
            Words.Add("s");
            Words.Add("e");
            Words.Add("w");
        }

        public override bool Act(VerbInstance instance)
        {
            instance.PlayerContext.InspectedElement = null;

            DescribedElementInstance.ElementLocations location = DescribedElementInstance.ElementLocations.Middle;

            if (instance.PlayerContext.Language.IsLocation(instance.Text, ref location))
                GoDirection(instance,location);

            return true;
        }
    }

    public class HelpVerb : Verb
    {
        public HelpVerb()
        {
            Words.Add("help");

            Arguments.Add(VerbArgumentTypes.Sentance);
        }

        public override bool Act(VerbInstance instance)
        {
            instance.PlayerContext.WriteLine(string.Empty);

            if (instance.Arguments.Count == 0)
            {
                instance.PlayerContext.WriteLine("Available Actions:");
                foreach (Verb verb in CommandParser.Verbs)
                    instance.PlayerContext.WriteLine(verb.HelpText());
            }
            else
            {
                Verb verb = CommandParser.FindVerb(instance.Arguments[0].Text);
                if (verb == null)
                    instance.PlayerContext.WriteLine(instance.Arguments[0].Text + " is not the name of an action");
                else
                    instance.PlayerContext.WriteLine(verb.HelpText());
            }

            return true;
        }
    }

    public class TellVerb : Verb
    {
        public TellVerb()
        {
            Words.Add("tell");

            Arguments.Add(VerbArgumentTypes.EnvironmentItem);
            Arguments.Add(VerbArgumentTypes.Sentance);
        }

        public override bool Act(VerbInstance instance)
        {
            instance.PlayerContext.WriteLine(string.Empty);

            if (instance.Arguments.Count < 2)
            {
                if (instance.Arguments.Count == 0 || (instance.Arguments[0] as VerbElementArgument == null))
                    instance.PlayerContext.WriteLine("Who do you want to say what to?");
                else
                {
                    if (instance.PlayerContext.PendingArguments.Count == 1)
                    {
                        instance.Arguments.Insert(0, instance.PlayerContext.PendingArguments[0]);
                        instance.PlayerContext.PendingArguments.Clear();
                    }
                    else
                    {
                        instance.PlayerContext.PendingArguments.Add(instance.Arguments[0]);
                        instance.PlayerContext.WriteLine("What do you want to say to " + instance.PlayerContext.Language.CreateDescription(1,(instance.Arguments[0] as VerbElementArgument).Element.ElementDefintion) + "?");
                    }
                }
            }

            if (instance.Arguments.Count >= 2)
            {
                VerbElementArgument element = instance.Arguments[0] as VerbElementArgument;
                if (element == null)
                    instance.PlayerContext.WriteLine("You don't see " + instance.Arguments[0].Text + " anywhere");
                else
                {
                    string text = instance.Arguments[1].Text;
                    int i = 0;
                    if (instance.PlayerContext.Language.IsActionFillter(CommandParser.ReadWord(instance.Arguments[1].Text, ref i).ToLowerInvariant()))
                        text = text.Substring(i + 1);

                    instance.PlayerContext.WriteLine("You tell " + instance.PlayerContext.Language.CreateDescription(1,element.Element.ElementDefintion) + ", \"" + instance.PlayerContext.Language.MakeSentanceStart(text) + "\"");
                }
            }

            return true;
        }

    }
}
