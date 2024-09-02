using System;
using UnityEngine;

namespace _Code.FirstScene
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class AnimationObject : MonoBehaviour
    {
        [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }
        public Vector3 StartPosition { get; private set; }
        public float StartAngle { get; private set; }

        public void Init(float startAngle, Vector3 startPosition)
        {
            StartAngle = startAngle;
            StartPosition = startPosition;
        }
    }
}