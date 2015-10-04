using FlexLucene.Document;
using FlexLucene.Index;
using System;
using System.Collections.Concurrent;
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

        /// <summary>
        /// Performs a full-text search on the given list of strings.
        /// </summary>
        /// <param name="texts">The list of texts to be searched.</param>
        /// <param name="queryExpression">The Lucene query expression.</param>
        /// <param name="topN">The top N records to be returned</param>
        /// <returns>A list of document numbers, containing the items that satisfy the query expression.</returns>
        public static IList<int> FullTextMatch(this IList<string> texts, string queryExpression, int? topN = null)
        {
            // TODO: Use PLINQ if texts.Count > n

            using (var index = new FullTextIndex())
            {
                index.Add(texts);              

                return index.Search(queryExpression, topN).ToList();
            }
        }
    }
}
