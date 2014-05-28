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

        public void Describe(DescribedElementInstance start)
        {
            if (start == null)
            {
                foreach (DescribedElementInstance element in OwnedElements)
                    TextUtils.Language.WriteElement(element, true, Output);
            }
            else
                TextUtils.Language.WriteElement(start, true, Output);
        }

        public void WriteLine(string line)
        {
            if (Output != null)
                Output.OutputIOLine(line);
        }
	}
}
