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
        public event Action<int> OnSelectItem;

        public InventoryManager(float strangth, int maxItemCount)
        {
            _strangth = strangth;
            _maxItemCount = maxItemCount;

            _inventory = new ItemBase[_maxItemCount];
        }

        /// <summary>
        ///     インベントリのリストにアイテムを追加する
        /// </summary>
        /// <param name="item"取得したアイテム></param>
        public bool AddItem(ItemBase item)
        {
            ItemBase[] items = _inventory.Where(x => x != null).ToArray();

            //アイテムの所持数が最大値以上なら追加不可
            if (_maxItemCount <= items.Length) return false;

            float sumWeight = items.Sum(i => i.Weight) + item.Weight;
            float strangth = GetFinalStrangth();

            // アイテムの重さが最大重量を超える場合は追加しない
            if (strangth < sumWeight) return false;

            //空いている場所にアイテムを追加
            int index = Array.IndexOf(_inventory, null);
            _inventory[index] = item;
            OnItemsChanged?.Invoke(_inventory);

            OnWeightChanged?.Invoke(strangth, sumWeight);
            return true;
        }

        /// <summary>
        ///     インベントリのリストからアイテムを削除する
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(ItemBase item)
        {
            //アイテムをリストから削除
            int index = Array.IndexOf(_inventory, item);
            _inventory[index] = null;
            OnItemsChanged?.Invoke(_inventory);

            //現在の合計値をイベント発行する
            float sum = _inventory.Sum(i => i.Weight);
            OnWeightChanged?.Invoke(GetFinalStrangth(), sum);
        }

        /// <summary>
        ///     アイテムの選択
        /// </summary>
        public void SelectItem(float axis)
        {
            if (_inventory.Length <= 1) return; //一つ以下なら選択できない

            int value = (int)Mathf.Sign(axis);
            _selectIndex = (_selectIndex + _inventory.Length + value) % _inventory.Length;
            OnSelectItem?.Invoke(_selectIndex);

            Debug.Log($"index : {_selectIndex}");
        }

        public void UseSelectedItem(PlayerManager player)
        {
            if (_inventory.Length <= 0)
            {
                Debug.LogWarning("No items to use.");
                return;
            }
            ItemBase selectedItem = GetSelectedItem();
            if (selectedItem is IUseble usebleItem)
            {
                usebleItem.Use(player);
                RemoveItem(selectedItem);

                //一番近いアイテムをセレクトする
                _selectIndex = FindNearItemIndex(_selectIndex);
                OnSelectItem?.Invoke(_selectIndex);
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
            0 <= _inventory.Length ? _inventory[_selectIndex] : null;

        public IWithdrawable[] GetWithdrawalItems()
        {
            if (_inventory.Length <= 0) return Array.Empty<IWithdrawable>();

            return _inventory
                .Select(item => item as IWithdrawable)
                .Where(item => item != null)
                .ToArray();
        }

        public void AddStrangthBuff(Func<float, float> buff) => _weightBuff.Add(buff);
        public void RemoveStrangthBuff(Func<float, float> buff) => RemoveStrangthBuff(buff);

        private ItemBase[] _inventory;
        private int _selectIndex = 0;
        private float _strangth;
        private int _maxItemCount;
        private List<Func<float, float>> _weightBuff = new();

        private float GetFinalStrangth()
        {
            float maxStrangth = _strangth;

            //バフを適用
            foreach (var buff in _weightBuff)
            {
                if (buff != null) continue;
                maxStrangth = buff.Invoke(maxStrangth);
            }
            return maxStrangth;
        }

        /// <summary>
        ///     インデックスに近いアイテムの位置を返す
        ///     同じ距離なら右側を優先する
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        private int FindNearItemIndex(int origin)
        {
            //左右に探索する（右優先）
            for(int i = 1; i < _inventory.Length; i++)
            {
                if (Search(origin + i)) return origin + i;
                if (Search(origin - i)) return origin - i;
            }

            return -1;

            //指定されたインデックスがnullじゃないか確認する
            bool Search(int index)
            {
                index = (index + _inventory.Length) % _inventory.Length;
                return _inventory[index] != null;
            }
        }
    }
}
