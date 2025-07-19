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
        /// <summary>
        /// アイテムを取得したことを伝えられる
        /// </summary>
        public void HadGet(InventoryManager inventory)
        {
            //プレイヤーの見えない場所に飛ばす
            this.transform.position = Vector3.one * 10000;
            inventory.AddItem(this);
            OnHadGet?.Invoke(); // アイテムを取得したイベントを呼び出す
        }
    }
}
