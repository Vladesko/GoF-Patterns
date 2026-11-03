using GoFPatterns.Bridge.RemoteControl.Devices;
using GoFPatterns.Bridge.RemoteControl.Remotes;

namespace GoFPatterns.Bridge.RemoteControl
{
    public class DemonstrateRemoteControl
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Bridge: Telecomando ===\n");

            // 2 telecomandi x 2 dispositivi = 4 combinazioni, ma solo 2 + 2 classi.
            Console.WriteLine("Telecomando base + TV:");
            var basicTvRemote = new Remote(new Tv());
            basicTvRemote.TogglePower();
            basicTvRemote.VolumeUp();
            basicTvRemote.ChannelUp();

            Console.WriteLine("\nTelecomando avanzato + Radio:");
            var advancedRadioRemote = new AdvancedRemote(new Radio());
            advancedRadioRemote.TogglePower();
            advancedRadioRemote.VolumeUp();
            advancedRadioRemote.Mute();
            advancedRadioRemote.Unmute();
            advancedRadioRemote.SetFavoriteChannel(2);

            Console.WriteLine("\nTelecomando avanzato + TV:");
            var advancedTvRemote = new AdvancedRemote(new Tv());
            advancedTvRemote.TogglePower();
            advancedTvRemote.SetFavoriteChannel(42);
            advancedTvRemote.Mute();
            advancedTvRemote.TogglePower();
        }
    }
}
