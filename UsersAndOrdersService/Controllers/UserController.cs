using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UsersAndOrdersService.Data.Context;
using UsersAndOrdersService.Data.Repositories;
using UsersAndOrdersService.Model;

namespace UsersAndOrdersService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserRepository _userRepository;

        public UserController(DataContext context)
        {
            _userRepository = new UserRepository(context);
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
    }
}
