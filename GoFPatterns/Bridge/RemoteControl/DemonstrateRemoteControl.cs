using GoFPatterns.Bridge.RemoteControl.Devices;
using GoFPatterns.Bridge.RemoteControl.Remotes;

namespace GoFPatterns.Bridge.RemoteControl
{
    public class DemonstrateRemoteControl
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Bridge: Remote control ===\n");

            // 2 remotes x 2 devices = 4 combinations, but only 2 + 2 classes.
            Console.WriteLine("Basic remote + TV:");
            var basicTvRemote = new Remote(new Tv());
            basicTvRemote.TogglePower();
            basicTvRemote.VolumeUp();
            basicTvRemote.ChannelUp();

            Console.WriteLine("\nAdvanced remote + Radio:");
            var advancedRadioRemote = new AdvancedRemote(new Radio());
            advancedRadioRemote.TogglePower();
            advancedRadioRemote.VolumeUp();
            advancedRadioRemote.Mute();
            advancedRadioRemote.Unmute();
            advancedRadioRemote.SetFavoriteChannel(2);

            Console.WriteLine("\nAdvanced remote + TV:");
            var advancedTvRemote = new AdvancedRemote(new Tv());
            advancedTvRemote.TogglePower();
            advancedTvRemote.SetFavoriteChannel(42);
            advancedTvRemote.Mute();
            advancedTvRemote.TogglePower();
        }
    }
}
