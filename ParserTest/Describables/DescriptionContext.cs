using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ParserTest.Parser;
using ParserTest.IO;
using ParserTest.Language;
using ParserTest.Viewer;

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

        public DescribedElementInstance Parrent = null;

        public static DescribedElementInstance Empty = new DescribedElementInstance();

        public bool Perminant = false;

        public DescribedElementInstance()
        { 
        }

        public DescribedElementInstance(DescribedElementInstance o)
        {
            Location = o.Location;
            Quanity = 1;
            Parrent = o.Parrent;
            if (Parrent != null)
                Parrent.Children.Add(this);
            ElementDefintion = o.ElementDefintion;
        }

    
        public void Put(DescribedElementInstance item)
        {
            item.Parrent = this;
            Children.Add(item);
        }

        public DescribedElementInstance Pull( int quantity )
        {
            if (Perminant)
                return null;

            if (quantity == -1 || quantity >= Quanity || Parrent == null || Children.Count > 0 || ElementDefintion.Container) // taking the entire thing
            {
                if (Parrent != null)
                    Parrent.Children.Remove(this);
                Parrent = null;
                return this;
            }

            // pulling off a partial
            DescribedElementInstance e = new DescribedElementInstance(this);
            e.Quanity = quantity;
            Quanity -= quantity;

            return e.Pull(quantity);
        }
    }

    public class DescriptionContext
    {
        public List<DescribedElementInstance> Elements = new List<DescribedElementInstance>();
		public string ContextDescriptor = string.Empty;

        public void Describe(DescribedElementInstance start, ViewerContext player)
        {
            if (start == null)
            {
                Write(ContextDescriptor, player.Output);
                foreach (DescribedElementInstance element in Elements)
                    player.Language.WriteElement(element, true, player.Output, true);
            }
            else
                player.Language.WriteElement(start, true, player.Output, true);
        }

		private void Write (string line, IOutputInterface output)
		{
			if(output != null && line != string.Empty)
				output.OutputIOLine(line);
		}

        public DescribedElementInstance Get(DescribedElementInstance item, int quantity)
        {
            if (item.Perminant)
                return null;

            if (Elements.Contains(item))
            {
                DescribedElementInstance i = item.Pull(quantity);
                if (i == item)
                    Elements.Remove(i);

                return i;
            }

            return item.Pull(quantity);

        }

        public void Put(DescribedElementInstance item)
        {
            item.Parrent = null;
            Elements.Add(item);
        }
    }

    public class DescribedElementCache
    {
        public static List<DescribedElementDefintion> Cache = new List<DescribedElementDefintion>();

        public static DescribedElementInstance New(string elementDefName)
        {
            return New(elementDefName, 1, DescribedElementInstance.ElementLocations.Middle, null);
        }

        public static DescribedElementInstance New(string elementDefName, int quantity)
        {
            return New(elementDefName, quantity, DescribedElementInstance.ElementLocations.Middle, null);
        }

        public static DescribedElementInstance New(string elementDefName, int quantity, DescribedElementInstance.ElementLocations location)
        {
            return New(elementDefName, quantity, location, null);
        }

        public static DescribedElementInstance New(string elementDefName, DescribedElementInstance.ElementLocations location)
        {
            return New(elementDefName, 1, location, null);
        }

        public static DescribedElementInstance New(string elementDefName, DescribedElementInstance.ElementLocations location, DescribedElementInstance parrent)
        {
            return New(elementDefName, 1, location, parrent);
        }

        public static DescribedElementInstance New(string elementDefName, int quantity, DescribedElementInstance parrent)
        {
            return New(elementDefName, quantity, DescribedElementInstance.ElementLocations.Middle, parrent);
        }

        public static DescribedElementInstance New(string elementDefName, int quantity, DescribedElementInstance.ElementLocations location, DescribedElementInstance parrent)
        {
			string lowerName = elementDefName.ToLowerInvariant();
			DescribedElementDefintion def = Cache.Find(delegate(DescribedElementDefintion d){return (d.Name == lowerName);});

			if(def == null)
				def = new DescribedElementDefintion(elementDefName);

			DescribedElementInstance inst = new DescribedElementInstance();
			inst.ElementDefintion = def;

            inst.Quanity = quantity;
            inst.Location = location;

            if (parrent != null)
                parrent.Children.Add(inst);

			return inst;
		}
    }
}
