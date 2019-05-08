using UnityEngine;

public class MenuCubeBehaviour : MonoBehaviour
{
	void Update ()
	{
	    transform.Rotate(Vector3.up * Time.deltaTime * 30);
	}
}
