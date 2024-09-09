// Copyright (c) 2024 RFull Development
// This source code is managed under the MIT license. See LICENSE in the project root.
using System.Data;
using UserManagementApi.Database.Helpers;

namespace UserManagementApi.Database.Clients.Postgres
{
    public abstract class DatabaseClient()
    {
        protected string GenerateColumnListAllQuery(Type type)
        {
            List<string> names = QueryHelper.GetColumnNameListAll(type);
            List<string> paramNames = names.Select(name => $"\"{name}\"").ToList();
            string query = string.Join(',', paramNames);
            return query;
        }

        protected string GenerateColumnListQuery(object model, List<string>? excludeColumns = null)
        {
            QueryHelper helper = new(model, excludeColumns);
            List<string> names = helper.GetColumnNameList();
            List<string> paramNames = names.Select(name => $"\"{name}\"").ToList();
            string query = string.Join(',', paramNames);
            return query;
        }

        protected string GenerateInsertValueListQuery(object model, List<string>? excludeColumns = null)
        {
            QueryHelper helper = new(model, excludeColumns);
            List<string> names = helper.GetPropertyNameList();
            List<string> paramNames = names.Select(name => $"@{name}").ToList();
            string query = string.Join(',', paramNames);
            return query;
        }

        protected string GenerateUpdateSetListQuery(object model, List<string>? excludeColumns = null)
        {
            QueryHelper helper = new(model, excludeColumns);
            List<KeyValuePair<string, string>> items = helper.GetColumnList();
            List<string> paramItems = items.Select(item => $"\"{item.Key}\" = @{item.Value}").ToList();
            string query = string.Join(',', paramItems);
            return query;
        }
    }
}
