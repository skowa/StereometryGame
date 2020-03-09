using UnityEngine;

public class PlacementObject : MonoBehaviour
{
	[SerializeField]
	private bool IsSelected;

	public bool Selected
	{
		get
		{
			return this.IsSelected;
		}
		set
		{
			IsSelected = value;
		}
	}
}