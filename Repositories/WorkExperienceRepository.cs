using System.Data.SqlClient;
using CVBackend.Dtos;
using CVBackend.Models;
using Dapper;

namespace CVBackend.Repositories
{
    public class WorkExperienceRepository
    {
        private readonly IConfiguration _configuration;

        public WorkExperienceRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> AddWorkExperienceAsync(WorkExperienceDto workExperience)
        {
            var sql = @"INSERT INTO WorkExperience (JobTitle, Description, StartDate, EndDate, Location) 
                        VALUES (@JobTitle, @Description, @StartDate, @EndDate, @Location)";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var affectedRows = await connection.ExecuteAsync(sql, workExperience);
                return affectedRows > 0;
            }
        }

        public async Task<IEnumerable<WorkExperience>> GetAllWorkExperiencesAsync()
        {
            var sql = "SELECT * FROM WorkExperience";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                return await connection.QueryAsync<WorkExperience>(sql);
            }
        }

        public async Task<WorkExperience> GetWorkExperienceByIdAsync(int id)
        {
            var sql = "SELECT * FROM WorkExperience WHERE Id = @Id";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                return await connection.QuerySingleOrDefaultAsync<WorkExperience>(sql, new { Id = id });
            }
        }

        public async Task<bool> UpdateWorkExperienceAsync(int id, WorkExperience workExperience)
        {
            var sql = @"
                UPDATE WorkExperience 
                SET 
                    JobTitle = @JobTitle, 
                    Description = @Description, 
                    StartDate = @StartDate, 
                    EndDate = @EndDate, 
                    Location = @Location
                WHERE Id = @Id";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var affectedRows = await connection.ExecuteAsync(sql, new 
                { 
                    workExperience.JobTitle, 
                    workExperience.Description, 
                    workExperience.StartDate, 
                    workExperience.EndDate, 
                    workExperience.Location, 
                    Id = id 
                });

                return affectedRows > 0;
            }
        }

        public async Task<bool> DeleteWorkExperienceAsync(int id)
        {
            var sql = "DELETE FROM WorkExperience WHERE Id = @Id";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
                return affectedRows > 0;
            }
        }





    }
}