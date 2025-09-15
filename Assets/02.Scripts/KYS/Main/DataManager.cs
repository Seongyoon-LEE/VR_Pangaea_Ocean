using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;
using DG.Tweening.Plugins.Core.PathCore;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    public Dictionary<Vector2Int, List<TrashInfo>> dicTrash = new Dictionary<Vector2Int, List<TrashInfo>>(); // 쓰레기 정보를 담을 딕셔너리
    public Dictionary<Vector2Int, List<ObstacleInfo>> dicObstacle = new Dictionary<Vector2Int, List<ObstacleInfo>>(); // 장애물 정보를 담을 딕셔너리

    public Dictionary<int, WeightData> dicWeight = new Dictionary<int, WeightData>(); // 쓰레기 종류별 무게 정보가 들어가는 딕셔너리
    public Dictionary<string,bool> dicPuzzle = new Dictionary<string,bool>(); // 퍼즐 클리어 여부를 담는 딕셔너리
    public PlayerData PlayerData { get; set; }
    public bool PlayerDataSaved { private get; set; } = false; // 플레이어 데이터가 저장되었는지 여부

    public bool IsLoadingFinish { get; private set; } = false; // 쓰레기 로딩이 끝났는지 여부
    public bool IsTrashDataExist { get; private set; } = false; // 쓰레기 데이터가 존재하는지 여부
    public bool IsPlayerDataExist { get; private set; } = false; // 플레이어 데이터가 존재하는지 여부
    public bool IsPlayerLoading { get; private set; } = false; // 플레이어 데이터 선 로딩
    public bool IsTrashDataLoading { get; private set; } = false; // 쓰레기 데이터 등 선 로딩
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject); // 씬이 바뀌어도 파괴되지 않도록 설정
        LoadData();
    }

    private async void LoadData()
    {
        // 로딩 전에 dimImage로 화면 가리고, 입력 막는 등의 내용 추가
        await Task.Run(() =>
        {
            var weightJson = File.ReadAllText("./Assets/Resources/WeightData.json"); // 쓰레기 무게 데이터 파일 읽기
            var weightData = JsonConvert.DeserializeObject<WeightData[]>(weightJson); // JSON 데이터를 역직렬화
            this.dicWeight = weightData.ToDictionary(x => x.id); // 역직렬화된 데이터를 딕셔너리에 추가

            if (File.Exists("./Assets/Resources/TrashMapData.json"))
            {
                this.IsTrashDataExist = true; // 파일이 존재하면 데이터가 있다고 표시
                var trashJson = File.ReadAllText("./Assets/Resources/TrashMapData.json"); // 파일이 존재하면 해당 파일을 읽어옴
                /*string trashJson;
                using (var stream = File.Open("./Assets/Resources/TrashMapData.json", FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    
                    // 파일 읽기/쓰기 작업
                    using (var reader = new StreamReader(stream))
                    {
                        trashJson = reader.ReadToEnd();
                    }
                    
                }*/
                var trashData = JsonConvert.DeserializeObject<Dictionary<string, List<TrashInfo>>>(trashJson); // JSON 데이터를 딕셔너리로 역직렬화
                this.dicTrash = trashData.ToDictionary(x => Vector2IntParse(x.Key), x => x.Value); // 딕셔너리로 변환

                // TrashMapData가 있다면 당연히 ObstacleMapData도 있음

                var obstacleJson = File.ReadAllText("./Assets/Resources/ObstacleMapData.json"); // 파일이 존재하면 해당 파일을 읽어옴
                var obstacleData = JsonConvert.DeserializeObject<Dictionary<string, List<ObstacleInfo>>>(obstacleJson); // JSON 데이터를 딕셔너리로 역직렬화
                this.dicObstacle = obstacleData.ToDictionary(x => Vector2IntParse(x.Key), x => x.Value); // 딕셔너리로 변환

                // PuzzleMapData도 있음
                var puzzleJson = File.ReadAllText("./Assets/Resources/PuzzleMapData.json"); // 파일이 존재하면 해당 파일을 읽어옴
                this.dicPuzzle = JsonConvert.DeserializeObject<Dictionary<string, bool>>(puzzleJson);
            }

            if (File.Exists("./Assets/Resources/PlayerData.json"))
            {
                this.IsPlayerDataExist = true; // 플레이어 데이터 파일이 존재하면 데이터가 있다고 표시
                var json = File.ReadAllText("./Assets/Resources/PlayerData.json"); // 플레이어 데이터 파일 읽기
                this.PlayerData = JsonConvert.DeserializeObject<PlayerData>(json); // JSON 데이터를 역직렬화
            }
        });
        this.IsPlayerLoading = true;
        this.IsTrashDataLoading = true;
        // 로딩이 끝나면 정상적으로 게임 진행하는 내용 추가
        while (this.PlayerData == null)
        {
            await Task.Yield(); // 최초 실행 시 플레이어 데이터 생성까지 대기
        }
        IsLoadingFinish = true; // 로딩 완료
        Debug.Log("로딩 완료");
    }

    private Vector2Int Vector2IntParse(string key)
    {
        var parts = key.Trim('(', ')') // 괄호 때고
            .Split(','); // 쉼표로 분리

        int x = int.Parse(parts[0]);
        int y = int.Parse(parts[1]);

        return new Vector2Int(x, y);
    }
    public async void SaveData()
    {
        // OnApplicationQuit이 아닌 인게임에 들어가고 난 다음 게임을 끌때 따로 실행될수 있도록 빼야한다.
        string trashJson = null;
        string obstacleJson = null;
        string playerJson = null;
        string pJson = null;
        Debug.Log("저장시작");
        while (!this.PlayerDataSaved)
        {
            await Task.Yield(); // 플레이어 데이터가 저장될 때까지 대기
        }

        await Task.Run(() =>
        {
            trashJson = JsonConvert.SerializeObject(dicTrash); // 딕셔너리를 JSON으로 직렬화
            obstacleJson = JsonConvert.SerializeObject(dicObstacle); // 딕셔너리를 JSON으로 직렬화
            pJson = JsonConvert.SerializeObject(dicPuzzle); // 딕셔너리를 JSON으로 직렬화
            playerJson = JsonConvert.SerializeObject(PlayerData); // 플레이어 데이터를 JSON으로 직렬화
        });
        //await File.WriteAllTextAsync("./Assets/Resources/TrashMapData.json", trashJson);
        File.WriteAllText("./Assets/Resources/TrashMapData.json", trashJson);
        //await File.WriteAllTextAsync("./Assets/Resources/ObstacleMapData.json", obstacleJson);
        File.WriteAllText("./Assets/Resources/ObstacleMapData.json", obstacleJson);
        File.WriteAllText("./Assets/Resources/PlayerData.json", playerJson);
        //await File.WriteAllTextAsync("./Assets/Resources/PlayerData.json", playerJson);
        //await File.WriteAllTextAsync("./Assets/Resources/PuzzleMapData.json", pJson);
        File.WriteAllText("./Assets/Resources/PuzzleMapData.json", pJson);
        Debug.Log("저장됨");
        this.PlayerDataSaved = false; // 저장이 끝났으니 다시 false로 설정
    }

    /*private void OnApplicationQuit()
    {
        StartCoroutine(this.QuitRoutine());
    }
    IEnumerator QuitRoutine()
    {
        while (!PlayerDataSaved)
        {
            yield return null;
        }
    }*/
}

//Application.persistentDataPath : 빌드할때 저장이 잘 안된다면 경로명으로 이거 사용
//사용 예 : dataPath = Application.persistentDataPath + "/gameData.dat"