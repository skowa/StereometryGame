using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  TouchBehaviour: MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.collider.transform != null)
                {
                    if (hit.collider.transform.gameObject.name == "New Game Object")
                    {
                        print("yes");
                    }
                }
            }
        }
    }
}
