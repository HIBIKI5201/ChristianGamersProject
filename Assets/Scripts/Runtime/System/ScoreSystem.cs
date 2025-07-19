using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace ChristianGamers.Ingame.System
{
    public class ScoreSystem : MonoBehaviour
    {
        public static ScoreSystem Instance {get; private set;}
        
        [SerializeField] private Text _scoreText;
        private int _score = 0;
        public bool _isCountUp = false;
        
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Debug.Log("[ScoreSystem] インスタンス生成＆永続化");
            }
            else
            {
                Destroy(gameObject);
                Debug.Log("[ScoreSystem] 既に存在するため破棄");
                return;
            }
            
            List<int> scores = LoadScores();
            SaveScores(scores);
            
            if (_isCountUp)
            {
                AddScore(2);
                Debug.Log("Score +2");
            }
            else
            {
                AddScore(1);
                Debug.Log("Score +1");
            }
            _isCountUp = true;
            
            
        }
        
        public void AddScore(int amount)
        {
            _score += amount;
            Debug.Log($"[ScoreManager] スコア加算: +{amount}（現在のスコア: {_score}）");

            UpdateScoreText();
        }

        public void UpdateScoreText()
        {
            if (_scoreText != null)
            {
                _scoreText.text = "Score: " + _score;
            }
        }
        List<int> LoadScores()
        {
            List<int> scores = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                scores.Add(PlayerPrefs.GetInt("HighScore{i}" + 0));
            }
            return scores;
        }

        void SaveScores(List<int> scores)
        {
            for (int i = 0; i < 10; i++)
            {
                PlayerPrefs.SetInt("HighScore{i}", scores[i]);
            }
            PlayerPrefs.Save();
        }
    }
}
