using System.Text;

namespace GoFPatterns.Decorator.DataStream.Decorators
{
    // Decorador concreto: cifra los datos antes de escribirlos y los descifra después de leerlos.
    // Se usa un XOR simple solo para la demostración; NO es un cifrado real.
    public class EncryptionDecorator(IDataSource source, string key) : DataSourceDecorator(source)
    {
        private readonly byte[] _key = Encoding.UTF8.GetBytes(key);

        public override void Write(string data) =>
            base.Write(Convert.ToBase64String(Xor(Encoding.UTF8.GetBytes(data))));

        public override string Read() =>
            Encoding.UTF8.GetString(Xor(Convert.FromBase64String(base.Read())));

        private byte[] Xor(byte[] data)
        {
            var result = new byte[data.Length];
            for (var i = 0; i < data.Length; i++)
            {
                result[i] = (byte)(data[i] ^ _key[i % _key.Length]);
            }
            return result;
        }
    }
}
