using System.Collections.Generic;
using System.Linq;
using Lab2;

namespace databaseLab2
{
    public class Table
    {
        private readonly Model _model;

        public string Name { get; private set; }
        public string[] Columns { get; private set; }

        public Table(Model model, string tableName, string[] tableColumns)
        {
            Name = tableName;
            Columns = tableColumns;
            _model = model;
        }

        public QueryResult SelectQuery(List<string> colNames = null, string condition = null, int limit = 0)
        {
            var columns = (colNames == null || colNames.Count == 0) ?
                "*" :
                string.Join(",", colNames);
            var cond = condition == null ? "" : $"WHERE {condition}";
            var limString = limit == 0 ? "" : $"LIMIT {limit}";
            return _model.Request($"SELECT {columns} FROM {Name} {cond} {limString}");
        }

        public QueryResult InsertQuery(List<(string, bool)> data, List<string> colNames = null, int repeat = 1)
        {
            var columns = string.Join(",", colNames ?? Columns.ToList());
            var values = string.Join(",", data.Select(p => {
                if (!p.Item2) return $"'{p.Item1}'";
                else return p.Item1;
            }));
            var repValues = string.Join(",", Enumerable.Repeat($"({values})", repeat));
            return _model.Request($"INSERT INTO {Name} ({columns}) VALUES {repValues}", false);
        }

        public QueryResult RemoveQuery(string condition)
        {
            return _model.Request($"DELETE FROM {Name} WHERE {condition}", false);
        }

        public QueryResult UpdateQuery(List<(string, bool)> data, string condition, List<string> colNames = null)
        {
            var columns = colNames ?? Columns.ToList();
            var values = data.Select(p => {
                if (!p.Item2) return $"'{p.Item1}'";
                else return p.Item1;
            }).ToList();
            var set = "";
            for(int i = 0; i < columns.Count; i++)
            {
                set += $"{columns[i]} = {values[i]}";
            }
            return _model.Request($"UPDATE {Name} SET {set} WHERE {condition}", false);
        }
    }
}