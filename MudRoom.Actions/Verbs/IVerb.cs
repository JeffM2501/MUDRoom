using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudRoom.Actions.Verbs
{
    public interface IVerb
    {
        IEnumerable<string> VerbWords { get; }
        IEnumerable<VerbArgumentDefinition> DesiredVerbArguments { get; }

        bool WordIsVerb(string word);

        bool Act(VerbInstance instace);
    }
}
