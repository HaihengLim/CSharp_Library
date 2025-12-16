namespace StudentLibrary;

public class StudentList
{
	public event EventHandler<Student>? Created;
	public event EventHandler<Student>? Readed;
	public event EventHandler<Student>? Updated;
	public event EventHandler<Student>? Deleted;
	
	public List<Student> Students = new();

	public bool Create(Student? stu)
	{
		if (!IsValidStudent(stu)) return false;
		if (!IsValidStudentId(stu!.Id)) return false;
		if (!IsValidStudentName(stu.Name)) return false;
		if (!IsValidStudentGender(stu.Gender)) return false;
		if (!IsValidStudentScore(stu.Score)) return false;
		
		Students.Add(stu);
		Created?.Invoke(this, stu);
		return true;
	}

	public bool Read(Student? stu)
	{
		if (!IsValidStudent(stu)) return false;

		Readed?.Invoke(this, stu!);
		return true;
	}

	public bool Update(int id, string? newName = null, string? newGender = null, double? newScore = null)
	{
		var student = Students.FirstOrDefault(s => s.Id == id);
		bool updateAnyField = false;

		if (student == null) return false;
		if (newName != null && IsValidStudentName(newName))
		{
			student.Name = newName;
			updateAnyField = true;
		}
		if (!string.IsNullOrWhiteSpace(newGender))
		{
			if (ParsedGender(newGender, out Gender? parsedGender) && IsValidStudentGender(parsedGender))
			{
				student.Gender = parsedGender;
				updateAnyField = true;
			}
		}
		if (newScore.HasValue && IsValidStudentScore(newScore.Value))
		{
			student.Score = newScore.Value;
			updateAnyField = true;
		}

		if (updateAnyField)
		{
			Updated?.Invoke(this, student);
		}
		return true;
	}

	public bool Delete(int id)
	{
		var student = Students.FirstOrDefault(s => s.Id == id);

		if (student == null) return false;

		if (Students.Remove(student))
		{
			Deleted?.Invoke(this, student);
			return true;
		}

		return false;
	}

	private bool IsValidStudent(Student? stu) => stu != null;

	private bool IsValidStudentId(int id) => id >= 0;

	private bool IsValidStudentName(string? name) => !string.IsNullOrEmpty(name);

	private bool IsValidStudentGender(Gender? gender) => gender != null && Enum.IsDefined(typeof(Gender), gender.Value);

	private bool IsValidStudentScore(double score) => score is >= 0 and <= 100;

	private bool ParsedGender(string? gender, out Gender? result)
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

	public bool Initialize(string fileName)
	{
		try
		{
			using (StreamReader sr = new StreamReader(fileName))
			{
				string? line;
				int lineNumber = 0;

				Console.WriteLine($"Data read from {fileName}");

				while ((line = sr.ReadLine()) != null)
				{
					lineNumber++;
					string[] fields = line.Split('/');

					if (fields.Length == 4)
					{
						try
						{
							int id = 0;
							string name = fields[1];
							string gender = fields[2];
							double score = 0;

							if (!int.TryParse(fields[0], out id) || !IsValidStudentId(id))
							{
								Console.WriteLine($"Line {lineNumber}: Invalid ID format or value '{fields[0]}'. Skipping.");
								continue;
							}

							if (!IsValidStudentName(name))
							{
								Console.WriteLine($"Line {lineNumber}: Invalid Name format or value '{fields[1]}'. Skipping.");
								continue;
							}

							if (string.IsNullOrWhiteSpace(gender))
							{
								Console.WriteLine($"Line {lineNumber}: Invalid Gender format or value '{fields[2]}'. Skipping.");
								continue;
							}

							if (!double.TryParse(fields[3], out score) || !IsValidStudentScore(score))
							{
								Console.WriteLine($"Line {lineNumber}: Invalid Score format or value '{fields[3]}'. Skipping.");
								continue;
							}

							try
							{
								Student stu = new Student(id, name, gender, score);

								if (!Create(stu))
								{
									Console.WriteLine($"Data in {lineNumber} is invalid!");
								}
							}
							catch (ArgumentException)
							{
								Console.WriteLine($"Data is failed to read!");
							}
							catch (Exception)
							{
								Console.WriteLine("Data is unexpected error to read!");
							}
						}
						catch (FormatException)
						{
							Console.WriteLine("Could not convert data!");
						}
					}
					else
					{
						Console.WriteLine($"The Data in line {lineNumber} is missing data!");
					}
				}
			}
		}
		catch (FileNotFoundException)
		{
			Console.WriteLine($"File Name {fileName} could not found!");
			return false;
		}

		return true;
	}
}