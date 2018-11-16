using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFileParser
{
    class LinearDataSegment
    {
        public enum SegmentType
        {
            Number,
            String,
            Currency,
            Flags
        }

        public string VariableName { get; }
        public int StartIndex { get; }          // 0-indexed, since it will refer to a character position in a string
        public int EndIndex { get; }            // 0-indexed, since it will refer to a character position in a string
        public SegmentType DataType { get; }

        public LinearDataSegment(string inVarName, int inStart, int inEnd, SegmentType inType)
        {
            VariableName = inVarName;
            StartIndex = inStart;
            EndIndex = inEnd;
            DataType = inType;
        }
    }
}
