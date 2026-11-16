using Pos.Domain.Entities;
using Pos.Domain.ValueObjects;
namespace Pos.Domain.Repositories;
public interface IEvaluationRepository
{
    void Save(Evaluation evaluation);
    void Remove(EvaluationId id);
    IReadOnlyList<Evaluation> GetByStudent(StudentId studentId);
    Evaluation? GetById(EvaluationId id);
}
