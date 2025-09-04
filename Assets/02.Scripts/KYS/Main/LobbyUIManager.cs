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
            SceneManager.LoadScene(2); // 내부 로직 관련
            SceneManager.LoadScene(3, LoadSceneMode.Additive); // 스테이지(맵)
            SceneManager.LoadScene(4, LoadSceneMode.Additive); // 플레이어
        });
        this.exitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
