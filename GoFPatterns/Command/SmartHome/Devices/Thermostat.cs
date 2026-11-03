namespace GoFPatterns.Command.SmartHome.Devices
{
    // Receptor.
    public class Thermostat
    {
        public void SetTemperature(int degrees) => Console.WriteLine($"    Termostato: {degrees}°C");
    }
}
