namespace GoFPatterns.Bridge.RemoteControl.Devices
{
    // Concrete implementor - radio. "Channel" here is a frequency station.
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
            Console.WriteLine("  Radio: *static noise* ... on air");
        }

        public void Disable()
        {
            IsEnabled = false;
            Console.WriteLine("  Radio: off");
        }

        public void SetVolume(int volume)
        {
            // Radio has a smaller range of volume.
            Volume = Math.Clamp(volume, 0, 20);
            Console.WriteLine($"  Radio: volume {Volume}/20");
        }

        public void SetChannel(int channel)
        {
            Channel = (channel % _stations.Length + _stations.Length) % _stations.Length;
            Console.WriteLine($"  Radio: station {_stations[Channel]}");
        }
    }
}
