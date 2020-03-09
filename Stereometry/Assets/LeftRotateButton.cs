using UnityEngine;
using UnityEngine.Analytics;

public class LeftRotateButton : MonoBehaviour
{
	private Quaternion _rotationLeft;
	private bool _isHolding;

	private void Start()
	{
		_rotationLeft = Quaternion.Euler(0, 0.1f, 0);
	}

	private void Update()
	{
		while (_isHolding)
		{
			Game.MainShape.transform.rotation *= _rotationLeft;
		}
	}

	public void OnMouseDown()
	{
		_isHolding = true;
	}

	public void OnMouseUp()
	{
		_isHolding = false;
	}
}
