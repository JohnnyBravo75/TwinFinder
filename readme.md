![](https://github.com/Assets/images/TTwinFinder_Logo.png) 

Library for fuzzy string matching. 
Can be used to find doublets or similar patterns in strings.

Inspired from the original SimMetrics Library from Java around 2010, 
before a port to C# existed (but I never released it, up to now).

## string metrics/distances
* Levenshtein
* DamerauLevenshtein
* Jaccard
* ExtendedJaccard
* JaroWinkler
* DiceCoefficent 
* Editex
* ExtendedEditex
* LongestCommonSubsequence
* MongeElkan
* NGramDistance
* SmithWaterman

## phonetic codecs
* Soundex
* DoubleMetaphone
* Phonix
* EditexKey
* SimpleTextKey
* DaitchMokotoff (seems to be buggy)

## string tokenizer
* NGramTokenizer
* FirstNCharsTokenizer
* WhiteSpaceTokenizer
* WordTokenizer

## algorithms
* SortedNeigborhood
* Blocking

Different aggregators and cost functions are also implemented.

## License

[MIT](License.txt)