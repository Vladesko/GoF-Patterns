using GoFPatterns.Bridge.RemoteControl.Devices;
using GoFPatterns.Bridge.RemoteControl.Remotes;

namespace GoFPatterns.Bridge.RemoteControl
{
    public class DemonstrateRemoteControl
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Мост: пульт управления ===\n");

            // 2 пульта x 2 устройства = 4 комбинации, но всего 2 + 2 класса.
            Console.WriteLine("Простой пульт + телевизор:");
            var basicTvRemote = new Remote(new Tv());
            basicTvRemote.TogglePower();
            basicTvRemote.VolumeUp();
            basicTvRemote.ChannelUp();

            Console.WriteLine("\nРасширенный пульт + радио:");
            var advancedRadioRemote = new AdvancedRemote(new Radio());
            advancedRadioRemote.TogglePower();
            advancedRadioRemote.VolumeUp();
            advancedRadioRemote.Mute();
            advancedRadioRemote.Unmute();
            advancedRadioRemote.SetFavoriteChannel(2);

            Console.WriteLine("\nРасширенный пульт + телевизор:");
            var advancedTvRemote = new AdvancedRemote(new Tv());
            advancedTvRemote.TogglePower();
            advancedTvRemote.SetFavoriteChannel(42);
            advancedTvRemote.Mute();
            advancedTvRemote.TogglePower();
        }
    }
}
