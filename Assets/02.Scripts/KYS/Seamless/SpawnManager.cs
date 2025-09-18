using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.ImageEffects;


public class SpawnManager : MonoBehaviour
{
    public Terrain terrain; // 동적으로 생성될 수 있으니, Terrain을 받아오는 함수가 필요하다.
    public TornadoPosManager tornadoPosManager;
    public PoolingManager poolCtrl; //코드에서 동적으로 할당

    [SerializeField]
    private PlayerCellCheck player;

    private List<Vector2Int> loadCellList = new List<Vector2Int>(); // 현재 로드된 Cell을 담는 리스트, 이걸로 로드된 Cell을 관리할 수 있다.

    // cell 값을 id로 갖는, List를 셋팅해서 나중에 cell값을 치면 해당 cell에 있는 쓰래기 전체를 가져올수 있도록

    private int cellSize = 100; // 셀의 사이즈


    private IEnumerator Start()
    {
        GetTerrain();
        SetPoolingManager(); // 풀링 매니저 셋팅
        this.player = FindObjectOfType<PlayerCellCheck>(); // 플레이어 셀 이동 스크립트 가져오기
        this.player.GetCellFunc = this.GetCellFromPosition; // 플레이어 셀 이동 스크립트에 현재 Cell을 가져오는 함수를 할당
        this.player.CellMoveAction = CellMove; // 플레이어 셀 이동 스크립트에 Cell이 바뀔 때 호출할 액션을 할당

        // 데이터 매니저쪽과 연계해서, 데이터 로딩이 이루어진 다음 셋팅하게
        while (!DataManager.Instance.IsTrashDataLoading) // 데이터 매니저가 로딩이 끝날 때까지 대기
        {
            yield return null;
        }
        //로딩이 끝났으니 셋팅 시작
        if (!DataManager.Instance.IsTrashDataExist) // 데이터가 없다면
        {
            CreateTrashData(500); // 쓰레기 데이터를 생성
            //Obstacle도 여기서 생성해도 된다. 쓰래기 데이터가 있는데 장애물 데이터가 없는게 말이 안된다.
            CreateObstacleData(10); // 장애물 데이터를 생성
        }
        //여기까지 왔다면 쓰레기 데이터 셋팅 완료
        this.player.CellMoveAction(this.GetCellFromPosition(this.player.transform.position));
        // Cell초기화 이외에도 로딩 전에는 dimImage등을 띄우고 입력값 전부 안받다가 로딩 이후에 입력이 가능하도록 해야한다
    }

    public void SetNextStage()
    {
        GetTerrain();
        // 풀링 전체 언로딩
        this.poolCtrl.UnloadEverything();
        DataManager.Instance.UnloadData();
        CreateTrashData(500); // 쓰레기 데이터를 생성
        CreateObstacleData(10); // 장애물 데이터를 생성
        this.player.CellMoveAction(this.GetCellFromPosition(this.player.transform.position));
    }

    private void GetTerrain() // Terrain을 받아오는 함수
    {
        /*if (terrain == null)
        {
            terrain = FindObjectOfType<Terrain>();
        }*/
        terrain = FindObjectOfType<Terrain>(); // 맵이 바뀐 뒤에 다시 받아올수도 있음(null이 아닐 수 있다)
        this.tornadoPosManager = FindObjectOfType<TornadoPosManager>(); // terrain과 함께 새로 받아온다.
    }
    private void SetPoolingManager()
    {
        this.poolCtrl = FindObjectOfType<PoolingManager>();
        this.poolCtrl.Init(); // 풀링 매니저 초기화
    }
    public Vector2Int GetCellFromPosition(Vector3 pos) // 좌표값 기반 Cell값 반환 함수
    {
        int x = Mathf.FloorToInt(pos.x / cellSize);
        int y = Mathf.FloorToInt(pos.z / cellSize);
        return new Vector2Int(x, y);
    }
    private void CreateTrashData(int cnt)
    {
        int i = 0;
        for ( ; i < cnt; i++) // cnt만큼 쓰레기 정보 생성
        {
            var trashInfo = CreateTrash(i); // 쓰레기 정보를 생성하는 함수 호출
            var cellKey = new Vector2Int(trashInfo.cellX, trashInfo.cellY);
            if (!DataManager.Instance.dicTrash.ContainsKey(cellKey)) // 딕셔너리에 해당 키가 없으면 추가
            {
                DataManager.Instance.dicTrash.Add(cellKey, new List<TrashInfo>());
            }
            //여기 아래에는 반드시 해당 key(cell)값에 List가 있으니까
            DataManager.Instance.dicTrash[cellKey].Add(trashInfo); // 해당 cell에 쓰레기 정보를 추가

            //여기서 종류값 체크
            //종류가 부숴지는 종류라면 내부에 포함된 쓰레기의 개수만큼 동일한 내용을 넣는다
            if(trashInfo.kind == 13) // 묶인 쓰레기는 13번, 해초 쓰레기는 내부에 쓰레기 10~15개 포함
            {
                //위치는 같아야하고, 종류값만 12 이하의 숫자로 바꿔서 info 6개 생성
                trashInfo.count = Random.Range(10, 15 + 1);
                var innerCnt = i++ + trashInfo.count + 1;
                for( ; i < innerCnt; i++) 
                {
                    var newTrashInfo = new TrashInfo // 기존 쓰레기 정보를 복사
                    {
                        id = i, // 새로운 ID 할당
                        cellX = trashInfo.cellX,
                        cellY = trashInfo.cellY,
                        kind = Random.Range(0, 13), // 종류값을 0~12 사이로 랜덤하게 설정
                        status = trashInfo.status, // 상태값은 기존 쓰레기와 동일하게 설정
                        posX = trashInfo.posX,
                        posY = trashInfo.posY,
                        posZ = trashInfo.posZ,
                        height = trashInfo.height, // 높이는 기존 쓰레기와 동일하게 설정
                        rotX = Random.Range(0f, 360f),
                        rotY = Random.Range(0f, 360f),
                        rotZ = Random.Range(0f, 360f),
                    };
                    newTrashInfo.weight = DataManager.Instance.dicWeight[newTrashInfo.kind].weight + Random.Range(-0.5f, 1f); // 쓰레기 무게, 종류에 따른 무게 + 랜덤값
                    DataManager.Instance.dicTrash[cellKey].Add(newTrashInfo); // 해당 cell에 새로운 쓰레기 정보 추가
                }
                i--;//위쪽에 생성하는 for문, 그리고 바깥쪽 전체 for문에서 i를 2번 더하게 되기에, 한번 뺌
            }
            if (trashInfo.kind == 14) // 자동차 : 14번
            {
                trashInfo.count = 20;
                var innerCnt = i++ + trashInfo.count; 
                for (; i< innerCnt; i++) //자동차 쓰레기는 내부에 쓰레기 20개 포함중, 한개는 이미 생성됨
                {
                    var newTrashInfo = new TrashInfo
                    {
                        id = i, // 새로운 ID 할당
                        cellX = trashInfo.cellX,
                        cellY = trashInfo.cellY,
                        kind = trashInfo.kind,
                        status = trashInfo.status, // 상태값은 기존 쓰레기와 동일하게 설정
                        posX = trashInfo.posX,
                        posY = trashInfo.posY,
                        posZ = trashInfo.posZ,
                        height = trashInfo.height, // 높이는 기존 쓰레기와 동일하게 설정
                        rotX = trashInfo.rotX,
                        rotY = trashInfo.rotY,
                        rotZ = trashInfo.rotZ
                    };
                    newTrashInfo.weight = DataManager.Instance.dicWeight[newTrashInfo.kind].weight + Random.Range(-0.5f, 1f); // 쓰레기 무게, 종류에 따른 무게 + 랜덤값
                    DataManager.Instance.dicTrash[cellKey].Add(newTrashInfo); // 해당 cell에 새로운 쓰레기 정보 추가
                }
                i--;//위쪽에 생성하는 for문, 그리고 바깥쪽 전체 for문에서 i를 2번 더하게 되기에, 한번 뺌
            }
            if (trashInfo.kind == 15) // 박스 : 15번
            {
                trashInfo.count = Random.Range(3, 6);
                var innerCnt = i++ + trashInfo.count;
                for (; i < innerCnt; i++) //박스 쓰레기는 내부에 쓰레기 3~6개 포함중, 한개는 이미 생성됨
                {
                    var newTrashInfo = new TrashInfo
                    {
                        id = i, // 새로운 ID 할당
                        cellX = trashInfo.cellX,
                        cellY = trashInfo.cellY,
                        kind = trashInfo.kind,
                        status = trashInfo.status, // 상태값은 기존 쓰레기와 동일하게 설정
                        posX = trashInfo.posX,
                        posY = trashInfo.posY,
                        posZ = trashInfo.posZ,
                        height = trashInfo.height, // 높이는 기존 쓰레기와 동일하게 설정
                        rotX = trashInfo.rotX,
                        rotY = trashInfo.rotY,
                        rotZ = trashInfo.rotZ
                    };
                    newTrashInfo.weight = DataManager.Instance.dicWeight[newTrashInfo.kind].weight + Random.Range(-0.5f, 1f); // 쓰레기 무게, 종류에 따른 무게 + 랜덤값
                    DataManager.Instance.dicTrash[cellKey].Add(newTrashInfo); // 해당 cell에 새로운 쓰레기 정보 추가
                }
                i--;//위쪽에 생성하는 for문, 그리고 바깥쪽 전체 for문에서 i를 2번 더하게 되기에, 한번 뺌
            }
        }
    }
    private TrashInfo CreateTrash(int id)
    {
        int kind = Random.Range(0, this.poolCtrl.trashPrefabs.Count); // 쓰래기 종류값
        float x;
        float z;
        float y;
        float rotX;
        float rotY;
        float rotZ;
        float height;

        x = Random.Range(0.1f, 0.9f);
        z = Random.Range(0.1f, 0.9f);
        y = terrain.terrainData.GetInterpolatedHeight(x, z);
        //trashinfo에 들어갈 내용
        var pos = new Vector3(x * terrain.terrainData.size.x + terrain.transform.position.x
            , y + terrain.transform.position.y,
            z * terrain.terrainData.size.z + terrain.transform.position.z); // 좌표값
        if (kind == 13) // 임시 테스트용, 풀에 묶인 쓰레기의 종류값 13
        {
            rotX = 0;
            rotY = 0;
            rotZ = 0;
            height = Random.Range(3, 6f); // 풀에 묶인 쓰레기는 회전값을 0으로 고정하고 높이를 랜덤으로 설정
        }
        else
        {
            rotX = Random.Range(0f, 360f);
            rotY = Random.Range(0f, 360f);
            rotZ = Random.Range(0f, 360f);
            height = Random.Range(0, -(pos.y + 8));
        }

        var cell = this.GetCellFromPosition(pos); // 저 좌표값을 기반으로 한 cell값
        int status = (int)TrashStatus.Dirty; // eNum기반 쓰래기 상태값
        return new TrashInfo
        {
            id = id, // 쓰레기 ID
            cellX = cell.x,
            cellY = cell.y,
            kind = kind,
            status = status,
            posX = pos.x,
            posY = pos.y,
            posZ = pos.z,
            height = height,
            rotX = rotX,
            rotY = rotY,
            rotZ = rotZ,
            weight = DataManager.Instance.dicWeight[kind].weight + Random.Range(-0.5f, 1f) // 쓰레기 무게, 종류에 따른 무게 + 랜덤값
        }; // TrashInfo 오브젝트 생성
        // 이에 대해서 오브젝트를 생성하는게 아닌, json을 활용해서 데이터를 우선적으로 생성한다.
    }

    //CreateObstacleData() // 장애물 데이터 생성 함수, 위의 CreateTrashData와 비슷한 방식으로
    //안에 종류값에 따라서 추가로 생성 와바박 하는 내용이 필요없어진다.
    private void CreateObstacleData(int cnt)
    {
        for (int i = 0; i < cnt; i++) // cnt만큼 쓰레기 정보 생성
        {
            var obstacleInfo = CreateObstacle(i); // 쓰레기 정보를 생성하는 함수 호출
            var cellKey = new Vector2Int(obstacleInfo.cellX, obstacleInfo.cellY);
            if (!DataManager.Instance.dicObstacle.ContainsKey(cellKey)) // 딕셔너리에 해당 키가 없으면 추가
            {
                DataManager.Instance.dicObstacle.Add(cellKey, new List<ObstacleInfo>());
            }
            //여기 아래에는 반드시 해당 key(cell)값에 List가 있으니까
            DataManager.Instance.dicObstacle[cellKey].Add(obstacleInfo); // 해당 cell에 쓰레기 정보를 추가
        }
    }
    //CreateObstacle() // 장애물 정보 생성 함수, 위의 CreateTrash와 비슷한 방식으로,
    //단 안에 종류값 체크해서 생성되는 위치를 바꾸는 등의 내용이 추가된다.
    private ObstacleInfo CreateObstacle(int id)
    {
        int kind = Random.Range(0, this.poolCtrl.obstaclePrefabs.Count); // 장애물 종류값 (0 : 상어, 1 : 소용돌이)

        // 단, 장애물은 종류값에 따라서 생성 설정이 달라질 수 있음
        Vector3 pos; 
        if(kind == 0) // 상어의 경우
        {
            var x = Random.Range(0.1f, 0.9f);
            var z = Random.Range(0.1f, 0.9f);
            float y = terrain.terrainData.GetInterpolatedHeight(x, z);

            //trashinfo에 들어갈 내용
            pos = new Vector3(x * terrain.terrainData.size.x + terrain.transform.position.x
                , y + terrain.transform.position.y + Random.Range(10, -(y + terrain.transform.position.y)),
                z * terrain.terrainData.size.z + terrain.transform.position.z); // 좌표값
        }
        else // 소용돌이의 경우
        {
            //미리 잡혀있던 소용돌이의 좌표
            //단, 한 곳에 여러번 생기면 안되니 소용돌이 좌표를 컨트롤해줄 스크립트를 따로 짜서 거기서 받아야함
            pos = this.tornadoPosManager.GetTornadoPos();
        }
        var cell = this.GetCellFromPosition(pos); // 저 좌표값을 기반으로 한 cell값



        return new ObstacleInfo
        {
            id = id, // 쓰레기 ID
            cellX = cell.x,
            cellY = cell.y,
            kind = kind,
            posX = pos.x,
            posY = pos.y,
            posZ = pos.z,

        }; // TrashInfo 오브젝트 생성
        // 이에 대해서 오브젝트를 생성하는게 아닌, json을 활용해서 데이터를 우선적으로 생성한다.
    }

    public void CellMove(Vector2Int cell)// 셀 이동시 호출할 함수
    {
        var newLoadCellList = new List<Vector2Int>(); // 새로 로드할 Cell 리스트
        newLoadCellList.Add(cell); // 현재 Cell을 새로 로드할 Cell 리스트에 추가
        newLoadCellList.AddRange(GetAdjacentCells(cell)); // 현재 Cell의 인접 Cell들을 새로 로드할 Cell 리스트에 추가

        var unloadList = this.loadCellList.FindAll(x => !newLoadCellList.Contains(x)); // 현재 로드된 Cell 리스트에서 새로 로드할 Cell 리스트에 없는 Cell을 찾음

        foreach (var unloadCell in unloadList) // 현재 로드된 Cell 리스트에 있는 Cell들을 순회
        {
            UnloadCell(unloadCell); // 쓰레기 언로드 함수 호출
        }

        var loadList = newLoadCellList.FindAll(x => !this.loadCellList.Contains(x)); // 새로 로드할 Cell 리스트에서 현재 로드된 Cell 리스트에 없는 Cell을 찾음

        foreach (var loadCell in loadList) // 새로 로드할 Cell 리스트에 있는 Cell들을 순회
        {
            LoadCell(loadCell); // 쓰레기 로딩 함수 호출
        }
    }
    private List<Vector2Int> GetAdjacentCells(Vector2Int cell)
    {
        List<Vector2Int> adjacentCells = new List<Vector2Int>();
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue; // 현재 Cell은 제외
                Vector2Int adjacentCell = new Vector2Int(cell.x + dx, cell.y + dy);
                if (!adjacentCells.Contains(adjacentCell)) // 중복 체크
                {
                    adjacentCells.Add(adjacentCell); // 인접 Cell 추가
                }
            }
        }
        return adjacentCells; // 인접 Cell 리스트 반환
    }

    private void LoadCell(Vector2Int cell) // CellMove쪽에서 사용할 쓰레기 로딩용 함수
    {
        this.loadCellList.Add(cell); // 로드된 Cell 리스트에 추가
        if (DataManager.Instance.dicTrash.ContainsKey(cell)) // 딕셔너리에 해당 Cell이 존재하면
        {
            int i = 0;
            for(; i < DataManager.Instance.dicTrash[cell].Count; i++)
            {
                // 쓰레기 로드 로직


                //쓰레기를 받아왔는데, 종류값이 부숴지는 종류면-> 그 개수 만큼 건너 뛰고 로딩방법 변경
                if (DataManager.Instance.dicTrash[cell][i].kind == 13) // 해초 쓰레기는 13번
                {
                    var list = new List<TrashInfo>();
                    list.Add(DataManager.Instance.dicTrash[cell][i]); // 해당 쓰레기 정보를 리스트에 추가
                    for (int j = 1; j < DataManager.Instance.dicTrash[cell][i].count + 1; j++) // 해초 쓰레기는 내부에 쓰레기 랜덤 개수 포함(본인 미포함)
                    {
                        list.Add(DataManager.Instance.dicTrash[cell][i + j]); // 해당 쓰레기 정보를 리스트에 추가
                    }
                    i += list.Count; // 인덱스 넘기기
                    if(DataManager.Instance.dicTrash[cell][i - list.Count].status == (int)TrashStatus.Clean)
                    {
                        var trashCheck = list.Find(x => x.status != (int)TrashStatus.Clean);
                        if(trashCheck == null)
                        {
                            continue; // 내부 쓰레기 전부 청소된 상태라면 로딩하지 않는다
                        }
                    }
                    if (DataManager.Instance.dicTrash[cell][i - list.Count].status == (int)TrashStatus.Dirty) // 애초에 청소를 안했던 상태라면
                    {
                        this.poolCtrl.GetGrassTrash(list);
                        continue;
                    }
                    //여기로 나왔다면 청소를 했었는데 플레이어 사망등의 이유로 쓰레기가 원복된것
                    DataManager.Instance.dicTrash[cell][i - list.Count].status = (int)TrashStatus.Damaged; // 위에서 i값을 더하고 있었기 때문에 - Count를 해서 본래 해초가 바뀌도록
                    this.poolCtrl.GetGrassTrash(list);
                    continue;
                }
                if (DataManager.Instance.dicTrash[cell][i].kind >= 14) // 부숴지는 쓰레기는 14번 이상
                {
                    var list = new List<TrashInfo>();
                    for (int j = 0; j < DataManager.Instance.dicTrash[cell][i].count; j++) // 부숴지는 쓰레기는 내부에 쓰레기 랜덤 개수 포함(본인 포함)
                    {
                        list.Add(DataManager.Instance.dicTrash[cell][i + j]); // 해당 쓰레기 정보를 리스트에 추가
                    }
                    i += list.Count; // 인덱스 넘기기
                    var trashCheck = list.Find(x => x.status != (int)TrashStatus.Clean);
                    if (trashCheck == null)
                    {
                        continue; // 내부 쓰레기 전부 청소된 상태라면 로딩하지 않는다
                    }
                    //여기로 나왔다면 어떤식으로든 청소가 완료되지 않았었던것
                    this.poolCtrl.GetBreakableTrash(list);
                    continue;
                }

                // 그 개수만큼의 info의 status가 전부 clean인지 확인, 만약 clean이라면 생성하지 않는다.
                // 아니라면 우선 전체로 하나 만들고, 쓰레기 내부 Init에서 초기화한다
                // info들로 리스트(List<trashInfo>)를 만들어서, 그 리스트를 넘겨주고, 그 리스트를 받은 Init이 내부 체크해서,
                // damaged가 하나라도 있다면 부모 오브젝트 끄고 내부 레이어 변경시키고
                // clean인건 비활성화, damaged인것만 활성화, dirty상태(일부만 주웠는데 플레이어가 사망해서 돌아감)도 활성화
                // damaged없이 전부 dirty 상태라면 부모오브젝트를 활성화
                if (DataManager.Instance.dicTrash[cell][i].status == (int)TrashStatus.Clean) // 청소된 상태라면 로딩하지 않는다
                    continue;
                this.poolCtrl.GetTrash(DataManager.Instance.dicTrash[cell][i]); // 풀링 매니저에서 쓰레기 가져오기
            }
        }
        // Obstacle도 위와 비슷한 방식으로 여기에 if문 새로 써서 로드
        if (DataManager.Instance.dicObstacle.ContainsKey(cell)) // 딕셔너리에 해당 Cell이 존재하면
        {
            for (int i = 0; i < DataManager.Instance.dicObstacle[cell].Count; i++)
            {
                if (!DataManager.Instance.dicObstacle[cell][i].active)
                    continue;
                this.poolCtrl.GetObstacle(DataManager.Instance.dicObstacle[cell][i]); // 풀링 매니저에서 장애물 가져오기
            }
        }
    }
    private void UnloadCell(Vector2Int cell) // CellMove쪽에서 사용할 쓰레기 언로드용 함수
    {
        this.loadCellList.Remove(cell); // 로드된 Cell 리스트에서 제거
        if (DataManager.Instance.dicTrash.ContainsKey(cell)) // 딕셔너리에 해당 Cell이 존재하면
        {
            this.poolCtrl.UnloadTrash(cell); // 풀링 매니저에서 쓰레기 언로드
        }
        // Obstacle도 위와 비슷한 방식으로 여기에 if문 새로 써서 언로드
        if (DataManager.Instance.dicObstacle.ContainsKey(cell)) // 딕셔너리에 해당 Cell이 존재하면
        {
            this.poolCtrl.UnloadObstacle(cell); // 풀링 매니저에서 장애물 언로드
        }
    }
}