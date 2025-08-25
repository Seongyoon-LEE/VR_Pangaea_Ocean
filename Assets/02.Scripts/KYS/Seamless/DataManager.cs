using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    public Dictionary<Vector2Int, List<TrashInfo>> dicTrash = new Dictionary<Vector2Int, List<TrashInfo>>(); // 쓰레기 정보를 담을 딕셔너리
    public Dictionary<int, WeightData> dicWeight = new Dictionary<int, WeightData>(); // 쓰레기 종류별 무게 정보가 들어가는 딕셔너리
    public PlayerData playerData;
    public bool PlayerDataSaved { private get; set; } // 플레이어 데이터가 저장되었는지 여부

    public bool IsLoadingFinish { get; private set; } = false; // 쓰레기 로딩이 끝났는지 여부
    public bool IsTrashDataExist { get; private set; } = false; // 쓰레기 데이터가 존재하는지 여부
    public bool IsPlayerDataExist { get; private set; } = false; // 플레이어 데이터가 존재하는지 여부
    private void Awake()
    {
        Instance = this;
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
                var json = File.ReadAllText("./Assets/Resources/TrashMapData.json"); // 파일이 존재하면 해당 파일을 읽어옴
                var data = JsonConvert.DeserializeObject<Dictionary<string, List<TrashInfo>>>(json); // JSON 데이터를 딕셔너리로 역직렬화
                this.dicTrash = data.ToDictionary(x => Vector2IntParse(x.Key), x => x.Value); // 딕셔너리로 변환
            }
            if (File.Exists("./Assets/Resources/PlayerData.json"))
            {
                this.IsPlayerDataExist = true; // 플레이어 데이터 파일이 존재하면 데이터가 있다고 표시
                var json = File.ReadAllText("./Assets/Resources/PlayerData.json"); // 플레이어 데이터 파일 읽기
                this.playerData = JsonConvert.DeserializeObject<PlayerData>(json); // JSON 데이터를 역직렬화
            }
        });
        // 로딩이 끝나면 정상적으로 게임 진행하는 내용 추가
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
        string trashJson = null;
        string playerJson = null;
        this.PlayerDataSaved = false; // 저장 시작 전에 false로 설정

        await PlayerDataCheckAsync(); // 플레이어 데이터가 저장될 때까지 대기


        await Task.Run(() =>
        {
            trashJson = JsonConvert.SerializeObject(dicTrash); // 딕셔너리를 JSON으로 직렬화
            playerJson = JsonConvert.SerializeObject(playerData); // 플레이어 데이터를 JSON으로 직렬화
        });
        await File.WriteAllTextAsync("./Assets/Resources/TrashMapData.json", trashJson);
        await File.WriteAllTextAsync("./Assets/Resources/PlayerData.json", playerJson);
        Debug.Log("저장됨");
    }

    async Task PlayerDataCheckAsync()
    {
        while (!this.PlayerDataSaved)
        {
            await Task.Yield(); // 플레이어 데이터가 저장될 때까지 대기
        }
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes
    }

    private void OnApplicationQuit()
    {
        SaveData(); //애플리케이션 종료 시 쓰레기 데이터를 저장
    }
}

//Application.persistentDataPath : 빌드할때 저장이 잘 안된다면 경로명으로 이거 사용
//사용 예 : dataPath = Application.persistentDataPath + "/gameData.dat"