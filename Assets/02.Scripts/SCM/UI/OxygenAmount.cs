using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenAmount : MonoBehaviour
{
    public Image currentManaGlobe; // 드래그 앤 드롭
    private float oxygenTime;
    private float maxOxygenPoint = 100f;
    void Start()
    {
        GameManager.Instance.oxygenPoint = maxOxygenPoint;
    }

    private void Update()
    {
        if (GameManager.Instance.isRecovery) return;
        oxygenTime += Time.deltaTime;
    }
    private void LateUpdate()
    {
        if(oxygenTime >= 1f)
        {
            GameManager.Instance.oxygenPoint = Mathf.Clamp(--GameManager.Instance.oxygenPoint, 0, 100);
            //GameManager.Instance.oxygenPoint = Mathf.Clamp(--DataManager.Instance.playerData.oxygen, 0, 100);
            oxygenTime = 0f;
        }
        if (currentManaGlobe.gameObject.activeInHierarchy)
            UpdateOxygenAmount();
    }

    private void UpdateOxygenAmount()
    {
        float ratio = GameManager.Instance.oxygenPoint / maxOxygenPoint;
        //float ratio = DataManager.Instance.playerData.oxygen / maxOxygenPoint;
        float hight = currentManaGlobe.rectTransform.rect.height * 0.9f;
        currentManaGlobe.rectTransform.localPosition = new Vector3(0, hight * ratio - hight, 0);
    }
}
