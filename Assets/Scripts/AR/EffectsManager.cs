using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class EffectsManager : MonoBehaviour
{
	[SerializeField]
	private Button togglePlaneDetectionButton;

	[SerializeField]
	private ARPlaneManager aRPlaneManager;

	private void Start()
	{
		if (togglePlaneDetectionButton == null)
		{
			Debug.LogError("You must set buttons in the inspector");
			enabled = false;
			return;
		}

		togglePlaneDetectionButton.onClick.AddListener(TogglePlaneDetection);
	}

	private void TogglePlaneDetection()
	{
		aRPlaneManager.enabled = !aRPlaneManager.enabled;

		foreach (ARPlane plane in aRPlaneManager.trackables)
		{
			plane.gameObject.SetActive(aRPlaneManager.enabled);
		}
	}
}