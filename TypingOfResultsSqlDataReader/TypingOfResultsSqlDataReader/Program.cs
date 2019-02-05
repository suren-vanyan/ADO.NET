using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypingOfResultsSqlDataReader
{
    class Program
    {
        static void Main(string[] args)
        {
           var task= Task.Run(() => ReadDataAsync());
            try
            {
                //task.GetAwaiter().GetResult();
                ReadScalarDataAsync().GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        static async Task ReadScalarDataAsync()
        {
            string connectioString= @"Data Source=DESKTOP-NIKJ2PC;Initial Catalog=UltimateFightingChampionship;Integrated Security=True";
            using (SqlConnection connection=new SqlConnection(connectioString))
            {
                await connection.OpenAsync();
                string expression = "Select Count(*) from MixedMartialArtist";
                SqlCommand command = new SqlCommand();
                command.CommandText = expression;
                command.Connection = connection;
                object ObjCount =await command.ExecuteScalarAsync();
                expression = "Select Max(Age) from MixedMartialArtist";
                command.CommandText = expression;
                object maxAge = command.ExecuteScalar();
                Console.WriteLine($"Table MixedMartialArtist has {ObjCount} objects");
                Console.WriteLine($"The most adult fighter {maxAge} years");


            }
        }
        static async Task ReadDataAsync()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionToGeneral"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string expression = "Select * From Person";
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(expression,connection);
                SqlDataReader reader = command.ExecuteReaderAsync().GetAwaiter().GetResult();
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                      
                        int id = reader.GetInt32(0);
                        DateTime dateTime = reader.GetDateTime(1);
                        string firstNanem = reader.GetString(2);
                        string lastName = reader.GetString(3);
                        Console.WriteLine($"Id={id} Date={dateTime} Name={firstNanem} SurName={lastName}");
                    }
                }
                reader.Close();
            }
        }
    }
}
