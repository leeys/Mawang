using UnityEngine;
using System.Collections;

public class MW_2DSprite : MonoBehaviour {

	// Use this for initialization
    // 이 스크립트는 2D 스프라이트를 좀더 쉽게 사용 할 수 있도록 도와주는 스크립트 입니다.


    public Vector2 pos;
    public float rot;
    public Vector2 size;
    public int zOrder;

    

	void Start () {
        Commit();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetPos(float x, float y)
    {
        pos.x = x;
        pos.y = y;
        Commit();
    }

    public void SetRot(float r)
    {
        rot = r;
        Commit();
    }

    public void SetSize(float x, float y)
    {
        size.x = x;
        size.y = y;
        Commit();
    }

    public void SetZothder(int z)
    {
        zOrder = z;
        Commit();
    }

    void Commit()
    {
        gameObject.transform.position = new Vector3(pos.x / 72.0f, pos.y / 72.0f, (float)zOrder / 10.0f);
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rot));
        gameObject.transform.localScale = new Vector3(size.x, size.y, 1);
    }

}
