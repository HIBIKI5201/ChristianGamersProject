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
        public const int MAX_SCORES = 5;
        public static IReadOnlyList<int> Score => _scores;
        public static int LastScore => _lastScore;

        public static void AddScore(int score)
        {
            _scores.Add(score);
            _scores = _scores.OrderBy(x => x).Take(MAX_SCORES).ToList(); // 降順にソート

            _lastScore = score;
            Debug.Log($"[ScoreData] スコア追加: {score}（現在のスコア数: {_scores.Count}）");
        }


        private static List<int> _scores = new();
        private static int _lastScore;
    }
}
