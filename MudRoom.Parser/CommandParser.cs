using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MudRoom.Actions;
using MudRoom.Actions.Verbs;

namespace MudRoom.Parser
{
    public class CommandParser
    {
        public List<IVerb> ActiveVerbs = new List<IVerb>();

        protected ParserResults PendingResults = null;

        public ParserResults ParseText(string text)
        {
            PendingResults = new ParserResults();
            PendingResults.RawLine = text.Trim();

            if (PendingResults.RawLine == string.Empty)
                return PendingResults;

            int lastCompleteVerb = -1;
            int count = 0;
            int lastGoodEndIndex = 0;

            while (count < PendingResults.RawLine.Length)
            {
                int saveCount = count;
                string word = ReadWord(PendingResults.RawLine, ref count).Trim().ToLowerInvariant();

                IVerb verb = FindVerb(word);

                if (verb != null)
                {
                }
                else
                {

                }

                if (verb != null)
                {

                }
                else
                    count++;
            }

            return PendingResults;
        }

        public string ReadWord(string text, ref int location)
        {
            int cur = location;

            while (cur < text.Length)
            {
                if (Char.IsWhiteSpace(text[cur]) || Char.IsPunctuation(text[cur]))
                    break;

                cur++;
            }

            int start = location;
            location = cur;
            return text.Substring(start, location - start);
        }

        public IVerb FindVerb(string word)
        {
            return ActiveVerbs.Find(delegate(IVerb v) { return v.WordIsVerb(word); });
        }
    }
}
