using System;
using System.Threading.Tasks;
using Assets.Scripts.Authorization;
using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Behaviour.ButtonsActions
{
	public class ProfileButtonsManager : MonoBehaviour
	{
		[SerializeField] private Button _signInButton;
		[SerializeField] private Button _signOutButton;
		[SerializeField] private Button _settingsButton;

		[SerializeField] private string _webClientId;

		[SerializeField] private GameObject _settingsScreen;
		[SerializeField] private Text _accountText;

		private GoogleAuthorization _googleAuthorization;
		private AuthApiService _authApiService;

		private void Start()
		{
			if (PlayerPrefs.HasKey("email") && !string.IsNullOrWhiteSpace(PlayerPrefs.GetString("email")))
			{
				SetUserLoggedInUI();
			}
			else
			{
				SetUserLoggedOutUI();
			}

			_googleAuthorization = new GoogleAuthorization(_webClientId);
			_authApiService = new AuthApiService();
			_authApiService.SignInUser(new User {Email = "afasfaf", Name = "asasda"});

			_signInButton.onClick.AddListener(OnSignIn);
			_signOutButton.onClick.AddListener(OnSignOut);
			_settingsButton.onClick.AddListener(OnSettings);
		}

		private void OnSignIn()
		{
			_googleAuthorization.SignInAsync().ContinueWith(OnAuthenticationFinished);
		}

		private void OnSignOut()
		{
			_googleAuthorization.SignOut();
			SetUserLoggedOutUI();

			PlayerPrefs.DeleteKey("email");
			PlayerPrefs.DeleteKey("name");
		}

		private void OnSettings()
		{
			_settingsScreen.SetActive(true);
			_signInButton.gameObject.SetActive(false);
			_signOutButton.gameObject.SetActive(false);
		}

		private async Task OnAuthenticationFinished(Task<User> task)
		{
			if (task.IsCompleted)
			{
				var user = await _authApiService.SignInUser(task.Result);

				PlayerPrefs.SetString("email", user.Email);
				PlayerPrefs.SetString("name", user.Name);
				if (PlayerPrefs.HasKey("level") && PlayerPrefs.GetInt("level") < user.LevelsPassed)
				{
					PlayerPrefs.SetInt("level", user.LevelsPassed);
				}

				SetUserLoggedInUI();
			}
		}

		private void SetUserLoggedInUI()
		{
			_accountText.text = $"{PlayerPrefs.GetString("email")}{Environment.NewLine}{PlayerPrefs.GetString("name")}{Environment.NewLine}Levels: {PlayerPrefs.GetInt("level")}";

			_signOutButton.gameObject.SetActive(true);
			_signInButton.gameObject.SetActive(false);
		}

		private void SetUserLoggedOutUI()
		{
			_accountText.text = string.Empty;

			_signOutButton.gameObject.SetActive(false);
			_signInButton.gameObject.SetActive(true);
		}
	}
}