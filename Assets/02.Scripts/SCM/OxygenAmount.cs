using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenAmount : MonoBehaviour
{
    public Image currentManaGlobe; // 드래그 앤 드롭
    public float oxygenPoint;
    private float maxOxygenPoint = 100f;
    void Start()
    {
        oxygenPoint = maxOxygenPoint;
    }

    private void LateUpdate()
    {
        if (currentManaGlobe.gameObject.activeInHierarchy)
            UpdateOxygenAmount();
    }

    private void UpdateOxygenAmount()
    {
        // 에셋에 정의된 내용
        float ratio = oxygenPoint / maxOxygenPoint;
        currentManaGlobe.rectTransform.localPosition = new Vector3(0, currentManaGlobe.rectTransform.rect.height * ratio - currentManaGlobe.rectTransform.rect.height, 0);

    }
}
