🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Singleton) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Singleton) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Singleton) | **Italiano** | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Singleton)

# Singleton (Istanza unica)

**Tipo:** pattern creazionale.

**Singleton** garantisce che una classe abbia **una sola istanza** e fornisce un **punto di accesso globale** a essa.

---

## Quale problema risolve

1. **Un'unica istanza.** Ci sono risorse che devono esistere in un'unica istanza per tutta l'applicazione: configurazione, logger, pool di connessioni, cache. Se ogni servizio creasse la propria configurazione, il file verrebbe letto molte volte e le modifiche fatte in un punto non sarebbero visibili altrove.
2. **Accesso globale.** A questa istanza bisogna accedere da diverse parti del programma, senza però permettere di sovrascriverla come una normale variabile globale.

## Soluzione

- Rendere il costruttore **privato**, così nessuno può chiamare `new`.
- Conservare l'unica istanza in un **campo statico**.
- Fornirla tramite una **proprietà statica** `Instance`, che crea l'oggetto al primo accesso (inizializzazione lazy).
- Rendere la classe `sealed`, così il vincolo non si può aggirare tramite una sottoclasse.

La difficoltà principale è il **multithreading**: se due thread accedono contemporaneamente a `Instance`, un'implementazione ingenua crea due oggetti.

---

## Partecipanti

| Ruolo | Classe nell'esempio | Descrizione |
|-------|---------------------|-------------|
| **Singleton** | `AppConfiguration`, `FileLogger` | Conserva l'unica istanza, nasconde il costruttore, fornisce l'istanza tramite `Instance` |
| **Client** | `CartService`, `PriceService`, classi demo | Accede al singleton tramite `ClassName.Instance` |

In sostanza il pattern ha un solo «attore»: la classe singleton stessa.

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
     CartService       PriceService       ← lo stesso oggetto
```

---

## Esempio 1. Configurazione dell'applicazione (`Configuration/`): con `Lazy<T>`

**Problema:** le impostazioni (stringa di connessione, valuta, limite del carrello) vengono caricate da un file una sola volta e usate da diversi servizi.

**Singleton**: `AppConfiguration`:

```csharp
public sealed class AppConfiguration
{
    private static readonly Lazy<AppConfiguration> _instance = new(() => new AppConfiguration());

    public static AppConfiguration Instance => _instance.Value;

    private AppConfiguration() { /* caricamento costoso delle impostazioni */ }
}
```

Di default `Lazy<T>` lavora in modalità `ExecutionAndPublication`: la factory viene chiamata **esattamente una volta**, anche se più thread accedono contemporaneamente a `Value`. È il modo consigliato in C#.

**Clients**: `CartService` e `PriceService` (cartella `Services/`) non sono collegati tra loro, ma entrambi ottengono `AppConfiguration.Instance`. Nella demo si vede che:
- il messaggio di caricamento compare solo al primo accesso (inizializzazione lazy);
- l'`InstanceId` è lo stesso per entrambi i servizi;
- la modifica della valuta con `Set` è subito visibile in `PriceService`;
- 100 accessi paralleli producono 1 sola istanza unica.

## Esempio 2. Logger (`Logger/`): thread safety manuale

**Problema:** mostrare perché un'implementazione ingenua è pericolosa e come funziona il classico lock con doppio controllo.

**Esempio negativo**: `NaiveLogger`:

```csharp
if (_instance is null)
{
    _instance = new NaiveLogger();   // due thread possono trovarsi qui contemporaneamente
}
```

Il costruttore lento (`Thread.Sleep`) rende evidente la race condition: la demo avvia 10 thread e vengono create più istanze invece di una.

**Singleton**: `FileLogger` con **double-checked locking**:

```csharp
if (_instance is null)              // 1° controllo: veloce, senza lock
{
    lock (_lock)
    {
        if (_instance is null)      // 2° controllo: un altro thread potrebbe aver già creato l'oggetto
        {
            _instance = new FileLogger();
        }
    }
}
```

Il campo è marcato `volatile`, così gli altri thread non vedono un oggetto costruito solo in parte. Il contatore `CreatedInstances` mostra che è stata creata esattamente un'istanza, e tutti i messaggi dei diversi thread finiscono nello stesso logger.

---

## Varianti di implementazione in C#

| Modo | Thread safety | Lazy | Commento |
|------|---------------|------|----------|
| `if (_instance == null)` senza lock | ❌ | ✅ | Solo per codice single-thread |
| `lock` + doppio controllo | ✅ | ✅ | Classico, ma è facile sbagliare |
| `static readonly Instance = new()` | ✅ | quasi | Creata al primo accesso alla classe (garantito dal CLR) |
| `Lazy<T>` | ✅ | ✅ | Il modo consigliato |
| Container DI `AddSingleton<T>()` | ✅ | ✅ | In ASP.NET Core è preferibile a un Singleton fatto a mano |

## Quando usarlo

- L'oggetto deve davvero essere uno solo (configurazione, logger, cache, pool).
- Serve una garanzia rigorosa di unicità, non un semplice «accordo di crearne uno solo».

## Quando non serve

- Se c'è un container DI, è meglio registrare il servizio come singleton (`services.AddSingleton<IConfig, AppConfig>()`) e ottenerlo tramite un'interfaccia: è la stessa unicità, ma senza stato globale e con la possibilità di sostituirlo nei test.
- Se il «singleton» serve solo per non passare una dipendenza come parametro, è una dipendenza nascosta, non un motivo per usare il pattern.

## Pro e contro

**Pro**
- Garanzia di un'unica istanza.
- Punto di accesso globale.
- Inizializzazione lazy: l'oggetto viene creato solo quando serve.

**Contro**
- Viola l'SRP: la classe svolge il proprio lavoro e gestisce anche il proprio ciclo di vita.
- Dipendenze nascoste: dalla firma di `CartService` non si vede che dipende dalla configurazione.
- Difficile da testare: lo stato globale sopravvive tra i test e non si può sostituire con un mock.
- Richiede attenzione in un ambiente multithread.

## Relazioni con altri pattern

- **Facade** è spesso un singleton: di solito basta una sola facade.
- **Abstract Factory**, **Builder**, **Prototype** (registro dei prototipi) possono essere implementati come Singleton.
- **Flyweight** è simile perché condivide oggetti, ma può avere molte istanze con stato intrinseco diverso, e sono immutabili; il Singleton è esattamente uno e può essere mutabile.
