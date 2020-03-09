using System;
using System.Threading.Tasks;
using Auth.DLL.Interfaces;
using Auth.DLL.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.DLL.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly DbContext _dbContext;

		public UserRepository(DbContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}

		public async Task<User> GetByEmailAsync(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
			{
				throw new ArgumentNullException(nameof(email));
			}

			return await _dbContext.Set<User>().FirstOrDefaultAsync(user => user.Email == email);
		}

		public async Task UpdateAsync(User user)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}

			var dbUser = await GetByEmailAsync(user.Email);
			if (dbUser == null)
			{
				throw new ArgumentException($"No user with email {user.Email} found.");
			}

			dbUser.LevelsPassed = user.LevelsPassed;

			await _dbContext.SaveChangesAsync();
		}

		public async Task AddAsync(User user)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}

			var dbUser = await GetByEmailAsync(user.Email);
			if (dbUser != null)
			{
				throw new ArgumentException($"User with email {user.Email} already exists.");
			}

			await _dbContext.Set<User>().AddAsync(user);
			await _dbContext.SaveChangesAsync();
		}
	}
}