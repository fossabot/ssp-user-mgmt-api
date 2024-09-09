var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.UserManagementApi>("usermanagementapi");

builder.Build().Run();
