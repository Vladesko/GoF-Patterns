namespace GoFPatterns.Command.SmartHome.Devices
{
    // Empfänger.
    public class Thermostat
    {
        public void SetTemperature(int degrees) => Console.WriteLine($"    Thermostat: {degrees}°C");
    }
}
