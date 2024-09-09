// Copyright (c) 2024 RFull Development
// This source code is managed under the MIT license. See LICENSE in the project root.
using System.Text.Json.Serialization;

namespace UserManagementApi.Database.Models
{
    public record class Config
    {
        [JsonPropertyName("host")]
        [JsonRequired]
        public required string Host { get; init; }
        [JsonPropertyName("port")]
        [JsonRequired]
        public required int Port { get; init; }
        [JsonPropertyName("sslMode")]
        [JsonRequired]
        public required string SslMode { get; init; }
        [JsonPropertyName("pooling")]
        public PoolingSection? Pooling { get; init; }
        [JsonPropertyName("account")]
        [JsonRequired]
        public required AccountSection Account { get; init; }
        [JsonPropertyName("database")]
        [JsonRequired]
        public required DatabaseSection Database { get; init; }

        public record class PoolingSection
        {
            [JsonPropertyName("minSize")]
            public required int MinSize { get; init; }
            [JsonPropertyName("maxSize")]
            public required int MaxSize { get; init; }
        }

        public record class AccountSection
        {
            [JsonPropertyName("username")]
            public required string Username { get; init; }
            [JsonPropertyName("password")]
            public required string Password { get; init; }
        }

        public record class DatabaseSection
        {
            [JsonPropertyName("name")]
            public required string Name { get; init; }
            [JsonPropertyName("schema")]
            public required string Schema { get; init; }
        }
    }
}
