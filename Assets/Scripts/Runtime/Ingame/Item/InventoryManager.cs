using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChristianGamers.Ingame.Item
{
    /// <summary>
    ///     インベントリシステム
    /// </summary>
    public class InventoryManager : MonoBehaviour
    {
        private List<ItemBase> _items = new List<ItemBase>();
        private int _itemIndex = 0;

        /// <summary>
        ///     インベントリのリストにアイテムを追加する
        /// </summary>
        /// <param name="item"取得したアイテム></param>
        public void AddItem(ItemBase item)
        {
            _items.Add(item);
        }

        /// <summary>
        ///     インベントリのリストからアイテムを削除する
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(ItemBase item)
        {
            _items.Remove(item);
        }

        /// <summary>
        ///     アイテムの選択
        /// </summary>
        public void SelectItem(float axis)
        {
            int value = Math.Sign(axis);

            if (axis == 0) return;
            _itemIndex = (_itemIndex + _items.Count + value) % _items.Count;

            Debug.Log($"index : {_itemIndex}");
        }

        public void UseSelectedItem()
        {
            if (_items.Count == 0)
            {
                Debug.LogWarning("No items to use.");
                return;
            }
            ItemBase selectedItem = GetSelectedItem();
            if (selectedItem is IUseble usebleItem)
            {
                usebleItem.Use();
                RemoveItem(selectedItem);

                //アイテム総数が減っているのでインデックスを減らす
                if (_itemIndex != 0) _itemIndex--; 
            }
            else
            {
                Debug.LogWarning("Selected item is not usable.");
            }
        }

        /// <summary>
        ///     選択されているアイテムを取得する
        /// </summary>
        /// <returns></returns>
        public ItemBase GetSelectedItem() =>
            0 < _items.Count ? _items[_itemIndex] : null;
    }
}
