using System.IO;
using System.Text;

namespace TwinFinder.Base.Utils
{
    public class StringWriterWithEncoding : StringWriter
    {
        private readonly Encoding encoding;

        public StringWriterWithEncoding(StringBuilder sb) : this(sb, Encoding.UTF8)
        {
        }

        public StringWriterWithEncoding(StringBuilder sb, Encoding encoding)
        {
            this.encoding = encoding;
        }

        public override Encoding Encoding
        {
            get
            {
                return this.encoding;
            }
        }
    }
}
