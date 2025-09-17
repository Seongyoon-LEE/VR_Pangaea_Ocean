using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum State
    {
        NORMAL, RECOVERY, VOLCANO, MAGMA, TORNADO
    }
    public State state = State.NORMAL;
    public static GameManager Instance;
    public int[] answerArr = { 4, 1, 6 }; // 퍼즐 정답에 사용(거북이 등껍질, 퍼즐 정답)
    private readonly int buffInc = 50; // 버프 1회당 증가량
    private float initMaxWeight = 300f; // 초기 최대 무게
    public float curMaxWeight = 300f; // 현재 최대 무게
    private readonly string turtleKey = "Turtle";
    private readonly string puzzleKey = "Puzzle";
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }
    private IEnumerator Start()
    {
        while (!DataManager.Instance.IsLoadingFinish)
        {
            yield return null;
        }

        
        BuffApp();
    }

    // 버프 적용
    // 초기화 후에도 실행하여 버프 초기화
    public void BuffApp()
    {
        curMaxWeight = initMaxWeight;
        foreach (var puzzle in DataManager.Instance.dicPuzzle)
        {
            if (puzzle.Key.StartsWith(turtleKey))
            {
                if (puzzle.Value) curMaxWeight += buffInc;
            }
        }
    }

    // 퍼즐키 이면 삭제하고 거북이키이면 false로 초기화 한다.
    public void puzzleDicReset(string dicKey)
    {
        // 딕셔너리를 foreach로 돌릴 때 제거를하게 되면 오류가 발생할 수 있다.
        // 따라서 따로 키를 저장했다가 한번에 삭제한다.
        List<string> tempKey = new List<string>(); // 키를 임시 저장할 리스트
        foreach (var dic in DataManager.Instance.dicPuzzle)
        {
            if (dic.Key.StartsWith(dicKey))
            {
                if (dicKey == turtleKey) DataManager.Instance.dicPuzzle[dic.Key] = false;
                else if (dicKey == puzzleKey) tempKey.Add(dic.Key);
            }
        }

        if (tempKey.Count > 0)
        {
            foreach (var key in tempKey)
            {
                DataManager.Instance.dicPuzzle.Remove(key);
            }
        }
    }

}
