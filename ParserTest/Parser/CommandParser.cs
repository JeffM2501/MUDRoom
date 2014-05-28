using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ParserTest.IO;
using ParserTest.Describables;
using ParserTest.Viewer;

using ParserTest.Language;

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
                    if (TextUtils.Language.IsFiller(lowerElement))
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
                    i++;

                    verb.Instance.RawArguments = ReadToNextVerb(rawElements, ref i);

                    // lets see what this verb wants

                    int argIndex = 0;
                    foreach (ParsedElement.MajorTypes argType in verb.Instance.Action.Arguments)
                    {
                        VerbInstance.ParsedArgument arg = ReadToNextArgument(argType, verb.Instance.RawArguments, ref argIndex, environmentContext, playerContext);
                        if (arg != null)
                            verb.Instance.ParsedArguments.Add(arg);
                    }
                        

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

        private static bool FindPossibleItems(string word, List<DescribedElementInstance> list, ref List<DescribedElementInstance> found)
        {
            foreach(DescribedElementInstance element in list)
            {
                if (word == element.ElementDefintion.Name) // name match
                {
                    found.Clear();
                    found.Add(element);
                    return true;
                }

                if (element.WordDescribesMe(word))
                    found.Add(element);
            }

            return found.Count > 0;
        }

        private static DescribedElementInstance FindBestItem(string word, List<string> adjetives, List<DescribedElementInstance> list)
        {
            if (list.Count == 0)
                return null;

            if (list.Count == 1)
                return list[0];

            int adjetiveCount = -1;
            DescribedElementInstance bestMatch = null;

            foreach (DescribedElementInstance i in list)
            {
                int count = 0;
                foreach(string a in adjetives)
                {
                    if (i.ElementDefintion.AdjetiveDescribesThis(a))
                        count++;
                }

                if (count > adjetiveCount)
                {
                    bestMatch = i;
                    adjetiveCount = count;
                }
            }

            return bestMatch;
        }

        private static bool IsItem(string word, List<string> adjetives, DescriptionContext environment, ViewerContext viewer, ref DescribedElementInstance item, List<DescribedElementInstance> possibles)
        {
            if (!FindPossibleItems(word, environment.Elements, ref possibles) && !FindPossibleItems(word, viewer.OwnedElements, ref possibles))
                return false;

            item = FindBestItem(word, adjetives, possibles);
            return true;
        }

        private static bool IsItemInList(string word, List<string> adjetives, List<DescribedElementInstance> list, ref DescribedElementInstance item, List<DescribedElementInstance> possibles)
        {
           if (!FindPossibleItems(word, list, ref possibles))
                return false;

           item = FindBestItem(word, adjetives, possibles);
            return true;
        }

        private static VerbInstance.ParsedArgument ReadToNextArgument(ParsedElement.MajorTypes argType, List<ParsedElement> arguments, ref int index, DescriptionContext environment, ViewerContext viewer)
        {
            bool done = false;

            List<string> adjetives = new List<string>();

            VerbInstance.ParsedArgument argument = new VerbInstance.ParsedArgument();

            while (!done)
            {
                if (index >= arguments.Count)
                    done = true;
                else
                {
                    argument.Argument = arguments[index];

                    string lowerWord = argument.Argument.Word.ToLower();
                    if (argument.Argument.MajorType != ParsedElement.MajorTypes.Filler)
                    {

                        bool isLoc = ParserTest.Language.TextUtils.Language.IsLocation(lowerWord, ref argument.Location);

                        if (isLoc)  // locations are nouns
                            argument.Argument.MajorType = ParsedElement.MajorTypes.Exit;

                        switch(argType)
                        {
                            case ParsedElement.MajorTypes.Noun:
                                if (isLoc || IsItem(lowerWord, adjetives, environment, viewer, ref argument.Element, argument.PossibleElements))  // locations or items are nouns
                                    return argument;
                                break;

                            case ParsedElement.MajorTypes.AnyItem:
                                if (IsItem(lowerWord, adjetives, environment, viewer, ref argument.Element, argument.PossibleElements))
                                    return argument;
                                break;

                            case ParsedElement.MajorTypes.EnvironmentItem:
                                if (IsItemInList(lowerWord, adjetives, environment.Elements, ref argument.Element, argument.PossibleElements))
                                    return argument;
                                break;

                            case ParsedElement.MajorTypes.PersonalItem:
                                if (IsItemInList(lowerWord, adjetives, viewer.OwnedElements, ref argument.Element, argument.PossibleElements))
                                    return argument;
                                break;

                            case ParsedElement.MajorTypes.Exit:
                                if (isLoc)  // locations are exists
                                    return argument;
                                break;

                            case ParsedElement.MajorTypes.Quanity:
                                if (UInt64.TryParse(argument.Argument.Word,out argument.Value))
                                {
                                    argument.Argument.MajorType = ParsedElement.MajorTypes.Quanity;
                                    return argument;
                                }
                                break;

                            case ParsedElement.MajorTypes.Unknown:
                                return argument; // they want anything
                        }

                        // we didn't find what we wanted, so save this as an adjective
                        adjetives.Add(argument.Argument.Word); 
                    }
                }

                index++;
            }

            return null;
        }
    }
}
