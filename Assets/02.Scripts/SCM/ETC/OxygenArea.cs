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
            StartCoroutine(OxygenHealing());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            GameManager.Instance.state = GameManager.State.NOMAL;
            StopAllCoroutines();
        }
    }

    IEnumerator OxygenHealing()
    {
        while (oxygenPoint > 0)
        {
            // 플레이어 산소량이 최대가 아닐 때 회복
            // 최대 일 때는 감소도 증가도 하지 않고 대기상태
            if (DataManager.Instance.playerData.oxygen < 100)
            {
                GameManager.Instance.state = GameManager.State.RECOVERY;
                oxygenPoint--;

                materialColor.a = alpha * (oxygenPoint / maxOxygen);
                mr.material.color = materialColor;
            }
            else
            {
                GameManager.Instance.state = (GameManager.State)99;
            }
            
            yield return OxygenTime;
        }
        // 회복 가능 산소를 다 사용되면
        transform.gameObject.SetActive(false);
        GameManager.Instance.state = GameManager.State.NOMAL;
    }
}
