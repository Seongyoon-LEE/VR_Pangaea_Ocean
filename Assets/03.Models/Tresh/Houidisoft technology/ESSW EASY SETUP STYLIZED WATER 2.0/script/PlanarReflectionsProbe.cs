using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ESSW.Editorcontroller
{
    [ExecuteAlways, AddComponentMenu("Rendering/Planar Reflection Probe")]
    public class PlanarReflectionsProbe : MonoBehaviour 
    {
        [Range(1, 4)] public int targetTextureID;
        [Space(10)]
        [SerializeField] private bool useCustomNormal = true;
        [SerializeField] private Vector3 customNormal = Vector3.zero;
        [Space(10)]
        [Header("Reflections Quality")]
        [Range(0.01f, 1.0f)] public float reflectionsQuality = 1f;
        public float farClipPlane = 1000;
        public bool renderBackground = true;
        [Space(10)]
        public bool renderInEditor = true;

        private GameObject _probeGO;
        private Camera _probe;
        private Skybox _probeSkybox;
        private readonly Dictionary<Camera, RenderTexture> _camTextureMap = new Dictionary<Camera, RenderTexture>();
        private readonly List<Camera> _ignoredCameras = new List<Camera>();

        private void OnEnable() => RenderPipelineManager.beginCameraRendering += PreRender;
        private void OnDisable() => Cleanup();
        private void OnDestroy() => Cleanup();

        private void Cleanup()
        {
            FinalizeProbe();
            RenderPipelineManager.beginCameraRendering -= PreRender;
            CleanupRenderTextures();
        }

        private void InitializeProbe()
        {
            _probeGO = new GameObject($"PlanarReflectionProbe_{GetInstanceID()}", typeof(Camera), typeof(Skybox))
            {
                hideFlags = HideFlags.HideAndDontSave
            };
            _probe = _probeGO.GetComponent<Camera>();
            _probeSkybox = _probeGO.GetComponent<Skybox>();
            _probeSkybox.enabled = false;
            _probeSkybox.material = null;
        }

        private void FinalizeProbe()
        {
            if (_probeGO == null) return;
            
            if (Application.isEditor) 
                DestroyImmediate(_probeGO);
            else 
                Destroy(_probeGO);
            
            _probeGO = null;
            _probe = null;
            _probeSkybox = null;
        }

        private void CleanupRenderTextures()
        {
            foreach (var texture in _camTextureMap.Values)
            {
                if (texture != null)
                {
                    texture.Release();
                    if (Application.isEditor)
                        DestroyImmediate(texture);
                    else
                        Destroy(texture);
                }
            }
            _camTextureMap.Clear();
        }

        private bool ShouldSkipCamera(Camera cam)
        {
            return cam == null || 
                   cam.cameraType == CameraType.Reflection ||
                   (!renderInEditor && cam.cameraType == CameraType.SceneView) ||
                   _ignoredCameras.Contains(cam);
        }

        private void PreRender(ScriptableRenderContext context, Camera cam)
        {
            if (ShouldSkipCamera(cam)) return;
            if (_probe == null) InitializeProbe();

            Vector3 normal = GetNormal();
            UpdateProbeSettings(cam);
            CreateRenderTexture(cam);
            UpdateProbeTransform(cam, normal);
            CalculateObliqueProjection(normal);

            // Ensure valid render target
            if (_probe.targetTexture == null || !_probe.targetTexture.IsCreated())
            {
                Debug.LogWarning("Reflection probe render texture not ready");
                return;
            }

            // Render using URP (with warning suppression)
            #pragma warning disable CS0618
            UniversalRenderPipeline.RenderSingleCamera(context, _probe);
            #pragma warning restore CS0618

            Shader.SetGlobalTexture($"_PlanarReflectionsTex{targetTextureID}", _probe.targetTexture);
        }

        private void UpdateProbeSettings(Camera cam)
        {
            _probe.CopyFrom(cam);
            _probe.enabled = false;
            _probe.cameraType = CameraType.Reflection;
            _probe.usePhysicalProperties = false;
            _probe.farClipPlane = farClipPlane;
            
            // Ensure proper color rendering
            _probe.forceIntoRenderTexture = true;
            _probe.allowMSAA = false;
            _probe.allowHDR = true;
            
            _probeSkybox.enabled = false;
            _probeSkybox.material = null;

            _probe.clearFlags = renderBackground ? cam.clearFlags : CameraClearFlags.SolidColor;
            _probe.backgroundColor = Color.clear;
            
            if (renderBackground && cam.TryGetComponent(out Skybox camSkybox))
            {
                _probeSkybox.material = camSkybox.material;
                _probeSkybox.enabled = camSkybox.enabled;
            }
        }

        private void CreateRenderTexture(Camera cam)
        {
            int width = Mathf.Max(8, Mathf.RoundToInt(cam.pixelWidth * reflectionsQuality));
            int height = Mathf.Max(8, Mathf.RoundToInt(cam.pixelHeight * reflectionsQuality));

            if (!_camTextureMap.TryGetValue(cam, out var texture) || 
                texture == null || 
                texture.width != width || 
                texture.height != height)
            {
                if (texture != null)
                {
                    texture.Release();
                    _camTextureMap.Remove(cam);
                }

                texture = new RenderTexture(width, height, 24, RenderTextureFormat.DefaultHDR)
                {
                    name = $"PlanarReflection_{cam.name}_{GetInstanceID()}",
                    useMipMap = false,
                    autoGenerateMips = false,
                    depthStencilFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.D32_SFloat_S8_UInt
                };
                
                texture.Create();
                _camTextureMap[cam] = texture;
            }

            _probe.targetTexture = texture;
        }

        private Vector3 GetNormal()
        {
            if (!useCustomNormal)
                return transform.forward;
            
            return customNormal == Vector3.zero ? Vector3.up : customNormal.normalized;
        }

        private void UpdateProbeTransform(Camera cam, Vector3 normal)
        {
            Vector3 positionOffset = cam.transform.position - transform.position;
            Vector3 projectedOffset = normal * Vector3.Dot(normal, positionOffset);
            _probe.transform.position = cam.transform.position - 2 * projectedOffset;

            Vector3 reflectedForward = Vector3.Reflect(cam.transform.forward, normal);
            Vector3 reflectedUp = Vector3.Reflect(cam.transform.up, normal);
            _probe.transform.rotation = Quaternion.LookRotation(reflectedForward, reflectedUp);
        }

        private void CalculateObliqueProjection(Vector3 normal)
        {
            Matrix4x4 viewMatrix = _probe.worldToCameraMatrix;
            Vector3 viewPosition = viewMatrix.MultiplyPoint(transform.position);
            Vector3 viewNormal = viewMatrix.MultiplyVector(normal).normalized;
            
            Vector4 clipPlane = new Vector4(
                viewNormal.x,
                viewNormal.y,
                viewNormal.z,
                -Vector3.Dot(viewPosition, viewNormal));
            
            _probe.projectionMatrix = _probe.CalculateObliqueMatrix(clipPlane);
        }

        public void IgnoreCamera(Camera cam)
        {
            if (cam != null && !_ignoredCameras.Contains(cam))
                _ignoredCameras.Add(cam);
        }

        public void UnignoreCamera(Camera cam)
        {
            if (cam != null)
                _ignoredCameras.Remove(cam);
        }

        public void ClearIgnoredList() => _ignoredCameras.Clear();
        public bool IsIgnoring(Camera cam) => cam != null && _ignoredCameras.Contains(cam);

        public static PlanarReflectionsProbe[] FindProbesRenderingTo(int id)
        {
            var allProbes = FindObjectsByType<PlanarReflectionsProbe>(FindObjectsSortMode.None);
            List<PlanarReflectionsProbe> matchingProbes = new List<PlanarReflectionsProbe>();
            
            foreach (var probe in allProbes)
            {
                if (probe.targetTextureID == id)
                    matchingProbes.Add(probe);
            }
            
            return matchingProbes.ToArray();
        }

        public static PlanarReflectionsProbe FindProbeRenderingTo(int id)
        {
            var allProbes = FindObjectsByType<PlanarReflectionsProbe>(FindObjectsSortMode.None);
            foreach (var probe in allProbes)
            {
                if (probe.targetTextureID == id)
                    return probe;
            }
            return null;
        }
    }
}