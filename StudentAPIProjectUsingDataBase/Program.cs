using System;
using System.Data;
using System.Net.Http;
using System.Net.Http.Json;

namespace StudentsAPIClientSide
{
    internal class Program
    {
        static readonly HttpClient client = new HttpClient();


        static async Task Main(string[] args)
        {
            client.BaseAddress = new Uri("https://localhost:7013/api/StudentsApi/");

            await StudentInfo();
            await PassedStudentInfo();
            await AverageStudentsGrade();
            await GetStudentByID();
            await AddNewStudent();
            await DeleteStudentByID();
            await UpdateStudent();
        }



        //                                 CRUD On API Using Console App
        static async Task StudentInfo()
        {
            try
            {
                Console.WriteLine("Students info Fetching");
                Console.WriteLine("-------------------------------------");
                var students = await client.GetFromJsonAsync<List<Students>>("All");
                if (students != null)
                {
                    foreach (var student in students)
                    {
                        Console.WriteLine("Id " + student.id);
                        Console.WriteLine("Name " + student.Name);
                        Console.WriteLine("Age " + student.Age);
                        Console.WriteLine("Grade " + student.Grade);
                        Console.WriteLine("----------------------------------");
                    }
                }
                else
                {
                    Console.WriteLine("List Is Empty");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }


        }

        static async Task PassedStudentInfo()
        {
            try
            {
                Console.WriteLine("\nPassed Students info Fetching");
                Console.WriteLine("-------------------------------------");
                var students = await client.GetFromJsonAsync<List<Students>>("Passed");
                if (students != null)
                {
                    foreach (var student in students)
                    {
                        Console.WriteLine("Id " + student.id);
                        Console.WriteLine("Name " + student.Name);
                        Console.WriteLine("Age " + student.Age);
                        Console.WriteLine("Grade " + student.Grade);
                        Console.WriteLine("----------------------------------");
                    }
                }
                else
                {
                    Console.WriteLine("List Is Empty");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }


        }

        static async Task AverageStudentsGrade()
        {
            try
            {
                Console.WriteLine("\nAVG Grade Fetching");
                Console.WriteLine("-------------------------------------");
                var students = await client.GetFromJsonAsync<double>("AverageGrade");
                if (students == null)
                {
                    Console.WriteLine($"No Content found");
                }
                else
                {
                    Console.WriteLine(students);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }

        static async Task GetStudentByID()
        {
            int id = 2;

            try
            {
                Console.WriteLine($"\nStudent With id {id} Info Fetching");
                Console.WriteLine("--------------------------------------------");
                var response = await client.GetAsync($"{id}");

                if (response.IsSuccessStatusCode)
                {
                    var student = await response.Content.ReadFromJsonAsync<Students>();
                    if (student != null)
                    {
                        Console.WriteLine($"ID: {student.id}, Name: {student.Name}, Age: {student.Age}, Grade: {student.Grade}");
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    Console.WriteLine($"Bad Request: Not accepted ID {id}");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"Not Found: Student with ID {id} not found.");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }

        static async Task AddNewStudent()
        {
            var newStudent = new Students();
            Console.Write("\nEnter Grad: ");
            string Grade = Console.ReadLine(); // Read the input as a string
            newStudent.Grade = int.Parse(Grade);
            Console.Write("\nEnter Age: ");
            string Age = Console.ReadLine(); // Read the input as a string
            newStudent.Age = int.Parse(Age);
            Console.Write("\nEnter Name: ");
            newStudent.Name = Console.ReadLine();
            try
            {
                Console.WriteLine("\n_____________________________");
                Console.WriteLine("\nAdding a new student...\n");

                var response = await client.PostAsJsonAsync("", newStudent);

                if (response.IsSuccessStatusCode)
                {
                    var addedStudent = await response.Content.ReadFromJsonAsync<Students>();
                    Console.WriteLine($"Added Student - ID: {addedStudent.id}, Name: {addedStudent.Name}, Age: {addedStudent.Age}, Grade: {addedStudent.Grade}");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    Console.WriteLine("Bad Request: Invalid student data.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async Task DeleteStudentByID()
        {
            int id = 1;

            try
            {
                Console.WriteLine("\n_____________________________");
                Console.WriteLine($"\nDeleting student with ID {id}...\n");
                var response = await client.DeleteAsync($"{id}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Student with ID {id} has been deleted.");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    Console.WriteLine($"Bad Request: Not accepted ID {id}");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"Not Found: Student with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async Task UpdateStudent()
        {
            int id = 2;

            Students updatedStudent = new Students();
            Console.Write("\nEnter Grad: ");
            string Grade = Console.ReadLine(); // Read the input as a string
            updatedStudent.Grade = int.Parse(Grade);
            Console.Write("\nEnter Age: ");
            string Age = Console.ReadLine(); // Read the input as a string
            updatedStudent.Age = int.Parse(Age);
            Console.Write("\nEnter Name: ");
            updatedStudent.Name = Console.ReadLine();

            try
            {
                Console.WriteLine("\n_____________________________");
                Console.WriteLine($"\nUpdating student with ID {id}...\n");
                var response = await client.PutAsJsonAsync($"{id}", updatedStudent);

                if (response.IsSuccessStatusCode)
                {
                    var student = await response.Content.ReadFromJsonAsync<Students>();
                    Console.WriteLine($"Updated Student: ID: {student.id}, Name: {student.Name}, Age: {student.Age}, Grade: {student.Grade}");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    Console.WriteLine("Failed to update student: Invalid data.");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"Student with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        //                                 CRUD On API Using Console App



    }

    class Students
    {
        public int id { get; set; }
        public int Age { get; set; }
        public string Name { get; set; }
        public int Grade { get; set; }
    }
}