using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

public class TerrainTrashDataManager : MonoBehaviour
{
    public static TerrainTrashDataManager Instance;
    public Terrain terrain; // 동적으로 생성될 수 있으니, Terrain을 받아오는 함수가 필요하다.
    public PoolingManager poolCtrl; //코드에서 동적으로 할당
    public Dictionary<Vector2Int, List<TrashInfo>> dicTrash = new Dictionary<Vector2Int, List<TrashInfo>>(); // 쓰레기 정보를 담을 딕셔너리
    private List<TrashInfo> trashList = new List<TrashInfo>(); // 쓰레기 정보의 직렬화에 사용할 리스트, 리스트를 직렬화 해놔야 역직렬화 할 수 있다.

    private List<Vector2Int> loadCellList = new List<Vector2Int>(); // 현재 로드된 Cell을 담는 리스트, 이걸로 로드된 Cell을 관리할 수 있다.

    private Dictionary<int, WeightData> dicWeight = new Dictionary<int, WeightData>(); // 쓰레기 종류별 무게 정보가 들어가는 딕셔너리

    // cell 값을 id로 갖는, List를 셋팅해서 나중에 cell값을 치면 해당 cell에 있는 쓰래기 전체를 가져올수 있도록

    private int cellSize = 40;

    private void Awake()
    {
        Instance = this;

        GetTerrain();
        SetPoolingManager(); // 풀링 매니저 셋팅

        var weightJson = File.ReadAllText("./Assets/Resources/WeightData.json"); // 쓰레기 무게 데이터 파일 읽기
        var weightData = JsonConvert.DeserializeObject<WeightData[]>(weightJson); // JSON 데이터를 역직렬화
        this.dicWeight = weightData.ToDictionary(x => x.id); // 역직렬화된 데이터를 딕셔너리에 추가

        if (File.Exists("./Assets/Resources/TrashMapData.json"))
        {
            var json = File.ReadAllText("./Assets/Resources/TrashMapData.json"); // 파일이 존재하면 해당 파일을 읽어옴
            var data = JsonConvert.DeserializeObject<TrashInfo[]>(json); // JSON 데이터를 역직렬화
            this.dicTrash = new Dictionary<Vector2Int, List<TrashInfo>>();

            foreach (var trash in data) // 역직렬화된 데이터를 딕셔너리에 추가
            {
                var cell = new Vector2Int(trash.cellX, trash.cellY);
                if (!dicTrash.ContainsKey(cell)) // 딕셔너리에 해당 키가 없으면 추가
                {
                    dicTrash.Add(cell, new List<TrashInfo>());
                }
                dicTrash[cell].Add(trash); // 해당 cell에 쓰레기 정보를 추가
            }
        }
        else
        {
            CreateTrashData(200);

            var json = JsonConvert.SerializeObject(trashList); // 딕셔너리를 JSON으로 직렬화

            File.WriteAllText("./Assets/Resources/TrashMapData.json", json);
        }
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
            this.trashList.Add(trashInfo); // 생성된 쓰레기 정보를 리스트에 추가


            var cellKey = new Vector2Int(trashInfo.cellX, trashInfo.cellY);
            if (!dicTrash.ContainsKey(cellKey)) // 딕셔너리에 해당 키가 없으면 추가
            {
                dicTrash.Add(cellKey, new List<TrashInfo>());
            }
            //여기 아래에는 반드시 해당 key(cell)값에 List가 있으니까
            dicTrash[cellKey].Add(trashInfo); // 해당 cell에 쓰레기 정보를 추가
        }
    }
    private TrashInfo CreateTrash(int id)
    {
        var x = Random.Range(0, 1f);
        var z = Random.Range(0, 1f);
        float height = terrain.terrainData.GetInterpolatedHeight(x, z);

        //trashinfo에 들어갈 내용
        var pos = new Vector3(x * terrain.terrainData.size.x + terrain.transform.position.x
            , height + terrain.transform.position.y + Random.Range(0, 100f),
            z * terrain.terrainData.size.z + terrain.transform.position.z); // 좌표값
        var cell = this.GetCellFromPosition(pos); // 저 좌표값을 기반으로 한 cell값
        int kind = Random.Range(0, 3); // eNum기반 쓰래기 종류값, 
        int status = Random.Range(0, 3); // eNum기반 쓰래기 상태값, 종류값 상태값 둘다 임시로 3넣은거고 eNum만들고 난 뒤에 그거 크기대로

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
            weight = this.dicWeight[kind].weight + Random.Range(-5f, 5f) // 쓰레기 무게, 종류에 따른 무게 + 랜덤값
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
        if (dicTrash.ContainsKey(cell)) // 딕셔너리에 해당 Cell이 존재하면
        {
            foreach (var trash in dicTrash[cell]) // 해당 Cell에 있는 쓰레기 정보를 순회
            {
                // 쓰레기 로드 로직
                this.poolCtrl.GetTrash(trash); // 풀링 매니저에서 쓰레기 가져오기
            }
        }
    }
    private void UnloadTrash(Vector2Int cell) // CellMove쪽에서 사용할 쓰레기 언로드용 함수
    {
        this.loadCellList.Remove(cell); // 로드된 Cell 리스트에서 제거
        if (dicTrash.ContainsKey(cell)) // 딕셔너리에 해당 Cell이 존재하면
        {
            foreach (var trash in dicTrash[cell]) // 해당 Cell에 있는 쓰레기 정보를 순회
            {
                // 쓰레기 언로드 로직
                this.poolCtrl.UnloadTrash(cell); // 풀링 매니저에서 쓰레기 언로드
            }
        }
    }
}