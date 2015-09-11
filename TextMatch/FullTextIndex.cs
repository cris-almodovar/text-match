using FlexLucene.Analysis;
using FlexLucene.Document;
using FlexLucene.Index;
using FlexLucene.Queryparser.Classic;
using FlexLucene.Search;
using FlexLucene.Store;
using System;
using System.Collections.Generic;

namespace TextMatch
{
    /// <summary>
    /// Encapsulates a Lucene in-memory index. 
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
        /// Initializes a new instance of the <see cref="FullTextIndex"/> class.
        /// </summary>
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

        public void AddItems(IList<string> items)
        {            
            for (var index = 0; index < items.Count; index++)
            {
                var document = CreateDocument();
                document.GetField("id").SetStringValue(index.ToString());
                document.GetField("text").SetStringValue(items[index] ?? String.Empty);

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

        public IEnumerable<int> Search(string queryExpression, int? topN = null)
        {
            RefreshReader();

            var query = _queryParser.Parse(queryExpression);
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

        public void Dispose()
        {
            _reader.Close();
            _writer.Close();
        }
    }
}
