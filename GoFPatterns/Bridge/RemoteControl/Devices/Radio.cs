namespace GoFPatterns.Bridge.RemoteControl.Devices
{
    // Implementador concreto: radio. Aquí el "canal" es una emisora de frecuencia.
    public class Radio : IDevice
    {
        private static readonly string[] _stations = ["Rock FM", "Jazz FM", "News FM", "Classic FM"];

        public string Name => "Radio";
        public bool IsEnabled { get; private set; }
        public int Volume { get; private set; } = 10;
        public int Channel { get; private set; }

        public void Enable()
        {
            IsEnabled = true;
            Console.WriteLine("  Radio: *ruido de estática* ... en el aire");
        }

        public void Disable()
        {
            IsEnabled = false;
            Console.WriteLine("  Radio: apagada");
        }

        public void SetVolume(int volume)
        {
            // La radio tiene un rango de volumen más pequeño.
            Volume = Math.Clamp(volume, 0, 20);
            Console.WriteLine($"  Radio: volumen {Volume}/20");
        }

        public void SetChannel(int channel)
        {
            Channel = (channel % _stations.Length + _stations.Length) % _stations.Length;
            Console.WriteLine($"  Radio: emisora {_stations[Channel]}");
        }
    }
}
