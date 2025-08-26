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
            DataManager.Instance.playerData.oxygen += GameManager.Instance.state switch
            {
                GameManager.State.NOMAL => -1,
                GameManager.State.RECOVERY => 1,
                GameManager.State.VOLCANO => -2,
                GameManager.State.MAGMA => -(maxOxygenPoint * 0.1f),
                _ => 0
            };
            DataManager.Instance.playerData.oxygen = Mathf.Clamp(DataManager.Instance.playerData.oxygen, 0, maxOxygenPoint);

            if (currentManaGlobe.gameObject.activeInHierarchy)
                UpdateOxygenAmount();

            yield return oxygenWs;
        }
    }
    private void UpdateOxygenAmount()
    {
        //float ratio = GameManager.Instance.oxygenPoint / maxOxygenPoint;
        float ratio = DataManager.Instance.playerData.oxygen / maxOxygenPoint;
        float hight = currentManaGlobe.rectTransform.rect.height * 0.9f;
        currentManaGlobe.rectTransform.localPosition = new Vector3(0, hight * ratio - hight, 0);
    }
}
