// 파일 경로: Assets/Editor/ScatterProbesOnTerrain.cs
using UnityEditor;
using UnityEngine;

public class ScatterProbesOnTerrain : EditorWindow
{
    int gridX = 10;
    int gridZ = 10;
    float heightOffset = 1.5f;

    [MenuItem("Tools/Scatter Light Probes (URP)")]
    static void OpenWindow()
    {
        GetWindow<ScatterProbesOnTerrain>("Scatter Probes").Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Terrain Light Probe Settings", EditorStyles.boldLabel);

        gridX = EditorGUILayout.IntField("Grid Count X", gridX);
        gridZ = EditorGUILayout.IntField("Grid Count Z", gridZ);
        heightOffset = EditorGUILayout.FloatField("Height Offset", heightOffset);

        if (GUILayout.Button("Scatter"))
            Scatter();
    }

    void Scatter()
    {
        var terrain = Terrain.activeTerrain;
        if (terrain == null)
        {
            Debug.LogError("씬에 Active Terrain이 없습니다.");
            return;
        }

        var data = terrain.terrainData;
        var origin = terrain.transform.position;
        Vector3[] probes = new Vector3[gridX * gridZ];
        int idx = 0;

        for (int x = 0; x < gridX; x++)
            for (int z = 0; z < gridZ; z++)
            {
                float wx = origin.x + data.size.x * x / (gridX - 1);
                float wz = origin.z + data.size.z * z / (gridZ - 1);
                float wy = terrain.SampleHeight(new Vector3(wx, 0, wz)) + origin.y + heightOffset;
                probes[idx++] = new Vector3(wx, wy, wz);
            }

        var go = new GameObject($"LightProbeGroup_{gridX}x{gridZ}");
        var group = go.AddComponent<LightProbeGroup>();
        group.probePositions = probes;
        Selection.activeGameObject = go;
    }
}