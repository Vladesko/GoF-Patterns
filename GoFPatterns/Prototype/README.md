🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Prototype) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Prototype) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Prototype) | **Italiano** | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Prototype)

# Prototype (Prototipo)

**Tipo:** pattern creazionale.

**Prototype** permette di creare nuovi oggetti copiando oggetti già esistenti (i prototipi), senza dipendere dalle loro classi concrete.

---

## Quale problema risolve

Bisogna ottenere una copia di un oggetto. Il modo ingenuo è creare un oggetto della stessa classe e ricopiarvi tutti i campi. I problemi sono:

- alcuni campi possono essere **privati** e inaccessibili dall'esterno;
- il client deve **conoscere la classe concreta** dell'oggetto, mentre spesso ha a disposizione solo un'interfaccia (`IDocumentPrototype`, `Unit`);
- l'oggetto può essere **costoso da configurare** (inizializzazione lunga, caricamento della configurazione), e ripetere tutto per ogni copia è dispendioso.

## Soluzione

Affidare la copia all'oggetto stesso. Tutti gli oggetti copiabili implementano un'interfaccia comune con il metodo `Clone()`. L'oggetto conosce i propri campi (anche quelli privati) e decide da solo come copiarli.

Spesso si aggiunge un **registro dei prototipi**: un archivio di oggetti preconfigurati da cui il client prende copie in base a una chiave.

### Copia superficiale e profonda

- La **shallow copy** (`MemberwiseClone()`) copia i campi «così come sono»: i tipi valore vengono copiati, mentre i campi di tipo riferimento della copia e dell'originale puntano agli **stessi** oggetti.
- La **deep copy** copia anche gli oggetti annidati: la copia è completamente indipendente.

Per gli oggetti immutabili (`string`, `record` con proprietà init) la differenza non conta: possono essere condivisi in sicurezza.

---

## Partecipanti

| Ruolo | Classe nell'esempio | Descrizione |
|-------|---------------------|-------------|
| **Prototype** | `IDocumentPrototype`, `Unit` | Dichiara il metodo di clonazione `Clone()` |
| **ConcretePrototype** | `Contract`, `Invoice`, `Archer`, `Mage` | Implementa `Clone()`, copiando il proprio stato |
| **Prototype Registry** | `DocumentTemplateRegistry` | Conserva i prototipi configurati e ne restituisce copie in base a una chiave (partecipante facoltativo) |
| **Client** | `Spawner`, `DemonstratePrototype` | Crea nuovi oggetti chiamando `Clone()` sul prototipo |

## Schema

```text
 Client ───────────► «interface» IDocumentPrototype
                        Clone(): IDocumentPrototype
                              ▲              ▲
                              │              │
                          Contract        Invoice
                     Clone() => new Contract(this)

 DocumentTemplateRegistry
   _templates: chiave ─► prototipo
   Create(key) => _templates[key].Clone()
```

---

## Esempio 1. Modelli di documento (`Documents/`)

**Problema:** in un sistema di gestione documentale ci sono dei modelli (NDA, fattura UE). Un nuovo documento si crea copiando il modello, dopodiché si compilano i dati specifici.

**Prototype**: `IDocumentPrototype` con il metodo `Clone()`.

**ConcretePrototype**: `Contract` contiene i campi di tipo riferimento `DocumentStyle Style` e `List<string> Sections`. La copia profonda è implementata tramite un **costruttore di copia privato**: in questo modo la classe ha accesso a tutti i propri campi:

```csharp
private Contract(Contract source)
{
    Title = source.Title;
    Company = source.Company;
    Style = source.Style.Copy();       // copia dell'oggetto annidato
    Sections = [.. source.Sections];   // copia della lista
}

public IDocumentPrototype Clone() => new Contract(this);
```

Per confronto c'è `ShallowClone()`, basato su `MemberwiseClone()`. Nella demo si vede che dopo aver modificato `shallow.Style.FontSize` e `shallow.Sections` **cambia anche l'originale**, perché condividono lo stesso oggetto stile e la stessa lista. Le modifiche alla copia profonda non toccano l'originale.

`Invoice` è il secondo prototipo: ha un proprio insieme di campi e una propria implementazione di `Clone()`.

**Registry**: `DocumentTemplateRegistry` conserva i modelli per chiave e in `Create(key)` restituisce sempre una **copia**, quindi il modello stesso non viene mai rovinato:

```csharp
var ndaForAcme = (Contract)registry.Create("nda");
ndaForAcme.Company = "Acme";
```

## Esempio 2. Generatore di unità di gioco (`GameUnits/`)

**Problema:** in un gioco bisogna generare molte unità dello stesso tipo («arciere d'élite», «mago del fuoco»), le cui impostazioni vengono caricate dalla configurazione.

**Prototype**: la classe astratta `Unit` con `abstract Unit Clone()`.

**ConcretePrototype**: `Archer` e `Mage` (cartella `Units/`) copiano i propri campi specifici; `Mage` copia la lista degli incantesimi, così i cloni non la condividono.

**Client**: `Spawner` riceve **qualsiasi** `Unit` e genera nuove istanze senza sapere se si tratta di un arciere o di un mago:

```csharp
public Unit Spawn(Position position)
{
    var unit = prototype.Clone();
    unit.Name = $"{prototype.Name} #{++_counter}";
    unit.Position = position;
    return unit;
}
```

`Position` è un `record`. È immutabile, quindi non serve copiarlo: si può condividere tra i cloni. La demo mostra anche il meccanismo di clonazione dei record integrato in C#, l'espressione `with`: `start with { X = 7 }`.

---

## Quando usarlo

- Il codice non deve dipendere dalle classi concrete degli oggetti copiati.
- Creare un oggetto è costoso, ma servono molte istanze simili.
- C'è un insieme di configurazioni tipiche, e invece di molte sottoclassi è più comodo conservare prototipi già configurati.

## Quando non serve

- Gli oggetti sono semplici ed economici: è più facile chiamare il costruttore.
- Gli oggetti sono immutabili: si possono condividere senza copiarli.

## Pro e contro

**Pro**
- Clonazione senza legarsi alle classi concrete.
- Meno codice di inizializzazione ripetuto.
- Un'alternativa all'ereditarietà per ottenere «varianti» di un oggetto.

**Contro**
- È difficile clonare oggetti con grafi complessi (riferimenti circolari, risorse come connessioni e file).
- Bisogna fare molta attenzione alla copia shallow/deep.

## Relazioni con altri pattern

- **Abstract Factory** può conservare un insieme di prototipi e clonarli invece di crearli con `new`.
- **Memento**: per oggetti semplici lo snapshot dello stato si può ottenere per clonazione.
- **Composite** e **Decorator**: la clonazione aiuta a copiare le strutture complesse di questi pattern.
- Il registro dei prototipi è spesso un **Singleton**.

> In .NET esiste l'interfaccia `ICloneable`, ma se ne sconsiglia l'uso: restituisce `object` e non dice se la copia è profonda o superficiale. Meglio un'interfaccia tipizzata propria, come `IDocumentPrototype`.
