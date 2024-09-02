using System;
using UnityEngine;

namespace _Code.FirstScene
{
    public sealed class FirstSceneAnimation : MonoBehaviour
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private AnimationObject _animationPrefab;

        [Header("Animation settings")] 
        [SerializeField] [Range(3,12)] private int _objectsCount;
        [SerializeField] [Range(0.1f, 10f)] private float _speed;

        private AnimationObject[] _spawnedObjects;

        private void Awake()
        {
            _spawnedObjects = new AnimationObject[_objectsCount];
            for (var i = 0; i < _objectsCount; i++)
            {
                var spawnedObject = Instantiate(_animationPrefab, _parent);

                var angle = (float)i / _objectsCount * 2 * Mathf.PI;
                spawnedObject.transform.localPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
                spawnedObject.Init(angle, spawnedObject.transform.position);
                _spawnedObjects[i] = spawnedObject;
            }
        }

        private void Update()
        {
            if (_spawnedObjects.Length == 0)
                return;

            var timeScale = Time.time * _speed;
            
            for (var i = 0; i < _spawnedObjects.Length; i++)
            {
                var currentObject = _spawnedObjects[i];
                currentObject.transform.position =
                        currentObject.StartPosition +
                        new Vector3(Mathf.Cos(timeScale + currentObject.StartAngle), Mathf.Sin(timeScale + currentObject.StartAngle));

                var minColorValue = 0.3f;
                var colorMaxMultiplier = 1f - minColorValue;
                
                currentObject.SpriteRenderer.color = new Color(
                        minColorValue + Mathf.Tan(timeScale + currentObject.StartAngle) * colorMaxMultiplier,
                        minColorValue + Mathf.Sin(timeScale + currentObject.StartAngle) * colorMaxMultiplier,
                        minColorValue + Mathf.Cos(timeScale + currentObject.StartAngle) * colorMaxMultiplier);
            }
        }
    }
}