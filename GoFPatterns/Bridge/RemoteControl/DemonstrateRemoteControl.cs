using GoFPatterns.Bridge.RemoteControl.Devices;
using GoFPatterns.Bridge.RemoteControl.Remotes;

namespace GoFPatterns.Bridge.RemoteControl
{
    public class DemonstrateRemoteControl
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Bridge: Fernbedienung ===\n");

            // 2 Fernbedienungen x 2 Geräte = 4 Kombinationen, aber nur 2 + 2 Klassen.
            Console.WriteLine("Einfache Fernbedienung + Fernseher:");
            var basicTvRemote = new Remote(new Tv());
            basicTvRemote.TogglePower();
            basicTvRemote.VolumeUp();
            basicTvRemote.ChannelUp();

            Console.WriteLine("\nErweiterte Fernbedienung + Radio:");
            var advancedRadioRemote = new AdvancedRemote(new Radio());
            advancedRadioRemote.TogglePower();
            advancedRadioRemote.VolumeUp();
            advancedRadioRemote.Mute();
            advancedRadioRemote.Unmute();
            advancedRadioRemote.SetFavoriteChannel(2);

            Console.WriteLine("\nErweiterte Fernbedienung + Fernseher:");
            var advancedTvRemote = new AdvancedRemote(new Tv());
            advancedTvRemote.TogglePower();
            advancedTvRemote.SetFavoriteChannel(42);
            advancedTvRemote.Mute();
            advancedTvRemote.TogglePower();
        }
    }
}
