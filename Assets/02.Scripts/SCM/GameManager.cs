using System.Collections;
using System.Collections.Generic;
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
}
