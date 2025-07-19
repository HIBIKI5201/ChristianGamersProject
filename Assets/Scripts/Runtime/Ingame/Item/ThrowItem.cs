using ChristianGamers.Ingame.Item;
using ChristianGamers.Ingame.Player;
using SymphonyFrameWork.System;
using UnityEngine;

namespace ChristianGamers.Ingame.Item
{
    /// <summary>
    /// 投擲アイテム
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class ThrowItem : ItemBase, IUseble
    {
        public void Use()
        {
            PlayerManager player = ServiceLocator.GetInstance<PlayerManager>();
            if (player == null)
            {
                Debug.LogError("PlayerManagerが見つかりません。");
                return;
            }

            Transform muzzle = player.MuzzlePivot;

            // マズルピボットの位置にアイテムを配置
            this.transform.position = muzzle.position;
            float throwForce = player.ThrowPower;

            if (TryGetComponent(out Rigidbody rb))
            {
                rb.AddForce(muzzle.forward * throwForce, ForceMode.Impulse);
            }
            else
            {
                Debug.LogError("Rigidbodyがアタッチされていません。");
            }
        }
    }
}
