// Copyright (c) 2024 RFull Development
// This source code is managed under the MIT license. See LICENSE in the project root.
using UserManagementApi.Database.Models;

namespace UserManagementApi.Database.Clients
{
    public static class ConfigLoader
    {
        public static readonly Config Config;

        static ConfigLoader()
        {
            Config = LoadConfig();
        }

        private static Config LoadConfig()
        {
            ConfigurationBuilder configurationBuilder = new();
            configurationBuilder.AddEnvironmentVariables("USER_MGMT_DB_");
            var configuration = configurationBuilder.Build();

            string host = configuration["HOST"] ?? throw new InvalidOperationException();
            int port = int.Parse(configuration["PORT"] ?? throw new InvalidOperationException());
            string sslMode = configuration["SSL_MODE"] ?? throw new InvalidOperationException();
            Config.PoolingSection? pooling = LoadPoolingSection(configuration);
            Config.AccountSection account = LoadAccountSection(configuration);
            Config.DatabaseSection database = LoadDatabaseSection(configuration);
            Config config = new()
            {
                Host = host,
                Port = port,
                SslMode = sslMode,
                Pooling = pooling,
                Account = account,
                Database = database
            };
            return config;
        }

        private static Config.PoolingSection? LoadPoolingSection(IConfigurationRoot configuration)
        {
            string? minSizeRaw = configuration["POOLING_MIN_SIZE"];
            string? maxSizeRaw = configuration["POOLING_MAX_SIZE"];
            if ((minSizeRaw is null) && (maxSizeRaw is null))
            {
                return null;
            }
            int minSize = int.Parse(minSizeRaw ?? throw new InvalidOperationException());
            int maxSize = int.Parse(maxSizeRaw ?? throw new InvalidOperationException());
            Config.PoolingSection pooling = new()
            {
                MinSize = minSize,
                MaxSize = maxSize
            };
            return pooling;
        }

        private static Config.AccountSection LoadAccountSection(IConfigurationRoot section)
        {
            string username = section["USERNAME"] ?? throw new InvalidOperationException();
            string password = section["PASSWORD"] ?? throw new InvalidOperationException();
            Config.AccountSection account = new()
            {
                Username = username,
                Password = password
            };
            return account;
        }

        private static Config.DatabaseSection LoadDatabaseSection(IConfigurationRoot section)
        {
            string name = section["NAME"] ?? throw new InvalidOperationException();
            string schema = section["SCHEMA"] ?? throw new InvalidOperationException();
            Config.DatabaseSection database = new()
            {
                Name = name,
                Schema = schema
            };
            return database;
        }
    }
}
