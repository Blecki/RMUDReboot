using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMUD
{
    public class Noun
    {
        public List<String> Value;
        public Func<MudObject, bool> Available;

        public Noun(List<String> Value)
        {
            this.Value = Value;
            this.Available = null;
        }

        public Noun When(Func<MudObject, bool> AvailabilityFunction)
        {
            if (Available != null)
            {
                var lambdaAvailable = Available;
                Available = (obj) =>
                {
                    return lambdaAvailable(obj) && AvailabilityFunction(obj);
                };
            }
            else
                Available = AvailabilityFunction;

            return this;
        }
    }

    public class NounList
    {
        List<Noun> Nouns = new List<Noun>();

        public NounList() { }

        public Noun Add(IEnumerable<String> Range)
        {
            var r = new Noun(new List<String>(Range.Select(s => s.ToUpper())));
            Nouns.Add(r);
            return r;
        }

        public bool Match(String Word, MudObject Actor)
        {
            foreach (var noun in Nouns)
            {
                if (!noun.Value.Contains(Word))
                    continue;
                if (noun.Available != null)
                    return noun.Available(Actor);
                return true;
            }
            return false;
        }

        public void Remove(String Word)
        {
            Nouns.RemoveAll(n => n.Value.Contains(Word));
        }
    }

    public partial class MudObject
    {
        public Noun AddNoun(IEnumerable<String> Noun)
        {
            return GetProperty<NounList>("nouns").Add(Noun);
        }

        public Noun AddNoun(params String[] Noun)
        {
            return GetProperty<NounList>("nouns").Add(Noun);
        }
    }
}
