🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus/GoFPatterns/Singleton) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng/GoFPatterns/Singleton) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu/GoFPatterns/Singleton) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita/GoFPatterns/Singleton) | **Español**

# Singleton (Instancia única)

**Tipo:** patrón creacional.

**Singleton** garantiza que una clase tenga **una sola instancia** y proporciona un **punto de acceso global** a ella.

---

## Qué problema resuelve

1. **Una sola instancia.** Hay recursos que deben existir en una única instancia para toda la aplicación: la configuración, el logger, el pool de conexiones, la caché. Si cada servicio crea su propia configuración, el archivo se leerá muchas veces y los cambios hechos en un sitio no se verán en otro.
2. **Acceso global.** Hay que acceder a esa instancia desde distintas partes del programa, pero sin permitir que se sobrescriba como una variable global normal.

## Solución

- Hacer el constructor **privado** para que nadie pueda llamar a `new`.
- Guardar la única instancia en un **campo estático**.
- Entregarla mediante una **propiedad estática** `Instance`, que crea el objeto en el primer acceso (inicialización diferida).
- Hacer la clase `sealed` para que no se pueda saltar la restricción mediante una subclase.

La principal dificultad es la **concurrencia**: si dos hilos acceden a `Instance` al mismo tiempo, una implementación ingenua creará dos objetos.

---

## Participantes

| Rol | Clase en el ejemplo | Descripción |
|-----|---------------------|-------------|
| **Singleton** | `AppConfiguration`, `FileLogger` | Guarda la única instancia, oculta el constructor y entrega la instancia mediante `Instance` |
| **Client** | `CartService`, `PriceService`, clases de demo | Accede al singleton mediante `ClassName.Instance` |

En realidad el patrón tiene un único «actor»: la propia clase singleton.

## Esquema

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
     CartService       PriceService       ← el mismo objeto
```

---

## Ejemplo 1. Configuración de la aplicación (`Configuration/`): con `Lazy<T>`

**Tarea:** la configuración (cadena de conexión, moneda, límite del carrito) se carga una sola vez desde un archivo y la usan distintos servicios.

**Singleton**: `AppConfiguration`:

```csharp
public sealed class AppConfiguration
{
    private static readonly Lazy<AppConfiguration> _instance = new(() => new AppConfiguration());

    public static AppConfiguration Instance => _instance.Value;

    private AppConfiguration() { /* carga costosa de la configuración */ }
}
```

`Lazy<T>` funciona por defecto en modo `ExecutionAndPublication`: la fábrica se llama **exactamente una vez**, aunque varios hilos accedan a `Value` al mismo tiempo. Es la forma recomendada en C#.

**Clients**: `CartService` y `PriceService` (carpeta `Services/`) no están relacionados entre sí, pero ambos obtienen `AppConfiguration.Instance`. En la demo se ve que:
- el mensaje de carga solo se imprime en el primer acceso (inicialización diferida);
- el `InstanceId` es el mismo en ambos servicios;
- el cambio de moneda mediante `Set` se ve de inmediato en `PriceService`;
- 100 accesos en paralelo dan 1 instancia única.

## Ejemplo 2. Logger (`Logger/`): seguridad para hilos a mano

**Tarea:** mostrar por qué la implementación ingenua es peligrosa y cómo funciona el bloqueo clásico con doble comprobación.

**Antiejemplo**: `NaiveLogger`:

```csharp
if (_instance is null)
{
    _instance = new NaiveLogger();   // dos hilos pueden llegar aquí al mismo tiempo
}
```

Un constructor lento (`Thread.Sleep`) hace visible la condición de carrera: la demo lanza 10 hilos y se crean varias instancias en lugar de una.

**Singleton**: `FileLogger` con **double-checked locking**:

```csharp
if (_instance is null)              // 1.ª comprobación: rápida, sin bloqueo
{
    lock (_lock)
    {
        if (_instance is null)      // 2.ª comprobación: otro hilo pudo crear ya el objeto
        {
            _instance = new FileLogger();
        }
    }
}
```

El campo está marcado como `volatile` para que otros hilos no vean un objeto construido a medias. El contador `CreatedInstances` muestra que se ha creado exactamente una instancia y que todos los mensajes de los distintos hilos llegan al mismo logger.

---

## Variantes de implementación en C#

| Forma | Seguridad para hilos | Inicialización diferida | Comentario |
|-------|----------------------|-------------------------|------------|
| `if (_instance == null)` sin lock | ❌ | ✅ | Solo para código de un único hilo |
| `lock` + doble comprobación | ✅ | ✅ | Clásica, pero es fácil equivocarse |
| `static readonly Instance = new()` | ✅ | casi | Se crea en el primer acceso a la clase (lo garantiza el CLR) |
| `Lazy<T>` | ✅ | ✅ | La forma recomendada |
| Contenedor DI `AddSingleton<T>()` | ✅ | ✅ | En ASP.NET Core es preferible a un Singleton escrito a mano |

## Cuándo usarlo

- El objeto realmente debe ser único (configuración, logger, caché, pool).
- Se necesita una garantía estricta de unicidad, no un simple «acordamos crear solo uno».

## Cuándo no hace falta

- Si hay un contenedor DI, es mejor registrar el servicio como singleton (`services.AddSingleton<IConfig, AppConfig>()`) y obtenerlo a través de una interfaz: es la misma unicidad, pero sin estado global y con la posibilidad de sustituirlo en las pruebas.
- Si el «singleton» solo sirve para no pasar una dependencia como parámetro, es una dependencia oculta y no un motivo para usar el patrón.

## Ventajas y desventajas

**Ventajas**
- Garantía de una única instancia.
- Punto de acceso global.
- Inicialización diferida: el objeto se crea solo cuando hace falta.

**Desventajas**
- Viola el SRP: la clase hace su trabajo y además gestiona su propio ciclo de vida.
- Dependencias ocultas: en la firma de `CartService` no se ve que depende de la configuración.
- Difícil de probar: el estado global sobrevive entre pruebas y no se puede sustituir por un mock.
- Exige cuidado en entornos multihilo.

## Relación con otros patrones

- **Facade** a menudo se implementa como singleton: normalmente basta con una sola fachada.
- **Abstract Factory**, **Builder** y **Prototype** (registro de prototipos) pueden implementarse como Singleton.
- **Flyweight** se le parece en que comparte objetos, pero puede tener muchas instancias con distinto estado interno, y estas son inmutables; un Singleton es exactamente uno y puede ser mutable.
