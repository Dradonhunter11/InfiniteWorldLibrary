using System.IO;
using System.Text;

namespace InfiniteWorldLibrary.Utils
{
    internal class EmptyWriter : TextWriter
    {
        public override void WriteLine()
        {

        }

        public override void WriteLine(string value)
        {

        }

        public override void WriteLine(object value)
        {

        }

        public override Encoding Encoding => Encoding.ASCII;
    }

}
