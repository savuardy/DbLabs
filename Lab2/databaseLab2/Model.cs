using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using databaseLab2;

namespace Lab2
{
    public class Model
    {
        private readonly string _connectionSrting;
        private NpgsqlConnection _connection;

        public Model(string host, string bdName, string username, string pass)
        {
            _connectionSrting = $"Host={host};Username={username};Password={pass};Database={bdName}";
        }

        public bool Connect()
        {
            try
            {
                _connection = new NpgsqlConnection(_connectionSrting);
                _connection.Open();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to establish connection. Inner exception: {e}");
                return false;
            }
        }

        public QueryResult Request(string request, bool useReader = true)
        {
            Console.WriteLine($"\n[REQ]: Processing request '{request.Substring(0, Math.Min(request.Length, 500))}'");
            var cmd = new NpgsqlCommand(request, _connection);
            try
            {
                if (useReader)
                {
                    var reader = cmd.ExecuteReader();
                    var result = new List<object[]>();
                    while (reader.Read())
                    {
                        object[] objects = new object[reader.FieldCount];
                        reader.GetValues(objects);
                        result.Add(objects);
                    }
                    var cols = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                    reader.Close();
                    return new QueryResult(cols, result);
                }
                else return new QueryResult(cmd.ExecuteNonQuery());
            }
            catch (Exception e)
            {
                return new QueryResult(e);
            }
        }
    }
}