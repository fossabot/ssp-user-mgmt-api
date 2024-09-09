// Copyright (c) 2024 RFull Development
// This source code is managed under the MIT license. See LICENSE in the project root.
namespace UserManagementApi.Database.Interfaces
{
    public interface IUserDatabaseClient
    {
        public Task<Guid> CreateItemAsync();
        public Task<Models.Item?> GetItemAsync(Guid guid);
        public Task<Models.Name?> GetNameAsync(long itemId);
        public Task<int> SetNameAsync(Models.Name name);
        public Task<int> CreateEmailAsync(Models.Email email);
        public Task<List<Models.Email>> GetEmailListAsync(long itemId);
        public Task<List<Models.List>> GetUserListAsync(Guid guid, int limit);
        public Task<long> GetTotalCount();
        public Task<Guid> GetFirstGuid();
    }
}
