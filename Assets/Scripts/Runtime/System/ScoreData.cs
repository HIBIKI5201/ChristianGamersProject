using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ChristianGamers.System.Score
{
    /// <summary>
    ///     スコアのデータクラス
    /// </summary>
    public class ScoreData
    {
        public IReadOnlyList<int> Score => _scores;

        public void AddScore(int score)
        {
            _scores.Add(score);
            Debug.Log($"[ScoreData] スコア追加: {score}（現在のスコア数: {_scores.Count}）");

            _scores = _scores.OrderBy(x => x).Take(MAX_SCORES).ToList(); // 降順にソート
        }

        private const int MAX_SCORES = 5;

        private List<int> _scores = new();
    }
}
