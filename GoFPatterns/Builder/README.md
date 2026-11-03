🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Builder) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Builder) | **Deutsch** | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Builder) | [Español](https://github.com/Vladesko/GoF-Patterns/tree/spa/GoFPatterns/Builder)

# Builder (Erbauer)

**Typ:** Erzeugungsmuster.

**Builder** ermöglicht es, komplexe Objekte Schritt für Schritt zu erzeugen. Derselbe Bauprozess kann unterschiedliche Repräsentationen eines Objekts liefern, und der Client muss nicht wissen, wie genau das Objekt zusammengesetzt wird.

---

## Welches Problem wird gelöst

Stellen wir uns ein Objekt mit vielen Parametern vor, von denen einige optional sind und andere voneinander abhängen (Pizza: Größe, Teig, Soße, Belag, Käse; HTTP-Anfrage: Methode, URL, Header, Body, Timeout).

Ohne das Muster entsteht meist eine von zwei Varianten:

- **„Teleskop“-Konstruktor** – `new Pizza("Large", "thin", "tomato", null, null, true, ...)`. Es ist unklar, welches Argument wofür steht, und für jede Kombination braucht man eine eigene Überladung.
- **Objekt mit öffentlichen Settern** – man kann ein „halb zusammengebautes“ Objekt in ungültigem Zustand erhalten, und die Prüfungen verteilen sich über den gesamten Code.

## Lösung

Der Bauprozess wird aus dem Objekt selbst in eine eigene Klasse ausgelagert – den **Builder**. Der Builder bietet eine Reihe von Schritten (`SetSize`, `AddTopping`, `WithHeader`...), der Client ruft nur die benötigten auf und erhält am Ende das fertige Ergebnis (`GetResult()` / `Build()`), das vor der Herausgabe geprüft werden kann.

Werden dieselben Schrittfolgen häufig verwendet, lagert man sie in einen **Director** aus – eine Klasse, die die „Rezepte“ kennt und den Builder steuert.

---

## Beteiligte

| Rolle | Klasse im Beispiel | Beschreibung |
|------|-----------------|----------|
| **Builder** | `IPizzaBuilder` | Schnittstelle, die die Bauschritte deklariert, die allen Buildern gemeinsam sind |
| **ConcreteBuilder** | `ItalianPizzaBuilder`, `AmericanPizzaBuilder`, `HttpRequestBuilder` | Implementiert die Schritte auf eigene Weise, hält das entstehende Objekt und liefert das Ergebnis |
| **Product** | `Pizza`, `HttpRequestMessageModel` | Das komplexe Objekt, das zusammengebaut wird. Produkte verschiedener Builder müssen keine gemeinsame Schnittstelle haben |
| **Director** | `PizzaDirector` | Kennt die Reihenfolge der Schritte für typische Konfigurationen. Arbeitet mit dem Builder nur über die Schnittstelle. Optionaler Beteiligter |
| **Client** | `DemonstratePizzaBuilder`, `DemonstrateHttpRequestBuilder` | Erzeugt den Builder, übergibt ihn dem Director oder ruft die Schritte selbst auf |

## Schema

```text
 Client ──► Director ──────────► «interface» IPizzaBuilder
              MakeMargherita()      Reset(), SetSize(), SetDough(),
              MakePepperoni()       SetSauce(), AddTopping(), GetResult()
                                          ▲                 ▲
                                          │                 │
                              ItalianPizzaBuilder   AmericanPizzaBuilder
                                          │                 │
                                          └──── erzeugt ────┴──► Pizza
```

---

## Beispiel 1. Pizzeria (`PizzaShop/`) – klassischer Builder mit Director

**Aufgabe:** Eine Pizzeria backt dieselben Rezepte (Margherita, Pepperoni) in verschiedenen Stilen. Italienische Pizza – dünner Teig, Parmesan; amerikanische – dicker Teig, BBQ-Soße, doppelter Belag, Cheddar.

**Product** – `Pizza`: eine gewöhnliche Klasse mit Eigenschaften und einer Liste von Belägen.

**Builder** – `IPizzaBuilder` deklariert die Schritte:

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

**ConcreteBuilder** – jeder Builder interpretiert die Schritte auf seine Weise. `AmericanPizzaBuilder` macht den Teig zum Beispiel dick und verdoppelt den Belag:

```csharp
public void SetDough(string dough) => _pizza.Dough = $"dicker {dough}";
public void AddTopping(string topping) => _pizza.Toppings.Add($"doppelt {topping}");
```

`GetResult()` gibt die fertige Pizza zurück und ruft sofort `Reset()` auf, damit der Builder für die nächste Bestellung bereit ist.

**Director** – `PizzaDirector` speichert die „Rezepte“. Er weiß nicht, welcher Builder ihm übergeben wurde, und ruft nur Methoden der Schnittstelle auf:

```csharp
public Pizza MakeMargherita()
{
    _builder.Reset();
    _builder.SetSize("mittel");
    _builder.SetDough("Weizenteig");
    _builder.SetSauce("Tomate");
    _builder.AddTopping("Mozzarella");
    _builder.AddTopping("Basilikum");
    return _builder.GetResult();
}
```

**Client** – `DemonstratePizzaBuilder` zeigt drei Dinge:
1. Dasselbe Rezept liefert mit `ItalianPizzaBuilder` und `AmericanPizzaBuilder` unterschiedliche Pizzen;
2. der Builder des Directors lässt sich über `ChangeBuilder` austauschen;
3. der Builder kann auch ohne Director genutzt werden – um eine „eigene“ Pizza von Hand zusammenzustellen.

## Beispiel 2. HTTP-Anfrage (`HttpRequest/`) – Fluent Builder

**Aufgabe:** Eine HTTP-Anfrage zusammenstellen, bei der fast alle Parameter optional sind und die Anfrage nach der Erzeugung unveränderlich sein soll.

Das ist die in C# am weitesten verbreitete Variante des Musters (vgl. `StringBuilder`, `UriBuilder`, `WebApplication.CreateBuilder`, `HostBuilder`). Einen Director gibt es hier nicht, und eine Builder-Schnittstelle ist unnötig – es gibt nur einen Builder.

**Product** – `HttpRequestMessageModel`: alle Eigenschaften sind schreibgeschützt, der Konstruktor ist `internal`, das Objekt kann „von außen“ also nur über den Builder erzeugt werden.

**ConcreteBuilder** – `HttpRequestBuilder`. Jeder Schritt gibt `this` zurück, daher lassen sich die Aufrufe verketten:

```csharp
var request = new HttpRequestBuilder()
    .Post("https://api.shop.com/orders")
    .WithBearerToken("secret-token")
    .WithJsonBody("{ \"productId\": 42 }")
    .WithTimeout(TimeSpan.FromSeconds(5))
    .Build();
```

Schritte können „zusammengesetzt“ sein: `WithJsonBody` setzt nicht nur den Body, sondern auch den Header `Content-Type`, `WithBearerToken` bildet den Header `Authorization`.

`Build()` ist die einzige Stelle, an der die Korrektheit geprüft wird (URL vorhanden, GET ohne Body). Ein ungültiges Objekt lässt sich daher gar nicht erst erzeugen – die Demo zeigt die Ausnahme beim Versuch, eine Anfrage ohne URL zu bauen.

---

## Wann einsetzen

- Das Objekt hat viele optionale Parameter, und der Konstruktor wird zum „Teleskop“.
- Unterschiedliche Repräsentationen eines Objekts sollen mit denselben Schritten erzeugt werden.
- Das Objekt soll unveränderlich sein, aber schrittweise zusammengebaut werden.
- Zusammengesetzte Strukturen (Composite-Bäume, Dokumente, Anfragen) sollen Schritt für Schritt aufgebaut werden.

## Wann nicht nötig

- Das Objekt hat 2–3 Parameter – ein Konstruktor oder `required init`-Eigenschaften in C# genügen.
- Das Objekt ist einfach und erfordert keine Konsistenzprüfung der Parameter.

## Vor- und Nachteile

**Vorteile**
- Schrittweise Erzeugung, Schritte können aufgeschoben oder rekursiv ausgeführt werden.
- Derselbe Bau-Code für verschiedene Produkte.
- Isolation der komplexen Zusammenbaulogik von der Geschäftslogik des Produkts (SRP).
- Korrektheit und Unveränderlichkeit des Ergebnisses lassen sich garantieren.

**Nachteile**
- Mehr Klassen (Builder, Director).
- Der Client ist an konkrete Builder gebunden, wenn diese keine gemeinsame Schnittstelle haben.

## Beziehung zu anderen Mustern

- **Abstract Factory** erzeugt Familien von Objekten und gibt sie sofort zurück; **Builder** baut ein komplexes Objekt schrittweise und liefert es am Ende.
- **Composite** – oft sind es gerade dessen Bäume, die ein Builder zusammenbaut.
- **Factory Method** entwickelt sich häufig zu einem Builder, wenn es zu viele Parameter werden.
- Ein Builder kann ein **Singleton** sein, wenn er keinen Zustand hält.
