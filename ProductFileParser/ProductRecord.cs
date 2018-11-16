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

        public ProductRecord(int inID, string inDecsription, string inDisplayPrice, float inCalculatorPrice, string inDisplayPrice_Promo, float inCalculatorPrice_Promo, MeasurementType inUnit, string inSize, float inTax)
        {
            ID = inID;
            Description = inDecsription;
            DisplayPrice = inDisplayPrice;
            CalculatorPrice = inCalculatorPrice;
            DisplayPrice_Promo = inDisplayPrice_Promo;
            CalculatorPrice_Promo = inCalculatorPrice_Promo;
            UnitofMeasure = inUnit;
            Size = inSize;
            TaxRate = inTax;
        }

        public string OutputPlainText()
        {
            string output = "";

            output += "ID: " + ID + "\n";
            output += "Description: " + Description + "\n";
            output += "DisplayPrice: " + DisplayPrice + "\n";
            output += "CalculatorPrice: " + CalculatorPrice + "\n";
            output += "DisplayPrice_Promo: " + DisplayPrice_Promo + "\n";
            output += "CalculatorPrice_Promo: " + CalculatorPrice_Promo + "\n";
            output += "UnitofMeasure: " + UnitofMeasure + "\n";
            output += "Size: " + Size + "\n";
            output += "TaxRate: " + TaxRate + "\n";

            output += "\n";

            return output;
        }
    }
}
