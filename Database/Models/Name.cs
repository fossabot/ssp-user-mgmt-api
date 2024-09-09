// Copyright (c) 2024 RFull Development
// This source code is managed under the MIT license. See LICENSE in the project root.
using UserManagementApi.Database.Attributes;

namespace UserManagementApi.Database.Models
{
    public record class Name
    {
        [Column(Name = "item_id")]
        public long? ItemId { get; set; }
        [Column(Name = "first")]
        public string? First { get; set; }
        [Column(Name = "middle")]
        public string? Middle { get; set; }
        [Column(Name = "last")]
        public string? Last { get; set; }
        [Column(Name = "display")]
        public string? Display { get; set; }
        [Column(Name = "version")]
        public int? Version { get; set; }
    }
}
