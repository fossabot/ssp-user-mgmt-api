// Copyright (c) 2024 RFull Development
// This source code is managed under the MIT license. See LICENSE in the project root.
using System.Text.Json.Serialization;

namespace UserManagementApi.Models
{
    public record class User
    {
        [JsonPropertyName("id")]
        [JsonRequired]
        public required string Id { get; set; }

        [JsonPropertyName("name")]
        public UserName? Name { get; set; }

        [JsonPropertyName("contact")]
        public UserContact? Contact { get; set; }
    }
}
