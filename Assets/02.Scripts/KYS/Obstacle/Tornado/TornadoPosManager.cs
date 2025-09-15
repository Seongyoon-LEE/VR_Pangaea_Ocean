using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoPosManager : MonoBehaviour
{
    List<Transform> tornadoPoses = new List<Transform>();
    public void Init()
    {
        GetComponentsInChildren<Transform>(this.tornadoPoses);
    }
}
