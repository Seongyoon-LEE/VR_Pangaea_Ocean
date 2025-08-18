using UnityEditor;
using UnityEngine;

namespace ESSW.Editorcontroller
{
    public class WaterToolMenu : MonoBehaviour
    {
        private const string DefaultMaterialName = "water";
        private const string LowPolyPrefabPath = "LowPolyObject";
        private const string HighPolyPrefabPath = "HighPolyObject";

        [MenuItem("GameObject/3D Object/Add Water/Low Poly", false, 0)]
        public static void AddWaterWithLowPoly()
        {
            AddWaterSurface(LowPolyPrefabPath);
        }

        [MenuItem("GameObject/3D Object/Add Water/High Poly", false, 0)]
        public static void AddWaterWithHighPoly()
        {
            AddWaterSurface(HighPolyPrefabPath);
        }

        private static void AddWaterSurface(string prefabPath)
        {
            // Load resources
            Material defaultWaterMaterial = Resources.Load<Material>(DefaultMaterialName);
            if (defaultWaterMaterial == null)
            {
                Debug.LogError($"Default material '{DefaultMaterialName}' not found in Resources folder. Please add it.");
                return;
            }

            GameObject waterPrefab = LoadPrefab(prefabPath);
            if (waterPrefab == null)
            {
                Debug.LogError($"Water prefab '{prefabPath}' not found in Resources folder.");
                return;
            }

            // Get scene view position
            SceneView sceneView = SceneView.lastActiveSceneView;
            if (sceneView == null || sceneView.camera == null)
            {
                Debug.LogError("No active Scene View found! Please open the Scene View.");
                return;
            }

            Vector3 waterPosition = sceneView.camera.transform.position + sceneView.camera.transform.forward * 10f;

            // Create water object
            GameObject waterObject = (GameObject)PrefabUtility.InstantiatePrefab(waterPrefab);
            waterObject.transform.position = waterPosition;
            waterObject.transform.rotation = Quaternion.identity;
            waterObject.name = "Water Surface";
            waterObject.transform.localScale = new Vector3(200, 1, 200);
            
            Undo.RegisterCreatedObjectUndo(waterObject, "Create Water Surface");

            // Create and assign material
            Material newMaterial = new Material(defaultWaterMaterial)
            {
                name = "Water Material " + System.DateTime.Now.Ticks
            };

            string folderPath = "Assets/GeneratedMaterials";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets", "GeneratedMaterials");
            }

            string assetPath = $"{folderPath}/{newMaterial.name}.mat";
            AssetDatabase.CreateAsset(newMaterial, assetPath);
            
            Renderer renderer = waterObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = newMaterial;
            }

            // Add WaterShaderController component
            waterObject.AddComponent<WaterShaderController>();
 
            ReflectionProbe reflectionProbe = waterObject.AddComponent<ReflectionProbe>();
      //
            reflectionProbe.size = waterObject.transform.localScale + new Vector3(0, 10, 0); // Adjust height
            reflectionProbe.intensity = 0.8f;
            reflectionProbe.boxProjection = true; // Helps with indoor/contained reflections

        

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Selection.activeGameObject = waterObject;
            Debug.Log($"Created  water object  at {waterPosition}");
        }

        private static GameObject LoadPrefab(string prefabName)
        {
            return Resources.Load<GameObject>(prefabName);
        }
    }
}