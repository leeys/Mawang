using UnityEngine;
using System.Collections;

public class MW_CamMgr : MonoBehaviour
{

    Vector2 startPos;
    Vector2 movePos;
    Vector2 distPos;

    public tk2dCamera cam;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (!MW_GameMgr.me.isMovable)
            return;
        if(Input.touchCount < 2)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            if (Input.GetMouseButton(0) && MW_GameMgr.me.isMovable)
            {
                movePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                distPos = startPos - movePos;
                print(distPos);
                startPos = movePos;


                cam.transform.Translate(distPos * Time.deltaTime * 30);
            }
        }
        else
        {

                Touch touch1 = Input.GetTouch(0);

                Touch touch2 = Input.GetTouch(1);

                Vector2 curDist = touch1.position - touch2.position;

                Vector2 prevDist = (touch1.position - touch1.deltaPosition) - (touch2.position - touch2.deltaPosition);

                float delta = curDist.magnitude - prevDist.magnitude;

                if(delta > 0)
                {
                    cam.ZoomFactor += 1 * Time.deltaTime;
                }
                else if(delta < 0)
                {
                    cam.ZoomFactor -= 1 * Time.deltaTime;
                }
                
             
        }
       

        
	
	}
}
