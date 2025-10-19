﻿using System;
using Microsoft.Data.SqlClient;
using System.Data;

namespace StudentApiDataAccessLayer
{
    public class StudentDTO
    {
        public StudentDTO(int id, string name, int age, int grade)
        {
            this.Id = id;
            this.Name = name;
            this.Age = age;
            this.Grade = grade;
        }


        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Grade { get; set; }
    }

    public class DataLayer
    {
        static string _connectionString = "Server=localhost;Database=StudentsDB;User Id=sa;Password=123456;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30;";

        public static List<StudentDTO> GetAllStudents()
        {
            var StudentsList = new List<StudentDTO>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAllStudents", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StudentsList.Add(new StudentDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetInt32(reader.GetOrdinal("Age")),
                                reader.GetInt32(reader.GetOrdinal("Grade"))
                            ));
                        }
                    }
                }
            }
            return StudentsList;
        }

        public static List<StudentDTO> GetPassedStudents()
        {
            var StudentsList = new List<StudentDTO>();

            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                using(SqlCommand command = new SqlCommand("SP_GetPassedStudents",connection) )
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StudentsList.Add(new StudentDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetInt32(reader.GetOrdinal("Age")),
                                reader.GetInt32(reader.GetOrdinal("Grade"))
                            ));
                        }
                    }
                }
            }
            return StudentsList;
        }

        public static double GetAVGStudetnsGrades()
        {
            double AverageGrade = 0;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAverageGrade", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    object reader = command.ExecuteScalar();
                    if (reader != null)
                    {
                        AverageGrade = Convert.ToDouble(reader);
                    }
                    else
                    {
                        AverageGrade = 0;
                    }
                }
            }
            return AverageGrade;
        }

        public static StudentDTO GetStudentById(int studentId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SP_GetStudentById", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StudentId", studentId);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new StudentDTO
                        (
                            reader.GetInt32(reader.GetOrdinal("Id")),
                            reader.GetString(reader.GetOrdinal("Name")),
                            reader.GetInt32(reader.GetOrdinal("Age")),
                            reader.GetInt32(reader.GetOrdinal("Grade"))
                        );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public static int AddNewStudent(StudentDTO StudentDTO)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SP_AddStudent", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Name", StudentDTO.Name);
                command.Parameters.AddWithValue("@Age", StudentDTO.Age);
                command.Parameters.AddWithValue("@Grade", StudentDTO.Grade);
                var outputIdParam = new SqlParameter("@NewStudentId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputIdParam);

                connection.Open();
                command.ExecuteNonQuery();

                return (int)outputIdParam.Value;
            }
        }

        public static bool UpdateStudent(StudentDTO StudentDTO)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SP_UpdateStudent", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@StudentId", StudentDTO.Id);
                command.Parameters.AddWithValue("@Name", StudentDTO.Name);
                command.Parameters.AddWithValue("@Age", StudentDTO.Age);
                command.Parameters.AddWithValue("@Grade", StudentDTO.Grade);

                connection.Open();
                command.ExecuteNonQuery();
                return true;

            }
        }

        public static bool DeleteStudent(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SP_DeleteStudent", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StudentId", id);

                connection.Open();

                int rowsAffected = (int)command.ExecuteScalar();
                return (rowsAffected == 1);


            }
        }
    }
}
