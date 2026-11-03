namespace GoFPatterns.Bridge.RemoteControl.Devices
{
    // Конкретный исполнитель - радио. "Канал" здесь - это радиостанция на определённой частоте.
    public class Radio : IDevice
    {
        private static readonly string[] _stations = ["Rock FM", "Jazz FM", "News FM", "Classic FM"];

        public string Name => "Радио";
        public bool IsEnabled { get; private set; }
        public int Volume { get; private set; } = 10;
        public int Channel { get; private set; }

        public void Enable()
        {
            IsEnabled = true;
            Console.WriteLine("  Радио: *шум эфира* ... в эфире");
        }

        public void Disable()
        {
            IsEnabled = false;
            Console.WriteLine("  Радио: выключено");
        }

        public void SetVolume(int volume)
        {
            // У радио меньший диапазон громкости.
            Volume = Math.Clamp(volume, 0, 20);
            Console.WriteLine($"  Радио: громкость {Volume}/20");
        }

        public void SetChannel(int channel)
        {
            Channel = (channel % _stations.Length + _stations.Length) % _stations.Length;
            Console.WriteLine($"  Радио: станция {_stations[Channel]}");
        }
    }
}
