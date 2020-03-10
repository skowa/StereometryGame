using System.Threading.Tasks;
using Assets.Scripts.Model;

namespace Assets.Scripts.Authorization
{
	public interface IAuthApiService
    {
        Task<User> SignInUserAsync(User user);
        Task UpdateUserAsync(User user);
    }
}