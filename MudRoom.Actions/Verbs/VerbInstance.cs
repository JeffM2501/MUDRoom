using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudRoom.Actions.Verbs
{
    public class VerbInstance
    {
        public Type VerbID = null;

        public IVerb Action = null;
        public string Word = string.Empty;

        public List<VerbArgument> Arguments = new List<VerbArgument>();
        public bool Complete = false;

        public bool Act()
        {
            if (Action != null)
                return Action.Act(this);

            return true;
        }
    }
}
