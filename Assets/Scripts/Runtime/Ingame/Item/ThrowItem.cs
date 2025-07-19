using ChristianGamers.Ingame.Item;
using ChristianGamers.Ingame.Player;
using SymphonyFrameWork.System;
using UnityEngine;

namespace ChristianGamers.Ingame.Item
{
    /// <summary>
    /// 投擲アイテム
    /// </summary>
    public class ThrowItem : ItemBase , IUseble
    {
        [SerializeField] private float _throwForce = 10f;
        private Rigidbody2D _rb;

        public void Use()
        {
            PlayerManager player = ServiceLocator.GetInstance<PlayerManager>();
            this.transform.position = player.MuzzlePivot.position;

            _rb = GetComponent<Rigidbody2D>();
            _rb.AddForce(Vector3.forward *  _throwForce, (ForceMode2D)ForceMode.Impulse);
        }
    }
}
