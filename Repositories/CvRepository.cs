using System.Data.SqlClient;
using CVBackend.Dtos;
using CVBackend.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace CVBackend.Repositories
{
    public class CvRepository
    {
        private readonly IConfiguration _configuration;

        public CvRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<User> GetUserAsync(int userId)
        {
            var sql = @"SELECT * FROM Users WHERE UserId = @UserId;
                        SELECT * FROM Experiences WHERE UserId = @UserId;
                        SELECT * FROM Projects WHERE UserId = @UserId;";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                using (var multi = await connection.QueryMultipleAsync(sql, new { UserId = userId }))
                {
                    var user = await multi.ReadSingleOrDefaultAsync<User>();
                    if (user != null)
                    {
                        user.Experiences = (await multi.ReadAsync<Experience>()).ToList();
                        user.Projects = (await multi.ReadAsync<Project>()).ToList();
                    }
                    return user;
                }
            }
        }

        public async Task<bool> SaveUserAsync(User user)
        {
            var sqlUser = @"IF EXISTS (SELECT 1 FROM Users WHERE UserId = @UserId)
                            BEGIN
                                UPDATE Users
                                SET Name = @Name, JobTitle = @JobTitle, Email = @Email, ContactNumber = @ContactNumber, LinkedInLink = @LinkedInLink, GitHubLink = @GitHubLink, Address = @Address, Biography = @Biography, ProfessionalSummary = @ProfessionalSummary
                                WHERE UserId = @UserId;
                            END
                            ELSE
                            BEGIN
                                INSERT INTO Users (Name, JobTitle, Email, ContactNumber, LinkedInLink, GitHubLink, Address, Biography, ProfessionalSummary)
                                VALUES (@Name, @JobTitle, @Email, @ContactNumber, @LinkedInLink, @GitHubLink, @Address, @Biography, @ProfessionalSummary);
                                SELECT CAST(SCOPE_IDENTITY() as int);
                            END";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var userId = await connection.QuerySingleOrDefaultAsync<int>(sqlUser, user, transaction);
                        if (userId != 0)
                        {
                            user.UserId = userId;
                        }

                        var sqlDeleteExperience = @"DELETE FROM Experiences WHERE UserId = @UserId;";
                        await connection.ExecuteAsync(sqlDeleteExperience, new { UserId = user.UserId }, transaction);

                        var sqlInsertExperience = @"INSERT INTO Experiences (UserId, JobTitle, Description, StartDate, EndDate, Location, [Order])
                                                    VALUES (@UserId, @JobTitle, @Description, @StartDate, @EndDate, @Location, @Order);";

                        foreach (var exp in user.Experiences)
                        {
                            exp.UserId = user.UserId;
                            await connection.ExecuteAsync(sqlInsertExperience, exp, transaction);
                        }

                        var sqlDeleteProject = @"DELETE FROM Projects WHERE UserId = @UserId;";
                        await connection.ExecuteAsync(sqlDeleteProject, new { UserId = user.UserId }, transaction);

                        var sqlInsertProject = @"INSERT INTO Projects (UserId, Title, Role, Description, StartDate, EndDate, [Order])
                                                 VALUES (@UserId, @Title, @Role, @Description, @StartDate, @EndDate, @Order);";

                        foreach (var project in user.Projects)
                        {
                            project.UserId = user.UserId;
                            await connection.ExecuteAsync(sqlInsertProject, project, transaction);
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
