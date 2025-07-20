using ChristianGamers.Ingame.Sequence;
using SymphonyFrameWork.System;
using UnityEngine;
using UnityEngine.UI;

namespace ChristianGamers.Ingame.UI
{
    public class TimerText : MonoBehaviour
    {
        [SerializeField]
        private Text _minuteText;
        [SerializeField]
        private Text _secondText;

        private void Start()
        {
            IngameTimer timer = ServiceLocator.GetInstance<IngameTimer>();
            timer.OnTimeUpdate += TimerTextUpdate;
            TimerTextUpdate(timer.TimeLimit);
        }

        private void TimerTextUpdate(float time)
        {
            if (_minuteText == null) return;
            if (_secondText == null) return;

            _minuteText.text = Mathf.Floor(time / 60).ToString("0");
            _secondText.text = Mathf.CeilToInt(time % 60).ToString("00");
        }
    }
}
