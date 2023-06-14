using System.Data;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia_Dapper.Data;
using SocialMedia_Dapper.Models.DTOs;

namespace SocialMedia_Dapper.Controllers
{
    [Route("user")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _db;
        public UserController(IConfiguration config)
        {
            _db = new DataContext(config);
        }

        [HttpPost("addFriend/{friendId:int}")]
        public async Task<IActionResult> AddFriend(int friendId)
        {
            if (friendId != 0)
            {
                int myUserId = int.Parse(this.User.FindFirst("UserId").Value);
                if (myUserId != friendId)
                {
                    string sql = @"INSERT INTO UserRequest(RequestedUserId , UserId)
                        VALUES (@RequestedUserIdParam, @UserIdParam)";
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@RequestedUserIdParam", friendId, DbType.Int32);
                    parameters.Add("@UserIdParam", myUserId, DbType.Int32);
                    await _db.ExecuteSqlAsync(sql, parameters);
                    return Ok();
                }
                else return BadRequest();
            }
            else return BadRequest();
        }
        [HttpPost("acceptFriend/{userId:int}")]
        public async Task<IActionResult> AcceptFriend(int userId)
        {
            if (userId != 0)
            {
                string sql = @"EXEC AcceptFriend
                    @RequesteUserId = @RequestedUserIdParam,
                    @UserId = @UserIdParam";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@RequestedUserIdParam", this.User.FindFirst("UserId").Value, DbType.Int32);
                parameters.Add("@UserIdParam", userId, DbType.Int32);
                await _db.ExecuteSqlAsync(sql, parameters);
                return Ok();
            }
            else return BadRequest();
        }
        [HttpGet("getRequests")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetRequests()
        {
            string sql = @"EXEC GetRequests
                @MyUserId = @MyUserIdParam";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@MyUserIdParam", this.User.FindFirst("UserId").Value, DbType.Int32);
            var requests = await _db.LoadDataAsync<UserDTO>(sql, parameters);
            return Ok(requests);
        }
        [HttpGet("getFriends")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetFriends()
        {
            string sql = @"EXEC GetFriends
                @MyUserId = @MyUserIdParam";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@MyUserIdParam", this.User.FindFirst("UserId").Value, DbType.Int32);
            var friends = await _db.LoadDataAsync<UserDTO>(sql, parameters);
            return Ok(friends);
        }
        [HttpGet("getFeed")]
        public async Task<ActionResult<IEnumerable<PostDTO>>> GetFeed()
        {
            string sql = @"EXEC GetFeed
                @MyUserId = @MyUserIdParam";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@MyUserIdParam", this.User.FindFirst("UserId").Value, DbType.Int32);
            var feed = await _db.LoadDataAsync<UserDTO>(sql, parameters);
            return Ok(feed);
        }

    }
}