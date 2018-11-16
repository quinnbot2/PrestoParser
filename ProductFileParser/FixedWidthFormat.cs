using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Globalization;
using System.IO;

namespace ProductFileParser
{
    public interface ILinearFormat
    {
        bool SetFieldByName(string inVariableName, object inValue);
        ProductRecord ParseLine(string inLine);
    }

    public class FixedWidthFormat : ILinearFormat
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
        string Size;

        List<FixedWidthDataSegment> Segments;

        bool GoodData;
        CultureInfo CultureCurrency;
        float TaxRate;

        public FixedWidthFormat()
        {
            LoadSegmentConfig();
            ValidateConfig();

            // US Currency and tax rate are assumed in the example
            CultureCurrency = new CultureInfo("en-US");
            TaxRate = 7.775f;
        }

        private void LoadSegmentConfig()
        {
            Segments = new List<FixedWidthDataSegment>();

            // ### presumably, these segment defs could actually be read in from a config file of some kind...
            // ### since its not core to this example, I've hardcoded them for brevity's sake
            Segments.Add(new FixedWidthDataSegment("ID",                0, 8, FixedWidthDataSegment.SegmentType.Number));
            Segments.Add(new FixedWidthDataSegment("Description",       9, 68, FixedWidthDataSegment.SegmentType.String));
            Segments.Add(new FixedWidthDataSegment("SinglePrice",       69, 77, FixedWidthDataSegment.SegmentType.Currency));
            Segments.Add(new FixedWidthDataSegment("SinglePrice_Promo", 78, 86, FixedWidthDataSegment.SegmentType.Currency));
            Segments.Add(new FixedWidthDataSegment("SplitPrice",        87, 95, FixedWidthDataSegment.SegmentType.Currency));
            Segments.Add(new FixedWidthDataSegment("SplitPrice_Promo",  96, 104, FixedWidthDataSegment.SegmentType.Currency));
            Segments.Add(new FixedWidthDataSegment("SplitCount",        105, 113, FixedWidthDataSegment.SegmentType.Number));
            Segments.Add(new FixedWidthDataSegment("SplitCount_Promo",  114, 122, FixedWidthDataSegment.SegmentType.Number));
            Segments.Add(new FixedWidthDataSegment("Flags",             123, 132, FixedWidthDataSegment.SegmentType.Flags));
            Segments.Add(new FixedWidthDataSegment("Size",              133, 142, FixedWidthDataSegment.SegmentType.String));
        }

        private void ValidateConfig()
        {
            // must have at least one segment
            if (Segments.Count == 0)
                throw new ParserException("Fixed Width Format configuration has no segments.");

            int lastEndIndex = -1;
            int count = 0;

            // all segments must have a valid variable name, a start index less than end index, and not overlap with another segment
            foreach (FixedWidthDataSegment segment in Segments)
            {
                FieldInfo info = this.GetType().GetField(segment.VariableName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                if (info == null)
                    throw new ParserException("Segment " + count + " variable name '" + segment.VariableName + "' does not exist." );

                if (segment.StartIndex >= segment.EndIndex)
                    throw new ParserException("Segment " + count + " start value " + segment.StartIndex + " is greater than the end index " + segment.EndIndex + ".");

                if (segment.StartIndex <= lastEndIndex)
                    throw new ParserException("Segment " + count + " start value " + segment.StartIndex + " overlaps end index " + lastEndIndex + " of previous segment.");

                lastEndIndex = segment.EndIndex;
                count++;
            }
        }

        // FieldByName Interface implementation is used by segments to populate linear format data
        public bool SetFieldByName(string inVariableName, object inValue)
        {
            // use reflection to get the correct variable by name
            FieldInfo info = this.GetType().GetField(inVariableName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            // failsafe: with data validation above, this should theoretically never return null
            if (info == null)
                return false;

            info.SetValue(this, inValue);

            return true;
        }

        public ProductRecord ParseLine(string inLine)
        {
            // parse data segments out from the given string
            GoodData = true;
            foreach (FixedWidthDataSegment segment in Segments)
                GoodData &= segment.Parse(ref inLine, this);
            
            // convert the internal data into PrestoQ Product Record
            return GenerateProductRecord();
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
                    IsTaxed() ? TaxRate : 0f,
                    GoodData
                );

            return newRecord;
        }

        // choose single or split pricing and generate appropiate values
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
