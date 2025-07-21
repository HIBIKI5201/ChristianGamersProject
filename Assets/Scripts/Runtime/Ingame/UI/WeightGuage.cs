using ChristianGamers.Ingame.Player;
using DG.Tweening;
using SymphonyFrameWork.System;
using UnityEngine;
using UnityEngine.UI;

namespace ChristianGamers
{
    public class WeightGuage : MonoBehaviour
    {
        [SerializeField]
        private Image _gauge;

        [SerializeField, Min(0)]
        private float _guageFillDuration = 0.5f;

        void Start()
        {
            PlayerManager player = ServiceLocator.GetInstance<PlayerManager>();
            player.OnWeightChanged += WeightGaugeUpdate;
        }

        private void WeightGaugeUpdate(float max, float value)
        {
            _gauge.DOFillAmount(value / max, _guageFillDuration);
        }
    }
}
