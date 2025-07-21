using ChristianGamers.Ingame.Player;
using UnityEngine;

namespace ChristianGamers.Ingame.Item
{
    /// <summary>
    /// 投擲アイテム
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class ThrowItem : ItemBase, IUseble
    {
        [SerializeField]
        private float _breakableVelocityThreshold = 2;

        [SerializeField]
        private float _duration = 1;

        private bool _isUsing;
        private float _timer;

        public bool Use(PlayerManager player)
        {
            Transform muzzle = player.MuzzlePivot;

            // マズルピボットの位置にアイテムを配置
            this.transform.position = muzzle.position;
            float throwForce = player.ThrowPower;

            if (TryGetComponent(out Rigidbody rb))
            {
                rb.isKinematic = false; // 動かせるようにする
                rb.linearVelocity = Vector3.zero; // 既存の速度をリセット
                rb.AddForce(muzzle.forward * throwForce, ForceMode.Impulse);

                _isUsing = true;
                _timer = Time.time + _duration;
                return true;
            }
            else
            {
                Debug.LogError("Rigidbodyがアタッチされていません。");

                return false;
            }
        }

        private void Update()
        {
            if (!_isUsing) return;

            if (_timer < Time.time) //タイマーが切れたら終了
            {
                _isUsing = false;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!_isUsing) return;

            if (!collision.gameObject
                .TryGetComponent(out ThrowItemBreakable target))
                return;

            Vector3 dir = (target.transform.position - transform.position);
            dir.y = 0;
            dir.Normalize();

            target.Breaked(dir);
        }
    }
}
