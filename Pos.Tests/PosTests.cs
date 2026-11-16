using Pos.Domain.Entities;
using Pos.Domain.Factories;
using Pos.Domain.Services;
using Pos.Domain.Strategies;
using Pos.Domain.ValueObjects;
using Pos.Infrastructure.Grading;
using Pos.Infrastructure.Persistence;
using Xunit;

namespace Pos.Tests;

public class InvariantTests
{
    [Fact]
    public void Score_ThrowsWhenBelowZero()
    {
        Assert.Throws<ArgumentException>(() => new Score(-1, 10));
    }

    [Fact]
    public void Score_ThrowsWhenAboveMax()
    {
        Assert.Throws<ArgumentException>(() => new Score(11, 10));
    }

    [Fact]
    public void Score_AcceptsValidValue()
    {
        var s = new Score(7, 10);
        Assert.Equal(7, s.Value);
    }

    [Fact]
    public void Criterion_ThrowsWhenWeightOutOfRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Criterion(CriterionId.New(), "X", 10, 1.5));
    }
}

public class EvaluationFactoryTests
{
    [Fact]
    public void Create_ThrowsWhenStudentNotFound()
    {
        var students = new InMemoryStudentRepository();
        var criteria = new InMemoryCriterionRepository();
        criteria.Save(new Criterion(CriterionId.New(), "X", 10, 0.5));
        var factory = new EvaluationFactory(criteria, students);

        Assert.Throws<InvalidOperationException>(() =>
            factory.Create(StudentId.New(), criteria.GetAll()[0].Id, 5));
    }

    [Fact]
    public void Create_ThrowsWhenCriterionNotFound()
    {
        var students = new InMemoryStudentRepository();
        var criteria = new InMemoryCriterionRepository();
        students.Save(new Student(StudentId.New(), new FullName("Иван")));
        var factory = new EvaluationFactory(criteria, students);

        Assert.Throws<InvalidOperationException>(() =>
            factory.Create(students.GetAll()[0].Id, CriterionId.New(), 5));
    }

    [Fact]
    public void Create_ThrowsWhenScoreAboveMax()
    {
        var students = new InMemoryStudentRepository();
        var criteria = new InMemoryCriterionRepository();
        var sid = StudentId.New();
        var cid = CriterionId.New();
        
        students.Save(new Student(sid, new FullName("Иван")));
        criteria.Save(new Criterion(cid, "ДЗ", 10, 0.5));
        var factory = new EvaluationFactory(criteria, students);

        Assert.Throws<ArgumentException>(() => factory.Create(sid, cid, 15));
    }
}

public class FinalGradeCalculatorTests
{
    [Fact]
    public void WeightedAverage_CalculatesCorrectly()
    {
        var students = new InMemoryStudentRepository();
        var criteria = new InMemoryCriterionRepository();
        var evals = new InMemoryEvaluationRepository();

        var sid = StudentId.New();
        var c1 = new Criterion(CriterionId.New(), "ДЗ", 10, 0.4);
        var c2 = new Criterion(CriterionId.New(), "Экз", 100, 0.6);

        students.Save(new Student(sid, new FullName("Иван")));
        criteria.Save(c1);
        criteria.Save(c2);

        evals.Save(new Evaluation(EvaluationId.New(), sid, c1.Id, new Score(8, 10), DateTime.Now, null));
        evals.Save(new Evaluation(EvaluationId.New(), sid, c2.Id, new Score(70, 100), DateTime.Now, null));

        var strategy = new WeightedAverageStrategy();
        var calc = new FinalGradeCalculator(evals, criteria, strategy);
        var result = calc.CalculateFor(sid);

        Assert.Equal(74, result);
    }

    [Fact]
    public void RoundingDecorator_RoundsResult()
    {
        var criteria = new InMemoryCriterionRepository();
        var evals = new InMemoryEvaluationRepository();
        
        var sid = StudentId.New();
        var c1 = new Criterion(CriterionId.New(), "X", 3, 1.0);
        criteria.Save(c1);
        evals.Save(new Evaluation(EvaluationId.New(), sid, c1.Id, new Score(1, 3), DateTime.Now, null));

        var inner = new WeightedAverageStrategy();
        var decorator = new RoundingCalculatorDecorator(inner, 1);
        
        var raw = inner.Calculate(evals.GetByStudent(sid), criteria.GetAll());
        var rounded = decorator.Calculate(evals.GetByStudent(sid), criteria.GetAll());

        Assert.Equal(Math.Round(raw, 1), rounded);
    }
}
