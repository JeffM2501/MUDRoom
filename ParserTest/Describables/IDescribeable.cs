using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserTest.Describables
{
    public interface IDescribeable
    {
        public DescribedElementInstance GetDescriptionInstance();
    }
}
