using System;
using System.Threading.Tasks;
using Auth.API.Mappers;
using Auth.API.Models;
using Auth.DLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repository;

        public UserController(IUserRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("The email is empty.");
            }

            var dbUser = await _repository.GetByEmailAsync(email);
            if (dbUser == null)
            {
                return NotFound();
            }

            return Ok(dbUser.ToBllUser());
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("The user is null.");
            }

            await _repository.AddAsync(user.ToDbUser());

            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("The user is null.");
            }

            try
            {
                await _repository.UpdateAsync(user.ToDbUser());
            }
            catch (ArgumentException)
            {
                return NotFound($"No user with email {user.Email} found.");
            }

            return Ok();
        }
    }
}
