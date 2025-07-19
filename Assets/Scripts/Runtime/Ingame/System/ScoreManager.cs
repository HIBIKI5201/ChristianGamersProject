using SymphonyFrameWork.System;
using System;
using UnityEngine;

namespace ChristianGamers.System.Score
{
    /// <summary>
    ///     スコアを管理するクラス
    /// </summary>
    public class ScoreManager : MonoBehaviour
    {
        [Tooltip("スコア変化時のイベント。第一引数が合計値、第二引数が変化量")]
        public event Action<int, int> OnScoreChanged;

        private int _score = 0;

        /// <summary>
        ///     スコアを加算する
        /// </summary>
        /// <param name="amount"></param>
        public void AddScore(int amount)
        {
            _score += amount;
            Debug.Log($"[ScoreManager] スコア加算: +{amount}（現在のスコア: {_score}）");

            OnScoreChanged?.Invoke(_score, amount);
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
