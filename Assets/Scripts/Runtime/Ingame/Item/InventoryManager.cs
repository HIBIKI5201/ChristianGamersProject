using ChristianGamers.Ingame.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ChristianGamers.Ingame.Item
{
    /// <summary>
    ///     インベントリシステム
    /// </summary>
    [Serializable]
    public class InventoryManager
    {
        public event Action<IReadOnlyList<ItemBase>> OnItemsChanged;
        public event Action<float, float> OnWeightChanged;

        public InventoryManager(float maxWeight, int maxItemCount)
        {
            _maxWeight = maxWeight;
            _maxItemCount = maxItemCount;

            _items = new ItemBase[_maxItemCount];
        }

        private ItemBase[] _items;
        private int _selectIndex = 0;
        private float _maxWeight;
        private int _maxItemCount;

        /// <summary>
        ///     インベントリのリストにアイテムを追加する
        /// </summary>
        /// <param name="item"取得したアイテム></param>
        public bool AddItem(ItemBase item)
        {
            //最大値以上なら追加不可
            if (_maxItemCount <= _items.Length) return false;

            float sumWeight = _items.Sum(i => i.Weight) + item.Weight;

            // アイテムの重さが最大重量を超える場合は追加しない
            if (_maxWeight < sumWeight) return false;

            //空いている場所にアイテムを追加
            int index = Array.IndexOf(_items, null);
            _items[index] = item;
            OnItemsChanged?.Invoke(_items);

            OnWeightChanged?.Invoke(_maxWeight, sumWeight);
            return true;
        }

        /// <summary>
        ///     インベントリのリストからアイテムを削除する
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(ItemBase item)
        {
            //アイテムをリストから削除
            int index = Array.IndexOf(_items, item);
            _items[index] = null;
            OnItemsChanged?.Invoke(_items);

            //現在の合計値をイベント発行する
            float sum = _items.Sum(i => i.Weight);
            OnWeightChanged?.Invoke(_maxWeight, sum);
        }

        /// <summary>
        ///     アイテムの選択
        /// </summary>
        public void SelectItem(float axis)
        {
            if (_items.Length <= 0) return;

            int value = (int)Mathf.Sign(axis);
            _selectIndex = (_selectIndex + _items.Length + value) % _items.Length;

            Debug.Log($"index : {_selectIndex}");
        }

        public void UseSelectedItem(PlayerManager player)
        {
            if (_items.Length <= 0)
            {
                Debug.LogWarning("No items to use.");
                return;
            }
            ItemBase selectedItem = GetSelectedItem();
            if (selectedItem is IUseble usebleItem)
            {
                usebleItem.Use(player);
                RemoveItem(selectedItem);

                //アイテム総数が減っているのでインデックスを減らす
                if (_selectIndex != 0) _selectIndex--;
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
            0 <= _items.Length ? _items[_selectIndex] : null;

        public IWithdrawable[] GetWithdrawalItems()
        {
            if (_items.Length <= 0) return Array.Empty<IWithdrawable>();

            return _items
                .Select(item => item as IWithdrawable)
                .Where(item => item != null)
                .ToArray();
        }
    }
}
