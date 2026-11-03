namespace GoFPatterns.Command.SmartHome.Devices
{
    // Получатель.
    public class SecuritySystem
    {
        public void Arm() => Console.WriteLine("    Охрана: включена");
        public void Disarm() => Console.WriteLine("    Охрана: выключена");
    }
}
