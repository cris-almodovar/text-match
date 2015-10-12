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
        /// <param name="separatorChars">A string, whose individual characters will be used to break up the text into tokens.</param>
        /// <param name="enableStemming">if set to <c>true</c> the tokens generated will be stemmed using the Porter stemming algorithm.</param>
        /// <param name="ignoreCase">if set to <c>true</c> the tokens generated will be converted to lowercase, to enforce case-insensitive search.</param>
        public CustomAnalyzer(string separatorChars = CustomTokenizer.DEFAULT_SEPARATOR_CHARS, bool enableStemming = true, bool ignoreCase = true)
        {
            if (String.IsNullOrWhiteSpace(separatorChars))
                separatorChars = CustomTokenizer.DEFAULT_SEPARATOR_CHARS;

            _separatorChars = separatorChars;
            _enableStemming = enableStemming;
            _ignoreCase = ignoreCase;
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
        /// <param name="separatorChars">The stop tokens regex to be used in breaking up the text.</param>
        /// <param name="enableStemming">if set to <c>true</c> the tokens generated will be stemmed using the Porter stemming algorithm.</param>    
        /// <param name="ignoreCase">if set to <c>true</c> the tokens generated will be converted to lowercase, to enforce case-insensitive search.</param>
        /// <returns></returns>
        public static IEnumerable<string> Tokenize(string text, string separatorChars = CustomTokenizer.DEFAULT_SEPARATOR_CHARS, bool enableStemming = true, bool ignoreCase = true)
        {
            using (var analyzer = new CustomAnalyzer(separatorChars, enableStemming, ignoreCase))
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
