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
            if (args.Length != 1)
            {
                Console.WriteLine("Path argument required, for example:  PrestoParser.exe ./input-sample.txt");
                return;
            }

            LinearParser prestoParse = new LinearParser(args[0], new FixedWidthFormat());

            try
            {
                List<ProductRecord> records = prestoParse.Parse();
                
                foreach (ProductRecord r in records)
                    Console.Write(r.OutputPlainText());

                // output to console X good, Y bad records parsed

                // output records to json file here

                Console.WriteLine("Records Parsed: " + records.Count);
            }
            catch (ParserException exception)
            {
                Console.WriteLine("Parsing Failed with error: ");
                Console.Write(exception.Message);
            }
            
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
        }
    }
}
