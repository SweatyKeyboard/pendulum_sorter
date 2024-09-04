using _Code.Common;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Code.EndScene
{
    public sealed class EndScene : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;

        private void Awake()
        {
            SetScoreText();
        }

        private void SetScoreText()
        {
            var score = PlayerPrefs.GetInt(PrefsKeys.LAST_SCORE_PREFS_KEY, 0);
            _scoreText.text = score.ToString();
        }

        public void OpenMenu()
        {
            OpenMenuAsync().Forget();
        }
        
        public void RestartGame()
        {
            RestartGameAsync().Forget();
        }

        private async UniTaskVoid OpenMenuAsync()
        {
            await SceneManager.LoadSceneAsync(PrefsKeys.MENU_SCENE_INDEX);
        }

        private async UniTaskVoid RestartGameAsync()
        {
            await SceneManager.LoadSceneAsync(PrefsKeys.GAME_SCENE_INDEX);
        }
    }
}