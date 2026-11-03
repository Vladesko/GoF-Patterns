namespace GoFPatterns.Adapter.SensorFeature
{
    // Это устаревший код. Здесь нужен адаптер
    public class FahrenheitSensor
    {
        // Нужно для демонстрации
        // Должно быть приватным и получаться из другого сервиса или откуда-то ещё
        private readonly int CurrentTemperature = new Random().Next(55, 65); // Эмуляция температуры

        // Здесь выполняется некоторая работа. Это мог бы быть запрос к другому сервису
        public decimal GetCurrentTemperature() => CurrentTemperature;
    }
}
