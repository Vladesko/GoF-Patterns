namespace GoFPatterns.Adapter.SensorFeature
{
    /// <summary>
    /// Adapts a Fahrenheit temperature sensor to the Celsius interface.
    /// Converts temperature readings from Fahrenheit to Celsius.
    /// </summary>
    public class FahrenheitSensorAdapter(FahrenheitSensor fahrenheitSensor) : ITemperatureSensor
    {
        private readonly FahrenheitSensor _fahrenheitSensor = fahrenheitSensor;

        public decimal GetTemperatureCelsius() =>
            (_fahrenheitSensor.GetCurrentTemperature() - 32m) * 5m / 9m;
    }
}
