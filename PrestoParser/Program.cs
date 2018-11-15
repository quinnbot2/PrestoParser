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
            Parser test = new Parser();

            test.Parse("dummy filename");

            Console.ReadLine();
        }
    }
}
