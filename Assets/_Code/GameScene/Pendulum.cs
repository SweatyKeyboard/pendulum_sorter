using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Code.GameScene
{
    public sealed class Pendulum : MonoBehaviour
    {
        [SerializeField] private Transform _centerOfMass;
        [SerializeField] private Transform _ballPosition;
        [SerializeField] private Ball _ballPrefab;
        [SerializeField] private BallSOData[] _ballTypes;

        [Header("Animation settings")] 
        [SerializeField] [Range(0.1f, 10f)] private float _moveSpeed;
        [SerializeField] [Range(10f, 90f)] private float _moveAmplitude;
        [SerializeField] [Range(1f, 3f)] private float _respawnTime;
        
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
            _currentBall.Release();
            _currentBall = null;
            RespawnBall(_respawnTime).Forget();
        }

        private void Start()
        {
            RespawnBall().Forget();
        }

        private async UniTaskVoid RespawnBall(float delayBeforeSpawn = 0f)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delayBeforeSpawn));
            var ball = Instantiate(_ballPrefab, _ballPosition.position, Quaternion.identity, _ballPosition);
            ball.InitType(_ballTypes[Random.Range(0, _ballTypes.Length)]);
            _currentBall = ball;
        }
    }
}