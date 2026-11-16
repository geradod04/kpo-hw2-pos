using Pos.Domain.Entities;
using Pos.Domain.Repositories;
using Pos.Domain.ValueObjects;
namespace Pos.Infrastructure.Persistence;
public class InMemoryCriterionRepository : ICriterionRepository
{
    private readonly Dictionary<CriterionId, Criterion> _data = new();
    public void Save(Criterion c) => _data[c.Id] = c;
    public Criterion? GetById(CriterionId id) => _data.GetValueOrDefault(id);
    public IReadOnlyList<Criterion> GetAll() => _data.Values.ToList();
}
