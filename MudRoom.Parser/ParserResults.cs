using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MudRoom.Actions;

namespace MudRoom.Parser
{
    public class ParserResults
    {
        public ActionList Actions = new ActionList();
        public string RawLine = string.Empty;
    }
}
