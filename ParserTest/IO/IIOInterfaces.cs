using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserTest.IO
{
	public interface IInputInterface
	{
		bool HasInputIO();
		string GetInputIOLine();
	}

	public interface IOutputInterface
	{
		void OutputIOLine(string line);
	}
}
