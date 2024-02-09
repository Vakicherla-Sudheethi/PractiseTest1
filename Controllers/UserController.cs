using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using log4net;
using PractiseTest1.DTO;
using PractiseTest1.Entities;
using PractiseTest1.Repo;

namespace PractiseTest1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UnitOfWork unitOfWork;
        private readonly IConfiguration configuration;
        private readonly ILog _logger;

        public UserController(UnitOfWork unitOfWork, IConfiguration configuration, ILog logger)
        {
            this.unitOfWork = unitOfWork;
            this.configuration = configuration;
            _logger = logger;
        }

        [HttpGet, Route("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var users = unitOfWork.UserImplObject.GetAll();
            return Ok(users);
        }

        [HttpPost, Route("Validate")]
        [AllowAnonymous]
        public IActionResult Validate(Validate login)
        {
            try
            {
                User user = unitOfWork.UserImplObject.ValidateUser(login.Email, login.Password);

                AuthResponse authResponse = new AuthResponse();
                if (user != null)
                {
                    authResponse.UserName = user.Username;
                    authResponse.Rolename = user.RoleName;
                    authResponse.Token = GetToken(user);
                    authResponse.UserID = user.UserID;
                }
                return StatusCode(200, authResponse);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        private string GetToken(User? user)
        {
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature
            );

            var subject = new ClaimsIdentity(new[]
{
    new Claim(ClaimTypes.Name, user.Username),
    new Claim(ClaimTypes.Role, user.RoleName),
    new Claim(ClaimTypes.Email, user.Email),
});


            var expires = DateTime.UtcNow.AddMinutes(10);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = subject,
                Expires = expires,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            return jwtToken;
        }

        [HttpPost, Route("AddNewUser")]
        public IActionResult AddNewUser(UserDTO User)
        {
            try
            {
                bool status = unitOfWork.UserImplObject.Add(User);

                if (status)
                {
                    unitOfWork.SaveAll();
                    return Ok(User);
                }
                else
                {
                    return BadRequest("Error in adding user");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return StatusCode(500, $"Error adding user: {ex.Message}. Inner Exception: {ex.InnerException?.Message}");
            }
        }

        [HttpGet, Route("GetUserById/{id}")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                var user = unitOfWork.UserImplObject.GetById(id);

                if (user == null)
                    return NotFound();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut, Route("UpdateUser/{id}")]
        public IActionResult UpdateUser(int id, UserDTO userUpdate)
        {
            userUpdate.UserID = id;

            try
            {
                bool status = unitOfWork.UserImplObject.Update(userUpdate);

                if (status)
                {
                    unitOfWork.SaveAll();
                    return Ok(userUpdate);
                }
                else
                {
                    return BadRequest($"User with ID {id} not found or update failed");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete, Route("DeleteUser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                bool status = unitOfWork.UserImplObject.Delete(id);

                if (status)
                {
                    unitOfWork.SaveAll();
                    return Ok($"User with ID {id} deleted");
                }
                else
                {
                    return BadRequest($"User with ID {id} not found or delete failed");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
