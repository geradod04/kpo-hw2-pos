using Pos.Domain.ValueObjects;
namespace Pos.Domain.Entities;
public class Evaluation
{
    public EvaluationId Id { get; }
    public StudentId StudentId { get; }
    public CriterionId CriterionId { get; }
    public Score Score { get; }
    public DateTime Date { get; }
    public string? Comment { get; }
    public Evaluation(EvaluationId id, StudentId studentId, CriterionId criterionId,
                     Score score, DateTime date, string? comment)
    {
        Id = id;
        StudentId = studentId;
        CriterionId = criterionId;
        Score = score;
        Date = date;
        Comment = comment;
    }
}
