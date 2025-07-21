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

                return true;
            }
            else
            {
                Debug.LogError("Rigidbodyがアタッチされていません。");

                return false;
            }
        }
    }
}
