namespace GoFPatterns.Command.SmartHome.Devices
{
    // Receiver.
    public class Light(string room)
    {
        public void TurnOn() => Console.WriteLine($"    {room} light: on");
        public void TurnOff() => Console.WriteLine($"    {room} light: off");
    }
}
