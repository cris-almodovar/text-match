using FlexLucene.Analysis;
using FlexLucene.Analysis.Core;
using FlexLucene.Analysis.En;
using FlexLucene.Analysis.Pattern;
using FlexLucene.Analysis.Tokenattributes;
using java.util.regex;
using System;
using System.Collections.Generic;

namespace TextMatch
{
    /// <summary>
    /// Analyzer that uses the Porter stemming algorithm for English text.
    /// </summary>
    public class CustomAnalyzer : Analyzer
    {
        private string _stopTokensRegex;
        private bool _enableStemming;
        private bool _ignoreCase;
        public const string DEFAULT_STOP_TOKENS_REGEX = @"[\s,:;.()?!@#%^&*|/+÷°±{}\[\]<>\-`~'""$£€¢¥©®™•§†‡–—¶]";

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomAnalyzer" /> class.
        /// </summary>
        /// <param name="stopTokensRegex">A regular expression that will be used to break up the text into tokenize.</param>
        /// <param name="enableStemming">if set to <c>true</c> the tokens generated will be stemmed using the Porter stemming algorithm.</param>
        /// <param name="ignoreCase">if set to <c>true</c> the tokens generated will be converted to lowercase, to enforce case-insensitive search.</param>
        public CustomAnalyzer(string stopTokensRegex = DEFAULT_STOP_TOKENS_REGEX, bool enableStemming = true, bool ignoreCase = true)
        {
            if (String.IsNullOrWhiteSpace(stopTokensRegex))
                stopTokensRegex = DEFAULT_STOP_TOKENS_REGEX;

            _stopTokensRegex = stopTokensRegex;
            _enableStemming = enableStemming;
            _ignoreCase = ignoreCase;
        }

        protected override TokenStreamComponents createComponents(string fieldName)
        {            
            var pattern = Pattern.compile(_stopTokensRegex);
            var tokenizer = new PatternTokenizer(pattern, -1);
            var stream = _ignoreCase ? new LowerCaseFilter(tokenizer) as TokenStream : tokenizer as TokenStream;

            if (_enableStemming)
                stream = new PorterStemFilter(stream);          

            return new TokenStreamComponents(tokenizer, stream);
        }

        /// <summary>
        /// Breaks up the text into tokens.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="stopTokensRegex">The stop tokens regex to be used in breaking up the text.</param>
        /// <param name="enableStemming">if set to <c>true</c> the tokens generated will be stemmed using the Porter stemming algorithm.</param>    
        /// <param name="ignoreCase">if set to <c>true</c> the tokens generated will be converted to lowercase, to enforce case-insensitive search.</param>
        /// <returns></returns>
        public static IEnumerable<string> Tokenize(string text, string stopTokensRegex = DEFAULT_STOP_TOKENS_REGEX, bool enableStemming = true, bool ignoreCase = true)
        {
            var analyzer = new CustomAnalyzer(stopTokensRegex, enableStemming, ignoreCase);

            TokenStream stream = analyzer.TokenStream("text", text);
            CharTermAttribute attrib = stream.AddAttribute(java.lang.Class.forName("FlexLucene.Analysis.Tokenattributes.CharTermAttribute")) as CharTermAttribute;

            stream.Reset();
            while (stream.incrementToken())
            {
                yield return attrib.toString();
            }
            stream.End();
            stream.Close();
        }
    }
}
