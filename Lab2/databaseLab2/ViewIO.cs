using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace databaseLab2
{
    public class ViewIO
    {
        private readonly TextReader _reader;
        private readonly TextWriter _writer;

        public ViewIO(TextReader reader, TextWriter writer)
        {
            _reader = reader;
            _writer = writer;
        }

        public enum Operation
        {
            Select,
            SelectAll,
            Insert,
            Remove,
            Update,
            SelectQuery,
            Fill,
            Exit
        }

        public bool InputOperation(out Operation op)
        {
            op = Operation.Select;

            _writer.WriteLine("Select operation: ");
            _writer.WriteLine("0. Select");
            _writer.WriteLine("1. Select All");
            _writer.WriteLine("2. Insert");
            _writer.WriteLine("3. Remove");
            _writer.WriteLine("4. Update");
            _writer.WriteLine("5. Select Query");
            _writer.WriteLine("6. Fill with random values");
            _writer.WriteLine("7. == Exit ==");
            _writer.Write("&> ");
            _writer.Flush();

            var input = _reader.ReadLine();
            _writer.WriteLine();
            
            if (!int.TryParse(input, out var opInt) || opInt > 7 || opInt < 0)
                return false;

            op = (Operation)opInt;
            return true;
        }

        public bool InputTable(List<Table> tables, out Table table)
        {
            table = null;

            _writer.WriteLine("Select table (empty to exit): ");
            for(int i = 0; i < tables.Count; i++)
                Console.WriteLine($"{i}. {tables[i].Name}");
            _writer.Write("&> ");
            _writer.Flush(); 
            
            var input = _reader.ReadLine();
            _writer.WriteLine();
            
            if (!int.TryParse(input, out var opInt) || opInt >= tables.Count || opInt < 0)
                return false;

            table = tables[opInt];
            return true;
        }

        public List<string> InputColNames(Table table, bool allowNull)
        {
            _writer.WriteLine("Possible column names: " + string.Join(", ", table.Columns));
            _writer.Write("Input column names (separated by ',')" + (allowNull ? "(empty to use all columns): " : ": "));
            _writer.Flush();
          
            var str = _reader.ReadLine();
            if (allowNull && string.IsNullOrWhiteSpace(str))
                return null;
            else 
                return str.Split(',').ToList();
        }

        public string InputCond(Table table, bool allowNull)
        {
            _writer.Write("Input condition " + (allowNull ? "(empty not to use condition): " : ": "));
            _writer.Flush(); 
            
            var str = _reader.ReadLine();
            if (allowNull && string.IsNullOrWhiteSpace(str))
                return null;
            else
                return str;
        }

        public int InputLimit(Table table, bool allowNull)
        {
            _writer.Write("Input limit " + (allowNull ? "(empty or '0' not to use limiting): " : ": "));
            _writer.Flush();
            
            var str = _reader.ReadLine();
            if (allowNull && string.IsNullOrWhiteSpace(str))
                return 0;
            else
                return int.Parse(str);
        }

        public List<(string, bool)> InputInsertData(Table table)
        {
            _writer.Write("Input data to insert (separated by ',')(start element with '@' to use functions): ");
            _writer.Flush();
            
            var str = _reader.ReadLine();
            return str.Split(',').Select(p =>
            {
                if (p.StartsWith("@")) return (p.Substring(1), true);
                else return (p, false);
            }).ToList();
        }

        public bool InputQuery(Dictionary<string, Func<ViewIO, QueryResult>> queries, out Func<ViewIO, QueryResult> action)
        {
            action = null;
            _writer.WriteLine("Select query (empty to exit): ");
            for (int i = 0; i < queries.Count; i++)
                Console.WriteLine($"{i}. {queries.ElementAt(i).Key}");
            _writer.Write("&> ");
            _writer.Flush();

            var input = _reader.ReadLine();
            if (!int.TryParse(input, out var opInt) || opInt >= queries.Count || opInt < 0)
                return false;

            action = queries.ElementAt(opInt).Value;
            return true;
        }

        public string InputString(string prompt)
        {
            _writer.Write(prompt + ": ");
            return _reader.ReadLine();
        }
    }
}