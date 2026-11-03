🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Singleton) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Singleton) | **Deutsch** | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Singleton) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Singleton)

# Singleton (Einzelstück)

**Typ:** Erzeugungsmuster.

**Singleton** garantiert, dass eine Klasse **nur eine einzige Instanz** hat, und stellt einen **globalen Zugriffspunkt** auf sie bereit.

---

## Welches Problem wird gelöst

1. **Eine Instanz.** Manche Ressourcen dürfen in der gesamten Anwendung nur einmal existieren: Konfiguration, Logger, Verbindungspool, Cache. Erzeugt jeder Dienst seine eigene Konfiguration, wird die Datei mehrfach gelesen, und Änderungen an einer Stelle sind an anderer Stelle nicht sichtbar.
2. **Globaler Zugriff.** Auf diese Instanz muss aus verschiedenen Teilen des Programms zugegriffen werden, ohne dass man sie wie eine gewöhnliche globale Variable überschreiben kann.

## Lösung

- Den Konstruktor **privat** machen, damit niemand `new` aufrufen kann.
- Die einzige Instanz in einem **statischen Feld** speichern.
- Sie über eine **statische Eigenschaft** `Instance` herausgeben, die das Objekt beim ersten Zugriff erzeugt (verzögerte Initialisierung).
- Die Klasse `sealed` machen, damit die Einschränkung nicht über eine Unterklasse umgangen werden kann.

Die Hauptschwierigkeit ist **Multithreading**: Greifen zwei Threads gleichzeitig auf `Instance` zu, erzeugt eine naive Implementierung zwei Objekte.

---

## Beteiligte

| Rolle | Klasse im Beispiel | Beschreibung |
|------|-----------------|----------|
| **Singleton** | `AppConfiguration`, `FileLogger` | Hält die einzige Instanz, verbirgt den Konstruktor, gibt die Instanz über `Instance` heraus |
| **Client** | `CartService`, `PriceService`, Demo-Klassen | Greift über `ClassName.Instance` auf das Singleton zu |

Das Muster hat im Grunde nur einen „Akteur“ – die Singleton-Klasse selbst.

## Schema

```text
 ┌───────────────────────────────────────┐
 │ AppConfiguration  «sealed»            │
 │ - static _instance: Lazy<AppConfig>   │
 │ - AppConfiguration()   ← private      │
 │ + static Instance: AppConfiguration   │
 │ + Get(key), Set(key, value)           │
 └───────────────────────────────────────┘
          ▲                 ▲
          │ .Instance       │ .Instance
     CartService       PriceService       ← ein und dasselbe Objekt
```

---

## Beispiel 1. Anwendungskonfiguration (`Configuration/`) – mit `Lazy<T>`

**Aufgabe:** Die Einstellungen (Verbindungszeichenfolge, Währung, Warenkorblimit) werden einmal aus einer Datei geladen und von verschiedenen Diensten genutzt.

**Singleton** – `AppConfiguration`:

```csharp
public sealed class AppConfiguration
{
    private static readonly Lazy<AppConfiguration> _instance = new(() => new AppConfiguration());

    public static AppConfiguration Instance => _instance.Value;

    private AppConfiguration() { /* aufwendiges Laden der Einstellungen */ }
}
```

`Lazy<T>` arbeitet standardmäßig im Modus `ExecutionAndPublication`: Die Fabrikmethode wird **genau einmal** aufgerufen, selbst wenn mehrere Threads gleichzeitig auf `Value` zugreifen. Das ist der empfohlene Weg in C#.

**Clients** – `CartService` und `PriceService` (Ordner `Services/`) haben nichts miteinander zu tun, erhalten aber beide `AppConfiguration.Instance`. In der Demo sieht man:
- die Meldung über das Laden erscheint nur beim ersten Zugriff (Lazy-Verhalten);
- die `InstanceId` ist bei beiden Diensten gleich;
- eine Änderung der Währung über `Set` ist sofort in `PriceService` sichtbar;
- 100 parallele Zugriffe ergeben 1 eindeutige Instanz.

## Beispiel 2. Logger (`Logger/`) – Threadsicherheit von Hand

**Aufgabe:** Zeigen, warum eine naive Implementierung gefährlich ist und wie das klassische Double-Checked Locking funktioniert.

**Negativbeispiel** – `NaiveLogger`:

```csharp
if (_instance is null)
{
    _instance = new NaiveLogger();   // zwei Threads können gleichzeitig hier landen
}
```

Ein langsamer Konstruktor (`Thread.Sleep`) macht die Race Condition sichtbar: Die Demo startet 10 Threads, und es entstehen mehrere Instanzen statt einer.

**Singleton** – `FileLogger` mit **Double-Checked Locking**:

```csharp
if (_instance is null)              // 1. Prüfung: schnell, ohne Sperre
{
    lock (_lock)
    {
        if (_instance is null)      // 2. Prüfung: ein anderer Thread könnte das Objekt inzwischen erzeugt haben
        {
            _instance = new FileLogger();
        }
    }
}
```

Das Feld ist als `volatile` markiert, damit andere Threads kein teilweise konstruiertes Objekt sehen. Der Zähler `CreatedInstances` zeigt, dass genau eine Instanz erzeugt wurde und alle Nachrichten aus verschiedenen Threads im selben Logger landen.

---

## Implementierungsvarianten in C#

| Variante | Threadsicherheit | Lazy | Kommentar |
|--------|-------------------|-----------|-------------|
| `if (_instance == null)` ohne lock | ❌ | ✅ | Nur für Single-Thread-Code |
| `lock` + doppelte Prüfung | ✅ | ✅ | Klassiker, aber fehleranfällig |
| `static readonly Instance = new()` | ✅ | fast | Wird beim ersten Zugriff auf die Klasse erzeugt (von der CLR garantiert) |
| `Lazy<T>` | ✅ | ✅ | Empfohlener Weg |
| DI-Container `AddSingleton<T>()` | ✅ | ✅ | In ASP.NET Core einem selbstgeschriebenen Singleton vorzuziehen |

## Wann einsetzen

- Das Objekt muss wirklich einzigartig sein (Konfiguration, Logger, Cache, Pool).
- Man braucht eine strikte Garantie der Einzigartigkeit, nicht nur die „Absprache, nur eins zu erzeugen“.

## Wann nicht nötig

- Gibt es einen DI-Container, registriert man den Dienst besser als Singleton (`services.AddSingleton<IConfig, AppConfig>()`) und bezieht ihn über die Schnittstelle: Das ist dieselbe Einzigartigkeit, aber ohne globalen Zustand und mit der Möglichkeit, sie in Tests zu ersetzen.
- Braucht man das „Singleton“ nur, um eine Abhängigkeit nicht als Parameter übergeben zu müssen, ist das eine versteckte Abhängigkeit und kein Grund für das Muster.

## Vor- und Nachteile

**Vorteile**
- Garantie einer einzigen Instanz.
- Globaler Zugriffspunkt.
- Verzögerte Initialisierung – das Objekt wird erst erzeugt, wenn es gebraucht wird.

**Nachteile**
- Verletzt das SRP: Die Klasse erledigt ihre Arbeit und verwaltet zugleich ihren eigenen Lebenszyklus.
- Versteckte Abhängigkeiten: An der Signatur von `CartService` sieht man nicht, dass er von der Konfiguration abhängt.
- Schwer zu testen: Globaler Zustand überdauert Tests und lässt sich nicht durch ein Mock ersetzen.
- Erfordert Sorgfalt in einer Multithreading-Umgebung.

## Beziehung zu anderen Mustern

- **Facade** wird oft als Singleton umgesetzt – meist genügt eine einzige Fassade.
- **Abstract Factory**, **Builder**, **Prototype** (Prototyp-Registry) können als Singleton implementiert werden.
- **Flyweight** ähnelt ihm darin, dass Objekte geteilt werden, kann aber viele Instanzen mit unterschiedlichem intrinsischem Zustand haben, und diese sind unveränderlich; ein Singleton gibt es genau einmal, und es kann veränderlich sein.
