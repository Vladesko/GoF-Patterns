namespace GoFPatterns.Command.SmartHome.Devices
{
    // Receiver.
    public class SecuritySystem
    {
        public void Arm() => Console.WriteLine("    Security: armed");
        public void Disarm() => Console.WriteLine("    Security: disarmed");
    }
}
