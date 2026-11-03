namespace GoFPatterns.Command.SmartHome.Devices
{
    // Receiver.
    public class SecuritySystem
    {
        public void Arm() => Console.WriteLine("    Allarme: attivato");
        public void Disarm() => Console.WriteLine("    Allarme: disattivato");
    }
}
