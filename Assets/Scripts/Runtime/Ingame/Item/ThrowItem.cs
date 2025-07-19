using ChristianGamers.Ingame.Item;
using ChristianGamers.Ingame.Player;
using SymphonyFrameWork.System;
using UnityEngine;

namespace ChristianGamers.Ingame.Item
{
    /// <summary>
    /// 投擲アイテム
    /// </summary>
    public class ThrowItem : ItemBase, IUseble
    {
        private Rigidbody2D _rb;

        public void Use()
        {
            PlayerManager player = ServiceLocator.GetInstance<PlayerManager>();
            this.transform.position = player.MuzzlePivot.position;
            float throwForce = player.ThrowPower;

            _rb = GetComponent<Rigidbody2D>();
            _rb.AddForce(Vector3.forward * throwForce, (ForceMode2D)ForceMode.Impulse);
        }
    }
}
