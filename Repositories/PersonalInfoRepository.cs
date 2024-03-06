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

        
        public async Task<PersonalInfo> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM PersonalInfo WHERE Id = @Id";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var result = await connection.QuerySingleOrDefaultAsync<PersonalInfo>(sql, new { Id = id });
                return result;
            }
        }

        public async Task<bool> UpdatePersonalInfoAsync(PersonalInfo personalInfo)
        {
            var sql = @"UPDATE PersonalInfo SET 
                        Name = @Name, 
                        JobTitle = @JobTitle, 
                        Email = @Email, 
                        ContactNumber = @ContactNumber, 
                        LinkedInLink = @LinkedInLink, 
                        Address = @Address 
                        WHERE Id = @Id";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var affectedRows = await connection.ExecuteAsync(sql, new
                {
                    personalInfo.Name,
                    personalInfo.JobTitle,
                    personalInfo.Email,
                    personalInfo.ContactNumber,
                    personalInfo.LinkedInLink,
                    personalInfo.Address,
                    personalInfo.Id
                });

                return affectedRows > 0;
            }
        }

        public async Task<bool> DeletePersonalInfoAsync(int id)
        {
            var sql = "DELETE FROM PersonalInfo WHERE Id = @Id";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
                return affectedRows > 0;
            }
        }
    }
}
