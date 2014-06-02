using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MudRoom.Actions.Verbs;

namespace MudRoom.Actions
{
    public class ActionList
    {
        public List<VerbInstance> Actions = new List<VerbInstance>();

        public bool HasErrors()
        {
            foreach(VerbInstance action in Actions)
            {
                if (!action.Complete)
                    return true;
            }
            return false;
        }
    }
}
