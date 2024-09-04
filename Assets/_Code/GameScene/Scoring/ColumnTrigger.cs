using System;
using UnityEngine;

namespace _Code.GameScene.Scoring
{
    public sealed class ColumnTrigger : MonoBehaviour
    {
        public event Action<Ball> BallScored;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out Ball ball))
                return;
            
            BallScored?.Invoke(ball);
        }
    }
}