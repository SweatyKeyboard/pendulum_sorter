using System;
using _Code.Common;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Code.FirstScene
{
    public sealed class StartButton : MonoBehaviour
    {
        [SerializeField] private float _jumpDelay = 2f;
        [SerializeField] private float _jumpPower = 4f;
        [SerializeField] private int _jumpsCount = 1;
        [SerializeField] private float _jumpDuration = 0.5f;
        
        private float _lastPlayedTime;
        private Sequence _jumpSequence;
        
        private void Update()
        {
            if (_lastPlayedTime + _jumpDelay < Time.time)
            {
                _lastPlayedTime = Time.time;
                transform.DOJump(
                        transform.position,
                        _jumpPower,
                        _jumpsCount,
                        _jumpDuration).SetEase(Ease.OutCubic);
            }
        }

        public void StartGame()
        {
            StartGameAsync().Forget();
        }

        private async UniTaskVoid StartGameAsync()
        {
            await SceneManager.LoadSceneAsync(PrefsKeys.GAME_SCENE_INDEX);
        }
    }
}