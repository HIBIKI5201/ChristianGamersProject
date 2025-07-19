using ChristianGamers.Ingame.Item;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChristianGamers.Ingame.Item
{
    /// <summary>
    /// インベントリシステム
    /// </summary>
    public class InventoryManager : MonoBehaviour
    {
        private List<ItemBase> _items = new List<ItemBase>();
        private int _itemIndex = 0;

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
            int value = Math.Sign(axis);

            if (axis == 0) return;
            _itemIndex = (_itemIndex * 2 + value) % _items.Count;
        }

        /// <summary>
        ///     選択されているアイテムを取得する
        /// </summary>
        /// <returns></returns>
        public ItemBase GetSelectedItem() => _items[_itemIndex];
    }
}
