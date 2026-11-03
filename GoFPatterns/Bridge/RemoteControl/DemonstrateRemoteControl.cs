using GoFPatterns.Bridge.RemoteControl.Devices;
using GoFPatterns.Bridge.RemoteControl.Remotes;

namespace GoFPatterns.Bridge.RemoteControl
{
    public class DemonstrateRemoteControl
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Bridge: mando a distancia ===\n");

            // 2 mandos x 2 dispositivos = 4 combinaciones, pero solo 2 + 2 clases.
            Console.WriteLine("Mando básico + TV:");
            var basicTvRemote = new Remote(new Tv());
            basicTvRemote.TogglePower();
            basicTvRemote.VolumeUp();
            basicTvRemote.ChannelUp();

            Console.WriteLine("\nMando avanzado + Radio:");
            var advancedRadioRemote = new AdvancedRemote(new Radio());
            advancedRadioRemote.TogglePower();
            advancedRadioRemote.VolumeUp();
            advancedRadioRemote.Mute();
            advancedRadioRemote.Unmute();
            advancedRadioRemote.SetFavoriteChannel(2);

            Console.WriteLine("\nMando avanzado + TV:");
            var advancedTvRemote = new AdvancedRemote(new Tv());
            advancedTvRemote.TogglePower();
            advancedTvRemote.SetFavoriteChannel(42);
            advancedTvRemote.Mute();
            advancedTvRemote.TogglePower();
        }
    }
}
