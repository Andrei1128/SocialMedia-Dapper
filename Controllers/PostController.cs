using System.Data;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia_Dapper.Data;
using SocialMedia_Dapper.Models.DTOs;

namespace SocialMedia_Dapper.Controllers
{
    [Route("Post")]
    [Authorize]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly DataContext _db;
        public PostController(IConfiguration config)
        {
            _db = new DataContext(config);
        }

        [HttpPost]
        public async Task<ActionResult> Post(PostCreatedDTO post)
        {
            string sql = @"EXEC CreatePost
                @UserId = @UserIdParam,
                @Content = @ContentParam,
                @ImageURL = @ImageURLParam";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserIdParam", this.User.FindFirst("UserId").Value, DbType.Int32);
            parameters.Add("@ContentParam", post.Content, DbType.String);
            parameters.Add("@ImageURLParam", post.ImageURL, DbType.String);
            if (post.GroupId != 0)
            {
                sql += ",@GroupId = @GroupIdParam";
                parameters.Add("@GroupIdParam", post.GroupId, DbType.Int32);
            }
            await _db.ExecuteSqlAsync(sql, parameters);
            return Ok();
        }
    }
}