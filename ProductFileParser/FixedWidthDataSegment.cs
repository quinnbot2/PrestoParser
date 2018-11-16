using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFileParser
{
    class FixedWidthDataSegment
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

        public FixedWidthDataSegment(string inVarName, int inStart, int inEnd, SegmentType inType)
        {
            VariableName = inVarName;
            StartIndex = inStart;
            EndIndex = inEnd;
            DataType = inType;
        }

        public bool Parse(ref string inRawLine, ILinearFormat inFieldOwner)
        {
            // make sure the requested substring actually exists
            if ((StartIndex >= inRawLine.Length) || (EndIndex > inRawLine.Length))
                return false;

            string substring = inRawLine.Substring(StartIndex, EndIndex - StartIndex);

            // convert the substring and assign it to the correct variable
            bool valueAssigned = false;
            switch (DataType)
            {
                case SegmentType.Number:
                    valueAssigned = inFieldOwner.SetFieldByName(VariableName, Convert.ToInt32(substring));
                    break;
                case SegmentType.String:
                    valueAssigned = inFieldOwner.SetFieldByName(VariableName, substring);
                    break;
                case SegmentType.Currency:
                    float rawCurrency = (float)Convert.ToInt32(substring);
                    valueAssigned = inFieldOwner.SetFieldByName(VariableName, rawCurrency / 100f);
                    break;
                case SegmentType.Flags:
                    valueAssigned = inFieldOwner.SetFieldByName(VariableName, ConvertFlags(substring));
                    break;
                default:
                    break;
            }

            return valueAssigned;
        }

        private int ConvertFlags(string inSubstring)
        {
            int flags = 0;
            int bitMask = 1;

            for (int i = 0; i < inSubstring.Length; i++)
            {
                if (inSubstring[i] == 'Y')
                    flags = flags | bitMask;

                bitMask = bitMask << 1;
            }

            return flags;
        }
    }
}
