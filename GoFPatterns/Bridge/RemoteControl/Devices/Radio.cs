namespace GoFPatterns.Bridge.RemoteControl.Devices
{
    // Konkreter Implementierer – Radio. "Kanal" ist hier ein Radiosender.
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
            Console.WriteLine("  Radio: *Rauschen* ... auf Sendung");
        }

        public void Disable()
        {
            IsEnabled = false;
            Console.WriteLine("  Radio: aus");
        }

        public void SetVolume(int volume)
        {
            // Das Radio hat einen kleineren Lautstärkebereich.
            Volume = Math.Clamp(volume, 0, 20);
            Console.WriteLine($"  Radio: Lautstärke {Volume}/20");
        }

        public void SetChannel(int channel)
        {
            Channel = (channel % _stations.Length + _stations.Length) % _stations.Length;
            Console.WriteLine($"  Radio: Sender {_stations[Channel]}");
        }
    }
}
