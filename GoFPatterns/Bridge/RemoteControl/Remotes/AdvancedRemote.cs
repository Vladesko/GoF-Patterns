namespace GoFPatterns.Bridge.RemoteControl.Remotes
{
    // Уточнённая абстракция - расширяет пульт новыми возможностями.
    // Она работает с любым устройством, устройства менять не нужно.
    public class AdvancedRemote(IDevice device) : Remote(device)
    {
        private int _volumeBeforeMute;

        public void Mute()
        {
            Console.WriteLine($"[{nameof(AdvancedRemote)}] выключить звук");
            _volumeBeforeMute = Device.Volume;
            Device.SetVolume(0);
        }

        public void Unmute()
        {
            Console.WriteLine($"[{nameof(AdvancedRemote)}] включить звук");
            Device.SetVolume(_volumeBeforeMute);
        }

        public void SetFavoriteChannel(int channel)
        {
            Console.WriteLine($"[{nameof(AdvancedRemote)}] любимый канал");
            Device.SetChannel(channel);
        }
    }
}
