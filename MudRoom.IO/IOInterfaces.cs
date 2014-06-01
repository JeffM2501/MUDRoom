using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudRoom.IO
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
}
