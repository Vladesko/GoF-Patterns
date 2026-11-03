using GoFPatterns.Command.SmartHome.Commands;
using GoFPatterns.Command.SmartHome.Devices;

namespace GoFPatterns.Command.SmartHome
{
    public class DemonstrateSmartHome
    {
        public void Demonstrate()
        {
            Console.WriteLine("=== Command: Smart home remote ===\n");

            var livingRoom = new Light("Living room");
            var kitchen = new Light("Kitchen");
            var thermostat = new Thermostat();
            var security = new SecuritySystem();

            var livingRoomOn = new ActionCommand("Living room light on", livingRoom.TurnOn);
            var allLightsOff = new MacroCommand("All lights off",
                new ActionCommand("Living room off", livingRoom.TurnOff),
                new ActionCommand("Kitchen off", kitchen.TurnOff));

            // "Leave home" scenario is built from other commands, including another macro.
            var leaveHome = new MacroCommand("Leave home",
                allLightsOff,
                new ActionCommand("Eco temperature", () => thermostat.SetTemperature(17)),
                new ActionCommand("Arm security", security.Arm));

            var comeHome = new MacroCommand("Come home",
                new ActionCommand("Disarm security", security.Disarm),
                new ActionCommand("Comfort temperature", () => thermostat.SetTemperature(22)),
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

            // Commands are objects, so they can be stored and executed later.
            Console.WriteLine("\nScheduling commands for the night:");
            remote.Schedule(allLightsOff);
            remote.Schedule(new ActionCommand("Night temperature", () => thermostat.SetTemperature(19)));
            Console.WriteLine("  ...it's 23:00...");
            remote.RunScheduled();
        }
    }
}
