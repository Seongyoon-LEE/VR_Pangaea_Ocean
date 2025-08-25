using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenArea : MonoBehaviour
{
    private readonly string playerTag = "Player";
    private WaitForSeconds OxygenTime;
    private float oxygenPoint;
    private float maxOxygen = 100f;
    private MeshRenderer mr;
    private float alpha;
    private Color materialColor;
    void Start()
    {
        OxygenTime = new WaitForSeconds(1f);
        oxygenPoint = maxOxygen;
        mr = GetComponent<MeshRenderer>();
        materialColor = mr.material.color;
        alpha = mr.material.color.a;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            GameManager.Instance.isRecovery = true;
            StartCoroutine(OxygenHealing());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            GameManager.Instance.isRecovery = false;
            StopAllCoroutines();
        }
    }

    IEnumerator OxygenHealing()
    {
        while (true)
        {
            // 플레이어 산소량이 최대가 아니거나 회복 가능한 산소가 있을 때
            //if (DataManager.Instance.playerData.oxygen < 100 && oxygenPoint > 0)
            if (GameManager.Instance.oxygenPoint < 100 && oxygenPoint > 0)
            {
                //DataManager.Instance.playerData.oxygen++;
                GameManager.Instance.oxygenPoint++;
                oxygenPoint--;

                materialColor.a = alpha * (oxygenPoint / maxOxygen);
                mr.material.color = materialColor;
            }
            if (oxygenPoint == 0)
            {
                transform.gameObject.SetActive(false);
                GameManager.Instance.isRecovery = false;
                break;
            }
                

            yield return OxygenTime;
        }
    }
}
