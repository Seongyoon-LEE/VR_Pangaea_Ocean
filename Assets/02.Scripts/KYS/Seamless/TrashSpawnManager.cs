using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrashStatus // 쓰레기 상태를 나타내는 eNum
{
    Clean = 0,
    Dirty = 1,
    Damaged = 2
}
public class TrashSpawnManager : MonoBehaviour
{
    public Terrain terrain; // 동적으로 생성될 수 있으니, Terrain을 받아오는 함수가 필요하다.
    public PoolingManager poolCtrl; //코드에서 동적으로 할당

    [SerializeField]
    private PlayerCellCheck player;

    private List<Vector2Int> loadCellList = new List<Vector2Int>(); // 현재 로드된 Cell을 담는 리스트, 이걸로 로드된 Cell을 관리할 수 있다.

    // cell 값을 id로 갖는, List를 셋팅해서 나중에 cell값을 치면 해당 cell에 있는 쓰래기 전체를 가져올수 있도록

    private int cellSize = 40;


    private IEnumerator Start()
    {
        GetTerrain();
        SetPoolingManager(); // 풀링 매니저 셋팅
        this.player = FindObjectOfType<PlayerCellCheck>(); // 플레이어 셀 이동 스크립트 가져오기
        this.player.GetCellFunc = this.GetCellFromPosition; // 플레이어 셀 이동 스크립트에 현재 Cell을 가져오는 함수를 할당
        this.player.CellMoveAction = CellMove; // 플레이어 셀 이동 스크립트에 Cell이 바뀔 때 호출할 액션을 할당

        // 데이터 매니저쪽과 연계해서, 데이터 로딩이 이루어진 다음 셋팅하게
        while (!DataManager.Instance.IsLoadingFinish) // 데이터 매니저가 로딩이 끝날 때까지 대기
        {
            yield return null;
        }
        //로딩이 끝났으니 셋팅 시작
        if (!DataManager.Instance.IsDataExist) // 데이터가 없다면
        {
            CreateTrashData(200); // 쓰레기 데이터를 생성
            DataManager.Instance.SaveTrashData(); // 쓰레기 데이터를 저장
        }
        //여기까지 왔다면 쓰레기 데이터 셋팅 완료
        this.player.CellMoveAction(this.GetCellFromPosition(this.player.transform.position));
        // Cell초기화 이외에도 로딩 전에는 dimImage등을 띄우고 입력값 전부 안받다가 로딩 이후에 입력이 가능하도록 해야한다
    }
    private void GetTerrain() // Terrain을 받아오는 함수
    {
        if (terrain == null)
        {
            terrain = FindObjectOfType<Terrain>();
        }
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
        for (int i = 0; i < cnt; i++) // cnt만큼 쓰레기 정보 생성
        {
            var trashInfo = CreateTrash(i); // 쓰레기 정보를 생성하는 함수 호출


            var cellKey = new Vector2Int(trashInfo.cellX, trashInfo.cellY);
            if (!DataManager.Instance.dicTrash.ContainsKey(cellKey)) // 딕셔너리에 해당 키가 없으면 추가
            {
                DataManager.Instance.dicTrash.Add(cellKey, new List<TrashInfo>());
            }
            //여기 아래에는 반드시 해당 key(cell)값에 List가 있으니까
            DataManager.Instance.dicTrash[cellKey].Add(trashInfo); // 해당 cell에 쓰레기 정보를 추가
        }
    }
    private TrashInfo CreateTrash(int id)
    {
        var x = Random.Range(0.1f, 0.9f);
        var z = Random.Range(0.1f, 0.9f);
        float height = terrain.terrainData.GetInterpolatedHeight(x, z);

        //trashinfo에 들어갈 내용
        var pos = new Vector3(x * terrain.terrainData.size.x + terrain.transform.position.x
            , height + terrain.transform.position.y + Random.Range(0, 100f),
            z * terrain.terrainData.size.z + terrain.transform.position.z); // 좌표값
        var cell = this.GetCellFromPosition(pos); // 저 좌표값을 기반으로 한 cell값
        int kind = Random.Range(0, this.poolCtrl.trashPrefabs.Count); // 쓰래기 종류값
        int status = (int)TrashStatus.Dirty; // eNum기반 쓰래기 상태값

        //var rotation = Quaternion.identity; // 회전을 넣게 된다면 회전값
        float rotX = Random.Range(0f, 360f);
        float rotY = Random.Range(0f, 360f);
        float rotZ = Random.Range(0f, 360f);

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
            rotX = rotX,
            rotY = rotY,
            rotZ = rotZ,
            //weight = Random.Range(0f, 5f) // 쓰레기 무게, 종류에 따른 무게 + 랜덤값
            weight = DataManager.Instance.dicWeight[kind].weight + Random.Range(-0.5f, 1f) // 쓰레기 무게, 종류에 따른 무게 + 랜덤값
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
            UnloadTrash(unloadCell); // 쓰레기 언로드 함수 호출
        }

        var loadList = newLoadCellList.FindAll(x => !this.loadCellList.Contains(x)); // 새로 로드할 Cell 리스트에서 현재 로드된 Cell 리스트에 없는 Cell을 찾음

        foreach (var loadCell in loadList) // 새로 로드할 Cell 리스트에 있는 Cell들을 순회
        {
            LoadTrash(loadCell); // 쓰레기 로딩 함수 호출
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

    private void LoadTrash(Vector2Int cell) // CellMove쪽에서 사용할 쓰레기 로딩용 함수
    {
        this.loadCellList.Add(cell); // 로드된 Cell 리스트에 추가
        if (DataManager.Instance.dicTrash.ContainsKey(cell)) // 딕셔너리에 해당 Cell이 존재하면
        {
            foreach (var trash in DataManager.Instance.dicTrash[cell]) // 해당 Cell에 있는 쓰레기 정보를 순회
            {
                // 쓰레기 로드 로직
                if (trash.status == (int)TrashStatus.Clean) // 청소된 상태라면 로딩하지 않는다
                    return;
                this.poolCtrl.GetTrash(trash); // 풀링 매니저에서 쓰레기 가져오기
            }
        }
    }
    private void UnloadTrash(Vector2Int cell) // CellMove쪽에서 사용할 쓰레기 언로드용 함수
    {
        this.loadCellList.Remove(cell); // 로드된 Cell 리스트에서 제거
        if (DataManager.Instance.dicTrash.ContainsKey(cell)) // 딕셔너리에 해당 Cell이 존재하면
        {
            foreach (var trash in DataManager.Instance.dicTrash[cell]) // 해당 Cell에 있는 쓰레기 정보를 순회
            {
                // 쓰레기 언로드 로직
                this.poolCtrl.UnloadTrash(cell); // 풀링 매니저에서 쓰레기 언로드
            }
        }
    }
}