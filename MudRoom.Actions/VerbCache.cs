using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using MudRoom.Actions.Verbs;

namespace MudRoom.Actions
{
    public static class VerbCache
    {
        private static List<Type> RegisteredVerbs = new List<Type>();

        public static event EventHandler VerbAdded;

        public static void RegisterVerb(Type t)
        {
            lock(RegisteredVerbs)
            {
                if (t.IsInterface || !IsVerb(t) || RegisteredVerbs.Contains(t))
                    return;

                RegisteredVerbs.Add(t);
            }

            if (VerbAdded != null)
                VerbAdded(null,EventArgs.Empty);
        }

        public static List<IVerb> GetActiveVerbs()
        {
            List<IVerb> verbs = new List<IVerb>();
            lock (RegisteredVerbs)
            {
                foreach (Type t in RegisteredVerbs)
                    verbs.Add(Activator.CreateInstance(t) as IVerb);
            }

            return verbs;
        }

        private static bool IsVerb(Type t)
        {
            foreach(Type i in t.GetInterfaces())
            {
                if (i == typeof(IVerb))
                    return true;
            }

            return false;
        }
    }
}
