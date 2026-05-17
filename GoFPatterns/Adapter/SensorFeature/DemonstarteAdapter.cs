namespace GoFPatterns.Adapter.SensorFeature
{
    public class DemonstarteAdapter
    {
        public void Demonstarte()
        {
            Console.WriteLine("=== Adapter Pattern Demo ===\n");

            // Modern sensor - works directly
            ITemperatureSensor celsiusSensor = new CelsiusSensor();
            Console.WriteLine($"Celsius sensor: {celsiusSensor.GetTemperatureCelsius()}°C");

            // Legacy sensor - needs adapter
            var fahrenheitSensor = new FahrenheitSensor();
            Console.WriteLine($"Fahrenheit sensor (incompatible): {fahrenheitSensor.GetCurrentTemperature()}°F");

            // Using adapter
            ITemperatureSensor adapter = new FahrenheitSensorAdapter(fahrenheitSensor);
            Console.WriteLine($"Fahrenheit sensor via adapter: {adapter.GetTemperatureCelsius()}°C");

            // Both work with the same interface
            Console.WriteLine("\n=== Using unified interface ===");
            DisplayTemperature(celsiusSensor);
            DisplayTemperature(adapter);
        }

        private void DisplayTemperature(ITemperatureSensor sensor)
        {
            var temp = sensor.GetTemperatureCelsius();
            var status = temp switch
            {
                < 15 => "Cold ",
                >= 15 and < 25 => "Comfortable ",
                _ => "Hot "
            };
            Console.WriteLine($"Temperature: {temp}°C - {status}");
        }
    }
}
