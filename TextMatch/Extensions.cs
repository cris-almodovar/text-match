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
        /// Performs a full-text search match on a single text string, using multiple Lucene query expressions.
        /// </summary>
        /// <param name="text">The text to be searched/matched.</param>
        /// <param name="queryExpressions">The list of Lucene query expressions.</param>
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
        /// Performs a full-text search match on a list of texts, using multiple Lucene query expressions.
        /// </summary>
        /// <param name="texts">The text to be searched/matched.</param>
        /// <param name="queryExpressions">The list of Lucene query expressions.</param>
        /// <param name="topN">The limit on the number of matching query expressions to return, for each text.</param>
        /// <param name="cacheQuery">if set to <c>true</c> the query expressions are cached.
        /// This can increase performance if the query expressions are complex and/or the number
        /// of query expressions is large, and the query expressions are repeatedly used 
        /// (i.e. in a loop) to search/match the texts.</param>
        /// <returns>
        /// A list of MatchResult objects containing the status of the operation for each text.
        /// </returns>
        public static List<MatchResult> Match(this IList<string> texts, IList<string> queryExpressions, int? topN = null, bool cacheQuery = true)
        {
            using (var index = new FullTextIndex())
            {
                topN = topN ?? queryExpressions.Count;
                var matches = new List<MatchResult>();                

                for (var textId = 0; textId < texts.Count; textId++)
                {
                    // Process the current text; match it against each quey expression.

                    var text = texts[textId];
                    index.Add(text);                    

                    var matchingExpressions = new List<int>();
                    for (var queryId = 0; queryId < queryExpressions.Count; queryId++)
                    {
                        var query = queryExpressions[queryId];

                        if (index.Search(query, cacheQuery: cacheQuery).Count() > 0)                        
                            matchingExpressions.Add(queryId);

                        if (matchingExpressions.Count > topN)
                            break;   
                    }
                    
                    matches.Add(new MatchResult(matchingExpressions));

                    index.Clear(!cacheQuery); // Do not clear the query cache, unless we are not caching query expressions
                }

                return matches;
            }
        }
    }
}
