using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ParserTest.Describables;
using ParserTest.Parser;

namespace ParserTest
{
    public class Room : ElementInstance
    {
        public string Name = string.Empty;
        public String Description = string.Empty;

        public class ElementInRoom
        {
            public DescribedElementInstance.ElementLocations Location = DescribedElementInstance.ElementLocations.Middle;
            public ElementInstance Element;
        }

        public List<ElementInRoom> Elements = new List<ElementInRoom>();
    }
}
