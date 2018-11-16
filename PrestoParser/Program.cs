using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductFileParser;

namespace PrestoParser
{
    class Program
    {
        static void Main(string[] args)
        {
            LinearParser test = new LinearParser("./input-sample.txt");

            // ### wrap this in a try catch block to pick up the DataValidation and BadPath exceptions (tbd)
            List<ProductRecord> records = test.Parse();

            // loop over records
            // output to console X good, Y bad records parsed

            // output records to json file here

            Console.WriteLine("Records Parsed: " + records.Count);
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
        }
    }
}
