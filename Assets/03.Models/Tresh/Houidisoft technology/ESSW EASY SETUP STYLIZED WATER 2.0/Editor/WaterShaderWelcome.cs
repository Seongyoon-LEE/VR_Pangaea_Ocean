using UnityEditor;
using UnityEngine;

namespace ESSW.Welcome
{
public class WaterShaderWelcome : EditorWindow
{
    private static Texture2D logoTexture;
    private static Texture2D stepAddSurface;
    private static Texture2D stepTweakSettings;

    private Vector2 scroll;
    private static bool doNotShowAgain = false;
    private const string PREF_KEY = "WaterShaderWelcome_DoNotShow";

    [MenuItem("Tools/ESSW assets Welcome Guide")]
    public static void ShowWindow()
    {
        LoadAssets();

        var window = GetWindow<WaterShaderWelcome>("Welcome to ESSW Water Shader");
        window.minSize = new Vector2(520, 600);
        window.Show();
    }

    private static void LoadAssets()
    {
        logoTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Editor/Images/step_add_surface.png");
       
        stepAddSurface = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Editor/Images/step_add_surface.png");
        stepTweakSettings = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Editor/Images/step_tweak_settings.png");
    }

    private void OnGUI()
    {
        scroll = EditorGUILayout.BeginScrollView(scroll);
        GUILayout.Space(10);

        // Logo
        if (logoTexture)
        {
            GUILayout.Label(logoTexture, GUILayout.Height(100));
        }

        // Title
        GUILayout.Space(0);
        GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel) { fontSize = 18, alignment = TextAnchor.MiddleCenter };
        GUILayout.Label("Welcome to the ESSW Shader!", titleStyle);
        GUILayout.Space(0);

        GUIStyle boxStyle = new GUIStyle("box") { padding = new RectOffset(10, 10, 10, 10) };

        // Step 1
        GUILayout.BeginVertical(boxStyle);
        GUILayout.Label("Step 1: Add Water Surface", EditorStyles.boldLabel);
        GUILayout.Label("• Go to GameObject > 3D Object > Add Water.\n• This adds a water plane with the shader pre-configured.");
        if (stepAddSurface)
        {
            GUILayout.Label(stepAddSurface, GUILayout.Height(120));
        }
        GUILayout.EndVertical();

        GUILayout.Space(10);

        // Step 2
        GUILayout.BeginVertical(boxStyle);
        GUILayout.Label("Step 2: Insert the material on the Water Shader Controller.", EditorStyles.boldLabel);
        GUILayout.Label("•Tweak Shader Settings \n• Customize foam, color, depth, normal maps, and more in the Inspector.");
        if (stepTweakSettings)
        {
            GUILayout.Label(stepTweakSettings, GUILayout.Height(120));
        }
        GUILayout.EndVertical();

        GUILayout.Space(10);

        // Step 3
        GUILayout.BeginVertical(boxStyle);
        GUILayout.Label("Step 3: Play and Enjoy", EditorStyles.boldLabel);
        GUILayout.Label("• Hit Play to see the water in action.\n• Adjust lighting and post-processing for best visuals.");
        GUILayout.EndVertical();

        GUILayout.Space(20);
        // Step
        GUILayout.BeginVertical(boxStyle);
        GUILayout.Label(" Important Notes", EditorStyles.boldLabel);
        GUILayout.Label("•Enable Opaque Texture.\n• if the depth does not work in the game windows set the Deferred Rendering to Depth Texture Mode to Force Prepass. \n •nreduce the waves height if it looks wierd.\n •set the planar reflection ID the same as in script.");
        GUILayout.EndVertical();

        GUILayout.Space(20);
        // Rate Us Section
        GUILayout.BeginVertical(boxStyle);
        GUILayout.Label("Enjoying the Water Shader?", EditorStyles.boldLabel);
        GUILayout.Label("Support us by leaving a 5-star review on the Asset Store. It really helps us!");

        GUILayout.Space(5);
        if (GUILayout.Button("Rate Us ★★★★★", GUILayout.Height(30)))
        {
            Application.OpenURL("https://assetstore.unity.com/packages/slug/317597"); 
        }
        GUILayout.EndVertical();

        GUILayout.Space(10);
        doNotShowAgain = EditorGUILayout.ToggleLeft("Don't show this again", doNotShowAgain);

        GUILayout.Space(10);
        if (GUILayout.Button("Close", GUILayout.Height(30)))
        {
            if (doNotShowAgain)
                EditorPrefs.SetBool(PREF_KEY, true);

            this.Close();
        }

        EditorGUILayout.EndScrollView();
    }

    [InitializeOnLoadMethod]
    static void InitOnLoad()
    {
        if (!EditorPrefs.GetBool(PREF_KEY, false))
        {
            EditorApplication.update += ShowOnce;
        }
    }

    static void ShowOnce()
    {
        EditorApplication.update -= ShowOnce;
        ShowWindow();
    }
}
}