namespace GoFPatterns.Bridge.RemoteControl.Devices
{
    // Конкретный исполнитель - телевизор.
    public class Tv : IDevice
    {
        public string Name => "Телевизор";
        public bool IsEnabled { get; private set; }
        public int Volume { get; private set; } = 30;
        public int Channel { get; private set; } = 1;

        public void Enable()
        {
            IsEnabled = true;
            Console.WriteLine("  Телевизор: экран включается");
        }

        public void Disable()
        {
            IsEnabled = false;
            Console.WriteLine("  Телевизор: экран выключается");
        }

        public void SetVolume(int volume)
        {
            Volume = Math.Clamp(volume, 0, 100);
            Console.WriteLine($"  Телевизор: громкость {Volume}");
        }

        public void SetChannel(int channel)
        {
            Channel = Math.Max(1, channel);
            Console.WriteLine($"  Телевизор: канал {Channel}");
        }
    }
}
