using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Code.GameScene
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class Ball : MonoBehaviour
    {
        private const float DESTROY_ANIMATION_DURATION = 0.2f;
        [field: SerializeField] public Rigidbody2D Rigidbody { get; private set; }
        
        public EBallState State { get; private set; }

        private CancellationTokenSource _cancellationToken = new();

        public void ChangeState(EBallState state)
        {
            switch (state)
            {
                case EBallState.Released:
                    Release();
                    break;
                
                case EBallState.Sorted:
                    
                    break;
            }
        }

        private void Release()
        {
            transform.parent = null;
            Rigidbody.bodyType = RigidbodyType2D.Dynamic;
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

        private async UniTaskVoid SelfDestruction()
        {
            transform.DOScale(Vector3.zero, DESTROY_ANIMATION_DURATION).SetEase(Ease.OutCubic);
            await UniTask.Delay(TimeSpan.FromSeconds(DESTROY_ANIMATION_DURATION));
            Destroy(gameObject);
        }
    }
}