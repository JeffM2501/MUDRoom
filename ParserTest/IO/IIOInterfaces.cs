using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserTest.IO
{
	interface IInputInterface
	{
		bool HasInputIO();
		string GetInputIOLine();
	}

	interface IOutputInterface
	{
		void OutputIOLine(string line);
	}
}
