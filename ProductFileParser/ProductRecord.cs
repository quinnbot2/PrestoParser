using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFileParser
{
    public class ProductRecord
    {
        public enum MeasurementType
        {
            Each,
            PerPound
        }

        int ID;
        string Description;
        string DisplayPrice;
        float CalculatorPrice;
        string DisplayPrice_Promo;
        float CalculatorPrice_Promo;
        MeasurementType UnitofMeasure;
        string Size;
        float TaxRate;
    }
}
