﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using ParserTest.Parser;
using ParserTest.Parser.Actions;
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
            SampleWorld world= new SampleWorld();
            world.Define();

            CommonVerbs.Setup();

            ConsoleIO console = new ConsoleIO();
            CommandParser.Input = console;
            CommandParser.Output = console;
            CommandParser.Verbs.Add(new QuitVerb());
          
            console.OutputIOLine("Parser Startup");

            world.Environment.Describe(null);

            while (!Quit)
            {
                if (console.HasInputIO())
                    CommandParser.ProcessActions(CommandParser.Parse(console.GetInputIOLine(), world.Environment, world.Player));

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

	class SampleWorld
	{
        public DescriptionContext Environment = new DescriptionContext();
        public ViewerContext Player = new ViewerContext();

		public void Define()
		{
			BuildElements();
            PopulateRoom();
		}

		public void BuildElements()
		{
            DescribedElementDefintion simpleSword = new DescribedElementDefintion("PlainIronSword");
            simpleSword.ElementType = "sword";
            simpleSword.Adjetives.Add("plain");
            simpleSword.Adjetives.Add("iron");
            DescribedElementCache.Cache.Add(simpleSword);

            DescribedElementDefintion blueSword = new DescribedElementDefintion("BlueSword");
            blueSword.ElementType = "sword";
            blueSword.Adjetives.Add("shinny");
            blueSword.Adjetives.Add("blue");
            blueSword.Adjetives.Add("steel");
            DescribedElementCache.Cache.Add(blueSword);

            DescribedElementDefintion chest = new DescribedElementDefintion("FineChest");
            chest.ElementType = "chest";
            chest.Adjetives.Add("fine");
            chest.Adjetives.Add("wooden");
            DescribedElementCache.Cache.Add(chest);
		}

        public void PopulateRoom()
        {
            Environment.ContextDescriptor = "A simple room used for testing";

            Environment.Elements.Add(DescribedElementCache.New("PlainIronSword",DescribedElementInstance.ElementLocations.North));
            Environment.Elements.Add(DescribedElementCache.New("FineChest", DescribedElementInstance.ElementLocations.West));
            Environment.Elements.Add(DescribedElementCache.New("BlueSword", 2, DescribedElementInstance.ElementLocations.East));
        }
	}
}
