// Copyright (c) 2024 RFull Development
// This source code is managed under the MIT license. See LICENSE in the project root.
using System.Text.Json.Serialization;

namespace UserManagementApi.Models
{
    public record class Email
    {
        [JsonPropertyName("address")]
        [JsonRequired]
        public required string Address { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}
