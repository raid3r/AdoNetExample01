using DbExample.Models;
using Microsoft.Data.SqlClient;
using System.Data;

var connectionString = "Data Source=SILVERSTONE\\SQLEXPRESS;Initial Catalog=Academy2;Integrated Security=True;Connect Timeout=60;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";

//var t = new Teacher();
//Console.WriteLine(t.Name == null? null: t.Name.ToLower());
//Console.WriteLine(t.Name?.ToLower()?.ToUpper()?.Replace('z', 'x'));

// SELECT * FROM Teachers
static List<Teacher> GetAllTeachers(string connectionString)
{
    var teachers = new List<Teacher>();

    using (var connection = new SqlConnection(connectionString))
    {
        try
        {

            connection.Open();

            var sqlQuery = "SELECT Id,Name,Surname,Salary FROM Teachers";

            var command = new SqlCommand(sqlQuery, connection);

            // SELECT
            using (var reader = command.ExecuteReader())
            {
                /*
                 * {
                 * "Id": 1,                0
                 * "Name": "John",         1
                 * "Surname": "Doe",       2
                 * }
                 */
                while (reader.Read())
                {
                    //var id = reader["Id"]; // int
                    int id = reader.GetInt32(0); // int
                    string name = reader.GetString(1); // string
                    string surname = reader.GetString(2); // string
                    decimal salary = reader.GetDecimal(3); // decimal

                    teachers.Add(new Teacher
                    {
                        Id = id,
                        Name = name,
                        Surname = surname,
                        Salary = salary
                    });



                    //Console.WriteLine($"{id}, {name}, {surname} {salary}");
                }
            }

            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }


        return teachers;

    }
}



// INSERT
static void AddTeacher(string connectionString, Teacher teacher)
{
    using (var connection = new SqlConnection(connectionString))
    {
        try
        {
            connection.Open();

            var sqlQuery = 
                "INSERT INTO Teachers (Name, Surname, Salary, EmploymentDate, Position) " +
                "VALUES (@Name, @Surname, @Salary, @EmploymentDate, @Position); " +
                "SELECT SCOPE_IDENTITY()";

            var command = new SqlCommand(sqlQuery, connection);
            
            command.Parameters.AddWithValue("@Name", teacher.Name);
            command.Parameters.AddWithValue("@Surname", teacher.Surname);
            command.Parameters.AddWithValue("@Salary", teacher.Salary);

            command.Parameters.AddWithValue("@EmploymentDate", DateTime.Now);
            command.Parameters.AddWithValue("@Position", "Teacher");

            //command.ExecuteNonQuery(); // не потрібен результат


            teacher.Id = Convert.ToInt32(command.ExecuteScalar());

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

// DELETE
static void DeleteTeacher(string connectionString, int id)
{

    using (var connection = new SqlConnection(connectionString))
    {
       
        try
        {
            connection.Open();

            var sqlQuery = "DELETE FROM Teachers WHERE Id = @Id";

            var command = new SqlCommand(sqlQuery, connection);
            command.Parameters.AddWithValue("@Id", id);

            command.ExecuteNonQuery(); // не потрібен результат

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

// UPDATE
static void UpdateTeacher(string connectionString, Teacher teacher)
{
    using (var connection = new SqlConnection(connectionString))
    {
        try
        {
            connection.Open();

            var sqlQuery = "UPDATE Teachers SET Name = @Name, Surname = @Surname, Salary = @Salary WHERE Id = @Id";

            var command = new SqlCommand(sqlQuery, connection);
            command.Parameters.AddWithValue("@Id", teacher.Id);
            command.Parameters.AddWithValue("@Name", teacher.Name);
            command.Parameters.AddWithValue("@Surname", teacher.Surname);
            command.Parameters.AddWithValue("@Salary", teacher.Salary);

            command.ExecuteNonQuery(); // не потрібен результат

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

static void PrintTeacher(Teacher teacher)
{
    Console.WriteLine($"{teacher.Id}, {teacher.Name}, {teacher.Surname} {teacher.Salary}");
}

//var teacherNew = new Teacher
//{
//    Name = "John",
//    Surname = "Doe",
//    Salary = 1000
//};

//AddTeacher(connectionString, teacherNew);

//Console.WriteLine($"Added teacher: {teacherNew.Id}, {teacherNew.Name}, {teacherNew.Surname} {teacherNew.Salary}");

Console.WriteLine("All teachers:");

GetAllTeachers(connectionString).ForEach(PrintTeacher);

Console.WriteLine("Enter teacher id to update:"); // запитати id вчителя
var teacherToUpdateId = int.Parse(Console.ReadLine());
var teacherToUpdate = GetAllTeachers(connectionString).First(t => t.Id == teacherToUpdateId);
PrintTeacher(teacherToUpdate);
Console.WriteLine("New name:"); // запитати нове ім'я
var newName = Console.ReadLine();
Console.WriteLine("New surname:"); // запитати нове прізвище
var newSurname = Console.ReadLine();
Console.WriteLine("New salary:"); // запитати нову зарплату
var newSalary = decimal.Parse(Console.ReadLine()); // запитати нову зарплату
teacherToUpdate.Name = newName; // нове ім'я
teacherToUpdate.Surname = newSurname; // нове прізвище
teacherToUpdate.Salary = newSalary; // нова зарплата
UpdateTeacher(connectionString, teacherToUpdate); // оновити вчителя
Console.WriteLine($"Teacher updated");
/*
Console.Write("Enter teacher id to delete:");
var teacherToDeleteId = int.Parse(Console.ReadLine());

DeleteTeacher(connectionString, teacherToDeleteId);
*/

GetAllTeachers(connectionString).ForEach(PrintTeacher);
 

























//var subjects = new List<Subject>();

//using (var connection = new SqlConnection(connectionString))
//{
//    try
//    {

//        connection.Open();

//        var sqlQuery = "SELECT Id,Name FROM Subjects";

//        var command = new SqlCommand(sqlQuery, connection);


//        using (var reader = command.ExecuteReader())
//        {
//            /*
//             * {
//             * "Id": 1,                0
//             * "Name": "John",         1
//             * "Surname": "Doe",       2
//             * }
//             */
//            while (reader.Read())
//            {
//                //var id = reader["Id"]; // int
//                int id = reader.GetInt32(0); // int
//                string name = reader.GetString(1); // string

//                subjects.Add(new Subject
//                {
//                    Id = id,
//                    Name = name,
//                });



//                //Console.WriteLine($"{id}, {name}, {surname} {salary}");
//            }
//        }


//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Error: {ex.Message}");
//    }

//}

//foreach (var subject in subjects)
//{
//    Console.WriteLine($"{subject.Id}, {subject.Name}");
//}

/*
 * 1. 
 * Створіть класс який має усі поля таблиці Teachers
 * Заповніть масив цих об'єктів даними з бази даних
 * Виведіть дані на екран
 * 
 * 2. 
 * Створити код який видаляє запис по id
 * Створити код який додає запис таблиці Teachers
 * 
 * 
 * 
 */







/*

connection.Open();

    // Create a command to execute a SQL query
    using (SqlCommand command = new SqlCommand("SELECT * FROM Students", connection))
    {
        // Execute the command and read the results
        using (SqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                Console.WriteLine($"{reader["Id"]}, {reader["Name"]}, {reader["Age"]}");
            }
        }
    } 
 */