using ChristianGamers.System.Score;
using SymphonyFrameWork.System;
using UnityEngine;
using UnityEngine.UI;

namespace ChristianGamers
{
    public class ScoreText : MonoBehaviour
    {
        [SerializeField]
        private Text _scoreText;

        private void Start()
        {
            ScoreManager scoreManager = ServiceLocator.GetInstance<ScoreManager>();
            scoreManager.OnScoreChanged += HandleTextUpdate;
        }

        private void HandleTextUpdate(int sum, int amount)
        {
            _scoreText.text = $"Score : {sum}";
        }
    }
}
