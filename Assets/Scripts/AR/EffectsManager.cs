using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class EffectsManager : MonoBehaviour
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

	private void Start()
	{
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
		if (_isArMode)
		{
			_togglePlaneDetectionButton.onClick.AddListener(TogglePlaneDetection);
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
		SceneManager.LoadScene(_isArMode ? "Game" : "Levels");
	}

	private void RightAnswerHandler()
	{
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
		SceneManager.LoadScene("AR");
	}

	private void HideDescriptionHandler()
	{
		_description.SetActive(false);

		Resources.FindObjectsOfTypeAll<GameObject>().First(go => go.name == Game.CurrentLevelData.Type.ToString()).SetActive(true);
	}

	private bool CheckButtonsNotAssigned()
	{
		return _isArMode && _togglePlaneDetectionButton == null || !_isArMode && _arButton == null ||
		       _backButton == null || _cancelButton == null || _onceAgainButton == null ||
		       _rightAnswerButton == null || _toDoButton == null;
	}
}