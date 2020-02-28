using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

namespace Assets.Scripts.Behaviour.ButtonsActions
{
	public class GameModeButtonsManager : MonoBehaviour
	{
		[SerializeField] private Button _togglePlaneDetectionButton;
		[SerializeField] private Button _backButton;
		[SerializeField] private Button _cancelButton;
		[SerializeField] private Button _onceAgainButton;
		[SerializeField] private Button _toDoButton;
		[SerializeField] private Button _arButton;
		[SerializeField] private Button _rightAnswerButton;
		[SerializeField] private ARPlaneManager _aRPlaneManager;
		[SerializeField] private bool _isArMode;
		[SerializeField] private GameObject _description;
		[SerializeField] private Button _hideDescription;
		[SerializeField] private Button _fillShape;
		[SerializeField] private Button _rotateLeft;
		[SerializeField] private Button _rotateRight;
		private bool _isFilled;
		private readonly Vector3 _rotateSpeed = Vector3.zero;
		private Quaternion _rotationLeft;
		private Quaternion _rotationRight;

		private void Start()
		{
			_rotationLeft = Quaternion.Euler(0, 10f, 0);
			_rotationRight = Quaternion.Euler(0, -10f, 0);
			if (CheckButtonsNotAssigned())
			{
				Debug.LogError("You must set buttons in the inspector");
				enabled = false;
				return;
			}

			_cancelButton.onClick.AddListener(CancelButtonHandler);
			_backButton.onClick.AddListener(BackButtonHandler);
			_onceAgainButton.onClick.AddListener(OnceAgainButtonHandler);
			_rightAnswerButton.onClick.AddListener(RightAnswerHandler);
			_toDoButton.onClick.AddListener(ToDoButtonHandler);
			_hideDescription.onClick.AddListener(HideDescriptionHandler);
			_fillShape.onClick.AddListener(() =>
			{
				Game.MainShape.transform.GetChild(0).GetComponent<Renderer>().material = _isFilled
					? Resources.Load<Material>(Game.PathToTransparentMaterial)
					: Resources.Load<Material>(Game.PathToFilledMaterial);
				_isFilled = !_isFilled;
			});

			if (_isArMode)
			{
				_togglePlaneDetectionButton.onClick.AddListener(TogglePlaneDetection);
				_rotateLeft.onClick.AddListener(RotateLeftHandler);
				_rotateRight.onClick.AddListener(RotateRightHandler);
			}
			else
			{
				_arButton.onClick.AddListener(ArButtonHandler);
			}
		}

		private void TogglePlaneDetection()
		{
			_aRPlaneManager.enabled = !_aRPlaneManager.enabled;

			foreach (ARPlane plane in _aRPlaneManager.trackables)
			{
				plane.gameObject.SetActive(_aRPlaneManager.enabled);
			}
		}

		private void BackButtonHandler()
		{
			if (_isArMode)
			{
				StartCoroutine(LoadGameScene());
			}
			else
			{
				Game.MainShape = null;
				Game.IsLevelFilled = false;
				SceneManager.LoadScene("Levels");
			}
		}

		private void RightAnswerHandler()
		{
			Game.MainShape = null;
			Game.IsLevelFilled = false;
			SceneManager.LoadScene("Levels");
		}

		private void ToDoButtonHandler()
		{
			var shape = GameObject.Find(Game.CurrentLevelData.Type.ToString());
			shape.SetActive(false);

			_description.SetActive(true);
			GameObject text = _description.transform.GetChild(1).gameObject;
			text.GetComponent<Text>().text = Game.CurrentLevelData.Description;
		}

		private void OnceAgainButtonHandler()
		{
			CreateButtonsHelper.Action = ActionType.None;
			foreach (GameObject t in Game.Actions)
			{
				Destroy(t);
			}

			Game.Actions.Clear();
		}

		private void CancelButtonHandler()
		{
			if (Game.Actions.Count >= 1)
			{
				GameObject toBeRemoved = Game.Actions[Game.Actions.Count - 1];
				Game.Actions.Remove(toBeRemoved);
				Destroy(toBeRemoved);
			}
		}

		private void ArButtonHandler()
		{
			StartCoroutine(LoadARScene());
		}

		private IEnumerator LoadARScene()
		{
			yield return LoadScene("AR");

			Game.MainShape.SetActive(false);
		}

		private IEnumerator LoadGameScene() => LoadScene("Game");

		private IEnumerator LoadScene(string sceneName)
		{
			Scene currentScene = SceneManager.GetActiveScene();
			AsyncOperation loadScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
			loadScene.allowSceneActivation = false;
			while (loadScene.progress < 0.9f)
			{
				yield return null;
			}

			SceneManager.MoveGameObjectToScene(Game.MainShape, SceneManager.GetSceneByName(sceneName));
			loadScene.allowSceneActivation = true;
			while (!loadScene.isDone)
			{
				yield return null;
			}

			SceneManager.UnloadSceneAsync(currentScene);
		}

		private void HideDescriptionHandler()
		{
			_description.SetActive(false);

			Resources.FindObjectsOfTypeAll<GameObject>().First(go => go.name == Game.CurrentLevelData.Type.ToString()).SetActive(true);
		}

		private void RotateLeftHandler()
		{
			Game.MainShape.transform.rotation *= _rotationLeft;
		}

		private void RotateRightHandler()
		{
			Game.MainShape.transform.rotation *= _rotationRight;
			//Game.MainShape.transform.GetChild(0).Rotate(-_rotateSpeed * Time.deltaTime, Space.World);
		}

		private bool CheckButtonsNotAssigned()
		{
			return _isArMode && _togglePlaneDetectionButton == null || !_isArMode && _arButton == null ||
			       _backButton == null || _cancelButton == null || _onceAgainButton == null ||
			       _rightAnswerButton == null || _toDoButton == null;
		}
	}
}