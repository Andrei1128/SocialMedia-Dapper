
using Dapper;
using Microsoft.AspNetCore.Mvc;
using SocialMedia_Dapper.Data;
using SocialMedia_Dapper.Models.DTOs;
using SocialMedia_Dapper.Utilities;
using System.Data;

namespace SocialMedia_Dapper.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext db;
        private readonly AuthUtil authUtil;
        public AuthController(IConfiguration config)
        {
            authUtil = new AuthUtil(config);
            db = new DataContext(config);
        }
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterRequestDTO userForRegistration)
        {
            if (userForRegistration.Password == userForRegistration.ConfirmPassword)
            {
                string sqlCheckUser = @"SELECT Email FROM Users WHERE Email = @EmailParam";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmailParam", userForRegistration.Email, DbType.String);
                IEnumerable<string> result = await db.LoadDataAsync<string>(sqlCheckUser, parameters);
                if (result.Count() == 0)
                {
                    userForRegistration.Password = authUtil.EncodePassword(userForRegistration.Password);
                    string sql = @"EXEC CreateUser
                        @Email = @EmailParam,
                        @Password = @PasswordParam,
                        @Name = @NameParam";
                    parameters.Add("@PasswordParam", userForRegistration.Password, DbType.String);
                    parameters.Add("@NameParam", userForRegistration.Name, DbType.String);
                    if (await db.ExecuteSqlAsync(sql, parameters))
                    {
                        return Ok();
                    }
                    else return BadRequest("Failed to create the account!");
                }
                else return BadRequest("Email already used!");
            }
            else return BadRequest("Passwords does not match!");
        }
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(LoginRequestDTO userForLogin)
        {
            string loginSql = @"SELECT Id,Name,ImageURL FROM Users WHERE Email = @EmailParam AND Password = @PasswordParam";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@EmailParam", userForLogin.Email, DbType.String);
            parameters.Add("@PasswordParam", authUtil.EncodePassword(userForLogin.Password), DbType.String);
            UserDTO user = await db.LoadDataSingleAsync<UserDTO>(loginSql, parameters);
            if (user != null)
            {
                AuthResponseDTO loginResponseDTO = new AuthResponseDTO()
                {
                    Token = authUtil.CreateToken(user.Id),
                    User = user
                };
                return Ok(loginResponseDTO);
            }
            else return BadRequest("Invalid Email or Password!");
        }
    }
}