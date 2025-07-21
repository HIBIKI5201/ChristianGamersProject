using System;
using UnityEngine;

namespace ChristianGamers
{
    [CreateAssetMenu(fileName = nameof(PlayerData), menuName = nameof(PlayerData))]
    public class PlayerData : ScriptableObject
    {
        public float MoveSpeed => _moveSpeed;
        public Vector2 RotationSpeed => _rotationSpeed;
        public float CollectRange => _collectRange;
        public float AngleThreshold => _angleThreshold;
        public Vector3 CollectOffset => _collectOffset;
        public float Strangth => _strangth;
        public int MaxItemCount => _maxItemCount;
        public float ThrowPower => _throwPower;
        public WeightDebuffData[] WeightDebuff => _weightDebuff;

        [Header("移動系設定")]
        [SerializeField, Tooltip("移動速度")]
        private float _moveSpeed = 10;
        [SerializeField, Tooltip("Y回転速度")]
        private Vector2 _rotationSpeed = new Vector2(10, 10);

        [Header("アイテム収集系設定")]
        [SerializeField, Min(0), Tooltip("アイテムを収集する範囲")]
        private float _collectRange = 2.0f;
        [SerializeField, Range(0, 360), Tooltip("アイテムを収集するための角度の閾値（度数法）")]
        private float _angleThreshold = 45.0f;
        [SerializeField, Tooltip("アイテム収集範囲のオフセット")]
        private Vector3 _collectOffset = new Vector3(0, 0.5f, 0);

        [SerializeField, Tooltip("インベントリの最大重量")]
        private float _strangth = 10.0f;
        [SerializeField, Tooltip("インベントリの最大所持数")]
        private int _maxItemCount = 7;

        [SerializeField, Tooltip("投げる力の大きさ")]
        private float _throwPower = 10.0f;
        [SerializeField, Tooltip("重量デバフ")]
        private WeightDebuffData[] _weightDebuff;

        private void OnValidate()
        {
            Array.Sort(_weightDebuff, (a, b) => a.WeightThreshold.CompareTo(b.WeightThreshold));
        }

        [Serializable]
        public struct WeightDebuffData
        {
            public float WeightThreshold => _weightThreshold;
            public float DebuffScale => _debuffScale;

            [SerializeField]
            private float _weightThreshold;
            [SerializeField]
            private float _debuffScale;
        }
    }
}
