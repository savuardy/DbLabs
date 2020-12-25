using System;
using System.Collections.Generic;
using System.IO;
using Lab2;

namespace databaseLab2
{
    public class View
    {
        public struct DbSpecificData
        {
            public Model Model;
            public List<Table> Tables;
            public Dictionary<string, Func<ViewIO, QueryResult>> Queries;
            public Dictionary<string, Func<ViewIO, QueryResult>> Fills;
            public TextReader Reader;
            public TextWriter Writer;
        }
    
        private readonly ViewIO _io;
        private readonly DbSpecificData _data;

        public View(DbSpecificData data)
        {
            _data = data;
            _io = new ViewIO(
                data.Reader ?? Console.In,
                data.Writer ?? Console.Out);
        }

        public QueryResult Cycle()
        {
            if (_io.InputOperation(out var operation))
            {
                switch (operation)
                {
                    case ViewIO.Operation.Select:
                        {
                            if (_io.InputTable(_data.Tables, out var table))
                                return table.SelectQuery(
                                        _io.InputColNames(table, true),
                                        _io.InputCond(table, true),
                                        _io.InputLimit(table, true));
                            else return null;
                        }
                    case ViewIO.Operation.SelectAll:
                        {
                            if (_io.InputTable(_data.Tables, out var table))
                                return table.SelectQuery();
                            return null;
                        }
                    case ViewIO.Operation.Insert:
                        {
                            if (_io.InputTable(_data.Tables, out var table))
                                return table.InsertQuery(
                                        _io.InputInsertData(table),
                                        _io.InputColNames(table, true));
                            else return null;
                        }
                    case ViewIO.Operation.Remove:
                        {
                            if (_io.InputTable(_data.Tables, out var table))
                                return table.RemoveQuery(
                                        _io.InputCond(table, false));
                            else return null;
                        }
                    case ViewIO.Operation.Update:
                        {
                            if (_io.InputTable(_data.Tables, out var table))
                                return table.UpdateQuery(
                                        _io.InputInsertData(table),
                                        _io.InputCond(table, false),
                                        _io.InputColNames(table, true));
                            else return null;
                        }
                    case ViewIO.Operation.SelectQuery:
                        {
                            if (_io.InputQuery(_data.Queries, out var func))
                                return func(_io);
                            else return null;
                        }
                    case ViewIO.Operation.Fill:
                        {
                            if (_io.InputQuery(_data.Fills, out var func))
                                return func(_io);
                            else return null;
                        }
                    case ViewIO.Operation.Exit:
                        Environment.Exit(0);
                        return null;
                    default:
                        return null;
                }
            }
            else return null;
        }
    }
}
