🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Builder) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Builder) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Builder) | **Italiano** | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Builder)

# Builder (Costruttore)

**Tipo:** pattern creazionale.

**Builder** permette di creare oggetti complessi passo dopo passo. Lo stesso processo di costruzione può produrre rappresentazioni diverse dell'oggetto, e il client non è tenuto a sapere come esattamente l'oggetto viene assemblato.

---

## Quale problema risolve

Immaginiamo un oggetto con molti parametri, alcuni facoltativi e altri dipendenti tra loro (pizza: dimensione, impasto, salsa, condimenti, formaggio; richiesta HTTP: metodo, URL, header, corpo, timeout).

Senza il pattern di solito si finisce in uno di questi due casi:

- **Costruttore «telescopico»**: `new Pizza("Large", "thin", "tomato", null, null, true, ...)`. Non si capisce quale argomento serve a cosa, e per ogni combinazione serve un overload.
- **Oggetto con setter pubblici**: l'oggetto può essere ottenuto «assemblato a metà» e in uno stato non valido, e i controlli si sparpagliano in tutto il codice.

## Soluzione

Spostare il processo di costruzione dall'oggetto stesso in una classe separata, il **builder**. Il builder offre un insieme di passi (`SetSize`, `AddTopping`, `WithHeader`...), il client chiama solo quelli necessari e alla fine ottiene il risultato pronto (`GetResult()` / `Build()`), che può essere verificato prima di essere restituito.

Se le stesse sequenze di passi vengono usate spesso, si spostano in un **director**: una classe che conosce le «ricette» e guida il builder.

---

## Partecipanti

| Ruolo | Classe nell'esempio | Descrizione |
|-------|---------------------|-------------|
| **Builder** | `IPizzaBuilder` | Interfaccia che dichiara i passi di costruzione comuni a tutti i builder |
| **ConcreteBuilder** | `ItalianPizzaBuilder`, `AmericanPizzaBuilder`, `HttpRequestBuilder` | Implementa i passi a modo suo e conserva l'oggetto in costruzione; restituisce il risultato |
| **Product** | `Pizza`, `HttpRequestMessageModel` | L'oggetto complesso che viene assemblato. I prodotti di builder diversi non devono avere per forza un'interfaccia comune |
| **Director** | `PizzaDirector` | Conosce l'ordine dei passi per le configurazioni tipiche. Lavora con il builder solo tramite l'interfaccia. Partecipante facoltativo |
| **Client** | `DemonstratePizzaBuilder`, `DemonstrateHttpRequestBuilder` | Crea il builder, lo passa al director o chiama i passi da solo |

## Schema

```text
 Client ──► Director ──────────► «interface» IPizzaBuilder
              MakeMargherita()      Reset(), SetSize(), SetDough(),
              MakePepperoni()       SetSauce(), AddTopping(), GetResult()
                                          ▲                 ▲
                                          │                 │
                              ItalianPizzaBuilder   AmericanPizzaBuilder
                                          │                 │
                                          └──── crea ───────┴──► Pizza
```

---

## Esempio 1. Pizzeria (`PizzaShop/`): Builder classico con director

**Problema:** una pizzeria prepara le stesse ricette (margherita, pepperoni) in stili diversi. La pizza italiana ha impasto sottile e parmigiano; quella americana impasto alto, salsa BBQ, condimenti doppi e cheddar.

**Product**: `Pizza`, una normale classe con proprietà e una lista di condimenti.

**Builder**: `IPizzaBuilder` dichiara i passi:

```csharp
public interface IPizzaBuilder
{
    void Reset();
    void SetSize(string size);
    void SetDough(string dough);
    void SetSauce(string sauce);
    void AddTopping(string topping);
    void AddExtraCheese();
    Pizza GetResult();
}
```

**ConcreteBuilder**: ogni builder interpreta i passi a modo suo. Ad esempio, `AmericanPizzaBuilder` rende l'impasto alto e raddoppia i condimenti:

```csharp
public void SetDough(string dough) => _pizza.Dough = $"{dough} (alto)";
public void AddTopping(string topping) => _pizza.Toppings.Add($"{topping} x2");
```

`GetResult()` restituisce la pizza assemblata e chiama subito `Reset()`, così il builder è pronto per l'ordine successivo.

**Director**: `PizzaDirector` conserva le «ricette». Non sa quale builder gli è stato passato e chiama solo i metodi dell'interfaccia:

```csharp
public Pizza MakeMargherita()
{
    _builder.Reset();
    _builder.SetSize("media");
    _builder.SetDough("frumento");
    _builder.SetSauce("pomodoro");
    _builder.AddTopping("mozzarella");
    _builder.AddTopping("basilico");
    return _builder.GetResult();
}
```

**Client**: `DemonstratePizzaBuilder` mostra tre cose:
1. la stessa ricetta con `ItalianPizzaBuilder` e `AmericanPizzaBuilder` dà pizze diverse;
2. il builder del director può essere cambiato con `ChangeBuilder`;
3. il builder si può usare anche senza director, per assemblare a mano una pizza «personalizzata».

## Esempio 2. Richiesta HTTP (`HttpRequest/`): Builder fluente

**Problema:** assemblare una richiesta HTTP in cui quasi tutti i parametri sono facoltativi, mentre la richiesta, una volta creata, deve essere immutabile.

È la variante del pattern più diffusa in C# (cfr. `StringBuilder`, `UriBuilder`, `WebApplication.CreateBuilder`, `HostBuilder`). Qui non c'è un director e l'interfaccia del builder non serve: il builder è uno solo.

**Product**: `HttpRequestMessageModel`: tutte le proprietà sono di sola lettura e il costruttore è `internal`, quindi «dall'esterno» l'oggetto si può ottenere solo tramite il builder.

**ConcreteBuilder**: `HttpRequestBuilder`. Ogni passo restituisce `this`, quindi le chiamate si concatenano:

```csharp
var request = new HttpRequestBuilder()
    .Post("https://api.shop.com/orders")
    .WithBearerToken("secret-token")
    .WithJsonBody("{ \"productId\": 42 }")
    .WithTimeout(TimeSpan.FromSeconds(5))
    .Build();
```

I passi possono essere «composti»: `WithJsonBody` non solo imposta il corpo, ma aggiunge anche l'header `Content-Type`; `WithBearerToken` costruisce l'header `Authorization`.

`Build()` è l'unico punto in cui si verifica la correttezza (c'è un URL, una GET non ha corpo). Per questo un oggetto non valido semplicemente non si può ottenere: la demo mostra l'eccezione quando si prova ad assemblare una richiesta senza URL.

---

## Quando usarlo

- L'oggetto ha molti parametri facoltativi e il costruttore diventa un «telescopio».
- Bisogna creare rappresentazioni diverse dello stesso oggetto con gli stessi passi.
- L'oggetto deve essere immutabile, ma va assemblato gradualmente.
- Bisogna assemblare strutture composte (alberi Composite, documenti, richieste) passo dopo passo.

## Quando non serve

- L'oggetto ha 2–3 parametri: basta un costruttore o le proprietà `required init` di C#.
- L'oggetto è semplice e non richiede verifiche di coerenza tra i parametri.

## Pro e contro

**Pro**
- Creazione passo dopo passo: i passi si possono rimandare o eseguire ricorsivamente.
- Lo stesso codice di costruzione per prodotti diversi.
- Isola la logica di assemblaggio complessa dalla logica di business del prodotto (SRP).
- Possibilità di garantire la correttezza e l'immutabilità del risultato.

**Contro**
- Più classi (builder, director).
- Il client è legato ai builder concreti, se questi non hanno un'interfaccia comune.

## Relazioni con altri pattern

- **Abstract Factory** crea famiglie di oggetti e le restituisce subito; **Builder** assembla un solo oggetto complesso passo dopo passo e lo restituisce alla fine.
- **Composite**: spesso sono proprio i suoi alberi a essere assemblati da un Builder.
- **Factory Method** spesso evolve in un Builder quando i parametri diventano troppi.
- Un builder può essere un **Singleton**, se non conserva stato.
