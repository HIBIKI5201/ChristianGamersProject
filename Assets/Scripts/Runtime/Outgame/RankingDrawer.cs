using UnityEngine;
using SymphonyFrameWork.System;
using ChristianGamers.System.Score;
using UnityEngine.UI;

namespace ChristianGamers
{
    public class RankingDrawer : MonoBehaviour
    {
        [SerializeField] private Text _originalText;
        private Text[] _texts;

        void Start()
        {
            var scores = SaveDataSystem<ScoreData>.Data.Score;
            _texts = new Text[scores.Count];

            for (int i = 0; i < _texts.Length; i++)
            {
                var t = Instantiate(_originalText, transform);
                _texts[i] = t;
            }
        }


    }
}
