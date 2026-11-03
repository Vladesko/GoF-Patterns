namespace GoFPatterns.Adapter.SensorFeature
{
    public class DemonstarteAdapter
    {
        public void Demonstarte()
        {
            Console.WriteLine("=== Demo des Adapter-Musters ===\n");

            // Moderner Sensor – funktioniert direkt
            ITemperatureSensor celsiusSensor = new CelsiusSensor();
            Console.WriteLine($"Celsius-Sensor: {celsiusSensor.GetTemperatureCelsius()}°C");

            // Legacy-Sensor – benötigt einen Adapter
            var fahrenheitSensor = new FahrenheitSensor();
            Console.WriteLine($"Fahrenheit-Sensor (inkompatibel): {fahrenheitSensor.GetCurrentTemperature()}°F");

            // Verwendung des Adapters
            ITemperatureSensor adapter = new FahrenheitSensorAdapter(fahrenheitSensor);
            Console.WriteLine($"Fahrenheit-Sensor über Adapter: {adapter.GetTemperatureCelsius()}°C");

            // Beide arbeiten mit derselben Schnittstelle
            Console.WriteLine("\n=== Verwendung der einheitlichen Schnittstelle ===");
            DisplayTemperature(celsiusSensor);
            DisplayTemperature(adapter);
        }

        private void DisplayTemperature(ITemperatureSensor sensor)
        {
            var temp = sensor.GetTemperatureCelsius();
            var status = temp switch
            {
                < 15 => "Kalt ",
                >= 15 and < 25 => "Angenehm ",
                _ => "Heiß "
            };
            Console.WriteLine($"Temperatur: {temp}°C - {status}");
        }
    }
}
