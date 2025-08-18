using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace ESSW.Editorcontroller
{
[CustomEditor(typeof(WaterShaderController))]
public class WaterControllerEditor : Editor
{
    AnimBool showFoamAnim, showReflectionAnim, showWavesAnim, showColorsAnim, showNormalsAnim;

    void OnEnable()
    {
        showFoamAnim = new AnimBool(true); showFoamAnim.valueChanged.AddListener(Repaint);
        showReflectionAnim = new AnimBool(true); showReflectionAnim.valueChanged.AddListener(Repaint);
        showWavesAnim = new AnimBool(true); showWavesAnim.valueChanged.AddListener(Repaint);
        showColorsAnim = new AnimBool(true); showColorsAnim.valueChanged.AddListener(Repaint);
        showNormalsAnim = new AnimBool(true); showNormalsAnim.valueChanged.AddListener(Repaint);

        WaterShaderController controller = (WaterShaderController)target;
        if (controller != null && controller.waterMaterial != null)
        {
            ApplyFoamKeyword(controller);
            ApplyReflectionKeyword(controller);
        }
    }

    public override void OnInspectorGUI()
    {
        WaterShaderController controller = (WaterShaderController)target;

        if (controller.waterMaterial == null)
        {
            EditorGUILayout.HelpBox("Please assign a water material.", MessageType.Warning);
            controller.waterMaterial = (Material)EditorGUILayout.ObjectField("Water Material", controller.waterMaterial, typeof(Material), false);
            return;
        }

        controller.waterMaterial = (Material)EditorGUILayout.ObjectField(
            new GUIContent("Water Material", "Material that uses the water shader."),
            controller.waterMaterial, typeof(Material), false);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("🌊 Stylized Water Settings", EditorStyles.boldLabel);
        DrawSeparator();

        EditorGUILayout.LabelField("Planar Texture ID", EditorStyles.boldLabel);

        EditorGUILayout.Space();
        if (GUILayout.Button("Add planar Reflection Script", GUILayout.Height(30)))
        {
            AddWaterInteractionScript();
        }
        EditorGUILayout.Space();

        controller.selectedTexture = (WaterShaderController.PlanarReflectionTextureID)
            EditorGUILayout.EnumPopup(new GUIContent("Reflection Texture", "Select which reflection texture to use."),
            controller.selectedTexture);

        ApplyReflectionKeyword(controller);

        DrawSeparator();

        EditorGUILayout.LabelField("Foam Style", EditorStyles.boldLabel);
        controller.selectedFoamStyle = (WaterShaderController.FoamStyle)
            EditorGUILayout.EnumPopup(new GUIContent("Foam Style", "Select the foam rendering style."),
            controller.selectedFoamStyle);

        ApplyFoamKeyword(controller);

        DrawSeparator();

        ToggleKeywordGUI(controller.waterMaterial, "_FOAM", "Enable Foam", "Toggles foam effect.");
        ToggleKeywordGUI(controller.waterMaterial, "_REFRACTION", "Enable Refraction", "Toggles refraction effect.");
        controller.enableWaves = EditorGUILayout.Toggle(new GUIContent("Enable Waves", "Enable or disable the vertex movement"), controller.enableWaves);
        ToggleKeyword(controller.waterMaterial, "_ENABLE_WAVES", controller.enableWaves);

        DrawSeparator();

        controller.waterMaterial.SetFloat("_Smouthness", EditorGUILayout.Slider("Smouthness", controller.waterMaterial.GetFloat("_Smouthness"), 0f, 1f));
        controller.waterMaterial.SetFloat("_Metalic", EditorGUILayout.Slider("Metalic", controller.waterMaterial.GetFloat("_Metalic"), 0.1f, 0.7f));
        controller.waterMaterial.SetFloat("_Darkness", EditorGUILayout.Slider("Darkness", controller.waterMaterial.GetFloat("_Darkness"), 1f, 4f));

        DrawSeparator();

        showReflectionAnim.target = EditorGUILayout.Foldout(showReflectionAnim.target, " Reflection", true);
        if (EditorGUILayout.BeginFadeGroup(showReflectionAnim.faded))
        {
            controller.waterMaterial.SetFloat("_Reflect_power", EditorGUILayout.Slider("Reflection Power", controller.waterMaterial.GetFloat("_Reflect_power"), 0.01f, 5f));
            controller.waterMaterial.SetFloat("_Reflection_Distance", EditorGUILayout.FloatField("Reflection Distance", controller.waterMaterial.GetFloat("_Reflection_Distance")));
            controller.waterMaterial.SetFloat("_Distortion_Power", EditorGUILayout.Slider("Distortion Power", controller.waterMaterial.GetFloat("_Distortion_Power"), 0.01f, 1.7f));
        }
        EditorGUILayout.EndFadeGroup();
        DrawSeparator();

        showFoamAnim.target = EditorGUILayout.Foldout(showFoamAnim.target, " Foam", true);
        if (EditorGUILayout.BeginFadeGroup(showFoamAnim.faded))
        {
            switch (controller.selectedFoamStyle)
            {
                case WaterShaderController.FoamStyle.Wavy:
                    controller.waterMaterial.SetFloat("_Wave_freq_Style3", EditorGUILayout.FloatField("Wave Frequency", controller.waterMaterial.GetFloat("_Wave_freq_Style3")));
                    controller.waterMaterial.SetFloat("_Wave_Speed_Style3", EditorGUILayout.FloatField("Wave Speed", controller.waterMaterial.GetFloat("_Wave_Speed_Style3")));
                    controller.waterMaterial.SetFloat("_Wave_Dist_Style3", EditorGUILayout.FloatField("Wave Distance", controller.waterMaterial.GetFloat("_Wave_Dist_Style3")));
                    break;

                case WaterShaderController.FoamStyle.Hard:
                    controller.waterMaterial.SetTexture("_Foam_Texture", (Texture)EditorGUILayout.ObjectField("Foam Texture", controller.waterMaterial.GetTexture("_Foam_Texture"), typeof(Texture), false));
                    controller.waterMaterial.SetFloat("_cutoff2", EditorGUILayout.FloatField("Foam Cutoff", controller.waterMaterial.GetFloat("_cutoff2")));
                    controller.waterMaterial.SetFloat("_intensity2", EditorGUILayout.FloatField("Foam Intensity", controller.waterMaterial.GetFloat("_intensity2")));
                    controller.waterMaterial.SetFloat("_speed2", EditorGUILayout.Slider("Movement Speed", controller.waterMaterial.GetFloat("_speed2"), 0, 5));
                    controller.waterMaterial.SetFloat("_Scale2", EditorGUILayout.FloatField("Foam Scale", controller.waterMaterial.GetFloat("_Scale2")));
                    controller.waterMaterial.SetFloat("_amount2", EditorGUILayout.FloatField("Foam Amount", controller.waterMaterial.GetFloat("_amount2")));
                    break;

                case WaterShaderController.FoamStyle.Soft:
                    controller.waterMaterial.SetFloat("_foam_cutoff", EditorGUILayout.FloatField("Foam Cutoff", controller.waterMaterial.GetFloat("_foam_cutoff")));
                    controller.waterMaterial.SetFloat("_Foam_Intesity", EditorGUILayout.FloatField("Foam Intensity", controller.waterMaterial.GetFloat("_Foam_Intesity")));
                    controller.waterMaterial.SetFloat("_Foam_Speed", EditorGUILayout.Slider("Movement Speed", controller.waterMaterial.GetFloat("_Foam_Speed"), 0, 0.5f));
                    controller.waterMaterial.SetFloat("_Foam_Scale", EditorGUILayout.FloatField("Foam Scale", controller.waterMaterial.GetFloat("_Foam_Scale")));
                    controller.waterMaterial.SetFloat("_Foam_Amount", EditorGUILayout.FloatField("Foam Amount", controller.waterMaterial.GetFloat("_Foam_Amount")));
                    break;
            }
        }
        EditorGUILayout.EndFadeGroup();
        DrawSeparator();

        showWavesAnim.target = EditorGUILayout.Foldout(showWavesAnim.target, "🌁 Waves", true);
        if (EditorGUILayout.BeginFadeGroup(showWavesAnim.faded))
        {
            controller.waterMaterial.SetFloat("_Wave_Height", EditorGUILayout.FloatField("Wave Height", controller.waterMaterial.GetFloat("_Wave_Height")));
            controller.waterMaterial.SetFloat("_Wave_length", EditorGUILayout.Slider("Wave Length", controller.waterMaterial.GetFloat("_Wave_length"), 0.01f, 1f));
            controller.waterMaterial.SetFloat("_Wave_Speed", EditorGUILayout.FloatField("Wave Speed", controller.waterMaterial.GetFloat("_Wave_Speed")));
            controller.waterMaterial.SetFloat("_Peak_Sharpness", EditorGUILayout.FloatField("Peak Sharpness", controller.waterMaterial.GetFloat("_Peak_Sharpness")));
            controller.waterMaterial.SetVector("_Waves_Dir", EditorGUILayout.Vector2Field("Wave Direction", controller.waterMaterial.GetVector("_Waves_Dir")));
        }
        EditorGUILayout.EndFadeGroup();
        DrawSeparator();

        showColorsAnim.target = EditorGUILayout.Foldout(showColorsAnim.target, "🎨 Colors and Opacity", true);
        if (EditorGUILayout.BeginFadeGroup(showColorsAnim.faded))
        {
            EditorGUILayout.LabelField(" Texture ", EditorStyles.boldLabel);
            controller.waterMaterial.SetFloat("_water_movement_speed", EditorGUILayout.FloatField("Movement Speed", controller.waterMaterial.GetFloat("_water_movement_speed")));
            controller.waterMaterial.SetFloat("_Texture_scale", EditorGUILayout.FloatField("Texture Scale", controller.waterMaterial.GetFloat("_Texture_scale")));

            EditorGUILayout.Space();
            EditorGUILayout.LabelField(" Refraction ", EditorStyles.boldLabel);
            controller.waterMaterial.SetFloat("_Refraction_power", EditorGUILayout.Slider("Refraction Power", controller.waterMaterial.GetFloat("_Refraction_power"), 0f, 0.3f));
            controller.waterMaterial.SetFloat("_Depth1", EditorGUILayout.Slider("Water Depth", controller.waterMaterial.GetFloat("_Depth1"), 5f, 70f));
            controller.waterMaterial.SetFloat("_middle_color", EditorGUILayout.Slider("Middle Color Blend", controller.waterMaterial.GetFloat("_middle_color"), 0.01f, 1f));
            controller.waterMaterial.SetColor("_deep_water_color", EditorGUILayout.ColorField("Deep Color", controller.waterMaterial.GetColor("_deep_water_color")));
            controller.waterMaterial.SetColor("_Color", EditorGUILayout.ColorField("Middle Color", controller.waterMaterial.GetColor("_Color")));
            controller.waterMaterial.SetColor("_Shallow_water_color", EditorGUILayout.ColorField("Shallow Color", controller.waterMaterial.GetColor("_Shallow_water_color")));
            controller.waterMaterial.SetFloat("_Depth_Fade", EditorGUILayout.Slider("Outline Fade", controller.waterMaterial.GetFloat("_Depth_Fade"), 0.01f, 10f));
        }
        EditorGUILayout.EndFadeGroup();
        DrawSeparator();

        showNormalsAnim.target = EditorGUILayout.Foldout(showNormalsAnim.target, "📐 Normals", true);
        if (EditorGUILayout.BeginFadeGroup(showNormalsAnim.faded))
        {
            controller.waterMaterial.SetTexture("_First_Normal_Texture", (Texture)EditorGUILayout.ObjectField("First Normal", controller.waterMaterial.GetTexture("_First_Normal_Texture"), typeof(Texture), false));
            controller.waterMaterial.SetTexture("_Second_Normal_Texture", (Texture)EditorGUILayout.ObjectField("Second Normal", controller.waterMaterial.GetTexture("_Second_Normal_Texture"), typeof(Texture), false));
            controller.waterMaterial.SetFloat("_Normal_strenght", EditorGUILayout.Slider("Normal Strength", controller.waterMaterial.GetFloat("_Normal_strenght"), 0.1f, 1f));
        }
        EditorGUILayout.EndFadeGroup();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(controller.waterMaterial);
            EditorUtility.SetDirty(controller);
        }

        base.OnInspectorGUI();
    }

    private void ApplyFoamKeyword(WaterShaderController controller)
    {
        controller.waterMaterial.DisableKeyword("_FOAM_STYLE_STYLE1");
        controller.waterMaterial.DisableKeyword("_FOAM_STYLE_STYLE2");
        controller.waterMaterial.DisableKeyword("_FOAM_STYLE_STYLE3");

        switch (controller.selectedFoamStyle)
        {
            case WaterShaderController.FoamStyle.Soft:
                controller.waterMaterial.EnableKeyword("_FOAM_STYLE_STYLE1");
                break;
            case WaterShaderController.FoamStyle.Hard:
                controller.waterMaterial.EnableKeyword("_FOAM_STYLE_STYLE2");
                break;
            case WaterShaderController.FoamStyle.Wavy:
                controller.waterMaterial.EnableKeyword("_FOAM_STYLE_STYLE3");
                break;
        }
    }

    private void ApplyReflectionKeyword(WaterShaderController controller)
    {
        controller.waterMaterial.DisableKeyword("_TEXTUREID_TEX1");
        controller.waterMaterial.DisableKeyword("_TEXTUREID_TEX2");
        controller.waterMaterial.DisableKeyword("_TEXTUREID_TEX3");
        controller.waterMaterial.DisableKeyword("_TEXTUREID_TEX4");
        controller.waterMaterial.EnableKeyword($"_TEXTUREID_{controller.selectedTexture.ToString().ToUpper()}");
    }

    private void AddWaterInteractionScript()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            if (go.GetComponent<PlanarReflectionsProbe>() == null)
            {
                Undo.AddComponent<PlanarReflectionsProbe>(go);
                Debug.Log($"Added planar reflection script to {go.name}", go);
            }
            else
            {
                Debug.LogWarning($"{go.name} already has a planar reflection component", go);
            }
        }
    }

    void DrawSeparator()
    {
        GUILayout.Space(8);
        var rect = EditorGUILayout.GetControlRect(false, 1);
        EditorGUI.DrawRect(rect, new Color(0.15f, 0.15f, 0.15f));
        GUILayout.Space(8);
    }

    void ToggleKeywordGUI(Material mat, string keyword, string label, string tooltip)
    {
        bool enabled = mat.IsKeywordEnabled(keyword);
        bool newEnabled = EditorGUILayout.Toggle(new GUIContent(label, tooltip), enabled);
        ToggleKeyword(mat, keyword, newEnabled);
    }

    void ToggleKeyword(Material mat, string keyword, bool enabled)
    {
        if (enabled) mat.EnableKeyword(keyword);
        else mat.DisableKeyword(keyword);
    }
}
}
