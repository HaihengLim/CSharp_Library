namespace StudentLibrary;

public class Student
{
	private const int IdFieldWidth = -8;
	private const int NameFieldWidth = -20;
	private const int GenderFieldWidth = -10;
	private const int ScoreFieldWidth = 10;
	private const int GradeFieldWidth = 5;
	
	public int Id { get; set; }
	public string? Name { get; set; }
	public Gender? Gender { get; set; }
	public double Score { get; set; }

	public Student(int id, String? name, string? gender, double score)
	{
		Id = id;
		Name = name;

		if (gender != null && TryParseGender(gender, out Gender? parsedGender))
		{
			Gender = parsedGender;
		}
		
		Score = score;
	}

	public Student(){}

	// ReSharper disable once MemberCanBePrivate.Global
	public char Grade(double score)
	{
		char result = 'F';
		if (score >= 90) result = 'A';
		else if (score >= 80) result = 'B';
		else if (score >= 70) result = 'C';
		else if (score >= 60) result = 'D';
		else if (score >= 50) result = 'E';
		else if (score < 50) result = 'F';

		return result;
	}

	public string Info =>
		$"{Id,IdFieldWidth} {Name,NameFieldWidth} {Gender,GenderFieldWidth} {Score,ScoreFieldWidth} {Grade(Score),GradeFieldWidth}";

	public string Heading => $"{"ID", IdFieldWidth} {"Name", NameFieldWidth} {"Gender", GenderFieldWidth} {"Score", ScoreFieldWidth} {"Grade", GradeFieldWidth}";

	private bool TryParseGender(string? gender, out Gender? result)
	{
		result = null;
		
		if (string.IsNullOrWhiteSpace(gender)) return false;

		bool success = Enum.TryParse(
			gender,
			ignoreCase: true,
			out Gender parsedGender);

		if (success)
		{
			result = parsedGender;
			return true;
		}

		return false;
	}
}