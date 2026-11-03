namespace GoFPatterns.Adapter.SensorFeature
{
    /// <summary>
    /// Adatta un sensore di temperatura Fahrenheit all'interfaccia Celsius.
    /// Converte le letture della temperatura da Fahrenheit a Celsius.
    /// </summary>
    public class FahrenheitSensorAdapter(FahrenheitSensor fahrenheitSensor) : ITemperatureSensor
    {
        private readonly FahrenheitSensor _fahrenheitSensor = fahrenheitSensor;

        public decimal GetTemperatureCelsius() =>
            (_fahrenheitSensor.GetCurrentTemperature() - 32m) * 5m / 9m;
    }
}
