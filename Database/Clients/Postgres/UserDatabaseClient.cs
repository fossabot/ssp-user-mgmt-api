// Copyright (c) 2024 RFull Development
// This source code is managed under the MIT license. See LICENSE in the project root.
using Dapper;
using Npgsql;
using UserManagementApi.Database.Exceptions;
using UserManagementApi.Database.Interfaces;

namespace UserManagementApi.Database.Clients.Postgres
{
    public class UserDatabaseClient(string connectionString, string schema) : DatabaseClient, IUserDatabaseClient, IDisposable
    {
        private readonly NpgsqlConnection _connection = new(connectionString);
        private readonly string _schema = schema;

        public async Task<Guid> CreateItemAsync()
        {
            string table = $"{_schema}.item";
            string columns = GenerateColumnListAllQuery(typeof(Models.Item));
            string query = $"""
                INSERT INTO
                	{table}
                DEFAULT VALUES
                RETURNING
                    {columns};
                """;
            Models.Item? item = await _connection.QueryFirstOrDefaultAsync<Models.Item>(query);
            if ((item is null) || (item.Guid is not Guid guid))
            {
                throw new DatabaseException("Failed to create item.");
            }
            return guid;
        }

        public async Task<Models.Item?> GetItemAsync(Guid guid)
        {
            string table = $"{_schema}.item";
            string columns = GenerateColumnListAllQuery(typeof(Models.Item));
            string query = $"""
                SELECT
                    {columns}
                FROM
                	{table}
                WHERE
                	"guid" = @Guid;
                """;
            Models.Item? item = await _connection.QueryFirstOrDefaultAsync<Models.Item>(query, new
            {
                Guid = guid
            });
            return item;
        }

        public async Task<Models.Name?> GetNameAsync(long itemId)
        {
            if (itemId < 1)
            {
                throw new DatabaseParameterException(nameof(itemId));
            }

            string table = $"{_schema}.name";
            string columns = GenerateColumnListAllQuery(typeof(Models.Name));
            string query = $"""
                SELECT
                    {columns}
                FROM
                	{table}
                WHERE
                	"item_id" = @ItemId;
                """;
            Models.Name? item = await _connection.QueryFirstOrDefaultAsync<Models.Name>(query, new
            {
                ItemId = itemId
            });
            return item;
        }

        public async Task<int> SetNameAsync(Models.Name name)
        {
            if ((name is null) || (name.ItemId == null))
            {
                throw new DatabaseParameterException();
            }

            string table = $"{_schema}.name";
            string cte = "version_cte";
            string insertColumns = GenerateColumnListQuery(name, [
                nameof(Models.Name.Version)
                ]);
            string insertValues = GenerateInsertValueListQuery(name, [
                nameof(Models.Name.Version)
                ]);
            string updateSet = GenerateUpdateSetListQuery(name, [
                nameof(Models.Name.ItemId),
                nameof(Models.Name.Version)
                ]);
            string query = $"""
                WITH
                    {cte} AS (
                        SELECT
                            "version"
                        FROM
                            {table}
                        WHERE
                            "item_id" = @ItemId
                    )
                INSERT INTO
                	{table} ({insertColumns})
                VALUES
                	({insertValues})
                ON CONFLICT ("item_id") DO
                UPDATE
                SET
                    {updateSet},
                    "version" = {table}.version + 1
                WHERE
                    {table}.version = (
                        SELECT
                            "version"
                        FROM
                            {cte}
                    );
                """;
            int rows = await _connection.ExecuteAsync(query, name);
            return rows;
        }

        public async Task<int> CreateEmailAsync(Models.Email email)
        {
            if ((email is null) || (email.ItemId == null) || string.IsNullOrEmpty(email.Address))
            {
                throw new DatabaseParameterException();
            }

            string table = $"{_schema}.email";
            List<string> excludeColumns = [
                nameof(Models.Name.Version)
                ];
            string columns = GenerateColumnListQuery(email, excludeColumns);
            string values = GenerateInsertValueListQuery(email, excludeColumns);
            string query = $"""
                INSERT INTO
                	{table} ({columns})
                VALUES
                	({values});
                """;
            int rows = await _connection.ExecuteAsync(query, email);
            return rows;
        }

        public async Task<List<Models.Email>> GetEmailListAsync(long itemId)
        {
            if (itemId < 1)
            {
                throw new DatabaseParameterException(nameof(itemId));
            }

            string table = $"{_schema}.email";
            string columns = GenerateColumnListAllQuery(typeof(Models.Email));
            string query = $"""
                SELECT
                    {columns}
                FROM
                	{table}
                WHERE
                	"item_id" = @ItemId;
                """;
            IEnumerable<Models.Email> results = await _connection.QueryAsync<Models.Email>(query, new
            {
                ItemId = itemId
            });
            List<Models.Email> items = results.ToList();
            return items;
        }

        public async Task<List<Models.List>> GetUserListAsync(Guid guid, int limit)
        {
            limit = Math.Min(Math.Max(limit, 1), 128);

            string cte = $"{_schema}.item";
            string table = $"{_schema}.list";
            string columns = GenerateColumnListAllQuery(typeof(Models.List));
            string condition;
            if (guid != Guid.Empty)
            {
                condition = """
                    	"id" >= (
                    		SELECT
                    			"id"
                    		FROM
                    			id_cte
                    	)
                    """;
            }
            else
            {
                condition = "TRUE";
            }
            string query = $"""
                WITH
                	id_cte AS (
                		SELECT
                			"id"
                		FROM
                            {cte}
                		WHERE
                			"guid" = @Guid
                	)
                SELECT
                    {columns}
                FROM
                	{table}
                WHERE
                    {condition}
                LIMIT
                	@Limit;
                """;
            IEnumerable<Models.List> results = await _connection.QueryAsync<Models.List>(query, new
            {
                Guid = guid,
                Limit = limit
            });
            List<Models.List> items = results.ToList();
            return items;
        }

        public async Task<long> GetTotalCount()
        {
            string query = $"""
                SELECT
                    n_live_tup
                FROM
                    pg_catalog.pg_stat_user_tables
                WHERE
                    relname = 'item';
                """;
            long count = await _connection.ExecuteScalarAsync<long?>(query) ?? 0;
            return count;
        }

        public async Task<Guid> GetFirstGuid()
        {
            string table = $"{_schema}.item";
            string query = $"""
                SELECT
                    "guid"
                FROM
                    {table}
                ORDER BY
                    "id"
                LIMIT
                    1;
                """;
            Guid guid = await _connection.ExecuteScalarAsync<Guid?>(query) ?? Guid.Empty;
            return guid;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _connection.Dispose();
            }
        }
    }
}
