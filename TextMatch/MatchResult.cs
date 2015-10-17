using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextMatch
{
    [Serializable]
    public class MatchResult : IEnumerable<int>
    {
        private IList<int> _items;
        public bool Success { get; private set; }

        public MatchResult(IList<int> items)
        {
            if (items != null)
                _items = items;
            else
                _items = new List<int>();

            Success = _items.Count > 0;               
        }
        
        public int this[int i]
        {
            get { return _items[i]; }
        }        

        public IEnumerator<int> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }

    [Serializable]
    public class MultiMatchResult : IEnumerable<IList<int>> 
    {
        private IDictionary<int, IList<int>> _items;
        public bool Success { get; private set; }

        public MultiMatchResult(IDictionary<int, IList<int>> items)
        {
            if (items != null)
                _items = items;
            else
                _items = new Dictionary<int, IList<int>>();

            Success = _items.Values.Any(list => list.Count > 0);
        }

        public IList<int> this[int i]
        {
            get {  return _items.ContainsKey(i) ? _items[i] : null; }
        }

        public IEnumerator<IList<int>> GetEnumerator()
        {
            return _items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.Values.GetEnumerator();
        }
    }
}
