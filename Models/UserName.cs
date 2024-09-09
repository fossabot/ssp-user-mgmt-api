// Copyright (c) 2024 RFull Development
// This source code is managed under the MIT license. See LICENSE in the project root.
using System.Text.Json.Serialization;

namespace UserManagementApi.Models
{
    public record class UserName
    {
        [JsonPropertyName("first")]
        public string? First { get; set; }

        [JsonPropertyName("middle")]
        public string? Middle { get; set; }

        [JsonPropertyName("last")]
        public string? Last { get; set; }

        [JsonPropertyName("display")]
        public string? Display { get; set; }
    }
}
