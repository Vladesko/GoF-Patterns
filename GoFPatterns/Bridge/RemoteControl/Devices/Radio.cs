namespace GoFPatterns.Bridge.RemoteControl.Devices
{
    // Implementor concreto - radio. Il "canale" qui è una stazione radio.
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
            Console.WriteLine("  Radio: *fruscio* ... in onda");
        }

        public void Disable()
        {
            IsEnabled = false;
            Console.WriteLine("  Radio: spenta");
        }

        public void SetVolume(int volume)
        {
            // La radio ha un intervallo di volume più ridotto.
            Volume = Math.Clamp(volume, 0, 20);
            Console.WriteLine($"  Radio: volume {Volume}/20");
        }

        public void SetChannel(int channel)
        {
            Channel = (channel % _stations.Length + _stations.Length) % _stations.Length;
            Console.WriteLine($"  Radio: stazione {_stations[Channel]}");
        }
    }
}
