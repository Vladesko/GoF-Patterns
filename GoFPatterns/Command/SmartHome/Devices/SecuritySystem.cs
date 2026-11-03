namespace GoFPatterns.Command.SmartHome.Devices
{
    // Receptor.
    public class SecuritySystem
    {
        public void Arm() => Console.WriteLine("    Alarma: activada");
        public void Disarm() => Console.WriteLine("    Alarma: desactivada");
    }
}
