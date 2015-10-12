using System;
using System.Collections.Generic;
using System.IO;
using Lucene.Net.Analysis;

namespace TextMatch
{
    /// <summary>
    /// A tokenizer that breaks sentences using a list of separator chars
    /// </summary>
    class CustomTokenizer : CharTokenizer
    {
        public const string DEFAULT_SEPARATOR_CHARS = @",:;.()?!@#%^&*|/\+÷°±{}[]<>-`~'""$£€¢¥©®™•§†‡–—¶";
        readonly HashSet<char> _separatorCharsLookup;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTokenizer"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="separatorChars">The separator chars.</param>
        public CustomTokenizer(TextReader reader, string separatorChars) : base(reader)
        {
            _separatorCharsLookup = new HashSet<char>(separatorChars);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTokenizer"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public CustomTokenizer(TextReader reader) : this(reader, DEFAULT_SEPARATOR_CHARS)
        {
        }

        /// <summary>
        /// Determines whether the specified character is a token character.
        /// </summary>
        /// <param name="c">The character.</param>
        /// <returns></returns>
        protected override bool IsTokenChar(char c)
        {
            return !Char.IsWhiteSpace(c) && !_separatorCharsLookup.Contains(c);
        }
    }
}
