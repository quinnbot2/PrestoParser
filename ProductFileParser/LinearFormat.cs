using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Globalization;

namespace ProductFileParser
{
    class LinearFormat
    {
        // ### make this a dumb lineardata class?
        int ID;
        string Description;
        float SinglePrice;
        float SinglePrice_Promo;
        float SplitPrice;
        float SplitPrice_Promo;
        int SplitCount;
        int SplitCount_Promo;
        int Flags;
        string Size;

        List<LinearDataSegment> Segments;

        bool GoodData;
        string RawLine;
        CultureInfo CultureCurrency;
        float TaxRate;

        public LinearFormat()
        {
            Segments = new List<LinearDataSegment>();

            // segments hardcoded for brevity's sake
            Segments.Add(new LinearDataSegment("ID",                0, 8, LinearDataSegment.SegmentType.Number));
            Segments.Add(new LinearDataSegment("Description",       9, 68, LinearDataSegment.SegmentType.String));
            Segments.Add(new LinearDataSegment("SinglePrice",       69, 77, LinearDataSegment.SegmentType.Currency));
            Segments.Add(new LinearDataSegment("SinglePrice_Promo", 78, 86, LinearDataSegment.SegmentType.Currency));
            Segments.Add(new LinearDataSegment("SplitPrice",        87, 95, LinearDataSegment.SegmentType.Currency));
            Segments.Add(new LinearDataSegment("SplitPrice_Promo",  96, 104, LinearDataSegment.SegmentType.Currency));
            Segments.Add(new LinearDataSegment("SplitCount",        105, 113, LinearDataSegment.SegmentType.Number));
            Segments.Add(new LinearDataSegment("SplitCount_Promo",  114, 122, LinearDataSegment.SegmentType.Number));
            Segments.Add(new LinearDataSegment("Flags",             123, 132, LinearDataSegment.SegmentType.Flags));
            Segments.Add(new LinearDataSegment("Size",              133, 142, LinearDataSegment.SegmentType.String));

            // ### presumably, these segment defs could actually be read in from a config file,
            // ### which would allow the LinearFormat to handle many variations of fixed width data layouts

            // ### as an excercise, lets validate that start and ends dont overlap.  and that the var name is legit....

            // US Currency is assumed in the example
            CultureCurrency = new CultureInfo("en-US");
            TaxRate = 7.775f;
        }

        public ProductRecord ParseLine(string inLine)
        {
            RawLine = inLine;

            // parse data segments out from the given string
            GoodData = true;
            foreach (LinearDataSegment segment in Segments)
                GoodData &= ParseSegment(segment);
            
            // convert the internal data into PrestoQ Product Record
            return GenerateProductRecord();
        }

        // ### think im going to move this down into segment, hmm....
        bool ParseSegment(LinearDataSegment inSegment)
        {
            // make sure the requested substring actually exists
            if ((inSegment.StartIndex >= RawLine.Length) || (inSegment.EndIndex > RawLine.Length))
                return false;

            string substring = RawLine.Substring(inSegment.StartIndex, inSegment.EndIndex - inSegment.StartIndex);

            // use reflection to get the correct variable by name
            FieldInfo info = this.GetType().GetField(inSegment.VariableName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            // failsafe: with data validation above, this should theoretically never return null
            if (info == null)
                return false;

            // convert the substring and assign it to the correct variable
            switch (inSegment.DataType)
            {
                case LinearDataSegment.SegmentType.Number:
                    info.SetValue(this, Convert.ToInt32(substring));
                    break;
                case LinearDataSegment.SegmentType.String:
                    info.SetValue(this, substring);
                    break;
                // ### check that negative values work...
                case LinearDataSegment.SegmentType.Currency:
                    float rawCurrency = (float)Convert.ToInt32(substring);
                    info.SetValue(this, rawCurrency / 100f);
                    break;
                case LinearDataSegment.SegmentType.Flags:
                    info.SetValue(this, ConvertFlags(substring));
                    break;
                // any unknown type is bad data from this perspective
                default:
                    return false;
            }

            return true;
        }

        int ConvertFlags(string inSubstring)
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

        // convert internal data up into PrestoQ product record type
        ProductRecord GenerateProductRecord()
        {
            float calculatorPrice, calculatorPricePromo;
            string displayPrice, displayPricePromo;

            GeneratePrices(SinglePrice, SplitPrice, SplitCount, out calculatorPrice, out displayPrice);
            GeneratePrices(SinglePrice_Promo, SplitPrice_Promo, SplitCount_Promo, out calculatorPricePromo, out displayPricePromo);

            ProductRecord newRecord = new ProductRecord(
                    ID,
                    Description,
                    displayPrice,
                    calculatorPrice,
                    displayPricePromo,
                    calculatorPricePromo,
                    IsWeighed() ? ProductRecord.MeasurementType.PerPound : ProductRecord.MeasurementType.Each,
                    Size,
                    IsTaxed() ? TaxRate : 0f
                );

            return newRecord;
        }

        // choose single or split pricing
        void GeneratePrices(float inSinglePrice, float inSplitPrice, int inCount, out float outCalculatorPrice, out string outDisplay )
        {
            float price = inSinglePrice;

            if (inSplitPrice != 0f)
                price = inSplitPrice / (float)inCount;

            // ### test a split price that comes out to more than 4 decimal places
            // ### need to implement half-down rounding, i guess?
            outCalculatorPrice = (float)Math.Round(price, 4, MidpointRounding.ToEven);
            outDisplay = outCalculatorPrice.ToString("C", CultureCurrency);
        }

        bool IsWeighed() { return (Flags & 4) != 0; }
        bool IsTaxed() { return (Flags & 16) != 0; }
    }
}
