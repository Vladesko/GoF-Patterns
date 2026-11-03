namespace GoFPatterns.Adapter.SensorFeature
{
    // This is a class for a modern feature. No adapter is needed
    public class CelsiusSensor : ITemperatureSensor
    {

        // Needed for demonstration
        // Should be private or obtained from another service
        private readonly int CurrentTemperature = new Random().Next(15, 35); // Temperature emulation

        // Some work happens here. It would be a request to another service
        public decimal GetTemperatureCelsius() => CurrentTemperature;
    }
}
