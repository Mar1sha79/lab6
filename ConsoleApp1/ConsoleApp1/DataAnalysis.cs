using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public static class StudentAnalysis
{
    private const string StudentsFileName = "students.txt";

    public class Student
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int School { get; set; }
        public int Score { get; set; }
    }

    // Метод для добавления нового ученика через консоль
    public static void AddStudentThroughConsole()
    {
        Console.WriteLine("\n=== ДОБАВЛЕНИЕ НОВОГО УЧЕНИКА ===");

        // Используем MyInputHelpers вместо InputHelpers
        string lastName = InputHelpers.ReadString("Введите фамилию ученика: ");
        string firstName = InputHelpers.ReadString("Введите имя ученика: ");
        int school = InputHelpers.ReadInt("Введите номер школы (1-99): ", 1, 99);
        int score = InputHelpers.ReadInt("Введите балл (1-100): ", 1, 100);

        string studentData = $"{lastName} {firstName} {school} {score}";

        // Создаем файл, если его нет
        if (!File.Exists(StudentsFileName))
        {
            File.WriteAllText(StudentsFileName, "", Encoding.UTF8);
        }

        // Добавляем в файл
        File.AppendAllText(StudentsFileName, studentData + Environment.NewLine, Encoding.UTF8);

        Console.WriteLine("Ученик успешно добавлен в файл!");
        Console.WriteLine("Добавлена запись: " + studentData);
    }

    // Показывает текущее содержимое файла
    public static void ShowFileContent()
    {
        if (!File.Exists(StudentsFileName))
        {
            Console.WriteLine("Файл не существует. Добавьте учеников через меню.");
            return;
        }

        string[] lines = File.ReadAllLines(StudentsFileName, Encoding.UTF8);
        if (lines.Length == 0)
        {
            Console.WriteLine("Файл пуст. Добавьте учеников через меню.");
            return;
        }

        Console.WriteLine("\n=== ТЕКУЩЕЕ СОДЕРЖИМОЕ ФАЙЛА ===");
        for (int i = 0; i < lines.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {lines[i]}");
        }
        Console.WriteLine($"Всего записей: {lines.Length}");
    }

    // Очищает файл
    public static void ClearFile()
    {
        if (File.Exists(StudentsFileName))
        {
            File.WriteAllText(StudentsFileName, "", Encoding.UTF8);
            Console.WriteLine("Файл очищен!");
        }
        else
        {
            Console.WriteLine("Файл не существует.");
        }
    }

    // Создает пустой файл для учеников
    private static void CreateDefaultStudentsFile()
    {
        File.WriteAllText(StudentsFileName, "", Encoding.UTF8);
    }

    // Читает данные учеников из файла
    public static List<Student> ReadStudentsFromFile()
    {
        if (!File.Exists(StudentsFileName))
        {
            CreateDefaultStudentsFile();
            throw new InvalidOperationException("Файл " + StudentsFileName + " создан. Заполните файл данными через меню.");
        }

        var students = new List<Student>();
        string[] lines = File.ReadAllLines(StudentsFileName, Encoding.UTF8);

        if (lines.Length == 0)
            throw new InvalidOperationException("Файл " + StudentsFileName + " пуст. Заполните файл данными через меню.");

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 4)
            {
                if (int.TryParse(parts[2], out int school) && school >= 1 && school <= 99 &&
                    int.TryParse(parts[3], out int score) && score >= 1 && score <= 100)
                {
                    students.Add(new Student
                    {
                        LastName = parts[0],
                        FirstName = parts[1],
                        School = school,
                        Score = score
                    });
                }
            }
        }

        if (students.Count < 5)
            throw new InvalidOperationException("Недостаточно данных. В файле должно быть не менее 5 учеников. Сейчас: " + students.Count);

        return students;
    }

    // Анализирует школы с баллом выше среднего
    public static (List<int> schools, double? singleSchoolAverage) FindSchoolsAboveAverage(List<Student> students)
    {
        if (students == null || students.Count < 5)
            throw new ArgumentException("Должно быть не менее 5 учеников");

        // Группируем по школам и вычисляем средний балл
        var schoolAverages = students
            .GroupBy(s => s.School)
            .Select(g => new { School = g.Key, Average = g.Average(s => s.Score) })
            .ToList();

        // Средний балл по району
        double districtAverage = students.Average(s => s.Score);

        // Школы с баллом выше среднего по району
        var aboveAverageSchools = schoolAverages
            .Where(s => s.Average > districtAverage)
            .OrderBy(s => s.School)
            .ToList();

        var schoolNumbers = aboveAverageSchools.Select(s => s.School).ToList();

        double? singleAverage = null;
        if (aboveAverageSchools.Count == 1)
        {
            singleAverage = aboveAverageSchools[0].Average;
        }

        return (schoolNumbers, singleAverage);
    }

    // Показывает информацию о файле с учениками
    public static (string fileName, long fileSize, DateTime createdTime) GetStudentsFileInfo()
    {
        if (!File.Exists(StudentsFileName))
        {
            CreateDefaultStudentsFile();
        }

        FileInfo fileInfo = new FileInfo(StudentsFileName);
        return (fileInfo.Name, fileInfo.Length, fileInfo.CreationTime);
    }
}