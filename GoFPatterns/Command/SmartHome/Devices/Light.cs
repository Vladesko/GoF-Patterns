namespace GoFPatterns.Command.SmartHome.Devices
{
    // Получатель.
    public class Light(string room)
    {
        public void TurnOn() => Console.WriteLine($"    Свет ({room}): включён");
        public void TurnOff() => Console.WriteLine($"    Свет ({room}): выключен");
    }
}
