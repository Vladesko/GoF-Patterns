namespace GoFPatterns.Adapter.SensorFeature
{
    //That is adapter for Modern feature
    public class FarinhateSensorAdapter(FarinhateSensor farinhateSensor) : ITemperatureSensor
    {
        private readonly FarinhateSensor _farinhateSensor = farinhateSensor;

        public decimal GetTemperatureCelsius() =>
            (_farinhateSensor.GetCurrentTemperature() - 32m) * 5m / 9m;
    }
}
