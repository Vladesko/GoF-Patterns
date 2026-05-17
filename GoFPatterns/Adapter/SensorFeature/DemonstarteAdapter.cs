namespace GoFPatterns.Adapter.SensorFeature
{
    //That is a client class
    public class Sensor
    {
        public void PrintTempereture(ITemperatureSensor sensor) =>
            sensor.GetTemperatureCelsius();
    }

    public class DemonstarteAdapter
    {
        public void Demonstrate()
        {
            Sensor sensor = new Sensor();

            var celsiusSensor = new CelsiusSensor();
            var farinhateSensor = new FarinhateSensor();
            var farinhateSensorAdapter = new FarinhateSensorAdapter(farinhateSensor);


            Console.WriteLine($"Print temperature by celsious sensor: {celsiusSensor.GetTemperatureCelsius()}");
            Console.WriteLine($"Print temperature by farinhate sensor: {farinhateSensor.GetCurrentTemperature()}");
            Console.WriteLine($"Print temperature by celsious with adapter sensor: {farinhateSensorAdapter.GetTemperatureCelsius()}");
        }
    }
}
