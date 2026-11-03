🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Bridge) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Bridge) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Bridge) | **Italiano** | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Bridge)

# Bridge (Ponte)

**Tipo:** pattern strutturale.

**Bridge** divide una classe grande (o una gerarchia) in due gerarchie indipendenti, **l'astrazione** e **l'implementazione**, che possono evolvere separatamente l'una dall'altra. Il collegamento tra loro è un riferimento (il «ponte») dall'astrazione all'implementazione.

---

## Quale problema risolve

Ci sono dei telecomandi (base, avanzato) e dei dispositivi (televisore, radio). Se si risolve il problema con l'ereditarietà, si ottiene una classe per ogni combinazione:

```text
Remote
 ├── TvRemote
 ├── RadioRemote
 ├── AdvancedTvRemote
 └── AdvancedRadioRemote
```

Aggiungiamo un proiettore: altre due classi. Aggiungiamo un «telecomando vocale»: un'altra classe per ogni dispositivo. Il numero di classi cresce come un **prodotto** (N telecomandi × M dispositivi) e il codice viene duplicato.

Il motivo è che la classe cerca di estendersi contemporaneamente **in due direzioni indipendenti** (tipo di telecomando e tipo di dispositivo).

## Soluzione

Sostituire l'ereditarietà con la **composizione**. Una direzione («cosa facciamo»: telecomando/report) diventa la gerarchia dell'**astrazione**, l'altra («come lo facciamo»: dispositivo/formato) la gerarchia dell'**implementazione**. L'astrazione mantiene un riferimento all'oggetto implementazione e gli delega il lavoro.

Ora le classi sono N + M, e qualsiasi telecomando funziona con qualsiasi dispositivo.

---

## Partecipanti

| Ruolo | Classe nell'esempio | Descrizione |
|-------|---------------------|-------------|
| **Abstraction** | `Remote`, `Report` | Logica di controllo di alto livello. Mantiene un riferimento all'Implementor e gli delega il lavoro |
| **RefinedAbstraction** | `AdvancedRemote`, `SalesReport`, `InventoryReport` | Estende l'astrazione senza toccare le implementazioni |
| **Implementor** | `IDevice`, `IReportRenderer` | Interfaccia dell'implementazione: un insieme di operazioni primitive. Non deve per forza coincidere con l'interfaccia dell'astrazione |
| **ConcreteImplementor** | `Tv`, `Radio`, `PlainTextRenderer`, `HtmlRenderer` | Implementazioni delle primitive dipendenti dalla piattaforma |
| **Client** | classi demo | Collega l'astrazione all'implementazione desiderata |

## Schema

```text
        Abstraction                         Implementor
 ┌──────────────────────┐   ponte    ┌────────────────────────┐
 │ Remote               │◆─────────► │ «interface» IDevice    │
 │ - Device: IDevice    │            │ Enable(), Disable()    │
 │ + TogglePower()      │            │ SetVolume(), SetChannel│
 │ + VolumeUp()         │            └────────────────────────┘
 └──────────────────────┘                  ▲          ▲
            ▲                              │          │
 ┌──────────────────────┐                 Tv        Radio
 │ AdvancedRemote       │
 │ + Mute(), Unmute()   │
 └──────────────────────┘
```

---

## Esempio 1. Telecomandi e dispositivi (`RemoteControl/`)

**Implementor**: `IDevice` descrive le operazioni **primitive** del dispositivo: accendere, spegnere, impostare volume e canale.

**ConcreteImplementor**: `Tv` e `Radio` (cartella `Devices/`) le implementano in modo diverso: il televisore ha il volume 0–100, la radio 0–20 e i canali sono una lista di stazioni percorsa in cerchio.

**Abstraction**: `Remote` (cartella `Remotes/`) implementa le azioni di **alto livello** tramite le primitive del dispositivo:

```csharp
public class Remote(IDevice device)
{
    protected readonly IDevice Device = device;   // questo è il «ponte»

    public void VolumeUp() => Device.SetVolume(Device.Volume + 5);
    public void ChannelUp() => Device.SetChannel(Device.Channel + 1);
}
```

**RefinedAbstraction**: `AdvancedRemote` aggiunge `Mute`, `Unmute`, `SetFavoriteChannel`. Per farlo non è stato necessario modificare né `Tv` né `Radio`: «muto» è semplicemente `SetVolume(0)` memorizzando il volume precedente.

**Client**: la demo assembla le combinazioni `Remote + Tv`, `AdvancedRemote + Radio`, `AdvancedRemote + Tv`: 4 combinazioni possibili da 4 classi, invece di 4 classi-combinazione separate più la base.

## Esempio 2. Report e formati (`Reports/`)

**Problema:** ci sono tipi di report (vendite, magazzino) e formati di output (testo semplice, HTML). Domani potrebbero aggiungere un report sui clienti e il formato PDF.

**Implementor**: `IReportRenderer` sa disegnare le primitive: titolo, riga «nome: valore», piè di pagina.

**ConcreteImplementor**: `PlainTextRenderer` (allineamento in colonne) e `HtmlRenderer` (tag) nella cartella `Renderers/`.

**Abstraction**: la classe astratta `Report` mantiene un `IReportRenderer` e dichiara `Build()`.

**RefinedAbstraction**: `SalesReport` e `InventoryReport` (cartella `ReportTypes/`) sanno **cosa** mostrare: le vendite calcolano il totale, il report di magazzino segnala gli articoli in esaurimento. **Come** appare lo decide il renderer:

```csharp
var lines = new List<string> { Renderer.RenderTitle("Magazzino") };
foreach (var (item, count) in stock)
{
    var value = count < LowStockLimit ? $"{count} (scarso!)" : count.ToString();
    lines.Add(Renderer.RenderRow(item, value));
}
```

**Client**: la demo scorre tutti i renderer e tutti i report; l'implementazione può essere scelta a runtime (ad esempio in base all'header `Accept` della richiesta).

---

## Quando usarlo

- Una classe deve essere estesa in due (o più) direzioni indipendenti.
- Bisogna cambiare l'implementazione a runtime.
- Si vuole dividere una classe «monolitica» con diverse varianti della stessa funzionalità (ad esempio, il lavoro con diversi database/piattaforme).
- Codice multipiattaforma: l'astrazione è la GUI/logica, l'implementazione è l'API di un sistema operativo specifico.

## Quando non serve

- Esiste una sola implementazione e non se ne prevedono altre: sarebbe un livello di astrazione superfluo.

## Pro e contro

**Pro**
- Il numero di classi cresce come una somma, non come un prodotto.
- Astrazione e implementazione evolvono indipendentemente (Open/Closed).
- Il client lavora solo con l'astrazione di alto livello, senza vedere i dettagli della piattaforma.
- L'implementazione può essere sostituita a runtime.

**Contro**
- Complica il codice se in realtà la gerarchia è una sola.

## Relazioni con altri pattern

- **Adapter** si applica a classi incompatibili già esistenti, per farle funzionare insieme. **Bridge** si progetta in anticipo, perché le parti evolvano indipendentemente.
- **Strategy** è strutturato in modo simile (un oggetto delega il lavoro a un altro oggetto tramite un'interfaccia), ma risolve un altro problema: sostituire un algoritmo. In Bridge anche l'astrazione stessa è una gerarchia.
- **Abstract Factory** può creare coppie coerenti «astrazione + implementazione».
- **Builder** + **Bridge**: il director svolge il ruolo dell'astrazione, i builder quello delle implementazioni.
