using System.IO.Compression;
using System.Text;

namespace GoFPatterns.Decorator.DataStream.Decorators
{
    // Concrete decorator - compresses data before writing and decompresses after reading.
    public class CompressionDecorator(IDataSource source) : DataSourceDecorator(source)
    {
        public override void Write(string data) => base.Write(Compress(data));

        public override string Read() => Decompress(base.Read());

        private static string Compress(string data)
        {
            using var output = new MemoryStream();
            using (var gzip = new GZipStream(output, CompressionLevel.SmallestSize))
            {
                var bytes = Encoding.UTF8.GetBytes(data);
                gzip.Write(bytes);
            }
            return Convert.ToBase64String(output.ToArray());
        }

        private static string Decompress(string data)
        {
            using var input = new MemoryStream(Convert.FromBase64String(data));
            using var gzip = new GZipStream(input, CompressionMode.Decompress);
            using var reader = new StreamReader(gzip, Encoding.UTF8);
            return reader.ReadToEnd();
        }
    }
}
