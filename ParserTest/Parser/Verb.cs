using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserTest.Parser
{
    public class Verb
    {
        public List<string> Words = new List<string>();
        public List<ParsedElement.MajorTypes> Arguments = new List<ParsedElement.MajorTypes>();

        public bool HasString(string name)
        {
            return Words.Contains(name);
        }

        public virtual void Act(string word, List<ParsedElement> Arguments)
        {

        }
    }

}
