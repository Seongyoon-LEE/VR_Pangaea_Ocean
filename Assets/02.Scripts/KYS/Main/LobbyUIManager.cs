using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    private Button startBtn;
    private Button exitBtn;
    void Start()
    {
        startBtn = GameObject.Find("StartBtn").GetComponent<Button>();
        exitBtn = GameObject.Find("ExitBtn").GetComponent<Button>();
        this.startBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(5); // 내부 로직 관련
            SceneManager.LoadScene(6, LoadSceneMode.Additive); // 플레이어
            if (DataManager.Instance.IsPlayerDataExist)
            {
                SceneManager.LoadScene(DataManager.Instance.PlayerData.stageIdx, LoadSceneMode.Additive); // 데이터에 해당하는 씬 로딩
                //SceneManager.LoadScene(3, LoadSceneMode.Additive); // 스테이지(맵)
            }
            else
            {
                SceneManager.LoadScene(1, LoadSceneMode.Additive); // 스테이지(맵)
            }
        });
        this.exitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
