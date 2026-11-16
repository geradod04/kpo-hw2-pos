using Pos.Domain.Entities;
using Pos.Domain.ValueObjects;
namespace Pos.Domain.Repositories;
public interface ICriterionRepository
{
    void Save(Criterion criterion);
    Criterion? GetById(CriterionId id);
    IReadOnlyList<Criterion> GetAll();
}
