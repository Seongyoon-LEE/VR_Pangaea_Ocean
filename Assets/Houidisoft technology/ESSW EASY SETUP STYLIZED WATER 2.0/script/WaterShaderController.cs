using UnityEngine;

namespace ESSW.Editorcontroller
{
[RequireComponent(typeof(Renderer))]
public class WaterShaderController : MonoBehaviour
{
    public Material waterMaterial;
   [HideInInspector] 
   public bool enableWaves = true;
    [HideInInspector]
   public PlanarReflectionTextureID selectedTexture;
   [HideInInspector] 
    public FoamStyle selectedFoamStyle;

    public enum PlanarReflectionTextureID
    {
        Tex1,
        Tex2,
        Tex3,
        Tex4
    }

    public enum FoamStyle
    {
        Soft,
        Hard,
        Wavy
    }

    private void Awake()
    {
        // Automatically assign the material from the Renderer if not set manually
        if (waterMaterial == null)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                waterMaterial = renderer.sharedMaterial;
            }
        }
    }
}
}