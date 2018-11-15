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
            LinearParser test = new LinearParser("dummy filename");

            List<ProductRecord> records = test.Parse();

            // output records to json file here

            Console.ReadLine();
        }
    }
}
