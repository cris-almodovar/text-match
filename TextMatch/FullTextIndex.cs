using System;
using System.Collections.Generic;
using Lucene.Net.Analysis;
using Lucene.Net.Contrib.Management;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using LuceneVersion = Lucene.Net.Util.Version;

namespace TextMatch
{
    /// <summary>
    /// Encapsulates a Lucene in-memory full-text index. 
    /// </summary>    
    public class FullTextIndex : IDisposable
    {
        private Directory _directory;
        private Analyzer _analyzer;
        private IndexWriter _writer;            
        private QueryParser _queryParser; 
        private SearcherManager _searcherManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="FullTextIndex" /> class.
        /// </summary>
        /// <param name="enableStemming">if set to <c>true</c>, the FullTextIndex will stem 
        /// the tokens that make up the texts, using the Porter stemming algorithm.</param>
        /// <param name="ignoreCase">if set to <c>true</c>, character casing is ignored.</param>
        /// <param name="separatorChars">A string whose component characters will be used to split the texts into tokens.</param>
        public FullTextIndex(bool enableStemming = true, bool ignoreCase = true, string separatorChars = CustomTokenizer.DEFAULT_SEPARATOR_CHARS)
        {
            _directory = new RAMDirectory();
            _analyzer = new CustomAnalyzer(enableStemming, ignoreCase, separatorChars); 
            
            _writer = new IndexWriter(_directory, _analyzer, IndexWriter.MaxFieldLength.UNLIMITED);            
            _searcherManager = new SearcherManager(_writer);

            _queryParser = new QueryParser(LuceneVersion.LUCENE_30, "text", _analyzer); 
        }

        /// <summary>
        /// Adds texts to the index.
        /// </summary>
        /// <param name="texts">The texts to add.</param>
        public void Add(IList<string> texts)
        {            
            for (var index = 0; index < texts.Count; index++)
            {
                var document = CreateDocument();
                document.GetField("id").SetValue(index.ToString());
                document.GetField("text").SetValue(texts[index] ?? String.Empty);

                _writer.AddDocument(document);
            }

            _writer.Commit();
            _searcherManager.MaybeReopen();      
        }

        /// <summary>
        /// Adds a single text item to the index.
        /// </summary>
        /// <param name="text">The text to add.</param>
        public void Add(string text)
        {
            Add(new[] { text });
        }

        private Document CreateDocument()
        {
            var document = new Document();
            document.Add(new Field("id", "", Field.Store.YES, Field.Index.NOT_ANALYZED));
            document.Add(new Field("text", "", Field.Store.NO, Field.Index.ANALYZED));

            return document;
        }

        /// <summary>
        /// Searches the index for texts that match the query expression.
        /// </summary>
        /// <param name="queryExpression">The query expression.</param>
        /// <param name="topN">The limit on the number of matching texts to return.</param>
        /// <returns>The index positions of the texts that match the query expression.</returns>
        public IEnumerable<int> Search(string queryExpression, int? topN = null)
        { 
            Query query = null;       
            try
            {
                query = _queryParser.Parse(queryExpression);

            }
            catch (ParseException e)
            {
                throw new InvalidQueryException(String.Format("The queryExpression is invalid: {0}.", queryExpression), e);
            }

            if (query == null)
                throw new InvalidQueryException("The query expression is not initialized.");


            using (var searcher = _searcherManager.Acquire().Searcher)
            {                
                var maxDocs = topN ?? Int32.MaxValue;
                var topDocs = searcher.Search(query, maxDocs);

                if (topDocs.TotalHits > 0)
                {
                    foreach (var sd in topDocs.ScoreDocs)
                    {
                        var doc = searcher.Doc(sd.Doc);
                        var id = doc.GetField("id").StringValue;

                        yield return Int32.Parse(id);
                    }
                }
            }
           
        }


        /// <summary>
        /// Breaks up the specified text into tokens.
        /// </summary>
        /// <param name="text">The text to tokenize.</param>
        /// <param name="enableStemming">if set to <c>true</c>, the FullTextIndex will stem 
        /// the tokens that make up the texts, using the Porter stemming algorithm.</param>
        /// <param name="ignoreCase">if set to <c>true</c>, character casing is ignored.</param>
        /// <param name="separatorChars">A string whose component characters will be used to split the texts into tokens.</param>              
        /// <returns></returns>
        public static IEnumerable<string> Tokenize(string text, bool enableStemming = true, bool ignoreCase = true, string separatorChars = CustomTokenizer.DEFAULT_SEPARATOR_CHARS)
        {
            return CustomAnalyzer.Tokenize(text, enableStemming, ignoreCase, separatorChars);  
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _searcherManager.Dispose();
            _writer.Dispose();
        }        
    }

    [Serializable]
    public class InvalidQueryException : Exception
    {
        public InvalidQueryException(string message) : base(message)
        {
        }

        public InvalidQueryException(string message, Exception innerException) : base(message, innerException)
        {
        }       
    }
}
