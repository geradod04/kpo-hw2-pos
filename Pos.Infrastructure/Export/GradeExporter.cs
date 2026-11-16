using Pos.Domain.Entities;
namespace Pos.Infrastructure.Export;
public abstract class GradeExporter
{
    public void Export(Student student, double grade, string path)
    {
        var content = Format(student, grade);
        File.WriteAllText(path, content);
    }
    protected abstract string Format(Student student, double grade);
}

public class CsvGradeExporter : GradeExporter
{
    protected override string Format(Student student, double grade)
    {
        return $"student_id,name,grade`n{student.Id},{student.Name.Value},{grade}";
    }
}

public class JsonGradeExporter : GradeExporter
{
    protected override string Format(Student student, double grade)
    {
        return $"{{`n  `"studentId`": `"{student.Id}`",`n  `"name`": `"{student.Name.Value}`",`n  `"grade`": {grade}`n}}";
    }
}
