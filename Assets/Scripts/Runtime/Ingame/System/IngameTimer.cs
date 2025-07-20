using System;
using UnityEngine;

namespace ChristianGamers
{
    public class IngameTimer : MonoBehaviour
    {
        public event Action<float> OnTimeUpdate;

        /// <summary>
        ///     タイマーをストップする
        /// </summary>
        public void Stop() => _isStop = true;

        private float _startTime;
        private bool _isStop;

        private void Start()
        {
            _startTime = Time.time;
        }

        private void Update()
        {
            if (_isStop) return;

            OnTimeUpdate?.Invoke(Time.time - _startTime);
        }
    }
}
