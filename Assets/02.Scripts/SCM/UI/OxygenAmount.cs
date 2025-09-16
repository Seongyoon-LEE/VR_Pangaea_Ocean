using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenAmount : MonoBehaviour
{
    public Image currentManaGlobe; // 드래그 앤 드롭
    private float maxOxygenPoint = 100f;
    private WaitForSeconds oxygenWs;
    IEnumerator Start()
    {
        while (!DataManager.Instance.IsLoadingFinish)
        {
            yield return null;
        }
        oxygenWs = new WaitForSeconds(1f);

        if (DataManager.Instance.PlayerData.isBoarding)
            GameManager.Instance.state = GameManager.State.RECOVERY;

        StartCoroutine(OxygenUpdate());
    }


    IEnumerator OxygenUpdate()
    {
        while (true)
        {
            yield return oxygenWs;

            DataManager.Instance.PlayerData.oxygen += GameManager.Instance.state switch
            {
                GameManager.State.NORMAL => 0, // 평상시 수중
                GameManager.State.RECOVERY => 10, // 회복
                GameManager.State.VOLCANO => -2, // 화산지대
                GameManager.State.MAGMA => -(maxOxygenPoint * 0.1f), // 용암에 닿았을 때
                GameManager.State.TORNADO => -(maxOxygenPoint * 0.2f), // 토네이도에 닿았을 때
                _ => 0 // 기타 사항
            };
            DataManager.Instance.PlayerData.oxygen = Mathf.Clamp(DataManager.Instance.PlayerData.oxygen, 0, maxOxygenPoint);

            // UI가 활성화 되어있다면 UI 업데이트
            if (currentManaGlobe.gameObject.activeInHierarchy)
                UpdateOxygenAmount();

            // 산소량이 0이하일 때 보트로 리스폰
            if (DataManager.Instance.PlayerData.oxygen <= 0)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().PlayerDie();
                UpdateOxygenAmount();
            }

            
        }
    }
    // UI Update
    private void UpdateOxygenAmount()
    {
        float ratio = DataManager.Instance.PlayerData.oxygen / maxOxygenPoint; // 비율 계산
        float hight = currentManaGlobe.rectTransform.rect.height * 0.9f;
        currentManaGlobe.rectTransform.localPosition = new Vector3(0, hight * ratio - hight, 0);
    }
}
