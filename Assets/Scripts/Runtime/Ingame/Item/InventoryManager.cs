using ChristianGamers.Ingame.Item;
using System.Collections.Generic;
using UnityEngine;

namespace ChristianGamers
{
    public class InventoryManager : MonoBehaviour
    {
        private List<ItemBase> _items;

        /// <summary>
        /// インベントリのリストにアイテムを追加する
        /// </summary>
        /// <param name="item"取得したアイテム></param>
        public void AddItem(ItemBase item)
        {
            _items.Add(item);
        }

        /// <summary>
        /// アイテムの選択
        /// </summary>
        public void SelectItem(float axis)
        {

        }
    }
}
