namespace GoFPatterns.Adapter.SensorFeature
{
    /// <summary>
    /// Passt einen Fahrenheit-Temperatursensor an die Celsius-Schnittstelle an.
    /// Rechnet die Temperaturwerte von Fahrenheit in Celsius um.
    /// </summary>
    public class FahrenheitSensorAdapter(FahrenheitSensor fahrenheitSensor) : ITemperatureSensor
    {
        private readonly FahrenheitSensor _fahrenheitSensor = fahrenheitSensor;

        public decimal GetTemperatureCelsius() =>
            (_fahrenheitSensor.GetCurrentTemperature() - 32m) * 5m / 9m;
    }
}
