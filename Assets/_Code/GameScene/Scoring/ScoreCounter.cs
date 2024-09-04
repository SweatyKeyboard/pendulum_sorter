using System;
using _Code.Common;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Code.GameScene.Scoring
{
    public sealed class ScoreCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private ColumnTrigger[] _columnTriggers;
        
        private const int ROWS_COUNT = 3;
        private const int COLUMNS_COUNT = 3;
        private const float DELAY_BEFORE_CHECK = 0.5f;
        
        private Ball[,] _ballsMatrix = new Ball[COLUMNS_COUNT, ROWS_COUNT];
        
        private int _score = 0;

        private int Score
        {
            get => _score;
            
            set
            {
                _score = value;
                _scoreText.text = _score.ToString();
            }
        }

        private void Awake()
        {
            for (var i = 0; i < _columnTriggers.Length; i++)
            {
                var cached = i;
                _columnTriggers[i].BallScored += ball => OnBallScored(cached, ball);
            }
        }

        private void OnBallScored(int columnIndex, Ball ballType)
        {
            var rowIndex = GetBallsCountInRow(columnIndex);
            _ballsMatrix[columnIndex, rowIndex] = ballType;
            CheckConditionsWithDelay(columnIndex, rowIndex, DELAY_BEFORE_CHECK).Forget();
        }


        private int GetBallsCountInRow(int columnIndex)
        {
            for (var i = 0; i < ROWS_COUNT; i++)
            {
                if (_ballsMatrix[columnIndex, i] == null)
                    return i;
            }
            return ROWS_COUNT;
        }

        private async UniTaskVoid CheckConditionsWithDelay(int columnIndex, int rowIndex, float delay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            
            var targetType = _ballsMatrix[columnIndex, rowIndex].BallType.Type;
            
            var conditionsArray = Enum.GetValues(typeof(EScoreConditionType)) as EScoreConditionType[];

            foreach (var condition in conditionsArray)
            {
                if (CheckCondition(condition, columnIndex, rowIndex, targetType))
                    break;
            }

            RecalculateIndexesAfterGravity();
            CheckForLose();
        }

        private void RecalculateIndexesAfterGravity()
        {
            for (var i = 0; i < COLUMNS_COUNT; i++)
            {
                for (var j = 1; j < ROWS_COUNT; j++)
                {
                    if (_ballsMatrix[i, j - 1] == null)
                        (_ballsMatrix[i, j - 1], _ballsMatrix[i, j]) = (_ballsMatrix[i, j], _ballsMatrix[i, j - 1]);
                }
            }
        }

        private bool CheckCondition(EScoreConditionType conditionType, int columnIndex, int rowIndex, EBallType targetType)
        {
            var isConditionMet = true;
            var minDimension = Mathf.Min(COLUMNS_COUNT, ROWS_COUNT);

            if (conditionType == EScoreConditionType.DiagonalPrimary && columnIndex != rowIndex)
                return false;
            if (conditionType == EScoreConditionType.DiagonalSecondary && columnIndex != minDimension - rowIndex - 1)
                return false;
            
            var iterationsCount = conditionType switch
            {
                    EScoreConditionType.Horizontal => COLUMNS_COUNT,
                    EScoreConditionType.Vertical => ROWS_COUNT,
                    EScoreConditionType.DiagonalPrimary => minDimension,
                    EScoreConditionType.DiagonalSecondary => minDimension,
            };
            
            for (var i = 0; i < iterationsCount; i++)
            {
                var iterationXValue = GetIterationXValue(conditionType, columnIndex, i);
                var iterationYValue = GetIterationYValue(conditionType, rowIndex, i, minDimension);
                
                if (iterationXValue == columnIndex && iterationYValue == rowIndex)
                    continue;

                if (_ballsMatrix[iterationXValue, iterationYValue]?.BallType.Type != targetType)
                {
                    isConditionMet = false;
                }
            }

            if (isConditionMet)
            {
                Score += _ballsMatrix[columnIndex, rowIndex].BallType.Score;
                for (var i = 0; i < iterationsCount; i++)
                {
                    var iterationXValue = GetIterationXValue(conditionType, columnIndex, i);
                    var iterationYValue = GetIterationYValue(conditionType, rowIndex, i, minDimension);
                    
                    _ballsMatrix[iterationXValue, iterationYValue].SelfDestruction().Forget();
                    _ballsMatrix[iterationXValue, iterationYValue] = null;
                }
            }

            return isConditionMet;
        }

        private int GetIterationYValue(EScoreConditionType conditionType, int rowIndex, int i, int minDimension)
        {
            var iterationYValue = conditionType switch
            {
                    EScoreConditionType.Horizontal => i,
                    EScoreConditionType.Vertical => rowIndex,
                    EScoreConditionType.DiagonalPrimary => i,
                    EScoreConditionType.DiagonalSecondary => minDimension - i - 1,
            };
            return iterationYValue;
        }

        private int GetIterationXValue(EScoreConditionType conditionType, int columnIndex, int i)
        {
            var iterationXValue = conditionType switch
            {
                    EScoreConditionType.Horizontal => columnIndex,
                    EScoreConditionType.Vertical => i,
                    EScoreConditionType.DiagonalPrimary => i,
                    EScoreConditionType.DiagonalSecondary => i,
            };
            return iterationXValue;
        }

        private async UniTaskVoid CheckForLose()
        {
            foreach (var ball in _ballsMatrix)
            {
                if (ball == null)
                    return;
            }
            
            PlayerPrefs.SetInt(PrefsKeys.LAST_SCORE_PREFS_KEY, Score);
            PlayerPrefs.Save();

            await SceneManager.LoadSceneAsync(PrefsKeys.FINISH_SCENE_INDEX);
        }
    }
}