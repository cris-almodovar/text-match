﻿using FlexLucene.Analysis;
using FlexLucene.Analysis.Core;
using FlexLucene.Analysis.En;
using FlexLucene.Analysis.Pattern;
using java.util.regex;

namespace TextMatch
{
    /// <summary>
    /// Analyzer that uses the Porter stemming algorithm for English text.
    /// </summary>
    class CustomAnalyzer : Analyzer
    {
        private string _stopTokensPattern;
        private bool _enableStemming;
        private bool _ignoreCase;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomAnalyzer" /> class.
        /// </summary>
        /// <param name="stopTokensPattern">A regular expression that will be used to tokenize the input text.</param>
        /// <param name="enableStemming">if set to <c>true</c> the tokens generated will be stemmed using the Porter stemming algorithm.</param>
        /// <param name="ignoreCase">if set to <c>true</c> the tokens generated will be converted to lowercase, to faciliate case-insensitive search.</param>
        public CustomAnalyzer(string stopTokensPattern, bool enableStemming = true, bool ignoreCase = true)
        {
            _stopTokensPattern = stopTokensPattern;
            _enableStemming = enableStemming;
            _ignoreCase = ignoreCase;
        }

        protected override TokenStreamComponents createComponents(string fieldName)
        {
            //var patternString =  ConfigurationManager.AppSettings["StopTokensPattern"] ?? @"[\s,:;.()?!]";
            var pattern = Pattern.compile(_stopTokensPattern);
            var tokenizer = new PatternTokenizer(pattern, -1);
            var stream = _ignoreCase ? new LowerCaseFilter(tokenizer) as TokenStream : tokenizer as TokenStream;

            if (_enableStemming)
                stream = new PorterStemFilter(stream);          

            return new TokenStreamComponents(tokenizer, stream);
        }
    }
}