using UnityEngine;
using System.Collections;

public class MW_WarriorMoveAI : MonoBehaviour {

    public enum MoveWayState
    {
        e_mNOTSTATE = -1,
        e_mLeft = 0,
        e_mRight,
        e_mUp,
        e_mDown,
    };

    // 타일에서 필요한 데이터 값을 로드한다.
    public MW_WarriorCreate.TileData[,] LoadData;

    public MoveWayState MoveState = MoveWayState.e_mDown;
    public float Speed = 10.0f;

    public MoveWayState[] MoveStateBuffer = new MoveWayState[4];

    public int Overlab_X = 0;
    public int Overlab_Y = 0;

    private int FindStateCount = 0;

    void BufferReSet() { for(int i = 0;i < 4;i++) MoveStateBuffer[i] = MoveWayState.e_mNOTSTATE; }

	// Use this for initialization
	void Awake () {
        LoadData = (MW_WarriorCreate.TileData[,])MW_WarriorCreate.GetInstance.NowTile.Clone();
	}
	
	// Update is called once per frame
	void Update () {
	
        if(transform.position.y > 0.0f)
            Destroy(this.gameObject);

        MoveUpdate();       // 움직임 업데이트
        CrossWayUpdate();   // 길찾기 업데이트
	}

    bool TileCrashPoint()
    {
        float tempY = (int)transform.position.y;
        float tempX = (int)transform.position.x;

		if(tempX + 0.1f >= transform.position.x && tempX - 0.1f <= transform.position.x &&
            tempY + 0.1f >= transform.position.y && tempY - 0.1f <= transform.position.y)
            return true;

        return false;
    }

    void CrossWayUpdate()
    {
        if(!TileCrashPoint()) return;                       // 4개의 버퍼를 둠
                                                            // 현재 교차로에서 길이 있는 방향 선택
        int x = 0;                                          // 선택된 방향에서 한번 갔던길인지 확인
        int y = 0;                                          // 둘 다 만족하는 조건을 버퍼에 넣음
        x = (int)transform.position.x;                      // 버퍼를 랜덤으로 돌려서 갈 방향 지정
        y = (int)transform.position.y;

        if(Overlab_X == x && Overlab_Y == y)    // 현재 있는 타일일 경우 교차로 체크 하지않음
            return;
        
        Overlab_X = x; Overlab_Y = y;

        LoadData[-y,x].Count ++;    // 현재 있는 길 위치를 한번 지나간 경험이 있는 위치로 카운터 올려줌

        BufferReSet();  // 버퍼 초기화

        CountSort(x, y);    // 만족하는 길을 찾아 버퍼에 저장

        // 계산 끝난 후 그 계산값에 따라 방향을 랜덤지정
        FindStateCount = Random.Range(0,4);

        FinedState();

        if(MoveStateBuffer[FindStateCount] != MoveState)     // 교차로 넘어가기 전 좌표 초기화
        {
            transform.position = LoadData[-y,x].Tile_Pos;
        }

        MoveState = MoveStateBuffer[FindStateCount];

        if(MoveStateBuffer[1] == MoveWayState.e_mNOTSTATE)
            LoadData[-y,x].Count++;
    }

    void FinedState()
    {
        if(MoveStateBuffer[FindStateCount] == MoveWayState.e_mNOTSTATE)
        {
            FindStateCount++;
            if(FindStateCount > 3) FindStateCount = 0;
            FinedState();
        }
    }

    void CountSort(int x, int y)
    {
        int BufferCount = 0;

        int CountTemp = 0;

        if(LoadData[-y,x-1].Tile_Type == MW_Tile.TileIndex.eEmpty)
            CountTemp = LoadData[-y,x-1].Count;
        else if(LoadData[-y,x+1].Tile_Type == MW_Tile.TileIndex.eEmpty)
            CountTemp = LoadData[-y,x+1].Count;
        else if(LoadData[-y-1,x].Tile_Type == MW_Tile.TileIndex.eEmpty)
            CountTemp = LoadData[-y-1,x].Count;
        else if(LoadData[-y+1,x].Tile_Type == MW_Tile.TileIndex.eEmpty)
            CountTemp = LoadData[-y+1,x].Count;

        if(x != 0 || x < MW_TileMgr.me.x)
        {
			// 왼쪽 비교
            if(LoadData[-y,x-1].Tile_Type == MW_Tile.TileIndex.eEmpty)
            {
                if(LoadData[-y,x-1].Count < CountTemp)
                {
                    BufferCount = 0; BufferReSet();
                    MoveStateBuffer[BufferCount] = MoveWayState.e_mLeft;
                }
                else if(LoadData[-y,x-1].Count == CountTemp)
                {
                    MoveStateBuffer[BufferCount] = MoveWayState.e_mLeft;
					BufferCount++;
                }
            }

            // 오른쪽 비교
            if(LoadData[-y,x+1].Tile_Type == MW_Tile.TileIndex.eEmpty)
            {
                if(LoadData[-y,x+1].Count < CountTemp)
                {
                    BufferCount = 0; BufferReSet();
                    MoveStateBuffer[BufferCount] = MoveWayState.e_mRight;
                }
                else if(LoadData[-y,x+1].Count == CountTemp)
                {
                    MoveStateBuffer[BufferCount] = MoveWayState.e_mRight;
					BufferCount++;
                }
            }
        }

        if(y != 0 || y > MW_TileMgr.me.y)
        {
            // 위 비교
            if(LoadData[-y-1,x].Tile_Type == MW_Tile.TileIndex.eEmpty)
            {
                if(LoadData[-y-1,x].Count < CountTemp)
                {
                    BufferCount = 0; BufferReSet();
                    MoveStateBuffer[BufferCount] = MoveWayState.e_mUp;
                }
                else if(LoadData[-y-1,x].Count == CountTemp)
                {
                    MoveStateBuffer[BufferCount] = MoveWayState.e_mUp;
					BufferCount++;
                }
            }

            // 아래 비교
            if(LoadData[-y+1,x].Tile_Type == MW_Tile.TileIndex.eEmpty)
            {
                if(LoadData[-y+1,x].Count < CountTemp)
                {
                    BufferCount = 0; BufferReSet();
                    MoveStateBuffer[BufferCount] = MoveWayState.e_mDown;
                }
                else if(LoadData[-y+1,x].Count == CountTemp)
                {
                    MoveStateBuffer[BufferCount] = MoveWayState.e_mDown;
					BufferCount++;
                }
            }
        }
    }

    void MoveUpdate()
    {
        switch(MoveState)
        {
        case MoveWayState.e_mLeft: transform.position = new Vector3( transform.position.x - SPEED(),transform.position.y,transform.position.z ); break;
		case MoveWayState.e_mRight: transform.position = new Vector3( transform.position.x + SPEED(), transform.position.y,transform.position.z ); break;
		case MoveWayState.e_mUp: transform.position = new Vector3( transform.position.x,transform.position.y + SPEED(),transform.position.z ); break;
		case MoveWayState.e_mDown: transform.position = new Vector3( transform.position.x,transform.position.y - SPEED(),transform.position.z ); break;
        }
    }

    float SPEED() { return Speed / 72.0f; }
}
