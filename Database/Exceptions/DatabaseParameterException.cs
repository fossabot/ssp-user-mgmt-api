// Copyright (c) 2024 RFull Development
// This source code is managed under the MIT license. See LICENSE in the project root.
namespace UserManagementApi.Database.Exceptions
{
    public class DatabaseParameterException : ArgumentException
    {
        public DatabaseParameterException() : base()
        {
        }

        public DatabaseParameterException(string message) : base(message)
        {
        }

        public DatabaseParameterException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
