namespace GoFPatterns.Bridge.RemoteControl.Remotes
{
    // Refined abstraction - estende il telecomando con nuove funzionalità.
    // Funziona con qualsiasi dispositivo, i dispositivi non devono essere modificati.
    public class AdvancedRemote(IDevice device) : Remote(device)
    {
        private int _volumeBeforeMute;

        public void Mute()
        {
            Console.WriteLine($"[{nameof(AdvancedRemote)}] muto");
            _volumeBeforeMute = Device.Volume;
            Device.SetVolume(0);
        }

        public void Unmute()
        {
            Console.WriteLine($"[{nameof(AdvancedRemote)}] audio riattivato");
            Device.SetVolume(_volumeBeforeMute);
        }

        public void SetFavoriteChannel(int channel)
        {
            Console.WriteLine($"[{nameof(AdvancedRemote)}] canale preferito");
            Device.SetChannel(channel);
        }
    }
}
