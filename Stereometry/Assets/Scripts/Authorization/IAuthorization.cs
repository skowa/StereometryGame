using System.Threading.Tasks;
using Assets.Scripts.Model;

namespace Assets.Scripts.Authorization
{
	public interface IAuthorization
	{
		Task<User> SignInAsync();
		void SignOut();
	}
}