🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Adapter) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Adapter) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Adapter) | **Italiano** | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Adapter)

# Adapter (Adattatore)

**Tipo:** pattern strutturale.

**Adapter** permette a oggetti con **interfacce incompatibili** di lavorare insieme. L'adapter è un «adattatore»: avvolge un oggetto esistente e offre al client l'interfaccia che il client si aspetta.

Esempi dalla vita reale: l'adattatore per la presa elettrica di un altro paese, USB → HDMI, un convertitore di formati di file.

---

## Quale problema risolve

Un sistema di monitoraggio della temperatura lavora con i sensori tramite l'interfaccia `ITemperatureSensor`, che restituisce la temperatura **in gradi Celsius**. Tutti i nuovi sensori la implementano.

Ma esiste un vecchio sensore `FahrenheitSensor` (codice legacy o libreria di terze parti):
- ha un **nome di metodo diverso**: `GetCurrentTemperature()`;
- restituisce la temperatura **in Fahrenheit**;
- il suo codice non si può modificare (non è nostro, oppure altro codice dipende da esso).

Il client non può semplicemente usare questo sensore: le interfacce sono incompatibili. Riscrivere il client per ogni sensore «speciale» è una cattiva idea: nel client comparirebbero `if (sensor is FahrenheitSensor)` e formule di conversione.

## Soluzione

Creare una classe **adapter** che:
- implementa **l'interfaccia richiesta dal client** (`ITemperatureSensor`);
- mantiene un riferimento all'**oggetto da adattare** (`FahrenheitSensor`);
- nei metodi dell'interfaccia chiama i metodi dell'oggetto adattato e **converte** i dati (Fahrenheit → Celsius).

Il client lavora con l'adapter come con qualsiasi altro sensore e non sa che all'interno c'è un vecchio dispositivo.

---

## Partecipanti

| Ruolo | Classe nell'esempio | Descrizione |
|-------|---------------------|-------------|
| **Target** | `ITemperatureSensor` | L'interfaccia che il client si aspetta |
| **Adaptee** | `FahrenheitSensor` | Classe esistente con un'interfaccia incompatibile |
| **Adapter** | `FahrenheitSensorAdapter` | Implementa Target e traduce le chiamate in chiamate all'Adaptee |
| **Client** | `DemonstarteAdapter` (metodo `DisplayTemperature`) | Lavora solo con Target |
| *(implementazione normale di Target)* | `CelsiusSensor` | Sensore moderno: non ha bisogno di un adapter |

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
                └──────────────┘   │ GetTemperatureCelsius(): │       │ (Adaptee, legacy)      │
                                   │  (F - 32) * 5 / 9        │       │ GetCurrentTemperature()│
                                   └──────────────────────────┘       └────────────────────────┘
```

---

## Esempio. Sensori di temperatura (`SensorFeature/`)

**Target**: `ITemperatureSensor`:

```csharp
public interface ITemperatureSensor
{
    decimal GetTemperatureCelsius();
}
```

**Implementazione normale di Target**: `CelsiusSensor`, un sensore moderno che implementa direttamente l'interfaccia (la temperatura è emulata con un numero casuale tra 15 e 35 °C).

**Adaptee**: `FahrenheitSensor`, un vecchio sensore con il proprio metodo `GetCurrentTemperature()`, che restituisce gradi Fahrenheit (emulazione 55–65 °F). **Non** implementa l'interfaccia `ITemperatureSensor` e noi non lo modifichiamo.

**Adapter**: `FahrenheitSensorAdapter` riceve il vecchio sensore tramite primary constructor, implementa `ITemperatureSensor` ed esegue la conversione delle unità:

```csharp
public class FahrenheitSensorAdapter(FahrenheitSensor fahrenheitSensor) : ITemperatureSensor
{
    private readonly FahrenheitSensor _fahrenheitSensor = fahrenheitSensor;

    public decimal GetTemperatureCelsius() =>
        (_fahrenheitSensor.GetCurrentTemperature() - 32m) * 5m / 9m;
}
```

L'adapter converte due cose contemporaneamente: il **nome del metodo** (`GetCurrentTemperature` → `GetTemperatureCelsius`) e il **formato dei dati** (°F → °C).

**Client**: `DemonstarteAdapter`:
1. mostra che `CelsiusSensor` funziona direttamente;
2. mostra il valore «grezzo» del vecchio sensore in Fahrenheit, incompatibile con il client;
3. avvolge il vecchio sensore: `ITemperatureSensor adapter = new FahrenheitSensorAdapter(fahrenheitSensor);`
4. passa **entrambi** i sensori allo stesso metodo `DisplayTemperature(ITemperatureSensor sensor)`, che valuta la temperatura (freddo / confortevole / caldo) e non sa nulla dell'adapter.

---

## Object adapter e class adapter

- **Object adapter** (come nell'esempio): usa la **composizione**, cioè mantiene un riferimento all'oggetto adattato. Funziona con qualsiasi sottoclasse dell'Adaptee. È la variante principale in C#.
- **Class adapter**: **eredita** dall'Adaptee e implementa Target. In C# è possibile solo se Target è un'interfaccia (non esiste l'ereditarietà multipla di classi) e lega l'adapter a una classe concreta.

## Quando usarlo

- Bisogna usare una classe esistente, ma la sua interfaccia non è adatta.
- Si lavora con codice legacy che non può essere modificato.
- Integrazione di librerie di terze parti e API esterne con la propria interfaccia (ad esempio, il proprio `IPaymentGateway` sopra l'SDK di un sistema di pagamento).
- Bisogna ricondurre diverse classi eterogenee a un'interfaccia unica.

## Quando non serve

- La classe originale può essere semplicemente modificata.
- L'interfaccia non va «tradotta» ma semplificata per un intero sottosistema: questo è il compito della Facade.

## Pro e contro

**Pro**
- Separa la conversione dell'interfaccia dalla logica di business (SRP).
- Nuovi adapter si aggiungono senza modificare il client (Open/Closed).
- Permette di riutilizzare codice esistente.

**Contro**
- Più classi.
- Un livello di indirezione in più.

## Relazioni con altri pattern

| Pattern | Differenza rispetto ad Adapter |
|---------|--------------------------------|
| **Bridge** | Viene progettato in anticipo, perché le parti evolvano indipendentemente; Adapter si applica a codice già esistente |
| **Decorator** | Mantiene l'interfaccia e aggiunge comportamento; Adapter cambia l'interfaccia |
| **Proxy** | Mantiene la stessa interfaccia e controlla l'accesso; Adapter offre un'interfaccia diversa |
| **Facade** | Crea una nuova interfaccia semplificata per un intero sottosistema; Adapter adatta un'unica interfaccia esistente |

## Esempi in .NET

- `StreamReader` / `StreamWriter`: adattano lo `Stream` di byte al lavoro con il testo.
- `SqlDataAdapter`: adatta una sorgente dati a un `DataSet`.
- I provider di logging (Serilog, NLog) per `Microsoft.Extensions.Logging.ILogger`: adapter di logger di terze parti verso l'interfaccia standard.
