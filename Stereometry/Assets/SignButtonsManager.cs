using System.Net.Http;
using UnityEngine;
using UnityEngine.UI;

public class SignButtonsManager : MonoBehaviour
{
	[SerializeField] private Button _signInButton;
	[SerializeField] private Button _signUpButton;
	[SerializeField] private Button _settingsButton;
	[SerializeField] private GameObject _parametersWindow;

    private void Start()
    {
	    _signInButton.onClick.AddListener(SignInHandler);
    }

    private async void SignInHandler()
    {
	    var httpClient = new HttpClient();
	    var signing = await httpClient.GetAsync("http://stereometry-auth.azurewebsites.net/weatherforecast");
    }
}
