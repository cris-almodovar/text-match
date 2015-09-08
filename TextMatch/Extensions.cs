using FlexLucene.Document;
using FlexLucene.Index;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextMatch
{
    public static class Extensions
    {
        internal static void SetStringValue(this IndexableField field, string value)
        {
            (field as Field).SetStringValue(value);
        }

        public static IEnumerable<int> FullTextMatch(this IList<string> items, string queryExpression, int? topN = null)
        {
            using (var index = new FullTextIndex())
            {
                index.AddItems(items);              

                return index.Search(queryExpression, topN);
            }
        }
    }
}
