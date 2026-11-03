namespace GoFPatterns.Bridge.RemoteControl.Devices
{
    // Konkreter Implementierer – Fernseher.
    public class Tv : IDevice
    {
        public string Name => "Fernseher";
        public bool IsEnabled { get; private set; }
        public int Volume { get; private set; } = 30;
        public int Channel { get; private set; } = 1;

        public void Enable()
        {
            IsEnabled = true;
            Console.WriteLine("  Fernseher: Bildschirm schaltet sich ein");
        }

        public void Disable()
        {
            IsEnabled = false;
            Console.WriteLine("  Fernseher: Bildschirm schaltet sich aus");
        }

        public void SetVolume(int volume)
        {
            Volume = Math.Clamp(volume, 0, 100);
            Console.WriteLine($"  Fernseher: Lautstärke {Volume}");
        }

        public void SetChannel(int channel)
        {
            Channel = Math.Max(1, channel);
            Console.WriteLine($"  Fernseher: Kanal {Channel}");
        }
    }
}
