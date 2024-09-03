using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Code.GameScene
{
    public sealed class Pendulum : MonoBehaviour
    {
        [SerializeField] private Transform _centerOfMass;
        [SerializeField] private Transform _ballPosition;
        [SerializeField] private Ball _ballPrefab;

        [Header("Animation settings")] 
        [SerializeField] [Range(0.1f, 10f)] private float _moveSpeed;
        [SerializeField] [Range(10f, 90f)] private float _moveAmplitude;
        
        private Ball _currentBall;

        private void Update()
        {
            _centerOfMass.localEulerAngles = new Vector3(0, 0, Mathf.Cos(Time.time * _moveSpeed) * _moveAmplitude);


            if (_currentBall == null)
                return;
            
            if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
            {
                ReleaseBall();
            }
        }

        private void ReleaseBall()
        {
            _currentBall.ChangeState(EBallState.Released);
            _currentBall = null;
            RespawnBall(1f).Forget();
        }

        private void Start()
        {
            RespawnBall().Forget();
        }

        private async UniTaskVoid RespawnBall(float delayBeforeSpawn = 0f)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delayBeforeSpawn));
            var ball = Instantiate(_ballPrefab, _ballPosition.position, Quaternion.identity, _ballPosition);
            _currentBall = ball;
        }
    }
}