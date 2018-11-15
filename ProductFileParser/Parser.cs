using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFileParser
{
    public class Parser
    {
        string filename;

        public void Parse(string inFilename)
        {
            filename = inFilename;

            Console.WriteLine("File Parsed");
        }
    }
}
