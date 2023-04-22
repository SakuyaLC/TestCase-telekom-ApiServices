using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UsersAndOrdersService.Data.Context;
using UsersAndOrdersService.Data.DTO;
using UsersAndOrdersService.Data.Repositories;
using UsersAndOrdersService.Model;

namespace UsersAndOrdersService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserController(DataContext context, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = new UserRepository(context);
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login(UserLoginDTO userLoginDTO)
        {
            if (!await _userRepository.Authorize(userLoginDTO.Email, userLoginDTO.Password)){
                return NotFound();
            }

            return Ok(await CreateToken(userLoginDTO));
        }

        [HttpGet("/get-users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepository.GetUsers();
            return Ok(users);
        }

        [HttpGet("/get-user/{id}")]
        public async Task<IActionResult> GetSpecificUser(int id)
        {
            var user = await _userRepository.GetSpecificUser(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost("/create-user")]
        public async Task<IActionResult> CreateUser(User user)
        {
            await _userRepository.CreateUser(user);

            return CreatedAtAction(nameof(GetSpecificUser), new { id = user.Id }, user);
        }

        [HttpPatch("/update-user/{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            var userExists = await _userRepository.UserExists(id);

            if (!userExists)
            {
                return NotFound();
            }

            await _userRepository.UpdateUser(user);

            return Ok(user);
        }

        [HttpDelete("/delete-user/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userExists = await _userRepository.UserExists(id);

            if (!userExists)
            {
                return NotFound();
            }

            var user = await _userRepository.GetSpecificUser(id);

            await _userRepository.DeleteUser(user);

            return NoContent();
        }

        private async Task<string> CreateToken(UserLoginDTO userLoginDTO)
        {

            string role;

            User user = await _userRepository.GetUserByEmail(userLoginDTO.Email);

            if (user.Role == UserRole.User) role = "user";
            else role = "admin";

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("JwtSettings:Key").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            Console.WriteLine(jwt.ToString());

            return jwt;
        }


    }
}
