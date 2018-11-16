using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProductFileParser
{
    public class LinearParser : Parser
    {
        public LinearParser(string inSource) : base(inSource) {}

        public override List<ProductRecord> Parse()
        {
            // ### should this be a hash table of records on SKU?
            List<ProductRecord> parsedRecords = new List<ProductRecord>();

            // check that the filename is valid
            if (File.Exists(Source))
            {
                // throw an exception here?
            }
            
            // open the file and parse each line into a ProductRecord
            using (StreamReader fileStream = new StreamReader(Source))
            {
                LinearFormat format = new LinearFormat();
                string line;

                while ((line = fileStream.ReadLine()) != null)
                    parsedRecords.Add( format.ParseLine(line) );
            }
            
            return parsedRecords;
        }
    }
}
