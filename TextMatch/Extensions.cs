using System.Collections.Generic;
using System.Linq;

namespace TextMatch
{
    public static class Extensions
    {
        /// <summary>
        /// Performs a full-text search/match on the given list of strings.
        /// </summary>
        /// <param name="texts">The list of texts to be searched/matched.</param>
        /// <param name="queryExpression">The Lucene query expression.</param>
        /// <param name="topN">The limit on the number of matching texts to return.</param>
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
        /// Performs a full-text search match on a single text string, using multiple query expressions.
        /// </summary>
        /// <param name="text">The text to be searched/matched.</param>
        /// <param name="queryExpressions">A list of Lucene query expressions.</param>
        /// <param name="topN">The limit on the number of matching query expressions to return.</param>        
        /// <returns>
        /// A MatchResult object containing the status of the operation, and if successful,
        /// a list of numbers representing the index number of the query expression that matched the text.
        /// </returns>
        public static MatchResult Match(this string text, IList<string> queryExpressions, int? topN = null)
        {
            using (var index = new FullTextIndex())
            {
                index.Add(text);
                
                topN = topN ?? queryExpressions.Count;
                var matchingExpressions = new List<int>();

                for (var i = 0; i < queryExpressions.Count; i++)
                {
                    var query = queryExpressions[i];

                    if (index.Search(query).Count() > 0)
                    {
                        matchingExpressions.Add(i);
                        if (matchingExpressions.Count >= topN)
                            break;                       
                    }                   
                }

                return new MatchResult(matchingExpressions);                              
            }
        }

        /// <summary>
        /// Performs a full-text search match on a list of texts, using multiple query expressions.
        /// </summary>
        /// <param name="texts">The texts.</param>
        /// <param name="queryExpressions">The list of Lucene query expressions.</param>
        /// <param name="topN">The limit on the number of matching texts to return.</param>
        /// <param name="cacheQuery">if set to <c>true</c> the query expressions are cached.
        /// This can increase performance if the query expressions are complex and/or the number
        /// of query expressions is large, and the query expressions are repeatedly used 
        /// (i.e. in a loop) to search/match the texts.</param>
        /// <returns>
        /// A MultiMatchResult object containing the status of the operation, and if successful,
        /// a list where the index represents the text number and the contents is a list
        /// of  query expressions that matched the text.
        /// </returns>
        public static MultiMatchResult Match(this IList<string> texts, IList<string> queryExpressions, int? topN = null, bool cacheQuery = false)
        {
            using (var index = new FullTextIndex())
            {
                topN = topN ?? texts.Count;
                var matches = new Dictionary<int, IList<int>>();
                var matchesCount = 0;

                for (var textId = 0; textId < texts.Count; textId++)
                {
                    var text = texts[textId];
                    index.Add(text);

                    var matchingExpressions = new List<int>();

                    for (var queryId = 0; queryId < queryExpressions.Count; queryId++)
                    {
                        var query = queryExpressions[queryId];

                        if (index.Search(query, cacheQuery: cacheQuery).Count() > 0)                        
                            matchingExpressions.Add(queryId);   
                    }                    
                    
                    matches[textId] = matchingExpressions;

                    if (matchingExpressions.Count > 0)
                        matchesCount += 1;

                    if (matchesCount >= topN)
                        break;

                    index.Clear(!cacheQuery);
                }

                return new MultiMatchResult(matches);
            }
        }
    }
}
