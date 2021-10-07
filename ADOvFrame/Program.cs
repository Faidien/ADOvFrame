using System.Configuration;
using System.Data.Common;
using static System.Console;
using System.Data;
using System;

namespace ADOvFrame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WriteLine("* * * * *  Fun with Provider Factories * * * * *");
            string dataProvider = ConfigurationManager.AppSettings["provider"];
            string connectionString = ConfigurationManager.ConnectionStrings["AutoLotSqlProvider"].ConnectionString;
            DbProviderFactory factory = DbProviderFactories.GetFactory(dataProvider);
            using (DbConnection connection = factory.CreateConnection())
            {
                if (connection == null)
                {
                    ShowError("Connection");
                    return;
                }
                WriteLine($"Your connection object is a:{connection.GetType().Name}");
                connection.ConnectionString = connectionString;
                connection.Open();
                // Создать объект команды
                DbCommand command = factory.CreateCommand();
                if (command == null)
                {
                    ShowError("Command");
                    return;

                }
                WriteLine($"Your command object is a: {command.GetType().Name}");
                command.Connection = connection;
                command.CommandText = "SELECT * FROM Inventory";
                //Вывести данные с помощью объекта чтения данных
                using (DbDataReader dataReader = command.ExecuteReader())
                {
                    WriteLine($"Your data reader object is a: {dataReader.GetType().Name}");
                    WriteLine("\n* * * * * Current Inventory * * * * ");
                    while (dataReader.Read())
                        WriteLine($"-> Car #{dataReader["CarID"]} is a {dataReader["Make"]}.");

                }
            }
            ReadLine();

        }

        private static void ShowError(string objectName)
        {
            WriteLine($"There was an issue creating the {objectName}");
            ReadLine();
        }
    }
}
