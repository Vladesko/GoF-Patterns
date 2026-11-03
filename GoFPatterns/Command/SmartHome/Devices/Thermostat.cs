namespace GoFPatterns.Command.SmartHome.Devices
{
    // Получатель.
    public class Thermostat
    {
        public void SetTemperature(int degrees) => Console.WriteLine($"    Термостат: {degrees}°C");
    }
}
