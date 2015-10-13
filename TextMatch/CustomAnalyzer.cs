using System;
using System.Collections.Generic;
using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Tokenattributes;

namespace TextMatch
{
    /// <summary>
    /// Analyzer that uses the Porter stemming algorithm for English text.
    /// </summary>
    public class CustomAnalyzer : Analyzer
    {
        private string _separatorChars;
        private bool _enableStemming;
        private bool _ignoreCase;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomAnalyzer" /> class.
        /// </summary>
        /// <param name="enableStemming">if set to <c>true</c>, the FullTextIndex will stem 
        /// the tokens that make up the texts, using the Porter stemming algorithm.</param>
        /// <param name="ignoreCase">if set to <c>true</c>, character casing is ignored.</param>
        /// <param name="separatorChars">A string whose component characters will be used to split the texts into tokens.</param>   
        public CustomAnalyzer(bool enableStemming = true, bool ignoreCase = true, string separatorChars = CustomTokenizer.DEFAULT_SEPARATOR_CHARS)
        {
            if (String.IsNullOrWhiteSpace(separatorChars))
                separatorChars = CustomTokenizer.DEFAULT_SEPARATOR_CHARS;
            
            _enableStemming = enableStemming;
            _ignoreCase = ignoreCase;
            _separatorChars = separatorChars;
        }

        /// <summary>
        /// Tokenizes the text contained in the specified TextReader.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>        
        public override TokenStream TokenStream(string fieldName, TextReader reader)
        {
            var tokenizer = new CustomTokenizer(reader, _separatorChars);
            var stream = _ignoreCase ? new LowerCaseFilter(tokenizer) as TokenStream : tokenizer as TokenStream;

            if (_enableStemming)
                stream = new PorterStemFilter(stream);

            return stream;
        }


        /// <summary>
        /// Breaks up the text into tokens.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="enableStemming">if set to <c>true</c>, the FullTextIndex will stem 
        /// the tokens that make up the texts, using the Porter stemming algorithm.</param>
        /// <param name="ignoreCase">if set to <c>true</c>, character casing is ignored.</param>
        /// <param name="separatorChars">A string whose component characters will be used to split the texts into tokens.</param> 
        /// <returns></returns>
        public static IEnumerable<string> Tokenize(string text, bool enableStemming = true, bool ignoreCase = true, string separatorChars = CustomTokenizer.DEFAULT_SEPARATOR_CHARS)
        {
            using (var analyzer = new CustomAnalyzer(enableStemming, ignoreCase, separatorChars))
            {                
                using (var stream = analyzer.TokenStream("text", new StringReader(text)))
                {
                    var attrib = stream.GetAttribute<ITermAttribute>();
                    while (stream.IncrementToken())
                    {                        
                        yield return attrib.Term;
                    }

                }
            }
        }
    }
}
