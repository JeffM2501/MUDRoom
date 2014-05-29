using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ParserTest.Describables;
using ParserTest.Language;
using ParserTest.IO;
using ParserTest.Parser;

namespace ParserTest.Viewer
{
	public class ViewerContext
	{
        public IOutputInterface Output = null;

		public List<DescribedElementInstance> OwnedElements = new List<DescribedElementInstance>();

        public DescribedElementInstance InspectedElement = null;

        public VerbInstance LastVerb = null;

        public List<VerbArgument> PendingArguments = new List<VerbArgument>();

        public ILanguage Language = TextUtils.EnglishLanguage;

        public void Describe(DescribedElementInstance start)
        {
            if (start == null)
            {
                foreach (DescribedElementInstance element in OwnedElements)
                    Language.WriteElement(element, true, Output, false);
            }
            else
                Language.WriteElement(start, true, Output, false);
        }

        public void WriteLine(string line)
        {
            if (Output != null)
                Output.OutputIOLine(line);
        }

        public DescribedElementInstance Get(DescribedElementInstance item, int quantity)
        {
            if (item.Perminant)
                return null;

            if (OwnedElements.Contains(item))
            {
                DescribedElementInstance i = item.Pull(quantity);
                if (i == item)
                    OwnedElements.Remove(i);

                return i;
            }

            return item.Pull(quantity);
        }

        public void Put(DescribedElementInstance item)
        {
            item.Parrent = null;
            OwnedElements.Add(item);
        }
	}
}
