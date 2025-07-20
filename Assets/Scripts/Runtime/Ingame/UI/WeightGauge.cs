using ChristianGamers.Ingame.Player;
using SymphonyFrameWork.System;
using UnityEngine;
using UnityEngine.UI;

namespace ChristianGamers
{
    public class WeightGauge : MonoBehaviour
    {
        [SerializeField]
        private Image _gauge;

        void Start()
        {
            PlayerManager player = ServiceLocator.GetInstance<PlayerManager>();
            player.OnWeightChanged += WeightGaugeUpdate;
        }

        private void WeightGaugeUpdate(float max, float value)
        {
            _gauge.fillAmount = value / max;
        }
    }
}
