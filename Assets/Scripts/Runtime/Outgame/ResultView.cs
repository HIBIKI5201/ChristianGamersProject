using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace ChristianGamers
{
    public class ResultView : MonoBehaviour
    {
        [SerializeField] private Image _panel;
        [SerializeField] Text _rankingText;

        void Start()
        {
            _panel.color = Color.clear;
            Color rankingTextColor = _rankingText.color;
            rankingTextColor.a = 0;
            _rankingText.color = rankingTextColor;
        }

        public void View()
        {
            _panel.DOFade(1, 1);
            _rankingText.DOFade(1, 1);
        }
    }
}
