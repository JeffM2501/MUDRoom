using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ParserTest.Language;

namespace ParserTest.Describables
{
    public class DescribedElementDefintion
    {
        public string Name = string.Empty;
        public string Description = string.Empty;

        public string ElementType = string.Empty;
        public List<string> Adjetives = new List<string>();

        private List<string> AdjetiveCache = new List<string>();

        public enum ElementTypes
        {
            Item,
            Container,
            Exit,
            Feature,
        }

        public bool Container = false;

        public bool SingleIsPair = false; // PANTS!

        public bool AdjetiveDescribesThis(string adj)
        {
            if (Adjetives.Count == 0)
                return false;

            if (Adjetives.Count != AdjetiveCache.Count)
            {
                AdjetiveCache.Clear();
                foreach (string a in Adjetives)
                    AdjetiveCache.Add(a.ToLower());
            }

            return AdjetiveCache.Contains(adj);
        }

        public DescribedElementDefintion() { }
        public DescribedElementDefintion(string name)
        {
            Name = name.ToLowerInvariant();
        }

        public static DescribedElementDefintion Empty = new DescribedElementDefintion();
    }
}
