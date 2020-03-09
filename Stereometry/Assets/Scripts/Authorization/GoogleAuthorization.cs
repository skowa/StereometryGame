using System.Threading.Tasks;
using Assets.Scripts.Model;
using Google;

namespace Assets.Scripts.Authorization
{
	public class GoogleAuthorization : IAuthorization
	{
		private readonly GoogleSignInConfiguration _configuration;

		public GoogleAuthorization(string webClientId)
		{
			_configuration = new GoogleSignInConfiguration
			{
				WebClientId = webClientId,
				RequestIdToken = true,
				RequestEmail = true,
				UseGameSignIn = false
			};
		}

		public async Task<User> SignInAsync()
		{
			GoogleSignIn.Configuration = _configuration;
			GoogleSignInUser googleUser = await GoogleSignIn.DefaultInstance.SignIn();

			return new User
			{
				Email = googleUser.Email,
				Name = googleUser.DisplayName
			};
		}

		public void SignOut()
		{
			GoogleSignIn.DefaultInstance.SignOut();
		}
	}
}