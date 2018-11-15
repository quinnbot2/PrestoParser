using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFileParser
{
    public class LinearParser : Parser
    {
        public LinearParser(string inSource) : base(inSource) {}

        public override List<ProductRecord> Parse()
        {
            List<ProductRecord> parsedRecords = new List<ProductRecord>();
            
            // check that the filename is valid

            // open the file
            // look at file size and predefine a list of that size?   should it be a hash on SKU?

            // read the line
            // error check for size / format?

            // parse the line into a record

            // add the record to the collection

            Console.WriteLine("File Parsed");

            return parsedRecords;
        }
    }
}
