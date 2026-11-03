🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/TemplateMethod) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/TemplateMethod) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/TemplateMethod) | **Italiano** | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/TemplateMethod)

# Template Method (Metodo modello)

**Tipo:** pattern comportamentale.

**Template Method** definisce lo **scheletro di un algoritmo** in un metodo della classe base, delegando alle sottoclassi l'implementazione di singoli passi. Le sottoclassi possono ridefinire i passi, ma **non possono cambiare la struttura** dell'algoritmo.

---

## Quale problema risolve

Bisogna importare clienti da CSV e da JSON. Il processo è lo stesso in entrambi i casi: preparare i dati → analizzarli → verificarli → salvarli → (fare qualcosa dopo l'importazione). Cambia solo il passo di **analisi** (parsing).

Se si scrivono due classi indipendenti `CsvImporter` e `JsonImporter`:
- la verifica e il salvataggio sono **duplicati**;
- si corregge un errore nella convalida in una classe e ci si dimentica dell'altra;
- quando arriva l'XML bisogna copiare tutto una terza volta;
- nessuno garantisce che tutti gli importer eseguano i passi nello stesso ordine.

## Soluzione

Spostare l'algoritmo comune nella **classe base** sotto forma di un metodo (il template method) che chiama i passi in ordine. I passi sono di tre tipi:

- **astratti**: ogni sottoclasse deve implementarli (`Parse`);
- **comuni**: implementati nella classe base e uguali per tutti (`Save`);
- **hook**: metodi `virtual` con un'implementazione di default (spesso vuota), che la sottoclasse **può** ridefinire per «inserirsi» nell'algoritmo (`Preprocess`, `AfterImport`).

Il template method stesso **non è virtuale**: le sottoclassi non possono cambiare l'ordine dei passi.

---

## Partecipanti

| Ruolo | Classe nell'esempio | Descrizione |
|-------|---------------------|-------------|
| **AbstractClass** | `CustomerImporter`, `HotBeverage` | Contiene il template method (`Import`, `Prepare`) e dichiara i passi: astratti, comuni e hook |
| **ConcreteClass** | `CsvCustomerImporter`, `JsonCustomerImporter`, `PartnerCsvCustomerImporter`, `Tea`, `GreenTea`, `Coffee` | Implementa i passi astratti e, se serve, ridefinisce gli hook. Non tocca l'algoritmo in sé |

## Schema

```text
 ┌──────────────────────────────────────────┐
 │ «abstract» CustomerImporter              │
 │ + Import(raw)          ← template method │
 │     Preprocess()       ← hook (virtual)  │
 │     Parse()            ← abstract        │
 │     Validate()         ← virtual         │
 │     Save()             ← comune (private)│
 │     AfterImport()      ← hook (vuoto)    │
 └──────────────────────────────────────────┘
          ▲                         ▲
 ┌──────────────────────┐  ┌──────────────────────┐
 │ CsvCustomerImporter  │  │ JsonCustomerImporter │
 │ Preprocess(): salta  │  │ Parse(): JSON        │
 │   l'intestazione     │  │ AfterImport(): email │
 │ Parse(): CSV         │  └──────────────────────┘
 └──────────────────────┘
          ▲
 ┌────────────────────────────┐
 │ PartnerCsvCustomerImporter │
 │ Validate(): regole severe  │
 └────────────────────────────┘
```

---

## Esempio 1. Importazione dei clienti (`DataImport/`)

**AbstractClass**: `CustomerImporter`. Il template method `Import`:

```csharp
public IReadOnlyList<Customer> Import(string rawData)
{
    var content = Preprocess(rawData);          // hook
    var records = Parse(content);               // abstract
    var valid = records.Where(Validate).ToList();   // virtual
    Save(valid);                                // passo comune
    AfterImport(valid);                         // hook
    return valid;
}
```

- `Parse` è astratto: ogni formato ha il proprio.
- `Validate` è la verifica comune (l'e-mail contiene `@`, l'età è entro limiti ragionevoli), ma può essere resa più severa.
- `Save` è `private`: la sottoclasse non può né ridefinire né chiamare separatamente questo passo.
- `Preprocess` e `AfterImport` sono hook.

**ConcreteClass** (cartella `Importers/`):
- `CsvCustomerImporter`: analizza le righe separate da `;` e ridefinisce l'hook `Preprocess` per eliminare la riga di intestazione;
- `JsonCustomerImporter`: analizza il JSON con `System.Text.Json` e ridefinisce l'hook `AfterImport` (invio delle e-mail di benvenuto);
- `PartnerCsvCustomerImporter`: eredita dall'**importer CSV** e si limita a rendere più severo `Validate` (solo maggiorenni con e-mail aziendale), chiamando `base.Validate`. Il parsing CSV e l'intero algoritmo li ottiene gratis.

**Client**: la demo lavora con tutti gli importer tramite la classe base e chiama lo stesso `Import`. Dall'output si vede che i passi sono gli stessi e cambiano solo i dettagli.

## Esempio 2. Bevande calde (`Beverages/`): l'esempio classico con gli hook

**AbstractClass**: `HotBeverage` con il template method `Prepare()`:

```csharp
public void Prepare()
{
    BoilWater();
    Brew();
    PourInCup();
    if (CustomerWantsCondiments())
        AddCondiments();
}
```

- `BoilWater`, `PourInCup`: passi comuni;
- `Brew`, `AddCondiments`: astratti;
- `WaterTemperature` e `CustomerWantsCondiments()`: **hook**: di default 100°C e «sì, gli extra servono».

**ConcreteClass** (cartella `Drinks/`):
- `Tea`: solo i passi obbligatori;
- `GreenTea`: ridefinisce gli hook: acqua a 80°C e nessun extra;
- `Coffee`: l'hook `CustomerWantsCondiments()` dipende dalla scelta del cliente (parametro del costruttore).

Gli hook mostrano come una sottoclasse possa **guidare il flusso** dell'algoritmo (saltare un passo) senza modificare l'algoritmo stesso.

---

## Il «principio di Hollywood»

*«Non chiamarci, ti chiameremo noi».* È la classe base a chiamare i metodi delle sottoclassi, non il contrario. La sottoclasse non controlla l'algoritmo: fornisce solo i dettagli quando le vengono richiesti.

## Quando usarlo

- Diverse classi fanno la stessa cosa e differiscono solo per alcuni passi.
- Bisogna permettere di estendere solo determinate parti dell'algoritmo, senza permettere di cambiarne la struttura.
- Bisogna eliminare la duplicazione del codice comune in classi simili.
- Esempi: importazione/esportazione di dati, generazione di report, elaborazione delle richieste (in ASP.NET Core `ControllerBase`, `BackgroundService.ExecuteAsync`), framework di test (`SetUp` → test → `TearDown`).

## Quando non serve

- Le varianti dell'algoritmo devono cambiare **durante l'esecuzione**: meglio Strategy.
- I passi sono molti e le sottoclassi ridefiniscono quasi tutto: di fatto non c'è un algoritmo comune.

## Pro e contro

**Pro**
- Il codice comune è in un unico punto, senza duplicazioni.
- La struttura dell'algoritmo è protetta dalle modifiche.
- Le sottoclassi ridefiniscono solo ciò che è davvero diverso.

**Contro**
- Accoppiamento rigido tramite **ereditarietà**: le modifiche alla classe base si ripercuotono su tutte le sottoclassi.
- È limitato dalla struttura dello scheletro: un nuovo passo nel mezzo richiede di modificare la classe base.
- Si può violare il principio di sostituzione di Liskov se una sottoclasse ridefinisce un passo in modo «inaspettato» (ad esempio sopprimendo un passo obbligatorio).
- Più passi ci sono, più è difficile la manutenzione.

## Relazioni con altri pattern

- **Strategy** risolve un problema simile tramite la **composizione** e permette di cambiare l'intero algoritmo a runtime. **Template Method** lo fa tramite l'**ereditarietà**, e la variante è fissata a livello di classe.
- **Factory Method** è un caso particolare di Template Method: il passo dell'algoritmo è la creazione di un oggetto. In questo progetto `NotificationFactory.Notify()` di `FactoryMethod/NotifySystem` è un template method che chiama il factory method `CreateNotification()`.
