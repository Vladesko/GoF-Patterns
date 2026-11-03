namespace GoFPatterns.Decorator.DataStream.Decorators
{
    // Конкретный декоратор - не изменяет данные, только добавляет логирование вокруг вызова.
    public class LoggingDecorator(IDataSource source, string name) : DataSourceDecorator(source)
    {
        public override void Write(string data)
        {
            Console.WriteLine($"  [{name}] запись {data.Length} символов");
            base.Write(data);
        }

        public override string Read()
        {
            var data = base.Read();
            Console.WriteLine($"  [{name}] чтение {data.Length} символов");
            return data;
        }
    }
}
