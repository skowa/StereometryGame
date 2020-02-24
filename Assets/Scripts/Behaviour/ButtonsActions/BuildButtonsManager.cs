using UnityEngine;
using UnityEngine.UI;

public class BuildButtonsManager : MonoBehaviour
{
	[SerializeField] private Button _dotButton;
	[SerializeField] private Button _lineButton;
	[SerializeField] private Button _parallelLineButton;
	[SerializeField] private Button _checkButton;
	[SerializeField] private Button _rotateButton;
	[SerializeField] private Button _createButton;

	private void Start()
	{
		if (CheckButtonsNotAssigned())
		{
			Debug.LogError("You must set buttons in the inspector");
			enabled = false;
			return;
		}

		_dotButton.onClick.AddListener(() =>
		{
			CreateButtonsHelper.Action = ActionType.Dot;
			EnableOkButton();
		});

		_lineButton.onClick.AddListener(() =>
		{
			CreateButtonsHelper.Action = ActionType.Line;
			EnableOkButton();
		});

		_parallelLineButton.onClick.AddListener(() =>
		{
			CreateButtonsHelper.Action = ActionType.ParallelLine;
			EnableOkButton();
		});

		_rotateButton.onClick.AddListener(() =>
		{
			var shape = GameObject.Find($"{Game.CurrentLevelData.Type.ToString()}Core");

			shape.transform.localRotation = Quaternion.identity;
			shape.transform.Rotate(-8, -12, 0.5f);
		});

		_checkButton.onClick.AddListener(() =>
		{
			CreateButtonsHelper.Action = ActionType.Check;
			EnableOkButton();
		});

		_createButton.onClick.AddListener(CreateButtonsHelper.CreateObject);
	}

	private void EnableOkButton()
	{
		var okButton = GameObject.Find("OK");
		okButton.GetComponent<Image>().enabled = true;
		okButton.GetComponent<Collider2D>().enabled = true;
	}

	private bool CheckButtonsNotAssigned()
	{
		return _dotButton == null || _lineButton == null || _parallelLineButton == null || _checkButton == null ||
		       _rotateButton == null;
	}
}
