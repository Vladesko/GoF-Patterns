namespace GoFPatterns.Command.SmartHome.Devices
{
    // Receptor.
    public class Light(string room)
    {
        public void TurnOn() => Console.WriteLine($"    Luz de {room}: encendida");
        public void TurnOff() => Console.WriteLine($"    Luz de {room}: apagada");
    }
}
