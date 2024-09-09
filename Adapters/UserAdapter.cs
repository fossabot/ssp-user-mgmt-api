// Copyright (c) 2024 RFull Development
// This source code is managed under the MIT license. See LICENSE in the project root.
using UserManagementApi.Database.Interfaces;
using UserManagementApi.Models;

namespace UserManagementApi.Adapters
{
    public class UserAdapter(IUserDatabaseClient client)
    {
        private readonly IUserDatabaseClient _client = client;

        public async Task<string> CreateItemAsync()
        {
            Guid guid;
            try
            {
                guid = await _client.CreateItemAsync();
            }
            catch (Exception e)
            {
                throw new IOException("Failed to create user.", e);
            }
            string id = guid.ToString();
            return id;
        }

        public async Task<User?> GetUserAsync(string id)
        {
            if (!Guid.TryParse(id, out Guid guid))
            {
                throw new ArgumentException(null, nameof(id));
            }

            Database.Models.Item? dbItem = await _client.GetItemAsync(guid);
            if (dbItem is null)
            {
                return null;
            }
            if ((dbItem.Id is not long dbItemId) || (dbItem.Guid is null))
            {
                throw new InvalidDataException();
            }

            UserName? name = await GetNameAsync(dbItemId);
            UserContact? contact = await GetContactAsync(dbItemId);
            User user = new()
            {
                Id = id,
                Name = name,
                Contact = contact
            };
            return user;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            ArgumentNullException.ThrowIfNull(user);
            if (!Guid.TryParse(user.Id, out Guid guid))
            {
                throw new ArgumentNullException(user.Id);
            }

            Database.Models.Item? dbItem = await _client.GetItemAsync(guid);
            if (dbItem is null)
            {
                return false;
            }
            if ((dbItem.Id is not long dbItemId) || (dbItem.Guid is null))
            {
                throw new InvalidDataException();
            }

            bool success = true;
            if (user.Name is not null)
            {
                success = await SetNameAsync(dbItemId, user.Name);
            }
            return success;
        }

        private async Task<UserName?> GetNameAsync(long dbItemId)
        {
            if (dbItemId < 1)
            {
                throw new InvalidDataException();
            }

            Database.Models.Name? dbName = await _client.GetNameAsync(dbItemId);
            if (dbName is null)
            {
                return null;
            }

            UserName name = new()
            {
                First = dbName.First,
                Middle = dbName.Middle,
                Last = dbName.Last,
                Display = dbName.Display,
            };
            return name;
        }

        public async Task<bool> SetNameAsync(long dbItemId, UserName name)
        {
            ArgumentNullException.ThrowIfNull(name);

            Database.Models.Name dbName = new()
            {
                ItemId = dbItemId,
                First = name.First,
                Middle = name.Middle,
                Last = name.Last,
                Display = name.Display
            };
            int result = await _client.SetNameAsync(dbName);
            bool success = result > 0;
            return success;
        }

        private async Task<UserContact?> GetContactAsync(long dbItemId)
        {
            if (dbItemId < 1)
            {
                throw new InvalidDataException();
            }

            UserContact contact = new();
            List<Email> emailAddresses = await GetEmailListAsync(dbItemId);
            if (emailAddresses.Count > 0)
            {
                contact.EmailAddresses = emailAddresses;
            }

            bool valid = false;
            foreach (var property in contact.GetType().GetProperties())
            {
                if (property.GetValue(contact) is not null)
                {
                    valid = true;
                    break;
                }
            }
            if (!valid)
            {
                return null;
            }
            return contact;
        }

        private async Task<List<Email>> GetEmailListAsync(long dbItemId)
        {
            if (dbItemId < 1)
            {
                throw new InvalidDataException();
            }

            List<Database.Models.Email> dbEmailAddresses = await _client.GetEmailListAsync(dbItemId);
            List<Email> emailAddresses = [];
            foreach (Database.Models.Email dbEmail in dbEmailAddresses)
            {
                if (string.IsNullOrEmpty(dbEmail.Address))
                {
                    throw new InvalidDataException();
                }
                Email emailAddress = new()
                {
                    Address = dbEmail.Address
                };
                emailAddresses.Add(emailAddress);
            }
            return emailAddresses;
        }

        public async Task<List<User>> GetUserListAsync(string? id, int limit)
        {
            Guid guid;
            if (id != null)
            {
                if (!Guid.TryParse(id, out guid))
                {
                    throw new ArgumentException(null, nameof(id));
                }
            }
            else
            {
                guid = await _client.GetFirstGuid();
            }

            List<Database.Models.List> dbItems = await _client.GetUserListAsync(guid, limit);
            List<User> users = [];
            foreach (Database.Models.List dbItem in dbItems)
            {
                if ((dbItem.Id is not long dbItemId) || (dbItem.Guid is not Guid dbItemGuid))
                {
                    throw new InvalidDataException();
                }

                UserName? name = await GetNameAsync(dbItemId);
                UserContact? contact = await GetContactAsync(dbItemId);
                User user = new()
                {
                    Id = dbItemGuid.ToString(),
                    Name = name,
                    Contact = contact
                };
                users.Add(user);
            }
            return users;
        }

        public async Task<long> GetTotalCountAsync()
        {
            long count = await _client.GetTotalCount();
            return count;
        }
    }
}
