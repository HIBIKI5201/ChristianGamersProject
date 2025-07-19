using UnityEngine;

namespace ChristianGamers.Ingame.Item
{
    public class ItemBase : MonoBehaviour
    {
        
        /// <summary>
        /// アイテムを取得したことを伝えられる
        /// </summary>
        public void HadGet(InventoryManager inventory)
        {
            //プレイヤーの見えない場所に飛ばす
            this.transform.position = Vector3.one * 10000;
            inventory.AddItem(this);
        }


    }
}
