# ПОС: Преподавательская Оценка Студентов

Консольное приложение для учета оценок студентов, спроектированное с применением DDD, гексагональной архитектуры и паттернов GoF.

## Архитектура: Ports and Adapters

Гексагональная архитектура. Зависимости направлены строго внутрь (к Домену).

- Pos.Domain (Ядро): Entities, Value Objects, Domain Services, Ports.
- Pos.Application (Use Cases): Сценарии использования, Фасады, Фабрики. Зависит только от Domain.
- Pos.Infrastructure (Адаптеры): Реализация портов (InMemory репозитории), UI (ConsoleUI). Зависит от Domain и Application.
- Pos.Console (Host): Composition Root. Настраивает DI-контейнер и связывает адаптеры с портами.

**Стрелка зависимостей:** `Console` → `Infrastructure` → `Application` → `Domain`.

## DDD (Domain-Driven Design)

### Ubiquitous Language
В коде используются: `Student`, `Criterion`, `Evaluation`, `Score`.

### Entity vs Value Object
- `Student`, `Criterion`, `Evaluation` — Entities, так как имеют уникальный `Id` и жизненный цикл.
- `Score` — Value Object, так как определяется только своим значением.

### Инварианты
1. `Score` не может быть отрицательным (защищено в конструкторе VO).
2. Оценка не может превышать `MaxScore` критерия (защищено в `EvaluationFactory`).
3. Оценку нельзя выставить по несуществующему критерию (защищено в `EvaluationFactory`).

### Repository
Интерфейсы (`IStudentRepository` и др.) объявлены в Домене (порты), а реализация (`InMemory...`) вынесена в Инфраструктуру.

### Domain Service
`EvaluationResultCalculator` инкапсулирует логику подсчета итога, которая не принадлежит одной сущности.

## Паттерны GoF

### Порождающие: Factory Method (`EvaluationFactory`)
**Проблема:** Создание `Evaluation` требует проверки инвариантов (сравнение балла с `MaxScore` критерия).  
**Решение:** Фабрика инкапсулирует эту проверку. UI просто просит создать оценку, а Фабрика гарантирует, что инвариант не будет нарушен.

### Структурные: Facade (`PosFacade`)
**Проблема:** Консольному UI пришлось бы внедрять 3 репозитория, фабрику и калькулятор, зная о внутренней кухне приложения.  
**Решение:** Фасад предоставляет простые методы (`AddStudent`, `GradeStudent`), скрывая сложность взаимодействия доменных объектов.

### Поведенческие: Strategy (`IGradingStrategy`)
**Проблема:** Правила подсчета итога могут меняться (взвешенное среднее, простое среднее, медиана).  
**Решение:** `EvaluationResultCalculator` использует стратегию. Можно легко подменить `WeightedAverageStrategy` на другую без изменения кода калькулятора (OCP).

## Запуск

```bash
dotnet run --project Pos.Console
dotnet test
