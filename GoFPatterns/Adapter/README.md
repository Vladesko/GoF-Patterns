🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Adapter) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Adapter) | **Deutsch** | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Adapter) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Adapter)

# Adapter (Adapter)

**Typ:** Strukturmuster.

**Adapter** ermöglicht die Zusammenarbeit von Objekten mit **inkompatiblen Schnittstellen**. Ein Adapter ist ein „Zwischenstecker“: Er umhüllt ein vorhandenes Objekt und bietet dem Client die Schnittstelle, die der Client erwartet.

Beispiele aus dem Alltag: ein Reisestecker für die Steckdose eines anderen Landes, USB → HDMI, ein Dateiformatkonverter.

---

## Welches Problem wird gelöst

Ein Temperaturüberwachungssystem arbeitet mit Sensoren über die Schnittstelle `ITemperatureSensor`, die die Temperatur **in Grad Celsius** liefert. Alle neuen Sensoren implementieren sie.

Es gibt aber einen alten Sensor `FahrenheitSensor` (Legacy-Code oder eine Fremdbibliothek):
- er hat einen **anderen Methodennamen** – `GetCurrentTemperature()`;
- er liefert die Temperatur **in Fahrenheit**;
- sein Code darf nicht geändert werden (er gehört jemand anderem oder anderer Code hängt von ihm ab).

Der Client kann diesen Sensor nicht einfach verwenden: Die Schnittstellen sind inkompatibel. Den Client für jeden „besonderen“ Sensor umzuschreiben ist eine schlechte Idee: Im Client tauchen dann `if (sensor is FahrenheitSensor)` und Umrechnungsformeln auf.

## Lösung

Man erstellt eine **Adapter**-Klasse, die:
- die **vom Client benötigte Schnittstelle** implementiert (`ITemperatureSensor`);
- eine Referenz auf das **adaptierte Objekt** hält (`FahrenheitSensor`);
- in den Methoden der Schnittstelle die Methoden des adaptierten Objekts aufruft und die Daten **umwandelt** (Fahrenheit → Celsius).

Der Client arbeitet mit dem Adapter wie mit jedem anderen Sensor und weiß nicht, dass dahinter ein altes Gerät steckt.

---

## Beteiligte

| Rolle | Klasse im Beispiel | Beschreibung |
|------|-----------------|----------|
| **Target** | `ITemperatureSensor` | Die Schnittstelle, die der Client erwartet |
| **Adaptee** | `FahrenheitSensor` | Vorhandene Klasse mit inkompatibler Schnittstelle |
| **Adapter** | `FahrenheitSensorAdapter` | Implementiert Target und übersetzt Aufrufe in Aufrufe an Adaptee |
| **Client** | `DemonstarteAdapter` (Methode `DisplayTemperature`) | Arbeitet nur mit Target |
| *(gewöhnliche Implementierung von Target)* | `CelsiusSensor` | Moderner Sensor – braucht keinen Adapter |

## Schema

```text
                     ┌─────────────────────────────┐
 Client ────────────►│ «interface» ITemperatureSensor│
 DisplayTemperature  │ GetTemperatureCelsius()     │
                     └─────────────────────────────┘
                          ▲                  ▲
                          │                  │
                ┌──────────────┐   ┌──────────────────────────┐       ┌────────────────────────┐
                │ CelsiusSensor│   │ FahrenheitSensorAdapter  │──────►│ FahrenheitSensor       │
                └──────────────┘   │ GetTemperatureCelsius(): │       │ (Adaptee, Legacy)      │
                                   │  (F - 32) * 5 / 9        │       │ GetCurrentTemperature()│
                                   └──────────────────────────┘       └────────────────────────┘
```

---

## Beispiel. Temperatursensoren (`SensorFeature/`)

**Target** – `ITemperatureSensor`:

```csharp
public interface ITemperatureSensor
{
    decimal GetTemperatureCelsius();
}
```

**Gewöhnliche Implementierung von Target** – `CelsiusSensor`: ein moderner Sensor, der die Schnittstelle direkt implementiert (die Temperatur wird durch eine Zufallszahl von 15–35 °C emuliert).

**Adaptee** – `FahrenheitSensor`: ein alter Sensor mit eigener Methode `GetCurrentTemperature()`, die Fahrenheit liefert (Emulation 55–65 °F). Die Schnittstelle `ITemperatureSensor` implementiert er **nicht**, und wir ändern ihn auch nicht.

**Adapter** – `FahrenheitSensorAdapter` erhält den alten Sensor über einen Primärkonstruktor, implementiert `ITemperatureSensor` und rechnet die Einheiten um:

```csharp
public class FahrenheitSensorAdapter(FahrenheitSensor fahrenheitSensor) : ITemperatureSensor
{
    private readonly FahrenheitSensor _fahrenheitSensor = fahrenheitSensor;

    public decimal GetTemperatureCelsius() =>
        (_fahrenheitSensor.GetCurrentTemperature() - 32m) * 5m / 9m;
}
```

Der Adapter wandelt zwei Dinge auf einmal um: den **Methodennamen** (`GetCurrentTemperature` → `GetTemperatureCelsius`) und das **Datenformat** (°F → °C).

**Client** – `DemonstarteAdapter`:
1. zeigt, dass `CelsiusSensor` direkt funktioniert;
2. zeigt den „rohen“ Wert des alten Sensors in Fahrenheit – mit dem Client ist er inkompatibel;
3. umhüllt den alten Sensor: `ITemperatureSensor adapter = new FahrenheitSensorAdapter(fahrenheitSensor);`
4. übergibt **beide** Sensoren an dieselbe Methode `DisplayTemperature(ITemperatureSensor sensor)`, die die Temperatur bewertet (kalt / angenehm / heiß) und nichts vom Adapter weiß.

---

## Objektadapter und Klassenadapter

- **Objektadapter** (wie im Beispiel) – nutzt **Komposition**: hält eine Referenz auf das adaptierte Objekt. Funktioniert mit allen Unterklassen von Adaptee. Die Standardvariante in C#.
- **Klassenadapter** – **erbt** von Adaptee und implementiert Target. In C# nur möglich, wenn Target eine Schnittstelle ist (es gibt keine Mehrfachvererbung von Klassen), und bindet den Adapter an eine konkrete Klasse.

## Wann einsetzen

- Eine vorhandene Klasse soll verwendet werden, aber ihre Schnittstelle passt nicht.
- Arbeit mit Legacy-Code, der nicht geändert werden darf.
- Integration von Fremdbibliotheken und externen APIs mit eigener Schnittstelle (z. B. ein eigenes `IPaymentGateway` über dem SDK eines Zahlungssystems).
- Mehrere heterogene Klassen sollen auf eine einheitliche Schnittstelle gebracht werden.

## Wann nicht nötig

- Die ursprüngliche Klasse kann einfach geändert werden.
- Die Schnittstelle soll nicht „übersetzt“, sondern für ein ganzes Subsystem vereinfacht werden – das ist die Aufgabe von Facade.

## Vor- und Nachteile

**Vorteile**
- Trennt die Umwandlung der Schnittstelle von der Geschäftslogik (SRP).
- Neue Adapter kommen ohne Änderung am Client hinzu (Open/Closed).
- Ermöglicht die Wiederverwendung vorhandenen Codes.

**Nachteile**
- Mehr Klassen.
- Eine zusätzliche Indirektionsebene.

## Beziehung zu anderen Mustern

| Muster | Unterschied zu Adapter |
|---------|--------------------|
| **Bridge** | Wird von vornherein entworfen, damit sich die Teile unabhängig entwickeln; Adapter wird auf bereits vorhandenen Code angewendet |
| **Decorator** | Behält die Schnittstelle bei und ergänzt Verhalten; Adapter ändert die Schnittstelle |
| **Proxy** | Behält dieselbe Schnittstelle bei und kontrolliert den Zugriff; Adapter bietet eine andere Schnittstelle |
| **Facade** | Schafft eine neue, vereinfachte Schnittstelle für ein ganzes Subsystem; Adapter passt eine einzelne vorhandene an |

## Beispiele in .NET

- `StreamReader` / `StreamWriter` – passen den bytebasierten `Stream` an die Arbeit mit Text an.
- `SqlDataAdapter` – passt eine Datenquelle an ein `DataSet` an.
- Logging-Provider (Serilog, NLog) für `Microsoft.Extensions.Logging.ILogger` – Adapter für fremde Logger an die Standardschnittstelle.
