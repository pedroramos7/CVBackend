using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using CVBackend.Models;

namespace CVBackend.Repositories
{
    public class PersonalInfoRepository
    {
        private readonly IConfiguration _configuration;

        public PersonalInfoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<PersonalInfo>> GetAllAsync()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT * FROM PersonalInfo";
                return await connection.QueryAsync<PersonalInfo>(query);
            }
        }

        public async Task<bool> AddPersonalInfoAsync(PersonalInfo personalInfo)
        {
            var sql = @"INSERT INTO PersonalInfo 
                    (Name, JobTitle, Email, ContactNumber, LinkedInLink, Address) 
                        VALUES (
                            @Name, 
                            @JobTitle, 
                            @Email, 
                            @ContactNumber, 
                            @LinkedInLink, 
                            @Address
                            )";
            
            using(var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var affectedRows = await connection.ExecuteAsync(sql, personalInfo);
                return affectedRows > 0;
            }
        }

        // Implement other methods using Dapper here
    }
}
