namespace GoFPatterns.Command.SmartHome.Devices
{
    // Receiver.
    public class Thermostat
    {
        public void SetTemperature(int degrees) => Console.WriteLine($"    Termostato: {degrees}°C");
    }
}
