using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Model;
using Newtonsoft.Json;

namespace Assets.Scripts.Authorization
{
    public class AuthApiService : IAuthApiService
    {
        private readonly string _hostName = "https://stereometry-auth.azurewebsites.net/api";

        public async Task<User> SignInUserAsync(User user)
        {
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage result = await httpClient.GetAsync($"{_hostName}/user/{user.Email}");
                if (result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<User>(await result.Content.ReadAsStringAsync());
                }

                StringContent data = GetUserAsStringContent(user);
                await httpClient.PostAsync($"{_hostName}/user", data);

                return user;
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent data = GetUserAsStringContent(user);
                await httpClient.PutAsync($"{_hostName}/user", data);
            }
        }

        private StringContent GetUserAsStringContent(User user)
        {
            string userAsJson = JsonConvert.SerializeObject(user);

            return new StringContent(userAsJson, Encoding.UTF8, "application/json"); ;
        }
    }
}