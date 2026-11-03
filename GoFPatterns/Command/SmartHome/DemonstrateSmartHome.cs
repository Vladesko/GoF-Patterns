using GoFPatterns.Command.SmartHome.Commands;
using GoFPatterns.Command.SmartHome.Devices;

namespace GoFPatterns.Command.SmartHome
{
    public class DemonstrateSmartHome
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Command: telecomando della casa intelligente ===\n");

            var livingRoom = new Light("Soggiorno");
            var kitchen = new Light("Cucina");
            var thermostat = new Thermostat();
            var security = new SecuritySystem();

            var livingRoomOn = new ActionCommand("Accendi la luce del soggiorno", livingRoom.TurnOn);
            var allLightsOff = new MacroCommand("Spegni tutte le luci",
                new ActionCommand("Spegni il soggiorno", livingRoom.TurnOff),
                new ActionCommand("Spegni la cucina", kitchen.TurnOff));

            // Lo scenario "Esci di casa" è composto da altri comandi, incluso un'altra macro.
            var leaveHome = new MacroCommand("Esci di casa",
                allLightsOff,
                new ActionCommand("Temperatura eco", () => thermostat.SetTemperature(17)),
                new ActionCommand("Attiva l'allarme", security.Arm));

            var comeHome = new MacroCommand("Rientra a casa",
                new ActionCommand("Disattiva l'allarme", security.Disarm),
                new ActionCommand("Temperatura comfort", () => thermostat.SetTemperature(22)),
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

            // I comandi sono oggetti, quindi possono essere memorizzati ed eseguiti in seguito.
            Console.WriteLine("\nPianificazione dei comandi per la notte:");
            remote.Schedule(allLightsOff);
            remote.Schedule(new ActionCommand("Temperatura notturna", () => thermostat.SetTemperature(19)));
            Console.WriteLine("  ...sono le 23:00...");
            remote.RunScheduled();
        }
    }
}
