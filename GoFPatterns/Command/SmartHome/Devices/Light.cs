namespace GoFPatterns.Command.SmartHome.Devices
{
    // Receiver.
    public class Light(string room)
    {
        public void TurnOn() => Console.WriteLine($"    Luce {room}: accesa");
        public void TurnOff() => Console.WriteLine($"    Luce {room}: spenta");
    }
}
