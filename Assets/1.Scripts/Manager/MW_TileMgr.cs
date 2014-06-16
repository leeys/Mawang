using UnityEngine;
using System.Collections;


public class MW_TileMgr : MonoBehaviour
{
    public MW_Tile tile;
    public Camera cam;
    public int x;
    public int y;

    public MW_Tile[,] tiles;

    public static MW_TileMgr me;
    // Use this for initialization

    void Start()
    {
        me = GetComponent<MW_TileMgr>();

        MakeWorld();

        SetCamera();
    }

    private void SetCamera()
    {
        Vector2 _center;
        _center.x = tiles[0, x - 1].transform.position.x - tiles[0, 0].transform.position.x;

        cam.transform.Translate(new Vector2(_center.x / 2, 0));
    }
    private void MakeWorld()
    {
        //동적할당
        tiles = new MW_Tile[y, x];

        //가운대 땅 만들기
        for (int i = 1; i < y - 1; i++)
        {
            for (int j = 1; j < x - 1; j++)
            {
                Vector2 _pos = ccp(j * 72, -(i * 72));
                tiles[i, j] = (MW_Tile)Instantiate(tile, _pos, Quaternion.identity);
                tiles[i, j].SetTile(MW_Tile.TileIndex.eDirt);
                tiles[i, j].transform.parent = this.gameObject.transform;
                tiles[i, j].TilePos.x = j;
                tiles[i, j].TilePos.y = i;
            }
        }

        //겉에 테두리 만들기
        for (int i = 0; i < x; i++)
        {
            tiles[0, i] = (MW_Tile)Instantiate(tile, ccp(i * 72, 0), Quaternion.identity);
            tiles[y - 1, i] = (MW_Tile)Instantiate(tile, ccp(i * 72, -(y - 1) * 72), Quaternion.identity);
           
            tiles[0, i].SetTile(MW_Tile.TileIndex.eStone);
            tiles[y - 1, i].SetTile(MW_Tile.TileIndex.eStone);
            
            tiles[0, i].transform.parent = this.gameObject.transform;
            tiles[y - 1, i].transform.parent = this.gameObject.transform;

        }
        for (int i = 1; i < y - 1; i++)
        {
            tiles[i, 0] = (MW_Tile)Instantiate(tile, ccp(0, -i * 72), Quaternion.identity);
            tiles[i, x - 1] = (MW_Tile)Instantiate(tile, ccp((x - 1) * 72, -i * 72), Quaternion.identity);
            tiles[i, 0].SetTile(MW_Tile.TileIndex.eStone);
            tiles[i, x - 1].SetTile(MW_Tile.TileIndex.eStone);
            tiles[i, 0].transform.parent = this.gameObject.transform;
            tiles[i, x - 1].transform.parent = this.gameObject.transform;
        }

        tiles[0, x / 2].SetTile(MW_Tile.TileIndex.eEmpty);

    }
    // Update is called once per frame
    void Update()
    {
        //타일 충돌처리
        if (Input.GetMouseButton(0) && !MW_GameMgr.me.isMovable)
        {
            //마우스로 캐스팅
            CastingTile();
        }
    }

    private void CastingTile()
    {


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

        if (hit.transform != null)
        {
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {

                    if(CheckTile(i, j, ref hit))
                    {
                        TouchTile(i, j);
                        
                    }

                }
            }
        }

    }

    private void TouchTile(int i, int j)
    {
        switch(tiles[i, j].index)
        {
            case MW_Tile.TileIndex.eDirt:
                tiles[i, j].SetTile(MW_Tile.TileIndex.eEmpty);
                break;
        }
       
    }

    private bool CheckTile(int i, int j, ref RaycastHit2D hit)
    {
        if (tiles[i, j] != null && tiles[i, j].gameObject.Equals(hit.transform.gameObject))
        {
            //제일 위의 블럭
            if (i == 0)
                return false;
            //제일 밑의 블럭
            if (i == y - 1)
                return false;
            //제일 왼쪽
            if (j == 0)
                return false;
            //제일 오른쪽
            if (j == x - 1)
                return false;


            if (tiles[i + 1, j].index == MW_Tile.TileIndex.eEmpty |
			    tiles[i - 1, j].index == MW_Tile.TileIndex.eEmpty |
			    tiles[i, j + 1].index == MW_Tile.TileIndex.eEmpty |
			    tiles[i, j - 1].index == MW_Tile.TileIndex.eEmpty)
            {
                return true;
            }

            return false;
        }
        return false;


    }
    Vector2 ccp(int x, int y)
    {
        Vector2 v;
        v.x = (float)x / 72.0f;
        v.y = (float)y / 72.0f;

        return v;
    }

    Vector2 ccp(float x, float y)
    {
        Vector2 v;
        v.x = x / 72.0f;
        v.y = y / 72.0f;

        return v;
    }

    public MW_Tile.TileIndex ReturnTile(int x, int y)
    {
        //y,x거꿀로
        return tiles[y, x].index;
    }
}
    