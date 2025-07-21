using System;
using UnityEngine;

namespace ChristianGamers.Ingame.Sequence
{
    public class IngameTimer : MonoBehaviour
    {
        public event Action<float> OnTimeUpdate;
        public event Action OnTimeUp;

        public float TimeLimit => _timeLimit;
        public void Play()
        {
            _startTime = Time.time;
            _isStop = false;
        }

        /// <summary>
        ///     タイマーをストップする
        /// </summary>
        public void Stop() => _isStop = true;

        [SerializeField]
        private float _timeLimit = 90;

        private bool _isTimeUp;
        private float _startTime;
        private bool _isStop;

        private void Awake()
        {
            _isStop = true;
        }

        private void Update()
        {
            if (_isStop) return;

            float remainTime = _timeLimit
                - (Time.time - _startTime); //経過時間を引く

            OnTimeUpdate?.Invoke(remainTime);

            if (!_isTimeUp && remainTime < 0)
            {
                _isTimeUp = true;
                TimeUp();
            }
        }

        private void TimeUp()
        {
            OnTimeUp?.Invoke();
        }
    }
}
