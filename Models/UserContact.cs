// Copyright (c) 2024 RFull Development
// This source code is managed under the MIT license. See LICENSE in the project root.
using System.Text.Json.Serialization;

namespace UserManagementApi.Models
{
    public record class UserContact
    {
        [JsonPropertyName("emailAddresses")]
        public List<Email>? EmailAddresses { get; set; }
    }
}
