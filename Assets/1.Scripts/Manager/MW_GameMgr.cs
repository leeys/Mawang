using UnityEngine;
using System.Collections;

public class MW_GameMgr : MonoBehaviour
{

    public bool isMovable = false;
    public tk2dUIToggleButton toggle;

    public static MW_GameMgr me;


	// Use this for initialization
	void Start () {
        me = GetComponent<MW_GameMgr>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetToggle()
    {
        isMovable = toggle.IsOn;
    }
}
