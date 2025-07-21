using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace ChristianGamers
{
    public class ResultView : MonoBehaviour
    {
        [SerializeField] private Image _panel;
        [SerializeField] private Text _rankingText;
        [SerializeField] private Text _lastText;
        [SerializeField] private Text _lastScoreText;

        void Start()
        {
            _panel.color = Color.clear;
            Color rankingTextColor = _rankingText.color;
            rankingTextColor.a = 0;
            _rankingText.color = rankingTextColor;
            Color lastTextColor = _lastText.color;
            lastTextColor.a = 0;
            _lastText.color = lastTextColor;
            Color lastScoreTextColor = _lastScoreText.color;
            lastScoreTextColor.a = 0;
            _lastScoreText.color = lastScoreTextColor;
        }

        public void View()
        {
            _panel.DOFade(1, 1);
            _rankingText.DOFade(1, 1);
            _lastText.DOFade(1, 1);
            _lastScoreText.DOFade(1, 1);

        }
    }
}
