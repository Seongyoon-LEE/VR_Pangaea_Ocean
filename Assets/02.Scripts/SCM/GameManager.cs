using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum State
    {
        NOMAL, RECOVERY, VOLCANO, MAGMA
    }
    public State state = State.NOMAL;
    public static GameManager Instance;
    public int[] answerArr = { 4, 1, 6 }; // 퍼즐 정답에 사용(거북이 등껍질, 퍼즐 정답)
    private int buff = 0; // 버프 횟수
    private int buffInc = 50; // 버프 1회당 증가량

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    // 버프 적용
    public void BuffApp()
    {
        if (buff < 3)
        {
            buff++;
            // 최대 무게값 관련 변수가 없음
            //DataManager.Instance.PlayerData. += buffInc;
        }
    }

    // 버프 초기화
    public void BuffReset()
    {
        //DataManager.Instance.PlayerData. -= (buff * buffInc);
    }
}
