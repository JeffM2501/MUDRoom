using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ParserTest.IO
{
	public class ConsoleIO: IInputInterface, IOutputInterface
	{
		public List<string> PendingInputLines = new List<string>();
		
		public virtual bool HasInputIO()
		{
			return PendingInputLines.Count > 0;
		}
	}
}
