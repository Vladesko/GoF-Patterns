namespace GoFPatterns.Bridge.RemoteControl.Remotes
{
    // Verfeinerte Abstraktion – erweitert die Fernbedienung um neue Funktionen.
    // Sie funktioniert mit jedem Gerät, die Geräte müssen nicht geändert werden.
    public class AdvancedRemote(IDevice device) : Remote(device)
    {
        private int _volumeBeforeMute;

        public void Mute()
        {
            Console.WriteLine($"[{nameof(AdvancedRemote)}] Stummschalten");
            _volumeBeforeMute = Device.Volume;
            Device.SetVolume(0);
        }

        public void Unmute()
        {
            Console.WriteLine($"[{nameof(AdvancedRemote)}] Ton wieder an");
            Device.SetVolume(_volumeBeforeMute);
        }

        public void SetFavoriteChannel(int channel)
        {
            Console.WriteLine($"[{nameof(AdvancedRemote)}] Lieblingskanal");
            Device.SetChannel(channel);
        }
    }
}
