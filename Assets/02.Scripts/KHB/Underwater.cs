using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Underwater : MonoBehaviour
{
    [Header("플레이어 카메라")]
    public Transform maincamera;
    public Volume postprocessingVolume;

    [Header("수면 / 심해 깊이")]
    public float surfaceY = 0f;     // 수면 높이
    public float maxDepth = -300f;   // 심해 깊이

    [Header("Volume Profiles")]
    public VolumeProfile surface;
    public VolumeProfile underwater;

    private LiftGammaGain liftGammaGain;
    private ColorAdjustments colorAdjust;

    [Header("색상 보간")]
    public Color shallowColor = new Color(0.3f, 0.7f, 0.8f); // 얕은 수면 (청록)
    public Color deepColor = new Color(0f, 0.05f, 0.2f);  // 심해 (남색)

    [Header("최소 수중 효과 비율")]
    [Range(0f, 0.5f)]
    public float minUnderwaterEffect = 0.2f; // 들어가자마자 최소 20% 적용

    [Header("Fog (가시거리)")]
    public float minFogDensity = 0.01f; // 수면 근처
    public float maxFogDensity = 0.2f;  // 심해 (짙은 안개)

    [Header("태양 빛 감쇠")]
    public Light sunLight;                // Directional Light (태양)
    public float surfaceIntensity = 1f;   // 수면 위에서 밝기
    public float deepIntensity = 0.1f; // 심해에서 밝기

    void Start()
    {
        // underwater 프로파일 안에서 컴포넌트 찾아오기
        if (underwater != null)
        {
            underwater.TryGet(out liftGammaGain);
            underwater.TryGet(out colorAdjust);
        }

        // Fog 기본 세팅
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
    }

    private void Update()
    {
        if (maincamera.position.y < surfaceY)
        {
            EnableEffects(true);
            UpdateUnderwaterEffects();
        }
        else
        {
            EnableEffects(false);
        }
    }

    void EnableEffects(bool activate)
    {
        if (activate)
        {
            postprocessingVolume.profile = underwater;
        }
        else
        {
            postprocessingVolume.profile = surface;
            RenderSettings.fogDensity = 0f; // 위에서는 Fog 제거
            if (sunLight != null)
                sunLight.intensity = surfaceIntensity; // 원래 밝기 복구
        }
    }

    void UpdateUnderwaterEffects()
    {
        // 깊이에 따라 0~1 사이 보간값 계산
        float depth = Mathf.Clamp01((surfaceY - maincamera.position.y) / Mathf.Abs(maxDepth));

        // 최소 수중 효과 적용
        depth = Mathf.Clamp01(minUnderwaterEffect + depth * (1f - minUnderwaterEffect));

        if (liftGammaGain != null)
        {
            // 밝기/톤 보간
            liftGammaGain.lift.value = Color.Lerp(new Color(1.05f, 1.05f, 1.05f, 0), new Color(0.9f, 0.9f, 0.9f, 0), depth);
            liftGammaGain.gamma.value = Color.Lerp(new Color(1f, 1f, 1f, 0), new Color(0.85f, 0.85f, 0.85f, 0), depth);
            liftGammaGain.gain.value = Color.Lerp(new Color(1f, 1f, 1f, 0), new Color(0.7f, 0.7f, 0.7f, 0), depth);
        }

        if (colorAdjust != null)
        {
            // 수면 -> 심해 색상 보간
            colorAdjust.colorFilter.value = Color.Lerp(shallowColor, deepColor, depth);
        }

        // Fog 밀도(가시거리) 보간
        RenderSettings.fogColor = Color.Lerp(shallowColor, deepColor, depth);
        RenderSettings.fogDensity = Mathf.Lerp(minFogDensity, maxFogDensity, depth);

        // 태양 빛 감쇠
        if (sunLight != null)
        {
            sunLight.intensity = Mathf.Lerp(surfaceIntensity, deepIntensity, depth);
        }
    }
}
