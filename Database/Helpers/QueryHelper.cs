// Copyright (c) 2024 RFull Development
// This source code is managed under the MIT license. See LICENSE in the project root.
using System.Reflection;
using UserManagementApi.Database.Attributes;
using UserManagementApi.Database.Exceptions;

namespace UserManagementApi.Database.Helpers
{
    public class QueryHelper(object model, List<string>? excludeColumns = null)
    {
        private readonly object _model = model;
        private readonly PropertyInfo[] _properties = model.GetType().GetProperties();
        private readonly List<string> _excludeColumns = excludeColumns ?? [];

        public List<string> GetPropertyNameList()
        {
            List<string> names = [];
            foreach (var property in _properties)
            {
                string name = property.Name;
                if (_excludeColumns.Contains(name))
                {
                    continue;
                }
                object? value = property.GetValue(_model);
                if (value is null)
                {
                    continue;
                }
                names.Add(name);
            }
            return names;
        }

        public static List<string> GetPropertyNameListAll(Type type)
        {
            PropertyInfo[] properties = type.GetProperties();
            List<string> names = properties.Select(property => property.Name).ToList();
            return names;
        }

        public List<string> GetColumnNameList()
        {
            List<string> names = [];
            foreach (var property in _properties)
            {
                string name = property.Name;
                if (_excludeColumns.Contains(name))
                {
                    continue;
                }
                object? value = property.GetValue(_model);
                if (value is null)
                {
                    continue;
                }
                ColumnAttribute? column = property.GetCustomAttribute<ColumnAttribute>() ?? throw new DatabaseException();
                names.Add(column.Name);
            }
            return names;
        }

        public static List<string> GetColumnNameListAll(Type type)
        {
            PropertyInfo[] properties = type.GetProperties();
            List<string> names = [];
            foreach (var property in properties)
            {
                ColumnAttribute column = property.GetCustomAttribute<ColumnAttribute>() ?? throw new DatabaseException();
                names.Add(column.Name);
            }
            return names;
        }

        public List<KeyValuePair<string, string>> GetColumnList()
        {
            List<KeyValuePair<string, string>> items = [];
            foreach (var property in _properties)
            {
                string name = property.Name;
                if (_excludeColumns.Contains(name))
                {
                    continue;
                }
                object? value = property.GetValue(_model);
                if (value is null)
                {
                    continue;
                }
                ColumnAttribute column = property.GetCustomAttribute<ColumnAttribute>() ?? throw new DatabaseException();
                items.Add(new KeyValuePair<string, string>(column.Name, name));
            }
            return items;
        }
    }
}
