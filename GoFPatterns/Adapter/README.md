🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Adapter) | **English** | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Adapter) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Adapter) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Adapter)

# Adapter

**Type:** structural pattern.

**Adapter** allows objects with **incompatible interfaces** to work together. An adapter is a "plug converter": it wraps an existing object and provides the client with the interface the client expects.

Real-life examples: a plug adapter for another country's socket, USB → HDMI, a file format converter.

---

## The problem

A temperature monitoring system works with sensors through the `ITemperatureSensor` interface, which returns the temperature **in degrees Celsius**. All new sensors implement it.

But there is an old sensor, `FahrenheitSensor` (legacy code or a third-party library):
- it has a **different method name** — `GetCurrentTemperature()`;
- it returns the temperature **in Fahrenheit**;
- its code cannot be changed (it belongs to someone else, or other code depends on it).

The client cannot simply use this sensor: the interfaces are incompatible. Rewriting the client for every "special" sensor is a bad idea: the client would fill up with `if (sensor is FahrenheitSensor)` checks and conversion formulas.

## Solution

Create an **adapter** class that:
- implements the **interface the client needs** (`ITemperatureSensor`);
- holds a reference to the **adaptee** (`FahrenheitSensor`);
- in the interface methods calls the adaptee's methods and **converts** the data (Fahrenheit → Celsius).

The client works with the adapter like with any other sensor and does not know that there is an old device inside.

---

## Participants

| Role | Class in the example | Description |
|------|----------------------|-------------|
| **Target** | `ITemperatureSensor` | The interface the client expects |
| **Adaptee** | `FahrenheitSensor` | An existing class with an incompatible interface |
| **Adapter** | `FahrenheitSensorAdapter` | Implements Target and translates calls into Adaptee calls |
| **Client** | `DemonstarteAdapter` (method `DisplayTemperature`) | Works only with Target |
| *(regular Target implementation)* | `CelsiusSensor` | A modern sensor — it needs no adapter |

## Diagram

```text
                     ┌─────────────────────────────┐
 Client ────────────►│ «interface» ITemperatureSensor│
 DisplayTemperature  │ GetTemperatureCelsius()     │
                     └─────────────────────────────┘
                          ▲                  ▲
                          │                  │
                ┌──────────────┐   ┌──────────────────────────┐       ┌────────────────────────┐
                │ CelsiusSensor│   │ FahrenheitSensorAdapter  │──────►│ FahrenheitSensor       │
                └──────────────┘   │ GetTemperatureCelsius(): │       │ (Adaptee, legacy)      │
                                   │  (F - 32) * 5 / 9        │       │ GetCurrentTemperature()│
                                   └──────────────────────────┘       └────────────────────────┘
```

---

## Example. Temperature sensors (`SensorFeature/`)

**Target** — `ITemperatureSensor`:

```csharp
public interface ITemperatureSensor
{
    decimal GetTemperatureCelsius();
}
```

**Regular Target implementation** — `CelsiusSensor`: a modern sensor that implements the interface directly (the temperature is emulated with a random number in the 15–35 °C range).

**Adaptee** — `FahrenheitSensor`: an old sensor with its own `GetCurrentTemperature()` method returning Fahrenheit (emulated 55–65 °F). It does **not** implement `ITemperatureSensor`, and we do not change it.

**Adapter** — `FahrenheitSensorAdapter` receives the old sensor through a primary constructor, implements `ITemperatureSensor` and converts the units:

```csharp
public class FahrenheitSensorAdapter(FahrenheitSensor fahrenheitSensor) : ITemperatureSensor
{
    private readonly FahrenheitSensor _fahrenheitSensor = fahrenheitSensor;

    public decimal GetTemperatureCelsius() =>
        (_fahrenheitSensor.GetCurrentTemperature() - 32m) * 5m / 9m;
}
```

The adapter converts two things at once: the **method name** (`GetCurrentTemperature` → `GetTemperatureCelsius`) and the **data format** (°F → °C).

**Client** — `DemonstarteAdapter`:
1. shows that `CelsiusSensor` works directly;
2. shows the "raw" Fahrenheit value of the old sensor — it is incompatible with the client;
3. wraps the old sensor: `ITemperatureSensor adapter = new FahrenheitSensorAdapter(fahrenheitSensor);`
4. passes **both** sensors to the same `DisplayTemperature(ITemperatureSensor sensor)` method, which rates the temperature (cold / comfortable / hot) and knows nothing about the adapter.

---

## Object adapter vs. class adapter

- **Object adapter** (as in the example) — uses **composition**: it holds a reference to the adaptee. It works with any Adaptee subclass. The main option in C#.
- **Class adapter** — **inherits** from Adaptee and implements Target. In C# it is possible only if Target is an interface (there is no multiple class inheritance), and it ties the adapter to a specific class.

## When to use

- You need to use an existing class, but its interface does not fit.
- Working with legacy code that cannot be changed.
- Integrating third-party libraries and external APIs behind your own interface (e.g. your own `IPaymentGateway` on top of a payment system SDK).
- You need to bring several heterogeneous classes to a single interface.

## When not to use

- The original class can simply be modified.
- The interface does not need to be "translated" but simplified for a whole subsystem — that is the job of Facade.

## Pros and cons

**Pros**
- Separates interface conversion from business logic (SRP).
- New adapters are added without changing the client (Open/Closed).
- Allows reusing existing code.

**Cons**
- More classes.
- An extra level of indirection.

## Relations with other patterns

| Pattern | Difference from Adapter |
|---------|-------------------------|
| **Bridge** | Designed up front so that the parts can evolve independently; Adapter is applied to already existing code |
| **Decorator** | Keeps the interface and adds behavior; Adapter changes the interface |
| **Proxy** | Keeps the same interface and controls access; Adapter provides a different interface |
| **Facade** | Creates a new simplified interface for a whole subsystem; Adapter adapts one existing interface |

## Examples in .NET

- `StreamReader` / `StreamWriter` — adapt a byte `Stream` for working with text.
- `SqlDataAdapter` — adapts a data source to a `DataSet`.
- Logging providers (Serilog, NLog) for `Microsoft.Extensions.Logging.ILogger` — adapters of third-party loggers to the standard interface.
