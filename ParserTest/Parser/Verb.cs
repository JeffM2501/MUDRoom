using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ParserTest.Describables;
using ParserTest.Viewer;

namespace ParserTest.Parser
{
    public enum VerbArgumentTypes
    {
        Verb,
        Noun,
        Adjetive,
        AnyItem,
        EnvironmentItem,
        PersonalItem,
        Filler,
        Exit,
        Quanity,
        Anything,
        Sentance,
        Invalid,
    }

    public class VerbArgument
    {
        public VerbArgumentTypes ArgumentType = VerbArgumentTypes.Invalid;
        public string Text = string.Empty;

        public VerbArgument() { }

        public VerbArgument(VerbArgumentTypes argType, string text)
        {
            ArgumentType = argType;
            Text = text;
        }
    }

    public class VerbElementArgument : VerbArgument
    {
        public DescribedElementInstance Element = null;
        public List<DescribedElementInstance> PossibleElements = new List<DescribedElementInstance>();

        public VerbElementArgument()
        {
            ArgumentType = VerbArgumentTypes.AnyItem;
        }

        public VerbElementArgument(string text)
        {
            ArgumentType = VerbArgumentTypes.AnyItem;
            Text = text;
        }
    }

    public class VerbLocationArgument : VerbArgument
    {
        public DescribedElementInstance.ElementLocations Location = DescribedElementInstance.ElementLocations.Middle;

        public VerbLocationArgument(DescribedElementInstance.ElementLocations loc)
        {
            ArgumentType = VerbArgumentTypes.Exit;
            Location = loc;
        }

        public VerbLocationArgument(string text, DescribedElementInstance.ElementLocations loc)
        {
            ArgumentType = VerbArgumentTypes.Exit;
            Location = loc;
            Text = text;
        }
    }

    public class VerbValueArgument : VerbArgument
    {
        public UInt64 Value = UInt64.MaxValue;

        public VerbValueArgument()
        {
            ArgumentType = VerbArgumentTypes.Quanity;
        }

        public VerbValueArgument(string text, UInt64 val)
        {
            ArgumentType = VerbArgumentTypes.Quanity;
            Value = val;
            Text = text;
        }
    }

    public class VerbInstance
    {
        public Verb Action = null;
        public string Text = string.Empty;
        public string PrefixText = string.Empty;

        public List<VerbArgument> Arguments = new List<VerbArgument>();

        public DescriptionContext EnvironmnetContext = null;
        public ViewerContext PlayerContext = null;

        public bool Act()
        {
            if (Action != null)
                return Action.Act(this);

            return true;
        }
    }

    public class Verb
    {
        public List<string> Words = new List<string>();
        public List<VerbArgumentTypes> Arguments = new List<VerbArgumentTypes>();

        public bool HasString(string name)
        {
            return Words.Contains(name);
        }

        public virtual bool Act(VerbInstance instance)  // return true if it worked, false if we want to keep the verb on the stack
        {
            return true;
        }

        protected string HelpTextCache = string.Empty;
        public string HelpText()
        {
            if (HelpTextCache == string.Empty)
            {
                StringBuilder builder = new StringBuilder();

                if (Words.Count == 0)
                    builder.Append("***ERROR " + this.GetType().Name.ToString() + "****");
                else
                {
                    builder.Append(Words[0]);
                    if (Words.Count > 1)
                    {
                        builder.Append(" [");
                        for(int i = 1; i < Words.Count; i++)
                        {
                            builder.Append(Words[i]);
                            if (i != Words.Count - 1)
                                builder.Append(", ");
                        }
                        builder.Append("]");
                    }
                }
                builder.Append(" ");
                for (int i = 0; i < Arguments.Count; i++)
                {
                    builder.Append(Arguments[i].ToString());
                    if (i != Words.Count - 1)
                        builder.Append(" ");
                }

                HelpTextCache = builder.ToString();
            }

            return HelpTextCache;
        }
    }

}
