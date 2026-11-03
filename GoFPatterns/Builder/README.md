🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Builder) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Builder) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Builder) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Builder) | **Español**

# Builder (Constructor)

**Tipo:** patrón creacional.

**Builder** permite crear objetos complejos paso a paso. Un mismo proceso de construcción puede dar distintas representaciones del objeto, y el cliente no tiene por qué saber cómo se ensambla exactamente.

---

## Qué problema resuelve

Imaginemos un objeto con muchos parámetros, algunos opcionales y otros dependientes entre sí (pizza: tamaño, masa, salsa, ingredientes, queso; petición HTTP: método, URL, cabeceras, cuerpo, tiempo de espera).

Sin el patrón suele ocurrir una de dos cosas:

- **Constructor «telescópico»**: `new Pizza("grande", "fina", "tomate", null, null, true, ...)`. No está claro qué argumento hace qué, y cada combinación necesita su propia sobrecarga.
- **Objeto con setters públicos**: se puede obtener un objeto «a medio montar» y en un estado incorrecto, y las comprobaciones acaban dispersas por todo el código.

## Solución

Sacar el proceso de construcción del propio objeto a una clase aparte: el **constructor (builder)**. El builder ofrece un conjunto de pasos (`SetSize`, `AddTopping`, `WithHeader`...), el cliente llama solo a los que necesita y al final obtiene el resultado listo (`GetResult()` / `Build()`), que se puede validar antes de entregarlo.

Si las mismas secuencias de pasos se usan a menudo, se trasladan a un **director**: una clase que conoce las «recetas» y dirige al builder.

---

## Participantes

| Rol | Clase en el ejemplo | Descripción |
|-----|---------------------|-------------|
| **Builder** | `IPizzaBuilder` | Interfaz que declara los pasos de construcción comunes a todos los builders |
| **ConcreteBuilder** | `ItalianPizzaBuilder`, `AmericanPizzaBuilder`, `HttpRequestBuilder` | Implementa los pasos a su manera y guarda el objeto en construcción; entrega el resultado |
| **Product** | `Pizza`, `HttpRequestMessageModel` | El objeto complejo que se ensambla. Los productos de distintos builders no tienen por qué compartir una interfaz |
| **Director** | `PizzaDirector` | Conoce el orden de los pasos para las configuraciones habituales. Trabaja con el builder solo a través de la interfaz. Participante opcional |
| **Client** | `DemonstratePizzaBuilder`, `DemonstrateHttpRequestBuilder` | Crea el builder y se lo pasa al director o llama a los pasos por sí mismo |

## Esquema

```text
 Client ──► Director ──────────► «interface» IPizzaBuilder
              MakeMargherita()      Reset(), SetSize(), SetDough(),
              MakePepperoni()       SetSauce(), AddTopping(), GetResult()
                                          ▲                 ▲
                                          │                 │
                              ItalianPizzaBuilder   AmericanPizzaBuilder
                                          │                 │
                                          └──── crean ──────┴──► Pizza
```

---

## Ejemplo 1. Pizzería (`PizzaShop/`): Builder clásico con director

**Tarea:** una pizzería prepara las mismas recetas (margarita, pepperoni) en distintos estilos. La pizza italiana lleva masa fina y parmesano; la americana, masa gruesa, salsa BBQ, ingredientes dobles y cheddar.

**Product**: `Pizza`, una clase normal con propiedades y una lista de ingredientes.

**Builder**: `IPizzaBuilder` declara los pasos:

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

**ConcreteBuilder**: cada builder interpreta los pasos a su manera. Por ejemplo, `AmericanPizzaBuilder` hace la masa gruesa y duplica los ingredientes:

```csharp
public void SetDough(string dough) => _pizza.Dough = $"gruesa de {dough}";
public void AddTopping(string topping) => _pizza.Toppings.Add($"doble de {topping}");
```

`GetResult()` devuelve la pizza montada e inmediatamente llama a `Reset()`, para que el builder quede listo para el siguiente pedido.

**Director**: `PizzaDirector` guarda las «recetas». No sabe qué builder ha recibido y solo llama a los métodos de la interfaz:

```csharp
public Pizza MakeMargherita()
{
    _builder.Reset();
    _builder.SetSize("mediana");
    _builder.SetDough("trigo");
    _builder.SetSauce("tomate");
    _builder.AddTopping("mozzarella");
    _builder.AddTopping("albahaca");
    return _builder.GetResult();
}
```

**Client**: `DemonstratePizzaBuilder` muestra tres cosas:
1. la misma receta con `ItalianPizzaBuilder` y con `AmericanPizzaBuilder` da pizzas diferentes;
2. el builder del director se puede cambiar con `ChangeBuilder`;
3. el builder también se puede usar sin director, para montar a mano una pizza «propia».

## Ejemplo 2. Petición HTTP (`HttpRequest/`): Builder fluido

**Tarea:** construir una petición HTTP en la que casi todos los parámetros son opcionales y que, una vez creada, debe ser inmutable.

Es la variante del patrón más extendida en C# (compárese con `StringBuilder`, `UriBuilder`, `WebApplication.CreateBuilder`, `HostBuilder`). Aquí no hay director, y tampoco hace falta una interfaz de builder, porque solo hay uno.

**Product**: `HttpRequestMessageModel`. Todas sus propiedades son de solo lectura y el constructor es `internal`, es decir, desde «fuera» el objeto solo se puede obtener a través del builder.

**ConcreteBuilder**: `HttpRequestBuilder`. Cada paso devuelve `this`, así que las llamadas se encadenan:

```csharp
var request = new HttpRequestBuilder()
    .Post("https://api.shop.com/orders")
    .WithBearerToken("secret-token")
    .WithJsonBody("{ \"productId\": 42 }")
    .WithTimeout(TimeSpan.FromSeconds(5))
    .Build();
```

Los pasos pueden ser «compuestos»: `WithJsonBody` no solo establece el cuerpo, sino que también añade la cabecera `Content-Type`, y `WithBearerToken` genera la cabecera `Authorization`.

`Build()` es el único lugar donde se comprueba la validez (hay URL, una petición GET no tiene cuerpo). Por eso es sencillamente imposible obtener un objeto incorrecto: la demo muestra la excepción al intentar construir una petición sin URL.

---

## Cuándo usarlo

- El objeto tiene muchos parámetros opcionales y el constructor se convierte en un «telescopio».
- Hay que crear distintas representaciones de un objeto con los mismos pasos.
- El objeto debe ser inmutable, pero construirse de forma gradual.
- Hay que construir paso a paso estructuras compuestas (árboles Composite, documentos, peticiones).

## Cuándo no hace falta

- El objeto tiene 2–3 parámetros: basta con un constructor o con propiedades `required init` de C#.
- El objeto es sencillo y no requiere comprobar la coherencia de los parámetros.

## Ventajas y desventajas

**Ventajas**
- Creación paso a paso; los pasos se pueden aplazar o ejecutar de forma recursiva.
- El mismo código de construcción sirve para distintos productos.
- Aísla la lógica compleja de ensamblado de la lógica de negocio del producto (SRP).
- Permite garantizar la validez y la inmutabilidad del resultado.

**Desventajas**
- Más clases (builders, director).
- El cliente queda ligado a builders concretos si estos no tienen una interfaz común.

## Relación con otros patrones

- **Abstract Factory** crea familias de objetos y los devuelve de inmediato; **Builder** ensambla un único objeto complejo paso a paso y lo entrega al final.
- **Composite**: a menudo son precisamente sus árboles los que construye un Builder.
- **Factory Method** evoluciona con frecuencia hacia un Builder cuando los parámetros se vuelven demasiados.
- Un builder puede ser **Singleton** si no guarda estado.
