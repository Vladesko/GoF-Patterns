🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Decorator) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Decorator) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Decorator) | **Italiano** | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Decorator)

# Decorator (Decoratore)

**Tipo:** pattern strutturale.

**Decorator** permette di aggiungere dinamicamente un nuovo comportamento a un oggetto, «avvolgendolo» in oggetti wrapper con la stessa interfaccia. I wrapper si possono annidare l'uno nell'altro in qualsiasi ordine e quantità.

---

## Quale problema risolve

Una caffetteria vende bevande a cui si possono aggiungere latte, sciroppo, formato grande... Se si risolve con l'ereditarietà:

```text
Espresso
 ├── EspressoWithMilk
 ├── EspressoWithSyrup
 ├── EspressoWithMilkAndSyrup
 ├── LargeEspressoWithMilkAndSyrup
 └── ...  (e tutto lo stesso per il tè)
```

Il numero di sottoclassi cresce in modo combinatorio. Inoltre l'ereditarietà è **statica**: durante l'esecuzione del programma non si può «aggiungere latte» a un oggetto già creato. E il doppio latte non si può proprio esprimere senza un'altra classe.

Lo stesso vale per un archivio dati: a volte serve la compressione, a volte la cifratura, a volte entrambe, a volte anche il logging: non si possono creare sottoclassi per ogni combinazione.

## Soluzione

Sostituire l'ereditarietà con la **composizione**. Il decorator:
- implementa **la stessa interfaccia** dell'oggetto avvolto;
- mantiene un riferimento all'oggetto avvolto;
- gli delega le chiamate, aggiungendo il proprio comportamento **prima o dopo**.

Poiché per il client il decorator appare come un normale componente, può essere avvolto da un altro decorator: si ottiene una «matrioska». Il client non sa se sta lavorando con l'oggetto originale o con una catena di wrapper.

---

## Partecipanti

| Ruolo | Classe nell'esempio | Descrizione |
|-------|---------------------|-------------|
| **Component** | `IBeverage`, `IDataSource` | Interfaccia comune sia per i wrapper sia per gli oggetti avvolti |
| **ConcreteComponent** | `Espresso`, `BlackTea`, `InMemoryDataSource` | L'oggetto base a cui si aggiunge comportamento |
| **BaseDecorator** | `BeverageDecorator`, `DataSourceDecorator` | Mantiene un riferimento al Component e di default gli delega semplicemente le chiamate |
| **ConcreteDecorator** | `MilkDecorator`, `SyrupDecorator`, `LargeSizeDecorator`, `CompressionDecorator`, `EncryptionDecorator`, `LoggingDecorator` | Aggiungono un comportamento specifico prima/dopo la chiamata dell'oggetto avvolto |
| **Client** | classi demo | Assembla la combinazione di wrapper desiderata e ci lavora tramite il Component |

## Schema

```text
            ┌───────────────────────┐
 Client ───►│ «interface» IBeverage │◄─────────────────┐
            │ GetDescription()      │                  │ avvolge
            │ GetCost()             │                  │
            └───────────────────────┘                  │
               ▲                 ▲                     │
               │                 │                     │
          Espresso    ┌──────────────────────────┐     │
          BlackTea    │ «abstract» BeverageDecorator│◆──┘
                      │ - Beverage: IBeverage    │
                      └──────────────────────────┘
                         ▲          ▲           ▲
                   MilkDecorator  SyrupDecorator  LargeSizeDecorator

  new LargeSize( new Syrup( new Milk( new Espresso() ) ) )
       │             │          │          └─ 2.00
       │             │          └─ +0.50
       │             └─ +0.70
       └─ ×1.5
```

---

## Esempio 1. Caffetteria (`Coffee/`)

**Component**: `IBeverage` con i metodi `GetDescription()` e `GetCost()`.

**ConcreteComponent**: `Espresso` e `BlackTea` (cartella `Beverages/`) conoscono solo il proprio prezzo base.

**BaseDecorator**: `BeverageDecorator` (cartella `AddOns/`) conserva la bevanda avvolta e le delega tutto tramite metodi `virtual`:

```csharp
public abstract class BeverageDecorator(IBeverage beverage) : IBeverage
{
    protected IBeverage Beverage { get; } = beverage;

    public virtual string GetDescription() => Beverage.GetDescription();
    public virtual decimal GetCost() => Beverage.GetCost();
}
```

**ConcreteDecorator**: ogni extra ridefinisce i metodi, chiamando `base` e completando il risultato:

```csharp
public class MilkDecorator(IBeverage beverage) : BeverageDecorator(beverage)
{
    public override string GetDescription() => $"{base.GetDescription()} + latte";
    public override decimal GetCost() => base.GetCost() + 0.50m;
}
```

- `SyrupDecorator` mostra che un decorator può avere **un proprio stato** (il gusto dello sciroppo).
- `LargeSizeDecorator` non somma ma **moltiplica** il prezzo. Per questo nella demo si vede che **l'ordine dei wrapper conta**: «grande (espresso + sciroppo)» costa più di «espresso grande + sciroppo».

**Client**: assembla le bevande «a matrioska»: `new MilkDecorator(new MilkDecorator(new Espresso()))` è un doppio latte senza una classe apposita. Gli stessi extra funzionano senza modifiche anche con il tè.

## Esempio 2. Sorgente dati (`DataStream/`)

**Problema:** i dati (ad esempio i dati di un cliente con il numero della carta) vengono salvati in un archivio. A seconda delle impostazioni bisogna comprimerli, cifrarli, registrarli nel log, in qualsiasi combinazione.

**Component**: `IDataSource` con `Write`/`Read`.

**ConcreteComponent**: `InMemoryDataSource` conserva la stringa così com'è (simulazione di un file o di un record di database).

**BaseDecorator**: `DataSourceDecorator` (cartella `Decorators/`).

**ConcreteDecorator**: qui si vede bene il comportamento «prima» e «dopo»:

```csharp
// CompressionDecorator
public override void Write(string data) => base.Write(Compress(data));   // PRIMA della scrittura: comprimere
public override string Read() => Decompress(base.Read());                // DOPO la lettura: decomprimere
```

- `CompressionDecorator`: GZip + Base64.
- `EncryptionDecorator`: XOR con una chiave (solo a scopo dimostrativo, non è una vera cifratura!).
- `LoggingDecorator`: non modifica i dati, scrive solo la dimensione nella console.

**Client**: la demo avvolge l'archivio:

```csharp
IDataSource source = new LoggingDecorator(
    new CompressionDecorator(
        new EncryptionDecorator(protectedStorage, "secret")),
    "client");
```

In scrittura i dati passano dall'esterno verso l'interno (log → compressione → cifratura → archivio), in lettura dall'interno verso l'esterno. Nell'archivio c'è una stringa compressa e cifrata, mentre il client legge il testo originale senza nemmeno saperlo.

È esattamente così che sono strutturati gli stream in .NET: `new GZipStream(new CryptoStream(new FileStream(...)))` è un Decorator classico. Sono decorator anche `BufferedStream`, `StreamReader` (sopra uno `Stream`) e, in ASP.NET Core, la pipeline dei middleware e la decorazione dei servizi nella DI (ad esempio con Scrutor).

---

## Quando usarlo

- Bisogna aggiungere responsabilità agli oggetti **dinamicamente** e in modo trasparente per il client.
- L'estensione tramite ereditarietà è impossibile (la classe è `sealed`) o porta a un'esplosione del numero di sottoclassi.
- Bisogna combinare «sovrastrutture» indipendenti: cache, logging, tentativi ripetuti, convalida, metriche.

## Quando non serve

- Serve una sola combinazione fissa di comportamenti: è più semplice scrivere una classe.
- Il comportamento deve dipendere da quali wrapper sono già presenti (i decorator non dovrebbero conoscersi tra loro).

## Pro e contro

**Pro**
- Più flessibile dell'ereditarietà: il comportamento si aggiunge e si combina a runtime.
- Ogni decorator è responsabile di una sola cosa (SRP).
- Si può applicare più volte (doppio latte).
- Si possono estendere classi `sealed` e codice di terze parti tramite la loro interfaccia.

**Contro**
- Molte piccole classi.
- È difficile rimuovere un wrapper specifico dal centro della catena.
- Il comportamento dipende dall'ordine dei wrapper: è facile non accorgersene.
- Il codice di configurazione della «matrioska» può essere ingombrante.

## Relazioni con altri pattern

- **Adapter** cambia l'interfaccia di un oggetto; **Decorator** lascia la stessa interfaccia ed estende il comportamento; **Proxy** lascia la stessa interfaccia e **controlla l'accesso** (di solito gestisce da solo il ciclo di vita dell'oggetto, mentre il decorator riceve l'oggetto dal client).
- **Composite** è simile nella struttura (composizione ricorsiva), ma il decorator ha un solo oggetto annidato e lo scopo è aggiungere comportamento.
- Anche **Chain of Responsibility** passa la richiesta lungo una catena, ma i suoi anelli possono **interrompere** l'elaborazione; il decorator passa sempre la chiamata avanti.
- **Strategy** cambia le «viscere» di un oggetto, **Decorator** il suo «involucro».
