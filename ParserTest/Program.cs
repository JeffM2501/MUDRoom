using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using ParserTest.Parser;
using ParserTest.Describables;
using ParserTest.Viewer;
using ParserTest.IO;

using ParserTest.Language;

namespace ParserTest
{
    class Program
    {
        public static bool Quit = false;

        static void Main(string[] args)
        {
			SampleWorld.Define();
            CommonVerbs.SetupLanugage();

            ConsoleIO console = new ConsoleIO();
            CommandParser.Input = console;
            CommandParser.Output = console;
            CommandParser.Verbs.Add(new QuitVerb());

            DescriptionContext environment = new DescriptionContext();
            ViewerContext player = new ViewerContext();

            console.OutputIOLine("Parser Startup");

            while (!Quit)
            {
                if (console.HasInputIO())
                    CommandParser.ProcessActions(CommandParser.Parse(console.GetInputIOLine(), environment, player));

                Thread.Sleep(10);
            }

            console.Dispose();
        }

        public class QuitVerb : Verb
        {
            public QuitVerb()
            {
                Words.Add("quit");
                Words.Add("exit");
            }

            public override void Act(VerbInstance instance)
            {
                Program.Quit = true;
            }
        }
    }

	static class SampleWorld
	{
		public static void Define()
		{
			BuildElements();
		}

		public static void BuildElements()
		{

		}
	}
}
