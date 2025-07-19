using SymphonyFrameWork.System;
using System;
using UnityEngine;

namespace ChristianGamers.System.Score
{
    public class ScoreManager : MonoBehaviour
    {
        public event Action<int> OnScoreChanged;

        private int _score = 0;

        /// <summary>
        ///     スコアを加算する
        /// </summary>
        /// <param name="amount"></param>
        public void AddScore(int amount)
        {
            _score += amount;
            Debug.Log($"[ScoreManager] スコア加算: +{amount}（現在のスコア: {_score}）");

            OnScoreChanged?.Invoke(_score);
        }

        /// <summary>
        ///     スコアを保存する
        /// </summary>
        public void SaveScores()
        {
            SaveDataSystem<ScoreData>.Data.AddScore(_score);
            SaveDataSystem<ScoreData>.Save();
        }
    }
}
