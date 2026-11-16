using Pos.Domain.Entities;
using Pos.Domain.Repositories;
using Pos.Domain.ValueObjects;
namespace Pos.Application.UseCases;
public class AddCriterionUseCase
{
    private readonly ICriterionRepository _repo;
    public AddCriterionUseCase(ICriterionRepository repo) { _repo = repo; }
    public CriterionId Execute(string name, int maxScore, double weight)
    {
        var criterion = new Criterion(CriterionId.New(), name, maxScore, weight);
        _repo.Save(criterion);
        return criterion.Id;
    }
}
