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

    private void Update()
    {
        oxygenPoint -= Time.deltaTime;
        oxygenPoint = Mathf.Clamp(oxygenPoint, 0, 100);
    }
    private void LateUpdate()
    {
        if (currentManaGlobe.gameObject.activeInHierarchy)
            UpdateOxygenAmount();
    }

    private void UpdateOxygenAmount()
    {
        float ratio = oxygenPoint / maxOxygenPoint;
        float hight = currentManaGlobe.rectTransform.rect.height * 0.9f;
        currentManaGlobe.rectTransform.localPosition = new Vector3(0, hight * ratio - hight, 0);

    }
}
