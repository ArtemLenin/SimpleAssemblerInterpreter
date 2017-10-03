using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assambler
{
    class RegexBuilder : IEnumerable
    {
        readonly List<Tuple<string, string>> items = new List<Tuple<string, string>>();

        public void Add(string name, string regex)
        {
            items.Add(Tuple.Create(name, regex));
        }

        public string Build()
        {
            return string.Join("|", items.Select(x => string.Format("(?<{0}>{1})", x.Item1, x.Item2)));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
