using UnityEngine;
using SymphonyFrameWork.System;
using ChristianGamers.System.Score;
using UnityEngine.UI;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace ChristianGamers
{
    public class RankingDrawer : MonoBehaviour
    {
        [SerializeField] private Text _originalText;
        [SerializeField] private Text _lastScoreText;
        private Text[] _texts;

        void Start()
        {
            var scores = SaveDataSystem<ScoreData>.Data.Score;
            _texts = new Text[ScoreData.MAX_SCORES];

            for (int i = 0; i < _texts.Length; i++)
            {
                var t = Instantiate(_originalText, transform);
                _texts[i] = t;
            }
        }

        public void RankingView()
        {
            var scores = SaveDataSystem<ScoreData>.Data.Score;
            
            for (int i = 0; i < _texts.Length; i++)
            {
                _texts[i].text = $"{i + 1}位 : {(i < scores.Count ? scores[i] : 0)} 点";
            }

            //自分のスコアを表示する処理
            int myScore = SaveDataSystem<ScoreData>.Data.LastScore;
            myScore.ToString();

            if (_lastScoreText != null)
            {
                _lastScoreText.text = $"{myScore} 点";
            }
        }
    }
}
