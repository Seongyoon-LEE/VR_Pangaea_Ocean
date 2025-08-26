using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagamaArea : MonoBehaviour
{
    private readonly string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            GameManager.Instance.state = GameManager.State.MAGMA;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            GameManager.Instance.state = GameManager.State.VOLCANO;
        }
    }
}
