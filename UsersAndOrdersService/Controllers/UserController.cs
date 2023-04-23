using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
using UsersAndOrdersService.Helper;
using UsersAndOrdersService.Model;

namespace UsersAndOrdersService.Controllers
{
    [ApiController]
    [Route("api/users")]
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
            var users = _mapper.Map<List<UserDTO>>(await _userRepository.GetUsers());
            return Ok(users);
        }

        [HttpGet("/get-user/{id}")]
        public async Task<IActionResult> GetSpecificUser(int id)
        {
            var user = _mapper.Map<UserDTO>(await _userRepository.GetSpecificUser(id));

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost("/create-user")]
        public async Task<IActionResult> CreateUser(UserCreationDTO userCreationDTO)
        {
            User user = _mapper.Map<User>(userCreationDTO);

            await _userRepository.CreateUser(user);

            return CreatedAtAction(nameof(GetSpecificUser), new { id = user.Id }, _mapper.Map<UserDTO>(user));
        }

        //[Authorize(Roles = "user, admin")]
        [HttpPatch("/update-user/{id}")]
        public async Task<IActionResult> UpdateUser(UserDTO user)
        {

            if (!await _userRepository.UserExists(user.Id))
            {
                return NotFound();
            }

            await _userRepository.UpdateUser(_mapper.Map<User>(user));

            return Ok(_mapper.Map<UserDTO>(await _userRepository.GetSpecificUser(user.Id)));
        }

        //[Authorize(Roles = "user, admin")]
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

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(5)), // время действия 5 минут
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }


    }
}
