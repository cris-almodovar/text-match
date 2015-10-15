[![Build status](https://ci.appveyor.com/api/projects/status/o3fkhj4up0mhdr02?svg=true)](https://ci.appveyor.com/project/cris-almodovar/text-match)

---

# TextMatch

**TextMatch** is a library for matching text strings against patterns (written as **Lucene** query expressions). 

If you have a large collection of text strings, and you need to find the ones that match certain text patterns - **TextMatch** can help. For this requirement, TextMatch is easier to use than regular expressions.

### Features

- Uses **Lucene** query expressions to define text patterns. Refer to  [this article](https://lucene.apache.org/core/2_9_4/queryparsersyntax.html) for an overview of the syntax.   
- Option to change the stop words/chars used in tokenization.  
- Option to enable/disable case-sensitive matching.
- Option to enable/disable word stemming.


### Installation

Open the Package Manager Console and install the [TextMatch nuget package](https://www.nuget.org/packages/TextMatch/).

	PM> Install-Package TextMatch  

### Usage

Refer to **TextMatch.Tests** for usage.

