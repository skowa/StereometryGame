using System;
using System.Threading.Tasks;
using Auth.API.Mappers;
using Auth.API.Models;
using Auth.DLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : ControllerBase
	{
		private readonly IUserRepository _repository;

		public UserController(IUserRepository repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		[HttpGet]
		public async Task<IActionResult> Get(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
			{
				return BadRequest("The email is empty.");
			}

			return Ok(await _repository.GetByEmailAsync(email));
		}

		[HttpPost]
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
		public async Task<IActionResult> Update([FromBody] User user)
		{
			if (user == null)
			{
				return BadRequest("The user is null.");
			}

			try
			{
				await _repository.AddAsync(user.ToDbUser());
			}
			catch (ArgumentException)
			{
				return NotFound($"No user with email {user.Email} found.");
			}

			return Ok();
		}
	}
}
