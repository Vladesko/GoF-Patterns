namespace GoFPatterns.Adapter.SensorFeature
{
    public class DemonstarteAdapter
    {
        public void Demonstarte()
        {
            Console.WriteLine("=== Demo del patrón Adapter ===\n");

            // Sensor moderno: funciona directamente
            ITemperatureSensor celsiusSensor = new CelsiusSensor();
            Console.WriteLine($"Sensor Celsius: {celsiusSensor.GetTemperatureCelsius()}°C");

            // Sensor heredado: necesita un adaptador
            var fahrenheitSensor = new FahrenheitSensor();
            Console.WriteLine($"Sensor Fahrenheit (incompatible): {fahrenheitSensor.GetCurrentTemperature()}°F");

            // Uso del adaptador
            ITemperatureSensor adapter = new FahrenheitSensorAdapter(fahrenheitSensor);
            Console.WriteLine($"Sensor Fahrenheit a través del adaptador: {adapter.GetTemperatureCelsius()}°C");

            // Ambos funcionan con la misma interfaz
            Console.WriteLine("\n=== Uso de la interfaz unificada ===");
            DisplayTemperature(celsiusSensor);
            DisplayTemperature(adapter);
        }

        private void DisplayTemperature(ITemperatureSensor sensor)
        {
            var temp = sensor.GetTemperatureCelsius();
            var status = temp switch
            {
                < 15 => "Frío ",
                >= 15 and < 25 => "Agradable ",
                _ => "Calor "
            };
            Console.WriteLine($"Temperatura: {temp}°C - {status}");
        }
    }
}
