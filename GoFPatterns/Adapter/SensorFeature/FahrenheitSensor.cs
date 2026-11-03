namespace GoFPatterns.Adapter.SensorFeature
{
    // This is legacy code. An adapter is needed
    public class FahrenheitSensor
    {
        // Needed for demonstration
        // Should be private and obtained from another service or similar
        private readonly int CurrentTemperature = new Random().Next(55, 65); // Temperature emulation

        // Some work happens here. It would be a request to another service
        public decimal GetCurrentTemperature() => CurrentTemperature;
    }
}
