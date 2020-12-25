using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Lab2;

namespace databaseLab2
{
    class Program
    {
        private const string ConfigFile = "dbConfig.xml";

        private static string _hostName;
        private static string _databaseName;
        private static string _login;
        private static string _password;
        
        private static void ReadConfig()
        {
            try
            {
                var doc = new XmlDocument();
                doc.Load(ConfigFile);
                var db = doc.GetElementsByTagName("database")[0];
                _hostName = db.Attributes["host"].Value;
                _databaseName = db.Attributes["db_name"].Value;
                db = doc.GetElementsByTagName("login")[0];
                _login = db.Attributes["username"].Value;
                _password = db.Attributes["password"].Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong while loading config...\n{ex}");
                Console.ReadKey(true);
                Environment.Exit(1);
            }
        }

        private static Model SetupDatabase()
        {
            var m = new Model(_hostName, _databaseName, _login, _password);
            if (!m.Connect())
            {
                Console.ReadKey(true);
                Environment.Exit(1);
            }
            return m;
        }

        private static List<Table> SetupTables(Model model)
        {
            return new List<Table>()
            {
                new Table(model, "abonement", new string[] { "abon_id", "abon_owner_id" }),
                new Table(model, "author", new string[] { "author_name" }),
                new Table(model, "authored_by", new string[] { "author_id", "book_id" }),
                new Table(model, "book", new string[] { "book_id", "book_name", "return_date", "abon_id" }),
                new Table(model, "reader", new string[] { "reader_name","reader_address","reader_dob" }),
                new Table(model, "buffer", new string[]{"randnumber"})
            };
        }

        private static View SetupView(Model model, List<Table> tables)
        {
            return new View(new View.DbSpecificData()
            {
                Model = model,
                Tables = tables,
                Fills = new Dictionary<string, Func<ViewIO, QueryResult>>()
                {
                    {
                        "Fill `authors` with random values",
                        p =>
                        {
                            var count = int.Parse(p.InputString("Input count"));
                            return tables[1].InsertQuery(
                                new List<(string, bool)>
                                {
                                    ("chr(trunc(65+random()*25)::int) || chr(trunc(65+random()*25)::int) ||chr(trunc(65+random()*25)::int) || chr(trunc(65+random()*25)::int)", true)
                                },
                                null,
                                count);
                        }
                    },
                    {
                        "Fill `books` with random values",
                        p =>
                        {
                            var count = int.Parse(p.InputString("Input count"));
                            return tables[4].InsertQuery(
                                new List<(string, bool)>
                                {
                                    ("trunc(100+random()*30000)::int", true),
                                    ("chr(trunc(65+random()*25)::int) || chr(trunc(65+random()*25)::int) || " +
                                     "chr(trunc(65+random()*25)::int) || chr(trunc(65+random()*25)::int)", true)
                                },
                                null,
                                count);
                        }
                    }
                },
                Queries = new Dictionary<string, Func<ViewIO, QueryResult>>()
                {
                    {
                        "Пошук книг, по даті повернення та вивід на екран",
                        p => 
                        {
                            return model.Request("SELECT book_name, author_name FROM book b "+
                                                 "JOIN authored_by ab on b.book_id=ab.book_id JOIN author a on ab.author_id=a.author_id " +
                                                 $" WHERE return_date < '{p.InputString("Input condition for return date :")}'");
                        }
                    },
                    {
                        "Пошук читачів, по їх адрессі та їх айді",
                        p => 
                        {
                            return model.Request("SELECT reader_name, reader_address, reader_dob FROM reader r " +
                                                 $"WHERE reader_address like '{p.InputString("Input condition for reader_address")}'" +
                                                 " and reader_id in (select abon_owner_id from abonement)");
                        }
                    },
                }
            });
        }

        static void Main()
        {
            ReadConfig();
            SetupDatabase();
            var model = SetupDatabase();
            var tables = SetupTables(model); 
            var view = SetupView(model, tables);

            Console.WriteLine("=======================================================");
            Console.WriteLine("|  |");
            Console.WriteLine("=======================================================\n");

            while (true)
            {
                var result = view.Cycle();
                if (result == null) Console.WriteLine("\n[FAILED]: Input failed");
                else Console.WriteLine(result);
                Console.WriteLine("\n");
            }
        }
    }
}