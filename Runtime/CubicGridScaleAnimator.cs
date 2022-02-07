using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CubicGridScaleAnimation
{
    public class CubicGridScaleAnimator : MonoBehaviour
    {
        public bool autoUpdate = true;
        public float timeMultiplier = 1.0f;
        public float baseScale = 1.0f;
        public float globalScaleIntensity = 0.0f;
        public float globalScaleSpeed = 1.0f;
        public float globalScalePhase = 0.0f;
        public float xScaleIntensity = 0.0f;
        public float xScaleFrequency = 1.0f;
        public float xScaleSpeed = 1.0f;
        public float xScalePhase = 0.0f;
        public float yScaleIntensity = 0.0f;
        public float yScaleFrequency = 1.0f;
        public float yScaleSpeed = 1.0f;
        public float yScalePhase = 0.0f;
        public float zScaleIntensity = 0.0f;
        public float zScaleFrequency = 1.0f;
        public float zScaleSpeed = 1.0f;
        public float zScalePhase = 0.0f;
        public float radiusScaleIntensity = 0.0f;
        public float radiusScaleFrequency = 1.0f;
        public float radiusScaleSpeed = 1.0f;
        public float radiusScalePhase = 0.0f;
        public float randomScaleIntensity = 0.0f;
        public float randomScaleSpeed = 1.0f;

        [SerializeField][HideInInspector] List<Transform> _currentGrid = new List<Transform>();
        [SerializeField][HideInInspector] GameObject _currentGridPoint;
        [SerializeField][HideInInspector] Vector3Int _currentGridSize;
        [SerializeField][HideInInspector] Vector3 _currentGridSpacing;
        [SerializeField][HideInInspector] List<float> _currentGridRandomPhase = new List<float>();

        const float TwoPi = Mathf.PI * 2.0f;

        float globalScaleTime = 0.0f;
        float xScaleTime = 0.0f;
        float yScaleTime = 0.0f;
        float zScaleTime = 0.0f;
        float radiusScaleTime = 0.0f;
        float randomScaleTime = 0.0f;

        public bool hasGrid
        {
            get { return _currentGrid.Count > 0; }
        }

        public GameObject currentGridPoint
        {
            get
            {
                if (hasGrid)
                {
                    return _currentGridPoint;
                }
                throw new Exception("CubicGridScaleAnimator does not have a grid.");
            }
        }

        public Vector3Int currentGridSize
        {
            get
            {
                if (hasGrid)
                {
                    return _currentGridSize;
                }
                throw new Exception("CubicGridScaleAnimator does not have a grid.");
            }
        }

        public Vector3 currentGridSpacing
        {
            get
            {
                if (hasGrid)
                {
                    return _currentGridSpacing;
                }
                throw new Exception("CubicGridScaleAnimator does not have a grid.");
            }
        }

        public void DeleteGrid()
        {
            if (!hasGrid) return;
            foreach(Transform point in _currentGrid)
            {
    #if UNITY_EDITOR
                if (EditorApplication.isPlaying)
                {
                    Destroy(point.gameObject);
                }
                else
                {
                    DestroyImmediate(point.gameObject);
                }
    #else
                Destroy(point.gameObject);
    #endif
            }
            _currentGrid.Clear();
            _currentGridRandomPhase.Clear();
        }

        public void CreateGrid(GameObject point, Vector3Int size, Vector3 spacing)
        {
            if (point == null)
            {
                Debug.LogWarning("CubicGridScaleAnimator#CreateGrid: Grid point must not be null.");
                return;
            }
            if (size.x < 0 || size.y < 0 || size.z < 0)
            {
                Debug.LogWarning("CubicGridScaleAnimator#CreateGrid: Grid size must be positive values.");
                return;
            }

            DeleteGrid();
            var half = new Vector3(
                0.5f * (size.x - 1) * spacing.x,
                0.5f * (size.y - 1) * spacing.y,
                0.5f * (size.z - 1) * spacing.z
            );
            for (var zi = 0; zi < size.z; zi++)
            {
                for (var yi = 0; yi < size.y; yi++)
                {
                    for (var xi = 0; xi < size.x; xi++)
                    {
                        var position = new Vector3(spacing.x * xi, spacing.y * yi, spacing.z * zi) - half;
                        var instance = Instantiate(point, position, Quaternion.identity, transform);
                        instance.name = $"Point_x{xi}_y{yi}_z{zi}";
                        instance.hideFlags = HideFlags.NotEditable;
                        _currentGrid.Add(instance.transform);
                        _currentGridRandomPhase.Add(UnityEngine.Random.value);
                    }
                }
            }
            _currentGridPoint = point;
            _currentGridSize = size;
            _currentGridSpacing = spacing;
        }

        public void UpdateGrid(float deltaTime)
        {
            if (!hasGrid) return;

            float t = deltaTime * timeMultiplier; 
            globalScaleTime = (globalScaleTime + t * globalScaleSpeed) % 1.0f;
            xScaleTime = (xScaleTime + t * xScaleSpeed) % 1.0f;
            yScaleTime = (yScaleTime + t * yScaleSpeed) % 1.0f;
            zScaleTime = (zScaleTime + t * zScaleSpeed) % 1.0f;
            radiusScaleTime = (radiusScaleTime + t * radiusScaleSpeed) % 1.0f;
            randomScaleTime = (randomScaleTime + t * randomScaleSpeed) % 1.0f;

            var halfGlobalScaleIntensity = 0.5f * globalScaleIntensity;
            var halfXScaleIntensity = 0.5f * xScaleIntensity;
            var halfYScaleIntensity = 0.5f * yScaleIntensity;
            var halfZScaleIntensity = 0.5f * zScaleIntensity;
            var halfRadiusScaleIntensity = 0.5f * radiusScaleIntensity;
            var halfRandomScaleIntensity = 0.5f * randomScaleIntensity;
            var gs = baseScale * Mathf.Cos(TwoPi * (globalScaleTime + globalScalePhase)) * halfGlobalScaleIntensity + (1.0f - halfGlobalScaleIntensity);
            var gridSizeXY = _currentGridSize.x * _currentGridSize.y;
            var xOffset = xScaleTime + xScalePhase;
            var yOffset = yScaleTime + yScalePhase;
            var zOffset = zScaleTime + zScalePhase;
            var radiusOffset = radiusScaleTime + radiusScalePhase;
            foreach (var point in _currentGrid.Select((value, index) => new { value, index }))
            {
                var x = (float)(point.index % _currentGridSize.x) / _currentGridSize.x;
                var y = Mathf.Floor(point.index % gridSizeXY / _currentGridSize.x) / _currentGridSize.y;
                var z = Mathf.Floor(point.index / gridSizeXY) / _currentGridSize.z;
                var xs = Mathf.Cos(TwoPi * (x * xScaleFrequency + xOffset)) * halfXScaleIntensity + (1.0f - halfXScaleIntensity);
                var ys = Mathf.Cos(TwoPi * (y * yScaleFrequency + yOffset)) * halfYScaleIntensity + (1.0f - halfYScaleIntensity);
                var zs = Mathf.Cos(TwoPi * (z * zScaleFrequency + zOffset)) * halfZScaleIntensity + (1.0f - halfZScaleIntensity);

                var radius = Mathf.Sqrt((x - 0.5f) * (x - 0.5f) + (y - 0.5f) * (y - 0.5f) + (z - 0.5f) * (z - 0.5f));
                var rs = Mathf.Cos(TwoPi * (radius * radiusScaleFrequency + radiusOffset)) * halfRadiusScaleIntensity + (1.0f - halfRadiusScaleIntensity);

                var ns = Mathf.Cos(TwoPi * (randomScaleTime + _currentGridRandomPhase[point.index])) * halfRandomScaleIntensity + (1.0f - halfRandomScaleIntensity);

                var s = gs * xs * ys * zs * rs * ns;
                point.value.transform.localScale = new Vector3(s, s, s);
            }
        }

        void Update()
        {
            if (autoUpdate)
            {
                UpdateGrid(Time.deltaTime);
            }
        }

    }
}

