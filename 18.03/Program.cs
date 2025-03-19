using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace _18._03
{
    internal class Program
    {
        static private string connectionString = @"Data Source=DESKTOP-QUFQFS7\SQLEXPRESS;Initial Catalog=VegetablesFruitsDB;Integrated Security=True";
        static private List<VegetablesFruits> vegetablesFruits = new List<VegetablesFruits>();

        static void Main()
        {
            string connectionString = @"Data Source=DESKTOP-QUFQFS7\SQLEXPRESS;Initial Catalog=VegetablesFruitsDB;Integrated Security=True";

            ConnectToDatabase(connectionString);
        }

        static void ConnectToDatabase(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Успішне підключення до бази даних 'Овочі та фрукти'.");
                    GetVegetablesFruits(connection);
                    while (true)
                    {
                        Console.WriteLine("Enter:\n1 to print all vegetables and fruits\n2 to show min calories\n3 to show count of vegetables\n4 to print limit calories:\n");
                        string input = Console.ReadLine();
                        if (input == "1") PrintVegetablesFruits();
                        if (input == "2") PrintMinCalories();
                        if (input == "3") CountOfVegetables();
                        if (input == "4") CaloriesLimit();
                        else Console.WriteLine("invalid input");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка підключення: {ex.Message}");
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                        Console.WriteLine("Підключення закрито.");
                    }
                }
            }
        }

        static void GetVegetablesFruits(SqlConnection connection)
        {
            try
            {
                var command = new SqlCommand("SELECT * FROM FruitsAndVegetables", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        vegetablesFruits.Add(new VegetablesFruits
                        {
                            ID = (int)reader["ID"],
                            Name = reader["Name"].ToString(),
                            Type = reader["Type"].ToString(),
                            Color = reader["Color"].ToString(),
                            Calories = (int)reader["Calories"]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка виконання запиту: {ex.Message}");
            }
        }

        static void PrintVegetablesFruits()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-15} {4,-10}", "Id", "Name", "Type", "Color", "Calories");

                foreach (var item in vegetablesFruits)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-15} {4,-10}", item.ID, item.Name, item.Type, item.Color, item.Calories);
                }
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка виконання запиту: {ex.Message}");
            }
        }

        static void PrintMinCalories()
        {
            try
            {
                VegetablesFruits minCalories = vegetablesFruits.OrderBy(x => x.Calories).First();
                Console.WriteLine("Min calories: " + minCalories.Name + " - " + minCalories.Calories);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка виконання запиту: {ex.Message}");
            }
        }

        static void CountOfVegetables()
        {
            try
            {
                int count = vegetablesFruits.Count(x => x.Type == "Овоч");
                Console.WriteLine("Count of vegetables: " + count);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка виконання запиту: {ex.Message}");
            }
        }

        static void CaloriesLimit()
        {
            Console.Write("Enter calories to limit: ");
            int limit = int.Parse(Console.ReadLine());

            try
            {
                var result = vegetablesFruits.Where(x => x.Calories < limit);
                foreach (var item in result)
                {
                    Console.WriteLine(item.Name + " - " + item.Calories);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка виконання запиту: {ex.Message}");
            }
        }
    }
}
