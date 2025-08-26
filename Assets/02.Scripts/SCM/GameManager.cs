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
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

}
