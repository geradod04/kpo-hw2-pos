using Microsoft.Extensions.DependencyInjection;
using Pos.Application.UseCases;
using Pos.Domain.Factories;
using Pos.Domain.Repositories;
using Pos.Domain.Services;
using Pos.Domain.Strategies;
using Pos.Domain.ValueObjects;
using Pos.Infrastructure.Export;
using Pos.Infrastructure.Grading;
using Pos.Infrastructure.Persistence;

namespace Pos.App;

class Program
{
    static void Main()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IStudentRepository, InMemoryStudentRepository>();
        services.AddSingleton<ICriterionRepository, InMemoryCriterionRepository>();
        services.AddSingleton<IEvaluationRepository, InMemoryEvaluationRepository>();

        services.AddSingleton<EvaluationFactory>();
        services.AddSingleton<IFinalGradeStrategy>(sp => 
            new RoundingCalculatorDecorator(new WeightedAverageStrategy(), 2));
        services.AddSingleton<FinalGradeCalculator>();

        services.AddSingleton<AddStudentUseCase>();
        services.AddSingleton<AddCriterionUseCase>();
        services.AddSingleton<AddEvaluationUseCase>();
        services.AddSingleton<CalculateFinalGradeUseCase>();

        services.AddSingleton<GradeExporter, CsvGradeExporter>();

        var provider = services.BuildServiceProvider();

        Console.WriteLine("=== ПОС: Преподавательская Оценка Студентов ===");
        Console.WriteLine("«Обратная связь — спорт для двоих»\n");

        var addStudent = provider.GetRequiredService<AddStudentUseCase>();
        var addCriterion = provider.GetRequiredService<AddCriterionUseCase>();
        var addEval = provider.GetRequiredService<AddEvaluationUseCase>();
        var calc = provider.GetRequiredService<CalculateFinalGradeUseCase>();
        var students = provider.GetRequiredService<IStudentRepository>();
        var criteria = provider.GetRequiredService<ICriterionRepository>();
        var evals = provider.GetRequiredService<IEvaluationRepository>();
        var exporter = provider.GetRequiredService<GradeExporter>();

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\n--- МЕНЮ ---");
            Console.WriteLine("1. Добавить студента");
            Console.WriteLine("2. Добавить критерий");
            Console.WriteLine("3. Выставить оценку");
            Console.WriteLine("4. Посмотреть оценки студента");
            Console.WriteLine("5. Рассчитать итог");
            Console.WriteLine("6. Экспортировать итог в CSV");
            Console.WriteLine("7. Список студентов");
            Console.WriteLine("8. Список критериев");
            Console.WriteLine("0. Выход");

            var choice = Console.ReadLine()?.Trim();
            try
            {
                switch (choice)
                {
                    case "1":
                        Console.Write("ФИО студента: ");
                        var name = Console.ReadLine();
                        var sid = addStudent.Execute(name!);
                        Console.WriteLine($"Студент добавлен. ID: {sid}");
                        break;

                    case "2":
                        Console.Write("Название критерия: ");
                        var cName = Console.ReadLine();
                        Console.Write("Макс. балл: ");
                        var max = int.Parse(Console.ReadLine()!);
                        Console.Write("Вес (0..1): ");
                        var w = double.Parse(Console.ReadLine()!.Replace(',', '.'));
                        var cid = addCriterion.Execute(cName!, max, w);
                        Console.WriteLine($"Критерий добавлен. ID: {cid}");
                        break;

                    case "3":
                        Console.Write("ID студента: ");
                        var sId = new StudentId(Guid.Parse(Console.ReadLine()!));
                        Console.Write("ID критерия: ");
                        var cId = new Pos.Domain.ValueObjects.CriterionId(Guid.Parse(Console.ReadLine()!));
                        Console.Write("Балл: ");
                        var score = int.Parse(Console.ReadLine()!);
                        Console.Write("Комментарий (опц.): ");
                        var comment = Console.ReadLine();
                        addEval.Execute(sId, cId, score, string.IsNullOrWhiteSpace(comment) ? null : comment);
                        Console.WriteLine("Оценка выставлена.");
                        break;

                    case "4":
                        Console.Write("ID студента: ");
                        var sId4 = new StudentId(Guid.Parse(Console.ReadLine()!));
                        foreach (var e in evals.GetByStudent(sId4))
                            Console.WriteLine($"  [{e.Date:dd.MM.yy}] критерий {e.CriterionId}: {e.Score.Value}/{e.Score.MaxScore}{(e.Comment != null ? $" — {e.Comment}" : "")}");
                        break;

                    case "5":
                        Console.Write("ID студента: ");
                        var sId5 = new StudentId(Guid.Parse(Console.ReadLine()!));
                        var (studName, grade) = calc.Execute(sId5);
                        Console.WriteLine($"Итог по {studName}: {grade}");
                        break;

                    case "6":
                        Console.Write("ID студента: ");
                        var sId6 = new StudentId(Guid.Parse(Console.ReadLine()!));
                        var (studName6, grade6) = calc.Execute(sId6);
                        var student = students.GetById(sId6)!;
                        var path = $"grade_{sId6}.csv";
                        exporter.Export(student, grade6, path);
                        Console.WriteLine($"Экспортировано в {path}");
                        break;

                    case "7":
                        foreach (var s in students.GetAll())
                            Console.WriteLine($"  {s.Id} — {s.Name}");
                        break;

                    case "8":
                        foreach (var c in criteria.GetAll())
                            Console.WriteLine($"  {c.Id} — {c.Name} (max={c.MaxScore}, вес={c.Weight})");
                        break;

                    case "0":
                        exit = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}
