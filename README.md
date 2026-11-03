🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu) | **Italiano** | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa)

# GoF Patterns: i design pattern della Gang of Four in C#

Progetto didattico con tutti i 23 design pattern classici della «Gang of Four» (GoF), implementati in C# su .NET 10.

Ogni pattern si trova nella propria cartella, che contiene un `README.md` con la spiegazione (problema, soluzione, partecipanti, schema, pro e contro, relazioni con altri pattern) e 1-2 esempi pratici. Tutti gli esempi si possono avviare da un unico menu nella console.

## Come avviarlo

Serve [.NET 10 SDK](https://dotnet.microsoft.com/download).

```bash
dotnet run --project GoFPatterns
```

All'avvio compare un menu con l'elenco di tutte le demo raggruppate per tipo di pattern. Inserite il numero della demo e premete Invio; `0` chiude il programma.

## Pattern

### Creazionali

| Pattern | Descrizione |
|---------|-------------|
| [Abstract Factory](GoFPatterns/AbtractFactory/README.md) | Crea famiglie di oggetti correlati senza legarsi alle loro classi concrete |
| [Builder](GoFPatterns/Builder/README.md) | Costruisce oggetti complessi passo dopo passo |
| [Factory Method](GoFPatterns/FactoryMethod/README.md) | Lascia alle sottoclassi la scelta della classe da istanziare |
| [Prototype](GoFPatterns/Prototype/README.md) | Crea nuovi oggetti copiando quelli esistenti |
| [Singleton](GoFPatterns/Singleton/README.md) | Garantisce un'unica istanza della classe con un punto di accesso globale |

### Strutturali

| Pattern | Descrizione |
|---------|-------------|
| [Adapter](GoFPatterns/Adapter/README.md) | Permette a oggetti con interfacce incompatibili di lavorare insieme |
| [Bridge](GoFPatterns/Bridge/README.md) | Separa l'astrazione dall'implementazione, così possono evolvere indipendentemente |
| [Composite](GoFPatterns/Composite/README.md) | Raggruppa oggetti in strutture ad albero e li tratta in modo uniforme |
| [Decorator](GoFPatterns/Decorator/README.md) | Aggiunge comportamento a un oggetto in modo dinamico avvolgendolo |
| [Facade](GoFPatterns/Facade/README.md) | Offre un'interfaccia semplice verso un sottosistema complesso |
| [Flyweight](GoFPatterns/Flyweight/README.md) | Risparmia memoria condividendo lo stato comune tra molti oggetti |
| [Proxy](GoFPatterns/Proxy/README.md) | Un sostituto che controlla l'accesso all'oggetto reale |

### Comportamentali

| Pattern | Descrizione |
|---------|-------------|
| [Chain of Responsibility](GoFPatterns/ChainOfResponsibility/README.md) | Passa la richiesta lungo una catena di handler |
| [Command](GoFPatterns/Command/README.md) | Trasforma una richiesta in un oggetto, con supporto a code e annullamento |
| [Interpreter](GoFPatterns/Interpreter/README.md) | Definisce la grammatica di un linguaggio semplice e ne interpreta le frasi |
| [Iterator](GoFPatterns/Iterator/README.md) | Attraversa una collezione senza esporne la struttura interna |
| [Mediator](GoFPatterns/Mediator/README.md) | Gli oggetti comunicano tramite un mediatore invece che direttamente |
| [Memento](GoFPatterns/Memento/README.md) | Salva e ripristina lo stato di un oggetto senza violarne l'incapsulamento |
| [Observer](GoFPatterns/Observer/README.md) | Meccanismo di sottoscrizione per ricevere notifiche sulle modifiche |
| [State](GoFPatterns/State/README.md) | L'oggetto cambia comportamento quando cambia il suo stato interno |
| [Strategy](GoFPatterns/Strategy/README.md) | Famiglia di algoritmi intercambiabili a runtime |
| [Template Method](GoFPatterns/TemplateMethod/README.md) | Scheletro di un algoritmo i cui passi vengono definiti dalle sottoclassi |
| [Visitor](GoFPatterns/Visitor/README.md) | Aggiunge nuove operazioni a una gerarchia di classi senza modificarle |
