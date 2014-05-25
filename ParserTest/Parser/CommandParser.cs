using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserTest.Parser
{
    public class VerbElement : ParsedElement
    {
        public Verb Action = null;

        public List<ParsedElement> RawArguments = new List<ParsedElement>();

        public VerbElement(Verb action)
        {
            Action = action;
            MajorType = MajorTypes.Verb;
        }
    }

    public class CommandParser
    {
        public static List<Verb> Verbs = new List<Verb>();
        public static List<string> Fillers = new List<string>();

        public static List<ParsedElement> Parse(string line)
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
                ParsedElement verb = rawElements[i];
                if (verb.MajorType == ParsedElement.MajorTypes.Verb)
                {
                    List<ParsedElement> verbArgs = ReadToNextVerb(rawElements, ref i);

                    // lets see what this verb wants
                }
                else
                    i++;
            }


            return ElementTree;
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
