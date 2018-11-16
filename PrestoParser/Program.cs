using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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
                // parse the passed in data file
                List<ProductRecord> records = prestoParse.Parse();

                int goodCount = 0;

                // simple json exporter
                using (StreamWriter writer = new StreamWriter("./output.json", false))
                {
                    writer.WriteLine("{ \"Record\" : [");

                    ProductRecord lastRecord = records.Last();
                    foreach (ProductRecord r in records)
                    {
                        writer.WriteLine(r.OutputJSON() + ((r != lastRecord) ? ", " : ""));

                        goodCount += r.IsGood() ? 1 : 0;
                    }

                    writer.WriteLine("] }");
                }

                // output good/bad record status to console
                Console.WriteLine("Records Parsed: " + records.Count + ", Bad Records: " + (records.Count - goodCount));
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
