namespace GoFPatterns.Adapter.SensorFeature
{
    // Это класс для современной функциональности. Адаптер здесь не нужен
    public class CelsiusSensor : ITemperatureSensor
    {

        // Нужно для демонстрации
        // Должно быть приватным или получаться из другого сервиса
        private readonly int CurrentTemperature = new Random().Next(15, 35); // Эмуляция температуры

        // Здесь выполняется некоторая работа. Это мог бы быть запрос к другому сервису
        public decimal GetTemperatureCelsius() => CurrentTemperature;
    }
}
