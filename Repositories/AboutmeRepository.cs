using System.Data.SqlClient;
using CVBackend.Dtos;
using CVBackend.Models;
using Dapper;

namespace CVBackend.Repositories
{
    public class AboutmeRepository
    {
        private readonly IConfiguration _configuration;

        public AboutmeRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<bool> AddAboutMeAsync(Aboutme aboutMe)
        {
            var sql = @"INSERT INTO AboutMe (Biography) VALUES (@Biography)";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var affectedRows = await connection.ExecuteAsync(sql, aboutMe);
                return affectedRows > 0;
            }
        }

        public async Task<Aboutme> GetAboutMeAsync(int id)
        {
            var sql = "SELECT * FROM AboutMe WHERE Id = @Id";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                return await connection.QuerySingleOrDefaultAsync<Aboutme>(sql, new { Id = id });
            }
        }


        public async Task<bool> UpdateAboutMeAsync(int id, AboutmeDto aboutMeDto)
        {
            var sql = @"UPDATE AboutMe SET Biography = @Biography WHERE Id = @Id";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var affectedRows = await connection.ExecuteAsync(sql, new { aboutMeDto.Biography, Id = id });
                return affectedRows > 0;
            }
        }

        public async Task<bool> DeleteAboutMeAsync(int id)
        {
            var sql = "DELETE FROM AboutMe WHERE Id = @Id";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
                return affectedRows > 0;
            }
        }

    }
}