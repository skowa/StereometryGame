using System;
using Assets.Scripts.Behaviour.ButtonsActions;
using Assets.Scripts.Constants;
using UnityEngine;
using UnityEngine.UI;

public class BuildButtonsManager : MonoBehaviour
{
    private const string Filled = "Filled";

	[SerializeField] private Button _dotButton;
	[SerializeField] private Button _lineButton;
	[SerializeField] private Button _parallelLineButton;
	[SerializeField] private Button _checkButton;
	[SerializeField] private Button _rotateButton;
	[SerializeField] private Button _createButton;
	[SerializeField] private Button _crossSectionButton;

    public void ResetButtonByActionType(ActionType actionType)
    {
        Button buttonToReset;
        switch (actionType)
        {
            case ActionType.Dot:
                buttonToReset = _dotButton;

                break;

            case ActionType.Line:
                buttonToReset = _lineButton;

                break;

            case ActionType.ParallelLine:
                buttonToReset = _parallelLineButton;

                break;

            case ActionType.CrossSection:
                buttonToReset = _crossSectionButton;

                break;

            default:
                buttonToReset = null;

                break;
        }

        if (buttonToReset != null)
        {
            string currentImageName = buttonToReset.gameObject.GetComponent<Image>().sprite.name;

            buttonToReset.gameObject.GetComponent<Image>().sprite = GetNotFilledSprite(currentImageName);
        }
    }

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
            HandleButtonClick(_dotButton, ActionType.Dot);
		});

		_lineButton.onClick.AddListener(() =>
		{
            HandleButtonClick(_lineButton, ActionType.Line);
        });

		_parallelLineButton.onClick.AddListener(() =>
		{
            HandleButtonClick(_parallelLineButton, ActionType.ParallelLine);
        });

		_rotateButton.onClick.AddListener(() =>
		{
            CreateNewConstructionHelper.ResetAction();

            var shape = GameObject.Find($"{Game.CurrentLevelData.Type.ToString()}Core");

			shape.transform.localRotation = Quaternion.identity;
			shape.transform.Rotate(-8, -12, 0.5f);
        });

		_crossSectionButton.onClick.AddListener(() =>
        {
            HandleButtonClick(_crossSectionButton, ActionType.CrossSection);

            GameOkButton.Enable(CreateNewConstructionHelper.Action == ActionType.CrossSection);
        });

		_checkButton.onClick.AddListener(() =>
		{
            CreateNewConstructionHelper.ResetAction();

            CreateNewConstructionHelper.Action = ActionType.Check;
            CreateNewConstructionHelper.CreateObject();
        });

		_createButton.onClick.AddListener(CreateNewConstructionHelper.CreateObject);
	}

    private void HandleButtonClick(Button button, ActionType actionType)
    {
        string currentImageName = button.gameObject.GetComponent<Image>().sprite.name;
        if (CreateNewConstructionHelper.Action == actionType)
        {
            CreateNewConstructionHelper.ResetAction();
            button.gameObject.GetComponent<Image>().sprite = GetNotFilledSprite(currentImageName);
        }
        else
        {
            ResetButtonByActionType(CreateNewConstructionHelper.Action);
            CreateNewConstructionHelper.Action = actionType;
            button.gameObject.GetComponent<Image>().sprite = GetFilledSprite(currentImageName);
        }
	}

    private Sprite GetNotFilledSprite(string currentImageName)
    {
        return Resources.Load<Sprite>($"{PathConstants.PathToSprites}{currentImageName.Remove(currentImageName.LastIndexOf(Filled, StringComparison.Ordinal))}");
    }

	private Sprite GetFilledSprite(string currentImageName)
    {
        return Resources.Load<Sprite>($"{PathConstants.PathToSprites}{currentImageName}{Filled}");
    }

    private bool CheckButtonsNotAssigned()
	{
		return _dotButton == null || _lineButton == null || _parallelLineButton == null || _checkButton == null ||
		       _rotateButton == null;
	}
}
