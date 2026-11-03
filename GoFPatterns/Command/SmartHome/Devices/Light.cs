namespace GoFPatterns.Command.SmartHome.Devices
{
    // Empfänger.
    public class Light(string room)
    {
        public void TurnOn() => Console.WriteLine($"    Licht {room}: an");
        public void TurnOff() => Console.WriteLine($"    Licht {room}: aus");
    }
}
