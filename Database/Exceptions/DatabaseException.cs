﻿// Copyright (c) 2024 RFull Development
// This source code is managed under the MIT license. See LICENSE in the project root.
namespace UserManagementApi.Database.Exceptions
{
    public class DatabaseException : IOException
    {
        public DatabaseException() : base()
        {
        }

        public DatabaseException(string message) : base(message)
        {
        }

        public DatabaseException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
