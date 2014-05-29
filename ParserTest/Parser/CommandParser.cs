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
    public class CommandParser
    {
        public static IInputInterface Input;
        public static IOutputInterface Output;

        public static List<Verb> Verbs = new List<Verb>();

        public class ParserResults
        {
            public List<VerbInstance> Comands = new List<VerbInstance>();
            public string RawLine = string.Empty;
        }

        public static string ReadWord(string text, ref int location)
        {
            int cur = location;

            while (cur < text.Length)
            {
                if (Char.IsWhiteSpace(text[cur]) || Char.IsPunctuation(text[cur]))
                    break;

                cur++;
            }

            int start = location;
            location = cur;
            return text.Substring(start, location - start);
        }

        public static ParserResults Parse(string line, DescriptionContext environmentContext, ViewerContext playerContext)
        {
            VerbInstance lastVerb = playerContext.LastVerb;
            playerContext.LastVerb = null;

            ParserResults results = new ParserResults();
            results.RawLine = line.Trim();

            if (results.RawLine == string.Empty)
                return results;

            int count = 0;

            int lastEnd = 0;

            while (count < line.Length)
            {
                int saveCount = count;
                string word = ReadWord(line, ref count).Trim().ToLowerInvariant();

                if (word != string.Empty && !playerContext.Language.IsFiller(word) || !playerContext.Language.IsConnector(word))
                {
                    VerbInstance verb = new VerbInstance();
                    verb.Action = FindVerb(word);
                    verb.Text = word;

                    if (verb.Action == null)
                    {
                        if (lastVerb != null)
                        {
                            if (!playerContext.Language.IsConnector(word))
                                count = saveCount-1;  // use the last verb and parse this like arguments
                            verb.Action = lastVerb.Action;
                            verb.Text = lastVerb.Text;
                        }
                        else
                            continue;
                    }
                    else
                        lastVerb = verb;    // so that connectors
        
  
                    verb.PlayerContext = playerContext;
                    verb.EnvironmnetContext = environmentContext;

                    if (lastEnd != saveCount)
                        verb.PrefixText = line.Substring(lastEnd, saveCount - lastEnd);

                    lastEnd = count;

                    count += 1;
                    bool done = false;
                    while (!done && verb.Action.Arguments.Count > 0)
                    {
                        List<string> adjetives = new List<string>();

                        for (int a = 0; a < verb.Action.Arguments.Count; a++)
                        {
                            if (verb.Action.Arguments[a] == VerbArgumentTypes.Sentance) // easy out if they want the entire line
                            {
                                VerbArgument arg = new VerbArgument(VerbArgumentTypes.Sentance,line.Substring(count));
                                results.Comands.Add(verb);
                                return results;
                            }

                            if (count >= line.Length)
                            { 
                                done = true;
                                break;
                            }
                            int c = count;

                            bool foundArg = false;

                            while (!foundArg)
                            {
                                string argWord = ReadWord(line, ref count).Trim().ToLowerInvariant();
                                lastEnd = count;
                                count++;

                                if (argWord == string.Empty)
                                    continue;

                                VerbArgument arg = GetArgForWord(verb.Action.Arguments[a],argWord,adjetives,environmentContext,playerContext);

                                if (arg == null)
                                {
                                    if (playerContext.Language.IsConnector(argWord))
                                    {
                                        foundArg = true;
                                        done = true;
                                        a = verb.Action.Arguments.Count;
                                        break;
                                    }
                                    else
                                    {
                                        if (!playerContext.Language.IsFiller(argWord))
                                            adjetives.Add(argWord);
                                    }
                                }
                                else
                                {
                                    verb.Arguments.Add(arg);
                                    foundArg = true;
                                }

                                if (count >= line.Length || verb.Arguments.Count == verb.Action.Arguments.Count)
                                {
                                    done = true;
                                    break;
                                }
                            }

                            if (done)
                                break;
                        }
                    }

                    results.Comands.Add(verb);
                }
            }
            return results;
        }

        public static bool ProcessActions(ParserResults results)
        {
            bool allGood = true;

            foreach (VerbInstance verb in results.Comands)
            {
                if (verb.Action == null)
                {
                    WriteUnknownError(verb.Text);
                    allGood = false;
                }
                else
                {
                    if (!verb.Act())
                        verb.PlayerContext.LastVerb = verb;
                }
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

        public static bool FindPossibleItems(string word, List<DescribedElementInstance> list, ref List<DescribedElementInstance> found)
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

        public static DescribedElementInstance FindBestItem(string word, List<string> adjetives, List<DescribedElementInstance> list)
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

        public static bool IsItem(string word, List<string> adjetives, DescriptionContext environment, ViewerContext viewer, ref DescribedElementInstance item, List<DescribedElementInstance> possibles)
        {
            if (!FindPossibleItems(word, environment.Elements, ref possibles) && !FindPossibleItems(word, viewer.OwnedElements, ref possibles))
                return false;

            item = FindBestItem(word, adjetives, possibles);
            return true;
        }

        public static bool IsItemInList(string word, List<string> adjetives, List<DescribedElementInstance> list, ref DescribedElementInstance item, List<DescribedElementInstance> possibles)
        {
           if (!FindPossibleItems(word, list, ref possibles))
                return false;

           item = FindBestItem(word, adjetives, possibles);
            return true;
        }

        public static VerbArgument GetArgForWord(VerbArgumentTypes expectedType, string word, List<string> adjetives, DescriptionContext environment, ViewerContext viewer)
        {
            switch (expectedType)
            {
                case VerbArgumentTypes.Noun:
                    {
                        DescribedElementInstance.ElementLocations loc = DescribedElementInstance.ElementLocations.Middle;
                        if (ParserTest.Language.TextUtils.Language.IsLocation(word, ref loc))
                            return new VerbLocationArgument(word, loc);

                        VerbElementArgument e = new VerbElementArgument(word);
                        if (IsItem(word, adjetives, environment, viewer, ref e.Element, e.PossibleElements))  // locations or items are nouns
                            return e;     
                    }
                    break;

                case VerbArgumentTypes.AnyItem:
                    {
                        VerbElementArgument e = new VerbElementArgument(word);
                        if (IsItem(word, adjetives, environment, viewer, ref e.Element, e.PossibleElements))  // locations or items are nouns
                            return e;     
                    }
                    break;

                case VerbArgumentTypes.EnvironmentItem:
                    {
                        VerbElementArgument e = new VerbElementArgument(word);
                        if (IsItemInList(word, adjetives, environment.Elements, ref e.Element, e.PossibleElements))  // locations or items are nouns
                            return e;
                    }
                    break;

                case VerbArgumentTypes.PersonalItem:
                    {
                        VerbElementArgument e = new VerbElementArgument(word);
                        if (IsItemInList(word, adjetives, viewer.OwnedElements, ref e.Element, e.PossibleElements))  // locations or items are nouns
                            return e;
                    }
                    break;

                case VerbArgumentTypes.Adjetive:
                    {
                        DescribedElementInstance.ElementLocations loc = DescribedElementInstance.ElementLocations.Middle;
                        if (TextUtils.Language.IsFiller(word) || ParserTest.Language.TextUtils.Language.IsLocation(word, ref loc))
                            return null;

                        VerbElementArgument e = new VerbElementArgument(word);
                        if (IsItem(word, adjetives, environment, viewer, ref e.Element, e.PossibleElements))  // locations or items are nouns
                            return null;

                        return new VerbArgument(VerbArgumentTypes.Adjetive, word);
                    }

                case VerbArgumentTypes.Filler:
                     if (TextUtils.Language.IsFiller(word))
                         new VerbArgument(VerbArgumentTypes.Filler, word);
                    break;

                case VerbArgumentTypes.Exit:
                    {
                        DescribedElementInstance.ElementLocations loc = DescribedElementInstance.ElementLocations.Middle;
                        if (ParserTest.Language.TextUtils.Language.IsLocation(word, ref loc))
                            return new VerbLocationArgument(word, loc);
                    }
                    break;

                case VerbArgumentTypes.Quanity:
                    {
                        UInt64 q = 0;
                        if (UInt64.TryParse(word, out q))
                            return new VerbValueArgument(word, q);
                    }
                    break;

                case VerbArgumentTypes.Anything:
                    return new VerbArgument(VerbArgumentTypes.Anything, word);
            }
            return null;
        }
    }
}
