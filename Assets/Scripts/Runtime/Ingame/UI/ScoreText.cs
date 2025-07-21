using ChristianGamers.System.Score;
using DG.Tweening;
using SymphonyFrameWork.System;
using UnityEngine;
using UnityEngine.UI;

namespace ChristianGamers
{
    public class ScoreText : MonoBehaviour
    {
        [SerializeField]
        private Text _scoreText;

        [SerializeField]
        private float _scoreCountUpDuration = 0.2f;
        private void Start()
        {
            ScoreManager scoreManager = ServiceLocator.GetInstance<ScoreManager>();
            scoreManager.OnScoreChanged += HandleScoreTextUpdate;
        }

        private void HandleScoreTextUpdate(int sum, int amount)
        {
            DOTween.To(
                () => sum - amount,
                n => _scoreText.text = $"Score : {n.ToString("0000")}", 
                sum, _scoreCountUpDuration);
            
        }
    }
}
