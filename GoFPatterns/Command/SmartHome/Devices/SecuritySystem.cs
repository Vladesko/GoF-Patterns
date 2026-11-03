namespace GoFPatterns.Command.SmartHome.Devices
{
    // Empfänger.
    public class SecuritySystem
    {
        public void Arm() => Console.WriteLine("    Alarmanlage: scharf");
        public void Disarm() => Console.WriteLine("    Alarmanlage: entschärft");
    }
}
