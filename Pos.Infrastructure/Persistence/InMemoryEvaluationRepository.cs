using Pos.Domain.Entities;
using Pos.Domain.Repositories;
using Pos.Domain.ValueObjects;
namespace Pos.Infrastructure.Persistence;
public class InMemoryEvaluationRepository : IEvaluationRepository
{
    private readonly Dictionary<EvaluationId, Evaluation> _data = new();
    public void Save(Evaluation e) => _data[e.Id] = e;
    public void Remove(EvaluationId id) => _data.Remove(id);
    public Evaluation? GetById(EvaluationId id) => _data.GetValueOrDefault(id);
    public IReadOnlyList<Evaluation> GetByStudent(StudentId sid) =>
        _data.Values.Where(e => e.StudentId == sid).ToList();
}
