namespace GoFPatterns.Adapter.SensorFeature
{
    public class DemonstarteAdapter
    {
        public void Demonstarte()
        {
            Console.WriteLine("=== Demo del pattern Adapter ===\n");

            // Sensore moderno - funziona direttamente
            ITemperatureSensor celsiusSensor = new CelsiusSensor();
            Console.WriteLine($"Sensore Celsius: {celsiusSensor.GetTemperatureCelsius()}°C");

            // Sensore legacy - serve un adapter
            var fahrenheitSensor = new FahrenheitSensor();
            Console.WriteLine($"Sensore Fahrenheit (incompatibile): {fahrenheitSensor.GetCurrentTemperature()}°F");

            // Uso dell'adapter
            ITemperatureSensor adapter = new FahrenheitSensorAdapter(fahrenheitSensor);
            Console.WriteLine($"Sensore Fahrenheit tramite adapter: {adapter.GetTemperatureCelsius()}°C");

            // Entrambi funzionano con la stessa interfaccia
            Console.WriteLine("\n=== Uso dell'interfaccia unificata ===");
            DisplayTemperature(celsiusSensor);
            DisplayTemperature(adapter);
        }

        private void DisplayTemperature(ITemperatureSensor sensor)
        {
            var temp = sensor.GetTemperatureCelsius();
            var status = temp switch
            {
                < 15 => "Freddo ",
                >= 15 and < 25 => "Confortevole ",
                _ => "Caldo "
            };
            Console.WriteLine($"Temperatura: {temp}°C - {status}");
        }
    }
}
