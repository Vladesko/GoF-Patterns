🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Decorator) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Decorator) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Decorator) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Decorator) | **Español**

# Decorator (Decorador)

**Tipo:** patrón estructural.

**Decorator** permite añadir dinámicamente comportamiento nuevo a un objeto «envolviéndolo» en objetos envoltorio con la misma interfaz. Los envoltorios se pueden anidar unos dentro de otros en cualquier orden y cantidad.

---

## Qué problema resuelve

Una cafetería vende bebidas a las que se puede añadir leche, sirope, tamaño grande... Si se resuelve con herencia:

```text
Espresso
 ├── EspressoWithMilk
 ├── EspressoWithSyrup
 ├── EspressoWithMilkAndSyrup
 ├── LargeEspressoWithMilkAndSyrup
 └── ...  (y lo mismo para el té)
```

El número de subclases crece de forma combinatoria. Además, la herencia es **estática**: no se puede «añadir leche» a un objeto ya creado mientras el programa se ejecuta. Y la leche doble ni siquiera se puede expresar sin otra clase más.

Lo mismo pasa con un almacenamiento de datos: a veces hace falta compresión, a veces cifrado, a veces ambas cosas, a veces además logging; no hay subclases suficientes para cada combinación.

## Solución

Sustituir la herencia por **composición**. El decorador:
- implementa **la misma interfaz** que el objeto envuelto;
- guarda una referencia al objeto envuelto;
- le delega las llamadas, añadiendo su propio comportamiento **antes o después**.

Como para el cliente el decorador parece un componente normal, se puede envolver en otro decorador más, y así se obtiene una «matrioska». El cliente no sabe si trabaja con el objeto original o con una cadena de envoltorios.

---

## Participantes

| Rol | Clase en el ejemplo | Descripción |
|-----|---------------------|-------------|
| **Component** | `IBeverage`, `IDataSource` | Interfaz común para los envoltorios y para los objetos envueltos |
| **ConcreteComponent** | `Espresso`, `BlackTea`, `InMemoryDataSource` | El objeto base al que se añade comportamiento |
| **BaseDecorator** | `BeverageDecorator`, `DataSourceDecorator` | Guarda una referencia a Component y por defecto simplemente le delega las llamadas |
| **ConcreteDecorator** | `MilkDecorator`, `SyrupDecorator`, `LargeSizeDecorator`, `CompressionDecorator`, `EncryptionDecorator`, `LoggingDecorator` | Añaden un comportamiento concreto antes/después de la llamada al objeto envuelto |
| **Client** | clases de demo | Monta la combinación de envoltorios necesaria y trabaja con ella a través de Component |

## Esquema

```text
            ┌───────────────────────┐
 Client ───►│ «interface» IBeverage │◄─────────────────┐
            │ GetDescription()      │                  │ envuelve
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

## Ejemplo 1. Cafetería (`Coffee/`)

**Component**: `IBeverage` con los métodos `GetDescription()` y `GetCost()`.

**ConcreteComponent**: `Espresso` y `BlackTea` (carpeta `Beverages/`) solo conocen su precio base.

**BaseDecorator**: `BeverageDecorator` (carpeta `AddOns/`) guarda la bebida envuelta y le delega todo mediante métodos `virtual`:

```csharp
public abstract class BeverageDecorator(IBeverage beverage) : IBeverage
{
    protected IBeverage Beverage { get; } = beverage;

    public virtual string GetDescription() => Beverage.GetDescription();
    public virtual decimal GetCost() => Beverage.GetCost();
}
```

**ConcreteDecorator**: cada complemento sobrescribe los métodos, llama a `base` y completa el resultado:

```csharp
public class MilkDecorator(IBeverage beverage) : BeverageDecorator(beverage)
{
    public override string GetDescription() => $"{base.GetDescription()} + leche";
    public override decimal GetCost() => base.GetCost() + 0.50m;
}
```

- `SyrupDecorator` muestra que un decorador puede tener **estado propio** (el sabor del sirope).
- `LargeSizeDecorator` no suma, sino que **multiplica** el precio. Por eso en la demo se ve que **el orden de los envoltorios importa**: «grande (espresso + sirope)» es más caro que «espresso grande + sirope».

**Client**: monta las bebidas como una «matrioska»: `new MilkDecorator(new MilkDecorator(new Espresso()))` es leche doble sin una clase aparte. Los mismos complementos funcionan sin cambios también con el té.

## Ejemplo 2. Fuente de datos (`DataStream/`)

**Tarea:** los datos (por ejemplo, los datos de un cliente con número de tarjeta) se guardan en un almacenamiento. Según la configuración, hay que comprimirlos, cifrarlos o registrarlos en el log, en cualquier combinación.

**Component**: `IDataSource` con `Write`/`Read`.

**ConcreteComponent**: `InMemoryDataSource` guarda la cadena tal cual (simulación de un archivo o de un registro en la BD).

**BaseDecorator**: `DataSourceDecorator` (carpeta `Decorators/`).

**ConcreteDecorator**: aquí se ve bien el comportamiento «antes» y «después»:

```csharp
// CompressionDecorator
public override void Write(string data) => base.Write(Compress(data));   // ANTES de escribir: comprimir
public override string Read() => Decompress(base.Read());                // DESPUÉS de leer: descomprimir
```

- `CompressionDecorator`: GZip + Base64.
- `EncryptionDecorator`: XOR con una clave (solo para la demostración, ¡no es un cifrado real!).
- `LoggingDecorator`: no modifica los datos, solo escribe su tamaño en la consola.

**Client**: la demo envuelve el almacenamiento:

```csharp
IDataSource source = new LoggingDecorator(
    new CompressionDecorator(
        new EncryptionDecorator(protectedStorage, "secret")),
    "cliente");
```

Al escribir, los datos pasan de fuera hacia dentro (log → compresión → cifrado → almacenamiento); al leer, de dentro hacia fuera. En el almacenamiento queda una cadena comprimida y cifrada, mientras que el cliente lee el texto original sin enterarse siquiera.

Así es exactamente como funcionan los streams en .NET: `new GZipStream(new CryptoStream(new FileStream(...)))` es un Decorator clásico. También son decoradores `BufferedStream`, `StreamReader` (sobre un `Stream`) y, en ASP.NET Core, el pipeline de middleware y la decoración de servicios en DI (por ejemplo, con Scrutor).

---

## Cuándo usarlo

- Hay que añadir responsabilidades a los objetos **dinámicamente** y sin que el cliente lo note.
- Ampliar mediante herencia es imposible (la clase es `sealed`) o provoca una explosión de subclases.
- Hay que combinar «capas» independientes: caché, logging, reintentos, validación, métricas.

## Cuándo no hace falta

- Se necesita una única combinación fija de comportamiento: es más sencillo escribir una clase.
- El comportamiento debe depender de qué envoltorios hay ya (los decoradores no deben conocerse entre sí).

## Ventajas y desventajas

**Ventajas**
- Más flexible que la herencia: el comportamiento se añade y combina en tiempo de ejecución.
- Cada decorador se encarga de una sola cosa (SRP).
- Se puede aplicar varias veces (leche doble).
- Permite ampliar clases `sealed` y código de terceros a través de su interfaz.

**Desventajas**
- Muchas clases pequeñas.
- Es difícil quitar un envoltorio concreto de la mitad de la cadena.
- El comportamiento depende del orden de los envoltorios, y es fácil pasarlo por alto.
- El código de configuración de la «matrioska» puede resultar engorroso.

## Relación con otros patrones

- **Adapter** cambia la interfaz del objeto; **Decorator** mantiene la misma interfaz y amplía el comportamiento; **Proxy** mantiene la misma interfaz y **controla el acceso** (normalmente gestiona él mismo el ciclo de vida del objeto, mientras que el decorador recibe el objeto del cliente).
- **Composite** se le parece estructuralmente (composición recursiva), pero el decorador tiene un solo objeto anidado y su objetivo es añadir comportamiento.
- **Chain of Responsibility** también pasa la petición por una cadena, pero sus eslabones pueden **interrumpir** el procesamiento; el decorador siempre pasa la llamada adelante.
- **Strategy** cambia las «entrañas» del objeto; **Decorator**, su «envoltura».
