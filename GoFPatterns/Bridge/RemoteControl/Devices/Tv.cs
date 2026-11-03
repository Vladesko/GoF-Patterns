namespace GoFPatterns.Bridge.RemoteControl.Devices
{
    // Implementor concreto - TV.
    public class Tv : IDevice
    {
        public string Name => "TV";
        public bool IsEnabled { get; private set; }
        public int Volume { get; private set; } = 30;
        public int Channel { get; private set; } = 1;

        public void Enable()
        {
            IsEnabled = true;
            Console.WriteLine("  TV: lo schermo si accende");
        }

        public void Disable()
        {
            IsEnabled = false;
            Console.WriteLine("  TV: lo schermo si spegne");
        }

        public void SetVolume(int volume)
        {
            Volume = Math.Clamp(volume, 0, 100);
            Console.WriteLine($"  TV: volume {Volume}");
        }

        public void SetChannel(int channel)
        {
            Channel = Math.Max(1, channel);
            Console.WriteLine($"  TV: canale {Channel}");
        }
    }
}
