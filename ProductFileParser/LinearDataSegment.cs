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

        int StartIndex;
        int EndIndex;
        SegmentType DataType;
    }
}
