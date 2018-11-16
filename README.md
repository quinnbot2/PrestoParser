# PrestoParser
[![Build status](https://ci.appveyor.com/api/projects/status/8trsd3xjx8wu7rmm?svg=true)](https://ci.appveyor.com/project/DQ88745/prestoparser)

This is a fixed width file parser code test for PrestoQ.  Excercise the parser by running the PrestoParser.exe and passing it an input file of the agreed upon contractual format.  

## Parser
At the highest level the library uses the abstract class Parser to generalize interaction with all parsing systems.  The only derived class implementing it is LinearParser, which reads a flat file line by line.  The expectation is that this Parser class should also be extended to consume data from other sources, say HTMLParser or DBParser.

## LinearParser
To use the LinearParser, pass it in a source path to the flat file and a FixedWidthFormat.  Again, here Format is accepted as an argument with the assumption that other formats (Variable-width, etc) can also be written.  So long as the formats obey the ILinearFormat interface the parser shouldnt care.

## FixedWidthFormat
Fixed Width Formats are parse a string via a series of segments with start and end indexes.  The Format uses this list of segments in order to parse up the individual lines of flat text it is given.  While the current set of segments is from code, the Format handles them generically, meaning that in theory any arrangement of the segments can be used without needing to recompile.

NOTE: The segments mentioned above are assumed to be in ascending order for ease of validation.

### Exceptions
The Parser will throw ParserExceptions in a few situations and calls to Parse should be protected by a try/catch block
1. Filename is invalid
2. No Segments defined
3. Segment data invalid (bad id, bad start/end, overlapping)
