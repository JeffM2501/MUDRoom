using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ParserTest.IO;
using ParserTest.Describables;
using ParserTest.Viewer;

namespace ParserTest.Parser
{
    public class VerbElement : ParsedElement
    {
        public VerbInstance Instance = new VerbInstance();

        public VerbElement(Verb action)
        {
            Instance.Action = action;
            MajorType = MajorTypes.Verb;
        }
    }

    public class CommandParser
    {
        public static IInputInterface Input;
        public static IOutputInterface Output;

        public static List<Verb> Verbs = new List<Verb>();
        public static List<string> Fillers = new List<string>();

        public static List<ParsedElement> Parse(string line, DescriptionContext environmentContext, ViewerContext playerContext)
        {
            List<ParsedElement> rawElements = new List<ParsedElement>();
            // see if we can figure out what each of these words are and filter the empty ones

            foreach (string rawElement in line.Split(" ,; ".ToCharArray()))
            {
                string lowerElement = rawElement.ToLowerInvariant();

                ParsedElement element = new ParsedElement();
                element.Word = rawElement;

                // see if it's a verb
                Verb action = FindVerb(lowerElement);

                if (action != null)
                {
                    element = new VerbElement(action);
                }
                else
                {
                    if (Fillers.Contains(lowerElement))
                        element.MajorType = ParsedElement.MajorTypes.Filler;
                    else
                        element.MajorType = ParsedElement.MajorTypes.Unknown;
                }

                rawElements.Add(element);
            }

            // now build this into a verb tree

            List<ParsedElement> ElementTree = new List<ParsedElement>();

            for (int i = 0; i < rawElements.Count; i++)
            {
                ParsedElement word = rawElements[i];
                if (word.MajorType == ParsedElement.MajorTypes.Verb)
                {
                    VerbElement verb = word as VerbElement;
                    verb.Instance.EnvironmnetContext = environmentContext;
                    verb.Instance.PlayerContext = playerContext;
                    verb.Instance.Word = verb.Word;

                    verb.Instance.RawArguments = ReadToNextVerb(rawElements, ref i);

                    // lets see what this verb wants

                    // add the top verb to the tree
                    ElementTree.Add(verb);
                }
                else
                {
                    ElementTree.Add(word);
                    i++;
                }
            }

            return ElementTree;
        }

        public static bool ProcessActions(List<ParsedElement> elemnts)
        {
            bool allGood = true;

            foreach(ParsedElement element in elemnts)
            {
                VerbElement verb = element as VerbElement;
                if (verb == null)
                {
                    WriteUnknownError(element.Word);
                    allGood = false;
                }
                else
                    verb.Instance.Act();
            }

            return allGood;
        }

        private static void WriteUnknownError(string word)
        {
            Write("I don't know how to " + word);
        }

        private static void Write(string line)
        {
            if (Output != null)
                Output.OutputIOLine(line);
        }

        public static Verb FindVerb(string word)
        {
            return Verbs.Find(delegate(Verb v) { return v.HasString(word); });
        }

        private static List<ParsedElement> ReadToNextVerb(List<ParsedElement> elements, ref int index)
        {
            List<ParsedElement> things = new List<ParsedElement>();

            while (index < elements.Count)
            {
                if (elements[index].MajorType == ParsedElement.MajorTypes.Verb)
                    break;

                if (elements[index].MajorType != ParsedElement.MajorTypes.Filler)
                    things.Add(elements[index]);

                index++;
            }

            return things;
        }
    }
}
