using UnityEngine;
using System.Collections;

public class MW_WarriorCreate : MonoBehaviour {

    public struct TileData
    {
        public MW_Tile.TileIndex Tile_Type; // 타일의 현재상태
        public Vector3 Tile_Pos;            // 타일의 좌표
        public int Count;
    };

    public GameObject Warrior;
    public TileData[,] NowTile;         // 타일에서 필요한 부분만 얻은값.
    public MW_Tile[,] Original_Tile;    // 타일의 모든 값

    public static MW_WarriorCreate GetInstance;

	// Use this for initialization
	void Awake () {
        GetInstance = GetComponent<MW_WarriorCreate>();
	    //TileDataUpdate();
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.A))
        {
            TileDataUpdate();    // 타일에서 필요한 값만 가져와 NowTile 에 저장
            Instantiate(Warrior, new Vector3(15.0f, 0.0f, 0.0f), transform.rotation);
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            ///TileDataUpdate();    // 타일에서 필요한 값만 가져와 NowTile 에 저장
        }
	}

    void TileDataUpdate()
    {

        Original_Tile = (MW_Tile[,])MW_TileMgr.me.tiles.Clone();
        NowTile = new TileData[MW_TileMgr.me.y,MW_TileMgr.me.x];

        for(int i = 0;i < MW_TileMgr.me.y;i++)
        {
            for(int j = 0;j < MW_TileMgr.me.x;j++)
            {
                NowTile[i,j].Tile_Type = Original_Tile[i,j].index;
                NowTile[i,j].Tile_Pos = Original_Tile[i,j].transform.position;
                NowTile[i,j].Count = 0;
            }
        }

        print("TileData Reload");
    }
}
