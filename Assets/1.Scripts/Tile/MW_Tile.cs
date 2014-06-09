using UnityEngine;
using System.Collections;

public class MW_Tile : MonoBehaviour {

    public enum TileIndex
    {
        eEmpty = 0,
        eDirt,
        eStone,
    }

    public Vector2 TilePos = new Vector2();

    public TileIndex index = TileIndex.eEmpty;
    tk2dSprite sprite;

	void Awake()
	{
		sprite = gameObject.GetComponent<tk2dSprite>();
	}
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetTile(TileIndex tileindex)
    {
        index = tileindex;

        switch(index)
        {
            case TileIndex.eDirt:
                sprite.SetSprite("dirt");
                break;
            case TileIndex.eStone:
                sprite.SetSprite("stone");
                break;
            case TileIndex.eEmpty:
                sprite.SetSprite("empty");
                break;
        }
    }
}
