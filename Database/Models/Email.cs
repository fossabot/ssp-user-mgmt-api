// Copyright (c) 2024 RFull Development
// This source code is managed under the MIT license. See LICENSE in the project root.
using UserManagementApi.Database.Attributes;

namespace UserManagementApi.Database.Models
{
    public record class Email
    {
        [Column(Name = "item_id")]
        public long? ItemId { get; set; }
        [Column(Name = "address")]
        public string? Address { get; set; }
        [Column(Name = "description")]
        public string? Description { get; set; }
        [Column(Name = "version")]
        public int? Version { get; set; }
    }
}
