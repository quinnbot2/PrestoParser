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
        ILinearFormat Format;

        public LinearParser(string inSource, ILinearFormat inFormat) : base(inSource)
        {
            Format = inFormat;
        }

        public override List<ProductRecord> Parse()
        {
            // ### should this be a hash table of records on SKU?
            List<ProductRecord> parsedRecords = new List<ProductRecord>();

            // ensure that the filename is valid
            if (!File.Exists(Source))
                throw new ParserException("File path: '" + Source + "' does not exist.");

            // open the file and parse each line into a ProductRecord
            using (StreamReader fileStream = new StreamReader(Source))
            {
                string line;

                while ((line = fileStream.ReadLine()) != null)
                    parsedRecords.Add( Format.ParseLine(line) );
            }
            
            return parsedRecords;
        }
    }
}
