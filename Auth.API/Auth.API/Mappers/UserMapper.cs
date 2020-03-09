using Auth.API.Models;
using DbUser = Auth.DLL.Models.User;

namespace Auth.API.Mappers
{
	internal static class UserMapper
	{
		internal static DbUser ToDbUser(this User user)
		{
			return new DbUser
			{
				UserId = user.UserId,
				Email = user.Email,
				Name = user.Name,
				LevelsPassed = user.LevelsPassed
			};
		}

		internal static User ToBllUser(this DbUser dbUser)
		{
			return new User
			{
				UserId = dbUser.UserId,
				Email = dbUser.Email,
				Name = dbUser.Name,
				LevelsPassed = dbUser.LevelsPassed
			};
		}
	}
}