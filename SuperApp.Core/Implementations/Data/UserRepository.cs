﻿using Microsoft.Data.Sqlite;
using SuperApp.Core.Extensions;
using SuperApp.Core.Interfaces.Data;
using SuperApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperApp.Core.Implementations.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly IDatabase _database;

        public UserRepository(IDatabase database)
        {
            _database = database;
        }

        public async Task<IEnumerable<UserDataModel>> QueryAllAsync()
        {
            string command = "SELECT Id, Login, Email, FirstName, LastName, Phone FROM Users";
            var result = await _database.GetAsync<UserDataModel>(command, new DbParameter[] { }, CommandType.Text);

            return result;
        }

        public async Task<IEnumerable<UserDataModel>> GetByPageAsync(int skip, int pageSize)
        {
            string command = "SELECT Id, Login, Email, FirstName, LastName, Phone " +
                "FROM Users " +
                "ORDER BY Id ASC " +
                "LIMIT @take OFFSET @skip";
            skip.EnsureNonNegative();
            pageSize.EnsureNonNegative();

            DbParameter[] dbParams = new DbParameter[]
            {
                new SqliteParameter("@take", pageSize),
                new SqliteParameter("@skip", skip)
            };

            var result = await _database.GetAsync<UserDataModel>(command, dbParams, CommandType.Text);

            return result;
        }

        public async Task<int> GetCountAsync()
        {
            string command = "SELECT Count(Id) FROM Users";
            var result = await _database.GetScalarAsync<int>(command, new DbParameter[] { }, CommandType.Text);

            return result;
        }
    }
}
