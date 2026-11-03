namespace GoFPatterns.Adapter.SensorFeature
{
    public class DemonstarteAdapter
    {
        public void Demonstarte()
        {
            Console.WriteLine("=== Демонстрация паттерна Адаптер ===\n");

            // Современный датчик - работает напрямую
            ITemperatureSensor celsiusSensor = new CelsiusSensor();
            Console.WriteLine($"Датчик в Цельсиях: {celsiusSensor.GetTemperatureCelsius()}°C");

            // Устаревший датчик - нужен адаптер
            var fahrenheitSensor = new FahrenheitSensor();
            Console.WriteLine($"Датчик в Фаренгейтах (несовместим): {fahrenheitSensor.GetCurrentTemperature()}°F");

            // Используем адаптер
            ITemperatureSensor adapter = new FahrenheitSensorAdapter(fahrenheitSensor);
            Console.WriteLine($"Датчик в Фаренгейтах через адаптер: {adapter.GetTemperatureCelsius()}°C");

            // Оба работают через один и тот же интерфейс
            Console.WriteLine("\n=== Использование единого интерфейса ===");
            DisplayTemperature(celsiusSensor);
            DisplayTemperature(adapter);
        }

        private void DisplayTemperature(ITemperatureSensor sensor)
        {
            var temp = sensor.GetTemperatureCelsius();
            var status = temp switch
            {
                < 15 => "Холодно ",
                >= 15 and < 25 => "Комфортно ",
                _ => "Жарко "
            };
            Console.WriteLine($"Температура: {temp}°C - {status}");
        }
    }
}
