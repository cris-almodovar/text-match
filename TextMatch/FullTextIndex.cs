using FlexLucene.Analysis;
using FlexLucene.Analysis.Tokenattributes;
using FlexLucene.Document;
using FlexLucene.Index;
using FlexLucene.Queryparser.Classic;
using FlexLucene.Search;
using FlexLucene.Store;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

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
        private DirectoryReader _reader;
        private QueryParser _queryParser;
        private IndexSearcher _searcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="FullTextIndex" /> class.
        /// </summary>
        /// <param name="enableStemming">if set to <c>true</c>, words are stemmed using the Porter stemming algorithm.</param>
        /// <param name="ignoreCase">if set to <c>true</c>, casing is ignored.</param>
        /// <param name="stopTokensPattern">A regular expression that will be used to tokenize the texts that are added to the index.</param>
        public FullTextIndex(bool enableStemming = true, bool ignoreCase = true, string stopTokensPattern = null)
        {
            _directory = new RAMDirectory();
            _analyzer = new CustomAnalyzer(stopTokensPattern, enableStemming, ignoreCase);            

            var config = new IndexWriterConfig(_analyzer);
            _writer = new IndexWriter(_directory, config);

            _reader = DirectoryReader.Open(_writer, true);

            _queryParser = new QueryParser("text", _analyzer);
            _searcher = new IndexSearcher(_reader);            
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
                document.GetField("id").SetStringValue(index.ToString());
                document.GetField("text").SetStringValue(texts[index] ?? String.Empty);

                _writer.AddDocument(document);
            }

            _writer.Commit();
        }

        private Document CreateDocument()
        {
            var document = new Document();
            document.Add(new StringField("id", "", Field.Store.YES));
            document.Add(new TextField("text", "", Field.Store.NO));

            return document;
        }

        /// <summary>
        /// Searches the index for texts that match the query expression.
        /// </summary>
        /// <param name="queryExpression">The query expression.</param>
        /// <param name="topN">The limit on the number of matching texts to return.</param>
        /// <returns></returns>
        public IEnumerable<int> Search(string queryExpression, int? topN = null)
        {
            RefreshReader();

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

            var maxDocs = topN ?? _reader.NumDocs();
            var topDocs = _searcher.Search(query, maxDocs);            

            if (topDocs.TotalHits > 0)
                {
                    foreach (var sd in topDocs.ScoreDocs)
                    {
                        var doc = _searcher.Doc(sd.Doc);
                        var id = doc.GetField("id").stringValue();

                        yield return Int32.Parse(id);
                    }
                }
            

        }

        private void RefreshReader()
        {
            var updatedReader = DirectoryReader.OpenIfChanged(_reader, _writer, true);
            if (updatedReader != null)
            {
                lock (_reader)
                {
                    var oldReader = _reader;
                    _reader = updatedReader;

                    oldReader.Close();
                    _searcher = new IndexSearcher(_reader);
                }
            }
        }

        /// <summary>
        /// Breaks up the specified text into tokens.
        /// </summary>
        /// <param name="text">The text to tokenize.</param>
        /// <param name="stopTokensRegex">The stop tokens regular expression.</param>
        /// <param name="enableStemming">if set to <c>true</c>, the text will be stemmed.</param>
        /// <param name="ignoreCase">if set to <c>true</c>, case is ignored..</param>
        /// <returns></returns>
        public static IEnumerable<string> Tokenize(string text, string stopTokensRegex = CustomAnalyzer.DEFAULT_STOP_TOKENS_REGEX, bool enableStemming = true, bool ignoreCase = true)
        {
            return CustomAnalyzer.Tokenize(text, stopTokensRegex, enableStemming, ignoreCase);  
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _reader.Close();
            _writer.Close();
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
