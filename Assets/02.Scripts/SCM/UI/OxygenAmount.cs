using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenAmount : MonoBehaviour
{
    public Image currentManaGlobe; // 드래그 앤 드롭
    private float maxOxygenPoint = 100f;
    private WaitForSeconds oxygenWs;
    void Start()
    {
        oxygenWs = new WaitForSeconds(1f);
        StartCoroutine(OxygenUpdate());
    }


    IEnumerator OxygenUpdate()
    {
        while (true)
        {
            DataManager.Instance.PlayerData.oxygen += GameManager.Instance.state switch
            {
                GameManager.State.NOMAL => -1, // 평상시 수중
                GameManager.State.RECOVERY => 1, // 회복
                GameManager.State.VOLCANO => -2, // 화산지대
                GameManager.State.MAGMA => -(maxOxygenPoint * 0.1f), // 용암에 닿았을 때
                _ => 0 // 기타 사항
            };
            DataManager.Instance.PlayerData.oxygen = Mathf.Clamp(DataManager.Instance.PlayerData.oxygen, 0, maxOxygenPoint);

            if (currentManaGlobe.gameObject.activeInHierarchy)
                UpdateOxygenAmount();

            yield return oxygenWs;
        }
    }
    private void UpdateOxygenAmount()
    {
        //float ratio = GameManager.Instance.oxygenPoint / maxOxygenPoint;
        float ratio = DataManager.Instance.PlayerData.oxygen / maxOxygenPoint;
        float hight = currentManaGlobe.rectTransform.rect.height * 0.9f;
        currentManaGlobe.rectTransform.localPosition = new Vector3(0, hight * ratio - hight, 0);
    }
}
