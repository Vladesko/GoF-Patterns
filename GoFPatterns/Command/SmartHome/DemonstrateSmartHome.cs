using GoFPatterns.Command.SmartHome.Commands;
using GoFPatterns.Command.SmartHome.Devices;

namespace GoFPatterns.Command.SmartHome
{
    public class DemonstrateSmartHome
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Command: Smart-Home-Fernbedienung ===\n");

            var livingRoom = new Light("Wohnzimmer");
            var kitchen = new Light("Küche");
            var thermostat = new Thermostat();
            var security = new SecuritySystem();

            var livingRoomOn = new ActionCommand("Licht Wohnzimmer an", livingRoom.TurnOn);
            var allLightsOff = new MacroCommand("Alle Lichter aus",
                new ActionCommand("Wohnzimmer aus", livingRoom.TurnOff),
                new ActionCommand("Küche aus", kitchen.TurnOff));

            // Das Szenario "Haus verlassen" wird aus anderen Befehlen gebaut, darunter ein weiteres Makro.
            var leaveHome = new MacroCommand("Haus verlassen",
                allLightsOff,
                new ActionCommand("Eco-Temperatur", () => thermostat.SetTemperature(17)),
                new ActionCommand("Alarmanlage scharf schalten", security.Arm));

            var comeHome = new MacroCommand("Nach Hause kommen",
                new ActionCommand("Alarmanlage entschärfen", security.Disarm),
                new ActionCommand("Komforttemperatur", () => thermostat.SetTemperature(22)),
                livingRoomOn);

            var remote = new RemoteControl();
            remote.SetCommand(1, livingRoomOn);
            remote.SetCommand(2, allLightsOff);
            remote.SetCommand(3, leaveHome);
            remote.SetCommand(4, comeHome);

            remote.PressButton(1);
            remote.PressButton(3);
            remote.PressButton(4);
            remote.PressButton(9);

            // Befehle sind Objekte, daher können sie gespeichert und später ausgeführt werden.
            Console.WriteLine("\nBefehle für die Nacht werden eingeplant:");
            remote.Schedule(allLightsOff);
            remote.Schedule(new ActionCommand("Nachttemperatur", () => thermostat.SetTemperature(19)));
            Console.WriteLine("  ...es ist 23:00 Uhr...");
            remote.RunScheduled();
        }
    }
}
