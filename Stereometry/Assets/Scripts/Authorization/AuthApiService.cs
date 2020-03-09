using System.Collections;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Model;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Authorization
{
	public class AuthApiService
	{
		private readonly string _hostName = "http://stereometry-auth.azurewebsites.net/";

		public AuthApiService()
		{
			
		}

		//public IEnumerator SignInUser(User user)
		//{
		//	UnityWebRequest dbUserRequest = UnityWebRequest.Get($"{_hostName}user/{user.Email}");
		//	yield return dbUserRequest.SendWebRequest();

		//	if (!dbUserRequest.isNetworkError && !dbUserRequest.isHttpError)
		//	{
		//		User userResponse = JsonUtility.FromJson<User>(dbUserRequest.downloadHandler.text);
		//		if (userResponse != null)
		//		{

		//		}
		//	}
		//}

		public async Task<User> SignInUser(User user)
		{
			using (var httpClient = new HttpClient())
			{
				HttpResponseMessage result = await httpClient.GetAsync($"{_hostName}user/{user.Email}");
				if (result.IsSuccessStatusCode)
				{
					return JsonConvert.DeserializeObject<User>(await result.Content.ReadAsStringAsync());
				}

				string userAsJson = JsonConvert.SerializeObject(user);
				var data = new StringContent(userAsJson, Encoding.UTF8, "application/json");
				result = await httpClient.PostAsync($"{_hostName}user", data);

				return user;
			}
		}
	}
}