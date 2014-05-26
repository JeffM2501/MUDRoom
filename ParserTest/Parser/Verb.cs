using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ParserTest.Describables;
using ParserTest.Viewer;

namespace ParserTest.Parser
{

    public class VerbInstance
    {
        public Verb Action = null;

        public string Word = string.Empty;

        public List<ParsedElement> RawArguments = new List<ParsedElement>();
        public List<ParsedElement> ParsedElements = new List<ParsedElement>();
        public DescriptionContext EnvironmnetContext = null;
        public ViewerContext PlayerContext = null;

        public void Act()
        {
            if (Action != null)
                Action.Act(this);
        }
    }


    public class Verb
    {
        public List<string> Words = new List<string>();
        public List<ParsedElement.MajorTypes> Arguments = new List<ParsedElement.MajorTypes>();

        public bool HasString(string name)
        {
            return Words.Contains(name);
        }

        public virtual void Act(VerbInstance instance)
        {

        }
    }

}
