🌐 [Русский](https://github.com/Vladesko/GoF-Patterns/tree/rus) | [English](https://github.com/Vladesko/GoF-Patterns/tree/eng) | [Deutsch](https://github.com/Vladesko/GoF-Patterns/tree/deu) | [Italiano](https://github.com/Vladesko/GoF-Patterns/tree/ita) | **Español**

# Patrones de diseño GoF en C#

Proyecto de aprendizaje con los 23 patrones de diseño de la «Banda de los Cuatro» (Gang of Four, GoF) implementados en C# (.NET 10). Cada patrón está en su propia carpeta, con un README que explica el problema, la solución, los participantes y un esquema, además de 1–2 ejemplos prácticos que se pueden ejecutar desde un menú de consola.

## Cómo ejecutarlo

Requisitos: [.NET 10 SDK](https://dotnet.microsoft.com/download).

```bash
dotnet run --project GoFPatterns
```

Se abrirá un menú con todas las demos agrupadas por tipo de patrón. Escriba el número de la demo y pulse Intro; `0` cierra el programa.

## Patrones

### Creacionales

| Patrón | Ejemplos |
|--------|----------|
| [Abstract Factory (Fábrica abstracta)](GoFPatterns/AbtractFactory/README.md) | Proveedores de pago |
| [Builder (Constructor)](GoFPatterns/Builder/README.md) | Pizzería, petición HTTP |
| [Factory Method (Método fábrica)](GoFPatterns/FactoryMethod/README.md) | Abonos de gimnasio, notificaciones |
| [Prototype (Prototipo)](GoFPatterns/Prototype/README.md) | Plantillas de documentos, unidades de juego |
| [Singleton (Instancia única)](GoFPatterns/Singleton/README.md) | Configuración, logger |

### Estructurales

| Patrón | Ejemplos |
|--------|----------|
| [Adapter (Adaptador)](GoFPatterns/Adapter/README.md) | Sensores de temperatura |
| [Bridge (Puente)](GoFPatterns/Bridge/README.md) | Mandos a distancia, informes |
| [Composite (Compuesto)](GoFPatterns/Composite/README.md) | Sistema de archivos, estructura organizativa |
| [Decorator (Decorador)](GoFPatterns/Decorator/README.md) | Cafetería, fuente de datos |
| [Facade (Fachada)](GoFPatterns/Facade/README.md) | Cine en casa, realización de pedidos |
| [Flyweight (Peso ligero)](GoFPatterns/Flyweight/README.md) | Bosque, formato de texto |
| [Proxy (Intermediario)](GoFPatterns/Proxy/README.md) | Carga diferida de imágenes, cuenta bancaria |

### De comportamiento

| Patrón | Ejemplos |
|--------|----------|
| [Chain of Responsibility (Cadena de responsabilidad)](GoFPatterns/ChainOfResponsibility/README.md) | Tickets de soporte, middleware |
| [Command (Comando)](GoFPatterns/Command/README.md) | Editor de texto con Undo/Redo, hogar inteligente |
| [Interpreter (Intérprete)](GoFPatterns/Interpreter/README.md) | Expresiones matemáticas, búsqueda de productos |
| [Iterator (Iterador)](GoFPatterns/Iterator/README.md) | Colección de libros, recorrido de árbol |
| [Mediator (Mediador)](GoFPatterns/Mediator/README.md) | Sala de chat, formulario de registro |
| [Memento (Recuerdo)](GoFPatterns/Memento/README.md) | Historial del editor de texto, partidas guardadas |
| [Observer (Observador)](GoFPatterns/Observer/README.md) | Precio del producto |
| [State (Estado)](GoFPatterns/State/README.md) | Ciclo de vida del pedido, máquina expendedora |
| [Strategy (Estrategia)](GoFPatterns/Strategy/README.md) | Coste de envío, ordenación |
| [Template Method (Método plantilla)](GoFPatterns/TemplateMethod/README.md) | Importación de datos, bebidas calientes |
| [Visitor (Visitante)](GoFPatterns/Visitor/README.md) | Figuras |
