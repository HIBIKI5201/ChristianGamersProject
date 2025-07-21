using System;
using UnityEngine;

namespace ChristianGamers
{
    [CreateAssetMenu(fileName = nameof(PlayerData), menuName = "GameData/" + nameof(PlayerData))]
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

        /// <summary>
        ///     与えられた重量に対して一番大きいデバフを探す
        ///     無ければ１を返す
        /// </summary>
        /// <param name="weight"></param>
        /// <returns></returns>
        public float GetWeightDebuff(float weight, float strangth)
        {
            if (_weightDebuffDatas == null || _weightDebuffDatas.Length == 0)
                return 1;

            //一番大きいデバフを探す
            for (int i = _weightDebuffDatas.Length - 1; 0 <= i; i--)
            {
                WeightDebuffData data = _weightDebuffDatas[i];

                if (data.WeightThreshold < weight / strangth)
                {
                    return _weightDebuffDatas[i].DebuffScale;
                }
            }

            //無ければ１を返す
            return 1;
        }

        private void OnEnable()
        {
            //閾値の量に応じてソートする
            Array.Sort(_weightDebuffDatas, (a, b) => -a.WeightThreshold.CompareTo(b.WeightThreshold));
        }

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
        private WeightDebuffData[] _weightDebuffDatas;

        [Serializable]
        private struct WeightDebuffData
        {
            public float WeightThreshold => _weightThreshold;
            public float DebuffScale => _debuffScale;

            [SerializeField, Range(0, 1)]
            private float _weightThreshold;
            [SerializeField]
            private float _debuffScale;
        }
    }
}
