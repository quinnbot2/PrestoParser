using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFileParser
{
    class LinearFormat
    {
        int ID;
        string Description;
        float SinglePrice;
        float SinglePrice_Promo;
        float SplitPrice;
        float SplitPrice_Promo;
        int SplitCount;
        int SplitCount_Promo;
        int Flags;

        List<LinearDataSegment> Segments;

        public LinearFormat()
        {
            // define segments here
        }

        ProductRecord ParseLine(string inLine)
        {
            return null;
        }
    }
}
