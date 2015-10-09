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
        /// <returns>
        /// A MatchResult object containing the status of the operation, and if successful,
        /// a list of numbers representing the index number of the text  that matched the query expression.
        /// </returns>
        public static MatchResult Match(this IList<string> texts, string queryExpression, int? topN = null)
        {
            using (var index = new FullTextIndex())
            {
                index.Add(texts);                  
                var docIds = index.Search(queryExpression, (topN ?? texts.Count)).ToList();
                return new MatchResult(docIds);
            }
        }

        /// <summary>
        /// Performs a full-text search on the single text item.
        /// </summary>
        /// <param name="text">The text to be searched.</param>
        /// <param name="queryExpressions">The Lucene query expressions.</param>
        /// <param name="topN">The top N records to be returned</param>
        /// <returns>
        /// A MatchResult object containing the status of the operation, and if successful,
        /// a list of numbers representing the index number of the query expression that matched the text.
        /// </returns>
        public static MatchResult Match(this string text, IList<string> queryExpressions, int? topN = null)
        {
            using (var index = new FullTextIndex())
            {
                index.Add(text);
                
                var topM = queryExpressions.Count;
                var matchingExpressions = new List<int>();

                for (var i = 0; i < queryExpressions.Count; i++)
                {
                    var query = queryExpressions[i];

                    if (index.Search(query).Count() > 0)
                    {
                        matchingExpressions.Add(i);
                        if (matchingExpressions.Count >= topM)
                            break;                       
                    }                   
                }

                return new MatchResult(matchingExpressions);                              
            }
        }
    }
}
