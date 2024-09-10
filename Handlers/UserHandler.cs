// Copyright (c) 2024 RFull Development
// This source code is managed under the MIT license. See LICENSE in the project root.
using Microsoft.AspNetCore.Mvc;
using UserManagementApi.Adapters;
using UserManagementApi.Database.Clients.Postgres;
using UserManagementApi.Database.Interfaces;

namespace UserManagementApi.Handlers
{
    public static class UserHandler
    {
        public static void AddUserHandler(this WebApplication app)
        {
            var group = app.MapGroup("/users");
            group.MapGet("/", GetUserListAsync);
            group.MapPost("/", CreateUserAsync);
            group.MapGet("/{id}", GetUserAsync);
            group.MapPut("/{id}", UpdateUserAsync);
            group.MapDelete("/{id}", DeleteUserAsync);
        }

        private static async Task<IResult> GetUserListAsync(
            [FromQuery(Name = "start-id")] string? startId,
            [FromQuery(Name = "limit")] int limit = 20
            )
        {
            List<Models.User>? users;
            long totalCount;
            try
            {
                DatabaseClientFactory databaseClientFactory = new();
                IUserDatabaseClient databaseClient = databaseClientFactory.CreateUserDatabaseClient();
                UserAdapter adapter = new(databaseClient);
                users = await adapter.GetUserListAsync(startId, limit);
                totalCount = await adapter.GetTotalCountAsync();
            }
            catch (ArgumentException)
            {
                return Results.BadRequest();
            }
            Models.UserListGetResponse response = new()
            {
                TotalCount = totalCount,
                Count = users.Count,
                Users = users
            };
            return Results.Ok(response);
        }

        private static async Task<IResult> CreateUserAsync()
        {
            string id;
            try
            {
                DatabaseClientFactory databaseClientFactory = new();
                IUserDatabaseClient databaseClient = databaseClientFactory.CreateUserDatabaseClient();
                UserAdapter adapter = new(databaseClient);
                id = await adapter.CreateItemAsync();
            }
            catch (ArgumentException)
            {
                return Results.BadRequest();
            }
            Models.UserCreateResponse response = new()
            {
                Id = id
            };
            return Results.Ok(response);
        }

        private static async Task<IResult> GetUserAsync(
            [FromRoute(Name = "id")] string id
            )
        {
            Models.User? user;
            try
            {
                DatabaseClientFactory databaseClientFactory = new();
                IUserDatabaseClient databaseClient = databaseClientFactory.CreateUserDatabaseClient();
                UserAdapter adapter = new(databaseClient);
                user = await adapter.GetUserAsync(id);
            }
            catch (ArgumentException)
            {
                return Results.BadRequest();
            }
            if (user is null)
            {
                return Results.NotFound();
            }
            Models.UserGetResponse response = new()
            {
                Id = user.Id,
                Name = user.Name,
                Contact = user.Contact
            };
            return Results.Ok(response);
        }

        private static async Task<IResult> UpdateUserAsync(
            [FromRoute(Name = "id")] string id,
            [FromBody] Models.UserUpdateRequest updateRequest
            )
        {
            bool success;
            try
            {
                DatabaseClientFactory databaseClientFactory = new();
                IUserDatabaseClient databaseClient = databaseClientFactory.CreateUserDatabaseClient();
                UserAdapter adapter = new(databaseClient);
                Models.User user = new()
                {
                    Id = id,
                    Name = updateRequest.Name,
                    Contact = updateRequest.Contact
                };
                success = await adapter.UpdateUserAsync(user);
            }
            catch (ArgumentException)
            {
                return Results.BadRequest();
            }
            if (!success)
            {
                return Results.NotFound();
            }
            return Results.Ok();
        }

        private static void DeleteUserAsync(
            [FromRoute(Name = "id")] string id
            )
        {
            Results.BadRequest();
            return;
        }
    }
}
