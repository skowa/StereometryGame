using System;
using System.Threading.Tasks;
using Assets.Scripts.Authorization;
using Assets.Scripts.Constants;
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
        [SerializeField] private Button _resetButton;
        [SerializeField] private Button _okButton;

        [SerializeField] private string _webClientId;

        [SerializeField] private GameObject _settingsScreen;
        [SerializeField] private Text _accountText;

        private GoogleAuthorization _googleAuthorization;
        private AuthApiService _authApiService;

        private void Start()
        {
            if (PlayerPrefs.HasKey(PlayerPrefsConstants.EmailPref) && !string.IsNullOrWhiteSpace(PlayerPrefs.GetString(PlayerPrefsConstants.EmailPref)))
            {
                SetUserLoggedInUI();
            }
            else
            {
                SetUserLoggedOutUI();
            }

            _googleAuthorization = new GoogleAuthorization(_webClientId);
            _authApiService = new AuthApiService();

            _signInButton.onClick.AddListener(OnSignIn);
            _signOutButton.onClick.AddListener(OnSignOut);
            _settingsButton.onClick.AddListener(OnSettings);

            _resetButton.onClick.AddListener(OnReset);
            _okButton.onClick.AddListener(OnOk);
        }

        private void OnSignIn()
        {
            _googleAuthorization.SignInAsync().ContinueWith(OnAuthenticationFinished);
        }

        private void OnSignOut()
        {
            _googleAuthorization.SignOut();
            SetUserLoggedOutUI();

            PlayerPrefs.DeleteKey(PlayerPrefsConstants.EmailPref);
            PlayerPrefs.DeleteKey(PlayerPrefsConstants.NamePref);
            PlayerPrefs.SetInt(PlayerPrefsConstants.LevelPref, 1);
        }

        private void OnSettings()
        {
            _settingsScreen.SetActive(true);
            _signInButton.gameObject.SetActive(false);
            _signOutButton.gameObject.SetActive(false);
            _settingsButton.gameObject.SetActive(false);
        }

        private void OnReset()
        {
            PlayerPrefs.SetInt(PlayerPrefsConstants.LevelPref, 1);
            _authApiService.UpdateUserAsync(new User
                { Email = PlayerPrefs.GetString(PlayerPrefsConstants.EmailPref), LevelsPassed = 1 });
        }

        private void OnOk()
        {
            _settingsScreen.SetActive(false);
            if (PlayerPrefs.HasKey(PlayerPrefsConstants.EmailPref))
            {
                _signOutButton.gameObject.SetActive(true);
            }
            else
            {
                _signInButton.gameObject.SetActive(true);
            }

            _settingsButton.gameObject.SetActive(true);
        }

        private async Task OnAuthenticationFinished(Task<User> task)
        {
            if (task.IsCompleted)
            {
                var user = await _authApiService.SignInUserAsync(task.Result);

                PlayerPrefs.SetString(PlayerPrefsConstants.EmailPref, user.Email);
                PlayerPrefs.SetString(PlayerPrefsConstants.NamePref, user.Name);
                if (PlayerPrefs.HasKey(PlayerPrefsConstants.LevelPref) && PlayerPrefs.GetInt(PlayerPrefsConstants.LevelPref) < user.LevelsPassed + 1)
                {
                    PlayerPrefs.SetInt(PlayerPrefsConstants.LevelPref, user.LevelsPassed + 1);
                }

                SetUserLoggedInUI();
            }
        }

        private void SetUserLoggedInUI()
        {
            _accountText.text =
                $"{PlayerPrefs.GetString(PlayerPrefsConstants.EmailPref)}{Environment.NewLine}{PlayerPrefs.GetString("name")}{Environment.NewLine}Levels: {PlayerPrefs.GetInt(PlayerPrefsConstants.LevelPref)}";

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