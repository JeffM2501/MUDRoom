using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ParserTest.Parser;
using ParserTest.IO;
using ParserTest.Language;

namespace ParserTest.Describables
{
    public class DescribedElementInstance
    {
        public enum ElementLocations
        {
            North,
            South,
            East,
            West,
            Up,
            Down,
            Middle,
        }

        public ElementLocations Location = ElementLocations.Middle;

        public DescribedElementDefintion ElementDefintion;
        public int Quanity = 1;

		public bool DescribeChildren = false;
        public List<DescribedElementInstance> Children = new List<DescribedElementInstance>();

        public static DescribedElementInstance Empty = new DescribedElementInstance();
    }

    public class DescriptionContext
    {
        public List<DescribedElementInstance> Elements = new List<DescribedElementInstance>();
		public string ContextDescriptor = string.Empty;

		public void Describe(DescribedElementInstance start, IOutputInterface output)
		{
			if(start == null)
			{
				Write(ContextDescriptor, output);
			}
			else
				TextUtils.English.WriteElement(start, true, output);
		}

		public void Describe(DescribedElementInstance start)
		{
			Describe(start,CommandParser.Output);
		}

		private void Write (string line, IOutputInterface output)
		{
			if(output != null && line != string.Empty)
				output.OutputIOLine(line);
		}
    }

    public class DescribedElementCache
    {
        public static List<DescribedElementDefintion> Cache = new List<DescribedElementDefintion>();

        public static DescribedElementInstance New(string elementDefName)
        {
			string lowerName = elementDefName.ToLowerInvariant();
			DescribedElementDefintion def = Cache.Find(delegate(DescribedElementDefintion d){return (d.Name == lowerName);});

			if(def == null)
				def = new DescribedElementDefintion(elementDefName);

			DescribedElementInstance inst = new DescribedElementInstance();
			inst.ElementDefintion = def;

			inst.Quanity = 1;
			return inst;
		}
    }
}
