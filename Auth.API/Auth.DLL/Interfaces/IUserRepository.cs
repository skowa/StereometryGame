using System.Threading.Tasks;
using Auth.DLL.Models;

namespace Auth.DLL.Interfaces
{
	public interface IUserRepository
	{
		Task<User> GetByEmailAsync(string email);

		Task UpdateAsync(User user);

		Task AddAsync(User user);
	}
}