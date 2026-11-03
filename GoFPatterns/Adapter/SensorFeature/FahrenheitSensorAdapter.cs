namespace GoFPatterns.Adapter.SensorFeature
{
    /// <summary>
    /// Адаптирует датчик температуры в градусах Фаренгейта к интерфейсу для градусов Цельсия.
    /// Преобразует показания температуры из Фаренгейтов в Цельсии.
    /// </summary>
    public class FahrenheitSensorAdapter(FahrenheitSensor fahrenheitSensor) : ITemperatureSensor
    {
        private readonly FahrenheitSensor _fahrenheitSensor = fahrenheitSensor;

        public decimal GetTemperatureCelsius() =>
            (_fahrenheitSensor.GetCurrentTemperature() - 32m) * 5m / 9m;
    }
}
