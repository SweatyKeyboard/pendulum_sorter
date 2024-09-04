using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Code.GameScene
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class Ball : MonoBehaviour
    {
        private const float DESTROY_ANIMATION_DURATION = 0.2f;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private ParticleSystem _particleSystem;
        
        public BallSOData BallType { get; private set; }
        private CancellationTokenSource _cancellationToken = new();
        private bool _isAlreadyDestroying;

        public void Release()
        {
            transform.parent = null;
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        }

        public async UniTaskVoid StartDestroyTimer(float delayTime)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delayTime), cancellationToken: _cancellationToken.Token);
            SelfDestruction().Forget();
        }

        public async UniTaskVoid StopDestroyTimer()
        {
            _cancellationToken.Cancel();
            _cancellationToken.Dispose();
            _cancellationToken = new CancellationTokenSource();
        }

        public async UniTaskVoid SelfDestruction()
        {
            if (_isAlreadyDestroying)
                return;

            _particleSystem.gameObject.SetActive(true);
            var startColor = BallType.Color;
            var endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

            var colorOverLifetime = _particleSystem.colorOverLifetime;
            var gradientColor = colorOverLifetime.color;
            gradientColor.colorMin = startColor;
            gradientColor.colorMax = endColor;
            colorOverLifetime.color = gradientColor;
            
            _particleSystem.Play();
            
            _isAlreadyDestroying = true;
            transform.DOScale(Vector3.zero, DESTROY_ANIMATION_DURATION).SetEase(Ease.OutCubic);
            await UniTask.Delay(TimeSpan.FromSeconds(DESTROY_ANIMATION_DURATION));
            Destroy(gameObject);
        }

        public void InitType(BallSOData ballType)
        {
            _spriteRenderer.color = ballType.Color;
            BallType = ballType;
        }
    }
}