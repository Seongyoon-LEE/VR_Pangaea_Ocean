using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMaterial : MonoBehaviour
{
    public List<Material> materials; // 드래그 앤 드랍 : 기본, 성공, 실패
    private MeshRenderer meshRenderer;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void MaterialSetting(int index)
    {
        if (meshRenderer != null)
        {
            meshRenderer.material = materials[index];
        }
    }
}
