namespace GoFPatterns.Bridge.RemoteControl.Remotes
{
    // Refined abstraction - extends remote with new features.
    // It works with any device, devices don't need to be changed.
    public class AdvancedRemote(IDevice device) : Remote(device)
    {
        private int _volumeBeforeMute;

        public void Mute()
        {
            Console.WriteLine($"[{nameof(AdvancedRemote)}] mute");
            _volumeBeforeMute = Device.Volume;
            Device.SetVolume(0);
        }

        public void Unmute()
        {
            Console.WriteLine($"[{nameof(AdvancedRemote)}] unmute");
            Device.SetVolume(_volumeBeforeMute);
        }

        public void SetFavoriteChannel(int channel)
        {
            Console.WriteLine($"[{nameof(AdvancedRemote)}] favorite channel");
            Device.SetChannel(channel);
        }
    }
}
