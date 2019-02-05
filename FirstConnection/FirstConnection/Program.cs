using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConnection
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            try
            {
                //ConnectWithDB().GetAwaiter();
                Task.Run(() => ReadDataAsync()).GetAwaiter().GetResult();
            }
            catch (Exception e) { Console.WriteLine(e.Message); }


            Console.Read();

        }

        private static async Task ConnectWithDB()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            string sqlExpression = "Update Person Set FirstName='Karen' Where Id=3 ";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand();
                command.CommandText = sqlExpression;
                command.Connection = connection;
                int number = command.ExecuteNonQuery();
                Console.WriteLine($"Add {number} object");
            }
            Console.WriteLine("Connection closed...");
        }

        static async  void ReadDataAsync()
        {
            string coonectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            string sqlExpress = "Select * from Person";
            using (SqlConnection connection = new SqlConnection(coonectionString))
            {
                await  connection.OpenAsync();
                SqlCommand command = new SqlCommand(sqlExpress, connection);
                using (SqlDataReader dataReader = await command.ExecuteReaderAsync())            
                if (dataReader.HasRows)
                {
                   
                    while (dataReader.Read())
                    {
                        Console.WriteLine($"{dataReader.GetName(0)}={dataReader.GetValue(0)}");
                        Console.WriteLine($"{dataReader.GetName(1)}={dataReader.GetValue(1)}");
                        Console.WriteLine($"{dataReader.GetName(2)}={dataReader.GetValue(2)}\n");
                    }
                }
            }
        }
    }
}
