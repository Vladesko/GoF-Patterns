namespace GoFPatterns.Adapter.SensorFeature
{
    /// <summary>
    /// Adapta un sensor de temperatura en Fahrenheit a la interfaz en Celsius.
    /// Convierte las lecturas de temperatura de Fahrenheit a Celsius.
    /// </summary>
    public class FahrenheitSensorAdapter(FahrenheitSensor fahrenheitSensor) : ITemperatureSensor
    {
        private readonly FahrenheitSensor _fahrenheitSensor = fahrenheitSensor;

        public decimal GetTemperatureCelsius() =>
            (_fahrenheitSensor.GetCurrentTemperature() - 32m) * 5m / 9m;
    }
}
