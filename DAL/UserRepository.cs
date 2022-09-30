using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        public UserRepository(string connection)
        {
            _connectionString = connection;
        }

        public async void CreateAsync(UserDal user)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO Users (FIO, Phone, Email, Password, LastLogin) VALUES(@FIO, @Phone, @Email, @Password, @LastLogin)";
                await db.ExecuteAsync(query, user);
            }
        }

        public async Task<UserDal> GetByPhoneAsync(string phone)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryFirstOrDefaultAsync<UserDal>("SELECT * FROM Users WHERE Phone = @phone", new { phone });
            }
        }

        public async Task<bool> IsUserExistsAsync(UserDal user)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var phone = user.Phone ?? "";
                var email = user.Email ?? "";
                var query = await db.QueryAsync<object>("SELECT * FROM Users WHERE Phone = @phone OR Email = @email",
                    new { phone, email });
                return query.Any();
            }
        }

        public async Task UpdateLastLoginAsync(string phone, DateTime loginTime)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                await db.ExecuteAsync("UPDATE Users SET LastLogin = @loginTime WHERE Phone = @phone",
                    new { loginTime, phone });
            }
        }
    }
}