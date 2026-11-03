namespace GoFPatterns.Decorator.DataStream.Decorators
{
    // Decorador concreto: no modifica los datos, solo añade registro (logging) alrededor de la llamada.
    public class LoggingDecorator(IDataSource source, string name) : DataSourceDecorator(source)
    {
        public override void Write(string data)
        {
            Console.WriteLine($"  [{name}] escritura de {data.Length} caracteres");
            base.Write(data);
        }

        public override string Read()
        {
            var data = base.Read();
            Console.WriteLine($"  [{name}] lectura de {data.Length} caracteres");
            return data;
        }
    }
}
