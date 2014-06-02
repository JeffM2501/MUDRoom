using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudRoom.Actions.Verbs
{
    public enum VerbArgumentTypes
    {
        Noun,
        Adjetive,
        Item,
        Container,
        Exit,
        Person,
        Feature,
        Filler,
        Quanity,
        Anything,
        Sentance,
        Invalid,
    }

    public enum VerbArgumentContextFilters
    {
        Environment,
        Personal,
        Container,
        Any,
    }

    public class VerbArgumentDefinition
    {
        public VerbArgumentTypes ArgumentType = VerbArgumentTypes.Anything;
        public VerbArgumentContextFilters ContextFilter = VerbArgumentContextFilters.Any;
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

        public virtual VerbArgument Clone()
        {
            return new VerbArgument(ArgumentType, Text);
        }
    }
}
