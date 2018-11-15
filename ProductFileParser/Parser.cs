using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFileParser
{
    public abstract class Parser
    {
        protected string Source;

        protected Parser(string inSource)
        {
            Source = inSource;
        }

        public virtual List<ProductRecord> Parse()
        {
            // check that the source is valid

            // loop/read the source in whatever manner is appropriate
            // collect ProductRecords

            // return record collection
            return null;
        }
    }
}
