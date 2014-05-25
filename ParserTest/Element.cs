using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserTest
{
    public class ElementInstance : IDescribeable
    {
        public static UInt64 InvalidID = UInt64.MinValue;

        public UInt64 ID = InvalidID;

        public virtual DescribedElementInstance GetDescriptionInstance()
        {
            return DescribedElementInstance.Empty;
        }
    }
}
