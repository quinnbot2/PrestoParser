using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFileParser
{
    public class ParserException : Exception
    {
        public ParserException(string message) : base(message) {}
    }

    public abstract class Parser
    {
        protected string Source;

        protected Parser(string inSource)
        {
            Source = inSource;
        }

        public abstract List<ProductRecord> Parse();
    }
}
