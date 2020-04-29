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
	[SerializeField] private Button _crossSectionButton;

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
			CreateNewConstructionHelper.Action = ActionType.Dot;
		});

		_lineButton.onClick.AddListener(() =>
		{
			CreateNewConstructionHelper.Action = ActionType.Line;
		});

		_parallelLineButton.onClick.AddListener(() =>
		{
			CreateNewConstructionHelper.Action = ActionType.ParallelLine;
		});

		_rotateButton.onClick.AddListener(() =>
		{
			var shape = GameObject.Find($"{Game.CurrentLevelData.Type.ToString()}Core");

			shape.transform.localRotation = Quaternion.identity;
			shape.transform.Rotate(-8, -12, 0.5f);
		});

		_crossSectionButton.onClick.AddListener(() =>
		{
			CreateNewConstructionHelper.Action = ActionType.CrossSection;
			EnableOkButton();
		});

		_checkButton.onClick.AddListener(() =>
		{
			CreateNewConstructionHelper.Action = ActionType.Check;
            CreateNewConstructionHelper.CreateObject();
            ;
        });

		_createButton.onClick.AddListener(CreateNewConstructionHelper.CreateObject);
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
