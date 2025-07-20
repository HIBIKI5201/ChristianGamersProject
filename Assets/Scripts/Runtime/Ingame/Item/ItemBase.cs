using System;
using UnityEngine;

namespace ChristianGamers.Ingame.Item
{
    /// <summary>
    /// アイテムの基本機能
    /// </summary>
    public class ItemBase : MonoBehaviour
    {
        public event Action OnHadGet;

        public float Weight => _weight;

        [SerializeField, Min(0), Tooltip("重さ")]
        private float _weight = 1;

        /// <summary>
        /// アイテムを取得したことを伝えられる
        /// </summary>
        public bool HadGet(InventoryManager inventory)
        {
            //プレイヤーの見えない場所に飛ばす
            if (inventory.AddItem(this))
            {
                this.transform.position = Vector3.one * 10000;

                // Rigidbodyがある場合は動かないようにする
                if (TryGetComponent(out Rigidbody rb))
                    rb.isKinematic = true;

                OnHadGet?.Invoke(); // アイテムを取得したイベントを呼び出す
                return true;
            }
            return false;
        }
    }
}
