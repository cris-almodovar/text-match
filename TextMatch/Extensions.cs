using FlexLucene.Document;
using FlexLucene.Index;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextMatch
{
    public static class Extensions
    {
        internal static void SetStringValue(this IndexableField field, string value)
        {
            (field as Field).SetStringValue(value);
        }
    }
}
