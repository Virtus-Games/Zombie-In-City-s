using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour
{

    private Vector3 firstPos;
    private Vector3 lastPos;
    private Vector3 direction;
    private Vector3 forwardDir;



    // PROPERTIES
    public Vector3 Direction
    {
        get
        {
            return direction;
        }
    }
    public Vector3 ForwardDirection
    {
        get
        {
            return forwardDir;
        }
    }


    private void Update()
    {
      
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                firstPos = touch.position;

            }
            if (touch.phase == TouchPhase.Moved)
            {
               
                lastPos = touch.position;
                direction = (new Vector3(lastPos.x,0, lastPos.y) - new Vector3(firstPos.x,0, firstPos.y)).normalized;
                Vector3 dir = lastPos - firstPos;
                forwardDir = Camera.main.ScreenToWorldPoint(dir);
               

            }

            if (touch.phase == TouchPhase.Ended)
            {

                lastPos = Vector3.zero;
                firstPos = Vector3.zero;
                direction = Vector3.zero;
            }
        }
    }
}
