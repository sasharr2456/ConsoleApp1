using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<int> Courses { get; set; }
}

public class Teacher
{
    public int Id { get; set; }
    public int Experience { get; set; }
    public string Name { get; set; }
    public List<int> Courses { get; set; }
}

public class Course
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int TeacherId { get; set; }
    public List<int> StudentIds { get; set; }
}

public class DatabaseManager
{
    private const string FilePath = "database.txt";

    public void SaveDatabase(List<Student> students, List<Teacher> teachers, List<Course> courses)
    {
        using (StreamWriter writer = new StreamWriter(FilePath))
        {
            foreach (var student in students)
            {
                writer.WriteLine($"student, {student.Id}, {student.Name}, {string.Join(", ", student.Courses)}");
            }
            foreach (var teacher in teachers)
            {
                writer.WriteLine($"teacher, {teacher.Id}, {teacher.Experience}, {teacher.Name}, {string.Join(", ", teacher.Courses)}");
            }
            foreach (var course in courses)
            {
                writer.WriteLine($"course, {course.Id}, {course.Name}, {course.TeacherId}, {string.Join(", ", course.StudentIds)}");
            }
        }
    }

    public void LoadDatabase(out List<Student> students, out List<Teacher> teachers, out List<Course> courses)
    {
        students = new List<Student>();
        teachers = new List<Teacher>();
        courses = new List<Course>();

        if (File.Exists(FilePath))
        {
            var lines = File.ReadAllLines(FilePath);
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length > 1)
                {
                    if (parts[0] == "student")
                    {
                        var student = new Student
                        {
                            Id = int.Parse(parts[1]),
                            Name = parts[2],
                            Courses = parts[3].Split(',').Select(int.Parse).ToList()
                        };
                        students.Add(student);
                    }
                    else if (parts[0] == "teacher")
                    {
                        var teacher = new Teacher
                        {
                            Id = int.Parse(parts[1]),
                            Experience = int.Parse(parts[2]),
                            Name = parts[3],
                            Courses = parts[4].Split(',').Select(int.Parse).ToList()
                        };
                        teachers.Add(teacher);
                    }
                    else if (parts[0] == "course")
                    {
                        var course = new Course
                        {
                            Id = int.Parse(parts[1]),
                            Name = parts[2],
                            TeacherId = int.Parse(parts[3]),
                            StudentIds = parts[4].Split(',').Select(int.Parse).ToList()
                        };
                        courses.Add(course);
                    }
                }
            }
        }
    }
}

class Program
{
    static void Main()
    {
        string filePath = "database.txt";
        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }
        DatabaseManager dbManager = new DatabaseManager();

        // Создание списка студентов, учителей и курсов
        List<Student> students = new List<Student>
        {
            new Student { Id = 1, Name = "Alice", Courses = new List<int> { 101, 102 } },
            new Student { Id = 2, Name = "Bob", Courses = new List<int> { 101 } }
        };

        List<Teacher> teachers = new List<Teacher>
        {
            new Teacher { Id = 1, Experience = 5, Name = "Eve", Courses = new List<int> { 101, 102 } }
        };

        List<Course> courses = new List<Course>
        {
            new Course { Id = 101, Name = "Math", TeacherId = 1, StudentIds = new List<int> { 1, 2 } },
            new Course { Id = 102, Name = "Science", TeacherId = 1, StudentIds = new List<int> { 1 } }
        };

        // Сохранение базы данных в файл
        dbManager.SaveDatabase(students, teachers, courses);

        // Загрузка базы данных из файла
        dbManager.LoadDatabase(out List<Student> loadedStudents, out List<Teacher> loadedTeachers, out List<Course> loadedCourses);
    }


}

