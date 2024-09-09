// Copyright (c) 2024 RFull Development
// This source code is managed under the MIT license. See LICENSE in the project root.
using UserManagementApi.Database.Attributes;

namespace UserManagementApi.Database.Models
{
    public record class List
    {
        [Column(Name = "id")]
        public long? Id { get; init; }
        [Column(Name = "guid")]
        public Guid? Guid { get; init; }
        [Column(Name = "display")]
        public string? Display { get; set; }
    }
}
